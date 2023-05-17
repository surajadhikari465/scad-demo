using System;
using Icon.ApiController.Common;
using Icon.ApiController.Controller.Monitoring;
using Icon.ApiController.Controller.QueueProcessors;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Controller.ControllerBuilders
{
    public class PriceControllerBuilder : IControllerBuilder
    {
        public ApiControllerBase ComposeController()
        {
            ControllerType.Type = "Price";

            var instance = ControllerType.Instance.ToString();
            var baseLogger = new NLogLoggerInstance<ApiControllerBase>(instance);
            baseLogger.Info("Running PriceControllerBuilder.ComposeController");

            var emailClient = new EmailClient(EmailHelper.BuildEmailClientSettings());
            var settings = ApiControllerSettings.CreateFromConfig("Icon", ControllerType.Instance);
            var computedClientId = $"{settings.Source}ApiController.Type-{settings.ControllerType}.{Environment.MachineName}.{Guid.NewGuid().ToString()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));

            IconDbContextFactory iconContextFactory = new IconDbContextFactory();

            var messageHistoryProcessor = BuilderHelpers.BuildMessageHistoryProcessor(instance, MessageTypes.Price, iconContextFactory);

            var queueProcessorLogger = new NLogLoggerInstance<PriceQueueProcessor>(instance);
            var serializer = new Serializer<Contracts.items>(
                new NLogLoggerInstance<Serializer<Contracts.items>>(instance),
                emailClient);
            var queueReader = new PriceQueueReader(
                new NLogLoggerInstance<PriceQueueReader>(instance),
                emailClient,
                new GetMessageQueueQuery<MessageQueuePrice>(
                    new NLogLoggerInstance<GetMessageQueueQuery<MessageQueuePrice>>(instance),
                    iconContextFactory),
                new UpdateMessageQueueStatusCommandHandler<MessageQueuePrice>(
                    new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueuePrice>>(instance), 
                    iconContextFactory));
            var saveToMessageHistoryCommandHandler = new SaveToMessageHistoryCommandHandler(
                new NLogLoggerInstance<SaveToMessageHistoryCommandHandler>(instance),
                iconContextFactory);
            var associateMessageToQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueuePrice>(
                new NLogLoggerInstance<AssociateMessageToQueueCommandHandler<MessageQueuePrice>>(instance),
                iconContextFactory);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueuePrice>(
                new NLogLoggerInstance<UpdateMessageQueueProcessedDateCommandHandler<MessageQueuePrice>>(instance), 
                iconContextFactory);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(
                new NLogLoggerInstance<UpdateMessageHistoryStatusCommandHandler>(instance), 
                iconContextFactory);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueuePrice>(
                new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueuePrice>>(instance), 
                iconContextFactory);

            var updateInProcessBusinessUnitCommandHandler = new UpdateInProcessBusinessUnitCommandHandler(iconContextFactory);
            var clearBusinessUnitInProcessCommandHandler = new ClearBusinessUnitInProcessCommandHandler(iconContextFactory);
            var getNextAvailableBusinessUnitQueryHandler = new GetNextAvailableBusinessUnitQuery(iconContextFactory);
            var markQueuedEntriesAsInProcessCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueuePrice>(
                    new NLogLoggerInstance<MarkQueuedEntriesAsInProcessCommandHandler<MessageQueuePrice>>(instance),
                    iconContextFactory);

            var monitorCommandHandler = new SaveMessageProcessorJobSummaryCommandHandler(
                new NLogLoggerInstance<SaveMessageProcessorJobSummaryCommandHandler>(instance), iconContextFactory);
            var monitor = new MessageProcessorMonitor(monitorCommandHandler);

            var priceQueueProcessor = new PriceQueueProcessor(
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
                monitor);

            return new ApiControllerBase(baseLogger, emailClient, messageHistoryProcessor, priceQueueProcessor);
        }
    }
}
