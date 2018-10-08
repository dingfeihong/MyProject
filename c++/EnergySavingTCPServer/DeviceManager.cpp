#include "DeviceManager.h"

DeviceManager* DeviceManager::instance;
mutex DeviceManager::insLock;

/**
  * 初始化函数
  */
void DeviceManager::init() {
	deviceId.clear();
	deviceType.clear();
	parentId.clear();
	meterDevices.clear();
	meterPower.clear();
	deviceArea.clear();
	deviceDep.clear();
	area.clear();
	dep.clear();
	meterRecord.clear();
}

/**
  * 构造函数
  * 从device.xml中读入
  *
  */
DeviceManager::DeviceManager() {
	int ec;
	this -> md_versionId = 0;
	this -> da_versionId = 0;
	this -> dd_versionId = 0;
	this -> a_versionId = 0;
	this -> d_versionId = 0;
	init();
	shared_ptr<XMLDocument> md_doc(new XMLDocument());
	shared_ptr<XMLDocument> da_doc(new XMLDocument());
	shared_ptr<XMLDocument> dd_doc(new XMLDocument());
	shared_ptr<XMLDocument> a_doc(new XMLDocument());
	shared_ptr<XMLDocument> d_doc(new XMLDocument());
	ec = md_doc -> LoadFile("xml/Device.xml");
	if(!ec) XMLtoMenMeterDevice(md_doc);
	return ;
	ec = da_doc -> LoadFile("/xml/DeviceArea.xml");
	if(!ec) XMLtoMenDeviceArea(da_doc);
	ec = dd_doc -> LoadFile("/xml/DeviceDep.xml");
	if(!ec) XMLtoMenDeviceDep(dd_doc);
	ec = a_doc -> LoadFile("/xml/Area.xml");
	if(!ec) XMLtoMenArea(a_doc);
	ec = d_doc -> LoadFile("/xml/Dep.xml");
	if(!ec) XMLtoMenDep(d_doc);
}

/**
  * 获取实例
  */
DeviceManager* DeviceManager::getInstance() {
	if(instance == NULL) {
		insLock.lock();
		if(instance == NULL) {
			instance = new DeviceManager();
		}
		insLock.unlock();
	}
	return DeviceManager::instance;
}

/**
  * 从文件中载入数据
  */
void DeviceManager::XMLtoMenMeterDevice(shared_ptr<XMLDocument> doc) {
	XMLElement *version = doc -> FirstChildElement("root") -> FirstChildElement("version_id");
	long long ver = atoll(version -> GetText());
	if(this -> md_versionId > ver) return ;
	this -> md_versionId = ver;
	rwmu.lock();
	deviceId.clear();
	deviceType.clear();
	parentId.clear();
	meterDevices.clear();
	meterPower.clear();
	XMLElement *item = doc -> FirstChildElement("root") -> FirstChildElement("item");
	while(item -> NextSiblingElement()) {
		std::string d_id = item -> FirstChildElement("device_id") -> GetText();
		deviceId.push_back(d_id);
		//std::string d_type = item -> FirstChildElement("device_type") -> GetText();
		//deviceType.push_back(d_type);
		std::string d_type = item -> FirstChildElement("power_name") -> GetText();
		deviceType.push_back(d_type);
		std::string p_id = item -> FirstChildElement("parent_dev_id") -> GetText();
		parentId.push_back(p_id);
		meterDevices[p_id].push_back(make_pair(d_id, d_type));
		//记录能源分项
		if(p_id == "-1") meterPower[d_id] = d_type;
		item = item -> NextSiblingElement();
	}
	doc -> SaveFile("xml/Device.xml");
	rwmu.unlock();
}

/**
  * 载入设备区域关系
  *
  */
void DeviceManager::XMLtoMenDeviceArea(shared_ptr<XMLDocument> doc) {
	XMLElement *version = doc -> FirstChildElement("root") -> FirstChildElement("version_id");
	long long ver = atoll(version -> GetText());
	if(this -> da_versionId > ver) return ;
	this -> da_versionId = ver;
	rwmu.lock();
	deviceArea.clear();
	XMLElement *item = doc -> FirstChildElement("root") -> FirstChildElement("item");
	while(item -> NextSiblingElement()) {
		std::string d_id = item -> FirstChildElement("device_id") -> GetText();
		int a_id = atoi(item -> FirstChildElement("area_id") -> GetText());
		double pof = atof(item -> FirstChildElement("percent_of_energy") -> GetText());
		deviceArea[d_id].push_back(make_pair(a_id, pof));
		item = item -> NextSiblingElement();
	}
	doc -> SaveFile("xml/DeviceArea.xml");
	rwmu.unlock();
}

/**
  * 载入设备部门关系
  *
  */
void DeviceManager::XMLtoMenDeviceDep(shared_ptr<XMLDocument> doc) {
	XMLElement *version = doc -> FirstChildElement("root") -> FirstChildElement("version_id");
	long long ver = atoll(version -> GetText());
	if(this -> dd_versionId > ver) return ;
	this -> dd_versionId = ver;
	rwmu.lock();
	deviceDep.clear();
	XMLElement *item = doc -> FirstChildElement("root") -> FirstChildElement("item");
	while(item -> NextSiblingElement()) {
		std::string d_id = item -> FirstChildElement("device_id") -> GetText();
		int a_id = atoi(item -> FirstChildElement("department_id") -> GetText());
		double pof = atof(item -> FirstChildElement("percent_of_energy") -> GetText());
		deviceDep[d_id].push_back(make_pair(a_id, pof));
		item = item -> NextSiblingElement();
	}
	doc -> SaveFile("xml/DeviceDep.xml");
	rwmu.unlock();
}

