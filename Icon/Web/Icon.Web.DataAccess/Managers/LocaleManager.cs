using System;

namespace Icon.Web.DataAccess.Managers
{
	public class LocaleManager
	{
		public int LocaleId { get; set; }
		public string LocaleName { get; set; }
		public DateTime? OpenDate { get; set; }
		public DateTime? CloseDate { get; set; }
		public int? LocaleParentId { get; set; }
		public int LocaleTypeID { get; set; }
		public int OwnerOrgPartyId { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string AddressLine3 { get; set; }
		public string City { get; set; }
		public int TerritoryId { get; set; }
		public string PostalCode { get; set; }
		public string County { get; set; }
		public string CountryId { get; set; }
		public int TimeZoneId { get; set; }
		public decimal? Latitude { get; set; }
		public decimal? Longitude { get; set; }
		public string UserName { get; set; }
		public bool SodiumWarningRequired { get; set; }
	}
}
