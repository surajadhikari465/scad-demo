using Dapper;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetHierarchyClassesQueryHandlerTests
    {
        private GetHierarchyClassesQueryHandler queryHandler;
        private GetHierarchyClassesParameters parameters;
        private TransactionScope transaction;
        private SqlConnection connection;
        private string testHierarchyName;
        private List<HierarchyClassModel> hierarchyClassModels;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            connection = SqlConnectionBuilder.CreateIconConnection();
            queryHandler = new GetHierarchyClassesQueryHandler(connection);
            parameters = new GetHierarchyClassesParameters();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetBrandsHierarchyClasses_BrandsHierarchyIdIsSet_ShouldReturnBrandsHierarchyMatchingHierarchyId()
        {
            //Given
            testHierarchyName = "Brands";
            hierarchyClassModels = connection.Query<HierarchyClassModel>(
                    @"DECLARE @hierarchyId INT = (SELECT TOP 1 hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)
                     DECLARE @hierarchyClassId INT
                     INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel)
                      VALUES (@hierarchyId, 'BrandTestClass1', 1)

                      SET @hierarchyClassId = SCOPE_IDENTITY()
                      
                     INSERT INTO dbo.HierarchyClassTrait([traitId],[hierarchyClassID],[uomID],[traitValue])
                     VALUES(66, @hierarchyClassId,Null, 'Test1')

                     INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel)
                     VALUES (@hierarchyId, 'BrandTestClass2', 1)

                     SET @hierarchyClassId = SCOPE_IDENTITY()
                      
                     INSERT INTO dbo.HierarchyClassTrait([traitId],[hierarchyClassID],[uomID],[traitValue])
                     VALUES(66, @hierarchyClassId,Null, 'Test2')

                     INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel)
                     VALUES (@hierarchyId, 'BrandTestClass3', 1)

                     SET @hierarchyClassId = SCOPE_IDENTITY()

                     INSERT INTO dbo.HierarchyClassTrait([traitId],[hierarchyClassID],[uomID],[traitValue])
                     VALUES(66, @hierarchyClassId,Null, 'Test3')

                      SELECT hierarchyID AS HierarchyId,
                          hierarchyClassID AS HierarchyClassId,
                          hierarchyClassName AS HierarchyClassName,
                          hierarchyLevel AS HierarchyLevel,
                          hierarchyLineage AS HierarchyLineage,
                          hierarchyParentClassId AS HierarchyParentClassId
                      FROM [dbo].[BrandHierarchyView]
                      WHERE hierarchyID = @hierarchyId
                      AND hierarchyClassName like '%BrandTestClass%'
                      ORDER BY hierarchyClassID",

                    new { HierarchyName = testHierarchyName })
                .ToList();
            parameters.HierarchyId = connection.QueryFirst<int>("SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName", new { HierarchyName = testHierarchyName });
            parameters.HierarchyId = hierarchyClassModels[0].HierarchyId;

            //When
            var results = queryHandler.Search(parameters).Where(hc => hc.HierarchyClassName.Contains("BrandTestClass")).ToList();

            //Then
            Assert.AreEqual(3, results.Count);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(hierarchyClassModels[i].HierarchyClassId, results[i].HierarchyClassId);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyClassName, results[i].HierarchyClassName);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyId, results[i].HierarchyId);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyLevel, results[i].HierarchyLevel);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyLineage, results[i].HierarchyLineage);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyParentClassId, results[i].HierarchyParentClassId);
            }
        }

        [TestMethod]
        public void GetManufacturerHierarchyClasses_ManufacturerHierarchyIdIsSet_ShouldReturnManufacturerHierarchyMatchingHierarchyId()
        {
            //Given
            testHierarchyName = "Manufacturer";
            hierarchyClassModels = connection.Query<HierarchyClassModel>(
                    @"DECLARE @hierarchyId INT = (SELECT TOP 1 hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)
                      INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel)
                      VALUES (@hierarchyId, 'ManufacturerTestClass1', 1),
                          (@hierarchyId, 'ManufacturerTestClass2', 1),
                          (@hierarchyId, 'ManufacturerTestClass3', 1)

                      SELECT hierarchyID AS HierarchyId,
                          hierarchyClassID AS HierarchyClassId,
                          hierarchyClassName AS HierarchyClassName,
                          hierarchyLevel AS HierarchyLevel,
                          hierarchyLineage AS HierarchyLineage,
                          hierarchyParentClassId AS HierarchyParentClassId
                      FROM [dbo].[ManufacturerHierarchyView]
                      WHERE hierarchyID = @hierarchyId
                      AND hierarchyClassName like '%ManufacturerTestClass%'
                      ORDER BY hierarchyClassID",
                    new { HierarchyName = testHierarchyName })
                .ToList();
            parameters.HierarchyId = connection.QueryFirst<int>("SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName", new { HierarchyName = testHierarchyName });
            parameters.HierarchyId = hierarchyClassModels[0].HierarchyId;

            //When
            var results = queryHandler.Search(parameters).Where(hc => hc.HierarchyClassName.Contains("ManufacturerTestClass")).ToList();

            //Then
            Assert.AreEqual(3, results.Count);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(hierarchyClassModels[i].HierarchyClassId, results[i].HierarchyClassId);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyClassName, results[i].HierarchyClassName);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyId, results[i].HierarchyId);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyLevel, results[i].HierarchyLevel);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyLineage, results[i].HierarchyLineage);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyParentClassId, results[i].HierarchyParentClassId);
            }
        }

        [TestMethod]
        public void GetFinancialHierarchyClasses_FinancialHierarchyNameIsSet_ShouldReturnFinancialHierarchyMatchingHierarchyName()
        {
            //Given
            testHierarchyName = "Financial";
            hierarchyClassModels = connection.Query<HierarchyClassModel>(
                    @"DECLARE @hierarchyId INT = (SELECT TOP 1 hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)
                      INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel)
                      VALUES (@hierarchyId, 'FinancialTestClass1', 1),
                          (@hierarchyId, 'FinancialTestClass2', 1),
                          (@hierarchyId, 'FinancialTestClass3', 1)
                  SELECT  
                      hierarchyID AS HierarchyId,
                      hierarchyClassID AS HierarchyClassId,
                      hierarchyClassName AS HierarchyClassName,
                      hierarchyLevel AS HierarchyLevel,
                      hierarchyLineage AS HierarchyLineage,
                      hierarchyParentClassId AS HierarchyParentClassId
                  FROM [dbo].[FinancialHierarchyView] 
                  WHERE hierarchyID = @hierarchyId
                  AND hierarchyClassName like '%FinancialTestClass%'
                  ORDER BY hierarchyClassID",
                    new { HierarchyName = testHierarchyName })
                .ToList();
            parameters.HierarchyId = connection.QueryFirst<int>("SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName", new { HierarchyName = testHierarchyName });

            //When
            var results = queryHandler.Search(parameters).Where(hc => hc.HierarchyClassName.Contains("FinancialTestClass")).ToList();

            //Then
            Assert.AreEqual(3, results.Count);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(hierarchyClassModels[i].HierarchyClassId, results[i].HierarchyClassId);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyClassName, results[i].HierarchyClassName);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyId, results[i].HierarchyId);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyLevel, results[i].HierarchyLevel);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyClassName, results[i].HierarchyLineage);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyParentClassId, results[i].HierarchyParentClassId);
            }
        }

        [TestMethod]
        public void GetTaxHierarchyClasses_TaxHierarchyIsASingleLevelHierarchy_TaxHierarchyLineageShouldBeTheNameOfTheHierarchy()
        {
            //Given
            testHierarchyName = "Tax";
            hierarchyClassModels = connection.Query<HierarchyClassModel>(
                    @"DECLARE @hierarchyId INT = (SELECT TOP 1 hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)
                      INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel)
                      VALUES (@hierarchyId, 'TaxTestClass1', 1),
                          (@hierarchyId, 'TaxTestClass2', 1),
                          (@hierarchyId, 'TaxTestClass3', 1)
                  SELECT  
                      hierarchyID AS HierarchyId,
                      hierarchyClassID AS HierarchyClassId,
                      hierarchyClassName AS HierarchyClassName,
                      hierarchyLevel AS HierarchyLevel,
                      hierarchyLineage AS HierarchyLineage,
                      hierarchyParentClassId AS HierarchyParentClassId
                  FROM [dbo].[TaxHierarchyView] 
                  WHERE hierarchyID = @hierarchyId
                  AND hierarchyClassName like '%TaxTestClass%'
                  ORDER BY hierarchyClassID",
                    new { HierarchyName = testHierarchyName })
                .ToList();
            parameters.HierarchyId = connection.QueryFirst<int>("SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName", new { HierarchyName = testHierarchyName });

            //When
            var results = queryHandler.Search(parameters).Where(hc => hc.HierarchyClassName.Contains("TaxTestClass")).ToList();

            //Then
            Assert.AreEqual(3, results.Count);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(hierarchyClassModels[i].HierarchyClassId, results[i].HierarchyClassId);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyClassName, results[i].HierarchyClassName);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyId, results[i].HierarchyId);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyLevel, results[i].HierarchyLevel);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyClassName, results[i].HierarchyLineage);
                Assert.AreEqual(hierarchyClassModels[i].HierarchyParentClassId, results[i].HierarchyParentClassId);
                Assert.AreEqual(1, hierarchyClassModels[i].HierarchyLevel);
            }
        }

        [TestMethod]
        public void GetHierarchyClasses_MerchandiseHierarchyIsAMultiLevelHierarchy_ShouldReturnLowestLevelMerchandiseHierarchyClassesAndMerchandiseHierarchyLineageWithSubTeamNameAppend()
        {
            //Given
            testHierarchyName = "Merchandise";
            InsertMerchandiseTestData();
            hierarchyClassModels = connection.Query<HierarchyClassModel>(
                    @" DECLARE @hierarchyId INT = (SELECT TOP 1 hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)
             SELECT 
            hierarchyID AS HierarchyId,
                hierarchyClassID AS HierarchyClassId,
                hierarchyClassName AS HierarchyClassName,
                hierarchyLevel AS HierarchyLevel,
                hierarchyLineage AS HierarchyLineage,
                hierarchyParentClassId AS HierarchyParentClassId
            FROM[dbo].[MerchandiseHierarchyView]
            WHERE hierarchyID = @hierarchyId
            AND hierarchyClassName like '%MerchandiseTestClass%'
            ORDER BY hierarchyClassID", new { HierarchyName = testHierarchyName })
                .ToList();
            parameters.HierarchyId = connection.QueryFirst<int>(
                "SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName",
                new { HierarchyName = testHierarchyName });

            //When
            var results = queryHandler.Search(parameters).Where(hc => hc.HierarchyClassName.Contains("MerchandiseTestClass")).ToList();

            //Then
            Assert.AreEqual(1, results.Count);
            var resultHierarchyClass = results.Where(hc => hc.HierarchyClassId == hierarchyClassModels[0].HierarchyClassId).FirstOrDefault();
            Assert.AreEqual(hierarchyClassModels[0].HierarchyClassId, resultHierarchyClass.HierarchyClassId);
            Assert.AreEqual(hierarchyClassModels[0].HierarchyClassName, resultHierarchyClass.HierarchyClassName);
            Assert.AreEqual(hierarchyClassModels[0].HierarchyId, resultHierarchyClass.HierarchyId);
            Assert.AreEqual(hierarchyClassModels[0].HierarchyLevel, resultHierarchyClass.HierarchyLevel);
            Assert.AreEqual(hierarchyClassModels[0].HierarchyLineage, resultHierarchyClass.HierarchyLineage);
            Assert.AreEqual(hierarchyClassModels[0].HierarchyParentClassId, resultHierarchyClass.HierarchyParentClassId);

        }

        [TestMethod]
        public void GetHierarchyClasses_NationalHierarchyIsAMultiLevelHierarchy_ShouldReturnLowestLevelNationalHierarchyClassesAndNationalHierarchyLineageWithNationalClassCodeAppend()
        {
            //Given
            testHierarchyName = "National";
            InsertNationalTestData();
            hierarchyClassModels = connection.Query<HierarchyClassModel>(
                    @" DECLARE @hierarchyId INT = (SELECT TOP 1 hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)
            SELECT
                hierarchyID AS HierarchyId,
                hierarchyClassID AS HierarchyClassId,
                hierarchyClassName AS HierarchyClassName,
                hierarchyLevel AS HierarchyLevel,
                hierarchyLineage AS HierarchyLineage,
                hierarchyParentClassId AS HierarchyParentClassId
            FROM[dbo].[NationalClassHierarchyView]
            WHERE hierarchyID = @hierarchyId
            AND hierarchyClassName like '%NationalTestClass%'
            ORDER BY hierarchyClassID", new { HierarchyName = testHierarchyName })
                .ToList();
            parameters.HierarchyId = connection.QueryFirst<int>(
                "SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName",
                new { HierarchyName = testHierarchyName });

            //When
            var results = queryHandler.Search(parameters).Where(hc => hc.HierarchyClassName.Contains("NationalTestClass")).ToList();

            //Then
            Assert.AreEqual(1, results.Count);
            var resultHierarchyClass = results.Where(hc => hc.HierarchyClassId == hierarchyClassModels[0].HierarchyClassId).FirstOrDefault();
            Assert.AreEqual(hierarchyClassModels[0].HierarchyClassId, resultHierarchyClass.HierarchyClassId);
            Assert.AreEqual(hierarchyClassModels[0].HierarchyClassName, resultHierarchyClass.HierarchyClassName);
            Assert.AreEqual(hierarchyClassModels[0].HierarchyId, resultHierarchyClass.HierarchyId);
            Assert.AreEqual(hierarchyClassModels[0].HierarchyLevel, resultHierarchyClass.HierarchyLevel);
            Assert.AreEqual(hierarchyClassModels[0].HierarchyLineage, resultHierarchyClass.HierarchyLineage);
            Assert.AreEqual(hierarchyClassModels[0].HierarchyParentClassId, resultHierarchyClass.HierarchyParentClassId);
        }

        [TestMethod]
        public void GetHierarchyClasses_NationalHierarchyLineageFilterIsSet_ShouldReturnNationalHierarchyClassesWithMatchingLineageFilter()
        {
            //Given
            testHierarchyName = "National";
            InsertNationalTestData();
            hierarchyClassModels = connection.Query<HierarchyClassModel>(
                @"DECLARE @hierarchyId INT = (SELECT TOP 1 hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)
                  SELECT hierarchyID AS HierarchyId,
                      hierarchyClassID AS HierarchyClassId,
                      hierarchyClassName AS HierarchyClassName,
                      hierarchyLevel AS HierarchyLevel,
                      hierarchyLineage AS HierarchyLineage,
                      hierarchyParentClassId AS HierarchyParentClassId
                  FROM [dbo].[NationalClassHierarchyView]
                  WHERE hierarchyID = @hierarchyId                  
                  ORDER BY hierarchyClassID",
                new { HierarchyName = testHierarchyName })
                .ToList();

            parameters.HierarchyId = connection.QueryFirst<int>("SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName", new { HierarchyName = testHierarchyName });
            parameters.HierarchyLineageFilter = "TestClass";

            //When
            var results = queryHandler.Search(parameters).OrderBy(hc => hc.HierarchyClassId).ToList();

            //Then
            Assert.AreEqual(1, results.Count);

            var hierarchyClass = hierarchyClassModels.Where(hc => hc.HierarchyLevel == 4 && hc.HierarchyClassName == "NationalTestClass4").FirstOrDefault();
            var resultHierarchyClass = results.Where(hc => hc.HierarchyLevel == 4 && hc.HierarchyClassName == "NationalTestClass4").FirstOrDefault();
            Assert.AreEqual(hierarchyClass.HierarchyClassId, resultHierarchyClass.HierarchyClassId);
            Assert.AreEqual(hierarchyClass.HierarchyClassName, resultHierarchyClass.HierarchyClassName);
            Assert.AreEqual(hierarchyClass.HierarchyId, resultHierarchyClass.HierarchyId);
            Assert.AreEqual(hierarchyClass.HierarchyLevel, resultHierarchyClass.HierarchyLevel);
            Assert.AreEqual(hierarchyClass.HierarchyParentClassId, resultHierarchyClass.HierarchyParentClassId);
            Assert.AreEqual(hierarchyClass.HierarchyLineage, resultHierarchyClass.HierarchyLineage);
        }


        [TestMethod]
        public void GetHierarchyClasses_MerchandiseHierarchyLineageFilterIsSet_ShouldReturnMerchandiseHierarchyClassesWithMatchingLineageFilter()
        {
            //Given
            testHierarchyName = "Merchandise";
            InsertMerchandiseTestData();
            hierarchyClassModels = connection.Query<HierarchyClassModel>(
                @"DECLARE @hierarchyId INT = (SELECT TOP 1 hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)
                  SELECT hierarchyID AS HierarchyId,
                      hierarchyClassID AS HierarchyClassId,
                      hierarchyClassName AS HierarchyClassName,
                      hierarchyLevel AS HierarchyLevel,
                      hierarchyLineage AS HierarchyLineage,
                      hierarchyParentClassId AS HierarchyParentClassId
                  FROM [dbo].[MerchandiseHierarchyView]
                  WHERE hierarchyID = @hierarchyId
                 AND hierarchyClassName like '%MerchandiseTestClass%'
                  ORDER BY hierarchyClassID",
                new { HierarchyName = testHierarchyName })
                .ToList();

            parameters.HierarchyId = connection.QueryFirst<int>("SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName", new { HierarchyName = testHierarchyName });
            parameters.HierarchyLineageFilter = "TestClass";

            //When
            var results = queryHandler.Search(parameters).OrderBy(hc => hc.HierarchyClassId).ToList();

            //Then
            Assert.AreEqual(1, results.Count);

            var hierarchyClass = hierarchyClassModels.Where(hc => hc.HierarchyLevel == 5 && hc.HierarchyClassName == "MerchandiseTestClass5").FirstOrDefault();
            var resultHierarchyClass = results.Where(hc => hc.HierarchyLevel == 5 && hc.HierarchyClassName == "MerchandiseTestClass5").FirstOrDefault();
            Assert.AreEqual(hierarchyClass.HierarchyClassId, resultHierarchyClass.HierarchyClassId);
            Assert.AreEqual(hierarchyClass.HierarchyClassName, resultHierarchyClass.HierarchyClassName);
            Assert.AreEqual(hierarchyClass.HierarchyId, resultHierarchyClass.HierarchyId);
            Assert.AreEqual(hierarchyClass.HierarchyLevel, resultHierarchyClass.HierarchyLevel);
            Assert.AreEqual(hierarchyClass.HierarchyParentClassId, resultHierarchyClass.HierarchyParentClassId);
            Assert.AreEqual(hierarchyClass.HierarchyLineage, resultHierarchyClass.HierarchyLineage);
        }

        [TestMethod]
        public void GetHierarchyClasses_BrandsHierarchyClassIdFilterIsSet_ShouldReturnBrandsHierarchyClassesWithByBrandsHierarchyClassId()
        {
            //Given
            testHierarchyName = "Brands";
            hierarchyClassModels = connection.Query<HierarchyClassModel>(
                   @"DECLARE @hierarchyId INT = (SELECT TOP 1 hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)
                      INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel)
                      VALUES (@hierarchyId, 'TestClass1', 1),
                          (@hierarchyId, 'TestClass2', 1),
                          (@hierarchyId, 'TestClass3', 1)

                      SELECT hierarchyID AS HierarchyId,
                          hierarchyClassID AS HierarchyClassId,
                          hierarchyClassName AS HierarchyClassName,
                          hierarchyLevel AS HierarchyLevel,
                          HierarchyLineage
                      FROM [dbo].[BrandHierarchyView]
                      WHERE hierarchyID = @hierarchyId
                      ORDER BY hierarchyClassID",
                   new { HierarchyName = testHierarchyName })
                   .ToList();
            parameters.HierarchyId = connection.QueryFirst<int>("SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName", new { HierarchyName = testHierarchyName });
            parameters.HierarchyClassId = hierarchyClassModels[0].HierarchyClassId;

            //When
            var results = queryHandler.Search(parameters).ToList();

            //Then
            Assert.AreEqual(1, results.Count);
            var expectedHierarchyClass = hierarchyClassModels[0];
            var resultHierarchyClass = results[0];
            Assert.AreEqual(expectedHierarchyClass.HierarchyClassId, resultHierarchyClass.HierarchyClassId);
            Assert.AreEqual(expectedHierarchyClass.HierarchyClassName, resultHierarchyClass.HierarchyClassName);
            Assert.AreEqual(expectedHierarchyClass.HierarchyId, resultHierarchyClass.HierarchyId);
            Assert.AreEqual(expectedHierarchyClass.HierarchyLevel, resultHierarchyClass.HierarchyLevel);
            Assert.AreEqual(expectedHierarchyClass.HierarchyLineage, resultHierarchyClass.HierarchyLineage);
            Assert.AreEqual(expectedHierarchyClass.HierarchyParentClassId, resultHierarchyClass.HierarchyParentClassId);
        }

        [TestMethod]
        public void GetHierarchyClasses_ManufacturerHierarchyClassIdFilterIsSet_ShouldReturnManufacturerHierarchyClassesWithByManufacturerHierarchyClassId()
        {
            //Given
            testHierarchyName = "Manufacturer";
            hierarchyClassModels = connection.Query<HierarchyClassModel>(
                   @"DECLARE @hierarchyId INT = (SELECT TOP 1 hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)
                      INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel)
                      VALUES (@hierarchyId, 'TestClass1', 1),
                          (@hierarchyId, 'TestClass2', 1),
                          (@hierarchyId, 'TestClass3', 1)

                      SELECT hierarchyID AS HierarchyId,
                          hierarchyClassID AS HierarchyClassId,
                          hierarchyClassName AS HierarchyClassName,
                          hierarchyLevel AS HierarchyLevel
                      FROM [dbo].[ManufacturerHierarchyView]
                      WHERE hierarchyID = @hierarchyId
                      ORDER BY hierarchyClassID",
                   new { HierarchyName = testHierarchyName })
                   .ToList();
            parameters.HierarchyId = connection.QueryFirst<int>("SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName", new { HierarchyName = testHierarchyName });
            parameters.HierarchyClassId = hierarchyClassModels[0].HierarchyClassId;

            //When
            var results = queryHandler.Search(parameters).ToList();

            //Then
            Assert.AreEqual(1, results.Count);
            var expectedHierarchyClass = hierarchyClassModels[0];
            var resultHierarchyClass = results[0];
            Assert.AreEqual(expectedHierarchyClass.HierarchyClassId, resultHierarchyClass.HierarchyClassId);
            Assert.AreEqual(expectedHierarchyClass.HierarchyClassName, resultHierarchyClass.HierarchyClassName);
            Assert.AreEqual(expectedHierarchyClass.HierarchyId, resultHierarchyClass.HierarchyId);
            Assert.AreEqual(expectedHierarchyClass.HierarchyLevel, resultHierarchyClass.HierarchyLevel);
            Assert.AreEqual(expectedHierarchyClass.HierarchyClassName, resultHierarchyClass.HierarchyLineage);
            Assert.AreEqual(expectedHierarchyClass.HierarchyParentClassId, resultHierarchyClass.HierarchyParentClassId);
        }

        private void InsertMerchandiseTestData()
        {
            testHierarchyName = "Merchandise";
            hierarchyClassModels = connection.Query<HierarchyClassModel>(
                    @"DECLARE @hierarchyId INT = (SELECT TOP 1 hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)
                      DECLARE @hierarchyClassId INT    

                       SET @hierarchyClassId = SCOPE_IDENTITY()                       
                      INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'MerchandiseTestClass1', 1, Null)
                          SELECT @hierarchyClassId
                      SET @hierarchyClassId = SCOPE_IDENTITY()

                      INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'MerchandiseTestClass2', 2, @hierarchyClassId)
                        SELECT @hierarchyClassId
                      SET @hierarchyClassId = SCOPE_IDENTITY()

                     INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'MerchandiseTestClass3', 3, @hierarchyClassId)
                        SELECT @hierarchyClassId
                      SET @hierarchyClassId = SCOPE_IDENTITY()

                      INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'MerchandiseTestClass4', 4, @hierarchyClassId)
                        SELECT @hierarchyClassId
                      SET @hierarchyClassId = SCOPE_IDENTITY()

                       INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'MerchandiseTestClass5', 5, @hierarchyClassId)
                       SELECT @hierarchyClassId
                     SET @hierarchyClassId = SCOPE_IDENTITY()

                     INSERT INTO dbo.HierarchyClassTrait([traitId],[hierarchyClassID],[uomID],[traitValue])
                     VALUES(49, @hierarchyClassId,Null, @hierarchyClassId)",
                    new { HierarchyName = testHierarchyName })
                .ToList();

        }

        private void InsertNationalTestData()
        {
            testHierarchyName = "National";
            hierarchyClassModels = connection.Query<HierarchyClassModel>(
                    @"DECLARE @hierarchyClassId INT
                      DECLARE @hierarchyId INT = (SELECT TOP 1 hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)
                       SET @hierarchyClassId = SCOPE_IDENTITY()                       
                      INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'NationalTestClass1', 1, Null)
                          SELECT @hierarchyClassId
                      SET @hierarchyClassId = SCOPE_IDENTITY()

                       INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'NationalTestClass2', 2, @hierarchyClassId)
                        SELECT @hierarchyClassId
                         SET @hierarchyClassId = SCOPE_IDENTITY()

                      INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'NationalTestClass3', 3, @hierarchyClassId)
                        SELECT @hierarchyClassId
                        SET @hierarchyClassId = SCOPE_IDENTITY()

                     INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'NationalTestClass4', 4, @hierarchyClassId)
                        SELECT @hierarchyClassId         
                       SET @hierarchyClassId = SCOPE_IDENTITY()

                      INSERT INTO dbo.HierarchyClassTrait([traitId],[hierarchyClassID],[uomID],[traitValue])
                     VALUES(69, @hierarchyClassId,Null, @hierarchyClassId)",
                    new { HierarchyName = testHierarchyName })
                .ToList();
        }
    }
}