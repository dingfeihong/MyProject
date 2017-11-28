#include "DataProcess.h"

#define DEBUG_GESTURE_MATCH			1
#define DEBUG_GESTURE_PAUSE			1
#define DEBUG_BASE					1
#define DEBUG_UNRECOGNIZE_BASE		0
#define DEBUG_PRINT_SHIFT_ENERGY	0

static int nu=0;
static float* _fft; //用于保存fft后的数据
static int fft_length; //fft后的数据长度
float* _gestBuffer=NULL; //基元队列
int _rear=0; //队尾

static const unsigned char BitReverseTable256[] = 
{
  0x00, 0x80, 0x40, 0xC0, 0x20, 0xA0, 0x60, 0xE0, 0x10, 0x90, 0x50, 0xD0, 0x30, 0xB0, 0x70, 0xF0, 
  0x08, 0x88, 0x48, 0xC8, 0x28, 0xA8, 0x68, 0xE8, 0x18, 0x98, 0x58, 0xD8, 0x38, 0xB8, 0x78, 0xF8, 
  0x04, 0x84, 0x44, 0xC4, 0x24, 0xA4, 0x64, 0xE4, 0x14, 0x94, 0x54, 0xD4, 0x34, 0xB4, 0x74, 0xF4, 
  0x0C, 0x8C, 0x4C, 0xCC, 0x2C, 0xAC, 0x6C, 0xEC, 0x1C, 0x9C, 0x5C, 0xDC, 0x3C, 0xBC, 0x7C, 0xFC, 
  0x02, 0x82, 0x42, 0xC2, 0x22, 0xA2, 0x62, 0xE2, 0x12, 0x92, 0x52, 0xD2, 0x32, 0xB2, 0x72, 0xF2, 
  0x0A, 0x8A, 0x4A, 0xCA, 0x2A, 0xAA, 0x6A, 0xEA, 0x1A, 0x9A, 0x5A, 0xDA, 0x3A, 0xBA, 0x7A, 0xFA,
  0x06, 0x86, 0x46, 0xC6, 0x26, 0xA6, 0x66, 0xE6, 0x16, 0x96, 0x56, 0xD6, 0x36, 0xB6, 0x76, 0xF6, 
  0x0E, 0x8E, 0x4E, 0xCE, 0x2E, 0xAE, 0x6E, 0xEE, 0x1E, 0x9E, 0x5E, 0xDE, 0x3E, 0xBE, 0x7E, 0xFE,
  0x01, 0x81, 0x41, 0xC1, 0x21, 0xA1, 0x61, 0xE1, 0x11, 0x91, 0x51, 0xD1, 0x31, 0xB1, 0x71, 0xF1,
  0x09, 0x89, 0x49, 0xC9, 0x29, 0xA9, 0x69, 0xE9, 0x19, 0x99, 0x59, 0xD9, 0x39, 0xB9, 0x79, 0xF9, 
  0x05, 0x85, 0x45, 0xC5, 0x25, 0xA5, 0x65, 0xE5, 0x15, 0x95, 0x55, 0xD5, 0x35, 0xB5, 0x75, 0xF5,
  0x0D, 0x8D, 0x4D, 0xCD, 0x2D, 0xAD, 0x6D, 0xED, 0x1D, 0x9D, 0x5D, 0xDD, 0x3D, 0xBD, 0x7D, 0xFD,
  0x03, 0x83, 0x43, 0xC3, 0x23, 0xA3, 0x63, 0xE3, 0x13, 0x93, 0x53, 0xD3, 0x33, 0xB3, 0x73, 0xF3, 
  0x0B, 0x8B, 0x4B, 0xCB, 0x2B, 0xAB, 0x6B, 0xEB, 0x1B, 0x9B, 0x5B, 0xDB, 0x3B, 0xBB, 0x7B, 0xFB,
  0x07, 0x87, 0x47, 0xC7, 0x27, 0xA7, 0x67, 0xE7, 0x17, 0x97, 0x57, 0xD7, 0x37, 0xB7, 0x77, 0xF7, 
  0x0F, 0x8F, 0x4F, 0xCF, 0x2F, 0xAF, 0x6F, 0xEF, 0x1F, 0x9F, 0x5F, 0xDF, 0x3F, 0xBF, 0x7F, 0xFF
};

