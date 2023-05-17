import pykka

import RobotCameraControl

class Actor(pykka.ThreadingActor):
    def on_receive(self, message):
        if "Start" in message:
            print("--RobotVideoHandler-- initializing")
            self.start()
        elif "Stop" in message:
            print("--RobotVideoHandler-- terminating")
            self.stop()
        else:
            print(message)

def stop():
    RobotCameraControl.stopVideo()

def start():
    RobotCameraControl.sendVideo()
