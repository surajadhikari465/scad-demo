using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.LocaleListener.MessageParsers;
using Icon.Logging;
using Moq;
using Icon.Esb.Subscriber;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Icon.Framework;
using Icon.Infor.Listeners.LocaleListener.Models;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.LocaleListener.Tests.MessageParsers
{
    [TestClass]
    public class LocaleMessageParserTests
    {
        private const string LocaleElementName = "locale";
        private const string IdElementName = "id";
        private const string NameElementName = "name";
        private const string CodeElementName = "code";
        private const string CompanyCode = "CMP";
        private const string CompanyName = "Global";
        private const string TypeElementName = "type";
        private const string AddressLine3ElementName = "addressLine3";
        private const string AddressLine2ElementName = "addressLine2";
        private const string AddressLine1ElementName = "addressLine1";
        private const string CityTypeElementName = "cityType";
        private const string TerrityTypeElementName = "territoryType";
        private const string CountryElementName = "country";
        private const string TimezoneElementName = "timezone";
        private const string PostalCodeElementName = "postalCode";
        private const string TraitElementName = "trait";
        private const string AddressElementName = "address";
        private const string ValueElementName = "value";
        private const string ActionAttributeName = "Action";
        private const string StoreCode = "STR";
        private const string MetroCode = "MTR";
        private const string RegionCode = "REG";
        private const string ChainCode = "CHN";
        private const string PhysicalElementName = "physical";
        private static readonly XNamespace LocaleNamespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/Locale/V2";
        private static readonly XNamespace LocaleTypeNamespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/LocaleType/V2";
        private static readonly XNamespace CommonRefTypesRetailNamespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1";
        private static readonly XNamespace CommonRefTypesLocaleNamespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/CommonRefTypes/V1";
        private static readonly XNamespace AddressNamespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/Address/V2";
        private static readonly XNamespace AddressTypeNamespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/AddressType/V2";
        private static readonly XNamespace PhysicalAddressNamespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/PhysicalAddress/V1";
        private static readonly XNamespace CommonRefTypesAddressNamespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1";
        private static readonly XNamespace TimezoneNamespace = "http://schemas.wfm.com/Enterprise/TimezoneMgmt/Timezone/V2";
        private static readonly XNamespace CountryNamespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/Country/V2";
        private static readonly XNamespace TraitNamespace = "http://schemas.wfm.com/Enterprise/TraitMgmt/Trait/V2";
        private static readonly XNamespace TraitValueNamespace = "http://schemas.wfm.com/Enterprise/TraitMgmt/TraitValue/V2";

        private LocaleMessageParser parser; 
        private Mock<ILogger<LocaleMessageParser>> mockLogger;
        private Mock<IEsbMessage> mockEsbMessage;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<LocaleMessageParser>>();
            parser = new LocaleMessageParser(mockLogger.Object);

            mockEsbMessage = new Mock<IEsbMessage>();
        }

        [TestMethod]
        public void ParseMessage_SingleStoreWithMissingProperties_EntireHierarchyLineageIsReturnedWithOneStoreRecord()
        {
            //Given
            XDocument message = XDocument.Load(@"TestMessages/OneStoreWithMissingTraits.xml");
            mockEsbMessage.SetupGet(m => m.MessageText)
                 .Returns(message.ToString());
            mockEsbMessage.Setup(m => m.GetProperty("SequenceId"))
                .Returns("1");
            mockEsbMessage.Setup(m => m.GetProperty("IconMessageID"))
                .Returns("0E984725-C51C-4BF4-9960-E1C80E27ABA0");
            var result = parser.ParseMessage(mockEsbMessage.Object);

            //Then
            var company = result;
            var chains = company.Locales;
            var regions = chains.SelectMany(l => l.Locales);
            var metros = regions.SelectMany(l => l.Locales);
            var stores = metros.SelectMany(l => l.Locales);

            Assert.AreEqual(1, chains.Count());
            Assert.AreEqual(1, regions.Count());
            Assert.AreEqual(1, metros.Count());
            Assert.AreEqual(1, stores.Count());

            AssertMessageIsEqualToLocale(message, company, chains);
        }

        [TestMethod]
        public void ParseMessage_EntireRegionMessage_ShouldParseAllMetrosAndStoresForTheRegion()
        {
            //Given
            XDocument message = XDocument.Load(@"TestMessages/EntireRegion.xml");
            mockEsbMessage.SetupGet(m => m.MessageText)
                .Returns(message.ToString());
            mockEsbMessage.Setup(m => m.GetProperty("SequenceId"))
                .Returns("1");
            mockEsbMessage.Setup(m => m.GetProperty("IconMessageID"))
                .Returns("0E984725-C51C-4BF4-9960-E1C80E27ABA0");

            //When
            var result = parser.ParseMessage(mockEsbMessage.Object);

            //Then
            var company = result;
            var chains = company.Locales;
            var regions = chains.SelectMany(l => l.Locales);
            var metros = regions.SelectMany(l => l.Locales);
            var stores = metros.SelectMany(l => l.Locales);

            Assert.AreEqual(1, chains.Count());
            Assert.AreEqual(1, regions.Count());
            Assert.AreEqual(6, metros.Count());
            Assert.AreEqual(42, stores.Count());

            AssertMessageIsEqualToLocale(message, company, chains);
        }

        [TestMethod]
        public void ParseMessage_StoreWithAllProperties_ReturnsOneStoreWithAllProperties()
        {
            //Given
            XDocument message = XDocument.Load(@"TestMessages/OneStoreWithAllTraits.xml");
            mockEsbMessage.SetupGet(m => m.MessageText)
                .Returns(message.ToString());
            mockEsbMessage.Setup(m => m.GetProperty("SequenceId"))
                .Returns("1");
            mockEsbMessage.Setup(m => m.GetProperty("IconMessageID"))
                .Returns("0E984725-C51C-4BF4-9960-E1C80E27ABA0");

            //When
            var result = parser.ParseMessage(mockEsbMessage.Object);

            //Then
            var company = result;
            var chains = company.Locales;
            var regions = chains.SelectMany(l => l.Locales);
            var metros = regions.SelectMany(l => l.Locales);
            var stores = metros.SelectMany(l => l.Locales);

            Assert.AreEqual(1, chains.Count());
            Assert.AreEqual(1, regions.Count());
            Assert.AreEqual(1, metros.Count());
            Assert.AreEqual(1, stores.Count());
            Assert.AreEqual(1, stores.First().SequenceId);
            Assert.AreEqual("0E984725-C51C-4BF4-9960-E1C80E27ABA0", stores.First().InforMessageId, true);

           AssertMessageIsEqualToLocale(message, company, chains);
        }

        private void AssertMessageIsEqualToLocale(XDocument message, LocaleModel company, IEnumerable<LocaleModel> chains)
        {
            AssertLocaleIsEqualToMessage(message, CompanyName, CompanyCode, company, null);
            foreach (var chain in chains)
            {
                AssertLocaleIsEqualToMessage(message, chain.Name, ChainCode, chain, company);
                foreach (var region in chain.Locales)
                {
                    AssertLocaleIsEqualToMessage(message, region.Name, RegionCode, region, chain);
                    foreach (var metro in region.Locales)
                    {
                        AssertLocaleIsEqualToMessage(message, metro.Name, MetroCode, metro, region);
                        foreach (var store in metro.Locales)
                        {
                            AssertLocaleIsEqualToMessage(message, store.Name, StoreCode, store, metro);
                        }
                    }
                }
            }
        }

        private static void AssertLocaleIsEqualToMessage(XDocument message, string name, string typeCode, LocaleModel locale, LocaleModel parentLocale)
        {
            var xNameSpace = typeCode == CompanyCode ? LocaleNamespace : CommonRefTypesLocaleNamespace;
            var xmlLocale = message.Descendants(xNameSpace + LocaleElementName)
                            .Single(e =>
                                e.Element(LocaleNamespace + NameElementName).Value == name &&
                                e.Element(LocaleNamespace + TypeElementName).Element(LocaleTypeNamespace + CodeElementName).Value == typeCode);
            var xmlId = xmlLocale.Element(LocaleNamespace + IdElementName).Value;
            var xmlName = xmlLocale.Element(LocaleNamespace + NameElementName).Value;
            var xmlAction = xmlLocale.Attribute(CommonRefTypesRetailNamespace + ActionAttributeName).Value;

            Assert.AreEqual(xmlName, locale.Name);
            Assert.AreEqual(xmlAction, locale.Action.ToString());

            if(typeCode == CompanyCode || typeCode == ChainCode)
            {
                Assert.AreEqual(null, locale.ParentLocaleId);
            }
            else
            {
                Assert.AreEqual(parentLocale.LocaleId, locale.ParentLocaleId);
            }

            if(typeCode == StoreCode)
            {
                var xmlAddress = xmlLocale.Descendants(CommonRefTypesLocaleNamespace + AddressElementName).Single();
                var xmlPhysicalAddress = xmlAddress.Descendants(AddressTypeNamespace + PhysicalElementName).Single();
                var xmlTraits = xmlLocale.Descendants(CommonRefTypesRetailNamespace + TraitElementName);

                Assert.AreEqual(0, locale.LocaleId);
                Assert.AreEqual(int.Parse(xmlId), locale.BusinessUnitId);
                Assert.IsNull(locale.Locales);
                Assert.AreEqual(xmlAddress.Element(AddressNamespace + IdElementName).Value, locale.Address.AddressId.ToString());
                Assert.AreEqual(xmlPhysicalAddress.Elements(PhysicalAddressNamespace + AddressLine1ElementName).SingleOrDefault()?.Value, locale.Address.AddressLine1);
                Assert.AreEqual(xmlPhysicalAddress.Elements(PhysicalAddressNamespace + AddressLine2ElementName).SingleOrDefault()?.Value, locale.Address.AddressLine2);
                Assert.AreEqual(xmlPhysicalAddress.Elements(PhysicalAddressNamespace + AddressLine3ElementName).SingleOrDefault()?.Value, locale.Address.AddressLine3);
                Assert.AreEqual(xmlPhysicalAddress.Element(PhysicalAddressNamespace + CityTypeElementName).Element(CommonRefTypesAddressNamespace + NameElementName).Value, locale.Address.CityName);
                Assert.AreEqual(xmlPhysicalAddress.Element(PhysicalAddressNamespace + TerrityTypeElementName).Element(CommonRefTypesAddressNamespace + CodeElementName).Value, locale.Address.TerritoryCode);
                Assert.AreEqual(xmlPhysicalAddress.Element(PhysicalAddressNamespace + CountryElementName).Element(CountryNamespace + NameElementName).Value, locale.Address.Country);
                Assert.AreEqual(xmlPhysicalAddress.Element(PhysicalAddressNamespace + TimezoneElementName).Element(TimezoneNamespace + CodeElementName).Value, locale.Address.TimeZoneName);
                Assert.AreEqual(xmlPhysicalAddress.Element(PhysicalAddressNamespace + PostalCodeElementName).Value, locale.Address.PostalCode);
                AssertTraitsAreEqual(Traits.Codes.StoreAbbreviation, xmlTraits, locale.LocaleTraits.Where(lt => lt.TraitId == Traits.StoreAbbreviation).FirstOrDefault().TraitValue);
                AssertTraitsAreEqual(Traits.Codes.PhoneNumber, xmlTraits, locale.LocaleTraits.Where(lt => lt.TraitId == Traits.PhoneNumber).FirstOrDefault().TraitValue);
                AssertTraitsAreEqual(Traits.Codes.Fax, xmlTraits, locale.LocaleTraits.Where(lt => lt.TraitId == Traits.Fax).FirstOrDefault().TraitValue);
                AssertTraitsAreEqual(Traits.Codes.ContactPerson, xmlTraits, locale.LocaleTraits.Where(lt => lt.TraitId == Traits.ContactPerson).FirstOrDefault().TraitValue);
               // AssertTraitsAreEqual(Traits.Codes.EwicAgency, xmlTraits, locale.EwicAgency);
                AssertTraitsAreEqual(Traits.Codes.IrmaStoreId, xmlTraits, locale.LocaleTraits.Where(lt => lt.TraitId == Traits.IrmaStoreId).FirstOrDefault().TraitValue);
                AssertTraitsAreEqual(Traits.Codes.StorePosType, xmlTraits, locale.LocaleTraits.Where(lt => lt.TraitId == Traits.StorePosType).FirstOrDefault().TraitValue);
            }
            else
            {
                Assert.AreEqual(int.Parse(xmlId), locale.LocaleId);
            }
        }

        private static void AssertTraitsAreEqual(string traitCode, IEnumerable<XElement> xmlTraits, string traitValue)
        {
            var xmlTraitValue = xmlTraits
                .SingleOrDefault(e => e.Element(TraitNamespace + CodeElementName).Value == traitCode)
                ?.Descendants(TraitValueNamespace + ValueElementName)
                ?.SingleOrDefault();

            if(xmlTraitValue == null)
            {
                Assert.AreEqual("", traitValue);
            }
            else
            {
                Assert.AreEqual(xmlTraitValue.Value, traitValue);
            }
        }
    }
}
