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
            BusinessLogic businessLogic = new BusinessLogic();

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
            businessLogic.DisplayFeesOnLapsedPermits();
            
        }
    }
}
