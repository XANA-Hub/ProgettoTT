import pykka

class Actor(pykka.ThreadingActor):
    def on_receive(self, message):
        if "Forward" in message:
            self.Forward(message)
        elif "Backward" in message:
            self.Backward(message)
        elif "Rotate_Left" in message:
            self.Rotate_Left(message)
        elif "Rotate_Right" in message:
            self.Rotate_Right(message)
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)


    def Forward(message):
        if "Start" in message:
            pass
        elif "Stop" in message:
            pass
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)

    def Backward(message):
        if "Start" in message:
            pass
        elif "Stop" in message:
            pass
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)

    def Rotate_Left(message):
        if "Start" in message:
            pass
        elif "Stop" in message:
            pass
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)

    def Rotate_Right(message):
        if "Start" in message:
            pass
        elif "Stop" in message:
            pass
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)