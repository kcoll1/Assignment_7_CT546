using System;
using System.Collections;
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

        //Declare the IDataLayer Interface
        private readonly IDataLayer _dataLayer;

        //Create Constructor of BusinessLogic Class that takes an IDataLayer interface object as a Parameter
        public BusinessLogic(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }

        //Variables for Valid and Lapsed Count
        private int _validVehicleCount = 0;
        private int _lapsedVehicleCount = 0;

        //Method to Show and Calculate the Valid and Lapsed Parking Permits
        public void DisplayOverallValidOrLapsedParkingPermits()
        {
            try
            {
                //Connect to DB
                _dataLayer.ConnectToDatabase(Constants.connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //Return all Vehicles from DB to the List created above
            _vehicles = _dataLayer.ReturnAllVehicles();

            _dataLayer.Dispose();
            Console.WriteLine();

            /* For Each vehicle read into the Vehicles array, call the Method to check if their Parking Permit is Lapsed or Valid, 
             Increment each counter according to the result and Display */
            foreach (Vehicle vehicle in _vehicles)
            {
                if (HasParkingPermitExpired(vehicle))
                {
                    _lapsedVehicleCount++;
                    Console.WriteLine(vehicle.ToString() + " - LAPSED PARKING PERMIT");
                }
                else
                {
                    _validVehicleCount++;
                    Console.WriteLine(vehicle.ToString() + " - VALID PARKING PERMIT");
                }

            }
            Console.WriteLine();

            //Print to Console the overall number of Lapsed and Valid Parking Permits
            Console.WriteLine("Overall Number of Lapsed Parking Permits: " + _lapsedVehicleCount);
            Console.WriteLine("Overall Number of Valid Parking Permits: " + _validVehicleCount);

            Console.WriteLine();
        }

        //Method to determine if a vehicle's permit has expired.
        public static bool HasParkingPermitExpired(Vehicle vechile)
        {

            DateTime expiryDate = vechile.Permit_Start.AddMonths(vechile.Permit_Duration);

            //If expiryDate is less than DateTime.Now, return true
            return (expiryDate < DateTime.Now) ? true : false;

        }

        /* Method to return the records from the Database and iterate through the Vehicle items, calls the below
        CalculateFeeOfLapsed Method to Determine the Fee due, based on how long the permit is out of Date 
        */
        public void UpdateFeesOnLapsedPermitsAndDisplay()
        {

            try
            {
                //Connect to DB
                _dataLayer.ConnectToDatabase(Constants.connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _vehicles = _dataLayer.ReturnAllVehicles();

            //Iterate over each vehicle in the _vehicles array
            foreach (Vehicle vechile in _vehicles)
            {
                //Call Method to determine if the Parking is expired for each record

                if (HasParkingPermitExpired(vechile))
                {
                    //Call Method to calculate the Fees Due on the Lapsed permits

                    double LapsedFee = CalculateFee(vechile);
                    DateTime ExpiryDate = vechile.Permit_Start.AddMonths(vechile.Permit_Duration);

                    //Call DataLayer class method to Update the Fees_Due column in the Database based on the given ID and print out this detail
                    _dataLayer.UpdateFeesInDatabase((int.Parse(vechile.Id)), LapsedFee);

                    Console.WriteLine(vechile.ToString() + " Months   Permit End Date: " + ExpiryDate.ToString("dd/MM/yyyy") + " - Fees Due: \u20AC" + LapsedFee);

                }

            }
            _dataLayer.Dispose();
            Console.WriteLine();
        }

        //Method to calculate the Fee from Lapsed Permit based on the Length of Days that the Permit has been expired
        public static double CalculateFee(Vehicle vehicle)
        {

            //Calculate the Expiry Date of the Parking Permit
            DateTime ExpiryDate = vehicle.Permit_Start.AddMonths(vehicle.Permit_Duration);

            //Subtract the Expiry Date from Todays Date and place result in a TimeSpan Object
            TimeSpan timeSpan = DateTime.Now.Subtract(ExpiryDate);

            //If statements to determine if the users permit warrants a minor, mid-size or Maximum fee, returns the Fees_Due (3 bands of Fee: 20.00, 60.00, 100.00)
            if (timeSpan < Constants.maxFeeTimeSpan)
            {
                if (timeSpan < Constants.midFeeTimeSpan)
                { 
                   
                        return Constants.minorFee;             
                }
                else
                {
                    return Constants.midFee;
                }
            }
            else
            {
                return Constants.maxFee;
            }
        }

        //Method to Update the RepaymentAmount including the 10% Premium due on Lapsed Parking Permit records
        public void UpdatePremiumOnLapsedAndDisplay()
        {

            try
            {
                //Call the Connect to the DB method and Return the Vehicles in DB to the _vehicles array
                _dataLayer.ConnectToDatabase(Constants.connectionString);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }

            _vehicles = _dataLayer.ReturnAllVehicles();
            //Iterate over each vehicle in the _vehicles array
            foreach (Vehicle vehicle in _vehicles)
            {
                //If the Parking Permit is expired
                if (HasParkingPermitExpired(vehicle))
                {
                    //Call the Method to calculate the Repayment Amount on Lapsed Parking Permits and store the Result in Repayment AMount variable
                    double RepaymentAmount = double.Parse(CalculateRepaymentFeeOnLapsed(vehicle.Permit_Duration).ToString("F"));

                    //Call a method to Update the Payment Amount Column in the DB with the value calculated above
                    _dataLayer.UpdatePaymentAmountInDatabase((int.Parse(vehicle.Id)), RepaymentAmount);

                    //Print out the details of the Vehicle record and the Payment amount
                    Console.WriteLine(vehicle.ToString() + " Months   - Calculated Repayment Amount including 10% Premium Due: \u20AC" + RepaymentAmount);

                }
            }

            //Close the DB Connection
            _dataLayer.Dispose();
            Console.WriteLine();
        }

        //Method to calculate the Repayment Fee of the Lapsed parking permits
        public static double CalculateRepaymentFeeOnLapsed(int Duration)
        {

            //Variables for the Amount Including and Excluding Premium
            double AmountWithoutPremium;
            double AmountIncludingPremium;

            //Calulate how much the customer paid for their most recent Parking Permit, ratePerMonth by Duration
            AmountWithoutPremium = (Constants.ratePerMonth * Duration);

            //Calulate how much the customer would need to pay for the same term again including a 10% premium for lapsed parking permit
            AmountIncludingPremium = AmountWithoutPremium += (AmountWithoutPremium * 10 / 100);
            return AmountIncludingPremium;
        }

        //Method to reallocate the Permit to a car owned by the same user
        public void ReallocatePermitToSameUsersCar()
        {
            //Allow a permit to be reallocated to a different car owned by the same user

            try
            {
                //Call the Connect to the DB Connection method
                _dataLayer.ConnectToDatabase(Constants.connectionString);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Return the Vehicles in DB to the _vehicles array
            _vehicles = _dataLayer.ReturnAllVehicles();

            Console.WriteLine();
            Console.WriteLine("List of Students with Valid Permits that can be transferred:");
            Console.WriteLine();

            //Iterate over the vehicles array and List out the Vehicles which have a valid Parking Permit
            foreach (Vehicle vehicle in _vehicles)
            {
                if (!HasParkingPermitExpired(vehicle))
                {

                    Console.WriteLine(vehicle.ToString());

                }
            }

            //Create an index variable and an Empty Arraylist to hold the ids of the Lapsed Vehicles   
            int index = 0;
            ArrayList idList = new ArrayList();

            //Read in the Ids to the Array list with the foreach loop and increment the index
            foreach (Vehicle vehicle in _vehicles)
            {
                idList.Add(int.Parse(vehicle.Id));
                index++;
            }

            //Read in from the Console the ID of the record which has the valid parking permit for transfer and store this in a variable
            Console.WriteLine();
            Console.WriteLine("Please Enter the ID of the Users Parking Permit that you would like to transfer:");
            int userIDForPermitReallocation = Convert.ToInt32(Console.ReadLine());

            //While the the List of Ids does not contain the value the user entered give error and make user re-renter the Id of the record for transfer
            while (!idList.Contains(userIDForPermitReallocation))
            {
                Console.WriteLine("Not a valid entry, Please Enter a valid ID from the above list:");
                userIDForPermitReallocation = Convert.ToInt32(Console.ReadLine());

            }

            //Return the vehicle which has the Parking Permit for Reallocation
            Vehicle vehicleForPermitTransfer = _dataLayer.GetVehicleById(userIDForPermitReallocation);

            //Read in from the Console the Model of the Vehicle the Parking Permit will be transferred to
            Console.WriteLine("Please Enter the Model of the New Vehicle the Parking Permit will be transferred to:");
            string NewVehicleModel = Console.ReadLine();

            //Read in from the console the Registration of the new vehicle
            Console.WriteLine("Please Enter the Registration of the New Vehicle the Parking Permit will be transferred to:");
            string NewVehicleReg = Console.ReadLine();
            Console.WriteLine();

            //Set the Model and Registation of the Vehicle whos parking permit is being reallocated to the New vehicle values read in from the console
            vehicleForPermitTransfer.Model = NewVehicleModel;
            vehicleForPermitTransfer.Reg = NewVehicleReg;

            //Delete the Old Vehicle Record from the DB
            _dataLayer.DeleteAVehicle(Convert.ToInt32(vehicleForPermitTransfer.Id));
            //Add the record with the New Vehicle details to the DB with the Orginal ID of the record to update
            _dataLayer.AddVehicle(vehicleForPermitTransfer);

            //Return the records in the DB to a List of Vehicles
            List<Vehicle> NewVehicles = _dataLayer.ReturnAllVehicles();
            //Sort the List based on ID descending using Linq
            List<Vehicle> SortedList = NewVehicles.OrderBy(newVec => newVec.Id).ToList();

            Console.WriteLine();
            Console.WriteLine("List of All Parking Permits in DB, Valid and Lapsed:");
            Console.WriteLine();
            //Iterate over the Vehicles in the Sorted List and Print out the details to the console to show the records currently in the DB
            foreach (Vehicle vehicle in SortedList)
            {

                Console.WriteLine(vehicle.ToString());

            }

            //Close the connection to the DB
            _dataLayer.Dispose();
            Console.WriteLine();

        }

    }

}
