import sys
import ActorsConfig
import RobotSocket as Socket

import ActorCore as Core
import ActorControlArm as ArmControl
import ActorControlMovements as MovementControl
import ActorVideoHandler as VideoHandler
import ActorAI as AI

TEST = False

def main():
    #start all the actors
    if TEST: print("Avvio gli attori")
    ActorsConfig.actorCore_ref = Core.Actor.start()
    ActorsConfig.actorArmControl_ref = ArmControl.Actor.start()
    ActorsConfig.actorMovementControl_ref = MovementControl.Actor.start()
    ActorsConfig.actorVideoHandler_ref = VideoHandler.Actor.start()
    ActorsConfig.actorAI_ref = AI.Actor.start()

    #start network support
    if TEST: print("Avvio le socket")
    if len(sys.argv) != 3:
        Socket.startSocket()                                        #socket TCP con i valori di default 127.0.0.1:25565
    else:
        Socket.startSocket(sys.argv[1], int(sys.argv[2]))           #socket TCP per i comandi, prende da input ip e porta a cui aprire la socket

if __name__ == "__main__":
    print("Inizializzazione RaspTank...")
    main()
    