static const unsigned char BitReverseTable64[] = 
{
  0x00,0x20,0x10,0x30,
  0x08,0x28,0x18,0x38,
  0x04,0x24,0x14,0x34,
  0x0C,0x2C,0x1C,0x3C,
  0x02,0x22,0x12,0x32,
  0x0A,0x2A,0x1A,0x3A,
  0x06,0x26,0x16,0x36,
  0x0E,0x2E,0x1E,0x3E,
  0x01,0x21,0x11,0x31,
  0x09,0x29,0x19,0x39,
  0x05,0x25,0x15,0x35,
  0x0D,0x2D,0x1D,0x3D,
  0x03,0x23,0x13,0x33,
  0x0B,0x2B,0x1B,0x3B,
  0x07,0x27,0x17,0x37,
  0x0F,0x2F,0x1F,0x3F 
};

static const unsigned char BitReverseTable32[] = {

    0x00, 0x10, 0x08, 0x18,
    0x04, 0x14, 0x0c, 0x1c,
    0x02, 0x12, 0x0a, 0x1a,
    0x06, 0x16, 0x0e, 0x1e,
    0x01, 0x11, 0x09, 0x19,
    0x05, 0x15, 0x0d, 0x1d,
    0x03, 0x13, 0x0b, 0x1b,
    0x07, 0x17, 0x0f, 0x1f
};

//位转换
int BitReverse(int j)
{
	
	return (BitReverseTable256[(j >> 5) & 0xff]) |
	(BitReverseTable32[j & 0x1f] << 8);
}

//加汉明窗
void Hamming(float x[], int length)
{
	float *wn = new float[length];
	//float* wn = new float[length];
	for (int i = 0; i < length; i++)
	{
		wn[i] = 0.54 - 0.46 * cos(2 * M_PI * i / (length - 1));
		x[i] = wn[i] * x[i];
	}
}

//FFT变换
void FFT(float x[], int length)
{
	//对float[] x 进行加窗处理
	Hamming(x, length);

	int n = length;
	nu = (int)(log((float)n) / log(2.0));
	int n2 = n / 2;
	int nu1 = nu - 1;

	float xre[LENGTH / 2];
	float xim[LENGTH / 2];
	float *magnitude = new float[n2];
	//float *decibel = new float[n2];

	float tr, ti, p, arg, c, s;
	for (int i = 0; i < n; i++)
	{
		xre[i] = x[i];
		xim[i] = 0.0f;
	}
	int k = 0;
	for (int l = 1; l <= nu; l++)
	{
		while (k < n)
		{
			for (int i = 1; i <= n2; i++)
			{
				p = BitReverse(k >> nu1);
				arg = 2 * (float)M_PI * p / n;
				c = (float)cos(arg);
				s = (float)sin(arg);
				tr = xre[k + n2] * c + xim[k + n2] * s;
				ti = xim[k + n2] * c - xre[k + n2] * s;
				xre[k + n2] = xre[k] - tr;
				xim[k + n2] = xim[k] - ti;
				xre[k] += tr;
				xim[k] += ti;
				k++;
			}
			k += n2;
		}
		k = 0;
		nu1--;
		n2 = n2 / 2;
	}
	k = 0;
	int r;
	while (k < n)
	{
		r = BitReverse(k);
		if (r > k)
		{
			tr = xre[k];
			ti = xim[k];
			xre[k] = xre[r];
			xim[k] = xim[r];
			xre[r] = tr;
			xim[r] = ti;
		}
		k++;
	}
	for (int i = 0; i < n / 2; i++)
		_fft[i] = 10.0 * log10((float)(sqrt((xre[i] * xre[i]) + (xim[i] * xim[i]))));
	fft_length = n / 2;
}

float Max(float A_range[], int start, int end)
{
	float maxmum = 0;
	for (int i = 0; i < end - start + 1; i++)
	{
		if (A_range[i + start] > maxmum)
		{
			maxmum = A_range[i];
		}
	}
	return maxmum;
}

