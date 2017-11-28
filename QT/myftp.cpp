#include "myftp.h"

#include "MyFtp.h"

#include <QDataStream>
#include<qurl.h>
namespace hy
{
    MyFtp::MyFtp( QObject * parent )
        : m_pDataSocket(NULL), m_pControlSocket(NULL)
    {
        m_bDebug = true;
        m_eCurrentState = Unconnected;
        m_eCurrentCommand = None;
        m_eTransferType = Ascii;
        m_eTransferMode = Active;
        m_eServerSystemType = SYS_OTHERS;
        m_pControlSocket = new QTcpSocket();

        // temporarily for this.
        connect_id = 100;
        login_id = 200;
        close_id = 300;
    }

    MyFtp::~MyFtp()
    {
        if ( NULL != m_pControlSocket )
            delete m_pControlSocket;
        if ( NULL != m_pDataSocket )
            delete m_pDataSocket;
    }

    int MyFtp::connectToHost( const QString & hostname, uint port )
    {
        // if the socket is busy, close it.
        if ( m_pControlSocket->isOpen() )
            m_pControlSocket->close();

        // connect to the host
        m_eCurrentState = Connecting;
        m_eCurrentCommand = ConnectToHost;
        emit commandStarted(connect_id);

        m_pControlSocket->connectToHost( hostname, port );
        QString response = getLine();
        if ( ! response.startsWith("220") )
        {
            emit commandFinished( connect_id, true );        // connect failure
            return connect_id;
        }
        else
        {
            emit commandFinished( connect_id, false );        // connect successfully
        }

        // connect to the host
        m_eCurrentState = Connected;
        return connect_id;
    }

    int MyFtp::login( const QString & username, const QString & password )
    {
        // set current state to be logging in.
        m_eCurrentState = LoggingIn;
        m_eCurrentCommand = Login;
        emit commandStarted(login_id);


        // send user name and password if needed
        sendLine( QObject::tr("USER ") + username );
        QString response = getLine();
        if ( !response.startsWith("331") && !response.startsWith("230") )
        {
            emit commandFinished( login_id, true );
            return login_id;
        }

        // if needing password
        if ( response.startsWith("331") )
        {
            sendLine( QObject::tr("PASS ") + password );
            response = getLine();
            if ( ! response.startsWith("230") )
            {
                emit commandFinished( login_id, true );
                return login_id;
            }
            else
            {
                emit commandFinished( login_id, false );
            }
        }
        else
        {
            emit commandFinished( login_id, false );
        }

        // set current state to be connected.
        m_eCurrentState = LoggedIn;
        return login_id;
    }

    int MyFtp::disconnect()
    {
        // set current state to be closing.
        m_eCurrentState = Closing;
        m_eCurrentCommand = Close;
        emit commandStarted(close_id);

        // Aborts the current command and deletes all scheduled commands.
        m_pControlSocket->abort();

        // Closes the connection to the FTP server.
        m_pControlSocket->close();

        emit commandFinished( close_id, false );

        // set current state to be closing.
        m_eCurrentState = Closed;
        return close_id;
    }

    int MyFtp::list( const QString &dir )
    {
        m_eCurrentCommand = List;
        // Get directory to be listed.
        QString directory = dir;
        if ( directory.isEmpty() )  //  if empty, current working directory.
        {
            directory = pwd();
        }

        // sending command, make it passive.
        sendLine("PASV");

        QString response = getLine();
        if ( ! response.startsWith("227") )
        {
            emit commandFinished( get_id, true );
            return list_id;
        }
        else
        {
            m_eTransferMode = Passive;
        }
        int begin = response.indexOf('(') +1;
        int end = response.indexOf( ')', begin );
        QString substring = response.mid( begin, end-begin );        // cut out ip and data port.

        int i;
        QString temp_ip;
        temp_ip.clear();
        // get IP string.
        end =-1;
        for ( i=0; i<3; i++ )
        {
            begin = end +1;
            end = substring.indexOf( ',', begin+1 );
            temp_ip.append( substring.mid( begin, end-begin ) );
            temp_ip.append(".");
        }

        begin = end +1;
        end = substring.indexOf( ',', begin+1 );
        temp_ip.append( substring.mid( begin, end-begin ) );

        // get port.
        uint temp_port =0;
        begin = end +1;
        end = substring.indexOf( ',', begin );
        temp_port = substring.mid( begin, end-begin ).toUInt() * 256;

        begin = end +1;
        temp_port += substring.mid( begin, -1 ).toUInt();

        // Create a new data link from Server to get all lists.
        emit commandStarted(list_id);
        sendLine( QObject::tr("LIST ") + directory );

        m_pDataSocket = new QTcpSocket(this);
        m_pDataSocket->connectToHost( temp_ip, temp_port );
        m_pDataSocket->write( "USER\r\n", 8 );

        response = getLine();
        if (! response.startsWith("150") )
        {
            emit commandFinished( put_id, true );
            return list_id;
        }

        char buffer[500];
        int byteRead = 0;
        while (true) {
            if ( ! m_pDataSocket->waitForReadyRead() )
                break;

            byteRead = m_pDataSocket->readLine( buffer, 500 );
            QString temp_string(buffer);
            QUrlInfo url = HLutil::parseFileList( temp_string, m_eServerSystemType );
            emit listInfo( url );
        }

        m_pDataSocket->flush();
        m_pDataSocket->close();
        delete m_pDataSocket;

        response = getLine();
        if ( ! response.startsWith("226 ") )
        {
            emit commandFinished( get_id, true );
        }
        else
        {
            emit commandFinished( get_id, false );
        }

        return list_id;
    }

