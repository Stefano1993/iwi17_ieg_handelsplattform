using System;
using BlackFriday.Interfaces;

namespace BlackFriday.Models
{
    public class Sell : ISell
    {
        public string Product { get; set; }
        public string Vendor { get; set; }
        public double Amount { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
