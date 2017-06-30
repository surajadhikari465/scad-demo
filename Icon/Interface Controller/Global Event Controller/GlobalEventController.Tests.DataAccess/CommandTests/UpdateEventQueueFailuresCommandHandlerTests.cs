using GlobalEventController.DataAccess.Commands;
using Icon.Framework;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class UpdateEventQueueFailuresCommandHandlerTests
    {
        private UpdateEventQueueFailuresCommandHandler handler;
        private UpdateEventQueueFailuresCommand command;
        private IconContext context;
        private IconDbContextFactory contextFactory;
        private TransactionScope transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.contextFactory = new IconDbContextFactory();
            this.context = new IconContext();
            this.command = new UpdateEventQueueFailuresCommand();
            this.handler = new UpdateEventQueueFailuresCommandHandler(contextFactory);
        }

        [TestCleanup]
        public void CleanupData()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void UpdateEventQueueFailures_ExistingEventQueues_ProcessedFailedDateIsUpdated()
        {
            // Given
            List<EventQueue> eventsToUpdate = GetEventQueueList(3);
            this.command.FailedEvents = eventsToUpdate;
            DateTime preTestDate = DateTime.Now;

            // When
            this.handler.Handle(this.command);

            // Then
            foreach (var eventQueue in eventsToUpdate)
            {
                EventQueue actual = this.context.EventQueue.AsNoTracking().First(q => q.QueueId == eventQueue.QueueId);
                var entry = this.context.Entry(actual);

                Assert.IsNotNull(actual.ProcessFailedDate, "The ProcessFailedDate is null.");
                Assert.IsTrue(actual.ProcessFailedDate >= preTestDate, "The ProcessFaileDate is not greater than or equal to the date just before test was run.");
            }
        }

        [TestMethod]
        public void UpdateEventQueueFailures_ExistingEventQueues_InProcessByIsNull()
        {
            // Given
            List<EventQueue> eventsToUpdate = GetEventQueueList(3);
            this.command.FailedEvents = eventsToUpdate;

            // When
            this.handler.Handle(this.command);

            // Then
            foreach (var eventQueue in eventsToUpdate)
            {
                EventQueue actual = this.context.EventQueue.First(q => q.QueueId == eventQueue.QueueId);
                var entry = this.context.Entry(actual);

                Assert.IsNull(actual.InProcessBy);
                Assert.IsTrue(entry.State == EntityState.Unchanged);
            }
        }

        private List<EventQueue> GetEventQueueList(int maxRows)
        {
            List<EventQueue> queuedEvents = new List<EventQueue>();
            queuedEvents = this.context.EventQueue.Take(maxRows).ToList();

            if (queuedEvents.Count == 0)
            {
                List<EventQueue> eventsToAdd = new List<EventQueue>();
                for (int i = 0; i < maxRows; i++)
                {
                    EventQueue eventQueue = new TestEventQueueBuilder().WithInProcessBy("test").WithRegionCode("MA").WithEventMessage("1234" + i.ToString()).Build();
                    eventsToAdd.Add(eventQueue);
                }

                this.context.EventQueue.AddRange(eventsToAdd);
                this.context.SaveChanges();

                queuedEvents = eventsToAdd;
            }

            return queuedEvents;
        }
    }
}
