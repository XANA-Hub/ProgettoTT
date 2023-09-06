import RPi.GPIO as GPIO
import time
import pigpio

#pwm = None
rangePWM=4000
dutycyclePWM=4000

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

def stop(pwm):
        stopForward(pwm)
        stopBackward(pwm)

def terminate(pwm):
        stop(pwm)
        pwm.set_mode(pinRF, 0)
        pwm.set_mode(pinRB, 0)
        pwm.set_mode(pinLF, 0)
        pwm.set_mode(pinLB, 0)
        pwm.stop()

def stopForward(pwm):
        pwm.set_PWM_dutycycle(pinRF, 0)
        pwm.set_PWM_dutycycle(pinLF, 0)
    
def stopBackward(pwm):
        pwm.set_PWM_dutycycle(pinRB, 0)
        pwm.set_PWM_dutycycle(pinLB, 0)

def stopLeft(pwm):
        pwm.set_PWM_dutycycle(pinRF, 0)
        pwm.set_PWM_dutycycle(pinLB, 0)

def stopRight(pwm):
        pwm.set_PWM_dutycycle(pinRB, 0)
        pwm.set_PWM_dutycycle(pinLF, 0)
        
def stopRightWheel(pwm):
        pwm.set_PWM_dutycycle(pinRB, 0)
        pwm.set_PWM_dutycycle(pinRF, 0)

def stopLeftWheel(pwm):
        pwm.set_PWM_dutycycle(pinLB, 0)
        pwm.set_PWM_dutycycle(pinLF, 0)

#HO INVERITO I PIN IN MODO DA UNIFORMARE IL COMPORTAMENTO ESSENDO I MOTORI SPECULARI
def turnRightWheelForward(pwm):
        pwm.set_PWM_dutycycle(pinRF, dutycyclePWM)
        pwm.set_PWM_dutycycle(pinRB, 0)
def turnRightWheelBackward(pwm):
        pwm.set_PWM_dutycycle(pinRF, 0)
        pwm.set_PWM_dutycycle(pinRB, dutycyclePWM)
def turnLeftWheelForward(pwm):
        pwm.set_PWM_dutycycle(pinLF, dutycyclePWM)
        pwm.set_PWM_dutycycle(pinLB, 0)
def turnLeftWheelBackward(pwm):
        pwm.set_PWM_dutycycle(pinLF, 0)
        pwm.set_PWM_dutycycle(pinLB, dutycyclePWM)

def moveForward(pwm):
        turnRightWheelForward(pwm)
        turnLeftWheelForward(pwm)

def moveBackward(pwm):
        turnRightWheelBackward(pwm)
        turnLeftWheelBackward(pwm)

def steerLeft(pwm):
        turnRightWheelForward(pwm)
        turnLeftWheelBackward(pwm)

def steerRight(pwm):
        turnRightWheelBackward(pwm)
        turnLeftWheelForward(pwm)

#DA TARARE - Frequenza default 800
def initLeftWheelForward(pwm):
        pwm.set_PWM_frequency(pinLF, 40)
        pwm.set_PWM_range(pinLF, rangePWM)
        pwm.set_mode(pinLF, 0)
def initLeftWheelBackward(pwm):
        pwm.set_PWM_frequency(pinLB, 40)
        pwm.set_PWM_range(pinLB, rangePWM)
        pwm.set_mode(pinLB, 0)
def initRightWheelForward(pwm):
        pwm.set_PWM_frequency(pinRF, 40)
        pwm.set_PWM_range(pinRF, rangePWM)
        pwm.set_mode(pinRF, 0)
def initRightWheelBackward(pwm):
        pwm.set_PWM_frequency(pinRB, 40)
        pwm.set_PWM_range(pinRB, rangePWM)
        pwm.set_mode(pinRB, 0)

def printPins(pwm):
        print(" Forward")
        print("         Right")
        printPinInfo(pwm, pinRF)
        print("         Left")
        printPinInfo(pwm, pinLF)
        print(" Backward")
        print("         Right")
        printPinInfo(pwm, pinRB)
        print("         Left")
        printPinInfo(pwm, pinLB)

def printPinInfo(pwm, pin):
        print("PIN: "+str(pin))
        print("mode: "+str(pwm.get_mode(pin)))
        print("frequency: "+str(pwm.get_PWM_frequency(pin)))
        print("range: "+str(pwm.get_PWM_range(pin)))
        print("dutycycle: "+str(pwm.get_PWM_dutycycle(pin)))
        print("real range: "+str(pwm.get_PWM_real_range(pin)))

def setSpeedPercentage(percentage):     #IN REALTA VALORE TRA 0 E 1 MA MI PIACE CHIAMARLO PERCENTUALE
        global dutycyclePWM     #Per modificarla per tutti i blocchi  e non solo all'interno della func
        if (percentage<0):
                dutycyclePWM=0
        elif (percentage>1):
                dutycyclePWM=rangePWM
        else:
                dutycyclePWM=int(percentage*rangePWM)
               

def initPwm():
        pwm = pigpio.pi()
        initLeftWheelForward(pwm)
        initLeftWheelBackward(pwm)
        initRightWheelForward(pwm)
        initRightWheelBackward(pwm)
        stop(pwm)
        setSpeedPercentage(1)
        return pwm
      

def test():
    try:

        print ('Test')
        pwm = initPwm()
        setSpeedPercentage(1)
        time.sleep(2)
        printPins(pwm)
        time.sleep(2)

        print("Prova forward")
        moveForward(pwm)
        time.sleep(2)
        print("Stop forward")
        stopForward(pwm)
        time.sleep(2)

        print("Prova backward")
        moveBackward(pwm)
        time.sleep(2)
        print("Stop Backward")
        stopBackward(pwm)
        time.sleep(2)

        print("Prova Destra")
        steerRight(pwm)
        time.sleep(2)
        print("Stop Destra")
        stopRight(pwm)
        time.sleep(2)

        print("Prova Sinistra")
        steerLeft(pwm)
        time.sleep(2)
        print("Stop Sinistra")
        stopLeft(pwm)
        time.sleep(2)

        print("Stop")
        terminate(pwm)
        print("Test terminato")
    except KeyboardInterrupt:
        if pwm is not None:
              terminate(pwm)
              
if __name__=='__main__':
        test()