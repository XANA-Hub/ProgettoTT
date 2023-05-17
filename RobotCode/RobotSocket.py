import socket
import ActorsConfig
import sys

HOST = "192.168.178.69"      # Standard loopback interface address (localhost)
PORT = 25565          # Port to listen on (non-privileged ports are > 1023)

connection = None
video_socket = None

def startSocket():
    try:
        print("Avvio del server con indirizzo "+HOST+"..")
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.setsockopt(socket.SOL_SOCKET,socket.SO_REUSEADDR,1)
            s.bind((HOST, PORT))
            s.listen(1)
            print("Server socket pronto")
            conn, addr = s.accept()
            print("Ricevuta richiesta")
            s.close()
            with conn:
                global connection
                connection = conn
                print(f"Connected by {addr}")
                #startVideoSocket(addr)
                #ActorsConfig.actorCore_ref.tell("ID:init;TYPE:Camera;BODY:Start")
                print("Inizio il server loop")
                while True:
                    data = conn.recv(1024)
                    if not data:
                        break
                    print(data.decode("utf-8"))
                    ActorsConfig.actorCore_ref.tell(data.decode("utf-8"))
    except KeyboardInterrupt:
        print("Rilevata interruzione utente")
        ActorsConfig.actorCore_ref.tell("ID:term;TYPE:Stop;BODY:None")
        #Non si interrompe credo per processi in background degli attori
        sys.exit()

    except:
        print("Rilevata eccezione")
        resetConfiguration()
        print("Reset connessioni effettuato")
        startSocket()

def startVideoSocket(addr):
    global video_socket
    print("Preparo la socket UDP")
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    s.connect((addr, PORT))
    video_socket = s

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