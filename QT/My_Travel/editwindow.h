#ifndef EDITWINDOW_H
#define EDITWINDOW_H

#include <QDialog>
#include"algraph.h"
namespace Ui {
class editwindow;
}

class editwindow : public QDialog
{
    Q_OBJECT

public:
    explicit editwindow(QWidget *parent = 0);
    ALGraph G;
    ~editwindow();
private slots:
    void addnode();
    void addpath();
    void delnode();
    void delpath();
private:
    Ui::editwindow *ui;
};

#endif // EDITWINDOW_H
