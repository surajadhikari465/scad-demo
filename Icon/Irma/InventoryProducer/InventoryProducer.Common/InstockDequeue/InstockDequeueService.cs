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
    public class InstockDequeueService : IInstockDequeueService
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

        public IList<InstockDequeueResult> GetDequeuedMessages()
        {
            List<InstockDequeueModel> dequeuedEvents = new List<InstockDequeueModel>();
            List<InstockDequeueResult> instockDequeueResults = new List<InstockDequeueResult>();
            try
            {
                dequeuedEvents = dequeueEvents.DequeueEventsFromIrma();
                if (dequeuedEvents != null && dequeuedEvents.Count > 0)
                {
                    instockDequeueResults = PrepareInstockData(dequeuedEvents);
                    ArchiveData(instockDequeueResults, null);
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> messageProperties = new Dictionary<string, string>()
                {
                    [Constants.MessageProperty.TransactionType] = settings.TransactionType
                };
                string errorMessage = $"A failure occurred when parsing dequeue results for {settings.TransactionType} type events";
                if (instockDequeueResults != null && instockDequeueResults.Count > 0)
                {
                    try
                    {
                        ArchiveData(instockDequeueResults, ex);
                    }
                    catch (Exception exception)
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
                        ex.GetType().ToString(),
                        ex.StackTrace,
                        "Fatal"
                        );
            }
            return instockDequeueResults;
        }

        private List<InstockDequeueResult> PrepareInstockData(List<InstockDequeueModel> dequeuedEvents)
        {
            List<InstockDequeueResult> instockDequeueResults = new List<InstockDequeueResult>();
            var sortedDequeuedEvents = dequeuedEvents.OrderBy(dequeuedEvent => dequeuedEvent.EventTypeCode);
            List<string> uniqueMessageTypes = sortedDequeuedEvents.Select(sortedDequeuedEvent => sortedDequeuedEvent.MessageType).Distinct().ToList();
            foreach (string currentMessageType in uniqueMessageTypes)
            {
                List<InstockDequeueModel> eventsWithCurrentMessageType = sortedDequeuedEvents.Where(sortedDequeuedEvent => currentMessageType.Equals(sortedDequeuedEvent.MessageType)).ToList();
                // Delete message types are: TransferOrderDelete, PurchaseOrderDelete, PurchaseLineDelete, TransferLineDelete
                if (currentMessageType.ToLower().Contains("delete"))
                {
                    eventsWithCurrentMessageType.ForEach(eventWithCurrentMessageType =>
                    {
                        GenerateInstockDequeuResult(eventWithCurrentMessageType, instockDequeueResults);
                    });
                }
                else
                {
                    List<InstockDequeueModel> eventsWithUniqueKeyID = eventsWithCurrentMessageType.GroupBy(eventWithCurrentMessageType => eventWithCurrentMessageType.KeyID).Select(group => group.First()).ToList();
                    eventsWithUniqueKeyID.ForEach(eventWithUniqueKeyID =>
                    {
                        GenerateInstockDequeuResult(eventWithUniqueKeyID, instockDequeueResults);
                    });
                }
            }
            return instockDequeueResults;
        }

        private void GenerateInstockDequeuResult(InstockDequeueModel instockDequeueEvent, List<InstockDequeueResult> instockDequeueResults)
        {
            string messageID = Guid.NewGuid().ToString();
            Dictionary<string, string> headers = GenerateHeaders(messageID, instockDequeueEvent);
            instockDequeueResults.Add(new InstockDequeueResult(instockDequeueEvent, headers));
        }

        private void ArchiveData(List<InstockDequeueResult> preparedDequeuedEvents, Exception originalException)
        {
            ArchiveInstockDequeueEvents archiveInstockDequeueEvents = new ArchiveInstockDequeueEvents(irmaContextFactory, settings, instockDequeueSerializer);
            foreach (InstockDequeueResult preparedDequeuedEvent in preparedDequeuedEvents)
            {
                try
                {
                    if (originalException == null)
                    {
                        archiveInstockDequeueEvents.Archive(preparedDequeuedEvent.InstockDequeueModel, preparedDequeuedEvent.Headers, null, null);
                    }
                    else
                    {
                        archiveInstockDequeueEvents.Archive(preparedDequeuedEvent.InstockDequeueModel, preparedDequeuedEvent.Headers, originalException.GetType().ToString(), originalException.ToString());
                    }
                }
                catch (Exception ex)
                {
                    inventoryLogger.LogError(
                        $@"Failed to archive Event Message for 
                        KeyID: {preparedDequeuedEvent.InstockDequeueModel.KeyID}, 
                        SecondaryID: {preparedDequeuedEvent.InstockDequeueModel.SecondaryKeyID}, 
                        MessageType: {preparedDequeuedEvent.InstockDequeueModel.MessageType}, 
                        EventTypeCode: {preparedDequeuedEvent.InstockDequeueModel.EventTypeCode}, 
                        InsertDate: {preparedDequeuedEvent.InstockDequeueModel.InsertDate}, 
                        MessageTimestampUtc: {preparedDequeuedEvent.InstockDequeueModel.MessageTimestampUtc}, 
                        Error Message: {ex.Message}"
                        , ex.StackTrace);
                    throw ex;
                }
            }
        }

        private Dictionary<string, string> GenerateHeaders(string messageID, InstockDequeueModel result)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                [Constants.MessageProperty.TransactionID] = messageID,
                [Constants.MessageProperty.TransactionType] = settings.TransactionType,
                [Constants.MessageProperty.Source] = "IRMA",
                [Constants.MessageProperty.MessageType] = result.MessageType,
                [Constants.MessageProperty.MessageNumber] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                [Constants.MessageProperty.RegionCode] = settings.RegionCode
            };
            return headers;
        }
    }
}