    int MyFtp::cwd( const QString & dir )
    {
        m_eCurrentCommand = Cd;
        emit commandStarted(cwd_id);
        // sending command.
        sendLine( QObject::tr("CWD ") + dir );

        QString response = getLine();
        if ( ! response.startsWith("250") )
        {
            emit commandFinished( cwd_id, true );
            return cwd_id;
        }
        else
        {
            emit commandFinished( cwd_id, false );
            return cwd_id;
        }
    }

    int MyFtp::bin()
    {
        emit commandStarted(bin_id);
        // sending command.
        sendLine( QObject::tr("TYPE I") );

        QString response = getLine();
        if ( ! response.startsWith("200") )
        {
            m_eTransferType = Binary;
            emit commandFinished( bin_id, true );
            return bin_id;
        }
        else
        {
            emit commandFinished( bin_id, false );
            return bin_id;
        }
    }

    int MyFtp::ascii()
    {
        emit commandStarted(ascii_id);
        // sending command.
        sendLine( QObject::tr("TYPE A") );

        QString response = getLine();
        if ( ! response.startsWith("200") )
        {
            m_eTransferType = Ascii;
            emit commandFinished( ascii_id, true );
            return ascii_id;
        }
        else
        {
            emit commandFinished( ascii_id, false );
            return ascii_id;
        }
    }

    int MyFtp::put(QIODevice *dev, const QString &file )
    {
        m_eCurrentCommand = Put;
        // sending command, make it passive.
        sendLine("PASV");

        QString response = getLine();
        if ( ! response.startsWith("227") )
        {
            emit commandFinished( put_id, true );
            return put_id;
        }
        else
        {
            m_eTransferMode = Passive;
        }
        int begin = response.indexOf('(')+1;
        int end = response.indexOf(')', begin);
        QString substring = response.mid( begin, end-begin );        // cut out ip and data port.

        int i;
        QString temp_ip;
        temp_ip.clear();
        // get IP string.
        for ( i=0; i<4; i++ )
        {
            begin = end +1;
            end = substring.indexOf( ',', begin );
            temp_ip.append( substring.mid( begin, end-begin ) );
        }

        // get port.
        uint temp_port =0;
        begin = end +1;
        end = substring.indexOf( ',', begin );
        temp_port = substring.mid( begin, end-begin ).toUInt() * 256;

        begin = end +2;
        temp_port += substring.mid( begin, -1 ).toUInt();

        // now set up a new data link use given ip and port.
        emit commandStarted(put_id);
        sendLine( QObject::tr("STOR ") + file );

        m_pDataSocket = new QTcpSocket();
        m_pDataSocket->connectToHost( temp_ip, temp_port );

        response = getLine();
        if (! response.startsWith("150 ") )
        {
            emit commandFinished( put_id, true );
            return put_id;
        }

        char buffer[4096];
        int byteRead = 0;
        memset( buffer, 0, 4096 );
        while ( ( byteRead = dev->read( buffer, 4096 ) ) != -1 ) {
            m_pDataSocket->write( buffer, byteRead );
        }

        m_pDataSocket->flush();
        m_pDataSocket->close();
        dev->close();
        delete m_pDataSocket;

        response = getLine();
        if ( ! response.startsWith("226 ") )
        {
            emit commandFinished( put_id, true );
        }
        else
        {
            emit commandFinished( put_id, false );
        }

        return put_id;

    }

