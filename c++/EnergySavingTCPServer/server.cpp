#include "Client.h"

void session(Client *client)
{
	/**
	 * 错误写法：会造成二次析构
	 * socket_ptr sock(client -> getMySocket());
	*/
	socket_ptr sock = client -> getMySocket();
	boost::system::error_code ec;
	try
	{
        if(!client -> verification()){
            return;
        }
		
		for (;;) {
			if(client -> canRead()) {
				puts("client get listen!");
                cout<<"client get listen!"<<endl;
				//接受一个消息
				std::shared_ptr<Message> msg(client -> readAMessage(ec));
				if(ec) {
                     cout<<"session read err!"<<endl;
                    break;
                }
				//输出消息信息
				//puts(msg -> getData());
				//处理消息
				client -> processMessage(msg);
			}
			else if(!WebClient::isReceiveEmpty(client -> getGatewayId())) {
				shared_ptr<Message> msg = WebClient::getReceiveMessage(client -> getGatewayId());
				client -> sendXML(msg -> getData(), msg -> getLength(), msg -> getType(), ec);
				if(ec) {
                    cout<<"session err!"<<endl;
                    break;
                }
			}
			sleep(1);
		}
	} catch (std::exception& e) {
		std::cerr << "Exception in TCPServer thread:" << e.what() << "\n";
	}
}


void server(boost::asio::io_service& io_service, unsigned short port)
{
	try
	{
		ip::tcp::acceptor a(io_service, ip::tcp::endpoint(ip::tcp::v4(), port));
		for (;;)
		{
			Client *client = new Client(io_service);
            cout<< "Tcp start listening" <<endl;
			a.accept(*(client -> getMySocket()));
            cout<< "Tcp accept connect" <<endl;
			boost::thread t(boost::bind(session, client));
		}
	} catch (std::exception& e) {
		std::cerr << "Exception in main thread: " << e.what() << "\n";
	}
}

void startWebClient() {
	try
	{
		boost::asio::io_service io_service;
		WebClient test(io_service);
		test.clientWork();
	} catch (std::exception& e) {
		std::cerr << "Exception in main WebClient thread: " << e.what() << "\n";
	}
}

void startDataCheck() {
	DataProcesser *processer = DataProcesser::getInstance();
	try
	{
		while(true) {
			
			cout <<endl<< "---------------------start-------------------" << endl;
            cout << getTime()+":server::startDataCheck"<<endl;
			processer -> checkIsReadyEnergy();
			
			
			vector<shared_ptr<Message> > messages = processer -> checkIsReady();
			for(auto msg : messages) {
				WebClient::putSendMessage(msg, true);
			}
			cout << "ReadyData:" <<messages.size()<< endl;
			
			vector<shared_ptr<Message> > meterVip = processer -> checkIsReadyVip();
			for(auto msg : meterVip) {
				WebClient::putSendMessage(msg, true);
			}
			cout << "ReadyVip:"<<meterVip.size() << endl;
			 
			vector<shared_ptr<Message> > deviceGH = processer -> checkIsReadyDevice();
			
			for(auto msg : deviceGH) {
				WebClient::putSendMessage(msg, true);
			}
			cout << "ReadyDevice:" << deviceGH.size() << endl;
			
			cout << getTime()+":server::endDataCheck"<<endl;
			cout << "--------------------- end -------------------" << endl<< endl;
			sleep(10);
		}
	} catch (std::exception& e) {
		std::cerr << "Exception in data check thread: " << e.what() << "\n";
	}
}

int main(int argc, char* argv[])
{
	try
	{
		if (argc != 2) {
			std::cerr << "Usage: blocking_tcp_echo_server <port>\n";
			return 1;
		}
		boost::asio::io_service io_service;
		//强制初始化
		cout << getTime()+ "系统初始化..." << endl;
		DataProcesser::getInstance();
		DeviceManager::getInstance();
		cout << getTime()+ "初始化完毕!" << endl;

		boost::thread t(startWebClient);
		boost::thread t2(startDataCheck);

		server(io_service, std::atoi(argv[1]));
	} catch (std::exception& e) {
		std::cerr << getTime()+ "Exception: " << e.what() << "\n";
	}

	return 0;
}
