import random
import pdb

col=12
k1=0
maxDepth=17


class Node:
    def __init__(self, value=-1,Left=None,Right=None,k=0,n=0):
        self.Left=Left#右树
        self.Right=Right#左树
        self.value=value#数据值
        self.k=k#层数
        self.n=n


def CreateGrid(k,n):
    #pdb.set_trace()
    node=Node()
    print("当前第",k," ",n)
    if k>=maxDepth:
        
        node = None
        
    else:
        #0-12随机int值
        q = random.randrange(0,col+1,1)
        node.k=k
        node.value=q
        node.n=n
        node.Left = CreateGrid(k+1,2*n+1)
        node.Right = CreateGrid(k+1,2*n+2)
    return node
        
a=CreateGrid(k1,0)