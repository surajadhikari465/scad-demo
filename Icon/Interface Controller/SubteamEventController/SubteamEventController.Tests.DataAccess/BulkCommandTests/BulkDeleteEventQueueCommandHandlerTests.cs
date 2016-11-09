using Icon.Framework;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using GlobalEventController.DataAccess.BulkCommands;

namespace SubteamEventController.Tests.DataAccess.BulkCommandTests
{
    [TestClass]
    public class BulkDeleteEventQueueCommandHandlerTests
    {
        private BulkDeleteEventQueueCommand command;
        private BulkDeleteEventQueueCommandHandler handler;
        private IconContext context;
        private List<EventQueue> testEventQueueList;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.command = new BulkDeleteEventQueueCommand();
            this.handler = new BulkDeleteEventQueueCommandHandler(this.context);
            this.testEventQueueList = new List<EventQueue>();

            testEventQueueList.Add(new TestEventQueueBuilder().WithRegionCode("SW").Build());
            testEventQueueList.Add(new TestEventQueueBuilder().WithRegionCode("FL").Build());
            testEventQueueList.Add(new TestEventQueueBuilder().WithRegionCode("MW").Build());
            testEventQueueList.Add(new TestEventQueueBuilder().WithRegionCode("RM").Build());

            this.context.EventQueue.AddRange(testEventQueueList);
            this.context.SaveChanges();
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.context.Dispose();
            this.context = new IconContext();

            foreach (var eq in this.testEventQueueList)
            {
                if (this.context.EventQueue.Any(q => q.QueueId == eq.QueueId))
                {
                    this.context.EventQueue.Remove(eq);
                }
            }
            this.context.SaveChanges();

            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        [TestMethod]
        public void BulkDeleteEventQueue_ListOfEventQueues_RowsAreDeletedFromTable()
        {
            // Given
            this.command.EventsToDelete = this.testEventQueueList;

            // When
            this.handler.Handle(this.command);

            // Then
            foreach (EventQueue eventQueue in this.testEventQueueList)
            {
                Assert.IsFalse(this.context.EventQueue.Any(q => q.QueueId == eventQueue.QueueId));
            }
        }
    }
}
