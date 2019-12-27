using Dapper;
using Icon.Common;
using Icon.Framework;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddItemsCommandHandlerTests
    {
        private AddItemCommandHandler commandHandler;
        private AddItemCommand command;
        private SqlDbProvider sqlDbProvider;
        private TransactionScope transaction;
        private int testBrandHierarchyClassId;
        private int testFinancialHierarchyClassId;
        private string testItemAttributesJson;
        private int testItemTypeId;
        private int testManufacturerHierarchyClassId;
        private int testMerchandiseHierarchyClassId;
        private int testTaxHierarchyClassId;
        private int testNationalHierarchyClassId;
        private int testBarcodeTypeId;
        private string testScanCode;
        private ItemDbModel expectedItem;

        [TestInitialize]
        public void Initialize()
        {
            this.sqlDbProvider = new SqlDbProvider();
            this.sqlDbProvider.Connection = TestHelper.Icon;
            this.transaction = new TransactionScope();
            commandHandler = new AddItemCommandHandler(this.sqlDbProvider);
            command = new AddItemCommand();

            testBarcodeTypeId = TestHelper.AddBarcodeType(this.sqlDbProvider.Connection as SqlConnection);
            testBrandHierarchyClassId = TestHelper.AddHierarchyClass(this.sqlDbProvider.Connection as SqlConnection, Hierarchies.Brands);
            testFinancialHierarchyClassId = TestHelper.AddHierarchyClass(this.sqlDbProvider.Connection as SqlConnection, Hierarchies.Financial);
            testManufacturerHierarchyClassId = TestHelper.AddHierarchyClass(this.sqlDbProvider.Connection as SqlConnection, Hierarchies.Manufacturer);

            int merchLevelOne = TestHelper.AddHierarchyClass(this.sqlDbProvider.Connection as SqlConnection, Hierarchies.Merchandise, "Unit Test MerchLevel1", 1);
            int merchLevelTwo = TestHelper.AddHierarchyClass(this.sqlDbProvider.Connection as SqlConnection, Hierarchies.Merchandise, "Unit Test MerchLevel2", 2, merchLevelOne);
            int merchLevelThree = TestHelper.AddHierarchyClass(this.sqlDbProvider.Connection as SqlConnection, Hierarchies.Merchandise, "Unit Test MerchLevel3", 3, merchLevelTwo);
            int merchLevelFour = TestHelper.AddHierarchyClass(this.sqlDbProvider.Connection as SqlConnection, Hierarchies.Merchandise, "Unit Test MerchLevel4", 4, merchLevelThree);
            testMerchandiseHierarchyClassId = TestHelper.AddHierarchyClass(
                this.sqlDbProvider.Connection as SqlConnection, Hierarchies.Merchandise, "Unit Test MerchLevel5", 5, merchLevelFour);

            int nationalLevelOne = TestHelper.AddHierarchyClass(this.sqlDbProvider.Connection as SqlConnection, Hierarchies.National, "Test National 1", 1);
            int nationalLevelTwo = TestHelper.AddHierarchyClass(this.sqlDbProvider.Connection as SqlConnection, Hierarchies.National, "Test National 2", 2, nationalLevelOne);
            int nationalLevelThree = TestHelper.AddHierarchyClass(this.sqlDbProvider.Connection as SqlConnection, Hierarchies.National, "Test National 3", 3, nationalLevelTwo);
            testNationalHierarchyClassId = TestHelper.AddHierarchyClass(
                this.sqlDbProvider.Connection as SqlConnection, Hierarchies.National, "Test National 4", 4, nationalLevelThree);

            testTaxHierarchyClassId = TestHelper.AddHierarchyClass(this.sqlDbProvider.Connection as SqlConnection, Hierarchies.Tax);
            testItemTypeId = TestHelper.AddItemType(this.sqlDbProvider.Connection as SqlConnection);
            testItemAttributesJson = "{\"CreatedBy\":\"ICON\",\"CreatedDateTimeUtc\":\"2014-06-06T13:25:38.22Z\"," +
                "\"CustomerFriendlyDescription\":\"Test Update Item Command Description\",\"FoodStampEligible\":\"false\"," +
                "\"ItemPack\":\"1\",\"Inactive\":\"false\",\"ModifiedBy\":\"1013359@wholefoods.com\",\"ModifiedDateTimeUtc\":\"2018-09-06T12:05:00.133975Z\"," +
                "\"POSDescription\":\"Test Update Item Command Description\",\"POSScaleTare\":\"0\"," +
                "\"ProductDescription\":\"Test Update Item Command Description\",\"ProhibitDiscount\":\"false\",\"RetailSize\":\"1\",\"UOM\":\"EA\"}";
            expectedItem = new ItemDbModel();

            TestHelper.AddHierarchyClassTrait(this.sqlDbProvider.Connection as SqlConnection, testNationalHierarchyClassId, Traits.NationalClassCode, "8754651");
            TestHelper.AddHierarchyClassTrait(
                this.sqlDbProvider.Connection as SqlConnection, testMerchandiseHierarchyClassId, Traits.MerchFinMapping, testFinancialHierarchyClassId.ToString());
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            this.sqlDbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void AddItemCommandHandler_ItemWithScanCodeProvidedWithAllRequiredAttributesSet_ItemAdded()
        {
            //Given
            testScanCode = "9000000000000";
            expectedItem = CreateTestItem(testScanCode, testBarcodeTypeId);
            MapToAddItemCommand(expectedItem);

            //When
            commandHandler.Execute(command);

            //Then
            var actualItem = GetAddItemModel(this.command.ItemId);
            AssertActualItemsEqualExpectedItems(expectedItem, actualItem, testScanCode);
            AssertScanCodesAreAssignedInBarcodeTypeRangePool(testBarcodeTypeId, testScanCode);
        }

        [TestMethod]
        public void AddItemCommandHandler_ItemWithScanCodeAssignedWithAllRequiredAttributesSet_MessageQueueItemRecordAdded()
        {
            //Given
            testScanCode = "9000000000000";
            expectedItem = CreateTestItem(testScanCode, testBarcodeTypeId);
            MapToAddItemCommand(expectedItem);

            //When
            commandHandler.Execute(command);

            //Then
            var messageQueueItem = this.sqlDbProvider.Connection.Query<MessageQueueItem>(
                    "SELECT * FROM esb.MessageQueueItem WHERE ItemId = @ItemId",
                    new { ItemId = this.command.ItemId },
                    this.sqlDbProvider.Transaction)
                .ToList();

            Assert.IsTrue(messageQueueItem.Count() > 0, "MessageQueueItem record was not added with AddItemCommandHandler.");
        }

        [TestMethod]
        public void AddItemCommandHandler_ItemWithScanCodeAssignedWithAllRequiredAttributesSet_IrmaEventQueueRecordAdded()
        {
            //Given
            testScanCode = "9000000000000";
            expectedItem = CreateTestItem(testScanCode, testBarcodeTypeId);
            MapToAddItemCommand(expectedItem);
            TestHelper.AddIrmaItemSubscription(this.sqlDbProvider.Connection as SqlConnection, testScanCode, "MA");

            //When
            commandHandler.Execute(command);

            //Then
            var eventQueue = this.sqlDbProvider.Connection.Query<MessageQueueItem>(
                    "SELECT * FROM app.EventQueue WHERE EventReferenceId = @ItemId AND ProcessFailedDate IS NULL ORDER BY InsertDate DESC",
                    new { ItemId = this.command.ItemId },
                    this.sqlDbProvider.Transaction)
                .ToList();

            Assert.IsTrue(eventQueue.Count() > 0, "EventQueue record was not added with AddItemCommandHandler.");
        }

        [TestMethod]
        public void AddItemCommandHandler_ItemWithNoScanCodeProvidedWithAllRequiredAttributesSet_ItemAdded()
        {
            //Given
            testScanCode = "9000000000000";
            expectedItem = CreateTestItem(scanCode: testScanCode, barcodeTypeId: testBarcodeTypeId, addScanCodeToRangePool: true, addScanCodeToItem: false);
            MapToAddItemCommand(expectedItem);

            //When
            commandHandler.Execute(command);

            //Then
            var actualItem = GetAddItemModel(this.command.ItemId);
            AssertActualItemsEqualExpectedItems(expectedItem, actualItem, testScanCode);
            AssertScanCodesAreAssignedInBarcodeTypeRangePool(testBarcodeTypeId, testScanCode);
        }

        [TestMethod]
        public void AddItemCommandHandler_ItemWithNoScanCodeProvidedWithAllRequiredAttributesSet_MessageQueueItemRecordAdded()
        {
            //Given
            testScanCode = "9000000000000";
            expectedItem = CreateTestItem(scanCode: testScanCode, barcodeTypeId: testBarcodeTypeId, addScanCodeToRangePool: true, addScanCodeToItem: false);
            MapToAddItemCommand(expectedItem);

            //When
            commandHandler.Execute(command);

            //Then
            var messageQueueItem = this.sqlDbProvider.Connection.Query<MessageQueueItem>(
                    "SELECT * FROM esb.MessageQueueItem WHERE ItemId = @ItemId",
                    new { ItemId = this.command.ItemId },
                    this.sqlDbProvider.Transaction)
                .ToList();

            Assert.IsTrue(messageQueueItem.Count() > 0, "MessageQueueItem record was not added with AddItemCommandHandler.");
        }

        [TestMethod]
        public void AddItemCommandHandler_ItemWithNoScanCodeProvidedWithAllRequiredAttributesSet_IrmaEventQueueRecordAdded()
        {
            //Given
            testScanCode = "9000000000000";
            expectedItem = CreateTestItem(scanCode: testScanCode, barcodeTypeId: testBarcodeTypeId, addScanCodeToRangePool: true, addScanCodeToItem: false);
            MapToAddItemCommand(expectedItem);
            TestHelper.AddIrmaItemSubscription(this.sqlDbProvider.Connection as SqlConnection, testScanCode, "MA");

            //When
            commandHandler.Execute(command);

            //Then
            var eventQueue = this.sqlDbProvider.Connection.Query<MessageQueueItem>(
                    "SELECT * FROM app.EventQueue WHERE EventReferenceId = @ItemId AND ProcessFailedDate IS NULL ORDER BY InsertDate DESC",
                    new { ItemId = this.command.ItemId },
                    this.sqlDbProvider.Transaction)
                .ToList();

            Assert.IsTrue(eventQueue.Count() > 0, "EventQueue record was not added with AddItemCommandHandler.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddItemCommandHandler_BarcodeRangeFull_ThrowsArgumentException()
        {
            //Given
            testScanCode = "9000000000000";
            expectedItem = CreateTestItem(testScanCode, testBarcodeTypeId, false, false);
            MapToAddItemCommand(expectedItem);

            //When
            commandHandler.Execute(command);

            //Then
            // Expected Argument Exception
        }

        private void AssertItemDoesNotExist(string scanCode)
        {
            var existingScanCode = this.sqlDbProvider.Connection.QueryFirstOrDefault<string>(
                "SELECT ScanCode FROM ScanCode sc WHERE sc.ScanCode = @ScanCode",
                new { ScanCode = scanCode });
            Assert.IsNull(existingScanCode);
        }

        private void AssertScanCodesAreAssignedInBarcodeTypeRangePool(int barcodeTypeId, string testScanCode)
        {
            var scanCodes = new List<string>{ testScanCode };
            var areAssigned = this.sqlDbProvider.Connection.QueryFirst<bool>(
                @"IF EXISTS (
		                SELECT 1
		                FROM dbo.BarcodeTypeRangePool btrp
		                WHERE btrp.BarcodeTypeId = @BarcodeTypeId
                            AND Assigned = 1
			                AND btrp.ScanCode IN (
				                SELECT ScanCode
				                FROM @ScanCodes
				                )
		                )
	                SELECT CAST(1 AS BIT)
                ELSE
	                SELECT CAST(0 AS BIT)",
                new
                {
                    BarcodeTypeId = barcodeTypeId,
                    ScanCodes = scanCodes
                        .Select(sc => new { ScanCode = sc })
                        .ToDataTable()
                        .AsTableValuedParameter("app.ScanCodeListType")
                });
            Assert.IsTrue(areAssigned);
        }

        private void AssertActualItemsEqualExpectedItems(ItemDbModel testExpectedItem, ItemDbModel testActualItem, string testScanCode)
        {
            Assert.AreEqual(testExpectedItem.BarcodeTypeId, testActualItem.BarcodeTypeId);
            Assert.AreEqual(testExpectedItem.BrandsHierarchyClassId, testActualItem.BrandsHierarchyClassId);
            Assert.AreEqual(testExpectedItem.FinancialHierarchyClassId, testActualItem.FinancialHierarchyClassId);
            Assert.AreEqual(testExpectedItem.ItemAttributesJson, testActualItem.ItemAttributesJson);
            Assert.AreEqual(testExpectedItem.ItemTypeId, testActualItem.ItemTypeId);
            Assert.AreEqual(testExpectedItem.ManufacturerHierarchyClassId, testActualItem.ManufacturerHierarchyClassId);
            Assert.AreEqual(testExpectedItem.MerchandiseHierarchyClassId, testActualItem.MerchandiseHierarchyClassId);
            Assert.AreEqual(testExpectedItem.NationalHierarchyClassId, testActualItem.NationalHierarchyClassId);
            Assert.AreEqual(testExpectedItem.TaxHierarchyClassId, testActualItem.TaxHierarchyClassId);
        }

        private ItemDbModel CreateTestItem(string scanCode, int barcodeTypeId, bool addScanCodeToRangePool = true, bool addScanCodeToItem = false)
        {
            if (addScanCodeToRangePool)
                TestHelper.AddScanCodeToBarcodeTypeRangePool(this.sqlDbProvider.Connection as SqlConnection, barcodeTypeId, scanCode);

            return new ItemDbModel
            {
                BarcodeTypeId = barcodeTypeId,
                BrandsHierarchyClassId = testBrandHierarchyClassId,
                FinancialHierarchyClassId = testFinancialHierarchyClassId,
                ItemAttributesJson = testItemAttributesJson,
                ItemTypeId = testItemTypeId,
                ManufacturerHierarchyClassId = testManufacturerHierarchyClassId,
                MerchandiseHierarchyClassId = testMerchandiseHierarchyClassId,
                NationalHierarchyClassId = testNationalHierarchyClassId,
                ScanCode = addScanCodeToItem ? scanCode : null,
                TaxHierarchyClassId = testTaxHierarchyClassId
            };
        }

        private List<ItemDbModel> GetItems(int barcodeTypeId)
        {
            return this.sqlDbProvider.Connection.Query<ItemDbModel>(
                @"SELECT * FROM dbo.ItemView i WHERE i.BarcodeTypeId = @BarcodeTypeId",
                new { BarcodeTypeId = barcodeTypeId })
                .ToList();
        }

        private List<ItemDbModel> GetItems(List<string> scanCodes)
        {
            return this.sqlDbProvider.Connection.Query<ItemDbModel>(
                @"SELECT * FROM dbo.ItemView i WHERE i.ScanCode IN (SELECT sc.ScanCode FROM @ScanCodes sc)",
                new
                {
                    ScanCodes = scanCodes
                        .Select(sc => new { ScanCode = sc })
                        .ToDataTable()
                        .AsTableValuedParameter("app.ScanCodeListType")
                })
                .ToList();
        }

        private ItemDbModel GetAddItemModel(int itemId)
        {
            return this.sqlDbProvider.Connection.QuerySingle<ItemDbModel>(
                @"SELECT * FROM dbo.ItemView i WHERE i.ItemId = @itemId",
                new { ItemId = itemId });
        }

        private void MapToAddItemCommand(ItemDbModel item)
        {
            this.command.BrandsHierarchyClassId = item.BrandsHierarchyClassId;
            this.command.FinancialHierarchyClassId = item.FinancialHierarchyClassId;
            this.command.ItemAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.ItemAttributesJson);
            this.command.ItemId = item.ItemId;
            this.command.ItemTypeId = item.ItemTypeId;
            this.command.ManufacturerHierarchyClassId = item.ManufacturerHierarchyClassId;
            this.command.MerchandiseHierarchyClassId = item.MerchandiseHierarchyClassId;
            this.command.NationalHierarchyClassId = item.NationalHierarchyClassId;
            this.command.ScanCode = item.ScanCode;
            this.command.SelectedBarCodeTypeId = item.BarcodeTypeId;
            this.command.TaxHierarchyClassId = item.TaxHierarchyClassId;
        }
    }
}