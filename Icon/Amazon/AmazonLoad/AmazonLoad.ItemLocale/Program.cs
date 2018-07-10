using AmazonLoad.Common;
using Dapper;
using Esb.Core.Mappers;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace AmazonLoad.ItemLocale
{
    class Program
    {
        public static IUomMapper uomMapper;
        public static Serializer<Contracts.items> serializer = new Serializer<Contracts.items>();

        static void Main(string[] args)
        {
            if (!Directory.Exists("ItemLocaleMessages"))
            {
                Directory.CreateDirectory("ItemLocaleMessages");
            }
            uomMapper = new UomMapper();

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);

            string formattedItemLocaleSql = SqlQueries.ItemLocaleSql.Replace("{region}", AppSettingsAccessor.GetStringSetting("Region"));
            var models = sqlConnection.Query<ItemLocaleModel>(formattedItemLocaleSql, buffered: false);

            var producer = new EsbConnectionFactory
            {
                Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("esb")
            }.CreateProducer();

            foreach (var modelBatch in models.Batch(100))
            {
                foreach (var modelGroup in modelBatch.GroupBy(m => m.BusinessUnitId))
                {
                    string itemMessage = BuildMessage(modelGroup.ToList());
                    string messageId = Guid.NewGuid().ToString();

                    producer.Send(
                        itemMessage,
                        messageId,
                        new Dictionary<string, string>
                        {
                            { "IconMessageID", messageId },
                            { "Source", "Mammoth"},
                            { "nonReceivingSysName", AppSettingsAccessor.GetStringSetting("NonReceivingSysName") }
                        });
                    File.WriteAllText($"ItemLocaleMessages/{messageId}.xml", itemMessage);
                }
            }
        }

        public static string BuildMessage(List<ItemLocaleModel> itemLocaleModels)
        {
            Contracts.items items = new Contracts.items
            {
                item = itemLocaleModels.Select(i => ConvertToContractsItemType(i)).ToArray()
            };

            return serializer.Serialize(items);
        }

        private static Contracts.ItemType ConvertToContractsItemType(ItemLocaleModel message)
        {
            return new Contracts.ItemType
            {
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
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
                            traits = new Contracts.TraitType[]
                            {
                                CreateTrait(message.CaseDiscount, Attributes.Codes.CaseDiscountEligible),
                                CreateTrait(message.TmDiscount, Attributes.Codes.TmDiscountEligible),
                                CreateTrait(message.AgeRestriction, Attributes.Codes.AgeRestrict),
                                CreateTrait(message.RestrictedHours, Attributes.Codes.RestrictedHours),
                                CreateTrait(message.Authorized, Attributes.Codes.AuthorizedForSale),
                                CreateTrait(message.Discontinued, Attributes.Codes.Discontinued),
                                CreateTrait(message.LabelTypeDescription, Attributes.Codes.LabelTypeDesc),
                                CreateTrait(message.LocalItem, Attributes.Codes.LocalItem),
                                CreateTrait(message.ProductCode, Attributes.Codes.ProductCode),
                                CreateTrait(message.RetailUnit, Attributes.Codes.RetailUnit),
                                CreateTrait(message.SignDescription, Attributes.Codes.SignCaption),
                                CreateTrait(message.Locality, Attributes.Codes.Locality),
                                CreateTrait(message.SignRomanceLong, Attributes.Codes.SignRomanceLong),
                                CreateTrait(message.SignRomanceShort, Attributes.Codes.SignRomanceShort),
                                CreateTrait(message.ColorAdded, Attributes.Codes.ColorAdded),
                                CreateTrait(message.CountryOfProcessing, Attributes.Codes.CountryOfProcessing),
                                CreateTrait(message.Origin, Attributes.Codes.Origin),
                                CreateTrait(message.ElectronicShelfTag, Attributes.Codes.ElectronicShelfTag),
                                CreateTrait(message.Exclusive, Attributes.Codes.Exclusive),
                                CreateTrait(message.NumberOfDigitsSentToScale, Attributes.Codes.NumberOfDigitsSentToScale),
                                CreateTrait(message.ChicagoBaby, Attributes.Codes.ChicagoBaby),
                                CreateTrait(message.TagUom, Attributes.Codes.TagUom),
                                CreateTrait(message.ScaleExtraText, Attributes.Codes.ScaleExtraText),
                                CreateTrait(message.LinkedItem, Attributes.Codes.LinkedScanCode),
                                CreateTrait(message.Msrp, Attributes.Codes.Msrp),
                                CreateTrait(message.SupplierName, Attributes.Codes.VendorName),
                                CreateTrait(message.SupplierItemID, Attributes.Codes.VendorItemId),
                                CreateTrait(message.SupplierCaseSize, Attributes.Codes.VendorCaseSize),
                                CreateTrait(message.IrmaVendorKey, Attributes.Codes.IrmaVendorKey),
                                CreateTrait(message.OrderedByInfor, Attributes.Codes.OrderedByInfor),
                                CreateTrait(message.AltRetailSize, Attributes.Codes.AltRetailSize),
                                CreateTrait(message.AltRetailUOM, Attributes.Codes.AltRetailUom),
                            }
                        }
                    }
                }
            };
        }

        private static Contracts.TraitType CreateTrait(decimal? traitValue, string traitCode)
        {
            var stringTraitValue = traitValue?.ToString("0.00");
            return CreateTrait(stringTraitValue, traitCode);
        }

        private static Contracts.TraitType CreateTrait(DateTime? traitValue, string traitCode)
        {
            string stringTraitValue = traitValue?.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
            return CreateTrait(stringTraitValue, traitCode);
        }

        private static Contracts.TraitType CreateTrait(bool? traitValue, string traitCode)
        {
            string stringTraitValue = null;
            if (traitValue.HasValue)
            {
                stringTraitValue = traitValue.Value ? "1" : "0";
                return CreateTrait(stringTraitValue, traitCode);
            }
            else
            {
                return CreateTrait(stringTraitValue, traitCode);
            }
        }

        private static Contracts.TraitType CreateTrait(int? traitValue, string traitCode)
        {
            var stringTraitValue = traitValue?.ToString();
            return CreateTrait(stringTraitValue, traitCode);
        }

        private static Contracts.TraitType CreateTrait(bool traitValue, string traitCode)
        {
            var stringTraitValue = traitValue ? "1" : "0";
            return CreateTrait(stringTraitValue, traitCode);
        }

        private static Contracts.TraitType CreateTrait(string traitValue, string traitCode)
        {
            return new Contracts.TraitType
            {
                ActionSpecified = true,
                Action = traitValue == null ? Contracts.ActionEnum.Delete : Contracts.ActionEnum.AddOrUpdate,
                code = traitCode,
                type = new Contracts.TraitTypeType
                {
                    description = Attributes.Descriptions.ByCode[traitCode],
                    value = new Contracts.TraitValueType[]
                    {
                        new Contracts.TraitValueType
                        {
                            value = traitValue == null ? string.Empty : traitValue
                        }
                    }
                }
            };
        }
    }
}
