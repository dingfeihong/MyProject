#include "stdafx.h"
#include"liberary.h"
//�û�ע��ע�� 

void reader(){ //�û��������˵�
	RNode *readidx=(RNode *)malloc(sizeof(RNode));
	char choice;
	readidx=R_load();//B������
	while(1)
	{
		system("CLS");
		choice=R_menu();//���ز˵�
		switch(choice){
		case 1: R_add(readidx);break;//������û�
	    case 2: R_search(readidx);break;//�û���ѯ
		case 3: readidx=R_delete(readidx);R_Del_Save(readidx);break;//�û�ɾ��
		case 4: R_ShowALL(readidx);break;//�û���ʾ
		case 5: return;//����
		}
	}
}
int R_menu()//�˵�
{
	char s[80];
	int c;
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  ���û�ע��\n");
	printf("\n\t\t\t\t2.  �û�����\n");
	printf("\n\t\t\t\t3.  �û�ע��\n");
	printf("\n\t\t\t\t4.  �û���ʾ\n");
	printf("\n\t\t\t\t5.  ����\n");
	do{
		printf("\n\n\t\t\t\t��ѡ��");
		gets(s);
		printf("\n");
		c=atoi(s);
	}while(c<0||c>5);
	return c;
}

void R_show(READER head)   // ��ʾ��������  
{
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	FILE *fp;
	if ((fp=fopen(Book,"rb"))==NULL)//��ʾ������Ϣ
	{
		printf("�ļ��޷�д��");
		getchar();
		return; 
	}
	BOOK book;
	printf("\n\t\t\t\t����:%-10s\n",head.name);
	printf("\n\t\t\t\t����:%-10s\n",head.job);
	printf("\n\t\t\t\t�Ա�:%-10s\n",head.sex);
	printf("\n\t\t\t\t֤������:%-10d\n",head.id);	
	if(head.booknum>0){
		printf("\n\t\t\t\t�û������飺\n");
		for(int i=0;i<head.booknum;i++){
			if(!fseek(fp,head.book[i],0))           //��ȡͼ����Ϣ
				fread(&book,sizeof(BOOK),1,fp);
			printf("\t\t\t\t%d.��%s��\n",i+1,book.name);
		}
	}
	getchar();
 }
void R_add(RNode *&t)
{
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	READER temp;
	printf("\n\n\t\t\t\t����������:");      
	gets(temp.name);
	printf("\n\n\t\t\t\t�����빤����λ:"); 
	gets(temp.job);
	fflush(stdin);
	printf("\n\n\t\t\t\t�������Ա�:");
	gets(temp.sex);
	printf("\n\n\t\t\t\t������֤������:");
	fflush(stdin);
	scanf("%d",&temp.id);
	fflush(stdin);
	temp.booknum=0;

	FILE *fp;

	RResult search=R_find(t,temp.id);
	if (search.tag)
	{
		printf("\n\t\t\t\t�鼮����ظ�\n");
		getchar();
		return; 
	}	

	if ((fp=fopen(Reader,"ab"))==NULL)
	{
		printf("\n\t\t\t\t�ļ��޷�д��");
		getchar();
		return; 
	}	

	fseek(fp,0L,SEEK_END);		
	long length=ftell(fp);//����ƫ����
	fseek(fp,0L,SEEK_SET);

	if(fwrite(&temp,sizeof(READER),1,fp)!=1)//д��
	{ 
		printf("\n\t\t\t\t�ļ��޷�д��");
		getchar();
		return; 
	}	
	fclose(fp);


	if ((fp=fopen(ReaderIdx,"ab"))==NULL)//������Ϣ�ļ�
	{
		printf("\n\t\t\t\t�ļ��޷�д��");
		getchar();
		return; 
	}

	READIDX tnew;//д��������Ϣ
	tnew.id=temp.id;
	tnew.offset=length;
	strcpy(tnew.name,temp.name);

	if(fwrite(&tnew,sizeof(READIDX),1,fp)!=1)//д��
	{ 
		printf("\n\t\t\t\t�ļ��޷�д��");
		getchar();
		return; 
	}	
	t=R_insert(t,tnew);//����B-��
	fclose(fp );
	printf("\n\t\t\t\tע��ɹ�\n");
	getchar();
}
void R_search(RNode *t){

	system("CLS");
	int c;
	char choice=' ';
	char s[12];                        /////////////�˵�
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t1.  ���ݱ�Ų���\n");
	printf("\n\t\t\t\t2.  ������������\n");
	printf("\n\t\t\t\t3.  ����\n");
	do{
		printf("\n\n\t\t\t\t��ѡ��");
		gets(s);
		printf("\n");
		c=atoi(s);
	}while(c<=0||c>3);

	RResult result;

	//long int num;
	switch(c)////��������ƫ��������
	{
	case 1: result=R_search1(t);break;//����Ŀ��
	case 2: result=R_search2(t);break;//����Ŀ���
	case 3:return;
	}

	if(!result.tag)
		return; 

	FILE *fp=fopen(Reader,"rb");
	READER user;
	if(!fseek(fp,result.pt->key[result.i].offset,0))//��ȡ��Ϣ
		fread(&user,sizeof(READER),1,fp);
	fclose(fp);
	R_show(user);	
}

