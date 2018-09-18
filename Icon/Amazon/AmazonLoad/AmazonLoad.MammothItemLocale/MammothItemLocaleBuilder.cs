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

        public static void LoadMammothItemLocalesAndSendMessages(IEsbProducer esbProducer,
            string mammothConnectionString, string region, int maxNumberOfRows, bool saveMessages,
            string saveMessagesDirectory, string nonReceivingSysName, bool sendToEsb = true)
        {
            string formattedSql = GetFormattedSqlForMammothItemLocaleQuery(region, maxNumberOfRows);

            SqlConnection sqlConnection = new SqlConnection(mammothConnectionString);

            // load data
            var models = sqlConnection.Query<MammothItemLocaleModel>(formattedSql, buffered: false, commandTimeout: 60);

            // now send the message(s) to the eSB
            SendMessagesToEsb(models, esbProducer, saveMessages, saveMessagesDirectory,
                nonReceivingSysName, maxNumberOfRows, sendToEsb);
        }

        internal static string GetFormattedSqlForMammothItemLocaleQuery(string region, int maxNumberOfRows)
        {
            string formattedSql = SqlQueries.MammothItemLocaleSql.Replace("{region}", AppSettingsAccessor.GetStringSetting("Region"));
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

        internal static int GetBatchSize(int maxNumberOfRows, int numberOfRecordsSent)
        {
            int batchSize = 100;
            if (maxNumberOfRows != 0 && maxNumberOfRows > 0)
            {
                if (maxNumberOfRows < batchSize)
                {
                    batchSize = maxNumberOfRows;
                }
                else if (numberOfRecordsSent < maxNumberOfRows)
                {
                    if (maxNumberOfRows - numberOfRecordsSent <= batchSize)
                    {
                        batchSize = maxNumberOfRows - numberOfRecordsSent;
                    }
                }
                else if (numberOfRecordsSent >= maxNumberOfRows)
                {
                    return -1;
                }
            }
            return batchSize;
        }

        internal static void SendMessagesToEsb(IEnumerable<MammothItemLocaleModel> models,
           IEsbProducer esbProducer, bool saveMessages, string saveMessagesDirectory,
           string nonReceivingSysName, int maxNumberOfRows, bool sendToEsbFlag = true)
        {
            var batchSize = GetBatchSize(maxNumberOfRows, NumberOfRecordsSent);
            if (batchSize < 0) return;

            foreach (var modelBatch in models.Batch(batchSize))
            {
                foreach (var modelGroup in modelBatch.GroupBy(m => m.BusinessUnitId))
                {
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