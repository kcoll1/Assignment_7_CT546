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
        public bool IsConnected { get; set; }
        private Vehicle returnedById;

        //In the Data Layer Construction pass the connection string as a parameter and initialise the Connection Object, Open Connection
        public DataLayer()
        {
            _dbConnection = new OleDbConnection();
        }

        //Method which takes in a Query for the DB as a Parameter, creates an OleDbCommand, an OleDbDataReader and reads the values from the Access DB and returns a List of Vehicle Objects
        public List<Vehicle> ReturnAllVehicles()
        {

          //  OleDbDataReader readData = _dataCreateCommand(Constants.queryReturnAllVehicles);

            OleDbCommand command = new OleDbCommand(Constants.queryReturnAllVehicles, DataLayer._dbConnection);

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

        public Vehicle GetVehicleById(int vehicleId)
        {

            //  OleDbDataReader readData = _dataCreateCommand(Constants.queryReturnAllVehicles);

            OleDbCommand command = new OleDbCommand(Constants.GetSpecificVehicle(vehicleId), DataLayer._dbConnection);

            OleDbDataReader readData = command.ExecuteReader();

            //While there is another record to read in the DB
            while (readData.Read())
            {
                //Add a new vehicle to the vehicles array from the OleDbDataReader object
                returnedById = new Vehicle()
                {
                    Id = readData[0].ToString(),
                    Model = readData[1].ToString(),
                    Reg = readData[2].ToString(),
                    Owner = readData[3].ToString(),
                    Apartment = readData[4].ToString(),
                    Permit_Start = DateTime.Parse(readData[5].ToString()),
                    Permit_Duration = int.Parse(readData[6].ToString()),

                };


            }
            readData.Close();
            return returnedById;
        }

        public void ConnectToDatabase(string ConnectionString) {
            _dbConnection.ConnectionString = ConnectionString;
            _dbConnection.Open();
            IsConnected = true;
        }
        //Method to dispose of the Connection, as the Data Layer Class implements IDisposable
        public void Dispose() {
            _dbConnection.Dispose();
            IsConnected = false;
        }


        public void ClearDatabase()
        {
            //Create the Ole Db command
            OleDbCommand command = new OleDbCommand(Constants.queryClearDB, _dbConnection);
            command.ExecuteNonQuery();
                
            
        }

        public void AddVehicle(Vehicle vehicle)
        {
        
                using (OleDbCommand command = new OleDbCommand(Constants.InsertQueryString(vehicle), _dbConnection))
                {

                    try
                    {
                        //Execute the SQL statement
                        command.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
        }

        //public void InsertFeesToDatabase(string DbQuery)
        //{

        //    //  OleDbDataReader readData = _dataCreateCommand(Constants.queryReturnAllVehicles);

        //    OleDbCommand command = new OleDbCommand(DbQuery, DataLayer._dbConnection);

        //    command.ExecuteNonQuery();


        //}


    }
}
