using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Models
{
	public class StoreModel
	{
		public List<StoreModel> Locales { get; set; }

		public int LocaleId { get; set; }
		public int? ParentLocaleId { get; set; }
		public int OwnerOrgPartyId { get; set; }
		public string LocaleName { get; set; }
		public int? LocaleTypeId { get; set; }
		public string LocaleTypeDesc { get; set; }
		public string RegionAbbreviation { get; set; }
		public string BusinessUnitId { get; set; }
		public DateTime? OpenDate { get; set; }
		public DateTime? CloseDate { get; set; }
		public string LocaleAddLink { get; set; }
		public string ParentLocaleName { get; set; }
		public int? ChildLocaleTypeCode { get; set; }
		public string EwicAgencyId { get; set; }

		public bool HasChildren
		{
			get
			{
				return Locales.Any();
			}
		}

		// Address properties.
		public int AddressID { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string AddressLine3 { get; set; }
		public string City { get; set; }
		public int? TerritoryId { get; set; }
		public string TerritoryCode { get; set; }
		public string PostalCode { get; set; }
		public string County { get; set; }
		public int? CountryId { get; set; }
		public string CountryCode { get; set; }
		public int? TimeZoneId { get; set; }
		public string TimeZoneCode { get; set; }
		public decimal? Latitude { get; set; }
		public decimal? Longitude { get; set; }
		public string PhoneNumber { get; set; }
		public string ContactPerson { get; set; }
		public string StoreAbbreviation { get; set; }
		public string IrmaStoreId { get; set; }
		public string StorePosType { get; set; }
		public string Fax { get; set; }
		public string CurrencyCode { get; set; }
		public string UserName { get; set; }
		public bool Ident { get; set; }
		public string LocalZone { get; set; }
		public string LiquorLicense { get; set; }
		public string PrimeMerchantID { get; set; }
		public string PrimeMerchantIDEncrypted { get; set; }
		public bool SodiumWarningRequired { get; set; }

		public StoreModel() { }
	}
}