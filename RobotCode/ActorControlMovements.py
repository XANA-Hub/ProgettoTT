import pykka

#import ActorsConfig
#import RobotWheelsControl

class Actor(pykka.ThreadingActor):
    def __init__(self):
        super().__init__()
        #self.pwm = RobotWheelsControl.InitPwm()

    def on_receive(self, message):
        if "Forward" in message:
            self.Forward(message)
        elif "Backward" in message:
            self.Backward(message)
        elif "Rotate_Left" in message:
            self.Rotate_Left(message)
        elif "Rotate_Right" in message:
            self.Rotate_Right(message)
        elif "Terminate" in message:
            print("--RobotControlMovement-- terminating")
            self.stop()
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)


    def Forward(self, message):
        if "Start" in message:
            print("--RobotControlMovement Forward-- " + message)
            #RobotWheelsControl.MoveForward(self.pwm)
        elif "Stop" in message:
            print("--RobotControlMovement Forward-- " + message)
            #RobotWheelsControl.Stop(self.pwm)
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)


    def Backward(self, message):
        if "Start" in message:
            print("--RobotControlMovement Backward-- " + message)
            #RobotWheelsControl.MoveBackward(self.pwm)
        elif "Stop" in message:
            print("--RobotControlMovement Backward-- " + message)
            #RobotWheelsControl.Stop(self.pwm)
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)


    def Rotate_Left(self, message):
        if "Start" in message:
            print("--RobotControlMovement Rotate_Left-- " + message)
            #RobotWheelsControl.SteerLeft(self.pwm)
        elif "Stop" in message:
            print("--RobotControlMovement Rotate_Left-- " + message)
            #RobotWheelsControl.Stop(self.pwm)
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)


    def Rotate_Right(self, message):
        if "Start" in message:
            print("--RobotControlMovement Rotate_Right-- " + message)
            #RobotWheelsControl.SteerRight(self.pwm)
        elif "Stop" in message:
            print("--RobotControlMovement Rotate_Right-- " + message)
            #RobotWheelsControl.Stop(self.pwm)
        else:
            print("--RobotControlMovement ERROR-- malformed message: " + message)