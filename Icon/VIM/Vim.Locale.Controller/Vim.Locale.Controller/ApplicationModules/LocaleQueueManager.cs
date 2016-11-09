using System;
using System.Collections.Generic;
using System.Linq;
using Vim.Common.ControllerApplication;
using Vim.Common.DataAccess;
using Vim.Common.DataAccess.Commands;
using Vim.Common.Email;
using Vim.Locale.Controller.DataAccess.Models;
using Vim.Locale.Controller.DataAccess.Queries;
using Vim.Logging;

namespace Vim.Locale.Controller.ApplicationModules
{
    public class LocaleQueueManager : IQueueManager<LocaleEventModel>
    {
        private ControllerApplicationSettings settings;
        private ICommandHandler<UpdateEventQueueInProcessCommand> updateEventQueueInProcessCommandHandler;
        private IQueryHandler<GetLocaleEventsQuery, List<LocaleEventModel>> getHierarchyClassEventsQueryHandler;
        private ICommandHandler<DeleteEventQueueCommand> deleteEventQueueCommandHandler;
        private ICommandHandler<UpdateFailedEventQueueCommand> updateFailedEventQueueCommandHandler;
        private IEmailClient emailClient;
        private IEmailMessageBuilder<List<LocaleEventModel>> emailMessageBuilder;
        private ILogger logger;

        public LocaleQueueManager(ControllerApplicationSettings settings,
            ICommandHandler<UpdateEventQueueInProcessCommand> updateEventQueueInProcessCommandHandler,
            IQueryHandler<GetLocaleEventsQuery, List<LocaleEventModel>> getHierarchyClassEventsQueryHandler,
            ICommandHandler<DeleteEventQueueCommand> deleteEventQueueCommandHandler,
            ICommandHandler<UpdateFailedEventQueueCommand> updateFailedEventQueueCommandHandler,
            IEmailClient emailClient,
            IEmailMessageBuilder<List<LocaleEventModel>> emailMessageBuilder,
            ILogger logger)
        {
            this.settings = settings;
            this.updateEventQueueInProcessCommandHandler = updateEventQueueInProcessCommandHandler;
            this.getHierarchyClassEventsQueryHandler = getHierarchyClassEventsQueryHandler;
            this.deleteEventQueueCommandHandler = deleteEventQueueCommandHandler;
            this.updateFailedEventQueueCommandHandler = updateFailedEventQueueCommandHandler;
            this.emailClient = emailClient;
            this.emailMessageBuilder = emailMessageBuilder;
            this.logger = logger;
        }

        public List<LocaleEventModel> Get(List<int> eventTypeIds)
        {
            var updateQueueParameters = new UpdateEventQueueInProcessCommand
            {
                EventTypeIds = eventTypeIds,
                Instance = this.settings.Instance,
                MaxNumberOfRowsToMark = this.settings.MaxNumberOfRowsToMark
            };
            this.updateEventQueueInProcessCommandHandler.Execute(updateQueueParameters);

            List<LocaleEventModel> stores = getHierarchyClassEventsQueryHandler.Search(new GetLocaleEventsQuery
            {
                Instance = this.settings.Instance
            });

            return stores == null ? new List<LocaleEventModel>() : stores;
        }

        public void Finalize(List<LocaleEventModel> queueRecords)
        {
            UpdateFailedEvents(queueRecords);
            DeleteEvents(queueRecords);
        }

        private void UpdateFailedEvents(List<LocaleEventModel> queueRecords)
        {
            var failedEvents = queueRecords.Where(q => !String.IsNullOrEmpty(q.ErrorMessage));
            if (failedEvents.Any())
            {
                this.updateFailedEventQueueCommandHandler.Execute(new UpdateFailedEventQueueCommand
                {
                    QueueIds = failedEvents.Select(q => q.QueueId)
                });
                emailClient.Send(emailMessageBuilder.BuildMessage(failedEvents.ToList()), "Failed Locale VIM Events");
                logger.Info(String.Format("Marked {0} VIM Locale events as failed.", failedEvents.Count()));
            }
        }

        private void DeleteEvents(List<LocaleEventModel> queueRecords)
        {
            var eventsToDelete = queueRecords.Where(q => String.IsNullOrEmpty(q.ErrorMessage));
            if (eventsToDelete.Any())
            {
                deleteEventQueueCommandHandler.Execute(new DeleteEventQueueCommand
                {
                    QueueIds = eventsToDelete.Select(q => q.QueueId)
                });
                logger.Info(String.Format("Deleted {0} VIM Locale events.", eventsToDelete.Count()));
            }
        }
    }
}
