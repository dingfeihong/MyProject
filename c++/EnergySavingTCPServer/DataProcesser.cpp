#include "DataProcesser.h"

DataProcesser* DataProcesser::instance = NULL;
mutex DataProcesser::insLock;
class WebClient;

/**
  * 构造函数
  */
DataProcesser::DataProcesser() {
	dataItemList.clear();
	meterVipList.clear();
	lastSend = time(NULL);
	lastSendVip = time(NULL);
	versionId = 0;
}

/**
  * 获取实例
  */
DataProcesser* DataProcesser::getInstance() {
	if(instance == NULL) {
		insLock.lock();
		if(instance == NULL) instance = new DataProcesser();
		insLock.unlock();
	}
	return instance;
}

/**
  * 将列表数据加入dataItemList
  */
void DataProcesser::addData(std::vector<dataItemTs> items) {
	lock_guard<std::mutex> lock(dataMutex);
	for(auto item : items) {
		this -> dataItemList.push_back(item);
	}
}

/**
  * 将数据加入dataItemList
  */
void DataProcesser::addData(dataItemTs item) {
	lock_guard<std::mutex> lock(dataMutex);
	this -> dataItemList.push_back(item);
}

/**
  * 将电表检测数据加入列表
  */
void DataProcesser::addData(meterVipTs item) {
	lock_guard<std::mutex> lock(dataMutex);
	this -> meterVipList.push_back(item);
}

/**
  * 将xml对象加入versionId时间戳
  */
void DataProcesser::addVersionId(shared_ptr<XMLDocument> doc) {
	XMLElement* root = doc -> FirstChildElement("root");
	root -> InsertFirstChild(doc -> NewElement("version_id"));
	root -> FirstChildElement("version_id") -> InsertFirstChild(doc -> NewText(to_string(versionId ++).c_str()));
}

/**
  * 打包xml按照1000行分开
  */
void DataProcesser::packageXML(vector<dataItemTs> &itemList, vector<shared_ptr<Message> > &ret) {
	shared_ptr<XMLDocument> airDoc = createBaseXML();
	shared_ptr<XMLDocument> lightDoc = createBaseXML();
	shared_ptr<XMLDocument> intelligentDoc = createBaseXML();
	shared_ptr<XMLDocument> electricityDoc = createBaseXML();
	shared_ptr<XMLDocument> otherDoc = createBaseXML();
	for(auto item : itemList) {
		XMLElement *root;
		shared_ptr<XMLDocument> doc;
		if(std::get<2>(item) == "空调设备") {
			doc = airDoc;
			root = airDoc -> FirstChildElement("root");
		}else if(std::get<2>(item) == "照明设备") {
			doc = lightDoc;
			root = lightDoc -> FirstChildElement("root");
		}else if(std::get<2>(item) == "智能插座设备") {
			doc = intelligentDoc;
			root = intelligentDoc -> FirstChildElement("root");
		}else if(std::get<2>(item) == "电表") {
			doc = electricityDoc;
			root = electricityDoc -> FirstChildElement("root");
		}else {
			doc = otherDoc;
			root = otherDoc -> FirstChildElement("root");
		}
		root -> InsertEndChild(doc -> NewElement("item"));
		XMLElement *theItem = root -> LastChildElement("item");
		theItem -> InsertFirstChild(doc -> NewElement("device_id"));
		theItem -> FirstChildElement("device_id") 
			-> InsertFirstChild(doc -> NewText(std::get<0>(item).c_str()));
		theItem -> InsertFirstChild(doc -> NewElement("consumption"));
		theItem -> FirstChildElement("consumption") 
			-> InsertFirstChild(doc -> NewText(to_string(std::get<1>(item)).c_str()));
		theItem -> InsertFirstChild(doc -> NewElement("time"));
		theItem -> FirstChildElement("time") 
			-> InsertFirstChild(doc -> NewText(std::get<3>(item).c_str()));
	}
	addVersionId(airDoc);
	addVersionId(lightDoc);
	addVersionId(intelligentDoc);
	addVersionId(electricityDoc);
	addVersionId(otherDoc);
	ofstream out("output.txt", ios::app);
	std::string airStr(xmlDocumentToString(airDoc));
	cout << airStr << endl;
	std::string lightStr(xmlDocumentToString(lightDoc));
	out << lightStr << endl;
	std::string intelligentStr(xmlDocumentToString(intelligentDoc));
	out << intelligentStr << endl;
	std::string electricityStr(xmlDocumentToString(electricityDoc));
	cout << electricityStr << endl;
	std::string otherStr(xmlDocumentToString(otherDoc));
	cout << otherStr << endl;
	Message airMsg(T_UP_AIR, airStr.size(), airStr);
	ret.push_back(make_shared<Message>(airMsg));
	Message lightMsg(T_UP_LIGHT, lightStr.size(), lightStr.c_str());
	ret.push_back(make_shared<Message>(lightMsg));
	Message socketMsg(T_UP_SOCKET, intelligentStr.size(), intelligentStr.c_str());
	ret.push_back(make_shared<Message>(socketMsg));
	Message eleMsg(T_UP_ELE, electricityStr.size(), electricityStr.c_str());
	ret.push_back(make_shared<Message>(eleMsg));
	Message otherMsg(T_UP_OTHER, otherStr.size(), otherStr.c_str());
	ret.push_back(make_shared<Message>(otherMsg));
	out.close();
}

