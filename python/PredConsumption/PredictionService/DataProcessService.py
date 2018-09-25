# -*- coding: utf-8 -*-
# filename: EnergyTset.py
from numpy import *

from sklearn import preprocessing
import pandas as pd
import time
from sklearn.svm import SVR
import datetime
import matplotlib.pyplot as plt


# 打开文件
def loadDataSet(fileName):
    # 读取数据
    table = pd.read_table(fileName, header=0, sep=",")
    return table

def getDataFromList(data):

    #时间格式处理
    data["Time"] = data["Time"].apply(lambda x: str(x))
    #data = data.drop('LastTestTime', axis=1)
    #时间类型处理

    data['Time'] = pd.to_datetime(data['Time'],format='%Y/%m/%d %H:%M:%S')
    #data = data.set_index('Time')
    #重命名列
  #  data=data.rename(index=str,columns={'Consumption':'Value'})
    print(data)
    for i in range(6):
        data = getHistoryHourValue(i + 1, data,24)

    '''
    for i in range(7):
        data = getHistoryValue(i + 1, data)
        '''
    data=data.dropna(how='any')
    data = data.reset_index(drop=True)

    data = data.drop('Time', axis=1)
    print(data)
    value=data.loc[:,['Value']]
    feature = data.drop('Value', axis=1)
    return value,feature

# 获取不包含天气参数的电表数据
def getData():
    data = loadDataSet('F:\document\project\python\PredConsumption\data\department_hour_collection.csv')
    #删除不需要的字段
    data = data.drop('ID', axis=1)
    data = data.drop('DepID', axis=1)
    data = data.drop('DepCode', axis=1)
    data = data.drop('EnterpriseID', axis=1)
    #时间格式处理
    data["Time"] = data["LastTestTime"].apply(lambda x: str(x)[:-2] + '00:00')
    data = data.drop('LastTestTime', axis=1)
    #时间类型处理
    data['Time'] = pd.to_datetime(data['Time'],format='%Y/%m/%d %H:%M:%S')
    #data = data.set_index('Time')
    #重命名列
    data=data.rename(index=str,columns={'Consumption':'Value'})
    print(data)

    for i in range(6):
        data = getHistoryHourValue(i + 1, data,24)
    '''
    for i in range(7):
        data = getHistoryValue(i + 1, data)
    
    data=data.dropna(how='any')
    data = data.reset_index(drop=True)
    '''
    data = data.drop('Time', axis=1)
    print(data)
    value=data.loc[:,['Value']]
    feature = data.drop('Value', axis=1)
    return value,feature


# 天气数据补全
def nullOperation(weather):
    return weather.interpolate()


# 电表数据，获取历史能耗
def getHistoryValue(offsetDay, data):
    offset = offsetDay*24
    data['DL' + str(offsetDay)] = data['Value']
    data['DL' + str(offsetDay)][offset:] =  data['DL' + str(offsetDay)] [:-offset]
    data['DL' + str(offsetDay)][:offset] = None
    return data


# 电表数据，获取历史小时能耗
def getHistoryHourValue(offsetHour, data, hourdisp=0):
    offset = offsetHour + hourdisp
    data['HL' + str(offsetHour)] = data['Value']
    data['HL' + str(offsetHour)][offset:]=data['HL' + str(offsetHour)][:-offset]
    data['HL' + str(offsetHour)][:offset]=None
    return data


if __name__ == "__main__":
    getData()