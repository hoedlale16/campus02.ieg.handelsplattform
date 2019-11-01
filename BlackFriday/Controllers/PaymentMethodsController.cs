using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using BlackFriday.ServiceClients;

namespace BlackFriday.Controllers
{
    [Produces("application/json")]
    [Route("api/PaymentMethods")]
    public class PaymentMethodsController : Controller
    {
        //https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
        private readonly ILogger<PaymentMethodsController> _logger;

        //private static readonly string creditcardServiceBaseAddress = "http://iegeasycreditcardservice.azurewebsites.net/";

        public PaymentMethodsController(ILogger<PaymentMethodsController> logger, [FromServices]SimpleCreditCartServiceClient creditcartServiceClient)
        {
            _logger = logger;
        }
        [HttpGet]
        public IEnumerable<string> Get([FromServices]SimpleCreditCartServiceClient creditcartServiceClient)
        {
            List<string> acceptedPaymentMethods = null;//= new string[] { "Diners", "Master" };
            //_logger.LogError("Accepted Paymentmethods");

            //Use the new awesome HttpClient with Resiliance and Failure Handling.
            acceptedPaymentMethods = creditcartServiceClient.GetPaymentMethods().Result;

          
            foreach (var item in acceptedPaymentMethods)
            {
                //_logger.LogError("Paymentmethod {0}", new object[] { item });

            }
            return acceptedPaymentMethods;
        }
    }
}