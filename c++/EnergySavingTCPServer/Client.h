#ifndef CLIENT_H
#define CLIENT_H

#include "WebClient.h"

/**
  * 客户端认证类 ： Asio封装类
  */
class Client : public AsioPlus {
	public:
		//构造函数
		Client(io_service &io_service);
		//客户端认证
		bool verification();
		//处理心跳包
		bool processHeartBeat(std::shared_ptr<Message> message);
		//处理能耗数据
		bool processEnergyData(std::shared_ptr<Message> message);
		//处理电表检测数据
		bool processMeterVip(std::shared_ptr<Message> message);
		//处理验证通过后的消息
		bool processMessage(std::shared_ptr<Message> message);
		//获取楼栋编号
		std::string getBuildingId();
		//获取网关编号
		std::string getGatewayId();
		//从xml中选出电表电量数据
		void selectDataFromXML(shared_ptr<XMLDocument> doc);
		//从xml中选出电表检测数据
		void selectMeterVipFromXML(shared_ptr<XMLDocument> doc);

	private :
		//楼栋编号
		std::string buildingId;
		//网关编号
		std::string gatewayId;
		//客户端的上次活跃时间
		ll lastActiveTime;
};

#endif
