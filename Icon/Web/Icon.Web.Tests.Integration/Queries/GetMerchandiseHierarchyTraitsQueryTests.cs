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
    public class GetMerchandiseHierarchyTraitsQueryTests
    {
        private GetMerchandiseHierarchyClassTraitsQuery queryHandler;
        private GetMerchandiseHierarchyClassTraitsParameters parameters;
        private TransactionScope transaction;
        private SqlConnection connection;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            connection = SqlConnectionBuilder.CreateIconConnection();
            queryHandler = new GetMerchandiseHierarchyClassTraitsQuery(connection);
            parameters = new GetMerchandiseHierarchyClassTraitsParameters();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetMerchandiseHierarchyTraits_TraitsExists_ValuesAreCorrect()
        {
            //Given
            int hierarchyClassId = this.InsertMerchandiseTestData();

            //When
            var response = this.queryHandler.Search(new GetMerchandiseHierarchyClassTraitsParameters()
            {
            });
            var testHierchyTraits = response.First(x => x.HierarchyClassId == hierarchyClassId);

            //Then
            Assert.AreEqual("Test Item Type", testHierchyTraits.ItemType);
            Assert.AreEqual(true, testHierchyTraits.ProhibitDiscount);
            Assert.AreEqual(1, testHierchyTraits.FinancialHierarchyClassId);
        }

        private int InsertMerchandiseTestData()
        {
            var level5MerchandiseHierarchyClassId = connection.QueryFirst<int>(
                    @"DECLARE @hierarchyId INT = (SELECT TOP 1 hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = @HierarchyName)
                      DECLARE @hierarchyClassId INT    
                      DECLARE @hierarchyClassIdLevel4 INT  
                    SET @hierarchyClassId = SCOPE_IDENTITY()                       
                    INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'MerchandiseTestClass1', 1, Null)
                    SET @hierarchyClassId = SCOPE_IDENTITY()

                    INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'MerchandiseTestClass2', 2, @hierarchyClassId)
                      SET @hierarchyClassId = SCOPE_IDENTITY()

                    INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'MerchandiseTestClass3', 3, @hierarchyClassId)
                      SET @hierarchyClassId = SCOPE_IDENTITY()

                    INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'MerchandiseTestClass4', 4, @hierarchyClassId)
                      SET @hierarchyClassIdLevel4 = SCOPE_IDENTITY()

                     INSERT INTO dbo.HierarchyClassTrait([traitId],[hierarchyClassID],[uomID],[traitValue])
                     VALUES((SELECT traitID from dbo.trait where traitCode='PRH'), @hierarchyClassIdLevel4,Null, '1')

                    INSERT INTO dbo.HierarchyClass(hierarchyID, hierarchyClassName, hierarchyLevel, hierarchyParentClassID)
                      VALUES   (@hierarchyId, 'MerchandiseTestClass5', 5, @hierarchyClassIdLevel4)
                     SET @hierarchyClassId = SCOPE_IDENTITY()

                    INSERT INTO dbo.HierarchyClassTrait([traitId],[hierarchyClassID],[uomID],[traitValue])
                     VALUES((SELECT traitID from dbo.trait where traitCode='MFM'), @hierarchyClassId,Null, '1')

                    INSERT INTO dbo.HierarchyClassTrait([traitId],[hierarchyClassID],[uomID],[traitValue])
                     VALUES((SELECT traitID from dbo.trait where traitCode='NM'), @hierarchyClassId,Null, 'Test Item Type')

                    select @hierarchyClassId",
                    new { HierarchyName = "Merchandise" });

            return level5MerchandiseHierarchyClassId;
        }
    }
}