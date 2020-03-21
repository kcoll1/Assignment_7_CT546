using System;
using System.Text;

namespace Assignment7
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            BusinessLogic businessLogic = new BusinessLogic();
            Console.WriteLine("PARKING PERMIT SYSTEM");
            Console.WriteLine("----------------------");
            Console.WriteLine();
            Console.WriteLine("List of Students, Vehicle Details and the Parking Permit Status:");
            businessLogic.OverallValidOrLapsedParkingPermits();
            Console.WriteLine();
            Console.WriteLine("List of Students with Lapsed Permits, Vehicle Details and the Parking Permit Fees Due:");
            Console.WriteLine();
            businessLogic.DetermineFeesOnLapsedPermits();
            
        }
    }
}
