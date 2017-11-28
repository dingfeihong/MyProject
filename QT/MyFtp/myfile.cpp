#include "myfile.h"

Myfile::Myfile()
{

}

QFile *Myfile::FCreatFile(QString defaultname){
    QString filename=NULL;
    filename = QFileDialog::getSaveFileName(NULL,
        QObject::tr("Save"),
        defaultname,
        QObject::tr("*.*")); //选择路径
    if(filename==NULL)return NULL;

    QFile *file=new QFile(filename);

    return file;
}

bool Myfile::FDownLoad(SOCKET data_sock,QString defaultname)
{
    int read_size=0,total=0;
    char Buf[RECV_BUF_LEN];
    QFile *file=FCreatFile(defaultname);

    if (!file->open(QIODevice::WriteOnly | QIODevice::Text))
        return false;


    memset(Buf, 0, sizeof(char) * RECV_BUF_LEN);
    while (0 != (read_size = recv(data_sock, Buf, sizeof(char) * RECV_BUF_LEN,0)))
    {
        if (read_size > 0)
        {
            file->write(Buf,read_size);

            total += read_size;
            if (total < 512) /* 小于 512 才显示 */
            {
               Buf[read_size] = 0;
            }
        }
    }
    free(Buf);
    file->close();
    free(file);
    return true;
}

bool Myfile::FUpLoad(SOCKET data_sock,QString filepath)
{
    int read_size=0;
    char Buf[RECV_BUF_LEN];
    QFile *file=new QFile(filepath);
    memset(Buf, 0, sizeof(char) * RECV_BUF_LEN);
    if (!file->open(QIODevice::ReadOnly))
        return false;
    do
    {
        read_size=file->read(Buf,RECV_BUF_LEN);
        if (read_size > 0)
        read_size = send(data_sock, Buf, read_size, 0);
    } while (read_size > 0);

    free(Buf);
    file->close();
    free(file);
    return true;
}
