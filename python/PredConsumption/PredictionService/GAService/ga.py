# 0.0 coding:utf-8 0.0

import matplotlib.pyplot as plt
import math
import numpy as np
from calobjValue import calobjValue,getFitnessValue,x_origin,fitnessFunction
from calfitValue import calfitValue
from selection import selection
from crossover import crossover
from mutation import mutation
from best import best
from geneEncoding import geneEncoding,geneDecoding

import numpy as np
import pandas as pd
import random
import EnergyAnalysis.DataProcess
import timeit
import EnergyAnalysis.GeneralRegression
import datetime
def Save(df, fileName):
	with open(fileName, 'w+') as f:
		f.write(str(df))
def Ratio(list):
	max=np.max(list)
	mean = np.mean(list)
	#a=np.sum([(x-mean)*(x-mean) for x in list])/len(list)
	std = np.std(list)
	#var = np.var(list)
	return std/(max-mean)
pop_size = 0		# 种群数量
for num in range(10):
#	print(materNUM)
	g_starttime = datetime.datetime.now()
	iteration_size=700 #迭代次数
	pop_size=20
	pc = 0.72			# 交配概率
	pm = 0.03           # 变异概率
	#max_value = 10      # 基因中允许出现的最大值
	chrom_length = x_origin.shape[1]	# 染色体长度

	results = [[]]		# 存储每一代的最优解，N个二元组
	r2_list = []		# 个体适应度(r2)
	mse_list = []		# 个体适应度(mse)
	fit_mean = []		# 平均适应度
	ratio_list = []
	# pop = [[0, 1, 0, 1, 0, 1, 0, 1, 0, 1] for i in range(pop_size)]
	pop = np.array(geneEncoding(pop_size, chrom_length))

	for i in range(iteration_size):
		print('\niteration:',i)
		starttime = datetime.datetime.now()
		r2_list,mse_list = getFitnessValue(pop)        # 个体评价
		#fit_value = calfitValue(obj_value)      # 淘汰
		ratio=np.std(r2_list)
		ratio_list.append(ratio)
		best_gen, r2_fitness,mse_fitness= best(pop, r2_list,mse_list)		# 第一个存储最优的解, 第二个存储最优基因
		print('std:', ratio)
		print('r2_fitness',r2_fitness)
		print('mse_fitness',mse_fitness)
		results.append([r2_fitness,mse_fitness, best_gen])
		pop=selection(pop, r2_list)		# 新种群复制
		crossover(pop, pc)		# 交配
		mutation(pop, pm)       # 变异

		endtime = datetime.datetime.now()
		print('Time cost:', (endtime - starttime).seconds)

	results = results[1:]
	#results.sort()
	#print(results[-1])
	best_r2=[x[0] for x in results]
	best_mse=[x[1] for x in results]
	best_gens=[x[2] for x in results]
#	savelist = materNUM
	savelist='\n'+'best_r2:'+str(np.max(best_r2))
	savelist=savelist+'\nbest_mse:'+str(best_mse[np.argmax(best_r2)])
	savelist=savelist+'\nbest_gens:'+ str(best_gens[np.argmax(best_r2)])
	savelist=savelist+'\niteration_size:'+str(iteration_size)
	savelist=savelist+'\npop_size:'+ str(pop_size)
	savelist=savelist+'\npc:'+ str(pc)
	savelist=savelist+'\npm'+ str(pm)
	print('best_r2:',np.max(best_r2))
	print('best_mse:',best_mse[np.argmax(best_r2)])
	print('best_gens:',best_gens[np.argmax(best_r2)])
	print('iteration_size:',iteration_size)
	print('pop_size:',pop_size)
	print('pc:',pc)
	print('pm',pm)

	#print(fit_value[1])
	g_endtime = datetime.datetime.now()
	print('Totoal time cost:', (g_endtime - g_starttime).seconds)
	savelist =savelist+'\nTotoal time cost:'+ str((g_endtime - g_starttime).seconds)

	savelist = savelist + '\nratio_list\n' + str(ratio_list)
	#print(results)
	#print("y =",results[-1][0], "x =",results[-1][1])


	fig = plt.figure(figsize=(18, 12))

	# plt.plot(X, Y)
	# plt.plot(X, Y2)
	plt.xticks( fontsize=18)
	plt.yticks(fontsize=18)

	ax1 = fig.add_subplot(111)
	p1,=ax1.plot([i for i in range(len(best_r2))], best_r2,linewidth=3)
	ax1.set_ylabel('Y values for R2',fontsize=18)
	ax1.set_title("Fitness",fontsize=18)

	ax2 = ax1.twinx()  # this is the important function
	plt.xticks( fontsize=18)
	plt.yticks(fontsize=18)
	p2,=ax2.plot([i for i in range(len(best_r2))], best_mse, 'r',linestyle=':')
	#p2,=ax2.plot([i for i in range(len(best_r2))], ratio_list, 'r',linestyle=':')
	#ax2.set_ylim([np.min(best_mse),np.max(best_mse)])
	ax2.set_ylabel('Y values for ratio_list',fontsize=18)

	l1 = plt.legend([p1, p2], ["R-Suqare", "Mean Square Err"], loc='upper right')
	#Save(savelist,'/res/'+str(datetime.datetime.now().day)+'/'+str(num)+str(datetime.datetime.now().day)+'.txt')
	plt.legend([p1, p2], ["R-Suqare", "best_mse"],bbox_to_anchor=(0.8, 1.1), loc=2, borderaxespad=0., handleheight=1.675,fontsize=18)

	Save(best_r2, 'HKL/' + 'day' + str(datetime.datetime.now().day) + 'hour' + str(
		datetime.datetime.now().hour) + 'minute' + str(datetime.datetime.now().minute) + 'r2.txt')
	Save(best_mse, 'HKL/' + 'day' + str(datetime.datetime.now().day) + 'hour' + str(
		datetime.datetime.now().hour) + 'minute' + str(datetime.datetime.now().minute) + 'mse.txt')

	Save(savelist,'HKL/'+'day'+str(datetime.datetime.now().day)+'hour'+str(datetime.datetime.now().hour)+'minute'+str(datetime.datetime.now().minute)+ '.txt')
	plt.savefig('HKL/'+'day'+str(datetime.datetime.now().day)+'hour'+str(datetime.datetime.now().hour)+'minute'+str(datetime.datetime.now().minute)+ materNUM+'.jpg')
	#plt.show()