// This work is licensed under the Creative Commons Attribution-ShareAlike 4.0 International License. 
// To view a copy of this license, visit http://creativecommons.org/licenses/by-sa/4.0/ 
// or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ClientTCPManager : MonoBehaviour {  	
	
    public string ipAddress = "192.168.178.69";
    public int port = 25565;
	private TcpClient socketConnection;
    


	// Use this for initialization 	
	private void Start () {

        // Imposta l'indirizzo IP e la porta del server a cui connettersi   
        ConnectToServer(ipAddress, port);

	}  
    private void ConnectToServer(string ipAddress, int port) {
        try {
            socketConnection = new TcpClient();
            socketConnection.Connect(ipAddress, port);
            Debug.Log("Connesso al server!");
        }
        catch (Exception e) {
            Debug.Log("Errore durante la connessione al server: " + e.Message);
        }
    }

	/// <summary> 	
	/// Runs in background clientReceiveThread; Listens for incomming data. 	
	/// </summary>     
	private void ListenForData() { 		
		try { 			

			Byte[] bytes = new Byte[1024];   

			while (true) { 

				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream()) { 					
					int length; 		

					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 

						var incommingData = new byte[length]; 						
						Array.Copy(bytes, 0, incommingData, 0, length);

						// Convert byte array to string message. 						
						string serverMessage = Encoding.UTF8.GetString(incommingData); 						
						Debug.Log("server message received as: " + serverMessage); 					
					} 				
				} 			
			}         
		}         
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}  


	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	public void SendData(string clientMessage) {     
        
		if (socketConnection == null) {             
			return;         
		}  		
        
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream(); 			
			if (stream.CanWrite) {                 	

				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.UTF8.GetBytes(clientMessage); 				
				
                // Write byte array to socketConnection stream.                 
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

}