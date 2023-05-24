using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
    
public class VideoReceiverManagerOLD : MonoBehaviour {

    private UdpConnection udpConnection;
    public string sendIp = "192.168.178.69";
    public int receivePort = 25565;
    public int sendPort = 25565;
 
    private void Start() {
 
        udpConnection = new UdpConnection();
        udpConnection.StartConnection(sendIp, sendPort, receivePort);
    }
 
    private void Update() {

        foreach (var message in udpConnection.getMessages()) {
            Debug.Log(message);
        }
 
        //udpConnection.Send("Hi!");
    }
 
    private void OnDestroy() {
        udpConnection.Stop();
    }

    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }

    
}
    
