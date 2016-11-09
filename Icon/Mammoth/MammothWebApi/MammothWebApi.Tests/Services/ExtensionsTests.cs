using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MammothWebApi.Service.Models;
using System.Collections.Generic;
using MammothWebApi.Tests.ModelBuilders;
using MammothWebApi.Service.Extensions;
using MammothWebApi.DataAccess.Models;
using System.Linq;
using Mammoth.Common.DataAccess;

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
                .Build());

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();

            // When
            List<StagingItemLocaleExtendedModel> itemLocaleExtendedStaging = itemLocales.ToStagingItemLocaleExtendedModel(now, transactionId);

            // Then
            var expected = itemLocales.OrderBy(il => il.ScanCode).ToList();
            var actual = itemLocaleExtendedStaging.OrderBy(ile => ile.ScanCode).ToList();

            Assert.AreEqual(expected.Count * 10, actual.Count); // 10 more rows per item-store row (one for each extended attribute)
            for (int i = 0; i < itemLocaleExtendedStaging.Count; i++)
            {
                Assert.IsNotNull(actual[i].AttributeValue); // Verify that none of the extended attributes added are not null
                Assert.AreEqual("SW", actual[i].Region);
                Assert.AreEqual(11111, actual[i].BusinessUnitId);
            }

            Assert.AreEqual(itemLocales[0].ColorAdded?.BoolToString(), actual[0].AttributeValue, "Color added attribute value did not get set.");
            Assert.AreEqual(itemLocales[0].CountryOfProcessing, actual[1].AttributeValue, "CountryOfProcessing attribute value did not get set.");
            Assert.AreEqual(itemLocales[0].Origin, actual[2].AttributeValue, "Origin attribute value did not get set.");
            Assert.AreEqual(itemLocales[0].ElectronicShelfTag?.BoolToString(), actual[3].AttributeValue, "ElectronicShelfTag attribute value did not get set.");
            Assert.AreEqual(itemLocales[0].Exclusive?.ToString("o"), actual[4].AttributeValue, "Exclusive attribute value did not get set.");
            Assert.AreEqual(itemLocales[0].NumberOfDigitsSentToScale.ToString(), actual[5].AttributeValue, "NumberOfDigitsSentToScale attribute value did not get set.");
            Assert.AreEqual(itemLocales[0].ChicagoBaby, actual[6].AttributeValue, "ChicagoBaby attribute value did not get set.");
            Assert.AreEqual(itemLocales[0].TagUom, actual[7].AttributeValue, "TagUom attribute value did not get set.");
            Assert.AreEqual(itemLocales[0].LinkedItem, actual[8].AttributeValue, "LinkedItem attribute value did not get set.");
            Assert.AreEqual(itemLocales[0].ScaleExtraText, actual[9].AttributeValue, "ScaleExtraText attribute value did not get set.");

            Assert.AreEqual(Attributes.ColorAdded, actual[0].AttributeId, "Color added attribute id did not get set.");
            Assert.AreEqual(Attributes.CountryOfProcessing, actual[1].AttributeId, "CountryOfProcessing attribute id did not get set.");
            Assert.AreEqual(Attributes.Origin, actual[2].AttributeId, "Origin attribute id did not get set.");
            Assert.AreEqual(Attributes.ElectronicShelfTag, actual[3].AttributeId, "ElectronicShelfTag attribute id did not get set.");
            Assert.AreEqual(Attributes.Exclusive, actual[4].AttributeId, "Exclusive attribute id did not get set.");
            Assert.AreEqual(Attributes.NumberOfDigitsSentToScale, actual[5].AttributeId, "NumberOfDigitsSentToScale attribute id did not get set.");
            Assert.AreEqual(Attributes.ChicagoBaby, actual[6].AttributeId, "ChicagoBaby attribute id did not get set.");
            Assert.AreEqual(Attributes.TagUom, actual[7].AttributeId, "TagUom attribute id did not get set.");
            Assert.AreEqual(Attributes.LinkedScanCode, actual[8].AttributeId, "LinkedScanCode attribute id did not get set.");
            Assert.AreEqual(Attributes.ScaleExtraText, actual[9].AttributeId, "ScaleExtraText attribute id did not get set.");

            Assert.AreEqual(itemLocales[1].ColorAdded?.BoolToString(), actual[10].AttributeValue, "Color added attribute value did not get set.");
            Assert.AreEqual(itemLocales[1].CountryOfProcessing, actual[11].AttributeValue, "CountryOfProcessing attribute value did not get set.");
            Assert.AreEqual(itemLocales[1].Origin, actual[12].AttributeValue, "Origin attribute value did not get set.");
            Assert.AreEqual(itemLocales[1].ElectronicShelfTag?.BoolToString(), actual[13].AttributeValue, "ElectronicShelfTag attribute value did not get set.");
            Assert.AreEqual(itemLocales[1].Exclusive?.ToString("o"), actual[14].AttributeValue, "Exclusive attribute value did not get set.");
            Assert.AreEqual(itemLocales[1].NumberOfDigitsSentToScale.ToString(), actual[15].AttributeValue, "NumberOfDigitsSentToScale attribute value did not get set.");
            Assert.AreEqual(itemLocales[1].ChicagoBaby, actual[16].AttributeValue, "ChicagoBaby attribute value did not get set.");
            Assert.AreEqual(itemLocales[1].TagUom, actual[17].AttributeValue, "TagUom attribute value did not get set.");
            Assert.AreEqual(itemLocales[1].LinkedItem, actual[18].AttributeValue, "LinkedItem attribute value did not get set.");
            Assert.AreEqual(itemLocales[1].ScaleExtraText, actual[19].AttributeValue, "ScaleExtraText attribute value did not get set.");

            Assert.AreEqual(Attributes.ColorAdded, actual[10].AttributeId, "Color added attribute id did not get set.");
            Assert.AreEqual(Attributes.CountryOfProcessing, actual[11].AttributeId, "CountryOfProcessing attribute id did not get set.");
            Assert.AreEqual(Attributes.Origin, actual[12].AttributeId, "Origin attribute id did not get set.");
            Assert.AreEqual(Attributes.ElectronicShelfTag, actual[13].AttributeId, "ElectronicShelfTag attribute id did not get set.");
            Assert.AreEqual(Attributes.Exclusive, actual[14].AttributeId, "Exclusive attribute id did not get set.");
            Assert.AreEqual(Attributes.NumberOfDigitsSentToScale, actual[15].AttributeId, "NumberOfDigitsSentToScale attribute id did not get set.");
            Assert.AreEqual(Attributes.ChicagoBaby, actual[16].AttributeId, "ChicagoBaby attribute id did not get set.");
            Assert.AreEqual(Attributes.TagUom, actual[17].AttributeId, "TagUom attribute id did not get set.");
            Assert.AreEqual(Attributes.LinkedScanCode, actual[18].AttributeId, "LinkedScanCode attribute id did not get set.");
            Assert.AreEqual(Attributes.ScaleExtraText, actual[19].AttributeId, "ScaleExtraText attribute id did not get set.");

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(transactionId, actual[i].TransactionId);
            }
        }
    }
}
