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
    public class InsertEventQueueToIconCommandHandlerTest
    {
        private Mock<ILogger<InsertEventQueueToIconCommandHandler>> mockLogger;
        private IconContext context;
        private InsertEventQueueToIconCommand command;
        private InsertEventQueueToIconCommandHandler handler;
        private EventQueue eventQueueEntry;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.mockLogger = new Mock<ILogger<InsertEventQueueToIconCommandHandler>>();
            this.context = new IconContext(); 
            this.command = new InsertEventQueueToIconCommand();
            this.handler = new InsertEventQueueToIconCommandHandler(this.mockLogger.Object, this.context);
            this.eventQueueEntry = new EventQueue();

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
        private void BuildEventQueueEntry()
        {
            var random = new Random();
            eventQueueEntry = new TestEventQueueBuilder()
                .WithEventMessage(random.Next(1000000, 1000000000).ToString())
                .WithEventReferenceId(random.Next(10, 10000)).WithEventId(1).Build();

        }
        [TestMethod]
        public void InsertEventQueueToIcon_EventQueueEntryIsReady_EventQueueEntryIsInserted()
        {
            // Given
            BuildEventQueueEntry();
            command.eventQueueEntry = eventQueueEntry;

            // When
            this.handler.Execute(this.command);

            // Then
            Assert.IsTrue(context.EventQueue.Any(q => q.EventId == eventQueueEntry.EventId && q.EventMessage == eventQueueEntry.EventMessage && q.EventReferenceId == eventQueueEntry.EventReferenceId));
        }
    }
}
