using Icon.ApiController.Common;
using Icon.ApiController.Controller.Monitoring;
using Icon.ApiController.Controller.QueueProcessors;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.RenewableContext;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Logging;
using Contracts = Icon.Esb.Schemas.Wfm.PreGpm.Contracts;

namespace Icon.ApiController.Controller.ControllerBuilders
{
    public class HierarchyControllerBuilder : IControllerBuilder
    {
        public ApiControllerBase ComposeController()
        {
            ControllerType.Type = "Hierarchy";

            var instance = ControllerType.Instance.ToString();
            var baseLogger = new NLogLoggerInstance<ApiControllerBase>(instance);
            var emailClient = new EmailClient(EmailHelper.BuildEmailClientSettings());
            var producer = new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig("HierarchyQueueName"));
            var settings = ApiControllerSettings.CreateFromConfig("Icon", ControllerType.Instance);

            IconDbContextFactory iconContextFactory = new IconDbContextFactory();

            producer.OpenConnection();

            var messageHistoryProcessor = BuilderHelpers.BuildMessageHistoryProcessor(instance, MessageTypes.Hierarchy, producer, iconContextFactory);

            var queueProcessorLogger = new NLogLoggerInstance<HierarchyQueueProcessor>(instance);
            var queueReader = new HierarchyQueueReader(
                new NLogLoggerInstance<HierarchyQueueReader>(instance),
                emailClient,
                new GetMessageQueueQuery<MessageQueueHierarchy>(new NLogLoggerInstance<GetMessageQueueQuery<MessageQueueHierarchy>>(instance), iconContextFactory),
                new UpdateMessageQueueStatusCommandHandler<MessageQueueHierarchy>(
                    new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueHierarchy>>(instance), 
                    iconContextFactory));
            var serializer = new Serializer<Contracts.HierarchyType>(
                new NLogLoggerInstance<Serializer<Contracts.HierarchyType>>(instance),
                emailClient);
            var getFinancialHierarchyClassesQueryHandler = new GetFinancialHierarchyClassesQuery(iconContextFactory);
            var saveXmlMessageCommandHandler = new SaveToMessageHistoryCommandHandler(
                new NLogLoggerInstance<SaveToMessageHistoryCommandHandler>(instance),
                iconContextFactory);
            var associateMessageToMessageQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueHierarchy>(
                new NLogLoggerInstance<AssociateMessageToQueueCommandHandler<MessageQueueHierarchy>>(instance),
                iconContextFactory);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueHierarchy>(
                new NLogLoggerInstance<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueHierarchy>>(instance), 
                iconContextFactory);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(
                new NLogLoggerInstance<UpdateMessageHistoryStatusCommandHandler>(instance), 
                iconContextFactory);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueHierarchy>(
                new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueHierarchy>>(instance), 
                iconContextFactory);
            var updateStagedProductStatusCommandHandler = new UpdateStagedProductStatusCommandHandler(
                new NLogLoggerInstance<UpdateStagedProductStatusCommandHandler>(instance), 
                iconContextFactory);
            var updateSentToEsbHierarchyTraitCommandHandler = new UpdateSentToEsbHierarchyTraitCommandHandler(new NLogLoggerInstance<UpdateSentToEsbHierarchyTraitCommandHandler>(instance), iconContextFactory);
            var markQueuedEntriesAsInProcessCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueHierarchy>(new NLogLoggerInstance<MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueHierarchy>>(instance), iconContextFactory);

            var monitorCommandHandler = new SaveMessageProcessorJobSummaryCommandHandler(
                new NLogLoggerInstance<SaveMessageProcessorJobSummaryCommandHandler>(instance), iconContextFactory);
            var monitor = new MessageProcessorMonitor(monitorCommandHandler);

            var hierarchyQueueProcessor = new HierarchyQueueProcessor(
                settings,
                queueProcessorLogger,
                queueReader,
                serializer,
                getFinancialHierarchyClassesQueryHandler,
                saveXmlMessageCommandHandler,
                associateMessageToMessageQueueCommandHandler,
                setProcessedDateCommandHandler,
                updateMessageHistoryCommandHandler,
                updateMessageQueueStatusCommandHandler,
                updateStagedProductStatusCommandHandler,
                updateSentToEsbHierarchyTraitCommandHandler,
                markQueuedEntriesAsInProcessCommandHandler,
                producer,
                monitor);

            return new ApiControllerBase(baseLogger, emailClient, messageHistoryProcessor, hierarchyQueueProcessor, producer);
        }
    }
}
