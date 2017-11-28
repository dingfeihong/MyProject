#-------------------------------------------------
#
# Project created by QtCreator 2015-05-02T14:33:37
#
#-------------------------------------------------

QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = My_Travel
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    algraph.cpp \
    editwindow.cpp

HEADERS  += mainwindow.h \
    algraph.h \
    editwindow.h

FORMS    += mainwindow.ui \
    editwindow.ui

RESOURCES += \
    pic.qrc

DISTFILES += \
    pic/logo.ico \
    pic/logo.rc
RC_FILE=pic/logo.rc
