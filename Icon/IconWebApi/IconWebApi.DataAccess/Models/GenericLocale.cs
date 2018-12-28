using System;

namespace IconWebApi.DataAccess.Models
{
	public class GenericLocale
	{
		public int LocaleId { get; set; }
		public string LocaleName { get; set; }
		public int LocaleTypeId { get; set; }
		public int ParentLocaleId { get; set; }
		public DateTime? LocaleOpenDate { get; set; }
		public DateTime? LocaleCloseDate { get; set; }
		public string RegionCode { get; set; }
		public int BusinessUnitId { get; set; }
		public string StoreAbbreviation { get; set; }
		public string CurrencyCode { get; set; }
		public string SubType { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string AddressLine3 { get; set; }
		public string CityName { get; set; }
		public string TerritoryCode { get; set; }
		public string PostalCode { get; set; }
		public string CountryCode { get; set; }
		public string CountryName { get; set; }
	}
}
