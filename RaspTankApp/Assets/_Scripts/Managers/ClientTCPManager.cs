using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClientTCPManager : MonoBehaviour {
	
    public string ipAddress = "192.168.178.69";
    public int port = 25565;

    private TcpClient client;

    private void Start() {

        // Imposta l'indirizzo IP e la porta del server a cui connettersi   
        ConnectToServer();
    }

    private void ConnectToServer() {

        Loom.RunAsync(() => {
            try {
                client = new TcpClient(ipAddress, port);
                ReceiveAndPrintData();
            }
            catch (SocketException socketException) {
                Debug.Log("TCP: Socket exception: " + socketException);
            }
        });
    }

    private void ReceiveAndPrintData() {

        Byte[] bytes = new Byte[1024];
        int length = 0;

        Debug.Log("TCP: Sono dentro a ReceiveAndPrintData");
        using (NetworkStream stream = client.GetStream()) {
            while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {
                var incomingData = new byte[length];
                Array.Copy(bytes, 0, incomingData, 0, length);

                string serverMessage = Encoding.ASCII.GetString(incomingData);
                Debug.Log("TCP: Messaggio ricevuto: " + serverMessage);
            }
        }
    }

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
            Debug.Log("TCP: Socket exception: " + socketException);
        }
    }

    public void Enable() {
        gameObject.SetActive(true);
    }

    public void Disable() {
        gameObject.SetActive(false);
    }

    private void OnApplicationQuit() {
        if (client != null) {
            client.Close();
        }
    }
}
