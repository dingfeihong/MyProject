 # -*- coding: utf-8 -*-
# filename: KMeans.py
from numpy import *
import pandas as pd
import matplotlib.pyplot as plt
import pdb 
#打开文件

def loadDataSet(fileName):    
    table = pd.read_table(fileName,header=None)
    table.insert(0,-1,1)
    table.columns=table.columns+1
    #print(dataMat)
    data = table.iloc[:, 0:3]
    label = table[3]
    return data,label

def sigmoid(inX):
    return 1.0/(1+exp(-inX))

def gradAscent(dataMat,labelMat):
    #data=dataMat.iloc[:,0:3]
    #label=dataMat[3]
    m,n=shape(dataMat)
    alpha=0.001
    maxCycles=500
    weights = pd.Series([1.0 for x in range(n)])
    for i in range(maxCycles):
        h=sigmoid(dataMat.dot(weights))
        error=labelMat-h
        weights=weights+alpha*dataMat.T.dot(error)
    return weights


def stocgradAscent(dataMat,labelMat,numIter=150):
    #data=dataMat.iloc[:,0:3]
    #label=dataMat[3]
    m,n=shape(dataMat)

    weights = pd.Series([1.0 for x in range(n)])
    for i in range(numIter):
        dataIndex=[m for m in range(m)]
             for j in range(m):
            alpha = 4/(1.0+i+j)+0.01
            randIndex = int(random.uniform(0, len(dataIndex)))
            h=sigmoid(sum(dataMat.iloc[randIndex].dot(weights)))
            error=labelMat[randIndex]-h
            weights=weights+alpha*dataMat.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     [randIndex]*error
            del(dataIndex[randIndex])
    return weights


def plotBestFit(weights,dataMat, labelMat):
    n = shape(dataMat)[0]
    xcord1 = []; ycord1 = []
    xcord2 = []; ycord2 = []
    for i in range(n):
        if (labelMat[i])== 1:
            xcord1.append(dataMat.iat[i,1]); ycord1.append(dataMat.iat[i,2])
        else:
            xcord2.append(dataMat.iat[i,1]); ycord2.append(dataMat.iat[i,2])
    fig = plt.figure()
    ax = fig.add_subplot(111)
    ax.scatter(xcord1, ycord1, s=30, c='red', marker='s')
    ax.scatter(xcord2, ycord2, s=30, c='green')
    x = arange(-3.0, 3.0, 0.1)
    y = (-weights[0]-weights[1]*x)/weights[2]
    ax.plot(x, y)
    plt.xlabel('X1'); plt.ylabel('X2');
    plt.show()

if __name__=="__main__":
    data,label=loadDataSet('testSet.txt')
    weight=stocgradAscent(data, label)
    print(weight)
    plotBestFit(weight, data, label)