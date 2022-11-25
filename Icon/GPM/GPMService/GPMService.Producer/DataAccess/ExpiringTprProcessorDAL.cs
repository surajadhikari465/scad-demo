using GPMService.Producer.Model.DBModel;
using GPMService.Producer.Settings;
using Icon.DbContextFactory;
using Icon.Logging;
using Mammoth.Framework;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GPMService.Producer.DataAccess
{
    internal class ExpiringTprProcessorDAL : IExpiringTprProcessorDAL
    {
        private const int DB_TIMEOUT_IN_SECONDS = 10;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly ILogger<ExpiringTprProcessorDAL> logger;

        public ExpiringTprProcessorDAL(
            IDbContextFactory<MammothContext> mammothContextFactory,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ILogger<ExpiringTprProcessorDAL> logger
            )
        {
            this.mammothContextFactory = mammothContextFactory;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.logger = logger;
        }
        public IEnumerable<GetExpiringTprsQueryModel> GetExpiringTprs(MammothContext mammothContext, string region)
        {
            string getExpiringTprsSqlQuery = $@"SELECT
	CAST(endingPrice.Region AS NVARCHAR(2)) AS Region,
	endingPrice.PriceID,
	endingPrice.GpmID,
	endingPrice.ItemID,
	endingPrice.BusinessUnitID,
	endingPrice.StartDate,
	endingPrice.EndDate,
	endingPrice.Price,
	endingPrice.PriceType,
	endingPrice.PriceTypeAttribute,
	endingPrice.SellableUOM,
	endingPrice.CurrencyCode,
	endingPrice.Multiple,
	endingPrice.TagExpirationDate,
	endingPrice.InsertDateUtc,
	endingPrice.ModifiedDateUtc,
	it.itemTypeCode as ItemTypeCode,
	i.ScanCode as ScanCode,
	lc.StoreName as StoreName,
	PSNumber as SubTeamNumber,
	PercentOff as PercentOff
FROM gpm.Prices endingPrice
INNER JOIN Items i on i.ItemID = endingPrice.ItemID
INNER JOIN ItemTypes it on i.ItemTypeID = it.itemTypeID
INNER JOIN Locale lc on lc.Region = @Region1 
	AND endingPrice.BusinessUnitID = lc.BusinessUnitID
WHERE endingPrice.Region = @Region2
AND endingPrice.PriceType = 'TPR'
AND endingPrice.EndDate = CAST(DATEADD(s, -1, CAST(CAST(SYSDATETIME() AS DATE) AS datetime2)) as datetime2)
AND NOT EXISTS
	(SELECT 1
	FROM gpm.Prices activePrice 
	WHERE activePrice.Region = @Region3
		AND activePrice.StartDate = CAST(CAST(SYSDATETIME() AS DATE) AS datetime2)
		AND activePrice.PriceType = 'TPR'
		AND endingPrice.Region = activePrice.Region
		AND endingPrice.ItemID = activePrice.ItemID
		AND endingPrice.BusinessUnitID = activePrice.BusinessUnitID
		AND endingPrice.PriceType = activePrice.PriceType)
ORDER BY  endingPrice.BusinessUnitId,  endingPrice.ItemId";
            mammothContext.Database.CommandTimeout = gpmProducerServiceSettings.JdbcQueryTimeoutInSeconds;
            return mammothContext
                .Database
                .SqlQuery<GetExpiringTprsQueryModel>(
                getExpiringTprsSqlQuery,
                new SqlParameter("@Region1", region),
                new SqlParameter("@Region2", region),
                new SqlParameter("@Region3", region)
                ).AsEnumerable();
        }
    }
}
