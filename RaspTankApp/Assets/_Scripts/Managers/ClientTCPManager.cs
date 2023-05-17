using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ClientTCPManager : MonoBehaviour {  	
	
    public string ipAddress = "192.168.178.69";
    public int port = 25565;

	private TcpClient client;
	private Thread clientReceiveThread; 	
    

	// Use this for initialization 	
	private void Start () {

        // Imposta l'indirizzo IP e la porta del server a cui connettersi   
        ConnectToServer();

	}  


	private void ConnectToServer () { 		
		try {  			
			clientReceiveThread = new Thread (new ThreadStart(ListenForData)); 			
			clientReceiveThread.IsBackground = true; 			
			clientReceiveThread.Start();  

			Debug.Log("TCP: Connessione al server avvenuta con successo!");		
		} 		
		catch (Exception e) { 			
			Debug.Log("On client connect exception " + e); 		
		} 	
	}  	


	private void ListenForData() { 		
		try { 			

			client = new TcpClient(ipAddress, port);  			
			Byte[] bytes = new Byte[1024];   

			while (true) { 			

				// Get a stream object for reading 				
				using (NetworkStream stream = client.GetStream()) { 

					int length; 			

					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 

						var incommingData = new byte[length]; 						
						Array.Copy(bytes, 0, incommingData, 0, length); 

						// Convert byte array to string message. 						
						string serverMessage = Encoding.ASCII.GetString(incommingData); 						
						Debug.Log("Server message received as: " + serverMessage); 					
					} 				
				} 			
			}         
		}         
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}  	

	public void SendData(string clientMessage) {     
        
		if (client == null) {             
			return;         
		}  		
        
		try {
				
			// Get a stream object for writing. 			
			NetworkStream stream = client.GetStream(); 			
			if (stream.CanWrite) {                 	

				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.UTF8.GetBytes(clientMessage); 				
				
                // Write byte array to client stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);                 
				Debug.Log("Client sent his message - should be received by server");             
			}         
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	} 

    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }

	void OnApplicationQuit() {

        if (client != null) 
        	client.Close();
    }

}