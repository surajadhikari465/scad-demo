using Dapper;
using Icon.Web.DataAccess.Models;
using Icon.Web.Tests.Integration.TestModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Icon.Web.Tests.Integration.TestHelpers
{
    public class ItemTestHelper
    {
        private IDbConnection dbConnection;
        public IDictionary<string, List<TestHierarchyClassModel>> TestHierarchyClasses { get; set; }
        public ItemDbModel TestItem { get; set; }
        public string TestScanCode { get; set; }
        public List<TestItemTypeModel> TestItemTypes { get; set; }
        public List<TestScanCodeTypeModel> TestScanCodesTypes { get; set; }
        public List<TestBarcodeTypeModel> TestBarcodeTypes { get; set; }

        public ItemTestHelper()
        {
            TestHierarchyClasses = new Dictionary<string, List<TestHierarchyClassModel>>();
            TestItem = new ItemDbModel();
            TestScanCode = "9999999999999";
            TestItemTypes = new List<TestItemTypeModel>();
            TestScanCodesTypes = new List<TestScanCodeTypeModel>();
            TestBarcodeTypes = new List<TestBarcodeTypeModel>();
        }

        public void Initialize(IDbConnection dbConnection, bool initializeTestItem = true, bool saveItem = true)
        {
            this.dbConnection = dbConnection;
            TestItemTypes = SqlDataGenerator.CreateItemTypes(
                dbConnection,
                new List<TestItemTypeModel>
                {
                    new TestItemTypeModel { ItemTypeDesc = "Test Item Type Description1", ItemTypeCode = "TS1" },
                    new TestItemTypeModel { ItemTypeDesc = "Test Item Type Description2", ItemTypeCode = "TS2" }
                });
            TestScanCodesTypes = SqlDataGenerator.CreateScanCodeTypes(
                dbConnection,
                new List<TestScanCodeTypeModel>
                {
                    new TestScanCodeTypeModel { ScanCodeTypeDesc = "Scan Code Type Description1" },
                    new TestScanCodeTypeModel { ScanCodeTypeDesc = "Scan Code Type Description2" }
                });

            TestBarcodeTypes = SqlDataGenerator.CreateBarcodeTypes(
               dbConnection,
               new List<TestBarcodeTypeModel>
               {
                    new TestBarcodeTypeModel { BarcodeType = "Barcode1" },
                    new TestBarcodeTypeModel { BarcodeType = "Barcode2" }
               });

            SqlDataGenerator.CreateHierarchies(dbConnection);

            var merchLevel1 = SqlDataGenerator.CreateHierarchyClasses(
                dbConnection,
                new List<TestHierarchyClassModel>
                {
                    new TestHierarchyClassModel { HierarchyName = "Merchandise", HierarchyClassName = "Merchandise Level 1", HierarchyLevel = 1 }
                });
            var merchLevel2 = SqlDataGenerator.CreateHierarchyClasses(
               dbConnection,
               new List<TestHierarchyClassModel>
               {
                    new TestHierarchyClassModel { HierarchyName = "Merchandise", HierarchyClassName = "Merchandise Level 2", HierarchyLevel = 2, HierarchyParentClassId=merchLevel1.First().Value.First().HierarchyClassId }
               });
            var merchLevel3 = SqlDataGenerator.CreateHierarchyClasses(
               dbConnection,
               new List<TestHierarchyClassModel>
               {
                    new TestHierarchyClassModel { HierarchyName = "Merchandise", HierarchyClassName = "Merchandise Level 3", HierarchyLevel = 3, HierarchyParentClassId=merchLevel2.First().Value.First().HierarchyClassId  }
               });
            var merchLevel4 = SqlDataGenerator.CreateHierarchyClasses(
               dbConnection,
               new List<TestHierarchyClassModel>
               {
                    new TestHierarchyClassModel { HierarchyName = "Merchandise", HierarchyClassName = "Merchandise Level 4", HierarchyLevel = 4 , HierarchyParentClassId=merchLevel3.First().Value.First().HierarchyClassId }
               });


            var nationalLevel1 = SqlDataGenerator.CreateHierarchyClasses(
              dbConnection,
              new List<TestHierarchyClassModel>
              {
                    new TestHierarchyClassModel { HierarchyName = "National", HierarchyClassName = "National Level 1", HierarchyLevel = 1 }
              });
            var nationalLevel2 = SqlDataGenerator.CreateHierarchyClasses(
               dbConnection,
               new List<TestHierarchyClassModel>
               {
                    new TestHierarchyClassModel { HierarchyName = "National", HierarchyClassName = "National Level 2", HierarchyLevel = 2, HierarchyParentClassId=nationalLevel1.First().Value.First().HierarchyClassId }
               });
            var nationalLevel3 = SqlDataGenerator.CreateHierarchyClasses(
               dbConnection,
               new List<TestHierarchyClassModel>
               {
                    new TestHierarchyClassModel { HierarchyName = "National", HierarchyClassName = "National Level 3", HierarchyLevel = 3, HierarchyParentClassId=nationalLevel2.First().Value.First().HierarchyClassId  }
               });

            TestHierarchyClasses = SqlDataGenerator.CreateHierarchyClasses(
                dbConnection,
                new List<TestHierarchyClassModel>
                {
                    new TestHierarchyClassModel { HierarchyName = "Merchandise", 
                        HierarchyClassName = "Merchandise 1", 
                        HierarchyLevel = 5, 
                        HierarchyParentClassId=merchLevel4.First().Value.First().HierarchyClassId},
                    new TestHierarchyClassModel { HierarchyName = "Merchandise", HierarchyClassName = "Merchandise 2", HierarchyLevel = 5, HierarchyParentClassId=merchLevel4.First().Value.First().HierarchyClassId},
                    new TestHierarchyClassModel { HierarchyName = "Brands", HierarchyClassName = "Brands 1", HierarchyLevel = 1 },
                    new TestHierarchyClassModel { HierarchyName = "Brands", HierarchyClassName = "Brands 2", HierarchyLevel = 1 },
                    new TestHierarchyClassModel { HierarchyName = "Tax", HierarchyClassName = "Tax 1", HierarchyLevel = 1 },
                    new TestHierarchyClassModel { HierarchyName = "Tax", HierarchyClassName = "Tax 2", HierarchyLevel = 1 },
                    new TestHierarchyClassModel { HierarchyName = "Financial", HierarchyClassName = "Financial 1 (12345)", HierarchyLevel = 1 },
                    new TestHierarchyClassModel { HierarchyName = "Financial", HierarchyClassName = "Financial 2 (56789)", HierarchyLevel = 1 },
                    new TestHierarchyClassModel { HierarchyName = "National", HierarchyClassName = "National 1", HierarchyLevel = 4, HierarchyParentClassId=nationalLevel3.First().Value.First().HierarchyClassId },
                    new TestHierarchyClassModel { HierarchyName = "National", HierarchyClassName = "National 2", HierarchyLevel = 4 , HierarchyParentClassId=nationalLevel3.First().Value.First().HierarchyClassId},
                    new TestHierarchyClassModel { HierarchyName = "Manufacturer", HierarchyClassName = "Manufacturer 1", HierarchyLevel = 1 },
                    new TestHierarchyClassModel { HierarchyName = "Manufacturer", HierarchyClassName = "Manufacturer 2", HierarchyLevel = 1 }
                });

            if (initializeTestItem)
            {
                var testItem = CreateDefaultTestItem();
                if (saveItem)
                {
                    SaveItem(testItem);
                }
                TestItem = testItem;
            }
        }

        public ItemDbModel CreateDefaultTestItem()
        {

            var itemType = TestItemTypes.First();

            return new ItemDbModel
            {
                ItemTypeId = itemType.ItemTypeId,
                ItemTypeCode = itemType.ItemTypeCode,
                ItemAttributesJson = "{\"CreatedBy\":\"ICON\"," +
                    "\"CreatedDateTimeUtc\":\"2014-06-06T13:25:38.22Z\"," +
                    "\"CustomerFriendlyDescription\":\"Test Update Item Command Description\"," +
                    "\"FoodStampEligible\":\"false\"," +
                    "\"ItemPack\":\"1\"," +
                    "\"Inactive\":\"false\"," +
                    "\"ModifiedBy\":\"1013359@wholefoods.com\"," +
                    "\"ModifiedDateTimeUtc\":\"2018-09-06T12:05:00.133975Z\"," +
                    "\"POSDescription\":\"Test Update Item Command Description\"," +
                    "\"POSScaleTare\":\"0\"," +
                    "\"ProductDescription\":\"Test Update Item Command Description\"," +
                    "\"ProhibitDiscount\":\"false\"," +
                    "\"RetailSize\":\"1\"," +
                    "\"UOM\":\"EA\"}",
                ScanCode = TestScanCode,
                ScanCodeTypeId = TestScanCodesTypes.First().ScanCodeTypeId,
                BarcodeTypeId = TestBarcodeTypes.First().BarcodeTypeId,
                MerchandiseHierarchyClassId = TestHierarchyClasses["Merchandise"].First().HierarchyClassId,
                BrandsHierarchyClassId = TestHierarchyClasses["Brands"].First().HierarchyClassId,
                TaxHierarchyClassId = TestHierarchyClasses["Tax"].First().HierarchyClassId,
                FinancialHierarchyClassId = TestHierarchyClasses["Financial"].First().HierarchyClassId,
                NationalHierarchyClassId = TestHierarchyClasses["National"].First().HierarchyClassId,
                ManufacturerHierarchyClassId = TestHierarchyClasses["Manufacturer"].First().HierarchyClassId,
                Merchandise = TestHierarchyClasses["Merchandise"].First().HierarchyLineage,
                Brands = TestHierarchyClasses["Brands"].First().HierarchyLineage,
                Tax = TestHierarchyClasses["Tax"].First().HierarchyLineage,
                Financial = TestHierarchyClasses["Financial"].First().HierarchyLineage,
                National = TestHierarchyClasses["National"].First().HierarchyLineage,
                Manufacturer = TestHierarchyClasses["Manufacturer"].First().HierarchyLineage,
            };
        }

        public ItemDbModel SaveItem(ItemDbModel item)
        {
            var itemId = dbConnection.QueryFirst<int>(@"
                INSERT INTO dbo.Item(itemTypeID, ItemAttributesJson)
                VALUES (@ItemTypeId, @ItemAttributesJson)

                DECLARE @newItemId INT = SCOPE_IDENTITY()

                INSERT INTO dbo.ScanCode(scanCode, scanCodeTypeId, itemId, barcodeTypeId)
                VALUES (@ScanCode, @ScanCodeTypeId, @newItemId, @barcodeTypeId)

                INSERT INTO dbo.ItemHierarchyClass(itemID, hierarchyClassID)
                VALUES (@newItemId, @MerchandiseHierarchyClassId),
                       (@newItemId, @BrandsHierarchyClassId),
                       (@newItemId, @TaxHierarchyClassId),
                       (@newItemId, @FinancialHierarchyClassId),
                       (@newItemId, @NationalHierarchyClassId),
                       (@newItemId, @ManufacturerHierarchyClassId)

                SELECT @newItemId",
                item);
            item.ItemId = itemId;

            return item;
        }

        public void CreateItemNutrition(ItemDbModel item)
        {
            dbConnection.Execute($@"INSERT INTO nutrition.ItemNutrition(Plu, RecipeName, Calories)
                VALUES ({item.ScanCode}, 'Test', 1);");
        }

        public void UpdateItem(ItemDbModel item)
        {
            dbConnection.Query(@"
                UPDATE dbo.Item
                SET ItemAttributesJSON=@ItemAttributesJSON
                WHERE
                ItemId = @itemId;", item);
        }

        public void UpdateItemType(ItemDbModel item)
        {
            dbConnection.Execute(@"
                UPDATE dbo.Item
                SET ItemTypeId = @itemTypeId
                WHERE ItemId = @itemId", new { itemTypeId = item.ItemTypeId, itemId = item.ItemId });
        }

        public void UpdateItemHierarchyClass(int itemId, int originalItemHierarchyClassId, int newItemHierarchyClassId)
        {
            dbConnection.Query(@"
                UPDATE dbo.ItemHierarchyClass
                SET hierarchyClassId = @newItemHierarchyClassId
                WHERE
                ItemId = @itemId
                AND hierarchyClassId=@originalItemHierarchyClassId;",
                new
                {
                    itemId = itemId,
                    originalItemHierarchyClassId = originalItemHierarchyClassId,
                    newItemHierarchyClassId = newItemHierarchyClassId
                });
        }
    }
}