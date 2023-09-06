import pykka
import RobotSocket as rs

class Actor(pykka.ThreadingActor):
    def on_receive(self, message):
        print("--RobotAI-- " + message)
        if "Terminate" in message:
            print("--RobotAI-- terminating")
            self.stop()
        elif "Identify_Current" in message:
            self.identify()
        else:
            print("--RobotAI ERROR-- malformed message: " + message)



    def identify():
        rs.sendAIresponse("test_string")
