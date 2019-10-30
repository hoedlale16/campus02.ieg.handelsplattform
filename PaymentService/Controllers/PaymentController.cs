using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentService.Models;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [FormatFilter]
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        /**
         * GET to receive an example of how to create a well formated object in JSON,XML or CVS.
         **/
        [HttpGet]
        public IActionResult Get()
        {
            CreditcardTransaction creditcardTransaction = new CreditcardTransaction();
            creditcardTransaction.Amount = 13.37;
            creditcardTransaction.CreditcardNumber = "1234 5678 9012";
            creditcardTransaction.CreditcardType = "Master";
            creditcardTransaction.ReceiverName = "The rich AG";

            if (Request.Headers["Accept"] == "text/csv")
            {
                return Content(String.Format("{0};{1};{2};{3}",
                    creditcardTransaction.CreditcardNumber,
                    creditcardTransaction.CreditcardType,
                    creditcardTransaction.Amount,
                    creditcardTransaction.ReceiverName));
            }

            return Ok(creditcardTransaction);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]CreditcardTransaction paymentTransaction)
        {
            try
            {
                _logger.LogError("Received a new payment for Credit-Cart: {0} Amount:{1} Receiver: {2}",
                    new object[] {
                    paymentTransaction.CreditcardNumber,
                    paymentTransaction.Amount,
                    paymentTransaction.ReceiverName
                    });

                if (ModelState.IsValid == false)
                {
                    return BadRequest(ModelState);
                }

                //Do some crazy payment shit, but not as crazy as Hybris does it!

                return Ok(new { id = System.Guid.NewGuid() });
            } catch (Exception e)
            {
                _logger.LogError("Error while processing payment", e);
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }
    }
}