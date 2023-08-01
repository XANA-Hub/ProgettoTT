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
            #Body = messageToken[0]
            Address = messageToken[1]
            if TEST: print("--RobotVideoHandler-- starting video transmission with address: " + str(Address))
            self.startVideo(Address)
            
        elif "Disconnect" in message:
            if TEST: print("--RobotVideoHandler-- stopping video transmission")
            self.stopVideo()
        elif "Client Crashed" in message:  #check sintassi e check se fare le stesse cose che fa Disconnect
            if TEST: print("--RobotVideoHandler-- stopping video transmission")
            self.stopVideo()

        elif "Terminate" in message:
            if TEST: print("--RobotVideoHandler-- terminating")
            #potrebbe venire invocata quando il video è in corso? devo fare una chiusura della socket video?
            #un admin potrebbe spegnere tutto in qualsiasi momento, meglio chiuderla
            self.stopVideo()
            self.stop()                                 # teminazione dell'attore
        else:
            print("--RobotVideoHandler ERROR-- malformed message: " + message)
            

    def startVideo(self, address):
        if TEST: print("--ActorVideoHandler-- start stream video iniziata")
        messageToken = address.split(":")                  # 192.168.n.n:PORTA
        stream_file = rs.startVideoSocket(messageToken[0], int(messageToken[1]))
        RobotCameraControl.sendVideo(self.camera, stream_file)
        if TEST: print("--ActorVideoHandler-- start stream video conclusa")

        
    def stopVideo(self):
        RobotCameraControl.stopVideo(self.camera)
        rs.closeVideoSocket()
        if TEST: print("--ActorVideoHandler-- stop stream video conclusa")
        #se invoco la close quando non esiste la socket? Possibile da "Terminate"


