#include "tools.h"

/**
  * 获取md5
  * 输入：需要加密的data
  * 输出：md5值
  */
string getMd5(string data) {
	unsigned char md[16];
	char buf[33] = {'\0'};
	char tmp[3] = {'\0'};
	MD5_CTX ctx;
	MD5_Init(&ctx);
	MD5_Update(&ctx, data.c_str(), data.size());
	MD5_Final(md, &ctx);
	for(int i=0; i<16; i++) {
		sprintf(tmp, "%02X", md[i]);
		strcat(buf, tmp);
	}
	string ret = buf;
	return ret;
}

/**
  * 获取指定长度的随机字符串（字节串）
  * 输入：获取长度
  * 输出：字节串
  */
string getRandomString(int len) {
	srand(time(NULL));
	string ret = "";
	for(int i=0; i<len; i++) 
		ret += 'a' + rand()%26;
	return ret;
}

/**
  * 获取给定的xml文档的根节点
  * 输入：xml文本串
  * 输出：xml根节点   ((((内存泄漏！)))
  */
XMLElement * getXMLRootElement(const char *data) {
	XMLDocument* doc = new XMLDocument();
	doc -> Parse(data);
	return doc -> FirstChildElement(_RT_);
}

/**
  * 将整形转换为字节流
  * 输入：整形
  * 输出：字节流
  */
shared_ptr<BYTE> IntToByte(int num) {
	shared_ptr<BYTE> ret(new BYTE[4]);
	for(int i=3; i>=0; i--, num>>=8) ret.get()[i] = char(num);
	return ret;
}

/**
  * 返回一条消息中的common元素
  * 输入：消息的文本串
  * 输出：pair<string, string>(buildingId, gatewayId)
  */
pss getCommonContent(const char *data) {
	XMLElement *root = getXMLRootElement(data);
	std::string buildingId = root -> FirstChildElement(_CM_) -> FirstChildElement(_BID_) -> GetText();
	std::string gatewayId = root -> FirstChildElement(_CM_) -> FirstChildElement(_GID_) -> GetText();
	return MP(buildingId, gatewayId);
}

/**
  * 返回一条消息中的common元素
  * 输出：消息的文本串
  * 输出：pair<string, string>(UploadDataCenterID, CreateTime)
  */
pss getCommonContentGH(const char *data) {
	XMLElement *root = getXMLRootElement(data);
	std::string UPCId = root -> FirstChildElement(_CM_) -> FirstChildElement(_UPCID_) -> GetText();
	std::string createTime = root -> FirstChildElement(_CM_) -> FirstChildElement(_CT_) -> GetText();
	return MP(UPCId, createTime);
}

/**
  * 返回一条消息中的versionId元素
  * 输入：消息的文本串
  * 输出：string versionId
  */
std::string getVersionIdFromXML(char *data) {
	XMLElement *versionId = getXMLRootElement(data) -> FirstChildElement("version_id");
	if(versionId == NULL) return "";
	std::string ret = versionId -> GetText();
	return ret;
}

/**
  *创建一个空的xml
  *输出:XMLDocument
  */
std::shared_ptr<XMLDocument> createBaseXML() {
	std::shared_ptr<XMLDocument> doc(new XMLDocument());
	XMLDeclaration * declaration = doc -> NewDeclaration((new string("xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\" ")) -> c_str());
	doc -> InsertFirstChild(declaration);
	doc -> SetBOM(true);
	doc -> InsertEndChild(doc -> NewElement(_RT_));
	return doc;
}


/**
  * 创建一个带有固定common元素的xml
  *	输入：buildingId, gatewayId, type
  * 输出：XMLDocument 元素
  */
std::shared_ptr<XMLDocument> createCommonXML(std::string buildingId, std::string gatewayId, std::string type) {
	std::shared_ptr<XMLDocument> doc = createBaseXML();
	XMLElement *p = doc -> FirstChildElement(_RT_);
	XMLElement *q = NULL;
	p -> InsertEndChild(doc -> NewElement(_CM_)); p = p -> FirstChildElement(_CM_);
	p -> InsertEndChild(doc -> NewElement(_BID_)); q = p -> FirstChildElement(_BID_);
	q -> InsertEndChild(doc -> NewText(buildingId.c_str()));
	p -> InsertEndChild(doc -> NewElement(_GID_)); q = p -> FirstChildElement(_GID_);
	q -> InsertEndChild(doc -> NewText(gatewayId.c_str()));
	p -> InsertEndChild(doc -> NewElement(_TP_)); q = p -> FirstChildElement(_TP_);
	q -> InsertEndChild(doc -> NewText(type.c_str()));
	return doc;
}

/**
  * 创建一个带有固定common元素的xml
  * 与上个函数不同的是会加入data元素
  * 输入：UploadDataCenterID, CreateTime
  * 输出：XMLDocument元素
  */
std::shared_ptr<XMLDocument> createCommonXML(std::string UPCId, std::string createTime) {
	std::shared_ptr<XMLDocument> doc = createBaseXML();
	XMLElement *p = doc -> FirstChildElement(_RT_);
	XMLElement *q = NULL;
	p -> InsertEndChild(doc -> NewElement(_CM_)); p -> InsertEndChild(doc -> NewElement("data"));
	p = p -> FirstChildElement(_CM_);
	p -> InsertEndChild(doc -> NewElement(_UPCID_)); q = p -> FirstChildElement(_UPCID_);
	q -> InsertEndChild(doc -> NewText(UPCId.c_str()));
	p -> InsertEndChild(doc -> NewElement(_CT_)); q = p -> FirstChildElement(_CT_);
	q -> InsertEndChild(doc -> NewText(createTime.c_str()));
	return doc;
	
}

