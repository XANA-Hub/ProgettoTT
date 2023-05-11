import pykka

class Actor(pykka.ThreadingActor):
    def on_receive(self, message):
        if "Up" in message:
            self.ArmUp(message)
        elif "Down" in message:
            self.ArmDown(message)
        elif "Grab" in message:
            self.Grab(message)
        elif "Release" in message:
            self.Release(message)
        elif "Stop" in message:
            print("--RobotControlArm-- terminating")
            self.stop()
        else:
            print("--RobotControlArm ERROR-- malformed message: " + message)


    def ArmUp(self, message):
        print(message)


    def ArmDown(self, message):
        print(message)


    def Grab(self, message):
        print(message)
        

    def Release(self, message):
        print(message)