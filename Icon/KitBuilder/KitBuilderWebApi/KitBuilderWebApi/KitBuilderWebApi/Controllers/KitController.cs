using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitBuilderWebApi.DataAccess.Repository;
using KitBuilderWebApi.DatabaseModels;
using KitBuilderWebApi.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KitBuilderWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Kit")]
    public class KitController : Controller
    {
        private IRepository<Kit> kitRepository;
        private IRepository<Status> statusRespository;

        private ILogger<KitController> logger;

        public KitController(IRepository<Kit> kitRepository,
            IRepository<Status> statusRespository,
            ILogger<KitController> logger)
        {
            this.kitRepository = kitRepository;
            this.statusRespository = statusRespository;
            this.logger = logger;
        }



        public IActionResult GetKits(KitParameters kitParameters)
        {
            if (!ModelState.IsValid || kitParameters == null)
                return BadRequest(ModelState);


            return Ok();
        }

        
    }

    public class KitDto
    {

    }
}