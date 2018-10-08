#MakeFile

linkFile = link/server.o link/Message.o link/AsioPlus.o link/Client.o link/WebClient.o link/DataProcesser.o link/DeviceManager.o link/tools.o

server: $(linkFile)
	g++ -o server $(linkFile) lib/libtinyxml2.a -l pthread -l boost_system -l boost_thread -l crypto
link/server.o: server.cpp Client.h AsioPlus.h Message.h tools.h WebClient.h DeviceManager.h DataProcesser.h PersistentSet.h
	g++ -std=c++11 -o link/server.o -g -c server.cpp
link/Client.o: Client.cpp Client.h AsioPlus.h Message.h tools.h WebClient.h DeviceManager.h DataProcesser.h PersistentSet.h
	g++ -std=c++11 -o link/Client.o -g -c Client.cpp
link/WebClient.o: WebClient.cpp AsioPlus.h Message.h tools.h DeviceManager.h DataProcesser.h PersistentSet.h
	g++ -std=c++11 -o link/WebClient.o -g -c WebClient.cpp
link/AsioPlus.o: AsioPlus.cpp AsioPlus.h Message.h tools.h
	g++ -std=c++11 -o link/AsioPlus.o -g -c AsioPlus.cpp
link/Message.o: Message.cpp Message.h tools.h
	g++ -std=c++11 -o link/Message.o -g -c Message.cpp
link/DataProcesser.o: DataProcesser.cpp DataProcesser.h DeviceManager.h PersistentSet.h tools.h
	g++ -std=c++11 -o link/DataProcesser.o -g -c DataProcesser.cpp
link/DeviceManager.o: DeviceManager.cpp DeviceManager.h tools.h
	g++ -std=c++11 -o link/DeviceManager.o -g -c DeviceManager.cpp
#link/PersistentSet.o: PersistentSet.cpp PersistentSet.h tools.h
#g++ -std=c++11 -o link/PersistentSet.o -c PersistentSet.cpp
link/tools.o: tools.cpp tools.h
	g++ -std=c++11 -o link/tools.o -g -c tools.cpp

.PHONY: clean

clean:
	rm link/*.o
