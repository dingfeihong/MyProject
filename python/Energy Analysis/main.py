 # -*- coding: utf-8 -*-
# filename: EnergyTset.py
import numpy as np
import pandas as pd
from sklearn.svm import SVR
from sklearn.cross_validation import train_test_split
from sklearn.linear_model import LinearRegression
from sklearn.model_selection import cross_val_predict
from sklearn import preprocessing
from sklearn.metrics import mean_squared_error
from sklearn.metrics import r2_score
from sklearn import metrics
import datetime
import matplotlib.pyplot as plt
import pdb
#打开文件

def loadDataSet(fileName):
    #读取数据
    table = pd.read_table(fileName,header = 0,sep=",")
    return table


if __name__=="__main__":
    data=loadDataSet('data\Data.txt')
    #print(data)

    weather = loadDataSet('data\WeatherData.txt')
    #print(weather)

    df=pd.merge(data, weather, how='inner',left_on='Time',right_on="Date" )
    df=df.drop('Date', axis=1)
    #print(type(df))
    print(df)

    X = df.iloc[:,2:]
    y = df['Value']
    normalized_X=preprocessing.normalize(X)

    #X_train, X_test, y_train, y_test = train_test_split(normalized_X, y,test_size=0.4, random_state=0)
    #model = SVR()
    model = LinearRegression()
    #clf.fit(X_train, y_train)
    #result = clf.predict(X)
    #print(result)
    model.fit(normalized_X,y)
    y_pred=model.predict(normalized_X)

    print(y_pred)
    print(mean_squared_error(y, y_pred))
    #plt.show()
    #print(type(delta))