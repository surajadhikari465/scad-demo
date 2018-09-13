using AmazonLoad.Common;
using Dapper;
using Icon.Common;
using Icon.Esb.Producer;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace AmazonLoad.MammothPrice
{
    public static class MammothPriceBuilder
    {
        public static EsbMessageSendResult LoadMammothPricesAndSendMessages(IEsbProducer esbProducer, string connectionString, string region,
            int maxNumberOfRows, bool saveMessages, string saveMessagesDirectory, string nonReceivingSysName)
        {
            // first query the GPM status for the region
            var isGpmActive = IsGpmActive(connectionString, region);

            IEnumerable<IPriceModel> priceModels = null;

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                // load mammoth prices
                if (isGpmActive)
                {
                    priceModels = LoadMammothGpmPrices(sqlConnection, region, maxNumberOfRows);
                }
                else
                {
                    priceModels = LoadMammothNonGpmPrices(sqlConnection, region, maxNumberOfRows);
                }

                // now send the price message(s) to the eSB
                return SendMessagesToEsb(priceModels, esbProducer, saveMessages, saveMessagesDirectory, nonReceivingSysName);
            }
        }

        public static bool IsGpmActive(string connectionString, string region)
        {
            // first query the GPM status for the region
            bool isGpmActive = false;
            string sqlForGpmStatus = SqlQueries.QueryRegionGpmStatusSql.Replace("{region}", region);
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var gpmQueryResults = sqlConnection.Query<bool>(sqlForGpmStatus, buffered: false, commandTimeout: 60);
                if (gpmQueryResults != null && gpmQueryResults.Count() == 1)
                {
                    isGpmActive = gpmQueryResults.Single();
                }
            }

            return isGpmActive;
        }

        public static string GetFormattedSqlQueryForGpmPrices(string region, int maxNumberOfRows)
        {
            // query for gpm price data
            string sqlForGpmPrices = SqlQueries.PriceGpmSql.Replace("{region}", region);
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

        public static string GetFormattedSqlQueryForNonGpmPrices(string region, int maxNumberOfRows)
        {
            string sqlForNonGpmPrices = SqlQueries.PriceSql.Replace("{region}", region);
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

        public static IEnumerable<PriceModelGpm> LoadMammothGpmPrices(SqlConnection sqlConnection, string region, int maxNumberOfRows)
        {
            IEnumerable<PriceModelGpm> priceModels = null;

            // query for gpm price data
            string sqlForGpmPrices = GetFormattedSqlQueryForGpmPrices(region, maxNumberOfRows);

            priceModels = sqlConnection.Query<PriceModelGpm>(sqlForGpmPrices, buffered: false, commandTimeout: 60);

            return priceModels;
        }

        public static IEnumerable<PriceModel> LoadMammothNonGpmPrices(SqlConnection sqlConnection, string region, int maxNumberOfRows)
        {
            IEnumerable<PriceModel> priceModels = null;

            // query for non-gpm price data
            string sqlForNonGpmPrices = GetFormattedSqlQueryForNonGpmPrices(region, maxNumberOfRows);

            //using (var sqlConnection = new SqlConnection(connectionString))
            //{
                priceModels = sqlConnection.Query<PriceModel>(sqlForNonGpmPrices, buffered: false, commandTimeout: 60);
            //}

            return priceModels;
        }

        public static EsbMessageSendResult SendMessagesToEsb(IEnumerable<IPriceModel> models,
            IEsbProducer esbProducer, bool saveMessages, string saveMessagesDirectory, string nonReceivingSysName)
        {
            int numberOfRecordsSent = 0;
            int numberOfMessagesSent = 0;
            foreach (var modelBatch in models.Batch(100))
            {
                foreach (var modelGroup in modelBatch.GroupBy(m => m.BusinessUnitId))
                {
                    string message = string.Empty;
                    if (models.GetType() == typeof(IEnumerable<PriceModel>))
                    {
                        message = MessageBuilderForPreGpmPrice.BuildPreGpmMessage((modelGroup as IEnumerable<PriceModel>).ToList());
                    }
                    else if (models.GetType() == typeof(IEnumerable<PriceModelGpm>))
                    {
                        message = MessageBuilderForGpmPrice.BuildGpmMessage((modelGroup as IEnumerable<PriceModelGpm>).ToList());
                    }
                    string messageId = Guid.NewGuid().ToString();

                    esbProducer.Send(
                        message,
                        messageId,
                        new Dictionary<string, string>
                        {
                            { "IconMessageID", messageId },
                            { "Source", "Icon" },
                            { "nonReceivingSysName", nonReceivingSysName }
                        });
                    numberOfRecordsSent += modelGroup.Count();
                    numberOfMessagesSent += 1;
                    if (saveMessages)
                    {
                        File.WriteAllText($"{saveMessagesDirectory}/{messageId}.xml", message);
                    }
                }
            }

            return new EsbMessageSendResult(numberOfRecordsSent, numberOfMessagesSent);
        }
    }
}
