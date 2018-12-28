using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using IconWebApi.DataAccess.Models;
using IconWebApi.DataAccess.Queries;
using IconWebApi.Extensions;
using IconWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace IconWebApi.Controllers
{
	public class LocaleController : ApiController
	{
		private IQueryHandler<GetLocalesQuery, IEnumerable<GenericLocale>> getLocalesQueryHandler;
		private ILogger logger;

		public LocaleController(IQueryHandler<GetLocalesQuery, IEnumerable<GenericLocale>> getLocalesQueryHandler,
			ILogger logger)
		{
			this.getLocalesQueryHandler = getLocalesQueryHandler;
			this.logger = logger;
		}

		[HttpGet]
		[Route("api/Locale/Chains")]
		public IHttpActionResult Chains(bool includeChildren = false, bool includeAddress = false)
		{
			if (!includeChildren && includeAddress)
			{
				this.logger.Warn("The Chains search criteria are invalid because includeChildren is false while includeAddress is true. Address is only assoicated with stores.");
				return BadRequest("The Chains search criteria are invalid because includeChildren is false while includeAddress is true. Address is only assoicated with stores.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var locales = getLocalesQueryHandler.Search(new GetLocalesQuery()
				{
					includeAddress = includeAddress
				});

				var chainLocales = locales.Where(l => l.LocaleTypeId == LocaleTypes.Chain);
				IEnumerable<Chain> chains = chainLocales.ToIChainModel();

				if (includeChildren)
				{
					AssignRegionsToChain(chains, locales, includeAddress);
				}

				return Json(chains);
			}
			catch (Exception e)
			{
				this.logger.Error("Error performing Chains Http Get request. " + e.Message);
				return InternalServerError(new Exception(
					"There was an error retrieving Chains from the Icon database. Please reach out to the support team for assistance. - " + e.Message));
			}
		}

		[HttpGet]
		[Route("api/Locale/Chains/{chainId}")]
		public IHttpActionResult Chains(int chainId, bool includeChildren = false, bool includeAddress = false)
		{
			if (!includeChildren && includeAddress)
			{
				this.logger.Warn("The Chain search by ID criteria are invalid because includeChildren is false while includeAddress is true. Address is only assoicated with stores.");
				return BadRequest("The Chain search by ID criteria are invalid because includeChildren is false while includeAddress is true. Address is only assoicated with stores.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var locales = getLocalesQueryHandler.Search(new GetLocalesQuery()
				{
					includeAddress = includeAddress
				});

				var chainLocales = locales.Where(l => l.LocaleId == chainId && l.LocaleTypeId == LocaleTypes.Chain);
				IEnumerable<Chain> chains = chainLocales.ToIChainModel();

				if (includeChildren)
				{
					AssignRegionsToChain(chains, locales, includeAddress);
				}

				return Json(chains);
			}
			catch (Exception e)
			{
				this.logger.Error("Error performing Chain by ID Http Get request. " + e.Message);
				return InternalServerError(new Exception(
					"There was an error retrieving Chain by ID from the Icon database. Please reach out to the support team for assistance. - " + e.Message));
			}
		}

		[HttpGet]
		[Route("api/Locale/Regions")]
		public IHttpActionResult Regions(bool includeChildren = false, bool includeAddress = false)
		{
			if (!includeChildren && includeAddress)
			{
				this.logger.Warn("The Regions search criteria are invalid because includeChildren is false while includeAddress is true. Address is only assoicated with stores.");
				return BadRequest("The Regions search criteria are invalid because includeChildren is false while includeAddress is true. Address is only assoicated with stores.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var locales = getLocalesQueryHandler.Search(new GetLocalesQuery()
				{
					includeAddress = includeAddress
				});

				var regionLocales = locales.Where(l => l.LocaleTypeId == LocaleTypes.Region);
				IEnumerable<Region> regions = regionLocales.ToRegionModel();

				if (includeChildren)
				{
					AssignMetrosToRegion(regions, locales, includeAddress);
				}

				return Json(regions);
			}
			catch (Exception e)
			{
				this.logger.Error("Error performing Regions Http Get request. " + e.Message);
				return InternalServerError(new Exception(
					"There was an error retrieving Regions from the Icon database. Please reach out to the support team for assistance. - " + e.Message));
			}
		}

		[HttpGet]
		[Route("api/Locale/Regions/{regionId}")]
		public IHttpActionResult Regions(int regionId, bool includeChildren = false, bool includeAddress = false)
		{
			if (!includeChildren && includeAddress)
			{
				this.logger.Warn("The Region by ID search criteria are invalid because includeChildren is false while includeAddress is true. Address is only assoicated with stores.");
				return BadRequest("The Region by ID search criteria are invalid because includeChildren is false while includeAddress is true. Address is only assoicated with stores.");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var locales = getLocalesQueryHandler.Search(new GetLocalesQuery()
				{
					includeAddress = false
				});

				var regionLocaless = locales.Where(l => l.LocaleId == regionId && l.LocaleTypeId == LocaleTypes.Region);
				IEnumerable<Region> regions = regionLocaless.ToRegionModel();

				if (includeChildren)
				{
					AssignMetrosToRegion(regions, locales, false);
				}

				return Json(regions);
			}
			catch (Exception e)
			{
				this.logger.Error("Error performing Region by ID Http Get request. " + e.Message);
				return InternalServerError(new Exception(
					"There was an error retrieving Region by ID from the Icon database. Please reach out to the support team for assistance. - " + e.Message));
			}
		}

		[HttpGet]
		[Route("api/Locale/Stores")]
		public IHttpActionResult Stores(bool includeChildren = false, bool includeAddress = false, int regionId = 0, string regionAbbr = "", int metroId = 0, string storeAbbr = "", int storeBu = 0)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var locales = getLocalesQueryHandler.Search(new GetLocalesQuery()
				{
					includeAddress = includeAddress
				});

				var storeLocales = locales.Where(l => l.LocaleTypeId == LocaleTypes.Store);

				if (regionId > 0)
				{
					var regionLocale = locales.Where(l => l.LocaleId == regionId && l.LocaleTypeId == LocaleTypes.Region).FirstOrDefault();
					var metroLocales = locales.Where(l => l.ParentLocaleId == regionLocale.LocaleId);
					storeLocales = (from s in storeLocales
									join m in metroLocales on s.ParentLocaleId equals m.LocaleId
									select new GenericLocale
									{
										LocaleId = s.LocaleId,
										LocaleName = s.LocaleName,
										LocaleTypeId = s.LocaleTypeId,
										ParentLocaleId = s.ParentLocaleId,
										LocaleOpenDate = s.LocaleOpenDate,
										LocaleCloseDate = s.LocaleCloseDate,
										StoreAbbreviation = s.StoreAbbreviation,
										BusinessUnitId = s.BusinessUnitId,
										CurrencyCode = s.CurrencyCode,
										AddressLine1 = s.AddressLine1,
										AddressLine2 = s.AddressLine2,
										AddressLine3 = s.AddressLine3,
										CityName = s.CityName,
										TerritoryCode = s.TerritoryCode,
										PostalCode = s.PostalCode,
										CountryCode = s.CountryCode,
										CountryName = s.CountryName
									});
				}

				if (regionAbbr.Length > 0)
				{
					var regionLocale = locales.Where(l => l.RegionCode == regionAbbr && l.LocaleTypeId == LocaleTypes.Region).FirstOrDefault();
					var metroLocales = locales.Where(l => l.ParentLocaleId == regionLocale.LocaleId);
					storeLocales = (from s in storeLocales
									join m in metroLocales on s.ParentLocaleId equals m.LocaleId
									select new GenericLocale
									{
										LocaleId = s.LocaleId,
										LocaleName = s.LocaleName,
										LocaleTypeId = s.LocaleTypeId,
										ParentLocaleId = s.ParentLocaleId,
										LocaleOpenDate = s.LocaleOpenDate,
										LocaleCloseDate = s.LocaleCloseDate,
										StoreAbbreviation = s.StoreAbbreviation,
										BusinessUnitId = s.BusinessUnitId,
										CurrencyCode = s.CurrencyCode,
										AddressLine1 = s.AddressLine1,
										AddressLine2 = s.AddressLine2,
										AddressLine3 = s.AddressLine3,
										CityName = s.CityName,
										TerritoryCode = s.TerritoryCode,
										PostalCode = s.PostalCode,
										CountryCode = s.CountryCode,
										CountryName = s.CountryName
									});
				}

				if (metroId > 0)
				{
					var metroLocale = locales.Where(l => l.LocaleId == metroId && l.LocaleTypeId == LocaleTypes.Metro).FirstOrDefault();
					storeLocales = storeLocales.Where(l => l.ParentLocaleId == metroLocale.LocaleId);
				}

				if (storeAbbr.Length > 0)
				{
					storeLocales = storeLocales.Where(l => l.StoreAbbreviation.ToUpper() == storeAbbr.Trim().ToUpper());
				}

				if (storeBu > 0)
				{
					storeLocales = storeLocales.Where(l => l.BusinessUnitId == storeBu);
				}

				IEnumerable<Store> stores = storeLocales.ToStoreModel(includeAddress);

				if (includeChildren)
				{
					AssignVenuesToStore(stores, locales);
				}

				return Json(stores);
			}
			catch (Exception e)
			{
				this.logger.Error("Error performing Stores Http Get request. " + e.Message);
				return InternalServerError(new Exception(
					"There was an error retrieving Stores from the Icon database. Please reach out to the support team for assistance. - " + e.Message));
			}
		}

		[HttpGet]
		[Route("api/Locale/Stores/{storeId}")]
		public IHttpActionResult Stores(int storeId, bool includeChildren = false, bool includeAddress = false)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var locales = getLocalesQueryHandler.Search(new GetLocalesQuery()
				{
					includeAddress = includeAddress
				});

				var storeLocales = locales.Where(l => l.LocaleId == storeId && l.LocaleTypeId == LocaleTypes.Store);
				IEnumerable<Store> stores = storeLocales.ToStoreModel(includeAddress);

				if (includeChildren)
				{
					AssignVenuesToStore(stores, locales);
				}

				return Json(stores);
			}
			catch (Exception e)
			{
				this.logger.Error("Error performing Store by ID Http Get request. " + e.Message);
				return InternalServerError(new Exception(
					"There was an error retrieving Store by ID from the Icon database. Please reach out to the support team for assistance. - " + e.Message));
			}
		}

		[HttpGet]
		[Route("api/Locale/Venues")]
		public IHttpActionResult Venues()
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var locales = getLocalesQueryHandler.Search(new GetLocalesQuery()
				{
					includeAddress = false
				});

				var venueLocales = locales.Where(l => l.LocaleTypeId == LocaleTypes.Venue);
				IEnumerable<Venue> venues = venueLocales.ToVenueModel();

				return Json(venues);
			}
			catch (Exception e)
			{
				this.logger.Error("Error performing Venues Http Get request. " + e.Message);
				return InternalServerError(new Exception(
					"There was an error retrieving Venues from the Icon database. Please reach out to the support team for assistance. - " + e.Message));
			}
		}

		[HttpGet]
		[Route("api/Locale/Venues/{venueId}")]
		public IHttpActionResult Venues(int venueId)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}

				var locales = getLocalesQueryHandler.Search(new GetLocalesQuery()
				{
					includeAddress = false
				});

				var venueLocales = locales.Where(l => l.LocaleId == venueId && l.LocaleTypeId == LocaleTypes.Venue);

				IEnumerable<Venue> venues = venueLocales.ToVenueModel();

				return Json(venues);
			}
			catch (Exception e)
			{
				this.logger.Error("Error performing Venue by ID Http Get request. " + e.Message);
				return InternalServerError(new Exception(
					"There was an error retrieving Venue by ID from the Icon database. Please reach out to the support team for assistance. - " + e.Message));
			}
		}
		private void AssignRegionsToChain(IEnumerable<Chain> chains, IEnumerable<GenericLocale> locales, bool addressIncluded)
		{
			foreach (var chain in chains)
			{
				chain.Regions = locales.Where(l => l.LocaleTypeId == LocaleTypes.Region && l.ParentLocaleId == chain.ChainId).ToRegionModel();
				AssignMetrosToRegion(chain.Regions, locales, addressIncluded);
			}
		}

		private void AssignMetrosToRegion(IEnumerable<Region> regions, IEnumerable<GenericLocale> locales, bool addressIncluded)
		{
			foreach (var region in regions)
			{
				region.Metros = locales.Where(l => l.LocaleTypeId == LocaleTypes.Metro && l.ParentLocaleId == region.RegionId).ToMetroModel();
				AssignStoresToMetro(region.Metros, locales, addressIncluded);
			}
		}

		private void AssignStoresToMetro(IEnumerable<Metro> metros, IEnumerable<GenericLocale> locales, bool addressIncluded)
		{
			foreach (var metro in metros)
			{
				metro.Stores = locales.Where(l => l.LocaleTypeId == LocaleTypes.Store && l.ParentLocaleId == metro.MetroId).ToStoreModel(addressIncluded);
				AssignVenuesToStore(metro.Stores, locales);
			}
		}

		private void AssignVenuesToStore(IEnumerable<Store> stores, IEnumerable<GenericLocale> venueLocales)
		{
			foreach (var store in stores)
			{
				store.Venues = venueLocales.Where(l => l.LocaleTypeId == LocaleTypes.Venue && l.ParentLocaleId == store.StoreId).ToVenueModel();
			}
		}
	}
}
