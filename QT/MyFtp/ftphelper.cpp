#include "ftphelper.h"
#include<stdlib.h>
#include<time.h>
#include<qtimer>
#include <fstream>
#include<qmessagebox.h>
#include <fcntl.h>
#include<QFileDialog>
#include<unistd.h>
FtpHelper::FtpHelper(QTextEdit* _msg,QListWidget *_lst)
{
    this->msg=_msg;
    this->list=_lst;
    IsRecvAlready = TRUE;
    log_sign=false;
    myftp.Status=FTPS_CLOSED;
    lock=true;
    err=0;
}

bool Judge(QString tmp){
    return tmp.contains(QRegExp("[\\x4e00-\\x9fa5]+"));
}

bool FtpHelper::InitFtp(){
    msg->append("正在连接FTP服务器");
    if(Connect()&&Login()&&Pasv()&&getDataSocket()&&ListFile(NULL)){
        log_sign=true;
        msg->append("欢迎");
    }
    else{
        log_sign=false;
        msg->append("连接失败");
    }
    return log_sign;
}

bool FtpHelper::Connect(){
    struct sockaddr_in server;
    WSADATA wsaData;

    int nRet;
    if((nRet = WSAStartup(MAKEWORD(2,2),&wsaData)) != 0){
        return false;
    }

    int result;
    memset(&server, 0, sizeof(struct sockaddr_in));

    msg->append("正在初始化socket");
    control_sock = socket(AF_INET, SOCK_STREAM, 0);;
    if (control_sock == INVALID_SOCKET){
        err=WSAGetLastError();
        msg->append("初始化socket错误，错误代码:"+QString::number(err,10));
        return false;
    }

    host=gethostbyname(myftp.m_pszServerIp);
    if(host==NULL)
    {
        err=WSAGetLastError();
        msg->append("初始化socket错误，错误代码:"+QString::number(err,10));
        return false;
    }

    memcpy(&server.sin_addr,host->h_addr_list[0],host->h_length);
    server.sin_family = AF_INET;
    server.sin_port = htons(myftp.m_nServerPort);

    msg->append("正在连接到服务器端");
    result= connect(control_sock,(struct sockaddr *)&server, sizeof(server));
    /* 连接到服务器端 */
    if (result == SOCKET_ERROR)
      {
          err=WSAGetLastError();

          msg->append("连接服务器错误，错误代码:"+QString::number(err,10));
          if (WSAGetLastError() != WSAEWOULDBLOCK)
          {
              closesocket(control_sock);
              return SOCKET_ERROR;
          }
      }
     /* 客户端接收服务器端的一些欢迎信息 */
    if(!RecvControl("220")){
        err=WSAGetLastError();
        msg->append("连接服务器错误，错误代码:"+QString::number(err,10));
        return SOCKET_ERROR;
    }
    return true;
}

bool FtpHelper::getDataSocket(){    
    //创建套接字，与服务端相连
    msg->append("尝试创建数据套接字...");
    struct sockaddr_in server;
    if((data_sock=socket(AF_INET,SOCK_STREAM,0))==(SOCKET)-1){

        err=WSAGetLastError();
        msg->append("创建数据套接字失败，错误代码+"+QString::number(err,10));
        return false;
    }
    memset(&server,0,sizeof(server));
    server.sin_family=AF_INET;
    server.sin_port=htons(myftp.m_nDataPort);
    memcpy(&server.sin_addr,host->h_addr_list[0],host->h_length);

    if(connect(data_sock,(struct sockaddr*)(&server),sizeof(struct sockaddr))==-1){
       err=WSAGetLastError();
       msg->append("建立数据链路失败,错误代码"+QString::number(err,10));
       return false;
    }
    msg->append("创建数据套接字成功");
    return true;
}

bool FtpHelper::Login(){
    if(!lock){
        msg->append("上一个命令尚未完成");
        return false;
    }
    msg->append("正在登陆.....");
    if( (!SendControl("USER",myftp.m_pszUserName)) || (!RecvControl("331")) ){
        err=WSAGetLastError();
        msg->append("登录失败，错误代码"+QString::number(err,10));
        return FALSE;
    }

    if( (!SendControl("PASS",myftp.m_pszPassWord)) || (!RecvControl("230")) )
    {
        err=WSAGetLastError();
        msg->append("登录失败，错误代码"+QString::number(err,10));
        return FALSE;
    }
    msg->append("登陆成功，欢迎");
    lock=true;
    return true;
}

