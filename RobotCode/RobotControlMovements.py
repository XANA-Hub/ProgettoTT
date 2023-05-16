import pykka

import ActorsConfig
import RobotWheelsControl

class Actor(pykka.ThreadingActor):
    def __init__(self):
        super().__init__()
        self.pwm = RobotWheelsControl.InitPwm

    def on_receive(self, message):
        if "Forward" in message:
            self.Forward(message)
        elif "Backward" in message:
            self.Backward(message)
        elif "Rotate_Left" in message:
            self.Rotate_Left(message)
        elif "Rotate_Right" in message:
            self.Rotate_Right(message)
        elif "Stop" in message:
            print("--RobotControlMovement-- terminating")
            self.stop()
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)


    def Forward(self, message):
        if "Start" in message:
            RobotWheelsControl.MoveForward(self.pwm)
            print(message)
            pass
        elif "Stop" in message:
            RobotWheelsControl.StopForward(self.pwm)
            print(message)
            pass
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)


    def Backward(self, message):
        if "Start" in message:
            RobotWheelsControl.MoveBackward(self.pwm)
            print(message)
            pass
        elif "Stop" in message:
            RobotWheelsControl.StopBackward(self.pwm)
            print(message)
            pass
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)


    def Rotate_Left(self, message):
        if "Start" in message:
            RobotWheelsControl.SteerLeft(self.pwm)
            print(message)
            pass
        elif "Stop" in message:
            RobotWheelsControl.StopLeft(self.pwm)
            print(message)
            pass
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)


    def Rotate_Right(self, message):
        if "Start" in message:
            RobotWheelsControl.SteerRight(self.pwm)
            print(message)
            pass
        elif "Stop" in message:
            RobotWheelsControl.StopRight(self.pwm)
            print(message)
            pass
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)