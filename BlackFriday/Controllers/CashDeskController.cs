using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlackFriday.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using BlackFriday.ServiceClients;

namespace BlackFriday.Controllers
{
    [Produces("application/json")]
    [Route("api/CashDesk")]
    public class CashDeskController : Controller
    {

        private readonly ILogger<CashDeskController> _logger;
        //private static readonly string creditcardServiceBaseAddress="http://iegeasycreditcardservice.azurewebsites.net/";

        public CashDeskController(ILogger<CashDeskController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(string id)
        {
            return Content("OK");
        }

        [HttpPost]
        public IActionResult Post([FromBody]Basket basket, [FromServices]SimpleCreditCartServiceClient creditcartServiceClient)
        {
           _logger.LogError("TransactionInfo Creditcard: {0} Product:{1} Amount: {2}", new object[] { basket.CustomerCreditCardnumber, basket.Product, basket.AmountInEuro});

            //Mapping
            CreditcardTransaction creditCardTransaction = new CreditcardTransaction()
            {
                Amount = basket.AmountInEuro,
                CreditcardNumber = basket.CustomerCreditCardnumber,
                ReceiverName = basket.Vendor
            };

            //Use the new awesome HttpClient with Resiliance and Failure Handling.
            bool result = creditcartServiceClient.PostCreditcartTransaction(creditCardTransaction).Result;

            if (result == true) {
                return CreatedAtAction("Get", new { id = Guid.NewGuid() }, creditCardTransaction);
            } 
            
            //Fallback
            return BadRequest();

            /*
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(creditcardServiceBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response =  client.PostAsJsonAsync(creditcardServiceBaseAddress + "/api/CreditcardTransactions", creditCardTransaction).Result;
            response.EnsureSuccessStatusCode();

            return CreatedAtAction("Get", new { id = System.Guid.NewGuid() }, creditCardTransaction);
            */
        }
    }
}