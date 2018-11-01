using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Builds Price message for specific item/store combination.
        /// Also, assumes that one message is for only one item/store relationship
        /// </summary>
        /// <param name="request">List of prices for a specific item-store combination</param>
        /// <returns></returns>
        public string BuildMessage(PriceResetMessageBuilderModel request)
        {
            Contracts.items items = new Contracts.items();
            var basePrice = request.PriceResetPrices.First(); // for assigning item specific attributes to items object

            var prices = request.PriceResetPrices.Where(p => !p.PercentOff.HasValue).Select(p => CreatePriceType(p)).ToArray();
            var rewards = request.PriceResetPrices.Where(p => p.PercentOff.HasValue && p.PriceType == ItemPriceTypes.Codes.Rewards).Select(p => CreateRewardType(p)).ToArray();

            var storeItemAttributes = new StoreItemAttributesType
            {
                scanCode = new Contracts.ScanCodeType[]
                {
                    new Contracts.ScanCodeType
                    {
                        code = basePrice.ScanCode
                    }
                }
            };

            if (prices.Any())
            {
                storeItemAttributes.prices = prices;
            }

            if (rewards.Any())
            {
                storeItemAttributes.rewards = rewards;
            }
            
            items.item = new ItemType[]
            {
                new ItemType
                {
                    id = basePrice.ItemId,
                    @base = new Contracts.BaseItemType
                    {
                        type = new Contracts.ItemTypeType
                        {
                            code = basePrice.ItemTypeCode,
                            description = basePrice.ItemTypeDesc
                        }
                    },
                    locale = new LocaleType[]
                    {
                        new Contracts.LocaleType
                        {
                            id = basePrice.BusinessUnitId.ToString(),
                            name = basePrice.StoreName,
                            type = new Contracts.LocaleTypeType
                            {
                                code = Contracts.LocaleCodeType.STR,
                                description = Contracts.LocaleDescType.Store
                            },
                            Item = storeItemAttributes // contains all the price data
                        }
                    }
                }
            };

            return serializer.Serialize(items, new Utf8StringWriter());
        }

        private RewardType CreateRewardType(PriceResetPrice price)
        {
            var rewardType = new Contracts.RewardType
            {
                Id = price.GpmId?.ToString(),
                Action = ActionEnum.Add,
                ActionSpecified = true,
                type = new RewardTypeType
                {
                    id = Enum.Parse(typeof(Contracts.PriceTypeIdType), price.PriceType).ToString(),
                    description = ItemPriceTypes.Descriptions.ByCode[price.PriceType],
                },
                uom = new UomType
                {
                    codeSpecified = true,
                    nameSpecified = false,
                    code = GetEsbUomCode(price.SellableUom)
                },
                rewardMultiple = price.Multiple,
                rewardPercentageSpecified = true,
                rewardPercentage = price.PercentOff.Value,
                rewardStartDateSpecified = true,
                rewardStartDate = price.StartDate,
                rewardEndDateSpecified = true,
                rewardEndDate = price.EndDate.Value
            };

            return rewardType;
        }

        private Contracts.PriceType CreatePriceType(PriceResetPrice price)
        {
            var priceType = new Contracts.PriceType
            {
                Action = ActionEnum.Add,
                ActionSpecified = true,
                Id = price.GpmId?.ToString(),
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