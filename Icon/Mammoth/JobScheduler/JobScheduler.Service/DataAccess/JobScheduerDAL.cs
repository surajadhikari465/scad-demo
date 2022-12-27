using Icon.DbContextFactory;
using JobScheduler.Service.Model.DBModel;
using Mammoth.Framework;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace JobScheduler.Service.DataAccess
{
    internal class JobScheduerDAL : IJobScheduerDAL
    {
        private const int DB_TIMEOUT_IN_SECONDS = 10;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;

        public JobScheduerDAL(IDbContextFactory<MammothContext> mammothContextFactory)
        {
            this.mammothContextFactory = mammothContextFactory;
        }

        public List<GetJobSchedulesQueryModel> GetJobSchedules()
        {
            string getJobSchedulesSqlQuery = @"SELECT TOP(1000)
	JobScheduleId ,
	JobName,
	Region,
	DestinationQueueName,
	StartDateTimeUtc,
	LastScheduledDateTimeUtc,
	LastRunEndDateTimeUtc,
	-- DatePart returns number of minutes off of UTC time. This is setup for Central time
	CASE 
		-- 5 hour difference when in Daylight Savings
		WHEN DATEPART(TZoffset, SYSDATETIMEOFFSET()) = -300 THEN DATEADD(hour, -1, NextScheduledDateTimeUtc)
		ELSE NextScheduledDateTimeUtc
	END AS NextScheduledDateTimeUtc,
	IntervalInSeconds,
	Enabled,
	Status,
	XmlObject
FROM [app].[JobSchedule]
WHERE StartDateTimeUtc <=SYSUTCDATETIME()
	AND NextScheduledDateTimeUtc <=SYSUTCDATETIME()
	AND Enabled = 1
	AND Status = 'ready'
	AND ISNULL(DestinationQueueName, '') <> ''
";
            List<GetJobSchedulesQueryModel> jobSchedules = new List<GetJobSchedulesQueryModel>();
            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                jobSchedules = mammothContext.Database.SqlQuery<GetJobSchedulesQueryModel>(getJobSchedulesSqlQuery).ToList();
            }
            return jobSchedules;
        }

        public void UpdateLastRunDateTime(int jobScheduleId)
        {
            string updateLastRunDateTimeSqlQeury = @"	UPDATE  [app].[JobSchedule]
	SET LastScheduledDateTimeUtc = NextScheduledDateTimeUtc
		,NextScheduledDateTimeUtc = DATEADD(ss, CEILING(CAST(DATEDIFF(ss, NextScheduledDateTimeUtc, SYSUTCDATETIME()) as FLOAT) / IntervalInSeconds) * IntervalInSeconds, NextScheduledDateTimeUtc)
	WHERE JobScheduleID = @JobScheduleID";
            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                mammothContext
                    .Database
                    .ExecuteSqlCommand(
                    updateLastRunDateTimeSqlQeury,
                    new SqlParameter("@JobScheduleID", jobScheduleId)
                    );
            }
        }
    }
}