float* get_energy(float fre_range[], float A_range[])
{
	float* result = new float[3];
	float energy_right = 0;
	float energy_left = 0;
	for (int j = 0; j < REC_RANGE / 2; j++)
	{
		energy_left = energy_left + A_range[j + 0];
		energy_right = energy_right + A_range[j + REC_RANGE / 2 + 1];
	}
	float delta_energy = energy_right - energy_left;
	result[0] = delta_energy;
	result[1] = energy_left;
	result[2] = energy_right;
	return result;
}

//手势基元识别
float basicGest_rec(float fre[], float A[])
{

	float* fre_range = new float[REC_RANGE];
	float* A_range = new float[REC_RANGE];
	float* result_energy = new float[3];
	float energy, borderleft, borderright, bwidth, qujian, velocity, energy_left, energy_right, amplitude;
	borderright = 0;
	borderleft = 0;
	bwidth = 0;
	qujian = 0;
	velocity = 999;
	energy_left = 0;
	energy_right = 0;
	for (int i = 0; i < REC_RANGE; i++)
	{
		fre_range[i] = fre[i + (int) ((float) F_SOURCE / F_SAMPLE  * LENGTH / 2.0 - REC_RANGE / 2) ];
		A_range[i] = A[i + (int) ((float) F_SOURCE / F_SAMPLE * LENGTH / 2.0 - REC_RANGE / 2) ];
	}
	result_energy = get_energy(fre_range, A_range);//获取两侧能量
	energy = result_energy[0];
	energy_left = result_energy[1];
	energy_right = result_energy[2];
	int band_start = REC_RANGE / 2; //基频所处的位置
	float max = 0;
	int max_index = 0;
	for (int i = 0; i < REC_RANGE; i++)
	{
		if (A_range[i] > A_range[max_index])
		{
			max_index = i;
			max = A_range[i];
		}
	}

	int yuzhi = (int)(max * 5 / 6);
	float gest_base = 888;
	float charac_A = A_range[band_start];
	amplitude = A_range[band_start];

	float shift = max_index - band_start;

	if(DEBUG_PRINT_SHIFT_ENERGY)
		cout << "shift:{" << shift<< "},left_en:{" <<energy_left<< "},right_en:{" <<energy_right<< "}" <<endl;

	if (fabs(shift) <= THRESHOLD_SHIFT_PAUSE)
	{
		gest_base = GEST_BASE_PAUSE;
		if(DEBUG_GESTURE_PAUSE)
			cout<<"                 ******** 暂停！*******                  "<<endl;
	}
	else
	{
		if (energy_left > THRESHOLD_ENERGY - 30 && energy_right > THRESHOLD_ENERGY || energy_left > THRESHOLD_ENERGY && energy_right > THRESHOLD_ENERGY - 10)
		{
			gest_base = GEST_BASE_BIDIRECT;
			if(DEBUG_BASE)
				cout << "双手反向运动**************************************************" << endl;
		}
		else
		{
			if (shift > 0)
			{
				if (shift < THRESHOLD_SHIFT_COME)
				{
					gest_base = GEST_BASE_COME_SLOW;
					if(DEBUG_BASE)
						cout << "慢 靠近****************************" << endl;
				}
				else
				{
					gest_base = GEST_BASE_COME_FAST;
					if(DEBUG_BASE)
						cout << "快 靠近****************************" << endl;
				}
			}
			else if (shift < 0)
			{
				if (shift > THRESHOLD_SHIFT_AWAY)
				{
					gest_base = GEST_BASE_AWAY_SLOW;
					if(DEBUG_BASE)
						cout << "                             ****************************慢 远离" << endl;
				}
				else
				{
					gest_base = GEST_BASE_AWAY_FAST;
					if(DEBUG_BASE)
						cout << "                             ****************************快 远离" << endl;
				}
			}
		}
	}

	if (gest_base == GEST_BASE_UNRECOG)
	{
		//从基频向两侧扫描，找到特定点的带宽
		int num = 0;
		while (true)
		{
			//从基频出发向左边找，找基频左边的第一个边界点
			while (num < band_start && (A_range[band_start - num] >= yuzhi))
			{
				num++;
			}
			if (num >= band_start)
			{
				//出错
				gest_base = -999;
				break;
			}
			if (Max(A_range, 0, band_start - num) < yuzhi)
			{
				//左边的值都小于阈值则确定左边界点
				borderleft = fre_range[band_start - num + 1];
				break;
			}
			else
			{
				//继续找左边界点
				num++;
			}
		}
		if (gest_base != -999)
		{
			num = 0;
			while (true)
			{
				//和左边界点类似，找右边界点
				while ((num + band_start) < REC_RANGE && A_range[band_start + num] >= yuzhi)
				{
					num++;
				}
				if (num + band_start >= sizeof(A_range) / sizeof(float))
				{
					gest_base = -999;
					break;
				}
				if (Max(A_range, band_start + num, sizeof(A_range) / sizeof(float) - 1) < yuzhi)
				{
					borderright = fre_range[band_start + num - 1];
					break;
				}
				else
				{
					num++;
				}
			}
		}
		if (gest_base != -999)
		{
			//得到频率带宽；右边界点的频率－左边界点的频率
			bwidth = borderright - borderleft;
			//得到带宽差值
			qujian = fabs(borderright - F_SOURCE) - fabs(F_SOURCE - borderleft);
			if (fabs(bwidth) <= 70 || fabs(qujian) <= 20)
			{
				gest_base = GEST_BASE_UNRECOG;
			}
			else//检测到有手势时
			{
				//判断速度
				if (bwidth < 150)
				{
					velocity = -1;
				}
				else if (bwidth >= 150 && bwidth < 180)
				{
					velocity = 0;
				}
				else
				{
					velocity = 1;
				}                    //进行手势区分
									 //首先判断是否为双手运动手势
				if (energy_left > THRESHOLD_ENERGY && energy_right > THRESHOLD_ENERGY)
				{
					gest_base = GEST_BASE_BIDIRECT;
					if(DEBUG_BASE)
						cout << "双手反向运动**************************************************" << endl;
				}
				//再判断靠近远离手势
				else if (energy >= 0)//energy = energy_right - energy_left 
				{
					gest_base = GEST_BASE_COME;
					if(DEBUG_BASE)
						cout << "靠近****************************" << endl;

				}
				else
				{
					gest_base = GEST_BASE_AWAY;
					if(DEBUG_BASE)
						cout << "                             ****************************远离" << endl;
				}
			}
		}
		else
		{
			bwidth = -999;
			qujian = -999;
			if (energy >= 0)
			{
				gest_base = GEST_BASE_UNRECOG;
				//gest_base = 1;
				if(DEBUG_UNRECOGNIZE_BASE)
					cout<<"未识别 靠近****************************"<<endl;
			}
			else
			{
				gest_base = GEST_BASE_UNRECOG;
				//gest_base = -1;
				if(DEBUG_UNRECOGNIZE_BASE)
					cout<<"                             未识别****************************远离"<<endl;
			}
		}
	}

	return gest_base;
}


