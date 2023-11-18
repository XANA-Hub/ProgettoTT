import time
#import picamera2
import socket   #solo per il test
#import libcamera

isVideoStarted = False

'''
def initCamera():
    camera = picamera2.Picamera2()
    return camera

def killCamera(camera):
    stopVideo(camera)
    camera.close()

def captureImage(camera, path):
    preview_config = camera.create_preview_configuration(main={"size": (640, 480)},transform=libcamera.Transform(hflip=1,vflip=1))
    camera.configure(preview_config)
    camera.start()
    time.sleep(2) #FARE DEI TEST
    metadata = camera.capture_file(path)
    print(metadata)

def sendVideo(camera, video_socket):
        try:
            print("Mi preparo ad inviare il flusso video ad "+str(video_socket))
            stream = video_socket.makefile('wb')
            camera.configure(camera.create_video_configuration(main={"size": (400, 300)}))
            #encoder=picamera2.encoders.H264Encoder(1000000)
            encoder = picamera2.encoders.JpegEncoder(q=90)
            output=picamera2.outputs.FileOutput(stream)
            camera.start_recording(encoder, output, quality=picamera2.encoders.Quality.VERY_HIGH)
            global isVideoStarted
            isVideoStarted = True
        except:
            print("Eccezione durante la creazione del flusso video")
            stopVideo(camera)

def stopVideo(camera):
    global isVideoStarted
    if isVideoStarted:
        camera.stop_recording()
        isVideoStarted = False

def test():
    try:
        camera = initCamera()
        print("Test fotocamera")
        captureImage(camera, "image.jpg")
        killCamera(camera)
        #Funziona
        print("Test flusso video")
        video_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        video_socket.connect(("192.168.178.149", 25565))
        camera = initCamera()
        sendVideo(camera, video_socket)
        print("Flusso avviato")
        time.sleep(10)
        stopVideo(camera)
        print("Flusso interrotto")
    except KeyboardInterrupt:
        killCamera(camera)
              

if __name__=='__main__':
        test()
        '''