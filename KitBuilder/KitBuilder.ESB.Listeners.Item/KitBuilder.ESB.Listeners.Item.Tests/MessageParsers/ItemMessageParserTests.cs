﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Logging;
using KitBuilder.ESB.Listeners.Item.Service;
using KitBuilder.ESB.Listeners.Item.Service.MessageParsers;
using KitBuilder.ESB.Listeners.Item.Service.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace KitBuilder.ESB.Listeners.Item.Tests.MessageParsers
{
    [TestClass]
    public class ItemMessageParserTests
    {
        private ItemMessageParser itemMessageParser;
        private ItemListenerSettings settings;
        private Mock<ILogger<ItemMessageParser>> mockLogger;

        private Mock<IEsbMessage> mockEsbMessage;
        private string message;

        [TestInitialize]
        public void Initialize()
        {
            settings = new ItemListenerSettings();
            mockLogger = new Mock<ILogger<ItemMessageParser>>();

            itemMessageParser = new ItemMessageParser(settings, mockLogger.Object);
            mockEsbMessage = new Mock<IEsbMessage>();
            mockEsbMessage.Setup(m => m.GetProperty("IconMessageID")).Returns(Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void ParseMessage_ProductMessageWith3Products_ShouldReturn3ItemModels()
        {
            //Given
            message = File.ReadAllText(@"TestMessages\ProductMessageWith3Items.xml");

            mockEsbMessage.SetupGet(m => m.MessageText)
                .Returns(message);

            //When
            var items = itemMessageParser.ParseMessage(mockEsbMessage.Object).ToList();

            //Then
            Assert.AreEqual(3, items.Count());
            AssertItemsAreEqualToXml(items);
        }

        [TestMethod]
        public void ParseMessage_ExceptionIsThrown_ShouldThrowExceptionWithParseMessageExceptionErrorMessage()
        {
            //Given
            IEnumerable<ItemModel> items = null;
            mockEsbMessage.SetupGet(m => m.MessageText)
                .Returns("Bad Message");
            Exception expectedException = null;

            //When
            try
            {
                items = itemMessageParser.ParseMessage(mockEsbMessage.Object);
            }
            catch (Exception e)
            {
                expectedException = e;
            }

            //Then
            Assert.IsTrue(expectedException != null && expectedException.Message == "Failed to parse Infor Item message.");
        }

        [TestMethod]
        public void ParseMessage_ValidateSequenceIdIsTurnedOn_ShouldParseSequenceId()
        {
            //Given
            settings.ValidateSequenceId = true;
            message = File.ReadAllText(@"TestMessages\ProductMessageWith3Items.xml");

            mockEsbMessage.SetupGet(m => m.MessageText)
                .Returns(message);
            mockEsbMessage.Setup(m => m.GetProperty("SequenceID"))
                .Returns("1234");

            //When
            var items = itemMessageParser.ParseMessage(mockEsbMessage.Object).ToList();

            //Then
            Assert.AreEqual(3, items.Count);
            AssertItemsAreEqualToXml(items, 1234);
        }

        [TestMethod]
        public void ParseMessage_ProductMessageWithKosherYes_ShouldReturnItemModelWithExpectedValue()
        {
            //Given
            const string expectedAttributeVal = "Yes";
            var xmlTestData = XDocument.Load(@"TestMessages/ProductMessageWithNullItemSignAttributes.xml");
            SetXmlTraitValue(xmlTestData, "KSH", expectedAttributeVal);
            mockEsbMessage.SetupGet(m => m.MessageText).Returns(xmlTestData.ToString());

            //When
            var item = itemMessageParser.ParseMessage(mockEsbMessage.Object).FirstOrDefault();

            //Then
            Assert.AreEqual(expectedAttributeVal, item.Kosher);
        }

        [TestMethod]
        public void ParseMessage_ProductMessageWithKosherNo_ShouldReturnItemModelExpectedValue()
        {
            //Given
            var expectedAttributeVal = "No";
            var xmlTestData = XDocument.Load(@"TestMessages/ProductMessageWithNullItemSignAttributes.xml");
            SetXmlTraitValue(xmlTestData, "KSH", expectedAttributeVal);
            mockEsbMessage.SetupGet(m => m.MessageText).Returns(xmlTestData.ToString());

            //When
            var item = itemMessageParser.ParseMessage(mockEsbMessage.Object).FirstOrDefault();

            //Then
            Assert.AreEqual(expectedAttributeVal, item.Kosher);
        }
        [TestMethod]
        public void ParseMessage_ProductMessageWithKosherEmpty_ShouldReturnItemModelWithExpectedValue()
        {
            //Given
            var expectedAttributeVal = "";
            var xmlTestData = XDocument.Load(@"TestMessages/ProductMessageWithNullItemSignAttributes.xml");
            SetXmlTraitValue(xmlTestData, "KSH", expectedAttributeVal);
            mockEsbMessage.SetupGet(m => m.MessageText).Returns(xmlTestData.ToString());

            //When
            var item = itemMessageParser.ParseMessage(mockEsbMessage.Object).FirstOrDefault();

            //Then
            Assert.AreEqual(expectedAttributeVal, item.Kosher);
        }

        private void SetXmlTraitValue(XDocument xmlDoc, string traitCode, string traitValue)
        {
            // sample expected xml structure:
            //  <crf:trait>
            //    <tra:code>PAS</tra:code>
            //      <tra:type>
            //        <trt:description>Pasture Raised</trt:description>
            //        <trt:value> 
            //          <trv:value>1</trv:value>      
            //        </trt:value>       
            //     </tra:type>
            //  </crf:trait>
            xmlDoc.Root.Descendants().Where(trait => trait.Value.ToString() == traitCode).First()
                .ElementsAfterSelf().First().Descendants().Last().Value = traitValue;
        }

		[TestMethod]
		private void AssertItemsAreEqualToXml(IEnumerable<ItemModel> items, decimal? sequenceId = null)
        {
            //Copied these values directly from ProductMessageWith3Items.xml. Updating that test message will require updating
            //this code.
            var messageItems = new List<ItemModel>
            {
                new ItemModel
                {
                    ItemId = 198757,
                    ItemTypeCode = ItemTypes.Codes.RetailSale,
                    ScanCode = "9948243625",
                    ScanCodeType = ScanCodeTypes.Descriptions.Upc,
                    MerchandiseHierarchyClassId = "84305",
                    BrandsHierarchyClassId = "41674",
                    TaxHierarchyClassId = "0155030",
                    FinancialHierarchyClassId = "4200",
                    NationalHierarchyClassId = "122449",
                    ProductDescription = "ALMOND MILK UNSWEETENED OG",
                    PosDescription = "365 OG UNSWT ALMD MLK",
                    FoodStampEligible = "1",
                    PosScaleTare = "1.1",
                    ProhibitDiscount = "0",
                    PackageUnit = "1",
                    RetailSize = "32",
                    RetailUom = "OZ",
                    AnimalWelfareRating = string.Empty,
                    Biodynamic = "0",
                    CheeseMilkType = string.Empty,
                    CheeseRaw = "0",
                    EcoScaleRating = string.Empty,
                    GlutenFree = string.Empty,
                    Kosher = string.Empty,
                    Msc = "0",
                    NonGmo = string.Empty,
                    Organic = "Quality Assurance International (QAI)",
                    PremiumBodyCare = "0",
                    FreshOrFrozen = string.Empty,
                    SeafoodCatchType  = string.Empty,
                    Vegan = string.Empty,
                    Vegetarian = "0",
                    WholeTrade = "0",
                    GrassFed = "0",
                    PastureRaised = "0",
                    FreeRange = "0",
                    DryAged = "0",
                    AirChilled = "0",
                    MadeInHouse = "0",
                    AlcoholByVolume = "0",
                    CaseinFree = "0",
                    DrainedWeight = "0",
                    DrainedWeightUom = "0",
                    FairTradeCertified = "0",
                    Hemp = "0",
                    LocalLoanProducer = "0",
                    MainProductName = "0",
                    NutritionRequired = "0",
                    OrganicPersonalCare = "0",
                    Paleo = "0",
                    ProductFlavorType = "0",
                    CustomerFriendlyDescription = "Test Customer Friendly Description",
                    BrandsHierarchyName = "365",
					FlexibleText = "Test Item FlexibleText 1"
				},
                new ItemModel
                {
                    ItemId = 198759,
                    ItemTypeCode = ItemTypes.Codes.Coupon,
                    ScanCode = "29456600000",
                    ScanCodeType = ScanCodeTypes.Descriptions.ScalePlu,
                    MerchandiseHierarchyClassId = "83876",
                    BrandsHierarchyClassId = "41674",
                    TaxHierarchyClassId = "0100001",
                    FinancialHierarchyClassId = "1100",
                    NationalHierarchyClassId = "122491",
                    ProductDescription = "ALMOND MILK VANILLA OG",
                    PosDescription = "365 OG VAN ALMD MLK",
                    FoodStampEligible = "1",
                    PosScaleTare = "1.2",
                    ProhibitDiscount = "0",
                    PackageUnit = "1",
                    RetailSize = "64",
                    RetailUom = "OZ",
                    AnimalWelfareRating = string.Empty,
                    Biodynamic = "0",
                    CheeseMilkType = string.Empty,
                    CheeseRaw = "0",
                    EcoScaleRating = string.Empty,
                    GlutenFree = string.Empty,
                    Kosher = string.Empty,
                    Msc = "0",
                    NonGmo = string.Empty,
                    Organic = "Quality Assurance International (QAI)",
                    PremiumBodyCare = "0",
                    FreshOrFrozen = string.Empty,
                    SeafoodCatchType = string.Empty,
                    Vegan = string.Empty,
                    Vegetarian = "0",
                    WholeTrade = "0",
                    GrassFed = "0",
                    PastureRaised = "0",
                    FreeRange = "0",
                    DryAged = "0",
                    AirChilled = "0",
                    MadeInHouse = "0",
                    AlcoholByVolume = "1",
                    CaseinFree = "1",
                    DrainedWeight = "1",
                    DrainedWeightUom = "1",
                    FairTradeCertified = "1",
                    Hemp = "1",
                    LocalLoanProducer = "1",
                    MainProductName = "1",
                    NutritionRequired = "1",
                    OrganicPersonalCare = "1",
                    Paleo = "1",
                    ProductFlavorType = "1",
                    CustomerFriendlyDescription = "Test Customer Friendly Description",
                    BrandsHierarchyName = "365",
					FlexibleText = "Test Item FlexibleText 2"
				},
                new ItemModel
                {
                    ItemId = 198760,
                    ItemTypeCode = ItemTypes.Codes.NonRetail,
                    ScanCode = "223344",
                    ScanCodeType = ScanCodeTypes.Descriptions.PosPlu,
                    MerchandiseHierarchyClassId = "83871",
                    BrandsHierarchyClassId = "41674",
                    TaxHierarchyClassId = "0100001",
                    FinancialHierarchyClassId = "1700",
                    NationalHierarchyClassId = "122586",
                    ProductDescription = "ALMOND MILK UNSWEETENED OG",
                    PosDescription = "365 OG UNSWT ALMD MLK",
                    FoodStampEligible = "1",
                    PosScaleTare = "1.3",
                    ProhibitDiscount = "0",
                    PackageUnit = "1",
                    RetailSize = "64",
                    RetailUom = "OZ",
                    AnimalWelfareRating = string.Empty,
                    Biodynamic = "0",
                    CheeseMilkType = string.Empty,
                    CheeseRaw = "0",
                    EcoScaleRating = string.Empty,
                    GlutenFree = string.Empty,
                    Kosher = string.Empty,
                    Msc = "0",
                    NonGmo = string.Empty,
                    Organic = "Quality Assurance International (QAI)",
                    PremiumBodyCare = "0",
                    FreshOrFrozen = string.Empty,
                    SeafoodCatchType = string.Empty,
                    Vegan = string.Empty,
                    Vegetarian = "0",
                    WholeTrade = "0",
                    GrassFed = "0",
                    PastureRaised = "0",
                    FreeRange = "0",
                    DryAged = "0",
                    AirChilled = "0",
                    MadeInHouse = "0",
                    AlcoholByVolume = "0",
                    CaseinFree = "0",
                    DrainedWeight = "0",
                    DrainedWeightUom = "0",
                    FairTradeCertified = "0",
                    Hemp = "0",
                    LocalLoanProducer = "0",
                    MainProductName = "0",
                    NutritionRequired = "0",
                    OrganicPersonalCare = "0",
                    Paleo = "0",
                    ProductFlavorType = "0",
                    CustomerFriendlyDescription = "",
                    BrandsHierarchyName = "Test",
					FlexibleText = "Test Item FlexibleText 3"
				}
            };
            var listItems = items.ToList();

            for (int i = 0; i < listItems.Count(); i++)
            {
                Assert.AreEqual(messageItems[i].ItemId, listItems[i].ItemId);
                Assert.AreEqual(messageItems[i].ItemTypeCode, listItems[i].ItemTypeCode);
                Assert.AreEqual(messageItems[i].ScanCode, listItems[i].ScanCode);
                Assert.AreEqual(messageItems[i].ScanCodeType, listItems[i].ScanCodeType);
                Assert.AreEqual(messageItems[i].MerchandiseHierarchyClassId, listItems[i].MerchandiseHierarchyClassId);
                Assert.AreEqual(messageItems[i].BrandsHierarchyClassId, listItems[i].BrandsHierarchyClassId);
                Assert.AreEqual(messageItems[i].TaxHierarchyClassId, listItems[i].TaxHierarchyClassId);
                Assert.AreEqual(messageItems[i].FinancialHierarchyClassId, listItems[i].FinancialHierarchyClassId);
                Assert.AreEqual(messageItems[i].NationalHierarchyClassId, listItems[i].NationalHierarchyClassId);
                Assert.AreEqual(messageItems[i].ProductDescription, listItems[i].ProductDescription);
                Assert.AreEqual(messageItems[i].PosDescription, listItems[i].PosDescription);
                Assert.AreEqual(messageItems[i].FoodStampEligible, listItems[i].FoodStampEligible);
                Assert.AreEqual(messageItems[i].PosScaleTare, listItems[i].PosScaleTare);
                Assert.AreEqual(messageItems[i].ProhibitDiscount, listItems[i].ProhibitDiscount);
                Assert.AreEqual(messageItems[i].PackageUnit, listItems[i].PackageUnit);
                Assert.AreEqual(messageItems[i].RetailSize, listItems[i].RetailSize);
                Assert.AreEqual(messageItems[i].RetailUom, listItems[i].RetailUom);
                Assert.AreEqual(messageItems[i].AnimalWelfareRating, listItems[i].AnimalWelfareRating);
                Assert.AreEqual(messageItems[i].Biodynamic, listItems[i].Biodynamic);
                Assert.AreEqual(messageItems[i].CheeseMilkType, listItems[i].CheeseMilkType);
                Assert.AreEqual(messageItems[i].CheeseRaw, listItems[i].CheeseRaw);
                Assert.AreEqual(messageItems[i].EcoScaleRating, listItems[i].EcoScaleRating);
                Assert.AreEqual(messageItems[i].GlutenFree, listItems[i].GlutenFree);
                Assert.AreEqual(messageItems[i].Kosher, listItems[i].Kosher);
                Assert.AreEqual(messageItems[i].Msc, listItems[i].Msc);
                Assert.AreEqual(messageItems[i].NonGmo, listItems[i].NonGmo);
                Assert.AreEqual(messageItems[i].Organic, listItems[i].Organic);
                Assert.AreEqual(messageItems[i].PremiumBodyCare, listItems[i].PremiumBodyCare);
                Assert.AreEqual(messageItems[i].FreshOrFrozen, listItems[i].FreshOrFrozen);
                Assert.AreEqual(messageItems[i].SeafoodCatchType, listItems[i].SeafoodCatchType);
                Assert.AreEqual(messageItems[i].Vegan, listItems[i].Vegan);
                Assert.AreEqual(messageItems[i].Vegetarian, listItems[i].Vegetarian);
                Assert.AreEqual(messageItems[i].WholeTrade, listItems[i].WholeTrade);
                Assert.AreEqual(messageItems[i].GrassFed, listItems[i].GrassFed);
                Assert.AreEqual(messageItems[i].PastureRaised, listItems[i].PastureRaised);
                Assert.AreEqual(messageItems[i].FreeRange, listItems[i].FreeRange);
                Assert.AreEqual(messageItems[i].DryAged, listItems[i].DryAged);
                Assert.AreEqual(messageItems[i].AirChilled, listItems[i].AirChilled);
                Assert.AreEqual(messageItems[i].MadeInHouse, listItems[i].MadeInHouse);
                Assert.AreEqual(messageItems[i].AlcoholByVolume, listItems[i].AlcoholByVolume);
                Assert.AreEqual(messageItems[i].CaseinFree, listItems[i].CaseinFree);
                Assert.AreEqual(messageItems[i].DrainedWeight, listItems[i].DrainedWeight);
                Assert.AreEqual(messageItems[i].DrainedWeightUom, listItems[i].DrainedWeightUom);
                Assert.AreEqual(messageItems[i].FairTradeCertified, listItems[i].FairTradeCertified);
                Assert.AreEqual(messageItems[i].Hemp, listItems[i].Hemp);
                Assert.AreEqual(messageItems[i].LocalLoanProducer, listItems[i].LocalLoanProducer);
                Assert.AreEqual(messageItems[i].MainProductName, listItems[i].MainProductName);
                Assert.AreEqual(messageItems[i].NutritionRequired, listItems[i].NutritionRequired);
                Assert.AreEqual(messageItems[i].OrganicPersonalCare, listItems[i].OrganicPersonalCare);
                Assert.AreEqual(messageItems[i].Paleo, listItems[i].Paleo);
                Assert.AreEqual(messageItems[i].ProductFlavorType, listItems[i].ProductFlavorType);
                Assert.AreEqual(sequenceId, listItems[i].SequenceId);
                Assert.AreEqual(messageItems[i].CustomerFriendlyDescription, listItems[i].CustomerFriendlyDescription);
                Assert.AreEqual(messageItems[i].BrandsHierarchyName, listItems[i].BrandsHierarchyName);
            }
        }

    }
}
