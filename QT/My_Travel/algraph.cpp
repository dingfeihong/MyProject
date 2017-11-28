
#include"algraph.h"
#include<qfile.h>
#include<qmessagebox.h>
#include <qtextstream.h>

int part_num(ALGraph g,int v,Signal *sign){//判断连通图顶点数
    int num=0;
    int stack[MAX];
    int top=0;
    ArcNode *p;

    //入栈顶点v,栈中存储的是顶点的下标
    stack[top]=v;
    top++;
    sign->visited[v]=1;
    num++;


    while(top!=0){
        //出栈栈顶元素
        top--;
        p=g.adjlist[stack[top]].firstarc;
        while(p){
            if(sign->visited[p->adjverx]==0){
                sign->visited[p->adjverx]=1;
                stack[top]=p->adjverx;
                top++;
                num++;
            }
            p=p->nextarc;
        }
    }
    return num;
}

void Length_least(ALGraph g,int v,int *num,int Nodenum,Signal *sign){//推荐路径

    if(*num==Nodenum)
        return;
    int n=0;//循环变量
    int i,j;

    if(sign->visited[v]==0){//未访问过的顶点
       sign->visited[v]=1;
       (*num)++;
    }

    //入栈
    sign->stack[sign->stack_length]=v;
    sign->stack_length++;

    ArcNode *p;
    p=g.adjlist[v].firstarc;

    ArcNode temp;
    ArcNode sort[MAX];//一个顶点的最大出度
    bool flag=true;

    while(p){
        sort[n].adjverx=p->adjverx;
        sort[n].id=p->id;
        sort[n].nextarc=p->nextarc;
        n++;
        if(sign->visited[p->adjverx]==0)
            flag=false;
        p=p->nextarc;
    }

    p=g.adjlist[v].firstarc;
    if(((flag==true)||(p&&p->nextarc==NULL))&&*num<Nodenum){
        sign->arcStack[sign->arcStack_length]=p->id;
        sign->arcStack_length++;
        sign->stack[sign->stack_length]=p->adjverx;
        sign->stack_length++;
        return;
    }

    //将与顶点相连的边根据长短排序
    for(j=0;j<n-1;j++)
       for(i=0;i<n-1-j;i++)
        {
           if(         (sign->visited[sort[i].adjverx]>sign->visited[sort[i+1].adjverx])
              || ((sign->visited[sort[i].adjverx]==sign->visited[sort[i+1].adjverx])
                  &&(g.path[sort[i].id].Distance>g.path[sort[i+1].id].Distance)))
            {
                temp=sort[i];
                sort[i]=sort[i+1];
                sort[i+1]=temp;
            }
        }

    for(int k=0;k<n;k++){
        sign->arcStack[sign->arcStack_length]=sort[k].id;
        sign->arcStack_length++;
        Length_least(g,sort[k].adjverx,num,Nodenum,sign);
    }
}

void DelEdge(ALGraph *G,int m,int n){//删除边
    int i=-1,j;
    if(m>=G->n||n>=G->n){//m n超出范围
        //QMessageBox::warning(NULL,"name","can't find",QMessageBox::Yes);
        return;
    }
    ANode *p=G->adjlist[m].firstarc;
    while(p){//查看邻接表中mn相连
        if(p->adjverx==n)
            i=p->id;
        p=p->nextarc;
    }

    if(i==-1){//假如不相连，警告
       // QMessageBox::warning(NULL,"name","can't find",QMessageBox::Yes);
        return;
    }
    else
        for(j=i;j<G->e;j++){//删除两点的边详细信息
            G->path[j].Distance=G->path[j+1].Distance;
            G->path[j].name=G->path[j+1].name;
        //G->path[i].Distance=0;
        }
    for(j=0;j<G->n;j++){
        p=G->adjlist[j].firstarc;
        while(p){
            if(p->id>=i)
                p->id-=1;
            p=p->nextarc;
        }
    }//边的编号--

    //邻接表中 链表中边关系删除
    ALDelete(G->adjlist[m].firstarc,n);G->adjlist[m].ndata.arcnum--;
    ALDelete(G->adjlist[n].firstarc,m);G->adjlist[n].ndata.arcnum--;
    G->e--;
}

