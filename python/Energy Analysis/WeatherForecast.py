import urllib.request
import pandas as pd
from datetime import datetime
from datetime import timedelta
from bs4 import BeautifulSoup
from apscheduler.schedulers.blocking import BlockingScheduler
endTime=10
def GetData(url,num,endTime): #url，num：城市编码
    html = urllib.request.urlopen(url).read()
    soup = BeautifulSoup(html,'html.parser',from_encoding='utf-8')
    res_data = soup.findAll('script')
    weather_list = soup.select('div[class="curve_livezs"]')
    weather_data = res_data[3]
    for x in weather_data:
        weather1 = x
    index_start = weather1.find("{")  # 目前weather1还是一个字符串，需要将里面的json截取出来
    index_end = weather1.find(";")
    weather_str = weather1[index_start:index_end]
    weather = eval(weather_str)  # 将字符串转换成json
    weather_dict = weather["od"]
    weather_date = weather_dict["od0"]  # 时间
    weather_position_name = weather_dict["od1"]  # 地点
    weather_list = list(reversed(weather["od"]["od2"]))
    s = pd.Series([weather["od"]], index=['Time', 'Value'])
    insert_list = []  # 存放每小时的数据的list，用于之后插入数据库
    df=pd.DataFrame(columns=['Time', 'Temperature','Rain','Humidity','WindDirection','WindPower'])
    index=0
    for item in weather_list:
        # od21小时，od22温度，od26降雨，od24风向，od25风力
        '''
        weather_item = {}
        weather_item['time'] = item['od21']
        weather_item['temperature'] = item['od22']
        weather_item['rain'] = item['od26']
        weather_item['humidity'] = item['od27']
        weather_item['windDirection'] = item['od24']
        weather_item['windPower'] = item['od25']
        '''
        s = pd.Series([GetTime(item['od21'],endTime,index),item['od22'],item['od26'],item['od27'],GetWindDirection(item['od24']),item['od25']], index=['Time', 'Temperature','Rain','Humidity','WindDirection','WindPower'])
        df = df.append(s, ignore_index=True)
        index=index+1
        #insert_list.append(weather_item)
    return df
def GetWindDirection(windDirection):
    dict={}
    dict['东风']=0
    dict['东南风']=1
    dict['南风'] = 2
    dict['西南风'] = 3
    dict['西风'] = 4
    dict['西北风'] = 5
    dict['北风'] = 6
    dict['东北风'] = 7
    dict['暂无风向']= 8
    return dict[windDirection]
def GetTime(time,endTime,index):
    if(index<24-endTime):
        return getDay(1) + ' ' + time + ':00:00'
    else:
        return getDay(0) + ' ' + time + ':00:00'

#获取时间
def getDay(delta):
    today=datetime.today()
    oneday=timedelta(days=delta)
    day=today-oneday
    return str(day)[:10]

def Save(df,fileName):
    with open(fileName, 'a') as f:
        df.to_csv(f, header=True, index=False)

def loadDataSet(fileName):
    #读取数据
    table = pd.read_table(fileName,header = 0,sep=",")
    return table

def LoadAndSave(df,fileName):
    olddf = loadDataSet(fileName)
    with open(fileName, 'w+') as f:
        df=olddf.append(df)
        print(df)
        df=df.drop_duplicates(['Time'],keep='first')
        print(df)
        df.to_csv(f, header=True, index=False,sep=",")
def tick():
    print('Tick! The time is: %s' % datetime.now())
if __name__ == "__main__":

    fileName='data\Real-time weather.txt'
    df=GetData("http://www.weather.com.cn/weather1d/101200101.shtml",101200101,endTime)
    LoadAndSave(df,fileName)


    scheduler = BlockingScheduler()
    scheduler.add_job(tick,'cron', second='*/3', hour=endTime)
    try:
        scheduler.start()
    except (KeyboardInterrupt, SystemExit):
        scheduler.shutdown()
