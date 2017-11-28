#include "stdafx.h"
#include"liberary.h"
//图书借阅
void borrow(){
	BNode *book=B_load();//外存读取
	RNode *reader=R_load();//外存读取
	char choice;
	while(1)
	{
		system("CLS");
		choice=RT_menu();//加载菜单
		switch(choice){
		case 1: M_borrow(book,reader);break;//借书
		case 2: B_return(book,reader);break;//还书
		case 3: return;//返回
		}
	}
}

int RT_menu()//菜单
{
	char s[12];
	int c;
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  借书\n");
	printf("\n\t\t\t\t2.  还书\n");
	printf("\n\t\t\t\t3.  返回\n");
	do{
		printf("\n\n\t\t\t\t请选择：");
		gets(s);
		printf("\n");
		c=atoi(s);
	}while(c<0||c>3);
	return c;
}
void M_borrow(BNode *t,RNode *rt){
	system("CLS");
	int c;
	char choice=' ';
	char s[12];                        /////////////菜单
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  根据书目编号查找\n");
	printf("\n\t\t\t\t2.  根据书目名字查找\n");
	printf("\n\t\t\t\t3.  返回\n");
	do{
		printf("\n\n\t\t\t\t请选择：");
		gets(s);
		printf("\n");
		c=atoi(s);
	}while(c<0||c>3);

	BResult borrow;

	//long int num;
	switch(c)////所借数的偏移量借书
	{
	case 1: borrow=B_borrow1(t);break;//按数目名
	case 2: borrow=B_borrow2(t);break;//按书目编号
	case 3:return;
	}
	if(!borrow.tag){
		printf("\n\t\t\t\t找不到这本书\n");
		getchar();
		return; 
	}	

	FILE *fp;                      //打开书目详细信息文件
	if ((fp=fopen(Book,"rb"))==NULL)
	{
		printf("\n\t\t\t\t文件无法写入\n");
		getchar();
		return; 
	}	

	BOOK book;
	if(!fseek(fp,borrow.pt->key[borrow.i].offset,0))           //读取所借书目信息
		fread(&book,sizeof(BOOK),1,fp);
	fclose(fp);
	B_show(book);                //展示书目信息

	if(book.exist<0){    //当书目全都借出去
		printf("\n\t\t\t\t书已无剩余\n");
		getchar();
		return;
	}

	while(1){
		printf("\n\t\t\t\t是否要借这本书:《%s》 (y\\n)",book.name);
		fflush(stdin);
		scanf("%c",&choice);
		fflush(stdin);
		if(choice=='n'||choice=='y')
			break;
	}	
	if(choice=='n')
		return;

	/*printf("请输入用户ID\n");//关联用户
	int id;
	scanf("%d",&id);
	fflush(stdin);*/

	RResult temp=R_search1(rt);//找到用户偏移量
	if(!temp.tag){
		getchar();
		return;
	}
	
	book.exist--;
	book.reader[book.all-book.exist-1]=temp.pt->key[temp.i].offset;//保存用户偏移量到书目信息
	if ((fp=fopen(Book,"rb+"))==NULL)//修改书目信息
	{
		printf("\n\t\t\t\t文件无法写入");
		getchar();
		return; 
	}

	fseek(fp,borrow.pt->key[borrow.i].offset,SEEK_SET);		
	if(fwrite(&book,sizeof(BOOK),1,fp)!=1)
	{ 
		printf("\n\t\t\t\t文件不能写入数据,请检查后重新运行.\n");		
		getchar();
		return; 
	}	
	fclose(fp);

	if ((fp=fopen(Reader,"rb"))==NULL) //修改读者信息
	{
		printf("\n\t\t\t\t文件无法写入\n");
		getchar();
		return; 
	}	
	READER user;
	if(!fseek(fp,temp.pt->key[temp.i].offset,0))
		fread(&user,sizeof(READER),1,fp);
	fclose(fp);
	if(user.booknum>=5){    //当书目全都借出去
		printf("\n\t\t\t\t你不能再借书了\n");
		getchar();
		return;
	}
	user.book[user.booknum]=borrow.pt->key[borrow.i].offset;        //记录所借书偏移量
	user.booknum++;

	if ((fp=fopen(Reader,"rb+"))==NULL) //读者信息写入文件
	{
		printf("\n\t\t\t\t文件无法写入");
		getchar();
		return; 
	}	
	fseek(fp,temp.pt->key[temp.i].offset,SEEK_SET);		
	if(fwrite(&user,sizeof(READER),1,fp)!=1)
	{ 
		printf("\n\t\t\t\t文件不能写入数据,请检查后重新运行.\n");		
		getchar();
		return; 
	}	
	printf("\n\t\t\t\t借书成功\n");
	getchar();
	fclose(fp);	
}
void B_return(BNode *t,RNode *rt){
	//system("CLS");
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	RResult temp=R_search1(rt);
	if (!temp.tag)
		return; 

	FILE *fp;                      //打开读者详细信息文件
	if ((fp=fopen(Reader,"rb"))==NULL)
	{
		printf("\n\t\t\t\t文件无法写入\n");
		getchar();
		return; 
	}	

	READER user;	
	if(!fseek(fp,temp.pt->key[temp.i].offset,0))           //读取用户信息
		fread(&user,sizeof(READER),1,fp);
	fclose(fp);

	while(1)
	{
		R_show(user);        //展示用户信息   
		printf("\n\t\t\t\t准备还哪一本书,输入0是返回：\n");
	    int choice;
		do{
			fflush(stdin);
	        scanf("%d",&choice);
		}while(choice<0||choice>user.booknum);
        if(choice==0)break;

		long int num=user.book[choice-1];                   //保存要还书的偏移量
		int i;
		for(i=choice-1;i<user.booknum-1;i++)
			user.book[i]=user.book[i+1];
		user.booknum--;

		if ((fp=fopen(Reader,"rb+"))==NULL)                    //打开用户信息
		{
			printf("\n\t\t\t\t文件无法写入");
			getchar();
		    return; 
	    }
		if(!fseek(fp,temp.pt->key[temp.i].offset,0))           //写入用户信息
		    fwrite(&user,sizeof(READER),1,fp);
		fclose(fp);

		BOOK book;
	    if ((fp=fopen(Book,"rb"))==NULL)//修改书目信息
		{
			printf("\n\t\t\t\t文件无法写入");
			getchar();
		    return; 
	    }
		if(!fseek(fp,num,0))           //读取图书信息
	        fread(&book,sizeof(BOOK),1,fp);
	    fclose(fp);
		
		for(i=0;book.reader[i]!=temp.pt->key[temp.i].offset&&i<(book.all-book.exist-1);i++);
		for(;i<(book.all-book.exist-1);i++)
			book.reader[i]=book.reader[i+1];
		book.exist++;

		if ((fp=fopen(Book,"rb+"))==NULL)//修改书目信息
		{
			printf("\n\t\t\t\t文件无法写入");
			getchar();
			return; 
		}	
		fseek(fp,num,SEEK_SET);		
		if(fwrite(&book,sizeof(READER),1,fp)!=1)
		{ 
			printf("\n\t\t\t\t文件不能写入数据,请检查后重新运行.\n");		
			getchar();
			exit(1);
		}	
		fclose(fp);
		printf("\n\t\t\t\t还书成功\n\n");
		getchar();
		
	}
}

BResult B_borrow1(BNode *t){
	KeyType key;	
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t请输入书的编号:");	
	fflush(stdin);
	scanf("%d",&key);
	fflush(stdin);
	BResult result=B_find(t,key);//根据书目编号查找
	return result;
}
BResult B_borrow2(BNode *t){//B-树遍历
	char name[20];
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t请输入书名:");
	gets(name);

	BNode *p;
	BNode *qu[MAXSIZE];//建立队列
	BResult ret;
	ret.tag=false;
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//根节点入队

	while(front!=rear){
		front=(front+1)%MAXSIZE;
		p=qu[front];
		for(i=1;i<p->keynum+1;i++)
			if(!strcmp(name,p->key[i].name)){//当姓名匹配，查找到，并返回
				ret.i=i,ret.tag=1,ret.pt=p;
				return ret;
			}
		for(i=0;i<=p->keynum;i++)
		{
			if(p->ptr[i])
			{
				rear=(rear+1)%MAXSIZE;//子节点入队
				qu[rear]=p->ptr[i];
			}
		}
	}
	return ret;
}