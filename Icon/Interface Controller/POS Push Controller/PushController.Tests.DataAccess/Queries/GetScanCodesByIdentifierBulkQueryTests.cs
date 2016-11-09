using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.DataAccess.Queries;
using System.Collections.Generic;
using System.Linq;

namespace PushController.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetScanCodesByIdentifierBulkQueryTests
    {
        private GetScanCodesByIdentifierBulkQueryHandler getScanCodesQueryHandler;
        private GlobalIconContext context;
        private Mock<ILogger<GetScanCodesByIdentifierBulkQueryHandler>> mockLogger;
        private List<string> testIdentifiers;
        private ScanCode testItem;
        private ScanCode testValidatedItem;
        private ScanCode testNonMerchandiseItem;

        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());
            mockLogger = new Mock<ILogger<GetScanCodesByIdentifierBulkQueryHandler>>();

            getScanCodesQueryHandler = new GetScanCodesByIdentifierBulkQueryHandler(mockLogger.Object, context);
        }

        private void FindTestItem()
        {
            testItem = context.Context.ScanCode.First();

            testIdentifiers = new List<string> { testItem.scanCode };
        }

        private void FindTestValidatedItem()
        {
            testValidatedItem = context.Context.ScanCode.First(sc => sc.Item.ItemTrait.Any(it => it.traitID == Traits.ValidationDate));

            testIdentifiers = new List<string> { testValidatedItem.scanCode };
        }

        private void FindTestNonMerchandiseItem()
        {
            testNonMerchandiseItem = context.Context.ScanCode.First(sc =>
                sc.Item.ItemHierarchyClass.FirstOrDefault(ihc =>
                    ihc.HierarchyClass.hierarchyID == Hierarchies.Merchandise).HierarchyClass.HierarchyClassTrait.Any(hct =>
                        hct.traitID == Traits.NonMerchandise));

            testIdentifiers = new List<string> { testNonMerchandiseItem.scanCode };
        }

        [TestMethod]
        public void GetScanCodesByIdentifierBulk_NullList_WarningShouldBeLogged()
        {
            // Given.
            
            // When.
            var query = new GetScanCodesByIdentifierBulkQuery
            {
                Identifiers = null
            };

            var scanCodes = getScanCodesQueryHandler.Execute(query);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetScanCodesByIdentifierBulk_EmptyList_WarningShouldBeLogged()
        {
            // Given.

            // When.
            var query = new GetScanCodesByIdentifierBulkQuery
            {
                Identifiers = new List<string>()
            };

            var scanCodes = getScanCodesQueryHandler.Execute(query);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetScanCodesByIdentifierBulk_BasicItem_ResultsShouldContainAllRequiredInformation()
        {
            // Given.
            FindTestItem();

            // When.
            var query = new GetScanCodesByIdentifierBulkQuery
            {
                Identifiers = testIdentifiers
            };

            var scanCodes = getScanCodesQueryHandler.Execute(query);

            // Then.
            Assert.AreEqual(testItem.scanCode, scanCodes[0].ScanCode);
            Assert.AreEqual(testItem.scanCodeID, scanCodes[0].ScanCodeId);
            Assert.AreEqual(testItem.scanCodeTypeID, scanCodes[0].ScanCodeTypeId);
            Assert.AreEqual(testItem.ScanCodeType.scanCodeTypeDesc, scanCodes[0].ScanCodeTypeDesc);
            Assert.AreEqual(testItem.itemID, scanCodes[0].ItemId);
            Assert.AreEqual(testItem.Item.ItemType.itemTypeCode, scanCodes[0].ItemTypeCode);
            Assert.AreEqual(testItem.Item.ItemType.itemTypeDesc, scanCodes[0].ItemTypeDesc);
        }

        [TestMethod]
        public void GetScanCodesByIdentifierBulk_ValidatedItem_ResultsShouldContainValidationDate()
        {
            // Given.
            FindTestValidatedItem();

            // When.
            var query = new GetScanCodesByIdentifierBulkQuery
            {
                Identifiers = testIdentifiers
            };

            var scanCodes = getScanCodesQueryHandler.Execute(query);

            // Then.
            string validationDate = testValidatedItem.Item.ItemTrait.Single(it => it.traitID == Traits.ValidationDate).traitValue;

            Assert.AreEqual(validationDate, scanCodes[0].ValidationDate);
        }

        [TestMethod]
        public void GetScanCodesByIdentifierBulk_NonMerchandiseItem_ResultsShouldContainNonMerchandiseTrait()
        {
            // Given.
            FindTestNonMerchandiseItem();

            // When.
            var query = new GetScanCodesByIdentifierBulkQuery
            {
                Identifiers = testIdentifiers
            };

            var scanCodes = getScanCodesQueryHandler.Execute(query);

            // Then.
            string nonMerchandiseTrait = testNonMerchandiseItem.Item.ItemHierarchyClass.Single(ihc =>
                ihc.HierarchyClass.hierarchyID == Hierarchies.Merchandise).HierarchyClass.HierarchyClassTrait.Single(hct => hct.traitID == Traits.NonMerchandise).traitValue;

            Assert.AreEqual(nonMerchandiseTrait, scanCodes[0].NonMerchandiseTrait);
        }
    }
}
