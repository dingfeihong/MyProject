#include "editwindow.h"
#include "ui_editwindow.h"
#include"algraph.h"
#include <QTextCodec>


editwindow::editwindow(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::editwindow)
{
    ui->setupUi(this);
}
void editwindow::addnode()
{

   load(&G);
   // G.n=G.e=0;
   QString name=ui->line_name->text();
   QString info=ui->text_info->toPlainText();

  // AddNode(&G,name,info);
   Save(G);
   this->close();
}

void editwindow::addpath()
{
   //G.n=G.e=0;
    //ALGraph G;
    load(&G);
   QString start=ui->line_start->text();
   QString final=ui->line_final->text();
   QString arc=ui->line_arc->text();
   QString length=ui->line_length->text();

   Add_Path(&G,start,final,arc,length.toInt());
   Save(G);
   this->close();
}

void editwindow::delpath()
{
    int i=0,m=-1,n=-1;
   //G.n=G.e=0;
   ALGraph G;
   load(&G);
   QString start=ui->line_start_del->text();
   QString final=ui->line_final_del->text();
   for(i=0;i<G.n;i++){
       if(G.adjlist[i].ndata.name==start)
           m=i;
       if(G.adjlist[i].ndata.name==final)
           n=i;
   }
   if(m==-1||n==-1)
       return;

   DelEdge(&G,m,n);
   Save(G);
   this->close();
}

void editwindow::delnode()
{
   //G.n=G.e=0;
    ALGraph G;
    load(&G);
    QString name=ui->line_name_del->text();

    DelNode(&G,name);
    Save(G);
    this->close();
}
editwindow::~editwindow()
{
    delete ui;
}
