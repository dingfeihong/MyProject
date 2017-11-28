#include "mainwindow.h"
#include "ui_mainwindow.h"
#include"algraph.h"
#include <QApplication>
#include"editwindow.h"
#include <QTextCodec>
#include<QMessageBox>
#include<QFileDialog>
#include<QMouseEvent>
QPushButton *bt=NULL;
int n=0;
//QPoint MousePress;
MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    //this->setWindowFlags(Qt::FramelessWindowHint);
    load(&G);    //载入图
    ui->setupUi(this);
    int i=0;
    ui->frame->installEventFilter(this);//设置事件

    QPushButton *btn[G.n];//button组

    //初始化所有button，设置坐标、字体等
    for(i = 0;i<G.n;i++){
        btn[i] = new QPushButton(G.adjlist[i].ndata.name,this);


        QFont Bfont("微软雅黑",9, QFont::Bold, false);
        btn[i]->setFont(Bfont);

        btn[i]->setStyleSheet("QPushButton { background-color: rgb(136, 136, 102); border-radius: 3px; color: rgb(255, 255, 255); }"
                                          "QPushButton:hover { background-color: rgb(170,170, 139);}"
                                          "QPushButton:pressed{border-image: url(button.png);}");
        btn[i]->setParent(ui->frame);

        btn[i]->setGeometry(G.adjlist[i].ndata.x,
                        G.adjlist[i].ndata.y,
                        G.adjlist[i].ndata.name.length()*15,
                        23);
        connect(btn[i],SIGNAL(clicked()),this,SLOT(introduct()));//链接槽函数
    }


    QString str = QString("QFrame#frame{border-image:url("+G.bgpic_path+")}");
    ui->frame->setStyleSheet(str);//设置地图图片

}

void MainWindow::addnode()//添加新顶点
{
    if (G.n>=MAX)//顶点数达到上限
    {
        QMessageBox::warning(NULL,"name","error",QMessageBox::Yes);
        return;
    }
    //保存景点名、信息、图片、坐标等信息
   QString name=ui->line_name->text();
   QString info=ui->text_info->toPlainText();
   QString path=ui->pic_path->text();
   QString x=ui->lineEdit_x->text();
   QString y=ui->lineEdit_y->text();

   //如果有空，警告并返回
   if (name.isEmpty()||info.isEmpty()
           ||path.isEmpty()
           ||x.isEmpty()||y.isEmpty())
   {
       QMessageBox::warning(NULL,"name","empty",QMessageBox::Yes);
       return;
   }

   if (FindNode(G,name)!=-1)
   {
       QMessageBox::warning(NULL,"name",name+"has exist",QMessageBox::Yes);
       return;
   }

   //判断路径是否为纯英文，不支持中文路径和特殊标记
   bool b = path.contains(QRegExp("[\\x4e00-\\x9fa5]+"));
   if(b)
   {
       QMessageBox::warning(NULL,"name","PATHERR",QMessageBox::Yes);
       return;
   }

   //创建新button并初始化button坐标、样式等信息
   QPushButton *pb = new QPushButton(name,this);
   //pb->setText(name);
   int length=name.length();
   QFont Bfont("微软雅黑",9, QFont::Bold, false);
   pb->setFont(Bfont);
   pb->setStyleSheet("QPushButton { background-color: rgb(136, 136, 102); border-radius: 3px; color: rgb(255, 255, 255); }"
                       "QPushButton:hover { background-color: rgb(170,170, 139);}"
                       "QPushButton:pressed{border-image: url(button.png);}");
   pb->setGeometry(x.toInt()-ui->frame->x(),y.toInt()-ui->frame->y()-10,length*15,23);
   pb->setParent(ui->frame);//设置父亲为frame
   connect(pb,SIGNAL(clicked()),this,SLOT(introduct()));//链接槽函数
   pb->show();

   AddNode(&G,name,info,x.toInt()-ui->frame->x(),y.toInt()-ui->frame->y()-10,path);
   Save(G);
   QMessageBox::warning(NULL,"name","success",QMessageBox::Yes);
   //this->close();
}

void MainWindow::addpath()//添加边
{

    //读取边起点终点坐标和长度等信息
   QString start=ui->line_start->text();
   QString final=ui->line_final->text();
   QString arc=ui->line_arc->text();
   QString length=ui->line_length->text();

   //有空信息，警告并返回
   if (start.isEmpty()||final.isEmpty()
           ||arc.isEmpty()
           ||length.isEmpty())
   {
       QMessageBox::warning(NULL,"name","empty",QMessageBox::Yes);
       return;
   }


   //调用函数在图结构添加边
   Add_Path(&G,start,final,arc,length.toInt());
   Save(G);//保存

   QMessageBox::warning(NULL,"name","success",QMessageBox::Yes);//提示成功
   return;
   //this->close();
}

