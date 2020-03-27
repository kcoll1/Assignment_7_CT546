using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment7
{
    //Interface containing all methods implemented in the DataLayer 
    public interface IDataLayer
    {
        List<Vehicle> ReturnAllVehicles();
        Vehicle GetVehicleById(int vehicleId);
        void AddVehicle(Vehicle vehicle);
        void DeleteAVehicle(int vehicleID);
        void UpdateFeesInDatabase(int Id, double feesDue);
        void UpdatePaymentAmountInDatabase(int Id, double PaymentAmount);
        void ClearDatabase();
        void ConnectToDatabase(string ConnectionString);
        void Dispose();
    }
}
