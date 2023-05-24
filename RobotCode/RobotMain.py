import ActorsConfig
import RobotSocket as Socket

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
    Socket.startSocket()

if __name__ == "__main__":
    print("Inizializzazione Robot")
    main()
    