/**
  * 检查函数
  * 功能：(1) 检查是否到达时间
  *		  (2) 若到达时间整合至xml发送
  *		  (3) 按设备种类分类(空调，照明，智能插座，电表，其他设备)
  */
vector<shared_ptr<Message> > DataProcesser::checkIsReady() {
	time_t current = time(NULL);
	vector<shared_ptr<Message> > ret; ret.clear();
	lock_guard<std::mutex> lock(dataMutex);
	cout << "剩余：" << current << ' ' << lastSend << ' ' << dataItemList.size() << endl;
	if(dataItemList.size() == 0 || current - lastSend <= 10) return ret;
	lastSend = current;
	vector<dataItemTs> itemList; itemList.clear();
	for(int i=0; i<dataItemList.size(); i++) {
		if((i + 1)%300 == 0) {	
			cout << "parse: " << itemList.size() << endl;
			packageXML(itemList, ret);	
			itemList.clear();
			usleep(100);
		}
		itemList.PB(dataItemList[i]);
	}
	packageXML(itemList, ret);
	cout << "parse: " << itemList.size() << endl;
	dataItemList.clear();
	return ret;
}

/**
 * 检查函数
 * 定时上传每个电表的检测信息
 */
std::vector<shared_ptr<Message> > DataProcesser::checkIsReadyVip() {
	time_t current = time(NULL);
	std::vector<shared_ptr<Message> > ret;
	ret.clear();
	lock_guard<std::mutex> lock(insLock);
	if(meterVipList.size() == 0 || current - lastSendVip < 10) return ret;
	lastSendVip = current;
	std::shared_ptr<XMLDocument> doc = createBaseXML();	
	XMLElement *root = doc -> FirstChildElement("root");
	addVersionId(doc);
	for(meterVipTs meter : meterVipList) {
		root -> InsertEndChild(doc -> NewElement("item"));
		XMLElement *item = root -> LastChildElement("item");
		item -> SetAttribute("id", std::get<0>(meter).c_str());
		item -> InsertFirstChild(doc -> NewElement("time"));
		item -> FirstChildElement("time") -> InsertFirstChild(doc -> NewText(std::get<1>(meter).c_str()));
		for(pss p : std::get<2>(meter)) {
			if(p.first == "AP") p.first = "AYGZPower";
			if(p.first == "BP") p.first = "BYGZPower";
			if(p.first == "CP") p.first = "CYGZPower";
			if(p.first == "APF") p.first = "AZPowerFactor";
			if(p.first == "BPF") p.first = "BZPowerFactor";
			if(p.first == "CPF") p.first = "CZPowerFactor";
			item -> InsertEndChild(doc -> NewElement(p.first.c_str()));
			item -> FirstChildElement(p.first.c_str()) -> InsertEndChild(doc -> NewText(p.second.c_str()));
		}
	}
	std::string meterVipxml(xmlDocumentToString(doc));
	cout << "------------------------" << endl;
	cout << T_METER_VIP << ' ' << meterVipxml.size() << endl;
	cout << meterVipxml << endl;
	Message msg(T_METER_VIP_UPLOAD, meterVipxml.size(), meterVipxml);
	ret.push_back(make_shared<Message>(msg));
	meterVipList.clear();
	return ret;
}

/**
  * 复制一份device元素到newDoc的data下
  */
void DataProcesser::insertElementInDoc(shared_ptr<XMLDocument> doc, XMLElement* device) {
	XMLElement* data= doc -> FirstChildElement("root") -> FirstChildElement("data");
	data -> InsertFirstChild(doc -> NewElement(device -> Name()));
	XMLElement* newDevice = data -> FirstChildElement(device -> Name());
	newDevice -> SetAttribute("id", device -> Attribute("id"));
	newDevice -> InsertFirstChild(doc -> NewElement("DeviceType"));
	newDevice -> FirstChildElement("DeviceType") -> InsertFirstChild(doc -> NewText(device -> FirstChildElement("DeviceType") -> GetText()));
	XMLElement *attribute = device -> FirstChildElement("DeviceAttributes");
	while(attribute) {
		newDevice -> InsertEndChild(doc -> NewElement("DeviceAttributes"));
		XMLElement *newAttribute = newDevice -> LastChildElement("DeviceAttributes");
		newAttribute -> SetAttribute("id", attribute -> Attribute("id"));
		XMLElement *child = attribute -> FirstChildElement();
		while(child) {
			newAttribute -> InsertEndChild(doc -> NewElement(child -> Name()));
			newAttribute -> LastChildElement(child -> Name()) -> InsertEndChild(doc -> NewText(child -> GetText()));
			child = child -> NextSiblingElement();
		}
		attribute = attribute -> NextSiblingElement();
	}
}

