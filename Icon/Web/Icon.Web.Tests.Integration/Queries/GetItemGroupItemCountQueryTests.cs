using System;
using System.Data;
using System.Linq;
using System.Transactions;
using Dapper;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Integration.Queries
{
    /// <summary>
    /// GetItemGroupItemCountQuery Tests.
    /// </summary>
    [TestClass]
    public class GetItemGroupItemCountQueryTests
    {
        private IDbConnection connection;
        private TransactionScope transaction;
        private GetItemGroupItemCountParameters queryParameters;
        private GetItemGroupItemCountQuery query;
        private ItemTestHelper itemTestHelper;

        /// <summary>
        /// Initialize Data for each data.
        /// </summary>
        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.connection = SqlConnectionBuilder.CreateIconConnection();
            this.queryParameters = new GetItemGroupItemCountParameters();
            this.query = new GetItemGroupItemCountQuery(this.connection);
            itemTestHelper = new ItemTestHelper();
            itemTestHelper.Initialize(this.connection, initializeTestItem: false);
        }

        /// <summary>
        /// Cleanup Data for each test.
        /// </summary>
        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Dispose();
            this.connection.Dispose();
        }

        /// <summary>
        /// Verify that GetItemGroupItemCountQuery returns values for Skus.
        /// </summary>
        [TestMethod]
        public void GetItemGroupItemCountQuery_Sku_Returns_values()
        {
            // Given
            long itemGroupId = CreateTestData(ItemGroupTypeId.Sku);

            // When
            this.queryParameters.ItemGroupTypeId = ItemGroupTypeId.Sku;
            var result = this.query.Search(this.queryParameters).ToList();

            // Then
            Assert.IsTrue(result.Count > 0);
            var skuCount = result.FirstOrDefault(s => s.ItemGroupId == itemGroupId);
            Assert.IsNotNull(itemGroupId);
            Assert.AreEqual(2, skuCount.CountOfItems);
        }

        /// <summary>
        /// Verify that GetItemGroupItemCountQuery returns values for Priceline.
        /// </summary>
        [TestMethod]
        public void GetItemGroupItemCountQuery_PriceLine_Returns_values()
        {
            // Given
            long itemGroupId = CreateTestData(ItemGroupTypeId.Priceline);

            // When
            this.queryParameters.ItemGroupTypeId = ItemGroupTypeId.Priceline;
            var result = this.query.Search(this.queryParameters).ToList();

            // Then
            Assert.IsTrue(result.Count > 0);
            var testPriceLineCount = result.FirstOrDefault(p => p.ItemGroupId == itemGroupId);
            Assert.IsNotNull(testPriceLineCount);
            Assert.AreEqual(2, testPriceLineCount.CountOfItems);
        }


        /// <summary>
        /// Verify that GetItemGroupQuery validates arguments.
        /// </summary>
        [TestMethod]
        public void GetItemGroupItemCountQuery_Validate_Argument_Null()
        {
            // Given

            // When / Then
            Assert.ThrowsException<ArgumentNullException>(() => this.query.Search(null));
            Assert.ThrowsException<ArgumentNullException>(() => new GetItemGroupQuery(null));
        }

        private long CreateTestData(ItemGroupTypeId itemGroupTypeId)
        {
            var item = itemTestHelper.CreateDefaultTestItem();
            item.ScanCode = "1234567890101";
            itemTestHelper.SaveItem(item);

            var item2 = itemTestHelper.CreateDefaultTestItem();
            item2.ScanCode = "1234567890102";
            itemTestHelper.SaveItem(item2);

            var attributesJson = "{ \"SKUDescription\":\"ItemGroup Descrition Test 1\"}";

            var itemGroupId = this.connection.QuerySingle<long>(@"
            INSERT INTO [dbo].[ItemGroup]([ItemGroupTypeId], [ItemGroupAttributesJson], [LastModifiedBy])
            VALUES (@ItemGroupTypeId, @ItemGroupAttributesJson, 'test');
            SELECT SCOPE_IDENTITY();", new
            {
                ItemGroupTypeId = (int)itemGroupTypeId,
                ItemGroupAttributesJson = attributesJson
            });

            this.connection.Execute(@"
            INSERT INTO [dbo].[ItemGroupMember] ([ItemId],[ItemGroupId],[IsPrimary],[LastModifiedBy])
            VALUES (@ItemId, @ItemGroupId,@IsPrimary, 'Test')", new
            {
                ItemId = item.ItemId,
                ItemGroupId = itemGroupId,
                IsPrimary = 1
            });

            this.connection.Execute(@"
            INSERT INTO [dbo].[ItemGroupMember] ([ItemId],[ItemGroupId],[IsPrimary],[LastModifiedBy])
            VALUES (@ItemId, @ItemGroupId,@IsPrimary, 'Test')", new
            {
                ItemId = item2.ItemId,
                ItemGroupId = itemGroupId,
                IsPrimary = 0
            });
            return itemGroupId;
        }
    }
}
