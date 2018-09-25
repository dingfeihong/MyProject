from __future__ import print_function
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd
from PredConsumption.PredictionService import DataProcessService
from PredConsumption.PredictionService import RegressionService
from sklearn.cross_validation import cross_val_score, ShuffleSplit
from sklearn import preprocessing
from sklearn.cross_validation import train_test_split
from sklearn.metrics import mean_squared_error,r2_score
from sklearn.feature_selection import VarianceThreshold
from sklearn.feature_selection import SelectKBest
from sklearn.feature_selection import chi2
from statsmodels.graphics.tsaplots import plot_pacf,plot_acf
from scipy.stats import pearsonr
import statsmodels as sm
from pylab import *
#主要根据一个神经网络来讲解基础的tensorflow的应用。在实现一个神经网络的过程，来讲解tensorflow的应用。具体讲了每个函数的作用和意义。
#还有就是讲解了dropout的作用，非线性激励函数，损失函数，还有优化算法。

#相关性评估
def AutocoorelationEvaluate(ts):

    plt.title('Autocoorelation Evaluate')
    rcParams['figure.figsize'] = 10, 5
    plt.title(u'test')
    plt.plot(ts)
    plot_acf(ts)
    show()
def ADFEvaluate(ts):
    temp = np.array(ts)
    t = sm.tsa.stattools.adfuller(temp)  # ADF检验
    output = pd.DataFrame(
        index=['Test Statistic Value', "p-value", "Lags Used", "Number of Observations Used", "Critical Value(1%)",
               "Critical Value(5%)", "Critical Value(10%)"], columns=['value'])
    output['value']['Test Statistic Value'] = t[0]
    output['value']['p-value'] = t[1]
    output['value']['Lags Used'] = t[2]
    output['value']['Number of Observations Used'] = t[3]
    output['value']['Critical Value(1%)'] = t[4]['1%']
    output['value']['Critical Value(5%)'] = t[4]['5%']
    output['value']['Critical Value(10%)'] = t[4]['10%']
    print(output)
def pearsonrPlot(names, correlation, pValue):
    pearsonrFig = plt.figure('Pearsonr')
    size=len(names)
    plotSize1 = int(len(pValue) / 2)  # 图一总长
    plotSize2 = len(pValue) - plotSize1  # 图二总长

    totalWidth = 2.6
    width = 1.2
    ticks = [i * 3 for i in range(size)]

    subplot1 = plt.subplot(2, 1, 1)

    plt.title('Pearsonr and P-Value I')
    # x = x - (total_width - width) / 2

    plt.ylim(ymax=1, ymin=-0.5)
    bar1 = plt.bar([i * totalWidth for i in range(plotSize1)], correlation[:plotSize1], width=width,label='Correlation coefficient')
    bar2 = plt.bar([i * totalWidth + width for i in range(plotSize1)], pValue[:plotSize1], width=width, label='p-value')
    plt.xticks([i * totalWidth + width / 2 for i in range(plotSize1)], names[:plotSize1], rotation=90)

    exp = lambda x: x - 0.05 if (x < 0) else x
    for rect in bar1 + bar2:
        h = rect.get_height()
        subplot1.text(rect.get_x() + rect.get_width() / 2, exp(h), '%0.2f' % (h), ha='center',va='bottom', size=6)

    subplot2 = plt.subplot(2, 1, 2)

    plt.title('Pearsonr and P-Value II')

    plt.ylim(ymax=1, ymin=-0.5)
    bar1 = plt.bar([i * totalWidth for i in range(plotSize2)], correlation[plotSize1:], width=width,label='Correlation coefficient')
    bar2 = plt.bar([i * totalWidth + width for i in range(plotSize2)], pValue[plotSize1:], width=width, label='p-value')
    plt.xticks([i * totalWidth + width / 2 for i in range(plotSize2)], names[plotSize1:], rotation=90)

    for rect in bar1 + bar2:
        h = rect.get_height()
        subplot2.text(rect.get_x() + rect.get_width() / 2, exp(h), '%0.2f' % (h), ha='center',va='bottom', size=6)

    #pearsonrFig.legend()

    #pearsonrFig.show()
