using Icon.DbContextFactory;
using Icon.Esb.Schemas.Mammoth;
using IrmaPriceListenerService.Model;
using Mammoth.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace IrmaPriceListenerService.Archive
{
    public class MessageArchiver : IMessageArchiver
    {
        private IDbContextFactory<MammothContext> mammothDbContextFactory;
        private const int DB_TIMEOUT_SECONDS = 10;

        public MessageArchiver(IDbContextFactory<MammothContext> mammothContextFactory)
        {
            this.mammothDbContextFactory = mammothContextFactory;
        }

        public void ArchivePriceMessage(SQSExtendedClientReceiveModel message, IList<MammothPriceType> mammothPrices, IList<MammothPriceWithErrorType> mammothPriceWithErrors)
        {
            ArchiveMessageToDb(message);
            if (mammothPrices != null)
            {
                var pricesAddedWithErrorDetails = AddErrorDetailsToAllPrices(mammothPrices, mammothPriceWithErrors);
                ArchivePricesWithErrorToDb(pricesAddedWithErrorDetails, message);
            }
        }

        private IList<MammothPriceWithErrorType> AddErrorDetailsToAllPrices(
            IList<MammothPriceType> mammothPrices, IList<MammothPriceWithErrorType> existingPricesWithErrorList)
        {
            IList<MammothPriceWithErrorType> mammothPricesAddedWithErrorDetails = new List<MammothPriceWithErrorType>();
            foreach (var mammothPrice in mammothPrices)
            {
                MammothPriceWithErrorType newMammothPriceWithError = new MammothPriceWithErrorType()
                {
                    MammothPrice = mammothPrice
                };

                string mammothPriceJson = JsonConvert.SerializeObject(mammothPrice);

                if (existingPricesWithErrorList != null)
                {
                    var matchedPricesWithError = existingPricesWithErrorList.Where(x => (
                        x.MammothPrice.Region.Equals(mammothPrice.Region) &&
                        x.MammothPrice.ItemId == mammothPrice.ItemId &&
                        x.MammothPrice.BusinessUnit == mammothPrice.BusinessUnit &&
                        x.MammothPrice.GpmId == mammothPrice.GpmId
                    )).ToList();

                    // Hoping matched count is 0 or 1, since Tibco app doesn't implement any logic for multiple matches
                    if (matchedPricesWithError.Count > 0)
                    {
                        newMammothPriceWithError.ErrorCode = matchedPricesWithError[0].ErrorCode;
                        newMammothPriceWithError.ErrorDetails = matchedPricesWithError[0].ErrorDetails;
                    }
                }

                mammothPricesAddedWithErrorDetails.Add(newMammothPriceWithError);
            }
            return mammothPricesAddedWithErrorDetails;
        }

        private void ArchivePricesWithErrorToDb(IList<MammothPriceWithErrorType> mammothPricesWithError, SQSExtendedClientReceiveModel message)
        {
            using (var mammothContext = mammothDbContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_SECONDS;

                string archivePriceWithErrorQuery = @"
                    INSERT INTO esb.PriceMessageArchiveDetail(
	                    MessageAction,
	                    Region,
	                    GpmID,
	                    ItemID,
	                    BusinessUnitID,
	                    MessageID,
	                    JsonObject,
                        ErrorCode,
                        ErrorDetails)
                    VALUES(@MessageAction, @Region, @GpmId, @ItemId, @BusinessUnitId, @MessageId, @MessageJson, @ErrorCode, @ErrorDetails)";

                using (var transaction = mammothContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var mammothPriceWithError in mammothPricesWithError)
                        {
                            var mammothPrice = mammothPriceWithError.MammothPrice;
                            string mammothPriceJson = JsonConvert.SerializeObject(mammothPrice);

                            mammothContext.Database.ExecuteSqlCommand(
                                archivePriceWithErrorQuery,
                                new SqlParameter("@MessageAction", mammothPrice.Action),
                                new SqlParameter("@Region", mammothPrice.Region),
                                new SqlParameter("@GpmId", mammothPrice.GpmId),
                                new SqlParameter("@ItemId", mammothPrice.ItemId),
                                new SqlParameter("@BusinessUnitId", mammothPrice.BusinessUnit),
                                new SqlParameter("@MessageId", message.MessageAttributes[Constants.MessageAttribute.TransactionId]),
                                new SqlParameter("@MessageJson", mammothPriceJson),
                                new SqlParameter("@ErrorCode", (object)mammothPriceWithError.ErrorCode ?? DBNull.Value),
                                new SqlParameter("@ErrorDetails", (object)mammothPriceWithError.ErrorDetails ?? DBNull.Value)
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
        }

        private void ArchiveMessageToDb(SQSExtendedClientReceiveModel message)
        {
            string messageAttributesJson = JsonConvert.SerializeObject(message.MessageAttributes);
            string messageBody = message.S3Details[0].Data.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");

            using (var mammothContext = mammothDbContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_SECONDS;

                string messageArchiveQuery = @"
                    INSERT INTO esb.MessageArchive(
	                    MessageID,
	                    MessageTypeID,
	                    MessageStatusID,
	                    MessageHeadersJson,
	                    MessageBody)
                    SELECT 
                    @MessageId,
                    (SELECT MessageTypeId FROM esb.MessageType WHERE MessageTypeName = @MessageType) as MessageTypeID,
                    (SELECT MessageStatusId FROM esb.MessageStatus WHERE MessageStatusName = 'Sent') as MessageStatusID,
                    @MessageHeadersJson,
                    @MessageBody";

                mammothContext.Database.ExecuteSqlCommand(
                    messageArchiveQuery,
                    new SqlParameter("@MessageId", message.MessageAttributes[Constants.MessageAttribute.TransactionId]),
                    new SqlParameter("@MessageType", "Irma Price"),
                    new SqlParameter("@MessageHeadersJson", messageAttributesJson),
                    new SqlParameter("@MessageBody", messageBody)
                );
            }
        }
    }
}
