using Icon.DbContextFactory;
using Icon.Esb.Schemas.Mammoth;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Irma.Framework;
using System.Data.SqlTypes;

namespace IrmaPriceListenerService.DataAccess
{
    public class IrmaPriceDAL : IIrmaPriceDAL
    {
        private IDbContextFactory<IrmaContext> irmaDbContextFactory;
        private IrmaPriceListenerServiceSettings serviceSettings;
        private const int DB_TIMEOUT_IN_SECONDS = 15;

        private const string StagingQuery = @"INSERT INTO [infor].[StagingMammothPrice]
            ([BusinessUnit_ID]
            ,[ItemId]
            ,[Multiple]
            ,[Price]
            ,[StartDate]
            ,[EndDate]
            ,[PriceType]
            ,[PriceTypeAttribute]
            ,[SellableUOM]
	        ,[Action]
	        , [TransactionId])
        VALUES
            (@BusinessUnitId,
            @ItemId, 
            @Multiple, 
            @Price, 
            @StartDate, 
            @EndDate, 
            @PriceType, 
            @PriceTypeAttribute, 
            @SellableUom,
            @Action, 
            @TransactionId)";

        public IrmaPriceDAL(IDbContextFactory<IrmaContext> irmaDbContextFactory, IrmaPriceListenerServiceSettings serviceSettings)
        {
            this.irmaDbContextFactory = irmaDbContextFactory;
            this.serviceSettings = serviceSettings;
        }

        public void DeleteStagedMammothPrices(string transactionId)
        {
            const string deleteStagedRecordsQuery = @"DELETE infor.StagingMammothPrice
                WHERE TransactionId = @mammothTransactionId

                DELETE infor.StagingIrmaPrice
                WHERE TransactionId = @irmaTransactionId";

            using (var irmaContext = irmaDbContextFactory.CreateContext($"Irma_{serviceSettings.IrmaRegionCode}"))
            {
                irmaContext.Database.ExecuteSqlCommand(
                    deleteStagedRecordsQuery,
                    new SqlParameter("@mammothTransactionId", transactionId),
                    new SqlParameter("@irmaTransactionId", transactionId)
                );
            }

        }

        public void LoadMammothPricesBatch(IList<MammothPriceType> mammothPrices, string transactionId)
        {
            using (var irmaContext = irmaDbContextFactory.CreateContext($"Irma_{serviceSettings.IrmaRegionCode}"))
            {
                irmaContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                using (var transaction = irmaContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var mammothPrice in mammothPrices)
                        {
                            LoadMammothPricesSingle(mammothPrice, transactionId, irmaContext);
                        }
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public void LoadMammothPricesSingle(MammothPriceType mammothPrice, string transactionId)
        {
            using (var irmaContext = irmaDbContextFactory.CreateContext($"Irma_{serviceSettings.IrmaRegionCode}"))
            {
                LoadMammothPricesSingle(mammothPrice, transactionId, irmaContext);
            }
        }

        private void LoadMammothPricesSingle(MammothPriceType mammothPrice, string transactionId, IrmaContext irmaContext)
        {
            DateTime? endDate = null;
            if (mammothPrice.EndDate.HasValue)
            {
                if (Constants.Action.Delete.Equals(mammothPrice.Action.ToUpper())
                    && mammothPrice.EndDate.Value > DateTime.Today)
                {
                    endDate = DateTime.Today;
                }
                else
                {
                    // Yesterday's date
                    endDate = mammothPrice.EndDate.Value.Add(-(TimeSpan.FromHours(23) + TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(59)));
                }
            }

            irmaContext.Database.ExecuteSqlCommand(
                StagingQuery,
                new SqlParameter("@BusinessUnitId", mammothPrice.BusinessUnit),
                new SqlParameter("@ItemId", mammothPrice.ItemId),
                new SqlParameter("@Multiple", mammothPrice.Multiple),
                new SqlParameter("@Price", mammothPrice.Price),
                new SqlParameter("@StartDate", mammothPrice.StartDate),
                new SqlParameter("@EndDate", endDate.HasValue? endDate.Value : SqlDateTime.Null),
                new SqlParameter("@PriceType", mammothPrice.PriceType),
                new SqlParameter("@PriceTypeAttribute", mammothPrice.PriceTypeAttribute),
                new SqlParameter("@SellableUom", mammothPrice.SellableUom),
                new SqlParameter("@Action", mammothPrice.Action),
                new SqlParameter("@TransactionId", transactionId)
            );
        }

        public void UpdateIrmaPrice(string transactionId)
        {
            string updateIrmaPriceStoredProcedure = "EXEC infor.BatchUpdatePriceFromGPM @transactionId";

            using (var irmaContext = irmaDbContextFactory.CreateContext($"Irma_{serviceSettings.IrmaRegionCode}"))
            {
                irmaContext.Database.ExecuteSqlCommand(
                    updateIrmaPriceStoredProcedure,
                    new SqlParameter("@transactionId", transactionId)
                );
            }
        }
    }
}
