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

        elif "Start" in messageToken[TYPE]:
            #start will initialize the connection with the client for the stream video
            ActorsConfig.actorVideoHandler_ref.tell(messageToken[TYPE][5:] + ";" + messageToken[BODY][5:])          # Start;192.168.n.n:PORTA

        elif "AI_Recognition" in messageToken[TYPE]:
            ActorsConfig.actorAI_ref.tell(messageToken[BODY][5:])

        elif "Terminate" in messageToken[TYPE]:         #can only be generate by the system (socket class) for complete termination of the robot for now
            self.terminating(messageToken[TYPE][5:])
            self.stop()

        elif "Disconnect" in messageToken[TYPE]:
            ActorsConfig.actorArmControl_ref.tell(messageToken[TYPE][5:])           #will reset the position of the arm
            ActorsConfig.actorMovementControl_ref.tell(messageToken[TYPE][5:])      #will stop the weels if left running by the client
            
            ActorsConfig.actorVideoHandler_ref.tell(messageToken[TYPE][5:])         #disconnect will close the connection for the stream video

        elif "Client Crashed" in messageToken[TYPE]:
            #ha senso che i vari attori sappiano la differenza tra una disconnect e un crash?
            self.clientCrash(messageToken[TYPE][5:])

        else:
            print("--RobotCore ERROR-- malformed message: " + message)


    def clientCrash(self, message):
        ActorsConfig.actorArmControl_ref.tell(message)                              #will reset the position of the arm
        ActorsConfig.actorMovementControl_ref.tell(message)                         #will stop the weels if left running by the client

        ActorsConfig.actorVideoHandler_ref.tell(message)                            #Client crash will close the connection for the stream video


    def terminating(self, messageToken):
        
        #send "Terminate" message to all actors
        ActorsConfig.actorArmControl_ref.tell(messageToken)
        ActorsConfig.actorMovementControl_ref.tell(messageToken)
        ActorsConfig.actorVideoHandler_ref.tell(messageToken)
        ActorsConfig.actorAI_ref.tell(messageToken)

        #terminate himself
        print("--RobotCore-- terminating")
        self.stop()