void MainWindow::delnode()//删除景点
{
    QString name=ui->line_name->text();//获取景点名
    if (name.isEmpty()||bt==NULL)
    {
        QMessageBox::warning(NULL,"name","empty",QMessageBox::Yes);
        return;
    }
    //景点名和获取的button不匹配，退出
    if(bt->text()!=ui->line_name->text()) {
        QMessageBox::warning(NULL,"name","please press the button you want to delete",QMessageBox::Yes);
        return;
    }
    if(FindNode(G,ui->line_name->text())==-1) {
        QMessageBox::warning(NULL,"name","can't find the scence",QMessageBox::Yes);
        return;
    }

    bt->deleteLater();//button删除
    DelNode(&G,name);//图结构中删除顶点
    Save(G);
    QMessageBox::warning(NULL,"name","success",QMessageBox::Yes);
    //this->close();
}

void MainWindow::delpath()//删除边
{
    int i=0,m=-1,n=-1;
    //获取边起始点 、终点名
   QString start=ui->line_start_edit->text();
   QString final=ui->line_final_edit->text();

   //有空，返回退出
   if (start.isEmpty()||final.isEmpty())
   {
       QMessageBox::warning(NULL,"name","empty",QMessageBox::Yes);
       return;
   }

  // 遍历获取顶点编号
   for(i=0;i<G.n;i++){
       if(G.adjlist[i].ndata.name==start)
           m=i;
       if(G.adjlist[i].ndata.name==final)
           n=i;
   }

   if(m==-1||n==-1)
   {
       QMessageBox::warning(NULL,"name","can‘t find",QMessageBox::Yes);
       return;
   }

   //调用函数删除
   DelEdge(&G,m,n);
   Save(G);
   QMessageBox::warning(NULL,"name","success",QMessageBox::Yes);

}

void MainWindow::mousePressEvent(QMouseEvent *e)//读取鼠标单击位置坐标
{
   if(e->x()<2||e->y()<13||e->x()>749||e->y()>544)
       return;

   //转化为字符串，显示坐标到界面
   QString strx=QString::number(e->pos().x());
   QString stry= QString::number(e->pos().y());

   ui->lineEdit_x->setText(strx);
   ui->lineEdit_y->setText(stry);

}

bool MainWindow::eventFilter(QObject *, QEvent *evt)//地图移动函数
{
    static QPoint lastPnt;
    static bool isHover = false;
    if(evt->type() == QEvent::MouseButtonPress)//鼠标按下
    {
        QMouseEvent* e = static_cast<QMouseEvent*>(evt);
        if(ui->frame->rect().contains(e->pos()) && //is the mouse is clicking the key
            (e->button() == Qt::LeftButton)) //if the mouse click the right key
        {
            lastPnt = e->pos();//获取鼠标按下位置
            isHover = true;
        }
    }
    else if(evt->type() == QEvent::MouseMove && isHover)
    {
        QMouseEvent* e = static_cast<QMouseEvent*>(evt);
        int dx=ui->frame->x()+e->pos().x()-lastPnt.x();//鼠标按下时位置和当前位置差即frame移动位置
        int dy=ui->frame->y()+e->pos().y()-lastPnt.y();

        //判断边界
        if(dx>0)
            dx=0;
        if(dx<-1011)
            dx=-1011;
        if(dy>0)
            dy=0;
        if(dy<-750)
            dy=-750;
        ui->frame->move(dx,dy);//移动
    }

    else if(evt->type() == QEvent::MouseButtonRelease && isHover)
    {
        isHover = false;
    }
    return false;
}

