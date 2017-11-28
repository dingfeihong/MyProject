#include "stdfx.h"

#define DEBUG_FFT				1	//是否进行FFT变换
#define DEBUG_BUFFUL_PRINT		0	//FFT缓冲区满则打印一个0
#define DEBUG_SAVE_BIN_FILE 	0	//是否将采集到的数据存到sample.bin文件(注意：要关闭FFT并打印0)
#define DEBUG_SAVE_SEQ_FILE 	0	//是否将序列号数据存到seq.txt文件(注意：要关闭FFT并打印0)
#define DEBUG_WIFICAR			0	//是否根据识别的手势号驱动小车
#define FFT_MAX_COUNT 			50000

static char *device = "/dev/spidev0.0";
static uint8_t mode;
static uint8_t bits = 8;
static uint32_t speed = 4*1000*1000;
static uint16_t delay;
static uint16_t sampleno;

//thread :deliver buffer[64] to big buffer[32768]
pthread_t thread_deliver;

//dmabuffer
uint8_t rx[DMASIZE] = { 0, };

//rx buf has 3 status
int rxbufStatus = 0;

//num of bytes within DMADATASIZE hava been read
int numRead = 0;

//fftbuffer*2
uint8_t fftbuffer[FFTSIZE * 2] = { 0, };

//fft block num
int fftblocknum = 0;

//the index of next write bit of fftffer
int fftbufferindex = 0;

//for test:  print the sequence of each packet
int packetSeqIndex = 0;
uint8_t packetSeq[3600];

//for test: print the raw all packet
int rawPacketIndex = 0;
uint8_t rawPacket[51600];

//fftblocl is full :flag
int fftblock1isfull = 0;
int fftblock2isfull = 0;

//WifiHelper for control car
WifiHelper wifi;

FILE *fileBin;
FILE *fileSeq;

void transfer (int fd);
void configSPI (int fd);

void openBinFile()
{
	fileBin = fopen("sample.bin", "w");	//wait for changing
}

int writeTime=0;
void appendFile(uint8_t *buffer)
{
	
	fwrite(buffer, FFTSIZE, 1, fileBin);	
	fflush(fileBin);
	
	writeTime++;
	if(writeTime % 50 == 0){
		fclose(fileBin);
		openBinFile();
	}
}

void openSeqFile()
{
	fileSeq = fopen("seq.txt", "w");	//wait for changing
}

void appendSeqFile(int seq_num)
{
	static int seqCount = 0;
	fprintf(fileSeq, "%.2X %.2X ", seq_num>>8 & 0xff, seq_num & 0xff);
	seqCount++;
	if(seqCount % 32 == 0)
		fflush(fileSeq);
	
}

static void pabort (const char *s)
{
	perror (s);
	abort ();
}

void toFFTbuffer (int start, int end)
{

	//printf("%d %d %d\n",fftbufferindex,start,end);
	int i = 0;
	int len = end - start + 1;
	if (fftbufferindex % FFTSIZE + len < FFTSIZE)
	{
		memcpy(fftbuffer+fftbufferindex,rx+start,len);
		fftbufferindex+=len;
	}
	else
	{     len=((FFTSIZE * 2 - fftbufferindex) % FFTSIZE);

		memcpy(fftbuffer+fftbufferindex,rx+start,len);
		fftbufferindex+=len;	

		if (fftbufferindex == FFTSIZE)
			fftblock1isfull = 1;
		else
		{
			//cout<<"fftbufferindex "<<fftbufferindex<<endl;
			fftblock2isfull = 1;
			fftbufferindex = 0;
		}

		memcpy(fftbuffer+fftbufferindex,rx+start+len,end-start-len+1);
		fftbufferindex+=(end-start-len+1);

	}

}

//thread deliver function
void *threadDeliver (void* ptr)
{
	//open spi device
	int fd;
	fd = open (device, O_RDWR);
	if (fd < 0)
		pabort ("can't open device");
	configSPI (fd);

	//set thread:when cancel signal came,it will end immediately
	pthread_setcanceltype (PTHREAD_CANCEL_ASYNCHRONOUS, NULL);
	//circle to transfer data from spi
	transfer (fd);

	close (fd);
	pthread_exit (NULL);
}

//create deliver thread
void transfer_thread_create (void)
{
	int temp;
	if ((temp = pthread_create (&thread_deliver, NULL, threadDeliver, NULL)) != 0)
	{
		printf ("create fail!\n");
	}
}


