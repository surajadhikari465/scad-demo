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
    /// GetItemGroupQuery Tests.
    /// </summary>
    [TestClass]
    public class GetItemGroupPageQueryTests
    {
        private const string PrimaryItemScanCode = "1234567890101";
        private const string ItemGroupDescription = "{ \"SKUDescription\":\"ItemGroup Descrition Test 1\", \"PriceLineDescription\":\"ItemGroup Description Test 2\"}";
        private IDbConnection connection;
        private TransactionScope transaction;
        private GetItemGroupParameters queryParameters;
        private GetItemGroupPageQuery query;
        private ItemTestHelper itemTestHelper;

        /// <summary>
        /// Initialize Data for each data.
        /// </summary>
        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.connection = SqlConnectionBuilder.CreateIconConnection();
            this.queryParameters = new GetItemGroupParameters(){ 
                PageSize = 100, 
                RowsOffset =0, 
                SearchTerm = null,
                SortColumn = ItemGroupColumns.ItemGroupId, 
                ItemGroupTypeId = ItemGroupTypeId.Sku,
                SortOrder = SortOrder.Descending
            };
            this.query = new GetItemGroupPageQuery(this.connection);
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
        /// Verify that GetItemGroupQuery returns values for Skus.
        /// </summary>
        [TestMethod]
        public void GetItemGroupQuery_Sku_returns_values()
        {
            // Given
            long itemGroupId = CreateTestData(ItemGroupTypeId.Sku);

            // When
            this.queryParameters.ItemGroupTypeId = DataAccess.Models.ItemGroupTypeId.Sku;
            var result = this.query.Search(this.queryParameters).ToList();

            // Then
            Assert.IsTrue(result.Count > 0);
            var itemGroup = result.FirstOrDefault(sku => sku.ScanCode == PrimaryItemScanCode);
            Assert.IsNotNull(itemGroup);
            Assert.AreEqual("ItemGroup Descrition Test 1", itemGroup.SKUDescription);
            Assert.AreEqual(PrimaryItemScanCode, itemGroup.ScanCode);
        }


        /// <summary>
        /// Verify that GetItemGroupQuery with Filter returns values for Skus.
        /// </summary>
        [TestMethod]
        public void GetItemGroupQuery_Sku_with_filters_returns_values()
        {
            // Given
            // The database contains more that 1 tofu product

            // When
            this.queryParameters.ItemGroupTypeId = DataAccess.Models.ItemGroupTypeId.Sku;
            this.queryParameters.SearchTerm = "Tofu";
            var result = this.query.Search(this.queryParameters);

            // Then
            Assert.IsTrue(result.Count > 0);
        }

        /// <summary>
        /// Verify that GetItemGroupQuery returns values for Priceline.
        /// </summary>
        [TestMethod]
        public void GetItemGroupQuery_PriceLine_returns_values()
        {
            // Given
            long itemGroupId = CreateTestData(ItemGroupTypeId.Priceline);

            // When
            this.queryParameters.ItemGroupTypeId = DataAccess.Models.ItemGroupTypeId.Priceline;
            var result = this.query.Search(this.queryParameters).ToList();

            // Then
            Assert.IsTrue(result.Count > 0);
            var itemGroup = result.FirstOrDefault(sku => sku.ScanCode == PrimaryItemScanCode);
            Assert.IsNotNull(itemGroup);
            Assert.AreEqual("ItemGroup Description Test 2", itemGroup.PriceLineDescription);
            Assert.AreEqual(PrimaryItemScanCode, itemGroup.ScanCode);
        }

        // <summary>
        /// Verify that GetItemGroupQuery with Filter returns values for price line.
        /// </summary>
        [TestMethod]
        public void GetItemGroupQuery_PriceLine_with_filters_returns_values()
        {
            // Given
            // The database contains more that 1 tofu product

            // When
            this.queryParameters.ItemGroupTypeId = DataAccess.Models.ItemGroupTypeId.Priceline;
            this.queryParameters.SearchTerm = "Tofu";
            var result = this.query.Search(this.queryParameters).ToList();

            // Then
            Assert.IsTrue(result.Count > 0);
        }

        // <summary>
        /// Verify that GetItemGroupQuery with Filter order by item count returns values for Priceline.
        /// </summary>
        [TestMethod]
        public void GetItemGroupQuery_PriceLine_with_filters_order_by_ItemCount_returns_values()
        {
            // Given
            // The database contains more that 1 tofu product

            // When
            this.queryParameters.ItemGroupTypeId = DataAccess.Models.ItemGroupTypeId.Priceline;
            this.queryParameters.SearchTerm = "Tofu";
            this.queryParameters.SortColumn = ItemGroupColumns.ItemCount;
            var result = this.query.Search(this.queryParameters).ToList();

            // Then
            Assert.IsTrue(result.Count > 0);
        }

        // <summary>
        /// Verify that GetItemGroupQuery with Filter order by item count returns values for Skus.
        /// </summary>
        [TestMethod]
        public void GetItemGroupQuery_Sku_with_filters_order_by_ItemCount_returns_values()
        {
            // Given
            // The database contains more that 1 tofu product

            // When
            this.queryParameters.ItemGroupTypeId = DataAccess.Models.ItemGroupTypeId.Sku;
            this.queryParameters.SearchTerm = "Tofu";
            this.queryParameters.SortColumn = ItemGroupColumns.ItemCount;
            this.queryParameters.SortOrder = SortOrder.Ascending;
            var result = this.query.Search(this.queryParameters);

            // Then
            Assert.IsTrue(result.Count > 0);
        }

        // <summary>
        /// Verify that GetItemGroupQuery with no Filter order by item count returns values for Skus.
        /// </summary>
        [TestMethod]
        public void GetItemGroupQuery_Sku_no_filters_order_by_ItemCount_returns_values()
        {
            // Given
            long itemGroupId = CreateTestData(ItemGroupTypeId.Sku);

            // When
            this.queryParameters.ItemGroupTypeId = DataAccess.Models.ItemGroupTypeId.Sku;
            this.queryParameters.SearchTerm = null;
            this.queryParameters.SortColumn = ItemGroupColumns.ItemCount;
            this.queryParameters.SortOrder = SortOrder.Ascending;
            var result = this.query.Search(this.queryParameters).ToList();

            // Then
            Assert.IsTrue(result.Count > 0);
        }

        /// <summary>
        /// Verify that GetItemGroupQuery validates arguments.
        /// </summary>
        [TestMethod]
        public void GetItemGroupQuery_Validate_Argument_Null()
        {
            // Given

            // When / Then
            Assert.ThrowsException<ArgumentNullException>(() => this.query.Search(null));
            Assert.ThrowsException<ArgumentNullException>(() => new GetItemGroupPageQuery(null));
        }

        private long CreateTestData(ItemGroupTypeId itemGroupTypeId)
        {
            var item = itemTestHelper.CreateDefaultTestItem();
            item.ScanCode = PrimaryItemScanCode;
            itemTestHelper.SaveItem(item);

            var item2 = itemTestHelper.CreateDefaultTestItem();
            item2.ScanCode = "1234567890102";
            itemTestHelper.SaveItem(item2);


            var itemGroupId = this.connection.QuerySingle<long>(@"
            INSERT INTO [dbo].[ItemGroup]([ItemGroupTypeId], [ItemGroupAttributesJson], [LastModifiedBy])
            VALUES (@ItemGroupTypeId, @ItemGroupAttributesJson, 'test');
            SELECT SCOPE_IDENTITY();", new
            {
                ItemGroupTypeId = (int)itemGroupTypeId,
                ItemGroupAttributesJson = ItemGroupDescription
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
