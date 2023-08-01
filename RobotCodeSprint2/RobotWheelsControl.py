import RPi.GPIO as GPIO
import time
import pigpio

pwm = None
rangePWM=4000

#Ruota sinistra
pinLB = 24
pinLF = 23
#Ruota destra
pinRB = 6
pinRF = 5

def Stop(pwm):
    pwm.set_mode(pinLF, 0)
    pwm.set_mode(pinRF, 0)
    pwm.set_mode(pinLB, 0)
    pwm.set_mode(pinRB, 0)

def StopForward(pwm):
    pwm.set_mode(pinLF, 0)
    pwm.set_mode(pinRF, 0)
    
def StopBackward(pwm):
    pwm.set_mode(pinLB, 0)
    pwm.set_mode(pinRB, 0)

def StopLeft(pwm):
    pwm.set_mode(pinLB, 0)
    pwm.set_mode(pinRF, 0)

def StopRight(pwm):
    pwm.set_mode(pinLF, 0)
    pwm.set_mode(pinRB, 0)

def MoveForward(pwm):
        InitLeftWheelForward(pwm)
        InitRightWheelForward(pwm)

def MoveBackward(pwm):
        InitLeftWheelBackward(pwm)
        InitRightWheelBackward(pwm)

def SteerLeft(pwm):
        InitLeftWheelBackward(pwm)
        InitRightWheelForward(pwm)

def SteerRight(pwm):
        InitLeftWheelForward(pwm)
        InitRightWheelBackward(pwm)
        
def InitLeftWheelForward(pwm):
        pwm.set_mode(pinLF, pigpio.OUTPUT)
        pwm.set_PWM_frequency(pinLF, 50)
        pwm.set_PWM_range(pinLF, rangePWM)
        pwm.set_PWM_dutycycle(pinLF, rangePWM)

def InitLeftWheelBackward(pwm):
        pwm.set_mode(pinLB, pigpio.OUTPUT)
        pwm.set_PWM_frequency(pinLB, 50)
        pwm.set_PWM_range(pinLB, rangePWM)
        pwm.set_PWM_dutycycle(pinLB, rangePWM)

def InitRightWheelForward(pwm):
        pwm.set_mode(pinRF, pigpio.OUTPUT)
        pwm.set_PWM_frequency(pinRF, 200)
        pwm.set_PWM_range(pinRF, rangePWM)
        pwm.set_PWM_dutycycle(pinRF, 0)

def InitRightWheelBackward(pwm):
        pwm.set_mode(pinRB, pigpio.OUTPUT)
        pwm.set_PWM_frequency(pinRB, 200)
        pwm.set_PWM_range(pinRB, rangePWM)
        pwm.set_PWM_dutycycle(pinRB, 0)

def PrintModes(pwm):
        print("LF: "+str(pwm.get_mode(pinLF)))
        print("LB: "+str(pwm.get_mode(pinLB)))
        print("RF: "+str(pwm.get_mode(pinRF)))
        print("RB: "+str(pwm.get_mode(pinRB)))

def InitPwm():
        return pigpio.pi()
      

def test():
    try:
        print ('Test')
        pwm = InitPwm()
        PrintModes(pwm)
        print("Prova forward")
        MoveForward(pwm)
        PrintModes(pwm)
        time.sleep(3)
        print("Stop forward")
        StopForward(pwm)
        time.sleep(3)
        print("Prova backward")
        MoveBackward(pwm)
        PrintModes(pwm)
        time.sleep(3)
        print("Stop Backward")
        StopBackward(pwm)
        time.sleep(3)
        print("Prova Destra")
        SteerRight(pwm)
        PrintModes(pwm)
        time.sleep(3)
        print("Stop Destra")
        StopRight(pwm)
        time.sleep(3)
        print("Prova Sinistra")
        SteerLeft(pwm)
        PrintModes(pwm)
        time.sleep(3)
        print("Stop Sinistra")
        StopLeft(pwm)
        print("Stop")
        Stop(pwm)
        print("Test terminato")
    except KeyboardInterrupt:
        if pwm is not None:
              Stop(pwm)
              
if __name__=='__main__':
        test()