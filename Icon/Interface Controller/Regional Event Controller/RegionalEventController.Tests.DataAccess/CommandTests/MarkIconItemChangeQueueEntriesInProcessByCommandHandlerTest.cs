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
    public class MarkIconItemChangeQueueEntriesInProcessByCommandHandlerTest
    {
        private Mock<ILogger<MarkIconItemChangeQueueEntriesInProcessByCommandHandler>> mockLogger;
        private IrmaContext context;
        private MarkIconItemChangeQueueEntriesInProcessByCommand command;
        private MarkIconItemChangeQueueEntriesInProcessByCommandHandler handler;
        private List<IconItemChangeQueue> itemChangeQueueEntries;
        
        [TestInitialize]
        public void InitializeData()
        {
            this.mockLogger = new Mock<ILogger<MarkIconItemChangeQueueEntriesInProcessByCommandHandler>>();
            this.context = new IrmaContext(); // this is the FL ItemCatalog_Test database
            this.command = new MarkIconItemChangeQueueEntriesInProcessByCommand();
            this.handler = new MarkIconItemChangeQueueEntriesInProcessByCommandHandler(this.mockLogger.Object, this.context);
            this.itemChangeQueueEntries = new List<IconItemChangeQueue>();

            string sql = @"delete dbo.IconItemChangeQueue";
            int returnCode = context.Database.ExecuteSqlCommand(sql);
           
            StartupOptions.Instance = 123;
        }

        private void BuildIconItemChangeQueueEntries(int numberOfEntries)
        {
            var random = new Random();

            List<int> itemKeys = new List<int>();
            itemKeys = (from i in this.context.Item
                        select i.Item_Key).Take(numberOfEntries).ToList();

            while (numberOfEntries > 0)
            {
                itemChangeQueueEntries.Add(new TestIconItemChangeQueueBuilder().WithIdentifier(random.Next(1000000, 1000000000).ToString()).WithItemKey(itemKeys.First()).WithInProcessBy(StartupOptions.Instance.ToString()));
                numberOfEntries--;
            }

            this.context.IconItemChangeQueue.AddRange(itemChangeQueueEntries);
            this.context.SaveChanges();

            foreach (IconItemChangeQueue itemChangeQueueEntry in itemChangeQueueEntries)
            {
                itemChangeQueueEntry.QID = context.IconItemChangeQueue
                    .Where(q => q.Item_Key == itemChangeQueueEntry.Item_Key && q.Identifier == itemChangeQueueEntry.Identifier)
                    .Select(q => q.QID).FirstOrDefault();
            }
        }

        [TestCleanup]
        public void CleanupData()
        {
            foreach (IconItemChangeQueue itemChangeQueueEntry in itemChangeQueueEntries)
            {
                this.context.Entry(itemChangeQueueEntry).Reload();
                IconItemChangeQueue queue = context.IconItemChangeQueue.Where(q => q.QID == itemChangeQueueEntry.QID).FirstOrDefault();
                if (queue != null)
                    this.context.IconItemChangeQueue.Remove(queue);
            }

            this.context.SaveChanges();

            if (this.context != null)
            {
                this.context.Dispose();
            }
        }
        [TestMethod]
        public void MarkIconItemChangeQueueEntriesInProcess_MoreEntriesThanMaxInTheQueue_UpToMaxNumberOfQueueEntriesAreUpdatedAsInProcess()
        {
            // Given
            BuildIconItemChangeQueueEntries(5);
            command.Instance = StartupOptions.Instance.ToString();
            command.MaxQueueEntriesToProcess = 3;

            // When
            this.handler.Execute(command);

            // Then
            int updatedCtr = context.IconItemChangeQueue.Where(q => q.InProcessBy == StartupOptions.Instance.ToString()).Count();
            Assert.IsTrue(command.MaxQueueEntriesToProcess == updatedCtr);
        }

        [TestMethod]
        public void MarkIconItemChangeQueueEntriesInProcess_LessEntriesThanMaxInTheQueue_AllQueueEntriesAreUpdatedAsInProcess()
        {
            // Given
            int maxNumberOfEntriesToBeMarked = 2;
            BuildIconItemChangeQueueEntries(maxNumberOfEntriesToBeMarked);
            command.Instance = StartupOptions.Instance.ToString();
            command.MaxQueueEntriesToProcess = 3;

            // When
            this.handler.Execute(command);

            // Then
            int updatedCtr = context.IconItemChangeQueue.Where(q => q.InProcessBy == StartupOptions.Instance.ToString()).Count();
            Assert.IsTrue(maxNumberOfEntriesToBeMarked == updatedCtr);
        }
    }
}
