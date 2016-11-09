using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Testing.Common
{
    class TestEventQueueCustomBuilder
    {
        private int queueId;
        private int eventId;
        private string eventMessage;
        private int eventReferenceId;
        private string regionCode;
        private DateTime insertDate;
        private DateTime? processFailedDate;
        private string inProcessBy;
        private string eventTypeName;

        public TestEventQueueCustomBuilder()
        {
            this.queueId = 0;
            this.eventId = EventTypes.ItemValidation;
            this.eventMessage = "123412341234";
            this.eventReferenceId = 1;
            this.regionCode = "FL";
            this.insertDate = DateTime.Now;
            this.processFailedDate = null;
            this.inProcessBy = null;
            this.eventTypeName = "Item Validation";
        }

        public TestEventQueueCustomBuilder WithQueueId(int queueId)
        {
            this.queueId = queueId;
            return this;
        }

        public TestEventQueueCustomBuilder WithEventId(int eventId)
        {
            this.eventId = eventId;
            return this;
        }

        public TestEventQueueCustomBuilder WithEventMessage(string eventMessage)
        {
            this.eventMessage = eventMessage;
            return this;
        }

        public TestEventQueueCustomBuilder WithEventReferenceId(int eventReferenceId)
        {
            this.eventReferenceId = eventReferenceId;
            return this;
        }

        public TestEventQueueCustomBuilder WithRegionCode(string regionCode)
        {
            this.regionCode = regionCode;
            return this;
        }

        public TestEventQueueCustomBuilder WithInsertDate(DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        public TestEventQueueCustomBuilder WithProcessFailedDate(DateTime? processFailedDate)
        {
            this.processFailedDate = processFailedDate;
            return this;
        }

        public TestEventQueueCustomBuilder WithInProcessBy(string inProcessBy)
        {
            this.inProcessBy = inProcessBy;
            return this;
        }

        public TestEventQueueCustomBuilder WithEventTypeName(string eventTypeName)
        {
            this.eventTypeName = eventTypeName;
            return this;
        }

        public EventQueueCustom Build()
        {
            EventQueueCustom eventQueue = new EventQueueCustom();
            eventQueue.QueueId = this.queueId;
            eventQueue.EventId = this.eventId;
            eventQueue.EventMessage = this.eventMessage;
            eventQueue.EventReferenceId = this.eventReferenceId;
            eventQueue.RegionCode = this.regionCode;
            eventQueue.InsertDate = this.insertDate;
            eventQueue.ProcessFailedDate = this.processFailedDate;
            eventQueue.InProcessBy = this.inProcessBy;
            eventQueue.EventName = this.eventTypeName;

            return eventQueue;
        }

        public static implicit operator EventQueueCustom(TestEventQueueCustomBuilder builder)
        {
            return builder.Build();
        }
    }
}
