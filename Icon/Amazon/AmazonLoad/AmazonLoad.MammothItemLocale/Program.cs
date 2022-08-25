using CommandLine;
using CommandLine.Attributes;
using Icon.Common;
using System;
using System.Configuration;
using System.IO;
using NLog;
using Icon.ActiveMQ.Producer;
using Icon.ActiveMQ;

namespace AmazonLoad.MammothItemLocale
{


    class Program
    {
        public static string mammothConnectionString { get; set; }
        public static bool saveMessages { get; set; }
        public static string saveMessagesDirectory { get; set; }
        public static string region { get; set; }
        public static int maxNumberOfRows { get; set; }
        public static string nonReceivingSysName { get; set; }
        public static int connectionTimeoutSeconds { get; set; }
        public static int clientSideProcessGroupCount { get; set; }
        public static int numberOfRecordsPerMQMessage { get; set; }
        public static bool sendToMQ { get; set; }
        public static string transactionType { get; set; }
        public static DateTime startTime { get; set; }
        public static int threadCount { get; set; }
        public static int startGroup { get; set; }
        public static int endGroup { get; set; }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal static void DisplayOptionInformation()
        {


            logger.Info("Usage:");
            logger.Info("   AmazonLoad.MammothItemLocale.exe [-RunMode value]");
            logger.Info("");
            logger.Info("Value:");
            logger.Info("Check: Display record count from staging table.");
            logger.Info("Stage: Clear staging table. Stage new set of data.");
            logger.Info("Process: Process any unprocessed records in staging table. Run again to resume after error.");
            logger.Info("ClearStaging: Clear records from staging table to reset.");
            logger.Info("ResetProcessed: Clear Processed Flags in staging.");

        }
        static int Main(string[] args)
        {
            try
            {
                if (!Parser.TryParse(args, out Options options))
                {
                    logger.Error("Unable to parse options. Exiting.");
                    return 1;
                }

                if (options.RunMode == null)
                {
                    Parser.DisplayHelp<Options>();
                }

                startGroup = options.StartGroup;
                endGroup = options.EndGroup;

                startTime = DateTime.Now;
                region = AppSettingsAccessor.GetStringSetting("Region");
                maxNumberOfRows = AppSettingsAccessor.GetIntSetting("MaxNumberOfRows", 0);
                saveMessages = AppSettingsAccessor.GetBoolSetting("SaveMessages");
                saveMessagesDirectory = AppSettingsAccessor.GetStringSetting("SaveMessagesDirectory");
                nonReceivingSysName = AppSettingsAccessor.GetStringSetting("NonReceivingSysName");
                sendToMQ = AppSettingsAccessor.GetBoolSetting("SendMessagesToMQ", false);
                mammothConnectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
                transactionType = AppSettingsAccessor.GetStringSetting("TransactionType", "Item/Locale");
                connectionTimeoutSeconds = AppSettingsAccessor.GetIntSetting("ConnectionTimeoutSeconds", 7200);
                numberOfRecordsPerMQMessage = AppSettingsAccessor.GetIntSetting("numberOfRecordsPerMQMessage", 100);
                clientSideProcessGroupCount = AppSettingsAccessor.GetIntSetting("ClientSideProcessGroupCount", 1000);
                threadCount = AppSettingsAccessor.GetIntSetting("threadCount", 2);


                logger.Info($"Current RunMode: {options.RunMode}");
                logger.Info("");
                logger.Info("Flags:");
                logger.Info($"  region: {region}");
                logger.Info($"  connectionTimeoutSeconds: {connectionTimeoutSeconds}");
                logger.Info($"  MaxNumberOfRows: {maxNumberOfRows}");
                logger.Info($"  SaveMessages: {saveMessages}");
                logger.Info($"  SaveMessages: \"{saveMessagesDirectory}\"");
                logger.Info($"  NonReceivingSysName: \"{nonReceivingSysName}\"");
                logger.Info($"  SendMessagesToMQ: {sendToMQ}");
                logger.Info($"  NumberOfRecordsPerMQMessage: {numberOfRecordsPerMQMessage}");
                logger.Info($"  ClientSideProcessGroupCount: {clientSideProcessGroupCount}");



                if (!sendToMQ)
                {
                    logger.Info($"  SendMessagesToMQ: OFF =>: messages not actually sending to MQ queue!");
                }
                logger.Info("");

                switch (options.RunMode.ToLowerInvariant())
                {
                    case "check":
                        CheckStagingTable();
                        break;
                    case "stage":
                        if (options.StartGroup < 0 && options.EndGroup < 0)
                        {
                            Console.WriteLine("The staging process will only perform first step. To perform second step rerun the command with -StartRange and -EndRange attributes");
                        }
                        StageRecords();
                        break;
                    case "process":
                        FullExtract();
                        break;
                    case "clearstaging":
                        ClearStagingTable();
                        break;
                    case "resetprocessed":
                        ResetProcessedRecords();
                        break;
                    case "opsgenietest":
                        OpsGenieTest();
                        break;
                    case "help":
                        DisplayHelp();
                        break;
                    default:
                        logger.Info($"Unknown RunMode supplied. {options.RunMode} Exiting.");
                        break;
                }

                var endTime = DateTime.Now;
                logger.Info($"[{endTime}] ({(endTime - startTime):hh\\:mm\\:ss} elapsed)");
                if(!options.KillAfterCompletion)
                {
                    Console.ReadKey();
                }
            }
            catch(Exception e)
            {
                logger.Error(e);
                Console.ReadKey();
                return 2;
            }
            return 0;
        }

