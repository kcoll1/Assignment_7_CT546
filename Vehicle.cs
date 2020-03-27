using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment7
{
    public class Vehicle
    {
        public Vehicle() { 
        
        }
        //Vehicle Constructor
        public Vehicle(int Id, string model, string reg, string owner, string apartment, DateTime permitStart, int permitDuration, double fees, double repaymentAmount) {
            
            this.Id = Id.ToString();
            this.Model = model;
            this.Reg = reg;
            this.Owner = owner;
            this.Apartment = apartment;
            this.Permit_Start = permitStart;
            this.Permit_Duration = permitDuration;
            this.Fees_Due = fees;
            this.Repayment_Amount = repaymentAmount;
        
        }

        //Variables with getters and setters for each
        public string Id { get; set; }
        public string Model { get; set; }
        public string Reg { get; set; }
        public string Owner { get; set; }
        public string Apartment { get; set; }
        public DateTime Permit_Start { get; set; }
        public int Permit_Duration { get; set; }
        public double Fees_Due { get; set; }
        public double Repayment_Amount { get; set; }

        //Overidden ToString method for the repeated string which prints out the Vehicle properties
        public override string ToString() {
            string vehicleAsString = ""+ Id.ToString() + "   " + Owner.ToString() + "   " + Model.ToString() + "   "
                   + Reg.ToString() + "   Apartment No." + Apartment.ToString() + "   Permit Start Date: "
                   + Permit_Start.ToString("dd/MM/yyyy") + "   Permit Duration:" + Permit_Duration.ToString();
            return vehicleAsString;

        }
    }
}
