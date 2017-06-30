using GlobalEventController.Common;
using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GlobalEventController.Controller.EventOperations
{
    public class EventProcessor : IEventProcessor
	{
		private IEventQueues queues;
		private ILogger<EventProcessor> logger;
		private IEventServiceProvider eventServiceProvider;
		private ExceptionHandler<EventProcessor> exceptionHandler;
        private IDataIssueMessageCollector dataIssueMessage;
        private IEventArchiver eventArchiver;

        public EventProcessor(IEventQueues queues,
			ILogger<EventProcessor> logger,
			IEventServiceProvider eventServiceProvider,
            IDataIssueMessageCollector dataIssueMessage,
            IEventArchiver eventArchiver)
		{
			this.exceptionHandler = new ExceptionHandler<EventProcessor>(logger);
			this.queues = queues;
			this.logger = logger;
			this.eventServiceProvider = eventServiceProvider;
            this.dataIssueMessage = dataIssueMessage;
            this.eventArchiver = eventArchiver;
        }

		public void ProcessBrandNameUpdateEvents()
		{
			// Filter QueuedEvent list to Brand Events
			List<EventQueue> brandNameUpdateQueuedEvents = this.queues.QueuedEvents
				.Where(q => q.EventId == EventTypes.BrandNameUpdate)
				.ToList();

			if (brandNameUpdateQueuedEvents.Count == 0)
			{
				logger.Info("There are no brand name update events to process.");
				return;
			}
			
			EnumerateAndProcessEventRows(brandNameUpdateQueuedEvents);    
		}

		public void ProcessTaxEvents()
		{
			// Filter QueuedEvent list to Tax events
			List<EventQueue> taxQueuedEvents = this.queues.QueuedEvents
				.Where(q => q.EventId == EventTypes.NewTaxHierarchy || q.EventId == EventTypes.TaxNameUpdate)
				.ToList();

			if (taxQueuedEvents.Count == 0)
			{
				logger.Info("There are no tax events to process.");
				return;
			}

			EnumerateAndProcessEventRows(taxQueuedEvents);
		}

        public void ProcessBrandDeleteEvents()
        {
            // Filter QueuedEvent list to Brand Events
            List<EventQueue> brandDeleteQueuedEvents = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.BrandDelete)
                .ToList();

            if (brandDeleteQueuedEvents.Count == 0)
            {
                logger.Info("There are no brand delete events to process.");
                return;
            }

            EnumerateAndProcessEventRows(brandDeleteQueuedEvents);
        }

        public void ProcessNationalClassAddOrUpdateEvents()
        {
            // Filter QueuedEvent list to National Class Events
            List<EventQueue> nationalClassEvents = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.NationalClassUpdate)
                .ToList();

            if (nationalClassEvents.Count == 0)
            {
                logger.Info("There are no National Class Update events to process.");
                return;
            }

            EnumerateAndProcessEventRows(nationalClassEvents);
        }

        public void ProcessNationalClassDeleteEvents()
        {
            // Filter QueuedEvent list to National Class Events
            List<EventQueue> nationalClassEvents = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.NationalClassDelete)
                .ToList();

            if (nationalClassEvents.Count == 0)
            {
                logger.Info("There are no National Class Delete events to process.");
                return;
            }

            EnumerateAndProcessEventRows(nationalClassEvents);
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
                    eventService.EventTypeId = queuedEvent.EventId;
				
					logger.Debug(String.Format("Start event service for event: {0}. Region = {1}, ReferenceId = {2}, Message = {3}",
						eventName, queuedEvent.RegionCode, queuedEvent.EventReferenceId, queuedEvent.EventMessage));

					eventService.Run();

					this.queues.ProcessedEvents.Add(queuedEvent);
					this.queues.QueuedEvents.Remove(queuedEvent);
                    this.eventArchiver.Events.Add(new ArchiveEventModelWrapper<EventQueue>(queuedEvent).ToEventArchive());

                    if (eventService.RegionalItemMessage != null && eventService.RegionalItemMessage.Count() > 0)
                    {
                        dataIssueMessage.Message.AddRange(eventService.RegionalItemMessage);
                    }

                    logger.Info(String.Format("Successfully processed event: {0}.  Region = {1}, ReferenceId = {2}, Message = {3}",
						eventName,
						queuedEvent.RegionCode,
						queuedEvent.EventReferenceId,
						queuedEvent.EventMessage));
				}
				catch (Exception ex)
				{
                    var failedEvent = new FailedEvent { Event = queuedEvent, FailureReason = ex.ToString() };
                    this.queues.FailedEvents.Add(failedEvent);
                    this.eventArchiver.Events.Add(
                        new ArchiveEventModelWrapper<FailedEvent>(failedEvent)
                        {
                            ErrorCode = Constants.ApplicationErrors.Codes.UnexpectedError,
                            ErrorDetails = ex.ToString()
                        }.ToEventArchive());

                    logger.Error(String.Format("Execution failed for event handler: {0}.  Processing will continue to the next event in the queue.",
                        eventService != null ? eventService.GetType().ToString() : String.Empty));
					exceptionHandler.HandleException(ex, this.GetType(), MethodBase.GetCurrentMethod(), 
                        eventService != null ? eventService.GetType() : typeof(IEventService),
						queuedEvent.RegionCode, queuedEvent.EventReferenceId.Value, queuedEvent.EventMessage);
				}
			}
		}
    }
}
