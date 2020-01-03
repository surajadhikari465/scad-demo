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
    public class InvoiceDataController : ControllerBase
    {
        private IIrmaMobileService service;
        private readonly ILogger<InvoiceDataController> logger;

        public InvoiceDataController(IIrmaMobileService service, ILogger<InvoiceDataController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<List<InvoiceCharge>> OrderInvoiceCharges([FromRoute]string region, [FromQuery]int orderId)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(InvoiceDataController)}.{nameof(OrderInvoiceCharges)}");
            
            var result = await service.GetOrderInvoiceChargesAsync(region, orderId);

            return result;
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<List<InvoiceCharge>> AllocatedInvoiceCharges([FromRoute]string region)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(InvoiceDataController)}.{nameof(AllocatedInvoiceCharges)}");
            
            var result = await service.GetAllocatedInvoiceChargesAsync(region);

            return result;
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<List<ListsSubteam>> NonallocatedInvoiceCharges([FromRoute]string region, [FromQuery]int orderId)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(InvoiceDataController)}.{nameof(NonallocatedInvoiceCharges)}");
            
            var result = await service.GetNonallocatedInvoiceChargesAsync(region, orderId);

            return result;
        }

        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<Result> AddInvoiceCharge([FromRoute]string region, [FromBody]InvoiceChargeModel model)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(InvoiceDataController)}.{nameof(AddInvoiceCharge)}");

            var result = await service.AddInvoiceChargeAsync(region, model); 

            return result;
        }

        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<Result> RemoveInvoiceCharge([FromRoute]string region, [FromBody]RemoveInvoiceChargeModel model)
        { 
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(InvoiceDataController)}.{nameof(RemoveInvoiceCharge)}");

            var result = await service.RemoveInvoiceChargeAsync(region, model.ChargeId); 

            return result;
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<List<Currency>> Currencies([FromRoute]string region)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(InvoiceDataController)}.{nameof(Currencies)}");
            
            var result = await service.GetCurrenciesAsync(region);

            return result;
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<List<ReasonCode>> RefuseCodes(string region)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(InvoiceDataController)}.{nameof(RefuseCodes)}");

            var result = await service.GetReasonCodesAsync(region, "RR");

            return result;
        }

        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<Result> RefuseOrder([FromRoute]string region, [FromBody]RefuseOrderModel model)
        { 
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(InvoiceDataController)}.{nameof(RefuseOrder)}");

            var result = await service.RefuseOrderAsync(region, model.OrderId, model.UserId, model.ReasonCodeId); 

            return result;
        }
    }
}
