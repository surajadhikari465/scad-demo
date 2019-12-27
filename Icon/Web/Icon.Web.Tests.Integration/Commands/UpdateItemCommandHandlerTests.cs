using Dapper;
using Icon.Framework;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class UpdateItemCommandHandlerTests
    {
        private UpdateItemCommandHandler commandHandler;
        private UpdateItemCommand command;
        private IDbProvider db;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;
        private int? testBrandHierarchyClassId;
        private int? testFinancialHierarchyClassId;
        private string testItemAttributesJson;
        private int? testItemTypeId;
        private int? testManufacturerHierarchyClassId;
        private int? testMerchandiseHierarchyClassIdLvl1;
        private int? testMerchandiseHierarchyClassIdLvl2;
        private int? testMerchandiseHierarchyClassIdLvl3;
        private int? testMerchandiseHierarchyClassIdLvl4;
        private int? testMerchandiseHierarchyClassId;
        private int? testTaxHierarchyClassId;
        private int? testNationalHierarchyClassIdLvl1;
        private int? testNationalHierarchyClassIdLvl2;
        private int? testNationalHierarchyClassIdLvl3;
        private int? testNationalHierarchyClassId;
        private string testScanCode;

        [TestInitialize]
        public void Initialize()
        {
            this.db = new SqlDbProvider();
            this.db.Connection = TestHelper.Icon as SqlConnection;
            transaction = new TransactionScope();
            sqlConnection = this.db.Connection as SqlConnection;
            commandHandler = new UpdateItemCommandHandler(db);
            command = new UpdateItemCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.db.Connection.Dispose();
            transaction.Dispose();
        }

        [TestMethod]
        public void UpdateItems_ItemValueschanged_ItemIsUpdated()
        {
            //Given
            testBrandHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Brands);
            testFinancialHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Financial);
            testManufacturerHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Manufacturer);
            testMerchandiseHierarchyClassIdLvl1 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl1", 1, null);
            testMerchandiseHierarchyClassIdLvl2 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl2", 2, testMerchandiseHierarchyClassIdLvl1.Value);
            testMerchandiseHierarchyClassIdLvl3 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl3", 3, testMerchandiseHierarchyClassIdLvl2.Value);
            testMerchandiseHierarchyClassIdLvl4 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl4", 4, testMerchandiseHierarchyClassIdLvl3.Value);
            testMerchandiseHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl5", 5, testMerchandiseHierarchyClassIdLvl4.Value);
            testNationalHierarchyClassIdLvl1 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.National, "TestNationalLvl1", 1, null);
            testNationalHierarchyClassIdLvl2 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.National, "TestNationalLvl2", 2, testNationalHierarchyClassIdLvl1.Value);
            testNationalHierarchyClassIdLvl3 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.National, "TestNationalLvl3", 3, testNationalHierarchyClassIdLvl2.Value);
            testNationalHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.National, "TestNationalLvl4", 4, testNationalHierarchyClassIdLvl3.Value);
            testTaxHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Tax);
            testItemTypeId = TestHelper.AddItemType(this.db.Connection as SqlConnection);
            testScanCode = "7778889996";
            testItemAttributesJson = "{\"CreatedBy\":\"ICON\",\"CreatedDateTimeUtc\":\"2014-06-06T13:25:38.22Z\"," +
                "\"CustomerFriendlyDescription\":\"Test Update Item Command Description\",\"FoodStampEligible\":\"false\"," +
                "\"ItemPack\":\"1\",\"Inactive\":\"false\",\"ModifiedBy\":\"1013359@wholefoods.com\",\"ModifiedDateTimeUtc\":\"2018-09-06T12:05:00.133975Z\"," +
                "\"POSDescription\":\"Test Update Item Command Description\",\"POSScaleTare\":\"0\"," +
                "\"ProductDescription\":\"Test Update Item Command Description\",\"ProhibitDiscount\":\"false\",\"RetailSize\":\"1\",\"UOM\":\"EA\"}";

            TestHelper.AddHierarchyClassTrait(this.db.Connection as SqlConnection,
                hierarchyClassId: testMerchandiseHierarchyClassId.Value,
                traitId: Traits.MerchFinMapping,
                traitValue: testFinancialHierarchyClassId.ToString());
            TestHelper.AddHierarchyClassTrait(this.db.Connection as SqlConnection,
                hierarchyClassId: testNationalHierarchyClassId.Value,
                traitId: Traits.NationalClassCode,
                traitValue: "57457545");

            var itemIds = AddItems(sqlConnection: this.db.Connection as SqlConnection, count: 1, testScanCode: testScanCode);
            var expectedItem = itemIds
                .Select(i => new ItemDbModel
                {
                    ItemId = i,
                    ScanCode = testScanCode,
                    BrandsHierarchyClassId = testBrandHierarchyClassId.Value,
                    FinancialHierarchyClassId = testFinancialHierarchyClassId.Value,
                    ItemAttributesJson = testItemAttributesJson,
                    ItemTypeId = testItemTypeId.Value,
                    ManufacturerHierarchyClassId = testManufacturerHierarchyClassId.Value,
                    MerchandiseHierarchyClassId = testMerchandiseHierarchyClassId.Value,
                    NationalHierarchyClassId = testNationalHierarchyClassId.Value,
                    TaxHierarchyClassId = testTaxHierarchyClassId.Value
                })
                .First();
            command.BrandsHierarchyClassId = expectedItem.BrandsHierarchyClassId;
            command.FinancialHierarchyClassId = expectedItem.FinancialHierarchyClassId;
            command.ItemAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(expectedItem.ItemAttributesJson);
            command.ItemId = expectedItem.ItemId;
            command.ItemTypeId = expectedItem.ItemTypeId;
            command.ManufacturerHierarchyClassId = expectedItem.ManufacturerHierarchyClassId;
            command.MerchandiseHierarchyClassId = expectedItem.MerchandiseHierarchyClassId;
            command.NationalHierarchyClassId = expectedItem.NationalHierarchyClassId;
            command.TaxHierarchyClassId = expectedItem.TaxHierarchyClassId;
            command.ScanCode = expectedItem.ScanCode;

            //When
            commandHandler.Execute(command);

            //Then
            var items = itemIds.Select(i => GetUpdateItemModel(i)).First();

            Assert.AreEqual(expectedItem.BrandsHierarchyClassId, items.BrandsHierarchyClassId);
            Assert.AreEqual(expectedItem.FinancialHierarchyClassId, items.FinancialHierarchyClassId);
            Assert.AreEqual(expectedItem.ItemAttributesJson, items.ItemAttributesJson);
            Assert.AreEqual(expectedItem.ItemTypeId, items.ItemTypeId);
            Assert.AreEqual(expectedItem.ManufacturerHierarchyClassId, items.ManufacturerHierarchyClassId);
            Assert.AreEqual(expectedItem.MerchandiseHierarchyClassId, items.MerchandiseHierarchyClassId);
            Assert.AreEqual(expectedItem.NationalHierarchyClassId, items.NationalHierarchyClassId);
            Assert.AreEqual(expectedItem.TaxHierarchyClassId, items.TaxHierarchyClassId);
        }

        [TestMethod]
        public void UpdateItems_ItemValuesChanged_EventQueueRecordCreated()
        {
            //Given
            testBrandHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Brands);
            testFinancialHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Financial);
            testManufacturerHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Manufacturer);
            testMerchandiseHierarchyClassIdLvl1 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl1", 1, null);
            testMerchandiseHierarchyClassIdLvl2 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl2", 2, testMerchandiseHierarchyClassIdLvl1.Value);
            testMerchandiseHierarchyClassIdLvl3 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl3", 3, testMerchandiseHierarchyClassIdLvl2.Value);
            testMerchandiseHierarchyClassIdLvl4 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl4", 4, testMerchandiseHierarchyClassIdLvl3.Value);
            testMerchandiseHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl5", 5, testMerchandiseHierarchyClassIdLvl4.Value);
            testNationalHierarchyClassIdLvl1 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.National, "TestNationalLvl1", 1, null);
            testNationalHierarchyClassIdLvl2 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.National, "TestNationalLvl2", 2, testNationalHierarchyClassIdLvl1.Value);
            testNationalHierarchyClassIdLvl3 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.National, "TestNationalLvl3", 3, testNationalHierarchyClassIdLvl2.Value);
            testNationalHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.National, "TestNationalLvl4", 4, testNationalHierarchyClassIdLvl3.Value);
            testTaxHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Tax);
            testItemTypeId = TestHelper.AddItemType(this.db.Connection as SqlConnection);
            testScanCode = "7778889996";
            testItemAttributesJson = "{\"CreatedBy\":\"ICON\",\"CreatedDateTimeUtc\":\"2014-06-06T13:25:38.22Z\"," +
                "\"CustomerFriendlyDescription\":\"Test Update Item Command Description\",\"FoodStampEligible\":\"false\"," +
                "\"ItemPack\":\"1\",\"Inactive\":\"false\",\"ModifiedBy\":\"1013359@wholefoods.com\",\"ModifiedDateTimeUtc\":\"2018-09-06T12:05:00.133975Z\"," +
                "\"POSDescription\":\"Test Update Item Command Description\",\"POSScaleTare\":\"0\"," +
                "\"ProductDescription\":\"Test Update Item Command Description\",\"ProhibitDiscount\":\"false\",\"RetailSize\":\"1\",\"UOM\":\"EA\"}";

            TestHelper.AddHierarchyClassTrait(this.db.Connection as SqlConnection,
                hierarchyClassId: testMerchandiseHierarchyClassId.Value,
                traitId: Traits.MerchFinMapping,
                traitValue: testFinancialHierarchyClassId.ToString());
            TestHelper.AddHierarchyClassTrait(this.db.Connection as SqlConnection,
                hierarchyClassId: testNationalHierarchyClassId.Value,
                traitId: Traits.NationalClassCode,
                traitValue: "57457545");

            var itemIds = AddItems(sqlConnection: this.db.Connection as SqlConnection, count: 1, testScanCode: testScanCode);
            var expectedItem = itemIds
                .Select(i => new ItemDbModel
                {
                    ItemId = i,
                    ScanCode = testScanCode,
                    BrandsHierarchyClassId = testBrandHierarchyClassId.Value,
                    FinancialHierarchyClassId = testFinancialHierarchyClassId.Value,
                    ItemAttributesJson = testItemAttributesJson,
                    ItemTypeId = testItemTypeId.Value,
                    ManufacturerHierarchyClassId = testManufacturerHierarchyClassId.Value,
                    MerchandiseHierarchyClassId = testMerchandiseHierarchyClassId.Value,
                    NationalHierarchyClassId = testNationalHierarchyClassId.Value,
                    TaxHierarchyClassId = testTaxHierarchyClassId.Value
                })
                .First();
            command.BrandsHierarchyClassId = expectedItem.BrandsHierarchyClassId;
            command.FinancialHierarchyClassId = expectedItem.FinancialHierarchyClassId;
            command.ItemAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(expectedItem.ItemAttributesJson);
            command.ItemId = expectedItem.ItemId;
            command.ItemTypeId = expectedItem.ItemTypeId;
            command.ManufacturerHierarchyClassId = expectedItem.ManufacturerHierarchyClassId;
            command.MerchandiseHierarchyClassId = expectedItem.MerchandiseHierarchyClassId;
            command.NationalHierarchyClassId = expectedItem.NationalHierarchyClassId;
            command.TaxHierarchyClassId = expectedItem.TaxHierarchyClassId;
            command.ScanCode = expectedItem.ScanCode;

            //When
            commandHandler.Execute(command);

            //Then
            var eventQueue = this.db.Connection.Query<MessageQueueItem>(
                    "SELECT * FROM app.EventQueue WHERE EventReferenceId = @ItemId AND ProcessFailedDate IS NULL ORDER BY InsertDate DESC",
                    new { ItemId = this.command.ItemId },
                    this.db.Transaction)
                .ToList();

            Assert.IsTrue(eventQueue.Count() > 0, "EventQueue record was not added with UpdateItemCommandHandler.");
        }

        [TestMethod]
        public void UpdateItems_ItemValuesChanged_ItemMessageRecordCreated()
        {
            //Given
            testBrandHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Brands);
            testFinancialHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Financial);
            testManufacturerHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Manufacturer);
            testMerchandiseHierarchyClassIdLvl1 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl1", 1, null);
            testMerchandiseHierarchyClassIdLvl2 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl2", 2, testMerchandiseHierarchyClassIdLvl1.Value);
            testMerchandiseHierarchyClassIdLvl3 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl3", 3, testMerchandiseHierarchyClassIdLvl2.Value);
            testMerchandiseHierarchyClassIdLvl4 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl4", 4, testMerchandiseHierarchyClassIdLvl3.Value);
            testMerchandiseHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Merchandise, "TestMerchLvl5", 5, testMerchandiseHierarchyClassIdLvl4.Value);
            testNationalHierarchyClassIdLvl1 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.National, "TestNationalLvl1", 1, null);
            testNationalHierarchyClassIdLvl2 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.National, "TestNationalLvl2", 2, testNationalHierarchyClassIdLvl1.Value);
            testNationalHierarchyClassIdLvl3 = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.National, "TestNationalLvl3", 3, testNationalHierarchyClassIdLvl2.Value);
            testNationalHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.National, "TestNationalLvl4", 4, testNationalHierarchyClassIdLvl3.Value);
            testTaxHierarchyClassId = TestHelper.AddHierarchyClass(this.db.Connection as SqlConnection, Hierarchies.Tax);
            testItemTypeId = TestHelper.AddItemType(this.db.Connection as SqlConnection);
            testScanCode = "7778889996";
            testItemAttributesJson = "{\"CreatedBy\":\"ICON\",\"CreatedDateTimeUtc\":\"2014-06-06T13:25:38.22Z\"," +
                "\"CustomerFriendlyDescription\":\"Test Update Item Command Description\",\"FoodStampEligible\":\"false\"," +
                "\"ItemPack\":\"1\",\"Inactive\":\"false\",\"ModifiedBy\":\"1013359@wholefoods.com\",\"ModifiedDateTimeUtc\":\"2018-09-06T12:05:00.133975Z\"," +
                "\"POSDescription\":\"Test Update Item Command Description\",\"POSScaleTare\":\"0\"," +
                "\"ProductDescription\":\"Test Update Item Command Description\",\"ProhibitDiscount\":\"false\",\"RetailSize\":\"1\",\"UOM\":\"EA\"}";

            TestHelper.AddHierarchyClassTrait(this.db.Connection as SqlConnection,
                hierarchyClassId: testMerchandiseHierarchyClassId.Value,
                traitId: Traits.MerchFinMapping,
                traitValue: testFinancialHierarchyClassId.ToString());
            TestHelper.AddHierarchyClassTrait(this.db.Connection as SqlConnection,
                hierarchyClassId: testNationalHierarchyClassId.Value,
                traitId: Traits.NationalClassCode,
                traitValue: "57457545");

            var itemIds = AddItems(sqlConnection: this.db.Connection as SqlConnection, count: 1, testScanCode: testScanCode);
            var expectedItem = itemIds
                .Select(i => new ItemDbModel
                {
                    ItemId = i,
                    ScanCode = testScanCode,
                    BrandsHierarchyClassId = testBrandHierarchyClassId.Value,
                    FinancialHierarchyClassId = testFinancialHierarchyClassId.Value,
                    ItemAttributesJson = testItemAttributesJson,
                    ItemTypeId = testItemTypeId.Value,
                    ManufacturerHierarchyClassId = testManufacturerHierarchyClassId.Value,
                    MerchandiseHierarchyClassId = testMerchandiseHierarchyClassId.Value,
                    NationalHierarchyClassId = testNationalHierarchyClassId.Value,
                    TaxHierarchyClassId = testTaxHierarchyClassId.Value
                })
                .First();
            command.BrandsHierarchyClassId = expectedItem.BrandsHierarchyClassId;
            command.FinancialHierarchyClassId = expectedItem.FinancialHierarchyClassId;
            command.ItemAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(expectedItem.ItemAttributesJson);
            command.ItemId = expectedItem.ItemId;
            command.ItemTypeId = expectedItem.ItemTypeId;
            command.ManufacturerHierarchyClassId = expectedItem.ManufacturerHierarchyClassId;
            command.MerchandiseHierarchyClassId = expectedItem.MerchandiseHierarchyClassId;
            command.NationalHierarchyClassId = expectedItem.NationalHierarchyClassId;
            command.TaxHierarchyClassId = expectedItem.TaxHierarchyClassId;
            command.ScanCode = expectedItem.ScanCode;

            //When
            commandHandler.Execute(command);

            //Then
            var messageQueueItem = this.db.Connection.Query<MessageQueueItem>(
                    "SELECT * FROM esb.MessageQueueItem WHERE ItemId = @ItemId",
                    new { ItemId = this.command.ItemId },
                    this.db.Transaction)
                .ToList();

            Assert.IsTrue(messageQueueItem.Count() > 0, "MessageQueueItem record was not added with EditItemCommandHandler.");
        }

        private List<int> AddItems(SqlConnection sqlConnection, int count, string testScanCode)
        {
            var itemIds = new List<int>();
            var itemTypeId = ItemTypes.RetailSale;
            var itemAttributesJson = @"{}";
            for (int i = 0; i < count; i++)
            {
                var scanCode = (testScanCode + i.ToString());
                var itemId = sqlConnection.QueryFirst<int>(
                    @"
                    INSERT dbo.Item(ItemTypeId, ItemAttributesJson) 
                    VALUES (@ItemTypeId, @ItemAttributesJson)

                    DECLARE @itemId INT = SCOPE_IDENTITY()

                    INSERT dbo.ScanCode(itemID, scanCode, scanCodeTypeID, localeID, BarcodeTypeId)
                    VALUES (@itemId, @ScanCode, 1, 1, 1)

                    INSERT app.IRMAItemSubscription (regioncode, identifier, insertdate, deletedate)
                    VALUES('MA', @ScanCode, GETDATE(), NULL)

                    DECLARE @hierarchyClassId INT
                    DECLARE @merchHierarchyClassId INT

                    DECLARE @merchandiseHierarchyId INT = (SELECT hierarchyId FROM dbo.Hierarchy WHERE hierarchyName = 'Merchandise')
                    DECLARE @brandHierarchyId INT = (SELECT hierarchyId FROM dbo.Hierarchy WHERE hierarchyName = 'Brands')
                    DECLARE @taxHierarchyId INT = (SELECT hierarchyId FROM dbo.Hierarchy WHERE hierarchyName = 'Tax')
                    DECLARE @nationalHierarchyId INT = (SELECT hierarchyId FROM dbo.Hierarchy WHERE hierarchyName = 'National')
                    DECLARE @financialHierarchyId INT = (SELECT hierarchyId FROM dbo.Hierarchy WHERE hierarchyName = 'Financial')
                    DECLARE @manufacturerHierarchyId INT = (SELECT hierarchyId FROM dbo.Hierarchy WHERE hierarchyName = 'Manufacturer')
                    DECLARE @merchFinMappingTraitId INT = (SELECT traitID FROM Trait WHERE traitCode = 'MFM')
                    DECLARE @nationalClassCodeTraitId INT = (SELECT traitID FROM Trait WHERE traitCode = 'NCC')


                    -- add all levels of merch hierarchy
                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
                    VALUES('TestMerchHierarchyLvl1', @merchandiseHierarchyId, 1, NULL)
                    SELECT @merchHierarchyClassId = SCOPE_IDENTITY()

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
                    VALUES('TestMerchHierarchyLvl2', @merchandiseHierarchyId, 2, @merchHierarchyClassId)
                    SELECT @merchHierarchyClassId = SCOPE_IDENTITY()

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
                    VALUES('TestMerchHierarchyLvl3', @merchandiseHierarchyId, 3, @merchHierarchyClassId)
                    SELECT @merchHierarchyClassId = SCOPE_IDENTITY()

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
                    VALUES('TestMerchHierarchyLvl4', @merchandiseHierarchyId, 4, @merchHierarchyClassId)
                    SELECT @merchHierarchyClassId = SCOPE_IDENTITY()

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
                    VALUES('TestMerchHierarchyLvl5', @merchandiseHierarchyId, 5, @merchHierarchyClassId)
                    SELECT @merchHierarchyClassId = SCOPE_IDENTITY()
                    INSERT dbo.ItemHierarchyClass(itemId, hierarchyClassId, localeId)
                    VALUES (@itemId, @merchHierarchyClassId, 1)

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

                    -- add all levels of national hierarchy
                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
                    VALUES('TestNationalHierarchyLvl1', @merchandiseHierarchyId, 1, NULL)
                    SELECT @hierarchyClassId = SCOPE_IDENTITY()

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
                    VALUES('TestNationalHierarchyLvl2', @merchandiseHierarchyId, 2, @hierarchyClassId)
                    SELECT @hierarchyClassId = SCOPE_IDENTITY()

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
                    VALUES('TestNationalHierarchyLvl3', @merchandiseHierarchyId, 3, @hierarchyClassId)
                    SELECT @hierarchyClassId = SCOPE_IDENTITY()

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
                    VALUES('TestNationalHierarchyLvl4', @nationalHierarchyId, 4, @hierarchyClassId)
                    SELECT @hierarchyClassId = SCOPE_IDENTITY()
                    INSERT dbo.ItemHierarchyClass(itemId, hierarchyClassId, localeId)
                    VALUES (@itemId, @hierarchyClassId, 1)

                    -- For NCC trait
                    INSERT dbo.HierarchyClassTrait(hierarchyClassID, traitID, traitValue)
                    VALUES(@hierarchyClassId, @nationalClassCodeTraitId, '545465658787')

                    INSERT dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel)
                    VALUES('TestItemHierarchyClass', @financialHierarchyId, 1)
                    SELECT @hierarchyClassId = SCOPE_IDENTITY()
                    INSERT dbo.ItemHierarchyClass(itemId, hierarchyClassId, localeId)
                    VALUES (@itemId, @hierarchyClassId, 1)

                    -- For MFM trait
                    INSERT dbo.HierarchyClassTrait(hierarchyClassID, traitID, traitValue)
                    VALUES(@merchHierarchyClassId, @merchFinMappingTraitId, CAST(@hierarchyClassId as nvarchar))

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

        private ItemDbModel GetUpdateItemModel(int itemId)
        {
            return this.db.Connection.QuerySingle<ItemDbModel>(
                @"SELECT * FROM dbo.ItemView i WHERE i.ItemId = @itemId",
                new { ItemId = itemId });
        }
    }
}