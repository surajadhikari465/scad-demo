using Esb.Core.Serializer;
using Mammoth.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Schemas.Wfm.PreGpm;
using Contracts = Icon.Esb.Schemas.Wfm.PreGpm.Contracts;

namespace AmazonLoad.MammothPrice
{
    public static class MessageBuilderForPreGpmPrice
    {
        public static string BuildPreGpmMessage(List<PriceModel> messages)
        {
            List<Contracts.ItemType> items = new List<Contracts.ItemType>();
            foreach (var message in messages)
            {
                items.Add(
                    BuildPreGpmItemType(
                        message,
                        message.PriceTypeCode,
                        message.Price,
                        message.Multiple,
                        message.StartDate,
                        message.EndDate));
            }
            var preGpmSerializer = new Serializer<Contracts.items>();
            return preGpmSerializer.Serialize(new Contracts.items { item = items.ToArray() });
        }

        private static Contracts.ItemType BuildPreGpmItemType(
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
                                CreatePreGpmPriceType(message, priceTypeCode, price, multiple, startDate, endDate)
                            }
                        }
                    }
                }
            };

            return itemType;
        }

        private static Contracts.PriceType CreatePreGpmPriceType(
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
