# 0.0 coding:utf-8 0.0
# 解码并计算值

import math

from EnergyAnalysis import FeatureEvaluation
from EnergyAnalysis import GeneralRegression
from PredConsumption.PredictionService import DataProcessService

#materNUM='0020'
y_origin, x_origin = DataProcessService.getData()

x_origin = FeatureEvaluation.pearsonrFeatureSelect(y_origin, x_origin, plotSwitch=False)
def decodechrom(pop, chrom_length):
    temp = []
    for i in range(len(pop)):
        t = 0
        for j in range(chrom_length):
            t += pop[i][j] * (math.pow(2, j))
        temp.append(t)
    return temp


def calobjValue(pop, chrom_length, max_value):
    temp1 = []
    obj_value = []
    temp1 = decodechrom(pop, chrom_length)
    for i in range(len(temp1)):
        x = temp1[i] * max_value / (math.pow(2, chrom_length) - 1)
        obj_value.append(10 * math.sin(5 * x) + 7 * math.cos(4 * x))
    return obj_value

def getFitnessValue(pop):
    # 得到种群规模和决策变量的个数
    chromosomesdecoded=pop
    population, nums = chromosomesdecoded.shape
    # 初始化种群的适应度值为0
    r2_list = []
    mse_list = []
    # 计算适应度值
    for i in range(population):
        r2,mse = fitnessFunction(chromosomesdecoded[i])
        # 计算每个染色体被选择的概率
        r2_list.append(r2)
        mse_list.append(mse)
    return r2_list,mse_list

def fitnessFunction(X):

    # long running

    names=x_origin.columns.values
    newX=x_origin[names[X==1]]
    mse,r2=GeneralRegression.RegressionAdapt(newX, y_origin, 'GradientBoost', plotSwitch=False)
    return r2,mse
if __name__ == '__main__':
    pass
