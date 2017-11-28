#include "stdafx.h"
#include"liberary.h"
//书籍管理
int B_menu()//菜单
{
	char s[80];
	int c;
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  新书入库\n");
	printf("\n\t\t\t\t2.  书籍查找\n");
	printf("\n\t\t\t\t3.  旧书更新\n");
	printf("\n\t\t\t\t4.  旧书删除\n");
	printf("\n\t\t\t\t5.  返回\n");
	do{
		printf("\n\n\t\t\t\t请选择：");
		gets(s);
		printf("\n");
		c=atoi(s);
	}while(c<=0||c>5);
	return c;
}

void book(){//书籍管理主函数
	BNode *bookidx=(BNode *)malloc(sizeof(BNode));//建立B-树
	char choice;
	bookidx=B_load();//读取外存信息
	while(1)
	{
		system("CLS");
		choice=B_menu();//菜单
		switch(choice){
		case 1: B_add(bookidx);break;//插入新书信息
		case 2: B_search(bookidx);break;//书籍查询
		case 3: B_updata(bookidx);break;//书籍新购入
	    case 4: bookidx=B_delete(bookidx);B_Del_Save(bookidx);break;//书籍删除
		case 5: return;
		}
	}
}
void B_show(BOOK head)   // 显示通讯录内容  
{
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t书名:%-10s\n",head.name);
	printf("\n\t\t\t\t作者:%-10s\n",head.author);
	printf("\n\t\t\t\t类别:%-10s\n",head.classify);
	printf("\n\t\t\t\t书籍编号:%-10d\n",head.id);
	printf("\n\t\t\t\t馆藏量:%-10d\n",head.all);
	printf("\n\t\t\t\t目前在架上:%-10d\n\n",head.exist);
	getchar();
 }
