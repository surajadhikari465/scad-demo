using IconWebApi.DataAccess.Models;
using IconWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IconWebApi.Extensions
{
	public static class Extensions
	{
		public static IEnumerable<Chain> ToIChainModel(this IEnumerable<GenericLocale> locales)
		{
			List<Chain> chainModels = new List<Chain>();
			foreach (var locale in locales)
			{
				var chainModel = new Chain
				{
					ChainId = locale.LocaleId,
					ChainName = locale.LocaleName
				};
				chainModels.Add(chainModel);
			}

			return chainModels;
		}

		public static IEnumerable<Region> ToRegionModel(this IEnumerable<GenericLocale> locales)
		{
			List<Region> regionModels = new List<Region>();
			foreach (var locale in locales)
			{
				var regionModel = new Region
				{
					RegionId = locale.LocaleId,
					RegionName = locale.LocaleName,
					RegionCode = locale.RegionCode,
					ChainId = locale.ParentLocaleId
				};
				regionModels.Add(regionModel);
			}

			return regionModels;
		}

		public static IEnumerable<Metro> ToMetroModel(this IEnumerable<GenericLocale> locales)
		{
			List<Metro> metroModels = new List<Metro>();
			foreach (var locale in locales)
			{
				var metroModel = new Metro
				{
					MetroId = locale.LocaleId,
					MetroName = locale.LocaleName,
					RegionId = locale.ParentLocaleId
				};
				metroModels.Add(metroModel);
			}

			return metroModels;
		}

		public static IEnumerable<Store> ToStoreModel(this IEnumerable<GenericLocale> locales, bool includeAddress)
		{
			List<Store> storeModels = new List<Store>();
			foreach (var locale in locales)
			{
				if (includeAddress)
				{
					var storeModel = new Store
					{
						StoreId = locale.LocaleId,
						StoreName = locale.LocaleName,
						MetroId = locale.ParentLocaleId,
						OpenDate = locale.LocaleOpenDate,
						CloseDate = locale.LocaleCloseDate,
						BusinessUnitId = locale.BusinessUnitId,
						StoreAbbreviation = locale.StoreAbbreviation,
						CurrencyCode = locale.CurrencyCode,
						StoreAddress = new Address
						{
							AddressLine1 = locale.AddressLine1,
							AddressLine2 = locale.AddressLine2,
							AddressLine3 = locale.AddressLine3,
							cityName = locale.CityName,
							territoryCode = locale.TerritoryCode,
							postalCode = locale.PostalCode,
							countryCode = locale.CountryCode,
							countryName = locale.CountryName
						}
					};
					storeModels.Add(storeModel);
				}
				else
				{
					var storeModel = new Store
					{
						StoreId = locale.LocaleId,
						StoreName = locale.LocaleName,
						MetroId = locale.ParentLocaleId,
						OpenDate = locale.LocaleOpenDate,
						CloseDate = locale.LocaleCloseDate,
						BusinessUnitId = locale.BusinessUnitId,
						StoreAbbreviation = locale.StoreAbbreviation,
						CurrencyCode = locale.CurrencyCode,
					};
					storeModels.Add(storeModel);
				};
			}

			return storeModels;
		}

		public static IEnumerable<Venue> ToVenueModel(this IEnumerable<GenericLocale> locales)
		{
			List<Venue> venueModels = new List<Venue>();
			foreach (var locale in locales)
			{
				var venueModel = new Venue
				{
					VenueId = locale.LocaleId,
					VenueName = locale.LocaleName,
					StoreId = locale.ParentLocaleId,
					OpenDate = locale.LocaleOpenDate,
					CloseDate = locale.LocaleCloseDate,
					SubType = locale.SubType
				};
				venueModels.Add(venueModel);
			}

			return venueModels;
		}
	}
}