void DelNode(ALGraph *G,QString name){//删除顶点
    int i,j,no;
      for(i=0;i<G->n;i++){
          if(G->adjlist[i].ndata.name==name)//找到名字为name的顶点编号
              break;
      }
    if(i>=G->n){//未找到
        QMessageBox::warning(NULL,"name","can't find",QMessageBox::Yes);
        return;
    }
    no=i;



    for(j=0;j<G->n;j++)//删除跟顶点j有关的边
       DelEdge(G,no,j);

    for(;i<G->n-1;i++){
        G->adjlist[i]=G->adjlist[i+1];//顶点信息删除
        if(G->adjlist[i].ndata.id>=no)
            G->adjlist[i].ndata.id-=1;
    }

    ANode *p;//边终点修改
    for(j=0;j<G->n;j++){
        p=G->adjlist[j].firstarc;
        while(p){
            if(p->adjverx>=no)
                p->adjverx-=1;
            p=p->nextarc;
        }
    }



    G->n--;
}

void Add_Path(ALGraph *G,QString start,QString final,QString arc,int distance){

    int m=0,n=0;
    int i=0;

    //找到名字为“start”的顶点编号
    for(i=0;i<G->n;i++)
        if(G->adjlist[i].ndata.name==start)
            m=i;
    if(G->adjlist[m].ndata.name!=start)
        return;

    //找到名字为“final”的顶点编号
    for(n=0,i=0;i<G->n;i++)
        if(G->adjlist[i].ndata.name==final)
            n=i;
    if(G->adjlist[n].ndata.name!=final)
        return;

    //如果自己给自己添加边
    if(m==n)
        return;

     //如果该边关系在邻接表里已有，则退出函数
    ArcNode *q=G->adjlist[m].firstarc;    
    while(q!=NULL){
        if(q->adjverx==n){
            return;
        }
        q=q->nextarc;
    }

    G->path[G->e].Distance=distance;
    G->path[G->e].name=arc;
    ALInsert(G->adjlist[m].firstarc,n,G->e);//调用插入函数添加边关系
    ALInsert(G->adjlist[n].firstarc,m,G->e);
    G->e++;

    G->adjlist[m].ndata.arcnum++;
    G->adjlist[n].ndata.arcnum++;

}

void ALDelete(ArcNode *&firstarc,int adjverx){//链表(边关系)的删除
    ArcNode *p=firstarc,*q;
    if(p&&p->adjverx==adjverx){//如果该顶点与x顶点有边关系，且该边关系存于firstarc
        q=p->nextarc;
        free(p);
        firstarc=q;
        return;
    }
    while(p){
        if(p&&p->nextarc&&p->nextarc->adjverx==adjverx){	//如果该顶点与x顶点有边关系
            q=p->nextarc;                           //删除该边关系
            p->nextarc=q->nextarc;
            free(q);
        }
        p=p->nextarc;
    }
}

