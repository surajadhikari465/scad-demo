using Icon.ApiController.Common;
using Icon.ApiController.Controller.CollectionProcessors;
using Icon.ApiController.Controller.Mappers;
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
    public class ItemLocaleControllerBuilder : IControllerBuilder
    {
        ApiControllerBase IControllerBuilder.ComposeController(IRenewableContext<IconContext> globalContext)
        {
            ControllerType.Type = "ItemLocale";

            var instance = ControllerType.Instance.ToString();
            var baseLogger = new NLogLoggerInstance<ApiControllerBase>(instance);
            var emailClient = new EmailClient(EmailHelper.BuildEmailClientSettings());
            var producer = new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig("ItemQueueName"));
            var settings = ApiControllerSettings.CreateFromConfig("Icon", ControllerType.Instance);

            producer.OpenConnection();

            var messageHistoryProcessor = BuilderHelpers.BuildMessageHistoryProcessor(instance, MessageTypes.ItemLocale, producer, globalContext);

            var queueProcessorLogger = new NLogLoggerInstance<ItemLocaleQueueProcessor>(instance);
            var serializer = new Serializer<Contracts.items>(
                new NLogLoggerInstance<Serializer<Contracts.items>>(instance),
                emailClient);
            var queueReader = new ItemLocaleQueueReader(
                new NLogLoggerInstance<ItemLocaleQueueReader>(instance),
                emailClient,
                new GetMessageQueueQuery<MessageQueueItemLocale>(new NLogLoggerInstance<GetMessageQueueQuery<MessageQueueItemLocale>>(instance), globalContext),
                new GetItemByScanCodeQuery(
                    new NLogLoggerInstance<GetItemByScanCodeQuery>(instance),
                    globalContext),
                new UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>(
                    new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>>(instance), 
                    globalContext),
                new ProductSelectionGroupsMapper(new GetProductSelectionGroupsQuery(globalContext)));
            var productCollectionProcessor = new ProductCollectionProcessor(
                new NLogLoggerInstance<ProductCollectionProcessor>(instance),
                new Serializer<Contracts.items>(new NLogLoggerInstance<Serializer<Contracts.items>>(instance), emailClient),
                new GetItemsByIdQuery(globalContext),
                new GetFinancialClassByMerchandiseClassQuery(globalContext),
                new SaveToMessageHistoryCommandHandler(new NLogLoggerInstance<SaveToMessageHistoryCommandHandler>(instance), globalContext),
                new UpdateMessageHistoryStatusCommandHandler(new NLogLoggerInstance<UpdateMessageHistoryStatusCommandHandler>(instance), globalContext),
                producer);
            var saveToMessageHistoryCommandHandler = new SaveToMessageHistoryCommandHandler(
                new NLogLoggerInstance<SaveToMessageHistoryCommandHandler>(instance),
                globalContext);
            var associateMessageToQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>(new NLogLoggerInstance<AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>>(instance), globalContext);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>(
                new NLogLoggerInstance<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>>(instance), 
                globalContext);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(new NLogLoggerInstance<UpdateMessageHistoryStatusCommandHandler>(instance), globalContext);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>(
                new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>>(instance), 
                globalContext);

            var updateInProcessBusinessUnitCommandHandler = new UpdateInProcessBusinessUnitCommandHandler(globalContext);
            var clearBusinessUnitInProcessCommandHandler = new ClearBusinessUnitInProcessCommandHandler(globalContext);
            var getNextAvailableBusinessUnitQueryHandler = new GetNextAvailableBusinessUnitQuery(globalContext);
            var markQueuedEntriesAsInProcessCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>(new NLogLoggerInstance<MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>>(instance), globalContext);

            var monitorCommandHandler = new SaveMessageProcessorJobSummaryCommandHandler(
                new NLogLoggerInstance<SaveMessageProcessorJobSummaryCommandHandler>(instance), globalContext);
            var monitor = new MessageProcessorMonitor(monitorCommandHandler);

            var itemLocaleQueueProcessor = new ItemLocaleQueueProcessor(
                settings,
                queueProcessorLogger,
                globalContext,
                queueReader,
                serializer,
                productCollectionProcessor,
                saveToMessageHistoryCommandHandler,
                associateMessageToQueueCommandHandler,
                setProcessedDateCommandHandler,
                updateMessageHistoryCommandHandler,
                updateMessageQueueStatusCommandHandler,
                updateInProcessBusinessUnitCommandHandler,
                clearBusinessUnitInProcessCommandHandler,
                getNextAvailableBusinessUnitQueryHandler,
                markQueuedEntriesAsInProcessCommandHandler,
                producer,
                monitor);

            return new ApiControllerBase(baseLogger, emailClient, messageHistoryProcessor, itemLocaleQueueProcessor, producer);
        }
    }
}
