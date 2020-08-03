﻿using System;
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
    [TestClass]
    public class GetItemGroupAssociationSearchItemPartialQueryTests
    {
        private const string PrimaryItemScanCode =   "1234567890101";
        private const string SecondaryItemScanCode = "1234567890102";
        private const string ItemGroupDescription = "{\"PriceLineDescription\":\"Evil Cat Litter $.85 1 EA\",\"PriceLineRetailSize\":\"1\",\"PriceLineUOM\":\"EA\"}";
        private IDbConnection connection;
        private TransactionScope transaction;
        private GetItemGroupAssociationSearchItemPartialQuery query;
        private GetItemGroupAssociationSearchItemPartialParameters queryParameters;
        private ItemTestHelper itemTestHelper;

        /// <summary>
        /// Initialize Data for each data.
        /// </summary>
        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.connection = SqlConnectionBuilder.CreateIconConnection();
            this.queryParameters = new GetItemGroupAssociationSearchItemPartialParameters();
            this.query = new GetItemGroupAssociationSearchItemPartialQuery(this.connection);
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
        /// Test Search Method.
        /// </summary>
        [TestMethod]
        public void Search_test()
        {
            // Given
            int itemGroupId = CreateTestData(ItemGroupTypeId.Priceline);

            // When
            this.queryParameters.ExcludeItemGroupId = 123;
            this.queryParameters.ItemGroupTypeId = ItemGroupTypeId.Priceline;
            this.queryParameters.ScanCodePrefix = "123456789010%";
            this.queryParameters.MaxResultSize = 1000;
            var result = this.query.Search(this.queryParameters).ToList();

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(i => i.ScanCode == PrimaryItemScanCode));
            Assert.IsTrue(result.Any(i => i.ScanCode == SecondaryItemScanCode));
        }

        /// <summary>
        /// Validate Arguments.
        /// </summary>
        [TestMethod]
        public void GetItemGroupByIdQuery_Validate_Argument_Null()
        {
            // Given

            // When / Then
            Assert.ThrowsException<ArgumentNullException>(() => this.query.Search(null));
            Assert.ThrowsException<ArgumentNullException>(() => new GetItemGroupAssociationSearchItemPartialQuery(null));
        }


        /// <summary>
        /// Setup Data.
        /// </summary>
        /// <param name="itemGroupTypeId">ItemGroupTypeId</param>
        /// <returns>ItemGroupId</returns>
        private int CreateTestData(ItemGroupTypeId itemGroupTypeId)
        {
            var item = itemTestHelper.CreateDefaultTestItem();
            item.ScanCode = PrimaryItemScanCode;
            itemTestHelper.SaveItem(item);

            var item2 = itemTestHelper.CreateDefaultTestItem();
            item2.ScanCode = SecondaryItemScanCode;
            itemTestHelper.SaveItem(item2);


            var itemGroupId = this.connection.QuerySingle<int>(@"
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
