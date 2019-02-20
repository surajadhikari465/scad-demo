using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.Common.DataAccess;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace AmazonLoad.MammothItemLocale.Tests
{
    [TestClass]
    public class MessageBuilderForMammothItemLocaleTests
    {
        TestData testData = new TestData();

        [TestMethod]
        public void MessageBuilderForMammothItemLocale_ConvertToContractsItemType_ReturnsItemTypeAsExpected()
        {
            // Given
            var testItemLocale = testData.ItemLocale_10220_CamembertLocal;

            // When
            Contracts.ItemType itemType = MessageBuilderForMammothItemLocale.ConvertToContractsItemType(testItemLocale);

            // Then
            Assert.IsNotNull(itemType);
            Assert.AreEqual(testItemLocale.ItemId, itemType.id);
            Contracts.LocaleType store = itemType.locale.First();
            Assert.AreEqual(testItemLocale.BusinessUnitId.ToString(), store.id);
            Assert.AreEqual("STR", store.type.code.ToString());
            Assert.AreEqual("Store", store.type.description.ToString());
            Contracts.StoreItemAttributesType siAttribs = store.Item as Contracts.StoreItemAttributesType;
            Assert.AreEqual(testItemLocale.ScanCode, siAttribs.scanCode.First().code);
            Assert.AreEqual(34, siAttribs.traits.Count());
            // should not be any PSGs mammoth item-locale message
            Assert.IsNull(siAttribs.selectionGroups);
        }

        [TestMethod]
        public void MessageBuilderForMammothItemLocale_CreateTraitsForItemLocale_CreatesExpectedColorAddedTrait_WithValueFalse()
        {
            // Given
            var traitValue = "false";
            var expectedTraitTypeValue = "false";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.AddOrUpdate;
            var expectedTraitCode = "CLA";
            var expectedTraitDesc = "Color Added";

            var testItemLocale = testData.ItemLocale_10220_PaleAleWithDep;
            testItemLocale.ColorAdded = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForMammothItemLocale.CreateTraitsForMammothItemLocale(testItemLocale);

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
        public void MessageBuilderForMammothItemLocale_CreateTraitsForItemLocale_CreatesExpectedPrimeAffinityPSGTrait_WithValueTrue()
        {
            // Given
            var traitValue = "true";
            var expectedTraitTypeValue = "true";
            var expectedActionSpecified = true;
            var expectedAction = Contracts.ActionEnum.AddOrUpdate;
            var expectedTraitCode = "CLA";
            var expectedTraitDesc = "Color Added";

            var testItemLocale = testData.ItemLocale_10220_Pillow;
            testItemLocale.ColorAdded = traitValue;

            // When
            Contracts.TraitType[] traits = MessageBuilderForMammothItemLocale.CreateTraitsForMammothItemLocale(testItemLocale);

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
        public void MessageBuilderForMammothItemLocale_CreateTrait_CreatesBooleanFalseTraitValueWithAsAddOrUpdateAction()
        {
            // Given
            string traitCode = "FTC";
            string traitDesc = "Fair Trade Certified";
            string traitValue = "1";
            Contracts.ActionEnum? expectedAction = Contracts.ActionEnum.AddOrUpdate;

            // When
            var x = Attributes.Descriptions.ByCode[traitCode];
            Contracts.TraitType trait = MessageBuilderForMammothItemLocale.CreateTrait(traitValue, traitCode);

            // Then
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedAction.Value, trait.Action);
            Assert.AreEqual(expectedAction.HasValue, trait.ActionSpecified);
            Assert.AreEqual(traitCode, trait.code);
            Assert.AreEqual(traitDesc, trait.type.description);
            Assert.AreEqual(traitValue, trait.type.value[0].value);
        }

        [TestMethod]
        public void MessageBuilderForMammothItemLocale_CreateTraitCreatesBooleanFalseTraitValueWithAsAddOrUpdateAction()
        {
            // Given
            string traitCode = "FTC";
            string traitDesc = "Fair Trade Certified";
            string traitValue = "0";
            Contracts.ActionEnum? expectedAction = Contracts.ActionEnum.AddOrUpdate;

            // When
            Contracts.TraitType trait = MessageBuilderForMammothItemLocale.CreateTrait(traitValue, traitCode);

            // Then
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedAction.Value, trait.Action);
            Assert.AreEqual(expectedAction.HasValue, trait.ActionSpecified);
            Assert.AreEqual(traitCode, trait.code);
            Assert.AreEqual(traitDesc, trait.type.description);
            Assert.AreEqual(traitValue, trait.type.value[0].value);
        }

        [TestMethod]
        public void MessageBuilderForMammothItemLocale_CreateTraitCreatesBooleanNullTraitValueWithAsDeleteAction()
        {
            // Given
            string traitCode = "FTC";
            string traitDesc = "Fair Trade Certified";
            string traitValue = null;
            Contracts.ActionEnum? expectedAction = Contracts.ActionEnum.Delete;

            // When
            Contracts.TraitType trait = MessageBuilderForMammothItemLocale.CreateTrait(traitValue, traitCode);

            // Then
            Assert.IsNotNull(trait);
            Assert.AreEqual(expectedAction.Value, trait.Action);
            Assert.AreEqual(expectedAction.HasValue, trait.ActionSpecified);
            Assert.AreEqual(traitCode, trait.code);
            Assert.AreEqual(traitDesc, trait.type.description);
            Assert.AreEqual("", trait.type.value[0].value);
        }
    }
}