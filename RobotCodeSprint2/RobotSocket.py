import socket
from time import sleep
import ActorsConfig
import sys

TEST = True

HOST = "192.168.1.16"                      # Standard loopback interface address (localhost)
PORT = 25565                            # Standard port to listen on (non-privileged ports are > 1023)

socketVideo = None

def startSocket(ip = HOST, port = PORT):
    try:
        print("Avvio del server TCP con indirizzo " + ip + ":" + str(port))
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:            #creazione della socket tcp
            s.setsockopt(socket.SOL_SOCKET,socket.SO_REUSEADDR,1)
            s.bind((ip, port))
            s.listen(1)
            print("Server socket pronto")
            print("------------------------------------------------------------")

            # loop per collegare più client uno dopo l'altro
            while True:
                conn, addr = s.accept()
                
                conn.send(b"ready")  #questa send "sblocca" il client in attesa di poter mandare i comandi, serve per la gestione di più utenti che cercano di collegarsi contemporaneamente
                print("Collegato utente: " + str(addr))
                
                #Message format: "ID:init;TYPE:Camera"
                while True:     #loop per la ricezione dei messaggi
                    data = conn.recv(1024)
                    if not data:        #data is null, the client has crashed
                        ActorsConfig.actorCore_ref.tell("ID:000;TYPE:Client Crashed;BODY:None")     #this will tell the core to handle the crash
                        print("User crashed")
                        print("------------------------------------------------------------")
                        break
                    data = data.decode("utf-8")
                    if TEST: print("data: " + data)
                    ActorsConfig.actorCore_ref.tell(data)   #Nota, in caso di disconnect, giriamo il messaggio al core per la terminazione dello stream video
                    if "TYPE:Disconnect" in data:           #this way the connection closes and the socket goes back to the accept
                        print("User scollegato")
                        print("------------------------------------------------------------")
                        break
                    
    except KeyboardInterrupt:
        print("------------------------------------------------------------")
        print("Rilevata interruzione utente")
        ActorsConfig.actorCore_ref.tell("ID:term;TYPE:Terminate;BODY:None") #will terminate all actors before turning off
        sys.exit()



def startVideoSocket(ip, port):
    socketVideo = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    socketVideo.connect((ip, port))
    return socketVideo.makefile('wb')

def closeVideoSocket():
    print("closing video socket")
    if socketVideo is not None:         #da rivedere questo IF
        socketVideo.close()

def sendImg(ip, imgPath):
    socketVideo = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    with open(imgPath, 'rb') as file:
        file_data = file.read()
        print("Immagine letta")
        socketVideo.sendto(file_data,(ip,PORT))
        socketVideo.close()