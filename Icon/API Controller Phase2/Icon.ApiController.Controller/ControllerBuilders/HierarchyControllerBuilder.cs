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
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Controller.ControllerBuilders
{
    public class HierarchyControllerBuilder : IControllerBuilder
    {
        public ApiControllerBase ComposeController(IRenewableContext<IconContext> globalContext)
        {
            ControllerType.Type = "Hierarchy";

            var instance = ControllerType.Instance.ToString();
            var baseLogger = new NLogLoggerInstance<ApiControllerBase>(instance);
            var emailClient = new EmailClient(EmailHelper.BuildEmailClientSettings());
            var producer = new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig("HierarchyQueueName"));
            var settings = ApiControllerSettings.CreateFromConfig("Icon", ControllerType.Instance);

            producer.OpenConnection();

            var messageHistoryProcessor = BuilderHelpers.BuildMessageHistoryProcessor(instance, MessageTypes.Hierarchy, producer, globalContext);

            var queueProcessorLogger = new NLogLoggerInstance<HierarchyQueueProcessor>(instance);
            var queueReader = new HierarchyQueueReader(
                new NLogLoggerInstance<HierarchyQueueReader>(instance),
                emailClient,
                new GetMessageQueueQuery<MessageQueueHierarchy>(new NLogLoggerInstance<GetMessageQueueQuery<MessageQueueHierarchy>>(instance), globalContext),
                new UpdateMessageQueueStatusCommandHandler<MessageQueueHierarchy>(
                    new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueHierarchy>>(instance), 
                    globalContext));
            var serializer = new Serializer<Contracts.HierarchyType>(
                new NLogLoggerInstance<Serializer<Contracts.HierarchyType>>(instance),
                emailClient);
            var getFinancialHierarchyClassesQueryHandler = new GetFinancialHierarchyClassesQuery(globalContext);
            var saveXmlMessageCommandHandler = new SaveToMessageHistoryCommandHandler(
                new NLogLoggerInstance<SaveToMessageHistoryCommandHandler>(instance),
                globalContext);
            var associateMessageToMessageQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueHierarchy>(
                new NLogLoggerInstance<AssociateMessageToQueueCommandHandler<MessageQueueHierarchy>>(instance),
                globalContext);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueHierarchy>(
                new NLogLoggerInstance<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueHierarchy>>(instance), 
                globalContext);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(
                new NLogLoggerInstance<UpdateMessageHistoryStatusCommandHandler>(instance), 
                globalContext);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueHierarchy>(
                new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueHierarchy>>(instance), 
                globalContext);
            var updateStagedProductStatusCommandHandler = new UpdateStagedProductStatusCommandHandler(
                new NLogLoggerInstance<UpdateStagedProductStatusCommandHandler>(instance), 
                globalContext);
            var updateSentToEsbHierarchyTraitCommandHandler = new UpdateSentToEsbHierarchyTraitCommandHandler(new NLogLoggerInstance<UpdateSentToEsbHierarchyTraitCommandHandler>(instance), globalContext);
            var markQueuedEntriesAsInProcessCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueHierarchy>(new NLogLoggerInstance<MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueHierarchy>>(instance), globalContext);

            var monitorCommandHandler = new SaveMessageProcessorJobSummaryCommandHandler(
                new NLogLoggerInstance<SaveMessageProcessorJobSummaryCommandHandler>(instance), globalContext);
            var monitor = new MessageProcessorMonitor(monitorCommandHandler);

            var hierarchyQueueProcessor = new HierarchyQueueProcessor(
                settings,
                queueProcessorLogger,
                globalContext,
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
