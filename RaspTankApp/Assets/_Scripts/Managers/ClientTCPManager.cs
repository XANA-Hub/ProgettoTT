using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClientTCPManager : MonoBehaviour {
	
    private string ipAddress = "";
    public int port = -1;
    public int maxRetries = 5;
    private string serverMessage = "EMPTY";
    private bool isApplicationQuitting = false;

    private TcpClient client;

    private void Start() {
        
        // Carico l'IP e la porta direttamente dalle PlayerPrefs
        loadAddress();

        // Imposta l'indirizzo IP e la porta del server a cui connettersi   
        ConnectToServerWithRetry();
    }

    private void loadAddress() {

        if(PlayerPrefs.HasKey("masterIP")) {
            string localIP = PlayerPrefs.GetString("masterIP");
            ipAddress = localIP;

            Debug.Log("TCPManager: IP caricato: " + ipAddress);
        } else {
            Debug.LogError("TCPManager: IP non caricato correttamente!");
        }

        if(PlayerPrefs.HasKey("masterPort")) {
            string localPort = PlayerPrefs.GetString("masterPort");
            int.TryParse(localPort, out port);

            Debug.Log("TCPManager: porta caricata: " + port);
        } else {
            Debug.LogError("TCPManager: porta non caricata correttamente!");
        }

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

    // Controllo il messaggio ricevuto dal robot
    private void checkMessage(string message) {

        // Inizialmente vuota
        string battleMonster = "";

        switch (message) {
            case RobotCommands.readyResponse:
                Debug.Log("Ho ricevuto il comando READY - avvio il flusso video");
                MasterManager.instance.clientTCPManager.SendData(RobotCommands.start);
                break;

            case RobotCommands.identifyResponseWhiteKing:
                battleMonster = "Skeleton";
                break;
            case RobotCommands.identifyResponseWhiteQueen:
                battleMonster = "Witch";
                break;
            case RobotCommands.identifyResponseWhiteRook:
                battleMonster = "WhiteRook";
                break;
            case RobotCommands.identifyResponseWhiteBishop:
                battleMonster = "WhiteBishop";
                break;
            case RobotCommands.identifyResponseWhiteKnight:
                battleMonster = "WhiteKnight";
                break;
            case RobotCommands.identifyResponseWhitePawn:
                battleMonster = "WhitePawn";
                break;
            case RobotCommands.identifyResponseBlackKing:
                battleMonster = "Angel";
                break;
            case RobotCommands.identifyResponseBlackQueen:
                battleMonster = "BlackQueen";
                break;
            case RobotCommands.identifyResponseBlackRook:
                battleMonster = "BlackRook";
                break;
            case RobotCommands.identifyResponseBlackBishop:
                battleMonster = "BlackBishop";
                break;
            case RobotCommands.identifyResponseBlackKnight:
                battleMonster = "BlackKnight";
                break;
            case RobotCommands.identifyResponseBlackPawn:
                battleMonster = "BlackPawn";
                break;
        }

        if (!string.IsNullOrEmpty(battleMonster)) {
            Debug.Log("Riconosciuto " + message + ": avvio la battaglia!");
            PlayerPrefs.SetString("monsterToBattle", battleMonster);
            SceneHelper.LoadScene("Battle");
        }
        
    }


    private string GetMonsterName(string pieceName) {
        
        // Mapping dei nomi delle pedine agli equivalenti nomi dei mostri
        switch (pieceName) {
            case "WhiteKing":
                return "Skeleton";
            case "WhiteQueen":
                return "Witch";
            case "WhiteRook":
                return "WhiteRook";
            case "WhiteBishop":
                return "WhiteBishop";
            case "WhiteKnight":
                return "WhiteKnight";
            case "WhitePawn":
                return "WhitePawn";
            case "BlackKing":
                return "Angel";
            case "BlackQueen":
                return "BlackQueen";
            case "BlackRook":
                return "BlackRook";
            case "BlackBishop":
                return "BlackBishop";
            case "BlackKnight":
                return "BlackKnight";
            case "BlackPawn":
                return "BlackPawn";
            default:
                return "Unknown";
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