/**
  * 载入区域表
  *
  */
void DeviceManager::XMLtoMenArea(shared_ptr<XMLDocument> doc) {
	XMLElement *version = doc -> FirstChildElement("root") -> FirstChildElement("version_id");
	long long ver = atoll(version -> GetText());
	if(this -> a_versionId >= ver) return ;
	this -> a_versionId = ver;
	rwmu.lock();
	area.clear();
	XMLElement *item = doc -> FirstChildElement("root") -> FirstChildElement("item");
	while(item -> NextSiblingElement()) {
		int a_id = atoi(item -> FirstChildElement("area_id") -> GetText());
		int p_id = atoi(item -> FirstChildElement("parent_id") -> GetText());
		area[a_id] = p_id;
		item = item -> NextSiblingElement();
	}
	doc -> SaveFile("xml/Area.xml");
	rwmu.unlock();
}

/**
  * 载入部门表
  *
  */
void DeviceManager::XMLtoMenDep(shared_ptr<XMLDocument> doc) {
	XMLElement *version = doc -> FirstChildElement("root") -> FirstChildElement("version_id");
	long long ver = atoll(version -> GetText());
	if(this -> d_versionId >= ver) return ;
	this -> d_versionId = ver;
	rwmu.lock();
	dep.clear();
	XMLElement *item = doc -> FirstChildElement("root") -> FirstChildElement("item");
	while(item -> NextSiblingElement()) {
		int d_id = atoi(item -> FirstChildElement("dep_id") -> GetText());
		int p_id = atoi(item -> FirstChildElement("parent_id") -> GetText());
		dep[d_id] = p_id;
		item = item -> NextSiblingElement();
	}
	doc -> SaveFile("xml/Dep.xml");
	rwmu.unlock();
}

/**
  * 打印设备信息
  *
  */
void DeviceManager::md_print() {
	rwmu.lock_shared();
	cout << "md_print: " << endl;
	cout << "version: " << this -> md_versionId << endl;
	for(int i=0; i<deviceId.size(); i++) {
		cout << deviceId[i] << ' ' << deviceType[i] << ' ' << parentId[i] << endl;
	}
	cout << endl;
	cout << "meter: " << endl;
	for(auto it : meterDevices) {
		cout << it.first << ' ';
		for(auto dev : it.second) {
			cout << " (" << dev.first << "," << dev.second << ") ";
		}
	}
	cout << endl;
	rwmu.unlock_shared();
}

/**
  * 打印设备区域信息
  *
  */
void DeviceManager::da_print() {
	rwmu.lock_shared();
	cout << "da_print: " << endl;
	cout << "version: " << da_versionId << endl;
	for(auto it : deviceArea) {
		cout << it.first << ' ';
		for(auto area : it.second) {
			cout << " (" << area.first << "," << area.second << ") ";
		}
	}
	cout << endl;
	rwmu.unlock_shared();
}

/**
  * 打印设备部门信息
  *
  */
void DeviceManager::dd_print() {
	rwmu.lock_shared();
	cout << "dd_print: " << endl;
	cout << "version: " << dd_versionId << endl;
	for(auto it : deviceDep) {
		cout << it.first << ' ';
		for(auto dep : it.second) {
			cout << " (" << dep.first << "," << dep.second << ") ";
		}
	}
	cout << endl;
	rwmu.unlock_shared();
}

/**
  * 打印区域信息
  *
  */
void DeviceManager::a_print() {
	rwmu.lock_shared();
	cout << "a_print: " << endl;
	cout << "version: " << this -> a_versionId << endl;
	for(auto it : area) {
		cout << it.first << ' ' << it.second << endl;
	}
	cout << endl;
	rwmu.unlock_shared();
}

/**
  * 打印部门信息
  *
  */
void DeviceManager::d_print() {
	rwmu.lock_shared();
	cout << "d_print: " << endl;
	cout << "version: " << this -> d_versionId << endl;
	for(auto it : dep) {
		cout << it.first << ' ' << it.second << endl;
	}
	cout << endl;
	rwmu.unlock_shared();
}

/**
  * 给定电表耗电量计算各设备耗电量
  * 输入：电表id 电表耗电量
  * 输出：设备耗电量列表
  */
vector<dataItem> DeviceManager::getDeviceConsumption(std::string id, double con) {
	rwmu.lock_shared();
	vector<dataItem> ret; ret.clear();
	if(!meterRecord.count(id)) {
		meterRecord[id] = con;
		con = 0;
	}else {
		con -= meterRecord[id];
		meterRecord[id] += con;
	}
	if(con < 0) {
		cout << "con is nagtive!" << endl;
		con = 0;
	}
	//他是一个电表
	if(meterDevices.count(id)) {
		vector<pss> &md = meterDevices[id];
		ret.push_back(make_tuple(id, con, "电表"));
		for(pss mdi : md) {
			ret.push_back(make_tuple(mdi.first, con / md.size(), mdi.second));
		}
	}else {
		//如果该电表下无设备或者他不是一个电表，直接按照能源分项来统计
		if(meterPower.count(id)) ret.push_back(make_tuple(id, con, meterPower[id]));
		ret.push_back(make_tuple(id, con, "电表"));
	}
	rwmu.unlock_shared();
	return ret;
}

