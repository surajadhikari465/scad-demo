using AmazonLoad.Common;
using Dapper;
using Esb.Core.Serializer;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Factory;
using Mammoth.Common.DataAccess;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace AmazonLoad.Price
{
    class Program
    {
        public static Serializer<Contracts.items> serializer = new Serializer<Contracts.items>();
        public static string saveMessagesDirectory = "Messages";

        static void Main(string[] args)
        {
            var maxNumberOfRows = AppSettingsAccessor.GetIntSetting("MaxNumberOfRows", 0);
            var saveMessages = AppSettingsAccessor.GetBoolSetting("SaveMessages");
            if(saveMessages)
            {
                if(!Directory.Exists(saveMessagesDirectory))
                {
                    Directory.CreateDirectory(saveMessagesDirectory);
                }
            }

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);

            string formattedSql = SqlQueries.PriceSql.Replace("{region}", AppSettingsAccessor.GetStringSetting("Region"));
            if(maxNumberOfRows != 0)
            {
                formattedSql = formattedSql.Replace("{top query}", $"top {maxNumberOfRows}");
            }
            else
            {
                formattedSql = formattedSql.Replace("{top query}", "");
            }
            var models = sqlConnection.Query<PriceModel>(formattedSql, buffered: false, commandTimeout: 60);
            var producer = new EsbConnectionFactory
            {
                Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("esb")
            }.CreateProducer();

            int numberOfRecordsSent = 0;
            int numberOfMessagesSent = 0;
            foreach (var modelBatch in models.Batch(100))
            {
                foreach (var modelGroup in modelBatch.GroupBy(m => m.BusinessUnitId))
                {
                    string message = BuildMessage(modelGroup.ToList());
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

        public static string BuildMessage(List<PriceModel> messages)
        {
            List<Contracts.ItemType> items = new List<Contracts.ItemType>();
            foreach (var message in messages)
            {
                items.Add(
                    BuildItemType(
                        message, 
                        message.PriceTypeCode,
                        message.Price, 
                        message.Multiple, 
                        message.StartDate, 
                        message.EndDate));
            }
            return serializer.Serialize(new Contracts.items { item = items.ToArray() });
        }

        private static Contracts.ItemType BuildItemType(
            PriceModel message,
            string priceTypeCode,
            decimal price,
            int multiple,
            DateTime startDate,
            DateTime? endDate)
        {
            var itemType = new Contracts.ItemType
            {
                id = message.ItemId,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = message.ItemTypeCode,
                        description = message.ItemTypeDesc
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        ActionSpecified = true,
                        id = message.BusinessUnitId.ToString(),
                        name = message.LocaleName,
                        type = new Contracts.LocaleTypeType
                        {
                            code = Contracts.LocaleCodeType.STR,
                            description = Contracts.LocaleDescType.Store
                        },
                        Item = new Contracts.StoreItemAttributesType
                        {
                            scanCode = new Contracts.ScanCodeType[]
                            {
                                new Contracts.ScanCodeType
                                {
                                    code = message.ScanCode,
                                }
                            },
                            prices = new Contracts.PriceType[]
                            {
                                CreatePriceType(message, priceTypeCode, price, multiple, startDate, endDate)
                            }
                        }
                    }
                }
            };

            return itemType;
        }

        private static Contracts.PriceType CreatePriceType(
            PriceModel message,
            string priceTypeCode,
            decimal price,
            int multiple,
            DateTime startDate,
            DateTime? endDate)
        {
            var priceType = new Contracts.PriceType
            {
                type = new Contracts.PriceTypeType
                {
                    description = ItemPriceTypes.Descriptions.ByCode[priceTypeCode],
                    id = priceTypeCode,
                    type = string.IsNullOrWhiteSpace(message.SubPriceTypeCode) ? null
                        : new Contracts.PriceTypeType
                        {
                            description = message.SubPriceTypeCode,
                            id = message.SubPriceTypeCode,
                        }
                },
                uom = new Contracts.UomType
                {
                    codeSpecified = true,
                    nameSpecified = true,
                    code = GetEsbUomCode(message.UomCode),
                    name = GetEsbUomName(message.UomCode, message.ScanCode)
                },
                currencyTypeCode = GetEsbCurrencyTypeCode(message.CurrencyCode),
                priceAmount = new Contracts.PriceAmount
                {
                    amount = price,
                    amountSpecified = true
                },
                priceMultiple = multiple,
                priceStartDate = startDate,
                priceStartDateSpecified = true,
            };

            if (endDate.HasValue)
            {
                priceType.priceEndDate = endDate.Value;
                priceType.priceEndDateSpecified = true;
            }

            return priceType;
        }

        private static Contracts.WfmUomCodeEnumType GetEsbUomCode(string uomCode)
        {
            switch (uomCode)
            {
                case UomCodes.Each:
                    return Contracts.WfmUomCodeEnumType.EA;
                case UomCodes.Pound:
                    return Contracts.WfmUomCodeEnumType.LB;
                case UomCodes.Kilogram:
                    return Contracts.WfmUomCodeEnumType.KG;
                default:
                    return Contracts.WfmUomCodeEnumType.EA;
            }
        }

        private static Contracts.WfmUomDescEnumType GetEsbUomName(string uomCode, string scanCode)
        {
            switch (uomCode)
            {
                case UomCodes.Each:
                    return Contracts.WfmUomDescEnumType.EACH;
                case UomCodes.Pound:
                    return Contracts.WfmUomDescEnumType.POUND;
                case UomCodes.Kilogram:
                    return Contracts.WfmUomDescEnumType.KILOGRAM;
                default:
                    return Contracts.WfmUomDescEnumType.EACH;
            }
        }

        private static Contracts.CurrencyTypeCodeEnum GetEsbCurrencyTypeCode(string currencyTypeCode)
        {
            Contracts.CurrencyTypeCodeEnum currencyTypeCodeEnum;
            if (Enum.TryParse(currencyTypeCode, out currencyTypeCodeEnum))
            {
                return currencyTypeCodeEnum;
            }
            else
            {
                return Contracts.CurrencyTypeCodeEnum.USD;
            }
        }
    }
}
