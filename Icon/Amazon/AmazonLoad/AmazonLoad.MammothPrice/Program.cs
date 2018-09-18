using Icon.Common;
using Icon.Esb;
using Icon.Esb.Factory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace AmazonLoad.MammothPrice
{
    class Program
    {
        public static string saveMessagesDirectory = "Messages";

        static void Main(string[] args)
        {
            var startTime = DateTime.Now;
            Console.WriteLine($"[{startTime}] beginning...");

            var region = AppSettingsAccessor.GetStringSetting("Region");
            var maxNumberOfRows = AppSettingsAccessor.GetIntSetting("MaxNumberOfRows", 0);
            var saveMessages = AppSettingsAccessor.GetBoolSetting("SaveMessages");
            var saveMessagesDirectory = AppSettingsAccessor.GetStringSetting("SaveMessagesDirectory");
            var nonReceivingSysName = AppSettingsAccessor.GetStringSetting("NonReceivingSysName");
            var sendToEsb = AppSettingsAccessor.GetBoolSetting("SendMessagesToEsb", false);

            Console.WriteLine("Flags:");
            Console.WriteLine($"  Region: \"{region}\"");
            Console.WriteLine($"  MaxNumberOfRows: {maxNumberOfRows}");
            Console.WriteLine($"  SaveMessages: {saveMessages}");
            Console.WriteLine($"  SaveMessages: \"{saveMessagesDirectory}\"");
            Console.WriteLine($"  NonReceivingSysName: \"{nonReceivingSysName}\"");
            Console.WriteLine($"  SendMessagesToEsb: {sendToEsb}");
            if (!sendToEsb)
            {
                Console.WriteLine($"\"SendMessagesToEsb\" flag is OFF: messages not actually sending to ESB queue!");
            }
            Console.WriteLine("");

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

            var sendResult = MammothPriceBuilder
                .LoadMammothPricesAndSendMessages(
                    esbProducer,
                    connectionString,
                    region,
                    maxNumberOfRows,
                    saveMessages,
                    saveMessagesDirectory,
                    nonReceivingSysName,
                    sendToEsb);

            Console.WriteLine($"Number of records sent: {sendResult.NumberOfRecordsSent}.");
            Console.WriteLine($"Number of messages sent: {sendResult.NumberOfMessagesSent}."); var endTime = DateTime.Now;
            Console.WriteLine($"{endTime}] ({(endTime - startTime):hh\\:mm\\:ss} elapsed)");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

       
    }
}
