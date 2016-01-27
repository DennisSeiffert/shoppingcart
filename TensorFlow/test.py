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

import numpy
import PIL
from PIL import Image
from tensorflow.python.platform import gfile

import tensorflow as tf
import models

IMAGE_SIZE = 28
NUM_CHANNELS = 1
PIXEL_DEPTH = 255
NUM_LABELS = 10



def extract_data(filename):
    """Extract the images into a 4D tensor [image index, y, x, channels].
    Values are rescaled from [0, 255] down to [-0.5, 0.5].
    """
    print('Extracting', filename)

    im = Image.open(filename)
    img = im.resize((IMAGE_SIZE, IMAGE_SIZE), PIL.Image.ANTIALIAS)
    imagedata = list(img.getdata())
    data = numpy.asarray(imagedata, dtype=numpy.uint8).astype(numpy.float32)
    data = (data - (PIXEL_DEPTH / 2.0)) / PIXEL_DEPTH
    data = data.reshape(1, IMAGE_SIZE, IMAGE_SIZE, 1)

    return data

def create_graph():
  """"Creates a graph from saved GraphDef file and returns a saver."""
  # Creates graph from saved graph_def.pb.
  with gfile.FastGFile('tensorflow.pb', 'rb') as f:
    graph_def = tf.GraphDef()
    graph_def.ParseFromString(f.read())
    _ = tf.import_graph_def(graph_def, name='')


def main(argv=None):  # pylint: disable=unused-argument
    # Extract it into numpy arrays.
    image_data = extract_data("B_testImage.jpg")

    ######################################################
    eval_data = tf.placeholder(
            tf.float32,
            shape=(1, IMAGE_SIZE, IMAGE_SIZE, NUM_CHANNELS),
            name="test_data_input")

    # The variables below hold all the trainable weights. They are passed an
    # initial value which will be assigned when when we call:
    # {tf.initialize_all_variables().run()}
    conv1_weights = tf.Variable(
            tf.truncated_normal([5, 5, NUM_CHANNELS, 32],  # 5x5 filter, depth 32.
                                stddev=0.1,
                                seed=SEED,
                                name="conv1_weights"))
    conv1_biases = tf.Variable(tf.zeros([32]), name="conv1_biases")
    conv2_weights = tf.Variable(
            tf.truncated_normal([5, 5, 32, 64],
                                stddev=0.1,
                                seed=SEED))
    conv2_biases = tf.Variable(tf.constant(0.1, shape=[64]))
    fc1_weights = tf.Variable(  # fully connected, depth 512.
            tf.truncated_normal(
                    [IMAGE_SIZE // 4 * IMAGE_SIZE // 4 * 64, 512],
                    stddev=0.1,
                    seed=SEED))
    fc1_biases = tf.Variable(tf.constant(0.1, shape=[512]))
    fc2_weights = tf.Variable(
            tf.truncated_normal([512, NUM_LABELS],
                                stddev=0.1,
                                seed=SEED))
    fc2_biases = tf.Variable(tf.constant(0.1, shape=[NUM_LABELS]))

    #############################################################

    evalits = models.evaluation(models.inference(eval_data,
                              conv1_weights, conv1_biases,
                              conv2_weights, conv2_biases,
                              fc1_weights, fc1_biases,
                              fc2_weights, fc2_biases,
                              False))
    with tf.Session() as sess:
            predictions = sess.run(evalits, feed_dict={eval_data : eval_data})

if __name__ == '__main__':
    tf.app.run()
