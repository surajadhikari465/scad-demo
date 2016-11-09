using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using RegionalEventController.DataAccess.Queries;
using RegionalEventController.Common;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Icon.Testing.Builders;
using Irma.Testing.Builders;

namespace RegionalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetInvalidInProcessedQueueEntriesQueryHandlerTests
    {
        private IrmaContext context;
        private GetInvalidInProcessedQueueEntriesQuery query;
        private GetInvalidInProcessedQueueEntriesQueryHandler handler;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext();
            this.query = new GetInvalidInProcessedQueueEntriesQuery();
            this.handler = new GetInvalidInProcessedQueueEntriesQueryHandler(this.context);
            this.transaction = this.context.Database.BeginTransaction();

        }

        [TestCleanup]
        public void CleanupData()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
            }

            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        [TestMethod]
        public void GetInvalidInProcessQueueEntries_QIDsWithInstancePopulated_ReturnsInvalidQIDs()
        {
            // Given
            StartupOptions.Instance = 20;
            List<IconItemChangeQueue> changes = AddIconItemChangeQueues(StartupOptions.Instance);

            // Remove one QID from list - this will be return value.
            List<int> expected = new List<int>();
            expected.Add(changes[0].QID);

            changes.Remove(changes[0]);
            this.query.queueIds = changes.Select(c => c.QID).ToList();

            // When
            List<int> actual = this.handler.Execute(this.query);

            // Then
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i], "The actual QID did not match the expected QID.");
            }
        }

        private List<IconItemChangeQueue> AddIconItemChangeQueues(int instance)
        {
            List<IconItemChangeQueue> changeQueues = new List<IconItemChangeQueue>();

            int itemKeyOne = this.context.ItemIdentifier.First(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1).Item_Key;
            string identifierOne = this.context.ItemIdentifier.First(ii => ii.Item_Key == itemKeyOne).Identifier;
            IconItemChangeQueue changeQueueOne = new TestIconItemChangeQueueBuilder().WithItemKey(itemKeyOne)
                .WithIdentifier(identifierOne).WithInProcessBy(instance.ToString()).Build();
            changeQueues.Add(changeQueueOne);

            int itemKeyTwo = this.context.ItemIdentifier.First(ii => ii.Deleted_Identifier == 0 && ii.Default_Identifier == 1 && ii.Item_Key != itemKeyOne).Item_Key;
            string identifierTwo = this.context.ItemIdentifier.First(ii => ii.Item_Key == itemKeyTwo).Identifier;
            IconItemChangeQueue changeQueueTwo = new TestIconItemChangeQueueBuilder().WithItemKey(itemKeyTwo)
                .WithIdentifier(identifierTwo).WithInProcessBy(instance.ToString()).Build();
            changeQueues.Add(changeQueueTwo);

            int itemKeyThree = this.context.ItemIdentifier.First(ii => ii.Deleted_Identifier == 0 
                && ii.Default_Identifier == 1 && ii.Item_Key != itemKeyOne && ii.Item_Key != itemKeyTwo).Item_Key;
            string identifierThree = this.context.ItemIdentifier.First(ii => ii.Item_Key == itemKeyThree).Identifier;
            IconItemChangeQueue changeQueueThree = new TestIconItemChangeQueueBuilder().WithItemKey(itemKeyThree)
                .WithIdentifier(identifierThree).WithInProcessBy(instance.ToString()).Build();
            changeQueues.Add(changeQueueThree);

            this.context.IconItemChangeQueue.AddRange(changeQueues);
            this.context.SaveChanges();

            return changeQueues;
        }
    }
}
