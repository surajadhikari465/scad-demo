using GPMService.Producer.ErrorHandler;
using GPMService.Producer.GPMException;
using GPMService.Producer.Helpers;
using GPMService.Producer.Model;
using GPMService.Producer.Model.DBModel;
using GPMService.Producer.Serializer;
using GPMService.Producer.Settings;
using Icon.Common.Xml;
using Icon.DbContextFactory;
using Icon.Esb.Schemas.Mammoth;
using Icon.Logging;
using Mammoth.Framework;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Wfm.Aws.Helpers;

namespace GPMService.Producer.DataAccess
{
    internal class NearRealTimeProcessorDAL : INearRealTimeProcessorDAL
    {
        private const int DB_TIMEOUT_IN_SECONDS = 10;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly ILogger<NearRealTimeProcessorDAL> logger;
        private readonly ErrorEventPublisher errorEventPublisher;
        private readonly RetryPolicy retryPolicy;
        private readonly ISerializer<MammothPriceType> mammothPriceSerializer;
        private readonly ISerializer<PriceMessageArchiveType> priceMessageArchiveSerializer;

        public NearRealTimeProcessorDAL(
            IDbContextFactory<MammothContext> mammothContextFactory,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ILogger<NearRealTimeProcessorDAL> logger,
            ErrorEventPublisher errorEventPublisher,
            ISerializer<MammothPriceType> mammothPriceSerializer,
            ISerializer<PriceMessageArchiveType> priceMessageArchiveSerializer
            )
        {
            this.mammothContextFactory = mammothContextFactory;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.logger = logger;
            this.errorEventPublisher = errorEventPublisher;
            this.mammothPriceSerializer = mammothPriceSerializer;
            this.priceMessageArchiveSerializer = priceMessageArchiveSerializer;
            this.retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                gpmProducerServiceSettings.DbErrorRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(gpmProducerServiceSettings.DbRetryDelayInMilliseconds)
                );
        }

