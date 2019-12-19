using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using WFM.OutOfStock.API.Domain.Request;
using WFM.OutOfStock.API.Services;
using WFM.OutOfStock.API.Services.Exceptions;

namespace WFM.OutOfStock.API.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IOutOfStockService _outOfStockService;

        public ItemsController(IOutOfStockService outOfStockService)
        {
            _outOfStockService = outOfStockService;
        }

        /// <summary>
        /// Submits a list of UPCs.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/products
        ///     {
        ///         "name": "Sample List",
        ///         "regionCode": "SW",
        ///         "storeName": "Arbor Trails",
        ///         "items": [
        ///             "123456789123",
        ///             "234567891234"
        ///         ]
        ///     }
        /// </remarks>
        /// <param name="list">The list of items to submit.</param>
        /// <response code="200">If the list has been submitted successfully.</response>
        /// <response code="400">If the list fails validation.</response>
        /// <response code="502">If a downstream communication failure occurs.</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 502)]
        public async Task<ActionResult> SubmitList(
            [FromBody] UploadItemsRequest list)
        {
            try
            {
                await _outOfStockService.SubmitList(list);
                return Ok();
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