    int MyFtp::get( const QString &file, QIODevice * dev )
    {
        m_eCurrentCommand = Get;

        // sending command, make it passive.
        sendLine("PASV");

        QString response = getLine();
        if ( ! response.startsWith("227") )
        {
            emit commandFinished( get_id, true );
            return list_id;
        }
        else
        {
            m_eTransferMode = Passive;
        }
        int begin = response.indexOf('(') +1;
        int end = response.indexOf( ')', begin );
        QString substring = response.mid( begin, end-begin );        // cut out ip and data port.

        int i;
        QString temp_ip;
        temp_ip.clear();
        // get IP string.
        end =-1;
        for ( i=0; i<3; i++ )
        {
            begin = end +1;
            end = substring.indexOf( ',', begin+1 );
            temp_ip.append( substring.mid( begin, end-begin ) );
            temp_ip.append(".");
        }

        begin = end +1;
        end = substring.indexOf( ',', begin+1 );
        temp_ip.append( substring.mid( begin, end-begin ) );

        // get port.
        uint temp_port =0;
        begin = end +1;
        end = substring.indexOf( ',', begin );
        temp_port = substring.mid( begin, end-begin ).toUInt() * 256;

        begin = end +1;
        temp_port += substring.mid( begin, -1 ).toUInt();

        // now set up a new data link use given ip and port.
        emit commandStarted(get_id);
        sendLine( QObject::tr("RETR ") + file );

        m_pDataSocket = new QTcpSocket();
        m_pDataSocket->connectToHost( temp_ip, temp_port );

        response = getLine();
        if (! response.startsWith("150 ") )
        {
            emit commandFinished( put_id, true );
            return put_id;
        }

        char buffer[4096];
        int byteRead = 0;
        memset( buffer, 0, 4096 );
        while ( m_pDataSocket->waitForReadyRead() ) {
            if ( ( byteRead = m_pDataSocket->read( buffer, 4096 ) ) == -1 )
            {
                break;
            }
            dev->write( buffer, byteRead );
        }

        m_pDataSocket->flush();
        m_pDataSocket->close();
        dev->close();
        delete m_pDataSocket;
        m_pDataSocket = NULL;

        response = getLine();
        if ( ! response.startsWith("226 ") )
        {
            emit commandFinished( get_id, true );
        }
        else
        {
            emit commandFinished( get_id, false );
        }

        return get_id;
    }

    int MyFtp::syst()
    {
        emit commandStarted(syst_id);
        // sending command.
        sendLine( QObject::tr("SYST") );

        QString response = getLine();
        if ( ! response.startsWith("200") )
        {
            emit commandFinished( syst_id, true );
            return syst_id;
        }
        else
        {
            emit commandFinished( syst_id, false );
            return syst_id;
        }
    }

    int MyFtp::abort()
    {
        if ( NULL != m_pDataSocket )
            m_pDataSocket->abort();

        return abort_id;
    }

    QString MyFtp::pwd()
    {
        emit commandStarted(pwd_id);
        // send command.
        sendLine( QObject::tr("PWD") );

        QString response = getLine();
        if ( response.startsWith("257") )
        {
            int opening = response.indexOf('/');
            int closing = response.indexOf('\"', opening + 1);

            emit commandFinished( pwd_id, false );
            QString temp = response.mid( opening, closing-opening );
            return temp;
        }
        else
        {
            emit commandFinished( pwd_id, true );
            return QString();
        }
    }

    QString MyFtp::getLine()
    {
        if ( ! m_pControlSocket->isOpen() )
        {
            qDebug() << "error : MyFtp::getLine() :  socket is not open\n" << endl;
            return QString();
        }

        char buffer[500];
        m_pControlSocket->waitForReadyRead();
        m_pControlSocket->readLine(  buffer, 500 );
        m_pControlSocket->read(m_pControlSocket->bytesAvailable());

        qDebug() << "> get: " << buffer << endl;

        return QString(buffer);
    }

    void MyFtp::sendLine( QString cmd )
    {
        if ( ! m_pControlSocket->isOpen() )
        {
            qDebug() << "error : MyFtp::sendLine( QString cmd ) :  socket is not open\n" << endl;
        }

        m_pControlSocket->waitForBytesWritten();
        cmd.append("\r\n");
        m_pControlSocket->write( cmd.toStdString().c_str(), cmd.size() );
        qDebug() << "> send: " << cmd.toStdString().c_str() << endl;

    }

}
