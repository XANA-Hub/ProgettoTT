import pykka            #https://pykka.readthedocs.io/en/stable/quickstart/
import ActorsConfig

ID = 0
TYPE = 1
BODY = 2
TEST = True

class Actor(pykka.ThreadingActor):
    def on_receive(self, message):          #message is in the form: "ID:001;TYPE:qualcosa;BODY:qualcos'altro"

        #Testing print
        if TEST: print("--RobotCore-- " + message)
        
        messageToken = message.split(";")    #messageToken[0] = "ID:001"  [1] = "TYPE:qualcosa"  [2] = "BODY:qualcosa"
        
        if "Movement" in messageToken[TYPE]:
            ActorsConfig.actorMovementControl_ref.tell(messageToken[BODY][5:])

        elif "Robot_Arm" in messageToken[TYPE]:
            ActorsConfig.actorArmControl_ref.tell(messageToken[BODY][5:])

        elif "Terminate" in messageToken[TYPE]:     #can only be generate by the system (socket class) for complete termination of the robot
            self.terminating(messageToken[TYPE][5:])

        elif "Client Crashed" in messageToken[TYPE]:
            self.clientCrash()

        else:
            print("--RobotCore ERROR-- malformed message: " + message)


    def clientCrash(self):
        #for now in case of crash no action needs to be performed
        pass


    def terminating(self, messageToken):
        
        #send "Terminate" message to all actors
        ActorsConfig.actorArmControl_ref.tell(messageToken)
        ActorsConfig.actorMovementControl_ref.tell(messageToken)

        #terminate himself
        print("--RobotCore-- terminating")
        self.stop()
