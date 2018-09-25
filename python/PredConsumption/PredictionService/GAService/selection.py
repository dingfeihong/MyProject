# 0.0 coding:utf-8 0.0
# 选择

import random
import numpy as np

def sum(fit_value):
	total = 0
	for i in range(len(fit_value)):
		total += fit_value[i]
	return total


def cumsum(fit_value):
	for i in range(len(fit_value)-2, -1, -1):
		t = 0
		j = 0
		while(j <= i):
			t += fit_value[j]
			j += 1
		fit_value[i] = t
		fit_value[len(fit_value)-1] = 1


def selection(pop, fit_value):
	newfit_value = []
	min=np.min(fit_value)
	fit_prop=[x-min for x in fit_value]
	#fit_value[np.argmax(fit_value)]=np.max(fit_value)*1.5
	# 适应度总和
	total_fit = sum(fit_prop)
	for i in range(len(fit_prop)):
		newfit_value.append(fit_prop[i] / total_fit)
	# 计算累计概率
	cumsum(newfit_value)
	ms = []
	pop_len = len(pop)
	for i in range(pop_len):
		ms.append(random.random())
	ms.sort()
	fitin = 0
	newin = 0
	newpop = pop
	# 转轮盘选择法
	while newin < pop_len:
		if(ms[newin] < newfit_value[fitin]):
			newpop[newin] = pop[fitin]
			newin = newin + 1
		else:
			fitin = fitin + 1
	pop = newpop
	return pop

if __name__ == '__main__':
    pass