void load(ALGraph *G)   //读取外存
{
    ArcNode *p,*q;
    QString temp;
    int i=0,j=0;
    QFile file(DATA);//定义QFile类型
         if(!file.open(QIODevice::ReadOnly | QIODevice::Text))
          {
             //QMessageBox::warning(NULL,"Warnning","can't open",QMessageBox::Yes);

          }
      QTextStream in(&file);


      //顶点和边关系读取
      temp=in.readLine();
      if(temp.isEmpty()){//如果读取不到，设置默认值
          G->e=0;
          G->n=0;
          G->bgpic_path="pic/map.jpg";
          return;
      }
      G->n=temp.toInt();
      temp=in.readLine();
      G->e=temp.toInt();

      G->bgpic_path=in.readLine();
      for(;i<G->n;i++){//读取顶点
         temp=in.readLine();
         G->adjlist[i].ndata.arcnum=temp.toInt();
         temp=in.readLine();
         G->adjlist[i].ndata.id=temp.toInt();
         temp=in.readLine();
         G->adjlist[i].ndata.x=temp.toInt();
         temp=in.readLine();
         G->adjlist[i].ndata.y=temp.toInt();
         G->adjlist[i].ndata.info=in.readLine();
         G->adjlist[i].ndata.name=in.readLine();
         G->adjlist[i].ndata.pic_path=in.readLine();
         G->adjlist[i].firstarc=NULL;
          if(G->adjlist[i].ndata.arcnum>0){//有边时，读取边
              //读取firstarc
              G->adjlist[i].firstarc=(ArcNode *)malloc(sizeof(ArcNode));
              G->adjlist[i].firstarc->nextarc=NULL;

              temp=in.readLine();
              G->adjlist[i].firstarc->id=temp.toInt();

              temp=in.readLine();
              G->adjlist[i].firstarc->adjverx=temp.toInt();

              p=G->adjlist[i].firstarc;
              for(j=1;j<G->adjlist[i].ndata.arcnum;j++){//读取其他边
                  q=(ArcNode *)malloc(sizeof(ArcNode));
                  temp=in.readLine();
                  q->id=temp.toInt();
                  temp=in.readLine();
                  q->adjverx=temp.toInt();
                  q->nextarc=p->nextarc;
                  p->nextarc=q;
              }
          }
      }
     file.close();



     i=0,j=0;
     QFile p_file(P_DATA);
           if(!p_file.open(QIODevice::ReadOnly | QIODevice::Text))
           {
              //QMessageBox::warning(NULL,"Warnning","can't open",QMessageBox::Yes);

           }
       QTextStream p_in(&p_file);

       //边详细信息读取
       for(i=0;i<G->e;i++){
                 temp=p_in.readLine();
                 G->path[i].Distance=temp.toInt();
                 G->path[i].name=p_in.readLine();
              }
      p_file.close();
    return ;
 }

void Save(ALGraph g)//存储
{
    ArcNode *p=NULL;
    int i=0,j=0;
    QFile file(DATA);
     if(!file.open(QIODevice::ReadWrite | QIODevice::Text))
     {
        QMessageBox::warning(NULL,"name","can't open",QMessageBox::Yes);
     }
   QTextStream in(&file);
   QString temp;
   //存储图的顶点数、边数、背景图
   temp=QString::number(g.n);
   in<<temp<<"\n";
   temp=QString::number(g.e);
   in<<temp<<"\n";
   in<<g.bgpic_path<<"\n";//背景图
   for(;i<g.n;i++){
       //存储顶点信息
       temp=QString::number(g.adjlist[i].ndata.arcnum);
       in<<temp<<"\n";
       temp=QString::number(g.adjlist[i].ndata.id);
       in<<temp<<"\n";
       temp=QString::number(g.adjlist[i].ndata.x);
       in<<temp<<"\n";
       temp=QString::number(g.adjlist[i].ndata.y);
       in<<temp<<"\n";
       in<<g.adjlist[i].ndata.info<<"\n";
       in<<g.adjlist[i].ndata.name<<"\n";
       in<<g.adjlist[i].ndata.pic_path<<"\n";
       //存储边信息
       if(g.adjlist[i].firstarc){       
           temp=QString::number(g.adjlist[i].firstarc->id);
           in<<temp<<"\n";
           temp=QString::number(g.adjlist[i].firstarc->adjverx);
           in<<temp<<"\n";
           p=g.adjlist[i].firstarc;
           for(j=1;j<g.adjlist[i].ndata.arcnum&&p->nextarc;j++){
               p=p->nextarc;
               temp=QString::number(p->id);
               in<<temp<<"\n";
               temp=QString::number(p->adjverx);
               in<<temp<<"\n";

           }
       }
   }
   file.close();

   //存储边详细信息
    i=j=0;
    QFile P_file(P_DATA);
    if(!P_file.open(QIODevice::ReadWrite | QIODevice::Text))
    {
       QMessageBox::warning(NULL,"name","can't open",QMessageBox::Yes);
    }
    QTextStream P_in(&P_file);

    for(;i<g.e;i++){
        temp=QString::number(g.path[i].Distance);
        P_in<<temp<<"\n";
        P_in<<g.path[i].name<<"\n";
    }
    P_file.close();
    return ;
}

