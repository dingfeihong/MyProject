# 0.0 coding:utf-8 0.0
# 找出最优解和最优解的基因编码
import numpy as  np

def best(pop, r2_list,mse_list):
#     px = len(pop)
#     best_gen = pop[0]
#     best_fit = fit_value1[0]
#     best_fit2 = fit_value2[0]
#     for i in range(1, px):
#         if(fit_value1[i] > best_fit):
#             best_fit = fit_value1[i]
#             best_fit2 = fit_value2[i]
#             best_gen = pop[i]
    best_r2=np.max(r2_list)
    best_mse=mse_list[np.argmax(r2_list)]
    best_gen=pop[np.argmax(r2_list)]
    return [best_gen, best_r2,best_mse]

if __name__ == '__main__':
    pass
