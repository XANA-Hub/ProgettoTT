using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;

public class VideoReceiverManager : MonoBehaviour {   

    public int port = -1;
    public RawImage rawImage;
    private Texture2D receivedTexture;
    private UdpClient udpClient;
    private bool isListening = true;

    private void Start() {

        loadPort();

        receivedTexture = new Texture2D(640, 480);

        // Create a UDP client and bind it to a specific port
        udpClient = new UdpClient(port); 
        
        Loom.RunAsync(() => {
            ReceiveData();
        });
    }


    
    private void loadPort() {

        if(PlayerPrefs.HasKey("masterPort")) {
            string localPort = PlayerPrefs.GetString("masterPort");
            int.TryParse(localPort, out port);

            Debug.Log("UDPManager: porta caricata: " + port);
        } else {
            Debug.LogError("UDPManager: porta non caricata correttamente!");
        }

    }

    private void ReceiveData() {

        while (isListening) {
            try {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpClient.Receive(ref anyIP);

                // Converti i dati in una stringa utilizzando un encoding specifico (ad esempio, UTF-8)
                // string message = Encoding.UTF8.GetString(data);
                // Stampa il messaggio nella console di Unity
                // Debug.Log("Messaggio ricevuto: " + message);
                
                Loom.QueueOnMainThread(() => {
                    convertBytesToTexture(data);
                });

            }
            catch(Exception) {
                //Debug.LogError("Error receiving data: " + e.Message);
                continue;

            }
        }
    }

    private void convertBytesToTexture(byte[] byteArray) {
        try {
            receivedTexture.LoadImage(byteArray);
            rawImage.texture = receivedTexture;
        } catch(Exception e) {
            Debug.LogError("Errore conversione da byte a texture: " + e.Message);
        }
    }

    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }

    private void OnApplicationQuit() {
        if (isListening) {
            udpClient.Close();
            isListening = false;
        }
    }
}
