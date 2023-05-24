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
        elif "Terminate" in message:
            print("--RobotControlArm-- terminating")
            self.stop()
        else:
            print("--RobotControlArm ERROR-- malformed message: " + message)


    def ArmUp(self, message):
        pass


    def ArmDown(self, message):
        pass


    def Grab(self, message):
        pass
        

    def Release(self, message):
        pass