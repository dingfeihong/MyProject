// ͼ�����.cpp : �������̨Ӧ�ó������ڵ㡣
//
// ͨѶ¼.cpp : �������̨Ӧ�ó������ڵ㡣
#include "stdafx.h"
#include"liberary.h"
int M_menu()//���˵�
{
	char s[80];
	int c;
	printf("\n\n\t\t\t\tͼ�����ϵͳ\tversion:2.4\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  �鼮����\n");
	printf("\n\t\t\t\t2.  �û���Ϣά��\n");
	printf("\n\t\t\t\t3.  ���黹��\n");
	printf("\n\t\t\t\t4.  �˳�\n");
	do{
		printf("\n\n\t\t\t\t��ѡ��");
		gets(s);
		printf("\n");
		c=atoi(s);
	}while(c<=0||c>4);
	return c;
}
int main()
{	
	int choice;
	while(1)
	{
		system("CLS");
		choice=M_menu();//���ز˵�
		switch(choice){
		case 1: book();break;//ִ����Ŀ������
		case 2: reader();break;//ִ����Ŀ������
		case 3: borrow();break;//�軹�麯��
		case 4: return 0;
		}
	}
}

