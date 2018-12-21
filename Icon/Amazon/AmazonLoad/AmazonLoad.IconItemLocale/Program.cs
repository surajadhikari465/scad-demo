using Icon.Common;
using Icon.Esb;
using Icon.Esb.Factory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.IconItemLocale
{
    class Program
    {
        static void Main(string[] args)
        {
            var startTime = DateTime.Now;
            Console.WriteLine($"[{startTime}] beginning...");

            var maxNumberOfRows = AppSettingsAccessor.GetIntSetting("MaxNumberOfRows", 0);
            var saveMessages = AppSettingsAccessor.GetBoolSetting("SaveMessages");
            var saveMessagesDirectory = AppSettingsAccessor.GetStringSetting("SaveMessagesDirectory");
            var region = AppSettingsAccessor.GetStringSetting("Region");
            var nonReceivingSysName = AppSettingsAccessor.GetStringSetting("NonReceivingSysName");
            var sendToEsbFlag = AppSettingsAccessor.GetBoolSetting("SendMessagesToEsb", false);
            var transactionType = AppSettingsAccessor.GetStringSetting("TransactionType", "Item/Locale" );

            if (!sendToEsbFlag)
            {
                Console.WriteLine($"\"SendMessagesToEsb\" flag is OFF: messages not actually sending to ESB queue!");
            }

            var iconConnectionString = ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;
            var irmaConnectionString = ConfigurationManager.ConnectionStrings["ItemCatalog_" + region].ConnectionString;

            if (saveMessages && !Directory.Exists(saveMessagesDirectory))
            {
                Directory.CreateDirectory(saveMessagesDirectory);
            }

            var esbProducer = new EsbConnectionFactory
            {
                Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("esb")
            }.CreateProducer();

            var sendResult = IconItemLocaleBuilder
                .LoadItemLocalesAndSendMessages(
                    irmaConnectionString,
                    iconConnectionString,
                    esbProducer,
                    region,
                    maxNumberOfRows,
                    saveMessages,
                    saveMessagesDirectory,
                    nonReceivingSysName,
                    sendToEsbFlag,
                    transactionType);

            Console.WriteLine($"Number of records sent: {sendResult.NumberOfRecordsSent}.");
            Console.WriteLine($"Number of messages sent: {sendResult.NumberOfMessagesSent}.");
            var endTime = DateTime.Now;
            Console.WriteLine($"{endTime}] ({(endTime - startTime):hh\\:mm\\:ss} elapsed)");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
    }
}
