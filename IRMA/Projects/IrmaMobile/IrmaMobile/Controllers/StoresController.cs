using IrmaMobile.Domain.Models;
using IrmaMobile.Logging;
using IrmaMobile.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace IrmaMobile.Controllers
{
    [Route("api/{region}/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private IIrmaMobileService service;
        private ILogger<StoresController> logger;

        public StoresController(IIrmaMobileService service, ILogger<StoresController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        // GET: api/SubTeam/FL
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200)]
        public async Task<List<StoreModel>> Get(string region, [FromQuery] bool useVendorIdAsStoreNo = false)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(StoresController)}.{nameof(Get)}");

            return await service.GetStoresAsync(region, useVendorIdAsStoreNo);
        }
    }
}