using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using System.Text;
using AmazonLoad.Common;

namespace AmazonLoad.IconItemLocale.Tests
{
    [TestClass]
    public class ProductSelectionGroupMapperTests
    {
        string region = "MA";
        TestData testData = new TestData();

        string TestRegion
        {
            get { return region; }
            set { region = value; }
        }

        [TestMethod]
        public void IconItemLocalePsgMapper_FilterProductSelectionGroups_FillsTraitIdToItemLocaleMessageTraitValues()
        {
            // Given
            TestRegion = "MA";
            var testPsgs = testData.TestPsgsWithNulls;
            var psgMapper = new IconItemLocalePsgMapper();

            //When 
            var filteredPsgs = psgMapper.FilterProductSelectionGroups(testPsgs);

            // Then
            Assert.IsNotNull(filteredPsgs);
            // expect to see only traits which have non-null trait ids/values
            Assert.AreEqual(7, filteredPsgs.Count);
        }

        [TestMethod]
        public void IconItemLocalePsgMapper_LoadTraitIdToItemLocaleMessageTraitValues_BuildsTraitDictionary()
        {
            // Given
            TestRegion = "MA";
            var testPsgs = testData.TestPsgs;
            var psgMapper = new IconItemLocalePsgMapper();

            //When 
            var traitIdValueDictionary = psgMapper.LoadTraitIdToItemLocaleMessageTraitValues(testPsgs);

            // Then
            Assert.IsNotNull(traitIdValueDictionary);
            // expect to see only traits which apply to the defined PSGs 
            // Prohibit_Case_Discount, Restrict 18/Restrict 21, DateRestriction
            Assert.AreEqual(4, traitIdValueDictionary.Count);
        }

        [TestMethod]
        public void IconItemLocalePsgMapper_LoadProductSelectionGroups_FillsTraitProductSelectionGroups()
        {
            // Given
            TestRegion = "MA";
            var testPsgs = testData.TestPsgs;

            var psgMapper = new FakeIconItemLocalePsgMapper(testPsgs);

            //When 
            psgMapper.LoadProductSelectionGroups();

            // Then
            Assert.IsNotNull(psgMapper.TraitProductSelectionGroups);
            // expect to see:  all the app.ProductSelectionGroup entries with non-null trait ids & values
            //      CaseDiscountEligible, DateRestriction, Prohibit_Case_Discount,
            //     Prohibit_Discount_Items, Prohibit_TM_Discount, Restrict 18, Restrict 21
            Assert.AreEqual(7, psgMapper.TraitProductSelectionGroups.Count);
        }

        [TestMethod]
        public void IconItemLocalePsgMapper_LoadProductSelectionGroups_FillsTraitIdToItemLocaleMessageTraitValues()
        {
            // Given
            TestRegion = "MA";
            var testPsgs = testData.TestPsgs;

            var psgMapper = new FakeIconItemLocalePsgMapper(testPsgs);

            //When 
            psgMapper.LoadProductSelectionGroups();

            // Then
            Assert.IsNotNull(psgMapper.TraitIdToItemLocaleMessageTraitValues);
            // expect to see only traits which apply to the defined PSGs already loaded
            // Prohibit_Case_Discount, Restrict 18/Restrict 21, DateRestriction
            Assert.AreEqual(4, psgMapper.TraitIdToItemLocaleMessageTraitValues.Count);
        }

        [TestMethod]
        public void IconItemLocalePsgMapper_CreatePsgElementsForTraits_CreatesPsgElementForFalseTraits()
        {
            // Given
            TestRegion = "MA";
            var testPsgs = testData.TestPsgs;
            var testItem = new ItemLocaleModelForWormhole()
            {
                TMDiscountEligible = false,
                Case_Discount = false,
                AgeCode = null,
                Restricted_Hours = false
            };

            var psgMapper = new FakeIconItemLocalePsgMapper(testPsgs);
            psgMapper.LoadProductSelectionGroups();

            //When 
            var createdElements = psgMapper.CreatePsgElementsForTraits(testItem);

            // Then
            Assert.IsNotNull(createdElements);
            Assert.AreEqual(6, createdElements.Count);

            var prohibitTmDiscountElement = createdElements.Single(e => e.name == "Prohibit_TM_Discount");
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, prohibitTmDiscountElement.Action);

            var caseDiscountElement = createdElements.Single(e => e.name == "CaseDiscountEligible");
            Assert.AreEqual(Contracts.ActionEnum.Delete, caseDiscountElement.Action);

            var prohibitCaseDiscountElement = createdElements.Single(e => e.name == "Prohibit_Case_Discount");
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, prohibitCaseDiscountElement.Action);

            var restrict18Element = createdElements.Single(e => e.name == "Restrict 18");
            Assert.AreEqual(Contracts.ActionEnum.Delete, restrict18Element.Action);

            var restrict21Element = createdElements.Single(e => e.name == "Restrict 21");
            Assert.AreEqual(Contracts.ActionEnum.Delete, restrict21Element.Action);

            var dateRestrictionElement = createdElements.Single(e => e.name == "DateRestriction");
            Assert.AreEqual(Contracts.ActionEnum.Delete, dateRestrictionElement.Action);
        }

        [TestMethod]
        public void IconItemLocalePsgMapper_CreatePsgElementsForTraits_CreatesPsgElementForTrueTraits()
        {
            // Given
            TestRegion = "MA";
            var testPsgs = testData.TestPsgs;
            var testItem = new ItemLocaleModelForWormhole()
            {
                TMDiscountEligible = true,
                Case_Discount = true,
                AgeCode = 2,
                Restricted_Hours = true
            };

            var psgMapper = new FakeIconItemLocalePsgMapper(testPsgs);
            psgMapper.LoadProductSelectionGroups();

            //When 
            var createdElements = psgMapper.CreatePsgElementsForTraits(testItem);

            // Then
            Assert.IsNotNull(createdElements);
            Assert.AreEqual(6, createdElements.Count);

            var prohibitTmDiscountElement = createdElements.Single(e => e.name == "Prohibit_TM_Discount");
            Assert.AreEqual(Contracts.ActionEnum.Delete, prohibitTmDiscountElement.Action);

            var caseDiscountElement = createdElements.Single(e => e.name == "CaseDiscountEligible");
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, caseDiscountElement.Action);

            var prohibitCaseDiscountElement = createdElements.Single(e => e.name == "Prohibit_Case_Discount");
            Assert.AreEqual(Contracts.ActionEnum.Delete, prohibitCaseDiscountElement.Action);

            var restrict18Element = createdElements.Single(e => e.name == "Restrict 18");
            Assert.AreEqual(Contracts.ActionEnum.Delete, restrict18Element.Action);

            var restrict21Element = createdElements.Single(e => e.name == "Restrict 21");
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, restrict21Element.Action);

            var dateRestrictionElement = createdElements.Single(e => e.name == "DateRestriction");
            Assert.AreEqual(Contracts.ActionEnum.AddOrUpdate, dateRestrictionElement.Action);
        }
    }
}
