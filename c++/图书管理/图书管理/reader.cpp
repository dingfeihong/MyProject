#include "stdafx.h"
#include"liberary.h"
//用户注册注销 

void reader(){ //用户管理主菜单
	RNode *readidx=(RNode *)malloc(sizeof(RNode));
	char choice;
	readidx=R_load();//B树载入
	while(1)
	{
		system("CLS");
		choice=R_menu();//加载菜单
		switch(choice){
		case 1: R_add(readidx);break;//添加新用户
	    case 2: R_search(readidx);break;//用户查询
		case 3: readidx=R_delete(readidx);R_Del_Save(readidx);break;//用户删除
		case 4: R_ShowALL(readidx);break;//用户显示
		case 5: return;//返回
		}
	}
}
int R_menu()//菜单
{
	char s[80];
	int c;
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  新用户注册\n");
	printf("\n\t\t\t\t2.  用户查找\n");
	printf("\n\t\t\t\t3.  用户注销\n");
	printf("\n\t\t\t\t4.  用户显示\n");
	printf("\n\t\t\t\t5.  返回\n");
	do{
		printf("\n\n\t\t\t\t请选择：");
		gets(s);
		printf("\n");
		c=atoi(s);
	}while(c<0||c>5);
	return c;
}

void R_show(READER head)   // 显示读者内容  
{
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	FILE *fp;
	if ((fp=fopen(Book,"rb"))==NULL)//显示读者信息
	{
		printf("文件无法写入");
		getchar();
		return; 
	}
	BOOK book;
	printf("\n\t\t\t\t姓名:%-10s\n",head.name);
	printf("\n\t\t\t\t工作:%-10s\n",head.job);
	printf("\n\t\t\t\t性别:%-10s\n",head.sex);
	printf("\n\t\t\t\t证件号码:%-10d\n",head.id);	
	if(head.booknum>0){
		printf("\n\t\t\t\t用户所借书：\n");
		for(int i=0;i<head.booknum;i++){
			if(!fseek(fp,head.book[i],0))           //读取图书信息
				fread(&book,sizeof(BOOK),1,fp);
			printf("\t\t\t\t%d.《%s》\n",i+1,book.name);
		}
	}
	getchar();
 }
void R_add(RNode *&t)
{
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	READER temp;
	printf("\n\n\t\t\t\t请输入姓名:");      
	gets(temp.name);
	printf("\n\n\t\t\t\t请输入工作单位:"); 
	gets(temp.job);
	fflush(stdin);
	printf("\n\n\t\t\t\t请输入性别:");
	gets(temp.sex);
	printf("\n\n\t\t\t\t请输入证件号码:");
	fflush(stdin);
	scanf("%d",&temp.id);
	fflush(stdin);
	temp.booknum=0;

	FILE *fp;

	RResult search=R_find(t,temp.id);
	if (search.tag)
	{
		printf("\n\t\t\t\t书籍编号重复\n");
		getchar();
		return; 
	}	

	if ((fp=fopen(Reader,"ab"))==NULL)
	{
		printf("\n\t\t\t\t文件无法写入");
		getchar();
		return; 
	}	

	fseek(fp,0L,SEEK_END);		
	long length=ftell(fp);//计算偏移量
	fseek(fp,0L,SEEK_SET);

	if(fwrite(&temp,sizeof(READER),1,fp)!=1)//写入
	{ 
		printf("\n\t\t\t\t文件无法写入");
		getchar();
		return; 
	}	
	fclose(fp);


	if ((fp=fopen(ReaderIdx,"ab"))==NULL)//索引信息文件
	{
		printf("\n\t\t\t\t文件无法写入");
		getchar();
		return; 
	}

	READIDX tnew;//写入索引信息
	tnew.id=temp.id;
	tnew.offset=length;
	strcpy(tnew.name,temp.name);

	if(fwrite(&tnew,sizeof(READIDX),1,fp)!=1)//写入
	{ 
		printf("\n\t\t\t\t文件无法写入");
		getchar();
		return; 
	}	
	t=R_insert(t,tnew);//插入B-树
	fclose(fp );
	printf("\n\t\t\t\t注册成功\n");
	getchar();
}
void R_search(RNode *t){

	system("CLS");
	int c;
	char choice=' ';
	char s[12];                        /////////////菜单
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  根据编号查找\n");
	printf("\n\t\t\t\t2.  根据姓名查找\n");
	printf("\n\t\t\t\t3.  返回\n");
	do{
		printf("\n\n\t\t\t\t请选择：");
		gets(s);
		printf("\n");
		c=atoi(s);
	}while(c<=0||c>3);

	RResult result;

	//long int num;
	switch(c)////所借数的偏移量借书
	{
	case 1: result=R_search1(t);break;//按数目名
	case 2: result=R_search2(t);break;//按书目编号
	case 3:return;
	}

	if(!result.tag)
		return; 

	FILE *fp=fopen(Reader,"rb");
	READER user;
	if(!fseek(fp,result.pt->key[result.i].offset,0))//读取信息
		fread(&user,sizeof(READER),1,fp);
	fclose(fp);
	R_show(user);	
}

