using System;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateLocaleManager
    {
        public int LocaleId { get; set; }
        public string LocaleName { get; set; }
        public int? ParentLocaleId { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public int? OwnerOrgPartyId { get; set; }
        public int? LocaleTypeId { get; set; }
        public string StoreAbbreviation { get; set; }
        public string BusinessUnitId { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string CountyName { get; set; }
        public int CountryId { get; set; }
        public int TerritoryId { get; set; }
        public int TimezoneId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string EwicAgencyId { get; set; }
        public string Fax { get; set; }
        public string IrmaStoreId { get; set; }
        public string StorePosType { get; set; }

        public string UserName { get; set; }
    }
}