//获取手势基元
//buffer为将要处理的数据
//length为数据长度
//返回手势基元
float RenderFrequencyDomain(byte buffer[])
{
	float min = 1.7976931348623157E+308;
	float minHz = 0;
	float max = -1.7976931348623157E+308;
	float maxHz = 0;
	float range = 0;
	float scale = 0;

	/*fft变换前的数据处理*/
	float _wave[LENGTH / 2];
	int wave_length = LENGTH / 2;
	int h = 0;
	int count = LENGTH / 2;
	for (int i = 0; i <count; i++)
	{
		_wave[h] = (float)(buffer[2 * i] * 256 + buffer[2 * i + 1]) - 2048;
		h++;
	}

	_fft = new float[LENGTH / 4];
	FFT(_wave, LENGTH / 2); //做fft变换

	float scaleHz = (float)(F_SAMPLE / 2) / (float)fft_length;
	float* fre = new float[fft_length];
	float* A = new float[fft_length];//幅度
	float gest_base = 999;
	// get left min/max
	for (int x = 0; x <fft_length; x++)
	{
		float amplitude = _fft[x];
		if (min > amplitude)
		{
			min = amplitude;
			minHz = (float)x * scaleHz;
			//minHz = (float)x ;
		}
		if (max < amplitude && x>10)	//跳过直流分量
	
		{
			max = amplitude;
			maxHz = (float)x * scaleHz;
			//maxHz = (float)x ;
		}
		//保存2048个点的幅值和频率
		fre[x] = x * scaleHz;
		A[x] = amplitude;
	}
	//调用手势基元识别函数,第2/4个参数都是1
		gest_base = basicGest_rec(fre, A);
	return gest_base;
}

