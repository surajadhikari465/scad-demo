using AmazonLoad.Common;
using Dapper;
using Icon.Esb.Producer;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace AmazonLoad.MammothPrice
{
    public static class MammothPriceBuilder
    {
        public static int NumberOfRecordsSent = 0;
        public static int NumberOfMessagesSent = 0;
        internal static int DefaultBatchSize = 100;

        public static EsbMessageSendResult LoadMammothPricesAndSendMessages(IEsbProducer esbProducer,
            string mammothConnectionString, string region, int maxNumberOfRows,
            bool saveMessages, string saveMessagesDirectory, string nonReceivingSysName, bool sendToEsb = true)
        {
            IEnumerable<PriceModel> legacyPriceModels = null;
            IEnumerable<PriceModelGpm> gpmPriceModels = null;

            using (var sqlConnection = new SqlConnection(mammothConnectionString))
            {
                // first query the GPM status for the region
                var isGpmActive = IsGpmActive(sqlConnection, region);

                // query for stores in region
                var storesInRegion = LoadMammothLocales(mammothConnectionString, region);

                foreach (var store in storesInRegion)
                {
                    // load mammoth prices
                    if (isGpmActive)
                    {
                        // load mammoth prices
                        gpmPriceModels = LoadMammothGpmPrices(sqlConnection, region, store.BusinessUnit.ToString(), maxNumberOfRows);

                        // now send the price message(s) to the eSB
                        MammothPriceBuilder.SendMessagesToEsb(
                            gpmPriceModels: gpmPriceModels,
                            esbProducer: esbProducer,
                            saveMessages: saveMessages,
                            saveMessagesDirectory: saveMessagesDirectory,
                            nonReceivingSysName: nonReceivingSysName,
                            maxNumberOfRows: maxNumberOfRows,
                            sendToEsb: sendToEsb);
                    }
                    else
                    {
                        // load mammoth prices
                        legacyPriceModels = LoadMammothNonGpmPrices(sqlConnection, region, store.BusinessUnit.ToString(), maxNumberOfRows);

                        // now send the price message(s) to the eSB
                        MammothPriceBuilder.SendMessagesToEsb(
                            legacyPriceModels: legacyPriceModels,
                            esbProducer: esbProducer,
                            saveMessages: saveMessages,
                            saveMessagesDirectory: saveMessagesDirectory,
                            nonReceivingSysName: nonReceivingSysName,
                            maxNumberOfRows: maxNumberOfRows,
                            sendToEsb: sendToEsb);
                    }
                }
            }
            return new EsbMessageSendResult(NumberOfRecordsSent, NumberOfMessagesSent);
        }

        internal static bool IsGpmActive(SqlConnection sqlConnection, string region)
        {
            bool isGpmActive = false;
            string sqlForGpmStatus = GetFormattedSqlQueryFoQueryRegionGpmStatus(region);

            var gpmQueryResults = sqlConnection.Query<bool>(sqlForGpmStatus, buffered: false, commandTimeout: 60);
            if (gpmQueryResults != null && gpmQueryResults.Count() == 1)
            {
                isGpmActive = gpmQueryResults.Single();
            }

            return isGpmActive;
        }

        internal static string GetFormattedSqlQueryFoQueryRegionGpmStatus(string region)
        {
            return SqlQueries.QueryRegionGpmStatusSql.Replace("{region}", region);
        }

        internal static List<MammothLocaleModel> LoadMammothLocales(string connectionString, string region)
        {
            var sql = GetFormattedSqlForMammothLocalesQuery(region);

            var mamothItemLocaleModels = new List<MammothLocaleModel>();

            using (SqlConnection mammothSqlConnection = new SqlConnection(connectionString))
            {
                mamothItemLocaleModels = mammothSqlConnection.Query<MammothLocaleModel>(sql, buffered: false, commandTimeout: 60).ToList();
            }

            return mamothItemLocaleModels;
        }


        internal static string GetFormattedSqlForMammothLocalesQuery(string region)
        {
            string formattedSql = SqlQueries.QueryMammothLocalesByRegionSql
                    .Replace("{region}", region);
            return formattedSql;
        }

        internal static string GetFormattedSqlQueryForGpmPrices(string region, string businessUnit, int maxNumberOfRows)
        {
            // query for gpm price data
            string sqlForGpmPrices = SqlQueries.PriceGpmSql
                .Replace("{region}", region)
                .Replace("{businessUnit}", businessUnit);
            if (maxNumberOfRows != 0)
            {
                sqlForGpmPrices = sqlForGpmPrices.Replace("{top query}", $"top {maxNumberOfRows}");
            }
            else
            {
                sqlForGpmPrices = sqlForGpmPrices.Replace("{top query}", "");
            }
            return sqlForGpmPrices;
        }

        internal static string GetFormattedSqlQueryForNonGpmPrices(string region, string businessUnit, int maxNumberOfRows)
        {
            string sqlForNonGpmPrices = SqlQueries.PriceSql
                .Replace("{region}", region)
                .Replace("{businessUnit}", businessUnit);
            if (maxNumberOfRows != 0)
            {
                sqlForNonGpmPrices = sqlForNonGpmPrices.Replace("{top query}", $"top {maxNumberOfRows}");
            }
            else
            {
                sqlForNonGpmPrices = sqlForNonGpmPrices.Replace("{top query}", "");
            }
            return sqlForNonGpmPrices;
        }

        public static IEnumerable<PriceModelGpm> LoadMammothGpmPrices(SqlConnection sqlConnection, string region, string businessUnit, 
            int maxNumberOfRows)
        {
            IEnumerable<PriceModelGpm> priceModels = null;

            // query for gpm price data
            string sqlForGpmPrices = GetFormattedSqlQueryForGpmPrices(region, businessUnit, maxNumberOfRows);

            priceModels = sqlConnection.Query<PriceModelGpm>(sqlForGpmPrices, buffered: false, commandTimeout: 60);

            return priceModels;
        }

        public static IEnumerable<PriceModel> LoadMammothNonGpmPrices(SqlConnection sqlConnection, string region, string businessUnit, int maxNumberOfRows)
        {
            IEnumerable<PriceModel> priceModels = null;

            // query for non-gpm price data
            string sqlForNonGpmPrices = GetFormattedSqlQueryForNonGpmPrices(region, businessUnit, maxNumberOfRows);

            priceModels = sqlConnection.Query<PriceModel>(sqlForNonGpmPrices, buffered: false, commandTimeout: 60);

            return priceModels;
        }

        internal static void SendMessagesToEsb(IEnumerable<PriceModelGpm> gpmPriceModels,
            IEsbProducer esbProducer, bool saveMessages, string saveMessagesDirectory,
            string nonReceivingSysName, int maxNumberOfRows, bool sendToEsb = true)
        {
            var batchSize = Utils.CalcBatchSize(DefaultBatchSize, maxNumberOfRows, NumberOfRecordsSent);
            if (batchSize < 0) return;

            foreach (var modelBatch in gpmPriceModels.Batch(batchSize))
            {
                if (maxNumberOfRows != 0 && NumberOfRecordsSent >= maxNumberOfRows) return;

                foreach (var modelGroup in modelBatch.GroupBy(m => m.BusinessUnitId))
                {
                    string message = MessageBuilderForGpmPrice.BuildGpmMessage(modelGroup);
                    string messageId = Guid.NewGuid().ToString();

                    if (sendToEsb)
                    {
                        esbProducer.Send(
                        message,
                        messageId,
                        new Dictionary<string, string>
                        {
                            { "IconMessageID", messageId },
                            { "Source", "Icon" },
                            { "nonReceivingSysName", nonReceivingSysName }
                        });
                    }
                    NumberOfRecordsSent += modelGroup.Count();
                    NumberOfMessagesSent += 1;
                    if (saveMessages)
                    {
                        File.WriteAllText($"{saveMessagesDirectory}/{messageId}.xml", message);
                    }
                }
            }
        }

        internal static void SendMessagesToEsb(IEnumerable<PriceModel> legacyPriceModels,
            IEsbProducer esbProducer, bool saveMessages, string saveMessagesDirectory,
            string nonReceivingSysName, int maxNumberOfRows, bool sendToEsb = true)
        {
            var batchSize = Utils.CalcBatchSize(DefaultBatchSize, maxNumberOfRows, NumberOfRecordsSent);
            if (batchSize < 0) return;

            foreach (var modelBatch in legacyPriceModels.Batch(batchSize))
            {
                if (maxNumberOfRows != 0 && NumberOfRecordsSent >= maxNumberOfRows) return;

                foreach (var modelGroup in modelBatch.GroupBy(m => m.BusinessUnitId))
                {
                    string message = MessageBuilderForPreGpmPrice.BuildPreGpmMessage(modelGroup);
                    string messageId = Guid.NewGuid().ToString();

                    if (sendToEsb)
                    {
                        esbProducer.Send(
                        message,
                        messageId,
                        new Dictionary<string, string>
                        {
                            { "IconMessageID", messageId },
                            { "Source", "Icon" },
                            { "nonReceivingSysName", nonReceivingSysName }
                        });
                    }
                    NumberOfRecordsSent += modelGroup.Count();
                    NumberOfMessagesSent += 1;
                    if (saveMessages)
                    {
                        File.WriteAllText($"{saveMessagesDirectory}/{messageId}.xml", message);
                    }
                }
            }
        }
    }
}
