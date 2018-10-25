#include "Client.h"

/**
 * 构造函数
 * 调用父级函数初始化一个sock对象
 *
 */
Client::Client(io_service &io_service) : AsioPlus(io_service) {
	this -> buildingId = "NULL";
	this -> gatewayId = "NULL";
	this -> lastActiveTime = time(NULL);
}

/**
 * 客户端认证
 * 返回值 ： bool 认证是否成功
 */
bool Client::verification() {
	boost::system::error_code ec;
	//Step1: 接受客户端请求，获取客户端common数据
	std::shared_ptr<Message> message(this -> readAMessage(ec));	
	if(ec) {
        cout<<"client verification failed"<<endl;
        return false;
    }
	pss ids = getCommonContent(message -> getData());
	this -> buildingId = ids.first;
	this -> gatewayId = ids.second;
	//std::cout << this -> buildingId << ' ' << this -> gatewayId << std::endl;

	//Step2: 生成一串随机序列发送值客户端
	string randSQ = getRandomString(8);
	std::shared_ptr<XMLDocument> doc(new XMLDocument());
	doc -> Parse(message -> getData());
	XMLElement *sequence = doc -> NewElement("sequence");
	sequence -> InsertFirstChild(doc -> NewText(randSQ.c_str()));
	doc -> FirstChildElement() -> FirstChildElement("id_validate") -> InsertFirstChild(sequence);
	doc -> FirstChildElement() -> FirstChildElement("id_validate") -> SetAttribute("operation", "sequence");
	string xmlString(xmlDocumentToString(doc));
	//cout << xmlString << endl;
	//输出不带消息头的xml,下面需要加上消息头
	sendXML(xmlString.c_str(), xmlString.size(), T_ID_VALIDATE, ec);

	//Step3:客户端将接受到的随机串和本地存储的认证密钥链接组合成一串计算md5发送给服务器
	std::shared_ptr<Message> md5Message(this -> readAMessage(ec));
	XMLElement *md5Root = getXMLRootElement(md5Message -> getData());
	std::string clientMd5 = md5Root -> FirstChildElement("id_validate") -> FirstChildElement() -> GetText();
	//std::cout << "clientMd5: " << clientMd5 << std::endl;

	//Step4:服务器本地计算结果并比对是否相等
	std::string localComputeMd5 = getMd5(PASSWORD + randSQ);
	//std::cout << "serverMd5: " << localComputeMd5 << std::endl;
	//验证成功
	if(toLowerCase(clientMd5) == toLowerCase(localComputeMd5)) {
		std::shared_ptr<XMLDocument> answer = createCommonXML(this -> buildingId, this -> gatewayId, "id_validate");
		XMLElement *id_v = answer -> NewElement("id_validate");
		XMLElement *result = answer -> NewElement("result");
		answer -> InsertFirstChild(answer -> NewText(" "));
		result -> InsertFirstChild(answer -> NewText("pass"));
		answer -> FirstChildElement("root") -> InsertEndChild(id_v);
		answer -> FirstChildElement("root") -> FirstChildElement("id_validate") -> InsertFirstChild(result);
		answer -> FirstChildElement("root") -> FirstChildElement("id_validate") -> SetAttribute("operation", "result");
		string ansString(xmlDocumentToString(answer));
		ansString = ansString.substr(4);
		//cout << ansString << endl;
		sendXML(ansString.c_str(), ansString.size(), T_ID_VALIDATE, ec);
		cout << this -> buildingId << " 验证成功" << endl;
		return true;
	}
	return false;
}

/**
 * 处理心跳包
 * step1: 接受心跳包
 * step2: 返回服务器时间
 * 输入值: 心跳包消息
 * 输出值: 处理是否成功
 */
bool Client::processHeartBeat(std::shared_ptr<Message> message) {
	boost::system::error_code ec;
	std::shared_ptr<XMLDocument> doc(new XMLDocument());
	doc -> Parse(message -> getData());
	XMLElement* root = doc -> FirstChildElement("root");
	XMLElement* heartElement = root -> LastChildElement();
	heartElement -> SetAttribute("operation", "time");
	heartElement -> InsertFirstChild(doc -> NewElement("time"));
	heartElement = heartElement -> FirstChildElement("time");
	heartElement -> InsertFirstChild(doc -> NewText(getTimeString().c_str()));
	string xmlString(xmlDocumentToString(doc));
	sendXML(xmlString.c_str(), xmlString.size(), T_HEART_BEAT, ec);
	cout << "收到" << this -> buildingId << "心跳数据" << endl;
	this -> lastActiveTime = time(NULL);
	return true;
}

/**
  * 从xml中选出电表电量数据
  * step1: 从xml中解析出电表数据
  * step2: 将电表数据发送到DataProcesser中
  *
  */
void Client::selectDataFromXML(shared_ptr<XMLDocument> doc) {
	XMLElement *data = doc -> FirstChildElement("root") -> FirstChildElement("data");
	string operation = data -> Attribute("operation");
	if(operation == "finish") return ;
	string time = data -> FirstChildElement("time") -> GetText();
	XMLElement *meter = data -> FirstChildElement("meters") -> FirstChildElement("meter");
	DeviceManager *manager = DeviceManager::getInstance();
	DataProcesser *processer = DataProcesser::getInstance();
	while(meter) {
		string meterId = meter -> Attribute("id");
		string meterName = meter -> Attribute("name");
		double meterCost = atof(meter -> FirstChildElement("function") -> GetText());
		vector<dataItem> vd = manager -> getDeviceConsumption(meterId, meterCost);
		for(auto item : vd) {
			processer -> addData(make_tuple(get<0>(item), get<1>(item), get<2>(item), time));
		}
		meter = meter -> NextSiblingElement();
	}

}

