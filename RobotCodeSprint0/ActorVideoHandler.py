from time import sleep
import pykka

import RobotSocket as rs
import RobotCameraControl

class Actor(pykka.ThreadingActor):
    def __init__(self):
        super().__init__()
        self.camera = RobotCameraControl.initCamera()

    def on_receive(self, message):
        print("-----------------------ricevuto messaggio: " + message)
        if "Start" in message:
            messageToken = message.split(";")           # Start;192.168.n.n:PORTA
            Body = messageToken[0]
            Address = messageToken[1]
            print("--RobotVideoHandler-- starting video transmission with address: ")
            self.startVideo(Address)
        elif "Disconnect" in message:
            print("--RobotVideoHandler-- stopping video transmission")
            self.stopVideo()
        elif "Terminate" in message:
            print("--RobotVideoHandler-- terminating")
            self.stop()                                 # teminazione dell'attore
        else:
            print("--RobotVideoHandler ERROR-- malformed message: " + message)
            

    def startVideo(self, address):
        messageToken = address.split(":")                  # 192.168.n.n:PORTA
        rs.startVideoSocket(messageToken[0], messageToken[1])
        RobotCameraControl.sendVideo(self.camera, rs.socketVideo)

        
    def stopVideo(self):
        RobotCameraControl.stopVideo(self.camera)
        rs.closeVideoSocket()


