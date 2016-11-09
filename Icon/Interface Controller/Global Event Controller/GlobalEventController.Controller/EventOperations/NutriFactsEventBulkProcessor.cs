using GlobalEventController.Common;
using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.Controller.EventOperations
{
    public class NutriFactsEventBulkProcessor : IEventBulkProcessor
    {
        private IEventQueues queues;
        private ILogger<ItemEventBulkProcessor> logger;
        private IEventServiceProvider eventServiceProvider;
        private IQueryHandler<BulkGetValidatedItemsQuery, List<ValidatedItemModel>> bulkGetValidatedItems;
        private ExceptionHandler<ItemEventBulkProcessor> exceptionHandler;
        private IQueryHandler<GetIconItemNutritionQuery, List<ItemNutrition>> getIconItemNutritionQueryHandler;

        public NutriFactsEventBulkProcessor(IEventQueues queues,
            ILogger<ItemEventBulkProcessor> logger,
            IEventServiceProvider eventServiceProvider,
            IQueryHandler<BulkGetValidatedItemsQuery, List<ValidatedItemModel>> bulkGetValidatedItems,
            IQueryHandler<GetIconItemNutritionQuery, List<ItemNutrition>> getIconItemNutritionQueryHandler)
        {
            this.exceptionHandler = new ExceptionHandler<ItemEventBulkProcessor>(logger);
            this.queues = queues;
            this.logger = logger;
            this.eventServiceProvider = eventServiceProvider;
            this.bulkGetValidatedItems = bulkGetValidatedItems;
            this.getIconItemNutritionQueryHandler = getIconItemNutritionQueryHandler;
        }

        public void BulkProcessEvents()
        {
            // Filter Queued Events to Item Event Types
            IEnumerable<EventQueue> nutritionQueuedEvents = FilterEventQueueByEventTypes(new List<int> { EventTypes.NutritionAdd, EventTypes.NutritionUpdate });

            if (!nutritionQueuedEvents.Any())
            {
                logger.Info("There are no item events to process.");
                return;
            }

            // Populate Dictionary with Region as the Key and List of EventQueues as the value
            this.queues.RegionToEventQueueDictionary = nutritionQueuedEvents
                .GroupBy(e => e.RegionCode)
                .ToDictionary(group => group.Key, g => g.ToList());

            // Iterate Dictionary and perform Bulk Updates for All Regions
            foreach (KeyValuePair<string, List<EventQueue>> entry in this.queues.RegionToEventQueueDictionary)
            {
                ProcessEvents(entry.Key, entry.Value);
            }

        }

        private void ProcessEvents(string region, IEnumerable<EventQueue> eventsToProcess)
        {
            try
            {
                ExecuteBulkEventService(region, eventsToProcess.ToList(), false);
            }
            catch (Exception ex)
            {
                if ((eventsToProcess.Count()) > 1)
                {
                    foreach (var batch in eventsToProcess.Batch((eventsToProcess.Count() / 2)))
                    {
                        ProcessEvents(region, batch);
                    }
                }
                else
                {
                    logger.Error(String.Format("Class {0} Failed to Update ScanCode {1} for {2} region.  Exception: {3}.  InnerException: {4}",
                        this.GetType().Name, eventsToProcess.First().EventMessage, region, ex, ex.InnerException));
                    this.queues.FailedEvents.AddRange(eventsToProcess.Select(qe => new FailedEvent
                    {
                        Event = qe,
                        FailureReason = ex.ToString()
                    }));
                    eventServiceProvider.RefreshContexts();
                }
            }
        }

        private void ExecuteBulkEventService(string regionCode, List<EventQueue> queuedEvents, bool markFailedEvents = true)
        {
            // Instantiate BulkEventService
            IBulkEventService bulkNutritionEventService = eventServiceProvider.GetBulkItemNutriFactsEventService(regionCode);

            bulkNutritionEventService.ValidatedItemList = bulkGetValidatedItems.Handle(new BulkGetValidatedItemsQuery { Events = queuedEvents });
            bulkNutritionEventService.Region = regionCode;

            if (StartupOptions.NutritionEnabledRegions.Contains(regionCode))
            {
                var iconNutriFacts = getIconItemNutritionQueryHandler.Handle(new GetIconItemNutritionQuery { ScanCodes = queuedEvents.Select(e => e.EventMessage).ToList() });
                bulkNutritionEventService.ItemNutriFacts = iconNutriFacts.Select(nf => new NutriFactsModel(nf)).ToList();
            }

            logger.Info(String.Format("Begin Updating {0} rows in IRMA for {1} region.", queuedEvents.Count.ToString(), regionCode));

            // Run Service
            bulkNutritionEventService.Run();

            logger.Info(String.Format("Successfully processed {0} events for {1} region.", queuedEvents.Count.ToString(), regionCode));

            // Add to ProcessedEvents List
            this.queues.ProcessedEvents.AddRange(queuedEvents);

            // Remove from QueuedEvents List so that they do not get processed again with single item process
            this.queues.QueuedEvents.RemoveAll(qe => queuedEvents.Contains(qe));

        }

        private IEnumerable<EventQueue> FilterEventQueueByEventTypes(List<int> eventTypes)
        {
            IEnumerable<EventQueue> filteredEvents = this.queues.QueuedEvents.Where(q => eventTypes.Contains(q.EventId));
            return filteredEvents;
        }
    }
}
