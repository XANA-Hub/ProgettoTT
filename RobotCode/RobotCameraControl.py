import time
import picamera2
import socket   #da rimuovere
import libcamera

camera = None

def initCamera():
    camera = picamera2.Picamera2()
    preview_config = camera.create_preview_configuration(main={"size": (640, 480)},transform=libcamera.Transform(hflip=1,vflip=1))
    camera.configure(preview_config)
    camera.start()
    time.sleep(2)
    return camera

def killCamera(camera):
    camera.close()  
    camera = None 

def captureImage(path):
    camera = initCamera()
    metadata = camera.capture_file(path)
    print(metadata)
    killCamera(camera)

def sendVideo(video_socket):
        try:
            stream=video_socket.makefile('wb')
            camera = picamera2.Picamera2()
            camera.configure(camera.create_video_configuration(main={"size": (400, 300)}))
            encoder=picamera2.encoders.H264Encoder(1000000)
            #encoder = picamera2.encoders.JpegEncoder(q=90)
            output=picamera2.outputs.FileOutput(stream)
            camera.start_recording(encoder, output, quality=picamera2.encoders.Quality.VERY_HIGH)
            return camera
        except:
            camera.stop_recording()

def stopVideo(camera):
    camera.stop_recording()
    camera = None

def test():
    try:
        print("Test fotocamera")
        captureImage("image.jpg")
        print("Test flusso video")
        #Funziona
        video_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        video_socket.connect(("192.168.178.149", 25565))
        camera = sendVideo(video_socket)
        print("Flusso avviato")
        time.sleep(10)
        stopVideo(camera)
        print("Flusso interrotto")
    except KeyboardInterrupt:
            if camera is not None:
                try:
                    camera.stop_recording()
                except:
                    pass
              

if __name__=='__main__':
        test()