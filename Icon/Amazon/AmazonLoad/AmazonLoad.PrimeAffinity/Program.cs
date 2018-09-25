using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazonLoad.Common;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Factory;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace AmazonLoad.PrimeAffinityPsg
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
            var primeAffinityPsgId = AppSettingsAccessor.GetStringSetting("PrimeAffinityPsgId", "PrimeAffinityPSG");
            var primeAffinityPsgName = AppSettingsAccessor.GetStringSetting("PrimeAffinityPsgName", "PrimeAffinityPSG");
            var primeAffinityPsgType = AppSettingsAccessor.GetStringSetting("PrimeAffinityPsgType", "Consumable");

            Console.WriteLine("Flags:");
            Console.WriteLine($"  Region: {region}");
            Console.WriteLine($"  MaxNumberOfRows: {maxNumberOfRows}");
            Console.WriteLine($"  SaveMessages: {saveMessages}");
            Console.WriteLine($"  SaveMessagesDirectory: \"{saveMessagesDirectory}\"");
            Console.WriteLine($"  NonReceivingSysName: \"{nonReceivingSysName}\"");
            Console.WriteLine($"  SendMessagesToEsb: {sendToEsb}");
            Console.WriteLine($"  PrimePsgId: {primeAffinityPsgId}");
            Console.WriteLine($"  PrimePsgName: {primeAffinityPsgName}");
            Console.WriteLine($"  PrimePsgType: {primeAffinityPsgType}");
            if (!sendToEsb)
            {
                Console.WriteLine($"  \"SendMessagesToEsb\" flag is OFF: messages not actually sending to ESB queue!");
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

            PrimeAffinityPsgBuilder.LoadPrimeItemsAndSendMessages(
                esbProducer: producer,
                mammothConnectionString: mammothConnectionString,
                region: region,
                maxNumberOfRows: maxNumberOfRows,
                nonReceivingSysName: nonReceivingSysName,
                primePsgGroupId: primeAffinityPsgId,
                primePsgGroupName: primeAffinityPsgName,
                primePsgGroupType: "",
                saveMessages: saveMessages,
                saveMessagesDirectory: saveMessagesDirectory,
                sendToEsb: sendToEsb);

            Console.WriteLine($"Number of records sent: {PrimeAffinityPsgBuilder.NumberOfRecordsSent}.");
            Console.WriteLine($"Number of messages sent: {PrimeAffinityPsgBuilder.NumberOfMessagesSent}.");
            var endTime = DateTime.Now;
            Console.WriteLine($"[{endTime}] ({(endTime - startTime):hh\\:mm\\:ss} elapsed)");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
    }
}
