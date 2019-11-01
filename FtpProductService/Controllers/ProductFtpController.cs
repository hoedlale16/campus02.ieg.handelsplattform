using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FtpProductService.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ProductFtpController : ControllerBase
    {
        private readonly static string _hostname = "ftp://localhost:21";
        private readonly static string _user = "user";
        private readonly static string _pwd = "pa55w0rd";
        private readonly static string _fileName = "products.csv";

        private ILogger<ProductFtpController> _logger;
        public ProductFtpController(ILogger<ProductFtpController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client, NoStore = false)]
        public ActionResult<IEnumerable<string>> Get()
        {
            string[] products = { };

            WebClient request = new WebClient();
            string url = _hostname + "/" +_fileName;
            request.Credentials = new NetworkCredential(_user, _pwd);

            try
            {
                byte[] newFileData = request.DownloadData(url);
                string contentAsString = System.Text.Encoding.UTF8.GetString(newFileData);

                //Parse CVS content and and return content.
                products = contentAsString.Split(",", StringSplitOptions.RemoveEmptyEntries);
                _logger.LogError("Parsed [" + products.Length + "] products");
            }
            catch (WebException e)
            {
                _logger.LogError($"Error while parsing FTP file", e);
                return BadRequest();
            }

            return Ok(products);
        }
    }
}