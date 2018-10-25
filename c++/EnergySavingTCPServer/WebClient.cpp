#include "WebClient.h"

std::map<string, queue<Message> > WebClient::receiveMessage;
std::mutex WebClient::receiveMutex;
std::queue<Message> WebClient::sendMessage;
std::mutex WebClient::sendMutex;
/**
 * 工作函数
 * 功能：(1) 启动客户端并和服务器建立链接
 *       (2) 负责接受和发送信息
 */
void WebClient::clientWork() {
//	ip::tcp::endpoint ep(ip::address::from_string("218.193.132.86"), 8001);
        ip::tcp::endpoint ep(ip::address::from_string("218.193.132.86"),23);
//	ip::tcp::endpoint ep(ip::address::from_string("218.193.130.45"), 8001);
//	ip::tcp::endpoint ep(ip::address::from_string("218.193.130.43"), 13);
	//ip::tcp::endpoint ep(ip::address::from_string("10.131.241.172"), 13);
	//ip::tcp::endpoint ep(ip::address::from_string("10.131.4.15"), 60000);
	//ip::tcp::endpoint ep(ip::address::from_string("10.131.200.70"), 13);
	//ip::tcp::endpoint ep(ip::address::from_string("10.131.241.206"), 13);
	
	boost::system::error_code ec;
	bool isOpen = false;
	while(true) {
		if(this -> mySocket -> is_open()) {
			if(isOpen == false) {
				isOpen = true;
				this -> sendClientId();
			}
			if(this -> canRead()) {
				shared_ptr<Message> msg(this -> readAMessage(ec));
				if(ec) {this -> mySocket -> close() ;continue;}
				this -> processMessage(msg);
			}
			else if(!WebClient::isSendEmtpy()) {
				shared_ptr<Message> msg = getSendMessage();
				cout << endl << getTime()+ "：发送服务器数据：" << endl;
				cout << "Length: " << msg -> getLength() << endl;
                cout << "Type: " << msg -> getType() << endl;
				cout << "versionId: " << getVersionIdFromXML(msg -> getData()) << endl;
				
               // cout << "Data: " <<msg -> getData() << endl;
                
				hasSend[0][getVersionIdFromXML(msg -> getData())] = msg;
				this -> sendXML(msg -> getData(), msg -> getLength(), msg -> getType(), ec);
				
				cout <<"Remain sendMessage:"<<WebClient::sendMessage.size()<< endl<< endl;
				if(ec) 
					continue;
			}
			if(this -> sendHeartBeat(ec)) 
				continue;
			if(this -> reSend()) 
				continue;
			continue;
		} else {
			isOpen = false;
			sleep(1);
			cout << "连接断开，正在建立连接，请稍等..." << endl;
			if(this -> connectServer(ep)) {
				cout  << getTime()+ "连接建立成功！" << endl;
			}else {
				cout  << getTime()+ "重连失败，请检查服务器！" << endl;
			}
		}
	}
		sleep(1);
}

string getTime()
{
    time_t timep;
    time (&timep);
    char tmp[64];
    strftime(tmp, sizeof(tmp), "%Y-%m-%d %H:%M:%S",localtime(&timep) );
    return tmp;
}

/**
 * 构造函数, 在第一次实例化的时候使用
 * 功能：(1) 变量初始化
 *       (2) 启动工作线程
 */
WebClient::WebClient(io_service &io_service):AsioPlus(io_service) {
		hasSend[0].clear();
		hasSend[1].clear();
	}

/**
  *发送客户端标识符
  *
  */
void WebClient::sendClientId() {
	boost::system::error_code ec;
	shared_ptr<XMLDocument> doc = createBaseXML();
	XMLElement *root = doc -> FirstChildElement("root");
	root -> InsertFirstChild(doc -> NewElement("client_id"));
   	root = root -> FirstChildElement("client_id");
	root -> InsertFirstChild(doc -> NewText(CLIENTID));
	string xmlStr(xmlDocumentToString(doc));
	cout << xmlStr << endl;
	sendXML(xmlStr.c_str(), xmlStr.size(), T_ID_VALIDATE, ec);
	if(ec) this -> mySocket -> close();
}

/**
 * 连接服务器，失败的话睡眠1秒
 * 返回：成功与否
 */
