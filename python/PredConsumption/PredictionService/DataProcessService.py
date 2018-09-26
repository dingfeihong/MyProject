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
def getDataFromDict(inp_dict):
    depID=inp_dict['DepConsumptionList'][0]['depID']
    valueList=inp_dict['DepConsumptionList'][0]['ValueList']
    rawdata = pd.DataFrame(valueList)
    lastYeardf = pd.DataFrame(inp_dict['DepConsumptionList'][0]['LastYearValueList'])
    rawdata['YL']=lastYeardf['Value']
    df=rawdata[30*24:].reset_index(drop=True)#切片获取数据

    #月历史能耗
    df = getHistoryDayValue(30, df, rawdata)

    #一周历史能耗
    for i in range(7):
        df = getHistoryDayValue(i+1, df, rawdata)

    #24小时历史能耗
    for i in range(24):
        df = getHistoryHourValue(i+1, df, rawdata, 1)

    df = df.drop('Date', axis=1)
    print(df)
    df=df.fillna(value=0)

    value = df.loc[:, ['Value']]
    feature = df.drop('Value', axis=1)
    return value, feature
def getDataFromList(datalist):
    data = pd.DataFrame(datalist)
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


# 电表数据，获取以天为单位位移的历史能耗
def getHistoryDayValue(offsetDay, data):
    offset = offsetDay*24
    data['DL' + str(offsetDay)] = data['Value']
    data['DL' + str(offsetDay)][offset:] =  data['DL' + str(offsetDay)] [:-offset]
    data['DL' + str(offsetDay)][:offset] = None
    return data

def getHistoryDayValue(offsetDay, data,historydata):
    offset = offsetDay*24
    size=data.shape[0]
    data['DL' + str(offsetDay)] =  historydata[-(size+offset):-offset].reset_index(drop=True)['Value']
    #print(data)
    return data

# 电表数据，获取以小时为单位位移的历史能耗
def getHistoryHourValue(offsetHour, data, daydisp=0):
    offset = offsetHour + daydisp*24
    data['HL' + str(offsetHour)] = data['Value']
    data['HL' + str(offsetHour)][offset:]=data['HL' + str(offsetHour)][:-offset]
    data['HL' + str(offsetHour)][:offset]=None
    return data

def getHistoryHourValue(offsetHour, data,historydata, daydisp=0):
    offset = offsetHour + daydisp*24
    size = data.shape[0]
    data['HL' + str(offsetHour)]=historydata[-size-offset:-offset].reset_index(drop=True)['Value']
    #print(data)
    return data


if __name__ == "__main__":
    getData()