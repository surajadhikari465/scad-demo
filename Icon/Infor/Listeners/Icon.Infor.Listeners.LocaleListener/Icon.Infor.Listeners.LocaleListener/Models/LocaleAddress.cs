using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.LocaleListener.Models
{
   public class LocaleAddress
    {
        public LocaleAddress(int addressId, string addressLine1, string addressLine2, string addressLine3, string cityName,
                             string postalCode, string country, string territoryCode, string timeZoneName, string latitude,
                             string longitude, int? businessUnitId)
        {
            this.AddressId = addressId;
            this.AddressLine1 = addressLine1;
            this.AddressLine2 = addressLine2;
            this.AddressLine3 = addressLine3;
            this.CityName = cityName;
            this.PostalCode = postalCode;
            this.Country = country;
            this.TerritoryCode = territoryCode;
            this.TimeZoneName = timeZoneName;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.BusinessUnitId = businessUnitId;
        }
        public LocaleAddress()
        {

        }
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string TerritoryCode { get; set; }
        public string TimeZoneName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int? BusinessUnitId { get; set; }
    }
}
