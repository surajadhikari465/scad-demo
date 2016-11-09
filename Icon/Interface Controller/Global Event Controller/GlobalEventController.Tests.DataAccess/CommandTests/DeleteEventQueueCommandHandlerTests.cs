using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using GlobalEventController.DataAccess.Commands;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Icon.Testing.Builders;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class DeleteEventQueueCommandHandlerTests
    {
        private IconContext context;
        private DeleteEventQueueCommand command;
        private DeleteEventQueueCommandHandler handler;
        private List<EventQueue> testEventQueue;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.command = new DeleteEventQueueCommand();
            this.handler = new DeleteEventQueueCommandHandler(this.context);
            this.testEventQueue = new List<EventQueue>();
        }

        [TestCleanup]
        public void CleanupData()
        {
            // Cleanup just in case
            var cleanup1 = this.context.EventQueue.Find(this.testEventQueue[0].QueueId);
            var cleanup2 = this.context.EventQueue.Find(this.testEventQueue[1].QueueId);
            var cleanup3 = this.context.EventQueue.Find(this.testEventQueue[2].QueueId);

            if (cleanup1 != null || cleanup2 != null || cleanup3 != null)
            {
                this.context.EventQueue.RemoveRange(this.testEventQueue);
                this.context.SaveChanges();    
            }
            

            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        [TestMethod]
        public void DeleteEventQueue_EventQueueRows_RowsDeleted()
        {
            // Given
            this.testEventQueue = new List<EventQueue>();
            this.testEventQueue.Add(new TestEventQueueBuilder().WithEventId(EventTypes.BrandNameUpdate).WithInProcessBy("test").Build());
            this.testEventQueue.Add(new TestEventQueueBuilder().WithEventId(EventTypes.NewIrmaItem).WithInProcessBy("test").Build());
            this.testEventQueue.Add(new TestEventQueueBuilder().WithEventId(EventTypes.TaxNameUpdate).WithInProcessBy("test").Build());
            this.context.EventQueue.AddRange(this.testEventQueue);
            this.context.SaveChanges();

            this.command.ProcessedEvents = this.testEventQueue;

            // When
            this.handler.Handle(this.command);

            // Then
            IEnumerable<EventQueue> actual = this.context.Set<EventQueue>().SqlQuery("SELECT * FROM app.EventQueue WHERE QueueId IN ({0}, {1}, {2})",
                testEventQueue[0].QueueId, testEventQueue[1].QueueId, testEventQueue[2].QueueId);

            Assert.IsTrue(actual.Count() == 0);
        }
    }
}
