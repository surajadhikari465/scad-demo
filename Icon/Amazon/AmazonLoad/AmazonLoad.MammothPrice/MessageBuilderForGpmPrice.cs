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

        public static string BuildGpmMessage(IEnumerable<PriceModelGpm> gpmPriceModels)
        {
            Contracts.items items = new Contracts.items
            {
                item = gpmPriceModels.Select(p => BuildGpmItemType(p)).ToArray()
            };
            return gpmSerializer.Serialize(items);
        }

        internal static Contracts.ItemType BuildGpmItemType(PriceModelGpm gpmPriceModel)
        {
            var itemType = new Contracts.ItemType
            {
                id = gpmPriceModel.ItemId,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = gpmPriceModel.ItemTypeCode,
                        description = gpmPriceModel.ItemTypeDesc
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        ActionSpecified = true,
                        id = gpmPriceModel.BusinessUnitId.ToString(),
                        name = gpmPriceModel.LocaleName,
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
                                    code = gpmPriceModel.ScanCode,
                                }
                            },
                            prices = new Contracts.PriceType[]
                            {
                                CreateGpmPriceType(gpmPriceModel)
                            }
                        }
                    }
                }
            };

            return itemType;
        }

        internal static Contracts.PriceType CreateGpmPriceType(PriceModelGpm gpmPriceModel)
        {
            var priceTypeIdType = MapPriceIdTypeFromCode(gpmPriceModel.PriceType);

            var priceType = new Contracts.PriceType
            {
                Id = gpmPriceModel.GpmId.Value.ToString().ToUpper(),
                Action = ActionEnum.Add,
                ActionSpecified = true,
                type = new Contracts.PriceTypeType
                {
                    id = priceTypeIdType,
                    description = ItemPriceTypes.Descriptions.ByCode[priceTypeIdType.ToString()],
                    type = string.IsNullOrWhiteSpace(gpmPriceModel.SubPriceTypeCode) ? null
                        : new Contracts.PriceTypeType
                        {
                            id = MapPriceIdTypeFromCode(gpmPriceModel.SubPriceTypeCode),
                            description = gpmPriceModel.SubPriceTypeDesc,
                        }
                },
                uom = new Contracts.UomType
                {
                    codeSpecified = true,
                    nameSpecified = true,
                    code = GetEsbUomCode(gpmPriceModel.SellableUOM),
                    name = GetEsbUomName(gpmPriceModel.SellableUOM, gpmPriceModel.ScanCode)
                },
                currencyTypeCode = GetEsbCurrencyTypeCode(gpmPriceModel.CurrencyCode),
                priceAmount = new Contracts.PriceAmount
                {
                    amount = gpmPriceModel.Price,
                    amountSpecified = true
                },
                priceMultiple = gpmPriceModel.Multiple,
                priceStartDate = gpmPriceModel.StartDate,
                priceStartDateSpecified = true,
                traits = CreateTraitsForGpmPrice(gpmPriceModel)
            };

            if (gpmPriceModel.EndDate.HasValue)
            {
                priceType.priceEndDate = gpmPriceModel.EndDate.Value;
                priceType.priceEndDateSpecified = true;
            }

            return priceType;
        }

        internal static Contracts.TraitType[] CreateTraitsForGpmPrice(PriceModelGpm gpmPriceModel)
        {
            Contracts.TraitType nteTrait = CreateTrait(
                traitCode: "NTE",
                traitDesc: "NewTagExpiration",
                traitValue: gpmPriceModel.TagExpirationDate.HasValue
                     ? gpmPriceModel.TagExpirationDate.Value.ToString("yyyy'-'MM'-'dd'T'00:00:00")
                     : string.Empty,
                action: gpmPriceModel.TagExpirationDate.HasValue 
                    ? ActionEnum.AddOrUpdate 
                    : ActionEnum.Delete);

                return new Contracts.TraitType[]
                {
                    nteTrait
                };
        }

        internal static Contracts.TraitType CreateTrait(string traitCode, string traitDesc, string traitValue, ActionEnum? action = null)
        {
            var traitType = new Contracts.TraitType
            {
                code = traitCode,
                type = new Contracts.TraitTypeType
                {
                    description = traitDesc,
                    value = new Contracts.TraitValueType[]
                    {
                        new Contracts.TraitValueType
                        {
                            value = traitValue == null ? string.Empty : traitValue
                        }
                    }
                }
            };
            if (action.HasValue)
            {
                traitType.ActionSpecified = action.HasValue;
                traitType.Action = action.Value;
            };
            return traitType;
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
