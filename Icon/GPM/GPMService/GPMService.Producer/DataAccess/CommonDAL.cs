using GPMService.Producer.Settings;
using Icon.DbContextFactory;
using Icon.Logging;
using Mammoth.Framework;
using System.Data.SqlClient;

namespace GPMService.Producer.DataAccess
{
    internal class CommonDAL : ICommonDAL
    {
        private const int DB_TIMEOUT_IN_SECONDS = 10;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly ILogger<CommonDAL> logger;
        public CommonDAL(
            IDbContextFactory<MammothContext> mammothContextFactory,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ILogger<CommonDAL> logger
            )
        {
            this.mammothContextFactory = mammothContextFactory;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.logger = logger;
        }
        public void UpdateStatusToReady(int jobScheduleID)
        {
            string updateStatusToReadySqlStatement = $@"update app.JobSchedule
set status = 'ready', LastRunEndDateTimeUtc = GETUTCDATE()
where JobScheduleId  = @JobScheduleId";
            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                mammothContext
                        .Database
                        .ExecuteSqlCommand(
                        updateStatusToReadySqlStatement,
                        new SqlParameter("@JobScheduleId", jobScheduleID)
                        );
            }
        }

        public void UpdateStatusToRunning(int jobScheduleID)
        {
            string updateStatusToRunningSqlStatement = $@"update app.JobSchedule 
set status = 'running' 
where JobScheduleId = @JobScheduleId";
            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                mammothContext
                        .Database
                        .ExecuteSqlCommand(
                        updateStatusToRunningSqlStatement,
                        new SqlParameter("@JobScheduleId", jobScheduleID)
                        );
            }
        }
    }
}
