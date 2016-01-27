from __future__ import absolute_import
from __future__ import division
from __future__ import print_function

import tensorflow as tf


def inference(data,
              conv1_weights,
              conv1_biases,
              conv2_weights,
              conv2_biases,
              fc1_weights,
              fc1_biases,
              fc2_weights,
              fc2_biases,
              train=False):
    SEED = 66478
    conv1 = tf.nn.conv2d(data,
                        conv1_weights,
                        strides=[1, 1, 1, 1],
                        padding='SAME',
                        name="conv1")
    relu1 = tf.nn.relu(tf.nn.bias_add(conv1, conv1_biases),
                        name="relu1")
    pool1 = tf.nn.max_pool(relu1,
                          ksize=[1, 2, 2, 1],
                          strides=[1, 2, 2, 1],
                          padding='SAME',
                          name="pool1")
    conv2 = tf.nn.conv2d(pool1,
                        conv2_weights,
                        strides=[1, 1, 1, 1],
                        padding='SAME',
                        name="conv2")
    relu2 = tf.nn.relu(tf.nn.bias_add(conv2, conv2_biases),
                        name="relu2")
    pool2 = tf.nn.max_pool(relu2,
                          ksize=[1, 2, 2, 1],
                          strides=[1, 2, 2, 1],
                          padding='SAME',
                          name="pool2")
    pool_shape = pool2.get_shape().as_list()
    reshape = tf.reshape(
            pool2,
            [pool_shape[0], pool_shape[1] * pool_shape[2] * pool_shape[3]])
    hidden = tf.nn.relu(tf.matmul(reshape, fc1_weights) + fc1_biases)
    # Add a 50% dropout during training only. Dropout also scales
    # activations such that no rescaling is needed at evaluation time.
    if train:
        hidden = tf.nn.dropout(hidden, 0.5, seed=SEED)
    return tf.matmul(hidden, fc2_weights) + fc2_biases


def loss(logits, labels):
    loss = tf.reduce_mean(tf.nn.softmax_cross_entropy_with_logits(logits, labels))
    return loss

def training(loss, batch_size, train_size,
             fc1_weights, fc1_biases,
             fc2_weights, fc2_biases):

    tf.scalar_summary(loss.op.name, loss)
        # L2 regularization for the fully connected parameters.
    regularizers = (tf.nn.l2_loss(fc1_weights) + tf.nn.l2_loss(fc1_biases) +
                    tf.nn.l2_loss(fc2_weights) + tf.nn.l2_loss(fc2_biases))
    # Add the regularization term to the loss.
    loss += 5e-4 * regularizers

    # Optimizer: set up a variable that's incremented once per batch and
    # controls the learning rate decay.
    batch = tf.Variable(0)
    # Decay once per epoch, using an exponential schedule starting at 0.01.
    learning_rate = tf.train.exponential_decay(
            0.01,  # Base learning rate.
            batch * batch_size,  # Current index into the dataset.
            train_size,  # Decay step.
            0.95,  # Decay rate.
            staircase=True)
    # Use simple momentum for the optimization.
    return tf.train.MomentumOptimizer(learning_rate,
                                           0.9).minimize(loss,
                                                         global_step=batch)

def evaluation(logits):
    return tf.nn.softmax(logits)
