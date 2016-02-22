# Copyright 2015 Google Inc. All Rights Reserved.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
# ==============================================================================

"""Simple, end-to-end, LeNet-5-like convolutional MNIST model example.

This should achieve a test error of 0.7%. Please keep this model as simple and
linear as possible, it is meant as a tutorial for simple convolutional models.
Run with --self_test on the command line to exectute a short self-test.
"""
from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import PIL
import numpy
from PIL import Image
from threading import Lock

import tensorflow as tf

import models

def preprocess_image_data(im):
    """Extract the images into a 4D tensor [image index, y, x, channels].
    Values are rescaled from [0, 255] down to [-0.5, 0.5].
    """
    img = im.resize((models.IMAGE_SIZE, models.IMAGE_SIZE), PIL.Image.ANTIALIAS)
    if img.mode == 'RGB':
        img = img.convert('1')
    imagedata = list(img.getdata())
    data = numpy.asarray(imagedata, dtype=numpy.uint8).astype(numpy.float32)
    data = 255.0 - data
    for x in numpy.nditer(data, op_flags=['readwrite']):
        if x[...] < 100.0:
            x[...] = 0.0
        else:
            x[...] = 255.0
    data = (data - (models.PIXEL_DEPTH / 2.0)) / models.PIXEL_DEPTH
    data = data.reshape(1, models.IMAGE_SIZE, models.IMAGE_SIZE, models.NUM_CHANNELS)

    return data

def evaluate(image_data,
              conv1_weights, conv1_biases,
                                                 conv2_weights, conv2_biases,
                                                 fc1_weights, fc1_biases,
                                                 fc2_weights, fc2_biases):
    eval_data = tf.placeholder(
            tf.float32,
            shape=(1, models.IMAGE_SIZE, models.IMAGE_SIZE, models.NUM_CHANNELS),
            name="test_data_input")

    evalits = models.evaluation(models.inference(eval_data,
                                                 conv1_weights, conv1_biases,
                                                 conv2_weights, conv2_biases,
                                                 fc1_weights, fc1_biases,
                                                 fc2_weights, fc2_biases,
                                                 False))


    with models.tf.Session() as sess:
        lock = Lock()
        lock.acquire()
        try:
            saver = models.tf.train.Saver([ conv1_weights, conv1_biases,
                                                 conv2_weights, conv2_biases,
                                                 fc1_weights, fc1_biases,
                                                 fc2_weights, fc2_biases])
            ckpt = models.tf.train.get_checkpoint_state('checkpoints')
            if ckpt and ckpt.model_checkpoint_path:
                saver.restore(sess, ckpt.model_checkpoint_path)
        finally:
            lock.release() # release lock, no matter what
        argmax = tf.arg_max(evalits,1)
        return sess.run(argmax, feed_dict={eval_data : image_data})
    return -1

def main(argv=None):  # pylint: disable=unused-argument
    conv1_weights, conv1_biases, conv2_weights, conv2_biases, fc1_weights, fc1_biases, fc2_weights, fc2_biases, batch = models.createVariables()
    for i in range(0, 10, 1):
        image_data = preprocess_image_data(Image.open("CharacterRepository/data/test_Arial_Italic-"+str(i % 9)+".jpg"))
        result = evaluate(image_data,
                          conv1_weights, conv1_biases,
                          conv2_weights, conv2_biases,
                          fc1_weights, fc1_biases,
                          fc2_weights, fc2_biases)
        print(i % 9,result)

if __name__ == '__main__':
    tf.app.run()