RResult R_search1(RNode *t){//����Ų�ѯ
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	KeyType key;	
	printf("\n\t\t\t\t����������ID:");	
	fflush(stdin);
	scanf("%d",&key);
	fflush(stdin);
	RResult result=R_find(t,key);
	if(!result.tag){
		printf("\n\t\t\t\tδ���ҵ����û�\n");
		getchar();
	}
	return result;
}
RResult R_search2(RNode *t){//��������ѯ
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	char name[20];
	printf("\n\t\t\t\t����������:");
	gets(name);
	RResult ret;
	ret.tag=false;

	RNode *p;
	RNode *qu[MAXSIZE];//��������
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//���ڵ����

	while(front!=rear){
		front=(front+1)%MAXSIZE;//����
		p=qu[front];
		for(i=1;i<p->keynum+1;i++)
			if(!strcmp(name,p->key[i].name)){//���ҵ�������
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
	getchar();
	printf("\n\t\t\t\tδ���ҵ����û�\n");
	return ret;
}
RResult R_find(RNode *t,int key){//B-���йؼ��ֲ�ѯ
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

RNode *R_load(void)   //���ļ��е���������һ��ͨѶ¼��B��      
{ 
	 RNode *t;
	 READIDX p;
	 FILE *fp;
	 t=(RNode *)malloc(sizeof(RNode));//;B-�����ڵ�
	 t->keynum=0,t->parent=NULL;
	 for(int i=0;i<MAX;i++)
		 t->ptr[i]=NULL,t->key[i].id=-1;

	 fp=fopen(ReaderIdx,"rb+");//������ļ�
	 if (fp==NULL){
		 printf("\n\t\t\t\tcan not open file %s\n", Reader);
		 return t;
	 }
	 else
	 { 
		 while(!feof(fp))
			 if(fread(&p,sizeof(READIDX),1,fp)==1)//����B-��
				 t=R_insert(t,p);
		 fclose(fp);
		 return(t);
	 }
 }
RNode *R_delete(RNode *t)      
{
	int k;
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	printf("\n\n\t\t\t\t�������û�֤������:");
	fflush(stdin);
	scanf("%d",&k);
	fflush(stdin);
	
	char choice=' ';
	RResult search=R_find(t,k);
	if(!search.tag){
		printf("\n\t\t\t\tδ���ҵ����û�\n");
		getchar();
		return t;
	}

	FILE *fp=fopen(Reader,"rb");
	READER user;
	if(!fseek(fp,search.pt->key[search.i].offset,0))//��ȡ��Ϣ
		fread(&user,sizeof(READER),1,fp);
	fclose(fp);
	R_show(user);	


	while(1){
		printf("\n\t\t\t\t�Ƿ�Ҫע���� (y\\n):");
		fflush(stdin);
		scanf("%c",&choice);
		fflush(stdin);
		if(choice=='n'||choice=='y')
			break;
	}	
	if(choice=='n')
		return t;

	if(user.booknum>0){
		printf("\n\t\t\t\t���Ȱ�������黹\n");
		getchar();
		return t;
	}
	t=R_IndexDel(t,k);

	printf("\n\t\t\t\tע���ɹ�\n");
	getchar();
	return t;
}
RNode *R_insert(RNode *t,READIDX p){  //ReaderIdx B���Ĳ���
	RResult r=R_find(t,p.id);       //�������ؼ�����B-����λ��
	if(r.tag)
		return t;
	int j=0;
	for(j=r.pt->keynum+1;j>r.i;j--)
		r.pt->key[j]=r.pt->key[j-1];  
	r.pt->key[r.i+1]=p;                 //�ؼ��ֲ���
	r.pt->keynum++;
	r.pt->ptr[r.pt->keynum]=NULL;
	if(r.pt->keynum>=MAX){           //���ڵ���
		RNode *tnew=(RNode *)malloc(sizeof(RNode));
		for(int i=0;i<MAX;i++)
			tnew->ptr[i]=NULL,tnew->key[i].id=-1;
		int n=(MAX+1)/2;
		for(j=n+1;j<=r.pt->keynum;j++)
			tnew->key[j-n]=r.pt->key[j];
		r.pt->keynum=tnew->keynum=r.pt->keynum-n;
		if(!r.pt->parent){            //���˫�׽ڵ�Ϊ�գ�����ǰΪ���ڵ�
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
RNode *R_Restore(RNode *p,int i)//�ص���
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
RNode *R_Recdel(RNode *p,int k)//ɾ������
{
	if(!p) return p;//��Ϊ�գ�ֱ�ӷ���

	RResult found=R_find(p,k);//����
	if(!found.tag) 
	{
		printf("\n\t\t\t\t���Ҳ������û�\n");//���Ҳ����û�
		getchar();
		return p;
	}

	if(found.pt->ptr[found.i])//��ɾ���ڵ㲻��Ҷ��
	{
		RNode *q;
		for(q=found.pt->ptr[found.i];q->ptr[0];q=q->ptr[0]);//�ҵ���ǰ�ڵ�����С�ڵ�
		found.pt->key[found.i]=q->key[1];//�滻
		p=R_Recdel(found.pt->ptr[found.i],found.pt->key[found.i].id);//�ݹ�ɾ��
	}
	else //��ǰΪҶ�ӽڵ�
	{
		int j;
		for(j=found.i;j<=found.pt->keynum;++j)//ɾ���ýڵ�
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
		if(found.pt->keynum<(MAX-1)/2) //ɾ��Ҷ�ӽڵ��k��p��keynumС��min���ͽ��е���
			p=R_Restore(found.pt->parent,found.i);
	}
	return p;
}
RNode *R_IndexDel(RNode *root,int k)//����B-��ɾ��
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
	if ((fp=fopen(Reader,"rb+"))==NULL)//�޸���Ŀ��Ϣ
	{
		printf("\n\t\t\t\t�ļ��޷�д��");
		getchar;
		return; 
	}
	fseek(fp,0L,SEEK_END);		
	long length=ftell(fp);//����ƫ����
	fseek(fp,k,SEEK_SET);		
	if(fwrite(&book,sizeof(BOOK),1,fp)!=1)
	{ 
		printf("\n\t\t\t\t�ļ�����д������,�������������.\n");		
		getchar;
		return; 
	}	
	fclose(fp);
}
void R_MOVleft(RNode *p,int i)//�����
{
	int j;
	RNode *t, *x;	
	t=p->ptr[i-1];//�����ڵ��i�Ƶ����ֵ���
	t->keynum++;
	t->key[t->keynum]=p->key[i];
	t->ptr[t->keynum]=p->ptr[i]->ptr[0];	
	t=p->ptr[i];// �����ֵܵ�����ߵ�k�Ƶ����ڵ���
	x=p->ptr[i-1];
	p->key[i]=t->key[1];
	x->ptr[x->keynum]=t->ptr[1];	
	t->keynum--;// �����ֵܵ����йؼ�������һλ
	for(j=1;j<=t->keynum;++j)
	{
		t->key[j]=t->key[j+1];
		t->ptr[j]=t->ptr[j+1];
	}
}
void R_MOVright(RNode *p,int i)//�ҵ���
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
void R_Del_Save(RNode *t)//ɾ����洢���������
{
	FILE *fp;
	if ((fp=fopen(ReaderIdx,"wb"))==NULL)//�޸���Ŀ��Ϣ
	{
		printf("\n\t\t\t\t�ļ��޷�д��");
		getchar();
		return; 
	}
	
	RNode *p;
	RNode *qu[MAXSIZE];//��������
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//���ڵ����

	while(front!=rear){
		front=(front+1)%MAXSIZE;//����
		p=qu[front];
		for(i=1;i<p->keynum+1;i++)
			if(fwrite(&p->key[i],sizeof(READIDX),1,fp)!=1)//д��
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
void R_ShowALL(RNode *t)//B-������
{
	system("CLS");
	printf("\n\n\t\t\t\tͼ�����ϵͳ\n");
	printf("\t\t=============================================\n");
	printf("\n\t\t\t\t���\t����\n");
	RNode *p;
	RNode *qu[MAXSIZE];//��������
	RResult ret;
	ret.tag=false;
	int front=-1,rear=-1;
	int i=0;
	rear++;
	qu[rear]=t;	//���ڵ����	
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