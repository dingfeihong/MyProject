#include "AsioPlus.h"

/**
 * 构造函数
 */
AsioPlus::AsioPlus(io_service& io_service) 
	:mySocket(new ip::tcp::socket(io_service)) {}

/**
 * 获取当前的sock指针
 */
socket_ptr AsioPlus::getMySocket() {
	return this -> mySocket;
}

/**
 * 从缓冲去中读取固定的字节信息
 * 返回字节流
 */
BYTE* AsioPlus::readByByte(int length, boost::system::error_code &ec) {
	if(!this -> mySocket -> is_open()) return NULL;
	BYTE buff[max_length];
	BYTE *ret = new BYTE[length + 1];
	//socket, 缓冲区, 满足条件(返回0满足)
	read(*(this -> mySocket), boost::asio::buffer(buff), transfer_exactly(length), ec); 
	if(ec) {
		this -> mySocket -> close();
		return NULL;
	}
	for(int i=0; i<=length; i++) ret[i] = buff[i];
	return ret;
}

/**
 * 从缓冲去中读取固定的字符信息
 * 返回字符流
 */
char* AsioPlus::readByChar(int length, boost::system::error_code &ec) {
	if(!this -> mySocket -> is_open()) return NULL;
	char buff[max_length];
	char *ret = new char[length + 1];
	//socket, 缓冲区, 满足条件(返回0满足)
	read(*(this -> mySocket), boost::asio::buffer(buff), transfer_exactly(length), ec); 
	if(ec) {
		this -> mySocket -> close();
		return NULL;
	}
	for(int i=0; i<=length; i++) ret[i] = buff[i];
	return ret;
}

/**
 * 以十六进制输出固定长度的字节
 * str 字节流
 * length 字节流长度
 */
void AsioPlus::outByte(BYTE *str, int length) {
	for(int i=0; i<length; i++) 
		printf("%02hhx ", str[i]);
	puts("");
}

/**
 * 从缓冲区中读取一个整形变量
 *
 */
int AsioPlus::readInt(boost::system::error_code &ec) {
	BYTE *num = readByByte(4, ec);
	if(ec) return 0;
	int ret = 0;
	ret |= num[0]; ret <<= 8;
	ret |= num[1]; ret <<= 8;
	ret |= num[2]; ret <<= 8;
	ret |= num[3];
	return ret;
}


/**
 * 从缓冲区中读取一条消息
 *
 */
Message* AsioPlus::readAMessage(boost::system::error_code &ec) {
	try {
		BYTE *head = readByByte(2, ec); 
		if(ec) {mySocket -> close(); return NULL;}
		//puts("head: "); outByte(head, 2);
		//BYTE *version = readByByte(2, ec); 
		//puts("ver: "); outByte(version, 2);
		//新协议字段
		BYTE type = readByByte(1, ec)[0];
		if(ec) {mySocket -> close(); return NULL;}
		//puts("type: "); outByte(&type, 1);
		//BYTE *reserve = readByByte(8);
		//puts("rev: "); outByte(reserve, 8);
		//新协议字段
		int length = readInt(ec);
		if(ec) {mySocket -> close(); return NULL;}
		//printf("%d\n", length);
		const char *data = readByChar(length, ec);
		if(ec) {mySocket -> close(); return NULL;}
		Message *message = new Message(type, length, data);
		return message;
	} catch(std::exception e) {
		std::cerr << e.what() << std::endl;
		mySocket -> close();
		return NULL;
	}
	//outByte((BYTE*)data, length);
	//BYTE *CRC = readByByte(2);
	//puts("CRC: "); outByte(CRC, 2);
	//新协议字段
}

/**
 * 判断是否存在未读数据
 *
 */
bool AsioPlus::canRead() {
	return this -> mySocket -> available() > 0;
}

/**
 * 发送给定的xml至客户端
 * 输入：xml
 */
void AsioPlus::sendXML(const char *xml, int len, int type, boost::system::error_code &ec) {
	try {
		std::shared_ptr<Message> message(new Message((BYTE)type, len, xml));
		int L = 0;
		std::shared_ptr<BYTE> messageByte = message -> getSendMSG(L);
		char out[max_length] = {0};
		for(int i=0; i<L; i++) out[i] = messageByte.get()[i];
		int num = write(*(this -> mySocket), buffer(out), transfer_exactly(L));
		cout << "Has Send " << num << "/" << len << " Bytes" << endl;
	} catch(std::exception e) {
		std::cerr << e.what() << std::endl;
		mySocket -> close();
	}
}

