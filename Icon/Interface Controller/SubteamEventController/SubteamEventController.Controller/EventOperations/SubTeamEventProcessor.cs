using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GlobalEventController.Controller.EventOperations;
using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.Controller.Models;


namespace SubteamEventController.Controller.EventOperations
{
    public class SubTeamEventProcessor : ISubTeamEventProcessor
    {
		private IEventQueues queues;
        private ILogger<SubTeamEventProcessor> logger;
		private IEventServiceProvider eventServiceProvider;
        private ExceptionHandler<SubTeamEventProcessor> exceptionHandler;

        public SubTeamEventProcessor(IEventQueues queues,
            ILogger<SubTeamEventProcessor> logger,
            IEventServiceProvider eventServiceProvider)
		{
            this.exceptionHandler = new ExceptionHandler<SubTeamEventProcessor>(logger);
			this.queues = queues;
			this.logger = logger;
			this.eventServiceProvider = eventServiceProvider;
		}

        public void ProcessSubTeamUpdateEvents()
        {
            List<EventQueue> posDeptUpdateEvents = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.SubTeamUpdate)
                .ToList();

            if (posDeptUpdateEvents.Count == 0)
            {
                logger.Info("There are no Sub Team Update events to process.");
                return;
            }

            EnumerateAndProcessEventRows(posDeptUpdateEvents);    
        }

        //Duplicated from GloCon 
        private void EnumerateAndProcessEventRows(List<EventQueue> queuedEvents)
        {
            foreach (var queuedEvent in queuedEvents)
            {
                IEventService eventService = null;

                try
                {
                    string eventName = queuedEvent.EventType.EventName.MapToRegisteredEvent();

                    if (String.IsNullOrEmpty(eventName))
                    {
                        throw new ArgumentException(String.Format("There is no event in the interface controller mapped to the following event name.  Execution will continue without processing this event: EventName: {0}, QueueId = {1}",
                            queuedEvent.EventType.EventName, queuedEvent.QueueId));
                    }

                    eventService = eventServiceProvider.GetEventService(eventName.ToEnum<Enums.EventNames>(), queuedEvent.RegionCode);

                    if (eventService == null)
                    {
                        throw new InvalidOperationException(
                            String.Format("No event handler is mapped for the following event.  Execution will continue without processing this event: eventName = {0}", eventName));
                    }

                    eventService.ReferenceId = queuedEvent.EventReferenceId;
                    eventService.Message = queuedEvent.EventMessage;
                    eventService.Region = queuedEvent.RegionCode;


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
                    logger.Error(String.Format("Execution failed for event handler: {0}.  Processing will continue to the next event in the queue.",
                        eventService != null ? eventService.GetType().ToString() : String.Empty));
                    exceptionHandler.HandleException(ex, this.GetType(), MethodBase.GetCurrentMethod(),
                        eventService != null ? eventService.GetType() : typeof(IEventService),
                        queuedEvent.RegionCode, queuedEvent.EventReferenceId.Value, queuedEvent.EventMessage);

                    continue;
                }
            }
        }
    }
}