void ALInsert(ArcNode *&firstarc,int adjverx,int id){ //链表(边关系)的插入


    ArcNode *add;
    if(!(add=(ArcNode *)malloc(sizeof(ArcNode)))){
        printf("\n\t新节点空间分配失败\n\n");          //存储分配失败
        return;}
    add->adjverx=adjverx;                              //插入
    add->id=id;
    ArcNode *p=firstarc;
    add->nextarc=p;
    firstarc=add;
}

void AddNode(ALGraph *G,QString name,QString info,int x,int y,QString path){//添加顶点
    G->adjlist[G->n].ndata.name=name;                           //数据域赋值
    G->adjlist[G->n].ndata.info=info;
    G->adjlist[G->n].ndata.pic_path=path;
    G->adjlist[G->n].ndata.x=x;
    G->adjlist[G->n].ndata.y=y;
    G->adjlist[G->n].ndata.id=G->n;
    G->adjlist[G->n].ndata.arcnum=0;
    G->adjlist[G->n].firstarc=NULL;                    //边关系置为NULL
    G->n++;                                            //顶点个数加1
}

int Dist(ALGraph g,int a,int b)//查找ab两点是否有边，有边返回权值，无边返回无穷
{
    ArcNode *p;
    p=g.adjlist[a].firstarc;
    while(p!=NULL)
    {
        if(p->adjverx==b)
            return g.path[p->id].Distance;
        else
            p=p->nextarc;
    }
    return INF;
}

void Distpath(ALGraph g,int path[],int S[],int start,int final,QString *Qpath)
{
    int j=0,k=0,m=0,n=0;
    ANode *p;
    int apath[MAX],d;
    *Qpath="路径为:";
    if(S[final]==1&&final!=start)
    {       
        d=0;
        apath[d]=final;
        k=path[final];
        if(k==-1)
            printf("\n");
        else
        {
            while(k!=start)
            {
                d++;
                apath[d]=k;
                k=path[k];
            }
            d++;
            apath[d]=start;
            *Qpath+=g.adjlist[start].ndata.name;//+"\n";
            m=start;
            for(j=d-1;j>=0;j--){
                n=apath[j];
                p=g.adjlist[m].firstarc;
                while(p){
                    if(p->adjverx==n){
                        *Qpath+="->("+g.path[p->id].name+")";//\n";
                        break;
                    }
                    p=p->nextarc;
                }
                m=n;
                *Qpath+="->"+g.adjlist[n].ndata.name;//+"\n";
            }
        }
         *Qpath+="\n";
    }
    else
        *Qpath="无此路径\n";
}

QString FindPath(ALGraph g,int start,int final)//查找两个景点之间的最短路径
{                                   //a b表示两个不同的景点
    int dist[MAX],path[MAX];
    int s[MAX];
    int mindis=INF,i=0,j=0,u=-1;
    if(g.e==0)
        return "无路可通";
    for(i=0;i<g.n;i++)
    {
        dist[i]=Dist(g,start,i);//距离初始化
        s[i]=0;              //s置空
        if(dist[i]<INF)     //路径初始化
            path[i]=start;      //顶点a到顶点i有边时，置顶点i前一个顶点为v
        else
            path[i]=-1;     //顶点a到顶点i没有边时，置顶点i前一个顶点为-1
    }
    s[start]=1;path[start]=0;       //源点编号v放入s中
    for(i=0;i<g.n;i++)
    {
        mindis=INF;
        for(j=0;j<g.n;j++)//找到距离最小的顶点
            if(s[j]==0&&dist[j]<mindis)
            {
                u=j;
                mindis=dist[j];
            }
        if(u==-1)
            return "无路可通";
        s[u]=1;
        for(j=0;j<g.n;j++)
            if(s[j]==0)
                if(Dist(g,u,j)<INF&&dist[u]+Dist(g,u,j)<dist[j])
                {
                    dist[j]=dist[u]+Dist(g,u,j);
                    path[j]=u;
                }
    }
    if(path[final]==-1)
        return "无路可通";
    QString Qpath;

     Distpath(g,path,s,start,final,&Qpath);
     return Qpath;
}

int FindNode(ALGraph g,QString name){
    int i=0;
    for(;i<g.n;i++){
        if(g.adjlist[i].ndata.name==name){           
            return i;
        }
    }
    return -1;
}

