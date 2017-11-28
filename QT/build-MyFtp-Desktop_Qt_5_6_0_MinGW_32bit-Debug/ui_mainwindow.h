/********************************************************************************
** Form generated from reading UI file 'mainwindow.ui'
**
** Created by: Qt User Interface Compiler version 5.6.0
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_MAINWINDOW_H
#define UI_MAINWINDOW_H

#include <QtCore/QVariant>
#include <QtWidgets/QAction>
#include <QtWidgets/QApplication>
#include <QtWidgets/QButtonGroup>
#include <QtWidgets/QHeaderView>
#include <QtWidgets/QLabel>
#include <QtWidgets/QLineEdit>
#include <QtWidgets/QListWidget>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QMenuBar>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QStatusBar>
#include <QtWidgets/QTextEdit>
#include <QtWidgets/QToolBar>
#include <QtWidgets/QTreeView>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_MainWindow
{
public:
    QWidget *centralWidget;
    QLabel *label;
    QLineEdit *ServerIp;
    QLabel *label_2;
    QLineEdit *Port;
    QLabel *label_3;
    QLineEdit *Password;
    QLabel *label_4;
    QLineEdit *User;
    QPushButton *Conn;
    QPushButton *Break;
    QLabel *label_5;
    QLineEdit *LocalAddr;
    QTreeView *LocalTree;
    QPushButton *CLDBTN;
    QTextEdit *MSG;
    QListWidget *listWidget;
    QPushButton *Intro;
    QMenuBar *menuBar;
    QToolBar *mainToolBar;
    QStatusBar *statusBar;

    void setupUi(QMainWindow *MainWindow)
    {
        if (MainWindow->objectName().isEmpty())
            MainWindow->setObjectName(QStringLiteral("MainWindow"));
        MainWindow->resize(634, 470);
        centralWidget = new QWidget(MainWindow);
        centralWidget->setObjectName(QStringLiteral("centralWidget"));
        label = new QLabel(centralWidget);
        label->setObjectName(QStringLiteral("label"));
        label->setGeometry(QRect(10, 20, 41, 21));
        ServerIp = new QLineEdit(centralWidget);
        ServerIp->setObjectName(QStringLiteral("ServerIp"));
        ServerIp->setGeometry(QRect(50, 20, 101, 20));
        label_2 = new QLabel(centralWidget);
        label_2->setObjectName(QStringLiteral("label_2"));
        label_2->setGeometry(QRect(160, 20, 41, 21));
        Port = new QLineEdit(centralWidget);
        Port->setObjectName(QStringLiteral("Port"));
        Port->setGeometry(QRect(190, 20, 31, 20));
        label_3 = new QLabel(centralWidget);
        label_3->setObjectName(QStringLiteral("label_3"));
        label_3->setGeometry(QRect(230, 20, 41, 21));
        Password = new QLineEdit(centralWidget);
        Password->setObjectName(QStringLiteral("Password"));
        Password->setGeometry(QRect(380, 20, 81, 20));
        Password->setEchoMode(QLineEdit::Password);
        label_4 = new QLabel(centralWidget);
        label_4->setObjectName(QStringLiteral("label_4"));
        label_4->setGeometry(QRect(350, 20, 41, 21));
        User = new QLineEdit(centralWidget);
        User->setObjectName(QStringLiteral("User"));
        User->setGeometry(QRect(270, 20, 71, 20));
        Conn = new QPushButton(centralWidget);
        Conn->setObjectName(QStringLiteral("Conn"));
        Conn->setGeometry(QRect(470, 20, 61, 23));
        Break = new QPushButton(centralWidget);
        Break->setObjectName(QStringLiteral("Break"));
        Break->setGeometry(QRect(540, 20, 61, 23));
        label_5 = new QLabel(centralWidget);
        label_5->setObjectName(QStringLiteral("label_5"));
        label_5->setGeometry(QRect(10, 50, 41, 21));
        LocalAddr = new QLineEdit(centralWidget);
        LocalAddr->setObjectName(QStringLiteral("LocalAddr"));
        LocalAddr->setGeometry(QRect(50, 51, 191, 21));
        LocalTree = new QTreeView(centralWidget);
        LocalTree->setObjectName(QStringLiteral("LocalTree"));
        LocalTree->setGeometry(QRect(10, 80, 301, 261));
        CLDBTN = new QPushButton(centralWidget);
        CLDBTN->setObjectName(QStringLiteral("CLDBTN"));
        CLDBTN->setGeometry(QRect(250, 50, 51, 23));
        MSG = new QTextEdit(centralWidget);
        MSG->setObjectName(QStringLiteral("MSG"));
        MSG->setGeometry(QRect(10, 350, 611, 61));
        listWidget = new QListWidget(centralWidget);
        listWidget->setObjectName(QStringLiteral("listWidget"));
        listWidget->setGeometry(QRect(320, 80, 301, 261));
        Intro = new QPushButton(centralWidget);
        Intro->setObjectName(QStringLiteral("Intro"));
        Intro->setGeometry(QRect(540, 50, 61, 23));
        MainWindow->setCentralWidget(centralWidget);
        menuBar = new QMenuBar(MainWindow);
        menuBar->setObjectName(QStringLiteral("menuBar"));
        menuBar->setGeometry(QRect(0, 0, 634, 23));
        MainWindow->setMenuBar(menuBar);
        mainToolBar = new QToolBar(MainWindow);
        mainToolBar->setObjectName(QStringLiteral("mainToolBar"));
        MainWindow->addToolBar(Qt::TopToolBarArea, mainToolBar);
        statusBar = new QStatusBar(MainWindow);
        statusBar->setObjectName(QStringLiteral("statusBar"));
        MainWindow->setStatusBar(statusBar);

        retranslateUi(MainWindow);

        QMetaObject::connectSlotsByName(MainWindow);
    } // setupUi

    void retranslateUi(QMainWindow *MainWindow)
    {
        MainWindow->setWindowTitle(QApplication::translate("MainWindow", "FTP\345\256\242\346\210\267\347\253\257", 0));
        label->setText(QApplication::translate("MainWindow", "\344\270\273\346\234\272\357\274\232", 0));
        ServerIp->setText(QApplication::translate("MainWindow", "ftp.sjtu.edu.cn", 0));
        label_2->setText(QApplication::translate("MainWindow", "\347\253\257\345\217\243\357\274\232", 0));
        Port->setText(QApplication::translate("MainWindow", "21", 0));
        label_3->setText(QApplication::translate("MainWindow", "\347\224\250\346\210\267\345\220\215\357\274\232", 0));
        Password->setText(QApplication::translate("MainWindow", "222@qq.com", 0));
        label_4->setText(QApplication::translate("MainWindow", "\345\257\206\347\240\201\357\274\232", 0));
        User->setText(QApplication::translate("MainWindow", "anonymous", 0));
        Conn->setText(QApplication::translate("MainWindow", "\350\277\236\346\216\245", 0));
        Break->setText(QApplication::translate("MainWindow", "\346\226\255\345\274\200", 0));
        label_5->setText(QApplication::translate("MainWindow", "\346\234\254\345\234\260\357\274\232", 0));
        CLDBTN->setText(QApplication::translate("MainWindow", "\350\275\254\345\210\260", 0));
        Intro->setText(QApplication::translate("MainWindow", "\350\257\264\346\230\216", 0));
    } // retranslateUi

};

namespace Ui {
    class MainWindow: public Ui_MainWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MAINWINDOW_H
