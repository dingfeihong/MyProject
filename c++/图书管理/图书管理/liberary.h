/* 编程作业.cpp 
 * Name:丁飞鸿  Compiler version：Microsoft Visual studio 2010 
 * Date 2015.1   email address:dingfeihong@live.cn
 * 实验编号：编程作业  版本 2.4
 * Brief description of program and objective.        
 *----------------------------------------------------
 */
#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

#define ReaderIdx "ReaderIdx.dat"
#define Reader "Reader.dat"
#define BookIdx "BookIdx.dat"
#define Book "Book.dat"
#define MAX 5
#define MAXSIZE 50
typedef int KeyType;

////////读者相关结构体
typedef struct reader
{
	char name[20];            //姓名       
	char job[20];             //工作
	char sex[15];             //性别
	long int book[MAX];       //所借书的偏移量
	int booknum;              //借书数目
	int id;                   //证件号
}READER;//读者信息
typedef struct readidx
{
	int id;                   //编号
	char name[20];            //读者姓名
	long int offset;          //偏移量
}READIDX;//读者信息B树存储关键字
typedef struct rnode
{
	int keynum;               //节点个数
	READIDX key[MAX+1];       //关键字
	struct rnode *parent;     //双亲
	struct rnode *ptr[MAX+1];   //子节点
}RNode;//读者信息B树存储
typedef struct{
	RNode *pt;
	int i;
	bool tag;
}RResult;//用于查询的结果返回

////////图书相关结构体
typedef struct book
{
	char name[20];            //书名       
	char author[20];          //作者
	char classify[20];        //类别
	int  id;                  //编号
	long int reader[MAX];
	int all;                  //馆藏量
	int exist;                //在架上
}BOOK;//书目信息
typedef struct bookidx
{
	int id;                //编号
	char name[20];         //书名  
	char author[20];          //作者
	char classify[20];     //类别
	long int offset;       //偏移量
}BOOKIDX;//书目信息B树存储关键字
typedef struct bnode
{
	int keynum;               //节点个数
	BOOKIDX key[MAX+1];         //关键字
	struct bnode *parent;     //双亲
	struct bnode *ptr[MAX+1];   //子节点
}BNode;//书目信息B树存储
typedef struct{
	BNode *pt;
	int i;
	bool tag;
}BResult;//用于查询的结果返回

void book();//书籍管理
void reader();//读者管理
void borrow();//借还书

//索引加载
RNode *R_load(void);//读者信息加载
BNode *B_load(void);//书目信息加载

//菜单
int M_menu();//主菜单
int R_menu();//读者信息维护菜单
int B_menu();//书籍管理菜单
int RT_menu();//借书还书菜单

//////////读者信息函数
void R_search(RNode *t);//读者查询
void R_add(RNode *&t);//新用户注册
void R_show(READER head);//用户信息打印
void R_ShowALL(RNode *t);//所有用户信息打印

RNode *R_insert(RNode *t,READIDX p); //读者信息B-树插入
RResult R_find(RNode *t,int key);//B树查找
RResult R_search1(RNode *t);//按编号查询读者
RResult R_search2(RNode *t);//按姓名查询读者

void R_Del_Save(RNode *t);//删除后读者信息存储到外存
void R_MOVleft(RNode *p,int i);//B-树删除左合并
void R_MOVright(RNode *p,int i);//B-树删除右合并
RNode *R_Combine(RNode *p,int i);//B-树删除全合并
RNode *R_Restore(RNode *p,int i);//B-树重构造
RNode *R_Recdel(RNode *p,int k);//删除
RNode *R_IndexDel(RNode *root,int k);//B-树索引删除
RNode *R_delete(RNode *t); //读者信息注销

////////////书目信息函数
void B_Del_Save(BNode *t);//删除后书目信息存储到外存
void B_InforDel(long int k);//书目详细信息删除（未完成）
void B_MOVleft(BNode *p,int i);//B-树删除左合并
void B_MOVright(BNode *p,int i);//B-树删除右合并
void B_classify(BNode *t);//书籍按类查找
void B_author(BNode *t);//书籍按作者查找
BNode *B_Combine(BNode *p,int i);//B-树删除全合并
BNode *B_Restore(BNode *p,int i);//B-树重构造
BNode *B_Recdel(BNode *p,int k);//删除
BNode *B_IndexDel(BNode *root,int k);//书目索引（B-树）删除
BNode *B_delete(BNode *t);//旧书删除

BNode *B_insert(BNode *t,BOOKIDX p);//书籍信息B-树插入
BResult B_borrow1(BNode *t);//借书1
BResult B_borrow2(BNode *t);//借书2
BResult B_find(BNode *t,int key);

void M_borrow(BNode *t,RNode *rt);//借书
void B_return(BNode *t,RNode *rt);//还书

void B_updata(BNode *t);//旧书更新
void B_add(BNode *&t);//新书上架
void B_search(BNode *t);//书籍搜索
void B_show(BOOK head);//书籍展示
void B_ShowALL(BNode *t);//所有书籍信息打印