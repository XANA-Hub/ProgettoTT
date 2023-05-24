#import RPi.GPIO as GPIO
import time
#import pigpio

pwm = None

#Ruota sinistra
pinLB = 24
pinLF = 23
#Ruota destra
pinRB = 6
pinRF = 5

'''
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
        pwm.set_PWM_frequency(pinLF, 200)
        pwm.set_PWM_range(pinLF, 40000)
        pwm.set_PWM_dutycycle(pinLF, 3000)

def InitLeftWheelBackward(pwm):
        pwm.set_mode(pinLB, pigpio.OUTPUT)
        pwm.set_PWM_frequency(pinLB, 200)
        pwm.set_PWM_range(pinLB, 40000)
        pwm.set_PWM_dutycycle(pinLB, 3000)

def InitRightWheelForward(pwm):
        pwm.set_mode(pinRF, pigpio.OUTPUT)
        pwm.set_PWM_frequency(pinRF, 200)
        pwm.set_PWM_range(pinRF, 40000)
        pwm.set_PWM_dutycycle(pinRF, 2000)

def InitRightWheelBackward(pwm):
        pwm.set_mode(pinRB, pigpio.OUTPUT)
        pwm.set_PWM_frequency(pinRB, 200)
        pwm.set_PWM_range(pinRB, 40000)
        pwm.set_PWM_dutycycle(pinRB, 2000)

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
        time.sleep(3)
        print("Stop forward")
        StopForward(pwm)
        time.sleep(3)
        print("Prova backward")
        MoveBackward(pwm)
        time.sleep(3)
        print("Stop Backward")
        StopBackward(pwm)
        time.sleep(3)
        print("Prova Destra")
        SteerRight(pwm)
        time.sleep(3)
        print("Stop Destra")
        StopRight(pwm)
        time.sleep(3)
        print("Prova Sinistra")
        SteerLeft(pwm)
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
        '''