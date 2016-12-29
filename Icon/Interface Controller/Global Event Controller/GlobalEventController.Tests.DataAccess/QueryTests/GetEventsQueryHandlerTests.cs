using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetEventsQueryHandlerTests
    {
        private IconContext context;
        private EventQueue firstEvent;
        private EventQueue secondEvent;
        private GetEventsQueryHandler handler;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.handler = new GetEventsQueryHandler(this.context);

            this.firstEvent = new TestEventQueueBuilder().WithEventId(EventTypes.ItemValidation);
            this.secondEvent = new TestEventQueueBuilder().WithEventId(EventTypes.ItemUpdate).WithRegionCode("SW");

            this.context.EventQueue.Add(firstEvent);
            this.context.EventQueue.Add(secondEvent);
            this.context.SaveChanges();
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.context.EventQueue.Remove(this.context.EventQueue.Find(this.firstEvent.QueueId));
            this.context.EventQueue.Remove(this.context.EventQueue.Find(this.secondEvent.QueueId));
            this.context.SaveChanges();
            this.context.Dispose();
        }

        //[TestMethod]
        public void GetEventsQueryHandler_ValidmMaxEventsAndRegisteredEvents_ReturnsExpectedRows()
        {
            // Given
            List<string> registeredEvents = new List<string>();
            registeredEvents.Add("IconToIrmaItemUpdates");
            registeredEvents.Add("IconToIrmaItemValidation");
            
            List<EventQueue> expected = this.context.EventQueue.Where(q => q.EventId == EventTypes.ItemValidation || q.EventId == EventTypes.ItemUpdate).ToList();

            // When
            GetEventsQuery query = new GetEventsQuery { MaxNumberOfEvents = 10, RegisteredEvents = registeredEvents };
            List<EventQueue> actual = handler.Handle(query);

            // Then
            Assert.AreEqual(expected.Count, actual.Count);
        }
    }
}
