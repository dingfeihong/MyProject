#pragma once
#include "stdfx.h"
#include <math.h>

/*****数据处理，包括加窗、FFT*****/

//加汉明窗
//x为需要加窗的数组
//length为数组长度
void Hamming(float x[], int length); 

//FFT变换
//x为需要处理的数组
//length为x的数组长度
//fft保存处理后的数组
//fft_length为fft变换后的数组长
void FFT(float x[], int length);

/*****数据处理，包括加窗、FFT*****/


/*****形成手势基元队列*****/

//获取手势基元
//buffer为将要处理的数据
//返回手势基元
float RenderFrequencyDomain(byte buffer[]);

//生成基元队列,当手势结束时调用GestureMatch，返回识别的手势
int FrameProcess(byte buffer[]);

/*****形成手势基元队列*****/


/*******手势匹配*******/

//手势匹配
//gestBuffer为手势基元队列，rear为队尾
//返回识别的手势：0：未识别；1：左移；2：右移；3：双手反向；4：单拍；5：走近设备；6：远离设备
int GestureMatch(float* p_gestBuffer, int p_rear);

/*******手势匹配*******/
