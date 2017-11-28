#include "mainwindow.h"
#include <QApplication>
#include"algraph.h"
#include <QTextCodec>
int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    MainWindow w;

    w.setWindowTitle("My Travel");
    w.show();

    return a.exec();
}