//生成基元队列,当手势结束时调用GestureMatch，返回识别的手势
int FrameProcess(byte buffer[])
{
	float gesture; //单个基元
	int isEnd = 0; //结束标志，当为1时表示手势结束
	int gesture_judge=0; //识别的手势
	
	gesture=RenderFrequencyDomain(buffer); //获取手势基元

	int Maxgsize = 200;
	int N_end = 3;
	//连续接收到N个0清空队列
	if (_gestBuffer == NULL || _rear == Maxgsize - 1)
	{
		_gestBuffer = new float[Maxgsize];
		_rear = 0;
	}
	if (_rear > N_end)
	{
		int count = 0;
		for (int i = 0; i < N_end; i++)
		{
			if (_gestBuffer[_rear - 1 - i] != 0)
			{
				break;
			}
			else
			{
				count++;
			}
		}
		if (count == N_end) isEnd = 1;
	}
	if (isEnd == 1)//当前手势结束标志
	{
		gesture_judge=GestureMatch(_gestBuffer, _rear); //识别手势
		isEnd = 0;
		_gestBuffer = new float[Maxgsize];
		_rear = 0;
	}
	if (_rear != 0 || gesture != 0)
	{
		if (_rear >= N_end)
		{
			if (gesture != 0 && _gestBuffer[_rear - 1] == 0 && _gestBuffer[_rear - 2] != 0)
			{
				_gestBuffer[_rear - 1] = gesture;//前移一位,rear位置不变
			}
			else if (gesture != 0 && _gestBuffer[_rear - 1] == 0 && _gestBuffer[_rear - 2] == 0)
			{
				_gestBuffer[_rear - 2] = gesture;//前移两位,rear位置减一
				_rear--;
			}
			else if (gesture != 0 && _gestBuffer[_rear - 1] == 0 && _gestBuffer[_rear - 2] == 0 && _gestBuffer[_rear - 3] == 0)
			{
				_gestBuffer[_rear - 2] = gesture;//前移三位,rear位置减二
				_rear = _rear - 2;
			}
			else
			{
				_gestBuffer[_rear] = gesture;
				_rear++;
			}
		}
		else
		{
			_gestBuffer[_rear] = gesture;
			_rear++;
		}
	}

	return gesture_judge;
}


