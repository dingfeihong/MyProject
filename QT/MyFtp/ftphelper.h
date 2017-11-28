#ifndef FTPHELPER_H
#define FTPHELPER_H
#include<iostream>
#include<winsock2.h>
#include"resource.h"
#include<qtextedit.h>
#include <sys/types.h>
#include <sys/stat.h>
#include<QListWidget.h>
#include"myfile.h"
typedef struct MyFtp
{
public:
    char m_pszServerIp[20];//服务器Ip
    int m_nServerPort;//服务器端口
    int m_nDataPort;//数据端口
    int m_nTimeOut;//套接字多久没反应 自动返回
    char m_pszUserName[40];//用户名
    char m_pszPassWord[20];//密码
    int Status;//状态

    HANDLE hFile;
    DWORD dwAccess = GENERIC_READ | GENERIC_WRITE;
    DWORD dwCreate = CREATE_ALWAYS;
}MYFTP;

class FtpHelper
{
public:    

    FtpHelper(QTextEdit* _msg,QListWidget *_msg2);
    bool InitFtp();
    bool Logout();
    void DownLoad(QString filepath);
    void UpLoad(QString filepath,QString filename);
    void CMD(char *remote_file);
    void SetFtpData(char *pszServerIp,int nServerPort,int nTimeOut,char * pszUserName,char *pszPassWord);

protected:
    QTextEdit *msg;
    QListWidget *list;
    SOCKET control_sock;
    SOCKET data_sock; /* 数据套接口 */
    MYFTP myftp;
    struct hostent *host;

    int err;
    bool IsRecvAlready;
    bool log_sign;
    bool lock;
    //char m_szBuf[RECV_BUF_LEN];//接收命令回复数组

    bool getDataSocket();
    bool Connect();
    bool Login();
    bool Pasv();
    bool Port();
    bool ListFile(char *dir);
    bool SendControl(char *cmd, char *param);
    bool RecvControl(const char * pszResult);
    bool RecvControl(const char * pszResult,const char * pszResult2);

    void show(char *buf);
};
bool is_dir(const char* path);

#endif // FTPHELPER_H
