using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment7
{
    //Constants class for the Connection String and Query Strings
    public class Constants
    {
        //Constant values for the Connection String, Select and Delete All Records
        public const string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\karlc\H.Dip_Software_Design_Development\.NET Programming\Assignment 7\Assignment7.mdb";
        public const string queryReturnAllVehicles = "SELECT* from Vehicles";
        public const string queryClearDB = "DELETE * from Vehicles";
        public const double ratePerMonth = 10.00;
        public const double minorFee = 20.00;
        public const double midFee = 60.00;
        public const double maxFee = 100.00;
        public static TimeSpan minorFeeTimeSpan = new TimeSpan(15, 0, 0, 0);
        public static TimeSpan midFeeTimeSpan = new TimeSpan(30, 0, 0, 0);
        public static TimeSpan maxFeeTimeSpan = new TimeSpan(50, 0, 0, 0);
        public static string GetSpecificVehicle(int vehicleId) {
            return "SELECT * from Vehicles WHERE ID = " + vehicleId;
        }

        //Method to return the Query String for inserting a vehicle to the DB when given the vehicle object
        public static string InsertQueryString(Vehicle vehicle)
        {
            return "INSERT INTO Vehicles (ID, Vehicle_Model, Registration, Owner, Apartment, Permit_Start, Permit_Duration_Months, Fees_Due, Repayment_Amount) " +
                "values('" + vehicle.Id + "','" + vehicle.Model + "','" + vehicle.Reg + "','" + vehicle.Owner + "','" + vehicle.Apartment.ToString() + "','" + 
                vehicle.Permit_Start.ToString() + "','" + vehicle.Permit_Duration.ToString() + "','" + vehicle.Fees_Due.ToString() + "','" + vehicle.Repayment_Amount.ToString() + "')";
        }

        //Method to return the Query String for updating the Fees_Due column in the DB when given a vehicle ID and the Fees_Due
        public static string UpdateFeesInDatabaseString(int vehicleId, double feesDue)
        {
            return "UPDATE Vehicles SET Fees_Due =" + feesDue + " WHERE ID =" + vehicleId;
          
        }

        //Method to return the Query String for updating the Repayment Amount in the DB when given the vehicle ID and the Repayment Amount
        public static string UpdatePaymentAmountInDatabaseString(int vehicleId, double RepaymentAmount)
        {
            return "UPDATE Vehicles SET Repayment_Amount =" + RepaymentAmount + " WHERE ID =" + vehicleId;
           
        }

        //Method to return the Query String for Deleting a specific vehicle from the DB when given a vehicle ID
        public static string DeleteSpecificVehicleInDatabaseString(int vehicleId)
        {
            return "DELETE From Vehicles WHERE ID =" + vehicleId;

        }


    }
}
