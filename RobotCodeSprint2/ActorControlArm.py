import pykka
import RobotArmControl

TEST = True

class Actor(pykka.ThreadingActor):
    def __init__(self):
        super().__init__()
        self.pwm = RobotArmControl.InitPwm()
        RobotArmControl.initializeArm(self.pwm)
        RobotArmControl.initializeClaw(self.pwm)
        RobotArmControl.resetPosition(self.pwm)

    def on_receive(self, message):
        if "Up" in message:
            self.ArmUp(message)
        elif "Down" in message:
            self.ArmDown(message)
        elif "Grab" in message:
            self.Grab(message)
        elif "Release" in message:
            self.Release(message)

        elif "Disconnect" in message:
            if TEST: print("--RobotControlArm-- disconnecting")
            self.ResetPos()
        elif "Client Crashed" in message:
            if TEST: print("--RobotControlArm-- client crashed")
            self.ResetPos()
        elif "Terminate" in message:            #turn off ActorControlArm
            print("--RobotControlArm-- terminating")
            self.Terminate()
            self.stop()
        else:
            print("--RobotControlArm ERROR-- malformed message: " + message)


    def ArmUp(self, message):
        if TEST: print("--RobotControlArm ArmUp-- " + message)
        RobotArmControl.riseArm(self.pwm)


    def ArmDown(self, message):
        if TEST: print("--RobotControlArm ArmDown-- " + message)
        RobotArmControl.lowArm(self.pwm)


    def Grab(self, message):
        if TEST: print("--RobotControlArm Grab-- " + message)
        RobotArmControl.closeClaw(self.pwm)
        

    def Release(self, message):
        if TEST: print("--RobotControlArm Release-- " + message)
        RobotArmControl.openClaw(self.pwm)

    def ResetPos(self):
        RobotArmControl.resetPosition(self.pwm)

    def Terminate(self):
        RobotArmControl.terminate(self.pwm)
