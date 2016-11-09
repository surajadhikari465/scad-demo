using GlobalEventController.Common;
using SubteamEventController.Controller.EventServices;
using SubteamEventController.Controller.EventOperations;
using GlobalEventController.Controller.EventOperations;
using GlobalEventController.DataAccess.Commands;
using SubteamEventController.DataAccess.Queries;
using Icon.Framework;
using Icon.Logging;
using GlobalEventController.DataAccess.BulkCommands;
using SubteamEventController.DataAccess.BulkCommands;
using Icon.Common.Email;

namespace SubteamEventController.Controller
{
    public static class ControllerProvider
    {
        public static EmailClient emailClient;
        private static IconContext iconContext = new IconContext();

        public static SubteamEventControllerBase ComposeController()
        {
            var collectorLogger = new NLogLoggerInstance<EventCollector>(StartupOptions.Instance.ToString());
            var processorLogger = new NLogLoggerInstance<ItemSubTeamEventProcessor>(StartupOptions.Instance.ToString());
            var subTeamProcessorLogger = new NLogLoggerInstance<SubTeamEventProcessor>(StartupOptions.Instance.ToString());
            var bulkProcessorLogger = new NLogLoggerInstance<BulkItemSubTeamEventProcessor>(StartupOptions.Instance.ToString());
            var finalizerLogger = new NLogLoggerInstance<EventFinalizer>(StartupOptions.Instance.ToString());
            EventQueues eventBase = new EventQueues();

            return new SubteamEventControllerBase(
                eventBase,
                new EventCollector(
                    eventBase,
                    collectorLogger,
                    new BulkUpdateEventQueueInProcessCommandHandler(iconContext)),
                new BulkItemSubTeamEventProcessor(
                    eventBase,
                    bulkProcessorLogger,
                    new ServiceProvider(),
                    new BulkGetItemsWithSubTeamQueryHandler(iconContext)),
                new ItemSubTeamEventProcessor(
                    eventBase,
                    processorLogger,
                    new ServiceProvider()),
                new SubTeamEventProcessor(
                    eventBase,
                    subTeamProcessorLogger,
                    new ServiceProvider()),
                new SubTeamEventFinalizer(
                new EventFinalizer(
                    eventBase,
                    finalizerLogger,
                    new UpdateEventQueueFailuresCommandHandler(iconContext),
                    new BulkDeleteEventQueueCommandHandler(iconContext)),
                    eventBase,
                    emailClient));
        }
    }
}
