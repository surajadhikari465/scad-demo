using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Common.Builders
{
    public class TestAddressBuilder
    {
        private int addressId;
        private int addressTypeId;
        private string addressLine1;
        private string addressLine2;
        private string addressLine3;
        private string city;
        private int cityId;
        private int territoryId;
        private string postalCode;
        private int postalCodeId;
        private string county;
        private int countyId;
        private int countryId;
        private int timeZoneId;
        private decimal? latitude;
        private decimal? longitude;

        public TestAddressBuilder()
        {
            this.addressId = 0;
            this.addressTypeId = 1;         // Physical Address
            this.addressLine1 = "100 Main Street";
            this.addressLine2 = null;
            this.addressLine3 = null;
            this.city = "Springfield";
            this.territoryId = 51;          // TX
            this.postalCode = "55555";
            this.county = "Springfield";
            this.countyId = 0;
            this.countryId = 1;             // United States
            this.timeZoneId = 5;            // CST
            this.latitude = null;
            this.longitude = null;
        }

        internal TestAddressBuilder WithAddressId(int addressId)
        {
            this.addressId = addressId;
            return this;
        }

        internal TestAddressBuilder WithAddressTypeId(int addressTypeId)
        {
            this.addressTypeId = addressTypeId;
            return this;
        }

        internal TestAddressBuilder WithAddressLine1(string addressLine1)
        {
            this.addressLine1 = addressLine1;
            return this;
        }

        internal TestAddressBuilder WithAddressLine2(string addressLine2)
        {
            this.addressLine2 = addressLine2;
            return this;
        }

        internal TestAddressBuilder WithAddressLine3(string addressLine3)
        {
            this.addressLine3 = addressLine3;
            return this;
        }

        internal TestAddressBuilder WithCity(string city)
        {
            this.city = city;
            return this;
        }

        internal TestAddressBuilder WithTerritoryId(int territoryId)
        {
            this.territoryId = territoryId;
            return this;
        }

        internal TestAddressBuilder WithPostalCode(string postalCode)
        {
            this.postalCode = postalCode;
            return this;
        }

        internal TestAddressBuilder WithCounty(string county)
        {
            this.county = county;
            return this;
        }

        internal TestAddressBuilder WithCountyId(int countyId)
        {
            this.countyId = countyId;
            return this;
        }

        internal TestAddressBuilder WithCountryId(int country)
        {
            this.countryId = country;
            return this;
        }

        internal TestAddressBuilder WithTimeZoneId(int timeZoneCode)
        {
            this.timeZoneId = timeZoneCode;
            return this;
        }

        internal TestAddressBuilder WithLatitude(decimal? latitude)
        {
            this.latitude = latitude;
            return this;
        }

        internal TestAddressBuilder WithLongitude(decimal? longitude)
        {
            this.longitude = longitude;
            return this;
        }

        internal Address BuildAddress()
        {
            Address buildAddress = new Address { addressID = this.addressId, addressTypeID = this.addressTypeId };
            return buildAddress;
        }

        internal County BuildCounty()
        {
            County buildCounty = new County { countyID = this.countyId, countyName = this.county, territoryID = this.territoryId };
            return buildCounty;
        }

        internal City BuildCity()
        {
            City buildCity = new City { cityName = this.city, countyID = this.countyId, territoryID = this.territoryId };
            return buildCity;
        }

        internal PostalCode BuildPostalCode()
        {
            PostalCode buildPostalCode = new PostalCode { countryID = this.countryId, countyID = this.countyId, postalCode = this.postalCode };
            return buildPostalCode;
        }

        internal TestAddressBuilder WithCityId(int cityId)
        {
            this.cityId = cityId;
            return this;
        }

        internal TestAddressBuilder WithPostalCodeId(int postalCodeId)
        {
            this.postalCodeId = postalCodeId;
            return this;
        }

        internal PhysicalAddress BuildPhysicalAddress()
        {
            PhysicalAddress physicalAddress = new PhysicalAddress();
            physicalAddress.addressID = this.addressId;
            physicalAddress.addressLine1 = this.addressLine1;
            physicalAddress.addressLine2 = this.addressLine2;
            physicalAddress.addressLine3 = this.addressLine3;
            physicalAddress.cityID = this.cityId;
            physicalAddress.countryID = this.countryId;
            physicalAddress.territoryID = this.territoryId;
            physicalAddress.timezoneID = this.timeZoneId;
            physicalAddress.latitude = this.latitude;
            physicalAddress.longitude = this.longitude;
            physicalAddress.postalCodeID = this.postalCodeId;

            return physicalAddress;
        }
    }
}
