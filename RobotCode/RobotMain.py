import ActorsConfig
import RobotSocket as Socket

import RobotCore as Core
import RobotControlArm as ArmControl
import RobotControlMovements as MovementControl
import RobotAI as AI
import RobotVideoHandler as VideoHandler

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
    print("Programma avviato")
    main()
    '''
    #test message
    command = "ID:test;TYPE:Start;BODY:camera rollig, CHACK ACTION"
    ActorsConfig.actorCore_ref.tell(command)
    sleep(2)
    command = "ID:test;TYPE:Movement;BODY:MoveForward_start"
    ActorsConfig.actorCore_ref.tell(command)
    sleep(2)
    command = "ID:test;TYPE:Movement;BODY:MoveForward_stop"
    ActorsConfig.actorCore_ref.tell(command)
    sleep(2)
    command = "ID:test;TYPE:Movement;BODY:TurnLeft_start"
    ActorsConfig.actorCore_ref.tell(command)
    sleep(2)
    command = "ID:test;TYPE:Movement;BODY:TurnLeft_stop"
    ActorsConfig.actorCore_ref.tell(command)
    sleep(2)
    command = "ID:test;TYPE:AI_Recognition;BODY:searching for an enemy"
    ActorsConfig.actorCore_ref.tell(command)
    sleep(2)
    command = "ID:test;TYPE:Robot_Arm;BODY:lowering arm"
    ActorsConfig.actorCore_ref.tell(command)
    sleep(2)
    command = "ID:test;TYPE:Robot_Arm;BODY:grabbing enemy"
    ActorsConfig.actorCore_ref.tell(command)
    sleep(2)
    '''
    
