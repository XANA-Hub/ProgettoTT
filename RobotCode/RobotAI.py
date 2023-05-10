import pykka

class Actor(pykka.ThreadingActor):
    def on_receive(self, message):
        print(message)