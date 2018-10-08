#ifndef DATAPROCESSER_H
#define DATAPROCESSER_H

#include "Message.h"
#include "PersistentSet.h"
#include "DeviceManager.h"

/**
  * 负责定时处理任务 单例
  * (1) 定时合并消息队列中的消息投送
  * (2) 定时计算部门区域数据（若需要）
  *
  */
class DataProcesser {
	private:
		//待处理的数据
		std::vector<dataItemTs> dataItemList;
		//带处理的三相数据
		std::vector<meterVipTs> meterVipList;
		//私有化构造函数
		DataProcesser();
		//私有化赋值构造函数和=运算符
		DataProcesser(const DataProcesser &dp);
		DataProcesser& operator=(const DataProcesser &dp);

		atomic<long long> versionId;

		std::mutex dataMutex;
		//实例
		static DataProcesser* instance;
		static std::mutex insLock;

		time_t lastSend, lastSendVip;

		//私有函数
		//给xml添加一条时间戳
		void addVersionId(shared_ptr<XMLDocument> doc);
		//处理一条设备信息
		std::vector<shared_ptr<Message> > DeviceDataProcessing(shared_ptr<XMLDocument> doc);
		//复制一份device元素到newDoc的data下
		void insertElementInDoc(shared_ptr<XMLDocument> doc, XMLElement* device);
		//处理一条能耗信息
		void EnergyDataProcessing(shared_ptr<XMLDocument> doc);
		//打包一次能耗数据
		void packageXML(vector<dataItemTs> &itemList, vector<shared_ptr<Message> > &ret);

	public:
		static DataProcesser* getInstance();

		//将列表中数据加入dataItemList
		void addData(std::vector<dataItemTs> items);
  		//将数据加入dataItemList
		void addData(dataItemTs item);
		//将电表检测数据加入列表
		void addData(meterVipTs item);
		//检查函数 定时整合成xml投送队列
		std::vector<shared_ptr<Message> > checkIsReady();
		//检查函数 定时整合成xml投送队列 三相数据
		std::vector<shared_ptr<Message> > checkIsReadyVip();
		//检查函数 定时合成光华楼设备数据
		std::vector<shared_ptr<Message> > checkIsReadyDevice();
		//检查函数 定时合成光华楼能耗数据
		void checkIsReadyEnergy();
};

#endif
