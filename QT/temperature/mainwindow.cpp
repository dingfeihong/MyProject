#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_trans_clicked()
{
    QString s_cen=ui->Centigrade->text();
    if(s_cen==NULL) return;
    double cen=ui->Centigrade->text().toDouble();

    double fah=cen*33.8;
    ui->Fahrenheit->setText(QString::number(fah));
}
