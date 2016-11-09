using Icon.Framework;
using MessageGenerationWeb.MessageBuilders.Infrastructure;
using MessageGenerationWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using Contracts = Icon.Service.Library.ContractTypes;
using System.Data.SqlClient;
using System.Configuration;

namespace MessageGenerationWeb.MessageBuilders
{
    public class ItemPriceMessageBuilder
    {
        private ISerializer<Contracts.items> serializer;

        public ItemPriceMessageBuilder()
        {
            serializer = new Serializer<Contracts.items>();
        }

        public List<string> BuildDeleteMessages(List<ItemPriceDeleteModel> deleteModels)
        {
            List<string> messages = new List<string>();

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString))
            {
                string itemSql = @"select i.itemID as ItemId,
                                    it.itemTypeCode as ItemTypeCode,
                                    it.itemTypeDesc as ItemTypeDescription,
                                    sc.scanCodeID as ScanCodeId,
                                    sc.scanCode as ScanCode,
                                    sct.scanCodeTypeID as ScanCodeTypeId,
                                    sct.scanCodeTypeDesc as ScanCodeTypeDescription
                                from Item i
                                join ItemType it on i.itemTypeID = it.itemTypeID
                                join ScanCode sc on i.itemID = sc.itemID
                                join ScanCodeType sct on sc.scanCodeTypeID = sct.scanCodeTypeID
                                where sc.scanCode in @ScanCodes";
                var itemModels = db.Query(itemSql, new { ScanCodes = deleteModels.Select(m => m.ScanCode) }).ToList();

                string localeSql = @"select l.localeName as LocaleName,
                                            lt.traitValue as BusinessUnit
                                    from Locale l
                                    join LocaleTrait lt on l.localeID = lt.localeID
                                    join Trait t on t.traitID = lt.traitID
                                        and t.traitCode = 'BU'
                                    where lt.traitValue in @BusinessUnits";
                var localeModels = db.Query(localeSql, new { BusinessUnits = deleteModels.Select(m => m.BusinessUnit) }).ToList();

                var itemPriceList = (from deleteModel in deleteModels
                                    join itemModel in itemModels on deleteModel.ScanCode equals itemModel.ScanCode
                                    join localeModel in localeModels on deleteModel.BusinessUnit equals localeModel.BusinessUnit
                                    select new
                                        {
                                            ItemId = itemModel.ItemId,
                                            ItemTypeCode = itemModel.ItemTypeCode,
                                            ItemTypeDescription = itemModel.ItemTypeDescription,
                                            LocaleName = localeModel.LocaleName,
                                            BusinessUnit = localeModel.BusinessUnit,
                                            ScanCodeId = itemModel.ScanCodeId,
                                            ScanCode = itemModel.ScanCode,
                                            ScanCodeTypeId = itemModel.ScanCodeTypeId,
                                            ScanCodeTypeDescription = itemModel.ScanCodeTypeDescription,
                                            Price = deleteModel.Price,
                                            StartDate = deleteModel.StartDate,
                                            EndDate = deleteModel.EndDate
                                        }).ToList();

                foreach (var itemPriceData in itemPriceList)
                {
                    var message = new Contracts.items
                    {
                        item = new Contracts.ItemType[]
                        {
                            new Contracts.ItemType
                            {
                                id = itemPriceData.ItemId,
                                @base = new Contracts.BaseItemType
                                {
                                    type = new Contracts.ItemTypeType
                                    {
                                        code = itemPriceData.ItemTypeCode,
                                        description = itemPriceData.ItemTypeDescription
                                    }
                                },
                                locale = new Contracts.LocaleType[]
                                {
                                    new Contracts.LocaleType
                                    {
                                        Action = Contracts.ActionEnum.Delete,
                                        ActionSpecified = true,
                                        id = itemPriceData.BusinessUnit,
                                        name = itemPriceData.LocaleName,
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
                                                    id = itemPriceData.ScanCodeId,
                                                    code = itemPriceData.ScanCode,
                                                    typeId = itemPriceData.ScanCodeTypeId,
                                                    typeIdSpecified = true,
                                                    typeDescription = itemPriceData.ScanCodeTypeDescription
                                                }                                    
                                            },
                                            prices = new Contracts.PriceType[]
                                            {
                                                new Contracts.PriceType
                                                {
                                                    type = new Contracts.PriceTypeType
                                                    {
                                                        description = ItemPriceDescriptions.TemporaryPriceReduction,
                                                        id = ItemPriceTypes.Tpr.ToString()
                                                    },
                                                    uom = new Contracts.UomType
                                                    {
                                                        codeSpecified = true,
                                                        nameSpecified = true,
                                                        code = Contracts.WfmUomCodeEnumType.EA,//GetUomCode(message.UomCode),
                                                        name = Contracts.WfmUomDescEnumType.EACH//GetUomName(message.UomName, message.ScanCode)
                                                    },
                                                    currencyTypeCode = Contracts.CurrencyTypeCodeEnum.USD,// GetCurrencyTypeCode(message.CurrencyCode),
                                                    priceAmount = new Contracts.PriceAmount
                                                    {
                                                        amount = itemPriceData.Price,
                                                        amountSpecified = true
                                                    },
                                                    priceMultiple = 1,
                                                    priceStartDate = itemPriceData.StartDate.Value,
                                                    priceStartDateSpecified = true,
                                                    priceEndDate = new DateTime(2050, 1, 1),
                                                    priceEndDateSpecified = true
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    };

                    messages.Add(serializer.Serialize(message, new Utf8StringWriter()));
                }
            }

            return messages;
        }
    }
}