from time import sleep
import pykka

import RobotSocket as rs
import RobotCameraControl

TEST = True

class Actor(pykka.ThreadingActor):
    def __init__(self):
        super().__init__()
        self.camera = RobotCameraControl.initCamera()

    def on_receive(self, message):
        if TEST: print("--RobotVideoHandler-- ricevuto messaggio: " + message) 

        if "Start" in message:
            messageToken = message.split(";")           # Start;192.168.n.n:PORTA
            Address = messageToken[1]
            if TEST: print("--RobotVideoHandler-- starting video transmission with address: " + str(Address))
            self.startVideo(Address)
            
        #NOTA: for now Crash e Disconnection will be handled in the same way
        elif "Disconnect" in message:
            if TEST: print("--RobotVideoHandler-- stopping video transmission")
            self.stopVideo()
        elif "Client Crashed" in message:
            if TEST: print("--RobotVideoHandler-- stopping video transmission")
            self.stopVideo()

        elif "Terminate" in message:
            if TEST: print("--RobotVideoHandler-- terminating")
            self.stopVideo()
            self.stop()                                 # teminazione dell'attore
        else:
            print("--RobotVideoHandler ERROR-- malformed message: " + message)
            

    def startVideo(self, address):
        messageToken = address.split(":")                  # 192.168.n.n:PORTA
        stream_file = rs.startVideoSocket(messageToken[0], int(messageToken[1]))
        RobotCameraControl.sendVideo(self.camera, stream_file)
        if TEST: print("--ActorVideoHandler-- stream video started")

        
    def stopVideo(self):
        RobotCameraControl.stopVideo(self.camera)
        rs.closeVideoSocket()
        if TEST: print("--ActorVideoHandler--  stream video closed")
        #se invoco la close quando non esiste la socket? Possibile da "Terminate" <-- DA TESTARE


