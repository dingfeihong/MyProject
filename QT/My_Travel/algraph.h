#ifndef TRAVEL
#define TRAVEL

#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include <conio.h>
#include <QTextCodec>

#define MAX 80             //最大顶点数
#define INF 2147483647
typedef char ElemType;
typedef int InfoType;
#define DATA "data.dat"
#define P_DATA "path_data.dat"
typedef struct
{

   QString name;
   InfoType Distance;          //额外的信息(边权值)
}Path_Data; //定义邻接表链表中的节点结构
typedef struct ANode
{
   int id;
   int adjverx;
   ANode *nextarc;
}ArcNode; //定义邻接表链表中的节点结构

typedef struct
{
   int arcnum;
   int id;              //顶点编号
   int x;               //button坐标
   int y;
   QString name;        //景点名
   QString info;        //景点介绍
   QString pic_path;    //图片路径 
}Node_Data;//定义邻接表顶点集

typedef struct Vnode
{
   Node_Data ndata;
   ArcNode *firstarc;      //指向第一条边
}VNode;//定义邻接表顶点集

typedef struct
{
    VNode adjlist[MAX];//顶点
    Path_Data path[MAX];//边信息
    QString bgpic_path;//背景图
    int n,e;
}ALGraph;//定义邻接表

typedef struct signal
{
    int visited[MAX];//遍历标志
    int stack[MAX*2];//顶点栈
    int stack_length;//栈顶指针
    int arcStack[MAX*2];//边栈
    int arcStack_length;//栈顶指针
}Signal;//路线推荐函数输出记录路线用
void AddNode(ALGraph *G,QString name,QString info,int x,int y,QString path);
void load(ALGraph *G);
void Save(ALGraph g);
//int search(ALGraph *&G,int x);
void ALInsert(ArcNode *&firstarc,int adjverx,int id);

void MakeGraph(ALGraph *G);
void DFS(ALGraph G,int i);
void ALL_DFS(ALGraph G);
int Dist(ALGraph g,int a,int b);
void Distpath(ALGraph g,int dist[],int path[],int S[],int a,int b);
int FindNode(ALGraph g,QString name);
void ALDelete(ArcNode *&firstarc,int adjverx);
void DelEdge(ALGraph *G,int m,int n);
void DelNode(ALGraph *G,QString name);
void Add_Node(ALGraph *G);
void Add_Path(ALGraph *G,QString start,QString final,QString arc,int distance);
int part_num(ALGraph g,int v,Signal *sign);
void Length_least(ALGraph g,int v,int *num,int Nodenum,Signal *sign);

QString FindPath(ALGraph g,int start,int final);
void Distpath(ALGraph g,int path[],int start,int final,QString *Qpath);
#endif // TRAVEL



