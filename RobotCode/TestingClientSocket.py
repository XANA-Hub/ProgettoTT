import socket
from time import sleep

HOST = "127.0.0.1"      # The server's hostname or IP address
PORT = 65432            # The port used by the server

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.connect((HOST, PORT))
    print("connected with socket server")

    #test message
    s.sendall(b"ID:000;TYPE:Start;BODY:camera rollig, CHACK ACTION")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Movement;BODY:MoveForward_Start")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Movement;BODY:MoveForward_Stop")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Movement;BODY:Rotate_Left_Start")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Movement;BODY:Rotate_Left_Stop")
    sleep(2)
    s.sendall(b"ID:000;TYPE:AI_Recognition;BODY:searching for an enemy")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Robot_Arm;BODY:lowering arm")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Robot_Arm;BODY:grabbing enemy")
    sleep(2)
    s.sendall(b"ID:000;TYPE:Stop;BODY:terminating actors")
