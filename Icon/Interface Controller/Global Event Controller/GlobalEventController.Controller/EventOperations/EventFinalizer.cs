using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Logging;
using InterfaceController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Controller.EventOperations
{
    public class EventFinalizer : IEventFinalizer
    {
        private IEventQueues queues;
        private ILogger<EventFinalizer> logger;
        private ExceptionHandler<EventFinalizer> exceptionHandler;
        private ICommandHandler<UpdateEventQueueFailuresCommand> updateFailedEventHandler;
        private ICommandHandler<BulkDeleteEventQueueCommand> bulkDeleteEventQueueHandler;
        
        public EventFinalizer(IEventQueues queues,
            ILogger<EventFinalizer> logger,
            ICommandHandler<UpdateEventQueueFailuresCommand> updateFailedEventHandler,
            ICommandHandler<BulkDeleteEventQueueCommand> bulkDeleteEventQueueHandler)
        {
            this.queues = queues;
            this.logger = logger;
            this.updateFailedEventHandler = updateFailedEventHandler;
            this.bulkDeleteEventQueueHandler = bulkDeleteEventQueueHandler;

            this.exceptionHandler = new ExceptionHandler<EventFinalizer>(logger);
        }

        public void HandleFailedEvents()
        {
            if (this.queues.FailedEvents.Count == 0)
            {
                return;
            }

            try
            {
                UpdateEventQueueFailuresCommand updateEventQueue = new UpdateEventQueueFailuresCommand();
                updateEventQueue.FailedEvents = this.queues.FailedEvents.Select(e => e.Event).ToList();

                this.updateFailedEventHandler.Handle(updateEventQueue);
                logger.Info(String.Format("Marked {0} events as failed in app.EventQueue.", this.queues.FailedEvents.Count.ToString()));
            }
            catch (Exception e)
            {
                logger.Error("Could not mark event as failed.  Event details will be logged in later call.");
                exceptionHandler.HandleException(e, this.GetType(), MethodBase.GetCurrentMethod(), updateFailedEventHandler.GetType());
            }
        }

        public void DeleteEvents()
        {
            if (this.queues.ProcessedEvents.Count == 0)
            {
                return;
            }

            try
            {
                BulkDeleteEventQueueCommand bulkDeleteQueue = new BulkDeleteEventQueueCommand();
                bulkDeleteQueue.EventsToDelete = this.queues.ProcessedEvents;
                this.bulkDeleteEventQueueHandler.Handle(bulkDeleteQueue);

                this.logger.Info(String.Format("Bulk Deleted {0} events from app.EventQueue; QueueIDs {1} to {2}",
                    this.queues.ProcessedEvents.Count, this.queues.ProcessedEvents.Select(e => e.QueueId).Min().ToString(),
                    this.queues.ProcessedEvents.Select(e => e.QueueId).Max().ToString()));
            }
            catch (Exception e)
            {
                logger.Error("Unable to delete events from the Icon EventQueue table.");
                exceptionHandler.HandleException(e, this.GetType(), MethodBase.GetCurrentMethod());
                throw;
            }
        }
    }
}
