# -*- coding: cp936 -*-
from numpy import *



def forward_viterbi(obs, states, start_p, trans_p, emit_p):
   T = {}
   for state in states:
       ##          prob.           V. path  V. prob.
       T[state] = (start_p[state], [state], start_p[state]) 
       
   #对于每一个显性状态序列
   for output in obs:
       U = {}
       #当前每个隐形状态可能隐形状态可能
       for next_state in states:
           total = 0
           argmax = None
           valmax = 0
           for source_state in states:
               (prob, v_path, v_prob) = T[source_state]
               
               #当前状态为source_state  下个状态为next_state
               p = emit_p[source_state][output] * trans_p[source_state][next_state]
               prob *= p
               v_prob *= p
               total += prob
               if v_prob > valmax:
                   argmax = v_path + [next_state]
                   valmax = v_prob
           U[next_state] = (total, argmax, valmax)
       T = U
       
   ## apply sum/max to the final states:
   total = 0
   argmax = None
   valmax = 0
   for state in states:
       (prob, v_path, v_prob) = T[state]
       total += prob
       if v_prob > valmax:
           argmax = v_path
           valmax = v_prob
   return (total, argmax, valmax)
   

states=('Rainy','Sunny')#元组
obs=('walk','shop','clean')#元组
start_p={'Rainy':0.6,'Sunny':0.4}

trans_p={
    'Rainy':{'Rainy':0.7,'Sunny':0.3},
    'Sunny':{'Rainy':0.4,'Sunny':0.6}
    }
emit_p={
    'Rainy':{'walk':0.1,'shop':0.4,'clean':0.5},
    'Sunny':{'walk':0.6,'shop':0.3,'clean':0.1}
    }
    
observation=('walk','clean','walk')


#c=forward_viterbi(obs,states,start_p,transition_probility,emit_p)
#print c
