using System;

namespace Mammoth.Esb.LocaleListener.Models
{
    public class LocaleModel
    {
        public string Region { get; set; }
        public int BusinessUnitID { get; set; }
        public string StoreName { get; set; }
        public string StoreAbbrev { get; set; }
        public string PhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Territory { get; set; }
        public string TerritoryAbbrev { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string CountryAbbrev { get; set; }
        public string Timezone { get; set; }
        public DateTime? LocaleOpenDate { get; set; }
        public DateTime? LocaleCloseDate { get; set; }
    }
}
