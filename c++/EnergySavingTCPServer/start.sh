ulimit -c unlimited
nohup ./server 8000 > out.log 2>&1 &
