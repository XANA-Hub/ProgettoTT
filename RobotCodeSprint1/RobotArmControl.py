import RPi.GPIO as GPIO
import time
import pigpio

pwm = None
rangePWM = 4000

#Braccio
pinArm = 7
#Pinza
pinClaw = 8

def resetPosition(pwm):
      setServoPwm(pwm, pinArm, 90)
      setServoPwm(pwm, pinClaw, 140)
      pwm.set_mode(pinArm, 0)
      pwm.set_mode(pinClaw, 0)

def riseArm(pwm):
      pwm.set_PWM_dutycycle(pinArm, 1000)
      #time.sleep(1)
      #pwm.set_PWM_dutycycle(pinArm, 0)

def lowArm(pwm):
      pwm.set_PWM_dutycycle(pinArm, 300)
      #time.sleep(1)
      #pwm.set_PWM_dutycycle(pinArm, 0)

def openClaw(pwm):
      pwm.set_PWM_dutycycle(pinClaw, 80+(400/180)*140)
      time.sleep(1)
      pwm.set_PWM_dutycycle(pinClaw, 0)

def closeClaw(pwm):
      pwm.set_PWM_dutycycle(pinClaw, 80+(400/180)*90)
      time.sleep(1)
      pwm.set_PWM_dutycycle(pinClaw, 0)

def initializeArm(pwm):
      pwm.set_mode(pinArm, pigpio.OUTPUT)
      print("Arm range: "+str(pwm.get_PWM_real_range(pinArm)))
      pwm.set_PWM_frequency(pinArm, 180)
      pwm.set_PWM_range(pinArm, rangePWM)

def initializeClaw(pwm):
      pwm.set_mode(pinClaw, pigpio.OUTPUT)
      print("Claw range: "+str(pwm.get_PWM_real_range(pinClaw)))
      pwm.set_PWM_frequency(pinClaw, 180)
      pwm.set_PWM_range(pinClaw, rangePWM)

def InitPwm():
      pwm = pigpio.pi() 
      print("25 range: "+str(pwm.get_PWM_real_range(25)))
      #pwm.set_mode(25, pigpio.OUTPUT)
      #pwm.set_PWM_frequency(25, 200)
      #pwm.set_PWM_range(25, rangePWM)
      return pwm  

def PrintModes(pwm):
        print("Arm: "+str(pwm.get_mode(pinArm)))
        print("Arm freq: "+str(pwm.get_PWM_frequency(pinArm)))
        print("Arm range: "+str(pwm.get_PWM_range(pinArm)))
        print("Claw: "+str(pwm.get_mode(pinClaw)))
        print("Claw freq: "+str(pwm.get_PWM_frequency(pinClaw)))
        print("Claw range: "+str(pwm.get_PWM_range(pinClaw)))

def setServoPwm(pwm, pin, angle):
            angle=int(angle)
            pwm.set_PWM_dutycycle(pin, angle)

def test():
      try:
            print ('Test')
            pwm = InitPwm()
            PrintModes(pwm)
            print ('Inizializzo')
            initializeArm(pwm)
            initializeClaw(pwm)
            PrintModes(pwm)
            
            time.sleep(1)
            print("Alzo braccio")
            riseArm(pwm)
            time.sleep(1)
            print("Abbasso braccio ")
            lowArm(pwm)
            time.sleep(1)
            
            '''
            for i in range(200,1000,10):
                  print(str(i)+" pinArm")
                  setServoPwm(pwm, pinArm, i)
                  time.sleep(0.01)
            time.sleep(1)
            
            for i in range(1000,200,-10):
                  print(str(i)+" pinArm")
                  setServoPwm(pwm, pinArm, i)
                  time.sleep(0.01)
            print("Apro artiglio")
            openClaw(pwm)
            time.sleep(1)
            print("Chiudo artiglio")
            closeClaw(pwm)
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

            for i in range(100,500,5):
                  print("test con frequenza: " + str(i))
                  time.sleep(1)
                  pwm.set_PWM_frequency(pinArm, i)
                  riseArm(pwm)
                  time.sleep(1)
            '''
            time.sleep(1)
            print("Resetto la posizione")
            resetPosition(pwm)
            print("Test terminato")
            
      except KeyboardInterrupt:
            if pwm is not None:
                  resetPosition(pwm)
              

if __name__=='__main__':
        test()