bool WebClient::connectServer(ip::tcp::endpoint &ep) {
	try{
		boost::system::error_code ec;
		this -> mySocket -> connect(ep,ec);
		std::cout << getTime()+":WebClient::connect to Server" << std::endl;
		if(ec)
		{
			std::cout << boost::system::system_error(ec).what() << std::endl;
			return false;
		}
	} catch (exception &e) {
		cout << "not open" << endl;
		this -> mySocket -> close();
		cout << e.what() << endl;  
		return false;
	}
	this -> lastConnectTime = clock();
	return true;
}

/**
 * 发送心跳包
 * 输入：ec 错误码
 * 
 */
bool WebClient::sendHeartBeat(boost::system::error_code &ec) {
	clock_t localtime = clock();
	if(localtime - this -> lastConnectTime > 15000000) {
		this -> lastConnectTime = localtime;
		string content = getTimeString();
        cout  << "发送服务器心跳"<<endl;
		cout  << getTime()<<endl;;
		this -> sendXML(content.c_str(), content.size(), T_HEART_BEAT, ec);
		if(ec) {
            this -> mySocket -> close(); 
            return true;
        }
		return true;
	}
	return false;
}

/**
 * 放入收到消息队列
 * 输入：网关id 消息对象
 */
void WebClient::putReceiveMessage(std::string gatewayId, shared_ptr<Message> msg) {
	std::lock_guard<std::mutex> lck(receiveMutex);
	receiveMessage[gatewayId].push(*msg);
}

/**
 * 测试收到消息队列是否为空
 * 输入：网关id
 */
bool WebClient::isReceiveEmpty(std::string gatewayId) {
	std::lock_guard<std::mutex> lck(receiveMutex);
	return receiveMessage[gatewayId].empty();
}

/**
 * 取出收到消息队列 (先测试队列不为空！）
 * 输入：网关id
 * 输出：队头消息
 */
shared_ptr<Message> WebClient::getReceiveMessage(std::string gatewayId) {
	std::lock_guard<std::mutex> lck(receiveMutex);
	cout << "gatewayId: " << gatewayId << endl;
	shared_ptr<Message> msg = make_shared<Message>(WebClient::receiveMessage[gatewayId].front());
	cout << msg -> getData() << endl;
	WebClient::receiveMessage[gatewayId].pop();
	return msg;
}

/**
 * 放入发送消息队列
 * 输入：消息对象指针
 */
void WebClient::putSendMessage(shared_ptr<Message> msg, bool isCheck) {
	if(isCheck && msg -> getType() != 85 && msg -> getType() != 5) {
        int count=0;
		while(WebClient::sendMessage.size() > 3) {
            count++;
			cout << "消息队列已满,请稍后 " << count<<endl;
			sleep(5);
		}
	}
	std::lock_guard<std::mutex> lck(sendMutex);
	WebClient::sendMessage.push(*msg);
}

/**
 * 测试发送消息队列是够为空
 * 输出：是否为空
 */
bool WebClient::isSendEmtpy() {
	std::lock_guard<std::mutex> lck(sendMutex);
	return WebClient::sendMessage.empty();
}

/**
 * 取出发送消息队列（先测试队列不为空）
 * 输出：队头消息
 */
shared_ptr<Message> WebClient::getSendMessage() {
	std::lock_guard<std::mutex> lck(sendMutex);
	shared_ptr<Message> msg = make_shared<Message>(WebClient::sendMessage.front());
	WebClient::sendMessage.pop();
	return msg;
}

/**
 * 处理电表控制信息
 * 功能：将收到的消息放入对应网关的队列
 * 输入：电表控制消息指针
 */
void WebClient::receiveMeterControl(shared_ptr<Message> msg) {
	boost::system::error_code ec;
	cout  << getTime()+ "收到控制信息" << endl;
	pss ret = getCommonContent(msg -> getData());
	putReceiveMessage(ret.second, msg);
	shared_ptr<XMLDocument> doc = createCommonXML(ret.first, ret.second, "meter_control");
	XMLElement* root = doc -> FirstChildElement("root");
	root -> InsertEndChild(doc -> NewElement("data"));
	root = root -> FirstChildElement("data");
	root -> SetAttribute("operation", "request");
	root -> InsertEndChild(doc -> NewElement("result"));
	root -> FirstChildElement("result") -> InsertEndChild(doc -> NewText("ok"));
	root -> InsertEndChild(doc -> NewElement("time"));
	root -> FirstChildElement("time") -> InsertEndChild(doc -> NewText(getTimeString().c_str()));
	string content = xmlDocumentToString(doc);
	sendXML(content.c_str(), content.size(), T_METER_CONTROL, ec);
	if(ec) {this -> mySocket -> close(); return; }
}

