# encoding=utf-8
import requests
import json
import time
def Request(year, month):
    url = "http://d1.weather.com.cn/calendar_new/" + year + "/101020100_" + year + month + ".html"
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36",
        "Referer": "http://www.weather.com.cn/weather40d/101020100.shtml",
    }
    return requests.get(url, headers=headers)

def Parse(res):
    json_str = res.content.decode(encoding='utf-8')[11:]
    a=json.loads(json_str)
    return json.loads(json_str)

def ExtractValue(year,month):
    list=Parse(Request(year, month))
    newList=[]
    #subkey = {'date': 'Date', 'hmax': 'Hmax', 'hmin': 'Hmin', 'hgl': 'Humidity'}

    subkey = ['date', 'hmax','hmin','hgl']

    for dict in list:
        #subdict = {key: dict[key] for key in subkey}
        if dict['date'][4:6]!=month:
         continue
        sublist = [dict[key] for key in subkey] #提取原字典中部分键值对，并替换key为中文
        print(sublist)
        newList.append(sublist)
    return newList

def Save(list):
    file = open('data\WeatherData.txt', 'w')
    file.write('Date,Hmax,Hmin,Humidity\n')
    for sublist in list:

        #日期处理
        tmpDate=sublist[0]
        tmpDate=tmpDate[0:4]+'-'+tmpDate[4:6]+'-'+tmpDate[6:]+' 00:00:00'
        file.write(tmpDate)
        #去掉湿度的%
        for item in sublist[1:]:
            if item[-1]=="%":
                item=item[:-1]
            file.write(','+item)
        file.write("\t\n")
    file.close()

#return newList
if __name__ == '__main__':
    year = "2017"
    month = 1
    list=[]
    for i in range(month, 13):
        month = str(i) if i > 9 else "0" + str(i)  # 小于10的月份要补0
        list+=ExtractValue(year,month)
    Save(list)
    time.sleep(1)


