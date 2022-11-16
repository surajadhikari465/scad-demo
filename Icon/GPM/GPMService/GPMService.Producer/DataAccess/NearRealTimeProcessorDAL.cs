using GPMService.Producer.Helpers;
using GPMService.Producer.Model;
using GPMService.Producer.Model.DBModel;
using GPMService.Producer.Settings;
using Icon.DbContextFactory;
using Icon.Framework;
using Mammoth.Framework;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GPMService.Producer.DataAccess
{
    internal class NearRealTimeProcessorDAL : INearRealTimeProcessorDAL
    {
        private const int DB_TIMEOUT_IN_SECONDS = 10;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly RetryPolicy retrypolicy;

        public NearRealTimeProcessorDAL(
            IDbContextFactory<MammothContext> mammothContextFactory,
            GPMProducerServiceSettings gpmProducerServiceSettings
            )
        {
            this.mammothContextFactory = mammothContextFactory;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.retrypolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                gpmProducerServiceSettings.DbErrorRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(gpmProducerServiceSettings.DbRetryDelayInMilliseconds)
                );
        }  

        public IList<MessageSequenceModel> GetLastSequence(string correlationID)
        {
            IList<MessageSequenceModel> messageSequenceData = new List<MessageSequenceModel>();
            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                string getLastSequenceQuery = $@"SELECT 
                    MessageSequenceID, 
                    PatchFamilyID, 
                    PatchFamilySequenceID AS LastProcessedGpmSequenceID 
                    FROM gpm.MessageSequence 
                    WHERE PatchFamilyID = @CorrelationID";
                messageSequenceData = mammothContext
                    .Database
                    .SqlQuery<MessageSequenceModel>(
                    getLastSequenceQuery,
                    new SqlParameter("@CorrelationID", correlationID)
                    ).ToList();
            }
            return messageSequenceData;
        }

        public void ArchiveMessage(ReceivedMessage receivedMessage, string errorCode, string errorDetails)
        {
            string correlationID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.CorrelationID);
            string transactionID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionID);
            string transactionType = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionType);
            string resetFlag = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.ResetFlag);
            string source = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.Source);
            string nonReceivingSysName = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.nonReceivingSysName);
            string sequenceID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.SequenceID);
            string itemID = correlationID.Substring(0, correlationID.IndexOf("-"));
            string businessUnitID = correlationID.Substring(correlationID.IndexOf("-") + 1);
            string messageHeaders = $@"
{{""TransactionID"":""${transactionID}"", 
""CorrelationID"":""${correlationID}"", 
""SequenceID"":""${sequenceID}"", 
""TransactionType"":""${transactionType}"", 
""ResetFlag"":""${resetFlag}"", 
""Source"":""${source}"", 
""nonReceivingSysName"":""${nonReceivingSysName}""}}";
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
                    new SqlParameter("@ItemID", itemID),
                    new SqlParameter("@BusinessUnitID", businessUnitID),
                    new SqlParameter("@MessageID", transactionID),
                    new SqlParameter("@MessageHeaders", messageHeaders),
                    new SqlParameter("@Message", receivedMessage.esbMessage.MessageText),
                    new SqlParameter("@ErrorCode", (object)errorCode ?? DBNull.Value),
                    new SqlParameter("@ErrorDetails", (object)errorDetails ?? DBNull.Value)
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
            string archiveErrorResponseSQLStatement = $@"INSERT INTO [esb].[MessageArchive] (MessageID, MessageTypeID, MessageStatusID, MessageHeadersJson, MessageBody)
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
                    new SqlParameter("@MessageID", messageID),
                    new SqlParameter("@MessageTypeName", messageTypeName),
                    new SqlParameter("@MessageHeadersJson", jsonPropertiesString),
                    new SqlParameter("@MessageBody", xmlMessagePayload)
                    );
            }
        }
    }
}
