#pragma once
#include <WinSock2.h>
#pragma  comment(lib,"WS2_32")
class CInitSocket
{
public:
    CInitSocket(BYTE minorVer = 2,BYTE majorVer = 2)
    {
        WSADATA wsaData;
        WORD sockVersion = MAKEWORD(minorVer,majorVer);

        if(::WSAStartup(sockVersion,&wsaData)!= 0)
        {
            exit(0);
        }
    }
    ~CInitSocket(void)
    {
        ::WSACleanup();
    }
};
