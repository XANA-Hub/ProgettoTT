import pykka

class Actor(pykka.ThreadingActor):
    def on_receive(self, message):
        if "Stop" in message:
            print("--RobotVideoHandler-- terminating")
            self.stop()
        else:
            print(message)