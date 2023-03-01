using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ErrorMessagesMonitor.Model;
using Icon.DbContextFactory;
using Mammoth.Framework;
namespace ErrorMessagesMonitor.DataAccess
{
    internal class ErrorMessagesMonitorDAL : IErrorMessagesMonitorDAL
    {
        private const int DB_TIMEOUT_IN_SECONDS = 10;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;

        public ErrorMessagesMonitorDAL(IDbContextFactory<MammothContext> mammothContextFactory)
        {
            this.mammothContextFactory = mammothContextFactory;
        }

        public void MarkErrorMessageRecordsAsInProcess(string instanceID)
        {
            string markErrorMessageRecordsAsInProcess =
                $@"UPDATE 
                    gpm.ErrorMessages
                SET InstanceID = @InstanceID
                WHERE 
                    ProcessedDateUtc is null AND 
                    InstanceID is null";

            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                mammothContext.Database.ExecuteSqlCommand(
                    markErrorMessageRecordsAsInProcess,
                    new SqlParameter("@InstanceID", instanceID)
                );
            }
        }

        public IList<ErrorMessageModel> GetErrorMessages(string instanceID)
        {
            string getErrorMessages =
                $@"SELECT
            	    Application,
            	    ErrorCode,
            	    ErrorSeverity,
            	    COUNT(*) NumberOfErrors
                FROM gpm.ErrorMessages
                WHERE InstanceID = @InstanceID
                GROUP BY Application, ErrorCode, ErrorSeverity";

            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                return mammothContext.Database.SqlQuery<ErrorMessageModel>(
                    getErrorMessages,
                    new SqlParameter("@InstanceID", instanceID)
                ).ToList();
            }
        }

        public IList<ErrorDetailsModel> GetErrorDetails(string instanceID, ErrorMessageModel errorMessage)
        {
            string getErrorDetails =
                $@"SELECT
                    MessageID, 
                    ErrorDetails 
                FROM gpm.ErrorMessages
                WHERE
                    InstanceID = @InstanceID AND 
                    Application = @Application AND 
                    ErrorCode = @ErrorCode AND 
                    ErrorSeverity = @ErrorSeverity";

            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                return mammothContext.Database.SqlQuery<ErrorDetailsModel>(
                    getErrorDetails,
                    new SqlParameter("@InstanceID", instanceID),
                    new SqlParameter("@Application", errorMessage.Application),
                    new SqlParameter("@ErrorCode", errorMessage.ErrorCode),
                    new SqlParameter("@ErrorSeverity", errorMessage.ErrorSeverity)
                ).ToList();
            }
        }

        public void MarkErrorMessagesAsProcessed(string instanceID)
        {
            string markErrorMessagesAsProcessed =
                $@"UPDATE gpm.ErrorMessages
            SET ProcessedDateUtc = SYSUTCDATETIME()
            WHERE InstanceID = @InstanceID";

            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                mammothContext.Database.ExecuteSqlCommand(
                    markErrorMessagesAsProcessed,
                    new SqlParameter("@InstanceID", instanceID)
                );
            }
        }
    }
}
