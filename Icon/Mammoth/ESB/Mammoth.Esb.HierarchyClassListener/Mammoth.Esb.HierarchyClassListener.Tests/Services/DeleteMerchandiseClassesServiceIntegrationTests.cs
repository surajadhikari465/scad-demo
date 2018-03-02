using Dapper;
using Icon.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.Models;
using Mammoth.Esb.HierarchyClassListener.Commands;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Testing.Core;

namespace Mammoth.Esb.HierarchyClassListener.Tests.Services
{
    [TestClass]
    public class DeleteMerchandiseClassesServiceIntegrationTests
    {
        private DeleteMerchandiseClassService deleteMerchandiseClassesService;
        private ICommandHandler<DeleteMerchandiseClassParameter> deleteCommandHandler;
        private DeleteMerchandiseClassRequest deleteMerchandiseClassesRequest;
        private SqlDbProvider dbProvider;
        private DapperSqlFactory dapperSqlFactory;
        private ObjectBuilderFactory objectBuilderFactory;

        // test data
        const int segmentA_ID = 51;
        const int familyA_ID = 52;
        const int classA_ID = 53;
        const int brickA_ID = 54;
        const int subBrickA_ID = 55;
        const int segmentB_ID = 61;
        const int segmentC_ID = 71;
        const int familyC_ID = 72;
        const int classC_ID = 73;
        const int segmentD_ID = 81;
        const int familyD_ID = 82;
        const int classD_ID = 83;
        const int brickD_ID = 84;
        const int fin1_ID = 91;
        const int fin2_ID = 92;
        const int fin3_ID = 93;
        const int brand1_ID = 96;
        HierarchyClass segmentA;
        HierarchyClass familyA;
        HierarchyClass classA;
        HierarchyClass brickA;
        HierarchyClass subBrickA;
        HierarchyClass segmentB;
        HierarchyClass segmentC;
        HierarchyClass familyC;
        HierarchyClass classC;
        HierarchyClass segmentD;
        HierarchyClass familyD;
        HierarchyClass classD;
        HierarchyClass brickD;
        HierarchyClass financialClass1;
        HierarchyClass financialClass2;
        HierarchyClass financialClass3;
        HierarchyClass brandClass1;

