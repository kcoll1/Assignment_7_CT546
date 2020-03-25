using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace Assignment7
{
    public class BusinessLogic
    {
        //Create a list of vehicles to store values from the DB table
        private List<Vehicle> _vehicles = new List<Vehicle>();
        private DataLayer _dataLayer;

        public BusinessLogic(DataLayer dataLayer)
        {
            _dataLayer = dataLayer;

        }

        //Variables for Valid and Lapsed Count
        int ValidCount = 0;
        int LapsedCount = 0;

        //Method to Show and Calculate the Valid and Lapsed Parking Permits
        public void DisplayOverallValidOrLapsedParkingPermits()
        {
            _dataLayer.ConnectToDatabase(Constants.connectionString);
            _vehicles = _dataLayer.ReturnAllVehicles();

            Console.WriteLine();
                //For Each vehicle read into the Vehicles array, 
                // call the Method to check if their Parking Permit is Lapsed or Valid, Increment each counter according to the result and Display
           foreach (Vehicle v in _vehicles)
           {
                    if (CalculateIfParkingPermitExpired(v.Permit_Start, v.Permit_Duration) == false)
                    {
                        LapsedCount++;
                        Console.WriteLine(v.Id.ToString() + "   " + v.Owner.ToString() + "   " + v.Model.ToString() + "   " + v.Reg.ToString() + "   Apartment No." + v.Apartment.ToString() + "   Permit Start Date: " + v.Permit_Start.ToString("dd/MM/yyyy") + "   Permit Duration:" + v.Permit_Duration.ToString() + " Months   - LAPSED PARKING PERMIT");
                    }
                    else
                    {
                        ValidCount++;
                        Console.WriteLine(v.Id.ToString() + "   " + v.Owner.ToString() + "   " + v.Model.ToString() + "   " + v.Reg.ToString() + "   Apartment No." + v.Apartment.ToString() + "   Permit Start Date: " + v.Permit_Start.ToString("dd/MM/yyyy") + "   Permit Duration:" + v.Permit_Duration.ToString() + " Months   - VALID PARKING PERMIT");
                    }

           }
                Console.WriteLine();

                //Print to Console the overall number of Lapsed and Valid Parking Permits
                Console.WriteLine("Overall Number of Lapsed Parking Permits: " + LapsedCount);
                Console.WriteLine("Overall Number of Valid Parking Permits: " + ValidCount);

                Console.WriteLine();
        }

        //Method to Calculate the Expiry Date of the Parking Permit and Return false if Parking is Lapsed
        public bool CalculateIfParkingPermitExpired(DateTime startDate, int Duration)
        {
            //Calculate the Expiry Date of the Parking Permit
            DateTime ExpiryDate = startDate.AddMonths(Duration);

            if (ExpiryDate < DateTime.Now)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /* Method to return the records from the Database and iterate through the Vehicle items, calls the below
        CalculateFeeOfLapsed Method to Determine the Fee due based on how long the permit is out of Date (3 bands of Fee: 20.00, 60.00, 100.00)
        */
        public void UpdateFeesOnLapsedPermitsAndDisplay()
        {
            using (DataLayer _dataLayer = new DataLayer())
            {

                try 
                { 
                _dataLayer.ConnectToDatabase(Constants.connectionString);
                _vehicles = _dataLayer.ReturnAllVehicles();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


                foreach (Vehicle v in _vehicles)
                {
                    if (CalculateIfParkingPermitExpired(v.Permit_Start, v.Permit_Duration) == false)
                    {
                        double LapsedFee = CalculateFeeOfLapsed(v.Permit_Start, v.Permit_Duration);

                        _dataLayer.UpdateFeesInDatabase((int.Parse(v.Id)), LapsedFee);
                        Console.WriteLine(v.Id.ToString() + "   " + v.Owner.ToString() + "   " + v.Model.ToString() + "   " + v.Reg.ToString() + "   Apartment No." + v.Apartment.ToString() + "   Permit Start Date: " + v.Permit_Start.ToString("dd/MM/yyyy") + "   Permit Duration:" + v.Permit_Duration.ToString() + " Months   - Fees Due: \u20AC" + LapsedFee);
                        
                    }
                }
            }
            Console.WriteLine();
        }

        //Method to calculate the Fee from Lapsed Permit based on the Length of Days that the Permit has been expired
        public double CalculateFeeOfLapsed(DateTime startDate, int Duration)
        {

            //Calculate the Expiry Date of the Parking Permit
            DateTime ExpiryDate = startDate.AddMonths(Duration);

            //Subtract the Expiry Date from Todays Date and place result in a TimeSpan Object
            TimeSpan ts = DateTime.Now.Subtract(ExpiryDate);

            //Define the TimeSpan Objects to compare against
            TimeSpan minorFeeTimeSpan = new TimeSpan(15, 0, 0, 0);
            TimeSpan midFeeTimeSpan = new TimeSpan(30, 0, 0, 0);
            TimeSpan maxFeeTimeSpan = new TimeSpan(50, 0, 0, 0);

            //Define the double Fee amounts
            double minorFee = 20.00;
            double midFee = 60.00;
            double maxFee = 100.00;

            //If statements to determine if the users permit warrants a minor, mid-size or Maximum fee, returns the Fee
            if (ts < maxFeeTimeSpan)
            {
                if (ts < midFeeTimeSpan)
                {
                    if (ts < minorFeeTimeSpan)
                    {
                        return minorFee;
                    }
                    else
                    {
                        return minorFee;
                    }
                }
                else
                {
                    return midFee;
                }
            }
            else
            {
                return maxFee;
            }
        }

        public void UpdatePremiumOnLapsedAndDisplay()
        {

            using (DataLayer _dataLayer = new DataLayer())
            {

                try
                {
                    _dataLayer.ConnectToDatabase(Constants.connectionString);
                    _vehicles = _dataLayer.ReturnAllVehicles();

                }
                catch
                {
                    Console.WriteLine("Issue When returning the Vehicles.");

                }

                foreach (Vehicle v in _vehicles)
                {
                    
                    if (CalculateIfParkingPermitExpired(v.Permit_Start, v.Permit_Duration) == false)
                    {
                        double RepaymentAmount = double.Parse(CalculateRepaymentFeeOnLapsed(v.Permit_Duration).ToString("F"));
                        _dataLayer.UpdatePaymentAmountInDatabase((int.Parse(v.Id)), RepaymentAmount);
                        Console.WriteLine(v.Id.ToString() + "   " + v.Owner.ToString() + "   " + v.Model.ToString() + "   " + v.Reg.ToString() + "   Apartment No." + v.Apartment.ToString() + "   Permit Start Date: " + v.Permit_Start.ToString("dd/MM/yyyy") + "   Permit Duration:" + v.Permit_Duration.ToString() + " Months   - Calculated Repayment Amount including 10% Premium Due: \u20AC" + RepaymentAmount);


                    }
                }
            }
            Console.WriteLine();
        }

        public double CalculateRepaymentFeeOnLapsed(int Duration)
        {
            //Rate per month for a Permit
            double ratePerMonth = 10.00;

            double AmountWithoutPremium;
            double AmountIncludingPremium;

            //Calulate how much the customer paid for their most recent Parking Permit
            AmountWithoutPremium = (ratePerMonth * Duration);

            //Calulate how much the customer would need to pay for the same term again including a 10% premium for lapsed parking permit
            AmountIncludingPremium = AmountWithoutPremium += (AmountWithoutPremium * 10 / 100);
            return AmountIncludingPremium;      
        }

        public void ReallocatePermitToSameUsersCar()
        {
            //Allow a permit to be reallocated to a different car owned by the same user
            using (DataLayer _dataLayer = new DataLayer())
            {

                try
                {
                    _dataLayer.ConnectToDatabase(Constants.connectionString);
                    _vehicles = _dataLayer.ReturnAllVehicles();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine();
                Console.WriteLine("List of Students with Valid Permits that can be transferred:");
                Console.WriteLine();
                foreach (Vehicle v in _vehicles)
                {
                    if (CalculateIfParkingPermitExpired(v.Permit_Start, v.Permit_Duration) == true)
                    {

                        Console.WriteLine(v.Id.ToString() + "   " + v.Owner.ToString() + "   " + v.Model.ToString() + "   " + v.Reg.ToString() + "   Apartment No." + v.Apartment.ToString() + "   Permit Start Date: " + v.Permit_Start.ToString("dd/MM/yyyy") + "   Permit Duration:" + v.Permit_Duration.ToString() + " Months");

                    }
                }
            }
                Console.WriteLine();
                Console.WriteLine("Please Enter the ID of the Users Parking Permit that you would like to transfer:");
                int userIDForPermitReallocation = Convert.ToInt32(Console.ReadLine());

            //_dataLayer.GetVehicleById(userIDForPermitReallocation);
            using (DataLayer _dataLayer = new DataLayer())
            {
                _dataLayer.ConnectToDatabase(Constants.connectionString);
                Vehicle vehicleForPermitTransfer = _dataLayer.GetVehicleById(userIDForPermitReallocation);

                Console.WriteLine("Please Enter the Model of the New Vehicle the Parking Permit will be transferred to:");
                string NewVehicleModel= Console.ReadLine();

                Console.WriteLine("Please Enter the Registration of the New Vehicle the Parking Permit will be transferred to:");
                string NewVehicleReg = Console.ReadLine();
                Console.WriteLine();

                vehicleForPermitTransfer.Model = NewVehicleModel;
                vehicleForPermitTransfer.Reg = NewVehicleReg;

                _dataLayer.DeleteAVehicle(Convert.ToInt32(vehicleForPermitTransfer.Id));
                _dataLayer.AddVehicle(vehicleForPermitTransfer);

                List<Vehicle> NewVehicles = _dataLayer.ReturnAllVehicles();
                List<Vehicle> SortedList = NewVehicles.OrderBy(newVec => newVec.Id).ToList();
                Console.WriteLine();
                Console.WriteLine("List of Students with Valid Permits and Vehicle Details:");
                Console.WriteLine();
                foreach (Vehicle v in SortedList)
                {
                    //  _dataLayer.UpdateFeesInDatabase((int.Parse(v.Id)), LapsedFee);
                    Console.WriteLine(v.Id.ToString() + "   " + v.Owner.ToString() + "   " + v.Model.ToString() + "   " + v.Reg.ToString() + "   Apartment No." + v.Apartment.ToString() + "   Permit Start Date: " + v.Permit_Start.ToString("dd/MM/yyyy") + "   Permit Duration:" + v.Permit_Duration.ToString() + " Months");

                }
            
            }

            Console.WriteLine();

        }

    }

}
