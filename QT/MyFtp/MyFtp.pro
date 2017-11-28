#-------------------------------------------------
#
# Project created by QtCreator 2016-06-04T10:27:42
#
#-------------------------------------------------

QT       += core gui
QT       += network
greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = MyFtp
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    ftphelper.cpp \
    myfile.cpp

    myfile.cpp

HEADERS  += mainwindow.h \
    resource.h \
    ftphelper.h \
    myfile.h

FORMS    += mainwindow.ui
LIBS += -lws2_32
