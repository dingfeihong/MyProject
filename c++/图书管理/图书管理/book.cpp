#include "stdafx.h"
#include"liberary.h"
//�鼮����
int B_menu()//�˵�
{
	char s[80];
	int c;
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  �������\n");
	printf("\n\t\t\t\t2.  �鼮����\n");
	printf("\n\t\t\t\t3.  �������\n");
	printf("\n\t\t\t\t4.  ����ɾ��\n");
	printf("\n\t\t\t\t5.  ����\n");
	do{
		printf("\n\n\t\t\t\t��ѡ��");
		gets(s);
		printf("\n");
		c=atoi(s);
	}while(c<=0||c>5);
	return c;
}

void book(){//�鼮����������
	BNode *bookidx=(BNode *)malloc(sizeof(BNode));//����B-��
	char choice;
	bookidx=B_load();//��ȡ�����Ϣ
	while(1)
	{
		system("CLS");
		choice=B_menu();//�˵�
		switch(choice){
		case 1: B_add(bookidx);break;//����������Ϣ
		case 2: B_search(bookidx);break;//�鼮��ѯ
		case 3: B_updata(bookidx);break;//�鼮�¹���
	    case 4: bookidx=B_delete(bookidx);B_Del_Save(bookidx);break;//�鼮ɾ��
		case 5: return;
		}
	}
}
void B_show(BOOK head)   // ��ʾͨѶ¼����  
{
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t����:%-10s\n",head.name);
	printf("\n\t\t\t\t����:%-10s\n",head.author);
	printf("\n\t\t\t\t���:%-10s\n",head.classify);
	printf("\n\t\t\t\t�鼮���:%-10d\n",head.id);
	printf("\n\t\t\t\t�ݲ���:%-10d\n",head.all);
	printf("\n\t\t\t\tĿǰ�ڼ���:%-10d\n\n",head.exist);
	getchar();
 }
