using Icon.ApiController.Common;
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
        ApiControllerBase IControllerBuilder.ComposeController()
        {
            ControllerType.Type = "ItemLocale";

            var instance = ControllerType.Instance.ToString();
            var baseLogger = new NLogLoggerInstance<ApiControllerBase>(instance);
            var emailClient = new EmailClient(EmailHelper.BuildEmailClientSettings());
            var producer = new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig("ItemQueueName"));
            var settings = ApiControllerSettings.CreateFromConfig("Icon", ControllerType.Instance);

            IconDbContextFactory iconContextFactory = new IconDbContextFactory();

            producer.OpenConnection();

            var messageHistoryProcessor = BuilderHelpers.BuildMessageHistoryProcessor(instance, MessageTypes.ItemLocale, producer, iconContextFactory);

            var queueProcessorLogger = new NLogLoggerInstance<ItemLocaleQueueProcessor>(instance);
            var serializer = new Serializer<Contracts.items>(
                new NLogLoggerInstance<Serializer<Contracts.items>>(instance),
                emailClient);
            var queueReader = new ItemLocaleQueueReader(
                new NLogLoggerInstance<ItemLocaleQueueReader>(instance),
                emailClient,
                new GetMessageQueueQuery<MessageQueueItemLocale>(new NLogLoggerInstance<GetMessageQueueQuery<MessageQueueItemLocale>>(instance), iconContextFactory),
                new GetItemByScanCodeQuery(
                    new NLogLoggerInstance<GetItemByScanCodeQuery>(instance),
                    iconContextFactory),
                new UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>(
                    new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>>(instance), 
                    iconContextFactory),
                new ProductSelectionGroupsMapper(new GetProductSelectionGroupsQuery(iconContextFactory)));
            var saveToMessageHistoryCommandHandler = new SaveToMessageHistoryCommandHandler(
                new NLogLoggerInstance<SaveToMessageHistoryCommandHandler>(instance),
                iconContextFactory);
            var associateMessageToQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>(new NLogLoggerInstance<AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>>(instance), iconContextFactory);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>(
                new NLogLoggerInstance<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>>(instance), 
                iconContextFactory);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(new NLogLoggerInstance<UpdateMessageHistoryStatusCommandHandler>(instance), iconContextFactory);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>(
                new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>>(instance), 
                iconContextFactory);

            var updateInProcessBusinessUnitCommandHandler = new UpdateInProcessBusinessUnitCommandHandler(iconContextFactory);
            var clearBusinessUnitInProcessCommandHandler = new ClearBusinessUnitInProcessCommandHandler(iconContextFactory);
            var getNextAvailableBusinessUnitQueryHandler = new GetNextAvailableBusinessUnitQuery(iconContextFactory);
            var markQueuedEntriesAsInProcessCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>(new NLogLoggerInstance<MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>>(instance), iconContextFactory);

            var monitorCommandHandler = new SaveMessageProcessorJobSummaryCommandHandler(
                new NLogLoggerInstance<SaveMessageProcessorJobSummaryCommandHandler>(instance), iconContextFactory);
            var monitor = new MessageProcessorMonitor(monitorCommandHandler);

            var itemLocaleQueueProcessor = new ItemLocaleQueueProcessor(
                settings,
                queueProcessorLogger,
                queueReader,
                serializer,
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
