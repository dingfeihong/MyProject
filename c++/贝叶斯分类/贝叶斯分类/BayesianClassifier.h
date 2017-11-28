#include <string>
#include <vector>
#include <set>
#include <ctime> 
#include <algorithm>
#include <cmath>
#include <map>
using namespace std;
//  	1) Alcohol
//  	2) Malic acid
//  	3) Ash
// 	4) Alcalinity of ash  
//  	5) Magnesium
// 	6) Total phenols
//  	7) Flavanoids
//  	8) Nonflavanoid phenols
//  	9) Proanthocyanins
// 	10)Color intensity
//  	11)Hue
//  	12)OD280/OD315 of diluted wines
//  	13)Proline            
int TrainNum = 130;															//所有训练数据的范围
int TestNum = 48;													
struct OriginalData
{
	double A1;
	double A2;
	double A3;
	double A4;
    double A5;
    double A6;
	double A7;
	double A8;
	double A9;
	double A10;
	double A11;
	double A12;
	double A13;
	double A14;
};
