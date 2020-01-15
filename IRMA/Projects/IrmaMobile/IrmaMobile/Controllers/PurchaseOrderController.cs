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
    public class PurchaseOrderController : ControllerBase
    {
        private IIrmaMobileService service;
        private readonly ILogger<PurchaseOrderController> logger;

        public PurchaseOrderController(IIrmaMobileService service, ILogger<PurchaseOrderController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<Order> PurchaseOrder(string region, long purchaseOrderNum)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(PurchaseOrderController)}.{nameof(PurchaseOrder)}");

            var result = await service.GetPurchaseOrderAsync(region, purchaseOrderNum);

            return result;
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<List<Order>> PurchaseOrders(string region, string upc, int storeNumber)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(PurchaseOrderController)}.{nameof(PurchaseOrders)}");

            var result = await service.GetPurchaseOrdersAsync(region, upc, storeNumber);

            return result;
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<List<ReasonCode>> ReasonCodes(string region)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(PurchaseOrderController)}.{nameof(ReasonCodes)}");

            var result = await service.GetReasonCodesAsync(region, "RD");

            return result;
        }

        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<Result> ReceiveOrder([FromRoute]string region, [FromBody]ReceiveOrderModel model)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(PurchaseOrderController)}.{nameof(ReceiveOrder)}");

            var result = await service.ReceiveOrderAsync(region, model);

            return result;
        }

        
        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<Result> CloseOrder([FromRoute]string region, [FromBody]CloseOrderModel model)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(PurchaseOrderController)}.{nameof(CloseOrder)}");

            var result = await service.CloseOrderAsync(region, model.OrderId, model.UserId); 

            return result;
        }

        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<Result> ReopenOrder([FromRoute]string region, [FromBody]ReopenOrderModel model)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(PurchaseOrderController)}.{nameof(ReopenOrder)}");

            var result = await service.ReopenOrderAsync(region, model.OrderId); 

            return result;
        }
    }
}
