using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Models;
using Moq;
using Icon.Logging;
using Icon.Testing.Builders;
using RegionalEventController.Common;

namespace RegionalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class InsertEventQueuesToIconBulkCommandHandlerTest
    {
        private Mock<ILogger<InsertEventQueuesToIconBulkCommandHandler>> mockLogger;
        private IconContext context;
        private InsertEventQueuesToIconBulkCommand command;
        private InsertEventQueuesToIconBulkCommandHandler handler;
        private List<EventQueue> eventQueueEntries;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.mockLogger = new Mock<ILogger<InsertEventQueuesToIconBulkCommandHandler>>();
            this.context = new IconContext(); // this is the FL ItemCatalog_Test database
            this.command = new InsertEventQueuesToIconBulkCommand();
            this.handler = new InsertEventQueuesToIconBulkCommandHandler(this.mockLogger.Object, this.context);
            this.eventQueueEntries = new List<EventQueue>();

            StartupOptions.Instance = 123;
            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (transaction != null)
            {
                this.transaction.Rollback();
            }
        }

        private void BuildEventQueueEntries()
        {
            var random = new Random();
            eventQueueEntries.Add(new TestEventQueueBuilder()
                .WithEventMessage(random.Next(1000000, 1000000000).ToString())
                .WithEventReferenceId(random.Next(10, 10000)).WithEventId(1).Build());
            eventQueueEntries.Add(new TestEventQueueBuilder()
                .WithEventMessage(random.Next(1000000, 1000000000).ToString())
                .WithEventReferenceId(random.Next(10, 10000)).WithEventId(1).Build());
            eventQueueEntries.Add(new TestEventQueueBuilder()
                .WithEventMessage(random.Next(1000000, 1000000000).ToString())
                .WithEventReferenceId(random.Next(10, 10000)).WithEventId(1).Build());
        }
        [TestMethod]
        public void InsertEventQueuesToIcon_EventQueueEntriesAreReady_AllEventQueueEntriesAreInserted()
        {
            // Given
            BuildEventQueueEntries();
            command.EventQueueEntries = eventQueueEntries;

            // When
            this.handler.Execute(this.command);

            // Then
            foreach(EventQueue eventQueueEntry in eventQueueEntries)
            {
                Assert.IsTrue(context.EventQueue.Any(q => q.EventId == eventQueueEntry.EventId && q.EventMessage == eventQueueEntry.EventMessage && q.EventReferenceId == eventQueueEntry.EventReferenceId));
            }
        }
    }
}
