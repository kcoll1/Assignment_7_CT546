using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;

namespace Assignment7
{
    public class DataLayer : IDisposable
    {
        //Create the OleDbConnection
        private static OleDbConnection _dbConnection;

        public DataLayer(String connectionString)
        {
            _dbConnection = new OleDbConnection();
            _dbConnection.ConnectionString = connectionString;
            _dbConnection.Open();

        }

        public OleDbDataReader CreateCommand(string DbQuery) {

            OleDbCommand command = new OleDbCommand(DbQuery, DataLayer._dbConnection);

            OleDbDataReader readData = command.ExecuteReader();

            return readData;

        }
        public void Dispose() {
            _dbConnection.Dispose();
        }
    }
}
