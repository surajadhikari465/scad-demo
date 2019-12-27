using Dapper;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Icon.Web.Tests.Integration.TestHelpers
{
    public static class TestHelper
    {
        public static SqlConnection Icon => new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);

        internal static int AddHierarchyClass(SqlConnection sqlConnection, int hierarchyId, string hierarchyClassName = "Test", int hierarchyLevel = 1, int? parentHierarchyClassId = null)
        {
            return sqlConnection.QueryFirst<int>(
                @"INSERT INTO dbo.HierarchyClass(hierarchyClassName, hierarchyID, hierarchyLevel, hierarchyParentClassID)
                VALUES(@HierarchyClassName, @HierarchyId, @HierarchyLevel, @ParentHierarchyClassID)

                SELECT SCOPE_IDENTITY()",
                new
                {
                    HierarchyClassName = hierarchyClassName,
                    HierarchyId = hierarchyId,
                    HierarchyLevel = hierarchyLevel,
                    ParentHierarchyClassID = parentHierarchyClassId
                });
        }

        internal static void AddHierarchyClassTrait(SqlConnection sqlConnection, int hierarchyClassId, int traitId, string traitValue)
        {
            sqlConnection.Execute(
                @"INSERT INTO dbo.HierarchyClassTrait(hierarchyClassID, traitID, traitValue)
                VALUES(@HierarchyClassID, @TraitID, @TraitValue)",
                new
                {
                    HierarchyClassID = hierarchyClassId,
                    TraitID = traitId,
                    TraitValue = traitValue
                });
        }

        internal static int AddItemType(SqlConnection sqlConnection, string itemTypeCode = "TST", string itemTypeDesc = "Test")
        {
            return sqlConnection.QueryFirst<int>(
                @"INSERT INTO dbo.ItemType(itemTypeCode, itemTypeDesc)
                VALUES(@ItemTypeCode, @ItemTypeDesc)

                SELECT SCOPE_IDENTITY()",
                new
                {
                    ItemTypeCode = itemTypeCode,
                    ItemTypeDesc = itemTypeDesc
                });
        }

        internal static int AddBarcodeType(SqlConnection sqlConnection, string barcodeType = "TestBarcodeType", string beginRange = "82000", string endRange = "82199", bool isScalePlu = false)
        {
            return sqlConnection.QueryFirst<int>(
                @"INSERT INTO dbo.BarcodeType(BarcodeType, BeginRange, EndRange, ScalePLU)
                VALUES(@BarcodeType, @BeginRange, @EndRange, @ScalePLU)

                SELECT SCOPE_IDENTITY()",
                new
                {
                    BarcodeType = barcodeType,
                    BeginRange = beginRange,
                    EndRange = endRange,
                    ScalePLU = isScalePlu
                });
        }

        internal static int AddScanCodeToBarcodeTypeRangePool(SqlConnection sqlConnection, int barcodeTypeId, string scanCode = "82000", bool assigned = false)
        {
            return sqlConnection.Execute(
                @"INSERT INTO dbo.BarcodeTypeRangePool(BarcodeTypeId, ScanCode, Assigned)
                VALUES(@BarcodeTypeId, @ScanCode, @Assigned)",
                new
                {
                    BarcodeTypeId = barcodeTypeId,
                    ScanCode = scanCode,
                    Assigned = assigned
                });
        }

        internal static void AddIrmaItemSubscription(SqlConnection sqlConnection, string scanCode = "82000", string region = "MA")
        {
            sqlConnection.Execute(
                @"INSERT INTO app.IRMAItemSubscription (regioncode, identifier, insertdate, deletedate)
                VALUES(@region, @identifier, @insertdate, NULL)",
                new
                {
                    region = region,
                    identifier = scanCode,
                    insertdate = DateTime.Now
                });
        }

        internal static List<int> AddItems(SqlConnection sqlConnection, int count, string startOfScanCode = "7778889996")
        {
            var itemIds = new List<int>();
            var itemTypeId = ItemTypes.RetailSale;
            var itemAttributesJson = @"{}";
            for (int i = 0; i < count; i++)
            {
                var scanCode = (startOfScanCode + i).Substring(0, 13);
                var itemId = sqlConnection.QueryFirst<int>(
                    @"
                    INSERT dbo.Item(ItemTypeId, ItemAttributesJson) 
                    VALUES (@ItemTypeId, @ItemAttributesJson)

                    DECLARE @itemId INT = SCOPE_IDENTITY()

                    INSERT dbo.ScanCode(itemID, scanCode, scanCodeTypeID, localeID, BarcodeTypeId)
                    VALUES (@itemId, @ScanCode, 1, 1, 1)

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

        internal static ItemDbModel GetUpdateItemModel(SqlConnection sqlConnection, int itemId)
        {
            return sqlConnection.QuerySingle<ItemDbModel>(
                @"SELECT * FROM dbo.ItemView i WHERE i.ItemId = @itemId",
                new { ItemId = itemId });
        }
    }
}