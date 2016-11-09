using GlobalEventController.Common;
using GlobalEventController.Controller.EventOperations;
using SubteamEventController.Controller.EventOperations;
using Icon.Logging;
using System;

namespace SubteamEventController.Controller
{
    public class SubteamEventControllerBase
    {
        private IEventQueues eventBase;
        private IEventCollector collector;
        private IItemSubTeamEventBulkProcessor bulkProcessor;
        private IEventItemSubTeamProcessor itemProcessor;
        private ISubTeamEventProcessor subTeamEventprocessor;
        private IEventFinalizer finalizer;

        public SubteamEventControllerBase(IEventQueues eventBase,
            IEventCollector collector,
            IItemSubTeamEventBulkProcessor bulkProcessor,
            IEventItemSubTeamProcessor processor,
            ISubTeamEventProcessor subTeamEventprocessor,
            IEventFinalizer finalizer)
        {
            this.eventBase = eventBase;
            this.collector = collector;
            this.bulkProcessor = bulkProcessor;
            this.itemProcessor = processor;
            this.subTeamEventprocessor =  subTeamEventprocessor;
            this.finalizer = finalizer;
        }

        public void Start()
        {
            EventRegistrationService.RegisterEvents();

            // Continue to run until there are no events left to process.
            this.collector.GetEvents();
            while (this.eventBase.QueuedEvents.Count > 0)
            {
                this.bulkProcessor.BulkProcessItemSubTeamEvents();
                this.itemProcessor.ProcessItemSubTeamEvents();
                this.subTeamEventprocessor.ProcessSubTeamUpdateEvents();
                this.finalizer.HandleFailedEvents();
                this.finalizer.DeleteEvents();

                this.eventBase.QueuedEvents.Clear();
                this.eventBase.ProcessedEvents.Clear();
                this.eventBase.FailedEvents.Clear();
                this.eventBase.RegionToEventQueueDictionary.Clear();

                this.collector.GetEvents();

            }
        }
    }
}
