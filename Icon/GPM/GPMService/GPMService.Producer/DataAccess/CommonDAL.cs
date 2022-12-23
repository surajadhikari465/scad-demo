using GPMService.Producer.Settings;
using Icon.DbContextFactory;
using Icon.Esb.Schemas.Mammoth;
using Icon.Logging;
using Mammoth.Framework;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GPMService.Producer.DataAccess
{
    internal class CommonDAL : ICommonDAL
    {
        private const int DB_TIMEOUT_IN_SECONDS = 10;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly ILogger<CommonDAL> logger;
        private readonly RetryPolicy retryPolicy;
        public CommonDAL(
            IDbContextFactory<MammothContext> mammothContextFactory,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ILogger<CommonDAL> logger
            )
        {
            this.mammothContextFactory = mammothContextFactory;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.logger = logger;
            this.retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                gpmProducerServiceSettings.DbErrorRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(gpmProducerServiceSettings.DbRetryDelayInMilliseconds)
                );
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

        public IEnumerable<MammothPriceType> ArchiveActivePrice(MammothPricesType mammothPrices)
        {
            IEnumerable<MammothPriceType> mammothPricesWithPriceID = mammothPrices
                .MammothPrice
                .Where(x => x.PriceIDSpecified);
            string archiveActivePriceSqlStatement = $@"INSERT INTO gpm.ActivePriceSentArchive (Region, PriceID, InsertDateUtc)
VALUES (@Region, @PriceID, SYSUTCDATETIME())";
            retryPolicy.Execute(() =>
            {
            using (var mammothContext = mammothContextFactory.CreateContext())
                {
                    mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                    using (var transaction = mammothContext.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (MammothPriceType mammothPrice in mammothPricesWithPriceID)
                            {
                                mammothContext
                                    .Database
                                    .ExecuteSqlCommand(
                                    archiveActivePriceSqlStatement,
                                    new SqlParameter("@Region", mammothPrice.Region),
                                    new SqlParameter("@PriceID", mammothPrice.PriceID)
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
            return mammothPricesWithPriceID;
        }
    }
}
