using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess;
using System;
using System.Globalization;
using System.Linq;
using WebSupport.DataAccess.Models;
using WebSupport.Models;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace WebSupport.MessageBuilders
{
    public class PriceResetMessageBuilder : IMessageBuilder<PriceResetMessageBuilderModel>
    {
        private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        private ISerializer<Contracts.items> serializer;

        public PriceResetMessageBuilder(ISerializer<Contracts.items> serializer)
        {
            this.serializer = serializer;
        }

        public string BuildMessage(PriceResetMessageBuilderModel request)
        {
            Contracts.items items = new Contracts.items();
            items.item = request.PriceResetPrices
                .Select(p => CreatePriceMessageType(p))
                .ToArray();

            return serializer.Serialize(items, new Utf8StringWriter());
        }

        private ItemType CreatePriceMessageType(PriceResetPrice price)
        {
            ItemType item = new ItemType
            {
                id = price.ItemId,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = price.ItemTypeCode,
                        description = price.ItemTypeDesc
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        id = price.BusinessUnitId.ToString(),
                        name = price.StoreName,
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
                                    code = price.ScanCode,
                                }
                            },
                            prices = new Contracts.PriceType[]
                            {
                                CreatePriceType(price)
                            }
                        }
                    }
                }
            };

            return item;
        }

        private Contracts.PriceType CreatePriceType(PriceResetPrice price)
        {
            var priceType = new Contracts.PriceType
            {
                Action = ActionEnum.Add,
                ActionSpecified = true,
                gpmId = price.GpmId?.ToString(),
                type = new Contracts.PriceTypeType
                {
                    description = ItemPriceTypes.Descriptions.ByCode[price.PriceType],
                    id = (Contracts.PriceTypeIdType)Enum.Parse(typeof(Contracts.PriceTypeIdType), price.PriceType),
                    type = new PriceTypeType
                    {
                        description = ItemPriceTypes.Descriptions.ByCode[price.PriceTypeAttribute],
                        id = (Contracts.PriceTypeIdType)Enum.Parse(typeof(Contracts.PriceTypeIdType), price.PriceTypeAttribute)
                    }
                },
                uom = new Contracts.UomType
                {
                    codeSpecified = true,
                    nameSpecified = true,
                    code = GetEsbUomCode(price.SellableUom),
                    name = GetEsbUomName(price.SellableUom)
                },
                currencyTypeCode = GetEsbCurrencyTypeCode(price.CurrencyCode),
                priceAmount = new Contracts.PriceAmount
                {
                    amount = price.Price,
                    amountSpecified = true
                },
                priceMultiple = price.Multiple,
                priceStartDate = price.StartDate,
                priceStartDateSpecified = true,
                traits = CreateTraits(price)
            };

            if (price.EndDate.HasValue)
            {
                priceType.priceEndDate = price.EndDate.Value;
                priceType.priceEndDateSpecified = true;
            }

            return priceType;
        }

        private TraitType[] CreateTraits(PriceResetPrice price)
        {
            return new Contracts.TraitType[]
            {
                new TraitType
                {
                    ActionSpecified = false,
                    code = EsbConstants.TagExpirationDateTraitCode,
                    type = new TraitTypeType
                    {
                        description = EsbConstants.TagExpirationDateTraitDescription,
                        value = new TraitValueType[]
                        {
                            new TraitValueType
                            {
                                value = price.TagExpirationDate.HasValue
                                    ? price.TagExpirationDate.Value.ToString(DateTimeFormat, CultureInfo.InvariantCulture)
                                    : string.Empty
                            }
                        }
                    }
                }
            };
        }

        private Contracts.WfmUomCodeEnumType GetEsbUomCode(string uomCode)
        {
            switch (uomCode)
            {
                case UomCodes.Each:
                    return Contracts.WfmUomCodeEnumType.EA;
                case UomCodes.Pound:
                    return Contracts.WfmUomCodeEnumType.LB;
                default:
                    return Contracts.WfmUomCodeEnumType.EA;
            }
        }

        private Contracts.WfmUomDescEnumType GetEsbUomName(string uomCode)
        {
            switch (uomCode)
            {
                case UomCodes.Each:
                    return Contracts.WfmUomDescEnumType.EACH;
                case UomCodes.Pound:
                    return Contracts.WfmUomDescEnumType.POUND;
                default:
                    return Contracts.WfmUomDescEnumType.EACH;
            }
        }

        private Contracts.CurrencyTypeCodeEnum GetEsbCurrencyTypeCode(string currencyTypeCode)
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