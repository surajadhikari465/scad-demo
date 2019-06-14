using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.DataAccess.Commands;

using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Tests.QueueReaders
{
    [TestClass]
    public class LocaleQueueReaderTests
    {
        private LocaleQueueReader queueReader;
        private IconContext context;
        private TransactionScope transaction;
        private Mock<ILogger<LocaleQueueReader>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueLocale>, List<MessageQueueLocale>>> mockGetMessageQueueQuery;
        private GetLocaleLineageQuery getLocaleLineageQuery;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueLocale>>> mockUpdateMessageQueueStatusCommandHandler;
        private string chainName;
        private string regionName;
        private string metroName;
        private string storeName;
		private string venueName;
        private string cityName;
        private string countyName;
        private string territoryName;
        private string territoryCode;
        private string countryName;
        private string countryCode;
        private string addressLine1;
        private string addressLine2;
        private string addressLine3;
        private string postalCodeNumber;
        private string timezoneCode;
        private string timezoneName;
        private string storeAbbreviation;
        private string phoneNumber;
        private string businessUnitId;
		private string venueCode;
		private string venueOccupant;
		private string venueSubType;
		private string currencyCode;
		private Locale chain;
        private Locale region;
        private Locale[] metros;
        private Locale[] stores;
		private Locale[] venues;
        private Country country;
        private County county;
        private City city;
        private PostalCode postalCode;
        private Territory territory;
        private Timezone timezone;
        private string touchPointGroupId;
        private int storeLocaleId;

		

		[TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();

            context = new IconContext();

            chainName = "Test Chain";
            regionName = "Central";
            metroName = "MET_SD";
            storeName = "Hillcrest";
			venueName = "New_Venue";
            cityName = "San Diego";
            countyName = "San Diego County";
            territoryName = "California";
            territoryCode = "CZ";
            countryName = "Whole Foods Nation";
            countryCode = "WFM";
            addressLine1 = "123";
            addressLine2 = "Whole Foods";
            addressLine3 = "Blvd";
            postalCodeNumber = "12345";
            timezoneCode = "CDT";
            timezoneName = "(UTC-06:00) Central Time (US & Canada)";
            storeAbbreviation = "TST";
            phoneNumber = "512-999-9999";
            businessUnitId = "99999";
			venueCode = "1234";
			venueOccupant = "New Occupant";
			venueSubType = "Hospatality";
			currencyCode = "USD";
            touchPointGroupId = "TPG1";
            storeLocaleId = 99999;
            

            mockLogger = new Mock<ILogger<LocaleQueueReader>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueLocale>, List<MessageQueueLocale>>>();
            getLocaleLineageQuery = new GetLocaleLineageQuery(new IconDbContextFactory());
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueLocale>>>();

            BuildTestChain();
            BuildTestRegion();
            BuildTestMetros();
            BuildTestStores();
			BuildTestVenues();
            BuildPhysicalAddressEntities();
            BuildStoreAttributes();
			BuildVenueAttributes();
            queueReader = new LocaleQueueReader(
                mockLogger.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                getLocaleLineageQuery,
                mockUpdateMessageQueueStatusCommandHandler.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

		private void BuildVenueAttributes()
		{
			foreach (var venue in venues)
			{
				var venueCodeTrait = new LocaleTrait
				{
					localeID = venue.localeID,
					traitID = Traits.VenueCode,
					traitValue = venueCode
				};

				context.LocaleTrait.Add(venueCodeTrait);

				var venueOccupantTrait = new LocaleTrait
				{
					localeID = venue.localeID,
					traitID = Traits.VenueOccupant,
					traitValue = venueOccupant
				};

				context.LocaleTrait.Add(venueOccupantTrait);

				var venueSubTypeTrait = new LocaleTrait
				{
					localeID = venue.localeID,
					traitID = Traits.LocaleSubtype,
					traitValue = venueSubType
				};

                var touchPointGroupIdTrait = new LocaleTrait
                {
                    localeID = venue.localeID,
                    traitID = Traits.TouchPointGroupId,
                    traitValue = touchPointGroupId
                };

				context.LocaleTrait.Add(venueSubTypeTrait);
				context.SaveChanges();
			}
		}

		private void BuildStoreAttributes()
        {
            foreach (var store in stores)
            {
                var storeAbbreviationTrait = new LocaleTrait
                {
                    localeID = store.localeID,
                    traitID = Traits.StoreAbbreviation,
                    traitValue = storeAbbreviation
                };

                context.LocaleTrait.Add(storeAbbreviationTrait);

                var businessUnitIdTrait = new LocaleTrait
                {
                    localeID = store.localeID,
                    traitID = Traits.PsBusinessUnitId,
                    traitValue = businessUnitId
                };

				context.LocaleTrait.Add(businessUnitIdTrait);

				var currencyCodeTrait = new LocaleTrait
				{
					localeID = store.localeID,
					traitID = Traits.CurrencyCode,
					traitValue = currencyCode
				};

				context.LocaleTrait.Add(currencyCodeTrait);

                var phoneNumberTrait = new LocaleTrait
                {
                    localeID = store.localeID,
                    traitID = Traits.PhoneNumber,
                    traitValue = phoneNumber
                };

                context.LocaleTrait.Add(phoneNumberTrait);

                var address = new Address
                {
                    addressTypeID = AddressTypes.PhysicalAddress
                };

                context.Address.Add(address);
                context.SaveChanges();

                var localeAddress = new LocaleAddress
                {
                    addressID = address.addressID,
                    localeID = store.localeID,
                    AddressUsage = context.AddressUsage.Single(au => au.addressUsageID == AddressUsages.Shipping)
                };

                context.LocaleAddress.Add(localeAddress);

                var physicalAddress = new PhysicalAddress
                {
                    addressID = address.addressID,                    
                    addressLine1 = addressLine1,
                    addressLine2 = addressLine2,
                    addressLine3 = addressLine3,
                    cityID = city.cityID,
                    countryID = country.countryID,
                    postalCodeID = postalCode.postalCodeID,
                    territoryID = territory.territoryID,
                    timezoneID = timezone.timezoneID
                };

                context.PhysicalAddress.Add(physicalAddress);
                context.SaveChanges();
            }
        }
        
        private void BuildPhysicalAddressEntities()
        {
            country = new Country
            {
                countryCode = countryCode,
                countryName = countryName,
            };

            context.Country.Add(country);
            context.SaveChanges();

            territory = new Territory
            {
                territoryName = territoryName,
                territoryCode = territoryCode,
                countryID = country.countryID
            };

            context.Territory.Add(territory);
            context.SaveChanges();

            county = new County
            {
                countyName = countyName,
                territoryID = territory.territoryID
            };

            context.County.Add(county);
            context.SaveChanges();

            city = new City
            {
                countyID = county.countyID,
                territoryID = territory.territoryID,
                cityName = cityName
            };

            context.City.Add(city);
            context.SaveChanges();

            postalCode = new PostalCode
            {
                countryID = country.countryID,
                countyID = county.countyID,
                postalCode = postalCodeNumber
            };

            context.PostalCode.Add(postalCode);
            context.SaveChanges();

            timezone = new Timezone
            {
                timezoneCode = timezoneCode,
                timezoneName = timezoneName
            };

            context.Timezone.Add(timezone);
            context.SaveChanges();
        }

		private void BuildTestVenues()
		{
			venues = new Locale[3]
			{
				new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Venue).WithLocaleName(venueName+"1").WithParentLocaleId(stores[0].localeID),
				new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Venue).WithLocaleName(venueName+"2").WithParentLocaleId(stores[0].localeID),
				new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Venue).WithLocaleName(venueName+"3").WithParentLocaleId(stores[0].localeID)
			};

			context.Locale.AddRange(venues);
			context.SaveChanges();
		}

		private void BuildTestStores()
        {
            stores = new Locale[9]
            {
                new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Store).WithLocaleName(storeName+"1").WithParentLocaleId(metros[0].localeID),
                new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Store).WithLocaleName(storeName+"2").WithParentLocaleId(metros[0].localeID),
                new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Store).WithLocaleName(storeName+"3").WithParentLocaleId(metros[0].localeID),

                new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Store).WithLocaleName(storeName+"4").WithParentLocaleId(metros[1].localeID),
                new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Store).WithLocaleName(storeName+"5").WithParentLocaleId(metros[1].localeID),
                new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Store).WithLocaleName(storeName+"6").WithParentLocaleId(metros[1].localeID),

                new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Store).WithLocaleName(storeName+"7").WithParentLocaleId(metros[2].localeID),
                new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Store).WithLocaleName(storeName+"8").WithParentLocaleId(metros[2].localeID),
                new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Store).WithLocaleName(storeName+"9").WithParentLocaleId(metros[2].localeID)
            };

            context.Locale.AddRange(stores);
            context.SaveChanges();
        }

        private void BuildTestMetros()
        {
            metros = new Locale[3]
            {
                new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Metro).WithLocaleName(metroName+"1").WithParentLocaleId(this.region.localeID),
                new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Metro).WithLocaleName(metroName+"2").WithParentLocaleId(this.region.localeID),
                new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Metro).WithLocaleName(metroName+"3").WithParentLocaleId(this.region.localeID)
            };

            context.Locale.AddRange(metros);
            context.SaveChanges();
        }

        private void BuildTestRegion()
        {
            region = new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Region).WithLocaleName(regionName).WithParentLocaleId(this.chain.localeID);
            context.Locale.Add(region);
            context.SaveChanges();
        }

        private void BuildTestChain()
        {
            chain = new TestLocaleBuilder().WithLocaleTypeId(LocaleTypes.Chain).WithLocaleName(chainName);
            context.Locale.Add(chain);
            context.SaveChanges();
        }

        [TestMethod]
        public void GroupLocaleMessages_InvalidArgument_ShouldThrowException()
        {
            // Given.
            int exceptionCount = 0;

            // When.
            var messages = new List<MessageQueueLocale>();

            try { queueReader.GroupMessagesForMiniBulk(messages); }
            catch (Exception) { exceptionCount++; }

            messages = null;

            try { queueReader.GroupMessagesForMiniBulk(messages); }
            catch (Exception) { exceptionCount++; }

            // Then.
            Assert.AreEqual(2, exceptionCount);
        }

        [TestMethod]
        public void GroupLocaleMessages_OneMessage_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder()
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueLocales);

            // Then.
            Assert.AreEqual(1, messages.Count);
        }

        [TestMethod]
        public void GroupLocaleMessages_TwoMessages_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder(),
                new TestLocaleMessageBuilder()
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueLocales);

            // Then.
            Assert.AreEqual(1, messages.Count);
        }

        [TestMethod]
        public void GetLocaleMiniBulk_InvalidArguments_ExceptionShouldBeThrown()
        {
            // Given.
            var messages = new List<MessageQueueLocale>();
            int caughtExceptions = 0;

            // When.
            try { var miniBulk = queueReader.BuildMiniBulk(messages); }
            catch (ArgumentException) { caughtExceptions++; }

            messages = null;
            try { var miniBulk = queueReader.BuildMiniBulk(messages); }
            catch (ArgumentException) { caughtExceptions++; }

            // Then.
            int expectedExceptions = 2;
            Assert.AreEqual(expectedExceptions, caughtExceptions);
        }
        
        [TestMethod]
        public void GetLocaleMiniBulk_RegionMessage_MiniBulkShouldContainMetroAndStoreElements()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Region)
            };

            fakeMessageQueueLocales[0].LocaleId = this.region.localeID;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

            // Then.
            var region = miniBulk.locales[0];
            var firstMetro = miniBulk.locales[0].locales[0];
            var firstStore = miniBulk.locales[0].locales[0].locales[0];

            Assert.IsNotNull(region);
            Assert.IsNotNull(firstMetro);
            Assert.IsNotNull(firstStore);
        }

        [TestMethod]
        public void GetLocaleMiniBulk_RegionMessage_TopLevelChainElementShouldContainAllRequiredInformation()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Region)
            };

            fakeMessageQueueLocales[0].LocaleId = this.region.localeID;

            // When.
            var chainLocaleMessage = queueReader.BuildMiniBulk(fakeMessageQueueLocales).locales[0];

            // Then.
            var action = chainLocaleMessage.Action;
            var actionSpecified = chainLocaleMessage.ActionSpecified;
            var localeId = chainLocaleMessage.id;
            var localeName = chainLocaleMessage.name;
            var localeType = chainLocaleMessage.type;
            var localeTypeCode = localeType.code;
            var localeTypeDesc = localeType.description;
            var localeTraits = chainLocaleMessage.traits;
            var localeAddress = chainLocaleMessage.addresses;

            Assert.AreEqual(Contracts.ActionEnum.Inherit.ToString(), action.ToString());
            Assert.IsTrue(actionSpecified);
            Assert.AreEqual(localeId, chain.localeID.ToString());
            Assert.AreEqual(localeName, chain.localeName);
            Assert.IsNotNull(localeType);
            Assert.AreEqual(Contracts.LocaleCodeType.CHN, localeTypeCode);
            Assert.AreEqual(Contracts.LocaleDescType.Chain, localeTypeDesc);
            Assert.IsNull(localeTraits);
            Assert.IsNull(localeAddress);
        }

        [TestMethod]
        public void GetLocaleMiniBulk_RegionMessage_RegionElementShouldContainAllRequiredInformation()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Region)
            };

            fakeMessageQueueLocales[0].LocaleId = this.region.localeID;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

            // Then.
            var region = miniBulk.locales[0].locales[0];

            var action = region.Action;
            var actionSpecified = region.ActionSpecified;
            var localeId = region.id;
            var localeName = region.name;
            var localeType = region.type;
            var localeTypeCode = localeType.code;
            var localeTypeDesc = localeType.description;
            var localeTraits = region.traits;
            var localeAddress = region.addresses;

            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate.ToString(), action.ToString());
            Assert.IsTrue(actionSpecified);
            Assert.AreEqual(localeId, fakeMessageQueueLocales[0].LocaleId.ToString());
            Assert.AreEqual(localeName, regionName);
            Assert.IsNotNull(localeType);
            Assert.AreEqual(Contracts.LocaleCodeType.REG, localeTypeCode);
            Assert.AreEqual(Contracts.LocaleDescType.Region, localeTypeDesc);
            Assert.IsNull(localeTraits);
            Assert.IsNull(localeAddress);
        }

        [TestMethod]
        public void GetLocaleMiniBulk_RegionMessage_MetroElementShouldContainAllRequiredInformation()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Region)
            };

            fakeMessageQueueLocales[0].LocaleId = this.region.localeID;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

            // Then.
            var firstMetro = miniBulk.locales[0].locales[0].locales[0];

            var action = firstMetro.Action;
            var actionSpecified = firstMetro.ActionSpecified;
            var localeId = firstMetro.id;
            var localeName = firstMetro.name;
            var localeType = firstMetro.type;
            var localeTypeCode = localeType.code;
            var localeTypeDesc = localeType.description;
            var localeTraits = firstMetro.traits;
            var localeAddress = firstMetro.addresses;

            Assert.AreEqual(Contracts.ActionEnum.Inherit.ToString(), action.ToString());
            Assert.IsTrue(actionSpecified);
            Assert.AreEqual(localeId, metros[0].localeID.ToString());
            Assert.AreEqual(localeName, metros[0].localeName);
            Assert.IsNotNull(localeType);
            Assert.AreEqual(Contracts.LocaleCodeType.MTR, localeTypeCode);
            Assert.AreEqual(Contracts.LocaleDescType.Metro, localeTypeDesc);
            Assert.IsNull(localeTraits);
            Assert.IsNull(localeAddress);
        }

        [TestMethod]
        public void GetLocaleMiniBulk_RegionMessage_StoreElementShouldContainAllRequiredInformation()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Region)
            };

            fakeMessageQueueLocales[0].LocaleId = this.region.localeID;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

            // Then.
            var firstStore = miniBulk.locales[0].locales[0].locales[0].locales[0];

            var action = firstStore.Action;
            var actionSpecified = firstStore.ActionSpecified;
            var localeId = firstStore.id;
            var localeName = firstStore.name;
            var localeType = firstStore.type;
            var localeTypeCode = localeType.code;
            var localeTypeDesc = localeType.description;
            var localeTraits = firstStore.traits;
            var storeAbbreviation = localeTraits[0].type.value[0].value;
            var phoneNumber = localeTraits[1].type.value[0].value;
			var currencyCode = localeTraits[2].type.value[0].value;
            var localeAddress = firstStore.addresses;
            var addressId = localeAddress[0].id;
            var addressUsageCode = localeAddress[0].usage.code;
            var addressLine1 = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).addressLine1;
            var addressLine2 = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).addressLine2;
            var addressLine3 = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).addressLine3;
            var cityName = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).cityType.name;
            var territoryCode = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).territoryType.code;
            var territoryName = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).territoryType.name;
            var countryCode = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).country.code;
            var countryName = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).country.name;
            var postalCode = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).postalCode;
            var timezoneName = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).timezone.name;

            Assert.AreEqual(Contracts.ActionEnum.Inherit.ToString(), action.ToString());
            Assert.IsTrue(actionSpecified);
            Assert.AreEqual(localeId, businessUnitId);
            Assert.AreEqual(localeName, stores[0].localeName);
            Assert.IsNotNull(localeType);
            Assert.AreEqual(Contracts.LocaleCodeType.STR, localeTypeCode);
            Assert.AreEqual(Contracts.LocaleDescType.Store, localeTypeDesc);
            Assert.AreEqual(3, localeTraits.Length);
            Assert.AreEqual(storeAbbreviation, storeAbbreviation);
            Assert.AreEqual(phoneNumber, phoneNumber);
			Assert.AreEqual(currencyCode, currencyCode);
            Assert.AreEqual(addressId, stores[0].LocaleAddress.Single().addressID);
            Assert.AreEqual(addressUsageCode, stores[0].LocaleAddress.Single().AddressUsage.addressUsageCode);
            Assert.AreEqual(addressLine1, (addressLine1 + " " + addressLine2 + " " + addressLine3).Trim());
            Assert.AreEqual(cityName, city.cityName);
            Assert.AreEqual(territoryCode, territory.territoryCode);
            Assert.AreEqual(territoryName, territory.territoryName);
            Assert.AreEqual(countryCode, country.countryCode);
            Assert.AreEqual(countryName, country.countryName);
            Assert.AreEqual(postalCode, postalCodeNumber);
            Assert.AreEqual(timezoneName.ToString(), Contracts.TimezoneNameType.USCentral.ToString());
        }

        [TestMethod]
        public void GetLocaleMiniBulk_MetroMessage_MiniBulkShouldContainMetroAndRegionElements()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Metro)
            };

            fakeMessageQueueLocales[0].LocaleId = metros[0].localeID;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

            // Then.
            var region = miniBulk.locales[0];
            var metro = miniBulk.locales[0].locales[0];

            Assert.IsNotNull(region);
            Assert.IsNotNull(metro);
        }

        [TestMethod]
        public void GetLocaleMiniBulk_MetroMessage_TopLevelChainElementShouldContainAllRequiredInformation()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Metro)
            };

            fakeMessageQueueLocales[0].LocaleId = metros[0].localeID;

            // When.
            var chainLocaleMessage = queueReader.BuildMiniBulk(fakeMessageQueueLocales).locales[0];

            // Then.
            var action = chainLocaleMessage.Action;
            var actionSpecified = chainLocaleMessage.ActionSpecified;
            var localeId = chainLocaleMessage.id;
            var localeName = chainLocaleMessage.name;
            var localeType = chainLocaleMessage.type;
            var localeTypeCode = localeType.code;
            var localeTypeDesc = localeType.description;
            var localeTraits = chainLocaleMessage.traits;
            var localeAddress = chainLocaleMessage.addresses;

            Assert.AreEqual(Contracts.ActionEnum.Inherit.ToString(), action.ToString());
            Assert.IsTrue(actionSpecified);
            Assert.AreEqual(localeId, chain.localeID.ToString());
            Assert.AreEqual(localeName, chainName);
            Assert.IsNotNull(localeType);
            Assert.AreEqual(Contracts.LocaleCodeType.CHN, localeTypeCode);
            Assert.AreEqual(Contracts.LocaleDescType.Chain, localeTypeDesc);
            Assert.IsNull(localeTraits);
            Assert.IsNull(localeAddress);
        }

        [TestMethod]
        public void GetLocaleMiniBulk_MetroMessage_RegionElementShouldContainAllRequiredInformation()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Metro)
            };

            fakeMessageQueueLocales[0].LocaleId = metros[0].localeID;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

            // Then.
            var region = miniBulk.locales[0].locales[0];
            var action = region.Action;
            var actionSpecified = region.ActionSpecified;
            var localeId = region.id;
            var localeName = region.name;
            var localeType = region.type;
            var localeTypeCode = localeType.code;
            var localeTypeDesc = localeType.description;
            var localeTraits = region.traits;
            var localeAddress = region.addresses;

            Assert.AreEqual(Contracts.ActionEnum.Inherit.ToString(), action.ToString());
            Assert.IsTrue(actionSpecified);
            Assert.AreEqual(localeId, this.region.localeID.ToString());
            Assert.AreEqual(localeName, this.region.localeName);
            Assert.IsNotNull(localeType);
            Assert.AreEqual(Contracts.LocaleCodeType.REG, localeTypeCode);
            Assert.AreEqual(Contracts.LocaleDescType.Region, localeTypeDesc);
            Assert.IsNull(localeTraits);
            Assert.IsNull(localeAddress);
        }

        [TestMethod]
        public void GetLocaleMiniBulk_MetroMessage_MetroElementShouldContainAllRequiredInformation()
        {
            // Given.
            var metroLocaleId = metros[0].localeID;
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Metro)
            };
            fakeMessageQueueLocales[0].LocaleId = metroLocaleId;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

            // Then.
            var metro = miniBulk.locales[0].locales[0].locales[0];
            var action = metro.Action;
            var actionSpecified = metro.ActionSpecified;
            var localeId = metro.id;
            var localeName = metro.name;
            var localeType = metro.type;
            var localeTypeCode = localeType.code;
            var localeTypeDesc = localeType.description;
            var localeTraits = metro.traits;
            var localeAddress = metro.addresses;
            var locales = metro.locales;
            var storeCount = context.Locale.Count(l => l.parentLocaleID == metroLocaleId);

            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate.ToString(), action.ToString());
            Assert.IsTrue(actionSpecified);
            Assert.AreEqual(localeId, metros[0].localeID.ToString());
            Assert.AreEqual(localeName, metros[0].localeName);
            Assert.IsNotNull(localeType);
            Assert.AreEqual(Contracts.LocaleCodeType.MTR, localeTypeCode);
            Assert.AreEqual(Contracts.LocaleDescType.Metro, localeTypeDesc);
            Assert.IsNull(localeTraits);
            Assert.IsNull(localeAddress);
            Assert.AreEqual(storeCount, locales.Length);
        }

        [TestMethod]
        public void GetLocaleMiniBulk_StoreMessage_MiniBulkShouldContainStoreAndMetroAndRegionElements()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Store)
            };

            fakeMessageQueueLocales[0].LocaleId = stores[0].localeID;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

            // Then.
            var region = miniBulk.locales[0];
            var metro = region.locales[0];
            var store = metro.locales[0];

            Assert.IsNotNull(region);
            Assert.IsNotNull(metro);
            Assert.IsNotNull(store);
        }


        [TestMethod]
        public void GetLocaleMiniBulk_StoreMessage_MiniBulkShouldContainStoreAndMetroAndRegionElementsAndIncludeIconStoreType()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Store)
            };

            fakeMessageQueueLocales[0].LocaleId = stores[0].localeID;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

            // Then.
            var chn = miniBulk.locales[0];
            var reg = chn.locales[0];
            var metro = reg.locales[0];
            var store = metro.locales[0];
            var storeType = store.store;

            Assert.IsNotNull(chn);
            Assert.IsNotNull(reg);
            Assert.IsNotNull(metro);
            Assert.IsNotNull(store);
            Assert.IsNotNull(storeType);
            Assert.IsTrue(int.Parse(storeType.id)>0); // storeType.Id will be icon locale id.
        }

        [TestMethod]
        public void GetLocaleMiniBulk_StoreMessage_TopLevelChainElementShouldContainAllRequiredInformation()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Store)
            };

            fakeMessageQueueLocales[0].LocaleId = stores[0].localeID;

            // When.
            var chainLocaleMessage = queueReader.BuildMiniBulk(fakeMessageQueueLocales).locales[0];

            // Then.
            var action = chainLocaleMessage.Action;
            var actionSpecified = chainLocaleMessage.ActionSpecified;
            var localeId = chainLocaleMessage.id;
            var localeName = chainLocaleMessage.name;
            var localeType = chainLocaleMessage.type;
            var localeTypeCode = localeType.code;
            var localeTypeDesc = localeType.description;
            var localeTraits = chainLocaleMessage.traits;
            var localeAddress = chainLocaleMessage.addresses;

            Assert.AreEqual(Contracts.ActionEnum.Inherit.ToString(), action.ToString());
            Assert.IsTrue(actionSpecified);
            Assert.AreEqual(localeId, chain.localeID.ToString());
            Assert.AreEqual(localeName, chain.localeName);
            Assert.IsNotNull(localeType);
            Assert.AreEqual(Contracts.LocaleCodeType.CHN, localeTypeCode);
            Assert.AreEqual(Contracts.LocaleDescType.Chain, localeTypeDesc);
            Assert.IsNull(localeTraits);
            Assert.IsNull(localeAddress);
        }

        [TestMethod]
        public void GetLocaleMiniBulk_StoreMessage_RegionElementShouldContainAllRequiredInformation()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Store)
            };

            fakeMessageQueueLocales[0].LocaleId = stores[0].localeID;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

            // Then.
            var region = miniBulk.locales[0].locales[0];
            var action = region.Action;
            var actionSpecified = region.ActionSpecified;
            var localeId = region.id;
            var localeName = region.name;
            var localeType = region.type;
            var localeTypeCode = localeType.code;
            var localeTypeDesc = localeType.description;
            var localeTraits = region.traits;
            var localeAddress = region.addresses;

            Assert.AreEqual(Contracts.ActionEnum.Inherit.ToString(), action.ToString());
            Assert.IsTrue(actionSpecified);
            Assert.AreEqual(localeId, this.region.localeID.ToString());
            Assert.AreEqual(localeName, this.region.localeName);
            Assert.IsNotNull(localeType);
            Assert.AreEqual(Contracts.LocaleCodeType.REG, localeTypeCode);
            Assert.AreEqual(Contracts.LocaleDescType.Region, localeTypeDesc);
            Assert.IsNull(localeTraits);
            Assert.IsNull(localeAddress);
        }

        [TestMethod]
        public void GetLocaleMiniBulk_StoreMessage_MetroElementShouldContainAllRequiredInformation()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Store)
            };

            fakeMessageQueueLocales[0].LocaleId = stores[0].localeID;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

            // Then.
            var metro = miniBulk.locales[0].locales[0].locales[0];
            var action = metro.Action;
            var actionSpecified = metro.ActionSpecified;
            var localeId = metro.id;
            var localeName = metro.name;
            var localeType = metro.type;
            var localeTypeCode = localeType.code;
            var localeTypeDesc = localeType.description;
            var localeTraits = metro.traits;
            var localeAddress = metro.addresses;
            var locales = metro.locales;
            
            Assert.AreEqual(Contracts.ActionEnum.Inherit.ToString(), action.ToString());
            Assert.IsTrue(actionSpecified);
            Assert.AreEqual(localeId, metros[0].localeID.ToString());
            Assert.AreEqual(localeName, metros[0].localeName);
            Assert.IsNotNull(localeType);
            Assert.AreEqual(Contracts.LocaleCodeType.MTR, localeTypeCode);
            Assert.AreEqual(Contracts.LocaleDescType.Metro, localeTypeDesc);
            Assert.IsNull(localeTraits);
            Assert.IsNull(localeAddress);
            Assert.AreEqual(1, locales.Length);
        }

        [TestMethod]
        public void GetLocaleMiniBulk_StoreMessage_StoreElementShouldContainAllRequiredInformation()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Store)
            };

            fakeMessageQueueLocales[0].LocaleId = stores[0].localeID;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

            // Then.
            var store = miniBulk.locales[0].locales[0].locales[0].locales[0];
            var action = store.Action;
            var actionSpecified = store.ActionSpecified;
            var localeId = store.id;
            var localeName = store.name;
            var localeType = store.type;
            var localeTypeCode = localeType.code;
            var localeTypeDesc = localeType.description;
            var localeTraits = store.traits;
            var storeAbbreviation = localeTraits[0].type.value[0].value;
            var phoneNumber = localeTraits[1].type.value[0].value;
			var currencyCode = localeTraits[2].type.value[0].value;
			var localeAddress = store.addresses;
            var addressId = localeAddress[0].id;
            var addressUsageCode = localeAddress[0].usage.code;
            var addressLine1 = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).addressLine1;
            var addressLine2 = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).addressLine2;
            var addressLine3 = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).addressLine3;
            var cityName = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).cityType.name;
            var territoryCode = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).territoryType.code;
            var territoryName = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).territoryType.name;
            var countryCode = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).country.code;
            var countryName = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).country.name;
            var postalCode = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).postalCode;
            var timezoneName = (localeAddress[0].type.Item as Contracts.PhysicalAddressType).timezone.name;
            var locales = store.locales;

            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate.ToString(), action.ToString());
            Assert.IsTrue(actionSpecified);
            Assert.AreEqual(localeId, businessUnitId);
            Assert.AreEqual(localeName, stores[0].localeName);
            Assert.IsNotNull(localeType);
            Assert.AreEqual(Contracts.LocaleCodeType.STR, localeTypeCode);
            Assert.AreEqual(Contracts.LocaleDescType.Store, localeTypeDesc);
            Assert.AreEqual(3, localeTraits.Length);
            Assert.AreEqual(storeAbbreviation, storeAbbreviation);
            Assert.AreEqual(phoneNumber, phoneNumber);
			Assert.AreEqual(currencyCode, currencyCode);
			Assert.AreEqual(addressId, stores[0].LocaleAddress.Single().addressID);
            Assert.AreEqual(addressUsageCode, stores[0].LocaleAddress.Single().AddressUsage.addressUsageCode);
            Assert.AreEqual(addressLine1, (addressLine1 + " " + addressLine2 + " " + addressLine3).Trim());
            Assert.AreEqual(cityName, city.cityName);
            Assert.AreEqual(territoryCode, territory.territoryCode);
            Assert.AreEqual(territoryName, territory.territoryName);
            Assert.AreEqual(countryCode, country.countryCode);
            Assert.AreEqual(countryName, country.countryName);
            Assert.AreEqual(postalCode, postalCodeNumber);
            Assert.AreEqual(timezoneName.ToString(), Contracts.TimezoneNameType.USCentral.ToString());
            Assert.IsNull(locales);
        }

		[TestMethod]
		public void GetLocaleMiniBulk_VenueMessage_MiniBulkShouldContainVenueElelmets()
		{
			// Given.
			var fakeMessageQueueLocales = new List<MessageQueueLocale>
			{
				new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Venue)
			};

			fakeMessageQueueLocales[0].LocaleId = venues[0].localeID;

			// When.
			var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

			// Then.
			var region = miniBulk.locales[0];
			var metro = region.locales[0];
			var store = metro.locales[0];
			var venue = store.locales[0];

			Assert.IsNotNull(region);
			Assert.IsNotNull(metro);
			Assert.IsNotNull(store);
			Assert.IsNotNull(venue);
		}

		[TestMethod]
		public void GetLocaleMiniBulk_VenueMessage_RegionElementShouldContainAllRequiredInformation()
		{
			//Given
			var fakeMessageQueueLocales = new List<MessageQueueLocale>
			{
				new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Venue)
			};

			fakeMessageQueueLocales[0].LocaleId = venues[0].localeID;

			// When.
			var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

			// Then.
			var region = miniBulk.locales[0].locales[0];
			var action = region.Action;
			var actionSpecified = region.ActionSpecified;
			var localeId = region.id;
			var localeName = region.name;
			var localeType = region.type;
			var localeTypeCode = localeType.code;
			var localeTypeDesc = localeType.description;
			var localeTraits = region.traits;

			Assert.AreEqual(Contracts.ActionEnum.Inherit.ToString(), action.ToString());
			Assert.IsTrue(actionSpecified);
			Assert.AreEqual(localeId, this.region.localeID.ToString());
			Assert.AreEqual(localeName, this.region.localeName);
			Assert.IsNotNull(localeType);
			Assert.AreEqual(Contracts.LocaleCodeType.REG, localeTypeCode);
			Assert.AreEqual(Contracts.LocaleDescType.Region, localeTypeDesc);
			Assert.IsNull(localeTraits);
		}

		[TestMethod]
		public void GetLocaleMiniBulk_VenueMessage_MetroElementShouldContainAllRequiredInformation()
		{
			// Given.
			var fakeMessageQueueLocales = new List<MessageQueueLocale>
			{
				new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Venue)
			};

			fakeMessageQueueLocales[0].LocaleId = venues[0].localeID;

			// When.
			var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

			// Then.
			var metro = miniBulk.locales[0].locales[0].locales[0];
			var action = metro.Action;
			var actionSpecified = metro.ActionSpecified;
			var localeId = metro.id;
			var localeName = metro.name;
			var localeType = metro.type;
			var localeTypeCode = localeType.code;
			var localeTypeDesc = localeType.description;
			var localeTraits = metro.traits;
			var localeAddress = metro.addresses;
			var locales = metro.locales;


			Assert.AreEqual(Contracts.ActionEnum.Inherit.ToString(), action.ToString());
			Assert.IsTrue(actionSpecified);
			Assert.AreEqual(localeId, metros[0].localeID.ToString());
			Assert.AreEqual(localeName, metros[0].localeName);
			Assert.IsNotNull(localeType);
			Assert.AreEqual(Contracts.LocaleCodeType.MTR, localeTypeCode);
			Assert.AreEqual(Contracts.LocaleDescType.Metro, localeTypeDesc);
			Assert.IsNull(localeTraits);
			Assert.AreEqual(1, locales.Length);
		}

		[TestMethod]
		public void GetLocaleMiniBulk_VenueMessage_StoreElementShouldContainAllRequiredInformation()
		{
			// Given.
			var fakeMessageQueueLocales = new List<MessageQueueLocale>
			{
				new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Venue)
			};

			fakeMessageQueueLocales[0].LocaleId = venues[0].localeID;

			// When.
			var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

			// Then.
			var store = miniBulk.locales[0].locales[0].locales[0].locales[0];
			var action = store.Action;
			var actionSpecified = store.ActionSpecified;
			var localeId = store.store.id;
			var localeName = store.name;
			var localeType = store.type;
			var localeTypeCode = localeType.code;
			var localeTypeDesc = localeType.description;
			var localeTraits = store.traits;
			var localeAddress = store.addresses;
			var locales = store.locales;


			Assert.AreEqual(Contracts.ActionEnum.Inherit.ToString(), action.ToString());
			Assert.IsTrue(actionSpecified);
			Assert.AreEqual(localeId, stores[0].localeID.ToString());
			Assert.AreEqual(localeName, stores[0].localeName);
			Assert.IsNotNull(localeType);
			Assert.AreEqual(Contracts.LocaleCodeType.STR, localeTypeCode);
			Assert.AreEqual(Contracts.LocaleDescType.Store, localeTypeDesc);
			


		}

		[TestMethod]
		public void GetLocaleMiniBulk_VenueMessage_VenueElelmentShouldContainAllRequiredInformation()
		{
			// Given.
			var fakeMessageQueueLocales = new List<MessageQueueLocale>
			{
				new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Venue)
			};

			fakeMessageQueueLocales[0].LocaleId = venues[0].localeID;

			// When.
			var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

			// Then.
			var venue = miniBulk.locales[0].locales[0].locales[0].locales[0].locales[0];
            var store = miniBulk.locales[0].locales[0].locales[0].locales[0];
            var action = venue.Action;
			var actionSpecified = venue.ActionSpecified;
			var localeId = venue.id;
			var localeName = venue.name;
			var localeType = venue.type;
			var localeTypeCode = localeType.code;
			var localeTypeDesc = localeType.description;
			var localeTraits = venue.traits;
			var locales = venue.locales;

			Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate.ToString(), action.ToString());
			Assert.IsTrue(actionSpecified);
			Assert.AreEqual(localeName, venues[0].localeName);
			Assert.IsNotNull(localeType);
			Assert.AreEqual(Contracts.LocaleCodeType.VNU, localeTypeCode);
			Assert.AreEqual(Contracts.LocaleDescType.Venue, localeTypeDesc);
            Assert.AreEqual(businessUnitId, store.id);
			Assert.AreEqual(4, localeTraits.Length);
			Assert.IsNull(locales);
		}

        [TestMethod]
        public void GetLocaleMiniBulk_StoreMessageWithNullCloseDate_ReturnesStoreElementWithMinDateValue()
        {
            // Given.
            var fakeMessageQueueLocales = new List<MessageQueueLocale>
            {
                new TestLocaleMessageBuilder().WithLocaleTypeId(LocaleTypes.Store)
            };
            fakeMessageQueueLocales[0].LocaleId = stores[0].localeID;
            fakeMessageQueueLocales[0].LocaleCloseDate = null;

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueLocales);

            // Then.
            var store = miniBulk.locales[0].locales[0].locales[0].locales[0];
            Assert.AreEqual(DateTime.MinValue, store.closeDate);
            //Assert.IsNull(store.closeDate);
        }
    }
}
