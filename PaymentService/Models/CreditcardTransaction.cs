using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Models
{
    public class CreditcardTransaction
    {
        /**
        * Hödl A. : Es muss noch definiert wreden, welche Daten an das Payment-Service übergeben werden
        * Fürs erste werden die selben Informationen bei der CreditCartTransaction verwendet, damit hier 
        * einfach die "CreditCartTransaction funktioniert.
        * 
        * Erweiterung zum Standard: Datenvalidierung...
        **/

        [MinLength(5, ErrorMessage = "not a valid credit number")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="Require a credit cart number")]
        public string CreditcardNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Require a credit cart type")]
        public string CreditcardType { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Amount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Require a receiver name")]
        public string ReceiverName { get; set; }
    }
}
