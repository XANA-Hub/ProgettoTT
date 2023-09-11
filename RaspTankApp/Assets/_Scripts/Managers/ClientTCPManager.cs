using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClientTCPManager : MonoBehaviour {
	
    public string ipAddress = "192.168.178.69";
    public int port = 25565;
    public int maxRetries = 5;
    private string serverMessage = "EMPTY";
    private bool isApplicationQuitting = false;

    private TcpClient client;

    private void Start() {

        // Imposta l'indirizzo IP e la porta del server a cui connettersi   
        ConnectToServerWithRetry();
    }

    private void ConnectToServerWithRetry() {
        
        int currentRetry = 0;

        Loom.RunAsync(() => {
            while (currentRetry < maxRetries) {

                // Esci dal ciclo se l'applicazione sta terminando
                if (isApplicationQuitting) {
                    Debug.LogWarning("L'applicazione è stata terminata, smetto di tentare la connessione...");
                    return;
                }

                try {
                    client = new TcpClient(ipAddress, port);
                    ReceiveAndPrintData();
                    break; // Esci dal ciclo se la connessione ha successo
                }
                catch (SocketException socketException) {
                    currentRetry++;
                    Debug.Log("TCP Socket Exception (tentativo numero " + currentRetry + "): " + socketException);
                    if (currentRetry >= maxRetries) {
                        Debug.LogError("Connessione non riuscita dopo " + maxRetries + " tentativi. Smetto...");
                    }
                }
            }
        });

    }

    private void ReceiveAndPrintData() {

        Byte[] bytes = new Byte[1024];
        int length = 0;

        Debug.Log("TCP: Sono dentro a ReceiveAndPrintData");
        using NetworkStream stream = client.GetStream();
        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {
            var incomingData = new byte[length];
            Array.Copy(bytes, 0, incomingData, 0, length);
            serverMessage = Encoding.ASCII.GetString(incomingData);
            Debug.Log("TCP: Messaggio ricevuto: " + serverMessage);

            checkMessage(serverMessage);
        }
    }

    // Forse da togliere, serve nel caso voglia fare un RobotResponseManager
    public string getLatestServerMessage() {
        return serverMessage;
    }

    private void checkMessage(string message) {
        
        // Avvio il flusso video
        if(message ==  RobotCommands.readyResponse) {
            Debug.Log("Ho ricevuto il comando READY - avvio il flusso video");
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.start);
        }

        // Ho trovato un pezzo degli scacchi, mostro la schermata della battaglia
        else if(message == RobotCommands.identifyResponse) {
            Debug.Log("Ho ricevuto il comando IDENTIFIED - avvio la battaglia");
            MasterManager.instance.battleManager.StartBattle();
        }
    }

    /*
    private void ConnectToServer() {

        Loom.RunAsync(() => {
            try {
                client = new TcpClient(ipAddress, port);
                ReceiveAndPrintData();
            }
            catch (SocketException socketException) {
                Debug.Log("TCp Socket exception: " + socketException);
            }
        });
    }
    */
    
    // --------------------

    /*
    private void ReceiveAndPrintDataMultiple() {
       
        Byte[] bytes = new Byte[1024];

        while (true) {
            using (NetworkStream stream = client.GetStream()) {
                int length;

                while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {
                    var incommingData = new byte[length];
                    Array.Copy(bytes, 0, incommingData, 0, length);

                    string serverMessage = Encoding.ASCII.GetString(incommingData);
                    Debug.Log("TCP: Server message received as: " + serverMessage);
                }
            }
        }
            
    }
    */

    public void SendData(string clientMessage) {

        if (client == null) {
            return;
        }

        try {
            NetworkStream stream = client.GetStream();
            if (stream.CanWrite) {
                byte[] clientMessageAsByteArray = Encoding.UTF8.GetBytes(clientMessage);
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                Debug.Log("TCP: Messaggio inviato!");
            }
        }
        catch (SocketException socketException) {
            Debug.Log("TCP Socket exception: " + socketException);
        }
    }

    public void Enable() {
        gameObject.SetActive(true);
    }

    public void Disable() {
        gameObject.SetActive(false);
    }

    private void OnApplicationQuit() {
        isApplicationQuitting = true; // Imposta il flag quando l'applicazione sta terminando
        MasterManager.instance.clientTCPManager.SendData(RobotCommands.disconnect);
        client?.Close();
    }
}
