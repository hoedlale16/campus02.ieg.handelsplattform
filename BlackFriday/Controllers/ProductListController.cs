    using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackFriday.ServiceClients;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlackFriday.Controllers
{
    [Route("api/[controller]")]
    public class ProductListController : Controller
    {
        // GET: http://iegblackfriday.azurewebsites.net/api/productlist
        [HttpGet]
        public IActionResult Get([FromServices]SimpleFtpProductServiceClient simpleFtpProductServiceClient)
        {
            List<string> products = null;

            products = simpleFtpProductServiceClient.GetProductsFromDB().Result;
            //products = simpleFtpProductServiceClient.GetProductsFromFtp().Result;

            if (products == null)
            {
                return BadRequest();
            }

            return Ok(products);
            //return new string[] { "Windows Phone", "BlackBerry" };

        }
        
    }
}
