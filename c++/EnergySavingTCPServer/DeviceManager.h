#ifndef DEVICEMANAGER_H
#define DEVICEMANAGER_H

#include "tools.h"

/**
  * 设备信息缓存处理类 单例
  *
  */
class DeviceManager {
	private:
		//当前的版本号
		long long md_versionId;
		long long da_versionId;
		long long dd_versionId;
		long long a_versionId;
		long long d_versionId;
		vector<std::string> deviceId;
		vector<std::string> deviceType;
		vector<std::string> parentId;
		//电表下的设备列表
		map<std::string, vector<pss> > meterDevices;
		//电表的能源分项
		map<std::string, string> meterPower;
		//设备属于的区域
		map<std::string, vector<pair<int, double> > > deviceArea;
		//设备属于的部门
		map<std::string, vector<pair<int, double> > > deviceDep;
		//区域表
		map<int, int> area;
		//部门表
		map<int, int> dep;
		//之前电表的读数
		map<std::string, double> meterRecord;

		//读写锁
		boost::shared_mutex rwmu;

		//初始化函数
		void init();

		//构造函数
		DeviceManager();

		//私有化复制构造函数和=操作符
		DeviceManager(const DeviceManager& dm);
		DeviceManager& operator=(const DeviceManager& dm);

		static DeviceManager* instance;
		static mutex insLock;

	public:
		//获取实例
		static DeviceManager* getInstance();

		//从xml中载入数据
		void XMLtoMenMeterDevice(shared_ptr<XMLDocument> doc);
		void XMLtoMenDeviceArea(shared_ptr<XMLDocument> doc);
		void XMLtoMenDeviceDep(shared_ptr<XMLDocument> doc);
		void XMLtoMenArea(shared_ptr<XMLDocument> doc);
		void XMLtoMenDep(shared_ptr<XMLDocument> doc);
		//打印函数
		void md_print();
		void da_print();
		void dd_print();
		void a_print();
		void d_print();
		//给定电表耗电量计算各设备耗电量
		vector<dataItem> getDeviceConsumption(std::string id, double con);
};

#endif
