using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment7
{
    //Constants class for the Connection String and Query Strings
    public class Constants
    {
        public const string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\karlc\H.Dip_Software_Design_Development\.NET Programming\Assignment 7\Assignment7.mdb";
        public const string queryReturnAllVehicles = "SELECT* from Vehicles";
        public const string queryClearDB = "DELETE * from Vehicles";
        public const string queryUpdateVehicle = "UPDATE Vehicles";

        public static string GetSpecificVehicle(int vehicleId) {
            return "SELECT * from Vehicles WHERE ID = " + vehicleId;
        }

        public static string InsertQueryString(Vehicle vehicle)
        {
            return "INSERT INTO Vehicles (ID, Vehicle_Model, Registration, Owner, Apartment, Permit_Start, Permit_Duration_Months, Fees_Due, Repayment_Amount) values('" + vehicle.Id + "','" + vehicle.Model + "','" + vehicle.Reg + "','" + vehicle.Owner + "','" + vehicle.Apartment.ToString() + "','" + vehicle.Permit_Start.ToString() + "','" + vehicle.Permit_Duration.ToString() + "','" + vehicle.Fees_Due.ToString() + "','" + vehicle.Repayment_Amount.ToString() + "')";
        }
       

    }
}
