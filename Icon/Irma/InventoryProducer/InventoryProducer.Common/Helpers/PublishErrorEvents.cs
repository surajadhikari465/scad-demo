using Icon.DbContextFactory;
using InventoryProducer.Common.Schemas;
using InventoryProducer.Common.Serializers;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace InventoryProducer.Common.Helpers
{
    public class PublishErrorEvents
    {
        // publishes errors to gpm.ErrorMessages table using which alert is generated
        public static void SendToMammoth(
            IDbContextFactory<MammothContext> mammothContextFactory,
            string applicationName,
            string messageID,
            Dictionary<string, string> messageProperties,
            string message,
            string errorCode,
            string errorDetails,
            string errorSeverity
            )
        {
            ISerializer<ErrorMessage> serializer = new Serializer<ErrorMessage>();

            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = 10;
                string storeErrorSqlStatement = @"INSERT INTO gpm.ErrorMessages (Application, MessageID, MessageProperties, Message, ErrorCode, ErrorDetails, ErrorSeverity) VALUES (@Application, @MessageID, @MessageProperties, @Message, @ErrorCode, @ErrorDetails, @ErrorSeverity)";
                mammothContext
                    .Database
                    .ExecuteSqlCommand(
                    storeErrorSqlStatement,
                    new SqlParameter("@Application", applicationName),
                    new SqlParameter("@MessageID", messageID),
                    new SqlParameter("@MessageProperties", serializer.Serialize(ConvertToErrorMessageCanonical(messageProperties), new Utf8StringWriter())),
                    new SqlParameter("@Message", message),
                    new SqlParameter("@ErrorCode", errorCode),
                    new SqlParameter("@ErrorDetails", errorDetails),
                    new SqlParameter("@ErrorSeverity", errorSeverity)
                    );
            }
        }

        public static ErrorMessage ConvertToErrorMessageCanonical(Dictionary<string, string> messageProperties)
        {
            ErrorMessage errorMessage = new ErrorMessage
            {
                MessageProperties = new NameValuePair[messageProperties.Count]
            };
            int i = 0;
            foreach (string messageProperty in messageProperties.Keys)
            {
                errorMessage.MessageProperties[i++] = new NameValuePair
                {
                    name = messageProperty,
                    value = messageProperties[messageProperty]
                };
            }
            return errorMessage;
        }
    }
}
