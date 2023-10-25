using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClientTCPManager : MonoBehaviour {
	
    [Header("Number of retries before aborting connection")]
    public int maxRetries = 5;

    private string ipAddress = "";
    private string port = "";
    private string serverMessage = "EMPTY";
    private bool isApplicationQuitting = false;
    private ConnectionState connectionState;

    private TcpClient client;

    private void Start() {
        
        connectionState = ConnectionState.NOT_CONNECTED;
        ChangeConnectionState(Color.red, "NOT CONNECTED");

        // Carico l'IP e la porta direttamente dalle PlayerPrefs
        LoadIPAddress();
        LoadPort();

        // Imposta l'indirizzo IP e la porta del server a cui connettersi   
        ConnectToServerWithRetry();
    }

    // Carica IP e Porta del destinatario
    private void LoadIPAddress() {

        if(PlayerPrefs.HasKey("masterIP")) {
            ipAddress = PlayerPrefs.GetString("masterIP");
            Debug.Log("TCPManager: IP caricato: " + ipAddress);
        } else {
            Debug.LogError("TCPManager: IP non caricato correttamente!");
        }

    }

    private void LoadPort() {

        if(PlayerPrefs.HasKey("masterPort")) {
            port = PlayerPrefs.GetString("masterPort");
            Debug.Log("TCPManager: porta caricata: " + port);
        } else {
            Debug.LogError("TCPManager: porta non caricata correttamente!");
        }

    }

    private string GetMyIPAddress() {

        string ipAddress = string.Empty;

        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

        foreach (IPAddress ip in localIPs) {
            if (ip.AddressFamily == AddressFamily.InterNetwork) {
                ipAddress = ip.ToString();
                break;
            }
        }

        return ipAddress;
    }

    private void ChangeConnectionState(Color newColor, string newText) {

        // 0 = IconState
        // 1 = TextState
        MasterManager.instance.connectionState.transform.GetChild(0).gameObject.GetComponent<Image>().color = newColor;
        MasterManager.instance.connectionState.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().SetText(newText);
    }

    private void ConnectToServerWithRetry() {
        
        int currentRetry = 0;

        // Imposta lo stato sulla connessione in corso
        connectionState = ConnectionState.CONNECTION_IN_PROGRESS;
        
    
        Loom.RunAsync(() => {
            while (currentRetry < maxRetries) {

                Loom.QueueOnMainThread(() => {
                    ChangeConnectionState(Color.yellow, "CONNECTING Retry: " + currentRetry);
                });
                

                // Esci dal ciclo se l'applicazione sta terminando
                if (isApplicationQuitting) {
                    Debug.LogWarning("L'applicazione è stata terminata, smetto di tentare la connessione...");
                    return;
                }

                try {
                    client = new TcpClient(ipAddress, int.Parse(port));
                    ReceiveAndPrintData();

                    connectionState = ConnectionState.CONNECTED;
                    Loom.QueueOnMainThread(() => {
                        ChangeConnectionState(Color.green, "CONNECTED");
                    });

                    break; // Esci dal ciclo se la connessione ha successo
                }
                catch (SocketException socketException) {

                    currentRetry++;
                    Debug.Log("TCP Socket Exception (tentativo numero " + currentRetry + "): " + socketException);

                    if (currentRetry >= maxRetries) {

                        connectionState = ConnectionState.CONNECTION_ABORTED;
                        Loom.QueueOnMainThread(() => {
                            ChangeConnectionState(Color.red, "CONNECTION FAILED");
                        });
       
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

            CheckMessage(serverMessage);
        }
    }

    // Forse da togliere, serve nel caso voglia fare un RobotResponseManager
    public string GetLastServerMessage() {
        return serverMessage;
    }

    // Controllo il messaggio ricevuto dal robot
    private void CheckMessage(string message) {

        // Inizialmente vuota
        string battleMonster = "";

        switch (message) {

            case RobotCommands.readyResponse:
                Debug.Log("Ho ricevuto il comando READY - avvio il flusso video");
                Debug.Log("COMANDO START: " + RobotCommands.start + GetMyIPAddress() + ":" + port);
                MasterManager.instance.clientTCPManager.SendData(RobotCommands.start + GetMyIPAddress() + ":" + port);
                break;

            case RobotCommands.identifyResponseKing:
                battleMonster = "Primordial Blue Wizard";
                break;
            case RobotCommands.identifyResponseQueen:
                battleMonster = "Forgotten Evil";
                break;
            case RobotCommands.identifyResponseRook:
                battleMonster = "Archaic Minotaur";
                break;
            case RobotCommands.identifyResponseBishop:
                battleMonster = "Vampire Lord";
                break;
            case RobotCommands.identifyResponseKnight:
                battleMonster = "Shadow Dragons";
                break;
            case RobotCommands.identifyResponsePawn:
                battleMonster = "Red-Eyes Witch";
                break;
            case RobotCommands.identifyResponseNothing:
                Debug.LogWarning("Non è stato riconosciuto nulla!");
                break;
        }

        if (!string.IsNullOrEmpty(battleMonster)) {
            Debug.LogWarning("Riconosciuto " + message + ": avvio la battaglia!");
            Debug.LogWarning("Mostro da combattere: " + battleMonster);
            PlayerPrefs.SetString("monsterToBattle", battleMonster);

            // TODO: da fixare, il LoadScene si può fare soltanto sul Main Thread
            Loom.QueueOnMainThread(() => {
                SceneHelper.LoadScene("Battle");
            });
            
        }
        
    }

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

    public ConnectionState GetConnectionState() {
        return connectionState;
    }

    private void OnApplicationQuit() {
        isApplicationQuitting = true; // Imposta il flag quando l'applicazione sta terminando
        MasterManager.instance.clientTCPManager.SendData(RobotCommands.disconnect);
        client?.Close();
    }


    public void Enable() {
        gameObject.SetActive(true);
    }

    public void Disable() {
        gameObject.SetActive(false);
    }


}
