import urllib.request
import pandas as pd
from datetime import datetime
from datetime import timedelta
from bs4 import BeautifulSoup
from apscheduler.schedulers.blocking import BlockingScheduler
endTime=10
def GetData(url): #url，num：城市编码
    html = urllib.request.urlopen(url).read()
    soup = BeautifulSoup(html,'html.parser',from_encoding='utf-8')
    res_data = soup.findAll('script')
    weather_list = soup.select('div[class="curve_livezs"]')
    weather_data = res_data[4]
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
    endTime=int(weather_list[0]['od21'])
    for item in weather_list:
        # od21小时，od22温度，od26降雨，od24风向，od25风力
        s = pd.Series([GetTime(item['od21'],index,endTime),item['od22'],item['od26'],item['od27'],GetWindDirection(item['od24']),item['od25']], index=['Time', 'Temperature','Rain','Humidity','WindDirection','WindPower'])
        df = df.append(s, ignore_index=True)
        index=index+1
    print(df)
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
def GetTime(time,index,endTime):
    finalTime=''
    if(index<24-endTime):
        finalTime = getDay(1) + ' ' + time + ':00:00'
    else:
        finalTime = getDay(0) + ' ' + time + ':00:00'
    print(finalTime)
    return finalTime

#获取时间
def getDay(delta):
    today=datetime.today()
    oneday=timedelta(days=delta)
    day=today-oneday
    return str(day)[:10]

def Save(df,fileName):
    with open(fileName, 'w+') as f:
        df.to_csv(f, header=True, index=False)

def LoadAndSave(df,fileName):
    old = pd.read_csv(fileName, header=0)

    old['Time'] = pd.to_datetime(old['Time'])
    with open(fileName, 'w') as f:
        df['Time']=pd.to_datetime(df['Time'])
        new=old.append(df)
        new = new.sort_values(by=['Time'])
        new=new.drop_duplicates(['Time'],keep='first')
        print(new)
        new.to_csv(f, header=True, index=False)
def YPWeather():
    print('The time is: %s' % datetime.now())
    fileName='data/YP/YP-Real-time weather.csv'
    df = GetData("http://www.weather.com.cn/weather1dn/101021700.shtml")
    LoadAndSave(df, fileName)
    print(df)
    print('The time is: %s' % datetime.now()+"杨浦数据获取完毕")
def YPWeather2():
    print('Tick! The time is: %s' % datetime.now())
    df = GetData("http://www.weather.com.cn/weather1dn/101021700.shtml")
    Save(df, 'data/YP/YP-'+getDay(1) + '.csv')
    print(df)
    print('The time is: %s' % datetime.now()+"杨浦数据获取完毕")
def PDWeather():
    print('The time is: %s' % datetime.now())
    fileName='data/PD/PD-Real-time weather.csv'
    df = GetData("http://www.weather.com.cn/weather1dn/101020600.shtml")
    LoadAndSave(df, fileName)
    print(df)
    print('The time is: %s' % datetime.now()+"浦东数据获取完毕")
def PDWeather2():
    print('Tick! The time is: %s' % datetime.now())
    df = GetData("http://www.weather.com.cn/weather1dn/101020600.shtml")
    Save(df, 'data/PD/PD-'+getDay(1) + '.csv')
    print(df)
    print('The time is: %s' % datetime.now()+"浦东数据获取完毕")
if __name__ == "__main__":
    #YPWeather()
    #YPWeather2()
    #PDWeather()
    #PDWeather2()
    scheduler = BlockingScheduler()
    scheduler.add_job(YPWeather,'cron',hour=8,minute=5)
    scheduler.add_job(YPWeather2,'cron',hour=1,minute=5)
    scheduler.add_job(YPWeather, 'interval', hours=3)
    
    scheduler.add_job(PDWeather,'cron',hour=8,minute=7)
    scheduler.add_job(PDWeather2,'cron',hour=1,minute=7)
    scheduler.add_job(PDWeather, 'interval', hours=3)
    try:
        print("Start program\n")
        scheduler.start()
    except (KeyboardInterrupt, SystemExit):
        scheduler.shutdown()

