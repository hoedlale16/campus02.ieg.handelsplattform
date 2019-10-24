using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IEGEasyCreditCardService.Controllers
{
    [Route("api/[controller]")]
    public class AcceptedCreditCardsController : Controller
    {
        
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "American", "Diners", "Master","Visa", "Blue Monday" };
        }

       
    }
}
