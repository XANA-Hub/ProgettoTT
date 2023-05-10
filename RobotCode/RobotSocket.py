import socket
import ActorsConfig

HOST = "127.0.0.1"      # Standard loopback interface address (localhost)
PORT = 65432            # Port to listen on (non-privileged ports are > 1023)

def startSocket():
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.bind((HOST, PORT))
        s.listen()
        print("server socket pronto")
        conn, addr = s.accept()
        with conn:
            print(f"Connected by {addr}")
            while True:
                data = conn.recv(1024)
                if not data:
                    break
                ActorsConfig.actorCore_ref.tell(data.decode("utf-8"))
            
