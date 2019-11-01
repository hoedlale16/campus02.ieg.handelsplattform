using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoggingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private readonly ILogger<LoggingController> _logger;
        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }

        public IActionResult Post([FromBody]string loggingText)
        {
            _logger.LogError("Logging: {0}", new object[] { loggingText });
            return CreatedAtAction("Get", new { id = System.Guid.NewGuid() });
        }
    }
}