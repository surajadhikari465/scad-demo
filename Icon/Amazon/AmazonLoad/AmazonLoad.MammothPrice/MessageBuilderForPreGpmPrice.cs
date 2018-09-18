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
        public static string BuildPreGpmMessage(IEnumerable<PriceModel> messages)
        {
            List<Contracts.ItemType> items = new List<Contracts.ItemType>();
            foreach (var message in messages)
            {
                items.Add(BuildPreGpmItemType(message));
            }
            var preGpmSerializer = new Serializer<Contracts.items>();
            return preGpmSerializer.Serialize(new Contracts.items { item = items.ToArray() });
        }

        internal static Contracts.ItemType BuildPreGpmItemType(PriceModel priceModel)
        {
            var itemType = new Contracts.ItemType
            {
                id = priceModel.ItemId,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = priceModel.ItemTypeCode,
                        description = priceModel.ItemTypeDesc
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        id = priceModel.BusinessUnitId.ToString(),
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        ActionSpecified = true,
                        name = priceModel.LocaleName,
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
                                    code = priceModel.ScanCode,
                                }
                            },
                            prices = new Contracts.PriceType[]
                            {
                                CreatePreGpmPriceType(priceModel)
                            }
                        }
                    }
                }
            };

            return itemType;
        }

        internal static Contracts.PriceType CreatePreGpmPriceType(PriceModel priceModel)
        {
            var priceType = new Contracts.PriceType
            {
                type = new Contracts.PriceTypeType
                {
                    id = priceModel.PriceTypeCode,
                    description = ItemPriceTypes.Descriptions.ByCode[priceModel.PriceTypeCode],
                    type = string.IsNullOrWhiteSpace(priceModel.SubPriceTypeCode) ? null
                        : new Contracts.PriceTypeType
                        {
                            id = priceModel.SubPriceTypeCode,
                            description = priceModel.SubPriceTypeDesc,
                        }
                },
                uom = new Contracts.UomType
                {
                    codeSpecified = true,
                    nameSpecified = true,
                    code = GetEsbUomCode(priceModel.UomCode),
                    name = GetEsbUomName(priceModel.UomCode, priceModel.ScanCode)
                },
                currencyTypeCode = GetEsbCurrencyTypeCode(priceModel.CurrencyCode),
                priceAmount = new Contracts.PriceAmount
                {
                    amount = priceModel.Price,
                    amountSpecified = true
                },
                priceMultiple = priceModel.Multiple,
                priceStartDate = priceModel.StartDate,
                priceStartDateSpecified = true,
            };

            if (priceModel.EndDate.HasValue)
            {
                priceType.priceEndDate = priceModel.EndDate.Value;
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
