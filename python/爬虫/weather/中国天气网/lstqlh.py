# -*- coding: utf-8 -*-
"""
Created on Wed Jan 11 19:01:10 2017

@author: CompliceU
"""
import requests
#from urllib.request import urlopen
import psycopg2
from bs4 import BeautifulSoup
from zhongguotianqiwangcity import zhongguotianqiwangcity
import time


insertSql = "insert into weather_zgtqwhis(location,ymd,bWendu,yWendu,tianqi,fengxiang,fengli) values(%s,%s,%s,%s,%s,%s,%s)"
def get_data(location,date,location1):
    #final=[]
    print(date)
    url="http://lishi.tianqi.com/{}/{}.html".format(location,date,location1)
    url1="http://lishi.tianqi.com/{}/index.html".format(location)
    print(url)
    html=requests.get(url)
    if html.url!=url1:
        #print(html.text)
        bs0bj=BeautifulSoup(html.text,"html.parser")
        #print(bs0bj)
        soup=bs0bj.find("div",{"class":"tqtongji2"})
        ul=soup.findAll("ul")
        for i in range(1,len(ul)):
            temp=[]
            '''date=ul[i].find('a')
            temp.append(date,)'''
            li=ul[i].findAll('li')
            temp.append(li[0].string)
            temp.append(li[1].string,)
            temp.append(li[2].string,)
            temp.append(li[3].string,)
            temp.append(li[4].string,)
            temp.append(li[5].string)
            
            #final.append(temp)
            temp.insert(0,location1)
            print(temp)
            insert(insertSql, temp)
    
        
    
        

def creatTable():
    conn = psycopg2.connect(database="postgres", user="postgres", password="njau", host="192.168.116.59")
    print ("Opened database successfully")
    cur = conn.cursor()
    cur.execute('''CREATE TABLE weather_zgtqwhis
           (location char(100) NOT NULL,
           ymd              CHAR(50)       NOT NULL,
           bWendu           CHAR(50)    NOT NULL,
           yWendu           CHAR(50)     NOT NULL,
           tianqi          CHAR(150),
           fengxiang         char(150),
           fengli            char(150),
           PRIMARY KEY(location,ymd));''')
    print ("Table created successfully")
    conn.commit()
    conn.close()
    return True

def insert(insertSql, ls):
    try:
        conn = psycopg2.connect(database="postgres", user="postgres", password="njau", host="192.168.116.59")
        cur = conn.cursor()
    except Exception as e:
        print ("connnect error", str(e))
        return False
    try:
        # for arr in ls:
            #             print insertSql,arr
        cur.execute(insertSql, ls)

        conn.commit()
        succ = "insert into " + str(len(ls)) + " rows"
        print (succ)
    except Exception as e:
        conn.rollback()
        print ("error", e)
        return False
    finally:
        cur.close()
        conn.close()
    return True

'''   
def write_data(data, name):
    file_name = name
    with open(file_name, 'a', errors='ignore', newline='') as f:
            f_csv = csv.writer(f)
            f_csv.writerows(data)
'''

def transf(i):
	list1=[i]
	if (len(str(i))<2):
		list1[0]=0
		list1.append(i)
		return (str(list1[0])+str(list1[1])) 
	else:
		return str(i)
        
   
if __name__ == '__main__':
    creatTable()
    conn = psycopg2.connect(database="postgres", user="postgres", password="njau", host="192.168.116.59")
    cur = conn.cursor()
    year1=time.strftime('%Y',time.localtime(time.time()))
    month1=int(time.strftime('%m',time.localtime(time.time())))#获取当前年月
    for key in zhongguotianqiwangcity:
        location=zhongguotianqiwangcity[key]
        location1=key
        for year in range(2011,int(year1)):
            for month in range(1,13):
                #url="http://lishi.tianqi.com/luhe1/{}.html".format(date)
                date=str(year)+transf(month)
                print (date)
                get_data(location,date,location1)
        if month1==1:
            date=str(year1)+'01'
            print (date)
            get_data(location,date,location1)
        for month in range(1,month1):
            #url="http://lishi.tianqi.com/luhe1/{}.html".format(date)
            date=str(year1)+transf(month1-1)
            print (date)
            get_data(location,date,location1)
           
    