        public IList<MessageSequenceModel> GetLastSequence(string patchFamilyID)
        {
            IList<MessageSequenceModel> messageSequenceData = new List<MessageSequenceModel>();
            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                string getLastSequenceQuery = $@"SELECT TOP(100) 
                    MessageSequenceID, 
                    PatchFamilyID, 
                    PatchFamilySequenceID AS LastProcessedGpmSequenceID 
                    FROM gpm.MessageSequence 
                    WHERE PatchFamilyID = @PatchFamilyID";
                messageSequenceData = mammothContext
                    .Database
                    .SqlQuery<MessageSequenceModel>(
                    getLastSequenceQuery,
                    new SqlParameter("@PatchFamilyID", patchFamilyID)
                    ).ToList();
            }
            return messageSequenceData;
        }

        public void ArchiveMessage(ReceivedMessage receivedMessage, string errorCode, string errorDetails)
        {
            string correlationID = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.CorrelationID.ToLower());
            string transactionID = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.TransactionID.ToLower());
            string transactionType = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.TransactionType.ToLower());
            string resetFlag = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.ResetFlag.ToLower());
            string source = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.Source.ToLower());
            string nonReceivingSysName = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.nonReceivingSysName.ToLower());
            string sequenceID = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.SequenceID.ToLower());
            string itemID = correlationID.Substring(0, correlationID.IndexOf("-"));
            string businessUnitID = correlationID.Substring(correlationID.IndexOf("-") + 1);
            string messageHeaders = $@"
{{""TransactionID"":""{transactionID}"", 
""CorrelationID"":""{correlationID}"", 
""SequenceID"":""{sequenceID}"", 
""TransactionType"":""{transactionType}"", 
""ResetFlag"":""{resetFlag}"", 
""Source"":""{source}"", 
""nonReceivingSysName"":""{nonReceivingSysName}""}}";
            PriceMessageArchiveType priceMessageArchive = new PriceMessageArchiveType()
            {
                ItemID = int.Parse(itemID),
                BusinessUnitID = int.Parse(businessUnitID),
                MessageID = transactionID,
                MessageHeaders = messageHeaders,
                MessageBody = receivedMessage.sqsExtendedClientMessage.S3Details[0].Data,
                ErrorCode = errorCode,
                ErrorDetails = errorDetails,
            };
            try
            {
                using (var mammothContext = mammothContextFactory.CreateContext())
                {
                    mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                    string archiveSQLStatement = $@"INSERT INTO gpm.MessageArchivePrice 
(ItemID, BusinessUnitID, MessageID, MessageHeaders, Message, ErrorCode, ErrorDetails) 
VALUES (@ItemID, @BusinessUnitID, @MessageID, @MessageHeaders, @Message, @ErrorCode, @ErrorDetails)";
                    mammothContext
                        .Database
                        .ExecuteSqlCommand(
                        archiveSQLStatement,
                        new SqlParameter("@ItemID", priceMessageArchive.ItemID),
                        new SqlParameter("@BusinessUnitID", priceMessageArchive.BusinessUnitID),
                        new SqlParameter("@MessageID", priceMessageArchive.MessageID.Substring(0, priceMessageArchive.MessageID.Length <= 50 ? priceMessageArchive.MessageID.Length : 50)),
                        new SqlParameter("@MessageHeaders", priceMessageArchive.MessageHeaders),
                        // DB persist doesn't support utf-8 encoding
                        new SqlParameter("@Message", priceMessageArchive.MessageBody.Replace("UTF-8", "utf-16")),
                        new SqlParameter("@ErrorCode", (object)priceMessageArchive.ErrorCode ?? DBNull.Value),
                        new SqlParameter("@ErrorDetails", (object)priceMessageArchive.ErrorDetails ?? DBNull.Value)
                        );
                }
            }
            catch (Exception ex)
            {
                logger.Error($@"Failed to archive 
MessageID {transactionID}, 
MessageHeaders: {messageHeaders}. 
ErrorMsg: {ex.Message}");
                errorEventPublisher.PublishErrorEvent(
                    "GPMNearRealTime",
                    priceMessageArchive.MessageID,
                    new Dictionary<string, string>()
                    { {"MessageHeaders", priceMessageArchive.MessageHeaders } },
                    priceMessageArchiveSerializer.Serialize(priceMessageArchive, new Utf8StringWriter()),
                    ex.GetType().ToString(),
                    ex.Message
                    );
            }
        }

        public IList<GetRegionCodeQueryModel> GetRegionCodeQuery(string businessUnitID)
        {
            IList<GetRegionCodeQueryModel> regionQueryData = new List<GetRegionCodeQueryModel>();
            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                string regionQuery = $@"SELECT TOP(100) 
CAST(Region as VARCHAR) AS Region, 
LocaleID, BusinessUnitID, StoreName, 
StoreAbbrev 
FROM dbo.Locale WHERE BusinessUnitID = @BusinessUnitID";
                regionQueryData = mammothContext
                    .Database
                    .SqlQuery<GetRegionCodeQueryModel>(
                    regionQuery,
                    new SqlParameter("@BusinessUnitID", businessUnitID)
                    ).ToList();
            }
            return regionQueryData;
        }

        public void ArchiveErrorResponseMessage(string messageID, string messageTypeName, string xmlMessagePayload, Dictionary<string, string> messageProperties)
        {
            string jsonPropertiesString = JsonConvert.SerializeObject(new Dictionary<string, Dictionary<string, string>>() { { "MessageHeaders", messageProperties } }, Formatting.Indented);
            string archiveErrorResponseSQLStatement = $@"INSERT INTO esb.MessageArchive (MessageID, MessageTypeID, MessageStatusID, MessageHeadersJson, MessageBody)
SELECT 
@MessageID,
(SELECT MessageTypeId FROM esb.MessageType WHERE MessageTypeName = @MessageTypeName) as MessageTypeID,
(SELECT MessageStatusId FROM esb.MessageStatus WHERE MessageStatusName = 'Sent') as MessageStatusID,
@MessageHeadersJson,
@MessageBody";
            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                mammothContext
                    .Database
                    .ExecuteSqlCommand(
                    archiveErrorResponseSQLStatement,
                    new SqlParameter("@MessageID", messageID.Substring(0, messageID.Length <= 50 ? messageID.Length : 50)),
                    new SqlParameter("@MessageTypeName", messageTypeName),
                    new SqlParameter("@MessageHeadersJson", jsonPropertiesString),
                    new SqlParameter("@MessageBody", xmlMessagePayload.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", ""))
                    );
            }
        }

        public void ProcessPriceMessage(ReceivedMessage receivedMessage, MammothPricesType mammothPrices)
        {
            string messageID = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.TransactionID.ToLower()) ?? "";
            string resetFlag = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.ResetFlag.ToLower());
            string patchFamilyID = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.CorrelationID.ToLower());
            int patchFamilySequenceID = int.Parse(receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.SequenceID.ToLower()));
            try
            {
                retryPolicy.Execute(() =>
                {
                    using (var mammothContext = mammothContextFactory.CreateContext())
                    {
                        mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                        // TODO: Test transaction behaviour
                        using (var transaction = mammothContext.Database.BeginTransaction())
                        {
                            try
                            {
                                if (Constants.ResetFlagValues.ResetFlagTrueValue.Equals(resetFlag))
                                {
                                    DeleteAllPricesForItemIdBusinessUnitId(mammothContext, mammothPrices.MammothPrice[0].ItemId, mammothPrices.MammothPrice[0].BusinessUnit, mammothPrices.MammothPrice[0].Region);
                                }
                                // foreach cannot be used as MammothPrices does not have enumerator.
                                int i = 0;
                                for (i = 0; i < mammothPrices.MammothPrice.Length; i++)
                                {
                                    ExecutePriceCommand(mammothContext, mammothPrices.MammothPrice[i]);
                                }
                                AddOrUpdateMessageSequence(mammothContext, mammothPrices.MammothPrice[i - 1].ItemId, mammothPrices.MammothPrice[i - 1].BusinessUnit, patchFamilyID, patchFamilySequenceID, messageID);
                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                throw e;
                            }
                        }
                    }
                });
            }
            catch (Exception e)
            {
                throw new DatabaseErrorException(e.Message, e);
            }
        }

        private void AddOrUpdateMessageSequence(MammothContext mammothContext, int itemId, int businessUnit, string patchFamilyID, int patchFamilySequenceID, string messageID)
        {
            string addOrUpdateMessageSequenceStoredProcedure = $"EXEC gpm.AddOrUpdateMessageSequence @ItemID, @BusinessUnitID, @PatchFamilyID, @PatchFamilySequenceID, @MessageID";
            mammothContext
                .Database
                .ExecuteSqlCommand(
                addOrUpdateMessageSequenceStoredProcedure,
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@BusinessUnitID", businessUnit),
                new SqlParameter("@PatchFamilyID", patchFamilyID),
                new SqlParameter("@PatchFamilySequenceID", patchFamilySequenceID),
                new SqlParameter("@MessageID", messageID)
                );
        }

        private void ExecutePriceCommand(MammothContext mammothContext, MammothPriceType mammothPriceType)
        {
            if (!ValidAction(mammothPriceType))
            {
                throw new ActionNotSuppliedException($@"Price Not supplied, Item Id: {mammothPriceType.ItemId}, Business Unit ID: {mammothPriceType.BusinessUnit}, Region: {mammothPriceType.Region}");
            }
            else
            {
                if (Constants.PriceActions.Add.Equals(mammothPriceType.Action))
                {
                    int affectedRows = AddPrice(mammothContext, mammothPriceType);
                    if (affectedRows == 0)
                    {
                        throw new ZeroRowsImpactedException($@"Zero Rows were affected for the {mammothPriceType.Action} action. Item ID: {mammothPriceType.ItemId}, Region: {mammothPriceType.Region}, BusinessUnit: {mammothPriceType.BusinessUnit}");
                    }
                }
                else if (Constants.PriceActions.Update.Equals(mammothPriceType.Action))
                {
                    int updateAffectedRows = UpdatePrice(mammothContext, mammothPriceType);
                    if (updateAffectedRows == 0)
                    {
                        int addAffectedRows = AddPrice(mammothContext, mammothPriceType);
                        if (addAffectedRows == 0)
                        {
                            throw new ZeroRowsImpactedException($@"Zero Rows were affected for the {mammothPriceType.Action} action. Item ID: {mammothPriceType.ItemId}, Region: {mammothPriceType.Region}, BusinessUnit: {mammothPriceType.BusinessUnit}");
                        }
                    }
                }
                else if (Constants.PriceActions.Delete.Equals(mammothPriceType.Action))
                {
                    int affectedRows = DeletePrice(mammothContext, mammothPriceType);
                    if (affectedRows == 0)
                    {
                        throw new ZeroRowsImpactedException($@"Zero Rows were affected for the {mammothPriceType.Action} action. Item ID: {mammothPriceType.ItemId}, Region: {mammothPriceType.Region}, BusinessUnit: {mammothPriceType.BusinessUnit}");
                    }
                }
            }
        }

        private int DeletePrice(MammothContext mammothContext, MammothPriceType mammothPriceType)
        {
            string deletePriceStoredProcedure = @"EXEC gpm.DeletePrice 
@Region,
@ItemID,
@BusinessUnitID,
@StartDate,
@PriceType,
@NumberOfRowsDeleted OUTPUT";
            SqlParameter numberOfRowsDeletedParameter = new SqlParameter("@NumberOfRowsDeleted", SqlDbType.Int);
            numberOfRowsDeletedParameter.Direction = ParameterDirection.Output;
            mammothContext
                .Database
                .ExecuteSqlCommand(
                deletePriceStoredProcedure,
                new SqlParameter("@Region", mammothPriceType.Region),
                new SqlParameter("@ItemID", mammothPriceType.ItemId),
                new SqlParameter("@BusinessUnitID", mammothPriceType.BusinessUnit),
                new SqlParameter("@PriceType", mammothPriceType.PriceType),
                new SqlParameter("@StartDate", mammothPriceType.StartDate),
                numberOfRowsDeletedParameter
                );
            return int.Parse(numberOfRowsDeletedParameter.Value.ToString());
        }

        private int UpdatePrice(MammothContext mammothContext, MammothPriceType mammothPriceType)
        {
            string updatePriceStoredProcedure = @"EXEC gpm.UpdatePrice 
@Region,
@GpmID,
@ItemID,
@BusinessUnitID,
@Price,
@StartDate,
@EndDate,
@PriceType,
@PriceTypeAttribute,
@SellableUOM,
@CurrencyCode,
@Multiple,
@TagExpirationDate,
@PercentOff,
@NumberOfRowsUpdated OUTPUT";
            SqlParameter numberOfRowsUpdatedParameter = new SqlParameter("@NumberOfRowsUpdated", SqlDbType.Int);
            numberOfRowsUpdatedParameter.Direction = ParameterDirection.Output;
            mammothContext
                .Database
                .ExecuteSqlCommand(
                updatePriceStoredProcedure,
                new SqlParameter("@Region", mammothPriceType.Region),
                new SqlParameter("@GpmID", (object)mammothPriceType.GpmId ?? DBNull.Value),
                new SqlParameter("@ItemID", mammothPriceType.ItemId),
                new SqlParameter("@BusinessUnitID", mammothPriceType.BusinessUnit),
                new SqlParameter("@Price", mammothPriceType.Price),
                new SqlParameter("@StartDate", mammothPriceType.StartDate),
                new SqlParameter("@EndDate", (object)mammothPriceType.EndDate ?? DBNull.Value),
                new SqlParameter("@PriceType", mammothPriceType.PriceType),
                new SqlParameter("@PriceTypeAttribute", mammothPriceType.PriceTypeAttribute),
                new SqlParameter("@SellableUOM", mammothPriceType.SellableUom),
                new SqlParameter("@CurrencyCode", (object)mammothPriceType.CurrencyCode ?? DBNull.Value),
                new SqlParameter("@Multiple", mammothPriceType.Multiple),
                new SqlParameter("@TagExpirationDate", (object)mammothPriceType.TagExpirationDate ?? DBNull.Value),
                new SqlParameter("@PercentOff", (object)mammothPriceType.PercentOff ?? DBNull.Value),
                numberOfRowsUpdatedParameter
                );
            return int.Parse(numberOfRowsUpdatedParameter.Value.ToString());
        }

        private int AddPrice(MammothContext mammothContext, MammothPriceType mammothPriceType)
        {
            string addPriceStoredProcedure = @"EXEC gpm.AddPrice 
@Region, 
@GpmID, 
@ItemID, 
@BusinessUnitID, 
@Price, 
@StartDate, 
@EndDate, 
@PriceType,
@PriceTypeAttribute, 
@SellableUOM, 
@CurrencyCode, 
@Multiple, 
@TagExpirationDate, 
@PercentOff, 
@NumberOfRowsUpdated OUTPUT";
            SqlParameter numberOfRowsUpdatedParameter = new SqlParameter("@NumberOfRowsUpdated", SqlDbType.Int);
            numberOfRowsUpdatedParameter.Direction = ParameterDirection.Output;
            mammothContext
                .Database
                .ExecuteSqlCommand(
                addPriceStoredProcedure,
                new SqlParameter("@Region", mammothPriceType.Region),
                new SqlParameter("@GpmID", (object)mammothPriceType.GpmId ?? DBNull.Value),
                new SqlParameter("@ItemID", mammothPriceType.ItemId),
                new SqlParameter("@BusinessUnitID", mammothPriceType.BusinessUnit),
                new SqlParameter("@Price", mammothPriceType.Price),
                new SqlParameter("@StartDate", mammothPriceType.StartDate),
                new SqlParameter("@EndDate", (object)mammothPriceType.EndDate ?? DBNull.Value),
                new SqlParameter("@PriceType", mammothPriceType.PriceType),
                new SqlParameter("@PriceTypeAttribute", mammothPriceType.PriceTypeAttribute),
                new SqlParameter("@SellableUOM", mammothPriceType.SellableUom),
                new SqlParameter("@CurrencyCode", (object)mammothPriceType.CurrencyCode ?? DBNull.Value),
                new SqlParameter("@Multiple", mammothPriceType.Multiple),
                new SqlParameter("@TagExpirationDate", (object)mammothPriceType.TagExpirationDate ?? DBNull.Value),
                new SqlParameter("@PercentOff", (object)mammothPriceType.PercentOff ?? DBNull.Value),
                numberOfRowsUpdatedParameter
                );
            return int.Parse(numberOfRowsUpdatedParameter.Value.ToString());
        }

        private bool ValidAction(MammothPriceType mammothPriceType)
        {
            return mammothPriceType.Action.Length > 0;
        }

        private void DeleteAllPricesForItemIdBusinessUnitId(MammothContext mammothContext, int itemID, int businessUnitID, string region)
        {
            string deleteAllPricesForItemIdBusinessUnitIdStoredProcedure = $"EXEC gpm.DeleteAllPricesForItemIdBusinessUnitId @Region, @ItemID, @BusinessUnitID";
            mammothContext
                .Database
                .ExecuteSqlCommand(
                deleteAllPricesForItemIdBusinessUnitIdStoredProcedure,
                new SqlParameter("@Region", region),
                new SqlParameter("@ItemID", itemID),
                new SqlParameter("@BusinessUnitID", businessUnitID)
                );
        }

        public void InsertEmergencyPrices(MammothPricesType mammothPrices)
        {
            string insertEmergencyPricesSqlStatement = $@"INSERT INTO gpm.MessageQueueEmergencyPrice(ItemId, BusinessUnitId, PriceType, MammothPriceXml)
SELECT @ItemId, @BusinessUnitId, @PriceType, @MammothPriceXml";
            retryPolicy.Execute(() =>
            {
                using (var mammothContext = mammothContextFactory.CreateContext())
                {
                    mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                    using (var transaction = mammothContext.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < mammothPrices.MammothPrice.Length; i++)
                            {
                                MammothPriceType mammothPrice = mammothPrices.MammothPrice[i];
                                mammothContext
                                    .Database
                                    .ExecuteSqlCommand(
                                    insertEmergencyPricesSqlStatement,
                                    new SqlParameter("@ItemId", mammothPrice.ItemId),
                                    new SqlParameter("@BusinessUnitId", mammothPrice.BusinessUnit),
                                    new SqlParameter("@PriceType", mammothPrice.PriceType),
                                    new SqlParameter("@MammothPriceXml", mammothPriceSerializer.Serialize(mammothPrice, new Utf8StringWriter()))
                                    );
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            });
        }
    }
}
