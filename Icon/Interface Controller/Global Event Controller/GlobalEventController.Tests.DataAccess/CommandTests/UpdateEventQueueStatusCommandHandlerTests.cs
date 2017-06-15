using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Infrastructure;
using Icon.Framework;
using GlobalEventController.DataAccess.Commands;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Icon.Testing.Builders;
using Icon.Common;


namespace GlobalEventController.Tests.DataAccess
{
    [TestClass]
    public class UpdateEventQueueStatusCommandHandlerTests
    {
        private IconContext context;
        private EventQueue firstEvent;
        private EventQueue secondEvent;
        private UpdateEventQueueStatusCommandHandler handler;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.handler = new UpdateEventQueueStatusCommandHandler(this.context);

            this.firstEvent = new TestEventQueueBuilder();
            this.secondEvent = new TestEventQueueBuilder().WithRegionCode("SW");

            this.context.EventQueue.Add(firstEvent);
            this.context.EventQueue.Add(secondEvent);
            this.context.SaveChanges();
        }
        
        [TestCleanup]
        public void CleanupData()
        {
            this.context.Database.ExecuteSqlCommand("DELETE FROM app.EventQueue WHERE QueueId = @p0 OR QueueId = @p1",
                this.firstEvent.QueueId, this.secondEvent.QueueId);
        }

        [TestMethod]
        [ExpectedException(typeof(ConcurrencyException))]
        public void UpdateEventQueueStatusCommand_InProcessByChangedBeforeUpdate_DbUpdateConcurrencyExcpetionThrown()
        {
            // Given
            List<EventQueue> queuedEvents = this.context.EventQueue.Where(q => q.QueueId == this.firstEvent.QueueId || q.QueueId == this.secondEvent.QueueId).ToList();
            List<int> eventQueueIds = queuedEvents.Select(q => q.QueueId).ToList();

            // update row to create concurrency effect
            this.context.Database.ExecuteSqlCommand("UPDATE app.EventQueue SET InProcessBy = 'test' WHERE QueueId = @p0", this.firstEvent.QueueId);

            // When
            UpdateEventQueueStatusCommand command = new UpdateEventQueueStatusCommand { EventQueueIdList = eventQueueIds, Instance = 1 };
            handler.Handle(command);

            // Then
            // Expected Exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateEventQueueStatusCommand_EmptyEventIdList_LoggerPostsWarning()
        {
            // Given
            List<int> eventQueueIds = new List<int>();

            // When
            UpdateEventQueueStatusCommand command = new UpdateEventQueueStatusCommand { EventQueueIdList = eventQueueIds, Instance = 1 };
            handler.Handle(command);

            // Then
            // Expected Argument Exception
        }

        [TestMethod]
        public void UpdateEventQueueStatusCommand_ValidParameters_UpdatesInProcessBy()
        {
            // Given
            List<int> queuedEventIds = this.context.EventQueue.Where(q => q.EventMessage == this.firstEvent.EventMessage).Select(q => q.QueueId).ToList();
            int instance = 4;

            // When
            UpdateEventQueueStatusCommand command = new UpdateEventQueueStatusCommand { EventQueueIdList = queuedEventIds, Instance = instance };
            handler.Handle(command);

            // Then
            DbEntityEntry firstEntry = this.context.Entry(this.firstEvent);
            DbEntityEntry secondEntry = this.context.Entry(this.secondEvent);

            Assert.AreEqual(instance.ToString(), this.firstEvent.InProcessBy);
            Assert.IsTrue(firstEntry.State == EntityState.Unchanged);

            Assert.AreEqual(instance.ToString(), this.secondEvent.InProcessBy);
            Assert.IsTrue(secondEntry.State == EntityState.Unchanged);
        }
    }
}
