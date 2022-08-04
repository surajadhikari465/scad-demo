using Icon.DbContextFactory;
using InventoryProducer.Common.InstockDequeue.Model.DBModel;
using InventoryProducer.Common.InstockDequeue.Schemas;
using InventoryProducer.Common.Serializers;
using Irma.Framework;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace InventoryProducer.Common.InstockDequeue
{
    public class ArchiveInstockDequeueEvents
    {
        private readonly IDbContextFactory<IrmaContext> irmaContextFactory;
        private readonly InventoryProducerSettings settings;
        private readonly ISerializer<EventTypes> instockDequeueSerializer;
        private readonly RetryPolicy retrypolicy;

        public ArchiveInstockDequeueEvents(IDbContextFactory<IrmaContext> irmaContextFactory, InventoryProducerSettings settings, ISerializer<EventTypes> instockDequeueSerializer)
        {
            this.irmaContextFactory = irmaContextFactory;
            this.settings = settings;
            this.instockDequeueSerializer = instockDequeueSerializer;
            this.retrypolicy = RetryPolicy
                .Handle<Exception>()
                .WaitAndRetry(
                settings.DbErrorRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(settings.DbRetryDelayInMilliseconds)
                );
        }

        public void Archive(InstockDequeueModel instockDequeueModel, Dictionary<string, string> headers, string errorCode, string errorDetails)
        {
            using (var irmaContext = irmaContextFactory.CreateContext($"Irma_{settings.RegionCode}"))
            {
                irmaContext.Database.CommandTimeout = settings.DbCommandTimeoutInSeconds;
                string archiveSqlStatement = @"INSERT INTO amz.MessageArchiveEvent 
                    (KeyID, SecondaryKeyID, MessageType, EventTypeCode, InsertDate, MessageTimestampUtc, MessageID, MessageHeaders, Message, ErrorCode, ErrorDetails) 
                    VALUES 
                    (@KeyID, @SecondaryKeyID, @MessageType, @EventTypeCode, @InsertDate, @MessageTimestampUtc, @MessageID, @MessageHeaders, @Message, @ErrorCode, @ErrorDetails)";
                this.retrypolicy.Execute(() => InsertInDB(instockDequeueModel, headers, errorCode, errorDetails, irmaContext, archiveSqlStatement));
            }
        }

        private void InsertInDB(InstockDequeueModel instockDequeueModel, Dictionary<string, string> headers, string errorCode, string errorDetails, IrmaContext irmaContext, string archiveSqlStatement)
        {
            irmaContext
                .Database
                .ExecuteSqlCommand(
                archiveSqlStatement,
                new SqlParameter("@KeyID", instockDequeueModel.KeyID),
                new SqlParameter("@SecondaryKeyID", (object)instockDequeueModel.SecondaryKeyID ?? DBNull.Value),
                new SqlParameter("@MessageType", instockDequeueModel.MessageType),
                new SqlParameter("@EventTypeCode", instockDequeueModel.EventTypeCode),
                new SqlParameter("@InsertDate", instockDequeueModel.InsertDate),
                new SqlParameter("@MessageTimestampUtc", instockDequeueModel.MessageTimestampUtc),
                new SqlParameter("@MessageID", headers["TransactionID"]),
                new SqlParameter("@MessageHeaders", JsonConvert.SerializeObject(new Dictionary<string, Dictionary<string, string>>() { { "MessageHeaders", headers } }, Formatting.Indented)),
                new SqlParameter(
                    "@Message",
                    instockDequeueSerializer
                    .Serialize(
                        ConvertToEventTypesCanonical(instockDequeueModel), new Utf8StringWriter()
                        )
                    .Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "") // DB persist doesn't support utf-8 encoding
                    ),
                new SqlParameter("@ErrorCode", (object)errorCode ?? DBNull.Value),
                new SqlParameter("@ErrorDetails", (object)errorDetails ?? DBNull.Value)
                );
        }

        public static EventTypes ConvertToEventTypesCanonical(InstockDequeueModel instockDequeueModel)
        {
            EventTypes eventTypesCanonical = new EventTypes
            {
                EventType = new EventType[1]
            };
            eventTypesCanonical.EventType[0] = new EventType
            {
                QueueID = instockDequeueModel.QueueID,
                EventTypeCode = instockDequeueModel.EventTypeCode,
                MessageType = instockDequeueModel.MessageType,
                KeyID = instockDequeueModel.KeyID,
                InsertDate = instockDequeueModel.InsertDate.ToString("O"),
                MessageTimestampUtc = instockDequeueModel.MessageTimestampUtc.ToString("O"),
            };
            if (instockDequeueModel.SecondaryKeyID.HasValue)
            {
                eventTypesCanonical.EventType[0].SecondaryKeyID = instockDequeueModel.SecondaryKeyID.Value;
                eventTypesCanonical.EventType[0].SecondaryKeyIDSpecified = true;
            }
            return eventTypesCanonical;
        }
    }
}
