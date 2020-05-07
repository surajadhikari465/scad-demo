using IrmaMobile.Domain.Models;
using IrmaMobile.Logging;
using IrmaMobile.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IrmaMobile.Controllers
{
    [Route("api/{region}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IIrmaMobileService service;
        private readonly ILogger<UsersController> logger;

        public UsersController(IIrmaMobileService service, ILogger<UsersController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        public async Task<UserModel> Get([FromRoute]string region, [FromQuery] string userName)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(UsersController)}.{nameof(Get)}");

            return await service.GetUser(region, userName);
        }
    }
}