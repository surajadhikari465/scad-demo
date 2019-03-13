using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LocaleType = KitBuilderWebApi.Helper.LocaleType;

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

        [HttpGet("GetLocaleByTypeId")]
        public IActionResult GetLocaleByTypeId([FromQuery]string localeType)
        {
            LocaleType localeTypePassed;
            if (Enum.TryParse(localeType, true, out localeTypePassed))
            {
                var locales = from l in LocalesRepository.GetAll()
                              where l.LocaleTypeId == (int)localeTypePassed
                              select l;
                return Ok(locales.ToList());
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetMetroByRegionId")]
        public IActionResult GetMetroByRegionId([FromQuery] int regionId)
        {

            var locales = from l in LocalesRepository.GetAll()
                          where l.RegionId == regionId
                          select l;

            return Ok(locales.ToList());

        }

        [HttpGet("GetStoresByMetroId")]
        public IActionResult GetStoresBytoreId([FromQuery] int metroId)
        {

            var locales = from l in LocalesRepository.GetAll()
                          where l.MetroId == metroId
                          select l;

            return Ok(locales.ToList());

        }
    }
}