        [TestInitialize]
        public void Initialize()
        {
            var assembly = Assembly.GetExecutingAssembly();
            dapperSqlFactory = new DapperSqlFactory(assembly);
            objectBuilderFactory = new ObjectBuilderFactory(assembly);

            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();
            deleteCommandHandler = new DeleteMerchandiseClassCommandHandler(dbProvider);
            deleteMerchandiseClassesService = new DeleteMerchandiseClassService(deleteCommandHandler);

            SetupTestDataForMerchandiseHierarchyDelete();
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        protected HierarchyClassModel CreateHierarchyClassModelForTest(int classId,
            string className,
            string levelName = null,
            int parentId = 0,
            int hierarchyId = Hierarchies.Merchandise,
            ActionEnum action = ActionEnum.Delete,
            DateTime? timestamp = null)
        {
            return CreateHierarchyClassModelForTest(classId, className, hierarchyId, action, levelName, parentId, timestamp);
        }

        protected HierarchyClassModel CreateHierarchyClassModelForTest(int classId,
            string className,
            int hierarchyId = Hierarchies.Merchandise,
            ActionEnum action = ActionEnum.Delete,
            string levelName = null,
            int parentId = 0,
            DateTime? timestamp = null)
        {
            HierarchyClassModel hc = new HierarchyClassModel
            {
                Action = action,
                HierarchyClassId = classId,
                HierarchyClassName = className ?? $"{action} HierarchyClass {classId} Integration Test",
                HierarchyClassParentId = parentId,
                HierarchyId = hierarchyId,
                HierarchyLevelName = levelName ?? String.Empty,
                Timestamp = timestamp ?? DateTime.UtcNow
            };
            return hc;
        }

        protected HierarchyClass CreateHierarchyClassForTest(int classId,
            string className, int hierarchyId = Hierarchies.Merchandise, DateTime? timestamp = null)
        {
            var hc = new HierarchyClass
            {
                HierarchyID = hierarchyId,
                HierarchyClassID = classId,
                HierarchyClassName = className,
                AddedDate = timestamp ?? DateTime.UtcNow
            };
            return hc;
        }

        protected void SetupHierarchyClassTestData()
        {
            this.segmentA = CreateHierarchyClassForTest(segmentA_ID, "segment-A");
            this.familyA = CreateHierarchyClassForTest(familyA_ID, "family-A");
            this.classA = CreateHierarchyClassForTest(classA_ID, "class-A");
            this.brickA = CreateHierarchyClassForTest(brickA_ID, "brick-A");
            this.subBrickA = CreateHierarchyClassForTest(subBrickA_ID, "subBrick-A");

            this.segmentB = CreateHierarchyClassForTest(segmentB_ID, "segment-B");

            this.segmentC = CreateHierarchyClassForTest(segmentC_ID, "segment-C");
            this.familyC = CreateHierarchyClassForTest(familyC_ID, "family-C");
            this.classC = CreateHierarchyClassForTest(classC_ID, "class-C");

            this.segmentD = CreateHierarchyClassForTest(segmentD_ID, "segment-D");
            this.familyD = CreateHierarchyClassForTest(familyD_ID, "family-D");
            this.classD = CreateHierarchyClassForTest(classD_ID, "class-D");
            this.brickD = CreateHierarchyClassForTest(brickD_ID, "brick-D");

            this.financialClass1 = CreateHierarchyClassForTest(fin1_ID, "financial class 1", Hierarchies.Financial);
            this.financialClass2 = CreateHierarchyClassForTest(fin2_ID, "financial class 2", Hierarchies.Financial);
            this.financialClass3 = CreateHierarchyClassForTest(fin3_ID, "financial class 3", Hierarchies.Financial);

            this.brandClass1 = CreateHierarchyClassForTest(brand1_ID, "brand class 1", Hierarchies.Brands);

            var existingHierarchyClasses = new List<HierarchyClass>
            {
                segmentA, familyA, classA, brickA, subBrickA,
                segmentB,
                segmentC, familyC, classC,
                segmentD, familyD, classD, brickD,
                financialClass1, financialClass2, financialClass3,
                brandClass1
            };

            var insertHierarchyClassSql = dapperSqlFactory.BuildInsertSql<HierarchyClass>(includeScopeIdentity: false);
            int affectedRowCount = dbProvider.Connection.Execute(insertHierarchyClassSql, existingHierarchyClasses, dbProvider.Transaction);
            Assert.AreEqual(existingHierarchyClasses.Count, affectedRowCount, "Problem setting up HierarchyClass test data");
        }

        protected void SetupHierarchy_MerchandiseTestData()
        {
            var insertHierarchy_MerchandiseSql = $@"
                INSERT INTO dbo.Hierarchy_Merchandise 
	                (SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID, AddedDate)
                VALUES ({segmentA_ID}, {familyA_ID}, {classA_ID}, {brickA_ID}, {subBrickA_ID}, GETDATE())
	                , ({segmentB_ID}, null, null, null, null, GETDATE())
	                , ({segmentC_ID}, {familyC_ID}, {classC_ID}, null, null, GETDATE())
	                , ({segmentD_ID}, {familyD_ID}, {classD_ID}, {brickD_ID}, null, GETDATE())";
            var insertHierarchy_MerchandiseResult = dbProvider.Connection.Execute(
                sql: insertHierarchy_MerchandiseSql, param: null, transaction: dbProvider.Transaction);
            Assert.AreEqual(4, insertHierarchy_MerchandiseResult, "Problem setting up Hierarchy_Merchandise test data");
        }

        protected void SetupTestDataForMerchandiseHierarchyDelete()
        {
            SetupHierarchyClassTestData();
            SetupHierarchy_MerchandiseTestData();
        }

        protected int GetCount_HierarchyMerchandise()
        {
            var querySql = @"SELECT Count(HierarchyMerchandiseID) FROM dbo.Hierarchy_Merchandise;";
            var count = dbProvider.Connection.Query<int>(
                    sql: querySql, param: null, transaction: dbProvider.Transaction)
                .Single();
            return count;
        }

        protected int GetCount_HierarchyClass()
        {
            var querySql = @"SELECT Count(HierarchyClassID) FROM dbo.HierarchyClass;";
            var count = dbProvider.Connection.Query<int>(
                    sql: querySql, param: null, transaction: dbProvider.Transaction)
                .Single();
            return count;
        }

        [TestMethod]
        public void DeleteMerchandiseClassesService_RequestHasNoMerchandiseClasses_DoesNotDelete()
        {
            // Given
            int expectedHierarchy_MerchandiseDeleteCount = 0;
            int expectedHierarchyClassDeleteCount = 0;
            var testHierarchyClasses = new List<HierarchyClassModel>
            {
                CreateHierarchyClassModelForTest(financialClass1.HierarchyClassID, financialClass1.HierarchyClassName, Hierarchies.Financial),
                CreateHierarchyClassModelForTest(financialClass2.HierarchyClassID, financialClass2.HierarchyClassName, Hierarchies.Financial),
                CreateHierarchyClassModelForTest(financialClass2.HierarchyClassID, financialClass2.HierarchyClassName, Hierarchies.Financial),
            };
            deleteMerchandiseClassesRequest = new DeleteMerchandiseClassRequest { HierarchyClasses = testHierarchyClasses };
            var hierarchyClassCountBefore = GetCount_HierarchyClass();
            var merchandiseHierarchyCountBefore = GetCount_HierarchyMerchandise();

            // When
            deleteMerchandiseClassesService.ProcessHierarchyClasses(deleteMerchandiseClassesRequest);

            // Then
            var merchandiseHierarchyCountAfter = GetCount_HierarchyMerchandise();
            Assert.AreEqual(merchandiseHierarchyCountAfter,
                merchandiseHierarchyCountBefore - expectedHierarchy_MerchandiseDeleteCount,
                "No Hierarchy_Merchandise records should have been deleted during the test");
            var hierarchyClassCountAfter = GetCount_HierarchyClass();
            Assert.AreEqual(hierarchyClassCountAfter,
                hierarchyClassCountBefore - expectedHierarchyClassDeleteCount,
                "No HierarchyClass records should have been deleted during the test");
        }

        [TestMethod]
        public void DeleteMerchandiseClassesService_RequestLacksDeleteAction_DoesNotDelete()
        {
            // Given
            int expectedHierarchy_MerchandiseDeleteCount = 0;
            int expectedHierarchyClassDeleteCount = 0;
            var testHierarchyClasses = new List<HierarchyClassModel>
            {
                CreateHierarchyClassModelForTest(segmentC.HierarchyClassID, segmentC.HierarchyClassName, "SEGMENT", 0,
                    Hierarchies.Merchandise, ActionEnum.Add),
                CreateHierarchyClassModelForTest(familyC.HierarchyClassID, segmentC.HierarchyClassName, "FAMILY", segmentC.HierarchyClassID,
                    Hierarchies.Merchandise, ActionEnum.Add),
                CreateHierarchyClassModelForTest(classC.HierarchyClassID, segmentC.HierarchyClassName, "CLASS", familyC.HierarchyClassID,
                    Hierarchies.Merchandise, ActionEnum.Add),
            };
            deleteMerchandiseClassesRequest = new DeleteMerchandiseClassRequest { HierarchyClasses = testHierarchyClasses };
            var hierarchyClassCountBefore = GetCount_HierarchyClass();
            var merchandiseHierarchyCountBefore = GetCount_HierarchyMerchandise();

            // When
            deleteMerchandiseClassesService.ProcessHierarchyClasses(deleteMerchandiseClassesRequest);

            // Then
            var merchandiseHierarchyCountAfter = GetCount_HierarchyMerchandise();
            Assert.AreEqual(merchandiseHierarchyCountAfter,
                merchandiseHierarchyCountBefore - expectedHierarchy_MerchandiseDeleteCount,
                "No Hierarchy_Merchandise records should have been deleted during the test");
            var hierarchyClassCountAfter = GetCount_HierarchyClass();
            Assert.AreEqual(hierarchyClassCountAfter,
                hierarchyClassCountBefore - expectedHierarchyClassDeleteCount,
                "No HierarchyClass records should have been deleted during the test");
        }

        [TestMethod]
        public void DeleteMerchandiseClassesService_RequestHasValidMerchandiseClasses_DeletesHierarchyClasses()
        {
            // Given
            var testHierarchyClasses = new List<HierarchyClassModel>
            {
                CreateHierarchyClassModelForTest(segmentA.HierarchyClassID, segmentA.HierarchyClassName, "SEGMENT", 0),
                CreateHierarchyClassModelForTest(familyA.HierarchyClassID, familyA.HierarchyClassName, "FAMILY", segmentA.HierarchyClassID),
                CreateHierarchyClassModelForTest(classA.HierarchyClassID, classA.HierarchyClassName, "CLASS", familyA.HierarchyClassID),
                CreateHierarchyClassModelForTest(brickA.HierarchyClassID, brickA.HierarchyClassName, "BRICK", classA.HierarchyClassID),
                CreateHierarchyClassModelForTest(subBrickA.HierarchyClassID, subBrickA.HierarchyClassName, "SUBBRICK", brickA.HierarchyClassID),
                CreateHierarchyClassModelForTest(segmentB.HierarchyClassID, segmentB.HierarchyClassName, "SEGMENT", 0),
                CreateHierarchyClassModelForTest(segmentC.HierarchyClassID, segmentC.HierarchyClassName, "SEGMENT", 0),
                CreateHierarchyClassModelForTest(familyC.HierarchyClassID, familyC.HierarchyClassName, "FAMILY", segmentC.HierarchyClassID),
                CreateHierarchyClassModelForTest(classC.HierarchyClassID, segmentC.HierarchyClassName, "CLASS", familyC.HierarchyClassID),
                CreateHierarchyClassModelForTest(segmentD.HierarchyClassID, segmentD.HierarchyClassName, "SEGMENT", 0),
                CreateHierarchyClassModelForTest(familyD.HierarchyClassID, familyD.HierarchyClassName, "FAMILY", segmentD.HierarchyClassID),
                CreateHierarchyClassModelForTest(classD.HierarchyClassID, classD.HierarchyClassName, "CLASS", familyD.HierarchyClassID),
                CreateHierarchyClassModelForTest(brickD.HierarchyClassID, brickD.HierarchyClassName, "BRICK", classD.HierarchyClassID),
            };
            int expectedHierarchyClassDeleteCount = testHierarchyClasses.Count;
            var hierarchyClassCountBefore = GetCount_HierarchyClass();
            deleteMerchandiseClassesRequest = new DeleteMerchandiseClassRequest { HierarchyClasses = testHierarchyClasses };

            // When
            deleteMerchandiseClassesService.ProcessHierarchyClasses(deleteMerchandiseClassesRequest);

            // Then
            var hierarchyClassCountAfter = GetCount_HierarchyClass();
            Assert.AreEqual(hierarchyClassCountAfter,
                hierarchyClassCountBefore - expectedHierarchyClassDeleteCount,
                $"{expectedHierarchyClassDeleteCount} fewer HierarchyClass records should have been found after the test." +
                $" Observed counts: {hierarchyClassCountBefore} before, {hierarchyClassCountAfter} after");
        }

        [TestMethod]
        public void DeleteMerchandiseClassesService_RequestHasValidMerchandiseClasses_DeletesMerchandiseHierarchy()
        {
            // Given
            var testHierarchyClasses = new List<HierarchyClassModel>
            {
                CreateHierarchyClassModelForTest(segmentA.HierarchyClassID, segmentA.HierarchyClassName, "SEGMENT", 0),
                CreateHierarchyClassModelForTest(familyA.HierarchyClassID, familyA.HierarchyClassName, "FAMILY", segmentA.HierarchyClassID),
                CreateHierarchyClassModelForTest(classA.HierarchyClassID, classA.HierarchyClassName, "CLASS", familyA.HierarchyClassID),
                CreateHierarchyClassModelForTest(brickA.HierarchyClassID, brickA.HierarchyClassName, "BRICK", classA.HierarchyClassID),
                CreateHierarchyClassModelForTest(subBrickA.HierarchyClassID, subBrickA.HierarchyClassName, "SUBBRICK", brickA.HierarchyClassID),
                CreateHierarchyClassModelForTest(segmentB.HierarchyClassID, segmentB.HierarchyClassName, "SEGMENT", 0),
                CreateHierarchyClassModelForTest(segmentC.HierarchyClassID, segmentC.HierarchyClassName, "SEGMENT", 0),
                CreateHierarchyClassModelForTest(familyC.HierarchyClassID, familyC.HierarchyClassName, "FAMILY", segmentC.HierarchyClassID),
                CreateHierarchyClassModelForTest(classC.HierarchyClassID, segmentC.HierarchyClassName, "CLASS", familyC.HierarchyClassID),
                CreateHierarchyClassModelForTest(segmentD.HierarchyClassID, segmentD.HierarchyClassName, "SEGMENT", 0),
                CreateHierarchyClassModelForTest(familyD.HierarchyClassID, familyD.HierarchyClassName, "FAMILY", segmentD.HierarchyClassID),
                CreateHierarchyClassModelForTest(classD.HierarchyClassID, classD.HierarchyClassName, "CLASS", familyD.HierarchyClassID),
                CreateHierarchyClassModelForTest(brickD.HierarchyClassID, brickD.HierarchyClassName, "BRICK", classD.HierarchyClassID),
            };
            int expectedHierarchy_MerchandiseDeleteCount = 4;
            var merchandiseHierarchyCountBefore = GetCount_HierarchyMerchandise();
            deleteMerchandiseClassesRequest = new DeleteMerchandiseClassRequest { HierarchyClasses = testHierarchyClasses };

            // When
            deleteMerchandiseClassesService.ProcessHierarchyClasses(deleteMerchandiseClassesRequest);

            // Then
            var merchandiseHierarchyCountAfter = GetCount_HierarchyMerchandise();
            Assert.AreEqual(merchandiseHierarchyCountAfter,
                merchandiseHierarchyCountBefore - expectedHierarchy_MerchandiseDeleteCount,
                $"{expectedHierarchy_MerchandiseDeleteCount} fewer Hierarchy_Merchandise records should have been found after the test." +
                $" Observed counts: {merchandiseHierarchyCountBefore} before, {merchandiseHierarchyCountAfter} after");
        }

        [TestMethod]
        public void DeleteMerchandiseClassesService_RequestIncludesSomeNonMerchandiseHierarchyClasses_OnlyDeletesMerchandiseClasses()
        {
            // Given
            var testHierarchyClasses = new List<HierarchyClassModel>
            {
                CreateHierarchyClassModelForTest(segmentD.HierarchyClassID, segmentD.HierarchyClassName, "SEGMENT", 0),
                CreateHierarchyClassModelForTest(familyD.HierarchyClassID, familyD.HierarchyClassName, "FAMILY", segmentD.HierarchyClassID),
                CreateHierarchyClassModelForTest(classD.HierarchyClassID, classD.HierarchyClassName, "CLASS", familyD.HierarchyClassID),
                CreateHierarchyClassModelForTest(brickD.HierarchyClassID, brickD.HierarchyClassName, "BRICK", classD.HierarchyClassID),
                CreateHierarchyClassModelForTest(brandClass1.HierarchyClassID, brandClass1.HierarchyClassName, Hierarchies.Brands),
            };
            int expectedHierarchy_MerchandiseDeleteCount = 1;
            int expectedHierarchyClassDeleteCount = testHierarchyClasses.Count - 1;
            var hierarchyClassCountBefore = GetCount_HierarchyClass();
            var merchandiseHierarchyCountBefore = GetCount_HierarchyMerchandise();
            deleteMerchandiseClassesRequest = new DeleteMerchandiseClassRequest { HierarchyClasses = testHierarchyClasses };

            // When
            deleteMerchandiseClassesService.ProcessHierarchyClasses(deleteMerchandiseClassesRequest);

            // Then
            var merchandiseHierarchyCountAfter = GetCount_HierarchyMerchandise();
            Assert.AreEqual(merchandiseHierarchyCountAfter,
                merchandiseHierarchyCountBefore - expectedHierarchy_MerchandiseDeleteCount,
                $"{expectedHierarchyClassDeleteCount} fewer Hierarchy_Merchandise records should have been found after the test." +
                $" Observed counts: {merchandiseHierarchyCountBefore} before, {merchandiseHierarchyCountAfter} after");
            var hierarchyClassCountAfter = GetCount_HierarchyClass();
            Assert.AreEqual(hierarchyClassCountAfter,
                hierarchyClassCountBefore - expectedHierarchyClassDeleteCount,
                $"{expectedHierarchyClassDeleteCount} fewer HierarchyClass records should have been found after the test." +
                $" Observed counts: {hierarchyClassCountBefore} before, {hierarchyClassCountAfter} after");
        }

        [TestMethod]
        public void DeleteMerchandiseClassesService_RequestOnlyIncludesSubBrick_HigherMerchandiseHierarchyRemain()
        {
            // Given
            var testHierarchyClasses = new List<HierarchyClassModel>
            {
                CreateHierarchyClassModelForTest(subBrickA.HierarchyClassID, subBrickA.HierarchyClassName, "SUBBRICK", brickA.HierarchyClassID),
            };
            int expectedHierarchy_MerchandiseDeleteCount = 0;
            int expectedHierarchyClassDeleteCount = testHierarchyClasses.Count;
            var hierarchyClassCountBefore = GetCount_HierarchyClass();
            var merchandiseHierarchyCountBefore = GetCount_HierarchyMerchandise();
            deleteMerchandiseClassesRequest = new DeleteMerchandiseClassRequest { HierarchyClasses = testHierarchyClasses };

            // When
            deleteMerchandiseClassesService.ProcessHierarchyClasses(deleteMerchandiseClassesRequest);

            // Then
            var merchandiseHierarchyCountAfter = GetCount_HierarchyMerchandise();
            Assert.AreEqual(merchandiseHierarchyCountAfter,
                merchandiseHierarchyCountBefore - expectedHierarchy_MerchandiseDeleteCount,
                $"{expectedHierarchyClassDeleteCount} fewer Hierarchy_Merchandise records should have been found after the test." +
                $" Observed counts: {merchandiseHierarchyCountBefore} before, {merchandiseHierarchyCountAfter} after");
            var hierarchyClassCountAfter = GetCount_HierarchyClass();
            Assert.AreEqual(hierarchyClassCountAfter,
                hierarchyClassCountBefore - expectedHierarchyClassDeleteCount,
                $"{expectedHierarchyClassDeleteCount} fewer HierarchyClass records should have been found after the test." +
                $" Observed counts: {hierarchyClassCountBefore} before, {hierarchyClassCountAfter} after");
        }

        [TestMethod]
        public void DeleteMerchandiseClassesService_RequestOnlyIncludesFamily_HigherMerchandiseHierarchyRemain()
        {
            // deleting a family-level merch hierarchy class should not affect the segment association
            // Given
            var testHierarchyClasses = new List<HierarchyClassModel>
            {
                CreateHierarchyClassModelForTest(familyC.HierarchyClassID, familyC.HierarchyClassName, "FAMILY", segmentC.HierarchyClassID),
            };
            int expectedHierarchy_MerchandiseDeleteCount = 0;
            int expectedHierarchyClassDeleteCount = testHierarchyClasses.Count;
            var hierarchyClassCountBefore = GetCount_HierarchyClass();
            var merchandiseHierarchyCountBefore = GetCount_HierarchyMerchandise();
            deleteMerchandiseClassesRequest = new DeleteMerchandiseClassRequest { HierarchyClasses = testHierarchyClasses };

            // When
            deleteMerchandiseClassesService.ProcessHierarchyClasses(deleteMerchandiseClassesRequest);

            // Then
            var merchandiseHierarchyCountAfter = GetCount_HierarchyMerchandise();
            Assert.AreEqual(merchandiseHierarchyCountAfter,
                merchandiseHierarchyCountBefore - expectedHierarchy_MerchandiseDeleteCount,
                $"{expectedHierarchyClassDeleteCount} fewer Hierarchy_Merchandise records should have been found after the test." +
                $" Observed counts: {merchandiseHierarchyCountBefore} before, {merchandiseHierarchyCountAfter} after");
            var hierarchyClassCountAfter = GetCount_HierarchyClass();
            Assert.AreEqual(hierarchyClassCountAfter,
                hierarchyClassCountBefore - expectedHierarchyClassDeleteCount,
                $"{expectedHierarchyClassDeleteCount} fewer HierarchyClass records should have been found after the test." +
                $" Observed counts: {hierarchyClassCountBefore} before, {hierarchyClassCountAfter} after");
        }

        [TestMethod]
        public void DeleteMerchandiseClassesService_RequestOnlyIncludesClass_LowerMerchandiseHierarchyRemoved()
        {
            // deleting a class-level merch hierarchy class should not affect the segment or family associations
            // but it should null out the class-, brick-, and subBrick-level fields in the Merchandise_Hierarchy table
            // while leaving the brick and subBrick hierarchy classes themselves in place
            // Given
            var testHierarchyClasses = new List<HierarchyClassModel>
            {
                CreateHierarchyClassModelForTest(classA_ID, classA.HierarchyClassName, "CLASS", familyA.HierarchyClassID),
            };
            // no rows should be removed from Merchandise_Hierarchy
            int expectedHierarchy_MerchandiseDeleteCount = 0;
            // our request only has 1 class in it, so that should be the only one removed
            int expectedHierarchyClassDeleteCount = testHierarchyClasses.Count;
            var hierarchyClassCountBefore = GetCount_HierarchyClass();
            var merchandiseHierarchyCountBefore = GetCount_HierarchyMerchandise();
            deleteMerchandiseClassesRequest = new DeleteMerchandiseClassRequest { HierarchyClasses = testHierarchyClasses };

            // When
            deleteMerchandiseClassesService.ProcessHierarchyClasses(deleteMerchandiseClassesRequest);

            // Then
            // the same record should still be in Hierarchy_Merchandise, since only the class was deleted
            var merchandiseHierarchyCountAfter = GetCount_HierarchyMerchandise();
            Assert.AreEqual(merchandiseHierarchyCountAfter,
                merchandiseHierarchyCountBefore - expectedHierarchy_MerchandiseDeleteCount,
                $"{expectedHierarchyClassDeleteCount} fewer Hierarchy_Merchandise records should have been found after the test." +
                $" Observed counts: {merchandiseHierarchyCountBefore} before, {merchandiseHierarchyCountAfter} after");
            // only the HierarchyClass at the Class level should have been deleted, with all others still in place
            var hierarchyClassCountAfter = GetCount_HierarchyClass();
            Assert.AreEqual(hierarchyClassCountAfter,
                hierarchyClassCountBefore - expectedHierarchyClassDeleteCount,
                $"{expectedHierarchyClassDeleteCount} fewer HierarchyClass records should have been found after the test." +
                $" Observed counts: {hierarchyClassCountBefore} before, {hierarchyClassCountAfter} after");
            
            // check that the Merchandise_Hierarchy has had the Class, Brick, & SubBrick nulled out
            //   but that the segment & family are still there
            var querySql = $@"SELECT 
                    HierarchyMerchandiseID, SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID, AddedDate, ModifiedDate 
                    FROM dbo.Hierarchy_Merchandise 
                    WHERE SegmentHCID = {segmentA_ID}
                        AND FamilyHCID = {familyA_ID};";
            var dapperRow = dbProvider.Connection.QuerySingle(sql: querySql, param: null, transaction: dbProvider.Transaction);
            Assert.AreEqual(segmentA_ID, dapperRow.SegmentHCID);
            Assert.AreEqual(familyA_ID, dapperRow.FamilyHCID);
            Assert.IsNull(dapperRow.ClassHCID);
            Assert.IsNull(dapperRow.BrickHCID);
            Assert.IsNull(dapperRow.SubBrickHCID);
            // should have updated 
            Assert.IsNotNull(dapperRow.ModifiedDate);
            Assert.IsTrue(dapperRow.ModifiedDate >= dapperRow.AddedDate);

            // check that the HierarchyClasses originally associated with the Merchandise_Hierarchy record are still there
            //   (except for the target class)
            querySql = $@"SELECT * FROM dbo.HierarchyClass
                    WHERE HierarchyClassID IN ({segmentA_ID}, {familyA_ID}, {classA_ID}, {brickA_ID}, {subBrickA_ID})
                        AND HierarchyId = {Hierarchies.Merchandise};";
            var nonDeletedClasses = dbProvider.Connection.Query<HierarchyClass>(sql: querySql, param: null, transaction: dbProvider.Transaction);
            Assert.AreEqual(4, nonDeletedClasses.Count(), "Hierarchy Classes for segment, family, brick, & subbrick should not have been deleted");
            Assert.AreEqual("segment-A", nonDeletedClasses.Single(hc => hc.HierarchyClassID == segmentA_ID).HierarchyClassName);
            Assert.AreEqual("family-A", nonDeletedClasses.Single(hc => hc.HierarchyClassID == familyA_ID).HierarchyClassName);
            Assert.AreEqual("brick-A", nonDeletedClasses.Single(hc => hc.HierarchyClassID == brickA_ID).HierarchyClassName);
            Assert.AreEqual("subBrick-A", nonDeletedClasses.Single(hc => hc.HierarchyClassID == subBrickA_ID).HierarchyClassName);
        }

        [TestMethod]
        public void DeleteMerchandiseClassesService_RequestOnlyIncludesSegments_EntireHierarchyMerchandiseRecordsRemoved()
        {
            // Given
            var testHierarchyClasses = new List<HierarchyClassModel>
            {
                CreateHierarchyClassModelForTest(segmentA.HierarchyClassID, segmentA.HierarchyClassName, "SEGMENT", 0),
                CreateHierarchyClassModelForTest(segmentB.HierarchyClassID, segmentB.HierarchyClassName, "SEGMENT", 0),
                CreateHierarchyClassModelForTest(segmentC.HierarchyClassID, segmentC.HierarchyClassName, "SEGMENT", 0),
                CreateHierarchyClassModelForTest(segmentD.HierarchyClassID, segmentD.HierarchyClassName, "SEGMENT", 0),
            };
            int expectedHierarchy_MerchandiseDeleteCount = 4;
            int expectedHierarchyClassDeleteCount = testHierarchyClasses.Count;
            var hierarchyClassCountBefore = GetCount_HierarchyClass();
            var merchandiseHierarchyCountBefore = GetCount_HierarchyMerchandise();
            deleteMerchandiseClassesRequest = new DeleteMerchandiseClassRequest { HierarchyClasses = testHierarchyClasses };

            // When
            deleteMerchandiseClassesService.ProcessHierarchyClasses(deleteMerchandiseClassesRequest);

            // Then
            var merchandiseHierarchyCountAfter = GetCount_HierarchyMerchandise();
            Assert.AreEqual(merchandiseHierarchyCountAfter,
                merchandiseHierarchyCountBefore - expectedHierarchy_MerchandiseDeleteCount,
                $"{expectedHierarchyClassDeleteCount} fewer Hierarchy_Merchandise records should have been found after the test." +
                $" Observed counts: {merchandiseHierarchyCountBefore} before, {merchandiseHierarchyCountAfter} after");
            var hierarchyClassCountAfter = GetCount_HierarchyClass();
            Assert.AreEqual(hierarchyClassCountAfter,
                hierarchyClassCountBefore - expectedHierarchyClassDeleteCount,
                $"{expectedHierarchyClassDeleteCount} fewer HierarchyClass records should have been found after the test." +
                $" Observed counts: {hierarchyClassCountBefore} before, {hierarchyClassCountAfter} after");
        }
    }
}
