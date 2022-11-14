using Dapper;
using Mammoth.Common.DataAccess.DbProviders;

namespace Mammoth.Esb.AttributeListener.Commands
{
    public class ErrorMessageHandler
    {
        readonly IDbProvider db;

        public ErrorMessageHandler(IDbProvider dbProvider)
        {
            this.db = dbProvider;
        }

        public void HandleErrorMessage(string applicationName,
            string messageID,
            string messageProperties,
            string message,
            string errorCode,
            string errorDetails,
            string errorSeverity)
        {
            const string sql = @"
                INSERT INTO gpm.ErrorMessages (
                    Application,
                    MessageID,
                    MessageProperties,
                    Message,
                    ErrorCode,
                    ErrorDetails,
                    ErrorSeverity
                )
                VALUES (
                    @Application,
                    @MessageID,
                    @MessageProperties,
                    @Message,
                    @ErrorCode,
                    @ErrorDetails,
                    @ErrorSeverity
                )
                ";

            db.Connection.Execute(sql, new {
                applicationName,
                messageID,
                messageProperties,
                message,
                errorCode,
                errorDetails,
                errorSeverity
            });
        }
    }
}
