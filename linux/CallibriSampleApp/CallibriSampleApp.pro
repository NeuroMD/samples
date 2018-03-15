#-------------------------------------------------
#
# Project created by QtCreator 2016-12-05T12:32:26
#
#-------------------------------------------------

QT       += core gui charts

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = CallibriSampleApp
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    chart.cpp

HEADERS  += mainwindow.h \
    chart.h

FORMS    += mainwindow.ui

linux:QMAKE_CXXFLAGS += -gdwarf-3


win32:CONFIG(release, debug|release): LIBS += -L$$PWD/../../../linux/build-debug/release/ -lneurosdk
else:win32:CONFIG(debug, debug|release): LIBS += -L$$PWD/../../../cross-platform/build-libneurosdk-Desktop_Qt_5_10_0_MinGW_32bit-Debug/debug/ -lneurosdk
else:unix: LIBS += -L$$PWD/../../../linux/build-debug/ -lneurosdk -lbluetooth -lgattlib

INCLUDEPATH += $$PWD/../../../core/common/include
INCLUDEPATH += $$PWD/../../../core/linux/include
INCLUDEPATH += $$PWD/../../../core/windows/include
INCLUDEPATH += $$PWD/../../../utils/network
DEPENDPATH += $$PWD/../../../core/common/
