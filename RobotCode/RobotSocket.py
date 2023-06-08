import socket
from time import sleep
import ActorsConfig
import sys

HOST = "localhost"                      # Standard loopback interface address (localhost)
PORT = 25565                            # Port to listen on (non-privileged ports are > 1023)

def startSocket(ip = HOST, port = PORT):
    try:
        print("Avvio del server TCP con indirizzo " + ip + ":" + str(port))
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.setsockopt(socket.SOL_SOCKET,socket.SO_REUSEADDR,1)
            s.bind((ip, port))
            s.listen(1)
            print("Server socket pronto")

            # loop per collegare più client uno dopo l'altro
            while True:
                conn, addr = s.accept()
                conn.send(b"ready")  #questa send "sblocca" il clinet in attesa di poter mandare i comandi, serve per la gestione di più utenti che cercano di collegarsi contemporaneamente
                print("Collegato utente: " + str(addr))
                #startVideoSocket(addr[0])
                #ID:init;TYPE:Camera;
                #ActorsConfig.actorVideoHandler_ref.tell("BODY:Start")
                while True:     #loop per la ricezione dei messaggi
                    data = conn.recv(1024)
                    if not data:
                        #qua dentro posso capire quando il client crasha
                        ActorsConfig.actorCore_ref.tell("ID:000;TYPE:Client Crashed;BODY:None")     #this will tell the core to handle the crash
                        break
                    data = data.decode("utf-8")
                    print(data)
                    ActorsConfig.actorCore_ref.tell(data) #Anche la stop deve arrivare al core per far finire lo stream video e chiudere la relativa socket
                    if "Type:Stop" in data:  #this way the connection closes and the socket goes back to the accept
                        break
                print("User scollegato")
                    
    except KeyboardInterrupt:
        print("Rilevata interruzione utente")
        ActorsConfig.actorCore_ref.tell("ID:term;TYPE:Terminate;BODY:None")
        sys.exit()
        '''
    except:
        print("Rilevata eccezione")
        resetConfiguration()
        print("Reset connessioni effettuato")
        startSocket()
        

def resetConfiguration():
    global connection
    global video_socket
    if connection is not None:
        connection.close()
        connection = None
    if video_socket is not None:    
        video_socket.close()
        video_socket = None

def sendResult(data):
    connection.sendall(data.encode('utf-8'))
    '''

def startVideoSocket():
    global socketVideo
    socketVideo = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

def closeVideoSocket():
    print("closing video socket")
    socketVideo.close()




def sendVideoData(addr, port):
    print("start sending to address: " + addr + ":" + port)
    while True:
        socketVideo.sendto(b"Test", (addr, int(port)))
        sleep(3)