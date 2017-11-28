objects = DataProcess.o main.o

Gesture: DataProcess.o WifiHelper.o main.o
	g++ -std=c++11 -pthread -g DataProcess.o main.o WifiHelper.o -o Gesture

DataProcess.o: DataProcess.cpp DataProcess.h stdfx.h
	g++ -std=c++11 -pthread -O3 -g -c DataProcess.cpp

WifiHelper.o: WifiHelper.cpp WifiHelper.h stdfx.h
	g++ -std=c++11 -pthread -g -c WifiHelper.cpp

main.o: main.cpp stdfx.h
	g++ -std=c++11 -pthread -g -c main.cpp

clean:
	rm -f *.o Gesture core
