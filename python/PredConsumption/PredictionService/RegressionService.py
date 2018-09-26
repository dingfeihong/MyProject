# -*- coding: utf-8 -*-
# filename: EnergyTset.py

from sklearn import ensemble
from sklearn.ensemble import RandomForestRegressor
from sklearn.linear_model import LinearRegression
from sklearn.linear_model import Ridge
import pandas as pd
from PredConsumption.PredictionService import FeatureSelectService
from PredConsumption.PredictionService import DataProcessService
from pylab import *
from matplotlib.ticker import  MultipleLocator
from matplotlib.ticker import  FormatStrFormatter
import matplotlib.pyplot as plt
from sklearn.model_selection import train_test_split
import datetime
from sklearn.metrics import mean_squared_error, r2_score

# 打开文件
def FigurePlot():
    ax = plt.subplot(111)
    plt.xticks(fontsize=20, rotation=90)
    plt.yticks(fontsize=20)
    xmajorLocator = MultipleLocator(24)  # 将x主刻度标签设置倍数
    xmajorFormatter = FormatStrFormatter('%5.1f')  # 设置x轴标签文本的格式
    ymajorLocator = MultipleLocator(5)  # 将y轴主刻度标签设置倍数
    ymajorFormatter = FormatStrFormatter('%1.1f')  # 设置y轴标签文本的格式
    # 设置主刻度标签的位置,标签文本的格式
    ax.xaxis.set_major_locator(xmajorLocator)
    ax.xaxis.set_major_formatter(xmajorFormatter)
    ax.yaxis.set_major_locator(ymajorLocator)
    ax.yaxis.set_major_formatter(ymajorFormatter)

    # 修改次刻度
    xminorLocator = MultipleLocator(8)  # 将x轴次刻度标签设置倍数
    yminorLocator = MultipleLocator(1)  # 将此y轴次刻度标签设置倍数
    # 设置次刻度标签的位置,没有标签文本格式
    ax.xaxis.set_minor_locator(xminorLocator)
    ax.yaxis.set_minor_locator(yminorLocator)

    # 打开网格
    ax.xaxis.grid(True, which='major')  # x坐标轴的网格使用主刻度
    ax.yaxis.grid(True, which='minor')  # y坐标轴的网格使用次刻度
    plt.legend()
    plt.rcParams['font.sans-serif'] = ['SimHei']  # 用来正常显示中文标签


def Evaluate(testSet, predSet, plotSwitch=True):
    mse = mean_squared_error(testSet, predSet)
    r2 = r2_score(testSet, predSet)
    if (plotSwitch):
        print("mean squared error:", mse)
        print("R2:", r2, '\n')
    return mse, r2


def getModel():
    modelDict = {}

    modelLR = LinearRegression()

    modelRidge = Ridge(alpha=.5)

    modelRF = RandomForestRegressor(n_estimators=500, random_state=1)
    #
    # modelKeras = Sequential()

    modelAdaBoost = ensemble.AdaBoostRegressor(n_estimators=100)  # 这里使用50个决策树
    modelGradientBoost = ensemble.GradientBoostingRegressor(n_estimators=100)  # 这里使用100个决策树
    modelBagging = ensemble.BaggingRegressor()

    # # Score on the training set was:-14.596930317950676
    # modelExtraTree = make_pipeline(
    #     Normalizer(norm="l1"),
    #     ZeroCount(),
    #     StackingEstimator(
    #         estimator=ExtraTreesRegressor(bootstrap=True, max_features=0.35000000000000003, min_samples_leaf=17,
    #                                       min_samples_split=13, n_estimators=100)),
    #     ExtraTreesRegressor(bootstrap=False, max_features=0.6000000000000001, min_samples_leaf=1, min_samples_split=4,
    #                         n_estimators=100)
    # )

    # modelDict['SVR'] = svr
    modelDict['LR'] = modelLR
    modelDict['Ridge'] = modelRidge
    modelDict['RandomForest'] = modelRF
    modelDict['AdaBoost'] = modelAdaBoost
    modelDict['GradientBoost'] = modelGradientBoost
    modelDict['Bagging'] = modelBagging
    #modelDict['ExtraTree'] = modelExtraTree
    # modelDict['Keras'] = modelKeras
    return modelDict


