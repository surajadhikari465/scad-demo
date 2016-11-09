using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irma.Framework;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Models;
using Moq;
using Icon.Logging;
using Icon.Testing.Builders;
using RegionalEventController.Common;
using Irma.Testing.Builders;


namespace RegionalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class DeleteNewItemsFromIrmaQueueCommandHandlerTest
    {
        private Mock<ILogger<DeleteNewItemsFromIrmaQueueCommandHandler>> mockLogger;
        private IrmaContext context;
        private DeleteNewItemsFromIrmaQueueCommand command;
        private DeleteNewItemsFromIrmaQueueCommandHandler handler;
        private List<IrmaNewItem> failedToBeProccessedIrmaNewItems;
        private List<IrmaNewItem> successfullyProccessedIrmaNewItems;
        private List<IconItemChangeQueue> failedIconItemChangeQueueEntries;
        private List<IconItemChangeQueue> successfulIconItemChangeQueueEntries;

        [TestInitialize]
        public void InitializeData()
        {
            this.mockLogger = new Mock<ILogger<DeleteNewItemsFromIrmaQueueCommandHandler>>();
            this.context = new IrmaContext(); // this is the FL ItemCatalog_Test database
            this.command = new DeleteNewItemsFromIrmaQueueCommand();
            this.handler = new DeleteNewItemsFromIrmaQueueCommandHandler(this.mockLogger.Object, this.context);
            this.failedToBeProccessedIrmaNewItems = new List<IrmaNewItem>();
            this.successfullyProccessedIrmaNewItems = new List<IrmaNewItem>();
            this.failedIconItemChangeQueueEntries = new List<IconItemChangeQueue>();
            this.successfulIconItemChangeQueueEntries = new List<IconItemChangeQueue>();

            StartupOptions.Instance = 123;
        }

        [TestCleanup]
        public void CleanupData()
        {
            foreach (IconItemChangeQueue failedIconItemChangeQueueEntry in failedIconItemChangeQueueEntries)
            {
                this.context.Entry(failedIconItemChangeQueueEntry).Reload();
                IconItemChangeQueue queue = context.IconItemChangeQueue.Where(q => q.QID == failedIconItemChangeQueueEntry.QID).FirstOrDefault();
                if (queue != null)
                    this.context.IconItemChangeQueue.Remove(queue);
            }

            foreach (IconItemChangeQueue successfulIconItemChangeQueueEntry in successfulIconItemChangeQueueEntries)
            {
                this.context.Entry(successfulIconItemChangeQueueEntry).Reload();
                IconItemChangeQueue queue = context.IconItemChangeQueue.Where(q => q.QID == successfulIconItemChangeQueueEntry.QID).FirstOrDefault();
                if (queue != null)
                    this.context.IconItemChangeQueue.Remove(queue);
            }

            this.context.SaveChanges();

            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        private void BuildFailedToBeProccessedIrmaNewItems()
        {
            var random = new Random();

            List<int> itemKeys = new List<int>();
            itemKeys = (from i in this.context.Item
                        select i.Item_Key).Take(2).ToList();

            failedIconItemChangeQueueEntries.Add(new TestIconItemChangeQueueBuilder().WithIdentifier(random.Next(1000000, 1000000000).ToString()).WithItemKey(itemKeys.First()).WithInProcessBy(StartupOptions.Instance.ToString()));
            failedIconItemChangeQueueEntries.Add(new TestIconItemChangeQueueBuilder().WithIdentifier(random.Next(1000000, 1000000000).ToString()).WithItemKey(itemKeys.Last()).WithInProcessBy(StartupOptions.Instance.ToString()));

            this.context.IconItemChangeQueue.AddRange(failedIconItemChangeQueueEntries);
            this.context.SaveChanges();

            foreach (IconItemChangeQueue failedIconItemChangeQueueEntry in failedIconItemChangeQueueEntries)
            {
                failedIconItemChangeQueueEntry.QID = context.IconItemChangeQueue
                    .Where(q => q.Item_Key == failedIconItemChangeQueueEntry.Item_Key && q.Identifier == failedIconItemChangeQueueEntry.Identifier)
                    .Select(q => q.QID).FirstOrDefault();
            }

            failedToBeProccessedIrmaNewItems = failedIconItemChangeQueueEntries.ConvertAll(n => new RegionalEventController.DataAccess.Models.IrmaNewItem
            {
                QueueId = n.QID,
                IrmaItemKey = n.Item_Key,
                Identifier = n.Identifier,
                ProcessedByController = false
            });
        }

        private void BuildSuccessfullyProccessedIrmaNewItems()
        {
            var random = new Random();

            List<int> itemKeys = new List<int>();
            itemKeys = (from i in this.context.Item
                        select i.Item_Key).Take(2).ToList();

            successfulIconItemChangeQueueEntries.Add(new TestIconItemChangeQueueBuilder().WithIdentifier(random.Next(1000000, 1000000000).ToString()).WithItemKey(itemKeys.First()).WithInProcessBy(StartupOptions.Instance.ToString()));
            successfulIconItemChangeQueueEntries.Add(new TestIconItemChangeQueueBuilder().WithIdentifier(random.Next(1000000, 1000000000).ToString()).WithItemKey(itemKeys.Last()).WithInProcessBy(StartupOptions.Instance.ToString()));

            this.context.IconItemChangeQueue.AddRange(successfulIconItemChangeQueueEntries);
            this.context.SaveChanges();

            foreach (IconItemChangeQueue successfulIconItemChangeQueueEntry in successfulIconItemChangeQueueEntries)
            {
                successfulIconItemChangeQueueEntry.QID = context.IconItemChangeQueue
                    .Where(q => q.Item_Key == successfulIconItemChangeQueueEntry.Item_Key && q.Identifier == successfulIconItemChangeQueueEntry.Identifier)
                    .Select(q => q.QID).FirstOrDefault();
            }

            successfullyProccessedIrmaNewItems = successfulIconItemChangeQueueEntries.ConvertAll(n => new RegionalEventController.DataAccess.Models.IrmaNewItem
            {
                QueueId = n.QID,
                IrmaItemKey = n.Item_Key,
                Identifier = n.Identifier,
                ProcessedByController = true
            });
        }

        [TestMethod]
        public void DeleteNewItemsFromIrmaQueue_QueueEntriesFailToBeProcessed_QueueEntriesAreUpdatedWithProcessFailedDatePopulated()
        {
            // Given
            BuildFailedToBeProccessedIrmaNewItems();
            this.command.NewIrmaItems = this.failedToBeProccessedIrmaNewItems;

            // When
            this.handler.Execute(command);

            // Then
            foreach (IrmaNewItem failedToBeProccessedIrmaNewItem in failedToBeProccessedIrmaNewItems)
            {
                Assert.IsTrue(this.context.IconItemChangeQueue.Any(q => q.Item_Key == failedToBeProccessedIrmaNewItem.IrmaItemKey && q.Identifier == failedToBeProccessedIrmaNewItem.Identifier && q.ProcessFailedDate != null));
            }
        }

        [TestMethod]
        public void DeleteNewItemsFromIrmaQueue_QueueEntriesSuccessfullyProcessed_QueueEntriesAreDeleted()
        {
            // Given
            BuildSuccessfullyProccessedIrmaNewItems();
            this.command.NewIrmaItems = this.successfullyProccessedIrmaNewItems;

            // When
            this.handler.Execute(command);

            // Then
            foreach (IrmaNewItem successfullyProccessedIrmaNewItem in successfullyProccessedIrmaNewItems)
            {
                Assert.IsFalse(this.context.IconItemChangeQueue.Any(q => q.Item_Key == successfullyProccessedIrmaNewItem.IrmaItemKey && q.Identifier == successfullyProccessedIrmaNewItem.Identifier));
            }
        }
    }
}
