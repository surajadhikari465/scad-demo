using System;
using System.Data;
using System.Linq;
using System.Transactions;
using Dapper;
using Icon.Web.DataAccess;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddItemToItemGroupCommandHandlerTests
    {
        private const string PrimaryItemScanCode = "1234567890101";
        private const string SecondaryItemScanCode = "9912345678901";
        private const string ItemGroupDescription = "{\"PriceLineDescription\":\"Evil Cat Litter $.85 1 EA\",\"PriceLineRetailSize\":\"1\",\"PriceLineUOM\":\"EA\"}";

        private IDbConnection connection;
        private TransactionScope transaction;
        private GetItemGroupMembersQuery query;
        private GetItemGroupMembersParameters queryParameters;
        private AddItemToItemGroupCommandHandler commandHandler;
        private AddItemToItemGroupCommand commandParameters;
        private ItemTestHelper itemTestHelper;
        private ItemDbModel item1;
        private ItemDbModel item2;


        /// <summary>
        /// Initialize Data for each data.
        /// </summary>
        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.connection = SqlConnectionBuilder.CreateIconConnection();
            this.queryParameters = new GetItemGroupMembersParameters();
            this.query = new GetItemGroupMembersQuery(this.connection);
            this.commandParameters = new AddItemToItemGroupCommand();
            this.commandHandler = new AddItemToItemGroupCommandHandler(this.connection);
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
        /// Search test.
        /// </summary>
        [TestMethod]
        public void Execute_test()
        {
            // Given
            int itemGroupId = CreateTestData(ItemGroupTypeId.Priceline);

            // When
            commandHandler.Execute(
                new AddItemToItemGroupCommand
                {
                    ItemGroupId = itemGroupId,
                    ItemId = item2.ItemId,
                });

            // Then
            this.queryParameters.ItemGroupId = itemGroupId;
            var result = this.query.Search(this.queryParameters).ToList();
            Assert.AreEqual(2, result.Count);
            var itemGroupMember1 = result.FirstOrDefault(igm => igm.IsPrimary == true);
            var itemGroupMember2 = result.FirstOrDefault(igm => igm.IsPrimary == false);
            Assert.AreEqual(PrimaryItemScanCode, itemGroupMember1.ScanCode);
            Assert.AreEqual(SecondaryItemScanCode, itemGroupMember2.ScanCode);
        }

        /// <summary>
        /// Validates Arguments.
        /// </summary>
        [TestMethod]
        public void GetItemGroupQuery_Validate_Argument_Null()
        {
            // Given

            // When / Then
            Assert.ThrowsException<ArgumentNullException>(() => this.commandHandler.Execute(null));
            Assert.ThrowsException<ArgumentNullException>(() => new AddItemToItemGroupCommandHandler(null));
        }

        /// <summary>
        /// Setup Data.
        /// </summary>
        /// <param name="itemGroupTypeId">ItemGroupTypeId</param>
        /// <returns>ItemGroupId</returns>
        private int CreateTestData(ItemGroupTypeId itemGroupTypeId)
        {
            item1 = itemTestHelper.CreateDefaultTestItem();
            item1.ScanCode = PrimaryItemScanCode;
            itemTestHelper.SaveItem(item1);

            item2 = itemTestHelper.CreateDefaultTestItem();
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
                ItemId = item1.ItemId,
                ItemGroupId = itemGroupId,
                IsPrimary = 1
            });

            return itemGroupId;
        }
    }
}
