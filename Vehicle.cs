using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment7
{
    public class Vehicle
    {
        public Vehicle() { 
        
        }
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
        public string Id { get; set; }
        public string Model { get; set; }
        public string Reg { get; set; }
        public string Owner { get; set; }
        public string Apartment { get; set; }
        public DateTime Permit_Start { get; set; }
        public int Permit_Duration { get; set; }
        public double Fees_Due { get; set; }
        public double Repayment_Amount { get; set; }
    }
}
