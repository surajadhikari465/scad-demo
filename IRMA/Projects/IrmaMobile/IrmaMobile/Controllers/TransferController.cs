using System.Collections.Generic;
using System.Threading.Tasks;
using IrmaMobile.Domain.Models;
using IrmaMobile.Legacy;
using IrmaMobile.Logging;
using IrmaMobile.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IrmaMobile.Controllers
{
    [Route("api/{region}/[controller]/[Action]")]
    [ApiController]
    public class TransferController
    {
        private IIrmaMobileService service;
        private readonly ILogger<TransferController> logger;

        public TransferController(IIrmaMobileService service, ILogger<TransferController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<List<ListsSubteam>> SubteamByProductType([FromRoute]string region, [FromQuery]int productTypeId)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(TransferController)}.{nameof(SubteamByProductType)}");
            
            var result = await service.GetSubteamByProductTypeAsync(region, productTypeId);

            return result;
        }
    }
}