using Icon.ApiController.Common;
using Icon.ApiController.Controller;
using Icon.ApiController.Controller.HistoryProcessors;
using Icon.ApiController.Controller.QueueProcessors;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.ActiveMQ;
using Icon.ActiveMQ.Producer;
using Icon.Logging;
using Mammoth.ApiController.HistoryProcessors;
using Mammoth.ApiController.QueueProcessors;
using Mammoth.ApiController.QueueReaders;
using Mammoth.Framework;
using SimpleInjector;
using System;
using System.Collections.Generic;
using Contracts = Icon.Esb.Schemas.Wfm.PreGpm.Contracts;
using MammothDataAccess = Mammoth.ApiController.DataAccess;

namespace Mammoth.ApiController
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer(int instance, string controllerType)
        {
            var container = new Container();

            container.Register<ILogger<Serializer<Contracts.items>>, NLogLogger<Serializer<Contracts.items>>>();
            container.RegisterSingleton<IDbContextFactory<MammothContext>, MammothContextFactory>();
            container.RegisterSingleton(() => ApiControllerSettings.CreateFromConfig("Mammoth", instance));
            RegisterActiveMqProducer(container, controllerType, instance);
            container.Register<IQueueReader<MessageQueuePrice, Contracts.items>, MammothPriceQueueReader>();
            container.Register<IQueueReader<MessageQueueItemLocale, Contracts.items>, MammothItemLocaleQueueReader>();
            container.RegisterSingleton<ILogger>(() => new NLogLoggerSingleton(typeof(NLogLoggerSingleton)));
            container.Register<ILogger<ApiControllerBase>>(() => new NLogLoggerInstance<ApiControllerBase>(instance.ToString()));
            container.Register<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.Register<IHistoryProcessor, MammothMessageHistoryProcessor>();
            container.Register<ISerializer<Contracts.items>, Serializer<Contracts.items>>();
            container.Register<ApiControllerBase>();
            
            container.Register<ICommandHandler<AssociateMessageToQueueCommand<MessageQueuePrice, MessageHistory>>, MammothDataAccess.Commands.AssociateMessageToQueueCommandHandler<MessageQueuePrice>>();
            container.Register<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueuePrice>>, MammothDataAccess.Commands.MarkQueuedEntriesAsInProcessCommandHandler<MessageQueuePrice>>();
            container.Register<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueuePrice>>, MammothDataAccess.Commands.UpdateMessageQueueProcessedDateCommandHandler<MessageQueuePrice>>();
            container.Register<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueuePrice>>, MammothDataAccess.Commands.UpdateMessageQueueStatusCommandHandler<MessageQueuePrice>>();
            container.Register<IQueryHandler<GetMessageQueueParameters<MessageQueuePrice>, List<MessageQueuePrice>>, MammothDataAccess.Queries.GetMessageQueueQuery<MessageQueuePrice>>();

            container.Register<ICommandHandler<AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>>, MammothDataAccess.Commands.AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>>();
            container.Register<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>>, MammothDataAccess.Commands.MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>>();
            container.Register<ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>>, MammothDataAccess.Commands.UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>>();
            container.Register<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>>, MammothDataAccess.Commands.UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>>();
            container.Register<IQueryHandler<GetMessageQueueParameters<MessageQueueItemLocale>, List<MessageQueueItemLocale>>, MammothDataAccess.Queries.GetMessageQueueQuery<MessageQueueItemLocale>>();

            container.Register<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>, MammothDataAccess.Commands.SaveToMessageHistoryCommandHandler>();
            container.Register<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>, MammothDataAccess.Commands.UpdateMessageHistoryStatusCommandHandler>();

            RegisterQueueProcessorImplementation(container, controllerType, instance);


            return container;
        }

        private static void RegisterQueueProcessorImplementation(Container container, string controllerType, int instance)
        {
            switch (controllerType)
            {
                case "i":
                    container.Register<IQueueProcessor, MammothItemLocaleQueueProcessor>();
                    break;
                case "r":
                    container.Register<IQueueProcessor, MammothPriceQueueProcessor>();
                    break;
                default:
                    throw new ArgumentException(
                        $"No type implementation exists for controller type argument {controllerType}");
            }
        }

        private static void RegisterActiveMqProducer(Container container, string controllerType, int instance)
        {
            switch (controllerType)
            {
                case "i":
                    // Item Locale
                    container.Register<IActiveMQProducer>(
                        () => new ActiveMQProducer(ActiveMQConnectionSettings.CreateSettingsFromConfig("ActiveMqItemLocaleQueueName")), Lifestyle.Singleton);
                    break;
                case "r":
                    // Legacy Price
                    container.Register<IActiveMQProducer>(
                        () => new ActiveMQProducer(ActiveMQConnectionSettings.CreateSettingsFromConfig("ActiveMqLegacyPriceQueueName")), Lifestyle.Singleton);
                    break;
            }
        }
    }
}
