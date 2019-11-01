using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace DBProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDBController : ControllerBase
    {

        private readonly static string _hostname = "localhost";
        private readonly static string _user = "root";
        private readonly static string _pwd = "pa55w0rd";
        private readonly static string _database = "ieg";

        private ILogger<ProductDBController> _logger;
        public ProductDBController(ILogger<ProductDBController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client, NoStore = false)]
        public ActionResult<IEnumerable<string>> Get()
        {
            ArrayList products = new ArrayList();

            MySqlConnection connect = new MySqlConnection("SERVER=" + _hostname + "; user id=" + _user +
                                                          "; password=" + _pwd + "; database=" + _database);
            MySqlCommand cmd = new MySqlCommand("SELECT product_name FROM products;");
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connect;
            connect.Open();
            try
            {
                MySqlDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string product = dr.GetString("product_name");
                    products.Add(product);
                    _logger.LogError("Read product [" + product+ "]");
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while parsing FTP file", ex);
                return BadRequest();
            }
            finally
            {
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
            _logger.LogError("Parsed [" + products.Count + "] products");
            return Ok(products);
        }

           
    }
}