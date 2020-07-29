using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;
using Dapper;
using Icon.Web.Tests.Integration.TestHelpers;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetSkuBySkuIdQueryTests
    {
        private SqlConnection connection;
        private GetSkuBySkuIdQuery query;
        private TransactionScope transaction;
        private GetSkuBySkuIdParameters parameters;
        private const string PrimaryItemScanCode = "1234567890101";
        private const string ItemGroupDescription = "{ \"SkuDescription\":\"ItemGroup Descrition Test 150917697\"}";
        private ItemTestHelper itemTestHelper;

        [TestInitialize]
        public void InitializeData() 
        {
            transaction = new TransactionScope();
            connection = SqlConnectionBuilder.CreateIconConnection();
            query = new GetSkuBySkuIdQuery(connection);
            itemTestHelper = new ItemTestHelper();
            parameters = new GetSkuBySkuIdParameters();
            itemTestHelper.Initialize(this.connection, initializeTestItem: false);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            connection.Dispose();
        }

        [TestMethod]
        public void GetSkuBySkuIdQuery_SkuExists_ReturnsSku()
        {
            //When
            long itemGroupId = CreateTestData(ItemGroupTypeId.Sku);
            parameters.SkuId = (int)itemGroupId;
            //Then
            var result = query.Search(parameters);

            Assert.AreEqual(result.SkuId, parameters.SkuId);
        }

        [TestMethod]
        public void GetSkuBySkuIdQuery_SkuDoesNotExists_ReturnsNull()
        {
            //When
            parameters.SkuId =-1;
            //Then
            var result = query.Search(parameters);

            Assert.IsNull(result); ;
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