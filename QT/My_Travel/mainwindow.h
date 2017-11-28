#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include"editwindow.h"
namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    editwindow win;
    ALGraph G;
    ~MainWindow();
private slots:
    void addnode();
    void addpath();
    void delnode();
    void delpath();
    void introduct();
    void search();
    void hide_f();
    void recommend();
    void add_pic();
    void ret();
    void save_bg();
    void open_bg();
protected slots:
    bool eventFilter(QObject *, QEvent *);
protected:
    void mousePressEvent(QMouseEvent* event);
  //  void mouseMoveEvent(QMouseEvent *e);
private:
    Ui::MainWindow *ui;
};

#endif // MAINWINDOW_H
