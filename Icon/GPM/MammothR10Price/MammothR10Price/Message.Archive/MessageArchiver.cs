using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Icon.DbContextFactory;
using Icon.Esb.Schemas.Mammoth;
using Icon.Logging;
using Mammoth.Framework;
using MammothR10Price.Model;
using Newtonsoft.Json;

namespace MammothR10Price.Message.Archive
{
    public class MessageArchiver: IMessageArchiver
    {
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly ILogger<MessageArchiver> logger;
        private const int DB_TIMEOUT_IN_SECONDS = 10;

        public MessageArchiver(
            IDbContextFactory<MammothContext> mammothContextFactory,
            ILogger<MessageArchiver> logger)
        {
            this.mammothContextFactory = mammothContextFactory;
            this.logger = logger;
        }

        public void ArchiveMessage(
            IList<MammothPriceType> mammothPrices,
            string itemPriceXml,
            IDictionary<string, string> receivedMessageProperties,
            IDictionary<string, string> dbArchiveMessageProperties
            )
        {
            try
            {
                ArchiveMammothR10Prices(mammothPrices, receivedMessageProperties, dbArchiveMessageProperties);
                ArchiveItemPriceXml(itemPriceXml, receivedMessageProperties, dbArchiveMessageProperties);
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        private void ArchiveMammothR10Prices(
            IList<MammothPriceType> mammothPrices,
            IDictionary<string, string> receivedMessageProperties,
            IDictionary<string, string> dbArchiveMessageProperties
            )
        {
            string archiveMessageDetailsSqlStatement = @"
                INSERT INTO [gpm].[MessageArchivePriceR10](ItemID, BusinessUnitID, PriceType, StartDate, MessageID, MessageDetailJson)
                VALUES (@ItemId, @BusinessUnitId, @PriceType, @StartDate, @MessageId, @MessageDetailJson) ";

            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                using(var transaction = mammothContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var mammothPrice in mammothPrices)
                        {
                            var messageDetail = new PriceMessageDetail()
                            {
                                ItemID = mammothPrice.ItemId.ToString(),
                                BusinessUnitID = mammothPrice.BusinessUnit.ToString(),
                                MessageID = dbArchiveMessageProperties[Constants.MessageProperty.MessageId],
                                MessageHeaders = receivedMessageProperties,
                                MammothPrice = mammothPrice,
                                MammothMessageID = dbArchiveMessageProperties.ContainsKey(Constants.MessageProperty.MammothMessageId) ? dbArchiveMessageProperties[Constants.MessageProperty.MammothMessageId] : null
                            };

                            string messageDetailJson = JsonConvert.SerializeObject(
                                new Dictionary<string, PriceMessageDetail>() { { "MessageDetail", messageDetail } }
                                );
                            mammothContext.Database.ExecuteSqlCommand(
                               archiveMessageDetailsSqlStatement,
                               new SqlParameter("@ItemId", mammothPrice.ItemId),
                               new SqlParameter("@BusinessUnitId", mammothPrice.BusinessUnit),
                               new SqlParameter("@PriceType", mammothPrice.PriceType),
                               new SqlParameter("@StartDate", mammothPrice.StartDate),
                               new SqlParameter("@MessageId", dbArchiveMessageProperties[Constants.MessageProperty.MessageId]),
                               new SqlParameter("@MessageDetailJson", messageDetailJson)
                            );
                        }
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        private void ArchiveItemPriceXml(
            string itemPriceXml,
            IDictionary<string, string> receivedMessageProperties,
            IDictionary<string, string> dbArchiveMessageProperties
            )
        {
            string messagePropertiesJson = JsonConvert.SerializeObject(
                new Dictionary<string, IDictionary<string, string>>() { { "MessageHeaders", receivedMessageProperties } }
                );
            string archiveItemPriceXmlSqlStatement = @"
                INSERT INTO [esb].[MessageArchive] (MessageID, MessageTypeID, MessageStatusID, MessageHeadersJson, MessageBody)
                SELECT 
                @MessageId,
                (SELECT MessageTypeId FROM esb.MessageType WHERE MessageTypeName = 'Price') as MessageTypeID,
                (SELECT MessageStatusId FROM esb.MessageStatus WHERE MessageStatusName = 'Sent') as MessageStatusID,
                @MessageHeadersJson,
                @MessageBody ";

            using(var mammothContext = this.mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                mammothContext.Database.ExecuteSqlCommand(
                    archiveItemPriceXmlSqlStatement,
                    new SqlParameter("@MessageId", dbArchiveMessageProperties[Constants.MessageProperty.MessageId]),
                    new SqlParameter("@MessageHeadersJson", messagePropertiesJson),
                    new SqlParameter("@MessageBody", itemPriceXml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", ""))
                );
            }
        }
    }
}
