using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using IrmaMobile.Domain.Models;
using IrmaMobile.Legacy;
using IrmaMobile.Logging;
using IrmaMobile.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IrmaMobile.Controllers
{
    [Route("api/{region}/[controller]")]
    [ApiController]
    public class SubTeamsController : ControllerBase
    {
        private IIrmaMobileService service;
        private readonly ILogger<SubTeamsController> logger;

        public SubTeamsController(IIrmaMobileService service, ILogger<SubTeamsController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        // GET: api/SubTeam/FL
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200)]
        public async Task<List<SubteamModel>> Get(string region)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(SubTeamsController)}.{nameof(Get)}");

            return await service.GetSubteamsAsync(region);
        }
    }
}
