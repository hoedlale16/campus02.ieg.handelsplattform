using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackFriday.Models
{
    public class Basket
    {
        public string Product { get; set; }
        public string Vendor { get; set; }
        public string CustomerCreditCardnumber { get; set; }
        public double AmountInEuro { get; set; }
    }
}