/**
 * 缓存设备表信息
 * 将设备表转换成xml并缓存
 */
void WebClient::receiveDevice(shared_ptr<Message> msg) {
	shared_ptr<XMLDocument> doc(new XMLDocument());
	doc -> Parse(msg -> getData());
	DeviceManager::getInstance() -> XMLtoMenMeterDevice(doc);
	shared_ptr<XMLDocument> ansDoc = createBaseXML();
	XMLElement *root = ansDoc -> FirstChildElement("root");
	root -> InsertFirstChild(ansDoc -> NewElement("version_id"));
	root -> InsertFirstChild(ansDoc -> NewElement("result"));
	root -> FirstChildElement("result") -> InsertFirstChild(ansDoc -> NewText("success"));
	root -> FirstChildElement("version_id") -> InsertFirstChild(ansDoc -> NewText(doc -> FirstChildElement("root") -> FirstChildElement("version_id") -> GetText()));
	string answer = xmlDocumentToString(ansDoc);
	boost::system::error_code ec;
	sendXML(answer.c_str(), answer.size(), T_DEV, ec);
	if(ec) {this -> mySocket -> close();}
}

/**
 * 定时检查是够有重传消息
 * 
 */
bool WebClient::reSend() {
	clock_t localtime = clock();
	if(localtime-this -> lastCheckTime > 100000000) {
        cout<<"Time difference :"<<localtime-this -> lastCheckTime<<endl;
        
		this -> lastCheckTime = localtime;
		cout<<"检查重传"<< " hasSend[0]:"<<hasSend[0].size() << " hasSend[1]:"<<hasSend[1].size() << endl;
		for(auto it : hasSend[1]) {
			putSendMessage(it.second, false);
		}
	
		hasSend[1].clear();
		
		for(auto it : hasSend[0]) {
			hasSend[1][it.first] = it.second;
		}
		hasSend[0].clear();
		return true;
	}
	return false;
}

/**
 * 处理收到的信息
 * 输入：message指针
 
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
 */

void WebClient::processMessage(shared_ptr<Message> msg) {
	std::string versionId;
    cout<<"处理收到的信息 "<<msg -> getType()<<endl;
	switch(msg -> getType()) {
		case T_HEART_BEAT:
            //T_HEART_BEAT 2
            cout<<"T_HEART_BEAT"<<endl;
			break;
		case T_METER_CONTROL:
            //T_METER_CONTROL 5
            cout<<"T_METER_CONTROL"<<endl;
			receiveMeterControl(msg);
			break;
		case T_METER_CONTROL_ANS:
            //T_METER_CONTROL_ANS 85
            cout<<"T_METER_CONTROL_ANS"<<endl;
			cout << msg -> getData() << endl;
			break;
		case T_DEV:
            //T_DEV 100
            cout<<"T_DEV"<<endl;
			cout << msg -> getData() << endl;
			receiveDevice(msg);
			break;
		//T_METER_VIP_UPLOAD 86
        case T_METER_VIP_UPLOAD:
        //T_UP_AIR 110
		case T_UP_AIR:
        // T_UP_LIGHT 111
		case T_UP_LIGHT:
        //T_UP_SOCKET 112
		case T_UP_SOCKET:
        //T_UP_ELE 113
		case T_UP_ELE:
        //T_UP_OTHER 114
		case T_UP_OTHER:
        //T_GH_DEVICE 115
        case T_GH_DEVICE:
			versionId = getVersionIdFromXML(msg -> getData());
            cout<<"处理数据回馈 VersionId:"<<versionId<<endl;
			if(hasSend[0].count(versionId)){
                hasSend[0].erase(versionId);
                cout<<"hasSend[0] clean:"<<versionId<<endl;
            }
			else if(hasSend[1].count(versionId)){
                hasSend[1].erase(versionId);
                cout<<"hasSend[1] clean:"<<versionId<<endl;
            }
			break;
			//缓存部门，区域相关表功能不实现
		case T_AREA_DEV:
            cout<<"T_AREA_DEV"<<endl;
			break;
		case T_DEP_DEV:
            cout<<"T_DEP_DEV"<<endl;
			break;
		case T_AREA:
            cout<<"T_AREA"<<endl;
			break;
		case T_DEP:
            cout<<"T_DEP"<<endl;
			break;
	}
    cout<<endl;
}


