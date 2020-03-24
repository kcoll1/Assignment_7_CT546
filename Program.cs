using System;
using System.Text;

namespace Assignment7
{
    class Program
    {
        static void Main(string[] args)
        {
            //Specify the encoding for purpose of displaying Euro Symbols
            Console.OutputEncoding = Encoding.UTF8;

            //Instantiate the Business Logic class
            DataLayer dataLayer = new DataLayer();
            BusinessLogic businessLogic = new BusinessLogic(dataLayer);

            Console.WriteLine("PARKING PERMIT SYSTEM");
            Console.WriteLine("----------------------");
            Console.WriteLine();
            Console.WriteLine("List of Students, Vehicle Details and the Parking Permit Status:");

            //Call the Display Valid or Lapsed Parking Permit Method
            businessLogic.DisplayOverallValidOrLapsedParkingPermits();
            Console.WriteLine();
            Console.WriteLine("List of Students with Lapsed Permits, Vehicle Details and the Parking Permit Fees Due:");
            Console.WriteLine();

            //Call the Display Fees on Lapsed Permits Method
            businessLogic.UpdateFeesOnLapsedPermitsAndDisplay();
            Console.WriteLine();
            Console.WriteLine("List of Students with Lapsed Permits, Vehicle Details and the Repayment Fees Due Inc. 10% Premium:");
            Console.WriteLine();
            businessLogic.UpdatePremiumOnLapsedAndDisplay();
        }
    }
}
