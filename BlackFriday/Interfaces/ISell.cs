using System;

namespace BlackFriday.Interfaces
{
    public interface ISell
    {
        string Product { get; set; }
        string Vendor { get; set; }
        double Amount { get; set; }
        DateTimeOffset Date { get; set; }
    }
}
