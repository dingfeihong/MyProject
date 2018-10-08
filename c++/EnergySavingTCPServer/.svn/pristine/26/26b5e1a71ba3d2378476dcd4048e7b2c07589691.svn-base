#ifndef ASIOPLUS_H
#define ASIOPLUS_H

#include "Message.h"
/**
  * 处理和一个客户端的通信
  */
class AsioPlus {
	protected:
		socket_ptr mySocket;

		//以十六进制输出固定长度的字节
		void outByte(BYTE *str, int length);

	public:
		//构造函数
		AsioPlus(io_service & io_service);
		//获取当前的sock指针
		socket_ptr getMySocket();
		//从缓冲区中读取固定的字节信息
		BYTE * readByByte(int lenght, boost::system::error_code &ec);
		//从缓冲区中读取固定的字符信息
		char * readByChar(int lenght, boost::system::error_code &ec);
		//从缓冲区中读取一个整形变量
		int readInt(boost::system::error_code &ec);
		//从缓冲区中读取一条信息
		Message* readAMessage(boost::system::error_code &ec);
		//判断是否存在未读数据
		bool canRead();
		//发送给定的xml到客户端
		void sendXML(const char* xml, int len, int type, boost::system::error_code &ec);
};

#endif
