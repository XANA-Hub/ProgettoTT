#import RPi.GPIO as GPIO
import time
#import pigpio

pwm = None

#Braccio
pinArm = 7
#Pinza
pinClaw = 8
'''
def resetPosition(pwm):
      setServoPwm(pwm, pinArm, 90)
      setServoPwm(pwm, pinClaw, 140)
      pwm.set_mode(pinArm, 0)
      pwm.set_mode(pinClaw, 0)

def riseArm(pwm):
      pwm.set_PWM_dutycycle(pinArm, 80+(400/180)*90)

def lowArm(pwm):
      pwm.set_PWM_dutycycle(pinArm, 80+(400/180)*150)

def openClaw(pwm):
      pwm.set_PWM_dutycycle(pinArm, 80+(400/180)*140)

def closeClaw(pwm):
      pwm.set_PWM_dutycycle(pinArm, 80+(400/180)*90)

def initializeArm(pwm):
      pwm.set_mode(pinArm, pigpio.OUTPUT)
      pwm.set_PWM_frequency(pinArm, 200)
      pwm.set_PWM_range(pinArm, 40000)

def initializeClaw(pwm):
      pwm.set_mode(pinClaw, pigpio.OUTPUT)
      pwm.set_PWM_frequency(pinClaw, 50)
      pwm.set_PWM_range(pinClaw, 4000)

def InitPwm():
      pwm = pigpio.pi() 
      pwm.set_mode(25, pigpio.OUTPUT)
      pwm.set_PWM_frequency(25, 50)
      pwm.set_PWM_range(25, 4000)
      return pwm  

def PrintModes(pwm):
        print("Arm: "+str(pwm.get_mode(pinArm)))
        print("Claw: "+str(pwm.get_mode(pinClaw)))

def setServoPwm(pwm, pin, angle):
            angle=int(angle)
            pwm.set_PWM_dutycycle(pin, 3*(80+(400/180)*angle))

def test():
      try:
            print ('Test')
            pwm = InitPwm()
            PrintModes(pwm)
            initializeArm(pwm)
            initializeClaw(pwm)
            PrintModes(pwm)
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
            
            for i in range(90,150,1):
                  print(str(i)+" pinArm")
                  setServoPwm(pwm, pinArm, i)
                  time.sleep(0.01)
            time.sleep(1)
            for i in range(140,90,-1):
                  print(str(i)+" pinClaw")
                  setServoPwm(pwm, pinClaw, i)
                  time.sleep(0.01)
            time.sleep(1)
            for i in range(90,140,1):
                  print(str(i)+" pinClaw")
                  setServoPwm(pwm, pinClaw, i)
                  time.sleep(0.01)
            time.sleep(1)
            for i in range(150,90,-1):
                  print(str(i)+" pinArm")
                  setServoPwm(pwm, pinArm, i)
                  time.sleep(0.01)
            print("Resetto la posizione")
            resetPosition(pwm)
            print("Test terminato")
            
      except KeyboardInterrupt:
            if pwm is not None:
                  resetPosition(pwm)
              

if __name__=='__main__':
        test()
        '''