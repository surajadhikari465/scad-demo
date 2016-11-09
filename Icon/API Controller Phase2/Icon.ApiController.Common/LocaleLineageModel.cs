using System.Collections.Generic;

namespace Icon.ApiController.Common
{
    public class LocaleLineageModel
    {
        public int LocaleId { get; set; }
        public string LocaleName { get; set; }
        public string StoreAbbreviation { get; set; }
        public string BusinessUnitId { get; set; }
        public string PhoneNumber { get; set; }
        public int AddressId { get; set; }
        public string AddressUsageCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string CityName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string PostalCode { get; set; }
        public string TimezoneName { get; set; }
        public LocaleLineageModel AncestorLocale { get; set; }
        public List<LocaleLineageModel> DescendantLocales { get; set; }
    }
}
