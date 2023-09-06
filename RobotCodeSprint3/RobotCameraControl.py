import time
import picamera2
import RobotSocket as rs   #solo per il test
import libcamera

def initCamera():
    camera = picamera2.Picamera2()
    #POSSIBILI SIZE 640x480 - 400x300
    preview_config = camera.create_preview_configuration(main={"size": (640, 480)},transform=libcamera.Transform(hflip=1,vflip=1))
    camera.configure(preview_config)
    return camera


def killCamera(camera):
    stopVideo(camera)
    camera.close()


def captureImage(camera, path):
    camera.start()
    time.sleep(0) #SE 0 LUMINOSO SE >0 SI SCURISCE DEBOLMENTE - DEFAULT ERA 2 - DA FARE TEST IN MOVIMENTO
    metadata = camera.capture_file(path)
    print(metadata)


def sendVideo(camera, stream_file):
        try:
            #encoder=picamera2.encoders.H264Encoder(1000000)
            encoder = picamera2.encoders.JpegEncoder(q=90)
            output=picamera2.outputs.FileOutput(stream_file)
            camera.start_recording(encoder, output, quality=picamera2.encoders.Quality.VERY_HIGH)
        except:
            print("Eccezione durante la creazione del flusso video")
            stopVideo(camera)

def sendImg(camera, ip):
        try:
            captureImage(camera, "temp.jpg")
            rs.sendImg(ip, "temp.jpg")
        except:
            print("Eccezione durante l'invio di una immagine")
            stopVideo(camera)


def stopVideo(camera):  #NON DA PROBLEMI
    camera.stop_recording()


def test():
    try:     
        camera = initCamera()
        print("Test fotocamera")
        captureImage(camera, "image.jpg")
        stopVideo(camera)
        '''print("Invio")
        while True:
            time.sleep(1/24)
            sendImg(camera, "192.168.1.13")
        print("Invio completato")'''
        killCamera(camera)
        #Funziona
        
        print("Test flusso video")
        stream = rs.startVideoSocket("192.168.1.13", 25565)
        camera = initCamera()
        sendVideo(camera, stream)
        print("Flusso avviato")
        camera.capture_file("imageVideo.jpg")
        time.sleep(10)
        stopVideo(camera)
        print("Flusso interrotto")
    except KeyboardInterrupt:
        killCamera(camera)
              

if __name__=='__main__':
        test()
