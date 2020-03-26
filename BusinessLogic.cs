﻿using System;
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

        //Create Constructor of BusinessLogic Class that takes a DataLayer object as a Parameter
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
        public static bool CalculateIfParkingPermitExpired(DateTime startDate, int Duration)
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
                foreach (Vehicle v in _vehicles)
                {
                    //Call Method to determine if the Parking is expired for each record
                    if (CalculateIfParkingPermitExpired(v.Permit_Start, v.Permit_Duration) == false)
                    {
                        //Call Method to calculate the Fees Due on the Lapsed permits
                        double LapsedFee = CalculateFeeOfLapsed(v.Permit_Start, v.Permit_Duration);

                        //Call DataLayer class method to Update the Fees_Due column in the Database based on the given ID and print out this detail
                        _dataLayer.UpdateFeesInDatabase((int.Parse(v.Id)), LapsedFee);
                        Console.WriteLine(v.Id.ToString() + "   " + v.Owner.ToString() + "   " + v.Model.ToString() + "   " + v.Reg.ToString() + "   Apartment No." + v.Apartment.ToString() + "   Permit Start Date: " + v.Permit_Start.ToString("dd/MM/yyyy") + "   Permit Duration:" + v.Permit_Duration.ToString() + " Months   - Fees Due: \u20AC" + LapsedFee);
                        
                    }
                
            }
            _dataLayer.Dispose();
            Console.WriteLine();
        }

        //Method to calculate the Fee from Lapsed Permit based on the Length of Days that the Permit has been expired
        public static double CalculateFeeOfLapsed(DateTime startDate, int Duration)
        {

            //Calculate the Expiry Date of the Parking Permit
            DateTime ExpiryDate = startDate.AddMonths(Duration);

            //Subtract the Expiry Date from Todays Date and place result in a TimeSpan Object
            TimeSpan timeSpan = DateTime.Now.Subtract(ExpiryDate);

            //If statements to determine if the users permit warrants a minor, mid-size or Maximum fee, returns the Fees_Due (3 bands of Fee: 20.00, 60.00, 100.00)
            if (timeSpan < Constants.maxFeeTimeSpan)
            {
                if (timeSpan < Constants.midFeeTimeSpan)
                {
                    if (timeSpan < Constants.minorFeeTimeSpan)
                    {
                        return Constants.minorFee;
                    }
                    else
                    {
                        return Constants.minorFee;
                    }
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
            foreach (Vehicle v in _vehicles)
                {
                    //If the Parking Permit is expired
                    if (CalculateIfParkingPermitExpired(v.Permit_Start, v.Permit_Duration) == false)
                    {
                        //Call the Method to calculate the Repayment Amount on Lapsed Parking Permits and store the Result in Repayment AMount variable
                        double RepaymentAmount = double.Parse(CalculateRepaymentFeeOnLapsed(v.Permit_Duration).ToString("F"));

                        //Call a method to Update the Payment Amount Column in the DB with the value calculated above
                        _dataLayer.UpdatePaymentAmountInDatabase((int.Parse(v.Id)), RepaymentAmount);

                        //Print out the details of the Vehicle record and the Payment amount
                        Console.WriteLine(v.Id.ToString() + "   " + v.Owner.ToString() + "   " + v.Model.ToString() + "   " + v.Reg.ToString() + "   Apartment No." + v.Apartment.ToString() + "   Permit Start Date: " + v.Permit_Start.ToString("dd/MM/yyyy") + "   Permit Duration:" + v.Permit_Duration.ToString() + " Months   - Calculated Repayment Amount including 10% Premium Due: \u20AC" + RepaymentAmount);

                    }
                }

            //Close the DB Connection
            _dataLayer.Dispose();
            Console.WriteLine();
        }

        
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
                foreach (Vehicle v in _vehicles)
                {
                    if (CalculateIfParkingPermitExpired(v.Permit_Start, v.Permit_Duration) == true)
                    {

                        Console.WriteLine(v.Id.ToString() + "   " + v.Owner.ToString() + "   " + v.Model.ToString() + "   " + v.Reg.ToString() + "   Apartment No." + v.Apartment.ToString() + "   Permit Start Date: " + v.Permit_Start.ToString("dd/MM/yyyy") + "   Permit Duration:" + v.Permit_Duration.ToString() + " Months");

                    }
                }
            

                //Read in from the Console the ID of the record which has the valid parking permit for transfer and store this in a variable
                Console.WriteLine();
                Console.WriteLine("Please Enter the ID of the Users Parking Permit that you would like to transfer:");
                int userIDForPermitReallocation = Convert.ToInt32(Console.ReadLine());

                //Return the vehicle which has the Parking Permit for Reallocation
                Vehicle vehicleForPermitTransfer = _dataLayer.GetVehicleById(userIDForPermitReallocation);

                //Read in from the Console the Model of the Vehicle the Parking Permit will be transferred to
                Console.WriteLine("Please Enter the Model of the New Vehicle the Parking Permit will be transferred to:");
                string NewVehicleModel= Console.ReadLine();

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
                //Sort the List based on ID descending
                List<Vehicle> SortedList = NewVehicles.OrderBy(newVec => newVec.Id).ToList();

                Console.WriteLine();
                Console.WriteLine("List of All Parking Permits in DB, Valid and Lapsed:");
                Console.WriteLine();
                //Iterate over the Vehicles in the Sorted List and Print out the details to the console to show the records currently in the DB
                foreach (Vehicle v in SortedList)
                {
                   
                    Console.WriteLine(v.Id.ToString() + "   " + v.Owner.ToString() + "   " + v.Model.ToString() + "   " + v.Reg.ToString() + "   Apartment No." + v.Apartment.ToString() + "   Permit Start Date: " + v.Permit_Start.ToString("dd/MM/yyyy") + "   Permit Duration:" + v.Permit_Duration.ToString() + " Months");

                }

            //Close the connection to the DB
            _dataLayer.Dispose();
            Console.WriteLine();

        }

    }

}
