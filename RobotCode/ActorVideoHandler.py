from time import sleep
import pykka
import RobotSocket as rs

#import RobotCameraControl
#import RobotSocket

class Actor(pykka.ThreadingActor):
    def __init__(self):
        super().__init__()
        #self.camera = RobotCameraControl.initCamera()

    def on_receive(self, message):
        messageToken = message.split(";")    #Start;192.168.n.n:PORTA
        Body = messageToken[0]
        Address = messageToken[1]
        if "Start" in Body:
            print("--RobotVideoHandler-- starting video transmission with address: ")
            self.startVideo(Address)
        elif "Disconnect" in message:
            print("--RobotVideoHandler-- stopping video transmission")
            self.stopVideo()
        elif "Terminate" in message:
            print("--RobotVideoHandler-- terminating")
            self.stop() #teminazione dell'attore
        else:
            print("--RobotVideoHandler ERROR-- malformed message: " + message)
            

    def stopVideo(self):
        #RobotCameraControl.stopVideo(self.camera)
        rs.closeVideoSocket()

    def startVideo(self, address):
        messageToken = address.split(":")
        print("provo a far partire la funzione:")
        rs.startVideoSocket()
        rs.sendVideoData(messageToken[0], messageToken[1])
        #RobotCameraControl.sendVideo(self.camera, RobotSocket.socketVideo

