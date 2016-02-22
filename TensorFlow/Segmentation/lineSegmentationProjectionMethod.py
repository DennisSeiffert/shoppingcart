import numpy as np

def sumUpIntensitiesPerRow(inputMatrix):
    idVector = np.transpose(np.mat([1 for it in range(inputMatrix.shape[1])]))
    return inputMatrix * idVector

def normalizeActivations(activations):
    return (1.0 / np.max(activations)) * activations

def calculateAverageLineLength(normedActivations,threshold):
    consecutiveLines = []
    counter = 0
    for row in range(0, len(normedActivations) - 1):
        if normedActivations[row] > threshold:
            counter += 1
        if normedActivations[row] > threshold > normedActivations[row + 1]:
            consecutiveLines.append(counter)
            counter = 0
    return round(np.mean(consecutiveLines))

def findLineBreaks(normedActivations, averageLineLength, threshold):
    detectedLineBreaks = []
    for i, x in enumerate(normedActivations):
        lookaheadWindow = normedActivations[i:i+averageLineLength]
        lookaheadWindowMean = np.mean(lookaheadWindow)
        if lookaheadWindowMean > 0.95 and x > threshold:
            detectedLineBreaks.append((i, i + averageLineLength - 1))
            print 'found line at ' + `(i, i + averageLineLength)`
    return detectedLineBreaks


def aggregateLineBreaks(lineBreaks):
    aggregatedLineBreaks = []
    accu = lineBreaks[0]
    for i, l in enumerate(lineBreaks):
        if accu[1] >= l[0]:
            accu = (accu[0], l[1])
        else:
            aggregatedLineBreaks.append(accu)
            accu = l
    aggregatedLineBreaks.append(accu)
    return aggregatedLineBreaks


def calculateBreaks(input):
    activations = sumUpIntensitiesPerRow(np.mat(input))
    normedActivations = normalizeActivations(activations)
    meanActivation = np.mean(normedActivations)
    averageLineLength = calculateAverageLineLength(normedActivations, meanActivation)
    lineBreaks = findLineBreaks(normedActivations, 3, meanActivation)
    lineBreaks = aggregateLineBreaks(lineBreaks)
    return lineBreaks

input = np.array([[0,0,0,0,125,255],
         [0,0,0,0,125,255],
         [0,0,0,0,125,255],
         [0,0,0,0,125,255],
         [255,255,255,255,125,255],
         [0,0,0,0,125,255],
         [0,0,0,0,125,255],
         [255,255,255,255,125,255],
         [255,255,255,255,125,255],
         [255,255,255,255,0,255],
         [0,0,0,0,125,255],
         [0,0,0,0,125,255],
         [0,0,0,0,125,255],
         [0,0,0,0,125,255],
         [255,255,255,255,125,255],
         [255,255,255,255,125,255],
         [255,255,255,255,0,255]])


print calculateBreaks(input)





from PIL import Image, ImageDraw

#imageRaw = Image.open('/commondata/Programming/Repositories/shoppingcart/ShoppingCartTests/testImage.jpg')
imageRaw = Image.open('/commondata/Pictures/KameraUploads/CAM00241.jpg')
if imageRaw.mode == 'RGB':
    imageRaw = imageRaw.convert('L')
imagedata = np.array(imageRaw.getdata())

mean = imagedata.mean()
data = imagedata.reshape(imageRaw.height, imageRaw.width)
data = 1.0 * (data > mean)

draw = ImageDraw.Draw(imageRaw)

stepSize = 10
for step in range(stepSize, imageRaw.width, stepSize):
    lastColumnVector = np.mean(data[:, step-stepSize:step], axis=1)
    boundingbox = (step-stepSize,0,step,0)
    for index, rowValue in enumerate(lastColumnVector):
        # white line => no characters detected
        if rowValue > 0.5:
            # draw.line((step-stepSize, index, step, index), width=2)
            if boundingbox[3] > boundingbox[1]:
                draw.rectangle(boundingbox, outline='#ffffff')
            boundingbox = (boundingbox[0], index, boundingbox[2], index)
        # black line => possible character detection
        else:
            # draw.point((step, index, step+2, index+2), fill=255)
            boundingbox = (boundingbox[0], boundingbox[1], boundingbox[2], index)

imageRaw.show()


from numpy import *
# denoise data
def denoise(im,U_init,tolerance=0.1,tau=0.125,tv_weight=100):
  """ An implementation of the Rudin-Osher-Fatemi (ROF) denoising model
    using the numerical procedure presented in eq (11) A. Chambolle (2005).

    Input: noisy input image (grayscale), initial guess for U, weight of
    the TV-regularizing term, steplength, tolerance for stop criterion.

    Output: denoised and detextured image, texture residual. """

  m,n = im.shape # size of noisy image

  # initialize
  U = U_init
  Px = im # x-component to the dual field
  Py = im # y-component of the dual field
  error = 1

  while (error > tolerance):
    Uold = U

    # gradient of primal variable
    GradUx = roll(U,-1,axis=1)-U # x-component of U's gradient
    GradUy = roll(U,-1,axis=0)-U # y-component of U's gradient

    # update the dual varible
    PxNew = Px + (tau/tv_weight)*GradUx
    PyNew = Py + (tau/tv_weight)*GradUy
    NormNew = maximum(1,sqrt(PxNew**2+PyNew**2))

    Px = PxNew/NormNew # update of x-component (dual)
    Py = PyNew/NormNew # update of y-component (dual)

    # update the primal variable
    RxPx = roll(Px,1,axis=1) # right x-translation of x-component
    RyPy = roll(Py,1,axis=0) # right y-translation of y-component

    DivP = (Px-RxPx)+(Py-RyPy) # divergence of the dual field.
    U = im + tv_weight*DivP # update of the primal variable

    # update of error
    error = linalg.norm(U-Uold)/sqrt(n*m);

  return U,im-U # denoised image and texture residual

def histeq(im,nbr_bins=256):

   #get image histogram
   imhist,bins = histogram(im.flatten(),nbr_bins,normed=True)
   cdf = imhist.cumsum() #cumulative distribution function
   cdf = 255 * cdf / cdf[-1] #normalize

   #use linear interpolation of cdf to find new pixel values
   im2 = interp(im.flatten(),bins[:-1],cdf)

   return im2.reshape(im.shape), cdf

image2, _ = histeq(array(imageRaw))
Image.fromarray(image2).show()


data = image2.flatten()
data = 255.0 * (data > 5.0)

from matplotlib import pyplot as plt
hist,bins = np.histogram(data,256,[0,256])

cdf = hist.cumsum()
cdf_normalized = cdf * hist.max()/ cdf.max()

plt.plot(cdf_normalized, color = 'b')
plt.hist(data,256,[0,256], color = 'r')
plt.xlim([0,256])
plt.legend(('cdf','histogram'), loc = 'upper left')
plt.show()


data = data.reshape(imageRaw.height, imageRaw.width)

# data, _ = denoise(data, data)

convertedImage = Image.fromarray(np.uint8(data))

convertedImage.show()

for linebreak in calculateBreaks(data):
    draw = ImageDraw.Draw(imageRaw)
    draw.rectangle((0, linebreak[0], imageRaw.width, linebreak[1]), outline=127)


imageRaw.show()