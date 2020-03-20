using System;

namespace Assignment7
{
    class Program
    {
        static void Main(string[] args)
        {
            
            BusinessLogic businessLogic = new BusinessLogic();
            Console.WriteLine("PARKING PERMIT SYSTEM");
            Console.WriteLine("----------------------");
            businessLogic.CalculateNumberOfParkingPermits();
            
        }
    }
}
