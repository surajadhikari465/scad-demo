using InventoryProducer.Common.InstockDequeue.Model;
using System;
using System.Collections.Generic;
using Irma.Framework;
using Icon.DbContextFactory;
using System.Linq;
using InventoryProducer.Common.InstockDequeue.Model.DBModel;
using Mammoth.Framework;
using InventoryProducer.Common.InstockDequeue.Schemas;
using InventoryProducer.Common.Serializers;
using InventoryProducer.Common.Helpers;

namespace InventoryProducer.Common.InstockDequeue
{
    public class InstockDequeueService
    {
        private readonly InventoryProducerSettings settings;
        private readonly IDbContextFactory<IrmaContext> irmaContextFactory;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly DequeueEvents dequeueEvents;
        private readonly ISerializer<EventTypes> instockDequeueSerializer;
        private readonly InventoryLogger<InstockDequeueService> inventoryLogger;

        public InstockDequeueService(
            InventoryProducerSettings settings, 
            IDbContextFactory<IrmaContext> irmaContextFactory, 
            IDbContextFactory<MammothContext> mammothContextFactory, 
            DequeueEvents dequeueEvents,
            ISerializer<EventTypes> instockDequeueSerializer,
            InventoryLogger<InstockDequeueService> inventoryLogger
            )
        {
            this.settings = settings;
            this.irmaContextFactory = irmaContextFactory;
            this.mammothContextFactory = mammothContextFactory;
            this.dequeueEvents = dequeueEvents;
            this.instockDequeueSerializer = instockDequeueSerializer;
            this.inventoryLogger = inventoryLogger;
        }

        public List<InstockDequeueResult> GetDequeuedMessages()
        {
            List<InstockDequeueModel> dequeuedEvents = null;
            try
            {
                dequeuedEvents = dequeueEvents.DequeueEventsFromIrma();
                if (dequeueEvents != null && dequeuedEvents.Count > 0)
                {
                    dequeuedEvents = dequeuedEvents.OrderBy(dequeuedEvent => dequeuedEvent.EventTypeCode).ToList();
                    return ArchiveData(dequeuedEvents, null);
                }
            } catch (Exception ex)
            {
                Dictionary<string, string> messageProperties = new Dictionary<string, string>()
                {
                    ["TransactionType"] = settings.TransactionType
                };
                string errorMessage = $"A failure occurred when parsing dequeue results for {settings.TransactionType} type events";
                if (dequeuedEvents != null && dequeuedEvents.Count > 0)
                {
                    try
                    {
                        ArchiveData(dequeuedEvents, ex);
                    } catch (Exception exception)
                    {
                        // nothing to do. Already taken care in the respective method.
                    }
                    errorMessage = $"A failure occurred when dequeuing {settings.TransactionType} type events. List of KeyIDs: {string.Join(", ", dequeuedEvents.Select(dequeuedEvent => dequeuedEvent.KeyID).ToList())}";
                }
                inventoryLogger.LogError($"{errorMessage}. Error Message: {ex.Message}", ex.StackTrace);
                PublishErrorEvents.SendToMammoth(
                        mammothContextFactory,
                        $"{settings.TransactionType}InStockDequeueService",
                        "N/A",
                        messageProperties,
                        errorMessage,
                        "Data Issue",
                        ex.StackTrace,
                        "Fatal"
                        );
            }
            return new List<InstockDequeueResult>();
        }

        private List<InstockDequeueResult> ArchiveData(List<InstockDequeueModel> dequeuedEvents, Exception originalException)
        {
            ArchiveInstockDequeueEvents archiveInstockDequeueEvents = new ArchiveInstockDequeueEvents(irmaContextFactory, settings, instockDequeueSerializer);
            List<InstockDequeueResult> instockDequeueResults = new List<InstockDequeueResult>();
            foreach (InstockDequeueModel dequeuedEvent in dequeuedEvents)
            {
                string messageID = Guid.NewGuid().ToString();
                Dictionary<string, string> headers = GenerateHeaders(messageID, dequeuedEvent);
                instockDequeueResults.Add(new InstockDequeueResult(dequeuedEvent, headers));
                try
                {
                    if (originalException == null)
                    {
                        archiveInstockDequeueEvents.Archive(dequeuedEvent, headers, null, null);
                    }
                    else
                    {
                        archiveInstockDequeueEvents.Archive(dequeuedEvent, headers, null, originalException.ToString());
                    }
                } catch (Exception ex)
                {
                    inventoryLogger.LogError(
                        $@"Failed to archive Event Message for 
                        KeyID: ${dequeuedEvent.KeyID}, 
                        SecondaryID: {dequeuedEvent.SecondaryKeyID}, 
                        MessageType: {dequeuedEvent.MessageType}, 
                        EventTypeCode: {dequeuedEvent.EventTypeCode}, 
                        InsertDate: {dequeuedEvent.InsertDate}, 
                        MessageTimestampUtc: {dequeuedEvent.MessageTimestampUtc}, 
                        Error Message: {ex.Message}"
                        , ex.StackTrace);
                    throw ex;
                }
            }
            return instockDequeueResults;
        }

        private Dictionary<string, string> GenerateHeaders(string messageID, InstockDequeueModel result)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                ["TransactionID"] = messageID,
                ["TransactionType"] = settings.TransactionType,
                ["Source"] = "IRMA",
                ["MessageType"] = result.MessageType,
                ["MessageNumber"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                ["RegionCode"] = settings.RegionCode
            };
            return headers;
        }
    }
}
