#~/bun/sh
NUM=`ps sx | grep ./server | wc -l`
if [ $NUM = "1" ];then
	cd /home/cisl/Documents/EnergySavingTCPServer
	./start.sh
fi
