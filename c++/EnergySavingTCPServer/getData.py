from ftplib import FTP
import time
import os

d = {}

def work(path, ftp, num):
	ftp.cwd(path);
	str = ftp.nlst()
	print str
	for file in str:
		if not d.has_key(file):
			filename = 'RETR ' + file
			if len(num) == 1 :
				local = "data/"+num+"_"+file
			else :
				local = "deviceData/"+num+"_"+file
			try:
				f = open(local, 'wb')
				ftp.retrbinary(filename, f.write)
				f.close()
				d[file + num] = 1
				f = open(local, 'rb+')
				lines = f.readlines()
				f.close()
				if len(lines) == 0:
					os.remove(local)
			except :
				continue;


for i in range(0, 5):
	ftp = FTP()
	ftp.connect("10.141.208.135", 21)
	ftp.login("Administrator", "Cis-51355517")
	str = []
	work("/dcs/East-1/ems_ConsumptionData", ftp, '0')
	work("/dcs/East-2/ems_ConsumptionData", ftp, '1')
	work("/dcs/East-3/ems_ConsumptionData", ftp, '2')
	work("/dcs/East-4/ems_ConsumptionData", ftp, '3')
	work("/dcs/East-5/ems_ConsumptionData", ftp, '4')
	work("/dcs/West-1/ems_ConsumptionData", ftp, '5')
	work("/dcs/West-2/ems_ConsumptionData", ftp, '6')
	work("/dcs/West-3/ems_ConsumptionData", ftp, '7')
	work("/dcs/West-4/ems_ConsumptionData", ftp, '8')
	work("/dcs/floor-209-1/ems_ConsumptionData", ftp, '9')
	
	work("/dcs/East-1/ems_DeviceData", ftp, '10')
	work("/dcs/East-2/ems_DeviceData", ftp, '11')
	work("/dcs/East-3/ems_DeviceData", ftp, '12')
	work("/dcs/East-4/ems_DeviceData", ftp, '13')
	work("/dcs/East-5/ems_DeviceData", ftp, '14')
	work("/dcs/West-1/ems_DeviceData", ftp, '15')
	work("/dcs/West-2/ems_DeviceData", ftp, '16')
	work("/dcs/West-3/ems_DeviceData", ftp, '17')
	work("/dcs/West-4/ems_DeviceData", ftp, '18')
	work("/dcs/floor-209-1/ems_DeviceData", ftp, '19')
	ftp.quit()
	time.sleep(10)


