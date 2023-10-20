import tflite_runtime.interpreter as tflite
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

pic = pic.resize((224, 224), Image.ANTIALIAS)
pic.show()
pic = np.asarray(pic, dtype=np.float32)
pic = np.expand_dims(pic, axis=0)
pic /= 255 
pic = (pic - train_mean) / train_std
pic = np.transpose(pic, [0, 3, 1, 2])

input_tensor = np.array(pic, dtype=np.float32)
# Load the TFLite model and allocate tensors.
interpreter.set_tensor(input_details[0]['index'], input_tensor)
interpreter.invoke()

# Get output
output_data = interpreter.get_tensor(output_details[0]['index'])
print(output_data)

'''
results = np.squeeze(output_data, axis=0)
top_k_idx = np.argsort(results)[-num_top_pokemon:][::-1]
top_k_scores = results[top_k_idx]
top_k_labels = label_encoder.inverse_transform(top_k_idx)

def softmax(x):
    """Compute softmax values for each sets of scores in x."""
    e_x = np.exp(x - np.max(x))
    return e_x / e_x.sum()

top_k_scores = softmax(top_k_scores)

print("\n".join([f"{pred}: {score:.2%}" for (pred, score) in zip(top_k_labels, top_k_scores)]))
'''