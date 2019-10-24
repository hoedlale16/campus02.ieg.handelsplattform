using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackFriday.Models
{
    public class CreditcardTransaction
    {
        public string CreditcardNumber { get; set; }
        public double Amount { get; set; }
        public string ReceiverName { get; set; }
    }
}
