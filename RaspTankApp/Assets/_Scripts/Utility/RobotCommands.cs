using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RobotCommands {
    public const string forwardStart = "ID:000;TYPE:Movement;BODY:Forward Start";
    public const string forwardStop = "ID:000;TYPE:Movement;BODY:Forward Stop";
    public const string backwardStart = "ID:000;TYPE:Movement;BODY:Backward Start";
    public const string backwardStop = "ID:000;TYPE:Movement;BODY:Backward Stop";
    public const string rotateLeftStart = "ID:000;TYPE:Movement;BODY:Rotate_Left Start";
    public const string rotateLeftStop = "ID:000;TYPE:Movement;BODY:Rotate_Left Stop";
    public const string rotateRightStart = "ID:000;TYPE:Movement;BODY:Rotate_Right Start";
    public const string rotateRightStop = "ID:000;TYPE:Movement;BODY:Rotate_Right Stop";
    public const string armDown = "ID:000;TYPE:Robot_Arm;BODY:Down";
    public const string armUp = "ID:000;TYPE:Robot_Arm;BODY:Up";
    public const string armGrab = "ID:000;TYPE:Robot_Arm;BODY:Grab";
    public const string armRelease = "ID:000;TYPE:Robot_Arm;BODY:Release";
    public const string disconnect = "ID:000;TYPE:Disconnect;BODY:Disconnect";
    public const string start = "ID:000;TYPE:Start;BODY:192.168.1.13:25565";
    public const string AIIdentifyCurrent = "ID:000;TYPE:AI_Recognition;BODY:Identify_Current";



}
