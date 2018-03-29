# encoding=utf-8
import requests
import json
import time
import pandas as pd
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
    df=pd.DataFrame(columns=['Date','Hmax','Hmin','Humidity'])
    #subkey = {'date': 'Date', 'hmax': 'Hmax', 'hmin': 'Hmin', 'hgl': 'Humidity'}

    subkey = ['date', 'hmax','hmin','hgl']

    for dict in list:
        #subdict = {key: dict[key] for key in subkey}
        if dict['date'][4:6]!=month:
            continue
        sublist=[dict[key] for key in subkey]
        sublist[0]=sublist[0][0:4]+'-'+sublist[0][4:6]+'-'+sublist[0][6:]+' 00:00:00'
        sublist[3]=sublist[3][:-1]
        s = pd.Series(sublist, index=['Date','Hmax','Hmin','Humidity'])
        print(s)
        df=df.append(s,ignore_index=True)
    return df

def Save(df,fileName):
    with open(fileName, 'w+') as f:
        df.to_csv(f, header=True, index=False)

#return newList
if __name__ == '__main__':
    year = "2017"
    month = 1

    df=pd.DataFrame(columns=['Date','Hmax','Hmin','Humidity'])
    for i in range(month, 13):
        month = str(i) if i > 9 else "0" + str(i)  # 小于10的月份要补0
        df=df.append(ExtractValue(year,month))
    print(df)
    Save(df,'data\WeatherData.csv')


