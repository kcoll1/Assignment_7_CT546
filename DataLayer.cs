using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;

namespace Assignment7
{
    //Data Layer to Manage the OleDbConnection used by the Program
    public class DataLayer : IDisposable
    {
        //Create the OleDbConnection
        private static OleDbConnection _dbConnection;
        readonly List<Vehicle> _vehicles = new List<Vehicle>();

        //In the Data Layer Construction pass the connection string as a parameter and initialise the Connection Object, Open Connection
        public DataLayer()
        {
            _dbConnection = new OleDbConnection();
            _dbConnection.ConnectionString = Constants.connectionString;
            _dbConnection.Open();

        }

        //Method which takes in a Query for the DB as a Parameter, creates an OleDbCommand, an OleDbDataReader and reads the values from the Access DB and returns a List of Vehicle Objects
        public List<Vehicle> ReturnVehicles(string DbQuery)
        {

          //  OleDbDataReader readData = _dataCreateCommand(Constants.queryReturnAllVehicles);

            OleDbCommand command = new OleDbCommand(DbQuery, DataLayer._dbConnection);

            OleDbDataReader readData = command.ExecuteReader();

            //While there is another record to read in the DB
            while (readData.Read())
            {
                //Add a new vehicle to the vehicles array from the OleDbDataReader object
                _vehicles.Add(new Vehicle()
                {
                    Id = readData[0].ToString(),
                    Model = readData[1].ToString(),
                    Reg = readData[2].ToString(),
                    Owner = readData[3].ToString(),
                    Apartment = readData[4].ToString(),
                    Permit_Start = DateTime.Parse(readData[5].ToString()),
                    Permit_Duration = int.Parse(readData[6].ToString()),

                });

                
            }
            readData.Close();
            return _vehicles;
        }

        //Method to dispose of the Connection, as the Data Layer Class implements IDisposable
        public void Dispose() {
            _dbConnection.Dispose();
        }
    }
}
