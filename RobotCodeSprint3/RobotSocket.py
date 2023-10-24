import socket
from time import sleep
import ActorsConfig
import sys
import fcntl
import struct

TEST = True

HOST = "127.0.0.1"                      # Standard loopback interface address (localhost)
PORT = 25565                            # Standard port to listen on (non-privileged ports are > 1023)

socketVideo = None

def get_ip_address(ifname):
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    return socket.inet_ntoa(fcntl.ioctl(
        s.fileno(),
        0x8915,  # SIOCGIFADDR
        struct.pack('256s', bytes(ifname[:15], 'utf-8'))
    )[20:24])

def startSocket(ip = HOST, port = PORT):
    global conn
    ip = get_ip_address('wlan0')
    try:
        print("Avvio del server TCP con indirizzo " + ip + ":" + str(port))
        socketTCP = socket.socket(socket.AF_INET, socket.SOCK_STREAM)           #creazione della socket tcp
        with socketTCP as s:
            s.setsockopt(socket.SOL_SOCKET,socket.SO_REUSEADDR,1)
            s.bind((ip, port))
            s.listen(1)
            print("Server socket ready")

            # loop per collegare più client uno dopo l'altro
            while True:
                conn, addr = s.accept()
                
                conn.send(b"ready")  #questa send "sblocca" il client in attesa di poter mandare i comandi, serve per la gestione di più utenti che cercano di collegarsi contemporaneamente
                print("User "+str(addr)+" logged in")
                
                #Message format: "ID:init;TYPE:Camera"
                while True:     #loop per la ricezione dei messaggi
                    print("------------------------------------------------------------")
                    data = conn.recv(1024)
                    if not data:        #data is null, the client has crashed
                        ActorsConfig.actorCore_ref.tell("ID:000;TYPE:Client Crashed;BODY:None")     #this will tell the core to handle the crash
                        print("User "+str(addr)+" crashed")
                        break
                    data = data.decode("utf-8")
                    if TEST: print("data: " + data)
                    ActorsConfig.actorCore_ref.tell(data)   #Nota, in caso di disconnect, giriamo il messaggio al core per la terminazione dello stream video
                    if "TYPE:Disconnect" in data:           #this way the connection closes and the socket goes back to the accept
                        print("User "+str(addr)+" logged out")
                        break
                    
    except KeyboardInterrupt:
        print("------------------------------------------------------------")
        print("User Interrupt detected")
        ActorsConfig.actorCore_ref.tell("ID:term;TYPE:Terminate;BODY:None") #will terminate all actors before turning off
        sys.exit()



def sendAIresponse(response):
    print("socket converting to bytes...")
    print(response)
    response = bytes(response, 'utf-8')         #convert string to bytes
    print("socket sending...")
    print(conn)
    print(response)
    conn.send(response)
    print("finito di inviare")

def startVideoSocket(ip, port):
    socketVideo = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    socketVideo.connect((ip, port))
    return socketVideo.makefile('wb')

def closeVideoSocket():
    print("Closing video socket")
    if socketVideo is not None:         #da rivedere questo IF
        socketVideo.close()

def sendImg(ip, imgPath):
    socketVideo = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    with open(imgPath, 'rb') as file:
        file_data = file.read()
        print("Immagine letta")
        socketVideo.sendto(file_data,(ip,PORT))
        socketVideo.close()