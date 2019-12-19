using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using WFM.OutOfStock.API.Domain.Response;
using WFM.OutOfStock.API.Services;
using WFM.OutOfStock.API.Services.Exceptions;
using WFM.OutOfStock.API.Validation;

namespace WFM.OutOfStock.API.Controllers
{
    [Route("api/stores")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IOutOfStockService _outOfStockService;

        public StoresController(IOutOfStockService outOfStockService)
        {
            _outOfStockService = outOfStockService;
        }

        /// <summary>
        /// Retrieves a list of stores within the specified region.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/stores/region/MW
        /// </remarks>
        /// <param name="regionCode">The two-letter region code.</param>
        /// <returns>A list of stores and their business units.</returns>
        /// <response code="200">An array of stores with their store number.</response>
        /// <response code="400">If the specified region code does not exist.</response>
        /// <response code="502">If a downstream communication failure occurs.</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 502)]
        [Route("region/{regionCode}")]
        public async Task<ActionResult<List<StoreResponse>>> GetStoresByRegionCode(
            [Required, RegionCode] string regionCode)
        {
            try
            {
                var lists = await _outOfStockService.RetrieveStoresForRegion(regionCode);
                return Ok(lists);
            }
            catch (ServiceCommunicationException scex)
            {
                return StatusCode(502, new ProblemDetails
                {
                    Title = "Service Communication Error",
                    Detail = scex.Message,
                    Status = 502
                });
            }
        }
    }
}