def modelsSelect(x_origin, y_origin):
    X_train, X_test, y_train, y_test = train_test_split(x_origin, y_origin, random_state=42)
    result = []
    modelDict = getModel()

    y_add = np.array(y_test - y_test).T[0]
    print(y_add.shape)
    count = 0

    for modelName, model in modelDict.items():
        starttime = datetime.datetime.now()
        # if(modelName=='RandomForest'):
        #     model.fit(X_train, np.array(y_train).T.ravel())
        #     y_tmp = model.predict(X_test)
        #     print(modelName)
        #     Evaluate(np.array(y_test), y_tmp)
        #     y_add = y_add + y_tmp
        if modelName == 'RandomForest' or modelName == 'AdaBoost':
            model.fit(X_train, y_train.values.ravel())
            y_tmp = model.predict(X_test)
            print(modelName)

            y_add = y_add + y_tmp
        # elif modelName == 'Keras':
        #     model.add(Dense(units=30, activation='relu', init='normal', input_dim=X_train.shape[1]))
        #
        #     model.add(Dense(units=30))
        #     model.add(Dense(units=1))
        #     model.compile(loss='mse', optimizer=keras.optimizers.SGD(lr=0.00001, momentum=0.9, nesterov=True),
        #                   metrics=['accuracy'])
        #
        #     model.fit(X_train, y_train)
        #     y_tmp = model.predict(X_test)
        #     print('\n', modelName)
        #     # Evaluate(np.array(y_test), y_tmp)
        #     y_add = y_add + y_tmp.T[0]
        else:
            model.fit(X_train, y_train)
            y_tmp = model.predict(X_test)
            print(modelName)
            # Evaluate(np.array(y_test), y_tmp)
            y_add = y_add + y_tmp.T[0]

        endtime = datetime.datetime.now()
        print('Time cost:', (endtime - starttime).microseconds / 1000000)
        mse, r2 = Evaluate(np.array(y_test), y_tmp)
        result.append([modelName, mse, r2])
        count = count + 1

    y_pred = y_add / count
    # print("final")
    # Evaluate(y_test, y_pred)
    result = pd.DataFrame(result, columns=['name', 'mse', 'r2'])
    print(result)
    # LearningPlot(result)
    return y_pred
    # plt.plot(range(len(y_test)), y_test, c='k', label='data', zorder=1)
    # plt.plot(range(len(y_test)), y_pred, c='r', label='Predict')
    # FigurePlot()
    # plt.show()


def RegressionAdapt(x_Data, y_Data, modelName, plotSwitch=True):

    X_train, X_test, y_train, y_test = train_test_split(x_Data, y_Data, random_state=1)
    #    X_train, X_test=x_Data
    #   y_train, y_test=y_Data
    model = getModel()[modelName]
    model.fit(X_train, y_train)
    t1=time.time()
    if (plotSwitch == True):
        y_pred = model.predict(x_Data)
        t2= time.time()
        dt=t2-t1
        print(t2)
        print(t1)
        print(dt)
        mse, r2 = Evaluate(y_Data, y_pred, plotSwitch)
        RegressionAdapt = plt.figure(modelName)
        # plt.title(modelName)
        plt.plot(range(len(y_Data)), y_Data, c='k', label='data', zorder=1)
        plt.plot(range(len(y_Data)), y_pred, c='r', label='Predict',linestyle=':')

        FigurePlot()

        plt.xlabel('Hour/h', fontsize=16)
        plt.ylabel('Energy consumption data/Kwh', fontsize=16)
        plt.title('Energy Consumption Data Predict', fontsize=16)
        plt.show()
    else:
        y_pred = model.predict(X_test)
        mse, r2 = Evaluate(y_test, y_pred, plotSwitch)
        #RegressionAdapt = plt.figure(modelName)
    return mse, r2


# 模型拟合结果
def dataPlot(data):
    RegressionAdapt = plt.figure('Energy consumption data')
    plt.title('Energy consumption data',fontsize=20)

    plt.plot(range(len(data)), data, c='r', label='data', zorder=1)
    FigurePlot()
    # plt.show()


