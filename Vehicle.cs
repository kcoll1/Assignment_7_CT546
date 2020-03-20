using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment7
{
    public class Vehicle
    {
        public string Id { get; set; }
        public string Model { get; set; }
        public string Reg { get; set; }
        public string Owner { get; set; }
        public string Apartment { get; set; }
        public DateTime Permit_Start { get; set; }
        public int Permit_Duration { get; set; }
    }
}
