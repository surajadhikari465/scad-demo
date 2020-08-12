using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] 
    public class AddAddressCommandHandlerTests
    {
        private IconContext context;
        private AddAddressCommand addressCommand;
        private AddAddressCommandHandler addAddressHandler;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();

            this.transaction = this.context.Database.BeginTransaction();

            // Add new locale for testing
            Locale locale = new Locale();
            locale.localeName = "Add Address Test Locale";
            locale.localeTypeID = LocaleTypes.Store;
            locale.parentLocaleID = this.context.Locale.First(l => l.localeTypeID == LocaleTypes.Metro).localeID;
            locale.ownerOrgPartyID = 1;
            this.context.Locale.Add(locale);
            this.context.SaveChanges();

            this.addressCommand = new AddAddressCommand();
            addressCommand.AddressLine1 = "221B Baker St";
            addressCommand.AddressLine2 = "Suite A";
            addressCommand.AddressLine3 = "test";
            addressCommand.City = "Capitole Citys";
            addressCommand.TerritoryId = this.context.Territory.First().territoryID;
            addressCommand.PostalCode = "78746";
            addressCommand.CountryId = this.context.Country.First().countryID;
            addressCommand.TimeZoneId = this.context.Timezone.First().timezoneID;
            addressCommand.County = "Trraviss";
            addressCommand.LocaleId = locale.localeID;
            addressCommand.Latitude = 30.288919M;
            addressCommand.Longitude = -97.829823M;

            this.addAddressHandler = new AddAddressCommandHandler(this.context);
        }

        [TestCleanup]
        public void CleanupData()
        {
            transaction.Rollback();
            this.addressCommand = null;
            this.addAddressHandler = null;
            this.context = null;
        }

        [TestMethod]
        public void Execute_NewCity_CityAdded()
        {
            // Given
            string expectedCity = this.addressCommand.City;

            // When
            addAddressHandler.Execute(addressCommand);

            // Then
            var actualCity = this.context.City.First(c => c.cityName.Contains(this.addressCommand.City));
            var entry = this.context.Entry(actualCity);
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.AreEqual(expectedCity, actualCity.cityName);
        }

        [TestMethod]
        public void Execute_CityCountyTerritoryCombinationExists_CityAndCountyNotAdded()
        {
            // Given
            County existingCounty = new County { countyName = "Trraviss", territoryID = addressCommand.TerritoryId };
            City existingCity = new City { cityName = "Capitole Citys", countyID = existingCounty.countyID, territoryID = addressCommand.TerritoryId };
            this.context.County.Add(existingCounty);
            this.context.City.Add(existingCity);
            this.context.SaveChanges();

            // When
            addAddressHandler.Execute(addressCommand);

            // Then
            var actualCounty = this.context.County.First(c => c.countyID == existingCounty.countyID);
            var actualCity = this.context.City.First(c => c.cityID == existingCity.cityID);
            var entry = this.context.Entry(actualCity);
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.AreEqual(existingCounty.countyName, actualCounty.countyName);
            Assert.AreEqual(existingCity.cityName, actualCity.cityName);
            Assert.IsTrue(this.context.County.Count(c => c.countyName == this.addressCommand.County) == 1);
            Assert.IsTrue(this.context.City.Count(c => c.cityName == this.addressCommand.City) == 1);
        }

        [TestMethod]
        public void Execute_NewCounty_CountyAdded()
        {
            // Given
            string expectedCounty = this.addressCommand.County;

            // When
            addAddressHandler.Execute(addressCommand);

            // Then
            var actualCounty = this.context.County.First(c => c.countyName.Contains(this.addressCommand.County) && c.territoryID == addressCommand.TerritoryId);
            var entry = this.context.Entry(actualCounty);
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.AreEqual(expectedCounty, actualCounty.countyName);
        }

        [TestMethod]
        public void Execute_NewPostalCode_PostalCodeAdded()
        {
            // Given
            string expectedPostalCode = this.addressCommand.PostalCode;

            // When
            addAddressHandler.Execute(addressCommand);

            // Then
            var actualPostalCode = this.context.PostalCode.First(p => p.postalCode.Contains(this.addressCommand.PostalCode));
            var entry = this.context.Entry(actualPostalCode);
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.AreEqual(expectedPostalCode, actualPostalCode.postalCode);
        }

        [TestMethod]
        public void Execute_NewAddress_AddressRowAdded()
        {
            // When
            addAddressHandler.Execute(addressCommand);

            // Then
            var actualAddress = this.context.Address.Single(a => a.addressTypeID == AddressTypes.PhysicalAddress && a.addressID == addressCommand.AddressId);
            var entry = this.context.Entry(actualAddress);

            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.IsNotNull(actualAddress);
        }

        [TestMethod]
        public void Execute_NewAddress_LocaleAddressAssociationCreated()
        {
            // When
            addAddressHandler.Execute(addressCommand);

            // Then
            var actualLocaleAddress = this.context.LocaleAddress.Single(la => la.addressID == addressCommand.AddressId && la.localeID == addressCommand.LocaleId);
            var entry = this.context.Entry(actualLocaleAddress);

            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.IsNotNull(actualLocaleAddress);
        }

        [TestMethod]
        public void Execute_NewAddress_PhysicalAddressRowAdded()
        {
            // Given
            string expectedAddressLine1 = addressCommand.AddressLine1;
            string expectedAddressLine2 = addressCommand.AddressLine2;
            string expectedAddressLine3 = addressCommand.AddressLine3;
            int expectedTerritoryId = addressCommand.TerritoryId;
            int expectedCountryId = addressCommand.CountryId;
            int expectedTimeZoneId = addressCommand.TimeZoneId;
            decimal? expectedLatitude = addressCommand.Latitude;
            decimal? expectedLongitude = addressCommand.Longitude;

            // When
            addAddressHandler.Execute(addressCommand);

            // Then
            var actualPhysicalAddress = this.context.PhysicalAddress.Single(pa => pa.addressID == addressCommand.AddressId);
            var entry = this.context.Entry(actualPhysicalAddress);

            int expectedAddressId = addressCommand.AddressId;
            int expectedCityId = this.context.City.First(c => c.cityName == addressCommand.City).cityID;
            int expectedPostalCodeId = this.context.PostalCode.First(p => p.postalCode == addressCommand.PostalCode).postalCodeID;

            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.AreEqual(expectedAddressId, actualPhysicalAddress.addressID, String.Format("Physical Address: Expected AddressId {0} did not match actual addressId {1}", expectedAddressId, actualPhysicalAddress.addressID));
            Assert.AreEqual(expectedAddressLine1, actualPhysicalAddress.addressLine1, String.Format("Physical Address: Expected AddressLine1 {0} did not match actual AddressLine1 {1}", expectedAddressLine1, actualPhysicalAddress.addressLine1));
            Assert.AreEqual(expectedAddressLine2, actualPhysicalAddress.addressLine2, String.Format("Physical Address: Expected AddressLine2 {0} did not match actual AddressLine2 {1}", expectedAddressLine2, actualPhysicalAddress.addressLine2));
            Assert.AreEqual(expectedAddressLine3, actualPhysicalAddress.addressLine3, String.Format("Physical Address: Expected AddressLine3 {0} did not match actual AddressLine3 {1}", expectedAddressLine3, actualPhysicalAddress.addressLine3));
            Assert.AreEqual(expectedCityId, actualPhysicalAddress.cityID, String.Format("Physical Address: Expected CityId {0} did not match actual CityId {1}", expectedCityId, actualPhysicalAddress.cityID));
            Assert.AreEqual(expectedPostalCodeId, actualPhysicalAddress.postalCodeID, String.Format("Physical Address: Expected PostalCodeId {0} did not match actual PostalCodeId {1}", expectedPostalCodeId, actualPhysicalAddress.postalCodeID));
            Assert.AreEqual(expectedTerritoryId, actualPhysicalAddress.territoryID, String.Format("Physical Address: Expected TerritoryId {0} did not match actual TerritoryId {1}", expectedTerritoryId, actualPhysicalAddress.territoryID));
            Assert.AreEqual(expectedCountryId, actualPhysicalAddress.countryID, String.Format("Physical Address: Expected CountryId {0} did not match actual CountryId {1}", expectedCountryId, actualPhysicalAddress.countryID));
            Assert.AreEqual(expectedTimeZoneId, actualPhysicalAddress.timezoneID, String.Format("Physical Address: Expected TimeZoneId {0} did not match actual TimeZoneId {1}", expectedTimeZoneId, actualPhysicalAddress.timezoneID));
            Assert.AreEqual(expectedLatitude, actualPhysicalAddress.latitude, String.Format("Physical Address: Expected Latitude {0} did not match actual Latitude {1}", expectedLatitude, actualPhysicalAddress.latitude));
            Assert.AreEqual(expectedLongitude, actualPhysicalAddress.longitude, String.Format("Physical Address: Expected Longitude {0} did not match actual Longitude {1}", expectedLongitude, actualPhysicalAddress.longitude));
        }
    }
}