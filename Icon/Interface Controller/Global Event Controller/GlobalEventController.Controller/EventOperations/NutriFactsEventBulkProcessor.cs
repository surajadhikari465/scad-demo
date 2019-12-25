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
        private IEventArchiver eventArchiver;
        HashSet<int> hsEventIDs = new HashSet<int> { EventTypes.NutritionAdd, EventTypes.NutritionUpdate, EventTypes.NutritionDelete };

        public NutriFactsEventBulkProcessor(IEventQueues queues,
            ILogger<ItemEventBulkProcessor> logger,
            IEventServiceProvider eventServiceProvider,
            IQueryHandler<BulkGetValidatedItemsQuery, List<ValidatedItemModel>> bulkGetValidatedItems,
            IQueryHandler<GetIconItemNutritionQuery, List<ItemNutrition>> getIconItemNutritionQueryHandler,
            IEventArchiver eventArchiver)
        {
            this.exceptionHandler = new ExceptionHandler<ItemEventBulkProcessor>(logger);
            this.queues = queues;
            this.logger = logger;
            this.eventServiceProvider = eventServiceProvider;
            this.bulkGetValidatedItems = bulkGetValidatedItems;
            this.getIconItemNutritionQueryHandler = getIconItemNutritionQueryHandler;
            this.eventArchiver = eventArchiver;
        }

        public void BulkProcessEvents()
        {
            //Filter Queued Events to Item Event Types. In case of multiple events for the same item the last item will processed.
            var nutritionQueuedEvents = this.queues.QueuedEvents
                .Where(x => hsEventIDs.Contains(x.EventId))
                .GroupBy(x => new { x.EventMessage, Region = x.RegionCode ?? String.Empty })
                .Select(x => x.MaxBy(y => y.QueueId))
                .ToArray();

            if (!nutritionQueuedEvents.Any())
            {
                logger.Info("There are no item events to process.");
                return;
            }

            // Populate Dictionary with Region as the Key and List of EventQueues as the value
            this.queues.RegionToEventQueueDictionary = nutritionQueuedEvents
                    .Where(x => x.EventId != EventTypes.NutritionDelete)
                  .GroupBy(e => e.RegionCode)
                    .ToDictionary(group => group.Key, g => g.ToList());

            // Iterate Dictionary and perform Bulk Updates for All Regions
            foreach (KeyValuePair<string, List<EventQueue>> entry in this.queues.RegionToEventQueueDictionary)
            {
                ProcessEvents(entry.Key, entry.Value);
            }

            //Process Delete events. Nutrition deletes in regions as specified by the queued events
            ExecuteDeleteEventService(nutritionQueuedEvents.Where(x => x.EventId == EventTypes.NutritionDelete).ToList(), false);
        }

        private void ProcessEvents(string region, IEnumerable<EventQueue> eventsToProcess)
        {
            try
            {
                ExecuteAddUpdateEventService(region, eventsToProcess.ToList(), false);
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
                    var failedEvents = eventsToProcess.Select(qe => new FailedEvent
                    {
                        Event = qe,
                        FailureReason = ex.ToString()
                    });
                    this.queues.FailedEvents.AddRange(failedEvents);
                    eventArchiver.Events.AddRange(failedEvents.ToEventArchiveList(Constants.ApplicationErrors.Codes.UnexpectedError, ex.ToString()));
                }
            }
        }

        private void ExecuteAddUpdateEventService(string regionCode, List<EventQueue> queuedEvents, bool markFailedEvents = true)
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

            logger.Info(String.Format("Begin Updating {0} rows in IRMA for {1} region.", queuedEvents.Count().ToString(), regionCode));

            // Run Service
            bulkNutritionEventService.Run();

            if (bulkNutritionEventService.ItemNutriFacts != null)
            {
                eventArchiver.Events.AddRange(bulkNutritionEventService.ItemNutriFacts.ToEventArchiveList(queuedEvents));
            }

            logger.Info($"Successfully processed {queuedEvents.Count().ToString()} events for {regionCode} region.");

            var hsScanCode = new HashSet<string>(queuedEvents.Select(x => x.EventMessage), StringComparer.InvariantCultureIgnoreCase);
            var allApplicableEvents = this.queues.QueuedEvents.Where(x => hsScanCode.Contains(x.EventMessage) && hsEventIDs.Contains(x.EventId) && x.EventId != EventTypes.NutritionDelete).ToArray();

            // Add to ProcessedEvents List
            this.queues.ProcessedEvents.AddRange(allApplicableEvents);

            // Remove from QueuedEvents List so that they do not get processed again with single item process
            this.queues.QueuedEvents.RemoveAll(qe => allApplicableEvents.Contains(qe));
        }

        private void ExecuteDeleteEventService(List<EventQueue> queuedEvents, bool markFailedEvents = true)
        {
            if (queuedEvents == null || queuedEvents.Count() == 0) return;

            var isFailed = false;
            foreach (var region in queuedEvents.DistinctBy(qe => qe.RegionCode).Select(qe => qe.RegionCode))
            {
                try
                {
                    IEnumerable<EventQueue> queuedEventsForRegion = queuedEvents.Where(qe => qe.RegionCode == region);

                    var bulkNutritionEventService = eventServiceProvider.GetBulkItemNutriFactsEventService(region);
                    bulkNutritionEventService.Region = region;
                    bulkNutritionEventService.ValidatedItemList = queuedEventsForRegion.Select(x => new ValidatedItemModel { ScanCode = x.EventMessage, EventTypeId = x.EventId }).ToList();

                    int count = queuedEventsForRegion.Count();
                    logger.Info($"Begin deleting {count} nutrition scan codes in IRMA for {region} region.");
                    bulkNutritionEventService.Run();

                    logger.Info($"Successfully processed {count} events for {region} region.");
                }
                catch (Exception ex)
                {
                    isFailed = true;
                    this.logger.Error($"DeleteNutrition failed for {region}. {ex.Message}");
                }
            }

            if (!isFailed)
            {
                var events = this.queues.QueuedEvents.Where(x => x.EventId == EventTypes.NutritionDelete).ToArray();

                //Add to ProcessedEvents List
                this.queues.ProcessedEvents.AddRange(events);

                //Remove from QueuedEvents List so that they do not get processed again with single item process
                this.queues.QueuedEvents = this.queues.QueuedEvents.Except(events).ToList();
            }
        }

        private IEnumerable<EventQueue> FilterEventQueueByEventTypes(List<int> eventTypes)
        {
            IEnumerable<EventQueue> filteredEvents = this.queues.QueuedEvents.Where(q => eventTypes.Contains(q.EventId));
            return filteredEvents;
        }
    }
}