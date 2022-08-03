using Icon.DbContextFactory;
using InventoryProducer.Common.InstockDequeue.Model.DBModel;
using InventoryProducer.Common.InstockDequeue.Schemas;
using InventoryProducer.Common.Serializers;
using Irma.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace InventoryProducer.Common.InstockDequeue
{
    public class ArchiveInstockDequeueEvents
    {
        private readonly IDbContextFactory<IrmaContext> irmaContextFactory;
        private readonly InventoryProducerSettings settings;
        private readonly ISerializer<EventTypes> instockDequeueSerializer;

        public ArchiveInstockDequeueEvents(IDbContextFactory<IrmaContext> irmaContextFactory, InventoryProducerSettings settings, ISerializer<EventTypes> instockDequeueSerializer)
        {
            this.irmaContextFactory = irmaContextFactory;
            this.settings = settings;
            this.instockDequeueSerializer = instockDequeueSerializer;
        }

        public void Archive(InstockDequeueModel instockDequeueModel, Dictionary<String, String> headers, String errorCode, String errorDetails)
        {
            using (var irmaContext = irmaContextFactory.CreateContext("Irma_" + settings.RegionCode))
            {
                irmaContext.Database.CommandTimeout = settings.DbCommandTimeoutInSeconds;
                StringBuilder archiveSqlStatementBuilder = 
                    new StringBuilder()
                    .Append(@"INSERT INTO amz.MessageArchiveEvent")
                    .Append("(KeyID, SecondaryKeyID, MessageType, EventTypeCode, InsertDate, MessageTimestampUtc, MessageID, MessageHeaders, Message, ErrorCode, ErrorDetails)")
                    .Append("VALUES")
                    .Append("(@KeyID, @SecondaryKeyID, @MessageType, @EventTypeCode, @InsertDate, @MessageTimestampUtc, @MessageID, @MessageHeaders, @Message, @ErrorCode, @ErrorDetails)");
                irmaContext
                    .Database
                    .ExecuteSqlCommand(
                    archiveSqlStatementBuilder.ToString(), 
                    new SqlParameter("@KeyID", instockDequeueModel.KeyID),
                    new SqlParameter("@SecondaryKeyID", (object) instockDequeueModel.SecondaryKeyID ?? DBNull.Value),
                    new SqlParameter("@MessageType", instockDequeueModel.MessageType),
                    new SqlParameter("@EventTypeCode", instockDequeueModel.EventTypeCode),
                    new SqlParameter("@InsertDate", instockDequeueModel.InsertDate),
                    new SqlParameter("@MessageTimestampUtc", instockDequeueModel.MessageTimestampUtc),
                    new SqlParameter("@MessageID", headers["TransactionID"]),
                    new SqlParameter("@MessageHeaders", JsonConvert.SerializeObject(new Dictionary<String, Dictionary<String, String>>() {{"MessageHeaders", headers}}, Formatting.Indented)), 
                    new SqlParameter(
                        "@Message", 
                        instockDequeueSerializer
                        .Serialize(
                            ConvertToEventTypesCanonical(instockDequeueModel), new Utf8StringWriter()
                            )
                        .Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "") // DB persist doesn't support utf-8 encoding
                        ),
                    new SqlParameter("@ErrorCode", (object) errorCode ?? DBNull.Value), 
                    new SqlParameter("@ErrorDetails", (object) errorDetails ?? DBNull.Value)
                    );
            }
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
            if (instockDequeueModel.SecondaryKeyID != null)
            {
                eventTypesCanonical.EventType[0].SecondaryKeyID = (int)instockDequeueModel.SecondaryKeyID;
                eventTypesCanonical.EventType[0].SecondaryKeyIDSpecified = true;
            }
            return eventTypesCanonical;
        }
    }
}
