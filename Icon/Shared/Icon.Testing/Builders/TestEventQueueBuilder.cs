using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Testing.Builders
{
    public class TestEventQueueBuilder
    {
        private int queueId;
        private int eventId;
        private string eventMessage;
        private int eventReferenceId;
        private string regionCode;
        private DateTime insertDate;
        private DateTime? processFailedDate;
        private string inProcessBy;

        public TestEventQueueBuilder()
        {
            this.queueId = 0;
            this.eventId = EventTypes.ItemValidation;
            this.eventMessage = "123412341234";
            this.eventReferenceId = 1;
            this.regionCode = "FL";
            this.insertDate = DateTime.Now;
            this.processFailedDate = null;
            this.inProcessBy = null;
        }

        public TestEventQueueBuilder WithQueueId(int queueId)
        {
            this.queueId = queueId;
            return this;
        }

        public TestEventQueueBuilder WithEventId(int eventId)
        {
            this.eventId = eventId;
            return this;
        }

        public TestEventQueueBuilder WithEventMessage(string eventMessage)
        {
            this.eventMessage = eventMessage;
            return this;
        }

        public TestEventQueueBuilder WithEventReferenceId(int eventReferenceId)
        {
            this.eventReferenceId = eventReferenceId;
            return this;
        }

        public TestEventQueueBuilder WithRegionCode(string regionCode)
        {
            this.regionCode = regionCode;
            return this;
        }

        public TestEventQueueBuilder WithInsertDate(DateTime insertDate)
        {
            this.insertDate = insertDate;
            return this;
        }

        public TestEventQueueBuilder WithProcessFailedDate(DateTime? processFailedDate)
        {
            this.processFailedDate = processFailedDate;
            return this;
        }

        public TestEventQueueBuilder WithInProcessBy(string inProcessBy)
        {
            this.inProcessBy = inProcessBy;
            return this;
        }

        public EventQueue Build()
        {
            EventQueue eventQueue = new EventQueue();
            eventQueue.QueueId = this.queueId;
            eventQueue.EventId = this.eventId;
            eventQueue.EventMessage = this.eventMessage;
            eventQueue.EventReferenceId = this.eventReferenceId;
            eventQueue.RegionCode = this.regionCode;
            eventQueue.InsertDate = this.insertDate;
            eventQueue.ProcessFailedDate = this.processFailedDate;
            eventQueue.InProcessBy = this.inProcessBy;

            return eventQueue;
        }

        public static implicit operator EventQueue(TestEventQueueBuilder builder)
        {
            return builder.Build();
        }
    }
}
