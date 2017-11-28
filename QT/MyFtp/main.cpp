#include "mainwindow.h"
#include <QApplication>
#include<qnetwork.h>
#include <QTextCodec>
#include <QtNetwork/qnetworkrequest.h>

#include <QtNetwork/QSslConfiguration>
#include <QtNetwork/QSslSocket>
QNetworkRequest req;
QSslConfiguration config;


int main(int argc, char *argv[])
{
    config.setPeerVerifyMode(QSslSocket::VerifyNone);

    config.setProtocol(QSsl::TlsV1_1);
    req.setSslConfiguration(config);

    QApplication a(argc, argv);

    MainWindow w;
    w.show();

    return a.exec();
}
