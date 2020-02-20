using System.Collections.Generic;
using IrmaMobile.Domain.Models;
using IrmaMobile.Logging;
using IrmaMobile.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using System.Threading.Tasks;
using IrmaMobile.Legacy;

namespace IrmaMobile.Controllers
{
    [Route("api/{region}/[controller]")]
    [ApiController]
    public class ShrinkAdjustmentsController : ControllerBase
    {
        private IIrmaMobileService service;
        private readonly ILogger<ShrinkSubTypesController> logger;

        public ShrinkAdjustmentsController(IIrmaMobileService service, ILogger<ShrinkSubTypesController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200)]
        public async Task<List<ListsShrinkAdjustmentReason>> Get([FromRoute] string region)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(ShrinkAdjustmentsController)}.{nameof(Get)}");
            return await service.GetShrinkAdjustmentReasonsAsync(region);
        }

        // POST: api/FL/ShrinkAdjustments
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200)]
        public async Task<bool> Post([FromRoute] string region, [FromBody] ShrinkAdjustmentModel shrinkAdjustmentModel)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(ShrinkAdjustmentsController)}.{nameof(Post)}");

            return await service.AddShrinkAdjustment(region, shrinkAdjustmentModel);
        }
    }
}