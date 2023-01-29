using System;
using System.Collections.Generic;
using System.Linq;
using Icon.Esb.Producer;
using Icon.ActiveMQ.Producer;
using InventoryProducer.Common.InstockDequeue;
using InventoryProducer.Common;
using InventoryProducer.Common.InstockDequeue.Model;
using Icon.Esb.Schemas.Wfm.Contracts;
using InventoryProducer.Common.Serializers;
using Icon.DbContextFactory;
using Irma.Framework;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Producer.Helpers;
using Mammoth.Framework;
using System.Data.SqlClient;
using InventoryProducer.Common.InstockDequeue.Schemas;
using InventoryProducer.Common.Helpers;
using Polly.Retry;
using Polly;

namespace InventoryProducer.Producer.QueueProcessors
{
    public class InventorySpoilageQueueProcessor : IQueueProcessor
    {
        private readonly IDbContextFactory<IrmaContext> irmaContextFactory;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly InventoryProducerSettings settings;
        private readonly InventoryLogger<InventorySpoilageQueueProcessor> inventoryLogger;
        private readonly InstockDequeueService instockDequeueService;
        private readonly ISerializer<inventoryAdjustments> serializer;
        private readonly ISerializer<EventTypes> instockDequeueSerializer;
        private readonly IEsbProducer producer;
        private readonly IActiveMQProducer activeMqProducer;
        private readonly RetryPolicy retrypolicy;
        public InventorySpoilageQueueProcessor(
            IDbContextFactory<IrmaContext> irmaContextFactory,
            IDbContextFactory<MammothContext> mammothContextFactory,
            InventoryProducerSettings settings,
            InventoryLogger<InventorySpoilageQueueProcessor> inventoryLogger,
            InstockDequeueService instockDequeueService,
            ISerializer<inventoryAdjustments> serializer,
            ISerializer<EventTypes> instockDequeueSerializer,
            IEsbProducer producer,
            IActiveMQProducer activeMqProducer)
        {
            this.irmaContextFactory = irmaContextFactory;
            this.mammothContextFactory = mammothContextFactory;
            this.settings = settings;
            this.inventoryLogger = inventoryLogger;
            this.instockDequeueService = instockDequeueService;
            this.serializer = serializer;
            this.instockDequeueSerializer = instockDequeueSerializer;
            this.producer = producer;
            this.activeMqProducer = activeMqProducer;
            this.retrypolicy = RetryPolicy
                .Handle<Exception>()
                .WaitAndRetry(
                settings.ServiceMaxRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(settings.ServiceMaxRetryDelayInMilliseconds)
                );
        }

        public void ProcessMessageQueue()
        {
            inventoryLogger.LogInfo($"Starting {settings.TransactionType} producer.");
            IList<InstockDequeueResult> dequeuedMessages = instockDequeueService.GetDequeuedMessages();
            ArchiveInventoryEvents archiveInventoryEvents = new ArchiveInventoryEvents(irmaContextFactory, settings);
            foreach (InstockDequeueResult dequeuedMessage in dequeuedMessages)
            {
                try
                {
                    this.retrypolicy.Execute(() => PublishInventorySpoilageService(archiveInventoryEvents, dequeuedMessage));
                }
                catch (Exception ex)
                {
                    // this exception will happen after all retries
                    string instockDequeueModelXmlPayload = instockDequeueSerializer.Serialize(
                        ArchiveInstockDequeueEvents.ConvertToEventTypesCanonical(dequeuedMessage.InstockDequeueModel),
                        new Utf8StringWriter()
                        ).Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                    PublishErrorEvents.SendToMammoth(
                        mammothContextFactory,
                        "PublishInventorySpoilageService",
                        dequeuedMessage.Headers[Constants.MessageProperty.MessageNumber],
                        dequeuedMessage.Headers,
                        instockDequeueModelXmlPayload,
                        ex.GetType().ToString(),
                        ex.Message
                        );
                }
            }
            inventoryLogger.LogInfo($"Ending {settings.TransactionType} producer.");
        }

