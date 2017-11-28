#ifndef MYFTO_H
#define MYFTO_H
#include <QString>
#include<qiodevice.h>
#include<QtNetwork/qtcpsocket.h>
class QUrlInfo;
class MyFtp:public QObject
{
public:
    explicit MyFtp(QObject * parent =NULL);

    enum State {
    Unconnected,
    HostLookup,
    Connecting,
    Connected,
    LoggingIn,
    LoggedIn,
    Closing,
    Closed
    };

    enum Command {
       None,
       SetTransferMode,
       SetProxy,
       ConnectToHost,
       Login,
       Close,
       List,
       Cd,
       Get,
       Put,

       Remove,    // haven't accomplished
       Mkdir,
       Rmdir,
       Rename,
       RawCommand
    };

    enum TransferType {
       Binary,
       Ascii
    };

    enum TransferMode {
       Active,
       Passive
    };
    int connectToHost( const QString &hostname, uint port = 21 );//连接主机
    int login( const QString & username = QString("anonymous"), const QString & password);//登录
    int disconnect();  //断开
    int list( const QString &dir = QString() );//显示路径列表
    int cwd( const QString & dir );//改变当前目录
    int bin();//Enter binary mode for sending binary files.


    int ascii();
    int put( QIODevice *dev, const QString &file );//发送命令
    int get( const QString &file, QIODevice *dev );//下载文件
    int syst();//获取服务器信息
    int abort();//Cut down current data link.
    int setTransferMode( TransferMode mode );//设置模式
    State currentState() const;//获取当前状态
    Command currentCommand() const;//获取当前命令
    TransferType currentTransferType() const;//获取当前模式
    void openDebug( bool b )
    { m_bDebug = b; }
    protected :
        QString pwd();//返回连接的工作目录
        void sendLine( QString cmd );//发送命令
        QString getLine();//获得命令

    private :
        QTcpSocket * m_pControlSocket;
        QTcpSocket * m_pDataSocket;

        State m_eCurrentState;
        Command m_eCurrentCommand;
        TransferType m_eTransferType;
        TransferMode m_eTransferMode;
        //ServerSystemType m_eServerSystemType;

        QString m_sDataIP;
        uint m_iDataPort;
        bool m_bSetModeSuccess;

        // all ids for states.
        int connect_id;
        int login_id;
        int close_id;
        int list_id;
        int pwd_id;
        int cwd_id;
        int bin_id;
        int ascii_id;
        int put_id;
        int get_id;
        int transfertype_id;
        int syst_id;
        int abort_id;

        // For Debug.
        bool m_bDebug;


};
inline MyFtp::State MyFtp::currentState() const
{
    return m_eCurrentState;
}

inline MyFtp::Command MyFtp::currentCommand() const
{
    return m_eCurrentCommand;
}

inline MyFtp::TransferType MyFtp::currentTransferType() const
{
    return m_eTransferType;
}
#endif // MYFTO_H
