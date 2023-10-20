import tensorflow as tf
from keras.models import load_model


# Convert the model.
model = load_model("./Chessman-image-dataset/chess_best_model.h5")
converter = tf.lite.TFLiteConverter.from_keras_model(model)
tflite_model = converter.convert()

# Save the model.
with open('model.tflite', 'wb') as f:
  f.write(tflite_model)