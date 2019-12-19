using Dapper;
using Icon.Common.Models;
using Icon.Web.Tests.Integration.TestModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Icon.Web.Tests.Integration.TestHelpers
{
    internal static class SqlDataGenerator
    {
        internal static List<TestItemTypeModel> CreateItemTypes(IDbConnection dbConnection, List<TestItemTypeModel> testItemTypes)
        {
            List<TestItemTypeModel> testItemTypeResults = new List<TestItemTypeModel>();
            var sql = @"
                    INSERT INTO dbo.ItemType(itemTypeDesc, itemTypeCode) 
                    VALUES (@ItemTypeDesc, @ItemTypeCode); 
                    SELECT 
                        itemTypeID AS ItemTypeId,
                        itemTypeDesc AS ItemTypeDesc,
                        itemTypeCode AS ItemTypeCode
                    FROM dbo.ItemType
                    WHERE itemTypeID = SCOPE_IDENTITY()";

            foreach (var itemType in testItemTypes)
            {
                var testItemTypeModel = dbConnection.QueryFirst<TestItemTypeModel>(sql, itemType);
                testItemTypeResults.Add(testItemTypeModel);
            }

            return testItemTypeResults;
        }

        internal static int CreateAttribute(IDbConnection dbConnection, string attributeName)
        {
            return dbConnection.QueryFirst<int>(
                "INSERT INTO dbo.Attributes(AttributeName) VALUES (@AttributeName); SELECT SCOPE_IDENTITY();", 
                new { AttributeName = attributeName });
        }

        internal static List<TestScanCodeTypeModel> CreateScanCodeTypes(IDbConnection dbConnection, List<TestScanCodeTypeModel> testScanCodeTypeModels)
        {
            List<TestScanCodeTypeModel> results = new List<TestScanCodeTypeModel>();
            var sql = @"
                    INSERT INTO dbo.ScanCodeType(scanCodeTypeDesc) 
                    VALUES (@ScanCodeTypeDesc); 
                    SELECT 
                        scanCodeTypeId AS ScanCodeTypeId,
                        scanCodeTypeDesc AS ScanCodeTypeDesc
                    FROM dbo.ScanCodeType
                    WHERE scanCodeTypeId = SCOPE_IDENTITY()";
            foreach (var scanCodeType in testScanCodeTypeModels)
            {
                var testScanCodeType = dbConnection.QueryFirst<TestScanCodeTypeModel>(sql, scanCodeType);
                results.Add(testScanCodeType);
            }
            return results;
        }

        internal static List<TestBarcodeTypeModel> CreateBarcodeTypes(IDbConnection dbConnection, List<TestBarcodeTypeModel> testBarcodeTypeModels)
        {
            List<TestBarcodeTypeModel> results = new List<TestBarcodeTypeModel>();
            var sql = @"
                    INSERT INTO dbo.BarcodeType(barcodeType,BeginRange,EndRange,ScalePlu) 
                    VALUES (@BarcodeType, 1, 100,0); 
                    SELECT 
                        barcodeTypeId AS barcodeTypeId,
                        barcodeType AS BarcodeType
                    FROM dbo.BarcodeType
                    WHERE barcodeTypeId = SCOPE_IDENTITY()";
            foreach (var barcodeType in testBarcodeTypeModels)
            {
                var testbarCodeType = dbConnection.QueryFirst<TestBarcodeTypeModel>(sql, barcodeType);
                results.Add(testbarCodeType);
            }
            return results;
        }

        internal static void CreateHierarchies(IDbConnection dbConnection)
        {
            dbConnection.Execute(@"
                IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName IN ('Merchandise', 'Brands', 'Tax', 'Financial', 'National'))                
                BEGIN
                    INSERT INTO dbo.Hierarchy(hierarchyName)
                    VALUES ('Merchandise'),
                        ('Brands'),
                        ('Tax'),
                        ('Financial'),
                        ('National')

                    INSERT INTO dbo.HierarchyPrototype(hierarchyID, hierarchyLevel, hierarchyLevelName, itemsAttached)
                    SELECT hierarchyID, 1, 'Segment', 1
                    FROM dbo.Hierarchy
                    WHERE hierarchyName = 'Merchandise'

                    INSERT INTO dbo.HierarchyPrototype(hierarchyID, hierarchyLevel, hierarchyLevelName, itemsAttached)
                    SELECT hierarchyID, 2, 'Family', 1
                    FROM dbo.Hierarchy
                    WHERE hierarchyName = 'Merchandise'

                    INSERT INTO dbo.HierarchyPrototype(hierarchyID, hierarchyLevel, hierarchyLevelName, itemsAttached)
                    SELECT hierarchyID, 3, 'Class', 1
                    FROM dbo.Hierarchy
                    WHERE hierarchyName = 'Merchandise'

                    INSERT INTO dbo.HierarchyPrototype(hierarchyID, hierarchyLevel, hierarchyLevelName, itemsAttached)
                    SELECT hierarchyID, 4, 'GS1 Brick', 1
                    FROM dbo.Hierarchy
                    WHERE hierarchyName = 'Merchandise'

                    INSERT INTO dbo.HierarchyPrototype(hierarchyID, hierarchyLevel, hierarchyLevelName, itemsAttached)
                    SELECT hierarchyID, 5, 'Sub Brick', 1
                    FROM dbo.Hierarchy
                    WHERE hierarchyName = 'Merchandise'

                    INSERT INTO dbo.HierarchyPrototype(hierarchyID, hierarchyLevel, hierarchyLevelName, itemsAttached)
                    SELECT hierarchyID, 1, 'Brand', 1
                    FROM dbo.Hierarchy
                    WHERE hierarchyName = 'Brands'

                    INSERT INTO dbo.HierarchyPrototype(hierarchyID, hierarchyLevel, hierarchyLevelName, itemsAttached)
                    SELECT hierarchyID, 1, 'Tax', 1
                    FROM dbo.Hierarchy
                    WHERE hierarchyName = 'Tax'

                    INSERT INTO dbo.HierarchyPrototype(hierarchyID, hierarchyLevel, hierarchyLevelName, itemsAttached)
                    SELECT hierarchyID, 1, 'Financial', 1
                    FROM dbo.Hierarchy
                    WHERE hierarchyName = 'Financial'

                    INSERT INTO dbo.HierarchyPrototype(hierarchyID, hierarchyLevel, hierarchyLevelName, itemsAttached)
                    SELECT hierarchyID, 1, 'National Family', 1
                    FROM dbo.Hierarchy
                    WHERE hierarchyName = 'National'

                    INSERT INTO dbo.HierarchyPrototype(hierarchyID, hierarchyLevel, hierarchyLevelName, itemsAttached)
                    SELECT hierarchyID, 2, 'National Category', 1
                    FROM dbo.Hierarchy
                    WHERE hierarchyName = 'National'

                    INSERT INTO dbo.HierarchyPrototype(hierarchyID, hierarchyLevel, hierarchyLevelName, itemsAttached)
                    SELECT hierarchyID, 3, 'National Sub Category', 1
                    FROM dbo.Hierarchy
                    WHERE hierarchyName = 'National'

                    INSERT INTO dbo.HierarchyPrototype(hierarchyID, hierarchyLevel, hierarchyLevelName, itemsAttached)
                    SELECT hierarchyID, 4, 'National Class', 1
                    FROM dbo.Hierarchy
                    WHERE hierarchyName = 'National'
                END");
        }

        internal static IDictionary<string, List<TestHierarchyClassModel>> CreateHierarchyClasses(IDbConnection dbConnection, List<TestHierarchyClassModel> testHierarchyClassModels)
        {
            List<TestHierarchyClassModel> results = new List<TestHierarchyClassModel>();

            foreach (var hierarchyClass in testHierarchyClassModels)
            {
                var result = dbConnection.QueryFirst<TestHierarchyClassModel>(@"
                    DECLARE @hierarchyId INT = (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)

                    INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                    VALUES (@hierarchyId, @HierarchyClassName, @HierarchyLevel, @HierarchyParentClassId)

                    SELECT h.hierarchyName AS HierarchyName,
                        hc.hierarchyClassId AS HierarchyClassId,
                        hc.hierarchyClassName AS HierarchyClassName,
                        hc.hierarchyLevel AS HierarchyLevel,
                        hc.hierarchyParentClassId AS HierarchyParentClassId,
                        hc.hierarchyClassName  AS HierarchyLineage
                    FROM HierarchyClass hc
                    JOIN Hierarchy h ON hc.hierarchyID = h.hierarchyID
                    WHERE hc.HierarchyClassId = SCOPE_IDENTITY()",
                hierarchyClass);

                results.Add(result);
            }

            var merchHierarchies = results.Where(x => x.HierarchyName == "Merchandise").ToList();
            var financialHierarchies = results.Where(x => x.HierarchyName == "Financial").ToList();
            var nationalHierarchies = results.Where(x => x.HierarchyName == "National").ToList();

            for (int i=0; i < merchHierarchies.Count; i++)
            {
                if (merchHierarchies[i].HierarchyLevel == 5)
                {
                    dbConnection.Query(@"INSERT INTO dbo.HierarchyClassTrait(traitID,hierarchyClassId,uomID,traitValue) VALUES" +
                    "((SELECT traitId from dbo.Trait WHERE traitCode='MFM'),@hierarchyClassId,null,@traitValue)",
                    new
                    {
                        hierarchyClassId = merchHierarchies[i].HierarchyClassId,
                        traitValue = financialHierarchies[i].HierarchyClassId
                    });
                }
            }

            for (int i = 0; i < nationalHierarchies.Count; i++)
            {
                if (nationalHierarchies[i].HierarchyLevel == 4)
                {
                    dbConnection.Query(@"INSERT INTO dbo.HierarchyClassTrait(traitID,hierarchyClassId,uomID,traitValue) VALUES" +
                    "((SELECT traitId from dbo.Trait WHERE traitCode='NCC'),@hierarchyClassId,null,@traitValue)",
                    new
                    {
                        hierarchyClassId = nationalHierarchies[i].HierarchyClassId,
                        traitValue = "beer"
                    });
                }
            }



            return results
                .GroupBy(hc => hc.HierarchyName)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}