bool FtpHelper::Logout(){
    if(!lock){
        msg->append("上一个命令尚未完成");
        return false;
    }
    if( (!SendControl("QUIT",NULL))){// || (!RecvControl("221")) ){
      err=WSAGetLastError();
      msg->append("退出失败，错误代码"+QString::number(err,10));
      return FALSE;
    }
    char m_szBuf[RECV_BUF_LEN];
    memset(m_szBuf,0,RECV_BUF_LEN);
    int t=5;
    while(t>0){
      if( recv(control_sock,m_szBuf,RECV_BUF_LEN,0) == SOCKET_ERROR)
      {
          IsRecvAlready = TRUE;
          return FALSE;
      }

      msg->append("收到消息："+QString(QLatin1String(m_szBuf)));
      IsRecvAlready = TRUE;
      if(strstr(m_szBuf,"221"))
          break;
      int m=1000;
      while(m)m--;
      t--;
    }
    list->clear();
    lock=true;
    log_sign=false;
    msg->append("断开连接成功");
    return true;
}

bool FtpHelper::SendControl(char *cmd, char *param)
{
    char cmd_buf[RECV_BUF_LEN];
    int result;
    if (param)
        result = sprintf(cmd_buf, "%s %s\r\n", cmd, param);
    else
        result = sprintf(cmd_buf, "%s\r\n", cmd);

    if(IsRecvAlready)//如果上一个命令已经接受成功
    {
        if( send(control_sock, cmd_buf, result, 0) == SOCKET_ERROR )
            return FALSE;
    }
    else
    {//如果没有接受完 则等待100毫秒 然后继续发送
        Sleep(100);
        if(IsRecvAlready)//如果上一个命令已经接受成功
        {
            if( send(control_sock, cmd_buf, result, 0) == SOCKET_ERROR )
                return FALSE;
        }
    }
    IsRecvAlready = FALSE;
    return TRUE;
}

bool FtpHelper::RecvControl(const char * pszResult)//pszResult为判断是否成功执行命令
{    
    char m_szBuf[RECV_BUF_LEN];
    memset(m_szBuf,0,RECV_BUF_LEN);
    if( recv(control_sock,m_szBuf,RECV_BUF_LEN,0) == SOCKET_ERROR)
    {
        IsRecvAlready = TRUE;
        return FALSE;
    }
    if(pszResult != NULL)
    {
        if(!strstr(m_szBuf,pszResult))
        {
            msg->append("收到消息："+QString(QLatin1String(m_szBuf)));
            OutputDebugStringA(m_szBuf);
            IsRecvAlready = TRUE;
            return FALSE;
        }
    }
    IsRecvAlready = TRUE;
    msg->append("收到消息："+QString(QLatin1String(m_szBuf)));
    OutputDebugStringA(m_szBuf);
    return TRUE;
}
bool FtpHelper::RecvControl(const char * pszResult,const char * pszResult2)//pszResult为判断是否成功执行命令
{    
    char m_szBuf[RECV_BUF_LEN];
    memset(m_szBuf,0,RECV_BUF_LEN);
    if( recv(control_sock,m_szBuf,RECV_BUF_LEN,0) == SOCKET_ERROR)
    {
        IsRecvAlready = TRUE;
        return FALSE;
    }
    if(pszResult != NULL&&pszResult2!=NULL)
    {
        if(!(strstr(m_szBuf,pszResult)||strstr(m_szBuf,pszResult2)))
        {
            msg->append("收到消息："+QString(QLatin1String(m_szBuf)));
            OutputDebugStringA(m_szBuf);
            IsRecvAlready = TRUE;
            return FALSE;
        }
    }
    IsRecvAlready = TRUE;
    msg->append("收到消息："+QString(QLatin1String(m_szBuf)));
    OutputDebugStringA(m_szBuf);
    return TRUE;
}
void FtpHelper::SetFtpData(char *pszServerIp,int nServerPort,int nTimeOut,char * pszUserName,char *pszPassWord){
    strcpy_s(myftp.m_pszServerIp,pszServerIp);
    myftp.m_nServerPort = nServerPort;
    myftp.m_nTimeOut = nTimeOut;
    strcpy_s(myftp.m_pszUserName,pszUserName);
    strcpy_s(myftp.m_pszPassWord,pszPassWord);
}



int random_local_port(){
    int local_port;
    srand((unsigned)time(NULL));
    local_port = rand() % 40000 + 1025;
    return local_port;
}

