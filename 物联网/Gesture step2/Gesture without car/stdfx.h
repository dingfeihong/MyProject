#pragma once
#include <iostream>
#include <stdio.h>
#include <stdlib.h>
#include <fstream>
#include <stdint.h>
#include <pthread.h>
#include <string.h>
#include <unistd.h>
#include <getopt.h>
#include <fcntl.h>
#include <signal.h>
#include <sys/time.h>
#include <sys/ioctl.h>
#include <linux/types.h>
#include <linux/spi/spidev.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h>
#include <netinet/in.h>
#include <arpa/inet.h>

typedef unsigned char byte;
#include "DataProcess.h"
#include "WifiHelper.h"

#define _USE_MATH_DEFINES
#define LENGTH 			16384	//
#define F_SAMPLE		107143	//采样率
#define ARRAY_SIZE(a) 	(sizeof(a) / sizeof((a)[0]))
#define DMASIZE 		1028
#define DMADATASIZE		(DMASIZE - 4) 
#define FFTSIZE 		LENGTH	
#define F_SOURCE		39957	//波源频率
#define REC_RANGE		35		//基频左右两侧的范围

#define THRESHOLD_SHIFT_PAUSE	1	
#define THRESHOLD_ENERGY		1000//双手能量阈值
#define THRESHOLD_SHIFT_COME	12	//靠近的快和慢的分界
#define THRESHOLD_SHIFT_AWAY	-12	//远离的快和慢的分界

#define GEST_BASE_UNRECOG		0
#define GEST_BASE_PAUSE			0
#define GEST_BASE_COME			1
#define GEST_BASE_COME_SLOW		1
#define GEST_BASE_COME_FAST		1.5
#define GEST_BASE_AWAY			-1
#define GEST_BASE_AWAY_SLOW		-1
#define GEST_BASE_AWAY_FAST		-1.5
#define GEST_BASE_BIDIRECT		2

#define GEST_UNRECOG			0
#define GEST_LEFT				1
#define GEST_RIGHT				2
#define GEST_BIHAND				3
#define GEST_TAP				4
#define GEST_COME				5
#define GEST_AWAY				6

#define THRESHOLD_TIME_LONG		12
#define THRESHOLD_COME_OR_AWAY	7	

extern float* _gestBuffer; //基元队列
extern int _rear; //队尾

using namespace std;
