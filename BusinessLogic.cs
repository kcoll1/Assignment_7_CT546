using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;

namespace Assignment7
{
    public class BusinessLogic
    {
        
        public DataLayer db = new DataLayer(Constants.connectionString);
        List<Vehicle> vehicles = new List<Vehicle>();

        public void CalculateNumberOfParkingPermits() {

            using (db) {

                 try
                {
                    OleDbDataReader readData = db.CreateCommand(Constants.queryReturnAllVehicles);
                
                    //While there is data to read in the DB
                    while (readData.Read())
                    {
                        vehicles.Add(new Vehicle()
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
                    foreach (Vehicle v in vehicles)
                    {
                        if (CalculateExpiryDate(v.Permit_Start, v.Permit_Duration) == false)
                        {
                            Console.WriteLine(v.Owner.ToString());
                        }
                    }

                    //Close the DataReader object
                    readData.Close();

                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }

            
        }
        public bool CalculateExpiryDate(DateTime startDate, int Duration)
        {
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
    }
}
