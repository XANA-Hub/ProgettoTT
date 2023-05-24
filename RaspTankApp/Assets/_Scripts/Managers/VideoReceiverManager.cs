using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class VideoReceiverManager : MonoBehaviour {
	
    public int port = 25565;

    private UdpClient client;
    private IPEndPoint localEndPoint;
    private void Start() {

        Debug.Log("UDP: Avvio il receiver");
        ConnectToServer();
    }

    private void ConnectToServer() {

        // Imposta la porta del server a cui connettersi
        localEndPoint = new IPEndPoint(IPAddress.Any, port);

        Loom.RunAsync(() => {
            try {
                Debug.Log("UDP: sono dentro al thread");
                client = new UdpClient(localEndPoint);
                Debug.Log("UDP: nuovo client creato");
                ReceiveAndPrintData();
            }
            catch (SocketException socketException) {
                Debug.Log("UDP: Socket exception: " + socketException);
            }
        });

    }

    private void ReceiveAndPrintData() {

        IPEndPoint remoteEndPoint = localEndPoint;

        while (true) {
            Debug.Log("UDP: Mi preparo a ricevere, dentro al loop");
            byte[] receivedData = client.Receive(ref remoteEndPoint);
            //Debug.Log(receivedData);

            string serverMessage = Encoding.ASCII.GetString(receivedData);
            Debug.Log(serverMessage);
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
