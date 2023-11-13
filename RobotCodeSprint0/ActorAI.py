import pykka

class Actor(pykka.ThreadingActor):
    def on_receive(self, message):
        print("--RobotAI-- " + message)
        if "Terminate" in message:
            print("--RobotAI-- terminating")
            self.stop()
        elif "Identify_Current" in message:
            pass
        else:
            print("--RobotAI ERROR-- malformed message: " + message)