void transfer (int fd)
{
	int ret, i;
	uint8_t tx[DMASIZE] = { 0, };
	struct spi_ioc_transfer tr;

	tr.tx_buf = (unsigned long) tx;
	tr.rx_buf = (unsigned long) rx;
	tr.len = ARRAY_SIZE (tx);
	tr.delay_usecs = delay;
	tr.speed_hz = speed;
	tr.bits_per_word = bits;
	
	i = 1;

	if(DEBUG_SAVE_SEQ_FILE)
		openSeqFile();

	while (1)
	{
		//set thread cancel point
		pthread_testcancel ();

		ret = ioctl (fd, SPI_IOC_MESSAGE (1), &tr);

		uint8_t data8 = 0x0;
		int rxindex = 0;
		int seq;

		while (rxindex < DMASIZE)
		{
			data8 = rx[rxindex];
			switch (rxbufStatus)
			{
			case 0:
				if (data8 == 0xAA)
				{
					rxbufStatus = 1;
				}
				break;
			case 1:
				if (data8 == 0x55)
					rxbufStatus = 2;
				else
					rxbufStatus = 0;
				break;
			case 2:
				if(DEBUG_SAVE_SEQ_FILE)
					seq = data8;
				rxbufStatus = 3;
				break;
			case 3:
				if(DEBUG_SAVE_SEQ_FILE)
				{
					seq = (seq << 8) | ( data8 & 0xff);
					appendSeqFile(seq);
				}
				rxbufStatus = 4;
				break;
			case 4:
				//if the left in DMASIZE is less than DMADATASIZE need
				if ((DMADATASIZE - numRead) > (DMASIZE - rxindex))
				{

					numRead += (DMASIZE - rxindex);
				//then transfer the data rx[rxindex..DMASIZE -1] to fftbuf
					toFFTbuffer (rxindex, DMASIZE - 1);
					rxindex = DMASIZE - 1;
				}
				else
				{
				//or transfer rx[rxindex..DMADATASIZE-numRead+rxindex-1] to fftbuf
					toFFTbuffer (rxindex, DMADATASIZE - numRead + rxindex - 1);
					rxindex = DMADATASIZE - numRead + rxindex - 1;
					rxbufStatus = 0;
					numRead = 0;
				}
				break;
			default:
				break;
			}
			rxindex++;
		}
	}
}

void print_usage (char *prog)
{
	printf ("Usage: %s [-DsbdlHOLC3]\n", prog);
	puts (" -D --device   device to use (default /dev/spidev1.1)\n"
		  " -s --speed    max speed (Hz)\n"
		  " -d --delay    delay (usec)\n"
		  " -b --bpw      bits per word \n"
		  " -l --loop     loopback\n"
		  " -H --cpha     clock phase\n"
		  " -O --cpol     clock polarity\n" " -L --lsb      least significant bit first\n" " -C --cs-high chip select active high\n" " -3 --3wire    SI/SO signals shared\n");
	exit (1);
}

void parse_opts (int argc, char *argv[])
{
	while (1)
	{
		static struct option lopts[] = {
			{"device", 1, 0, 'D'},
			{"speed", 1, 0, 's'},
			{"delay", 1, 0, 'd'},
			{"bpw", 1, 0, 'b'},
			{"loop", 0, 0, 'l'},
			{"cpha", 0, 0, 'H'},
			{"cpol", 0, 0, 'O'},
			{"lsb", 0, 0, 'L'},
			{"cs-high", 0, 0, 'C'},
			{"3wire", 0, 0, '3'},
			{"sampleno", 0, 0, 'n'},			
			{NULL, 0, 0, 0},
		};
		int c;
		c = getopt_long (argc, argv, "D:s:d:b:lHOLC3n", lopts, NULL);
		if (c == -1)
			break;
		switch (c)
		{
		case 'D':
			device = optarg;
			break;
		case 's':
			speed = atoi (optarg);
			break;
		case 'd':
			delay = atoi (optarg);
			break;
		case 'b':
			bits = atoi (optarg);
			break;
		case 'l':
			mode |= SPI_LOOP;
			break;
		case 'H':
			mode |= SPI_CPHA;
			break;
		case 'O':
			mode |= SPI_CPOL;
			break;
		case 'L':
			mode |= SPI_LSB_FIRST;
			break;
		case 'C':
			mode |= SPI_CS_HIGH;
			break;
		case '3':
			mode |= SPI_3WIRE;
			break;
		case 'n':
			sampleno=atoi (optarg);
		default:
			print_usage (argv[0]);
			break;
		}
	}
}

