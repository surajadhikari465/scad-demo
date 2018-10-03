using AmazonLoad.Common;
using Icon.Esb.Producer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MoreLinq;

namespace AmazonLoad.PrimeAffinityPsg
{
    public class PrimeAffinityPsgBuilder
    {
        public static int NumberOfRecordsSent = 0;
        public static int NumberOfMessagesSent = 0;
        internal static int DefaultBatchSize = 100;
        internal const string gpmPriceType = "TPR";
        internal const string nonGpmPriceTypes = "'SAL','ISS','FRZ'";
        internal const string exludedPsNumbers = "2100,2200,2220";

        public static void LoadPrimeItemsAndSendMessages(IEsbProducer esbProducer,
            string mammothConnectionString, string region, int maxNumberOfRows, string nonReceivingSysName,
            bool saveMessages, string saveMessagesDirectory, bool sendToEsb = true)
        {
            // first query the GPM status for the region
            var isGpmActive = IsGpmActive(mammothConnectionString, region);

            // get the stores for the region
            var storesInRegion = LoadMammothLocales(mammothConnectionString, region);

            using (SqlConnection sqlConnection = new SqlConnection(mammothConnectionString))
            {
                foreach ( var store in storesInRegion)
                {
                    IEnumerable<PrimeAffinityPsgModel> primePsgs = LoadPrimeAffinityPsgs(
                        sqlConnection,
                        region,
                        store.BusinessUnit.ToString(),
                        isGpmActive,
                        maxNumberOfRows,
                        gpmPriceType,
                        nonGpmPriceTypes, 
                        exludedPsNumbers);

                    // now send the message(s) to the eSB
                    SendMessagesToEsb(
                         models: primePsgs,
                         esbProducer: esbProducer,
                         nonReceivingSysName: nonReceivingSysName,
                         maxNumberOfRows: maxNumberOfRows,
                         saveMessages: saveMessages,
                         saveMessagesDirectory: saveMessagesDirectory,
                         sendToEsbFlag: sendToEsb);
                }
            }
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

        internal static string GetFormattedSqlForPrimeAffinityPsgQuery(string region, bool isGpmActive, string businessUnit,
            int maxNumberOfRows, string gpmPriceType, string nonGpmPriceTypes, string excludedPsNumbers)
        {
            string formattedSql = isGpmActive
                ? SqlQueries.QueryMammothPrimeAffinityPsgsGpmActive
                    .Replace("{region}", region)
                    .Replace("{businessUnit}", businessUnit)
                    .Replace("{gpmPriceType}", gpmPriceType)
                    .Replace("{excluded PSNumbers}", excludedPsNumbers)
                : SqlQueries.QueryMammothPrimeAffinityPsgsGpmInactive
                    .Replace("{region}", region)
                    .Replace("{businessUnit}", businessUnit)
                    .Replace("{excluded PSNumbers}", excludedPsNumbers)
                    .Replace("{nonGpmPriceTypes}", nonGpmPriceTypes);

            if (maxNumberOfRows != 0)
            {
                formattedSql = formattedSql.Replace("{top query}", $"TOP {maxNumberOfRows}");
            }
            else
            {
                formattedSql = formattedSql.Replace("{top query}", "");
            }
            return formattedSql;
        }

        internal static IEnumerable<PrimeAffinityPsgModel> LoadPrimeAffinityPsgs(SqlConnection mammothSqlConnection, string region,
            string businessUnit, bool isGpmActive, int maxNumberOfRows, string gpmPriceType, string nonGpmPriceTypes, string exludedPsNumbers)
        {
            var sql = GetFormattedSqlForPrimeAffinityPsgQuery(region, isGpmActive, businessUnit, maxNumberOfRows, gpmPriceType, nonGpmPriceTypes, exludedPsNumbers);
            var mamothItemLocaleModels = mammothSqlConnection.Query<PrimeAffinityPsgModel>(sql, buffered: false, commandTimeout: 60);

            return mamothItemLocaleModels;
        }

        internal static bool IsGpmActive(string connectionString, string region)
        {
            bool isGpmActive = false;
            string sqlForGpmStatus = GetFormattedSqlQueryFoQueryRegionGpmStatus(region);

            using (SqlConnection mammothSqlConnection = new SqlConnection(connectionString))
            {
                var gpmQueryResults = mammothSqlConnection.Query<bool>(sqlForGpmStatus, buffered: false, commandTimeout: 60);
                if (gpmQueryResults != null && gpmQueryResults.Count() == 1)
                {
                    isGpmActive = gpmQueryResults.Single();
                }
            }

            return isGpmActive;
        }

        internal static string GetFormattedSqlQueryFoQueryRegionGpmStatus(string region)
        {
            return SqlQueries.QueryRegionGpmStatusSql.Replace("{region}", region);
        }

        internal static void SendMessagesToEsb(IEnumerable<PrimeAffinityPsgModel> models, IEsbProducer esbProducer,
            string nonReceivingSysName, int maxNumberOfRows, bool saveMessages, string saveMessagesDirectory, bool sendToEsbFlag = true)
        {
            var batchSize = Utils.CalcBatchSize(DefaultBatchSize, maxNumberOfRows, NumberOfRecordsSent);
            if (batchSize < 0) return;

            foreach (var modelBatch in models.Batch(batchSize))
            {
                foreach (var modelGroup in modelBatch.GroupBy(m => m.BusinessUnit))
                {
                    if (maxNumberOfRows != 0 && NumberOfRecordsSent >= maxNumberOfRows) return;
                    string message = MessageBuilderForPrimeAffinityPsg.BuildMessage(modelGroup);
                    string messageId = Guid.NewGuid().ToString();

                    if (sendToEsbFlag)
                    {
                        esbProducer.Send(
                        message,
                        messageId,
                        new Dictionary<string, string>
                        {
                            { "IconMessageID", messageId },
                            { "Source", "Mammoth"},
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
