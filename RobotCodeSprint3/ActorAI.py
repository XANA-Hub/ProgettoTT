import pykka
import RobotSocket as rs
import RobotAIControl

class Actor(pykka.ThreadingActor):
    def on_receive(self, message):
        #print("--RobotAI-- " + message)
        if "Terminate" in message:
            print("--RobotAI-- terminating")
            self.stop()
        elif "Identify_Current" in message:
            self.identify()
        else:
            print("--RobotAI ERROR-- malformed message: " + message)



    def identify(self):
        result = RobotAIControl.detect()        #funzione di libreria che ritorna una stringa se sono sopra un certo livello di sicurezza altrimenti un messaggio di errore
        print("Actor AI sending...")
        if isinstance(result, str):
            rs.sendAIresponse(result)
        else:
            rs.sendAIresponse("Error 501: calculated result is not a string")
