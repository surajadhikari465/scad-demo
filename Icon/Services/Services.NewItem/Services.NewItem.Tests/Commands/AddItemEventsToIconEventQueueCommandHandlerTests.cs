using Icon.Common.Context;
using Icon.Framework;
using Services.NewItem.Commands;
using Services.NewItem.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Services.NewItem.Tests.Commands
{
    [TestClass]
    public class AddItemEventsToIconEventQueueCommandHandlerTests
    {
        private AddItemEventsToIconEventQueueCommandHandler commandHandler;
        private AddItemEventsToIconEventQueueCommand command;
        private GlobalContext<IconContext> context;
        private TransactionScope transaction;
        private List<NewItemModel> newItems;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            context = new GlobalContext<IconContext>();
            context.Refresh();
            commandHandler = new AddItemEventsToIconEventQueueCommandHandler(context);
            command = new AddItemEventsToIconEventQueueCommand();
            newItems = new List<NewItemModel>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void AddItemEventsToIconEventQueue_1Item_1EventIsAddedToTheQueue()
        {
            //Given
            newItems.Add(new NewItemModel
            {
                IconItemId = 123456789,
                ScanCode = "123456789"
            });
            command.NewItems = newItems;

            //When
            commandHandler.Execute(command);

            //Then
            AssertNewItemsAreEqualToEventQueue();
        }

        [TestMethod]
        public void AddItemEventsToIconEventQueue_10Items_10EventsAreAddedToTheQueue()
        {
            //Given
            for (int i = 0; i < 10; i++)
            {
                newItems.Add(new NewItemModel
                {
                    IconItemId = 123456789 + i,
                    ScanCode = "123456789" + i.ToString()
                });
            }
            command.NewItems = newItems;

            //When
            commandHandler.Execute(command);

            //Then
            AssertNewItemsAreEqualToEventQueue();
        }

        private void AssertNewItemsAreEqualToEventQueue()
        {
            foreach (var newItem in newItems)
            {
                var itemEvent = context.Context.EventQueue.SingleOrDefault(e => e.EventReferenceId == newItem.IconItemId.Value);
                Assert.AreEqual(newItem.ScanCode, itemEvent.EventMessage);
                Assert.AreEqual(EventTypes.ItemUpdate, itemEvent.EventId);
                Assert.AreEqual(newItem.Region, itemEvent.RegionCode);
                Assert.IsNotNull(itemEvent.InsertDate);
                Assert.IsNull(itemEvent.ProcessFailedDate);
                Assert.IsNull(itemEvent.InProcessBy);
            }
        }
    }
}
