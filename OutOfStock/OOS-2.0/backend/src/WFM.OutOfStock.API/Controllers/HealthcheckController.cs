using Microsoft.AspNetCore.Mvc;

namespace WFM.OutOfStock.API.Controllers
{
    [Route("healthcheck")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}