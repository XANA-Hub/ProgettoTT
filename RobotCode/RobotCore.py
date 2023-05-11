import pykka            #https://pykka.readthedocs.io/en/stable/quickstart/

import ActorsConfig

ID = 0
TYPE = 1
BODY = 2

class Actor(pykka.ThreadingActor):
    def on_receive(self, message):          #message is in the form: "ID:001;TYPE:qualcosa;BODY:qualcos'altro"
        messageToken = message.split(";")    #messageToken[0] = "ID:001"  [1] = "TYPE:qualcosa"  [2] = "BODY:qualcosa"
        if "Movement" in messageToken[TYPE]:
            ActorsConfig.actorMovementControl_ref.tell(messageToken[BODY][5:])
        elif "Robot_Arm" in messageToken[TYPE]:
            ActorsConfig.actorArmControl_ref.tell(messageToken[BODY][5:])
        elif "AI_Recognition" in messageToken[TYPE]:
            ActorsConfig.actorAI_ref.tell(messageToken[BODY][5:])
        elif "Start" in messageToken[TYPE]:
            ActorsConfig.actorVideoHandler_ref.tell(messageToken[BODY][5:])
        elif "Stop" in messageToken[TYPE]:
            self.terminating(messageToken[TYPE][5:])
        else:
            print("--RobotCore ERROR-- malformed message: " + message)

    def terminating(self, message):

        #send "Stop" message to all actors
        ActorsConfig.actorAI_ref.tell(message)
        ActorsConfig.actorArmControl_ref.tell(message)
        ActorsConfig.actorMovementControl_ref.tell(message)
        ActorsConfig.actorVideoHandler_ref.tell(message)

        #terminate himself
        print("--RobotCore-- terminating")
        self.stop()
