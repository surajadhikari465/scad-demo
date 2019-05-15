using AmazonLoad.Common;
using Dapper;
using Icon.Esb.Producer;
using MoreLinq;
using Newtonsoft.Json;
using OutputColorizer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using System.Globalization;
using System.Configuration;

namespace AmazonLoad.MammothItemLocale
{
    public static class MammothItemLocaleBuilder
    {
        public static int NumberOfRecordsSent = 0;
        public static int NumberOfMessagesSent = 0;
        internal static int DefaultBatchSize = 100;
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public static void CheckStagingTable(string mammothConnectionString, int timeoutInSeconds)
        {
            using (var connection = new SqlConnection(mammothConnectionString))
            {
                var info = GetStagingTableInfo(connection, timeoutInSeconds);

                logger.Info($"Staging Table has{info.TotalRecords} record(s)");

                if (info.UnprocessedRecords > 0)
                {
                    logger.Info($"{info.UnprocessedRecords} record(s) have not been processed");
                    logger.Info($"{info.UnProcessedGroupIds.Count()} message groups have not been processed");
                }
                else
                {
                    logger.Info($"All records have been processed.");
                }
                    logger.Info("");
            }
     
        }

        internal static StagingTableInfo GetStagingTableInfo(SqlConnection connection, int timeoutInSeconds)
        {
            logger.Info("Checking status of staging table...");

            var info = new StagingTableInfo();

            info.TotalRecords= connection.QueryFirst<int>("select count(*)  from stage.ItemLocaleExportStaging with (nolock)", commandTimeout: timeoutInSeconds);
            if (info.TotalRecords > 0)
            {
                info.UnprocessedRecords = connection.QueryFirst<int>("select count(*) from stage.ItemLocaleExportStaging with (nolock) where Processed =0 ", commandTimeout: timeoutInSeconds);
                var groups = connection.Query<int?>("select distinct GroupId from stage.ItemLocaleExportStaging with (nolock) where Processed = 0", commandTimeout: timeoutInSeconds);
                info.UnProcessedGroupIds = groups.Any(g => g.HasValue) ? groups.Where(g => g.HasValue).Select(g=> g.Value).ToList() : new List<int>();
            }
                
            return info;
        }


        public static void ClearStagingTable(string mammothConnectionString, int commandTimeoutInSeconds)
        {

            using (SqlConnection sqlConnection = new SqlConnection(mammothConnectionString))
            {

                var stagingInfo = GetStagingTableInfo(sqlConnection, commandTimeoutInSeconds);

                if (stagingInfo.UnprocessedRecords > 0)
                {
                    logger.Info($"Staging table contains {stagingInfo.UnprocessedRecords} unprocessed record(s)");
                    logger.Info($"They will be cleared in 10 seconds. Press Control-C to Cancel...");

                    Enumerable.Range(0, 10).ForEach(x => Thread.Sleep(1000));
                }
                sqlConnection.Execute("truncate table stage.ItemLocaleExportStaging", commandTimeout: commandTimeoutInSeconds);
            }
        }


        public static void ResetProcessedRecords(string mammothConnectionString, int timeoutInSeconds)
        {
            using (SqlConnection sqlConnection = new SqlConnection(mammothConnectionString))
            {
                var stagingInfo = GetStagingTableInfo(sqlConnection, timeoutInSeconds);

                if (stagingInfo.UnprocessedRecords > 0)
                {
                    logger.Info($"Staging table contains {stagingInfo.TotalRecords} record(s)");
                    logger.Info($"Processed flags will be cleared in 10 seconds. Press Control-C to Cancel...");

                    Enumerable.Range(0, 10).ForEach(x => Thread.Sleep(1000));
                }

                sqlConnection.Execute("update stage.ItemLocaleExportStaging set Processed = 0");
            }
        }
        public static void StageRecords(string mammothConnectionString, int timeoutInSeconds, string region, int maxNumberOfRows, int numberOfRecordsPerEsbMessage)
        {
            using (SqlConnection sqlConnection = new SqlConnection(mammothConnectionString))
            {
                var stagingInfo = GetStagingTableInfo(sqlConnection, timeoutInSeconds);
                if (stagingInfo.UnprocessedRecords > 0)
                {
                    logger.Info($"Staging table contains {stagingInfo.UnprocessedRecords} unprocessed record(s)");
                    logger.Info($"They will be cleared in 10 seconds. Press Control-C to Cancel...");

                    Enumerable.Range(0, 10).ForEach(x => Thread.Sleep(1000));
                }

                StageMammothtemLocales(sqlConnection, region, maxNumberOfRows, numberOfRecordsPerEsbMessage, timeoutInSeconds);
                
            }
        }


