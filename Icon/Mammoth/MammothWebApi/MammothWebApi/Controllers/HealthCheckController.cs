using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Queries;
using System.Collections.Generic;
using System.Web.Http;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Models.DataMonster;

namespace MammothWebApi.Controllers
{
    public class HealthCheckController : ApiController
    {
        private IQueryHandler<GetHealthCheckQuery, int> getHealthCheckQueryHandler;
        public HealthCheckController(IQueryHandler<GetHealthCheckQuery, int> getHealthCheckQueryHandler)
        {
            this.getHealthCheckQueryHandler = getHealthCheckQueryHandler;
        }
        [HttpGet]
        [Route("api/healthcheck/check")]
        public IHttpActionResult Check()
        {
            var results = getHealthCheckQueryHandler.Search(new GetHealthCheckQuery());
            return Ok(results);
        }
    }
}