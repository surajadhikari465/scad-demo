﻿using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Common;
using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;

namespace GlobalEventController.Controller.EventOperations
{
    public class EventCollector : IEventCollector
    {
        private IEventQueues queues;
        private ILogger<EventCollector> logger;
        private IQueryHandler<BulkUpdateEventQueueInProcessCommand, List<EventQueueCustom>> getInProcessEventsHandler;
        private ExceptionHandler<EventCollector> exceptionHandler;

        public EventCollector(IEventQueues queues,
            ILogger<EventCollector> logger,
            IQueryHandler<BulkUpdateEventQueueInProcessCommand, List<EventQueueCustom>> getInProcessEventsHandler)
        {
            this.exceptionHandler = new ExceptionHandler<EventCollector>(logger);
            this.queues = queues;
            this.logger = logger;
            this.getInProcessEventsHandler = getInProcessEventsHandler;
        }

        public void GetEvents()
        {
            try
            {
                // Get Events and Mark in Process
                this.logger.Info("Start retreiving and marking rows in Process from app.EventQueue");
                this.queues.QueuedEvents = GetQueuedEventsFromIcon();

                logger.Info(String.Format("Retrieved and marked {0} event(s) in process from the Icon event queue.", this.queues.QueuedEvents.Count));
            }
            catch (Exception exception)
            {
                exceptionHandler.HandleException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                throw;
            }
        }

        private List<EventQueue> GetQueuedEventsFromIcon()
        {
            int maxQueueEventsToProcess;
            bool successfulParse = Int32.TryParse(ConfigurationManager.AppSettings["MaxQueueEntriesToProcess"], out maxQueueEventsToProcess);

            if (!successfulParse)
            {
                logger.Error("Unable to retrieve configuration: MaxQueueEntriesToProcess.  Using default value of 100.");
                maxQueueEventsToProcess = 100;
            }

            BulkUpdateEventQueueInProcessCommand inProcessQuery = new BulkUpdateEventQueueInProcessCommand();
            inProcessQuery.RegisteredEventNames = EventRegistrationService.RegisteredEvents;
            inProcessQuery.MaxRows = maxQueueEventsToProcess;
            inProcessQuery.Instance = StartupOptions.Instance.ToString();

            List<EventQueueCustom> custom = getInProcessEventsHandler.Handle(inProcessQuery);
            return custom.Select(c => new EventQueue
                {
                    QueueId = c.QueueId,
                    EventId = c.EventId,
                    EventMessage = c.EventMessage,
                    InsertDate = c.InsertDate,
                    EventReferenceId = c.EventReferenceId,
                    InProcessBy = c.InProcessBy,
                    ProcessFailedDate = c.ProcessFailedDate,
                    RegionCode = c.RegionCode,
                    EventType = new EventType { EventId = c.EventId, EventName = c.EventName }
                }).ToList();
        }
    }
}
