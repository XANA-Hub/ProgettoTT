import pykka
import RobotSocket as rs
import RobotAIControl
import RobotCameraControl
import ActorsConfig

TEST = True

class Actor(pykka.ThreadingActor):
    def __init__(self):
        super().__init__()
        if ActorsConfig.camera is None:
            ActorsConfig.camera = RobotCameraControl.initCamera()
        self.camera = ActorsConfig.camera

    def on_receive(self, message):
        if "Identify_Current" in message:
            print("--RobotAI-- detecting" + message)
            self.identify()

        elif "Terminate" in message:
            if TEST: print("--RobotAI-- terminating")
            self.stop()
        else:
            print("--RobotAI ERROR-- malformed message: " + message)

    def identify(self):
        #prima devo fare la foto
        path = "./image.jpg"
        RobotCameraControl.captureImage(self.camera, path)

        #poi devo fare la detect sulla foto
        tupleRes = RobotAIControl.detect(path)        #funzione di libreria che ritorna una stringa se sono sopra un certo livello di sicurezza altrimenti un messaggio di errore
        print(str(tupleRes[1]))
        if(tupleRes[1]<0.75):
            result = "Nothing"
        else:
            result = tupleRes[0]
        if TEST: print("Actor AI sending...")
        if isinstance(result, str):
            rs.sendAIresponse(result)
        else:
            rs.sendAIresponse("Error 501: calculated result is not a string")
