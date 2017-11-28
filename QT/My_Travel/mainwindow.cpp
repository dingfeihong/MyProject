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
    load(&G);    //����ͼ
    ui->setupUi(this);
    int i=0;
    ui->frame->installEventFilter(this);//�����¼�

    QPushButton *btn[G.n];//button��

    //��ʼ������button���������ꡢ�����
    for(i = 0;i<G.n;i++){
        btn[i] = new QPushButton(G.adjlist[i].ndata.name,this);


        QFont Bfont("΢���ź�",9, QFont::Bold, false);
        btn[i]->setFont(Bfont);

        btn[i]->setStyleSheet("QPushButton { background-color: rgb(136, 136, 102); border-radius: 3px; color: rgb(255, 255, 255); }"
                                          "QPushButton:hover { background-color: rgb(170,170, 139);}"
                                          "QPushButton:pressed{border-image: url(button.png);}");
        btn[i]->setParent(ui->frame);

        btn[i]->setGeometry(G.adjlist[i].ndata.x,
                        G.adjlist[i].ndata.y,
                        G.adjlist[i].ndata.name.length()*15,
                        23);
        connect(btn[i],SIGNAL(clicked()),this,SLOT(introduct()));//���Ӳۺ���
    }


    QString str = QString("QFrame#frame{border-image:url("+G.bgpic_path+")}");
    ui->frame->setStyleSheet(str);//���õ�ͼͼƬ

}

void MainWindow::addnode()//����¶���
{
    if (G.n>=MAX)//�������ﵽ����
    {
        QMessageBox::warning(NULL,"name","error",QMessageBox::Yes);
        return;
    }
    //���澰��������Ϣ��ͼƬ���������Ϣ
   QString name=ui->line_name->text();
   QString info=ui->text_info->toPlainText();
   QString path=ui->pic_path->text();
   QString x=ui->lineEdit_x->text();
   QString y=ui->lineEdit_y->text();

   //����пգ����沢����
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

   //�ж�·���Ƿ�Ϊ��Ӣ�ģ���֧������·����������
   bool b = path.contains(QRegExp("[\\x4e00-\\x9fa5]+"));
   if(b)
   {
       QMessageBox::warning(NULL,"name","PATHERR",QMessageBox::Yes);
       return;
   }

   //������button����ʼ��button���ꡢ��ʽ����Ϣ
   QPushButton *pb = new QPushButton(name,this);
   //pb->setText(name);
   int length=name.length();
   QFont Bfont("΢���ź�",9, QFont::Bold, false);
   pb->setFont(Bfont);
   pb->setStyleSheet("QPushButton { background-color: rgb(136, 136, 102); border-radius: 3px; color: rgb(255, 255, 255); }"
                       "QPushButton:hover { background-color: rgb(170,170, 139);}"
                       "QPushButton:pressed{border-image: url(button.png);}");
   pb->setGeometry(x.toInt()-ui->frame->x(),y.toInt()-ui->frame->y()-10,length*15,23);
   pb->setParent(ui->frame);//���ø���Ϊframe
   connect(pb,SIGNAL(clicked()),this,SLOT(introduct()));//���Ӳۺ���
   pb->show();

   AddNode(&G,name,info,x.toInt()-ui->frame->x(),y.toInt()-ui->frame->y()-10,path);
   Save(G);
   QMessageBox::warning(NULL,"name","success",QMessageBox::Yes);
   //this->close();
}

void MainWindow::addpath()//��ӱ�
{

    //��ȡ������յ�����ͳ��ȵ���Ϣ
   QString start=ui->line_start->text();
   QString final=ui->line_final->text();
   QString arc=ui->line_arc->text();
   QString length=ui->line_length->text();

   //�п���Ϣ�����沢����
   if (start.isEmpty()||final.isEmpty()
           ||arc.isEmpty()
           ||length.isEmpty())
   {
       QMessageBox::warning(NULL,"name","empty",QMessageBox::Yes);
       return;
   }


   //���ú�����ͼ�ṹ��ӱ�
   Add_Path(&G,start,final,arc,length.toInt());
   Save(G);//����

   QMessageBox::warning(NULL,"name","success",QMessageBox::Yes);//��ʾ�ɹ�
   return;
   //this->close();
}

void MainWindow::delnode()//ɾ������
{
    QString name=ui->line_name->text();//��ȡ������
    if (name.isEmpty()||bt==NULL)
    {
        QMessageBox::warning(NULL,"name","empty",QMessageBox::Yes);
        return;
    }
    //�������ͻ�ȡ��button��ƥ�䣬�˳�
    if(bt->text()!=ui->line_name->text()) {
        QMessageBox::warning(NULL,"name","please press the button you want to delete",QMessageBox::Yes);
        return;
    }
    if(FindNode(G,ui->line_name->text())==-1) {
        QMessageBox::warning(NULL,"name","can't find the scence",QMessageBox::Yes);
        return;
    }

    bt->deleteLater();//buttonɾ��
    DelNode(&G,name);//ͼ�ṹ��ɾ������
    Save(G);
    QMessageBox::warning(NULL,"name","success",QMessageBox::Yes);
    //this->close();
}

