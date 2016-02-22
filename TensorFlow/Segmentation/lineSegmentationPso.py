import numpy as np
from pyswarm import pso

# Define the objective (to be minimize)
def optimumLine(x, *args):
    i = x
    img = args
    lineCorridor = img[max((int(i)-1),0):min((int(i)+2, len(img)))]
    corridor = np.mat(lineCorridor) * np.transpose(np.mat([1 for it in range(200)]))
    return corridor.sum() + int(i)

def iIsInteger(x, *args):
    i = x
    return np.round(i,0)

# Define the other parameters
image = [[0 for x in range(200)] for y in range(500)]
args = image

# Define the lower and upper bounds for H, d, t, respectively
lb = [0]
ub = [5]

xopt, fopt = pso(optimumLine, lb, ub, args=args)

print xopt, fopt

# The optimal input values are approximately
#     xopt = [29, 2.4, 0.06]
# with function values approximately
#     weight          = 12 lbs
#     yield stress    = 100 kpsi (binding constraint)
#     buckling stress = 150 kpsi
#     deflection      = 0.2 in