using GlobalEventController.DataAccess.BulkCommands;
using Icon.Framework;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.BulkCommandTests
{
    [TestClass]
    public class BulkDeleteEventQueueCommandHandlerTests
    {
        private BulkDeleteEventQueueCommandHandler handler;
        private BulkDeleteEventQueueCommand command;
        private IconDbContextFactory contextFactory;
        private IconContext context;
        private List<EventQueue> testEventQueueList;
        private TransactionScope transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.context = new IconContext();
            this.command = new BulkDeleteEventQueueCommand();
            this.contextFactory = new IconDbContextFactory();
            this.handler = new BulkDeleteEventQueueCommandHandler(contextFactory);
            this.testEventQueueList = new List<EventQueue>();

            this.testEventQueueList.Add(new TestEventQueueBuilder().WithRegionCode("SW").Build());
            this.testEventQueueList.Add(new TestEventQueueBuilder().WithRegionCode("FL").Build());
            this.testEventQueueList.Add(new TestEventQueueBuilder().WithRegionCode("MW").Build());
            this.testEventQueueList.Add(new TestEventQueueBuilder().WithRegionCode("RM").Build());

            this.context.EventQueue.AddRange(testEventQueueList);
            this.context.SaveChanges();
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Dispose();
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
