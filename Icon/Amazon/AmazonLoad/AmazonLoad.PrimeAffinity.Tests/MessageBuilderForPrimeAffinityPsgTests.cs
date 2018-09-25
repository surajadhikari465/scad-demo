using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using System.Collections.Generic;
using System.IO;
using Icon.Framework;

namespace AmazonLoad.PrimeAffinityPsg.Tests
{
    [TestClass]
    public class MessageBuilderForPrimeAffinityPsgTests
    {
        TestData testData = new TestData();
        const string primeAffinityPSG_GroupId = "PrimeAffinityPSG";
        const string primeAffinityPSG_GroupName = "PrimeAffinityPSG";
        const string primeAffinityPSG_GroupType = "Consumable";

        private void AssertPrimeAffinityPsgAsExpected(Contracts.GroupTypeType psg, Contracts.ActionEnum expectedAction)
        {
            Assert.AreEqual(true, psg.ActionSpecified);
            Assert.AreEqual(expectedAction, psg.Action);
            Assert.AreEqual(primeAffinityPSG_GroupId, psg.id);
            Assert.AreEqual(primeAffinityPSG_GroupName, psg.name);
            Assert.AreEqual(primeAffinityPSG_GroupType, psg.type);
        }

        private void AssertLocaleTypeAsExpected(Contracts.LocaleType store, string expectedBusinessUnit, string expectedLocaleName)
        {
            var expectedLocaleTypeCode = Contracts.LocaleCodeType.STR.ToString(); // "STR"
            var expectedLocaleTypeDesc = Contracts.LocaleDescType.Store.ToString(); // "Store"
            var expectedLocaleTraitCode = TraitCodes.PsBusinessUnitId; // "BU"
            var expectedLocaleTraitTypeDesc = TraitDescriptions.PsBusinessUnitId; // "PS Business Unit ID"

            Assert.AreEqual(expectedBusinessUnit, store.id);
            Assert.AreEqual(expectedLocaleName, store.name);

            Contracts.LocaleTypeType storeType = store.type;
            Assert.AreEqual(Contracts.LocaleCodeType.STR.ToString(), storeType.code.ToString());
            Assert.AreEqual(Contracts.LocaleDescType.Store.ToString(), storeType.description.ToString());

            Contracts.TraitType storeBuTrait = store.traits.FirstOrDefault();
            Assert.AreEqual(expectedLocaleTraitCode, storeBuTrait.code);
            Contracts.TraitTypeType storeBuTraitType = storeBuTrait.type;
            Assert.AreEqual(expectedLocaleTraitTypeDesc, storeBuTraitType.description);
            var traitValue = storeBuTraitType.value.FirstOrDefault().value;
            Assert.AreEqual(expectedBusinessUnit, traitValue);
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_CreatePrimeAffinitySelectionGroup_CreatesExpectedSelectionGroups_WhenPrimeEligible()
        {
            // Given
            var testPrimeAffinityItem = testData.Item_FL_10130_Prime_C;
            Assert.IsTrue(testPrimeAffinityItem.PrimeEligible);

            // When
            Contracts.SelectionGroupsType selectionGroups = MessageBuilderForPrimeAffinityPsg.CreatePrimeAffinitySelectionGroup(
                testPrimeAffinityItem.PrimeEligible, primeAffinityPSG_GroupId, primeAffinityPSG_GroupName, primeAffinityPSG_GroupType);

            // Then
            Assert.IsNotNull(selectionGroups);
            var group = selectionGroups.group.FirstOrDefault();
            AssertPrimeAffinityPsgAsExpected(group, Contracts.ActionEnum.AddOrUpdate);
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_CreatePrimeAffinitySelectionGroup_CreatesExpectedSelectionGroups_WhenNotPrimeEligible()
        {
            // Given
            var testPrimeAffinityItem = testData.Item_FL_10130_NonPrime_B;
            Assert.IsFalse(testPrimeAffinityItem.PrimeEligible);

            // When
            Contracts.SelectionGroupsType selectionGroups = MessageBuilderForPrimeAffinityPsg.CreatePrimeAffinitySelectionGroup(
                testPrimeAffinityItem.PrimeEligible, primeAffinityPSG_GroupId, primeAffinityPSG_GroupName, primeAffinityPSG_GroupType);

            // Then
            Assert.IsNotNull(selectionGroups);
            var group = selectionGroups.group.FirstOrDefault();
            AssertPrimeAffinityPsgAsExpected(group, Contracts.ActionEnum.Delete);
        }
        
        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_ConvertToContractsItemType_ReturnsItemTypeAsExpected_ForNonGpmNonPrimeItem1()
        {
            // Given
            var testPrimeAffinityItem = testData.Item_MA_10181_NonPrime_A;

            // When
            Contracts.ItemType itemType = MessageBuilderForPrimeAffinityPsg.ConvertToContractsItemType(testPrimeAffinityItem);

            // Then
            Assert.IsNotNull(itemType);
            Assert.AreEqual(testPrimeAffinityItem.ItemId, itemType.id);
            Contracts.LocaleType store = itemType.locale.First();
            AssertLocaleTypeAsExpected(store, testPrimeAffinityItem.BusinessUnit.ToString(), testPrimeAffinityItem.LocaleName);
            Contracts.StoreItemAttributesType siAttribs = store.Item as Contracts.StoreItemAttributesType;
            Assert.AreEqual(testPrimeAffinityItem.ScanCode, siAttribs.scanCode.First().code);
            // should be a single selection group for PrimeAffinityPSG
            var group = siAttribs.selectionGroups.group.FirstOrDefault();
            AssertPrimeAffinityPsgAsExpected(group, Contracts.ActionEnum.Delete);
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_ConvertToContractsItemType_ReturnsItemTypeAsExpected_ForNonGpmNonPrimeItem2()
        {
            // Given
            var testPrimeAffinityItem = testData.Item_MA_10181_NonPrime_B;

            // When
            Contracts.ItemType itemType = MessageBuilderForPrimeAffinityPsg.ConvertToContractsItemType(testPrimeAffinityItem);

            // Then
            Assert.IsNotNull(itemType);
            Assert.AreEqual(testPrimeAffinityItem.ItemId, itemType.id);
            Contracts.LocaleType store = itemType.locale.First();
            AssertLocaleTypeAsExpected(store, testPrimeAffinityItem.BusinessUnit.ToString(), testPrimeAffinityItem.LocaleName);
            Contracts.StoreItemAttributesType siAttribs = store.Item as Contracts.StoreItemAttributesType;
            Assert.AreEqual(testPrimeAffinityItem.ScanCode, siAttribs.scanCode.First().code);
            // should be a single selection group for PrimeAffinityPSG
            var group = siAttribs.selectionGroups.group.FirstOrDefault();
            AssertPrimeAffinityPsgAsExpected(group, Contracts.ActionEnum.Delete);
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_ConvertToContractsItemType_ReturnsItemTypeAsExpected_ForNonGpmPrimeItem1()
        {
            // Given
            var testPrimeAffinityItem = testData.Item_MA_10181_Prime_C;

            // When
            Contracts.ItemType itemType = MessageBuilderForPrimeAffinityPsg.ConvertToContractsItemType(testPrimeAffinityItem);

            // Then
            Assert.IsNotNull(itemType);
            Assert.AreEqual(testPrimeAffinityItem.ItemId, itemType.id);
            Contracts.LocaleType store = itemType.locale.First();
            AssertLocaleTypeAsExpected(store, testPrimeAffinityItem.BusinessUnit.ToString(), testPrimeAffinityItem.LocaleName);
            Contracts.StoreItemAttributesType siAttribs = store.Item as Contracts.StoreItemAttributesType;
            Assert.AreEqual(testPrimeAffinityItem.ScanCode, siAttribs.scanCode.First().code);
            // should be a single selection group for PrimeAffinityPSG
            var group = siAttribs.selectionGroups.group.FirstOrDefault();
            AssertPrimeAffinityPsgAsExpected(group, Contracts.ActionEnum.AddOrUpdate);
        }
        
        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_ConvertToContractsItemType_ReturnsItemTypeAsExpected_ForNonGpmPrimeItem2()
        {
            // Given
            var testPrimeAffinityItem = testData.Item_MA_10181_Prime_D;

            // When
            Contracts.ItemType itemType = MessageBuilderForPrimeAffinityPsg.ConvertToContractsItemType(testPrimeAffinityItem);

            // Then
            Assert.IsNotNull(itemType);
            Assert.AreEqual(testPrimeAffinityItem.ItemId, itemType.id);
            Contracts.LocaleType store = itemType.locale.First();
            AssertLocaleTypeAsExpected(store, testPrimeAffinityItem.BusinessUnit.ToString(), testPrimeAffinityItem.LocaleName);
            Contracts.StoreItemAttributesType siAttribs = store.Item as Contracts.StoreItemAttributesType;
            Assert.AreEqual(testPrimeAffinityItem.ScanCode, siAttribs.scanCode.First().code);
            // should be a single selection group for PrimeAffinityPSG
            var group = siAttribs.selectionGroups.group.FirstOrDefault();
            AssertPrimeAffinityPsgAsExpected(group, Contracts.ActionEnum.AddOrUpdate);
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_ConvertToContractsItemType_ReturnsItemTypeAsExpected_ForGpmNonPrimeItem1()
        {
            // Given
            var testPrimeAffinityItem = testData.Item_FL_10130_NonPrime_A;

            // When
            Contracts.ItemType itemType = MessageBuilderForPrimeAffinityPsg.ConvertToContractsItemType(testPrimeAffinityItem);

            // Then
            Assert.IsNotNull(itemType);
            Assert.AreEqual(testPrimeAffinityItem.ItemId, itemType.id);
            Contracts.LocaleType store = itemType.locale.First();
            AssertLocaleTypeAsExpected(store, testPrimeAffinityItem.BusinessUnit.ToString(), testPrimeAffinityItem.LocaleName);
            Contracts.StoreItemAttributesType siAttribs = store.Item as Contracts.StoreItemAttributesType;
            Assert.AreEqual(testPrimeAffinityItem.ScanCode, siAttribs.scanCode.First().code);
            // should be a single selection group for PrimeAffinityPSG
            var group = siAttribs.selectionGroups.group.FirstOrDefault();
            AssertPrimeAffinityPsgAsExpected(group, Contracts.ActionEnum.Delete);
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_ConvertToContractsItemType_ReturnsItemTypeAsExpected_ForGpmNonPrimeItem2()
        {
            // Given
            var testPrimeAffinityItem = testData.Item_FL_10130_NonPrime_B;

            // When
            Contracts.ItemType itemType = MessageBuilderForPrimeAffinityPsg.ConvertToContractsItemType(testPrimeAffinityItem);

            // Then
            Assert.IsNotNull(itemType);
            Assert.AreEqual(testPrimeAffinityItem.ItemId, itemType.id);
            Contracts.LocaleType store = itemType.locale.First();
            AssertLocaleTypeAsExpected(store, testPrimeAffinityItem.BusinessUnit.ToString(), testPrimeAffinityItem.LocaleName);
            Contracts.StoreItemAttributesType siAttribs = store.Item as Contracts.StoreItemAttributesType;
            Assert.AreEqual(testPrimeAffinityItem.ScanCode, siAttribs.scanCode.First().code);
            // should be a single selection group for PrimeAffinityPSG
            var group = siAttribs.selectionGroups.group.FirstOrDefault();
            AssertPrimeAffinityPsgAsExpected(group, Contracts.ActionEnum.Delete);
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_ConvertToContractsItemType_ReturnsItemTypeAsExpected_ForGpmPrimeItem1()
        {
            // Given
            var testPrimeAffinityItem = testData.Item_FL_10130_Prime_C;

            // When
            Contracts.ItemType itemType = MessageBuilderForPrimeAffinityPsg.ConvertToContractsItemType(testPrimeAffinityItem);

            // Then
            Assert.IsNotNull(itemType);
            Assert.AreEqual(testPrimeAffinityItem.ItemId, itemType.id);
            Contracts.LocaleType store = itemType.locale.First();
            AssertLocaleTypeAsExpected(store, testPrimeAffinityItem.BusinessUnit.ToString(), testPrimeAffinityItem.LocaleName);
            Contracts.StoreItemAttributesType siAttribs = store.Item as Contracts.StoreItemAttributesType;
            Assert.AreEqual(testPrimeAffinityItem.ScanCode, siAttribs.scanCode.First().code);
            // should be a single selection group for PrimeAffinityPSG
            var group = siAttribs.selectionGroups.group.FirstOrDefault();
            AssertPrimeAffinityPsgAsExpected(group, Contracts.ActionEnum.AddOrUpdate);
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_ConvertToContractsItemType_ReturnsItemTypeAsExpected_ForGpmPrimeItem2()
        {
            // Given
            var testPrimeAffinityItem = testData.Item_FL_10130_Prime_D;

            // When
            Contracts.ItemType itemType = MessageBuilderForPrimeAffinityPsg.ConvertToContractsItemType(testPrimeAffinityItem);

            // Then
            Assert.IsNotNull(itemType);
            Assert.AreEqual(testPrimeAffinityItem.ItemId, itemType.id);
            Contracts.LocaleType store = itemType.locale.First();
            AssertLocaleTypeAsExpected(store, testPrimeAffinityItem.BusinessUnit.ToString(), testPrimeAffinityItem.LocaleName);
            Contracts.StoreItemAttributesType siAttribs = store.Item as Contracts.StoreItemAttributesType;
            Assert.AreEqual(testPrimeAffinityItem.ScanCode, siAttribs.scanCode.First().code);
            // should be a single selection group for PrimeAffinityPSG
            var group = siAttribs.selectionGroups.group.FirstOrDefault();
            AssertPrimeAffinityPsgAsExpected(group, Contracts.ActionEnum.AddOrUpdate);
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_BuildMessage_ReturnsXmlStringAsExpected_ForNonGpmNonPrimeItem1()
        {
            // Given
            var testPrimeAffinityItems = new List<PrimeAffinityPsgModel> {
                testData.Item_MA_10181_NonPrime_A,
            };
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_MA_10181_NonPrime_A.xml");

            // When
            string actualMsg = MessageBuilderForPrimeAffinityPsg.BuildMessage(testPrimeAffinityItems,
                primeAffinityPSG_GroupId, primeAffinityPSG_GroupName, primeAffinityPSG_GroupType);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_BuildMessage_ReturnsXmlStringAsExpected_ForNonGpmNonPrimeItem2()
        {
            // Given
            var testPrimeAffinityItems = new List<PrimeAffinityPsgModel> {
                testData.Item_MA_10181_NonPrime_B,
            };
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_MA_10181_NonPrime_B.xml");

            // When
            string actualMsg = MessageBuilderForPrimeAffinityPsg.BuildMessage(testPrimeAffinityItems,
                primeAffinityPSG_GroupId, primeAffinityPSG_GroupName, primeAffinityPSG_GroupType);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_BuildMessage_ReturnsXmlStringAsExpected_ForNonGpmPrimeItem1()
        {
            // Given
            var testPrimeAffinityItems = new List<PrimeAffinityPsgModel> {
                testData.Item_MA_10181_Prime_C,
            };
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_MA_10181_Prime_C.xml");

            // When
            string actualMsg = MessageBuilderForPrimeAffinityPsg.BuildMessage(testPrimeAffinityItems,
                primeAffinityPSG_GroupId, primeAffinityPSG_GroupName, primeAffinityPSG_GroupType);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_BuildMessage_ReturnsXmlStringAsExpected_ForNonGpmPrimeItem2()
        {
            // Given
            var testPrimeAffinityItems = new List<PrimeAffinityPsgModel> {
                testData.Item_MA_10181_Prime_D,
            };
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_MA_10181_Prime_D.xml");

            // When
            string actualMsg = MessageBuilderForPrimeAffinityPsg.BuildMessage(testPrimeAffinityItems,
                primeAffinityPSG_GroupId, primeAffinityPSG_GroupName, primeAffinityPSG_GroupType);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_BuildMessage_ReturnsXmlStringAsExpected_ForGpmNonPrimeItem1()
        {
            // Given
            var testPrimeAffinityItems = new List<PrimeAffinityPsgModel> {
                testData.Item_FL_10130_NonPrime_A,
            };
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_FL_10130_NonPrime_A.xml");

            // When
            string actualMsg = MessageBuilderForPrimeAffinityPsg.BuildMessage(testPrimeAffinityItems,
                primeAffinityPSG_GroupId, primeAffinityPSG_GroupName, primeAffinityPSG_GroupType);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_BuildMessage_ReturnsXmlStringAsExpected_ForGpmNonPrimeItem2()
        {
            // Given
            var testPrimeAffinityItems = new List<PrimeAffinityPsgModel> {
                testData.Item_FL_10130_NonPrime_B,
            };
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_FL_10130_NonPrime_B.xml");

            // When
            string actualMsg = MessageBuilderForPrimeAffinityPsg.BuildMessage(testPrimeAffinityItems,
                primeAffinityPSG_GroupId, primeAffinityPSG_GroupName, primeAffinityPSG_GroupType);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_BuildMessage_ReturnsXmlStringAsExpected_ForGpmPrimeItem1()
        {
            // Given
            var testPrimeAffinityItems = new List<PrimeAffinityPsgModel> {
                testData.Item_FL_10130_Prime_C,
            };
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_FL_10130_Prime_C.xml");

            // When
            string actualMsg = MessageBuilderForPrimeAffinityPsg.BuildMessage(testPrimeAffinityItems,
                primeAffinityPSG_GroupId, primeAffinityPSG_GroupName, primeAffinityPSG_GroupType);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_BuildMessage_ReturnsXmlStringAsExpected_ForGpmPrimeItem2()
        {
            // Given
            var testPrimeAffinityItems = new List<PrimeAffinityPsgModel> {
                testData.Item_FL_10130_Prime_D,
            };
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_FL_10130_Prime_D.xml");

            // When
            string actualMsg = MessageBuilderForPrimeAffinityPsg.BuildMessage(testPrimeAffinityItems,
                primeAffinityPSG_GroupId, primeAffinityPSG_GroupName, primeAffinityPSG_GroupType);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_BuildMessage_ReturnsXmlStringAsExpected_ForNonGpmMultipleItems()
        {
            // Given
            var testPrimeAffinityItems = new List<PrimeAffinityPsgModel> {
                testData.Item_MA_10181_NonPrime_A,
                testData.Item_MA_10181_NonPrime_B,
                testData.Item_MA_10181_Prime_C,
                testData.Item_MA_10181_Prime_D
            };
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_MA_10181_2Prime2Non.xml");

            // When
            string actualMsg = MessageBuilderForPrimeAffinityPsg.BuildMessage(testPrimeAffinityItems,
                primeAffinityPSG_GroupId, primeAffinityPSG_GroupName, primeAffinityPSG_GroupType);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }

        [TestMethod]
        public void MessageBuilderForPrimeAffinityPsg_BuildMessage_ReturnsXmlStringAsExpected_ForGpmMultipleItems()
        {
            // Given
            var testPrimeAffinityItems = new List<PrimeAffinityPsgModel> {
                testData.Item_FL_10130_Prime_C,
                testData.Item_FL_10130_Prime_D,
                testData.Item_FL_10130_NonPrime_A,
                testData.Item_FL_10130_NonPrime_B
            };
            var expectedMsg = File.ReadAllText("ExpectedTestMessage_FL_10130_2Prime2Non.xml");

            // When
            string actualMsg = MessageBuilderForPrimeAffinityPsg.BuildMessage(testPrimeAffinityItems,
                primeAffinityPSG_GroupId, primeAffinityPSG_GroupName, primeAffinityPSG_GroupType);

            // Then
            Assert.AreEqual(expectedMsg, actualMsg, "esb xml message");
        }
    }
}
