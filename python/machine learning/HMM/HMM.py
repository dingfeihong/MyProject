# -*- coding:utf-8 -*-
# Filename: viterbi.py
# Author：hankcs
# Date: 2014-05-13 下午8:51
import numpy
import pandas
import pdb
'''
#天气散步例子
states = ('Rainy', 'Sunny')
observations = ('walk', 'shop', 'clean')
start_probability = {'Rainy': 0.6, 'Sunny': 0.4} 
transition_probability = {
    'Rainy' : {'Rainy': 0.7, 'Sunny': 0.3},
    'Sunny' : {'Rainy': 0.4, 'Sunny': 0.6},
    } 
emission_probability = {
    'Rainy' : {'walk': 0.1, 'shop': 0.4, 'clean': 0.5},
    'Sunny' : {'walk': 0.6, 'shop': 0.3, 'clean': 0.1},
}

states = ('D4','D6','D8') 
observations = (1,6,3)
start_probability = {'D4':1/3,'D6':1/3,'D8':1/3} 
transition_probability = {
    'D4' : {'D4':1/3,'D6':1/3,'D8':1/3},
    'D6' : {'D4':1/3,'D6':1/3,'D8':1/3},
    'D8' : {'D4':1/3,'D6':1/3,'D8':1/3},
    } 
emission_probability = {
    'D4' : {1:1/4,2:1/4,3:1/4,4:1/4,5:0,6:0,7:0,8:0},
    'D6' : {1:1/6,2:1/6,3:1/6,4:1/6,5:1/6,6:1/6,7:0,8:0},
    'D8' : {1:1/8,2:1/8,3:1/8,4:1/8,5:1/8,6:1/8,7:1/8,8:1/8},
}

'''
#海藻天气例子

observations = ('Dry','Damp','Soggy','Soggy')
#observations = ['Dry'
states = ('Sunny','Cloudy','Rainy')

start_probability = {'Sunny':0.3,'Cloudy':0.3,'Rainy':0.4} 
transition_probability = {
    'Sunny' : {'Sunny':0.2,'Cloudy':0.3,'Rainy':0.5},
    'Cloudy' : {'Sunny':0.1,'Cloudy':0.5,'Rainy':0.4},
    'Rainy' : {'Sunny':0.6,'Cloudy':0.1,'Rainy':0.3},
    } 
emission_probability = {
    'Sunny' : {'Dry':0.1,'Damp':0.5,'Soggy':0.4},
    'Cloudy' : {'Dry':0.2,'Damp':0.4,'Soggy':0.4},
    'Rainy' : {'Dry':0.3,'Damp':0.6,'Soggy':0.1},
}
"""
:param obs:观测序列
:param states:隐状态
:param start_p:初始概率（隐状态）
:param trans_p:转移概率（隐状态）
:param m_emitProb: 发射概率 （隐状态表现为显状态的概率）
"""
class HMM:
    def __init__(self, states,start_probability, transition_probability, emission_probability):
        self.m_states = states
        self.m_startProb = start_probability
        self.m_transProb = transition_probability
        self.m_emitProb = emission_probability

    #打印路径概率表
    def print_dptable(self,V):
        print("    ",)
        for i in range(len(V)): print("%7d" % i,)
        print()
     
        for y in V[0].keys():
            print("%.5s: " % y,)
            for t in range(len(V)):
                print("%.7s" % ("%f" % V[t][y]),)
            print()
     
    # obs:观察值序列,alpha:前向概率 
    def Forward(self,obs):
        alpha = {state: self.m_startProb[state]*self.m_emitProb[state][obs[0]] for state in states} 
        for cur_obs in obs[1:]:
            #print(alpha)
            alpha = { next_state:sum( [ alpha[state]*self.m_transProb[state][next_state] for state in states])*self.m_emitProb[next_state][cur_obs] for next_state in states}
        print(alpha)
        return sum(alpha.values())

    # obs:观察值序列, beta:后向概率
    def Backword(self, obs):
        beta = {state: 1 for state in states}
        for cur_obs in obs[1:][::-1]:
            beta = {cur_state: sum([ beta[last_state] *self.m_transProb[cur_state][last_state]* self.m_emitProb[last_state][cur_obs]  for last_state in states]) for  cur_state in states}
        return sum(self.m_emitProb[state][obs[0]]*beta[state] * self.m_startProb[state] for state in states)

    def viterbi(self,obs):
        V = [{}]    # 路径概率表 V[时间][隐状态] = 概率
        path = {}     # 一个中间变量,代表当前状态是哪个隐状态

        # 初始化初始状态 (t == 0)
        for state in states:
            V[0][state] = self.m_startProb[state] * self.m_emitProb[state][obs[0]]
            path[state] = [state]

        # 对 t > 0 跑一遍维特比算法
        for t in range(1, len(obs)):
            V.append({})
            newpath = {}

            for cur_state in states:
                # 概率 隐状态 =  前状态是y0的概率 * y0转移到y的概率 * y表现为当前状态的概率
                #pdb.set_trace()
                (prob, state) = max(   [(V[t - 1][last_state] * self.m_transProb[last_state][cur_state] * self.m_emitProb[cur_state][obs[t]], last_state) for last_state in states]   )
                V[t][cur_state] = prob                # 记录路径
                newpath[cur_state] = path[state] + [cur_state]# 记录最大概率
            
            path = newpath# 不需要保留旧路径
     
        #self.print_dptable(V)
        (prob, state) = max([(V[len(obs) - 1][y], y) for y in states])
        return (prob, path[state])
     
     
tmp = HMM(states,start_probability,transition_probability,emission_probability)

print(tmp.Forward(observations))
print(tmp.Backword(observations))
print(tmp.viterbi(observations))