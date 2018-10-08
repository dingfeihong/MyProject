#include "PersistentSet.h"

/**
  * 构造函数
  * 从文件中读出数据
  * 速度较慢注意控制构造的次数
  */
template<class T>
PersistentSet<T>::PersistentSet(string _filename) {
	itemSet.clear();
	filename = _filename;
	ifstream in(filename);
	std::string item;
	while(in >> item) itemSet.insert(item);
	in.close();
	while(!itemBuff.empty()) itemBuff.pop();
}

/**
  * 持久化函数
  * 将新写入的数据存入文件
  */
template<class T>
void PersistentSet<T>::store() {
	cout << "store()" << endl;
	ofstream out(filename, ios::app);
	while(!itemBuff.empty()) {
		out << itemBuff.front() << endl;
		itemBuff.pop();
	}
	out.close();
}

/**
  * 是否存在元素 
  * 存在返回true 不存在返回true
  */
template<class T>
bool PersistentSet<T>::isExist(T item) {
	if(itemSet.count(item)) return true;
	return false;
}

/**
  * 插入一个元素
  * 当缓冲区中数量达到一定值时存入文件
  * 存在返回false 不存在返回true
  */
template<class T>
bool PersistentSet<T>::insertItem(T item) {
	if(itemSet.count(item)) return false;
	itemSet.insert(item);
	itemBuff.push(item);
	if(itemBuff.size() == 10) store();
	return true;
}

/**
  * 删除一个元素
  * 存在删除返回true 不存在返回false
  */
template<class T>
bool PersistentSet<T>::eraseItem(T item) {
	if(!itemSet.count(item))  return false;
	itemSet.erase(item);
	return true;
}

/**
  * 析构函数
  */
template<class T>
PersistentSet<T>::~PersistentSet() {
	store();
}