        public static void OpsGenieTest()
        {
            MammothItemLocaleBuilder.SendOpsGenieAlert("AmazonLoad.MammothLocale Test Ops Genie Message", "this  is just a test.", new System.Collections.Generic.Dictionary<string, string> { { "Exception", "test" } });
        }

        private static void DisplayHelp()
        {
            DisplayOptionInformation();
        }

        private static void StageRecords()
        {
            MammothItemLocaleBuilder.StageRecords(mammothConnectionString: mammothConnectionString,
                region: region,
                maxNumberOfRows: maxNumberOfRows,
                timeoutInSeconds: connectionTimeoutSeconds,
                numberOfRecordsPerMQMessage: numberOfRecordsPerMQMessage,
                startGroup: startGroup,
                endGroup: endGroup);
        }

        private static void ResetProcessedRecords()
        {
            MammothItemLocaleBuilder.ResetProcessedRecords(mammothConnectionString: mammothConnectionString, timeoutInSeconds: connectionTimeoutSeconds);
        }

        private static void ClearStagingTable()
        {
            MammothItemLocaleBuilder.ClearStagingTable(mammothConnectionString, connectionTimeoutSeconds);
        }

        private static void FullExtract()
        {
            if (saveMessages && !Directory.Exists(saveMessagesDirectory))
                Directory.CreateDirectory(saveMessagesDirectory);

            var producer = new ActiveMQProducer(ActiveMQConnectionSettings.CreateSettingsFromConfig("ActiveMqItemLocaleQueueName"));
            string clientId = $"AmazonLoad-{Guid.NewGuid()}";
            Console.WriteLine($"Client ID: {clientId}");
            producer.OpenConnection(clientId);

            MammothItemLocaleBuilder.ProcessMammothItemLocalesAndSendMessages(
                mqProducer: producer,
                mammothConnectionString: mammothConnectionString,
                region: region,
                maxNumberOfRows: maxNumberOfRows,
                saveMessages: saveMessages,
                saveMessagesDirectory: saveMessagesDirectory,
                nonReceivingSysName: nonReceivingSysName,
                transactionType: transactionType,
                connectionTimeoutSeconds: connectionTimeoutSeconds,
                clientSideProcessGroupCount: clientSideProcessGroupCount,
                numberOfRecordsPerMQMessage: numberOfRecordsPerMQMessage,
                sendToMQ: sendToMQ,
                threadCount: threadCount,
                startGroup: startGroup,
                endGroup: endGroup);

            logger.Info($"Number of records sent: {MammothItemLocaleBuilder.NumberOfRecordsSent}.");
            logger.Info($"Number of messages sent: {MammothItemLocaleBuilder.NumberOfMessagesSent}.");
        }

        private static void CheckStagingTable()
        {
            MammothItemLocaleBuilder.CheckStagingTable(mammothConnectionString, connectionTimeoutSeconds);
        }
    }

    internal class Options
    {
        [OptionalArgument("Help", "RunMode", "[ Check | Stage | Process | ClearStaging | ResetProcessed | Help]")]
        public string RunMode { get; set; }
        [OptionalArgument(-1, "StartGroup", "Starting range for staging or processing")]
        public int StartGroup { get; set; }
        [OptionalArgument(-1, "EndGroup", "Ending range for staging or processing")]
        public int EndGroup { get; set; }
        [OptionalArgument(false, "KillAfterCompletion", "Ending range for staging or processing")]
        public bool KillAfterCompletion { get; set; }
    }

}
