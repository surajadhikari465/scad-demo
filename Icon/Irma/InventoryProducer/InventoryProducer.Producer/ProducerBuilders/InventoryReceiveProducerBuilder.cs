using System;
using Icon.Esb;
using Icon.Esb.Producer;
using Icon.ActiveMQ;
using Icon.ActiveMQ.Producer;
using Icon.Logging;
using Irma.Framework;
using InventoryProducer.Common;
using InventoryProducer.Producer.QueueProcessors;
using InventoryProducer.Producer.DataAccess;
using InventoryProducer.Producer.Helpers;
using InventoryProducer.Producer.Mapper;
using InventoryProducer.Producer.Publish;
using InventoryProducer.Common.InstockDequeue;
using Mammoth.Framework;
using OrderReceipts = Icon.Esb.Schemas.Wfm.Contracts.orderReceipts;
using InventoryProducer.Common.Serializers;
using Icon.Esb.Schemas.Wfm.Contracts;
using InventoryProducer.Common.InstockDequeue.Schemas;
using Icon.DbContextFactory;
using InventoryProducer.Producer.Model.DBModel;

namespace InventoryProducer.Producer.ProducerBuilders
{
    public class InventoryReceiveProducerBuilder : ProducerBuilder<OrderReceipts, ReceiveModel, InventoryReceiveQueueProcessor>
    {
        protected override string ActiveMQQueueConfigName { get; } = "ActiveMqInventoryReceiveQueueName";
        protected override string ApplicationName { get; } = "PublishReceiveService";

        public override IQueueProcessor CreateQueueProcessor(
            InventoryProducerSettings settings,
            InventoryLogger<QueueProcessor<OrderReceipts, ReceiveModel>> logger,
            InstockDequeueService instockDequeueService,
            MessagePublisher messagePublisher,
            ArchiveInventoryEvents archiveInventoryEvents,
            ErrorEventPublisher errorEventPublisher,
            IDbContextFactory<IrmaContext> irmaContext,
            ISerializer<OrderReceipts> serializer
            )
        {
            var receiveDAL = new ReceiveDAL(irmaContext, settings);
            var receiveXmlCanonicalMapper = new ReceiveXmlCanonicalMapper(
                serializer
            );
            return new InventoryReceiveQueueProcessor(
                settings,
                logger,
                instockDequeueService,
                receiveDAL,
                receiveXmlCanonicalMapper,
                messagePublisher,
                archiveInventoryEvents,
                errorEventPublisher
            );
        }
    }
}