/**
  * 获取当前时间格式：YYYYMMDDHHMMSS
  * 输出：当前时间
  */
std::string getTimeString() {
	time_t local = time(NULL);
	struct tm *ptm;
	ptm = gmtime(&local);
	int year = ptm -> tm_year + 1900;
	int month = ptm -> tm_mon + 1;
	int day = ptm -> tm_mday;
	int hour = ptm -> tm_hour + 8;
	int minute = ptm -> tm_min;
	int second = ptm -> tm_sec;
	char localTime[100];
	sprintf(localTime, "%d%02d%02d%02d%02d%02d", year, month, day, hour, minute, second);
	string ret(localTime);
	return ret;
}

/**
  * 获取XMLDocument的字符串指针
  * 输入: XMLDocument
  * 输出: const char* 指向xml字符流
  */
std::string xmlDocumentToString(std::shared_ptr<XMLDocument> xml) {
	XMLPrinter printer;
	xml -> Print(&printer);
	string ret = printer.CStr();
	return ret;
}

/**
  * 转化为全是小写的字符串
  * 输入：带大写字母
  * 输出：小写字母
  */
string toLowerCase(string str) {
	for(int i=0; i<str.size(); i++) {
		if(str[i] >= 'A' && str[i] <= 'Z') str[i] = str[i] - 'A' + 'a';
	}
	return str;
}

/**
  * aes解密函数
  * 输入：密文串 长度
  * 输出：明文
  */
char* aesDecrypt(const char *content, int &len) {
	AES_KEY aes;
	BYTE key[16];
	for(int i=0; i<16; i++) key[i] = PASSWORD[i];
	BYTE iv[16];
	for(int i=0; i<16; i++) iv[i] = PASSWORD[i];
	BYTE *ret = new BYTE[len];
	BYTE *input = new BYTE[len];
	for(int i=0; i<len; i++) input[i] = content[i];
	AES_set_decrypt_key(key, 128, &aes);
	AES_cbc_encrypt(input, ret, len, &aes, iv, AES_DECRYPT);
	ret[len] = 0;
	int fill = ret[len - 1];
	ret[len - fill] = 0;
	len -= fill;
	return (char*)ret;
}

/**
  * aes加密函数
  * 输入：明文串 长度
  * 输出：密文
  */
char* aesEncrypt(std::string content, int &len) {
	AES_KEY aes;
	int nBei = (len - 1) / 16 + 1;
	int nTotal = nBei * 16;
	BYTE key[16];
	for(int i=0; i<16; i++) key[i] = PASSWORD[i];
	BYTE iv[16];
	for(int i=0; i<16; i++) iv[i] = PASSWORD[i];
	BYTE *ret = new BYTE[nTotal + 1];
	BYTE *input = new BYTE[nTotal + 1];
	int nNumber = nTotal - len;
	if(len % 16 == 0) nNumber = 16;
	memset(input, (char)nNumber, (sizeof (char)) * nTotal);
	for(int i=0; i<len; i++) input[i] = content.c_str()[i];
	AES_set_encrypt_key(key, 128, &aes);
	AES_cbc_encrypt(input, ret, nTotal, &aes, iv, AES_ENCRYPT);
	len = nTotal;
	return (char*)ret;
}

//获取某一目录下的所有文件名
vector<std::string> getFiles(std::string cate_dir)  
{  
	vector<std::string> files; files.clear();
	DIR *dir;  
	struct dirent *ptr;  
	char base[1000];  
	if ((dir=opendir(cate_dir.c_str())) == NULL) return files;
	while ((ptr=readdir(dir)) != NULL) {  
		if(strcmp(ptr->d_name,".")==0 || strcmp(ptr->d_name,"..")==0) 
			continue;  
		else if(ptr->d_type == 8)  files.push_back(ptr->d_name);  
		else if(ptr->d_type == 10) continue;  
		else if(ptr->d_type == 4)  files.push_back(ptr->d_name);  
	}  
	closedir(dir);  
	sort(files.begin(), files.end());
	return files;  
}

//获取光华楼数据文件
vector<std::string> getDataFiles() {
	DIR *dir;
	char basePath[100] = {0};
	getcwd(basePath, 999);
	string dataPath = basePath;
	dataPath += "/data";
	vector<std::string> files = getFiles(dataPath);
	return files;
}

//获取光华楼设备文件
vector<std::string> getDeviceFiles() {
	DIR *dir;
	char basePath[100] = {0};
	getcwd(basePath, 999);
	std::string dataPath = basePath;
	dataPath += "/deviceData";
	vector<std::string> files = getFiles(dataPath);
	return files;
}

//将光华楼的时间戳转换为正常时间戳
std::string TimeTrans(string time) {
	string ret = "";
	for(int i=0; i<time.size(); i++)
	   if(time[i] >= '0' && time[i] <= '9') {
		   ret += time[i];
	   }	   
	return ret;
}

//测试函数
void out(BYTE* text, int length) {
	for(int i=0; i<length; i++) 
		printf("%02hhx ", text[i]);
	puts("");
}

