﻿using CommandLine;
using CommandLine.Attributes;
using OutputColorizer;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Factory;
using System;
using System.Configuration;
using System.IO;
using NLog;
using AmazonLoad.Common;
using System.Globalization;

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
        public static int numberOfRecordsPerEsbMessage { get; set; }
        public static bool sendToEsb { get; set; }
        public static string transactionType { get; set; }
        public static DateTime startTime { get; set; }
        public static int threadCount { get; set; }

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
        static void Main(string[] args)
        {

            if (!Parser.TryParse(args, out Options options))
            {
                logger.Error("Unable to parse options. Exiting.");
                return;
            }

            if (options.RunMode == null)
            {
                Parser.DisplayHelp<Options>();
            }

            
            startTime = DateTime.Now;
            region = AppSettingsAccessor.GetStringSetting("Region");
            maxNumberOfRows = AppSettingsAccessor.GetIntSetting("MaxNumberOfRows", 0);
            saveMessages = AppSettingsAccessor.GetBoolSetting("SaveMessages");
            saveMessagesDirectory = AppSettingsAccessor.GetStringSetting("SaveMessagesDirectory");
            nonReceivingSysName = AppSettingsAccessor.GetStringSetting("NonReceivingSysName");
            sendToEsb = AppSettingsAccessor.GetBoolSetting("SendMessagesToEsb", false);
            mammothConnectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            transactionType = AppSettingsAccessor.GetStringSetting("TransactionType", "Item/Locale");
            connectionTimeoutSeconds = AppSettingsAccessor.GetIntSetting("ConnectionTimeoutSeconds", 7200);
            numberOfRecordsPerEsbMessage = AppSettingsAccessor.GetIntSetting("numberOfRecordsPerESBMessage", 100);
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
            logger.Info($"  SendMessagesToEsb: {sendToEsb}");
            logger.Info($"  NumberOfRecordsPerESBMessage: {numberOfRecordsPerEsbMessage}");
            logger.Info($"  ClientSideProcessGroupCount: {clientSideProcessGroupCount}");



            if (!sendToEsb)
            {
                logger.Info($"  SendMessagesToEsb: OFF =>: messages not actually sending to ESB queue!");
            }
            logger.Info("");

            switch (options.RunMode.ToLowerInvariant())
            {
                case "check":
                    CheckStagingTable();
                    break;
                case "stage":
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
                numberOfRecordsPerEsbMessage: numberOfRecordsPerEsbMessage);
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

            var producer = new EsbConnectionFactory { Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("esb") }.CreateProducer();

            MammothItemLocaleBuilder.ProcessMammothItemLocalesAndSendMessages(
                esbProducer: producer,
                mammothConnectionString: mammothConnectionString,
                region: region,
                maxNumberOfRows: maxNumberOfRows,
                saveMessages: saveMessages,
                saveMessagesDirectory: saveMessagesDirectory,
                nonReceivingSysName: nonReceivingSysName,
                transactionType: transactionType,
                connectionTimeoutSeconds: connectionTimeoutSeconds,
                clientSideProcessGroupCount: clientSideProcessGroupCount,
                numberOfRecordsPerEsbMessage: numberOfRecordsPerEsbMessage,
                sendToEsb: sendToEsb,
                threadCount: threadCount);

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
    }

}