void configSPI (int fd)
{
	int ret = 0;
	/*
	 * spi mode
	 */
	//mode &= ~SPI_CPHA;
	mode |= SPI_CPOL;
	mode |= SPI_CPHA;
	ret = ioctl (fd, SPI_IOC_WR_MODE, &mode);
	if (ret == -1)
		pabort ("can't set spi mode");
	ret = ioctl (fd, SPI_IOC_RD_MODE, &mode);
	if (ret == -1)
		pabort ("can't get spi mode");
	/*
	 * bits per word
	 */
	ret = ioctl (fd, SPI_IOC_WR_BITS_PER_WORD, &bits);
	if (ret == -1)
		pabort ("can't set bits per word");
	ret = ioctl (fd, SPI_IOC_RD_BITS_PER_WORD, &bits);
	if (ret == -1)
		pabort ("can't get bits per word");
	/*
	 * max speed hz
	 */
	ret = ioctl (fd, SPI_IOC_WR_MAX_SPEED_HZ, &speed);
	if (ret == -1)
		pabort ("can't set max speed hz");
	ret = ioctl (fd, SPI_IOC_RD_MAX_SPEED_HZ, &speed);
	if (ret == -1)
		pabort ("can't get max speed hz");
	printf ("spi mode: %d\n", mode);
	printf ("bits per word: %d\n", bits);
	printf ("max speed: %d Hz (%d KHz)\n", speed, speed / 1000);
}

void testPrint (int printType)
{
	int newindex;
	switch (printType)
	{
	//print the fft buffer
	case 1:
		for (newindex = 0; newindex < FFTSIZE*2; newindex++)
		{
			if (newindex % 32 == 0)
				puts ("");
			printf ("%.2X ", fftbuffer[newindex]);
		}
		puts ("");
		break;
	//print 256 (sequence of packet)
	case 2:
		for (newindex = 0; newindex < 256; newindex++)
		{
			if (newindex % 16 == 0)
				puts ("");
			printf ("%.2X ", packetSeq[newindex]);
		}
		puts ("");
		break;
	//print raw all packet
	case 3:	
		for(newindex = 0;newindex<51600;newindex++)
		{
			if(newindex % 26 == 0)
				puts("");
			printf("%0.2X ",rawPacket[newindex]);
		}
		puts("");
		break;
	default:
		break;
	}
	
}

void fftprocess ()
{
	//show which block will do fft
	int flag = 1;
	int flagend = 0;
    int gestureJudge=0;
	int fftCount = 0;

	if(DEBUG_SAVE_BIN_FILE)
		openBinFile();

	while (1)
	{
		switch (flag)
		{
		case 1:
			if (fftblock1isfull)
			{
				//do fft fftbuffer[0] 0---FFTSIZE-1
				if(DEBUG_FFT)
    				gestureJudge=FrameProcess(fftbuffer);
				
				//driver the car
				if(DEBUG_WIFICAR)
					wifi.Instru_Exec(gestureJudge);
				

				if(DEBUG_BUFFUL_PRINT)
				{
					fftCount++;
					cout<<'0';
					if(fftCount % 10 == 0)
						cout<<' ';
					if(fftCount % 50 == 0)
						cout<<endl;
					fflush(stdout);
				}
				if(DEBUG_SAVE_BIN_FILE)
					appendFile(fftbuffer);
				fftblock1isfull = 0;
				flag = 2;
			}
			break;
		case 2:
			if (fftblock2isfull)
			{
				//do fft fftbuffer[1] FFTSIZE----2*FFTSIZE-1
				if(DEBUG_FFT)
           			gestureJudge=FrameProcess(fftbuffer+FFTSIZE);

				//driver the car
				if(DEBUG_WIFICAR)
					wifi.Instru_Exec(gestureJudge);

				if(DEBUG_BUFFUL_PRINT)
				{
					fftCount++;
					cout<<'0';
					if(fftCount % 10 == 0)
						cout<<' ';
					if(fftCount % 50 == 0)
						cout<<endl;
					fflush(stdout);
				}
				if(DEBUG_SAVE_BIN_FILE)
					appendFile(fftbuffer+FFTSIZE);
				fftblock2isfull = 0;
				flag = 1;
			}
			break;
		default:
			flag = 1;
			break;
		}
		gestureJudge=0;

		if(fftCount >= FFT_MAX_COUNT)
			break;
	}
}

int main (int argc, char *argv[])
{
	//get the input argc
	parse_opts (argc, argv);

	//initial the wifihelper
	//wifi.InitWIFISocket();

	//start a thread for dma between from spi and dma buffer
	transfer_thread_create ();

	//circle to process fft
	fftprocess ();

	pthread_cancel(thread_deliver);

	pthread_join(thread_deliver ,NULL);

	return 0;
}
