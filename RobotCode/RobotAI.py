import pykka

import RobotSocket

class Actor(pykka.ThreadingActor):
    def on_receive(self, message):
        if "Start" in message:
            print("--RobotAI-- initializing")
            RobotSocket.sendResult("Risposta plausibile")
        if "Stop" in message:
            print("--RobotAI-- terminating")
            self.stop()
        else: 
            print(message)