void MainWindow::delpath()//ɾ����
{
    int i=0,m=-1,n=-1;
    //��ȡ����ʼ�� ���յ���
   QString start=ui->line_start_edit->text();
   QString final=ui->line_final_edit->text();

   //�пգ������˳�
   if (start.isEmpty()||final.isEmpty())
   {
       QMessageBox::warning(NULL,"name","empty",QMessageBox::Yes);
       return;
   }

  // ������ȡ������
   for(i=0;i<G.n;i++){
       if(G.adjlist[i].ndata.name==start)
           m=i;
       if(G.adjlist[i].ndata.name==final)
           n=i;
   }

   if(m==-1||n==-1)
   {
       QMessageBox::warning(NULL,"name","can��t find",QMessageBox::Yes);
       return;
   }

   //���ú���ɾ��
   DelEdge(&G,m,n);
   Save(G);
   QMessageBox::warning(NULL,"name","success",QMessageBox::Yes);

}

void MainWindow::mousePressEvent(QMouseEvent *e)//��ȡ��굥��λ������
{
   if(e->x()<2||e->y()<13||e->x()>749||e->y()>544)
       return;

   //ת��Ϊ�ַ�������ʾ���굽����
   QString strx=QString::number(e->pos().x());
   QString stry= QString::number(e->pos().y());

   ui->lineEdit_x->setText(strx);
   ui->lineEdit_y->setText(stry);

}

bool MainWindow::eventFilter(QObject *, QEvent *evt)//��ͼ�ƶ�����
{
    static QPoint lastPnt;
    static bool isHover = false;
    if(evt->type() == QEvent::MouseButtonPress)//��갴��
    {
        QMouseEvent* e = static_cast<QMouseEvent*>(evt);
        if(ui->frame->rect().contains(e->pos()) && //is the mouse is clicking the key
            (e->button() == Qt::LeftButton)) //if the mouse click the right key
        {
            lastPnt = e->pos();//��ȡ��갴��λ��
            isHover = true;
        }
    }
    else if(evt->type() == QEvent::MouseMove && isHover)
    {
        QMouseEvent* e = static_cast<QMouseEvent*>(evt);
        int dx=ui->frame->x()+e->pos().x()-lastPnt.x();//��갴��ʱλ�ú͵�ǰλ�òframe�ƶ�λ��
        int dy=ui->frame->y()+e->pos().y()-lastPnt.y();

        //�жϱ߽�
        if(dx>0)
            dx=0;
        if(dx<-1011)
            dx=-1011;
        if(dy>0)
            dy=0;
        if(dy<-750)
            dy=-750;
        ui->frame->move(dx,dy);//�ƶ�
    }

    else if(evt->type() == QEvent::MouseButtonRelease && isHover)
    {
        isHover = false;
    }
    return false;
}

void MainWindow::introduct()//���ܾ���
{
    if (QPushButton* btn = dynamic_cast<QPushButton*>(sender())){//��ȡ��������button
         QString text= btn->text();
         bt=btn;
         int i;
         i=FindNode(G,text);//���Ҿ���
         if(i>=G.n) {
             QMessageBox::warning(NULL,"name","can not fin the node",QMessageBox::Yes);
             return;
         }
         //��ʾ��Ϣ
         ui->text_intro->setText(G.adjlist[i].ndata.info);
         ui->text_info->setText(G.adjlist[i].ndata.info);

         //���Ѿ�������ʾ��·�����ҿ�
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

         //��ʾ������Ϣ
         if(x>450)
            x=430;
         if(y>360)
            y=330;
         ui->lineEdit_x->setText(QString::number(x));
         ui->lineEdit_y->setText(QString::number(y));

         //��ʾ����ͼƬ
         QString str = QString("QFrame#img_frame{border-image:url("+G.adjlist[i].ndata.pic_path+")}");
         ui->img_frame->setStyleSheet(str);
         ui->pic_path->setText(G.adjlist[i].ndata.pic_path);
       }

}

void MainWindow::search()//·�߲���
{
    QString start=ui->line_start->text();
    QString final=ui->line_final->text();
    //��ȡ·�߶�����Ϣ
    if (final.isEmpty()&&start.isEmpty())
    {
        QMessageBox::warning(NULL,"name","empty",QMessageBox::Yes);
        return;
    }
    int m=FindNode(G,start),n=FindNode(G,final);//���Ҿ���
    if(m==-1||n==-1){
        ui->text_path->setText("can not find the scenery");
        return;
    }

    ui->text_path->setText(FindPath(G,m,n));
}

void MainWindow::recommend()//�Ƽ�·��
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

    //��ʼ��ջ����Ϣ
    for(i=0;i<G.n;i++)
        sign.visited[i]=0;
    int num=0;



  //  �ж���ͨͼ·��
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
    ui->text_path->setText(text);//��ʾ
}

void MainWindow::save_bg()//�洢����ͼ
{
   QString path=ui->line_bg->text();//��ȡ·��
   if (path.isEmpty() )
   {
       QMessageBox::warning(NULL,"name","empty",QMessageBox::Yes);
       return;
   }
   QString str = QString("QFrame#frame{border-image:url("+path+")}");
   ui->frame->setStyleSheet(str);//���ñ���ͼ
   G.bgpic_path=path;

   Save(G);//����
}

void MainWindow::ret()//��ԭĬ��ͼƬ
{
   G.bgpic_path="pic/map.jpg";
   QString str = QString("QFrame#frame{border-image:url("+G.bgpic_path+")}");
   ui->frame->setStyleSheet(str);

   Save(G);
}

void MainWindow::add_pic()//��Ӿ�����Ƭ
{
    QString fileName;
    //�ô򿪴��ڻ�ȡ·��
    fileName = QFileDialog::getOpenFileName(this,tr("Open Config"),
             "./pic/", "Image files (*.bmp *.jpg *.pbm *.pgm *.png *.ppm *.xbm *.xpm);;All files (*.*)");


    if (fileName.isNull())
        return;
    ui->pic_path->setText(fileName);
}

void MainWindow::open_bg()//��ͼƬ
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
