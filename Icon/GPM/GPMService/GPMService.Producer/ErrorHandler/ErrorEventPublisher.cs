using GPMService.Producer.Serializer;
using Icon.Common.Xml;
using Icon.DbContextFactory;
using Icon.Esb.Schemas.Mammoth;
using Mammoth.Framework;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace GPMService.Producer.ErrorHandler
{
    internal class ErrorEventPublisher
    {
        private const int DB_TIMEOUT_IN_SECONDS = 10;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly ISerializer<ErrorMessage> serializer;

        public ErrorEventPublisher(
            IDbContextFactory<MammothContext> mammothContextFactory,
            ISerializer<ErrorMessage> serializer
            )
        {
            this.mammothContextFactory = mammothContextFactory;
            this.serializer = serializer;
        }

        public void PublishErrorEvent(
            string applicationName,
            string messageID,
            Dictionary<string, string> messageProperties,
            string message,
            string errorCode,
            string errorDetails,
            string errorSeverity
            )
        {
            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
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

        public ErrorMessage ConvertToErrorMessageCanonical(Dictionary<string, string> messageProperties)
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
