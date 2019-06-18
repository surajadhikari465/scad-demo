using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KitBuilderWebApi.Controllers
{
    [Route("api/Config")]

    public class ConfigController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<KitController> logger;

        public ConfigController(IConfiguration configuration, ILogger<KitController> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GetConfig()
        {
            logger.LogInformation("dumping config");
            var keys = new[]
            {
                "ASPNETCORE_ENVIRONMENT",
                "applicationName",
                "COMPUTERNAME",
                "connectionStrings:KitBuilderDBConnectionString",
                "WebApiBaseAddress:MammothBaseAddress",
                "ASPNETCORE_ENVIRONMENT"
            };
            var data = configuration.AsEnumerable().Where( w=> keys.Contains(w.Key)).ToList();
            

            return Ok(data);
        }
    }
}
