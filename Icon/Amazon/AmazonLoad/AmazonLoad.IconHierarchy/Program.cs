using AmazonLoad.Common;
using Dapper;
using Esb.Core.Serializer;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Factory;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace AmazonLoad.IconHierarchy
{
    class Program
    {
        private static Serializer<Contracts.HierarchyType> serializer = new Serializer<Contracts.HierarchyType>();
        public static string saveMessagesDirectory = "Messages";

        static void Main(string[] args)
        {
            var maxNumberOfRows = AppSettingsAccessor.GetIntSetting("MaxNumberOfRows", 0);
            var saveMessages = AppSettingsAccessor.GetBoolSetting("SaveMessages");
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

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            var formattedSql = SqlQueries.HierarchySql;
            if (maxNumberOfRows != 0)
            {
                formattedSql = formattedSql.Replace("{top query}", $"top {maxNumberOfRows}");
            }
            else
            {
                formattedSql = formattedSql.Replace("{top query}", "");
            }
            var models = sqlConnection.Query<HierarchyClassModel>(
                formattedSql, 
                new { HierarchyName = AppSettingsAccessor.GetStringSetting("HierarchyName") }, 
                buffered: false, 
                commandTimeout: 60);

            int numberOfRecordsSent = 0;
            int numberOfMessagesSent = 0;
            foreach (var modelBatch in models.Batch(100))
            {
                foreach (var modelGroup in modelBatch.GroupBy(m => m.HierarchyLevel))
                {
                    string message = BuildMessage(modelGroup);
                    string messageId = Guid.NewGuid().ToString();

                    producer.Send(
                        message,
                        messageId,
                        new Dictionary<string, string>
                        {
                            { "IconMessageID", messageId },
                            { "Source", "Icon" },
                            { "nonReceivingSysName", AppSettingsAccessor.GetStringSetting("NonReceivingSysName") }
                        });
                    numberOfRecordsSent += modelGroup.Count();
                    numberOfMessagesSent += 1;
                    if (saveMessages)
                    {
                        File.WriteAllText($"{saveMessagesDirectory}/{messageId}.xml", message);
                    }
                }
            }
            Console.WriteLine($"Number of records sent: {numberOfRecordsSent}.");
            Console.WriteLine($"Number of messages sent: {numberOfMessagesSent}.");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static string BuildMessage(IEnumerable<HierarchyClassModel> modelBatch)
        {
            Contracts.HierarchyType hierarchyType = new Contracts.HierarchyType
            {
                @class = modelBatch.Select(message => new Contracts.HierarchyClassType
                {
                    Action = Contracts.ActionEnum.AddOrUpdate,
                    ActionSpecified = true,
                    id = message.HierarchyClassId.ToString(),
                    name = message.HierarchyClassName,
                    level = message.HierarchyLevel,
                    parentId = new Contracts.hierarchyParentClassType
                    {
                        Value = message.HierarchyParentClassId.HasValue ? message.HierarchyParentClassId.Value : 0
                    }
                }).ToArray(),
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = modelBatch.First().HierarchyId,
                name = modelBatch.First().HierarchyName,
                prototype = new Contracts.HierarchyPrototypeType
                {
                    hierarchyLevelName = modelBatch.First().HierarchyLevelName,
                    itemsAttached = modelBatch.First().ItemsAttached ? "1" : "0"
                }
            };

            return serializer.Serialize(hierarchyType);
        }
    }
}
