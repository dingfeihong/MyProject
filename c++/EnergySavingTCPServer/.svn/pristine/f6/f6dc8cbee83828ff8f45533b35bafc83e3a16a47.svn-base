#ifndef __PERSISTENTSET__
#define __PERSISTENTSET__

#include "tools.h"

/**
  * 持久化集合
  */
template<class T>
class PersistentSet {
	private:
		//存放和内存同步的Set
		std::set<T> itemSet;
		std::queue<T> itemBuff;
		std::string filename;

		//持久化函数
		void store();

	public:
		//构造函数
		PersistentSet(const string _filename);
		//是否存在元素
		bool isExist(T item);
		//插入一条元素
		bool insertItem(T item);
		//删除一条元素
		bool eraseItem(T item);
		//析构函数
		~PersistentSet();
};

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


#endif