        public static void SendOpsGenieAlert(string message, string description, Dictionary<string, string> details = null)
        {

            var sendOpsGenie = ConfigurationManager.AppSettings["SendOpsGenieAlert"];

            if (sendOpsGenie.ToLower() == "true")
            {
                var alert = new OpsgenieTrigger();
                alert.TriggerAlert(message, description, details);
            }
        }

        
            public static void ProcessMammothItemLocalesAndSendMessages(IEsbProducer esbProducer,
            string mammothConnectionString, string region, int maxNumberOfRows, bool saveMessages,
            string saveMessagesDirectory, string nonReceivingSysName, string transactionType, int numberOfRecordsPerEsbMessage, int clientSideProcessGroupCount, int connectionTimeoutSeconds,  int threadCount, bool sendToEsb)
        {

            using (SqlConnection sqlConnection = new SqlConnection(mammothConnectionString))
            {

                var stagingInfo = GetStagingTableInfo(sqlConnection, connectionTimeoutSeconds);
                var numberOfGroups = stagingInfo.UnProcessedGroupIds.Count();
                var GroupRanges = CalculateRanges(stagingInfo.UnProcessedGroupIds, clientSideProcessGroupCount);

                logger.Info($"{numberOfGroups} esb message group(s) created");
                logger.Info($"{GroupRanges.Count()} client side data sets will be processed.");

                foreach (var g in GroupRanges)
                {
                    var first = g.Groups.First();
                    var last = g.Groups.Last();

                    logger.Info($"pulling data for groups {first}-{last}");
                    var models =
                        sqlConnection.Query<MammothItemLocaleModel>($"select * from stage.ItemLocaleExportStaging where groupid >= {first} and groupid <= {last}  and processed = 0", commandTimeout: connectionTimeoutSeconds).ToList();

                    try
                    {
                        var processingRange = Enumerable.Range(first, last+1);
                        logger.Info($"processing data for groups {first}-{last}");
                        Parallel.ForEach(processingRange, new ParallelOptions { MaxDegreeOfParallelism = threadCount }, (groupId) =>
                          {
                              var chunk = models.Where(w => w.GroupId == groupId);
                              SendMessagesToEsbServerSideGroups(chunk.ToList(), esbProducer, saveMessages, saveMessagesDirectory, nonReceivingSysName, transactionType, groupId, sendToEsb);
                          });
                        logger.Info($"finalizing groups {first}-{last}");
                        sqlConnection.Execute($"update stage.ItemLocaleExportStaging set Processed=1 where GroupId>={first} and GroupId <= {last} and Processed=0",commandTimeout: connectionTimeoutSeconds);

                    }
                    catch (Exception ex)
                    {
                        logger.Info($"group range {first}-{last} failed");
                        logger.Info(ex.Message);
                        if (ex.InnerException !=null)
                                logger.Info(ex.InnerException.Message);
                        logger.Info("");

                        var details = new Dictionary<string, string>()
                        {
                            { "Exception", ex.Message },
                            { "InnerException", ex.InnerException !=null ? ex.InnerException.Message : "" },
                            { "StackTrace", ex.StackTrace }
                        };

                        SendOpsGenieAlert($"AmazonLoad.MammothLocale failed at {DateTime.Now.ToString("G", DateTimeFormatInfo.InvariantInfo) }", "", details);
                    }
                }
            }
        }

        internal static IEnumerable<GroupRange> CalculateRanges(List<int> unprocessedGroups, int batchSize)
        {
            var batches = unprocessedGroups.OrderBy(o => o).Batch(batchSize).Select(s => new GroupRange { Groups= s.ToList() });
            return batches;
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

            // remove the control characters that are breaking the xml generation.
            logger.Info($"fixing invisible bits");
            mammothSqlConnection.Execute("update stage.ItemLocaleExportStaging set ScaleExtraText=Replace(ScaleExtraText, char(0x1e), '') where ScaleExtraText like '%'+char(0x1e)+'%'", commandTimeout: connectionTimeoutSeconds, commandType: CommandType.Text);

            return rowCount;

        }

        internal static void SendMessagesToEsbServerSideGroups(List<MammothItemLocaleModel> models,
            IEsbProducer esbProducer, bool saveMessages, string saveMessagesDirectory,
            string nonReceivingSysName, string transactionType, int groupId, bool sendToEsbFlag = true)
        {
            string message = "";
            if (!models.Any()) return;
            try
            {
                message = MessageBuilderForMammothItemLocale.BuildMessage(models);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                if (ex.InnerException != null) logger.Error(ex.InnerException.Message);
                throw;
            }

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
            
                Interlocked.Add(ref NumberOfRecordsSent, models.Count());
                Interlocked.Increment(ref NumberOfMessagesSent);
            
                if (saveMessages)
                {
                    File.AppendAllText($"{saveMessagesDirectory}/{messageId}.xml", JsonConvert.SerializeObject(headers) + Environment.NewLine + message);
                }
        }

        [Obsolete("Use SendMessagesToEsbServerSideGroups instead",false)]
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