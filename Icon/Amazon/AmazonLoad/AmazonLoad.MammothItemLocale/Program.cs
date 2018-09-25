using AmazonLoad.Common;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Factory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.MammothItemLocale
{
    class Program
    {
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
            var mammothConnectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;

            Console.WriteLine("Flags:");
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
                
            if (saveMessages)
            {
                if (!Directory.Exists(saveMessagesDirectory))
                {
                    Directory.CreateDirectory(saveMessagesDirectory);
                }
            }

            var producer = new EsbConnectionFactory
            {
                Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("esb")
            }.CreateProducer();

            MammothItemLocaleBuilder.LoadMammothItemLocalesAndSendMessages(
                esbProducer: producer,
                mammothConnectionString: mammothConnectionString,
                region: region,
                maxNumberOfRows: maxNumberOfRows,
                saveMessages: saveMessages,
                saveMessagesDirectory: saveMessagesDirectory,
                nonReceivingSysName: nonReceivingSysName,
                sendToEsb: sendToEsb);

            Console.WriteLine($"Number of records sent: {MammothItemLocaleBuilder.NumberOfRecordsSent}.");
            Console.WriteLine($"Number of messages sent: {MammothItemLocaleBuilder.NumberOfMessagesSent}.");
            var endTime = DateTime.Now;
            Console.WriteLine($"[{endTime}] ({(endTime - startTime):hh\\:mm\\:ss} elapsed)");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
    }
}
