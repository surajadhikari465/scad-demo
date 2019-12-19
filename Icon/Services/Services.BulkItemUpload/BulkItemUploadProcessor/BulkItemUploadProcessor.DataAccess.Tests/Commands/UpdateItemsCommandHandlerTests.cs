using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.DataAccess.Commands;
using Dapper;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace BulkItemUploadProcessor.DataAccess.Tests.Commands
{
    [TestClass]
    public class UpdateItemsCommandHandlerTests
    {
        private UpdateItemsCommandHandler commandHandler;
        private UpdateItemsCommand command;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;
        private int? testBrandHierarchyClassId;
        private int? testFinancialHierarchyClassId;
        private string testItemAttributesJson;
        private int? testItemTypeId;
        private int? testManufacturerHierarchyClassId;
        private int? testMerchandiseHierarchyClassId;
        private int? testTaxHierarchyClassId;
        private int? testNationalHierarchyClassId;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = TestHelpers.Icon;
            commandHandler = new UpdateItemsCommandHandler(sqlConnection);
            command = new UpdateItemsCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            sqlConnection.Dispose();
            transaction.Dispose();
        }

        [TestMethod]
        public void UpdateItems_SetAllProperties_ItemsAreUpdated()
        {
            //Given
            testBrandHierarchyClassId = TestHelpers.AddHierarchyClass(sqlConnection, Hierarchies.Brands);
            testFinancialHierarchyClassId = TestHelpers.AddHierarchyClass(sqlConnection, Hierarchies.Financial);
            testManufacturerHierarchyClassId = TestHelpers.AddHierarchyClass(sqlConnection, Hierarchies.Manufacturer);
            testMerchandiseHierarchyClassId = TestHelpers.AddHierarchyClass(sqlConnection, Hierarchies.Merchandise);
            testNationalHierarchyClassId = TestHelpers.AddHierarchyClass(sqlConnection, Hierarchies.National);
            testTaxHierarchyClassId = TestHelpers.AddHierarchyClass(sqlConnection, Hierarchies.Tax);
            testItemTypeId = TestHelpers.AddItemType(sqlConnection);
            testItemAttributesJson = @"{""ProductDescription"":""Test""}";

            var itemIds = AddItems(sqlConnection, 5);
            var expectedItems = itemIds
                .Select(i => new UpdateItemModel
                {
                    ItemId = i,
                    BrandsHierarchyClassId = testBrandHierarchyClassId,
                    FinancialHierarchyClassId = testFinancialHierarchyClassId,
                    ItemAttributesJson = testItemAttributesJson,
                    ItemTypeId = testItemTypeId,
                    ManufacturerHierarchyClassId = testManufacturerHierarchyClassId,
                    MerchandiseHierarchyClassId = testMerchandiseHierarchyClassId,
                    NationalHierarchyClassId = testNationalHierarchyClassId,
                    TaxHierarchyClassId = testTaxHierarchyClassId
                })
                .ToList();
            command.Items = expectedItems;

            //When
            commandHandler.Execute(command);

            //Then
            var items = itemIds.Select(i => GetUpdateItemModel(i)).ToList();

            Assert.AreEqual(5, items.Count);
            for (int i = 0; i < expectedItems.Count; i++)
            {
                Assert.AreEqual(expectedItems[i].BrandsHierarchyClassId.Value, items[i].BrandsHierarchyClassId.Value);
                Assert.AreEqual(expectedItems[i].FinancialHierarchyClassId.Value, items[i].FinancialHierarchyClassId.Value);
                Assert.AreEqual(expectedItems[i].ItemAttributesJson, items[i].ItemAttributesJson);
                Assert.AreEqual(expectedItems[i].ItemTypeId.Value, items[i].ItemTypeId.Value);
                Assert.AreEqual(expectedItems[i].ManufacturerHierarchyClassId.Value, items[i].ManufacturerHierarchyClassId.Value);
                Assert.AreEqual(expectedItems[i].MerchandiseHierarchyClassId.Value, items[i].MerchandiseHierarchyClassId.Value);
                Assert.AreEqual(expectedItems[i].NationalHierarchyClassId.Value, items[i].NationalHierarchyClassId.Value);
                Assert.AreEqual(expectedItems[i].TaxHierarchyClassId.Value, items[i].TaxHierarchyClassId.Value);
            }
        }

        private List<int> AddItems(SqlConnection sqlConnection, int count)
        {
            var itemIds = new List<int>();
            var itemTypeId = ItemTypes.RetailSale;
            var itemAttributesJson = @"{}";
            for (int i = 0; i < count; i++)
            {
                var scanCode = ("777888999666" + i).Substring(0, 13);
                var itemId = sqlConnection.QueryFirst<int>(
                    @"
                    INSERT dbo.Item(ItemTypeId, ItemAttributesJson) 
                    VALUES (@ItemTypeId, @ItemAttributesJson)

                    DECLARE @itemId INT = SCOPE_IDENTITY()

                    INSERT dbo.ScanCode(itemID, scanCode, scanCodeTypeID, localeID, BarcodeTypeId)
                    VALUES (@itemId, @ScanCode, 1, 1, 1)

                    DECLARE @hierarchyClassId INT

                    DECLARE @merchandiseHierarchyId INT = (SELECT hierarchyId FROM dbo.Hierarchy WHERE hierarchyName = 'Merchandise')
                    DECLARE @brandHierarchyId INT = (SELECT hierarchyId FROM dbo.Hierarchy WHERE hierarchyName = 'Brands')
                    DECLARE @taxHierarchyId INT = (SELECT hierarchyId FROM dbo.Hierarchy WHERE hierarchyName = 'Tax')
                    DECLARE @nationalHierarchyId INT = (SELECT hierarchyId FROM dbo.Hierarchy WHERE hierarchyName = 'National')
                    DECLARE @financialHierarchyId INT = (SELECT hierarchyId FROM dbo.Hierarchy WHERE hierarchyName = 'Financial')
                    DECLARE @manufacturerHierarchyId INT = (SELECT hierarchyId FROM dbo.Hierarchy WHERE hierarchyName = 'Manufacturer')

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel)
                    VALUES('TestItemHierarchyClass', @merchandiseHierarchyId, 1)
                    SELECT @hierarchyClassId = SCOPE_IDENTITY()
                    INSERT dbo.ItemHierarchyClass(itemId, hierarchyClassId, localeId)
                    VALUES (@itemId, @hierarchyClassId, 1)

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel)
                    VALUES('TestItemHierarchyClass', @brandHierarchyId, 1)
                    SELECT @hierarchyClassId = SCOPE_IDENTITY()
                    INSERT dbo.ItemHierarchyClass(itemId, hierarchyClassId, localeId)
                    VALUES (@itemId, @hierarchyClassId, 1)

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel)
                    VALUES('TestItemHierarchyClass', @taxHierarchyId, 1)
                    SELECT @hierarchyClassId = SCOPE_IDENTITY()
                    INSERT dbo.ItemHierarchyClass(itemId, hierarchyClassId, localeId)
                    VALUES (@itemId, @hierarchyClassId, 1)

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel)
                    VALUES('TestItemHierarchyClass', @nationalHierarchyId, 1)
                    SELECT @hierarchyClassId = SCOPE_IDENTITY()
                    INSERT dbo.ItemHierarchyClass(itemId, hierarchyClassId, localeId)
                    VALUES (@itemId, @hierarchyClassId, 1)

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel)
                    VALUES('TestItemHierarchyClass', @financialHierarchyId, 1)
                    SELECT @hierarchyClassId = SCOPE_IDENTITY()
                    INSERT dbo.ItemHierarchyClass(itemId, hierarchyClassId, localeId)
                    VALUES (@itemId, @hierarchyClassId, 1)

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel)
                    VALUES('TestItemHierarchyClass', @manufacturerHierarchyId, 1)
                    SELECT @hierarchyClassId = SCOPE_IDENTITY()
                    INSERT dbo.ItemHierarchyClass(itemId, hierarchyClassId, localeId)
                    VALUES (@itemId, @hierarchyClassId, 1)

                    SELECT @itemId",
                    new { ItemTypeId = itemTypeId, ScanCode = scanCode, ItemAttributesJson = itemAttributesJson });
                itemIds.Add(itemId);
            }
            return itemIds;
        }

        public UpdateItemModel GetUpdateItemModel(int itemId)
        {
            return sqlConnection.QuerySingle<UpdateItemModel>(
                @"SELECT * FROM dbo.ItemView i WHERE i.ItemId = @itemId",
                new { ItemId = itemId });
        }
    }
}
