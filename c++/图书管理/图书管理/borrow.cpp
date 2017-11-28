#include "stdafx.h"
#include"liberary.h"
//ͼ�����
void borrow(){
	BNode *book=B_load();//����ȡ
	RNode *reader=R_load();//����ȡ
	char choice;
	while(1)
	{
		system("CLS");
		choice=RT_menu();//���ز˵�
		switch(choice){
		case 1: M_borrow(book,reader);break;//����
		case 2: B_return(book,reader);break;//����
		case 3: return;//����
		}
	}
}

int RT_menu()//�˵�
{
	char s[12];
	int c;
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  ����\n");
	printf("\n\t\t\t\t2.  ����\n");
	printf("\n\t\t\t\t3.  ����\n");
	do{
		printf("\n\n\t\t\t\t��ѡ��");
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
	char s[12];                        /////////////�˵�
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  ������Ŀ��Ų���\n");
	printf("\n\t\t\t\t2.  ������Ŀ���ֲ���\n");
	printf("\n\t\t\t\t3.  ����\n");
	do{
		printf("\n\n\t\t\t\t��ѡ��");
		gets(s);
		printf("\n");
		c=atoi(s);
	}while(c<0||c>3);

	BResult borrow;

	//long int num;
	switch(c)////��������ƫ��������
	{
	case 1: borrow=B_borrow1(t);break;//����Ŀ��
	case 2: borrow=B_borrow2(t);break;//����Ŀ���
	case 3:return;
	}
	if(!borrow.tag){
		printf("\n\t\t\t\t�Ҳ����Ȿ��\n");
		getchar();
		return; 
	}	

	FILE *fp;                      //����Ŀ��ϸ��Ϣ�ļ�
	if ((fp=fopen(Book,"rb"))==NULL)
	{
		printf("\n\t\t\t\t�ļ��޷�д��\n");
		getchar();
		return; 
	}	

	BOOK book;
	if(!fseek(fp,borrow.pt->key[borrow.i].offset,0))           //��ȡ������Ŀ��Ϣ
		fread(&book,sizeof(BOOK),1,fp);
	fclose(fp);
	B_show(book);                //չʾ��Ŀ��Ϣ

	if(book.exist<0){    //����Ŀȫ�����ȥ
		printf("\n\t\t\t\t������ʣ��\n");
		getchar();
		return;
	}

	while(1){
		printf("\n\t\t\t\t�Ƿ�Ҫ���Ȿ��:��%s�� (y\\n)",book.name);
		fflush(stdin);
		scanf("%c",&choice);
		fflush(stdin);
		if(choice=='n'||choice=='y')
			break;
	}	
	if(choice=='n')
		return;

	/*printf("�������û�ID\n");//�����û�
	int id;
	scanf("%d",&id);
	fflush(stdin);*/

	RResult temp=R_search1(rt);//�ҵ��û�ƫ����
	if(!temp.tag){
		getchar();
		return;
	}
	
	book.exist--;
	book.reader[book.all-book.exist-1]=temp.pt->key[temp.i].offset;//�����û�ƫ��������Ŀ��Ϣ
	if ((fp=fopen(Book,"rb+"))==NULL)//�޸���Ŀ��Ϣ
	{
		printf("\n\t\t\t\t�ļ��޷�д��");
		getchar();
		return; 
	}

	fseek(fp,borrow.pt->key[borrow.i].offset,SEEK_SET);		
	if(fwrite(&book,sizeof(BOOK),1,fp)!=1)
	{ 
		printf("\n\t\t\t\t�ļ�����д������,�������������.\n");		
		getchar();
		return; 
	}	
	fclose(fp);

	if ((fp=fopen(Reader,"rb"))==NULL) //�޸Ķ�����Ϣ
	{
		printf("\n\t\t\t\t�ļ��޷�д��\n");
		getchar();
		return; 
	}	
	READER user;
	if(!fseek(fp,temp.pt->key[temp.i].offset,0))
		fread(&user,sizeof(READER),1,fp);
	fclose(fp);
	if(user.booknum>=5){    //����Ŀȫ�����ȥ
		printf("\n\t\t\t\t�㲻���ٽ�����\n");
		getchar();
		return;
	}
	user.book[user.booknum]=borrow.pt->key[borrow.i].offset;        //��¼������ƫ����
	user.booknum++;

	if ((fp=fopen(Reader,"rb+"))==NULL) //������Ϣд���ļ�
	{
		printf("\n\t\t\t\t�ļ��޷�д��");
		getchar();
		return; 
	}	
	fseek(fp,temp.pt->key[temp.i].offset,SEEK_SET);		
	if(fwrite(&user,sizeof(READER),1,fp)!=1)
	{ 
		printf("\n\t\t\t\t�ļ�����д������,�������������.\n");		
		getchar();
		return; 
	}	
	printf("\n\t\t\t\t����ɹ�\n");
	getchar();
	fclose(fp);	
}
void B_return(BNode *t,RNode *rt){
	//system("CLS");
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	RResult temp=R_search1(rt);
	if (!temp.tag)
		return; 

	FILE *fp;                      //�򿪶�����ϸ��Ϣ�ļ�
	if ((fp=fopen(Reader,"rb"))==NULL)
	{
		printf("\n\t\t\t\t�ļ��޷�д��\n");
		getchar();
		return; 
	}	

	READER user;	
	if(!fseek(fp,temp.pt->key[temp.i].offset,0))           //��ȡ�û���Ϣ
		fread(&user,sizeof(READER),1,fp);
	fclose(fp);

	while(1)
	{
		R_show(user);        //չʾ�û���Ϣ   
		printf("\n\t\t\t\t׼������һ����,����0�Ƿ��أ�\n");
	    int choice;
		do{
			fflush(stdin);
	        scanf("%d",&choice);
		}while(choice<0||choice>user.booknum);
        if(choice==0)break;

		long int num=user.book[choice-1];                   //����Ҫ�����ƫ����
		int i;
		for(i=choice-1;i<user.booknum-1;i++)
			user.book[i]=user.book[i+1];
		user.booknum--;

		if ((fp=fopen(Reader,"rb+"))==NULL)                    //���û���Ϣ
		{
			printf("\n\t\t\t\t�ļ��޷�д��");
			getchar();
		    return; 
	    }
		if(!fseek(fp,temp.pt->key[temp.i].offset,0))           //д���û���Ϣ
		    fwrite(&user,sizeof(READER),1,fp);
		fclose(fp);

		BOOK book;
	    if ((fp=fopen(Book,"rb"))==NULL)//�޸���Ŀ��Ϣ
		{
			printf("\n\t\t\t\t�ļ��޷�д��");
			getchar();
		    return; 
	    }
		if(!fseek(fp,num,0))           //��ȡͼ����Ϣ
	        fread(&book,sizeof(BOOK),1,fp);
	    fclose(fp);
		
		for(i=0;book.reader[i]!=temp.pt->key[temp.i].offset&&i<(book.all-book.exist-1);i++);
		for(;i<(book.all-book.exist-1);i++)
			book.reader[i]=book.reader[i+1];
		book.exist++;

		if ((fp=fopen(Book,"rb+"))==NULL)//�޸���Ŀ��Ϣ
		{
			printf("\n\t\t\t\t�ļ��޷�д��");
			getchar();
			return; 
		}	
		fseek(fp,num,SEEK_SET);		
		if(fwrite(&book,sizeof(READER),1,fp)!=1)
		{ 
			printf("\n\t\t\t\t�ļ�����д������,�������������.\n");		
			getchar();
			exit(1);
		}	
		fclose(fp);
		printf("\n\t\t\t\t����ɹ�\n\n");
		getchar();
		
	}
}

BResult B_borrow1(BNode *t){
	KeyType key;	
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t��������ı��:");	
	fflush(stdin);
	scanf("%d",&key);
	fflush(stdin);
	BResult result=B_find(t,key);//������Ŀ��Ų���
	return result;
}
BResult B_borrow2(BNode *t){//B-������
	char name[20];
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t����������:");
	gets(name);

	BNode *p;
	BNode *qu[MAXSIZE];//��������
	BResult ret;
	ret.tag=false;
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//���ڵ����

	while(front!=rear){
		front=(front+1)%MAXSIZE;
		p=qu[front];
		for(i=1;i<p->keynum+1;i++)
			if(!strcmp(name,p->key[i].name)){//������ƥ�䣬���ҵ���������
				ret.i=i,ret.tag=1,ret.pt=p;
				return ret;
			}
		for(i=0;i<=p->keynum;i++)
		{
			if(p->ptr[i])
			{
				rear=(rear+1)%MAXSIZE;//�ӽڵ����
				qu[rear]=p->ptr[i];
			}
		}
	}
	return ret;
}