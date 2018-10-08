#ifndef WEBCLIENT_H
#define WEBCLIENT_H

#include "AsioPlus.h"
#include "DataProcesser.h"

class DataProcesser;

/**
  * WebClient 类 负责和web服务器交互信息
  * 需要单例化
  * 功能：(1) 接受并推送电表控制信息
  *       (2) 接受并缓存更新数据表
  *       (3) 上传处理完毕的能耗数据
  */
class WebClient : public AsioPlus{
	private:
		static std::map<string, queue<Message> > receiveMessage;
		static std::mutex receiveMutex;

		static std::queue<Message> sendMessage;
		static std::mutex sendMutex;

		//(versionId, Message)已经发送的消息
		map<string, shared_ptr<Message> > hasSend[2];

		//连接服务器函数
		bool connectServer(ip::tcp::endpoint &ep);
		clock_t lastConnectTime;
		clock_t lastCheckTime;

	public:
		//放入收到消息队列
		static void putReceiveMessage(std::string gatewayId, shared_ptr<Message> msg);
		//测试收到消息队列是否为空
		static bool isReceiveEmpty(std::string gatewayId);
		//取出收到消息队列
		static shared_ptr<Message> getReceiveMessage(std::string gatewayId);
		//放入发送消息队列
		static void putSendMessage(shared_ptr<Message> msg, bool isCheck);
		//测试发送消息队列是否为空
		static bool isSendEmtpy();
		//取出发送消息队列
		static shared_ptr<Message> getSendMessage();


		//构造函数，负责变量初始化和自启工作线程
		WebClient(io_service &io_service);
		//工作函数
		void clientWork();
		//发送客户端id
		void sendClientId();
		//发送心跳包
		bool sendHeartBeat(boost::system::error_code &ec);
		//重传检查函数
		bool reSend();
		//处理收到的信息
		void processMessage(shared_ptr<Message> msg);
		//处理电表控制信息
		void receiveMeterControl(shared_ptr<Message> msg);
		//处理设备缓存信息
		void receiveDevice(shared_ptr<Message> msg);

};

#endif
