using AmazonLoad.Common;
using Dapper;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Producer;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.MammothItemLocale
{
    public static class MammothItemLocaleBuilder
    {
        public static int NumberOfRecordsSent = 0;
        public static int NumberOfMessagesSent = 0;
        internal static int DefaultBatchSize = 100;

        public static void LoadMammothItemLocalesAndSendMessages(IEsbProducer esbProducer,
            string mammothConnectionString, string region, int maxNumberOfRows, bool saveMessages,
            string saveMessagesDirectory, string nonReceivingSysName, bool sendToEsb = true)
        {
            SqlConnection sqlConnection = new SqlConnection(mammothConnectionString);

            // load data
            var models = LoadMammothtemLocales(sqlConnection, region, maxNumberOfRows);

            // now send the message(s) to the eSB
            SendMessagesToEsb(models, esbProducer, saveMessages, saveMessagesDirectory,
                nonReceivingSysName, maxNumberOfRows, sendToEsb);
        }

        internal static string GetFormattedSqlForMammothItemLocaleQuery(string region, int maxNumberOfRows)
        {
            string formattedSql = SqlQueries.MammothItemLocaleSql.Replace("{region}", region);
            if (maxNumberOfRows != 0)
            {
                formattedSql = formattedSql.Replace("{top query}", $"top {maxNumberOfRows}");
            }
            else
            {
                formattedSql = formattedSql.Replace("{top query}", "");
            }
            return formattedSql;
        }
        
        internal static IEnumerable<MammothItemLocaleModel> LoadMammothtemLocales(SqlConnection irmaSqlConnection, string region, int maxNumberOfRows)
        {
            var sql = GetFormattedSqlForMammothItemLocaleQuery(region, maxNumberOfRows);
            var mamothItemLocaleModels = irmaSqlConnection.Query<MammothItemLocaleModel>(sql, buffered: false, commandTimeout: 60);
            return mamothItemLocaleModels;
        }

        internal static void SendMessagesToEsb(IEnumerable<MammothItemLocaleModel> models,
           IEsbProducer esbProducer, bool saveMessages, string saveMessagesDirectory,
           string nonReceivingSysName, int maxNumberOfRows, bool sendToEsbFlag = true)
        {
            var batchSize = Utils.CalcBatchSize(DefaultBatchSize, maxNumberOfRows, NumberOfRecordsSent);
            if (batchSize < 0) return;

            foreach (var modelBatch in models.Batch(batchSize))
            {
                foreach (var modelGroup in modelBatch.GroupBy(m => m.BusinessUnitId))
                {
                    if (maxNumberOfRows != 0 && NumberOfRecordsSent >= maxNumberOfRows) return;
                    string message = MessageBuilderForMammothItemLocale.BuildMessage(modelGroup.ToList());
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