/* �����ҵ.cpp 
 * Name:���ɺ�  Compiler version��Microsoft Visual studio 2010 
 * Date 2015.1   email address:dingfeihong@live.cn
 * ʵ���ţ������ҵ  �汾 2.4
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

////////������ؽṹ��
typedef struct reader
{
	char name[20];            //����       
	char job[20];             //����
	char sex[15];             //�Ա�
	long int book[MAX];       //�������ƫ����
	int booknum;              //������Ŀ
	int id;                   //֤����
}READER;//������Ϣ
typedef struct readidx
{
	int id;                   //���
	char name[20];            //��������
	long int offset;          //ƫ����
}READIDX;//������ϢB���洢�ؼ���
typedef struct rnode
{
	int keynum;               //�ڵ����
	READIDX key[MAX+1];       //�ؼ���
	struct rnode *parent;     //˫��
	struct rnode *ptr[MAX+1];   //�ӽڵ�
}RNode;//������ϢB���洢
typedef struct{
	RNode *pt;
	int i;
	bool tag;
}RResult;//���ڲ�ѯ�Ľ������

////////ͼ����ؽṹ��
typedef struct book
{
	char name[20];            //����       
	char author[20];          //����
	char classify[20];        //���
	int  id;                  //���
	long int reader[MAX];
	int all;                  //�ݲ���
	int exist;                //�ڼ���
}BOOK;//��Ŀ��Ϣ
typedef struct bookidx
{
	int id;                //���
	char name[20];         //����  
	char author[20];          //����
	char classify[20];     //���
	long int offset;       //ƫ����
}BOOKIDX;//��Ŀ��ϢB���洢�ؼ���
typedef struct bnode
{
	int keynum;               //�ڵ����
	BOOKIDX key[MAX+1];         //�ؼ���
	struct bnode *parent;     //˫��
	struct bnode *ptr[MAX+1];   //�ӽڵ�
}BNode;//��Ŀ��ϢB���洢
typedef struct{
	BNode *pt;
	int i;
	bool tag;
}BResult;//���ڲ�ѯ�Ľ������

void book();//�鼮����
void reader();//���߹���
void borrow();//�軹��

//��������
RNode *R_load(void);//������Ϣ����
BNode *B_load(void);//��Ŀ��Ϣ����

//�˵�
int M_menu();//���˵�
int R_menu();//������Ϣά���˵�
int B_menu();//�鼮����˵�
int RT_menu();//���黹��˵�

//////////������Ϣ����
void R_search(RNode *t);//���߲�ѯ
void R_add(RNode *&t);//���û�ע��
void R_show(READER head);//�û���Ϣ��ӡ
void R_ShowALL(RNode *t);//�����û���Ϣ��ӡ

RNode *R_insert(RNode *t,READIDX p); //������ϢB-������
RResult R_find(RNode *t,int key);//B������
RResult R_search1(RNode *t);//����Ų�ѯ����
RResult R_search2(RNode *t);//��������ѯ����

void R_Del_Save(RNode *t);//ɾ���������Ϣ�洢�����
void R_MOVleft(RNode *p,int i);//B-��ɾ����ϲ�
void R_MOVright(RNode *p,int i);//B-��ɾ���Һϲ�
RNode *R_Combine(RNode *p,int i);//B-��ɾ��ȫ�ϲ�
RNode *R_Restore(RNode *p,int i);//B-���ع���
RNode *R_Recdel(RNode *p,int k);//ɾ��
RNode *R_IndexDel(RNode *root,int k);//B-������ɾ��
RNode *R_delete(RNode *t); //������Ϣע��

////////////��Ŀ��Ϣ����
void B_Del_Save(BNode *t);//ɾ������Ŀ��Ϣ�洢�����
void B_InforDel(long int k);//��Ŀ��ϸ��Ϣɾ����δ��ɣ�
void B_MOVleft(BNode *p,int i);//B-��ɾ����ϲ�
void B_MOVright(BNode *p,int i);//B-��ɾ���Һϲ�
void B_classify(BNode *t);//�鼮�������
void B_author(BNode *t);//�鼮�����߲���
BNode *B_Combine(BNode *p,int i);//B-��ɾ��ȫ�ϲ�
BNode *B_Restore(BNode *p,int i);//B-���ع���
BNode *B_Recdel(BNode *p,int k);//ɾ��
BNode *B_IndexDel(BNode *root,int k);//��Ŀ������B-����ɾ��
BNode *B_delete(BNode *t);//����ɾ��

BNode *B_insert(BNode *t,BOOKIDX p);//�鼮��ϢB-������
BResult B_borrow1(BNode *t);//����1
BResult B_borrow2(BNode *t);//����2
BResult B_find(BNode *t,int key);

void M_borrow(BNode *t,RNode *rt);//����
void B_return(BNode *t,RNode *rt);//����

void B_updata(BNode *t);//�������
void B_add(BNode *&t);//�����ϼ�
void B_search(BNode *t);//�鼮����
void B_show(BOOK head);//�鼮չʾ
void B_ShowALL(BNode *t);//�����鼮��Ϣ��ӡ