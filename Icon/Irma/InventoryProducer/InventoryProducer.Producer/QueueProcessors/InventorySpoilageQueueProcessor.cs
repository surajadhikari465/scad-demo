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
using System.Text;

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
        }

        public void ProcessMessageQueue()
        {
            inventoryLogger.LogInfo("Starting " + settings.TransactionType + " producer.");
            List<InstockDequeueResult> dequeuedMessages = instockDequeueService.GetDequeuedMessages();
            ArchiveInventoryEvents archiveInventoryEvents = new ArchiveInventoryEvents(irmaContextFactory, settings);
            foreach (InstockDequeueResult dequeuedMessage in dequeuedMessages)
            {
                try
                {
                    List<ShrinkDataModel> shrinkData = GetShrinkData(dequeuedMessage);
                    if (shrinkData.Count > 0)
                    {
                        inventoryAdjustments inventoryAdjustmentsCanonical = CreateInventorySpoilageCanonical(dequeuedMessage, shrinkData.ElementAt(0));
                        string inventoryAdjustmentsXmlPayload = serializer.Serialize(inventoryAdjustmentsCanonical, new Utf8StringWriter());

                        dequeuedMessage.headers["TransactionID"] = 
                        inventoryAdjustmentsCanonical.inventoryAdjustment[0].locationNumber.ToString() 
                        + settings.TransactionType 
                        + inventoryAdjustmentsCanonical.inventoryAdjustment[0].messageNumber;
                        dequeuedMessage.headers["MessageType"] = "Text";
                        dequeuedMessage.headers["nonReceivingSysName"] = settings.NonReceivingSystemsSpoilage;
                        try
                        {
                            PublishMessage(inventoryAdjustmentsXmlPayload, dequeuedMessage.headers);
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
                    string instockDequeueModelXmlPayload = instockDequeueSerializer.Serialize(
                        ArchiveInstockDequeueEvents.ConvertToEventTypesCanonical(dequeuedMessage.instockDequeueModel), 
                        new Utf8StringWriter()
                        ).Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                    PublishErrorEvents.SendToMammoth(
                        mammothContextFactory, 
                        "InventorySpoilageProcessor", 
                        dequeuedMessage.headers["MessageNumber"], 
                        dequeuedMessage.headers,
                        instockDequeueModelXmlPayload,
                        "Data Issue",
                        ex.StackTrace,
                        "Fatal"
                        );
                }
            }
            inventoryLogger.LogInfo("Ending " + settings.TransactionType + " producer.");
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
                eventType = dequeuedMessage.instockDequeueModel.EventTypeCode,
                messageNumber = dequeuedMessage.headers["MessageNumber"],
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
                adjustmentQuantity = new QuantityType1[1],
                userInfo = new UserType
                {
                    idNumber = requiredShrinkDataModel.UserID.ToString(),
                    name = requiredShrinkDataModel.UserName,
                },
                createDateTime = requiredShrinkDataModel.CurrentDateTime
            };
            canonicalObj.inventoryAdjustment[0].inventoryAdjustmentDetail[0].adjustmentQuantity[0] = new QuantityType1
            {
                value = requiredShrinkDataModel.AdjustmentQuantity,
                units = new UnitsType1
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
            using (var irmaContext = irmaContextFactory.CreateContext("Irma_" + settings.RegionCode))
            {
                irmaContext.Database.CommandTimeout = 120;
                StringBuilder shrinkDataSQLQueryBuilder = 
                    new StringBuilder()
                    .AppendFormat(@"SELECT TOP({0}) ", settings.BatchSize.ToString())
                    .Append("ih.ItemHistoryID as AdjustmentNumber, ")
                    .Append("s.BusinessUnit_ID LocationNumber, ")
                    .Append("s.Store_Name as LocationName, ")
                    .Append("st.SubDept_No as HostSubTeamNumber, ")
                    .Append("st.SubTeam_Name as HostSubTeamName, ")
                    .Append("ss.SubDept_No as SubTeamNumber, ")
                    .Append("ss.SubTeam_Name as SubTeamName, ")
                    .Append("ii.Identifier as DefaultScanCode, ")
                    .Append("ih.AdjustmentReason as ReasonCode, ")
                    .Append("case when IsNull(ih.Quantity, 0) = 0 ")
                    .Append("then ih.Weight else ih.Quantity ")
                    .Append("End as AdjustmentQuantity, ")
                    .Append("iu.Unit_Abbreviation as AdjustmentQuantityUOMCode, ")
                    .Append("ih.Item_Key as SourceItemKey, ")
                    .Append("ih.DateStamp as CurrentDateTime, ")
                    .Append("ih.CreatedBy as UserID, ")
                    .Append("us.FullName as UserName ")
                    .Append("FROM dbo.ItemHistory as ih (NOLOCK) ")
                    .Append("JOIN Store as s (NOLOCK) on s.Store_No = ih.Store_No ")
                    .Append("JOIN Item as i (NOLOCK) on i.Item_Key = ih.Item_Key ")
                    .Append("JOIN ItemIdentifier as ii (NOLOCK) on ii.Item_Key = i.Item_Key ")
                    .Append("JOIN ItemUnit as iu (NOLOCK) on iu.Unit_ID = i.Retail_Unit_ID ")
                    .Append("JOIN SubTeam st (NOLOCK) on st.SubTeam_No = i.SubTeam_No ")
                    .Append("JOIN SubTeam ss (NOLOCK) on ss.SubTeam_No = ih.SubTeam_No ")
                    .Append("JOIN Users us (NOLOCK) on ih.CreatedBy = us.User_ID ")
                    .Append("WHERE ii.Default_Identifier = 1 ")
                    .Append("and ih.ItemHistoryID = @KeyId");
                List<ShrinkDataModel> shrinkData = irmaContext.Database.SqlQuery<ShrinkDataModel>(
                    shrinkDataSQLQueryBuilder.ToString(), 
                    new SqlParameter("@KeyID", dequeuedMessage.instockDequeueModel.KeyID)
                    ).ToList();
                return shrinkData;
            }
        }

        private void PublishMessage(string xmlPayload, Dictionary<String, String> headers)
        {
            inventoryLogger.LogInfo(string.Format("Preparing to send message {0}.", headers["TransactionID"]));
            SendToActiveMq(xmlPayload, headers);
            SendToEsb(xmlPayload, headers);
            inventoryLogger.LogInfo(string.Format("Sent message {0}.", headers["TransactionID"]));
        }

        private void SendToEsb(String xmlMessage, Dictionary<string, string> messageProperties)
        {
            producer.Send(xmlMessage, messageProperties);
        }

        private void SendToActiveMq(String xmlMessage, Dictionary<string, string> messageProperties)
        {
            activeMqProducer.Send(xmlMessage, messageProperties);
        }
    }
}