        private void PublishInventorySpoilageService(ArchiveInventoryEvents archiveInventoryEvents, InstockDequeueResult dequeuedMessage)
        {
            try
            {
                List<ShrinkDataModel> shrinkData = GetShrinkData(dequeuedMessage);
                inventoryLogger.LogInfo($"Retrieved shrink data count: {shrinkData?.Count}.");
                if (shrinkData != null && shrinkData.Count > 0)
                {
                    inventoryAdjustments inventoryAdjustmentsCanonical = CreateInventorySpoilageCanonical(dequeuedMessage, shrinkData.ElementAt(0));
                    string inventoryAdjustmentsXmlPayload = serializer.Serialize(inventoryAdjustmentsCanonical, new Utf8StringWriter());

                    dequeuedMessage.Headers[Constants.MessageProperty.TransactionID] =
                    inventoryAdjustmentsCanonical.inventoryAdjustment[0].locationNumber.ToString()
                    + settings.TransactionType
                    + inventoryAdjustmentsCanonical.inventoryAdjustment[0].messageNumber;
                    dequeuedMessage.Headers[Constants.MessageProperty.MessageType] = "Text";
                    dequeuedMessage.Headers[Constants.MessageProperty.NonReceivingSystems] = settings.NonReceivingSystemsSpoilage;
                    try
                    {
                        PublishMessage(inventoryAdjustmentsXmlPayload, dequeuedMessage.Headers);
                        archiveInventoryEvents.Archive(
                            inventoryAdjustmentsXmlPayload,
                            inventoryAdjustmentsCanonical.inventoryAdjustment[0].eventType,
                            inventoryAdjustmentsCanonical.inventoryAdjustment[0].locationNumber,
                            inventoryAdjustmentsCanonical.inventoryAdjustment[0].adjustmentNumber,
                            0,
                            'P',
                            null,
                            inventoryAdjustmentsCanonical.inventoryAdjustment[0].messageNumber,
                            null
                            );
                    }
                    catch (Exception ex)
                    {
                        inventoryLogger.LogError(ex.Message, ex.StackTrace);
                        archiveInventoryEvents.Archive(
                            inventoryAdjustmentsXmlPayload,
                            inventoryAdjustmentsCanonical.inventoryAdjustment[0].eventType,
                            inventoryAdjustmentsCanonical.inventoryAdjustment[0].locationNumber,
                            inventoryAdjustmentsCanonical.inventoryAdjustment[0].adjustmentNumber,
                            0,
                            'U',
                            ex.Message,
                            inventoryAdjustmentsCanonical.inventoryAdjustment[0].messageNumber,
                            null
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                inventoryLogger.LogError(ex.Message, ex.StackTrace);
                throw ex;
            };
        }

        private inventoryAdjustments CreateInventorySpoilageCanonical(InstockDequeueResult dequeuedMessage, ShrinkDataModel requiredShrinkDataModel)
        {
            inventoryAdjustments canonicalObj = new inventoryAdjustments
            {
                inventoryAdjustment = new inventoryAdjustmentsInventoryAdjustment[1],
            };
            canonicalObj.inventoryAdjustment[0] = new inventoryAdjustmentsInventoryAdjustment
            {
                adjustmentNumber = requiredShrinkDataModel.AdjustmentNumber,
                adjustmentNumberSpecified = true,
                eventType = dequeuedMessage.InstockDequeueModel.EventTypeCode,
                messageNumber = dequeuedMessage.Headers[Constants.MessageProperty.MessageNumber],
                locationNumber = requiredShrinkDataModel.LocationNumber,
                locationName = requiredShrinkDataModel.LocationName,
                invAdjustmentSource = "IRMA",
                inventoryAdjustmentDetail = new inventoryAdjustmentsInventoryAdjustmentInventoryAdjustmentDetail[1],
            };
            canonicalObj.inventoryAdjustment[0].inventoryAdjustmentDetail[0] = new inventoryAdjustmentsInventoryAdjustmentInventoryAdjustmentDetail
            {
                adjustmentDetailNumber = requiredShrinkDataModel.AdjustmentNumber,
                adjustmentDetailNumberSpecified = true,
                sourceItemKey = requiredShrinkDataModel.SourceItemKey.ToString(),
                SubTeam = new SubTeamType
                {
                    subTeamNumber = requiredShrinkDataModel.SubTeamNumber.ToString(),
                    subTeamName = requiredShrinkDataModel.SubTeamName,
                    hostSubTeamNumber = requiredShrinkDataModel.HostSubTeamNumber.ToString(),
                    hostSubTeamName = requiredShrinkDataModel.HostSubTeamName,
                },
                defaultScanCode = requiredShrinkDataModel.DefaultScanCode,
                reasonCode = requiredShrinkDataModel.ReasonCode,
                adjustmentQuantity = new QuantityType[1],
                userInfo = new UserType
                {
                    idNumber = requiredShrinkDataModel.UserID.ToString(),
                    name = requiredShrinkDataModel.UserName,
                },
                createDateTime = new DateTimeOffset(requiredShrinkDataModel.CurrentDateTime).ToString("O")
            };
            canonicalObj.inventoryAdjustment[0].inventoryAdjustmentDetail[0].adjustmentQuantity[0] = new QuantityType
            {
                value = requiredShrinkDataModel.AdjustmentQuantity,
                valueSpecified = true,
                units = new UnitsType
                {
                    uom = new UomType()
                }
            };
            if (requiredShrinkDataModel.AdjustmentQuantityUOMCode != null)
            {
                Enum.TryParse<WfmUomCodeEnumType>(requiredShrinkDataModel.AdjustmentQuantityUOMCode.Trim().ToUpper(), out WfmUomCodeEnumType uomCode);
                canonicalObj.inventoryAdjustment[0].inventoryAdjustmentDetail[0].adjustmentQuantity[0].units.uom.code = uomCode;
                canonicalObj.inventoryAdjustment[0].inventoryAdjustmentDetail[0].adjustmentQuantity[0].units.uom.codeSpecified = true;
            }
            return canonicalObj;
        }

        private List<ShrinkDataModel> GetShrinkData(InstockDequeueResult dequeuedMessage)
        {
            using (var irmaContext = irmaContextFactory.CreateContext($"Irma_{settings.RegionCode}"))
            {
                irmaContext.Database.CommandTimeout = 120;
                string shrinkDataSQLQuery =
                    $@"SELECT TOP({settings.BatchSize}) 
                    ih.ItemHistoryID as AdjustmentNumber, 
                    s.BusinessUnit_ID LocationNumber, 
                    s.Store_Name as LocationName, 
                    st.SubDept_No as HostSubTeamNumber, 
                    st.SubTeam_Name as HostSubTeamName, 
                    ss.SubDept_No as SubTeamNumber, 
                    ss.SubTeam_Name as SubTeamName, 
                    ii.Identifier as DefaultScanCode, 
                    ih.AdjustmentReason as ReasonCode, 
                    case when IsNull(ih.Quantity, 0) = 0 
                    then ih.Weight else ih.Quantity 
                    End as AdjustmentQuantity, 
                    iu.Unit_Abbreviation as AdjustmentQuantityUOMCode, 
                    ih.Item_Key as SourceItemKey, 
                    ih.DateStamp as CurrentDateTime, 
                    ih.CreatedBy as UserID, 
                    us.FullName as UserName 
                    FROM dbo.ItemHistory as ih (NOLOCK) 
                    JOIN Store as s (NOLOCK) on s.Store_No = ih.Store_No 
                    JOIN Item as i (NOLOCK) on i.Item_Key = ih.Item_Key 
                    JOIN ItemIdentifier as ii (NOLOCK) on ii.Item_Key = i.Item_Key 
                    JOIN ItemUnit as iu (NOLOCK) on iu.Unit_ID = i.Retail_Unit_ID 
                    JOIN SubTeam st (NOLOCK) on st.SubTeam_No = i.SubTeam_No 
                    JOIN SubTeam ss (NOLOCK) on ss.SubTeam_No = ih.SubTeam_No 
                    JOIN Users us (NOLOCK) on ih.CreatedBy = us.User_ID 
                    WHERE ii.Default_Identifier = 1 
                    and ih.ItemHistoryID = @KeyId";
                List<ShrinkDataModel> shrinkData = irmaContext.Database.SqlQuery<ShrinkDataModel>(
                    shrinkDataSQLQuery,
                    new SqlParameter("@KeyID", dequeuedMessage.InstockDequeueModel.KeyID)
                    ).ToList();
                return shrinkData;
            }
        }

        private void PublishMessage(string xmlPayload, Dictionary<string, string> headers)
        {
            inventoryLogger.LogInfo(string.Format("Preparing to send message {0}.", headers[Constants.MessageProperty.TransactionID]));
            SendToActiveMq(xmlPayload, headers);
            SendToEsb(xmlPayload, headers);
            inventoryLogger.LogInfo(string.Format("Sent message {0}.", headers[Constants.MessageProperty.TransactionID]));
        }

        private void SendToEsb(string xmlMessage, Dictionary<string, string> messageProperties)
        {
            producer.Send(xmlMessage, messageProperties);
        }

        private void SendToActiveMq(string xmlMessage, Dictionary<string, string> messageProperties)
        {
            activeMqProducer.Send(xmlMessage, messageProperties);
        }
    }
}
