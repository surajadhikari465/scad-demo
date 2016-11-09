using GlobalEventController.Common;
using GlobalEventController.Controller.EventServices;
using GlobalEventController.Controller.EventOperations;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using Icon.Logging;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.Controller.Decorators.EventFinalizer;
using Icon.Common.Email;
using System.Collections.Generic;
using System.Linq;
using GlobalEventController.DataAccess.Infrastructure;

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

            var contextManager = new ContextManager();
            var dataIssueMessageCollector = new DataIssueMessageCollector(EmailClient.CreateFromConfig());

            return new GlobalControllerBase(
                queues,
                new EventCollector(
                    queues,
                    collectorLogger,
                    new BulkUpdateEventQueueInProcessCommandHandler(contextManager)),
                new ItemEventBulkProcessor(
                    queues,
                    bulkProcessorLogger,
                    new EventServiceProvider(contextManager),
                    new BulkGetValidatedItemsQueryHandler(contextManager, GlobalControllerSettings.CreateFromConfig()),
                    new GetIconItemNutritionQueryHandler(contextManager),
                    dataIssueMessageCollector),
                new NutriFactsEventBulkProcessor(
                    queues,
                    bulkProcessorLogger,
                    new EventServiceProvider(contextManager),
                    new BulkGetValidatedItemsQueryHandler(contextManager, GlobalControllerSettings.CreateFromConfig()),
                    new GetIconItemNutritionQueryHandler(contextManager)),
                new EventProcessor(
                    queues,
                    processorLogger,
                    new EventServiceProvider(contextManager),
                    dataIssueMessageCollector),
                new EventFinalizerEmailDecorator(
                    new EventFinalizer(
                        queues,
                        finalizerLogger,
                        new UpdateEventQueueFailuresCommandHandler(contextManager),
                        new BulkDeleteEventQueueCommandHandler(contextManager)),
                    queues,
                    EmailClient.CreateFromConfig()),
                contextManager,
                dataIssueMessageCollector);
        }
    }
}
