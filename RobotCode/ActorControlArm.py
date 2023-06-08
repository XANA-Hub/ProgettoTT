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
        print("--RobotControlArm ArmUp-- " + message)
        pass


    def ArmDown(self, message):
        print("--RobotControlArm ArmDown-- " + message)
        pass


    def Grab(self, message):
        print("--RobotControlArm Grab-- " + message)
        pass
        

    def Release(self, message):
        print("--RobotControlArm Release-- " + message)
        pass