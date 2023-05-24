import pykka

#import RobotCameraControl
#import RobotSocket

class Actor(pykka.ThreadingActor):
    def __init__(self):
        super().__init__()
        #self.camera = RobotCameraControl.initCamera()

    def on_receive(self, message):
        if "Start" in message:
            print("--RobotVideoHandler-- starting video transmission")
            self.startVideo()
        elif "Stop" in message:
            print("--RobotVideoHandler-- stopping video transmission")
            self.stopVideo()
        elif "Terminate" in message:
            print("--RobotVideoHandler-- terminating")
            self.stop() #teminazione dell'attore
        else:
            print("--RobotVideoHandler ERROR-- malformed message: " + message)
            

    def stopVideo(self):
        pass
        #RobotCameraControl.stopVideo(self.camera)

    def startVideo(self):
        pass
        #RobotCameraControl.sendVideo(self.camera, RobotSocket.video_socket)
        
