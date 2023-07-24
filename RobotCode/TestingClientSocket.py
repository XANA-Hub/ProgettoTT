import socket
from time import sleep

HOST = "192.168.1.177"      # The server's hostname or IP address
PORT = 25565                # The port used by the server

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.connect((HOST, PORT))
    print("connected with socket server")

    print("in attesa di risposta dal robot")
    s.recv(1024)
    print("robot pronto inizio invio dei comandi")

    #test message
    s.sendall(b"ID:000;TYPE:Start;BODY:127.0.0.1:5004")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Movement;BODY:MoveForward_Start")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Movement;BODY:MoveForward_Stop")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Movement;BODY:Rotate_Left_Start")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Movement;BODY:Rotate_Left_Stop")
    sleep(2)
    s.sendall(b"ID:000;TYPE:AI_Recognition;BODY:Identify_Current")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Robot_Arm;BODY:Down")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Robot_Arm;BODY:Grab")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Disconnect;BODY:EndDelVideoStream")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Terminate;BODY:TerminazionePerTesting")