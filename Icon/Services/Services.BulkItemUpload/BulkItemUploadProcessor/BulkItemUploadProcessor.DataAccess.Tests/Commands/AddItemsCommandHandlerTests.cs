using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.DataAccess.Commands;
using Dapper;
using Icon.Common;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace BulkItemUploadProcessor.DataAccess.Tests.Commands
{
    [TestClass]
    public class AddItemsCommandHandlerTests
    {
        private AddItemsCommandHandler commandHandler;
        private AddItemsCommand command;
        private SqlConnection sqlConnection;
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
        private List<string> testScanCodes;
        private List<AddItemModel> expectedItems;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = TestHelpers.Icon;
            commandHandler = new AddItemsCommandHandler(sqlConnection);
            command = new AddItemsCommand();

            testBarcodeTypeId = TestHelpers.AddBarcodeType(sqlConnection);
            testBrandHierarchyClassId = TestHelpers.AddHierarchyClass(sqlConnection, Hierarchies.Brands);
            testFinancialHierarchyClassId = TestHelpers.AddHierarchyClass(sqlConnection, Hierarchies.Financial);
            testManufacturerHierarchyClassId = TestHelpers.AddHierarchyClass(sqlConnection, Hierarchies.Manufacturer);
            testMerchandiseHierarchyClassId = TestHelpers.AddHierarchyClass(sqlConnection, Hierarchies.Merchandise);
            testNationalHierarchyClassId = TestHelpers.AddHierarchyClass(sqlConnection, Hierarchies.National);
            testTaxHierarchyClassId = TestHelpers.AddHierarchyClass(sqlConnection, Hierarchies.Tax);
            testItemTypeId = TestHelpers.AddItemType(sqlConnection);
            testItemAttributesJson = @"{""ProductDescription"":""Test""}";
            expectedItems = new List<AddItemModel>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            sqlConnection.Dispose();
            transaction.Dispose();
        }

        [TestMethod]
        public void AddItems_5ItemsNoScanCodeSetButWithAllRequiredAttributesSet_AddsAllItems()
        {
            //Given
            testScanCodes = new List<string> { "9000000000000", "9000000000001", "9000000000002", "9000000000003", "9000000000004" };
            testScanCodes.ForEach(sc => expectedItems.Add(CreateTestItem(sc, testBarcodeTypeId)));
            command.Items = expectedItems;

            //When
            commandHandler.Execute(command);

            //Then
            var actualItems = GetItems(testBarcodeTypeId);
            AssertActualItemsEqualExpectedItems(expectedItems, actualItems, testScanCodes);
            AssertScanCodesAreAssignedInBarcodeTypeRangePool(testBarcodeTypeId, testScanCodes);
        }

        [TestMethod]
        public void AddItems_2ItemsSameBarcodeTypeAnd1ItemHasScanCodeAssignedAndAssignedIsTopItem_AddsAllItems()
        {
            //Given
            testScanCodes = new List<string> { "9000000000000", "9000000000001" };
            expectedItems.Add(CreateTestItem(testScanCodes[0], testBarcodeTypeId, true, true));
            expectedItems.Add(CreateTestItem(testScanCodes[1], testBarcodeTypeId));
            command.Items = expectedItems;

            //When
            commandHandler.Execute(command);

            //Then
            var actualItems = GetItems(testBarcodeTypeId);
            AssertActualItemsEqualExpectedItems(expectedItems, actualItems, testScanCodes);
            AssertScanCodesAreAssignedInBarcodeTypeRangePool(testBarcodeTypeId, testScanCodes);
        }

        [TestMethod]
        public void AddItems_2ItemsSameBarcodeTypeAnd1ItemHasScanCodeAssignedAndAssignedIsBottomItem_AddsAllItems()
        {
            //Given
            testScanCodes = new List<string> { "9000000000000", "9000000000001" };
            expectedItems.Add(CreateTestItem(testScanCodes[0], testBarcodeTypeId));
            expectedItems.Add(CreateTestItem(testScanCodes[1], testBarcodeTypeId, true, true));
            command.Items = expectedItems;

            //When
            commandHandler.Execute(command);

            //Then
            var actualItems = GetItems(testBarcodeTypeId);
            AssertActualItemsEqualExpectedItems(expectedItems, actualItems, testScanCodes);
            AssertScanCodesAreAssignedInBarcodeTypeRangePool(testBarcodeTypeId, testScanCodes);
        }

        [TestMethod]
        public void AddItems_ItemHasAssignedScanCode_AddsItem()
        {
            //Given
            testScanCodes = new List<string> { "9000000000000" };
            expectedItems.Add(CreateTestItem(testScanCodes[0], testBarcodeTypeId, true, true));
            command.Items = expectedItems;

            //When
            commandHandler.Execute(command);

            //Then
            var actualItems = GetItems(testBarcodeTypeId);
            AssertActualItemsEqualExpectedItems(expectedItems, actualItems, testScanCodes);
            AssertScanCodesAreAssignedInBarcodeTypeRangePool(testBarcodeTypeId, testScanCodes);
        }

        [TestMethod]
        public void AddItems_ItemDoesNotHaveAssignedScanCode_AddsItem()
        {
            //Given
            testScanCodes = new List<string> { "9000000000000" };
            expectedItems.Add(CreateTestItem(testScanCodes[0], testBarcodeTypeId, true, false));
            command.Items = expectedItems;

            //When
            commandHandler.Execute(command);

            //Then
            var actualItems = GetItems(testBarcodeTypeId);
            AssertActualItemsEqualExpectedItems(expectedItems, actualItems, testScanCodes);
            AssertScanCodesAreAssignedInBarcodeTypeRangePool(testBarcodeTypeId, testScanCodes);
        }

        [TestMethod]
        public void AddItems_5ItemsAndOnly3AssignableBarcodes_Adds2ItemsAndReturnsInvalidItems()
        {
            //Given
            testScanCodes = new List<string> { "9000000000000", "9000000000001", "9000000000002", "9000000000003", "9000000000004" };
            expectedItems.Add(CreateTestItem(testScanCodes[0], testBarcodeTypeId));
            expectedItems.Add(CreateTestItem(testScanCodes[4], testBarcodeTypeId));

            var expectedInvalidItems = new List<AddItemModel>();
            expectedInvalidItems.Add(CreateTestItem(testScanCodes[1], testBarcodeTypeId, false));
            expectedInvalidItems.Add(CreateTestItem(testScanCodes[2], testBarcodeTypeId, false));
            expectedInvalidItems.Add(CreateTestItem(testScanCodes[3], testBarcodeTypeId, false));
            command.Items = expectedItems.Concat(expectedInvalidItems).ToList();

            //When
            commandHandler.Execute(command);

            //Then
            var actualItems = GetItems(testBarcodeTypeId);
            AssertActualItemsEqualExpectedItems(expectedItems, actualItems, testScanCodes);
            AssertScanCodesAreAssignedInBarcodeTypeRangePool(testBarcodeTypeId, testScanCodes);
            Assert.AreEqual(expectedInvalidItems.Count, command.InvalidItems.Count);
            foreach (var invalidItem in expectedInvalidItems)
            {
                Assert.IsTrue(command.InvalidItems.Any(e => e.Item == invalidItem));
            }
        }

        [TestMethod]
        public void AddItems_ItemIsUpc_AddsItem()
        {
            //Given
            var upcBarcodeTypeId = sqlConnection.QueryFirst<int>(
                "SELECT TOP 1 BarcodeTypeId FROM dbo.BarcodeType WHERE BarcodeType = 'UPC'");
            testScanCodes = new List<string> { "9000000000000" };
            expectedItems.Add(CreateTestItem(testScanCodes[0], upcBarcodeTypeId, false, true));
            command.Items = expectedItems;

            //When
            commandHandler.Execute(command);

            //Then
            var actualItems = GetItems(testScanCodes);
            AssertActualItemsEqualExpectedItems(expectedItems, actualItems, testScanCodes);
        }

        [TestMethod]
        public void AddItems_BarcodeRangeFull_ReturnsInvalidItem()
        {
            //Given
            testScanCodes = new List<string> { "9000000000000" };
            expectedItems.Add(CreateTestItem(testScanCodes[0], testBarcodeTypeId, false, false));
            command.Items = expectedItems;

            //When
            commandHandler.Execute(command);

            //Then
            AssertItemDoesNotExist(testScanCodes[0]);
            Assert.AreEqual(expectedItems[0], command.InvalidItems[0].Item);
        }

        [TestMethod]
        public void AddItems_MultipleBarcodeRangesAndEachRangeHas1ItemWithASpecifiedScanCode_AddsAllItems()
        {
            //Given
            var testBarcodeTypeId2 = TestHelpers.AddBarcodeType(sqlConnection, "TestBarcodeType2");
            var testBarcodeTypeId3 = TestHelpers.AddBarcodeType(sqlConnection, "TestBarcodeType3");

            testScanCodes = new List<string>
            {
                "9000000000000",
                "9000000000001",
                "9000000000002",
                "9000000000003",
                "9000000000004",
                "9000000000005",
            };
            expectedItems.Add(CreateTestItem(testScanCodes[0], testBarcodeTypeId, true, true));
            expectedItems.Add(CreateTestItem(testScanCodes[1], testBarcodeTypeId, true, false));
            expectedItems.Add(CreateTestItem(testScanCodes[2], testBarcodeTypeId2, true, true));
            expectedItems.Add(CreateTestItem(testScanCodes[3], testBarcodeTypeId2, true, false));
            expectedItems.Add(CreateTestItem(testScanCodes[4], testBarcodeTypeId3, true, true));
            expectedItems.Add(CreateTestItem(testScanCodes[5], testBarcodeTypeId3, true, false));
            command.Items = expectedItems;

            //When
            commandHandler.Execute(command);

            //Then
            var actualItems = GetItems(testBarcodeTypeId);
            var actualItems2 = GetItems(testBarcodeTypeId2);
            var actualItems3 = GetItems(testBarcodeTypeId3);
            AssertActualItemsEqualExpectedItems(expectedItems.Take(2).ToList(), actualItems, testScanCodes.Take(2).ToList());
            AssertActualItemsEqualExpectedItems(expectedItems.Skip(2).Take(2).ToList(), actualItems2, testScanCodes.Skip(2).Take(2).ToList());
            AssertActualItemsEqualExpectedItems(expectedItems.Skip(4).Take(2).ToList(), actualItems3, testScanCodes.Skip(4).Take(2).ToList());
            AssertScanCodesAreAssignedInBarcodeTypeRangePool(testBarcodeTypeId, testScanCodes.Take(2).ToList());
            AssertScanCodesAreAssignedInBarcodeTypeRangePool(testBarcodeTypeId2, new List<string> { testScanCodes[2], testScanCodes[3] });
            AssertScanCodesAreAssignedInBarcodeTypeRangePool(testBarcodeTypeId3, new List<string> { testScanCodes[4], testScanCodes[5] });
        }

        private void AssertItemDoesNotExist(string scanCode)
        {
            var existingScanCode = sqlConnection.QueryFirstOrDefault<string>(
                "SELECT ScanCode FROM ScanCode sc WHERE sc.ScanCode = @ScanCode",
                new { ScanCode = scanCode });
            Assert.IsNull(existingScanCode);
        }

        private void AssertScanCodesAreAssignedInBarcodeTypeRangePool(int barcodeTypeId, List<string> testScanCodes)
        {
            var areAssigned = sqlConnection.QueryFirst<bool>(
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
                    ScanCodes = testScanCodes
                        .Select(sc => new { ScanCode = sc })
                        .ToDataTable()
                        .AsTableValuedParameter("app.ScanCodeListType") 
                });
            Assert.IsTrue(areAssigned);
        }

        private void AssertActualItemsEqualExpectedItems(
            List<AddItemModel> testExpectedItems, 
            List<AddItemModel> testActualItems,
            List<string> testScanCodes)
        {
            Assert.AreEqual(testExpectedItems.Count, testActualItems.Count);
            for (int i = 0; i < testExpectedItems.Count; i++)
            {
                Assert.AreEqual(testExpectedItems[i].BarCodeTypeId, testActualItems[i].BarCodeTypeId);
                Assert.AreEqual(testExpectedItems[i].BrandsHierarchyClassId, testActualItems[i].BrandsHierarchyClassId);
                Assert.AreEqual(testExpectedItems[i].FinancialHierarchyClassId, testActualItems[i].FinancialHierarchyClassId);
                Assert.AreEqual(testExpectedItems[i].ItemAttributesJson, testActualItems[i].ItemAttributesJson);
                Assert.AreEqual(testExpectedItems[i].ItemTypeId, testActualItems[i].ItemTypeId);
                Assert.AreEqual(testExpectedItems[i].ManufacturerHierarchyClassId, testActualItems[i].ManufacturerHierarchyClassId);
                Assert.AreEqual(testExpectedItems[i].MerchandiseHierarchyClassId, testActualItems[i].MerchandiseHierarchyClassId);
                Assert.AreEqual(testExpectedItems[i].NationalHierarchyClassId, testActualItems[i].NationalHierarchyClassId);
                Assert.AreEqual(testExpectedItems[i].TaxHierarchyClassId, testActualItems[i].TaxHierarchyClassId);
            }
            Assert.IsTrue(testScanCodes.Intersect(testActualItems.Select(i => i.ScanCode)).Count() == testExpectedItems.Count);
        }

        private AddItemModel CreateTestItem(string scanCode, int barcodeTypeId, bool addScanCodeToRangePool = true, bool addScanCodeToItem = false)
        {
            if (addScanCodeToRangePool)
                TestHelpers.AddScanCodeToBarcodeTypeRangePool(sqlConnection, barcodeTypeId, scanCode);

            return new AddItemModel
            {
                BarCodeTypeId = barcodeTypeId,
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

        private List<AddItemModel> GetItems(int barcodeTypeId)
        {
            return sqlConnection.Query<AddItemModel>(
                @"SELECT * FROM dbo.ItemView i WHERE i.BarcodeTypeId = @BarcodeTypeId",
                new { BarcodeTypeId = barcodeTypeId })
                .ToList();
        }

        private List<AddItemModel> GetItems(List<string> scanCodes)
        {
            return sqlConnection.Query<AddItemModel>(
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

        public AddItemModel GetAddItemModel(int itemId)
        {
            return sqlConnection.QuerySingle<AddItemModel>(
                @"SELECT * FROM dbo.ItemView i WHERE i.ItemId = @itemId",
                new { ItemId = itemId });
        }
    }
}
