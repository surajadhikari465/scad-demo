using AmazonLoad.Common;
using Dapper;
using Esb.Core.Serializer;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Producer;
using Mammoth.Common.DataAccess;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace AmazonLoad.MammothPrice
{
    class Program
    {
        public static string saveMessagesDirectory = "Messages";

        static void Main(string[] args)
        {
            var maxNumberOfRows = AppSettingsAccessor.GetIntSetting("MaxNumberOfRows", 0);
            var saveMessages = AppSettingsAccessor.GetBoolSetting("SaveMessages");
            var region = AppSettingsAccessor.GetStringSetting("Region");
            var nonReceivingSysName = AppSettingsAccessor.GetStringSetting("NonReceivingSysName");

            var connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;

            if (saveMessages)
            {
                if(!Directory.Exists(saveMessagesDirectory))
                {
                    Directory.CreateDirectory(saveMessagesDirectory);
                }
            }

            var esbProducer = new EsbConnectionFactory
            {
                Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("esb")
            }.CreateProducer();

            var recordsAndMesssagesSent = MammothPriceBuilder
                .LoadMammothPricesAndSendMessages(
                    esbProducer,
                    connectionString,
                    region,
                    maxNumberOfRows,
                    saveMessages,
                    saveMessagesDirectory,
                    nonReceivingSysName);

            Console.WriteLine($"Number of records sent: {recordsAndMesssagesSent.NumberOfRecordsSent}.");
            Console.WriteLine($"Number of messages sent: {recordsAndMesssagesSent.NumberOfMessagesSent}.");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

       
    }
}
