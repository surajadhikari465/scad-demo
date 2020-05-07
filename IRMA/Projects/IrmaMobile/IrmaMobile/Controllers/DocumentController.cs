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
    public class DocumentController : ControllerBase
    {
        private IIrmaMobileService service;
        private readonly ILogger<DocumentController> logger;

        public DocumentController(IIrmaMobileService service, ILogger<DocumentController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<List<DSDVendor>> Vendors([FromRoute]string region, [FromQuery]int storeNo)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(DocumentController)}.{nameof(Vendors)}");
            
            var result = await service.GetVendorsAsync(region, storeNo);

            return result;
        }

        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<bool> IsDuplicateReceivingDocumentInvoiceNumber([FromRoute]string region, [FromQuery]string invoiceNumber, [FromQuery]int vendorId)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(DocumentController)}.{nameof(IsDuplicateReceivingDocumentInvoiceNumber)}");
            
            var result = await service.IsDuplicateReceivingDocumentInvoiceNumberAsync(region, invoiceNumber, vendorId);

            return result;
        }
    }
}
