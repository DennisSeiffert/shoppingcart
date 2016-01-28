#!/usr/bin/env python
import web
import test

urls = (
    '/classifier', 'options_classification'
    '/classifier/(.*)', 'get_classification'
)

app = web.application(urls, globals())

class options_classification:
    def OPTIONS(self):
        return urls

class get_classification:
    def GET(self, image):
	    return test.evaluate(image)

if __name__ == "__main__":
    app.run()