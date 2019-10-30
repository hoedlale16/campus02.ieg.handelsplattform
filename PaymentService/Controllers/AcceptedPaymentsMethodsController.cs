using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PaymentService.Controllers
{

    [Route("api/PaymentMethodsV2")]
    [FormatFilter]
    public class AcceptdPaymentsMethodsController : Controller
    {
        private readonly ILogger<AcceptdPaymentsMethodsController> _logger;

        private static string[] accepedMethods = { "American", "Diners", "Master", "Visa", "Blue Monday" };

        public AcceptdPaymentsMethodsController(ILogger<AcceptdPaymentsMethodsController> logger)
        {
            _logger = logger;
        }

        /*
         * Liefert Ergebnis basierend auf geforderten Accept-Header: application/json, application/xml, text/cvs)
         * 
         * Siehe Startup.cs des Projektes
         */
        [HttpGet]
        //[Produces("application/json")]
        //[Produces("application/xml")]
        public IActionResult Get()
        {
            if (Request.Headers["Accept"] == "text/csv")
            {
                return Content(String.Join(",", accepedMethods));
            }

            return Ok(accepedMethods);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if(id < 0 || id >= accepedMethods.Length)
            {
                return NotFound();
            }

            return Ok(accepedMethods[id]);            
        }
    }
}
