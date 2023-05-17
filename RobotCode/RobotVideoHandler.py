import pykka

import RobotCameraControl
import RobotSocket

class Actor(pykka.ThreadingActor):
    def __init__(self):
        super().__init__()
        self.camera = RobotCameraControl.initCamera()

    def on_receive(self, message):
        if "Start" in message:
            print("--RobotVideoHandler-- initializing")
            self.startVideo()
        elif "Stop" in message:
            print("--RobotVideoHandler-- terminating")
            self.stopVideo()
        else:
            print(message)

    def stopVideo(self):
        RobotCameraControl.stopVideo(self.camera)

    def startVideo(self):
        RobotCameraControl.sendVideo(self.camera, RobotSocket.video_socket)
