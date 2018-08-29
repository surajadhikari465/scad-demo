using Esb.Core.Serializer;
using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace AmazonLoad.MammothPrice
{
    public static class MessageBuilderForGpmPrice
    {
        public static Serializer<Contracts.items> gpmSerializer = new Serializer<Contracts.items>();

        public static string BuildGpmMessage(List<PriceModelGpm> messages)
        {
            List<Contracts.ItemType> items = new List<Contracts.ItemType>();
            foreach (var message in messages)
            {
                items.Add(
                    BuildGpmItemType(
                        message,
                        message.PriceType,
                        message.Price,
                        message.Multiple,
                        message.StartDate,
                        message.EndDate));
            }
            return gpmSerializer.Serialize(new Contracts.items { item = items.ToArray() });
        }

        private static Contracts.ItemType BuildGpmItemType(
            PriceModelGpm message,
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
                                CreateGpmPriceType(message, MapPriceIdTypeFromCode(priceTypeCode), price, multiple, startDate, endDate)
                            }
                        }
                    }
                }
            };

            return itemType;
        }

        private static Contracts.PriceType CreateGpmPriceType(
            PriceModelGpm message,
            PriceTypeIdType priceTypeIdType,
            decimal price,
            int multiple,
            DateTime startDate,
            DateTime? endDate)
        {
            var priceType = new Contracts.PriceType
            {
                type = new Contracts.PriceTypeType
                {
                    description = ItemPriceTypes.Descriptions.ByCode[priceTypeIdType.ToString()],
                    id = priceTypeIdType,
                    type =  new Contracts.PriceTypeType
                    {
                        id = priceTypeIdType,
                        description = message.PriceType,
                    }
                },
                uom = new Contracts.UomType
                {
                    codeSpecified = true,
                    nameSpecified = true,
                    code = GetEsbUomCode(message.SellableUOM),
                    name = GetEsbUomName(message.SellableUOM, message.ScanCode)
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

        private static PriceTypeIdType MapPriceIdTypeFromCode(string priceTypeCode)
        {
            var priceTypeIdType = new PriceTypeIdType();
            switch (priceTypeCode)
            {
                case Mammoth.Common.DataAccess.ItemPriceTypes.Codes.RegularPrice:
                    priceTypeIdType = PriceTypeIdType.REG;
                    break;
                case Mammoth.Common.DataAccess.ItemPriceTypes.Codes.TemporaryPriceReduction:
                    priceTypeIdType = PriceTypeIdType.TPR;
                    break;
                case Mammoth.Common.DataAccess.ItemPriceTypes.Codes.Competitive:
                    priceTypeIdType = PriceTypeIdType.CMP;
                    break;
                case Mammoth.Common.DataAccess.ItemPriceTypes.Codes.EverydayValue:
                    priceTypeIdType = PriceTypeIdType.EDV;
                    break;
                case Mammoth.Common.DataAccess.ItemPriceTypes.Codes.Clearance:
                    priceTypeIdType = PriceTypeIdType.CLR;
                    break;
                case Mammoth.Common.DataAccess.ItemPriceTypes.Codes.Discontinued:
                    priceTypeIdType = PriceTypeIdType.DIS;
                    break;
                case Mammoth.Common.DataAccess.ItemPriceTypes.Codes.MarketSale:
                    priceTypeIdType = PriceTypeIdType.MSAL;
                    break;
                case Mammoth.Common.DataAccess.ItemPriceTypes.Codes.SpecialSale:
                    priceTypeIdType = PriceTypeIdType.SSAL;
                    break;
                case Mammoth.Common.DataAccess.ItemPriceTypes.Codes.InStoreSpecial:
                    priceTypeIdType = PriceTypeIdType.ISS;
                    break;
                case Mammoth.Common.DataAccess.ItemPriceTypes.Codes.GloballySetPrice:
                    priceTypeIdType = PriceTypeIdType.GSP;
                    break;
                case "RWD":
                    priceTypeIdType = PriceTypeIdType.RWD;
                    break;
                default:
                    break;
            }
            return priceTypeIdType;
        }
    }
}
