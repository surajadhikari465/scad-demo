using IconWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IconWebApi.Tests.ModelBuilders
{
	internal class TestGenericLocaleBuilder
	{
		private int LocaleId;
		private string LocaleName;
		private int LocaleTypeId;
		private int ParentLocaleId;
		private DateTime? LocaleOpenDate;
		private DateTime? LocaleCloseDate;
		private string RegionCode;
		private int BusinessUnitId;
		private string StoreAbbreviation;
		private string CurrencyCode;
		private string SubType;
		private string AddressLine1;
		private string AddressLine2;
		private string AddressLine3;
		private string CityName;
		private string TerritoryCode;
		private string PostalCode;
		private string CountryCode;
		private string CountryName;

		public TestGenericLocaleBuilder()
		{
		}

		internal TestGenericLocaleBuilder WithLocaleId(int localeId)
		{
			this.LocaleId = localeId;
			return this;
		}

		internal TestGenericLocaleBuilder WithLocaleName(string localeName)
		{
			this.LocaleName = localeName;
			return this;
		}

		internal TestGenericLocaleBuilder WithLocaleTypeId(int localeTypeId)
		{
			this.LocaleTypeId = localeTypeId;
			return this;
		}

		internal TestGenericLocaleBuilder WithParentLocaleId(int parentLocaleId)
		{
			this.ParentLocaleId = parentLocaleId;
			return this;
		}

		internal TestGenericLocaleBuilder WithLocaleOpenDate(DateTime? localeOpenDate)
		{
			this.LocaleOpenDate = localeOpenDate;
			return this;
		}

		internal TestGenericLocaleBuilder WithLocaleCloseDate(DateTime? localeCloseDate)
		{
			this.LocaleCloseDate = localeCloseDate;
			return this;
		}

		internal TestGenericLocaleBuilder WithRegionCode(string regionCode)
		{
			this.RegionCode = regionCode;
			return this;
		}

		internal TestGenericLocaleBuilder WithBusinessUnitId(int businessUnitId)
		{
			this.BusinessUnitId = businessUnitId;
			return this;
		}

		internal TestGenericLocaleBuilder WithStoreAbbreviation(string storeAbbreviation)
		{
			this.StoreAbbreviation = storeAbbreviation;
			return this;
		}

		internal TestGenericLocaleBuilder WithCurrencyCode(string currencyCode)
		{
			this.CurrencyCode = currencyCode;
			return this;
		}

		internal TestGenericLocaleBuilder WithSubType(string subType)
		{
			this.SubType = subType;
			return this;
		}

		internal TestGenericLocaleBuilder WithAddressLine1(string addressLine1)
		{
			this.AddressLine1 = addressLine1;
			return this;
		}

		internal TestGenericLocaleBuilder WithAddressLine2(string addressLine2)
		{
			this.AddressLine2 = addressLine2;
			return this;
		}

		internal TestGenericLocaleBuilder WithAddressLine3(string addressLine3)
		{
			this.AddressLine3 = addressLine3;
			return this;
		}

		internal TestGenericLocaleBuilder WithCityName(string cityName)
		{
			this.CityName = cityName;
			return this;
		}

		internal TestGenericLocaleBuilder WithTerritoryCode(string territoryCode)
		{
			this.TerritoryCode = territoryCode;
			return this;
		}

		internal TestGenericLocaleBuilder WithPostalCode(string postalCode)
		{
			this.PostalCode = postalCode;
			return this;
		}

		internal TestGenericLocaleBuilder WithCountryCode(string countryCode)
		{
			this.CountryCode = countryCode;
			return this;
		}
		internal TestGenericLocaleBuilder WithCountryName(string countryName)
		{
			this.CountryName = countryName;
			return this;
		}

		internal GenericLocale Build()
		{
			var model = new GenericLocale
			{
				LocaleId = this.LocaleId,
				LocaleName = this.LocaleName,
				LocaleTypeId = this.LocaleTypeId,
				LocaleOpenDate = this.LocaleOpenDate,
				LocaleCloseDate = this.LocaleCloseDate,
				ParentLocaleId = this.ParentLocaleId,
				RegionCode = this.RegionCode,
				BusinessUnitId = this.BusinessUnitId,
				StoreAbbreviation = this.StoreAbbreviation,
				CurrencyCode = this.CurrencyCode,
				SubType = this.SubType,
				AddressLine1 = this.AddressLine1,
				AddressLine2 = this.AddressLine2,
				AddressLine3 = this.AddressLine3,
				CityName = this.CityName,
				TerritoryCode = this.TerritoryCode,
				PostalCode = this.PostalCode,
				CountryCode = this.CountryCode,
				CountryName = this.CountryName
			};

			return model;
		}
	}
}
