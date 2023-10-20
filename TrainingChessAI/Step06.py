from PIL import Image
import tflite_runtime.interpreter as tflite
import tensorflow as tf
import numpy as np

# answers ordered
classes = ["Bishop","King","Knight", "Pawn","Queen","Rook"]

# Predicts num_top_pokemon from image_file, using a tflite model
TFLITE_MODEL = "./model.tflite"
interpreter = tf.lite.Interpreter(TFLITE_MODEL)
interpreter.allocate_tensors()

# Get input and output tensors
input_details = interpreter.get_input_details()
output_details = interpreter.get_output_details()

# Load sample image
pic = './test.jpg'
pic = Image.open(pic)

pic = pic.resize((256, 256), Image.LANCZOS)
#pic.show()
pic = np.asarray(pic, dtype=np.float32)
pic = np.expand_dims(pic, axis=0)
pic /= 255 
#pic = (pic - train_mean) / train_std
#pic = np.transpose(pic, [0, 3, 1, 2])

input_tensor = np.array(pic, dtype=np.float32)
# Load the TFLite model and allocate tensors.
interpreter.set_tensor(input_details[0]['index'], input_tensor)
interpreter.invoke()

# Get output
output_data = interpreter.get_tensor(output_details[0]['index'])
print(output_data)
#output define the chance of the image being the corresponding piece
#aka the first number in the array defines how likely the image is to be a Bishop

#predict the image
#resultArray = model.predict(imageForModel , batch_size=32 , verbose=1)
answer = np.argmax(output_data , axis=1 )

text = classes[answer[0]]
print ('Predicted : '+ text)





