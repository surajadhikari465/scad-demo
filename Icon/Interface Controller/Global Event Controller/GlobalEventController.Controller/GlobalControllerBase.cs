using GlobalEventController.Common;
using GlobalEventController.Controller.EventOperations;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Logging;
using System;

namespace GlobalEventController.Controller
{
    public class GlobalControllerBase
    {
        private IEventQueues eventQueues;
        private IEventCollector collector;
        private IEventBulkProcessor bulkItemProcessor;
        private IEventProcessor processor;
        private IEventFinalizer finalizer;
        private IEventBulkProcessor bulkNutriFactsProcessor;
        private IContextManager contextManager;
        private IDataIssueMessageCollector dataIssueMessageCollector;

        public GlobalControllerBase(IEventQueues eventQueues,
            IEventCollector collector,
            IEventBulkProcessor bulkItemProcessor,
            IEventBulkProcessor bulkNutriFactsProcessor,
            IEventProcessor processor,
            IEventFinalizer finalizer,
            IContextManager contextManager,
            IDataIssueMessageCollector dataIssueMessageCollector)
        {
            this.eventQueues = eventQueues;
            this.collector = collector;
            this.bulkItemProcessor = bulkItemProcessor;
            this.bulkNutriFactsProcessor = bulkNutriFactsProcessor;
            this.processor = processor;
            this.finalizer = finalizer;
            this.contextManager = contextManager;
            this.dataIssueMessageCollector = dataIssueMessageCollector;
        }

        public void Start()
        {
            EventRegistrationService.RegisterEvents();

            // Continue to run until there are no events left to process.
            this.collector.GetEvents();
            while (this.eventQueues.QueuedEvents.Count > 0)
            {
                this.bulkItemProcessor.BulkProcessEvents();
                this.processor.ProcessBrandNameUpdateEvents();
                this.processor.ProcessBrandDeleteEvents();
                this.processor.ProcessTaxEvents();
                this.bulkNutriFactsProcessor.BulkProcessEvents();
                this.finalizer.HandleFailedEvents();
                this.finalizer.DeleteEvents();

                this.eventQueues.QueuedEvents.Clear();
                this.eventQueues.ProcessedEvents.Clear();
                this.eventQueues.FailedEvents.Clear();
                this.eventQueues.RegionToEventQueueDictionary.Clear();
                
                contextManager.RefreshContexts();
                this.collector.GetEvents();
            }

            //send out new email
            this.dataIssueMessageCollector.SendDataIssueMessage();
        }
    }
}
