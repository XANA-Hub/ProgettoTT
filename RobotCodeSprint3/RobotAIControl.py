from PIL import Image
import tflite_runtime.interpreter as tflite
import numpy as np

TEST = True

#Labels ordered
classes = ["Bishop","King","Knight", "Pawn","Queen","Rook"]

def detect(string imagePath):
    TFLITE_MODEL = "./model.tflite"
    interpreter = tflite.Interpreter(TFLITE_MODEL)
    interpreter.allocate_tensors()

    # Get input and output tensors
    input_details = interpreter.get_input_details()
    output_details = interpreter.get_output_details()

    # Load sample image
    pic = Image.open(imagePath)

    pic = pic.resize((256, 256), Image.LANCZOS)
    pic = np.asarray(pic, dtype=np.float32)
    pic = np.expand_dims(pic, axis=0)
    pic /= 255 

    input_tensor = np.array(pic, dtype=np.float32)
    interpreter.set_tensor(input_details[0]['index'], input_tensor)
    interpreter.invoke()

    # Get output
    output_data = interpreter.get_tensor(output_details[0]['index'])
    if TEST: print(output_data)

    #predict the image
    answer = np.argmax(output_data , axis=1 )

    if(output_data[answer[0]]>0,75){
        text = classes[answer[0]]
    }else{
        text = "Nothing"
    }
    return text

def test():
    predicted = detect('./test.jpeg')
    print ('Predicted : '+ predicted)

if __name__ == '__main__':
        test()