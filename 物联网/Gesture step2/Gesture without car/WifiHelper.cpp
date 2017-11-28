#include "stdfx.h"
#include "WifiHelper.h"


WifiHelper::WifiHelper()
{
	comm.head = 0xFF;
	comm.type = 0x00;
	comm.comm = 0x00;
	comm.data = 0x00;
	comm.tail = 0xFF;
        lastStatus = STATUS_STATIC;
}


WifiHelper::~WifiHelper()
{
	Close();
}

bool WifiHelper::InitWIFISocket() {
	if((m_sock=socket(AF_INET,SOCK_STREAM,0))==-1){
        		printf("socket() 失败\n");
		exit(1);
    	}

	int err;
	struct hostent *host = NULL;
	struct sockaddr_in addrSrv;

	char serverIp[]="192.168.1.1";

	if((host=gethostbyname(serverIp))==NULL){
        		printf("无法访问服务器: 192.168.1.1\n");
		exit(1);
    	}
    	sprintf(serverIp,"%s",host->h_addr_list[0]);

	addrSrv.sin_family = AF_INET;
	addrSrv.sin_port = htons(2001);
	memcpy(&addrSrv.sin_addr,host->h_addr_list[0],host->h_length);


	cout << "connecting..." << endl;
	if(connect(m_sock,(struct sockaddr*)(&addrSrv),sizeof(struct sockaddr))==-1){
        		printf("connect to server get wrong\n");
        		perror("error.txt");
        		exit(1);
    	}
	cout << "Wifi success" << endl;
	return true;
}

void WifiHelper::Close() {
	close(m_sock);
}

bool WifiHelper::Forward() {
        Stop();
        comm.comm = COM_FORWARD;
        if(write(m_sock,(char *)&comm,sizeof(UsrData))==-1){
            return false;
        }
        lastStatus=STATUS_FORWARD;
        return true;
}

bool WifiHelper::Backward() {
        Stop();
        comm.comm = COM_BACKWARD;
        if(write(m_sock,(char *)&comm,sizeof(UsrData))==-1){
            return false;
        }
        lastStatus=STATUS_BACKWARD;
        return true;
}

bool WifiHelper::Right() {
        Stop();
        comm.comm = COM_RIGHT;
        if(write(m_sock,(char *)&comm,sizeof(UsrData))==-1){
            return false;
        }
        thread_create();
        return true;
}

bool WifiHelper::Left() {
        Stop();
        comm.comm = COM_LEFT;
        if(write(m_sock,(char *)&comm,sizeof(UsrData))==-1){
            return false;
        }
        thread_create();
        return true;
}

bool WifiHelper::Stop() {
    comm.comm = COM_STOP;
    if(write(m_sock,(char *)&comm,sizeof(UsrData))==-1){
        return false;
    }
    return true;
}

bool WifiHelper::Tap() {
    Stop();
    lastStatus=STATUS_STATIC;
}

void WifiHelper::Instru_Exec(int choice) {
	switch (choice) {
		case GEST_UNRECOG: 
			//Stop(); 
			break;
		case GEST_LEFT: 
			Left(); break;
		case GEST_RIGHT: 
			Right(); break;
		case GEST_BIHAND: 
			//
			break;
		case GEST_COME: 
			Forward(); break;
		case GEST_TAP:
			Tap();	
			break;
		case GEST_AWAY:
			Backward(); break;
		default:break;
	}
}

//thread delay function
static void* threadDelay (void* ptr)
{
    //set thread:when cancel signal came,it will end immediately
    pthread_setcanceltype (PTHREAD_CANCEL_ASYNCHRONOUS, NULL);
 
    //insertt delay function :us
    usleep(4.5*1000*100);

    WifiHelper *wifi = (WifiHelper*)ptr;

    switch(wifi->lastStatus){
        case STATUS_FORWARD:
            wifi->Instru_Exec(GEST_COME);
            break;
        case STATUS_BACKWARD:
            wifi->Instru_Exec(GEST_AWAY);
            break;
        default:
            wifi->Instru_Exec(GEST_TAP);
            break;
    }
    pthread_exit (NULL);
}

//create delay thread
void WifiHelper::thread_create()
{
    pthread_t thread_delay;
    pthread_create (&thread_delay, NULL, threadDelay,this);
}