/**
  * 处理一条设备信息
  * 输入：消息xml对象
  * 输出：拆分后的消息列表
  */
std::vector<shared_ptr<Message> > DataProcesser::DeviceDataProcessing(shared_ptr<XMLDocument> doc) {
	std::vector<shared_ptr<Message> > ret; ret.clear();
	pss common = getCommonContentGH(xmlDocumentToString(doc).c_str());
	shared_ptr<XMLDocument> newDoc = NULL;
	XMLElement *data = NULL;
	int cnt = 0;
	XMLElement *device = doc -> FirstChildElement("root") -> FirstChildElement("data") -> FirstChildElement("Device");
	while(device) {
		if(cnt % 5 == 0) {
			if(newDoc) {
				addVersionId(newDoc);
				std::string xmldoc = xmlDocumentToString(newDoc);
				Message msg(T_GH_DEVICE, xmldoc.size(), xmldoc);
				ret.PB(make_shared<Message>(msg));
			}
			newDoc = createCommonXML(common.first, common.second);
		}
		//复制一份device元素到newDoc的data下
		insertElementInDoc(newDoc, device);
		cnt ++;
		device = device -> NextSiblingElement();
	}
	//cout << xmlDocumentToString(newDoc) << endl;
	if(cnt < 5) {
		addVersionId(doc);
		std::string xmldoc = xmlDocumentToString(doc);
		Message msg(T_GH_DEVICE, xmldoc.size(), xmldoc);
		ret.PB(make_shared<Message>(msg));
	}
	return ret;
}

/**
  * 检查函数 光华楼设备数据
  * 功能：(1) 检查是否到达时间
  *		  (2) 从文件中检查数据发送
  */
std::vector<shared_ptr<Message> > DataProcesser::checkIsReadyDevice() {
	PersistentSet<string> pset(FILENAME);
	int ec;
	vector<std::string> DeviceXMLNames = getDeviceFiles();
	vector<shared_ptr<Message> > ret; ret.clear();
	for(string name : DeviceXMLNames) if(!pset.isExist(name)) {
		shared_ptr<XMLDocument> doc = make_shared<XMLDocument>(new XMLDocument());
		pset.insertItem(name);
		string path = "deviceData/" + name;
		cout << path << " is processed" << endl;
		ec = doc -> LoadFile(path.c_str());
		if(ec) continue;
		vector<shared_ptr<Message>> msg = DeviceDataProcessing(doc);
		for(int i=0; i<msg.size(); i++) ret.PB(msg[i]);
		sleep(1);
	}
	return ret;
}

/**
  * 处理一条能耗信息
  * 输入：消息xml对象
  */
void DataProcesser::EnergyDataProcessing(shared_ptr<XMLDocument> doc) {
	DeviceManager *manager = DeviceManager::getInstance();
	DataProcesser *processer = DataProcesser::getInstance();
	XMLElement *device = doc -> FirstChildElement("root") -> FirstChildElement("data")
		-> FirstChildElement("Device");
	while(device) {
		XMLElement *item = device -> FirstChildElement("EnergyItemResult");
		std::string meterId = device -> Attribute("id");
		while(item) {
			std::string time = item -> FirstChildElement("F_GATHERTIME") -> GetText();
			std::string meterCost = item -> FirstChildElement("F_DATAVALUE") -> GetText();
			std::string statue = item -> FirstChildElement("F_STATUS") -> GetText();
			if(statue == "1") {
				vector<dataItem> vd = manager -> getDeviceConsumption(meterId, atof(meterCost.c_str()));
				for(auto item : vd) {
					processer -> addData(make_tuple(get<0>(item), get<1>(item), get<2>(item), TimeTrans(time)));
				}
			}
			item = item -> NextSiblingElement();
		}
		device = device -> NextSiblingElement();
	}
}

/**
 * 检查函数 光华楼能耗数据
 * 功能：(1) 检查是否到达时间
 *       (2) 依次能耗数据
 */
void DataProcesser::checkIsReadyEnergy() {
	PersistentSet<string> pset(FILENAME);
	int ec;
	vector<std::string> DataXMLNames = getDataFiles();
	for(string name : DataXMLNames) if(!pset.isExist(name)){
		shared_ptr<XMLDocument> doc = make_shared<XMLDocument>(new XMLDocument());
		string path = "data/" + name;
		ec = doc -> LoadFile(path.c_str());
		if(ec) continue;
		EnergyDataProcessing(doc);
		pset.insertItem(name);
		usleep(100);
	}
}