RResult R_search1(RNode *t){//按编号查询
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	KeyType key;	
	printf("\n\t\t\t\t请输入您的ID:");	
	fflush(stdin);
	scanf("%d",&key);
	fflush(stdin);
	RResult result=R_find(t,key);
	if(!result.tag){
		printf("\n\t\t\t\t未查找到该用户\n");
		getchar();
	}
	return result;
}
RResult R_search2(RNode *t){//按姓名查询
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	char name[20];
	printf("\n\t\t\t\t请输入姓名:");
	gets(name);
	RResult ret;
	ret.tag=false;

	RNode *p;
	RNode *qu[MAXSIZE];//建立队列
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//根节点入队

	while(front!=rear){
		front=(front+1)%MAXSIZE;//出队
		p=qu[front];
		for(i=1;i<p->keynum+1;i++)
			if(!strcmp(name,p->key[i].name)){//查找到，返回
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
	getchar();
	printf("\n\t\t\t\t未查找到该用户\n");
	return ret;
}
RResult R_find(RNode *t,int key){//B-树中关键字查询
	RNode *q=NULL;
	RResult r;	
	r.i=0,r.pt=t,r.tag=0;
	while(r.pt&&!r.tag){
		for(;r.i<r.pt->keynum&&r.pt->key[r.i+1].id<=key;r.i++);
		if(r.i>0&&r.pt->key[r.i].id==key)
			r.tag=1;
		else{
			q=r.pt;
			r.pt=r.pt->ptr[r.i];
		}		
	}
	if(!r.tag)
		r.pt=q;
	return r;
}

RNode *R_load(void)   //由文件中的数据生成一个通讯录的B树      
{ 
	 RNode *t;
	 READIDX p;
	 FILE *fp;
	 t=(RNode *)malloc(sizeof(RNode));//;B-树根节点
	 t->keynum=0,t->parent=NULL;
	 for(int i=0;i<MAX;i++)
		 t->ptr[i]=NULL,t->key[i].id=-1;

	 fp=fopen(ReaderIdx,"rb+");//打开外存文件
	 if (fp==NULL){
		 printf("\n\t\t\t\tcan not open file %s\n", Reader);
		 return t;
	 }
	 else
	 { 
		 while(!feof(fp))
			 if(fread(&p,sizeof(READIDX),1,fp)==1)//生成B-树
				 t=R_insert(t,p);
		 fclose(fp);
		 return(t);
	 }
 }
RNode *R_delete(RNode *t)      
{
	int k;
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	printf("\n\n\t\t\t\t请输入用户证件号码:");
	fflush(stdin);
	scanf("%d",&k);
	fflush(stdin);
	
	char choice=' ';
	RResult search=R_find(t,k);
	if(!search.tag){
		printf("\n\t\t\t\t未查找到该用户\n");
		getchar();
		return t;
	}

	FILE *fp=fopen(Reader,"rb");
	READER user;
	if(!fseek(fp,search.pt->key[search.i].offset,0))//读取信息
		fread(&user,sizeof(READER),1,fp);
	fclose(fp);
	R_show(user);	


	while(1){
		printf("\n\t\t\t\t是否要注销？ (y\\n):");
		fflush(stdin);
		scanf("%c",&choice);
		fflush(stdin);
		if(choice=='n'||choice=='y')
			break;
	}	
	if(choice=='n')
		return t;

	if(user.booknum>0){
		printf("\n\t\t\t\t请先把所有书归还\n");
		getchar();
		return t;
	}
	t=R_IndexDel(t,k);

	printf("\n\t\t\t\t注销成功\n");
	getchar();
	return t;
}
RNode *R_insert(RNode *t,READIDX p){  //ReaderIdx B树的插入
	RResult r=R_find(t,p.id);       //求出插入关键字在B-树的位置
	if(r.tag)
		return t;
	int j=0;
	for(j=r.pt->keynum+1;j>r.i;j--)
		r.pt->key[j]=r.pt->key[j-1];  
	r.pt->key[r.i+1]=p;                 //关键字插入
	r.pt->keynum++;
	r.pt->ptr[r.pt->keynum]=NULL;
	if(r.pt->keynum>=MAX){           //当节点满
		RNode *tnew=(RNode *)malloc(sizeof(RNode));
		for(int i=0;i<MAX;i++)
			tnew->ptr[i]=NULL,tnew->key[i].id=-1;
		int n=(MAX+1)/2;
		for(j=n+1;j<=r.pt->keynum;j++)
			tnew->key[j-n]=r.pt->key[j];
		r.pt->keynum=tnew->keynum=r.pt->keynum-n;
		if(!r.pt->parent){            //如果双亲节点为空，即当前为根节点
			RNode *tnew2=(RNode *)malloc(sizeof(RNode));
			for(int i=0;i<MAX;i++)
				tnew2->key[i].id=-1,tnew2->ptr[i]=NULL;
			tnew2->key[1]=r.pt->key[n];
			tnew2->parent=NULL;		
			tnew2->keynum=1;
			tnew->parent=r.pt->parent=tnew2;
			tnew2->ptr[0]=r.pt,tnew2->ptr[1]=tnew;
			t=tnew2;
		}
		else{
			tnew->parent=r.pt->parent;
			R_insert(tnew->parent,r.pt->key[n]);
			tnew->parent->ptr[tnew->parent->keynum]=tnew;
		}
	}	
	return t;
}
RNode *R_Combine(RNode *p,int i)
{
	int j;
	RNode *q=p->ptr[i];
	RNode *l=p->ptr[i-1];
	l->keynum++;
	l->key[l->keynum]=p->key[i];
	l->ptr[l->keynum]=q->ptr[0];
	for(j=1;j<=q->keynum;++j)
	{
		l->keynum++;
		l->key[l->keynum]=q->key[j];
		l->ptr[l->keynum]=q->ptr[j];
	}
	p->keynum--;
	for(j=i;j<=p->keynum;++j)
	{
		p->key[j]=p->key[j+1];
		p->ptr[j]=p->ptr[j+1];
	}	
	free(q);
	
	if(!p->keynum)
		p=l;

	return p;
}
RNode *R_Restore(RNode *p,int i)//重调整
{
	int keynum=p->keynum;
	if(i==0)
	{
		if(p->ptr[1]->keynum>(MAX-1)/2)
			R_MOVleft(p,1);
		else 
			p=R_Combine(p,1);
	}
	else if(i==keynum)
	{
		if(p->ptr[i-1]->keynum>(MAX-1)/2) 
			R_MOVright(p,i);
		else 
			p=R_Combine(p,i);
	}
	else if(p->ptr[i-1]->keynum >(MAX-1)/2) 
		R_MOVright(p,i);
	else if(p->ptr[i+1]->keynum >(MAX-1)/2) 
		R_MOVleft(p,i+1);
	else 
		p=R_Combine(p, i);
	return p;
}
RNode *R_Recdel(RNode *p,int k)//删除操作
{
	if(!p) return p;//树为空，直接返回

	RResult found=R_find(p,k);//查找
	if(!found.tag) 
	{
		printf("\n\t\t\t\t查找不到该用户\n");//查找不到用户
		getchar();
		return p;
	}

	if(found.pt->ptr[found.i])//当删除节点不是叶子
	{
		RNode *q;
		for(q=found.pt->ptr[found.i];q->ptr[0];q=q->ptr[0]);//找到当前节点下最小节点
		found.pt->key[found.i]=q->key[1];//替换
		p=R_Recdel(found.pt->ptr[found.i],found.pt->key[found.i].id);//递归删除
	}
	else //当前为叶子节点
	{
		int j;
		for(j=found.i;j<=found.pt->keynum;++j)//删除该节点
		{
			found.pt->key[j]=found.pt->key[j+1];
			found.pt->ptr[j]=found.pt->ptr[j+1];
		}
		found.pt->keynum--;
	}
	//else found.tag=R_Recdel(p->ptr[found.i],k);
	if(found.pt->parent)
	{
		for(found.i=0;found.i<=found.pt->parent->keynum; ++found.i)
		{
			if(found.pt->parent->ptr[found.i]==found.pt) 
				break;
		}		
		if(found.pt->keynum<(MAX-1)/2) //删完叶子节点的k后，p的keynum小于min，就进行调整
			p=R_Restore(found.pt->parent,found.i);
	}
	return p;
}
RNode *R_IndexDel(RNode *root,int k)//索引B-树删除
{
	RNode *p;
	root=R_Recdel(root, k);
	if(!(root)->keynum&&root->ptr[0])//
	{
		p=root;
		root=(root)->ptr[0];
		free(p);
	}
	return root;
}

void R_InforDel(long int k)
{
	FILE *fp;
	if ((fp=fopen(Reader,"rb+"))==NULL)//修改书目信息
	{
		printf("\n\t\t\t\t文件无法写入");
		getchar;
		return; 
	}
	fseek(fp,0L,SEEK_END);		
	long length=ftell(fp);//计算偏移量
	fseek(fp,k,SEEK_SET);		
	if(fwrite(&book,sizeof(BOOK),1,fp)!=1)
	{ 
		printf("\n\t\t\t\t文件不能写入数据,请检查后重新运行.\n");		
		getchar;
		return; 
	}	
	fclose(fp);
}
void R_MOVleft(RNode *p,int i)//左调整
{
	int j;
	RNode *t, *x;	
	t=p->ptr[i-1];//将父节点的i移到左兄弟里
	t->keynum++;
	t->key[t->keynum]=p->key[i];
	t->ptr[t->keynum]=p->ptr[i]->ptr[0];	
	t=p->ptr[i];// 将右兄弟的最左边的k移到父节点里
	x=p->ptr[i-1];
	p->key[i]=t->key[1];
	x->ptr[x->keynum]=t->ptr[1];	
	t->keynum--;// 将右兄弟的所有关键字左移一位
	for(j=1;j<=t->keynum;++j)
	{
		t->key[j]=t->key[j+1];
		t->ptr[j]=t->ptr[j+1];
	}
}
void R_MOVright(RNode *p,int i)//右调整
{
	int j;
	RNode *t;
	t=p->ptr[i];
	for(j=t->keynum;j>=1;--j)
	{
		t->key[j+1]=t->key[j];
		t->ptr[j+1]=t->ptr[j];
	}
	t->ptr[1]=t->ptr[0];
	t->keynum++;
	t->key[1]=p->key[i];

	t=p->ptr[i-1];
	p->key[i]=t->key[t->keynum];
	p->ptr[i]->ptr[0]=t->ptr[t->keynum];
	t->keynum--;
}
void R_Del_Save(RNode *t)//删除后存储索引到外存
{
	FILE *fp;
	if ((fp=fopen(ReaderIdx,"wb"))==NULL)//修改书目信息
	{
		printf("\n\t\t\t\t文件无法写入");
		getchar();
		return; 
	}
	
	RNode *p;
	RNode *qu[MAXSIZE];//建立队列
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//根节点入队

	while(front!=rear){
		front=(front+1)%MAXSIZE;//出队
		p=qu[front];
		for(i=1;i<p->keynum+1;i++)
			if(fwrite(&p->key[i],sizeof(READIDX),1,fp)!=1)//写入
			{ 
				printf("\n\t\t\t\t文件不能写入数据,请检查后重新运行.\n");
				getchar();
				return; 
			}	
		for(i=0;i<p->keynum;i++)
		{
			if(p->ptr[i])
			{
				rear=(rear+1)%MAXSIZE;//子节点入队
				qu[rear]=p->ptr[i];
			}
		}
	}
	fclose(fp);
	return ;
}
void R_ShowALL(RNode *t)//B-树遍历
{
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t编号\t姓名\n");
	RNode *p;
	RNode *qu[MAXSIZE];//建立队列
	RResult ret;
	ret.tag=false;
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//根节点入队	
	while(front!=rear){
		front=(front+1)%MAXSIZE;
		p=qu[front];		
		for(i=1;i<p->keynum+1;i++)
			printf("\n\t\t\t\t%d.\t%s\n",p->key[i].id,p->key[i].name);
		for(i=0;i<=p->keynum;i++)
		{
			if(p->ptr[i])
			{
				rear=(rear+1)%MAXSIZE;//子节点入队
				qu[rear]=p->ptr[i];
			}
		}
	}
	getchar();
}