/**
  * 处理能耗数据
  * step1：构造xmldoc对象
  * step2：返回应答信息，失败要求重传
  * 输入值：能耗数据包信息
  * 输出值：处理是否成功
  */
bool Client::processEnergyData(std::shared_ptr<Message> message) {
	boost::system::error_code ec;
	std::shared_ptr<XMLDocument> doc(new XMLDocument());
	if(doc -> Parse(message -> getData()) != 0) {
		std::cout << "XML parse error!" << std::endl;
		return false;
	}
	// 从xml中选出电表电量数据
	selectDataFromXML(doc);
	int len = message -> getLength();
	XMLElement* data = doc -> FirstChildElement("root") -> FirstChildElement("data");
	if(data -> FirstChildElement("energy_items")) {
		data -> DeleteChild(data -> FirstChildElement("energy_items"));
	}
	if(data -> FirstChildElement("meters")) {
		data -> DeleteChild(data -> FirstChildElement("meters"));
	}
	data -> InsertFirstChild(doc -> NewElement("ack"));
	data -> FirstChildElement("ack") -> InsertEndChild(doc -> NewText("OK"));	
	string xmlString(xmlDocumentToString(doc));
	//cout << xmlString << endl;
	xmlString.pop_back();
	int length = xmlString.size();
	char *tmp = aesEncrypt(xmlString.c_str(), length);
	sendXML(tmp, length, T_ENERGY_DATA, ec);
	cout << "收到" << this -> buildingId << "能耗数据" << endl;
	return true;
}

void Client::selectMeterVipFromXML(shared_ptr<XMLDocument> doc) {
	XMLElement *data = doc -> FirstChildElement("root") -> FirstChildElement("data");
	std::string time = data -> FirstChildElement("time") -> GetText();
	XMLElement *meter = data -> FirstChildElement("data_items");
	DataProcesser *processer = DataProcesser::getInstance();
	while(meter) {
		string meterId = meter -> Attribute("meter");
		//暂时不需要
		string meterType = meter -> Attribute("metertype");
		vector<pss> vs; vs.clear();
		XMLElement *it = meter -> FirstChildElement("item");
		while(it) {
			string one = it -> Attribute("code");
			string two = it -> GetText();
			vs.push_back(make_pair(one, two));
			it = it -> NextSiblingElement();
		}
		processer -> addData(make_tuple(meterId, time, vs));
		meter = meter -> NextSiblingElement();
	}
}

/**
 * 处理电表检测数据
 * 输入值：电表检测数据包
 * 输出值：处理是否成功
 */
bool Client::processMeterVip(std::shared_ptr<Message> message) {
	boost::system::error_code ec;
	std::shared_ptr<XMLDocument> doc(new XMLDocument());
	if(doc -> Parse(message -> getData()) != 0) {
		std::cout << "XML parse error!" << std::endl;
		return false;
	}
	selectMeterVipFromXML(doc);
	XMLElement *root = doc -> FirstChildElement("root") -> FirstChildElement("data");
	while(root -> FirstChildElement("data_items")) {
		root -> DeleteChild(root -> FirstChildElement("data_items"));
	}
	root -> SetAttribute("operation", "result");
	root -> InsertEndChild(doc -> NewElement("ack"));
	root -> FirstChildElement("ack") -> InsertFirstChild(doc -> NewText("ok"));
	std::string xmlString = xmlDocumentToString(doc);
	//cout << xmlString << endl;
	int length = xmlString.size();
	char *tmp = aesEncrypt(xmlString.c_str(), length);
	sendXML(tmp, length, T_METER_VIP, ec);
	cout << "收到" << this -> buildingId << "电表数据" << endl;
	return true;
}

/**
 * 处理验证通过后的消息
 * 输入：Message指针
 * 输出：消息的处理结果
 */
bool Client::processMessage(std::shared_ptr<Message> message) {
	bool ret = false;
	//cout << message -> getType() << endl;
	switch(message -> getType()) {
		case T_HEART_BEAT:
			ret = processHeartBeat(message);
			break;
		case T_ENERGY_DATA:
			ret = processEnergyData(message);
			break;
		case T_PERIOD:
			break;
		case T_METER_CONTROL:
			//将消息直接推送到webclient中
			message -> decryptData();
			message -> setType(T_METER_CONTROL_ANS);
			WebClient::putSendMessage(message, true);
			break;
		case T_METER_VIP:
			processMeterVip(message);
			break;
		case T_ELEC_SAFETY_DATA:
			break;
		case T_TEMP_CONTROL:
			break;
		case T_ELEC_COMTROL:
			break;
		case T_DOOR_SHOW:
			break;
		case T_APP_UPDATE:
			break;
	}
	return ret;
}

/**
 * 获取楼栋编号
 */
std::string Client::getBuildingId() {
	return this -> buildingId;
}

/**
 * 获取网关编号
 */
std::string Client::getGatewayId() {
	return this -> gatewayId;
}

