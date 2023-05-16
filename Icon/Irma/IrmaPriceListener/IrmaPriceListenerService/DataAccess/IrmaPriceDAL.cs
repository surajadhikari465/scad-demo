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
        private readonly IDbContextFactory<IrmaContext> irmaDbContextFactory;
        private readonly IrmaPriceListenerServiceSettings serviceSettings;
        private const int DB_TIMEOUT_IN_SECONDS = 15;

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
            string stagingQuery = @"INSERT INTO [infor].[StagingMammothPrice]
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
            ";
            List<SqlParameter> batchInsertSqlParameters = new List<SqlParameter>();
            int elementIndex = 0;
            foreach (var mammothPrice in mammothPrices)
            {
                DateTime? endDate = ProcessEndDate(mammothPrice);
                SqlParameter businessUnitIdParam = new SqlParameter($"@BusinessUnitId{elementIndex}", mammothPrice.BusinessUnit);
                SqlParameter itemIdParam = new SqlParameter($"@ItemId{elementIndex}", mammothPrice.ItemId);
                SqlParameter multipleParam = new SqlParameter($"@Multiple{elementIndex}", mammothPrice.Multiple);
                SqlParameter priceParam = new SqlParameter($"@Price{elementIndex}", mammothPrice.Price);
                SqlParameter startDateParam = new SqlParameter($"@StartDate{elementIndex}", mammothPrice.StartDate);
                SqlParameter endDateParam = new SqlParameter($"@EndDate{elementIndex}", endDate ?? SqlDateTime.Null);
                SqlParameter priceTypeParam = new SqlParameter($"@PriceType{elementIndex}", mammothPrice.PriceType);
                SqlParameter priceTypeAttributeParam = new SqlParameter($"@PriceTypeAttribute{elementIndex}", mammothPrice.PriceTypeAttribute);
                SqlParameter sellableUomParam = new SqlParameter($"@SellableUom{elementIndex}", mammothPrice.SellableUom);
                SqlParameter actionParam = new SqlParameter($"@Action{elementIndex}", mammothPrice.Action);
                SqlParameter transactionIdParam = new SqlParameter($"@TransactionId{elementIndex}", transactionId);
                batchInsertSqlParameters.Add(businessUnitIdParam);
                batchInsertSqlParameters.Add(itemIdParam);
                batchInsertSqlParameters.Add(multipleParam);
                batchInsertSqlParameters.Add(priceParam);
                batchInsertSqlParameters.Add(startDateParam);
                batchInsertSqlParameters.Add(endDateParam);
                batchInsertSqlParameters.Add(priceTypeParam);
                batchInsertSqlParameters.Add(priceTypeAttributeParam);
                batchInsertSqlParameters.Add(sellableUomParam);
                batchInsertSqlParameters.Add(actionParam);
                batchInsertSqlParameters.Add(transactionIdParam);
                elementIndex++;
                stagingQuery += $@"(
{businessUnitIdParam.ParameterName}, 
{itemIdParam.ParameterName}, 
{multipleParam.ParameterName}, 
{priceParam.ParameterName}, 
{startDateParam.ParameterName}, 
{endDateParam.ParameterName},
{priceTypeParam.ParameterName}, 
{priceTypeAttributeParam.ParameterName}, 
{sellableUomParam.ParameterName}, 
{actionParam.ParameterName}, 
{transactionIdParam.ParameterName}
), ";
            }
            // remove trailing comma and space from SQL command
            stagingQuery = stagingQuery.Remove(stagingQuery.Length - 2);
            using (var irmaContext = irmaDbContextFactory.CreateContext($"Irma_{serviceSettings.IrmaRegionCode}"))
            {
                irmaContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                irmaContext.Database.ExecuteSqlCommand(stagingQuery, batchInsertSqlParameters.ToArray());
            }
        }

        public void LoadMammothPricesSingle(MammothPriceType mammothPrice, string transactionId)
        {
            string StagingQuery = @"INSERT INTO [infor].[StagingMammothPrice]
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
            DateTime? endDate = ProcessEndDate(mammothPrice);
            using (var irmaContext = irmaDbContextFactory.CreateContext($"Irma_{serviceSettings.IrmaRegionCode}"))
            {
                irmaContext.Database.CommandTimeout = DB_TIMEOUT_IN_SECONDS;
                irmaContext.Database.ExecuteSqlCommand(
                StagingQuery,
                new SqlParameter("@BusinessUnitId", mammothPrice.BusinessUnit),
                new SqlParameter("@ItemId", mammothPrice.ItemId),
                new SqlParameter("@Multiple", mammothPrice.Multiple),
                new SqlParameter("@Price", mammothPrice.Price),
                new SqlParameter("@StartDate", mammothPrice.StartDate),
                new SqlParameter("@EndDate", endDate ?? SqlDateTime.Null),
                new SqlParameter("@PriceType", mammothPrice.PriceType),
                new SqlParameter("@PriceTypeAttribute", mammothPrice.PriceTypeAttribute),
                new SqlParameter("@SellableUom", mammothPrice.SellableUom),
                new SqlParameter("@Action", mammothPrice.Action),
                new SqlParameter("@TransactionId", transactionId)
            );
            }
        }

        private static DateTime? ProcessEndDate(MammothPriceType mammothPrice)
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

            return endDate;
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
