#include<winsock2.h>
#include<QFileDialog>
#include<qtextstream.h>
#include"resource.h"
class Myfile
{
public:
    Myfile();
    static bool FDownLoad(SOCKET data_sock,QString defaultname);
    static bool FUpLoad(SOCKET data_sock,QString filepath);
    static QFile *FCreatFile(QString defaultname);
};

