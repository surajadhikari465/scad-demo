using GlobalEventController.Controller.EventServices;
using GlobalEventController.Controller.EventOperations;
using SubteamEventController.DataAccess.BulkCommands;
using SubteamEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using SubteamEventController.DataAccess.Queries;
using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GlobalEventController.Controller.Models;

namespace SubteamEventController.Controller.EventOperations
{
    public class ItemSubTeamEventProcessor : IEventItemSubTeamProcessor
    {
        private IEventQueues queues;
        private ILogger<ItemSubTeamEventProcessor> logger;
        private IEventServiceProvider eventServiceProvider;
        private ExceptionHandler<ItemSubTeamEventProcessor> exceptionHandler;

        public ItemSubTeamEventProcessor(IEventQueues queues,
            ILogger<ItemSubTeamEventProcessor> logger,
            IEventServiceProvider eventServiceProvider)
        {
            this.exceptionHandler = new ExceptionHandler<ItemSubTeamEventProcessor>(logger);
            this.queues = queues;
            this.logger = logger;
            this.eventServiceProvider = eventServiceProvider;
        }

        /// <summary>
        /// Row by row processing of Item Events.
        /// </summary>
        public void ProcessItemSubTeamEvents()
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

            EnumerateAndProcessEventRows(itemQueuedEvents);
        }

        /// <summary>
        /// This is used for processing tax and brand type events which does processing one by one.
        /// This will also process item events if there are any bulk failure for any region.
        /// </summary>
        /// <param name="queuedEvents">List of the Events filtered to the specific type.</param>
        private void EnumerateAndProcessEventRows(List<EventQueue> queuedEvents)
        {
            foreach (var queuedEvent in queuedEvents)
            {
                string eventName = queuedEvent.EventType.EventName.MapToRegisteredEvent();

                if (String.IsNullOrEmpty(eventName))
                {
                    logger.Error(String.Format("There is no event in the interface controller mapped to the following event name.  Execution will continue without processing this event: EventName: {0}, QueueId = {1}",
                        queuedEvent.EventType.EventName, queuedEvent.QueueId));
                    continue;
                }

                IEventService eventService = eventServiceProvider.GetEventService(eventName.ToEnum<Enums.EventNames>(), queuedEvent.RegionCode);

                if (eventService == null)
                {
                    logger.Error(String.Format("No event handler is mapped for the following event.  Execution will continue without processing this event: eventName = {0}", eventName));
                    continue;
                }

                eventService.ReferenceId = queuedEvent.EventReferenceId;
                eventService.Message = queuedEvent.EventMessage;
                eventService.Region = queuedEvent.RegionCode;

                try
                {
                    logger.Debug(String.Format("Start event service for event: {0}. Region = {1}, ReferenceId = {2}, Message = {3}",
                        eventName, queuedEvent.RegionCode, queuedEvent.EventReferenceId, queuedEvent.EventMessage));

                    eventService.Run();
                    this.queues.ProcessedEvents.Add(queuedEvent);
                    this.queues.QueuedEvents.Remove(queuedEvent);

                    logger.Info(String.Format("Successfully processed event: {0}.  Region = {1}, ReferenceId = {2}, Message = {3}",
                        eventName,
                        queuedEvent.RegionCode,
                        queuedEvent.EventReferenceId,
                        queuedEvent.EventMessage));
                }
                catch (Exception ex)
                {
                    this.queues.FailedEvents.Add(new FailedEvent { Event = queuedEvent, FailureReason = ex.Message });
                    logger.Error(String.Format("Execution failed for event handler: {0}.  Processing will continue to the next event in the queue.", eventService.GetType()));
                    exceptionHandler.HandleException(ex, this.GetType(), MethodBase.GetCurrentMethod(), eventService.GetType(),
                        queuedEvent.RegionCode, queuedEvent.EventReferenceId.Value, queuedEvent.EventMessage);

                    continue;
                }
            }
        }
    }
}
