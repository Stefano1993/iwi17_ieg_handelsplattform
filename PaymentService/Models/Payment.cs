using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Controllers.Models
{
    public class Payment
    {
        public String Zahlungsart { get; set; }
        public int Preis { get; set; }
        public int Kontonummer { get; set; }
    }
}
