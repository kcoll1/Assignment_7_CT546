using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;

namespace Assignment7
{
    public class BusinessLogic
    {

        //Instance of the DataLayer class to create and initiate the connection to the DB
        DataLayer db = new DataLayer(Constants.connectionString);

        //Create a list of vehicles to store values from the DB table
        List<Vehicle> vehicles = new List<Vehicle>();

        //Method to Show and Calculate the Valid and Lapsed Parking Permits
        public void OverallValidOrLapsedParkingPermits() {

            //Variables for Valid and Lapsed Count
            int Valid = 0;
            int Lapsed = 0;

            //Using the Connection created earlier
            using (db) {

                 try
                  {
                    OleDbDataReader readData = db.CreateCommand(Constants.queryReturnAllVehicles);

                    //While there is another record to read in the DB
                    while (readData.Read())
                    {
                        //Add a new vehicle to the vehicles array from the OleDbDataReader object
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
                    Console.WriteLine();
                    //For Each vehicle read into the Vehicles array, 
                    // call the Method to check if their Parking Permit is Lapsed or Valid, Increment each counter according to the result and Display
                    foreach (Vehicle v in vehicles)
                    {
                        if (CalculateIfParkingPermitExpired(v.Permit_Start, v.Permit_Duration) == false)
                        { 
                            Lapsed++;
                            Console.WriteLine(v.Owner.ToString() + "   " + v.Model.ToString() + "   " + v.Reg.ToString() + "   Apartment No." + v.Apartment.ToString() + "   - LAPSED PARKING PERMIT");
                        }
                        else
                        {
                            Valid++;
                            Console.WriteLine(v.Owner.ToString() + "   " + v.Model.ToString() + "   " + v.Reg.ToString() + "   Apartment No." + v.Apartment.ToString() + "   - VALID PARKING PERMIT");
                        }

                    }


                     //Close the DataReader object
                    readData.Close();
                    Console.WriteLine();

                    //Print to Console the overall number of Lapsed and Valid Parking Permits
                    Console.WriteLine("Overall Number of Lapsed Parking Permits: " + Lapsed);
                    Console.WriteLine("Overall Number of Valid Parking Permits: " + Valid);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }

            
        }

        //Method to Calculate the Expiry Date of the Parking Permit and Return false if Parking is Lapsed
        public bool CalculateIfParkingPermitExpired(DateTime startDate, int Duration)
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