void MainWindow::introduct()//介绍景点
{
    if (QPushButton* btn = dynamic_cast<QPushButton*>(sender())){//获取被单击的button
         QString text= btn->text();
         bt=btn;
         int i;
         i=FindNode(G,text);//查找景点
         if(i>=G.n) {
             QMessageBox::warning(NULL,"name","can not fin the node",QMessageBox::Yes);
             return;
         }
         //显示信息
         ui->text_intro->setText(G.adjlist[i].ndata.info);
         ui->text_info->setText(G.adjlist[i].ndata.info);

         //并把景点名显示与路径查找框
         if(n==0){
         ui->line_start->setText(text);
         ui->line_start_edit->setText(text);
         n=(n+1)%2;
         }

         else if(n==1){
         ui->line_final->setText(text);
         ui->line_final_edit->setText(text);
         n=(n+1)%2;
         }
         ui->line_start_2->setText(text);
         ui->line_name->setText(text);

         int x=btn->geometry().x(),y=btn->geometry().y();

         //显示坐标信息
         if(x>450)
            x=430;
         if(y>360)
            y=330;
         ui->lineEdit_x->setText(QString::number(x));
         ui->lineEdit_y->setText(QString::number(y));

         //显示景点图片
         QString str = QString("QFrame#img_frame{border-image:url("+G.adjlist[i].ndata.pic_path+")}");
         ui->img_frame->setStyleSheet(str);
         ui->pic_path->setText(G.adjlist[i].ndata.pic_path);
       }

}

void MainWindow::search()//路线查找
{
    QString start=ui->line_start->text();
    QString final=ui->line_final->text();
    //获取路线顶点信息
    if (final.isEmpty()&&start.isEmpty())
    {
        QMessageBox::warning(NULL,"name","empty",QMessageBox::Yes);
        return;
    }
    int m=FindNode(G,start),n=FindNode(G,final);//查找景点
    if(m==-1||n==-1){
        ui->text_path->setText("can not find the scenery");
        return;
    }

    ui->text_path->setText(FindPath(G,m,n));
}

void MainWindow::recommend()//推荐路线
{
    int i=0;
    int j=0;

    int Nodenum=0;
    QString text=ui->line_start_2->text();
    if (text.isEmpty())
    {
        QMessageBox::warning(NULL,"name","empty",QMessageBox::Yes);
        return;
    }
    int no=FindNode(G,text);
    if(no==-1){
        ui->text_path->setText("can not find the scenery");
        return;
    }
    if(no>=G.n)
        return;
    Signal sign;
    sign.stack_length=0;
    sign.arcStack_length=0;

    //初始化栈等信息
    for(i=0;i<G.n;i++)
        sign.visited[i]=0;
    int num=0;



  //  判断连通图路径
    Nodenum=part_num(G,no,&sign);
    if(Nodenum<=1){
         ui->text_path->setText(text);
         return;
    }

    for(j=0;j<G.n;j++)
        sign.visited[j]=0;

    Length_least(G,no,&num,Nodenum,&sign);

    i=1;
    //QString path;
    while(i<sign.stack_length){
        text+="->("+G.path[sign.arcStack[i-1]].name+")->"+G.adjlist[sign.stack[i]].ndata.name;
        i++;
    }
    ui->text_path->setText(text);//显示
}

void MainWindow::save_bg()//存储背景图
{
   QString path=ui->line_bg->text();//读取路径
   if (path.isEmpty() )
   {
       QMessageBox::warning(NULL,"name","empty",QMessageBox::Yes);
       return;
   }
   QString str = QString("QFrame#frame{border-image:url("+path+")}");
   ui->frame->setStyleSheet(str);//设置背景图
   G.bgpic_path=path;

   Save(G);//保存
}

void MainWindow::ret()//还原默认图片
{
   G.bgpic_path="pic/map.jpg";
   QString str = QString("QFrame#frame{border-image:url("+G.bgpic_path+")}");
   ui->frame->setStyleSheet(str);

   Save(G);
}

void MainWindow::add_pic()//添加景点照片
{
    QString fileName;
    //用打开窗口获取路径
    fileName = QFileDialog::getOpenFileName(this,tr("Open Config"),
             "./pic/", "Image files (*.bmp *.jpg *.pbm *.pgm *.png *.ppm *.xbm *.xpm);;All files (*.*)");


    if (fileName.isNull())
        return;
    ui->pic_path->setText(fileName);
}

void MainWindow::open_bg()//打开图片
{
    QString fileName;
    fileName = QFileDialog::getOpenFileName(this,tr("Open Config"),
             "./pic/", "Image files (*.bmp *.jpg *.pbm *.pgm *.png *.ppm *.xbm *.xpm);;All files (*.*)");


    if (fileName.isNull())
        return;
    ui->line_bg->setText(fileName);
}

void MainWindow::hide_f()
{
    QString str = QString("QFrame#img_frame{border-image:url(:/new/prefix1/pic/psu.jpg)}");
    ui->img_frame->setStyleSheet(str);
}

MainWindow::~MainWindow()
{
    delete ui;
}
