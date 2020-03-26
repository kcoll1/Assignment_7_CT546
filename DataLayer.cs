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
        //Bool variable to hold IsConnected Property of the Connect to DB 
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
            //Clear the _vehicles array, so not to get duplicates in the List when called
            _vehicles.Clear();

            //Create the OleDbCommnd object and pass the query and connection
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
            //Close the DataReader Object and return the list of vehicles
            readData.Close();
            return _vehicles;

            
        }

        //Method to return a vehicle when provided with the ID of the vehicle record
        public Vehicle GetVehicleById(int vehicleId)
        {

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

        //Method to connect to the DB, Open the connection and specify that the Connection IsConnected when given a Connection String
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

        //Method to Clear the DB of all records
        public void ClearDatabase()
        {
            //Create the Ole Db command
            OleDbCommand command = new OleDbCommand(Constants.queryClearDB, _dbConnection);
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

        //Method to add a vehicle to the DB when passed a vehicle object
        public void AddVehicle(Vehicle vehicle)
        {

            OleDbCommand command = new OleDbCommand(Constants.InsertQueryString(vehicle), _dbConnection);
                
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
        //Delete a vehicle in the DB when passed a vehicle ID
        public void DeleteAVehicle(int vehicleID)
        {

            OleDbCommand command = new OleDbCommand(Constants.DeleteSpecificVehicleInDatabaseString(vehicleID), _dbConnection);
            

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

        //Update the Fees_Due column in the DB when passed an ID and Fees Due
        public void UpdateFeesInDatabase(int Id, double feesDue)
        {

            OleDbCommand command = new OleDbCommand(Constants.UpdateFeesInDatabaseString(Id, feesDue), DataLayer._dbConnection);

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

        public void UpdatePaymentAmountInDatabase(int Id, double PaymentAmount)
        {

            OleDbCommand command = new OleDbCommand(Constants.UpdatePaymentAmountInDatabaseString(Id, PaymentAmount), DataLayer._dbConnection);
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
}