void B_search(BNode *t){
	system("CLS");
	int c;
	char choice=' ';
	char s[12];                        /////////////菜单
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  根据书籍编号查找\n");
	printf("\n\t\t\t\t2.  根据书籍名字查找\n");
	printf("\n\t\t\t\t3.  根据书籍分类查找\n");
	printf("\n\t\t\t\t4.  根据书籍作者查找\n");
	printf("\n\t\t\t\t5.  显示所有图书\n");
	printf("\n\t\t\t\t6.  返回\n");
	do{
		printf("\n\n\t\t\t\t请选择：");
		gets(s);
		printf("\n");
		c=atoi(s);
	}while(c<=0||c>6);

	BResult result;

	//long int num;
	switch(c)////所借数的偏移量借书
	{
	case 1: result=B_borrow1(t);break;//按书编号
	case 2: result=B_borrow2(t);break;//按书名
	case 3:B_classify(t);return;
	case 4:B_author(t);return;
	case 5:B_ShowALL(t);return;
	case 6:return;
	}

	if(!result.tag){
		printf("\n\t\t\t\t找不到这本书\n");
		getchar();
		return; 
	}	

	FILE *fp=fopen(Book,"rb");//外存读取书籍信息
	BOOK temp;
	if(!fseek(fp,result.pt->key[result.i].offset,0))
		fread(&temp,sizeof(BOOK),1,fp);
	fclose(fp);

	B_show(temp);	//展示
}
void B_updata(BNode *t)    /* 按姓名查找通讯地址等  */
{
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	KeyType key;	
	printf("\n\t\t\t\t请输入要更新书籍的编号:");	
	fflush(stdin);
	scanf("%d",&key);
	fflush(stdin);
	BResult result=B_find(t,key);//B-树搜索
	if(!result.tag){
		printf("\n\t\t\t\t未查找到该书\n");
		getchar();
		return;
	}

	BOOK temp;

	FILE *fp=fopen(Book,"rb");//打开文件
	if(!fseek(fp,result.pt->key[result.i].offset,0))
		fread(&temp,sizeof(BOOK),1,fp);
	fclose(fp);
	B_show(temp);	

	int buy;
	fflush(stdin);
	printf("\n\t\t\t\t请输入新购买的书籍数量:");

	fflush(stdin);
	scanf("%d",&buy);
	fflush(stdin);

	temp.exist+=buy;//修改书籍信息
	temp.all+=buy;
	
	if ((fp=fopen(Book,"rb+"))==NULL)
	{
		printf("\n\t\t\t\t文件无法写入");
		getchar();
		return; 
	}	
	fseek(fp,result.pt->key[result.i].offset,SEEK_SET);		
	if(fwrite(&temp,sizeof(BOOK),1,fp)!=1)//写入
	{ 
		printf("\n\t\t\t\t文件不能写入数据,请检查后重新运行.\n");		
		getchar();
		return; 
	}	
	fclose(fp);
}
void B_add(BNode *&t)//新书插入
{
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	BOOK temp;
	printf("\n\t\t\t\t请输入书名:");      
	gets(temp.name);
	printf("\n\t\t\t\t请输入作者:"); 
	gets(temp.author);
	printf("\n\t\t\t\t请输入分类:"); 
	gets(temp.classify);
	printf("\n\t\t\t\t请输入书籍编号:");
	fflush(stdin);
	scanf("%d",&temp.id);
	fflush(stdin);
	printf("\n\t\t\t\t请输入馆藏量:");
	fflush(stdin);
	scanf("%d",&temp.all);
	temp.exist=temp.all;
	fflush(stdin);

	BResult search=B_find(t,temp.id);
	if(search.tag)
	{
		printf("\n\t\t\t\t书籍编号重复\n");
		getchar();
		return; 
	}	

	FILE *fp;
	if ((fp=fopen(Book,"ab"))==NULL)
	{
		printf("\n\t\t\t\t文件无法写入\n");
		getchar();
		return; 
	}	

	fseek(fp,0L,SEEK_END);		
	long length=ftell(fp);//计算偏移量
	fseek(fp,0L,SEEK_SET);

	if(fwrite(&temp,sizeof(BOOK),1,fp)!=1)
	{ 
		printf("\n\t\t\t\t文件不能写入数据,请检查后重新运行.\n");		
		getchar();
		return; 
	}	
	fclose(fp);


	if ((fp=fopen(BookIdx,"ab"))==NULL)
	{
		printf("\n\t\t\t\t文件无法写入");
		getchar();
		return; 
	}

	BOOKIDX tnew;//建立索引信息
	tnew.id=temp.id;
	tnew.offset=length;
	strcpy(tnew.name,temp.name);
	strcpy(tnew.classify,temp.classify);
	strcpy(tnew.author,temp.author);
	if(fwrite(&tnew,sizeof(BOOKIDX),1,fp)!=1)
	{ 
		printf("\n\t\t\t\t文件不能写入数据,请检查后重新运行.\n");		
		getchar();
		return; 
	}	
	t=B_insert(t,tnew);//新书插入索引
	fclose(fp );
	printf("\n\t\t\t\t添加成功\n");
	getchar();
}
void B_classify(BNode *t){//B-树遍历
	char classify[20];
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t请输入书籍类型:");
	gets(classify);
	bool search=0;
	BNode *p;
	BNode *qu[MAXSIZE];//建立队列
	BResult ret;
	ret.tag=false;
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//根节点入队
	printf("\n\t\t\t\t编号\t书籍\n");
	while(front!=rear){
		front=(front+1)%MAXSIZE;
		p=qu[front];
		for(i=1;i<p->keynum+1;i++)
			if(!strcmp(classify,p->key[i].classify)){//当姓名匹配，查找到，并返回
				printf("\n\t\t\t\t%d.\t%s\n",p->key[i].id,p->key[i].name);
				search=1;
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
	if(!search)
		printf("\n\t\t\t\t此分类下无书籍\n");
	getchar();
}
void B_author(BNode *t){//B-树遍历
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	char author[20];
	printf("\n\t\t\t\t请输入作者:");
	gets(author);
	bool search=0;
	BNode *p;
	BNode *qu[MAXSIZE];//建立队列
	BResult ret;
	ret.tag=false;
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//根节点入队
	printf("\n\t\t\t\t编号\t书籍\n");
	while(front!=rear){
		front=(front+1)%MAXSIZE;
		p=qu[front];
		for(i=1;i<p->keynum+1;i++)
			if(!strcmp(author,p->key[i].author)){//当姓名匹配，查找到，并返回
				printf("\n\t\t\t\t%d.\t%s\n",p->key[i].id,p->key[i].name);
				search=1;
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
	if(!search)
		printf("\n\t\t\t\t无此作者著作\n");
	getchar();
}
BNode *B_load(void)   //由文件中的数据生成一个通讯录的B树      
{ 
	 BNode *t;
	 BOOKIDX p;
	 FILE *fp;
	 t=(BNode *)malloc(sizeof(BNode));//;B-树根节点
	 t->keynum=0,t->parent=NULL;
	 for(int i=0;i<MAX;i++)
		 t->ptr[i]=NULL,t->key[i].id=-1;
	 fp=fopen(BookIdx,"rb+");//打开外存文件
	 if (fp==NULL){
		 printf("\n\t\t\t\tcan not open file %s\n", Book);
		 getchar();
		 return t;
	 }
	 else
	 { 
		 while(!feof(fp))
			 if(fread(&p,sizeof(BOOKIDX),1,fp)==1)//生成B-树
				 t=B_insert(t,p);
		 fclose(fp);
		 return(t);
	 }
 }
BNode *B_insert(BNode *t,BOOKIDX p){  //ReaderIdx B树的插入
	BResult r=B_find(t,p.id);       //求出插入关键字在B-树的位置
	if(r.tag)
		return t;
	int j=0;
	for(j=r.pt->keynum+1;j>r.i;j--)
		r.pt->key[j]=r.pt->key[j-1];  
	r.pt->key[r.i+1]=p;                 //关键字插入
	r.pt->keynum++;
	r.pt->ptr[r.pt->keynum]=NULL;
	if(r.pt->keynum>=MAX){           //当节点满
		BNode *tnew=(BNode *)malloc(sizeof(BNode));
		for(int i=0;i<MAX;i++)
			tnew->ptr[i]=NULL,tnew->key[i].id=-1;
		int n=(MAX+1)/2;
		for(j=n+1;j<=r.pt->keynum;j++)
			tnew->key[j-n]=r.pt->key[j];
		r.pt->keynum=tnew->keynum=r.pt->keynum-n;
		if(!r.pt->parent){            //如果双亲节点为空，即当前为根节点
			BNode *tnew2=(BNode *)malloc(sizeof(BNode));
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
			B_insert(tnew->parent,r.pt->key[n]);
			tnew->parent->ptr[tnew->parent->keynum]=tnew;
		}
	}	
	return t;
}
BResult B_find(BNode *t,int key){//B-树中关键字查询
	BNode *q=NULL;
	BResult r;	
	r.i=0,r.pt=t,r.tag=0;
	while(r.pt&&!r.tag){
		r.i=0;
		for(;r.i<r.pt->keynum&&r.pt->key[r.i+1].id<=key;r.i++);
		if(r.i>0&&r.pt->key[r.i].id==key)//查找到
			r.tag=1;
		else{//未查找到
			q=r.pt;
			r.pt=r.pt->ptr[r.i];
		}		
	}
	if(!r.tag)
		r.pt=q;
	return r;
}

BNode *B_Combine(BNode *p,int i)
{
	int j;
	BNode *q = p->ptr[i];
	BNode *l = p->ptr[i-1];
	l->keynum++;
	l->key[l->keynum] = p->key[i];
	l->ptr[l->keynum] = q->ptr[0];
	for(j = 1; j <= q->keynum; ++j)
	{
		l->keynum++;
		l->key[l->keynum] = q->key[j];
		l->ptr[l->keynum] = q->ptr[j];
	}
	p->keynum--;
	for(j = i; j <= p->keynum; ++j)
	{
		p->key[j] = p->key[j+1];
		p->ptr[j] = p->ptr[j+1];
	}
	free(q);

	if(!p->keynum)
		p=l;

	return p;
}
BNode *B_Restore(BNode *p,int i)
{
	int keynum = p->keynum;
	if(i == 0)
	{
		if(p->ptr[1]->keynum >(MAX-1)/2) 
			B_MOVleft(p, 1);
		else 
			p=B_Combine(p, 1);
	}
	else if(i == keynum)
	{
		if(p->ptr[i-1]->keynum >(MAX-1)/2) 
			B_MOVright(p, i);
		else 
			p=B_Combine(p, i);
	}
	else if(p->ptr[i-1]->keynum >(MAX-1)/2) 
		B_MOVright(p, i);
	else if(p->ptr[i+1]->keynum >(MAX-1)/2) 
		B_MOVleft(p, i+1);
	else 
		p=B_Combine(p, i);
	return p;
}
BNode *B_Recdel(BNode *p,int k)
{
	if(!p) return p;//树为空，直接返回

	BResult found=B_find(p,k);

	if(!found.tag) 
	{
		printf("查找不到该用户\n");//查找不到用户
		getchar();
		return p;
	}

	if(found.pt->ptr[found.i]!=NULL)
	{
		BNode *q;
		for(q=found.pt->ptr[found.i];q->ptr[0];q=q->ptr[0]);
		found.pt->key[found.i]=q->key[1];
		B_Recdel(found.pt->ptr[found.i],found.pt->key[found.i].id);
	}		
	else 
	{
		int j;
		for(j=found.i;j<=found.pt->keynum; ++j)
		{
			found.pt->key[j]=found.pt->key[j+1];		
			found.pt->ptr[j]=found.pt->ptr[j+1];
		}
		found.pt->keynum--;
	}
	//else found.tag = B_Recdel(p->ptr[found.i], k);

	if(found.pt->parent)
	{
		for(found.i=0;found.i<=found.pt->parent->keynum;++found.i)
		{
			if(found.pt->parent->ptr[found.i]==found.pt) break;
		}
		// 删完叶子节点的k后，p的keynum小于min，就进行调整
		if(found.pt->keynum<(MAX-1)/2) 
			p=B_Restore(found.pt->parent,found.i);
	}
	return p;
}
BNode *B_IndexDel(BNode *root,int k)//索引删除
{
	BNode *p;
	root=B_Recdel(root, k);
	if(!root->keynum&&root->ptr[0])
	{
		p=root;
		root=root->ptr[0];
		free(p);
	}
	return root;
}
BNode *B_delete(BNode *t) //删除函数    
{
	int k;
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	printf("\n\n\t\t\t\t请输入要删除图书编号:");
	fflush(stdin);
	scanf("%d",&k);
	fflush(stdin);
	
	char choice=' ';
	BResult search=B_find(t,k);
	if(!search.tag){
		printf("\n\t\t\t\t未查找到该书\n");
		getchar();
		return t;
	}


	FILE *fp=fopen(Book,"rb");
	BOOK temp;
	if(!fseek(fp,search.pt->key[search.i].offset,0))//读取信息
		fread(&temp,sizeof(BOOK),1,fp);
	fclose(fp);
	B_show(temp);	


	while(1){
		printf("\n\t\t\t\t是否要删除这本书？ (y\\n)\n");
		fflush(stdin);
		scanf("%c",&choice);
		fflush(stdin);
		if(choice=='n'||choice=='y')
			break;
	}	
	if(choice=='n')
		return t;


	t=B_IndexDel(t,k);

	printf("\n\t\t\t\t删除成功\n");
	getchar();
	return t;
}

void B_InforDel(long int k)//书籍详细信息文件删除
{
	FILE *fp;
	if ((fp=fopen(Reader,"rb+"))==NULL)//修改书籍信息
	{
		printf("\n\t\t\t\t文件无法写入");
		getchar();
		return; 
	}
	fseek(fp,0L,SEEK_END);		
	long length=ftell(fp);//计算偏移量
	fseek(fp,k,SEEK_SET);		
	if(fwrite(&book,sizeof(BOOK),1,fp)!=1)
	{ 
		printf("\n\t\t\t\t文件不能写入数据,请检查后重新运行.\n");		
		getchar();
		return; 
	}	
	fclose(fp);
}
void B_MOVleft(BNode *p,int i)
{
	int j;
	BNode *t, *x;
	// 将父节点的i移到左兄弟里
	t = p->ptr[i-1];
	t->keynum++;
	t->key[t->keynum] = p->key[i];
	t->ptr[t->keynum] = p->ptr[i]->ptr[0];
	// 将右兄弟的最左边的k移到父节点里
	t = p->ptr[i];
	x = p->ptr[i-1];
	p->key[i] = t->key[1];
	x->ptr[x->keynum] = t->ptr[1];
	// 将右兄弟的所有关键字左移一位
	t->keynum--;
	for(j = 1; j <= t->keynum; ++j)
	{
		t->key[j] = t->key[j+1];
		t->ptr[j] = t->ptr[j+1];
	}
}
void B_MOVright(BNode *p,int i)
{
	int j;
	BNode *t;
	t = p->ptr[i];
	for(j = t->keynum; j >= 1; --j)
	{
		t->key[j+1] = t->key[j];
		t->ptr[j+1] = t->ptr[j];
	}
	t->ptr[1] = t->ptr[0];
	t->keynum++;
	t->key[1] = p->key[i];

	t = p->ptr[i-1];
	p->key[i] = t->key[t->keynum];
	p->ptr[i]->ptr[0] = t->ptr[t->keynum];
	t->keynum--;
}
void B_Del_Save(BNode *t)//删除后存储索引
{
	FILE *fp;
	if ((fp=fopen(BookIdx,"wb"))==NULL)//修改书籍信息
	{
		printf("\n\t\t\t\t文件无法写入");
		getchar();
		return; 
	}
	
	BNode *p;
	BNode *qu[MAXSIZE];//建立队列
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//根节点入队

	while(front!=rear){
		front=(front+1)%MAXSIZE;//出队
		p=qu[front];
		for(i=1;i<p->keynum+1;i++)
			if(fwrite(&p->key[i],sizeof(BOOKIDX),1,fp)!=1)
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
void B_ShowALL(BNode *t)//B-树遍历
{
	system("CLS");
	printf("\n\n\t\t\t\t图书管理系统\n");
	printf("\t\t=============================================\n");
	if(t->keynum==0)
		printf("\n\t\t\t\t无书籍\n");
	BNode *p;
	BNode *qu[MAXSIZE];//建立队列
	BResult ret;
	ret.tag=false;
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//根节点入队
	printf("\n\t\t\t\t编号\t书籍\n");
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