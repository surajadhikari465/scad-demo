using AutoMapper;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class AddLocaleMessageCommandHandlerTests
    {
        private AddLocaleMessageCommandHandler commandHandler;

        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new AddLocaleMessageCommandHandler(context);
            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            context.Dispose();
        }

        [TestMethod]
        public void AddLocaleMessage_LocaleHasAllRequiredProperties_MessageCreated()
        {
            // Given
            AutoMapperWebConfiguration.Configure();
            Locale locale = new Locale
            {
                localeID = 12345,
                localeCloseDate = DateTime.Now,
                localeOpenDate = DateTime.Now,
                localeName = "MessageGenerator Test Locale",
                ownerOrgPartyID = 12345,
                parentLocaleID = 12345,
                localeTypeID = LocaleTypes.Store,
                LocaleTrait = new List<LocaleTrait> 
                { 
                    new LocaleTrait 
                    {
                        Trait = context.Trait.First(t => t.traitCode == TraitCodes.PsBusinessUnitId),
                        traitID = context.Trait.First(t => t.traitCode == TraitCodes.PsBusinessUnitId).traitID,
                        localeID = 12345,
                        traitValue = "MessageGenerator Test Business Value Id"
                    },
                    new LocaleTrait
                    {
                        Trait = context.Trait.First(t => t.traitCode == TraitCodes.StoreAbbreviation),
                        traitID = context.Trait.First(t => t.traitCode == TraitCodes.StoreAbbreviation).traitID,
                        localeID = 12345,
                        traitValue = "TSA"
                    },
                    new LocaleTrait
                    {
                        Trait = context.Trait.First(t => t.traitCode == TraitCodes.PhoneNumber),
                        traitID = context.Trait.First(t => t.traitCode == TraitCodes.PhoneNumber).traitID,
                        localeID = 12345,
                        traitValue = "123-456-7890"
                    }
                },
                LocaleAddress = new List<LocaleAddress>
                {
                    new LocaleAddress 
                    {
                        addressID = 678,
                        AddressUsage = new AddressUsage
                        {
                            addressUsageCode = "TST"
                        },
                        Address = new Address 
                        {
                            PhysicalAddress = new PhysicalAddress
                            {
                                addressLine1 = "Test AddressLine1",
                                addressLine2 = "Test AddressLine2",
                                addressLine3 = "Test AddressLine3",
                                City = new City { cityName = "Test CityName"},
                                Country = new Country { countryName = "Test CountryName", countryCode = "TCC"},
                                latitude = (decimal)33.3333333,
                                longitude = (decimal)44.444444,
                                PostalCode = new PostalCode { postalCode = "TPC"},
                                Territory = new Territory { territoryCode = "TTT", territoryName = "Test Territory" },
                                Timezone = new Timezone { timezoneCode = "TZN", timezoneName = "Test Time zone", posTimeZoneName = "R10 Timezone Name" }
                            }
                        }
                    }
                }
            };

            // When
            commandHandler.Execute(new AddLocaleMessageCommand { Locale = locale });

            // Then
            var message = context.MessageQueueLocale.Single(mql => mql.LocaleId == locale.localeID);

            var expectedAddress = locale.LocaleAddress.First().Address.PhysicalAddress;

            Assert.AreEqual(locale.localeID, message.LocaleId);
            Assert.AreEqual(locale.localeName, message.LocaleName);
            Assert.AreEqual(locale.localeOpenDate, message.LocaleOpenDate);
            Assert.AreEqual(locale.localeTypeID, message.LocaleTypeId);
            Assert.AreEqual(locale.ownerOrgPartyID, message.OwnerOrgPartyId);
            Assert.AreEqual(locale.parentLocaleID, message.ParentLocaleId);
            Assert.AreEqual(
                locale.LocaleTrait.FirstOrDefault(lt => lt.Trait.traitCode == TraitCodes.PsBusinessUnitId).traitValue,
                message.BusinessUnitId);
            Assert.AreEqual(
                locale.LocaleTrait.First(lt => lt.traitID == Traits.StoreAbbreviation).traitValue,
                message.StoreAbbreviation);
            Assert.AreEqual(
                locale.LocaleAddress.First().addressID,
                message.AddressId);
            Assert.AreEqual(
                locale.LocaleAddress.First().AddressUsage.addressUsageCode,
                message.AddressUsageCode);
            Assert.AreEqual(expectedAddress.addressLine1, message.AddressLine1);
            Assert.AreEqual(expectedAddress.addressLine2, message.AddressLine2);
            Assert.AreEqual(expectedAddress.addressLine3, message.AddressLine3);
            Assert.AreEqual(expectedAddress.City.cityName, message.CityName);
            Assert.AreEqual(expectedAddress.Country.countryCode, message.CountryCode);
            Assert.AreEqual(expectedAddress.Country.countryName, message.CountryName);
            Assert.AreEqual(expectedAddress.latitude, Decimal.Parse(message.Latitude));
            Assert.AreEqual(expectedAddress.longitude, Decimal.Parse(message.Longitude));
            Assert.AreEqual(expectedAddress.PostalCode.postalCode, message.PostalCode);
            Assert.AreEqual(expectedAddress.Territory.territoryCode, message.TerritoryCode);
            Assert.AreEqual(expectedAddress.Territory.territoryName, message.TerritoryName);
            Assert.AreEqual(expectedAddress.Timezone.timezoneCode, message.TimezoneCode);
            Assert.AreEqual(expectedAddress.Timezone.posTimeZoneName, message.TimezoneName);
            Assert.AreEqual(locale.LocaleTrait.First(lt => lt.traitID == Traits.PhoneNumber).traitValue,
                message.PhoneNumber);
            Assert.IsNull(message.InProcessBy);
            Assert.IsNull(message.ProcessedDate);

            // Cleanup
            Mapper.Reset();
        }
    }
}
