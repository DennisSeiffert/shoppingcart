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
import gzip
import numpy
import sys
import time
import os

import models
from PIL import Image



import tensorflow as tf
from six.moves import urllib
from six.moves import xrange  # pylint: disable=redefined-builtin

SOURCE_URL = 'http://yann.lecun.com/exdb/mnist/'
WORK_DIRECTORY = 'data'
VALIDATION_SIZE = 3000  # Size of the validation set.
BATCH_SIZE = 64
NUM_EPOCHS = 600
EVAL_BATCH_SIZE = 64
EVAL_FREQUENCY = 2000  # Number of steps between evaluations.

tf.app.flags.DEFINE_boolean("self_test", False, "True if running a self test.")
FLAGS = tf.app.flags.FLAGS

def maybe_download(filename):
  """Download the data from Yann's website, unless it's already here."""
  if not os.path.exists(WORK_DIRECTORY):
    os.mkdir(WORK_DIRECTORY)
  filepath = os.path.join(WORK_DIRECTORY, filename)
  if not os.path.exists(filepath):
    filepath, _ = urllib.request.urlretrieve(SOURCE_URL + filename, filepath)
    statinfo = os.stat(filepath)
    print('Successfully downloaded', filename, statinfo.st_size, 'bytes.')
  return filepath


def extract_mnist_data(filename, num_images):
  """Extract the images into a 4D tensor [image index, y, x, channels].
  Values are rescaled from [0, 255] down to [-0.5, 0.5].
  """
  print('Extracting', filename)
  with gzip.open(filename) as bytestream:
    bytestream.read(16)
    buf = bytestream.read(models.IMAGE_SIZE * models.IMAGE_SIZE * num_images)
    data = numpy.frombuffer(buf, dtype=numpy.uint8).astype(numpy.float32)
    data = (data - (models.PIXEL_DEPTH / 2.0)) / models.PIXEL_DEPTH
    data = data.reshape(num_images, models.IMAGE_SIZE, models.IMAGE_SIZE, 1)
    return data


def extract_mnist_labels(filename, num_images):
  """Extract the labels into a 1-hot matrix [image index, label index]."""
  print('Extracting', filename)
  with gzip.open(filename) as bytestream:
    bytestream.read(8)
    buf = bytestream.read(1 * num_images)
    labels = numpy.frombuffer(buf, dtype=numpy.uint8)
  # Convert to dense 1-hot representation.
  return (numpy.arange(models.NUM_LABELS) == labels[:, None]).astype(numpy.float32)


def extract_data(filename):
    """Extract the images into a 4D tensor [image index, y, x, channels].
    Values are rescaled from [0, 255] down to [-0.5, 0.5].
    """
    print('Extracting', filename)
    images = [line.rstrip('\r\n').split(" ", 2) for line in open(filename)]
    labels = numpy.ndarray(shape=(len(images), models.NUM_LABELS), dtype=numpy.float32)
    imageData = []
    i = 0
    for tuples in images:
        imageFilename = tuples[0]
        labels[i][:] = 0.0
        labels[i][int(tuples[1])] = 1.0
        im = Image.open("CharacterRepository/%s" % imageFilename)
        im = im.resize((models.IMAGE_SIZE, models.IMAGE_SIZE), PIL.Image.ANTIALIAS)
        imageData.extend(list(im.getdata()))
        i += 1
    data = numpy.asarray(imageData, dtype=numpy.uint8).astype(numpy.float32)
    data = 255.0 - data
    for x in numpy.nditer(data, op_flags=['readwrite']):
        if x[...] < 100.0:
            x[...] = 0.0
        else:
            x[...] = 255.0
    data = (data - (models.PIXEL_DEPTH / 2.0)) / models.PIXEL_DEPTH
    data = data.reshape(len(images), models.IMAGE_SIZE, models.IMAGE_SIZE, 1)
    return (data, labels)

def extract_labels(filename, num_images):
    """Extract the labels into a 1-hot matrix [image index, label index]."""
    print('Extracting', filename)
    with gzip.open(filename) as bytestream:
        bytestream.read(8)
        buf = bytestream.read(1 * num_images)
        labels = numpy.frombuffer(buf, dtype=numpy.uint8)
    # Convert to dense 1-hot representation.
    return (numpy.arange(models.NUM_LABELS) == labels[:, None]).astype(numpy.float32)