//手势匹配
//gestBuffer为手势基元队列，rear为队尾
//返回识别的手势：0：未识别；1：左移；2：右移；3：双手反向；4：单拍；5：走近设备；6：远离设备
int GestureMatch(float* p_gestBuffer, int p_rear)
{
	float* gestBuffer = p_gestBuffer;
	int rear = p_rear;
	int gest_judge = GEST_UNRECOG; //识别的手势

	//首先判断是否为长时间动作
	if (rear - 1 >= THRESHOLD_TIME_LONG)
	{
		int near_time = 0;
		int far_time = 0;
		for (int i = 0; i < rear - 1; i++)
		{
			if (gestBuffer[i] >= GEST_BASE_COME_SLOW)
			{
				near_time++;
			}
			if (gestBuffer[i] <= GEST_BASE_AWAY_SLOW)
			{
				far_time++;
			}
		}
		if (near_time >= THRESHOLD_COME_OR_AWAY && far_time < THRESHOLD_COME_OR_AWAY)
		{
			gest_judge = GEST_COME;
			if(DEBUG_GESTURE_MATCH)
				cout << "                手势5 走近设备" << endl;
		}
		else if (near_time < THRESHOLD_COME_OR_AWAY && far_time >= THRESHOLD_COME_OR_AWAY)
		{
			gest_judge = GEST_AWAY;
			if(DEBUG_GESTURE_MATCH)
				cout << "                    手势6 远离设备" << endl;
		}
		else
		{
			gest_judge = GEST_UNRECOG;
		}
	}
	else if (rear - 1 >= 3)
	{
		for (int i = 0; i < rear - 1; i++)
		{
			if (gestBuffer[i] == GEST_BASE_BIDIRECT)
			{
				gest_judge = GEST_BIHAND;
				if(DEBUG_GESTURE_MATCH)
					cout << "        手势3 双手反向" << endl;
				break;
			}
		}
		if (gest_judge != GEST_BIHAND)
		{
			//进行右移，左移手势的判断
			int size = 5;//左右移动手势的持续时间>=5
			for (int i = 0; i < rear - 1 - size; i++)
			{
				if (gestBuffer[i] >= GEST_BASE_COME_SLOW && 
					gestBuffer[i + 1] >= GEST_BASE_COME_SLOW && 
					gestBuffer[i + 2] >= GEST_BASE_COME_SLOW && 
					gestBuffer[i + 3] >= GEST_BASE_COME_SLOW && 
					gestBuffer[i + 4] >= GEST_BASE_COME_SLOW) 
				{
					for (int j = 0; j < size; j++)
					{
						if (gestBuffer[i + j] == GEST_BASE_COME_FAST)
						{
							gest_judge = GEST_LEFT;
							if(DEBUG_GESTURE_MATCH)
								cout << "手势1 左移" << endl;
							break;
						}
					}
					if (gest_judge == GEST_LEFT) break;
				}
				else if (gestBuffer[i] <= GEST_BASE_AWAY_SLOW &&
						gestBuffer[i + 1] <= GEST_BASE_AWAY_SLOW && 
						gestBuffer[i + 2] <= GEST_BASE_AWAY_SLOW && 
						gestBuffer[i + 3] <= GEST_BASE_AWAY_SLOW && 
						gestBuffer[i + 4] <= GEST_BASE_AWAY_SLOW) 
				{
					for (int j = 0; j < size; j++)
					{
						if (gestBuffer[i + j] == GEST_BASE_AWAY_FAST)
						{
							gest_judge = GEST_RIGHT;
							if(DEBUG_GESTURE_MATCH)
								cout << "    手势2 右移" << endl;
							break;
						}
					}
					if (gest_judge == GEST_RIGHT) break;
				}

			}
			if (gest_judge != GEST_LEFT && gest_judge != GEST_RIGHT)
			{
				int comeTime=0;
				int comeFastTime=0;
				int awayTime=0;
				int awayFastTime=0;
				for(int i =0;i<rear-1;i++)
				{
					if(gestBuffer[i] >= GEST_BASE_COME_SLOW)
					{
						comeTime++;
						if(gestBuffer[i] == GEST_BASE_COME_FAST)
							comeFastTime++;
					}
					else if(gestBuffer[i] <= GEST_BASE_AWAY_SLOW)
					{
						awayTime++;
						if(gestBuffer[i] == GEST_BASE_AWAY_FAST)
							awayFastTime++;
					}
				}
				if(comeFastTime>0&&awayTime==0)
				{
					gest_judge = GEST_LEFT;
					if(DEBUG_GESTURE_MATCH)
						cout << "手势1 左移" << endl;
				}
				else if(awayFastTime>0&&comeTime==0)
				{
					gest_judge = GEST_RIGHT;
					if(DEBUG_GESTURE_MATCH)
						cout << "    手势2 右移" << endl;
				}
				
			}
			if (gest_judge != GEST_LEFT && gest_judge != GEST_RIGHT)
			{

				for (int i = 0; i < rear - 1 - size; i++)
				{
					int comeCount = 0;
					int awayCount = 0;
					for(int j = i; j < i + size; j++)
					{
						if(gestBuffer[j] <= GEST_BASE_AWAY_SLOW)
							awayCount++;
						if(gestBuffer[j] >= GEST_BASE_COME_SLOW)
							comeCount++;
						if(awayCount >= 2 && comeCount >= 2)
						{
							gest_judge = GEST_TAP;
							if(DEBUG_GESTURE_MATCH)
								cout << "            手势4 单拍" << endl;
							break;
						}
					}	
					if(gest_judge == GEST_TAP)	//prevent from recognizing continuous single-tap
						break;
				}
			}
		}


	}
	else if (gestBuffer[0] == GEST_BASE_BIDIRECT || 
			gestBuffer[1] == GEST_BASE_BIDIRECT || 
			gestBuffer[2] == GEST_BASE_BIDIRECT || 
			gestBuffer[3] == GEST_BASE_BIDIRECT)
	{
		gest_judge = GEST_BIHAND;
		if(DEBUG_GESTURE_MATCH)
			cout << "手势3 双手反向" << endl;
	}
	return gest_judge;
}
