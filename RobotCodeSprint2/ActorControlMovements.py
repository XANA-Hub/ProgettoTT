import pykka
import RobotWheelsControl

TEST = True

class Actor(pykka.ThreadingActor):
    def __init__(self):
        super().__init__()
        self.pwm = RobotWheelsControl.InitPwm()

    def on_receive(self, message):
        if "Forward" in message:
            self.Forward(message)
        elif "Backward" in message:
            self.Backward(message)
        elif "Rotate_Left" in message:
            self.Rotate_Left(message)
        elif "Rotate_Right" in message:
            self.Rotate_Right(message)

        elif "Disconnect" in message:
            if TEST: print("--RobotControlMovement-- disconnecting")
            self.ResetPos()
        elif "Client Crashed" in message:
            if TEST: print("--RobotControlMovement-- client crashed")
            self.ResetPos()
        elif "Terminate" in message:    #turn off ActorControlMovement
            if TEST: print("--RobotControlMovement-- terminating")
            self.ResetPos()
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)


    def Forward(self, message):
        if "Start" in message:
            if TEST: print("--RobotControlMovement Forward-- " + message)
            RobotWheelsControl.MoveForward(self.pwm)
        elif "Stop" in message:
            if TEST: print("--RobotControlMovement Forward-- " + message)
            RobotWheelsControl.StopForward(self.pwm)
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)


    def Backward(self, message):
        if "Start" in message:
            if TEST: print("--RobotControlMovement Backward-- " + message)
            RobotWheelsControl.MoveBackward(self.pwm)
        elif "Stop" in message:
            if TEST: print("--RobotControlMovement Backward-- " + message)
            RobotWheelsControl.StopBackward(self.pwm)
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)


    def Rotate_Left(self, message):
        if "Start" in message:
            if TEST: print("--RobotControlMovement Rotate_Left-- " + message)
            RobotWheelsControl.SteerLeft(self.pwm)
        elif "Stop" in message:
            if TEST: print("--RobotControlMovement Rotate_Left-- " + message)
            RobotWheelsControl.StopLeft(self.pwm)
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)


    def Rotate_Right(self, message):
        if "Start" in message:
            if TEST: print("--RobotControlMovement Rotate_Right-- " + message)
            RobotWheelsControl.SteerRight(self.pwm)
        elif "Stop" in message:
            if TEST: print("--RobotControlMovement Rotate_Right-- " + message)
            RobotWheelsControl.StopRight(self.pwm)
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)

    def ResetPos(self):
        RobotWheelsControl.Stop(self.pwm)