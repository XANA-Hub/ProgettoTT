from tensorflow.keras.preprocessing.image import ImageDataGenerator
from tensorflow.keras.layers import Conv2D , MaxPooling2D , Flatten, Dense, Dropout
from tensorflow.keras.models import Sequential
from tensorflow.keras.optimizers import Adam
from tensorflow.keras.callbacks import EarlyStopping , ModelCheckpoint
import matplotlib.pyplot as plt
from glob import glob
from datetime import datetime
from packaging import version
import tensorflow as tf
from PIL import Image
import os



imgWidth = 256
imgHeight = 256
batchSize = 32
numOfEpochs = 100

%reload_ext tensorboard #magic command

NumOfClasses = 6
print (NumOfClasses)

if not os.path.exists('/content/training/classe_esempio'):
    # Crea la cartella
    os.makedirs('/content/training/classe_esempio')
    print(f"La cartella '{'/content/training/classe_esempio'}' è stata creata con successo.")
else:
    print(f"La cartella '{'/content/training/classe_esempio'}' esiste già.")

# Crea un'immagine vuota
dummy_image = Image.new('RGB', (256, 256), (255, 255, 255))  # Immagine bianca

# Salva l'immagine nella cartella di training
dummy_image.save('/content/training/classe_esempio/dummy_image.jpg')
try:
    img = Image.open('/content/training/classe_esempio/dummy_image.jpg')
    print("L'immagine è stata caricata correttamente.")
finally:
    pass

#data augmentation
train_datagen = ImageDataGenerator(rescale = 1/255.0,
                                    rotation_range = 30 ,
                                    zoom_range = 0.4 ,
                                    horizontal_flip=True,
                                    shear_range=0.4)


train_generator = train_datagen.flow_from_directory("/content/training/",
                                                    batch_size = batchSize,
                                                    class_mode = 'categorical',
                                                    target_size = (imgHeight,imgWidth))


validation_DIR = "./"
val_datagen = ImageDataGenerator(rescale = 1/255.0)

val_generator = val_datagen.flow_from_directory("/content/training/", #per semplicita'
                                                batch_size = batchSize,
                                                class_mode='categorical',
                                                target_size = (imgHeight, imgWidth))


#early stopping
callBack = EarlyStopping(monitor='val_loss', patience=5, verbose=1, mode='auto')


bestModelFileName = "./"
bestModel = ModelCheckpoint(bestModelFileName, monitor='val_accuracy', verbose=1, save_best_only=True)

#struttura rete
model = Sequential([
    Conv2D(32, (3,3) , activation='relu' , input_shape=(imgHeight, imgWidth, 3) ) ,
    MaxPooling2D(2,2),

    Conv2D(64 , (3,3) , activation='relu'),
    MaxPooling2D(2,2),

    Conv2D(64 , (3,3) , activation='relu'),
    MaxPooling2D(2,2),

    Conv2D(128 , (3,3) , activation='relu'),
    MaxPooling2D(2,2),

    Conv2D(256 , (3,3) , activation='relu'),
    MaxPooling2D(2,2),

    Flatten(),

    Dense(512 , activation='relu'),
    Dense(512 , activation='relu'),

    Dense(NumOfClasses , activation='softmax')
])

print (model.summary() )

model.compile(optimizer='Adam', loss='categorical_crossentropy', metrics=['accuracy'])

logdir="logs"
writer = tf.summary.create_file_writer(logdir)
tf.summary.trace_on(graph=True, profiler=True)
#tensorboard_callback = tf.keras.callbacks.TensorBoard(log_dir=logdir)

history = model.fit(train_generator,
                    epochs = 1,
                    verbose=1,
                    validation_data = val_generator,
                    callbacks = [bestModel])
with writer.as_default():
    tf.summary.trace_export(name="model_trace", step=0, profiler_outdir=logdir)

%tensorboard --logdir logs #magic command