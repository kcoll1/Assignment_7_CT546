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

        //In the Data Layer Construction pass the connection string as a parameter and initialise the Connection Object, Open Connection
        public DataLayer(String connectionString)
        {
            _dbConnection = new OleDbConnection();
            _dbConnection.ConnectionString = connectionString;
            _dbConnection.Open();

        }

        //Method which takes in a Query for the DB, creates an OleDbCommand and returns Data in an OleDbDataReader
        public OleDbDataReader CreateCommand(string DbQuery) {

            OleDbCommand command = new OleDbCommand(DbQuery, DataLayer._dbConnection);

            OleDbDataReader readData = command.ExecuteReader();

            return readData;

        }

        //Method to dispose of the Connection, as the Data Layer Class implements IDisposable
        public void Dispose() {
            _dbConnection.Dispose();
        }
    }
}
