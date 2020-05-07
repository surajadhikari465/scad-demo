using IrmaMobile.Legacy;
using IrmaMobile.Logging;
using IrmaMobile.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IrmaMobile.Controllers
{
    [Route("api/{region}/[controller]/[Action]")]
    [ApiController]
    public class ReceivingListController : ControllerBase
    {
        private IIrmaMobileService service;
        private readonly ILogger<ReceivingListController> logger;

        public ReceivingListController(IIrmaMobileService service, ILogger<ReceivingListController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<List<OrderItem>> ReceivingListEinvoiceExceptions([FromRoute]string region, [FromQuery]int orderId)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(ReceivingListController)}.{nameof(ReceivingListEinvoiceExceptions)}");
            
            var result = await service.GetReceivingListEinvoiceExceptionsAsync(region, orderId);

            return result;
        }
    }
}
