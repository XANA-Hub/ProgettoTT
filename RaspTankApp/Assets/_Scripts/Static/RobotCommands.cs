
// Lista di tutti i comandi inviabili al robot
public static class RobotCommands {

    // Movimento
    public const string forwardStart = "ID:000;TYPE:Movement;BODY:Forward Start";
    public const string forwardStop = "ID:000;TYPE:Movement;BODY:Forward Stop";
    public const string backwardStart = "ID:000;TYPE:Movement;BODY:Backward Start";
    public const string backwardStop = "ID:000;TYPE:Movement;BODY:Backward Stop";
    public const string rotateLeftStart = "ID:000;TYPE:Movement;BODY:Rotate_Left Start";
    public const string rotateLeftStop = "ID:000;TYPE:Movement;BODY:Rotate_Left Stop";
    public const string rotateRightStart = "ID:000;TYPE:Movement;BODY:Rotate_Right Start";
    public const string rotateRightStop = "ID:000;TYPE:Movement;BODY:Rotate_Right Stop";

    // Braccio
    public const string armDown = "ID:000;TYPE:Robot_Arm;BODY:Down";
    public const string armUp = "ID:000;TYPE:Robot_Arm;BODY:Up";
    public const string armGrab = "ID:000;TYPE:Robot_Arm;BODY:Grab";
    public const string armRelease = "ID:000;TYPE:Robot_Arm;BODY:Release";

    // Connessione
    public const string start = "ID:000;TYPE:Start;BODY:"; // Da aggiungere l'indirizzo IP del client e la porta
    public const string readyResponse = "ready"; // Per avviare flusso video
    public const string disconnect = "ID:000;TYPE:Disconnect;BODY:Disconnect";

    // AI
    public const string aiIdentifyCurrent = "ID:000;TYPE:AI_Recognition;BODY:Identify_Current";

    // Risposte pezzi degli scacchi riconosciuti
    public const string identifyResponseKing = "King";
    public const string identifyResponseQueen = "Queen";
    public const string identifyResponseRook = "Rook";
    public const string identifyResponseBishop = "Bishop";
    public const string identifyResponseKnight = "Knight";
    public const string identifyResponsePawn = "Pawn";
    public const string identifyResponseNothing = "Nd"; // Non ha trovato nulla



}
