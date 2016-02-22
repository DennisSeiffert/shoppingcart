#!/usr/bin/env python
from PIL import Image
from cStringIO import StringIO

import web

import test
import models
import tensorflow as tf1

urls = (
    '/classifier/(.*)', 'classification',
    '/classifier', 'classification'
)

app = web.application(urls, globals())


class classification:
    def GET(self, image):
        return image
    def POST(self):
        x = web.input(imageFile={})
        if 'imageFile' in x:
            imageRawData = x.imageFile.file.read()
            file_jpgdata = StringIO(imageRawData)
            imageData = Image.open(file_jpgdata)
            preprocessed_image_data = test.preprocess_image_data(imageData)
            tf1.reset_default_graph()
            conv1_weights, conv1_biases, conv2_weights, conv2_biases, fc1_weights, fc1_biases, fc2_weights, fc2_biases, batch = models.createVariables()
            return test.evaluate(preprocessed_image_data,
                                 conv1_weights, conv1_biases,
                                 conv2_weights, conv2_biases,
                                 fc1_weights, fc1_biases,
                                 fc2_weights, fc2_biases)
        return -1

if __name__ == "__main__":
    app.run()