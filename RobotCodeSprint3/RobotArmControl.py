import RPi.GPIO as GPIO
import time
import pigpio

TEST = False

#AVENDO UN PASSO DI 10 ALLA VOLTA E' PREFERIBILE UTILIZZARE MULTIPLI DI 10
defaultVariation=10
defaultArmPosition=400
defaultClawPosition=1950
rangePWM=4000
sAttesa=0
currentArmDutyCycle=defaultArmPosition
currentClawDutyCycle=defaultClawPosition
armHighPosition=900
armLowPosition=400
openedClawPosition=1400
closedClawPosition=1950
'''
range - range massimo del valore di duty_cycle, ci garantisce un certo livello di precisione
frequency - frequenza del segnale
dutycycle - valore per il quale il segnale rimane alto
'''

#Braccio
pinArm = 7
#Pinza
pinClaw = 8

def resetPosition(pwm):
      global currentArmDutyCycle
      global currentClawDutyCycle
      setServoArmPwm(pwm, pinArm, defaultArmPosition)
      currentArmDutyCycle=defaultArmPosition
      setServoClawPwm(pwm, pinClaw, defaultClawPosition)
      currentClawDutyCycle=defaultClawPosition

def terminate(pwm):
      resetPosition(pwm)
      pwm.set_mode(pinArm, 0)
      pwm.set_mode(pinClaw, 0)
      pwm.stop()

def riseArm(pwm):
      global currentArmDutyCycle
      setServoArmPwm(pwm, pinArm, armHighPosition)
      currentArmDutyCycle=armHighPosition

def lowArm(pwm):
      global currentArmDutyCycle
      setServoArmPwm(pwm, pinArm, armLowPosition)
      currentArmDutyCycle=armLowPosition

def openClaw(pwm):
      global currentClawDutyCycle
      setServoClawPwm(pwm, pinClaw, openedClawPosition)
      currentClawDutyCycle=openedClawPosition
      pwm.set_mode(pinClaw, 0)
      pwm.set_mode(pinClaw, 1)

def closeClaw(pwm):
      global currentClawDutyCycle
      setServoClawPwm(pwm, pinClaw, closedClawPosition)
      currentClawDutyCycle=closedClawPosition
      pwm.set_mode(pinClaw, 0)
      pwm.set_mode(pinClaw, 1)

def initializeArm(pwm):
      pwm.set_PWM_frequency(pinArm, 180)
      pwm.set_PWM_range(pinArm, rangePWM)
      pwm.set_mode(pinArm, 0)

def initializeClaw(pwm):
      pwm.set_PWM_frequency(pinClaw, 180)
      pwm.set_PWM_range(pinClaw, rangePWM)
      pwm.set_mode(pinClaw, 0)

def InitPwm():
      pwm = pigpio.pi()
      initializeArm(pwm)
      initializeClaw(pwm)
      resetPosition(pwm)
      setWaitingTimeInms(10)
      return pwm  

def PrintModes(pwm):
      print("Arm: "+str(pinArm))
      print("Arm mode: "+str(pwm.get_mode(pinArm)))
      print("Arm freq: "+str(pwm.get_PWM_frequency(pinArm)))
      print("Arm dutycycle: "+str(pwm.get_PWM_dutycycle(pinArm)))
      print("Arm range: "+str(pwm.get_PWM_range(pinArm)))
      print("Arm real range: "+str(pwm.get_PWM_real_range(pinArm)))
      print("Claw: "+str(pinClaw))
      print("Claw mode: "+str(pwm.get_mode(pinClaw)))
      print("Claw freq: "+str(pwm.get_PWM_frequency(pinClaw)))
      #print("Claw dutycycle: "+str(pwm.get_PWM_dutycycle(pinClaw)))
      print("Claw range: "+str(pwm.get_PWM_range(pinClaw)))
      print("Claw real range: "+str(pwm.get_PWM_real_range(pinClaw)))

def setServoArmPwm(pwm, pin, angle):
      global currentArmDutyCycle
      variation=0
      angle=int(angle)
      if(currentArmDutyCycle<angle):
            variation=defaultVariation
      else: variation=-defaultVariation
      for i in range(currentArmDutyCycle,angle,variation):
            pwm.set_PWM_dutycycle(pin, i)
            if TEST: print("Il pin "+str(pin)+" si è spostato a "+str(i))
            time.sleep(sAttesa)
      pwm.set_PWM_dutycycle(pin, angle)
      currentArmDutyCycle=angle

def setServoClawPwm(pwm, pin, angle):
      global currentClawDutyCycle
      variation=0
      angle=int(angle)
      if(currentClawDutyCycle<angle):
            variation=defaultVariation
      else: variation=-defaultVariation
      for i in range(currentClawDutyCycle,angle,variation):
            pwm.set_PWM_dutycycle(pin, i)
            if TEST: print("Il pin "+str(pin)+" si è spostato a "+str(i))
            time.sleep(sAttesa)
      pwm.set_PWM_dutycycle(pin, angle)
      currentClawDutyCycle=angle

def setWaitingTimeInms(ms):
        global sAttesa
        if (ms<0):
                sAttesa=0
        else:
                sAttesa=ms/1000

def test():
      try:
            print ('Test')
            pwm = InitPwm()
            time.sleep(2)

            #setWaitingTimeInms(10)
            
            print("Alzo braccio")
            riseArm(pwm)
            time.sleep(1)

            print("Apro artiglio")
            openClaw(pwm)
            time.sleep(1)
            print("Chiudo artiglio")
            closeClaw(pwm)
            time.sleep(1)

            print("Abbasso braccio ")
            lowArm(pwm)
            time.sleep(1)

            PrintModes(pwm)

            print("Termino")
            terminate(pwm)
            
      except KeyboardInterrupt:
            if pwm is not None:
                  terminate(pwm)
              

if __name__ == '__main__':
        test()