bool FtpHelper::Port(){
    /*SOCKET lstn_sock=socket(PF_INET, SOCK_STREAM, IPPROTO_TCP);; // 侦听 socket 句柄
    struct  sockaddr_in local;
    if((lstn_sock=socket(AF_INET,SOCK_STREAM,0))==-1){
        err=WSAGetLastError();
        msg->append("PORT创建套接字失败,错误代码"+QString::number(err,10) );
    }

    //PORT命令发送给服务器客户端的ip和端口(数据链路)
    local.sin_family = AF_INET;
    local.sin_port = htons(0);
    local.sin_addr.s_addr = INADDR_ANY;
    int result = bind(lstn_sock, (struct sockaddr *)&local, sizeof(local));
    if (result == SOCKET_ERROR)
      {
          closesocket(lstn_sock);
          return INVALID_SOCKET;
      }

    if( listen(lstn_sock, SOMAXCONN)==-1){
       msg->append("监听客户端data_socket失败");
       return false;
    }


    struct sockaddr_in soc_addr;  //套接字地址结构
    int addr_len = sizeof(soc_addr);
    unsigned short local_port;	//本地侦听端口

    // 得到侦听套接口地址
    result = getsockname(lstn_sock, (struct sockaddr *)&soc_addr, &addr_len);
    if (result == SOCKET_ERROR)
    {
        err=WSAGetLastError();
        msg->append("sockname error:"+ QString::number(err,10));
        return false;
    }

    // 保存本地侦听端口
    local_port = soc_addr.sin_port;

    //得到控制 socket 的本地地址
    result = getsockname(control_sock, (struct sockaddr *)&soc_addr, &addr_len);
    if (result == SOCKET_ERROR)
    {
        err=WSAGetLastError();
        msg->append("ctrl sockname error:"+ QString::number(err,10));
        return SOCKET_ERROR;
    }

    //根据地址和端口生成 PORT 命令, 都是网络字节序
    sprintf(m_szBuf, "%d,%d,%d,%d,%d,%d",
                    soc_addr.sin_addr.S_un.S_un_b.s_b1,
                    soc_addr.sin_addr.S_un.S_un_b.s_b2,
                    soc_addr.sin_addr.S_un.S_un_b.s_b3,
                    soc_addr.sin_addr.S_un.S_un_b.s_b4,
                    local_port & 0xFF, local_port >> 8);
    if( (!SendControl("PORT",m_szBuf)))// || (!RecvControl("200")) ){
   {     err=WSAGetLastError();
        msg->append("进入主动模式失败，错误代码"+QString::number(err,10));
        return false;
    }

    int numbytes=recv(control_sock,m_szBuf,RECV_BUF_LEN,0);
    if(numbytes== SOCKET_ERROR)
    {
        IsRecvAlready = TRUE;
        return FALSE;
    }

    if(!strstr(m_szBuf,"200"))
    {
        msg->append("收到消息："+QString(QLatin1String(m_szBuf)));
        IsRecvAlready = TRUE;
        return FALSE;
    }


    if((data_sock=accept(lstn_sock, (struct sockaddr *)(&soc_addr), &numbytes))==-1)
    {
        err=WSAGetLastError();
        msg->append("accept error:"+QString::number(err,10));
        return false;
    }

    if(data_sock == -1){
        printf("Sorry, you can't use PORT mode. There is something wrong when the server connect to you.\n");
        return false;
    }
    closesocket(lstn_sock);*/
    return true;
}

bool FtpHelper::Pasv()
{  
    char* buf_ptr;
    char m_szBuf[RECV_BUF_LEN];
    /* 客户端告诉服务器用被动模式 */
    msg->append("进入被动模式");
    if((!SendControl("PASV",NULL))){
        err=WSAGetLastError();
        msg->append("进入被动模式失败，错误代码"+QString::number(err,10));
        return false;
    }
    int t=5;
    while(t>0){
         memset(m_szBuf,0,RECV_BUF_LEN);
        if( recv(control_sock,m_szBuf,RECV_BUF_LEN,0) == SOCKET_ERROR)
        {
            IsRecvAlready = TRUE;
            return FALSE;
        }

        msg->append("收到消息："+QString(QLatin1String(m_szBuf)));
        IsRecvAlready = TRUE;
        if(strstr(m_szBuf,"227"))
            break;
        int m=1000;
        while(m)m--;
        t--;
    }
    IsRecvAlready = TRUE;

    msg->append("进入被动模式成功");

    int serverPort;
    buf_ptr=strrchr(m_szBuf,')');
    *buf_ptr='\0';
    buf_ptr = strrchr(m_szBuf, ',');
    serverPort = atoi(buf_ptr + 1);
    *buf_ptr = '\0';
    buf_ptr = strrchr(m_szBuf, ',');
    serverPort += atoi(buf_ptr + 1) * 256;


    myftp.m_nDataPort = serverPort;
    return true;
}

