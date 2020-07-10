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
    public class GetItemGroupQueryTests
    {
        private const string PrimaryItemScanCode = "1234567890101";
        private const string ItemGroupDescription = "{ \"SKUDescription\":\"ItemGroup Descrition Test 1\"}";
        private IDbConnection connection;
        private TransactionScope transaction;
        private GetItemGroupParameters queryParameters;
        private GetItemGroupQuery query;
        private ItemTestHelper itemTestHelper;

        /// <summary>
        /// Initialize Data for each data.
        /// </summary>
        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.connection = SqlConnectionBuilder.CreateIconConnection();
            this.queryParameters = new GetItemGroupParameters();
            this.query = new GetItemGroupQuery(this.connection);
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
        public void GetItemGroupQuery_Sku_Returns_values()
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
            Assert.AreEqual(ItemGroupDescription, itemGroup.ItemGroupAttributesJson);
            Assert.AreEqual(PrimaryItemScanCode, itemGroup.ScanCode);
        }

        /// <summary>
        /// Verify that GetItemGroupQuery returns values for Priceline.
        /// </summary>
        [TestMethod]
        public void GetItemGroupQuery_PriceLine_Returns_values()
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
            Assert.AreEqual(ItemGroupDescription, itemGroup.ItemGroupAttributesJson);
            Assert.AreEqual(PrimaryItemScanCode, itemGroup.ScanCode);
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
            Assert.ThrowsException<ArgumentNullException>(() => new GetItemGroupQuery(null));
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
