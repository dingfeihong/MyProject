#ifndef MESSAGE_H
#define MESSAGE_H

#include "tools.h"

//消息头
#define MSG_HEAD {0x1F, 0x1F}
//通信协议版本号
#define MSG_VERSION {0x00, 0x01}
/**
 * 消息种类
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
 */
#define T_ID_VALIDATE 1
#define T_HEART_BEAT 2
#define T_ENERGY_DATA 3
#define T_PERIOD 4
#define T_METER_CONTROL 5
#define T_METER_CONTROL_ANS 85
#define T_METER_VIP 6
#define T_METER_VIP_UPLOAD 86
#define T_ELEC_SAFETY_DATA 7
#define T_TEMP_CONTROL 8
#define T_ELEC_COMTROL 9
#define T_DOOR_SHOW 10
#define T_APP_UPDATE 11
//数据缓存
#define T_DEV 100
#define T_AREA_DEV 101
#define T_DEP_DEV 102
#define T_AREA 103
#define T_DEP 104
//能耗上传
#define T_UP_AIR 110
#define T_UP_LIGHT 111
#define T_UP_SOCKET 112
#define T_UP_ELE 113
#define T_UP_OTHER 114
//光华楼
#define T_GH_DEVICE 115


typedef unsigned char BYTE;

class Message {
	private:
		BYTE *version;
		BYTE type;
		int length;
		char *data;

	public:
		//构造函数
		Message(BYTE _type, int _length, const char *&_data);
		Message(int _type, int _length, string str);
		//深拷贝构造函数
		Message(const Message &msg);
		~Message();
		char* getVersion();
		int getType();
		void setType(int _type);
		int getLength();
		char* getData();
		std::shared_ptr<BYTE> getSendMSG(int &retLength);

		void decryptData();
};

#endif
