 # -*- coding: utf-8 -*-
# filename: EnergyTset.py
from numpy import *
import pandas as pd
import time
from sklearn.svm import SVR
import datetime
import matplotlib.pyplot as plt
import pdb
#打开文件

def loadDataSet(fileName):
    #读取数据
    table = pd.read_table(fileName,header = 0,sep=",")
    return table

def SaveDataSet(df,fileName):
    print(df)
    with open(fileName, 'w+') as f:
        df.to_csv(f, header=True, index=False)
def DataProcessDay(data):
    #删除无用列
    data = data.drop('AH_AnalogNo', axis=1)
    data["AH_Time"] = data["AH_Time"].apply(lambda x: x[0:18])
    # 将时间设置为索引
    data['AH_Time'] = pd.to_datetime(data['AH_Time'])
    # table = table.set_index('AH_Time')
    # table['week'] = pd.to_datetime(table['AH_Time']).dt.weekday
    # print(table)
    lastValue=data.iloc[0][1]
    startTime=data.iloc[0][0]
    endTime=data.tail(1).iloc[0][0]
    #print(pd.to_datetime(endTime))
    currentTime=startTime
    delta = datetime.timedelta(days=1)
    df=pd.DataFrame()

    #计算日能耗
    while(currentTime<endTime):
        currentTime=currentTime+delta
        currentMaxValue=data[data['AH_Time']<currentTime].max(0).iloc[1]
        s = pd.Series([str(currentTime)[:10]+' 00:00:00',currentMaxValue-lastValue],index=['Time','Value'])
        print(s)
        df = df.append(s, ignore_index=True)
        lastValue=currentMaxValue
    #df.set_index(['Time','Value'])
    df['Week'] = pd.to_datetime(df['Time']).dt.weekday
    return df

def DataProcessHour(data):
    #删除无用列
    data = data.drop('AH_AnalogNo', axis=1)
    data["AH_Time"] = data["AH_Time"].apply(lambda x: x[0:18])
    # 将时间设置为索引
    data['AH_Time'] = pd.to_datetime(data['AH_Time'])
    # table = table.set_index('AH_Time')
    # table['week'] = pd.to_datetime(table['AH_Time']).dt.weekday
    # print(table)
    lastValue=data.iloc[0][1]
    startTime=data.iloc[0][0]
    endTime=data.tail(1).iloc[0][0]
    #print(pd.to_datetime(endTime))
    currentTime=startTime
    delta = datetime.timedelta(hours=1)
    df=pd.DataFrame()

    #计算日能耗
    while(currentTime<endTime):
        currentTime=currentTime+delta
        currentMaxValue=data[data['AH_Time']<currentTime].max(0).iloc[1]
        s = pd.Series([str(currentTime)[:13]+':00:00',currentMaxValue-lastValue],index=['Time','Value'])
        print(s)
        df = df.append(s, ignore_index=True)
        lastValue=currentMaxValue
    #df.set_index(['Time','Value'])
    df['Week'] = pd.to_datetime(df['Time']).dt.weekday
    return df
if __name__=="__main__":
    rawData=loadDataSet('data\GuanghuaSelect.txt')
    SaveDataSet(DataProcessHour(rawData),"data\EnergyDataHour.csv",)