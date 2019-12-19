using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.DataAccess.Queries;
using System;
using System.Data;
using System.Transactions;
using Icon.Web.Tests.Integration.TestHelpers;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetItemHierarchyClassHistoryQueryTests
    {
        private IDbConnection dbConnection;
        private TransactionScope transaction;
        private GetItemHierarchyClassHistoryQuery queryHandler;
        private ItemTestHelper itemTestHelper;

        [TestInitialize]
        public void Initialize()
        {
            this.transaction = new TransactionScope();
            this.dbConnection = SqlConnectionBuilder.CreateIconConnection();
            this.queryHandler = new GetItemHierarchyClassHistoryQuery(this.dbConnection);
            this.itemTestHelper = new ItemTestHelper();
            this.itemTestHelper.Initialize(dbConnection);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Dispose();
            this.dbConnection.Dispose();
        }

        [TestMethod]
        public void Search_ShouldReturnHierarchyRecordsWithHistory()
        {
            this.itemTestHelper.UpdateItemHierarchyClass(this.itemTestHelper.TestItem.ItemId, 
                this.itemTestHelper.TestHierarchyClasses["Merchandise"][0].HierarchyClassId, 
                this.itemTestHelper.TestHierarchyClasses["Merchandise"][1].HierarchyClassId);

            this.itemTestHelper.UpdateItemHierarchyClass(this.itemTestHelper.TestItem.ItemId,
                this.itemTestHelper.TestHierarchyClasses["Brands"][0].HierarchyClassId,
                this.itemTestHelper.TestHierarchyClasses["Brands"][1].HierarchyClassId);

            this.itemTestHelper.UpdateItemHierarchyClass(this.itemTestHelper.TestItem.ItemId,
                this.itemTestHelper.TestHierarchyClasses["Tax"][0].HierarchyClassId,
                this.itemTestHelper.TestHierarchyClasses["Tax"][1].HierarchyClassId);

            this.itemTestHelper.UpdateItemHierarchyClass(this.itemTestHelper.TestItem.ItemId,
                this.itemTestHelper.TestHierarchyClasses["National"][0].HierarchyClassId,
                this.itemTestHelper.TestHierarchyClasses["National"][1].HierarchyClassId);

            this.itemTestHelper.UpdateItemHierarchyClass(this.itemTestHelper.TestItem.ItemId,
                this.itemTestHelper.TestHierarchyClasses["Financial"][0].HierarchyClassId,
                this.itemTestHelper.TestHierarchyClasses["Financial"][1].HierarchyClassId);

            this.itemTestHelper.UpdateItemHierarchyClass(this.itemTestHelper.TestItem.ItemId,
            this.itemTestHelper.TestHierarchyClasses["Manufacturer"][0].HierarchyClassId,
            this.itemTestHelper.TestHierarchyClasses["Manufacturer"][1].HierarchyClassId);

            var result = this.queryHandler.Search(new GetItemHierarchyClassHistoryParameters()
            {
                ItemId = this.itemTestHelper.TestItem.ItemId
            });

            Assert.AreEqual(2, result.BrandHierarchy.Count);
            Assert.AreEqual(2, result.FinancialHierarchy.Count);
            Assert.AreEqual(2, result.MerchHierarchy.Count);
            Assert.AreEqual(2, result.TaxHierarchy.Count);
            Assert.AreEqual(2, result.NationalHierarchy.Count);
            Assert.AreEqual(2, result.ManufacturerHierarchy.Count);

            Assert.IsTrue(result.BrandHierarchy[1].SysEndTimeUtc == DateTime.MaxValue);
            Assert.IsTrue(result.FinancialHierarchy[1].SysEndTimeUtc == DateTime.MaxValue);
            Assert.IsTrue(result.MerchHierarchy[1].SysEndTimeUtc == DateTime.MaxValue);
            Assert.IsTrue(result.TaxHierarchy[1].SysEndTimeUtc == DateTime.MaxValue);
            Assert.IsTrue(result.NationalHierarchy[1].SysEndTimeUtc == DateTime.MaxValue);
            Assert.IsTrue(result.ManufacturerHierarchy[1].SysEndTimeUtc == DateTime.MaxValue);
        }
    }
}