void B_search(BNode *t){
	system("CLS");
	int c;
	char choice=' ';
	char s[12];                        /////////////�˵�
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  �����鼮��Ų���\n");
	printf("\n\t\t\t\t2.  �����鼮���ֲ���\n");
	printf("\n\t\t\t\t3.  �����鼮�������\n");
	printf("\n\t\t\t\t4.  �����鼮���߲���\n");
	printf("\n\t\t\t\t5.  ��ʾ����ͼ��\n");
	printf("\n\t\t\t\t6.  ����\n");
	do{
		printf("\n\n\t\t\t\t��ѡ��");
		gets(s);
		printf("\n");
		c=atoi(s);
	}while(c<=0||c>6);

	BResult result;

	//long int num;
	switch(c)////��������ƫ��������
	{
	case 1: result=B_borrow1(t);break;//������
	case 2: result=B_borrow2(t);break;//������
	case 3:B_classify(t);return;
	case 4:B_author(t);return;
	case 5:B_ShowALL(t);return;
	case 6:return;
	}

	if(!result.tag){
		printf("\n\t\t\t\t�Ҳ����Ȿ��\n");
		getchar();
		return; 
	}	

	FILE *fp=fopen(Book,"rb");//����ȡ�鼮��Ϣ
	BOOK temp;
	if(!fseek(fp,result.pt->key[result.i].offset,0))
		fread(&temp,sizeof(BOOK),1,fp);
	fclose(fp);

	B_show(temp);	//չʾ
}
void B_updata(BNode *t)    /* ����������ͨѶ��ַ��  */
{
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	KeyType key;	
	printf("\n\t\t\t\t������Ҫ�����鼮�ı��:");	
	fflush(stdin);
	scanf("%d",&key);
	fflush(stdin);
	BResult result=B_find(t,key);//B-������
	if(!result.tag){
		printf("\n\t\t\t\tδ���ҵ�����\n");
		getchar();
		return;
	}

	BOOK temp;

	FILE *fp=fopen(Book,"rb");//���ļ�
	if(!fseek(fp,result.pt->key[result.i].offset,0))
		fread(&temp,sizeof(BOOK),1,fp);
	fclose(fp);
	B_show(temp);	

	int buy;
	fflush(stdin);
	printf("\n\t\t\t\t�������¹�����鼮����:");

	fflush(stdin);
	scanf("%d",&buy);
	fflush(stdin);

	temp.exist+=buy;//�޸��鼮��Ϣ
	temp.all+=buy;
	
	if ((fp=fopen(Book,"rb+"))==NULL)
	{
		printf("\n\t\t\t\t�ļ��޷�д��");
		getchar();
		return; 
	}	
	fseek(fp,result.pt->key[result.i].offset,SEEK_SET);		
	if(fwrite(&temp,sizeof(BOOK),1,fp)!=1)//д��
	{ 
		printf("\n\t\t\t\t�ļ�����д������,�������������.\n");		
		getchar();
		return; 
	}	
	fclose(fp);
}
void B_add(BNode *&t)//�������
{
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	BOOK temp;
	printf("\n\t\t\t\t����������:");      
	gets(temp.name);
	printf("\n\t\t\t\t����������:"); 
	gets(temp.author);
	printf("\n\t\t\t\t���������:"); 
	gets(temp.classify);
	printf("\n\t\t\t\t�������鼮���:");
	fflush(stdin);
	scanf("%d",&temp.id);
	fflush(stdin);
	printf("\n\t\t\t\t������ݲ���:");
	fflush(stdin);
	scanf("%d",&temp.all);
	temp.exist=temp.all;
	fflush(stdin);

	BResult search=B_find(t,temp.id);
	if(search.tag)
	{
		printf("\n\t\t\t\t�鼮����ظ�\n");
		getchar();
		return; 
	}	

	FILE *fp;
	if ((fp=fopen(Book,"ab"))==NULL)
	{
		printf("\n\t\t\t\t�ļ��޷�д��\n");
		getchar();
		return; 
	}	

	fseek(fp,0L,SEEK_END);		
	long length=ftell(fp);//����ƫ����
	fseek(fp,0L,SEEK_SET);

	if(fwrite(&temp,sizeof(BOOK),1,fp)!=1)
	{ 
		printf("\n\t\t\t\t�ļ�����д������,�������������.\n");		
		getchar();
		return; 
	}	
	fclose(fp);


	if ((fp=fopen(BookIdx,"ab"))==NULL)
	{
		printf("\n\t\t\t\t�ļ��޷�д��");
		getchar();
		return; 
	}

	BOOKIDX tnew;//����������Ϣ
	tnew.id=temp.id;
	tnew.offset=length;
	strcpy(tnew.name,temp.name);
	strcpy(tnew.classify,temp.classify);
	strcpy(tnew.author,temp.author);
	if(fwrite(&tnew,sizeof(BOOKIDX),1,fp)!=1)
	{ 
		printf("\n\t\t\t\t�ļ�����д������,�������������.\n");		
		getchar();
		return; 
	}	
	t=B_insert(t,tnew);//�����������
	fclose(fp );
	printf("\n\t\t\t\t��ӳɹ�\n");
	getchar();
}
void B_classify(BNode *t){//B-������
	char classify[20];
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t�������鼮����:");
	gets(classify);
	bool search=0;
	BNode *p;
	BNode *qu[MAXSIZE];//��������
	BResult ret;
	ret.tag=false;
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//���ڵ����
	printf("\n\t\t\t\t���\t�鼮\n");
	while(front!=rear){
		front=(front+1)%MAXSIZE;
		p=qu[front];
		for(i=1;i<p->keynum+1;i++)
			if(!strcmp(classify,p->key[i].classify)){//������ƥ�䣬���ҵ���������
				printf("\n\t\t\t\t%d.\t%s\n",p->key[i].id,p->key[i].name);
				search=1;
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
	if(!search)
		printf("\n\t\t\t\t�˷��������鼮\n");
	getchar();
}
void B_author(BNode *t){//B-������
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	char author[20];
	printf("\n\t\t\t\t����������:");
	gets(author);
	bool search=0;
	BNode *p;
	BNode *qu[MAXSIZE];//��������
	BResult ret;
	ret.tag=false;
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//���ڵ����
	printf("\n\t\t\t\t���\t�鼮\n");
	while(front!=rear){
		front=(front+1)%MAXSIZE;
		p=qu[front];
		for(i=1;i<p->keynum+1;i++)
			if(!strcmp(author,p->key[i].author)){//������ƥ�䣬���ҵ���������
				printf("\n\t\t\t\t%d.\t%s\n",p->key[i].id,p->key[i].name);
				search=1;
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
	if(!search)
		printf("\n\t\t\t\t�޴���������\n");
	getchar();
}
BNode *B_load(void)   //���ļ��е���������һ��ͨѶ¼��B��      
{ 
	 BNode *t;
	 BOOKIDX p;
	 FILE *fp;
	 t=(BNode *)malloc(sizeof(BNode));//;B-�����ڵ�
	 t->keynum=0,t->parent=NULL;
	 for(int i=0;i<MAX;i++)
		 t->ptr[i]=NULL,t->key[i].id=-1;
	 fp=fopen(BookIdx,"rb+");//������ļ�
	 if (fp==NULL){
		 printf("\n\t\t\t\tcan not open file %s\n", Book);
		 getchar();
		 return t;
	 }
	 else
	 { 
		 while(!feof(fp))
			 if(fread(&p,sizeof(BOOKIDX),1,fp)==1)//����B-��
				 t=B_insert(t,p);
		 fclose(fp);
		 return(t);
	 }
 }
BNode *B_insert(BNode *t,BOOKIDX p){  //ReaderIdx B���Ĳ���
	BResult r=B_find(t,p.id);       //�������ؼ�����B-����λ��
	if(r.tag)
		return t;
	int j=0;
	for(j=r.pt->keynum+1;j>r.i;j--)
		r.pt->key[j]=r.pt->key[j-1];  
	r.pt->key[r.i+1]=p;                 //�ؼ��ֲ���
	r.pt->keynum++;
	r.pt->ptr[r.pt->keynum]=NULL;
	if(r.pt->keynum>=MAX){           //���ڵ���
		BNode *tnew=(BNode *)malloc(sizeof(BNode));
		for(int i=0;i<MAX;i++)
			tnew->ptr[i]=NULL,tnew->key[i].id=-1;
		int n=(MAX+1)/2;
		for(j=n+1;j<=r.pt->keynum;j++)
			tnew->key[j-n]=r.pt->key[j];
		r.pt->keynum=tnew->keynum=r.pt->keynum-n;
		if(!r.pt->parent){            //���˫�׽ڵ�Ϊ�գ�����ǰΪ���ڵ�
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
BResult B_find(BNode *t,int key){//B-���йؼ��ֲ�ѯ
	BNode *q=NULL;
	BResult r;	
	r.i=0,r.pt=t,r.tag=0;
	while(r.pt&&!r.tag){
		r.i=0;
		for(;r.i<r.pt->keynum&&r.pt->key[r.i+1].id<=key;r.i++);
		if(r.i>0&&r.pt->key[r.i].id==key)//���ҵ�
			r.tag=1;
		else{//δ���ҵ�
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
	if(!p) return p;//��Ϊ�գ�ֱ�ӷ���

	BResult found=B_find(p,k);

	if(!found.tag) 
	{
		printf("���Ҳ������û�\n");//���Ҳ����û�
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
		// ɾ��Ҷ�ӽڵ��k��p��keynumС��min���ͽ��е���
		if(found.pt->keynum<(MAX-1)/2) 
			p=B_Restore(found.pt->parent,found.i);
	}
	return p;
}
BNode *B_IndexDel(BNode *root,int k)//����ɾ��
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
BNode *B_delete(BNode *t) //ɾ������    
{
	int k;
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	printf("\n\n\t\t\t\t������Ҫɾ��ͼ����:");
	fflush(stdin);
	scanf("%d",&k);
	fflush(stdin);
	
	char choice=' ';
	BResult search=B_find(t,k);
	if(!search.tag){
		printf("\n\t\t\t\tδ���ҵ�����\n");
		getchar();
		return t;
	}


	FILE *fp=fopen(Book,"rb");
	BOOK temp;
	if(!fseek(fp,search.pt->key[search.i].offset,0))//��ȡ��Ϣ
		fread(&temp,sizeof(BOOK),1,fp);
	fclose(fp);
	B_show(temp);	


	while(1){
		printf("\n\t\t\t\t�Ƿ�Ҫɾ���Ȿ�飿 (y\\n)\n");
		fflush(stdin);
		scanf("%c",&choice);
		fflush(stdin);
		if(choice=='n'||choice=='y')
			break;
	}	
	if(choice=='n')
		return t;


	t=B_IndexDel(t,k);

	printf("\n\t\t\t\tɾ���ɹ�\n");
	getchar();
	return t;
}

void B_InforDel(long int k)//�鼮��ϸ��Ϣ�ļ�ɾ��
{
	FILE *fp;
	if ((fp=fopen(Reader,"rb+"))==NULL)//�޸��鼮��Ϣ
	{
		printf("\n\t\t\t\t�ļ��޷�д��");
		getchar();
		return; 
	}
	fseek(fp,0L,SEEK_END);		
	long length=ftell(fp);//����ƫ����
	fseek(fp,k,SEEK_SET);		
	if(fwrite(&book,sizeof(BOOK),1,fp)!=1)
	{ 
		printf("\n\t\t\t\t�ļ�����д������,�������������.\n");		
		getchar();
		return; 
	}	
	fclose(fp);
}
void B_MOVleft(BNode *p,int i)
{
	int j;
	BNode *t, *x;
	// �����ڵ��i�Ƶ����ֵ���
	t = p->ptr[i-1];
	t->keynum++;
	t->key[t->keynum] = p->key[i];
	t->ptr[t->keynum] = p->ptr[i]->ptr[0];
	// �����ֵܵ�����ߵ�k�Ƶ����ڵ���
	t = p->ptr[i];
	x = p->ptr[i-1];
	p->key[i] = t->key[1];
	x->ptr[x->keynum] = t->ptr[1];
	// �����ֵܵ����йؼ�������һλ
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
void B_Del_Save(BNode *t)//ɾ����洢����
{
	FILE *fp;
	if ((fp=fopen(BookIdx,"wb"))==NULL)//�޸��鼮��Ϣ
	{
		printf("\n\t\t\t\t�ļ��޷�д��");
		getchar();
		return; 
	}
	
	BNode *p;
	BNode *qu[MAXSIZE];//��������
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//���ڵ����

	while(front!=rear){
		front=(front+1)%MAXSIZE;//����
		p=qu[front];
		for(i=1;i<p->keynum+1;i++)
			if(fwrite(&p->key[i],sizeof(BOOKIDX),1,fp)!=1)
			{ 
				printf("\n\t\t\t\t�ļ�����д������,�������������.\n");
				getchar();
				return; 
			}	
		for(i=0;i<p->keynum;i++)
		{
			if(p->ptr[i])
			{
				rear=(rear+1)%MAXSIZE;//�ӽڵ����
				qu[rear]=p->ptr[i];
			}
		}
	}
	fclose(fp);
	return ;
}
void B_ShowALL(BNode *t)//B-������
{
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	if(t->keynum==0)
		printf("\n\t\t\t\t���鼮\n");
	BNode *p;
	BNode *qu[MAXSIZE];//��������
	BResult ret;
	ret.tag=false;
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//���ڵ����
	printf("\n\t\t\t\t���\t�鼮\n");
	while(front!=rear){
		front=(front+1)%MAXSIZE;
		p=qu[front];
		for(i=1;i<p->keynum+1;i++)
			printf("\n\t\t\t\t%d.\t%s\n",p->key[i].id,p->key[i].name);
		for(i=0;i<=p->keynum;i++)
		{
			if(p->ptr[i])
			{
				rear=(rear+1)%MAXSIZE;//�ӽڵ����
				qu[rear]=p->ptr[i];
			}
		}
	}
	getchar();
}