#include <iostream>
#include <iomanip>

using namespace std;

int main(int argc, char *argv[])
{
    const double MATRIXTON_2_OUNCES = 35273.92;

    double weightOfCerealInOunces, weightOfCerealInTon,tonPerBox;
    cout << "Input the weight of a package of breakfast cereal in ounces: ";
    cin >> weightOfCerealInOunces;

    weightOfCerealInTon = weightOfCerealInOunces / MATRIXTON_2_OUNCES;
    if(weightOfCerealInTon!=0)
        tonPerBox = 1 / weightOfCerealInTon;
    else
        tonPerBox=0;
    // --------------- OUTPUT:  ---------------
    //format output:
    //cout<<std::setprecision(2)<<std::fixed<<std::showpoint<<endl;
    cout.setf(ios::fixed);
    cout.setf(ios::showpoint);
    cout.precision(2);


    cout << "Output the weight in metric tons: " << setw(9) << weightOfCerealInTon << endl;

    cout << "Output the number of boxes needed to yield 1 metric ton of cereal: " << setw(9) << tonPerBox << endl;;
    return 0;
}
