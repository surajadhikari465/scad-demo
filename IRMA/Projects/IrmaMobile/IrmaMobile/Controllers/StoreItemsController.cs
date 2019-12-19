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
    public class StoreItemsController : ControllerBase
    {
        private IIrmaMobileService service;
        private readonly ILogger<StoreItemsController> logger;

        public StoreItemsController(IIrmaMobileService service, ILogger<StoreItemsController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        // GET: api/Shr/FL
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200)]
        public async Task<StoreItemModel> Get(string region, int storeNo, int subteamNo, int? userId, string scanCode)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(StoreItemsController)}.{nameof(Get)}");

            //TODO: Make userID an int when we implement authentication
            return await service.GetStoreItemAsync(region, storeNo, subteamNo, userId, scanCode);
        }
    }
}