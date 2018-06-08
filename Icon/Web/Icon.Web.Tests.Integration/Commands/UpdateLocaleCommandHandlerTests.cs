using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class UpdateLocaleCommandHandlerTests
    {
        private IconContext context;
        private UpdateLocaleCommand updateLocaleCommand;
        private UpdateLocaleCommandHandler updateLocaleCommandHandler;
        private string localeName;
        private string updatedLocaleName;
        private string storeAbbreviation;
        private int testLocaleId;
        private string testBusinessUnitId;
        private string updatedTestBusinessUnitId;
        private string contactPerson;
        private string phoneNumber;
        private string fax;
        private string storePosType;
        private string irmaStoreId;
        private PhysicalAddress physicalAddress;
        Locale testLocale;
        DbContextTransaction transaction;
        private GetCurrencyForCountryQuery getCurrencyQuery;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            getCurrencyQuery = new GetCurrencyForCountryQuery(context);
            updateLocaleCommandHandler = new UpdateLocaleCommandHandler(this.context, getCurrencyQuery);

            localeName = "Integration Test Store";
            storeAbbreviation = "TST";
            updatedLocaleName = "New Integration Test Store";
            testBusinessUnitId = "77777";
            updatedTestBusinessUnitId = "77772";
            contactPerson = "Test Contact Person";
            phoneNumber = "TestPhoneNumber";
            fax = "TestFax";
            storePosType = "TestPosType";
            irmaStoreId = "TestIrmaStoreId";

            transaction = context.Database.BeginTransaction();

            testLocale = new Locale { parentLocaleID = 1, localeName = localeName, localeOpenDate = DateTime.Now, ownerOrgPartyID = 1, localeTypeID = 4 };

            // Add the test locales to the db.
            context.Locale.Add(testLocale);
            context.SaveChanges();
            testLocaleId = testLocale.localeID;

            // Add the locale traits.
            var businessUnitTrait = new LocaleTrait { localeID = testLocaleId, traitID = Traits.PsBusinessUnitId, traitValue = testBusinessUnitId };
            var phoneNumberTrait = new LocaleTrait { localeID = testLocaleId, traitID = Traits.PhoneNumber, traitValue = phoneNumber };
            var contactPersonTrait = new LocaleTrait { localeID = testLocaleId, traitID = Traits.ContactPerson, traitValue = contactPerson };
            var storeAbbreviationTrait = new LocaleTrait { localeID = testLocaleId, traitID = Traits.StoreAbbreviation, traitValue = storeAbbreviation };
            var storePosTypeTrait = new LocaleTrait { localeID = testLocaleId, traitID = Traits.StorePosType, traitValue = storePosType };
            var faxTrait = new LocaleTrait { localeID = testLocaleId, traitID = Traits.Fax, traitValue = fax };
            var irmaStoreIdTrait = new LocaleTrait { localeID = testLocaleId, traitID = Traits.IrmaStoreId, traitValue = irmaStoreId };

            context.LocaleTrait.Add(businessUnitTrait);
            context.LocaleTrait.Add(phoneNumberTrait);
            context.LocaleTrait.Add(contactPersonTrait);
            context.LocaleTrait.Add(storeAbbreviationTrait);
            context.LocaleTrait.Add(storePosTypeTrait);
            context.LocaleTrait.Add(faxTrait);
            context.LocaleTrait.Add(irmaStoreIdTrait);

            // Add address.
            var address = new Address { addressTypeID = AddressTypes.PhysicalAddress };
            context.Address.Add(address);
            context.SaveChanges();

            var country = new Country { countryName = "Test Country", countryCode = "TCC" };
            context.Country.Add(country);
            context.SaveChanges();

            var timezone = new Timezone { timezoneName = "Test Timezone", timezoneCode = "TTC" };
            context.Timezone.Add(timezone);
            context.SaveChanges();

            var territory = new Territory { territoryName = "Test Territory", territoryCode = "123", countryID = country.countryID };
            context.Territory.Add(territory);
            context.SaveChanges();

            var county = new County { countyName = "Test County", territoryID = territory.territoryID };
            context.County.Add(county);
            context.SaveChanges();

            var postalCode = new PostalCode { postalCode = "73301", countryID = country.countryID, countyID = county.countyID };
            context.PostalCode.Add(postalCode);
            context.SaveChanges();

            var city = new City { cityName = "Test City", countyID = county.countyID, territoryID = territory.territoryID };
            context.City.Add(city);
            context.SaveChanges();

            var localeAddress = new LocaleAddress { addressID = address.addressID, addressUsageID = AddressUsages.Shipping, localeID = testLocale.localeID };

            physicalAddress = new PhysicalAddress
            {
                addressID = address.addressID,
                addressLine1 = "Test addressLine1",
                addressLine2 = "Test addressLine2",
                addressLine3 = "Test addressLine3",
                cityID = city.cityID,
                countryID = country.countryID,
                latitude = (decimal)1.1,
                longitude = (decimal)1.2,
                postalCodeID = postalCode.postalCodeID,
                territoryID = territory.territoryID,
                timezoneID = timezone.timezoneID
            };

            context.LocaleAddress.Add(localeAddress);
            context.PhysicalAddress.Add(physicalAddress);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }
        
        [TestMethod]
        public void UpdateLocale_NewValuesAreSentForAllLocaleProperties_ShouldUpdateAllPropertiesToNewValues()
        {
            // Given.
            var updateCountry = new Country { countryName = "Updated Test Country", countryCode = "TTT" };
            var updateTerritory = new Territory { territoryName = "Updated Test Territory", territoryCode = "TTT", countryID = updateCountry.countryID };
            var updateCounty = new County { countyName = "Updated Test County", territoryID = updateTerritory.territoryID };
            var updateCity = new City { cityName = "Updated Test City", countyID = updateCounty.countyID, territoryID = updateTerritory.territoryID };
            var updatePostalCode = new PostalCode { postalCode = "99999", countyID = updateCounty.countyID, countryID = updateCountry.countryID };
            var updateTimezone = new Timezone { timezoneName = "Updated Test Timezone", timezoneCode = "TTT" };
            var updateEwicAgency = new Agency { AgencyId = "ZZ", Locale = new List<Locale>() };

            context.Country.Add(updateCountry);
            context.Territory.Add(updateTerritory);
            context.County.Add(updateCounty);
            context.City.Add(updateCity);
            context.PostalCode.Add(updatePostalCode);
            context.Timezone.Add(updateTimezone);
            context.Agency.Add(updateEwicAgency);

            context.SaveChanges();

            UpdateLocaleCommand command = new UpdateLocaleCommand
            {
                LocaleId = testLocale.localeID,
                LocaleName = "Updated Test Locale Name",
                OpenDate = new DateTime(1999, 9, 9),
                CloseDate = new DateTime(2000, 1, 1),
                StoreAbbreviation = "TST",
                BusinessUnitId = "Updated Test BusinessUnitId",
                ContactPerson = "Updated Contact Person",
                PhoneNumber = "Updated Phone Number",
                AddressId = testLocale.LocaleAddress.First().addressID,
                AddressLine1 = "Updated AddressLine1",
                AddressLine2 = "Updated AddressLine2",
                AddressLine3 = "Updated AddressLine3",
                CityName = "Updated Test City",
                PostalCode = "99999",
                CountyName = "Updated Test County",
                CountryId = updateCountry.countryID,
                TerritoryId = updateTerritory.territoryID,
                TimezoneId = updateTimezone.timezoneID,
                Latitude = 99.9m,
                Longitude = 79.9m,
                EwicAgencyId = "ZZ",
                Fax = "New Fax",
                StorePosType = "New Store Pos Type",
                IrmaStoreId = "New Irma Store Id",
                UserName = "Test User"
            };

            // When.
            updateLocaleCommandHandler.Execute(command);

            // Then.
            Assert.AreEqual(command.LocaleName, testLocale.localeName);
            Assert.AreEqual(command.OpenDate, testLocale.localeOpenDate);
            Assert.AreEqual(command.CloseDate, testLocale.localeCloseDate);
            Assert.AreEqual(command.BusinessUnitId, testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue);
            Assert.AreEqual(command.ContactPerson, testLocale.LocaleTrait.First(lt => lt.traitID == Traits.ContactPerson).traitValue);
            Assert.AreEqual(command.PhoneNumber, testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PhoneNumber).traitValue);
            Assert.AreEqual(command.StoreAbbreviation, testLocale.LocaleTrait.First(lt => lt.traitID == Traits.StoreAbbreviation).traitValue);
            Assert.AreEqual(command.EwicAgencyId, testLocale.Agency.Single().AgencyId);
            Assert.AreEqual(command.Fax, testLocale.LocaleTrait.First(lt => lt.traitID == Traits.Fax).traitValue);
            Assert.AreEqual(command.StorePosType, testLocale.LocaleTrait.First(lt => lt.traitID == Traits.StorePosType).traitValue);
            Assert.AreEqual(command.IrmaStoreId, testLocale.LocaleTrait.First(lt => lt.traitID == Traits.IrmaStoreId).traitValue);
            Assert.AreEqual(command.UserName, testLocale.LocaleTrait.First(lt => lt.traitID == Traits.ModifiedUser).traitValue);

            PhysicalAddress testAddress = testLocale.LocaleAddress.First().Address.PhysicalAddress;

            Assert.AreEqual(command.AddressId, testAddress.addressID);
            Assert.AreEqual(command.AddressLine1, testAddress.addressLine1);
            Assert.AreEqual(command.AddressLine2, testAddress.addressLine2);
            Assert.AreEqual(command.AddressLine3, testAddress.addressLine3);
            Assert.AreEqual(command.CityName, testAddress.City.cityName);
            Assert.AreEqual(command.PostalCode, testAddress.PostalCode.postalCode);
            Assert.AreEqual(command.CountyName, testAddress.City.County.countyName);
            Assert.AreEqual(command.CountryId, testAddress.countryID);
            Assert.AreEqual(command.TerritoryId, testAddress.territoryID);
            Assert.AreEqual(command.TimezoneId, testAddress.timezoneID);
            Assert.AreEqual(command.Latitude, testAddress.latitude.Value);
            Assert.AreEqual(command.Longitude, testAddress.longitude.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Sequence contains no elements")]
        public void UpdateLocale_LocaleDoesNotExist_ShouldThrowException()
        {
            updateLocaleCommandHandler.Execute(new UpdateLocaleCommand
            {
                LocaleId = -1
            });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Sequence contains no elements")]
        public void UpdateLocale_BusinessUnitTraitDoesNotExist_ShouldThrowException()
        {
            // Given.
            var businessUnitTraitForTestLocale = context.LocaleTrait.First(lt => lt.localeID == testLocale.localeID && lt.traitID == Traits.PsBusinessUnitId);

            context.LocaleTrait.Remove(businessUnitTraitForTestLocale);
            context.SaveChanges();

            // When.
            updateLocaleCommandHandler.Execute(new UpdateLocaleCommand
            {
                LocaleId = testLocale.localeID
            });

            // Then.
            // Expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateLocale_DuplicateLocaleName_ShouldThrowException()
        {
            // Given.
            var localeToUpdate = new Locale
            {
                parentLocaleID = 1,
                localeName = updatedLocaleName,
                localeOpenDate = DateTime.Now,
                ownerOrgPartyID = 1,
                localeTypeID = LocaleTypes.Store
            };

            context.Locale.Add(localeToUpdate);
            context.SaveChanges();

            var businessUnit = new LocaleTrait
            {
                localeID = localeToUpdate.localeID,
                traitID = Traits.PsBusinessUnitId,
                traitValue = updatedTestBusinessUnitId,
                Trait = context.Trait.First(t => t.traitCode == TraitCodes.PsBusinessUnitId)
            };

            context.LocaleTrait.Add(businessUnit);
            context.SaveChanges();

            updateLocaleCommand = new UpdateLocaleCommand
            {
                LocaleId = localeToUpdate.localeID,
                LocaleName = "InTeGrATion TeSt sToRE",
                BusinessUnitId = businessUnit.traitValue,
                StoreAbbreviation = "TST"
            };

            // When.
            updateLocaleCommandHandler.Execute(updateLocaleCommand);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateLocale_DuplicateStoreAbbreviation_ShouldThrowException()
        {
            // Given.
            var localeToUpdate = new Locale
            {
                parentLocaleID = 1,
                localeName = updatedLocaleName,
                localeOpenDate = DateTime.Now,
                ownerOrgPartyID = 1,
                localeTypeID = 4
            };

            context.Locale.Add(localeToUpdate);
            context.SaveChanges();

            var businessUnit = new LocaleTrait
            {
                localeID = localeToUpdate.localeID,
                traitID = Traits.PsBusinessUnitId,
                traitValue = updatedTestBusinessUnitId,
                Trait = context.Trait.First(t => t.traitCode == TraitCodes.PsBusinessUnitId)
            };

            context.LocaleTrait.Add(businessUnit);
            context.SaveChanges();

            updateLocaleCommand = new UpdateLocaleCommand
            {
                LocaleId = localeToUpdate.localeID,
                LocaleName = "InTeGrTion TSt sToRE",
                BusinessUnitId = businessUnit.traitValue,
                StoreAbbreviation = "INT"
            };

            // When.
            updateLocaleCommandHandler.Execute(updateLocaleCommand);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateLocale_DuplicateBusinessUnit_ShouldThrowException()
        {
            // Given.
            var localeToUpdate = new Locale
            {
                parentLocaleID = 1,
                localeName = updatedLocaleName,
                localeOpenDate = DateTime.Now,
                ownerOrgPartyID = 1,
                localeTypeID = 4
            };

            context.Locale.Add(localeToUpdate);
            context.SaveChanges();

            var businessUnit = new LocaleTrait
            {
                localeID = localeToUpdate.localeID,
                traitID = Traits.PsBusinessUnitId,
                traitValue = updatedTestBusinessUnitId,
                Trait = context.Trait.First(t => t.traitCode == TraitCodes.PsBusinessUnitId)
            };

            context.LocaleTrait.Add(businessUnit);
            context.SaveChanges();

            updateLocaleCommand = new UpdateLocaleCommand
            {
                LocaleId = localeToUpdate.localeID,
                LocaleName = updatedLocaleName,
                BusinessUnitId = testBusinessUnitId
            };

            // When.
            updateLocaleCommandHandler.Execute(updateLocaleCommand);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void UpdateLocale_TheSamePropertiesAreSentForAllLocaleProperties_ShouldNotMakeAnyUpdates()
        {
            // Given.
            var address = testLocale.LocaleAddress.First().Address.PhysicalAddress;
            var originalCityId = address.cityID;
            var originalCountyId = address.City.countyID;
            var originalPostalCodeId = address.postalCodeID;

            UpdateLocaleCommand command = new UpdateLocaleCommand
            {
                LocaleId = testLocale.localeID,
                LocaleName = testLocale.localeName,
                OpenDate = testLocale.localeOpenDate,
                CloseDate = testLocale.localeCloseDate,
                BusinessUnitId = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue,
                ContactPerson = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.ContactPerson).traitValue,
                PhoneNumber = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PhoneNumber).traitValue,
                AddressId = address.addressID,
                AddressLine1 = address.addressLine1,
                AddressLine2 = address.addressLine2,
                AddressLine3 = address.addressLine3,
                CityName = testLocale.LocaleAddress.First().Address.PhysicalAddress.City.cityName,
                PostalCode = address.PostalCode.postalCode,
                CountyName = address.City.County.countyName,
                CountryId = address.countryID.Value,
                TerritoryId = address.territoryID.Value,
                Latitude = address.latitude.Value,
                Longitude = address.longitude.Value,
                TimezoneId = address.timezoneID.Value,
                EwicAgencyId = null
            };

            // When.
            updateLocaleCommandHandler.Execute(command);

            // Then.
            Assert.AreEqual(command.LocaleName, testLocale.localeName);
            Assert.AreEqual(command.OpenDate, testLocale.localeOpenDate);
            Assert.AreEqual(command.CloseDate, testLocale.localeCloseDate);
            Assert.AreEqual(command.BusinessUnitId, testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue);
            Assert.AreEqual(command.ContactPerson, testLocale.LocaleTrait.First(lt => lt.traitID == Traits.ContactPerson).traitValue);
            Assert.AreEqual(command.PhoneNumber, testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PhoneNumber).traitValue);
            Assert.AreEqual(command.CityName, address.City.cityName);
            Assert.AreEqual(command.PostalCode, address.PostalCode.postalCode);
            Assert.AreEqual(command.CountyName, address.City.County.countyName);
            Assert.AreEqual(command.CountryId, address.countryID);
            Assert.AreEqual(command.TerritoryId, address.territoryID);
            Assert.AreEqual(command.Latitude, address.latitude);
            Assert.AreEqual(command.Longitude, address.longitude);
            Assert.AreEqual(command.TimezoneId, address.timezoneID);
            Assert.IsNull(testLocale.Agency.SingleOrDefault());

            // Assert that address is pointing to original City, PostalCode, and County.
            Assert.AreEqual(originalCityId, address.cityID);
            Assert.AreEqual(originalCountyId, address.City.countyID);
            Assert.AreEqual(originalPostalCodeId, address.postalCodeID);
        }

        [TestMethod]
        public void UpdateLocale_ExistingEwicAgencyIsRemovedFromLocale_AgencyLocaleMappingShouldBeDeleted()
        {
            // Given.
            var address = testLocale.LocaleAddress.First().Address.PhysicalAddress;
            var originalCityId = address.cityID;
            var originalCountyId = address.City.countyID;
            var originalPostalCodeId = address.postalCodeID;

            var testEwicAgency = new Agency { AgencyId = "ZZ", Locale = new List<Locale> { testLocale } };
            context.Agency.Add(testEwicAgency);
            context.SaveChanges();

            UpdateLocaleCommand command = new UpdateLocaleCommand
            {
                LocaleId = testLocale.localeID,
                LocaleName = testLocale.localeName,
                OpenDate = testLocale.localeOpenDate,
                CloseDate = testLocale.localeCloseDate,
                BusinessUnitId = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue,
                ContactPerson = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.ContactPerson).traitValue,
                PhoneNumber = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PhoneNumber).traitValue,
                AddressId = address.addressID,
                AddressLine1 = address.addressLine1,
                AddressLine2 = address.addressLine2,
                AddressLine3 = address.addressLine3,
                CityName = testLocale.LocaleAddress.First().Address.PhysicalAddress.City.cityName,
                PostalCode = address.PostalCode.postalCode,
                CountyName = address.City.County.countyName,
                CountryId = address.countryID.Value,
                TerritoryId = address.territoryID.Value,
                Latitude = address.latitude.Value,
                Longitude = address.longitude.Value,
                TimezoneId = address.timezoneID.Value,
                EwicAgencyId = null
            };

            // When.
            updateLocaleCommandHandler.Execute(command);

            // Then.
            Assert.IsNull(testLocale.Agency.SingleOrDefault());
        }

        [TestMethod]
        public void UpdateLocale_CityNameDoesNotExistInDatabase_ShouldCreateNewCityInDatabaseAndAssignToLocale()
        {
            // Given.
            var address = testLocale.LocaleAddress.First().Address.PhysicalAddress;
            var originalCityId = address.cityID;
            var originalCountyId = address.City.countyID;

            UpdateLocaleCommand command = new UpdateLocaleCommand
            {
                LocaleId = testLocale.localeID,
                LocaleName = testLocale.localeName,
                OpenDate = testLocale.localeOpenDate,
                CloseDate = testLocale.localeCloseDate,
                BusinessUnitId = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue,
                ContactPerson = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.ContactPerson).traitValue,
                PhoneNumber = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PhoneNumber).traitValue,
                AddressId = address.addressID,
                AddressLine1 = address.addressLine1,
                AddressLine2 = address.addressLine2,
                AddressLine3 = address.addressLine3,
                CityName = "Updated CityName",
                PostalCode = address.PostalCode.postalCode,
                CountyName = address.City.County.countyName,
                CountryId = address.countryID.Value,
                TerritoryId = address.territoryID.Value,
                Latitude = address.latitude.Value,
                Longitude = address.longitude.Value,
                TimezoneId = address.timezoneID.Value
            };

            // When.
            updateLocaleCommandHandler.Execute(command);

            // Then.
            Assert.AreNotEqual(originalCityId, address.cityID);
            Assert.AreEqual(originalCountyId, address.City.countyID);
            Assert.AreEqual(command.TerritoryId, address.City.territoryID);
            Assert.AreEqual(command.CityName, address.City.cityName);
            Assert.AreEqual(command.CountyName, address.City.County.countyName);
        }

        [TestMethod]
        public void UpdateLocale_CountyNameDoesNotExistInDatabase_ShouldCreateNewCountyInDatabaseAndAssignedToLocale()
        {
            // Given.
            var address = testLocale.LocaleAddress.First().Address.PhysicalAddress;
            var originalCountyId = address.City.countyID;
            var originalCityId = address.cityID;
            var originalPostalCodeId = address.postalCodeID;

            UpdateLocaleCommand command = new UpdateLocaleCommand
            {
                LocaleId = testLocale.localeID,
                LocaleName = testLocale.localeName,
                OpenDate = testLocale.localeOpenDate,
                CloseDate = testLocale.localeCloseDate,
                BusinessUnitId = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue,
                ContactPerson = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.ContactPerson).traitValue,
                PhoneNumber = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PhoneNumber).traitValue,
                AddressId = address.addressID,
                AddressLine1 = address.addressLine1,
                AddressLine2 = address.addressLine2,
                AddressLine3 = address.addressLine3,
                CityName = address.City.cityName,
                PostalCode = address.PostalCode.postalCode,
                CountyName = "Updated CountyName",
                CountryId = address.countryID.Value,
                TerritoryId = address.territoryID.Value,
                Latitude = address.latitude.Value,
                Longitude = address.longitude.Value,
                TimezoneId = address.timezoneID.Value
            };

            // When.
            updateLocaleCommandHandler.Execute(command);

            // Then.
            Assert.AreNotEqual(originalCountyId, address.City.countyID);
            Assert.AreNotEqual(originalCountyId, address.PostalCode.countyID);
            Assert.AreNotEqual(originalCityId, address.cityID);
            Assert.AreEqual(address.City.countyID, address.PostalCode.countyID);
            Assert.AreEqual(command.TerritoryId, address.City.County.territoryID);
            Assert.AreEqual(command.CountyName, address.City.County.countyName);
            Assert.AreEqual(command.CityName, address.City.cityName);
            Assert.AreEqual(originalPostalCodeId, address.postalCodeID);
            Assert.AreEqual(command.PostalCode, address.PostalCode.postalCode);
        }

        [TestMethod]
        public void UpdateLocale_PostalCodeDoesNotExistInDatabase_ShouldCreateNewPostalCodeInDatabaseAndAssignToLocalesAddress()
        {
            // Given.
            var address = testLocale.LocaleAddress.First().Address.PhysicalAddress;
            var originalCountyId = address.City.County.countyID;
            var originalPostalCodeId = address.postalCodeID;

            UpdateLocaleCommand command = new UpdateLocaleCommand
            {
                LocaleId = testLocale.localeID,
                LocaleName = testLocale.localeName,
                OpenDate = testLocale.localeOpenDate,
                CloseDate = testLocale.localeCloseDate,
                BusinessUnitId = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue,
                ContactPerson = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.ContactPerson).traitValue,
                PhoneNumber = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PhoneNumber).traitValue,
                AddressId = address.addressID,
                AddressLine1 = address.addressLine1,
                AddressLine2 = address.addressLine2,
                AddressLine3 = address.addressLine3,
                CityName = address.City.cityName,
                PostalCode = "123456",
                CountyName = address.City.County.countyName,
                CountryId = address.countryID.Value,
                TerritoryId = address.territoryID.Value,
                Latitude = address.latitude.Value,
                Longitude = address.longitude.Value,
                TimezoneId = address.timezoneID.Value
            };

            // When.
            updateLocaleCommandHandler.Execute(command);

            // Then.
            Assert.AreNotEqual(originalPostalCodeId, address.postalCodeID);
            Assert.AreEqual(originalCountyId, address.PostalCode.countyID);
            Assert.AreEqual(command.PostalCode, address.PostalCode.postalCode);
            Assert.AreEqual(address.City.County.countyID, address.PostalCode.countyID);
            Assert.AreEqual(command.CountryId, address.PostalCode.countryID);
        }

        [TestMethod]
        public void UpdateLocale_CityNameAndCountyNameAndPostalCodeDoNotExistInDatabase_ShouldCreateNewCityAndCountyAndPostalCodeAndAssignToLocalesAddress()
        {
            // Given.
            var address = testLocale.LocaleAddress.First().Address.PhysicalAddress;
            var originalCityId = address.cityID;
            var originalCountyId = address.City.County.countyID;
            var originalPostalCodeId = address.postalCodeID;

            UpdateLocaleCommand command = new UpdateLocaleCommand
            {
                LocaleId = testLocale.localeID,
                LocaleName = testLocale.localeName,
                OpenDate = testLocale.localeOpenDate,
                CloseDate = testLocale.localeCloseDate,
                StoreAbbreviation = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.StoreAbbreviation).traitValue,
                BusinessUnitId = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue,
                ContactPerson = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.ContactPerson).traitValue,
                PhoneNumber = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PhoneNumber).traitValue,
                AddressId = address.addressID,
                AddressLine1 = address.addressLine1,
                AddressLine2 = address.addressLine2,
                AddressLine3 = address.addressLine3,
                CityName = "Test Updated CityName",
                PostalCode = "123456",
                CountyName = "Test Updated CountyName",
                CountryId = address.countryID.Value,
                TerritoryId = address.territoryID.Value,
                Latitude = address.latitude.Value,
                Longitude = address.longitude.Value,
                TimezoneId = address.timezoneID.Value
            };

            // When.
            updateLocaleCommandHandler.Execute(command);

            // Then.
            Assert.AreNotEqual(originalCountyId, address.City.County.countyID);
            Assert.AreNotEqual(originalCountyId, address.PostalCode.countyID);
            Assert.AreNotEqual(originalCountyId, address.City.countyID);
            Assert.AreNotEqual(originalPostalCodeId, address.postalCodeID);
            Assert.AreNotEqual(originalCityId, address.cityID);

            Assert.AreEqual(command.CountyName, address.City.County.countyName);
            Assert.AreEqual(command.CityName, address.City.cityName);
            Assert.AreEqual(command.PostalCode, address.PostalCode.postalCode);

            Assert.AreEqual(address.City.County.countyID, address.PostalCode.countyID);
            Assert.AreEqual(address.City.countyID, address.PostalCode.countyID);
            Assert.AreEqual(command.TerritoryId, address.City.County.territoryID);
            Assert.AreEqual(command.TerritoryId, address.City.territoryID);
            Assert.AreEqual(command.CountryId, address.PostalCode.countryID);
        }

        [TestMethod]
        public void UpdateLocale_NewTerritoryIdIsPassed_ShouldCreateNewCityAndCountyAssignedToNewTerritoryAndAssignNewCountyToPostalCode()
        {
            // Given.
            var address = testLocale.LocaleAddress.First().Address.PhysicalAddress;
            var originalCityId = address.cityID;
            var originalCountyId = address.City.County.countyID;
            var originalPostalCodeId = address.postalCodeID;
            var originalTerritoryId = address.territoryID;

            Territory newTerritory = new Territory
            {
                countryID = address.countryID.Value,
                territoryCode = "TST",
                territoryName = "Test Territory",
                territoryID = 99999
            };

            context.Territory.Add(newTerritory);
            context.SaveChanges();

            UpdateLocaleCommand command = new UpdateLocaleCommand
            {
                LocaleId = testLocale.localeID,
                LocaleName = testLocale.localeName,
                OpenDate = testLocale.localeOpenDate,
                CloseDate = testLocale.localeCloseDate,
                BusinessUnitId = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue,
                ContactPerson = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.ContactPerson).traitValue,
                PhoneNumber = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PhoneNumber).traitValue,
                AddressId = address.addressID,
                AddressLine1 = address.addressLine1,
                AddressLine2 = address.addressLine2,
                AddressLine3 = address.addressLine3,
                CityName = address.City.cityName,
                PostalCode = address.PostalCode.postalCode,
                CountyName = address.City.County.countyName,
                CountryId = address.countryID.Value,
                TerritoryId = newTerritory.territoryID,
                Latitude = address.latitude.Value,
                Longitude = address.longitude.Value,
                TimezoneId = address.timezoneID.Value
            };

            // When.
            updateLocaleCommandHandler.Execute(command);

            // Then.
            Assert.AreNotEqual(originalTerritoryId.Value, address.territoryID);
            Assert.AreNotEqual(originalCountyId, address.City.County.countyID);
            Assert.AreNotEqual(originalCountyId, address.PostalCode.countyID);
            Assert.AreNotEqual(originalCountyId, address.City.countyID);
            Assert.AreNotEqual(originalCityId, address.cityID);

            Assert.AreEqual(newTerritory.territoryID, address.territoryID);
            Assert.AreEqual(newTerritory.territoryID, address.City.County.territoryID);
            Assert.AreEqual(newTerritory.territoryID, address.City.territoryID);

            Assert.AreEqual(command.CountyName, address.City.County.countyName);
            Assert.AreEqual(command.CityName, address.City.cityName);
            Assert.AreEqual(command.PostalCode, address.PostalCode.postalCode);

            Assert.AreEqual(address.City.County.countyID, address.PostalCode.countyID);
            Assert.AreEqual(address.City.countyID, address.PostalCode.countyID);
            Assert.AreEqual(command.TerritoryId, address.City.County.territoryID);
            Assert.AreEqual(command.TerritoryId, address.City.territoryID);
            Assert.AreEqual(command.CountryId, address.PostalCode.countryID);
        }

        [TestMethod]
        public void UpdateLocale_CurrencyCodeTraitIsAdded_LocaleShouldHaveExpectedValue()
        {
            // Given.
            var address = testLocale.LocaleAddress.First().Address.PhysicalAddress;
            var originalCityId = address.cityID;
            var originalCountyId = address.City.countyID;
            var originalPostalCodeId = address.postalCodeID;

            UpdateLocaleCommand command = new UpdateLocaleCommand
            {
                LocaleId = testLocale.localeID,
                LocaleName = testLocale.localeName,
                OpenDate = testLocale.localeOpenDate,
                CloseDate = testLocale.localeCloseDate,
                BusinessUnitId = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue,
                AddressId = address.addressID,
                CityName = testLocale.LocaleAddress.First().Address.PhysicalAddress.City.cityName,
                PostalCode = address.PostalCode.postalCode,
                CountryId = address.countryID.Value,
                TerritoryId = address.territoryID.Value,
                TimezoneId = address.timezoneID.Value,
                CurrencyCode = CurrencyCodes.Usd
            };
            var existing = testLocale.LocaleTrait.FirstOrDefault(t => t.traitID == Traits.Currency);

            // When.
            updateLocaleCommandHandler.Execute(command);

            // Then.
            Assert.IsNull(existing, "locale should not have had the currency trait yet");
            var updatedLocale = context.Locale
                .Include(l => l.LocaleTrait )
                .FirstOrDefault(l => l.localeID == testLocale.localeID);
            var updated = updatedLocale.LocaleTrait.FirstOrDefault(t => t.traitID == Traits.Currency);

            Assert.AreEqual(CurrencyCodes.Usd, updated.traitValue, "locale should have a currency trait value");
        }

        [TestMethod]
        public void UpdateLocale_CurrencyCodeTraitIsUpdated_LocaleShouldHaveExpectedValue()
        {
            // Given.
            var address = testLocale.LocaleAddress.First().Address.PhysicalAddress;
            var originalCityId = address.cityID;
            var originalCountyId = address.City.countyID;
            var originalPostalCodeId = address.postalCodeID;
            
            UpdateLocaleCommand setupCommand = new UpdateLocaleCommand
            {
                LocaleId = testLocale.localeID,
                LocaleName = testLocale.localeName,
                OpenDate = testLocale.localeOpenDate,
                CloseDate = testLocale.localeCloseDate,
                BusinessUnitId = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue,
                AddressId = address.addressID,
                CityName = testLocale.LocaleAddress.First().Address.PhysicalAddress.City.cityName,
                PostalCode = address.PostalCode.postalCode,
                CountryId = address.countryID.Value,
                TerritoryId = address.territoryID.Value,
                TimezoneId = address.timezoneID.Value,
                CurrencyCode = CurrencyCodes.Cad
            };
            updateLocaleCommandHandler.Execute(setupCommand);
            var existing = testLocale.LocaleTrait.FirstOrDefault(t => t.traitID == Traits.Currency);
            Assert.AreEqual(CurrencyCodes.Cad, existing.traitValue, "locale should have initially had expected currency trait value");

            // When.
            UpdateLocaleCommand updateCommand = new UpdateLocaleCommand
            {
                LocaleId = testLocale.localeID,
                LocaleName = testLocale.localeName,
                OpenDate = testLocale.localeOpenDate,
                CloseDate = testLocale.localeCloseDate,
                BusinessUnitId = testLocale.LocaleTrait.First(lt => lt.traitID == Traits.PsBusinessUnitId).traitValue,
                AddressId = address.addressID,
                PostalCode = address.PostalCode.postalCode,
                CountryId = address.countryID.Value,
                TerritoryId = address.territoryID.Value,
                TimezoneId = address.timezoneID.Value,
                CurrencyCode = CurrencyCodes.Usd
            };
            updateLocaleCommandHandler.Execute(updateCommand);

            // Then.
            var updated = testLocale.LocaleTrait.FirstOrDefault(t => t.traitID == Traits.Currency);
            Assert.AreEqual(CurrencyCodes.Usd, updated.traitValue, "locale should have updated currency trait value");
        }
    }
}
