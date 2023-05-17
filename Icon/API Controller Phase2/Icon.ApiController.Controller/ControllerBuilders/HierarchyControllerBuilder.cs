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
using Icon.ActiveMQ;
using Icon.ActiveMQ.Producer;

namespace Icon.ApiController.Controller.ControllerBuilders
{
    public class HierarchyControllerBuilder : IControllerBuilder
    {
        public ApiControllerBase ComposeController()
        {
            ControllerType.Type = "Hierarchy";

            var instance = ControllerType.Instance.ToString();
            var baseLogger = new NLogLoggerInstance<ApiControllerBase>(instance);
            baseLogger.Info("Running HierarchyControllerBuilder.ComposeController");

            var emailClient = new EmailClient(EmailHelper.BuildEmailClientSettings());
            var activeMqProducer = new ActiveMQProducer(ActiveMQConnectionSettings.CreateSettingsFromConfig("ActiveMqHierarchyQueueName"));
            var settings = ApiControllerSettings.CreateFromConfig("Icon", ControllerType.Instance);
            var computedClientId = $"{settings.Source}ApiController.Type-{settings.ControllerType}.{Environment.MachineName}.{Guid.NewGuid().ToString()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));


            IconDbContextFactory iconContextFactory = new IconDbContextFactory();

            baseLogger.Info("Opening ActiveMQ Connection");
            activeMqProducer.OpenConnection(clientId);
            baseLogger.Info("ActiveMQ Connection Opened");

            var messageHistoryProcessor = BuilderHelpers.BuildMessageHistoryProcessor(instance, MessageTypes.Hierarchy, iconContextFactory);

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
                monitor,
                activeMqProducer);

            return new ApiControllerBase(baseLogger, emailClient, messageHistoryProcessor, hierarchyQueueProcessor);
        }
    }
}
