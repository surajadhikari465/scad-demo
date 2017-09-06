using Esb.Core.MessageBuilders;
using Icon.Esb.Schemas.Wfm.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using WebSupport.DataAccess.Models;
using Mammoth.Common.DataAccess;
using Esb.Core.Serializer;
using System.Globalization;

namespace WebSupport.Models
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
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        ActionSpecified = true,
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
                            },
                            traits = new Contracts.TraitType[]
                            {
                                new TraitType
                                {
                                    Action = price.NewTagExpiration.HasValue ? ActionEnum.AddOrUpdate : ActionEnum.Delete,
                                    ActionSpecified = true,
                                    code = EsbConstants.NewTagExpirationTraitCode,
                                    type = new TraitTypeType
                                    {
                                        description = EsbConstants.NewTagExpirationTraitDescription,
                                        value = new TraitValueType[]
                                        {
                                            new TraitValueType
                                            {
                                                value = price.NewTagExpiration.HasValue
                                                    ? price.NewTagExpiration.Value.ToString(DateTimeFormat, CultureInfo.InvariantCulture)
                                                    : string.Empty
                                            }
                                        }
                                    }
                                }
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
                type = new Contracts.PriceTypeType
                {
                    description = ItemPriceTypes.Descriptions.ByCode[price.PriceType],
                    id = price.PriceType
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
                priceStartDateSpecified = true
            };

            if (price.EndDate.HasValue)
            {
                priceType.priceEndDate = price.EndDate.Value;
                priceType.priceEndDateSpecified = true;
            }

            return priceType;
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