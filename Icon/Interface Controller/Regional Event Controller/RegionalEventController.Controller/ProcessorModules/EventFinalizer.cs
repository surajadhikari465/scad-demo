using Icon.Common.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegionalEventController.Controller.Processors;
using RegionalEventController.DataAccess.Models;
using RegionalEventController.Controller.Email;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Interfaces;

namespace RegionalEventController.Controller.ProcessorModules
{
    public class EventFinalizer : IEventFinalizer
    {
        private List<IrmaNewItem> queues;
        private IEmailClient emailClient;
        private IBulkCommandHandler<DeleteNewItemsFromIrmaQueueCommand> deleteNewItemsFromIrmaQueueCommandHandler;


        public EventFinalizer(
            List<IrmaNewItem> queues,
            IEmailClient emailClient,
            IBulkCommandHandler<DeleteNewItemsFromIrmaQueueCommand> deleteNewItemsFromIrmaQueueCommandHandler)
        {
            this.queues = queues;
            this.emailClient = emailClient;
            this.deleteNewItemsFromIrmaQueueCommandHandler = deleteNewItemsFromIrmaQueueCommandHandler;
        }

        public void HandleFailedEvents()
        {
            List<IrmaNewItem> eventsHadException = queues
                .Where(s => !string.IsNullOrEmpty(s.FailureReason) && !s.IsInvalidQueueEntry)
                .ToList();

            if (eventsHadException.Any())
            {
                var failedEvents = EmailHelper.BuildEventQueueTable(eventsHadException);
                emailClient.Send(String.Format(Resource.ErrorFailedEvents, failedEvents), Resource.EmailSubjectFailedEvents);
            }
        }

        public void DeleteEvents()
        {
            DeleteNewItemsFromIrmaQueueCommand deleteNewItemsFromIrmaQueueCommand = new DeleteNewItemsFromIrmaQueueCommand();
            deleteNewItemsFromIrmaQueueCommand.NewIrmaItems = queues;

            deleteNewItemsFromIrmaQueueCommandHandler.Execute(deleteNewItemsFromIrmaQueueCommand);
        }
    }
}
