using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.DataAccess.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Transactions;
using Icon.Web.Tests.Integration.TestHelpers;
using Newtonsoft.Json;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetItemHistoryQueryHandlerTests
    {
        private IDbConnection dbConnection;
        private TransactionScope transaction;
        private GetItemHistoryQueryHandler queryHandler;
        private ItemTestHelper itemTestHelper;

        [TestInitialize]
        public void Initialize()
        {
            this.transaction = new TransactionScope();
            this.dbConnection = SqlConnectionBuilder.CreateIconConnection();
            this.dbConnection.Open();
            this.queryHandler = new GetItemHistoryQueryHandler(this.dbConnection);
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
        [Ignore("42363 - Tech Debt - Unit test need further investigation")]
        public void Search_ShouldReturnPrimaryItemRecordAndHistoryRecord()
        {
            var item = this.itemTestHelper.TestItem;
            item.ItemAttributesJson = JsonConvert.SerializeObject(
                new Dictionary<string, string>()
                {
                    { "A", "Value"}
                });

            // update item to create a history record
            this.itemTestHelper.UpdateItem(item);

            var result = this.queryHandler.Search(new GetItemHistoryParameters()
            {
                ItemId = this.itemTestHelper.TestItem.ItemId
            });

            Assert.AreEqual(2, result.ToList().Count, "There should be two records. The item record and one history record.");
         
        }
    }
}