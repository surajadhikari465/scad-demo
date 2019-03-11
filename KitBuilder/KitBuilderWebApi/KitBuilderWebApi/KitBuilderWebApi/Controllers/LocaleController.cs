using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KitBuilderWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Locales")]

    public class LocaleController : Controller
    {
        readonly IRepository<Locale> LocalesRepository;
        readonly ILogger<LocaleController> logger;

        public LocaleController(ILogger<LocaleController> logger, IRepository<Locale> localesRepository)
        {
            this.logger = logger;
            LocalesRepository = localesRepository;
        }

        [HttpGet("GetLocaleByType")]
        public IActionResult GetLocaleByType([FromQuery]string localeTypeDesc)
        {

            var locales = from l in LocalesRepository.GetAll()
                          where l.LocaleType.LocaleTypeDesc == localeTypeDesc
                          select l;

            return Ok(locales.ToList());

        }
    }
}
