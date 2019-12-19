using Dapper;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Icon.Services.Newitem.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.IntegrationTests
{
    //[TestClass()]
    public class EndtoEndTests
    {
        private ConnectionHelper connHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            this.connHelper = new ConnectionHelper();
            this.connHelper.ProviderFactory.BeginTransaction();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.connHelper.ProviderFactory.RollbackTransaction();
        }

        /// <summary>
        /// This test will insert a record into the esb.MessageQueueItem queue table then launch
        /// the NewItem service and then process the record
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task EndToEndTest()
        {
            TestRepository testRepository = new TestRepository(this.connHelper);

            await connHelper.ProviderFactory.Provider.Connection.ExecuteAsync("TRUNCATE TABLE esb.MessageQueueItem", null, connHelper.ProviderFactory.Transaction);

            TestRepository testDataRepository = new TestRepository(connHelper);

            int itemTypeId = await testDataRepository.InsertItemType();
            int itemId = await testDataRepository.InsertItem(itemTypeId);

            await testRepository.InsertMessageQueueItem(itemId);

            var process = System.Diagnostics.Process.Start(@"C:\Users\2202809\Documents\repos\SCAD\Icon\ItemService\Icon.Services.ItemPublisher\bin\Debug\Icon.Services.ItemPublisher.exe");

            await Task.Delay(2000);

            process.Kill();

            MessageQueueItemArchive entity = await connHelper.ProviderFactory.Provider.Connection.QueryFirstAsync<MessageQueueItemArchive>($@"SELECT
                    TOP(1)
                    [MessageQueueItem]
                   ,[ErrorOccurred]
                   ,[ErrorMessage]
                   ,[Message]
                   ,[MessageHeader]
                   FROM [esb].[MessageQueueItemArchive]
                   ORDER BY MessageQueueItemArchiveId DESC",
              null,
              connHelper.ProviderFactory.Provider.Transaction);

            Assert.IsTrue(string.IsNullOrWhiteSpace(entity.ErrorMessage));
            Assert.IsFalse(entity.ErrorOccurred);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(entity.Message));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(entity.MessageQueueItemJson));
        }
    }
}