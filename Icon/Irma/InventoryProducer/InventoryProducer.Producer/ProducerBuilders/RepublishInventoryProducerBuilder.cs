using Icon.Logging;
using InventoryProducer.Common;
using InventoryProducer.Producer.QueueProcessors;
using Irma.Framework;
using Mammoth.Framework;
using System;
using Icon.DbContextFactory;
using Icon.ActiveMQ.Producer;
using Icon.ActiveMQ;
using InventoryProducer.Producer.Publish;
using InventoryProducer.Producer.DataAccess;

namespace InventoryProducer.Producer.ProducerBuilders
{
    internal class RepublishInventoryProducerBuilder: IProducerBuilder
    {
        public InventoryProducerBase ComposeProducer(InventoryProducerSettings inventoryProducerSettings)
        {
            var instance = ProducerType.Instance.ToString();
            InventoryProducerSettings settings = InventoryProducerSettings.CreateFromConfig("IRMA", ProducerType.Instance);
            IDbContextFactory<IrmaContext> irmaContextFactory = new IrmaDbContextFactory();
            IDbContextFactory<MammothContext> mammothContextFactory = new MammothContextFactory();

            var activeMqProducer = new ActiveMQDynamicProducer(ActiveMQConnectionSettings.CreateSettingsFromConfig());

            var computedClientId = $"{settings.Source}InventoryProducer.Type-{settings.ProducerType}.{Environment.MachineName}.{Guid.NewGuid()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));

            var baseLogger = new InventoryLogger<InventoryProducerBase>(new NLogLoggerInstance<InventoryProducerBase>(instance), irmaContextFactory, settings);
            baseLogger.LogInfo("Running RepublishInventoryProducerBuilder.ComposeProducer");

            baseLogger.LogInfo("Opening ActiveMQ Connection");
            activeMqProducer.OpenConnection(clientId);
            baseLogger.LogInfo("ActiveMQ Connection Opened");

            var messagePublisher = new RepublishInventoryMessagePublisher(activeMqProducer);
            var republishInventoryDal = new RepublishInventoryDAL(irmaContextFactory, settings);

            var queueProcessorLogger = new InventoryLogger<RepublishInventoryQueueProcessor>(
                new NLogLoggerInstance<RepublishInventoryQueueProcessor>(instance),
                irmaContextFactory,
                settings
            );

            var republishInventoryQueueProcessor = new RepublishInventoryQueueProcessor(
                settings,
                republishInventoryDal,
                messagePublisher,
                mammothContextFactory,
                queueProcessorLogger
            );

            return new InventoryProducerBase(baseLogger, republishInventoryQueueProcessor);
        }
    }
}
