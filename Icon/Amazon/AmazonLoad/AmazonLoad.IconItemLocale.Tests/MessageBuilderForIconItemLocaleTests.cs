using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using AmazonLoad.Common;

namespace AmazonLoad.IconItemLocale.Tests
{
    [TestClass]
    public class MessageBuilderForIconItemLocaleTests
    {
        TestData testData = new TestData();

        [TestMethod]
        public void MessageBuilderForIconItemLocale_ConvertToContractsItemType_ReturnsItemTypeAsExpected()
        {
            // Given
            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            var psgMapper = new FakeIconItemLocalePsgMapper(testData.TestPsgs);
            MessageBuilderForIconItemLocale.PsgMapper = psgMapper;

            // When
            Contracts.ItemType itemType = MessageBuilderForIconItemLocale.ConvertToContractsItemType(testItemLocale);

            // Then
            Assert.IsNotNull(itemType);
            Assert.AreEqual(testItemLocale.ItemId, itemType.id);
            Contracts.LocaleType store = itemType.locale.First();
            Assert.AreEqual(testItemLocale.BusinessUnit.ToString(), store.id);
            Assert.AreEqual("STR", store.type.code.ToString());
            Assert.AreEqual("Store", store.type.description.ToString());
            Contracts.StoreItemAttributesType siAttribs = store.Item as Contracts.StoreItemAttributesType;
            Assert.AreEqual(testItemLocale.ScanCode, siAttribs.scanCode.First().code);
            Assert.AreEqual(testItemLocale.ScanCodeTypeId, siAttribs.scanCode.First().typeId);
            Assert.AreEqual(testItemLocale.ScanCodeTypeDesc, siAttribs.scanCode.First().typeDescription);
            Assert.AreEqual(8, siAttribs.traits.Count());
            Assert.AreEqual(6, siAttribs.selectionGroups.group.Count());
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_ConvertToContractsItemType_WhenLinkedItemReturns()
        {
            // Given
            var testItemLocale = testData.ItemLocale_BU10042_UPC_WithLinkedItem;
            var psgMapper = new FakeIconItemLocalePsgMapper(testData.TestPsgs);
            MessageBuilderForIconItemLocale.PsgMapper = psgMapper;

            // When
            Contracts.ItemType itemType = MessageBuilderForIconItemLocale.ConvertToContractsItemType(testItemLocale);

            // Then
            var siAttribs = itemType.locale.First().Item as Contracts.StoreItemAttributesType;
            Assert.AreEqual(1, siAttribs.links.Length);
            Assert.AreEqual(testItemLocale.LinkedItemId, siAttribs.links[0].parentId);
            Assert.AreEqual(testItemLocale.ItemId, siAttribs.links[0].childId);

            Assert.AreEqual(1, siAttribs.groups.Length);
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, siAttribs.groups[0].Action);
            Assert.AreEqual($"{testItemLocale.LinkedItemId}_{testItemLocale.ItemId}", siAttribs.groups[0].id);
            Assert.AreEqual("Deposit", siAttribs.groups[0].description);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_CreatesAllExpectedTraits()
        {
            // Given
            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            var expectedTraitCount = 8;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            Assert.IsNotNull(traits);
            Assert.AreEqual(expectedTraitCount, traits.Count());
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_CreatesExpectedLockedForSaleTrait_WithValueFalse()
        {
            // Given
            var traitValue = false;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.Delete;
            var expectedTraitCode = "RS";
            var expectedTraitDesc = "Locked For Sale";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.LockedForSale = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_CreatesExpectedLockedForSaleTrait_WithValueTrue()
        {
            // Given
            var traitValue = true;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.AddOrUpdate;
            var expectedTraitCode = "RS";
            var expectedTraitDesc = "Locked For Sale";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.LockedForSale = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_CreatesExpectedRecallTrait_WithValueFalse()
        {
            // Given
            var traitValue = false;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.Delete;
            var expectedTraitCode = "RCL";
            var expectedTraitDesc = "Recall";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.Recall = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_CreatesExpectedRecallTrait_WithValueTrue()
        {
            // Given
            var traitValue = true;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.AddOrUpdate;
            var expectedTraitCode = "RCL";
            var expectedTraitDesc = "Recall";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.Recall = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_CreatesExpectedSoldByWeightTrait_WithValueFalse()
        {
            // Given
            var traitValue = false;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.Delete;
            var expectedTraitCode = "SBW";
            var expectedTraitDesc = "Sold by Weight";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.Sold_By_Weight = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_CreatesExpectedSoldByWeightTrait_WithValueTrue()
        {
            // Given
            var traitValue = true;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.AddOrUpdate;
            var expectedTraitCode = "SBW";
            var expectedTraitDesc = "Sold by Weight";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.Sold_By_Weight = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_CreatesQuantityRequiredTrait_WithValueFalse()
        {
            // Given
            var traitValue = false;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.Delete;
            var expectedTraitCode = "QTY";
            var expectedTraitDesc = "Quantity Required";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.Quantity_Required = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_CreatesQuantityRequiredTrait_WithValueTrue()
        {
            // Given
            var traitValue = true;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.AddOrUpdate;
            var expectedTraitCode = "QTY";
            var expectedTraitDesc = "Quantity Required";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.Quantity_Required = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_CreatesPriceRequiredTrait_WithValueFalse()
        {
            // Given
            var traitValue = false;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.Delete;
            var expectedTraitCode = "PRQ";
            var expectedTraitDesc = "Price Required";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.Price_Required = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_CreatesPriceRequiredTrait_WithValueTrue()
        {
            // Given
            var traitValue = true;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.AddOrUpdate;
            var expectedTraitCode = "PRQ";
            var expectedTraitDesc = "Price Required";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.Price_Required = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_CreatesQuantityProhibitTrait_WithValueFalse()
        {
            // Given
            var traitValue = false;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.Delete;
            var expectedTraitCode = "QPR";
            var expectedTraitDesc = "Quantity Prohibit";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.QtyProhibit = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_CreatesQuantityProhibitTrait_WithValueTrue()
        {
            // Given
            var traitValue = true;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.AddOrUpdate;
            var expectedTraitCode = "QPR";
            var expectedTraitDesc = "Quantity Prohibit";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.QtyProhibit = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_MakesVisualVerifyTrait_WithValueFalse()
        {
            // Given
            var traitValue = false;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.Delete;
            var expectedTraitCode = "VV";
            var expectedTraitDesc = "Visual Verify";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.VisualVerify = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_MakesVisualVerifyTrait_WithValueTrue()
        {
            // Given
            var traitValue = true;
            var expectedTraitTypeValue = traitValue ? "1" : "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.AddOrUpdate;
            var expectedTraitCode = "VV";
            var expectedTraitDesc = "Visual Verify";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.VisualVerify = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_MakesPosScaleTare_WithValueZero()
        {
            // Given
            decimal? traitValue = 0;
            var expectedTraitTypeValue = "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.Delete;
            var expectedTraitCode = "SCT";
            var expectedTraitDesc = "POS Scale Tare";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.PosScaleTare = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_MakesPosScaleTare_WithValueNull()
        {
            // Given
            decimal? traitValue = null;
            var expectedTraitTypeValue = "0";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.Delete;
            var expectedTraitCode = "SCT";
            var expectedTraitDesc = "POS Scale Tare";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.PosScaleTare = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTraitsForItemLocale_MakesPosScaleTare_WithValueOne()
        {
            // Given
            decimal? traitValue = 1;
            var expectedTraitTypeValue = "1";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.AddOrUpdate;
            var expectedTraitCode = "SCT";
            var expectedTraitDesc = "POS Scale Tare";

            var testItemLocale = testData.ItemLocale_BU10042_UPC;
            testItemLocale.PosScaleTare = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForIconItemLocale.CreateTraitsForItemLocale(testItemLocale);

            // Then
            var trait = traits.FirstOrDefault(t => t.code == expectedTraitCode);
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedTraitCode, trait.code);
            Assert.AreEqual(expectedTraitDesc, trait.type.description);
            Assert.AreEqual(expectedTraitTypeValue, trait.type.value[0].value);
            Assert.AreEqual(expectedActionSpecified, trait.ActionSpecified);
            Assert.AreEqual(expectedAction, trait.Action);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTrait_CreatesBooleanTraitWithAddOrUpdateAction()
        {
            // Given
            string traitCode = "ABC";
            string traitDesc = "The abc trait";
            string traitValue = "1";
            Contracts.ActionEnum? action = Contracts.ActionEnum.AddOrUpdate;

            // When
            Contracts.TraitType trait = MessageBuilderForIconItemLocale.CreateTrait(traitCode, traitDesc, traitValue, action);

            // Then
            Assert.IsNotNull(trait);
            Assert.AreEqual(action.GetValueOrDefault(Contracts.ActionEnum.Add), trait.Action);
            Assert.AreEqual(action.HasValue, trait.ActionSpecified);
            Assert.AreEqual(traitCode, trait.code);
            Assert.AreEqual(traitDesc, trait.type.description);
            Assert.AreEqual(traitValue, trait.type.value[0].value);
        }

        [TestMethod]
        public void MessageBuilderForIconItemLocale_CreateTrait_CreatesBooleanTraitWithDeleteAction()
        {
            // Given
            string traitCode = "ABC";
            string traitDesc = "The abc trait";
            string traitValue = "0";
            Contracts.ActionEnum? action = Contracts.ActionEnum.Delete;

            // When
            Contracts.TraitType trait = MessageBuilderForIconItemLocale.CreateTrait(traitCode, traitDesc, traitValue, action);

            // Then
            Assert.IsNotNull(trait);
            Assert.AreEqual(action.GetValueOrDefault(Contracts.ActionEnum.AddOrUpdate), trait.Action);
            Assert.AreEqual(action.HasValue, trait.ActionSpecified);
            Assert.AreEqual(traitCode, trait.code);
            Assert.AreEqual(traitDesc, trait.type.description);
            Assert.AreEqual(traitValue, trait.type.value[0].value);
        }
    }
}