 # -*- coding: utf-8 -*-
# filename: EnergyTset.py
import numpy as np
import pandas as pd
from sklearn.cross_validation import train_test_split
from sklearn.linear_model import LinearRegression
from sklearn.model_selection import cross_val_predict
from sklearn import svm, datasets
from sklearn import preprocessing
from sklearn.metrics import mean_squared_error,r2_score
from matplotlib.ticker import MultipleLocator, FormatStrFormatter
import matplotlib.pyplot as plt

import datetime
from sklearn.model_selection import GridSearchCV
import pdb
#打开文件

def loadDataSet(fileName):
    #读取数据
    table = pd.read_table (fileName,header = 0,sep=",")
    return table

def FigurePlot():
    ax = plt.subplot(111)
    xmajorLocator = MultipleLocator(24)  # 将x主刻度标签设置为20的倍数
    xmajorFormatter = FormatStrFormatter('%5.1f')  # 设置x轴标签文本的格式
    ymajorLocator = MultipleLocator(0.5)  # 将y轴主刻度标签设置为0.5的倍数
    ymajorFormatter = FormatStrFormatter('%1.1f')  # 设置y轴标签文本的格式
    # 设置主刻度标签的位置,标签文本的格式
    ax.xaxis.set_major_locator(xmajorLocator)
    ax.xaxis.set_major_formatter(xmajorFormatter)
    ax.yaxis.set_major_locator(ymajorLocator)
    ax.yaxis.set_major_formatter(ymajorFormatter)

    # 修改次刻度
    xminorLocator = MultipleLocator(6)  # 将x轴次刻度标签设置为5的倍数
    yminorLocator = MultipleLocator(0.1)  # 将此y轴次刻度标签设置为0.1的倍数
    # 设置次刻度标签的位置,没有标签文本格式
    ax.xaxis.set_minor_locator(xminorLocator)
    ax.yaxis.set_minor_locator(yminorLocator)

    # 打开网格
    ax.xaxis.grid(True, which='major')  # x坐标轴的网格使用主刻度
    ax.yaxis.grid(True, which='minor')  # y坐标轴的网格使用次刻度
    plt.xlabel('Hour/h')
    plt.ylabel('Energy consumption data/Kwh')

    plt.rcParams['font.sans-serif'] = ['SimHei']  # 用来正常显示中文标签
if __name__=="__main__":
    data=loadDataSet('data\EnergyDataHourNew.csv')
    #plt.plot(range(len(data['Value'])),data['Value'])
    #plt.show()

    weather = loadDataSet('data\YP\YP-Real-time weather.csv')

    df=pd.merge(data, weather, how='inner',left_on='Time',right_on="Time" )
    df=df.drop('Time', axis=1)
    print(df)

    X = df.drop('Value', axis=1)
    y_test = df['Value']
    normalized_X=preprocessing.normalize(X)

    parameters = {'kernel': ('linear', 'rbf'), 'C': [1, 2, 4], 'gamma': [0.125, 0.25, 0.5, 1, 2, 4]}
    svr = svm.SVR()
    model = LinearRegression()
    #model = GridSearchCV(svr, parameters, n_jobs=-1)

    model.fit(normalized_X,y_test)

    y_pred=model.predict(normalized_X)
    a=len(y_test)
    b=len(df['Value'])
    print(y_pred)
    print(mean_squared_error(y_test, y_pred))

    print(r2_score(y_test, y_pred))

    plt.plot(range(len(y_test)), y_test, c='k', label='data', zorder=1)
    plt.hold('on')
    plt.plot(range(len(y_test)), y_pred, c='r',label='Predict')

    plt.xlabel('data')
    plt.ylabel('target')
    plt.title('Predict')
    plt.legend()

    plt.show()
