#ifndef TOOLS_H
#define TOOLS_H

#include <bits/stdc++.h>
#include <openssl/md5.h>
#include <openssl/aes.h>
#include <boost/asio.hpp>
#include <boost/bind.hpp>
#include <boost/thread/locks.hpp>
#include <boost/thread/shared_mutex.hpp>
#include <boost/thread/thread.hpp>
#include <unistd.h>
#include <dirent.h>

#include "tinyxml2.h"

#define MP(a, b) make_pair(a, b)
#define PB(a) push_back(a)

using namespace std;
using namespace tinyxml2;
using namespace boost::asio;
typedef unsigned char BYTE;
typedef long long ll;
typedef std::shared_ptr<ip::tcp::socket> socket_ptr;
typedef std::pair<int, int> pii;
typedef std::pair<std::string, std::string> pss;
typedef std::pair<std::string, double> psd;
//无时间戳的数据项(设备号，电量，设备类型)
typedef std::tuple<std::string, double, std::string> dataItem;
//带时间戳的数据项(设备号，电量，设备类型，时间戳)
typedef std::tuple<std::string, double, std::string, std::string> dataItemTs;
//带时间戳的三相数据(设备号，时间戳，三相数据)
typedef std::tuple<std::string, std::string, vector<pss>> meterVipTs;

//传送加密密文
const std::string PASSWORD = "iBTpRJ001iTEM001";
//消息最大长度
const int max_length = 1000000;

#define CLIENTID "client1"

/**
  * 通用字符常量
  */
#define _RT_ "root"
#define _CM_ "common"
#define _BID_ "building_id"
#define _GID_ "gateway_id"
#define _TP_ "type"
#define _UPCID_ "UploadDataCenterID"
#define _CT_ "CreateTime"

/**
  * 消息种类列表
  */
#define ID_V "id_validate"
#define HEART_B "heart_beat"
#define ENERGY_D "energy_data"
/*配置信息有两种*/
#define PERIOD "period"
#define PERIOD_A "period_ack"
#define METER_C "meter_control"
#define METER_V "meter_vip"
#define ELEC_S_D "elec_safety_data"
#define TEMP_C "temp_control"
#define ELEC_C "elec_control"
#define DOOR_S "door_show"
#define APP_U "app_update"

//持久化文件
#define FILENAME "set.txt"

//获取md5
string getMd5(string data);
//获取指定长度随机串
string getRandomString(int len);
//获取给定xml文档的根节点
XMLElement * getXMLRootElement(const char *data);
//将整形转换为字节流
shared_ptr<BYTE> IntToByte(int num);
//返回消息中的pair<string, string>(buildingId, gatewayId)
pss getCommonContent(const char *data);
//返回光华楼消息中的pair<string, string>(UploadDataCenterID, CreateTime)
pss getCommonContentGH(const char *data);
//返回一个空的XML
std::shared_ptr<XMLDocument> createBaseXML();
//返回一个带有固定common元素的xml
std::shared_ptr<XMLDocument> createCommonXML(std::string buildingId, std::string gatewayId, std::string type);
//返回一个带有固定common元素的xml（光华楼）
std::shared_ptr<XMLDocument> createCommonXML(std::string UPCId, std::string createTime);
//返回一条消息中的versionId元素
std::string getVersionIdFromXML(char *data);
//获取当前时间的格式串
std::string getTimeString();
//获取XMLDocument的字符串指针
std::string xmlDocumentToString(std::shared_ptr<XMLDocument> xml);
//将大写字母的字符串转化为全是小写的字符串
string toLowerCase(string str);
//aes解密函数
char* aesDecrypt(const char *content, int &len);
//aes加密函数
char* aesEncrypt(std::string content, int &len);
//获取某一目录下的所有文件名
vector<std::string> getFiles(std::string cate_dir);
//获取光华楼数据文件
vector<std::string> getDataFiles();
//获取光华楼设备文件
vector<std::string> getDeviceFiles();
//将光华楼的时间戳转换为正常时间戳
std::string TimeTrans(string time);
//测试函数
void out(BYTE* text, int lenght);

#endif
