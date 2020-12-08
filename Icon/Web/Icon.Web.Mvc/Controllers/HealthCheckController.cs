using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class HealthCheckController : Controller
    {
        private IQueryHandler<GetHealthCheckParameters, int> getHealthCheckQueryHandler;

        public HealthCheckController(IQueryHandler<GetHealthCheckParameters, int> getHealthCheckQueryHandler)
        {
            this.getHealthCheckQueryHandler = getHealthCheckQueryHandler;
        }

        [HttpGet]
        public int Check()
        {            
            return getHealthCheckQueryHandler.Search(new GetHealthCheckParameters());
        }
    }
}