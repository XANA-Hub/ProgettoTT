import RPi.GPIO as GPIO
import time
import pigpio

pwm = None
rangePWM=4000

'''
range - range massimo del valore di duty_cycle, ci garantisce un certo livello di precisione
frequency - frequenza del segnale
dutycycle - valore per il quale il segnale rimane alto
La potenza passata alle ruote risulta la percentuale dutycycle/range
'''

#Ruota sinistra
pinLB = 24
pinLF = 23
#Ruota destra (Invertiti)
pinRB = 5
pinRF = 6

def Stop(pwm):
        StopForward(pwm)
        StopBackward(pwm)

def Terminate(pwm):
       Stop(pwm)
       pwm.set_mode(pinRF, 0)
       pwm.set_mode(pinRB, 0)
       pwm.set_mode(pinLF, 0)
       pwm.set_mode(pinLB, 0)
       pwm.stop()

def StopForward(pwm):
        pwm.set_PWM_dutycycle(pinRF, 0)
        pwm.set_PWM_dutycycle(pinLF, 0)
    
def StopBackward(pwm):
        pwm.set_PWM_dutycycle(pinRB, 0)
        pwm.set_PWM_dutycycle(pinLB, 0)

def StopLeft(pwm):
        pwm.set_PWM_dutycycle(pinRF, 0)
        pwm.set_PWM_dutycycle(pinLB, 0)

def StopRight(pwm):
        pwm.set_PWM_dutycycle(pinRB, 0)
        pwm.set_PWM_dutycycle(pinLF, 0)

#HO INVERITO I PIN IN MODO DA UNIFORMARE IL COMPORTAMENTO ESSENDO I MOTORI SPECULARI
def TurnRightWheelForward(pwm):
        pwm.set_PWM_dutycycle(pinRF, rangePWM)
        pwm.set_PWM_dutycycle(pinRB, 0)
def TurnRightWheelBackward(pwm):
        pwm.set_PWM_dutycycle(pinRF, 0)
        pwm.set_PWM_dutycycle(pinRB, rangePWM)
def TurnLeftWheelForward(pwm):
        pwm.set_PWM_dutycycle(pinLF, rangePWM)
        pwm.set_PWM_dutycycle(pinLB, 0)
def TurnLeftWheelBackward(pwm):
        pwm.set_PWM_dutycycle(pinLF, 0)
        pwm.set_PWM_dutycycle(pinLB, rangePWM)

def MoveForward(pwm):
        TurnRightWheelForward(pwm)
        TurnLeftWheelForward(pwm)

def MoveBackward(pwm):
        TurnRightWheelBackward(pwm)
        TurnLeftWheelBackward(pwm)

def SteerLeft(pwm):
        TurnRightWheelForward(pwm)
        TurnLeftWheelBackward(pwm)

def SteerRight(pwm):
        TurnRightWheelBackward(pwm)
        TurnLeftWheelForward(pwm)

#DA TARARE - Frequenza default 800
def InitLeftWheelForward(pwm):
        pwm.set_PWM_frequency(pinLF, 1)
        pwm.set_PWM_range(pinLF, rangePWM)
        pwm.set_mode(pinLF, 0)

def InitLeftWheelBackward(pwm):
        pwm.set_PWM_frequency(pinLB, 1)
        pwm.set_PWM_range(pinLB, rangePWM)
        pwm.set_mode(pinLB, 0)
def InitRightWheelForward(pwm):
        pwm.set_PWM_frequency(pinRF, 1)
        pwm.set_PWM_range(pinRF, rangePWM)
        pwm.set_mode(pinRF, 0)
def InitRightWheelBackward(pwm):
        pwm.set_PWM_frequency(pinRB, 1)
        pwm.set_PWM_range(pinRB, rangePWM)
        pwm.set_mode(pinRB, 0)

def PrintModes(pwm):
        print("LF: "+str(pwm.get_mode(pinLF)))
        print("LB: "+str(pwm.get_mode(pinLB)))
        print("RF: "+str(pwm.get_mode(pinRF)))
        print("RB: "+str(pwm.get_mode(pinRB)))

def PrintPinInfo(pwm, pin):
        print("mode: "+str(pwm.get_mode(pin)))
        print("frequency: "+str(pwm.get_PWM_frequency(pin)))
        print("range: "+str(pwm.get_PWM_range(pin)))
        print("dutycycle: "+str(pwm.get_PWM_dutycycle(pin)))
        print("real range: "+str(pwm.get_PWM_real_range(pin)))

def InitPwm():
        pwm = pigpio.pi()
        InitLeftWheelForward(pwm)
        InitLeftWheelBackward(pwm)
        InitRightWheelForward(pwm)
        InitRightWheelBackward(pwm)
        return pwm
      

def test():
    try:
        print ('Test')
        pwm = InitPwm()
        PrintModes(pwm)
        time.sleep(2)

        print("Prova forward")
        MoveForward(pwm)
        PrintModes(pwm)
        time.sleep(2)
        print("Stop forward")
        StopForward(pwm)
        time.sleep(2)

        print("Prova backward")
        MoveBackward(pwm)
        PrintModes(pwm)
        time.sleep(2)
        print("Stop Backward")
        StopBackward(pwm)
        time.sleep(2)

        print("Prova Destra")
        SteerRight(pwm)
        PrintModes(pwm)
        time.sleep(2)
        print("Stop Destra")
        StopRight(pwm)
        time.sleep(2)

        print("Prova Sinistra")
        SteerLeft(pwm)
        PrintModes(pwm)
        time.sleep(2)
        print("Stop Sinistra")
        StopLeft(pwm)
        time.sleep(2)

        print("Stop")
        Terminate(pwm)
        print("Test terminato")
    except KeyboardInterrupt:
        if pwm is not None:
              Stop(pwm)
              
if __name__=='__main__':
        test()