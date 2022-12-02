using GPMService.Producer.Model.DBModel;
using GPMService.Producer.Serializer;
using GPMService.Producer.Settings;
using Icon.Common.Xml;
using Icon.DbContextFactory;
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
    internal class EmergencyPriceProcessorDAL : IEmergencyPriceProcessorDAL
    {
        private const int DB_TIMEOUT_IN_SECONDS = 10;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly ISerializer<MammothPriceType> mammothPriceSerializer;
        private readonly ILogger<EmergencyPriceProcessorDAL> logger;
        private readonly RetryPolicy retryPolicy;

        public EmergencyPriceProcessorDAL(
            IDbContextFactory<MammothContext> mammothContextFactory,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ISerializer<MammothPriceType> mammothPriceSerializer,
            ILogger<EmergencyPriceProcessorDAL> logger
            )
        {
            this.mammothContextFactory = mammothContextFactory;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.mammothPriceSerializer = mammothPriceSerializer;
            this.logger = logger;
            this.retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                gpmProducerServiceSettings.DbErrorRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(gpmProducerServiceSettings.DbRetryDelayInMilliseconds)
                );
        }

        public bool EmergencyPricesExist()
        {
            bool emergencyPricesExist = false;
            string emergencyPricesExistsQuery = @"SELECT
	CASE
		WHEN EXISTS (SELECT TOP 1 1 FROM gpm.MessageQueueEmergencyPrice) THEN CAST(1 AS BIT)
		ELSE CAST(0 AS BIT)
	END AS EmergencyPricesExist
";
            retryPolicy.Execute(() =>
            {
                using (var mammothContext = mammothContextFactory.CreateContext())
                {
                    mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                    EmergencyPricesExistQueryModel emergencyPricesExistQueryModel = mammothContext
                    .Database
                    .SqlQuery<EmergencyPricesExistQueryModel>(
                    emergencyPricesExistsQuery
                    ).ToList()[0];
                    emergencyPricesExist = emergencyPricesExistQueryModel.EmergencyPricesExist;
                }
            });
            return emergencyPricesExist;
        }

        public List<GetEmergencyPricesQueryModel> GetEmergencyPrices()
        {
            string dequeueEmergencyPricesStoredProcedure = "EXEC gpm.DequeueEmergencyPrices @emergencyPriceCount";
            List<GetEmergencyPricesQueryModel> emergencyPrices = new List<GetEmergencyPricesQueryModel>();
            retryPolicy.Execute(() =>
            {
                using (var mammothContext = mammothContextFactory.CreateContext())
                {
                    mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                    emergencyPrices = mammothContext.Database.SqlQuery<GetEmergencyPricesQueryModel>(
                        dequeueEmergencyPricesStoredProcedure,
                        new SqlParameter("@emergencyPriceCount", gpmProducerServiceSettings.EmergencyPriceDequeueCount)
                    ).ToList();
                }
            });
            return emergencyPrices;
        }

        public void InsertPricesIntoEmergencyQueue(MammothPricesType emergencyMammothPrices)
        {
            string insertEmergencyPricesSqlStatement = $@"INSERT INTO gpm.MessageQueueEmergencyPrice(ItemId, BusinessUnitId, PriceType, MammothPriceXml)
SELECT @ItemId, @BusinessUnitId, @PriceType, @MammothPriceXml 
WHERE NOT EXISTS (
	SELECT 1
	FROM gpm.MessageQueueEmergencyPrice q
	WHERE q.ItemId = @ItemId
		AND q.BusinessUnitId = @BusinessUnitId
		AND q.PriceType = @PriceType)";
            retryPolicy.Execute(() =>
            {
                using (var mammothContext = mammothContextFactory.CreateContext())
                {
                    mammothContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                    using (var transaction = mammothContext.Database.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < emergencyMammothPrices.MammothPrice.Length; i++)
                            {
                                MammothPriceType mammothPrice = emergencyMammothPrices.MammothPrice[i];
                                mammothContext
                                    .Database
                                    .ExecuteSqlCommand(
                                    insertEmergencyPricesSqlStatement,
                                    new SqlParameter("@ItemId", mammothPrice.ItemId),
                                    new SqlParameter("@BusinessUnitId", mammothPrice.BusinessUnit),
                                    new SqlParameter("@PriceType", mammothPrice.PriceType),
                                    new SqlParameter("@MammothPriceXml", mammothPriceSerializer.Serialize(mammothPrice, new Utf8StringWriter()))
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
        }
    }
}
