using Icon.Web.DataAccess.Models;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateLocaleManager
    {
        public UpdateLocaleManager(){}

        public UpdateLocaleManager(StoreModel storeModel)
        {
            LocaleId = storeModel.LocaleId;
            LocaleName = storeModel.LocaleName;
            ParentLocaleId = storeModel.ParentLocaleId;
            OpenDate = storeModel.OpenDate;
            CloseDate = storeModel.CloseDate;
            OwnerOrgPartyId = storeModel.OwnerOrgPartyId;
            LocaleTypeId = storeModel.LocaleTypeId;
            StoreAbbreviation = storeModel.StoreAbbreviation;
            BusinessUnitId = storeModel.BusinessUnitId;
            PhoneNumber = storeModel.PhoneNumber;
            Fax = storeModel.Fax;
            ContactPerson = storeModel.ContactPerson;
            AddressId = storeModel.AddressID;
            AddressLine1 = storeModel.AddressLine1;
            AddressLine2 = storeModel.AddressLine2;
            AddressLine3 = storeModel.AddressLine3;
            CityName = storeModel.City;
            PostalCode = storeModel.PostalCode;
            CountyName = storeModel.County;
            CountryId = storeModel.CountryId.Value;
            TerritoryId = storeModel.TerritoryId.Value;
            TimezoneId = storeModel.TimeZoneId.Value;
            Latitude = storeModel.Latitude;
            Longitude = storeModel.Longitude;
            EwicAgencyId = storeModel.EwicAgencyId;
            IrmaStoreId = storeModel.IrmaStoreId;
            StorePosType = storeModel.StorePosType;
            UserName = storeModel.UserName;
						Ident = storeModel.Ident;
						LocalZone = storeModel.LocalZone;
						LiquorLicense = storeModel.LiquorLicense;
						PrimeMerchantID = storeModel.PrimeMerchantID;
						PrimeMerchantIDEncrypted = storeModel.PrimeMerchantIDEncrypted;
        }
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
        public string CurrencyCode { get; set; }
        public string VenueCode { get; set; }
        public string VenueOccupant { get; set; }
        public int LocaleSubTypeId { get; set; }

				public bool Ident { get; set; }
				public string LocalZone { get; set; }
				public string LiquorLicense { get; set; }
				public string PrimeMerchantID { get; set; }
				public string PrimeMerchantIDEncrypted { get; set; }
    }
}