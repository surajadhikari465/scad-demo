using AmazonLoad.Common;
using Dapper;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Producer;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
            string saveMessagesDirectory, string nonReceivingSysName, string transactionType, int numberOfRecordsPerEsbMessage, int clientSideProcessGroupCount, int connectionTimeoutSeconds,  bool sendToEsb = true )
        {

            SqlConnection sqlConnection = new SqlConnection(mammothConnectionString);

            // stage data
           StageMammothtemLocales(sqlConnection, region, maxNumberOfRows, numberOfRecordsPerEsbMessage, connectionTimeoutSeconds);

            var numberOfGroups =
                sqlConnection.QueryFirst<int>("select max(GroupID) from stage.ItemLocaleExportStaging where processed = 0", commandTimeout: connectionTimeoutSeconds) + 1;

            Console.WriteLine($"{numberOfGroups} esb message group(s) created");

            var GroupRanges = CalculateRanges(numberOfGroups, clientSideProcessGroupCount);

            Console.WriteLine($"{GroupRanges.Count()} client side data sets will be processed.");

            foreach (var groupRange in GroupRanges)
            {
                Console.WriteLine($"message group range {groupRange.Start}-{groupRange.End}");
                var models =
                    sqlConnection.Query<MammothItemLocaleModel>($"select * from stage.ItemLocaleExportStaging where groupid >= {groupRange.Start} and groupid <= {groupRange.End}  and processed = 0", commandTimeout: connectionTimeoutSeconds).ToList();

                for (int i = groupRange.Start; i <= groupRange.End; i++)
                {
                    var chunk = models.Where(w => w.GroupId == i);
                    // now send the message(s) to the eSB
                    SendMessagesToEsbServerSideGroups(chunk.ToList(), esbProducer, saveMessages, saveMessagesDirectory, nonReceivingSysName, transactionType, sendToEsb);
                }
               
            }

        }

        internal static IEnumerable<GroupRange> CalculateRanges(int numberOfGroups, int batchSize)
        {
            return Enumerable.Range(0, numberOfGroups + 1).Batch(batchSize).Select(e => new GroupRange { Start = e.First(), End = e.Last() });
        }
        
        
        internal static int StageMammothtemLocales(SqlConnection mammothSqlConnection, string region, int maxNumberOfRows, int numberOfRecordsPerEsbMessage, int connectionTimeoutSeconds)
        {
            mammothSqlConnection.FireInfoMessageEventOnUserErrors = true;
            mammothSqlConnection.InfoMessage += (s, e) => { Console.Out.WriteLineAsync($"(sql) {e.Message}"); };

            var paramters = new DynamicParameters();
            paramters.Add("Region", dbType: DbType.String, direction: ParameterDirection.Input, value: region);
            paramters.Add("GroupSize", dbType: DbType.Int32, direction: ParameterDirection.Input, value: numberOfRecordsPerEsbMessage);
            paramters.Add("MaxRows", dbType: DbType.Int32, direction: ParameterDirection.Input, value: maxNumberOfRows);

            var rowCount = mammothSqlConnection.QueryFirst<int>("stage.ItemLocaleExport", paramters, commandTimeout: connectionTimeoutSeconds, commandType: CommandType.StoredProcedure); 
            return rowCount;
        }

        internal static void SendMessagesToEsbServerSideGroups(List<MammothItemLocaleModel> models,
            IEsbProducer esbProducer, bool saveMessages, string saveMessagesDirectory,
            string nonReceivingSysName, string transactionType, bool sendToEsbFlag = true)
        {
            string message = MessageBuilderForMammothItemLocale.BuildMessage(models);
            string messageId = Guid.NewGuid().ToString();
            var headers = new Dictionary<string, string>
            {
                { "IconMessageID", messageId },
                { "Source", "Mammoth"},
                { "nonReceivingSysName", nonReceivingSysName },
                { "TransactionType", transactionType }
            };

            if (!models.Any()) return;

            if (sendToEsbFlag)
            {
                esbProducer.Send(
                    message,
                    messageId,
                    headers);
            }
            NumberOfRecordsSent += models.Count();
            NumberOfMessagesSent += 1;
            if (saveMessages)
            {
                File.WriteAllText($"{saveMessagesDirectory}/{messageId}.xml", JsonConvert.SerializeObject(headers) + Environment.NewLine + message);
            }
        }

        internal static void SendMessagesToEsb(IEnumerable<MammothItemLocaleModel> models,
           IEsbProducer esbProducer, bool saveMessages, string saveMessagesDirectory,
           string nonReceivingSysName, int maxNumberOfRows, string transactionType, bool sendToEsbFlag = true)
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
                    var headers = new Dictionary<string, string>
                    {
                        { "IconMessageID", messageId },
                        { "Source", "Mammoth"},
                        { "nonReceivingSysName", nonReceivingSysName },
                        { "TransactionType", transactionType }
                    };

                    if (sendToEsbFlag)
                    {
                        esbProducer.Send(
                            message,
                            messageId,
                            headers);
                    }
                    NumberOfRecordsSent += modelGroup.Count();
                    NumberOfMessagesSent += 1;
                    if (saveMessages)
                    {
                        File.WriteAllText($"{saveMessagesDirectory}/{messageId}.xml", JsonConvert.SerializeObject(headers) + Environment.NewLine + message);
                    }
                }
            }
        }
    }
}