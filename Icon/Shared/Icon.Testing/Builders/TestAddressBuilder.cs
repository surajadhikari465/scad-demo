using Icon.Framework;

namespace Icon.Testing.Builders
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

        public TestAddressBuilder WithAddressId(int addressId)
        {
            this.addressId = addressId;
            return this;
        }

        public TestAddressBuilder WithAddressTypeId(int addressTypeId)
        {
            this.addressTypeId = addressTypeId;
            return this;
        }

        public TestAddressBuilder WithAddressLine1(string addressLine1)
        {
            this.addressLine1 = addressLine1;
            return this;
        }

        public TestAddressBuilder WithAddressLine2(string addressLine2)
        {
            this.addressLine2 = addressLine2;
            return this;
        }

        public TestAddressBuilder WithAddressLine3(string addressLine3)
        {
            this.addressLine3 = addressLine3;
            return this;
        }

        public TestAddressBuilder WithCity(string city)
        {
            this.city = city;
            return this;
        }

        public TestAddressBuilder WithTerritoryId(int territoryId)
        {
            this.territoryId = territoryId;
            return this;
        }

        public TestAddressBuilder WithPostalCode(string postalCode)
        {
            this.postalCode = postalCode;
            return this;
        }

        public TestAddressBuilder WithCounty(string county)
        {
            this.county = county;
            return this;
        }

        public TestAddressBuilder WithCountyId(int countyId)
        {
            this.countyId = countyId;
            return this;
        }

        public TestAddressBuilder WithCountryId(int country)
        {
            this.countryId = country;
            return this;
        }

        public TestAddressBuilder WithTimeZoneId(int timeZoneCode)
        {
            this.timeZoneId = timeZoneCode;
            return this;
        }

        public TestAddressBuilder WithLatitude(decimal? latitude)
        {
            this.latitude = latitude;
            return this;
        }

        public TestAddressBuilder WithLongitude(decimal? longitude)
        {
            this.longitude = longitude;
            return this;
        }

        public Address BuildAddress()
        {
            Address buildAddress = new Address { addressID = this.addressId, addressTypeID = this.addressTypeId };
            return buildAddress;
        }

        public County BuildCounty()
        {
            County buildCounty = new County { countyID = this.countyId, countyName = this.county, territoryID = this.territoryId };
            return buildCounty;
        }

        public City BuildCity()
        {
            City buildCity = new City { cityName = this.city, countyID = this.countyId, territoryID = this.territoryId };
            return buildCity;
        }

        public PostalCode BuildPostalCode()
        {
            PostalCode buildPostalCode = new PostalCode { countryID = this.countryId, countyID = this.countyId, postalCode = this.postalCode };
            return buildPostalCode;
        }

        public TestAddressBuilder WithCityId(int cityId)
        {
            this.cityId = cityId;
            return this;
        }

        public TestAddressBuilder WithPostalCodeId(int postalCodeId)
        {
            this.postalCodeId = postalCodeId;
            return this;
        }

        public PhysicalAddress BuildPhysicalAddress()
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
