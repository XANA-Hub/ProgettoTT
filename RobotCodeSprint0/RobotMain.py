import ActorsConfig
import RobotSocket as Socket
import sys

import ActorCore as Core
import ActorControlArm as ArmControl
import ActorControlMovements as MovementControl
import ActorAI as AI
import ActorVideoHandler as VideoHandler

def main():
    #start all the actors
    print("Avvio gli attori")
    ActorsConfig.actorCore_ref = Core.Actor.start()
    ActorsConfig.actorArmControl_ref = ArmControl.Actor.start()
    ActorsConfig.actorMovementControl_ref = MovementControl.Actor.start()
    ActorsConfig.actorAI_ref = AI.Actor.start()
    ActorsConfig.actorVideoHandler_ref = VideoHandler.Actor.start()

    #start network support


    print("Avvio le socket")
    if len(sys.argv) != 3:
        Socket.startSocket()                                    #socket TCP con i valori di default 127.0.0.1:25565
    else:
        Socket.startSocket(sys.argv[1], int(sys.argv[2]))            #socket TCP per i comandi, prende da input ip e porta a cui aprire la socket
    Socket.startVideoSocket()                                   #socket UDP per il video

if __name__ == "__main__":
    print("Inizializzazione Robot")
    main()
    