bool FtpHelper::ListFile(char *dir){
    if(!lock){
        msg->append("上一个命令尚未完成");
        return false;
    }
    msg->append("获取文件列表中...");

    if((!SendControl("NLST",dir)) || (!RecvControl("150","125")) )
    {
        err=WSAGetLastError();
        msg->append("获取文件列表失败，错误代码"+QString::number(err,10));
        return false;
    }

    char cRecvBuf[RECV_BUF_LEN];
    memset(cRecvBuf,0,RECV_BUF_LEN);
    int len= recv(data_sock, cRecvBuf, RECV_BUF_LEN, 0);
    //cRecvBuf[len]='\0';
    show(cRecvBuf);
    close(data_sock);
    lock=true;
    return true;
}

void FtpHelper::show(char *buf){
   /* std::ofstream file("file.txt",std::ios::out|std::ios::ate);
    if(!file)
    {
        std::cout<<"不可以打开文件"<<std::endl;
        exit(1);
    }
    file<<buf;
    file.close();*/
    char* tmp=strrchr(buf,'\r');
    list->clear();
    this->list->addItem("..");
    *tmp = '\0';
    while((tmp = strrchr(buf, '\r'))){
        this->list->addItem(QString(QLatin1String(tmp+2)));
        *tmp = '\0';
    }
    this->list->addItem(QString(QLatin1String(buf)));
}

void FtpHelper::CMD(char *remote_file){
    if(!lock){
        msg->append("上一个命令尚未完成");
        return;
    }
    if(!Pasv())
        return;
    if(!getDataSocket())
        return;
    if((SendControl("CWD",remote_file))&&(RecvControl("250"))){
        lock=true;
        ListFile(NULL);
        msg->append("跳转地址到"+QString(QLatin1String(remote_file))+"完毕");
        return;
    }

}

void FtpHelper::DownLoad(QString filepath){
    if(!lock){
        msg->append("上一个命令尚未完成");
        return;
    }
    if(!Pasv())
        return;
    if(!getDataSocket())
        return;  

    char *ch;
    QByteArray ba = filepath.toLatin1();
    ch=ba.data();

    if( (!SendControl("RETR",ch))||(!RecvControl("150","125")))
    {
        err=WSAGetLastError();
        msg->append("下载失败，错误代码"+QString::number(err,10));
        closesocket(data_sock);
        return;
    }

    //命令套接字已经发送命令，且已经接受到服务器返回的成功信息，接着在数据套接字中获取数据
    if(!Myfile::FDownLoad(data_sock,filepath)){
        msg->append("下载失败");
        return;
    }
    msg->append("下载完成");
    closesocket(data_sock);
    lock=true;
    return;
}

void FtpHelper::UpLoad(QString filepath,QString filename)
{
    if(!log_sign){
        msg->append("请先连接服务器");
        return;
    }
    if(!lock){
        msg->append("上一个命令尚未完成");
        return;
    }

    if(!Pasv())
        return;
    if(!getDataSocket())
        return;



    if(Judge(filename))
        filename="tmp";

    char *ch;
    QByteArray ba = filename.toLatin1();
    ch=ba.data();

    if((!SendControl("STOR",ch))||(!RecvControl("150","125"))){
        err=WSAGetLastError();
        msg->append("上传失败，错误代码"+QString::number(err,10));
        closesocket(data_sock);
        return;
    }

    if(!Myfile::FUpLoad(data_sock,filepath)){
        msg->append("上传失败");
        return;
    }

    msg->append("上传完成");
    closesocket(data_sock);
    this->list->addItem(filename);
}

bool is_dir(const char* path) {
    WCHAR wszClassName[256];
    memset(wszClassName,0,sizeof(wszClassName));
    MultiByteToWideChar(CP_ACP,0,path,strlen(path)+1,wszClassName,
    sizeof(wszClassName)/sizeof(wszClassName[0]));

    WIN32_FIND_DATA FindFileData;
    HANDLE hFind = FindFirstFile(wszClassName,&FindFileData);
    if(INVALID_HANDLE_VALUE == hFind)    return false;
    bool sign=FindFileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY;
    return sign;
}
