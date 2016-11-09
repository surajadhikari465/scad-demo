using GlobalEventController.Common;
using GlobalEventController.Controller.EventServices;
using SubteamEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.Controller.EventOperations;
using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubteamEventController.Controller.EventOperations
{
    public class BulkItemSubTeamEventProcessor : IItemSubTeamEventBulkProcessor
    {
        private IEventQueues queues;
        private ILogger<BulkItemSubTeamEventProcessor> logger;
        private IEventServiceProvider eventServiceProvider;
        private IQueryHandler<BulkGetItemsWithSubTeamQuery, List<ItemSubTeamModel>> bulkGetItemSubTeamModel;
        private ExceptionHandler<BulkItemSubTeamEventProcessor> exceptionHandler;

        public BulkItemSubTeamEventProcessor(IEventQueues queues,
            ILogger<BulkItemSubTeamEventProcessor> logger,
            IEventServiceProvider eventServiceProvider,
            IQueryHandler<BulkGetItemsWithSubTeamQuery, List<ItemSubTeamModel>> bulkGetItemSubTeamModel)
        {
            this.exceptionHandler = new ExceptionHandler<BulkItemSubTeamEventProcessor>(logger);
            this.queues = queues;
            this.logger = logger;
            this.eventServiceProvider = eventServiceProvider;
            this.bulkGetItemSubTeamModel = bulkGetItemSubTeamModel;
        }

        public void BulkProcessItemSubTeamEvents()
        {
            // Filter Queued Events to Item Event Types
            List<EventQueue> itemQueuedEvents = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.ItemSubTeamUpdate)
                .ToList();

            if (itemQueuedEvents.Count == 0)
            {
                logger.Info("There are no item events to process.");
                return;
            }

            // Populate Dictionary with Region as the Key and List of EventQueues as the value
            string eventName = null;
            this.logger.Debug("Start filling up Dictionary to sort regions and item updates.");
            foreach (var queuedEvent in itemQueuedEvents)
            {
                eventName = queuedEvent.EventType.EventName.MapToRegisteredEvent();
                List<EventQueue> existingEventsForRegion;
                if (!this.queues.RegionToEventQueueDictionary.TryGetValue(queuedEvent.RegionCode, out existingEventsForRegion))
                {
                    existingEventsForRegion = new List<EventQueue>();
                    this.queues.RegionToEventQueueDictionary[queuedEvent.RegionCode] = existingEventsForRegion;
                }
                existingEventsForRegion.Add(queuedEvent);
            }
            this.logger.Debug("Finished filling up Dictionary.");

            // Iterate Dictionary and perform Bulk Updates for All Regions
            foreach (KeyValuePair<string, List<EventQueue>> entry in this.queues.RegionToEventQueueDictionary)
            {
                try
                {
                    IBulkItemSubTeamEventService bulkItemEventService = eventServiceProvider.GetBulkItemSubTeamEventService((entry.Key));

                    logger.Debug(String.Format("Get scanCode information from Icon for {0} region.", entry.Key));
                    bulkItemEventService.ItemSubTeamModelList = bulkGetItemSubTeamModel.Handle(new BulkGetItemsWithSubTeamQuery { ScanCodes = entry.Value.Select(v => v.EventMessage).ToList() });

                    logger.Info(String.Format("Begin Bulk Updating {0} Items in IRMA for {1} region.", entry.Value.Count.ToString(), entry.Key));
                    bulkItemEventService.Run();

                    logger.Info(String.Format("Successfully bulk processed {0} item for {1} region.", entry.Value.Count.ToString(), entry.Key));

                    // Add to ProcessedEvents List
                    this.queues.ProcessedEvents.AddRange(entry.Value);

                    // Remove from QueuedEvents List so that they do not get processed again with single item process
                    this.queues.QueuedEvents.RemoveAll(qe => entry.Value.Contains(qe));
                }
                catch (Exception e)
                {
                    logger.Warn(String.Format("Failed to Bulk Update {0} Items for {1} region.  This region's item events will be processed row by row. Exception {2}",
                        entry.Value.Count.ToString(), entry.Key, e.GetBaseException()));
                    continue;
                }
            }
        }
    }
}
