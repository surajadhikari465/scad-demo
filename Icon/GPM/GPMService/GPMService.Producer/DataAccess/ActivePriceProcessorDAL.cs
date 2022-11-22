using GPMService.Producer.ErrorHandler;
using GPMService.Producer.Settings;
using Icon.DbContextFactory;
using Icon.Logging;
using Mammoth.Framework;
using Polly.Retry;
using System.Data.SqlClient;
using GPMService.Producer.Model.DBModel;
using System.Linq;
using System.Collections.Generic;

namespace GPMService.Producer.DataAccess
{
    internal class ActivePriceProcessorDAL : IActivePriceProcessorDAL
    {
        private const int DB_TIMEOUT_IN_SECONDS = 10;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly ILogger<ActivePriceProcessorDAL> logger;
        private readonly ErrorEventPublisher errorEventPublisher;

        public ActivePriceProcessorDAL(
            IDbContextFactory<MammothContext> mammothContextFactory,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ILogger<ActivePriceProcessorDAL> logger,
            ErrorEventPublisher errorEventPublisher
            )
        {
            this.mammothContextFactory = mammothContextFactory;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.logger = logger;
            this.errorEventPublisher = errorEventPublisher;
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
        public IEnumerable<GetActivePricesQueryModel> GetActivePrices(MammothContext mammothContext, string region)
        {
            string getActivePricesSqlQuery = $@"SELECT 
	PriceID, 
	CAST(p.Region AS NVARCHAR(2)) AS Region, 
	CAST(GpmID AS NVARCHAR(70)) AS GpmID, 
	p.ItemID, 
	p.BusinessUnitID, 
	StartDate, 
	EndDate, 
	Price, 
	PriceType, 
	PriceTypeAttribute, 
	SellableUOM, 
	CurrencyCode, 
	Multiple, 
	TagExpirationDate,
	InsertDateUtc, 
	ModifiedDateUtc,
	it.itemTypeCode as ItemTypeCode,
	Items.ScanCode as ScanCode,
	lc.StoreName as StoreName,
	PSNumber as SubTeamNumber,
       PercentOff as PercentOff
FROM gpm.Prices p
INNER JOIN Items on Items.ItemID = p.ItemID
INNER JOIN ItemTypes it on Items.ItemTypeID = it.itemTypeID
INNER JOIN Locale lc on p.BusinessUnitID = lc.BusinessUnitID
WHERE p.Region = @Region
	AND it.itemTypeCode NOT IN ( 'CPN', 'NRT')
	AND StartDate = CAST(GETDATE() AS date)
	AND NOT EXISTS
	(
		SELECT 1 
		FROM gpm.ActivePriceSentArchive a
		WHERE a.Region = @RegionArchive
			AND a.PriceID = p.PriceID
	)
	ORDER BY BusinessUnitID, p.ItemID, PriceType
	OPTION(RECOMPILE)";
            mammothContext.Database.CommandTimeout = gpmProducerServiceSettings.JdbcQueryTimeoutInSeconds;
            return mammothContext
                .Database
                .SqlQuery<GetActivePricesQueryModel>(
                getActivePricesSqlQuery,
                new SqlParameter("@Region", region),
                new SqlParameter("@RegionArchive", region)
                ).AsEnumerable();
        }
    }
}
