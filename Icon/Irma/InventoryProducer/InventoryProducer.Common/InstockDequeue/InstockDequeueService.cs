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
using System.Text;

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

        /*public List<String> getDequeuedMessages() 
        {
            bool eventsExist = true;
            while (eventsExist)
            {
                IrmaEvents dequeuedEvents = dequeueEvents.dequeueEventsFromIrma();
                if (dequeuedEvents.numberOfRecordsDequeued == 0)
                {
                    eventsExist = false;
                } else
                {
                    dequeuedEvents.resultsSet = dequeuedEvents.resultsSet.OrderBy(dequeuedEvent => dequeuedEvent.EventTypeCode).ToList();
                    List<String> uniqueMessageTypes = dequeuedEvents.resultsSet.Select(dequeuedEvent => dequeuedEvent.MessageType).Distinct().ToList();
                    archiveData(dequeuedEvents.resultsSet);
                }
            }
            return new List<String>();
        }*/

        public List<InstockDequeueResult> GetDequeuedMessages()
        {
            List<InstockDequeueModel> dequeuedEvents = null;
            try
            {
                dequeuedEvents = dequeueEvents.DequeueEventsFromIrma();
                if (dequeuedEvents.Count > 0)
                {
                    dequeuedEvents = dequeuedEvents.OrderBy(dequeuedEvent => dequeuedEvent.EventTypeCode).ToList();
                    return ArchiveData(dequeuedEvents, null);
                }
            } catch (Exception ex)
            {
                Dictionary<String, String> messageProperties = new Dictionary<String, String>();
                string errorMessage = "A failure occurred when parsing dequeue results for " + settings.TransactionType + " type events.";
                if (dequeuedEvents != null && dequeuedEvents.Count > 0)
                {
                    try
                    {
                        ArchiveData(dequeuedEvents, ex);
                    } catch (Exception exception)
                    {
                        // nothing to do. Already taken care in the respective method.
                    }
                    errorMessage = "A failure occurred when dequeuing " + settings.TransactionType + " type events. List of KeyIDs: " + dequeuedEvents.Select(dequeuedEvent => dequeuedEvent.KeyID).ToList().ToString();
                }
                inventoryLogger.LogError(errorMessage + " Error Message: " + ex.Message, ex.StackTrace);
                messageProperties["TransactionType"] = settings.TransactionType;
                PublishErrorEvents.SendToMammoth(
                        mammothContextFactory,
                        settings.TransactionType,
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
                String messageID = Guid.NewGuid().ToString();
                Dictionary<String, String> headers = GenerateHeaders(messageID, dequeuedEvent);
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
                        new StringBuilder()
                        .AppendFormat("Failed to archive Event Message for ")
                        .AppendFormat("KeyID: {0}, ", dequeuedEvent.KeyID)
                        .AppendFormat("SecondaryID: {0}, ", dequeuedEvent.SecondaryKeyID)
                        .AppendFormat("MessageType: {0}, ", dequeuedEvent.MessageType)
                        .AppendFormat("EventTypeCode: {0}, ", dequeuedEvent.EventTypeCode)
                        .AppendFormat("InsertDate: {0 } and ", dequeuedEvent.InsertDate)
                        .AppendFormat("MessageTimestampUtc: {0}. ", dequeuedEvent.MessageTimestampUtc)
                        .AppendFormat("Error Message: {0}.", ex.Message)
                        .ToString()
                        , ex.StackTrace);
                    throw ex;
                }
            }
            return instockDequeueResults;
        }

        private Dictionary<String, String> GenerateHeaders(string messageID, InstockDequeueModel result)
        {
            Dictionary<String, String> headers = new Dictionary<String, String>();
            headers["TransactionID"] = messageID;
            headers["TransactionType"] = settings.TransactionType;
            headers["Source"] = "IRMA";
            headers["MessageType"] = result.MessageType;
            headers["MessageNumber"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            headers["RegionCode"] = settings.RegionCode;
            return headers;
        }
    }
}
