using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using GlobalEventController.DataAccess.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Icon.Testing.Builders;
using GlobalEventController.DataAccess.Infrastructure;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class UpdateEventQueueFailuresCommandHandlerTests
    {
        private IconContext context;
        private UpdateEventQueueFailuresCommand command;
        private UpdateEventQueueFailuresCommandHandler handler;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.command = new UpdateEventQueueFailuresCommand();
            this.handler = new UpdateEventQueueFailuresCommandHandler(new ContextManager { IconContext = this.context });

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (this.transaction != null)
            {
                transaction.Rollback();
            }

            this.context.Dispose();
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
                EventQueue actual = this.context.EventQueue.First(q => q.QueueId == eventQueue.QueueId);
                var entry = this.context.Entry(actual);

                Assert.IsNotNull(actual.ProcessFailedDate, "The ProcessFailedDate is null.");
                Assert.IsTrue(actual.ProcessFailedDate >= preTestDate, "The ProcessFaileDate is not greater than or equal to the date just before test was run.");
                Assert.IsTrue(entry.State == EntityState.Unchanged);
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
