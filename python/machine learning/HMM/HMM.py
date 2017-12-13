# -*- coding:utf-8 -*-
# Filename: viterbi.py
# Author：hankcs
# Date: 2014-05-13 下午8:51
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
states = ('Sunny','Cloudy','Rainy') 
observations = ('Dry','Damp','Soggy')
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


obs = observations
start_p = start_probability
trans_p=transition_probability
emit_p=emission_probability

# 打印路径概率表
def print_dptable(V):
    print("    ",)
    for i in range(len(V)): print("%7d" % i,)
    print()
 
    for y in V[0].keys():
        print("%.5s: " % y,)
        for t in range(len(V)):
            print("%.7s" % ("%f" % V[t][y]),)
        print()
 
    # obs:观察值序列    
    # alpha:前向概率 prob:返回值,所要求的概率
def Forward(obs,prob):
    alpha = {state: start_probability[state]*emit_p[state][obs[0]] for state in states} 
    for cur_obs in obs[1:]:
        print(alpha)
        alpha = { next_state:sum( [ alpha[state]*trans_p[state][next_state] for state in states])*emit_p[next_state][cur_obs] for next_state in states}
    print(alpha)
    return sum(alpha.values())
def viterbi(obs, states, start_p, trans_p, emit_p):
    """
 
    :param obs:观测序列
    :param states:隐状态
    :param start_p:初始概率（隐状态）
    :param trans_p:转移概率（隐状态）
    :param emit_p: 发射概率 （隐状态表现为显状态的概率）
    :return:
    """
    # 路径概率表 V[时间][隐状态] = 概率
    V = [{}]
    # 一个中间变量,代表当前状态是哪个隐状态
    path = {}
 
    # 初始化初始状态 (t == 0)
    for y in states:
        V[0][y] = start_p[y] * emit_p[y][obs[0]]
        path[y] = [y]
 
    # 对 t > 0 跑一遍维特比算法
    for t in range(1, len(obs)):
        V.append({})
        newpath = {}
 
        for cur_state in states:
            # 概率 隐状态 =  前状态是y0的概率 * y0转移到y的概率 * y表现为当前状态的概率
            (prob, state) = max([(V[t - 1][last_state] * trans_p[last_state][cur_state] * emit_p[cur_state][obs[t]], last_state) for last_state in states])
            # 记录最大概率
            V[t][y] = prob
            # 记录路径
            newpath[y] = path[state] + [y]
 
        # 不需要保留旧路径
        path = newpath
 
    print_dptable(V)
    (prob, state) = max([(V[len(obs) - 1][y], y) for y in states])
    return (prob, path[state])
 
 
def example():
    return viterbi(observations,
                   states,
                   start_probability,
                   transition_probability,
                   emission_probability)
 
 
print(example())