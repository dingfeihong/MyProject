// 图书管理.cpp : 定义控制台应用程序的入口点。
//
// 通讯录.cpp : 定义控制台应用程序的入口点。
#include "stdafx.h"
#include"liberary.h"
int M_menu()//主菜单
{
	char s[80];
	int c;
	printf("\n\n\t\t\t\t图书管理系统\tversion:2.4\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  书籍管理\n");
	printf("\n\t\t\t\t2.  用户信息维护\n");
	printf("\n\t\t\t\t3.  借书还书\n");
	printf("\n\t\t\t\t4.  退出\n");
	do{
		printf("\n\n\t\t\t\t请选择：");
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
		choice=M_menu();//加载菜单
		switch(choice){
		case 1: book();break;//执行书目管理函数
		case 2: reader();break;//执行书目管理函数
		case 3: borrow();break;//借还书函数
		case 4: return 0;
		}
	}
}

