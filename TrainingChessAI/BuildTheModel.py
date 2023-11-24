from tensorflow.keras.preprocessing.image import ImageDataGenerator
from tensorflow.keras.layers import Conv2D , MaxPooling2D , Flatten, Dense, Dropout
from tensorflow.keras.models import Sequential
from tensorflow.keras.optimizers import Adam
from tensorflow.keras.callbacks import EarlyStopping , ModelCheckpoint
import tensorflow as tf
import matplotlib.pyplot as plt

from glob import glob

imgWidth = 256
imgHeight = 256
batchSize = 32
numOfEpochs = 100

TRAINING_DIR = "./Chessman-image-dataset/train"

NumOfClasses = len(glob('./Chessman-image-dataset/train/*'))
print (NumOfClasses)

#data augmentation
train_datagen = ImageDataGenerator(rescale = 1/255.0,
                                    rotation_range = 30 ,
                                    zoom_range = 0.4 ,
                                    horizontal_flip=True,
                                    shear_range=0.4)


train_generator = train_datagen.flow_from_directory(TRAINING_DIR,
                                                    batch_size = batchSize,
                                                    class_mode = 'categorical',
                                                    target_size = (imgHeight,imgWidth))


validation_DIR = "./Chessman-image-dataset/validation"
val_datagen = ImageDataGenerator(rescale = 1/255.0)

val_generator = val_datagen.flow_from_directory(validation_DIR,
                                                batch_size = batchSize,
                                                class_mode='categorical',
                                                target_size = (imgHeight, imgWidth))


#early stopping
callBack = EarlyStopping(monitor='val_loss', patience=5, verbose=1, mode='auto')


bestModelFileName = "./Chessman-image-dataset/chess_best_model.h5"
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

@tf.function
def traceme(x):
    return model(x)


logdir = "log"
writer = tf.summary.create_file_writer(logdir)
tf.summary.trace_on(graph=True, profiler=True)
# Forward pass
traceme(tf.zeros((1,256,256,3)))
with writer.as_default():
    tf.summary.trace_export(name="model_trace", step=0, profiler_outdir=logdir)
#tf.keras.utils.plot_model(model, to_file='model_plot.png', show_shapes=True, show_layer_names=True)

exit(0)

history = model.fit(train_generator,
                    epochs = numOfEpochs,
                    verbose=1,
                    validation_data = val_generator,
                    callbacks = [bestModel])

#stampa risultati ottenuti
acc = history.history['accuracy']
val_acc = history.history['val_accuracy']
loss = history.history['loss']
val_loss = history.history['val_loss']

epochs = range(len(acc))

#accuracy
fig = plt.figure(figsize=(14,7))
plt.plot(epochs, acc , 'r', label="Train accuracy")
plt.plot(epochs, val_acc , 'b', label="Validation accuracy")
plt.xlabel('Epochs')
plt.ylabel('Accuracy')
plt.title('Train and validation accuracy')
plt.legend(loc='lower right')
plt.show()

#loss
fig2 = plt.figure(figsize=(14,7))
plt.plot(epochs, loss , 'r', label="Train loss")
plt.plot(epochs, val_loss , 'b', label="Validation loss")
plt.xlabel('Epochs')
plt.ylabel('Loss')
plt.title('Train and validation Loss')
plt.legend(loc='upper right')
plt.show()

