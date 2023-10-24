from PIL import Image
import tflite_runtime.interpreter as tflite
import numpy as np

TEST = True

#Labels ordered
classes = ["Bishop","King","Knight", "Pawn","Queen","Rook"]

def detect(imagePath):
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

    if TEST: print ('class : '+ classes[answer[0]])
    if TEST: print ('prob : '+ str(output_data[0][answer[0]]))
    tuple = (classes[answer[0]], output_data[0][answer[0]])
    if TEST: print ('Predicted : '+ str(tuple))
    return tuple

def test():
    predicted = detect('./image.jpg')

if __name__ == '__main__':
        test()