using Dapper;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Infrastructure.Models.Builders;
using Icon.Services.ItemPublisher.Infrastructure.Models.Mappers;
using Icon.Services.ItemPublisher.Infrastructure.Repositories;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Icon.Services.Newitem.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Repositories.Tests
{
    [TestClass()]
    public class ItemPublisherRepositoryTests
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
        /// Tests that the insert of MessageQueueItemArchive records works correctly
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AddMessageQueueArchiveRecords_WasRecordAdded_RecordIsAdded()
        {
            // Given.
            TestRepository testRepository = new TestRepository(this.connHelper);
            ItemPublisherRepository repository = new ItemPublisherRepository(this.connHelper.ProviderFactory, new MessageQueueItemModelBuilder(new ItemMapper()));

            await repository.AddMessageQueueHistoryRecords(
                new List<MessageQueueItemArchive>()
                {
                    new MessageQueueItemArchive(new List<MessageQueueItemModel>(),
                    Guid.Parse("48a5364b-5748-493d-80ee-748cd3008869"),
                    "Message",
                    new Dictionary<string, string>() { { "test", "value" } },
                    "ErrorMessage",
                    new List<string>() { "WarningMessage" },
                    DateTime.Parse("1900-01-01"),
                    "machine",
                    false)});

            // When.
            Entities.MessageQueueItemArchive entity = await testRepository.SelectLatestInsertedRecord();

            // Then.
            Assert.IsNotNull(entity);
            Assert.AreEqual("ErrorMessage", entity.ErrorMessage);
            Assert.AreEqual("Message", entity.Message);
            Assert.IsTrue(entity.ErrorOccurred);
            Assert.AreEqual(@"""[]""", entity.MessageQueueItemJson);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(entity.MessageHeader));
            Assert.AreEqual("WarningMessage", entity.WarningMessage);
            Assert.AreEqual("machine", entity.Machine);
            Assert.AreEqual(DateTime.Parse("1900-01-01"), entity.InsertDateUTC);
        }

        [TestMethod]
        public async Task AddDeadLetterMessage_WasRecordAdded_RecordIsAdded()
        {
            // Given.
            TestRepository testRepository = new TestRepository(this.connHelper);
            ItemPublisherRepository repository = new ItemPublisherRepository(this.connHelper.ProviderFactory, new MessageQueueItemModelBuilder(new ItemMapper()));

            // When.
            await repository.AddDeadLetterMessageQueueRecord(new MessageDeadLetterQueue("exception", new List<int>{1}));
            var result = await testRepository.SelectLatestInsertedDeadLeatterRecord();

            // Then.
            Assert.AreEqual("exception", result.Exception);
            Assert.AreEqual(1, result.ItemIds[0]);
            Assert.AreEqual("GlobalItem", result.MessageType);
        }

        /// <summary>
        /// Tests that MessageQueueItem records will dequeue
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DequeueMessageQueueRecords_DidMessageDequeue_MessageIsDequeued()
        {
            TestRepository testRepository = new TestRepository(this.connHelper);

            await connHelper.ProviderFactory.Provider.Connection.ExecuteAsync("TRUNCATE TABLE esb.MessageQueueItem", null, this.connHelper.ProviderFactory.Transaction);

            ItemPublisherRepository repository = new ItemPublisherRepository(this.connHelper.ProviderFactory, new MessageQueueItemModelBuilder(new ItemMapper()));

            int itemTypeId = await testRepository.InsertItemType();
            int itemId = await testRepository.InsertItem(itemTypeId);
            int scanCodeId = await testRepository.InsertScanCode(itemId);
            await testRepository.InsertHierarchy("test");
            await testRepository.InsertNutrition("TEST");

            int id = await testRepository.InsertMessageQueueItem(itemId);

            List<MessageQueueItemModel> records = await repository.DequeueMessageQueueRecords();

            Assert.IsTrue(records.Count > 0);

            Assert.AreEqual(itemId, records.First().Item.ItemId);
        }

        /// <summary>
        /// Tests that duplicate ItemId is dequeued but ignored
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task DequeueMessageQueueRecords_DuplicateItemId_MessageIsDequeuedAndDuplicateIsIgnored()
        {
            TestRepository testRepository = new TestRepository(this.connHelper);

            await connHelper.ProviderFactory.Provider.Connection.ExecuteAsync("TRUNCATE TABLE esb.MessageQueueItem", null, this.connHelper.ProviderFactory.Transaction);

            ItemPublisherRepository repository = new ItemPublisherRepository(this.connHelper.ProviderFactory, new MessageQueueItemModelBuilder(new ItemMapper()));

            int itemTypeId = await testRepository.InsertItemType();
            int itemId = await testRepository.InsertItem(itemTypeId);
            await testRepository.InsertScanCode(itemId);
            await testRepository.InsertHierarchy("test");
            await testRepository.InsertNutrition("TEST");

            await testRepository.InsertMessageQueueItem(itemId);
            await testRepository.InsertMessageQueueItem(itemId);

            List<MessageQueueItemModel> records = await repository.DequeueMessageQueueRecords();

            Assert.AreEqual(1, records.Count);
        }
    }
}