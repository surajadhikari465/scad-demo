using AmazonLoad.Common;
using Dapper;
using Icon.Esb.Producer;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.IconItemLocale
{
    public static class IconItemLocaleBuilder
    {
        public static IconItemLocalePsgMapper PsgMapper = new IconItemLocalePsgMapper();
        public static int NumberOfRecordsSent = 0;
        public static int NumberOfMessagesSent = 0;
        internal static int DefaultBatchSize = 100;

        public static EsbMessageSendResult LoadItemLocalesAndSendMessages(string irmaConnectionString,
            string iconConnectionString, IEsbProducer esbProducer, string region, int maxNumberOfRows,
            bool saveMessages, string saveMessagesDirectory, string nonReceivingSysName, bool sendToEsbFlag, string transactionType)
        {
            List<IconStoreModel> iconStoreModels = null;

            //load store data from icon
            iconStoreModels = LoadIconStoreData(region, iconConnectionString).ToList();

            // load PSG data to use when building message
            PsgMapper = new IconItemLocalePsgMapper(iconConnectionString);
            PsgMapper.LoadProductSelectionGroups();

            //o Initial Load Application will read a list of valid stores from Icon for a given region using Icon’s dbo.Locale table.
            //o For each store the application will read validated items from IRMA using the dbo.ValidatedScanCode table.
            //o The Application will only pull Default Identifiers from IRMA.
            //o If Icon and IRMA are out of sync with linked items, pull the IRMA linked item value and don't read from dbo.ItemLink table
            //o Items will then be grouped into a message and sent to the ESB.
            //o Data read from Icon and IRMA’s databases will not be buffered in order to avoid using too much memory.
            //o Only Retail Flagged items will be passed from IRMA/ Icon

            foreach (var store in iconStoreModels)
            {
                if (maxNumberOfRows == 0 || (maxNumberOfRows > 0 && NumberOfRecordsSent < maxNumberOfRows))
                {
                    using (SqlConnection irmaSqlConnection = new SqlConnection(irmaConnectionString))
                    {
                        // load data
                        var wormholeItemLocaleModels = LoadItemLocalesForWormhole(irmaSqlConnection, store, maxNumberOfRows);

                        // now send the message(s) to the eSB
                        SendMessagesToEsb(wormholeItemLocaleModels, esbProducer,
                            saveMessages, saveMessagesDirectory, nonReceivingSysName, maxNumberOfRows, transactionType, sendToEsbFlag);
                    }
                }
            }

            return new EsbMessageSendResult(NumberOfRecordsSent, NumberOfMessagesSent);
        }

        internal static IEnumerable<ItemLocaleModelForWormhole> LoadItemLocalesForWormhole(SqlConnection irmaSqlConnection,
            IconStoreModel store, int maxNumberOfRows)
        {
            var irmaItemLocaleModels = LoadIrmaItemLocales(irmaSqlConnection, store.RegionCode, store.BusinessUnit, maxNumberOfRows);
            return irmaItemLocaleModels.Select(i => new ItemLocaleModelForWormhole(store, i));
        }

        internal static IEnumerable<ItemLocaleModelForIrma> LoadIrmaItemLocales(SqlConnection irmaSqlConnection,
            string region, int businessUnit, int maxNumberOfRows)
        {
            var sql = GetFormattedSqlForIrmaItemLocaleQuery(region, businessUnit.ToString(), maxNumberOfRows);
            var irmaItemLocaleModels = irmaSqlConnection.Query<ItemLocaleModelForIrma>(sql, buffered: false, commandTimeout: 60);
            return irmaItemLocaleModels;
        }

        internal static IList<IconStoreModel> LoadIconStoreData(string region, string iconConnectionString)
        {
            using (var iconSqlConnection = new SqlConnection(iconConnectionString))
            {
                var sqlForStoreQuery = GetFormattedSqlForIconStoreQuery(region);
                var storeModels = iconSqlConnection.Query<IconStoreModel>(sqlForStoreQuery, buffered: false, commandTimeout: 60);
                return storeModels.ToList();
            }
        }

        internal static string GetFormattedSqlForIconStoreQuery(string region)
        {
            string sql = SqlQueries.QueryIconValidStoresSql.Replace("{region}", region);
            return sql;
        }

        internal static string GetFormattedSqlForIrmaItemLocaleQuery(string region, string businessUnit, int maxNumberOfRows)
        {
            string sql = SqlQueries.QueryIrmaItemLocaleSql
                .Replace("{region}", region)
                .Replace("{businessUnit}", businessUnit);
            if (maxNumberOfRows != 0)
            {
                sql = sql.Replace("{top query}", $"top {maxNumberOfRows}");
            }
            else
            {
                sql = sql.Replace("{top query}", "");
            }
            return sql;
        }

        internal static void SendMessagesToEsb(IEnumerable<ItemLocaleModelForWormhole> models,
           IEsbProducer esbProducer, bool saveMessages, string saveMessagesDirectory,
           string nonReceivingSysName, int maxNumberOfRows, string transactionType, bool sendToEsbFlag = true)
        {
            var batchSize = Utils.CalcBatchSize(DefaultBatchSize, maxNumberOfRows, NumberOfRecordsSent);
            if (batchSize < 0) return;

            foreach (var modelBatch in models.Batch(batchSize))
            {
                if (maxNumberOfRows != 0 && NumberOfRecordsSent >= maxNumberOfRows) return;
                string message = MessageBuilderForIconItemLocale.BuildMessage(modelBatch, PsgMapper);

                string messageId = Guid.NewGuid().ToString();
                var headers = new Dictionary<string, string>
                {
                        { "IconMessageID", messageId },
                        { "Source", "Icon" },
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
                NumberOfRecordsSent += modelBatch.Count();
                NumberOfMessagesSent += 1;
                if (saveMessages)
                {
                    File.WriteAllText($"{saveMessagesDirectory}/{messageId}.xml", JsonConvert.SerializeObject(headers) + Environment.NewLine + message);
                }
            }
        }
    }
}
