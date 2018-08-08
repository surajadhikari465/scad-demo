using Icon.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Models
{
    public class LocaleGridRowViewModel
    {
        public List<LocaleGridRowViewModel> Locales { get; set; }

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

        [AllowHtml]
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
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public string StoreAbbreviation { get; set; }
        public string IrmaStoreId { get; set; }
        public string StorePosType { get; set; }
        public string Fax { get; set; }
        public string CurrencyCode { get; set; }

        // Venue Properties
        public string LocaleSubType { get; set; }

        [StringLength(255, ErrorMessage = "The Venue Code value cannot exceed 255 characters. ")]
        public string VenueCode { get; set; }

        [StringLength(255, ErrorMessage = "The Venue Occupant value cannot exceed 255 characters. ")]
        public string VenueOccupant { get; set; }
        public int LocaleSubTypeId { get; set; }

        public LocaleGridRowViewModel() { }

        public LocaleGridRowViewModel(Locale locale)
        {
            LocaleId = locale.localeID;
            LocaleName = locale.localeName;
            ParentLocaleId = locale.parentLocaleID;
            OwnerOrgPartyId = locale.ownerOrgPartyID;
            LocaleTypeId = locale.localeTypeID;
            LocaleTypeDesc = locale.LocaleType.localeTypeDesc;
            OpenDate = locale.localeOpenDate;
            CloseDate = locale.localeCloseDate;
            RegionAbbreviation = LocaleAccessor.GetRegionAbbreviation(locale);
            BusinessUnitId = LocaleAccessor.GetBusinessUnit(locale);

            AddressID = LocaleAccessor.GetAddressId(locale);
            AddressLine1 = LocaleAccessor.GetAddressLine1(locale);
            AddressLine2 = LocaleAccessor.GetAddressLine2(locale);
            AddressLine3 = LocaleAccessor.GetAddressLine3(locale);
            City = LocaleAccessor.GetCity(locale);
            PostalCode = LocaleAccessor.GetPostalCode(locale);
            County = LocaleAccessor.GetCounty(locale);
            TerritoryCode = LocaleAccessor.GetTerritoryCode(locale);
            CountryCode = LocaleAccessor.GetCountryCode(locale);
            TimeZoneCode = LocaleAccessor.GetTimeZoneCode(locale);
            TerritoryId = LocaleAccessor.GetTerritoryId(locale);
            CountryId = LocaleAccessor.GetCountryId(locale);
            TimeZoneId = LocaleAccessor.GetTimeZoneId(locale);
            Latitude = LocaleAccessor.GetLatitude(locale).ToString();
            Longitude = LocaleAccessor.GetLongitude(locale).ToString();
            PhoneNumber = LocaleAccessor.GetPhoneNumber(locale);
            ContactPerson = LocaleAccessor.GetContactPerson(locale);
            StoreAbbreviation = LocaleAccessor.GetStoreAbbreviation(locale);
            EwicAgencyId = LocaleAccessor.GetEwicAgencyId(locale);
            IrmaStoreId = LocaleAccessor.GetIrmaStoreId(locale);
            StorePosType = LocaleAccessor.GetStorePosType(locale);
            Fax = LocaleAccessor.GetFax(locale);
            CurrencyCode = LocaleAccessor.GetCurrencyCode(locale);
            LocaleSubType = LocaleAccessor.GetLocaleSubType(locale);
            VenueCode = LocaleAccessor.GetVenueCode(locale);
            VenueOccupant = LocaleAccessor.GetVenueOccupant(locale);
            Locales = new List<LocaleGridRowViewModel>();
        }
    }
}