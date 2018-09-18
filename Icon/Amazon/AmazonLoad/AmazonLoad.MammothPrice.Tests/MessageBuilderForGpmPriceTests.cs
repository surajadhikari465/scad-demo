using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace AmazonLoad.MammothPrice.Tests
{
    [TestClass]
    public class MessageBuilderForGpmPriceTests
    {
        TestData testData = new TestData();

        [TestMethod]
        public void MessageBuilderForGpmPrice_CreateGpmPriceType_CreatesValidGpmPriceTypeForReg()
        {
            // Given
            var testPrice = testData.PriceGpm_REG_10414_7777777777;

            // When
            Contracts.PriceType priceType = MessageBuilderForGpmPrice.CreateGpmPriceType(testPrice);

            // Then
            Assert.IsNotNull(priceType);
            Assert.AreEqual(testPrice.Multiple, priceType.priceMultiple);
            Assert.AreEqual(testPrice.StartDate, priceType.priceStartDate);
            Assert.AreEqual(testPrice.EndDate.HasValue ? testPrice.EndDate.Value : new DateTime(), priceType.priceEndDate);
            Assert.AreEqual(testPrice.CurrencyCode, priceType.currencyTypeCode.ToString());

            Contracts.PriceTypeType priceTypeType = priceType.type;
            Assert.AreEqual(testPrice.PriceType, priceTypeType.id.ToString());
            Assert.AreEqual(testPrice.PriceTypeDesc, priceTypeType.description);

            Contracts.PriceTypeType priceTypeSubtypeType = priceType.type.type;
            Assert.IsNull(priceTypeSubtypeType);

            Contracts.UomType uomType = priceType.uom;
            Assert.AreEqual(testPrice.SellableUOM, uomType.code.ToString());
            Assert.AreEqual(testPrice.UomName, uomType.name.ToString());

            Contracts.PriceAmount priceAmount = priceType.priceAmount;
            Assert.AreEqual(testPrice.Price, priceAmount.amount);
        }

        [TestMethod]
        public void MessageBuilderForGpmPrice_BuildGpmItemType_CreateValidItemTypeForReg()
        {
            // Given
            var testPrice = testData.PriceGpm_REG_10414_7777777777;

            // When
            Contracts.ItemType itemType = MessageBuilderForGpmPrice.BuildGpmItemType(testPrice);

            // Then
            Assert.IsNotNull(itemType);
            Assert.AreEqual(testPrice.ItemId, itemType.id);

            Contracts.BaseItemType baseType = itemType.@base;
            Contracts.ItemTypeType itemTypeType = baseType.type;
            Assert.AreEqual(testPrice.ItemTypeCode, itemTypeType.code);
            Assert.AreEqual(testPrice.ItemTypeDesc, itemTypeType.description);

            Contracts.LocaleType localeType = itemType.locale[0];
            Assert.AreEqual(testPrice.BusinessUnitId.ToString(), localeType.id);
            Assert.AreEqual(testPrice.LocaleName, localeType.name);
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, localeType.Action);

            Contracts.LocaleTypeType localeTypeType = localeType.type;
            Assert.AreEqual("STR", localeTypeType.code.ToString());
            Assert.AreEqual("Store", localeTypeType.description.ToString());

            Contracts.StoreItemAttributesType storeItemAttr = localeType.Item as Contracts.StoreItemAttributesType;
            Contracts.ScanCodeType scanCodeType = storeItemAttr.scanCode[0];
            Assert.AreEqual(testPrice.ScanCode, scanCodeType.code);

            Contracts.PriceType[] prices = storeItemAttr.prices;
            Assert.AreEqual(1, prices.Length);
            Assert.AreEqual(testPrice.Price, prices[0].priceAmount.amount);
        }

        [TestMethod]
        public void MessageBuilderForGpmPrice_CreateGpmPriceType_CreatesValidGpmPriceTypeForTpr()
        {
            // Given
            var testPrice = testData.PriceGpm_TPR_10414_6700760076;

            // When
            Contracts.PriceType priceType = MessageBuilderForGpmPrice.CreateGpmPriceType(testPrice);

            // Then
            Assert.IsNotNull(priceType);
            Assert.AreEqual(testPrice.Multiple, priceType.priceMultiple);
            Assert.AreEqual(testPrice.StartDate, priceType.priceStartDate);
            Assert.AreEqual(testPrice.EndDate.HasValue ? testPrice.EndDate.Value : new DateTime(), priceType.priceEndDate);
            Assert.AreEqual(testPrice.CurrencyCode, priceType.currencyTypeCode.ToString());

            Contracts.PriceTypeType priceTypeType = priceType.type;
            Assert.AreEqual(testPrice.PriceType, priceTypeType.id.ToString());
            Assert.AreEqual(testPrice.PriceTypeDesc, priceTypeType.description);

            Contracts.PriceTypeType priceTypeSubtypeType = priceType.type.type;
            Assert.IsNotNull(priceTypeSubtypeType);
            Assert.AreEqual(testPrice.SubPriceTypeCode, priceTypeSubtypeType.id.ToString());
            Assert.AreEqual(testPrice.SubPriceTypeDesc, priceTypeSubtypeType.description);

            Contracts.UomType uomType = priceType.uom;
            Assert.AreEqual(testPrice.SellableUOM, uomType.code.ToString());
            Assert.AreEqual(testPrice.UomName, uomType.name.ToString());

            Contracts.PriceAmount priceAmount = priceType.priceAmount;
            Assert.AreEqual(testPrice.Price, priceAmount.amount);
        }

        [TestMethod]
        public void MessageBuilderForGpmPrice_BuildGpmItemType_CreateValidItemTypeForTpr()
        {
            // Given
            var testPrice = testData.PriceGpm_TPR_10414_6700760076;

            // When
            Contracts.ItemType itemType = MessageBuilderForGpmPrice.BuildGpmItemType(testPrice);

            // Then
            Assert.IsNotNull(itemType);
            Assert.AreEqual(testPrice.ItemId, itemType.id);

            Contracts.BaseItemType baseType = itemType.@base;
            Contracts.ItemTypeType itemTypeType = baseType.type;
            Assert.AreEqual(testPrice.ItemTypeCode, itemTypeType.code);
            Assert.AreEqual(testPrice.ItemTypeDesc, itemTypeType.description);

            Contracts.LocaleType localeType = itemType.locale[0];
            Assert.AreEqual(testPrice.BusinessUnitId.ToString(), localeType.id);
            Assert.AreEqual(testPrice.LocaleName, localeType.name);
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, localeType.Action);

            Contracts.LocaleTypeType localeTypeType = localeType.type;
            Assert.AreEqual("STR", localeTypeType.code.ToString());
            Assert.AreEqual("Store", localeTypeType.description.ToString());

            Contracts.StoreItemAttributesType storeItemAttr = localeType.Item as Contracts.StoreItemAttributesType;
            Contracts.ScanCodeType scanCodeType = storeItemAttr.scanCode[0];
            Assert.AreEqual(testPrice.ScanCode, scanCodeType.code);

            Contracts.PriceType[] prices = storeItemAttr.prices;
            Assert.AreEqual(1, prices.Length);
            Assert.AreEqual(testPrice.Price, prices[0].priceAmount.amount);

            Contracts.PriceTypeType priceTypeSubtypeType = prices[0].type.type;
            Assert.IsNotNull(priceTypeSubtypeType);
            Assert.AreEqual(testPrice.SubPriceTypeCode, priceTypeSubtypeType.id.ToString());
            Assert.AreEqual(testPrice.SubPriceTypeDesc, priceTypeSubtypeType.description);
        }
    }
}
