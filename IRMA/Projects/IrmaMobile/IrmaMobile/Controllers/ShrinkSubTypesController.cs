using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using IrmaMobile.Domain.Models;
using IrmaMobile.Logging;
using IrmaMobile.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IrmaMobile.Controllers
{
    [Route("api/{region}/[controller]")]
    [ApiController]
    public class ShrinkSubTypesController : ControllerBase
    {
        private IIrmaMobileService service;
        private readonly ILogger<ShrinkSubTypesController> logger;

        public ShrinkSubTypesController(IIrmaMobileService service, ILogger<ShrinkSubTypesController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        // GET: api/ShrinkSubTypes/FL
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200)]
        public async Task<List<ShrinkSubTypeModel>> Get(string region)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(ShrinkSubTypesController)}.{nameof(Get)}");

            return await service.GetShrinkSubTypesAsync(region);
        }
    }
}