def PearsonrTest(data,disp,size=49):
    names=[]
    correlation=[]
    pValue=[]
    for i in range(size):
        if(i+1<10):
            index='000'+str(i+1)
        else:
            index='00'+str(i+1)

        if isinstance(disp,int):
            if disp-1==i:continue
            coefficient = pearsonr(data[i], data[disp-1])
        else:
            if disp  == index: continue
            coefficient=pearsonr(data[index], data[disp])
            names.append(index)
        correlation.append(coefficient[0])
        pValue.append(coefficient[1])
    pearsonrPlot(names, correlation, pValue)

def pearsonrFeatureSelect(value, feature,threshold=0.05,plotSwitch=True,skip=True):
    names=[]
    correlation=[]
    pValue=[]
    for i in feature.columns.values:
        coefficient = pearsonr(feature[i], value['Value'])
        if coefficient[1]>threshold and skip:
            continue
        names.append(i)
        correlation.append(coefficient[0])
        pValue.append(coefficient[1])
    if(plotSwitch==True):
        pearsonrPlot(names, correlation, pValue)
    return feature[names]

def ImportanceRank(x_Data, y_Data):
    importanceRankFig = plt.figure('ImportanceRank')
    plt.title('Importance Rank')
    model = RegressionService.RandomForestRegressor(n_estimators=500, random_state=1)
    model.fit(x_Data, y_Data)
    print(model.feature_importances_)
    names=[]
    for i in x_Data.columns.values:
        if isinstance(i, int):
            if (i + 1 < 10):
                index = '000' + str(i + 1)
            else:
                index='00'+str(i+1)
        else:index=i
        names.append(index)
    #names = x_Data.columns.values[0:]

    ticks = [i for i in range(len(names))]
    exp = lambda x: x - 0.05 if (x < 0) else x


    importanceRankBar = plt.bar(ticks, model.feature_importances_)
    plt.xticks(ticks, names, rotation=90)
    for rect in importanceRankBar:
        h = rect.get_height()
        plt.text(rect.get_x() + rect.get_width() / 2, exp(h), '%0.2f' % (h), ha='center', va='bottom', size=6)
def scoresEvaluation(value,feature):
    names = []
    scores = []
    rf = RegressionService.RandomForestRegressor(n_estimators=20, max_depth=4)
    for index in feature.columns.values:
        score = cross_val_score(rf, feature.loc[:,[index]], value, scoring="r2", cv=ShuffleSplit(len(feature), 3, .3))
        scores.append(round(np.mean(score), 3))
        names.append(index)
    print(sorted(scores, reverse=True))
    scoresPlot = plt.figure('scores')
    plt.title('R2 Scores')
    size = len(scores)

    totalWidth = 2.6
    width = 1.2
    bar = plt.bar([i * totalWidth + width for i in range(size)], scores, width=width, label='R2')
    plt.xticks([i * totalWidth + width / 2 for i in range(size)], names, rotation=90)

    exp = lambda x: x - 0.05 if (x < 0) else x
    for rect in bar:
        h = rect.get_height()
        plt.text(rect.get_x() + rect.get_width() / 2, exp(h), '%0.2f' % (h), ha='center', va='bottom', size=6)
    #return feature[names]

if __name__=="__main__":
    # Make up some real data
    dispStr='0020'
    disp=20
    num=2000
    x_data_origin, y_data_origin = DataProcessService.getData()

    value,feature=DataProcessService.getAllFeatures(dispStr)

    #data=pd.DataFrame(preprocessing.StandardScaler().fit_transform(metersData))

    #特征选择
    #X_new = SelectKBest(chi2, k=10).fit_transform(metersData.drop(dispStr, axis=1),metersData[dispStr])

    #newDataX=VarianceThreshold(threshold=3).fit_transform(data)
    #pearsonrFeatureSelect(value, feature)
    scoresEvaluation(value, feature)
    #ImportanceRank(feature, value)

    plt.show()
    #
    #
    # data = DataProcess.loadDataSet('data\MetersDataHourNew.csv')
    #
    # data['Time'] = pd.to_datetime(data['Time'])
    # ts=y_data_origin['Value'][0:1000]
    #
    # AutocoorelationEvaluate(ts)
    # ADFEvaluate(ts)