def main(argv=None):  # pylint: disable=unused-argument
    # Extract it into numpy arrays.
    (train_data, train_labels) = extract_data("CharacterRepository/train_images.txt")
    (test_data, test_labels) = extract_data("CharacterRepository/test_images.txt")

        # Get the data.
    #train_data_filename = maybe_download('train-images-idx3-ubyte.gz')
    #train_labels_filename = maybe_download('train-labels-idx1-ubyte.gz')
    #test_data_filename = maybe_download('t10k-images-idx3-ubyte.gz')
    #test_labels_filename = maybe_download('t10k-labels-idx1-ubyte.gz')

    # Extract it into numpy arrays.
    #train_data = extract_mnist_data(train_data_filename, 60000)
    #train_labels = extract_mnist_labels(train_labels_filename, 60000)
    #test_data = extract_mnist_data(test_data_filename, 10000)
    #test_labels = extract_mnist_labels(test_labels_filename, 10000)

    # Generate a validation set.
    validation_data = train_data[:VALIDATION_SIZE, ...]
    validation_labels = train_labels[:VALIDATION_SIZE]
    #train_data = train_data[VALIDATION_SIZE:, ...]
    #train_labels = train_labels[VALIDATION_SIZE:]
    num_epochs = NUM_EPOCHS
    train_size = train_labels.shape[0]

    # This is where training samples and labels are fed to the graph.
    # These placeholder nodes will be fed a batch of training data at each
    # training step using the {feed_dict} argument to the Run() call below.
    train_data_node = tf.placeholder(
            tf.float32,
            shape=(BATCH_SIZE, models.IMAGE_SIZE, models.IMAGE_SIZE, models.NUM_CHANNELS),
            name="training_data_input")
    train_labels_node = tf.placeholder(
            tf.float32,
            shape=(BATCH_SIZE, models.NUM_LABELS),
            name="training_labels_input")
    eval_data = tf.placeholder(
            tf.float32,
            shape=(EVAL_BATCH_SIZE, models.IMAGE_SIZE, models.IMAGE_SIZE, models.NUM_CHANNELS),
            name="test_data_input")

    conv1_weights, conv1_biases, conv2_weights, conv2_biases, fc1_weights, fc1_biases, fc2_weights, fc2_biases, batch = models.createVariables()

    # Training computation: logits + cross-entropy loss.
    logits = models.inference(train_data_node,
                              conv1_weights, conv1_biases, conv2_weights, conv2_biases, fc1_weights, fc1_biases, fc2_weights, fc2_biases, True)
    evalits = models.inference(eval_data,
                               conv1_weights, conv1_biases, conv2_weights, conv2_biases, fc1_weights, fc1_biases, fc2_weights, fc2_biases, False)
    loss = models.loss(logits, train_labels_node)

    learning_rate = models.createLearningRate(BATCH_SIZE, train_size, batch)

    optimizer = models.training(loss, learning_rate, batch,
                                fc1_weights, fc1_biases, fc2_weights, fc2_biases)

    # Predictions for the current training minibatch.
    train_prediction = models.evaluation(logits)

    # Predictions for the test and validation, which we'll compute less often.
    eval_prediction = models.evaluation(evalits)

    tf.scalar_summary('learning rate', learning_rate)
    tf.scalar_summary('loss', loss)
    tf.image_summary('training input images', train_data_node, 10)
    tf.histogram_summary('learned input image patterns', train_prediction)


    summary_op = tf.merge_all_summaries()
    # Small utility function to evaluate a dataset by feeding batches of data to
    # {eval_data} and pulling the results from {eval_predictions}.
    # Saves memory and enables this to run on smaller GPUs.
    def eval_in_batches(data, sess):
        """Get all predictions for a dataset by running it in small batches."""
        size = data.shape[0]
        if size < EVAL_BATCH_SIZE:
            raise ValueError("batch size for evals larger than dataset: %d" % size)
        predictions = numpy.ndarray(shape=(size, models.NUM_LABELS), dtype=numpy.float32)
        for begin in xrange(0, size, EVAL_BATCH_SIZE):
            end = begin + EVAL_BATCH_SIZE
            if end <= size:
                predictions[begin:end, :] = sess.run(
                        eval_prediction,
                        feed_dict={eval_data: data[begin:end, ...]})
            else:
                batch_predictions = sess.run(
                        eval_prediction,
                        feed_dict={eval_data: data[-EVAL_BATCH_SIZE:, ...]})
                predictions[begin:, :] = batch_predictions[begin - size:, :]
        return predictions

    # Create a local session to run the training.
    start_time = time.time()
    with tf.Session(config=tf.ConfigProto(log_device_placement=True)) as sess:
        # Run all the initializers to prepare the trainable parameters.
        tf.initialize_all_variables().run()
        print('Initialized!')

        summaryWriter = tf.train.SummaryWriter("/tmp/tensorflow" , graph_def=sess.graph_def)
        saver = tf.train.Saver();
        # Loop through training steps.
        for step in xrange(int(num_epochs * train_size) // BATCH_SIZE):
            # Compute the offset of the current minibatch in the data.
            # Note that we could use better randomization across epochs.
            offset = (step * BATCH_SIZE) % (train_size - BATCH_SIZE)
            batch_data = train_data[offset:(offset + BATCH_SIZE), ...]

            batch_labels = train_labels[offset:(offset + BATCH_SIZE)]
            # This dictionary maps the batch data (as a numpy array) to the
            # node in the graph is should be fed to.
            feed_dict = {train_data_node: batch_data,
                         train_labels_node: batch_labels}
            # Run the graph and fetch some of the nodes.
            _, l, lr, predictions = sess.run(
                    [optimizer, loss, learning_rate, train_prediction],
                    feed_dict=feed_dict)
            if step % EVAL_FREQUENCY == 0:
                elapsed_time = time.time() - start_time
                start_time = time.time()
                summaryWriter.add_summary(sess.run(summary_op, feed_dict = feed_dict), step)
                saver.save(sess, 'checkpoints/model.ckpt', global_step=step)
                print('Step %d (epoch %.2f), %.1f ms' %
                      (step, float(step) * BATCH_SIZE / train_size,
                       1000 * elapsed_time / EVAL_FREQUENCY))
                print('Minibatch loss: %.3f, learning rate: %.6f' % (l, lr))
                print('Minibatch error: %.1f%%' % models.error_rate(predictions, batch_labels))
                print('Validation error: %.1f%%' % models.error_rate(eval_in_batches(validation_data, sess), validation_labels))
                sys.stdout.flush()
        # Finally print the result!
        test_error = models.error_rate(eval_in_batches(test_data, sess), test_labels)
        print('Test error: %.1f%%' % test_error)

        if FLAGS.self_test:
            print('test_error', test_error)
            assert test_error == 0.0, 'expected 0.0 test_error, got %.2f' % (
                test_error,)


if __name__ == '__main__':
    tf.app.run()
