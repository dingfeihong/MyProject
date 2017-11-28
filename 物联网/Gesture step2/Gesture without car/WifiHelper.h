#pragma once

#define COM_LEFT 			0x02
#define COM_RIGHT			0x01
#define COM_FORWARD		0x04
#define COM_BACKWARD		0x03
#define COM_STOP			0x00

#define STATUS_FORWARD	0x1
#define STATUS_BACKWARD	0x2
#define STATUS_STATIC		0x0

struct UsrData {
	char head;
	char type;
	char comm;
	char data;
	char tail;
};
class WifiHelper
{
private:
	int m_sock;
	char m_ControlIp[20];//服务器Ip
	int m_Port;//服务器端口
	struct hostent *host;
	struct UsrData comm;
	bool Forward();
	bool Backward();
	bool Right();
	bool Left();
	bool Tap();
	bool Stop();
	void thread_create ();
public:
	WifiHelper();
	~WifiHelper();
	void Instru_Exec(int choice);
	bool InitWIFISocket();
	void Close();
	int lastStatus;
};

