#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "ftphelper.h"
#include "qdirmodel.h"
#include "cftp.h"
namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

private slots:
    void on_Conn_clicked();

    void on_CLDBTN_clicked();

    void on_LocalTree_doubleClicked(const QModelIndex &index);

    void on_LocalTree_clicked(const QModelIndex &index);

    void on_listWidget_doubleClicked(const QModelIndex &index);

    void on_Intro_clicked();

    void on_listWidget_clicked(const QModelIndex &index);

    void on_Break_clicked();

private:
    Ui::MainWindow *ui;
    FtpHelper *ftphelper;
    QDirModel *model;
};

#endif // MAINWINDOW_H
