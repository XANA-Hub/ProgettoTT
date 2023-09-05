using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class VideoReceiverManager : MonoBehaviour
{   
    public int port = 25565;
    public RawImage rawImage;
    private Texture2D receivedTexture;
    private UdpClient udpClient;
    private bool isListening = true;

    private void Start()
    {
        receivedTexture = new Texture2D(640, 480); // Initialize the Texture2D

        // Create a UDP client and bind it to a specific port
        udpClient = new UdpClient(port); // Change this port to the desired port number 
        
        Loom.RunAsync(() => {
            ReceiveData();
        });
    }

    private void ReceiveData()
    {
        while (isListening)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpClient.Receive(ref anyIP);

                // Converti i dati in una stringa utilizzando un encoding specifico (ad esempio, UTF-8)
                //string message = Encoding.UTF8.GetString(data);
                // Stampa il messaggio nella console di Unity
                //Debug.Log("Messaggio ricevuto: " + message);
                
                Loom.QueueOnMainThread(() => {
                    convertBytesToTexture(data);
                });

            }
            catch (Exception e)
            {
                Debug.LogError("Error receiving data: " + e.Message);
            }
        }
    }

    private void convertBytesToTexture(byte[] byteArray)
    {
        try
        {
            receivedTexture.LoadImage(byteArray);
            rawImage.texture = receivedTexture;
        }
        catch (Exception e)
        {
            Debug.LogError("Error converting bytes to texture: " + e.Message);
        }
    }

    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        // Stop listening and clean up resources when the script is destroyed
        isListening = false;
        udpClient.Close();
    }
}
