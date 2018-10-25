#include "Message.h"

/**
 * 构造函数
 *
 */
Message::Message(BYTE _type, int _length, const char *&_data) {
	this -> version = new BYTE[2];
	this -> version[0] = 0x00;
	this -> version[1] = 0x01;
	this -> type = _type;
	this -> length = _length;
	this -> data = new char[_length + 1];
	for(int i=0; i<_length; i++) this -> data[i] = _data[i];
	this -> data[_length] = 0;
	if(int(this -> type) == T_ENERGY_DATA || int(this -> type) == T_METER_VIP)  {
		this -> data = aesDecrypt(this -> data, this -> length);
	}
}

Message::Message(int _type, int _length, string str) {
	this -> version = new BYTE[2];
	this -> version[0] = 0x00;
	this -> version[1] = 0x01;
	this -> type = (BYTE)_type;
	this -> length = _length;
	this -> data = new char[_length + 1];
	for(int i=0; i<_length; i++) this -> data[i] = str[i];
	this -> data[_length] = 0;
}

/**
  * 解密消息内容
  */
void Message::decryptData() {
	this -> data = aesDecrypt(this -> data, this -> length);
}

/**
  * 深拷贝构造函数
  */
Message::Message(const Message &msg) {
	this -> version = new BYTE[2];
	this -> version[0] = msg.version[0];
	this -> version[1] = msg.version[1];
	this -> type = msg.type;
	this -> length = msg.length;
	this -> data = new char[this -> length + 1];
	for(int i=0; i<this -> length; i++) this -> data[i] = msg.data[i];
	this -> data[this -> length] = 0;
}

/**
  * 析构函数
  */
Message::~Message() {
	delete this -> version;
	delete this -> data;
}


/**
 * 获取消息版本号
 * 返回值：主版本号.次版本号
 */
char* Message::getVersion() {
	char *ret = new char[10];
	sprintf(ret, "%d.%d", (int)this -> version[0], (int)this -> version[1]);
	return ret;
}

/**
 * 返回消息类型
 *--------------------------------------
 * 0x01	身份认证					|
 * 0x02	心跳信息					|
 * 0x03	能耗数据					|
 * 0x04	配置信息					|
 * 0x05	电表控制					|
 * 0x06	电能参数					|
 * 0x06	用电安全数据				|
 * 0x08	空调温控与上报				|
 * 0x09	智能用电控制远程控制与上报	|
 * 0x0A	门显信息					|
 * 0x0B	程序远程升级				|
 * -------------------------------------
 *
 */
int Message::getType() {
	return (int)(this -> type);
}

/**
  * 设置消息类型
  */
void Message::setType(int _type) {
	this -> type = (BYTE)_type;
}

/**
  *返回消息体长度
  */
int Message::getLength() {
	return this -> length;
}

/**
 * 获取消息体
 *
 */
char* Message::getData() {
	return this -> data;
}

/**
  * 返回发送消息的字节流
  */
std::shared_ptr<BYTE> Message::getSendMSG(int &retLength) {
	BYTE buffer[max_length];
	int tp = 0;
	//构造消息头
	BYTE head[2] = MSG_HEAD;
	for(int i=0; i<2; i++) 
        buffer[tp ++] = head[i];
	//构造版本号
	for(int i=0; i<2; i++) buffer[tp ++] = this -> version[i];
	//构造消息种类
	buffer[tp ++] = this -> type;
	//构造保留字段 8个空字节
	//for(int i=0; i<8; i++) buffer[tp ++] = 0x00;
	//构造消息长度
	std::shared_ptr<BYTE> lengthByte = IntToByte(this -> length);
	for(int i=0; i<4; i++) buffer[tp ++] = lengthByte.get()[i];
	//构造消息
	for(int i=0; i<(this -> length); i++) buffer[tp ++] = this -> data[i];
	//构造CRC
	//BYTE CRC[2] = {0x00};
	//for(int i=0; i<2; i++) buffer[tp ++] = CRC[i];
	std::shared_ptr<BYTE> ret(new BYTE[tp]);
	for(int i=0; i<tp; i++) ret.get()[i] = buffer[i];
	retLength = tp;
	return ret;
}