def LearningPlot(result, title=None):
    if (title == None): title = 'LearningPlot'
    names = result['name']
    mse = result['mse']
    r2 = result['r2']

    learningPlot = plt.figure(title)

    size = len(result)

    totalWidth = 2.6
    width = 1.2
    ticks = [i * 3 for i in range(size)]

    # subplot1 = plt.subplot(2, 1, 1)

    plt.ylim(ymax=100, ymin=0)
    plt.title(title)

    # x = x - (total_width - width) / 2
    bar1 = plt.bar([i * totalWidth for i in range(size)], mse, width=width, label='mean square error')
    bar2 = plt.bar([i * totalWidth + width for i in range(size)], r2 * 20, width=width, label='R2')
    plt.xticks([i * totalWidth + width / 2 for i in range(size)], names,fontsize=16)
    plt.yticks(fontsize=20)
    exp = lambda x: x - 0.05 if (x < 0) else x
    for rect in bar1:
        h = rect.get_height()
        plt.text(rect.get_x() + rect.get_width() / 2, exp(h), '%0.2f' % (h), ha='center', va='bottom', size=16)
    for rect in bar2:
        h = rect.get_height()
        plt.text(rect.get_x() + rect.get_width() / 2, exp(h), '%0.4f' % (h / 20), ha='center', va='bottom', size=16)
    learningPlot.legend()


def getIntialPopulation(encodelength, populationSize):
    # 随机化初始种群为0
    chromosomes = np.zeros((populationSize, sum(encodelength)), dtype=np.uint8)
    for i in range(populationSize):
        chromosomes[i, :] = np.random.randint(0, 2, sum(encodelength))
        # print('chromosomes shape:', chromosomes.shape)
    return chromosomes


#
# def fitnessFunction(X=None):
#     y_origin, x_origin = DataProcess.getAllFeatures('0020')
#     names=x_origin.columns.values
#     X=getIntialPopulation(x_origin.shape[1],1)
#     newX=x_origin[names[X[0]==1]]
#     return -RegressionAdapt(newX, y_origin, 'RandomForest', plotSwitch=False)
def predictTest(datalist):
    data=pd.DataFrame(datalist)
    print(data)
    y_origin, x_origin = DataProcessService.getDataFromList(data)
    # 筛选特征
    # newDataX = VarianceThreshold(threshold=30).fit_transform(x_origin)
    #pearsonrSelectX = FeatureSelectService.pearsonrFeatureSelect(y_origin, x_origin, plotSwitch=False)
    #print(pearsonrSelectX.shape)
    result = modelsSelect(x_origin, y_origin)
    #LearningPlot(result, title='Feature selection2')

    #RegressionAdapt(pearsonrSelectX, y_origin, 'GradientBoost')
    return result

if __name__ == "__main__":
    # x_origin, y_origin = getData()
    datalen = 1000

    # 未筛选特征
    y_origin, x_origin = DataProcessService.getData()

    #dataPlot(y_origin[:24 * 7])
    print(y_origin[:24 * 7])
    plt.show()
    #result = modelsSelect(x_origin, y_origin)
    #LearningPlot(result, title='Before feature selection')

    # 筛选特征
    # newDataX = VarianceThreshold(threshold=30).fit_transform(x_origin)
    pearsonrSelectX = FeatureSelectService.pearsonrFeatureSelect(y_origin, x_origin, plotSwitch=False)
    print(pearsonrSelectX.shape)
    result = modelsSelect(pearsonrSelectX, y_origin)
    LearningPlot(result, title='Feature selection2')

    # 递归特征消除法，返回特征选择后的数据
    # 参数estimator为基模型
    # 参数n_features_to_select为选择的特征个数
    # train_pca_X=PCA(n_components= 50).fit_transform(pearsonrSelectX)#拟合度降低了




    # train_ref_X=RFE(estimator=getModel()['GradientBoost'], n_features_to_select=30).fit_transform(pearsonrSelectX, y_origin)
    # #print(newDataX.shape)
    # result=modelsSelect(train_ref_X, y_origin)
    # LearningPlot(result,title='Feature selection3')

    RegressionAdapt(pearsonrSelectX, y_origin, 'GradientBoost')
