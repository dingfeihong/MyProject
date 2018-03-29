#include <QCoreApplication>
#include <iostream>
#include <iomanip>

using namespace std;

int main(int argc, char *argv[])
{
    int maximumCapacity,numberOfpeople;
    while(true){
        const double  payIncrease = 0.076;
        double annualSalary,monthSalary;
        cout << "Input the annual salary,or press 0 to end the program:"<<endl;

        if(maximumCapacity==0)break;

        cin >> annualSalary;

        annualSalary*=(1+payIncrease);
        monthSalary=annualSalary/12;

        cout.setf(ios::fixed);
        cout.setf(ios::showpoint);
        cout.precision(2);

        cout << "annual Salary: " << setw(9) << annualSalary << endl;
         cout << "month Salary: " << setw(9) << monthSalary << endl;

    }
    return 0;
}
