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
        public const string queryInsertVehicle = "INSERT INTO Vehicles";
        public const string queryUpdateVehicle = "UPDATE INTO Vehicles";
    }
}
