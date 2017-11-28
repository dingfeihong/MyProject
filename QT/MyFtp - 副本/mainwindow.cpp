#include "mainwindow.h"
#include "ui_mainwindow.h"
#include<QMessageBox>
MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    model = new QDirModel;
    ui->LocalTree->setModel(model);
    ui->LocalAddr->setText("f:\\");
    ui->LocalTree->setRootIndex(model->index(ui->LocalAddr->text()));
    ftphelper=new FtpHelper(ui->MSG,ui->listWidget);
    ui->Break->setEnabled(false);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_Conn_clicked()
{
    QString  tmp=ui->ServerIp->text();
    char*  ServerIp;
    QByteArray ba1 = tmp.toLatin1();
    ServerIp=ba1.data();

    tmp=ui->Port->text();
    int  Port;
    Port=tmp.toInt();

    tmp=ui->User->text();
    char*  User;
    QByteArray ba2 = tmp.toLatin1();
    User=ba2.data();

    tmp=ui->Password->text();
    char*  Psd;
    QByteArray ba3 = tmp.toLatin1();
    Psd=ba3.data();

    ftphelper->SetFtpData(ServerIp,Port,2000,User,Psd);
    if(ftphelper->InitFtp()){
        ui->Conn->setEnabled(false);
        ui->Break->setEnabled(true);
    }
}

void MainWindow::on_CLDBTN_clicked()
{
    ui->LocalTree->setRootIndex(model->index(ui->LocalAddr->text()));
}

void MainWindow::on_LocalTree_doubleClicked(const QModelIndex &index)
{
    if(index.column()!=0)return;
    QModelIndex tmp=index;
    QString addr=index.data().toString();
    QString name=index.data().toString();
    while(tmp.parent()!=QModelIndex()){
        tmp=tmp.parent();
        addr=tmp.data().toString()+"\\"+addr;
    }
    ui->LocalAddr->setText(addr);

    char*  ch;
    QByteArray ba = addr.toLatin1();
    ch=ba.data();

    if(!is_dir(ch))
        ftphelper->UpLoad(ch);
}

void MainWindow::on_LocalTree_clicked(const QModelIndex &index)
{

}

void MainWindow::on_listWidget_doubleClicked(const QModelIndex &index)
{
     ui->MSG->append(index.data().toString());
     char*  ch;
     QByteArray ba = index.data().toString().toLatin1();
     ch=ba.data();
     ftphelper->DownLoad(ch);
}


void MainWindow::on_listWidget_clicked(const QModelIndex &index)
{
    ui->MSG->append(index.data().toString());
    char*  ch;
    QByteArray ba = index.data().toString().toLatin1();
    ch=ba.data();
    ftphelper->CMD(ch);
}
void MainWindow::on_Intro_clicked()
{
    QMessageBox message(QMessageBox::NoIcon, "说明", "FTP服务器文件列表无法区分文件与文件夹，双击实现下载,单击实现转到目录。由于QT5中文编码问题，路径和文件名不支持中文。", QMessageBox::Yes , NULL);
    message.exec();
}

void MainWindow::on_Break_clicked()
{
    if(ftphelper->Logout()){
        ui->Conn->setEnabled(true);
        ui->Break->setEnabled(false);
    }
}
