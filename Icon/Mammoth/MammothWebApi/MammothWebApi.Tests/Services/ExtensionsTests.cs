using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MammothWebApi.Service.Models;
using System.Collections.Generic;
using MammothWebApi.Tests.ModelBuilders;
using MammothWebApi.Service.Extensions;
using MammothWebApi.DataAccess.Models;
using System.Linq;
using Mammoth.Common.DataAccess;
using MammothWebApi.Tests.Helpers;

namespace MammothWebApi.Tests.Services
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void ToItemLocaleModelStaging_ListItemLocaleServiceModel_PropertiesMappedToCorrectItemLocaleModelStaging()
        {
            // Given
            List<ItemLocaleServiceModel> itemLocales = new List<ItemLocaleServiceModel>();
            itemLocales.Add(new TestItemLocaleServiceModelBuilder().WithScanCode("12345")
                .WithRegion("SW").WithBusinessUnit(11111).WithAuthorized(false).Build());
            itemLocales.Add(new TestItemLocaleServiceModelBuilder().WithScanCode("12346")
                .WithRegion("SW").WithBusinessUnit(11111).WithDiscountTm(false).Build());
            itemLocales.Add(new TestItemLocaleServiceModelBuilder().WithScanCode("12347")
                .WithRegion("SW").WithBusinessUnit(11111).WithRestrictionAge(1).Build());

            DateTime utcNow = DateTime.UtcNow;
            Guid transactionId = Guid.NewGuid();

            // When
            List<StagingItemLocaleModel> itemLocaleStaging = itemLocales.ToStagingItemLocaleModel(utcNow, transactionId);

            // Then
            var expected = itemLocales.OrderBy(il => il.ScanCode).ToList();
            var actual = itemLocaleStaging.OrderBy(s => s.ScanCode).ToList();

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].AgeRestriction, actual[i].Restriction_Age);
                Assert.AreEqual(expected[i].Authorized, actual[i].Authorized);
                Assert.AreEqual(expected[i].BusinessUnitId, actual[i].BusinessUnitID);
                Assert.AreEqual(expected[i].CaseDiscount, actual[i].Discount_Case);
                Assert.AreEqual(expected[i].Region, actual[i].Region);
                Assert.AreEqual(expected[i].ScanCode, actual[i].ScanCode);
                Assert.AreEqual(expected[i].TMDiscount, actual[i].Discount_TM);
                Assert.AreEqual(expected[i].RestrictedHours, actual[i].Restriction_Hours);

                Assert.AreEqual(expected[i].Discontinued, actual[i].Discontinued);
                Assert.AreEqual(expected[i].LabelTypeDescription, actual[i].LabelTypeDesc);
                Assert.AreEqual(expected[i].LocalItem, actual[i].LocalItem);
                Assert.AreEqual(expected[i].ProductCode, actual[i].Product_Code);
                Assert.AreEqual(expected[i].RetailUnit, actual[i].RetailUnit);
                Assert.AreEqual(expected[i].SignDescription, actual[i].Sign_Desc);
                Assert.AreEqual(expected[i].SignRomanceLong, actual[i].Sign_RomanceText_Long);
                Assert.AreEqual(expected[i].SignRomanceShort, actual[i].Sign_RomanceText_Short);
                Assert.AreEqual(transactionId, actual[i].TransactionId);
            }
        }

        [TestMethod]
        public void ToItemLocaleExtendedModelStaging_ListItemLocaleServiceModel_PropertiesMappedToCorrectItemLocaleModelStaging()
        {
            // Given
            // we each item-store object/data row will have a set number of extended attributes
            int numberOfExtendedAttributesPerItemLocale = ItemLocaleTestData.NumberOfExtendedAttributesPerItemLocale;
            List<ItemLocaleServiceModel> itemLocales = new List<ItemLocaleServiceModel>();
            itemLocales.Add(new TestItemLocaleServiceModelBuilder().WithScanCode("12345").WithRegion("SW").WithBusinessUnit(11111)
                .WithColorAdded(true)
                .WithOrigin("Canada")
                .WithCountryOfProcessing("USA")
                .WithElectronicShelfTag(true)
                .WithExclusive(DateTime.Now)
                .WithNumberOfDigitsSentToScale(4)
                .WithChicagoBaby("test")
                .WithTagUom("test tag uom")
                .WithLinkedItem("101010101")
                .WithScaleExtraText("extra text for scale item")
                .WithForceTare(true)
                .WithCfsSendToScale(true)
                .WithWrappedTareWeight("wrapped tare weight")
                .WithUnwrappedTareWeight("UNwrapped tare weight")
                .WithScaleItem(true)
                .WithUseBy("use by next week")
                .WithShelfLife(13)
                .Build());
            itemLocales.Add(new TestItemLocaleServiceModelBuilder().WithScanCode("12346").WithRegion("SW").WithBusinessUnit(11111)
                .WithColorAdded(true)
                .WithOrigin("Canada")
                .WithCountryOfProcessing("USA")
                .WithElectronicShelfTag(true)
                .WithExclusive(DateTime.Now)
                .WithNumberOfDigitsSentToScale(4)
                .WithChicagoBaby("test")
                .WithTagUom("test tag uom")
                .WithLinkedItem("101010101")
                .WithScaleExtraText("extra text for scale item")
                .WithForceTare(false)
                .WithCfsSendToScale(false)
                .WithWrappedTareWeight("wrapped tare weight")
                .WithUnwrappedTareWeight("UNwrapped tare weight")
                .WithScaleItem(false)
                .WithUseBy("use by xmas 2034")
                .WithShelfLife(5)
                .Build());

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();

            // When
            List<StagingItemLocaleExtendedModel> itemLocaleExtendedStaging = itemLocales.ToStagingItemLocaleExtendedModel(now, transactionId);

            // Then
            var expectedList = itemLocales.OrderBy(il => il.ScanCode).ToList();
            var actualList = itemLocaleExtendedStaging.OrderBy(ile => ile.ScanCode).ToList();

            // multiply the number of item-store rows by the number of expected ext. attributes to get the expected count
            Assert.AreEqual(expectedList.Count * numberOfExtendedAttributesPerItemLocale, actualList.Count);

            for (int testModelIndex = 0; testModelIndex < expectedList.Count; testModelIndex++)
            {
                var testModel = expectedList[testModelIndex];

                for (int actualAttributeIndex = 0; actualAttributeIndex < numberOfExtendedAttributesPerItemLocale; actualAttributeIndex++)
                {
                    Assert.AreEqual("SW", actualList[actualAttributeIndex].Region);
                    Assert.AreEqual(11111, actualList[actualAttributeIndex].BusinessUnitId);
                    Assert.AreEqual(transactionId, actualList[actualAttributeIndex].TransactionId);
                }

                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.ColorAdded,
                    testModel.ColorAdded?.BoolToString(),
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode,
                    Attributes.ColorAdded)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.CountryOfProcessing,
                    testModel.CountryOfProcessing,
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode
                    , Attributes.CountryOfProcessing)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.Origin,
                    testModel.Origin,
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode,
                    Attributes.Origin)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.ElectronicShelfTag,
                    testModel.ElectronicShelfTag?.BoolToString(),
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode, Attributes.ElectronicShelfTag)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.Exclusive,
                    testModel.Exclusive?.ToString("o"),
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode, Attributes.Exclusive)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.NumberOfDigitsSentToScale,
                    testModel.NumberOfDigitsSentToScale.ToString(),
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode, Attributes.NumberOfDigitsSentToScale)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.ChicagoBaby,
                    testModel.ChicagoBaby,
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode, Attributes.ChicagoBaby)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.TagUom,
                    testModel.TagUom,
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode, Attributes.TagUom)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.LinkedScanCode,
                    testModel.LinkedItem,
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode, Attributes.LinkedScanCode)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.ScaleExtraText,
                    testModel.ScaleExtraText,
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode, Attributes.ScaleExtraText)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.ForceTare,
                    testModel.ForceTare?.ToString(),
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode, Attributes.ForceTare)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.CfsSendToScale,
                    testModel.SendtoCFS?.ToString(),
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode, Attributes.CfsSendToScale)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.WrappedTareWeight,
                    testModel.WrappedTareWeight,
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode, Attributes.WrappedTareWeight)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.UnwrappedTareWeight,
                    testModel.UnwrappedTareWeight,
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode, Attributes.UnwrappedTareWeight)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.UseByEab,
                    testModel.UseBy,
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode, Attributes.UseByEab)));
                AssertStagedItemLocaleExtendedAttributeAsExpected(
                    Attributes.ShelfLife,
                    testModel.ShelfLife?.ToString(),
                    actualList.SingleOrDefault(attr => MatchAttribute(attr, testModel.ScanCode, Attributes.ShelfLife)));
            }
        }

        protected static Func<StagingItemLocaleExtendedModel, string, int, bool> MatchAttribute =
            (model, scanCode, attributeId) => model.ScanCode==scanCode && model.AttributeId == attributeId;


        protected void AssertStagedItemLocaleExtendedAttributeAsExpected( int attributeId,
            string expectedValueAsString, StagingItemLocaleExtendedModel actualAttribute)
        {
            var attributeCode = GetCodeForAttribute(attributeId);
            Assert.AreEqual(attributeId, actualAttribute.AttributeId,
                $"{attributeCode} attribute ({attributeId}) id did not get set.");
            if (expectedValueAsString != null)
            {
                Assert.IsNotNull(actualAttribute.AttributeValue);
            }
            Assert.AreEqual(expectedValueAsString, actualAttribute.AttributeValue,
                $"{attributeCode} ({attributeId}) value did not get set. Expected '{expectedValueAsString}' Actual '{actualAttribute.AttributeValue}'");
        }

        protected static string GetCodeForAttribute(int attributeId)
        {
            switch (attributeId)
            {
                case Attributes.AgeRestrict: return Attributes.Codes.AgeRestrict;
                case Attributes.MadeWithOrganicGrapes: return Attributes.Codes.MadeWithOrganicGrapes;
                case Attributes.MadeWithBiodynamicGrapes: return Attributes.Codes.MadeWithBiodynamicGrapes;
                case Attributes.NutritionRequired: return Attributes.Codes.NutritionRequired;
                case Attributes.PrimeBeef: return Attributes.Codes.PrimeBeef;
                case Attributes.RainforestAlliance: return Attributes.Codes.RainforestAlliance;
                case Attributes.SmithsonianBirdFriendly: return Attributes.Codes.SmithsonianBirdFriendly;
                case Attributes.Wic: return Attributes.Codes.Wic;
                case Attributes.ForceTare: return Attributes.Codes.ForceTare;
                case Attributes.ShelfLife: return Attributes.Codes.ShelfLife;
                case Attributes.UnwrappedTareWeight: return Attributes.Codes.UnwrappedTareWeight;
                case Attributes.WrappedTareWeight: return Attributes.Codes.WrappedTareWeight;
                case Attributes.UseByEab: return Attributes.Codes.UseByEab;
                case Attributes.CfsSendToScale: return Attributes.Codes.CfsSendToScale;
                case Attributes.VendorCaseSize: return Attributes.Codes.VendorCaseSize;
                case Attributes.VendorName: return Attributes.Codes.VendorName;
                case Attributes.VendorItemId: return Attributes.Codes.VendorItemId;
                case Attributes.IrmaVendorKey: return Attributes.Codes.IrmaVendorKey;
                case Attributes.OrderedByInfor: return Attributes.Codes.OrderedByInfor;
                case Attributes.GlobalPricingProgram: return Attributes.Codes.GlobalPricingProgram;
                case Attributes.AltRetailSize: return Attributes.Codes.AltRetailSize;
                case Attributes.AltRetailUom: return Attributes.Codes.AltRetailUom;
                case Attributes.FlexibleText : return Attributes.Codes.FlexibleText;
                case Attributes.FairTradeCertified: return Attributes.Codes.FairTradeCertified;
                case Attributes.CustomerFriendlyDescription: return Attributes.Codes.CustomerFriendlyDescription;
                case Attributes.RetailUnit: return Attributes.Codes.RetailUnit;
                case Attributes.AuthorizedForSale: return Attributes.Codes.AuthorizedForSale;
                case Attributes.CaseDiscountEligible: return Attributes.Codes.CaseDiscountEligible;
                case Attributes.ChicagoBaby: return Attributes.Codes.ChicagoBaby;
                case Attributes.ColorAdded: return Attributes.Codes.ColorAdded;
                case Attributes.CountryOfProcessing: return Attributes.Codes.CountryOfProcessing;
                case Attributes.Discontinued: return Attributes.Codes.Discontinued;
                case Attributes.ElectronicShelfTag: return Attributes.Codes.ElectronicShelfTag;
                case Attributes.Exclusive: return Attributes.Codes.Exclusive;
                case Attributes.LabelTypeDesc: return Attributes.Codes.LabelTypeDesc;
                case Attributes.LinkedScanCode: return Attributes.Codes.LinkedScanCode;
                case Attributes.LocalItem: return Attributes.Codes.LocalItem;
                case Attributes.Locality: return Attributes.Codes.Locality;
                case Attributes.NumberOfDigitsSentToScale: return Attributes.Codes.NumberOfDigitsSentToScale;
                case Attributes.Origin: return Attributes.Codes.Origin;
                case Attributes.ProductCode: return Attributes.Codes.ProductCode;
                case Attributes.RestrictedHours: return Attributes.Codes.RestrictedHours;
                case Attributes.ScaleExtraText: return Attributes.Codes.ScaleExtraText;
                case Attributes.SignCaption: return Attributes.Codes.SignCaption;
                case Attributes.SoldByWeight: return Attributes.Codes.SoldByWeight;
                case Attributes.TagUom: return Attributes.Codes.TagUom;
                case Attributes.TmDiscountEligible: return Attributes.Codes.TmDiscountEligible;
                case Attributes.SignRomanceShort: return Attributes.Codes.SignRomanceShort;
                case Attributes.SignRomanceLong: return Attributes.Codes.SignRomanceLong;
                case Attributes.Msrp: return Attributes.Codes.Msrp;

                default:
                    break;
            }
            return null;
        }
    }
}
