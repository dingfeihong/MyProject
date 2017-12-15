# -*- coding: utf-8 -*-
# filename: KMeans.py
from numpy import *
import pandas as pd
import matplotlib.pyplot as plt
import pdb 
#打开文件
def loadDataSet(fileName):    
    dataMat = pd.read_table(fileName,header=None,names=['x','y'])
    #print(dataMat)
    '''
    dataMat = []
    fr = open(fileName)
    for line in fr.readlines():
        curLine = mat(line.strip().split('\t')
        
        #fltLine = map(float,curLine)
        print(fltLine)
        dataMat.append(curLine)
    '''
    return dataMat

#计算欧式距离
def distEclud(vecA,vecB):
    return sqrt(sum(power(vecA-vecB,2)))

#构建包含K个随机质心的集合
def randCent(dataSet,k):
    n = shape(dataSet)[1]
    #centroids = pd.DataFrame(zeros((k,n)))
    minJ = pd.Series([min(dataSet[j]) for j in dataSet.columns])
        
    rangeJ = pd.Series([max(dataSet[j])-min(dataSet[j]) for j in dataSet.columns])
    randJ =  pd.DataFrame(random.rand(k,n))
    centroids = randJ[:]*rangeJ+minJ
    '''
    for j in range(n):
       
        #质心坐标范围[min(dataSet),max(dataSe)]
        centroids[:,j] = mat(minJ+rangeJ*random.rand(k,1))
    '''
    return centroids

#KMeans
#datSet:数据集
#distMeas:距离计算
#createCent:中心点集
def kMeans(dataSet,k,distMeas=distEclud,createCent=randCent):
    m = shape(dataSet)[0]   #数据点个数
    clusterAssment = pd.DataFrame(zeros((m,2)))  #
    centriods = createCent(dataSet,k)   #聚类中心点集
    changeSign = True       #算法终止标记
    while changeSign:
        #计算
        changeSign = False  #重置迭代终止标记
        for i in range(m):
            minDist = inf   #当前最小距离
            minIndex = -1   #当前最小距离的点集下标
            #计算点到每个聚类中心距离
            for j in range(k):
                
                tmp = distMeas(dataSet.ix[i],centriods.ix[j])

                #如果当前点离该聚类中心距离更近，更新最近距离和最近聚类中心
                if tmp < minDist:
                    minDist = tmp
                    minIndex = j

            #当最近距离发生改变，重置标记并记录改变
            if clusterAssment.iat[i,0] != minIndex:
                changeSign = True
            clusterAssment.ix[i] = minIndex,minDist**2
        #print centriods

        #重新计算k个聚类中心
        for i in range(k):
            #pdb.set_trace() 
            #centData = dataSet[nonzero(clusterAssment[:,0].A == i)[0]]
            centriods.ix[i] = mean(dataSet.ix[[j for j in range(m) if dataSet.iat[j,0]==i)
        
    return centriods,clusterAssment
if __name__=="__main__":
    datMat = loadDataSet('testSet.txt')
    
    centroids, cluster = kMeans(datMat,4)
    print(centroids)
    print(centroids[:,0].A)
    fig = plt.figure()  
    ax1 = fig.add_subplot(111)  
    ax1.set_title('Scatter Plot')   
    #设置X轴标签  
    plt.xlabel('X')  
    #设置Y轴标签  
    plt.ylabel('Y')  
    #画散点图  
    ax1.scatter(list(datMat[:,0]),list(datMat[:,1]))  
    ax1.scatter(list(centroids[:,0]),list(centroids[:,1]))  
    #显示所画的图  
    plt.show()  
