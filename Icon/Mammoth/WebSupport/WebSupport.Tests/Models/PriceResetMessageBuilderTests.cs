using Esb.Core.Serializer;
using Icon.Esb.Schemas.Wfm.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using WebSupport.DataAccess.Models;
using WebSupport.MessageBuilders;
using WebSupport.Models;
using Mammoth.Common.DataAccess;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace WebSupport.Tests.Models
{
    [TestClass]
    public class PriceResetMessageBuilderTests
    {
        private PriceResetMessageBuilder priceResetMessageBuilder;
        private Serializer<Contracts.items> serializer;
        private PriceResetMessageBuilderModel request;

        [TestInitialize]
        public void Initialize()
        {
            serializer = new Serializer<Contracts.items>();
            priceResetMessageBuilder = new PriceResetMessageBuilder(serializer);
            request = new PriceResetMessageBuilderModel();
        }

        [TestMethod]
        public void BuildMessage_PricesExist_ReturnsPriceMessage()
        {
            //Given
            PriceResetPrice priceResetPrice = new PriceResetPrice
            {
                BusinessUnitId = 1234,
                CurrencyCode = "USD",
                EndDate = DateTime.Now,
                GpmId = Guid.NewGuid(),
                ItemId = 1,
                ItemTypeCode = "RTL",
                ItemTypeDesc = "Retail Sale",
                Multiple = 1,
                Price = 4.00m,
                PriceType = "REG",
                PriceTypeAttribute = "REG",
                ScanCode = "123456789",
                SellableUom = "EA",
                StartDate = DateTime.Now,
                StoreName = "TEST STORE"
            };
            request.PriceResetPrices = new List<PriceResetPrice>
            {
                priceResetPrice
            };

            //When
            var message = priceResetMessageBuilder.BuildMessage(request);

            //Then
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Contracts.items));
            MemoryStream stream = new MemoryStream();
            XDocument document = XDocument.Parse(message);
            document.Save(stream);
            stream.Position = 0;
            Contracts.items items = xmlSerializer.Deserialize(stream) as Contracts.items;

            Assert.IsNotNull(items);

            Contracts.ItemType item = items.item.Single();
            Assert.AreEqual(priceResetPrice.ItemId, item.id);
            Assert.AreEqual(priceResetPrice.ItemTypeCode, item.@base.type.code);
            Assert.AreEqual(priceResetPrice.ItemTypeDesc, item.@base.type.description);

            Contracts.LocaleType locale = item.locale.Single();
            Assert.AreEqual(priceResetPrice.BusinessUnitId.ToString(), locale.id);
            Assert.AreEqual(priceResetPrice.StoreName, locale.name);
            Assert.AreEqual(Contracts.LocaleCodeType.STR, locale.type.code);
            Assert.AreEqual(Contracts.LocaleDescType.Store, locale.type.description);
            Assert.IsFalse(locale.ActionSpecified);

            Contracts.ScanCodeType scanCode = (locale.Item as Contracts.StoreItemAttributesType).scanCode.Single();
            Assert.AreEqual(priceResetPrice.ScanCode, scanCode.code);

            Contracts.PriceType price = (locale.Item as Contracts.StoreItemAttributesType).prices.Single();
            Assert.AreEqual(Contracts.ActionEnum.Add, price.Action);
            Assert.IsTrue(price.ActionSpecified);
            Assert.AreEqual(ItemPriceTypes.Descriptions.ByCode[priceResetPrice.PriceType], price.type.description);
            Assert.AreEqual(Contracts.PriceTypeIdType.REG, price.type.id);
            Assert.AreEqual(priceResetPrice.GpmId.ToString(), price.Id);
            Assert.AreEqual(ItemPriceTypes.Descriptions.ByCode[priceResetPrice.PriceTypeAttribute], price.type.type.description);
            Assert.AreEqual(Contracts.PriceTypeIdType.REG, price.type.type.id);
            Assert.IsTrue(price.uom.codeSpecified);
            Assert.IsTrue(price.uom.nameSpecified);
            Assert.AreEqual(Contracts.WfmUomCodeEnumType.EA, price.uom.code);
            Assert.AreEqual(Contracts.WfmUomDescEnumType.EACH, price.uom.name);
            Assert.AreEqual(Contracts.CurrencyTypeCodeEnum.USD, price.currencyTypeCode);
            Assert.AreEqual(priceResetPrice.Price, price.priceAmount.amount);
            Assert.IsTrue(price.priceAmount.amountSpecified);
            Assert.AreEqual((int)priceResetPrice.Multiple, price.priceMultiple);
            Assert.IsTrue(price.priceStartDateSpecified);
            Assert.AreEqual(priceResetPrice.StartDate, price.priceStartDate);
            Assert.IsTrue(price.priceEndDateSpecified);
            Assert.AreEqual(priceResetPrice.EndDate, price.priceEndDate);

            Contracts.TraitType[] traits = price.traits;
            Assert.IsNotNull(traits);
            Assert.AreEqual(1, traits.Length);

            Contracts.TraitType newTagExpiration = traits[0];
            Assert.IsFalse(newTagExpiration.ActionSpecified);
            Assert.AreEqual(EsbConstants.TagExpirationDateTraitCode, newTagExpiration.code);
            Assert.AreEqual(EsbConstants.TagExpirationDateTraitDescription, newTagExpiration.type.description);
            Assert.AreEqual(string.Empty, newTagExpiration.type.value.First().value);
        }

        [TestMethod]
        public void BuildMessage_PriceHasTagExpirationDate_ShouldAddNewTagExpirationTrait()
        {
            //Given
            PriceResetPrice priceResetPrice = new PriceResetPrice
            {
                BusinessUnitId = 1234,
                CurrencyCode = "USD",
                EndDate = DateTime.Now,
                GpmId = Guid.NewGuid(),
                ItemId = 1,
                ItemTypeCode = "RTL",
                ItemTypeDesc = "Retail Sale",
                Multiple = 1,
                Price = 4.00m,
                PriceType = "REG",
                PriceTypeAttribute = "REG",
                ScanCode = "123456789",
                SellableUom = "EA",
                StartDate = DateTime.Now,
                StoreName = "TEST STORE",
                TagExpirationDate = DateTime.Today
            };
            request.PriceResetPrices = new List<PriceResetPrice>
            {
                priceResetPrice
            };

            //When
            var message = priceResetMessageBuilder.BuildMessage(request);

            //Then
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Contracts.items));
            MemoryStream stream = new MemoryStream();
            XDocument document = XDocument.Parse(message);
            document.Save(stream);
            stream.Position = 0;
            Contracts.items items = xmlSerializer.Deserialize(stream) as Contracts.items;

            Assert.IsNotNull(items);

            Contracts.ItemType item = items.item.Single();
            Assert.AreEqual(priceResetPrice.ItemId, item.id);
            Assert.AreEqual(priceResetPrice.ItemTypeCode, item.@base.type.code);
            Assert.AreEqual(priceResetPrice.ItemTypeDesc, item.@base.type.description);

            Contracts.LocaleType locale = item.locale.Single();
            Assert.AreEqual(priceResetPrice.BusinessUnitId.ToString(), locale.id);
            Assert.AreEqual(priceResetPrice.StoreName, locale.name);
            Assert.AreEqual(Contracts.LocaleCodeType.STR, locale.type.code);
            Assert.AreEqual(Contracts.LocaleDescType.Store, locale.type.description);
            Assert.IsFalse(locale.ActionSpecified);

            Contracts.ScanCodeType scanCode = (locale.Item as Contracts.StoreItemAttributesType).scanCode.Single();
            Assert.AreEqual(priceResetPrice.ScanCode, scanCode.code);

            Contracts.PriceType price = (locale.Item as Contracts.StoreItemAttributesType).prices.Single();
            Assert.AreEqual(Contracts.ActionEnum.Add, price.Action);
            Assert.IsTrue(price.ActionSpecified);
            Assert.AreEqual(ItemPriceTypes.Descriptions.ByCode[priceResetPrice.PriceType], price.type.description);
            Assert.AreEqual(Contracts.PriceTypeIdType.REG, price.type.id);
            Assert.AreEqual(priceResetPrice.GpmId.ToString(), price.Id);
            Assert.AreEqual(ItemPriceTypes.Descriptions.ByCode[priceResetPrice.PriceTypeAttribute], price.type.type.description);
            Assert.AreEqual(Contracts.PriceTypeIdType.REG, price.type.type.id);
            Assert.IsTrue(price.uom.codeSpecified);
            Assert.IsTrue(price.uom.nameSpecified);
            Assert.AreEqual(Contracts.WfmUomCodeEnumType.EA, price.uom.code);
            Assert.AreEqual(Contracts.WfmUomDescEnumType.EACH, price.uom.name);
            Assert.AreEqual(Contracts.CurrencyTypeCodeEnum.USD, price.currencyTypeCode);
            Assert.AreEqual(priceResetPrice.Price, price.priceAmount.amount);
            Assert.IsTrue(price.priceAmount.amountSpecified);
            Assert.AreEqual((int)priceResetPrice.Multiple, price.priceMultiple);
            Assert.IsTrue(price.priceStartDateSpecified);
            Assert.AreEqual(priceResetPrice.StartDate, price.priceStartDate);
            Assert.IsTrue(price.priceEndDateSpecified);
            Assert.AreEqual(priceResetPrice.EndDate, price.priceEndDate);

            Contracts.TraitType[] traits = price.traits;
            Assert.IsNotNull(traits);
            Assert.AreEqual(1, traits.Length);

            Contracts.TraitType newTagExpiration = traits[0];
            Assert.IsFalse(newTagExpiration.ActionSpecified);
            Assert.AreEqual(EsbConstants.TagExpirationDateTraitCode, newTagExpiration.code);
            Assert.AreEqual(EsbConstants.TagExpirationDateTraitDescription, newTagExpiration.type.description);
            Assert.AreEqual(DateTime.Today, DateTime.Parse(newTagExpiration.type.value.First().value));
        }

        [TestMethod]
        public void BuildMessage_PriceHasGspPriceTypeAttribute_ShouldBuildMessage()
        {
            //Given
            PriceResetPrice priceResetPrice = new PriceResetPrice
            {
                BusinessUnitId = 1234,
                CurrencyCode = "USD",
                EndDate = DateTime.Now,
                GpmId = Guid.NewGuid(),
                ItemId = 1,
                ItemTypeCode = "RTL",
                ItemTypeDesc = "Retail Sale",
                Multiple = 1,
                Price = 4.00m,
                PriceType = "REG",
                PriceTypeAttribute = "GSP",
                ScanCode = "123456789",
                SellableUom = "EA",
                StartDate = DateTime.Now,
                StoreName = "TEST STORE",
                TagExpirationDate = DateTime.Today
            };
            request.PriceResetPrices = new List<PriceResetPrice>
            {
                priceResetPrice
            };

            //When
            var message = priceResetMessageBuilder.BuildMessage(request);

            //Then
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Contracts.items));
            MemoryStream stream = new MemoryStream();
            XDocument document = XDocument.Parse(message);
            document.Save(stream);
            stream.Position = 0;
            Contracts.items items = xmlSerializer.Deserialize(stream) as Contracts.items;

            Assert.IsNotNull(items);

            Contracts.ItemType item = items.item.Single();
            Assert.AreEqual(priceResetPrice.ItemId, item.id);
            Assert.AreEqual(priceResetPrice.ItemTypeCode, item.@base.type.code);
            Assert.AreEqual(priceResetPrice.ItemTypeDesc, item.@base.type.description);

            Contracts.LocaleType locale = item.locale.Single();
            Assert.AreEqual(priceResetPrice.BusinessUnitId.ToString(), locale.id);
            Assert.AreEqual(priceResetPrice.StoreName, locale.name);
            Assert.AreEqual(Contracts.LocaleCodeType.STR, locale.type.code);
            Assert.AreEqual(Contracts.LocaleDescType.Store, locale.type.description);
            Assert.IsFalse(locale.ActionSpecified);

            Contracts.ScanCodeType scanCode = (locale.Item as Contracts.StoreItemAttributesType).scanCode.Single();
            Assert.AreEqual(priceResetPrice.ScanCode, scanCode.code);

            Contracts.PriceType price = (locale.Item as Contracts.StoreItemAttributesType).prices.Single();
            Assert.AreEqual(Contracts.ActionEnum.Add, price.Action);
            Assert.IsTrue(price.ActionSpecified);
            Assert.AreEqual(ItemPriceTypes.Descriptions.ByCode[priceResetPrice.PriceType], price.type.description);
            Assert.AreEqual(Contracts.PriceTypeIdType.REG, price.type.id);
            Assert.AreEqual(priceResetPrice.GpmId.ToString(), price.Id);
            Assert.AreEqual(ItemPriceTypes.Descriptions.ByCode[priceResetPrice.PriceTypeAttribute], price.type.type.description);
            Assert.AreEqual(Contracts.PriceTypeIdType.GSP, price.type.type.id);
            Assert.IsTrue(price.uom.codeSpecified);
            Assert.IsTrue(price.uom.nameSpecified);
            Assert.AreEqual(Contracts.WfmUomCodeEnumType.EA, price.uom.code);
            Assert.AreEqual(Contracts.WfmUomDescEnumType.EACH, price.uom.name);
            Assert.AreEqual(Contracts.CurrencyTypeCodeEnum.USD, price.currencyTypeCode);
            Assert.AreEqual(priceResetPrice.Price, price.priceAmount.amount);
            Assert.IsTrue(price.priceAmount.amountSpecified);
            Assert.AreEqual((int)priceResetPrice.Multiple, price.priceMultiple);
            Assert.IsTrue(price.priceStartDateSpecified);
            Assert.AreEqual(priceResetPrice.StartDate, price.priceStartDate);
            Assert.IsTrue(price.priceEndDateSpecified);
            Assert.AreEqual(priceResetPrice.EndDate, price.priceEndDate);

            Contracts.TraitType[] traits = price.traits;
            Assert.IsNotNull(traits);
            Assert.AreEqual(1, traits.Length);

            Contracts.TraitType newTagExpiration = traits[0];
            Assert.IsFalse(newTagExpiration.ActionSpecified);
            Assert.AreEqual(EsbConstants.TagExpirationDateTraitCode, newTagExpiration.code);
            Assert.AreEqual(EsbConstants.TagExpirationDateTraitDescription, newTagExpiration.type.description);
            Assert.AreEqual(DateTime.Today, DateTime.Parse(newTagExpiration.type.value.First().value));
        }

        [TestMethod]
        public void BuildMessage_PriceTypeTprPriceTypeAttributeMsal_ShouldBuildMessage()
        {
            //Given
            PriceResetPrice priceResetPrice = new PriceResetPrice
            {
                BusinessUnitId = 1234,
                CurrencyCode = "USD",
                EndDate = DateTime.Now,
                GpmId = Guid.NewGuid(),
                ItemId = 1,
                ItemTypeCode = "RTL",
                ItemTypeDesc = "Retail Sale",
                Multiple = 1,
                Price = 4.00m,
                PriceType = "TPR",
                PriceTypeAttribute = "MSAL",
                ScanCode = "123456789",
                SellableUom = "EA",
                StartDate = DateTime.Now,
                StoreName = "TEST STORE",
                TagExpirationDate = DateTime.Today
            };
            request.PriceResetPrices = new List<PriceResetPrice>
            {
                priceResetPrice
            };

            //When
            var message = priceResetMessageBuilder.BuildMessage(request);

            //Then
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Contracts.items));
            MemoryStream stream = new MemoryStream();
            XDocument document = XDocument.Parse(message);
            document.Save(stream);
            stream.Position = 0;
            Contracts.items items = xmlSerializer.Deserialize(stream) as Contracts.items;

            Assert.IsNotNull(items);

            Contracts.ItemType item = items.item.Single();
            Assert.AreEqual(priceResetPrice.ItemId, item.id);
            Assert.AreEqual(priceResetPrice.ItemTypeCode, item.@base.type.code);
            Assert.AreEqual(priceResetPrice.ItemTypeDesc, item.@base.type.description);

            Contracts.LocaleType locale = item.locale.Single();
            Assert.AreEqual(priceResetPrice.BusinessUnitId.ToString(), locale.id);
            Assert.AreEqual(priceResetPrice.StoreName, locale.name);
            Assert.AreEqual(Contracts.LocaleCodeType.STR, locale.type.code);
            Assert.AreEqual(Contracts.LocaleDescType.Store, locale.type.description);
            Assert.IsFalse(locale.ActionSpecified);

            Contracts.ScanCodeType scanCode = (locale.Item as Contracts.StoreItemAttributesType).scanCode.Single();
            Assert.AreEqual(priceResetPrice.ScanCode, scanCode.code);

            Contracts.PriceType price = (locale.Item as Contracts.StoreItemAttributesType).prices.Single();
            Assert.AreEqual(Contracts.ActionEnum.Add, price.Action);
            Assert.IsTrue(price.ActionSpecified);
            Assert.AreEqual(ItemPriceTypes.Descriptions.ByCode[priceResetPrice.PriceType], price.type.description);
            Assert.AreEqual(Contracts.PriceTypeIdType.TPR, price.type.id);
            Assert.AreEqual(priceResetPrice.GpmId.ToString(), price.Id);
            Assert.AreEqual(ItemPriceTypes.Descriptions.ByCode[priceResetPrice.PriceTypeAttribute], price.type.type.description);
            Assert.AreEqual(Contracts.PriceTypeIdType.MSAL, price.type.type.id);
            Assert.IsTrue(price.uom.codeSpecified);
            Assert.IsTrue(price.uom.nameSpecified);
            Assert.AreEqual(Contracts.WfmUomCodeEnumType.EA, price.uom.code);
            Assert.AreEqual(Contracts.WfmUomDescEnumType.EACH, price.uom.name);
            Assert.AreEqual(Contracts.CurrencyTypeCodeEnum.USD, price.currencyTypeCode);
            Assert.AreEqual(priceResetPrice.Price, price.priceAmount.amount);
            Assert.IsTrue(price.priceAmount.amountSpecified);
            Assert.AreEqual((int)priceResetPrice.Multiple, price.priceMultiple);
            Assert.IsTrue(price.priceStartDateSpecified);
            Assert.AreEqual(priceResetPrice.StartDate, price.priceStartDate);
            Assert.IsTrue(price.priceEndDateSpecified);
            Assert.AreEqual(priceResetPrice.EndDate, price.priceEndDate);

            Contracts.TraitType[] traits = price.traits;
            Assert.IsNotNull(traits);
            Assert.AreEqual(1, traits.Length);

            Contracts.TraitType newTagExpiration = traits[0];
            Assert.IsFalse(newTagExpiration.ActionSpecified);
            Assert.AreEqual(EsbConstants.TagExpirationDateTraitCode, newTagExpiration.code);
            Assert.AreEqual(EsbConstants.TagExpirationDateTraitDescription, newTagExpiration.type.description);
            Assert.AreEqual(DateTime.Today, DateTime.Parse(newTagExpiration.type.value.First().value));
        }
    }
}
