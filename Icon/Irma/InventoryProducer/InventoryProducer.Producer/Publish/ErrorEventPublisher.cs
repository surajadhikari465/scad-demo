using Icon.DbContextFactory;
using InventoryProducer.Common.InstockDequeue;
using InventoryProducer.Common.InstockDequeue.Model;
using InventoryProducer.Common.InstockDequeue.Schemas;
using InventoryProducer.Common.Helpers;
using InventoryProducer.Common.Serializers;
using Mammoth.Framework;
using System;

namespace InventoryProducer.Producer.Publish
{
    public class ErrorEventPublisher: IErrorEventPublisher
    {
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly ISerializer<EventTypes> instockDequeueSerializer;
        private readonly string applicationName;

        public ErrorEventPublisher(IDbContextFactory<MammothContext> mammothContextFactory, 
            ISerializer<EventTypes> instockDequeueSerializer, string applicationName)
        {
            this.mammothContextFactory = mammothContextFactory;
            this.instockDequeueSerializer = instockDequeueSerializer;
            this.applicationName = applicationName;
        }

        public void PublishErrorEventToMammoth(InstockDequeueResult instockDequeueResult, Exception exception)
        {
            string instockDequeueModelXmlPayload =
                this.instockDequeueSerializer.Serialize(
                    ArchiveInstockDequeueEvents.ConvertToEventTypesCanonical(instockDequeueResult.InstockDequeueModel),
                    new Utf8StringWriter()
                ).Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");

            PublishErrorEvents.SendToMammoth(
                this.mammothContextFactory,
                this.applicationName,
                instockDequeueResult.Headers["MessageNumber"],
                instockDequeueResult.Headers,
                instockDequeueModelXmlPayload,
                "Data Issue",
                exception.StackTrace,
                "Fatal"
            );
        }
    }
}
