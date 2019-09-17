using AmazonLoad.Common;
using Dapper;
using Esb.Core.Serializer;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Factory;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Icon.Esb.Producer;
using Icon.Framework;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace AmazonLoad.IconHierarchy
{
    class Program
    {
        private static Serializer<Contracts.HierarchyType> serializer = new Serializer<Contracts.HierarchyType>();
        public static string saveMessagesDirectory = "Messages";

        static void Main(string[] args)
        {
            var startTime = DateTime.Now;
            Console.WriteLine($"[{startTime}] beginning...");

            var maxNumberOfRows = AppSettingsAccessor.GetIntSetting("MaxNumberOfRows", 0);
            var saveMessages = AppSettingsAccessor.GetBoolSetting("SaveMessages");
            var saveMessagesDirectory = AppSettingsAccessor.GetStringSetting("SaveMessagesDirectory");
            var nonReceivingSysName = AppSettingsAccessor.GetStringSetting("NonReceivingSysName");
            var sendToEsb = AppSettingsAccessor.GetBoolSetting("SendMessagesToEsb", false);
            
            Console.WriteLine("Flags:");
            Console.WriteLine($"  MaxNumberOfRows: {maxNumberOfRows}");
            Console.WriteLine($"  SaveMessages: {saveMessages}");
            Console.WriteLine($"  SaveMessagesDirectory: \"{saveMessagesDirectory}\"");
            Console.WriteLine($"  NonReceivingSysName: \"{nonReceivingSysName}\"");
            Console.WriteLine($"  SendMessagesToEsb: {sendToEsb}");
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

            IEsbProducer producer = null;
            if (sendToEsb)
            producer = new EsbConnectionFactory
            {
                Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("esb")
            }.CreateProducer();

            var hierarchyName = AppSettingsAccessor.GetStringSetting("HierarchyName");

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
                new { HierarchyName = hierarchyName }, 
                buffered: false, 
                commandTimeout: 60);


            IEnumerable<HierarchyTraitModel> traitData = null;
            traitData = LoadTraitData(sqlConnection,hierarchyName);
            



            int numberOfRecordsSent = 0;
            int numberOfMessagesSent = 0;
            foreach (var modelBatch in models.Batch(100))
            {
                foreach (var modelGroup in modelBatch.GroupBy(m => m.HierarchyLevel))
                {
                    string message = BuildMessage(modelGroup, traitData);
                    string messageId = Guid.NewGuid().ToString();
                    var headers = new Dictionary<string, string>
                    {
                        { "IconMessageID", messageId },
                        { "Source", "Icon" },
                        { "nonReceivingSysName", AppSettingsAccessor.GetStringSetting("NonReceivingSysName") },
                        { "TransactionType", AppSettingsAccessor.GetStringSetting("TransactionType","Hierarchy") }
                    };

                    if (sendToEsb)
                    {
                        producer.Send(
                            message,
                            messageId,
                            headers);
                    }
                    numberOfRecordsSent += modelGroup.Count();
                    numberOfMessagesSent += 1;
                    if (saveMessages)
                    {
                        File.WriteAllText($"{saveMessagesDirectory}/{messageId}.xml", JsonConvert.SerializeObject(headers) + Environment.NewLine + message);
                    }
                }
            }
            Console.WriteLine($"Number of records sent: {numberOfRecordsSent}.");
            Console.WriteLine($"Number of messages sent: {numberOfMessagesSent}.");
            var endTime = DateTime.Now;
            Console.WriteLine($"[{endTime}] ({(endTime - startTime):hh\\:mm\\:ss} elapsed)");
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static IEnumerable<HierarchyTraitModel> LoadTraitData(SqlConnection sqlConnection, string hierarchyName)
        {

            if ( !(new [] {"Brands"}.Contains(hierarchyName)) ) return null;

            var traitSql =
                $@"select hc.hierarchyClassID, t.traitCode, t.traitDesc, hct.traitValue 
                   from hierarchy h inner join hierarchyclass hc on h.hierarchyID = hc.hierarchyID
                   inner join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
                   inner join Trait t on hct.traitID = t.traitID
                   where h.hierarchyName = '{hierarchyName}'
                   and t.traitcode in ('BA','GRD','PCO','ZIP','LCL')";

            var data = sqlConnection.Query<HierarchyTraitModel>(traitSql, new {HieararchyName = hierarchyName});
            return data;
        }

        private static string BuildMessage( IEnumerable<HierarchyClassModel> modelBatch, IEnumerable<HierarchyTraitModel> traitData)
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
                    },

                    traits = BuildTraits(message, traitData)
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
        private static Contracts.TraitType[] BuildTraits(HierarchyClassModel hierarchyClassModel, IEnumerable<HierarchyTraitModel> traitData)
        {
            //FYI: Add NationaClassCode if for The National Hierarchy messages only
            List<Contracts.TraitType> traits = new List<Contracts.TraitType>();

            if (hierarchyClassModel.HierarchyId == Hierarchies.National)
            {
                traits.Add(BuildTrait(TraitCodes.NationalClassCode, String.Empty, hierarchyClassModel.NationalClassCode));
            }

            if (hierarchyClassModel.HierarchyId == Hierarchies.Brands)
            {

                var brandAbbreviation = traitData.FirstOrDefault(t =>t.HierarchyClassId == hierarchyClassModel.HierarchyClassId && t.TraitCode == "BA")?.TraitValue;
                var zipCode = traitData.FirstOrDefault(t => t.HierarchyClassId == hierarchyClassModel.HierarchyClassId && t.TraitCode == "ZIP")?.TraitValue;
                var designation = traitData.FirstOrDefault(t => t.HierarchyClassId == hierarchyClassModel.HierarchyClassId && t.TraitCode == "GRD")?.TraitValue;
                var locality = traitData.FirstOrDefault(t => t.HierarchyClassId == hierarchyClassModel.HierarchyClassId && t.TraitCode == "LCL")?.TraitValue;
                var parentCompany= traitData.FirstOrDefault(t => t.HierarchyClassId == hierarchyClassModel.HierarchyClassId && t.TraitCode == "PCO")?.TraitValue;

                traits.AddRange(new List<Contracts.TraitType>()
                    {
                        BuildTrait(TraitCodes.BrandAbbreviation, TraitDescriptions.BrandAbbreviation, brandAbbreviation),
                        BuildTrait(TraitCodes.ZipCode, TraitDescriptions.ZipCode, zipCode),
                        BuildTrait(TraitCodes.Designation, TraitDescriptions.Designation, designation),
                        BuildTrait(TraitCodes.Locality, TraitDescriptions.Locality, locality),
                        BuildTrait(TraitCodes.ParentCompany, TraitDescriptions.ParentCompany, parentCompany)
                    });
            }

            return traits.Where(b => b.type.value[0].value != string.Empty).ToArray();
        }
        private static Contracts.TraitType BuildTrait(string traitCode, string traitDescription, string value)
        {
            var trait = new Contracts.TraitType
            {
                code = traitCode,
                type = new Contracts.TraitTypeType
                {
                    description = traitDescription,
                    value = new Contracts.TraitValueType[]
                            {
                                                new Contracts.TraitValueType
                                                {
                                                        value = value ?? string.Empty,
                                                }
                            }
                }
            };

            return trait;
        }


    }
}
