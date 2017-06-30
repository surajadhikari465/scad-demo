using GlobalEventController.Common;
using GlobalEventController.Controller.Decorators.EventFinalizer;
using GlobalEventController.Controller.EventOperations;
using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.Controller
{
    public static class ControllerProvider
    {
        public static void IntializeSettings()
        {
            using (var context = new IconContext())
            {
                context.Database.Connection.Open();

                GetRegionalSettingsBySettingsKeyNameQuery regionalSettingsQuery = new GetRegionalSettingsBySettingsKeyNameQuery(context);
                List<RegionalSettingsModel> regionalSettings = regionalSettingsQuery.Handle(new GetRegionalSettingsBySettingsKeyNameParameters { SettingsKeyName = ConfigurationConstants.SendItemNutritionUpdatesToIRMASettingsKey });
                //Get regions that are configured to receive Nutrition Events
                StartupOptions.NutritionEnabledRegions = regionalSettings.Where(rs => rs.Value == true).Select(a => a.RegionCode).ToList();
                if (StartupOptions.NutritionEnabledRegions == null)
                {
                    StartupOptions.NutritionEnabledRegions = new List<string>();
                }
            }
        }
        public static GlobalControllerBase ComposeController()
        {
            var collectorLogger = new NLogLoggerInstance<EventCollector>(StartupOptions.Instance.ToString());
            var processorLogger = new NLogLoggerInstance<EventProcessor>(StartupOptions.Instance.ToString());
            var bulkProcessorLogger = new NLogLoggerInstance<ItemEventBulkProcessor>(StartupOptions.Instance.ToString());
            var finalizerLogger = new NLogLoggerInstance<EventFinalizer>(StartupOptions.Instance.ToString());
            EventQueues queues = new EventQueues();

            var iconContextFactory = new IconDbContextFactory();
            var irmaContextFactory = new RegionalIrmaDbContextFactory();
            var dataIssueMessageCollector = new DataIssueMessageCollector(EmailClient.CreateFromConfig());
            var eventArchiver = new EventArchiver(new ArchiveEventsCommandHandler(iconContextFactory),
                new NLogLoggerInstance<EventArchiver>(StartupOptions.Instance.ToString()));

            return new GlobalControllerBase(
                queues,
                new EventCollector(
                    queues,
                    collectorLogger,
                    new BulkUpdateEventQueueInProcessCommandHandler(iconContextFactory)),
                new ItemEventBulkProcessor(
                    queues,
                    bulkProcessorLogger,
                    new EventServiceProvider(iconContextFactory, irmaContextFactory),
                    new BulkGetValidatedItemsQueryHandler(iconContextFactory, GlobalControllerSettings.CreateFromConfig()),
                    new GetIconItemNutritionQueryHandler(iconContextFactory),
                    dataIssueMessageCollector,
                    eventArchiver),
                new NutriFactsEventBulkProcessor(
                    queues,
                    bulkProcessorLogger,
                    new EventServiceProvider(iconContextFactory, irmaContextFactory),
                    new BulkGetValidatedItemsQueryHandler(iconContextFactory, GlobalControllerSettings.CreateFromConfig()),
                    new GetIconItemNutritionQueryHandler(iconContextFactory),
                    eventArchiver),
                new EventProcessor(
                    queues,
                    processorLogger,
                    new EventServiceProvider(iconContextFactory, irmaContextFactory),
                    dataIssueMessageCollector,
                    eventArchiver),
                new EventFinalizerEmailDecorator(
                    new EventFinalizer(
                        queues,
                        finalizerLogger,
                        new UpdateEventQueueFailuresCommandHandler(iconContextFactory),
                        new BulkDeleteEventQueueCommandHandler(iconContextFactory)),
                    queues,
                    EmailClient.CreateFromConfig()),
                dataIssueMessageCollector,
                eventArchiver);
        }
    }
}
