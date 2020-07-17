using Dapper;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetItemGroupUnfilteredResultsCountQueryTests
    {
        private const string PrimaryItemScanCode = "1234567890101";
        private const string ItemGroupDescription = "{ \"SkuDescription\":\"ItemGroup Descrition Test 1\"}";
        private IDbConnection connection;
        private TransactionScope transaction;
        private GetItemGroupUnfilteredResultsCountQueryParameters queryParameters;
        private GetItemGroupUnfilteredResultsCountQuery query;
        private ItemTestHelper itemTestHelper;

        /// <summary>
        /// Initialize Data for each data.
        /// </summary>
        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.connection = SqlConnectionBuilder.CreateIconConnection();
            this.queryParameters = new GetItemGroupUnfilteredResultsCountQueryParameters()
            {                
                ItemGroupTypeId = ItemGroupTypeId.Sku,                
            };

            this.query = new GetItemGroupUnfilteredResultsCountQuery(this.connection);
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

        // <summary>
        /// Verify that GetFilteredResultsCountQuery with Filter returns values for Skus.
        /// </summary>
        [TestMethod]
        public void GetUnfilteredResultsCountQuery_Sku_returns_values()
        {
            // Given
            long itemGroupId = CreateTestData(ItemGroupTypeId.Sku);

            // When
            this.queryParameters.ItemGroupTypeId = DataAccess.Models.ItemGroupTypeId.Sku;
            var result = this.query.Search(this.queryParameters);

            // Then
            Assert.IsTrue(result > 0);
        }

        // <summary>
        /// Verify that GetFilteredResultsCountQuery with Filter returns values for Priceline.
        /// </summary>
        [TestMethod]
        public void GetUnfilteredResultsCountQuery_Priceline_returns_values()
        {
            // Given
            long itemGroupId = CreateTestData(ItemGroupTypeId.Priceline);

            // When
            this.queryParameters.ItemGroupTypeId = DataAccess.Models.ItemGroupTypeId.Priceline;
            var result = this.query.Search(this.queryParameters);

            // Then
            Assert.IsTrue(result > 0);
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
