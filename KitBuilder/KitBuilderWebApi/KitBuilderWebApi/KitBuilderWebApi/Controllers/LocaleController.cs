using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Repository;
using KitBuilderWebApi.Helper;
using KitBuilderWebApi.QueryParameters;
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

		public LocaleController(
			ILogger<LocaleController> logger,
			IRepository<Locale> localesRepository)
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

		[HttpGet("GetMetrosByRegionId")]
		public IActionResult GetMetroByRegionId([FromQuery] int regionId)
		{

			var locales = from l in LocalesRepository.GetAll()
						  where l.RegionId == regionId && l.LocaleTypeId == (int)LocaleType.Metro
						  select l;

			return Ok(locales.ToList());

		}

		[HttpGet("GetStoresByMetroId")]
		public IActionResult GetStoresByMetroId([FromQuery] int metroId)
		{

			var locales = from l in LocalesRepository.GetAll()
						  where l.MetroId == metroId && l.LocaleTypeId == (int)LocaleType.Store
						  select l;

			return Ok(locales.ToList());

		}

		[HttpGet("GetLocales")]
		public IActionResult GetKGetLocales([FromQuery] LocaleSearchParameters localeSearchParameters)
		{
			if (localeSearchParameters == null)
			{
				logger.LogWarning("The localeSearchParameters object passed is either null or does not contain any rows.");
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var allLocales = from l in LocalesRepository.GetAll()
							 select l;

			BuildQueryToFilterLocaleData(localeSearchParameters, ref allLocales);

			var selectedLocales = from l in allLocales
								  join m in LocalesRepository.GetAll()
									on l.MetroId equals m.LocaleId into lGroup
								  from m in lGroup.DefaultIfEmpty()
								  join s in LocalesRepository.GetAll()
									on l.StoreId equals s.LocaleId into sGroup
								  from s in sGroup.DefaultIfEmpty()
								  select new
								  {
									  LocaleId = l.LocaleId,
									  LocaleName = l.LocaleName,
									  RegionCode = l.RegionCode,
									  Metro = m.LocaleName,
									  StoreAbbreviation = l.StoreAbbreviation == null ? s.StoreAbbreviation : l.StoreAbbreviation,
									  ChainId = l.ChainId,
									  RegionId = l.RegionId,
									  MetroId = l.MetroId,
									  StoreId = l.StoreId
								  };

			return Ok(selectedLocales);
		}

		internal void BuildQueryToFilterLocaleData(LocaleSearchParameters localeSearchParameters,
			ref IQueryable<Locale> allLocales)
		{
			if (!string.IsNullOrEmpty(localeSearchParameters.LocaleName))
			{
				var localeNameForWhereClause = localeSearchParameters.LocaleName.Trim().ToLower();
				allLocales = allLocales.Where(k => k.LocaleName.Contains(localeNameForWhereClause));
			}
			if (!string.IsNullOrEmpty(localeSearchParameters.StoreAbbreviation))
			{
				var StoreAbbreviationForWhereClause = localeSearchParameters.StoreAbbreviation.Trim().ToLower();
				allLocales = allLocales.Where(k => k.StoreAbbreviation.Contains(StoreAbbreviationForWhereClause));
			}

			if (localeSearchParameters.BusinessUnitId != null && localeSearchParameters.BusinessUnitId > 0)
			{
				allLocales = allLocales.Where(k => k.BusinessUnitId == localeSearchParameters.BusinessUnitId);
			}
		}
	}
}
