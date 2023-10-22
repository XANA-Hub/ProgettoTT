import pykka
import RobotSocket as rs
import RobotAIControl
import RobotCameraControl

TEST = True

class Actor(pykka.ThreadingActor):
    def __init__(self):
        super().__init__()
        self.camera = RobotCameraControl.initCamera()

    def on_receive(self, message):
        #print("--RobotAI-- " + message)
        elif "Identify_Current" in message:
            self.identify()

        if "Terminate" in message:
            if TEST: print("--RobotAI-- terminating")
            self.stop()
        else:
            print("--RobotAI ERROR-- malformed message: " + message)

    def identify(self):
        #prima devo fare la foto
        path = "./image.jpg"
        RobotCameraControl.captureImage(self.camera, path)

        #poi devo fare la detect sulla foto
        result = RobotAIControl.detect(path)        #funzione di libreria che ritorna una stringa se sono sopra un certo livello di sicurezza altrimenti un messaggio di errore
        if TEST: print("Actor AI sending...")
        if isinstance(result, str):
            rs.sendAIresponse(result)
        else:
            rs.sendAIresponse("Error 501: calculated result is not a string")
