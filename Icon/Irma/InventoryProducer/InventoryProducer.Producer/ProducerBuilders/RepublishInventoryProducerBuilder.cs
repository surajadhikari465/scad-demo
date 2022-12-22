using Icon.ActiveMQ.Producer;
using Icon.ActiveMQ;
using Icon.Esb.Producer;
using Icon.Esb;
using Icon.Logging;
using InventoryProducer.Common;
using InventoryProducer.Producer.QueueProcessors;
using Irma.Framework;
using Mammoth.Framework;
using System;
using Icon.DbContextFactory;

namespace InventoryProducer.Producer.ProducerBuilders
{
    internal class RepublishInventoryProducerBuilder: IProducerBuilder
    {
        public InventoryProducerBase ComposeProducer(InventoryProducerSettings inventoryProducerSettings)
        {
            string applicationName = "RepublishInventoryMessagesService";
            var instance = ProducerType.Instance.ToString();
            InventoryProducerSettings settings = InventoryProducerSettings.CreateFromConfig("IRMA", ProducerType.Instance);
            IDbContextFactory<IrmaContext> irmaContextFactory = new IrmaDbContextFactory();
            IDbContextFactory<MammothContext> mammothContextFactory = new MammothContextFactory();

            var producer = new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig("InventoryTopicName"));

            var computedClientId = $"{settings.Source}InventoryProducer.Type-{settings.ProducerType}.{Environment.MachineName}.{Guid.NewGuid()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));

            var baseLogger = new InventoryLogger<InventoryProducerBase>(new NLogLoggerInstance<InventoryProducerBase>(instance), irmaContextFactory, settings);
            baseLogger.LogInfo("Running RepublishInventoryProducerBuilder.ComposeProducer");

            baseLogger.LogInfo("Opening ESB Connection");
            producer.OpenConnection(clientId);
            baseLogger.LogInfo("ESB Connection Opened");

            var queueProcessorLogger = new InventoryLogger<InventoryTransferQueueProcessor>(
                new NLogLoggerInstance<InventoryTransferQueueProcessor>(instance),
                irmaContextFactory,
                settings
            );

            var republishInventoryQueueProcessor = new RepublishInventoryQueueProcessor();

            return new InventoryProducerBase(baseLogger, republishInventoryQueueProcessor);
        }
    }
}
