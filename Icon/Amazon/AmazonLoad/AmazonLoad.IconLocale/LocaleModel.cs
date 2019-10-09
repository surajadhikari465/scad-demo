using System;

namespace AmazonLoad.IconLocale
{
    internal class LocaleModel
    {
        public int ChainId { get; internal set; }
        public string ChainName { get; internal set; }
        public int? RegionId { get; internal set; }
        public string RegionName { get; internal set; }
        public int? MetroId { get; internal set; }
        public string MetroName { get; internal set; }
        public int? StoreId { get; internal set; }
        public string StoreName { get; internal set; }
        public string BusinessUnit { get; set; }
        public string StoreAbbreviation { get; set; }
        public string PhoneNumber { get; set; }
        public int AddressId { get; internal set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; internal set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Territory { get; set; }
        public string TerritoryAbbrev { get; set; }
        public string Country { get; set; }
        public string CountryAbbrev { get; set; }
        public string Timezone { get; set; }
        public string CurrencyCode { get; set; }
        public string SodiumWarningRequired { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
    }
}