using System;
using System.Collections.Generic;
using Icon.Esb.Schemas.Mammoth;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Logging;
using MammothR10Price.Serializer;
using Items = Icon.Esb.Schemas.Wfm.Contracts.items;
using Icon.Common.Xml;

namespace MammothR10Price.Mapper
{
    public class ItemPriceCanonicalMapper: IMapper<IList<MammothPriceType>, Items>
    {
        ILogger<ItemPriceCanonicalMapper> logger;
        ISerializer<Items> serializer;

        static readonly DateTime REGULAR_PRICE_START_DATE = new DateTime(1970, 1, 1, 0, 0, 0);

        public ItemPriceCanonicalMapper(ILogger<ItemPriceCanonicalMapper> logger, ISerializer<Items> serializer)
        {
            this.logger = logger;
            this.serializer = serializer;
        }

        public Items Transform(IList<MammothPriceType> mammothPrices)
        {
            Items itemPrices = new Items();
            itemPrices.item = new ItemType[mammothPrices.Count];
            
            for(int i=0; i<mammothPrices.Count; i++)
            {
                itemPrices.item[i] = new ItemType()
                {
                    id = mammothPrices[i].ItemId,
                    @base = new BaseItemType()
                    {
                        type = new ItemTypeType()
                        {
                            code = mammothPrices[i].ItemTypeCode
                        }
                    },
                    locale = new LocaleType[]
                    {
                        BuildLocaleType(mammothPrices[i])
                    }
                };
            }
            return itemPrices;
        }

        private LocaleType BuildLocaleType(MammothPriceType mammothPrice)
        {
            return new LocaleType()
            {
                Action = Constants.Action.DELETE.Equals(mammothPrice.Action.ToUpper()) ? ActionEnum.Delete : ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = mammothPrice.BusinessUnit.ToString(),
                name = mammothPrice.StoreName,
                type = new LocaleTypeType()
                {
                    code = LocaleCodeType.STR,
                    description = LocaleDescType.Store
                },
                Item = BuildStoreItemAttribute(mammothPrice)
            };
        }

        private StoreItemAttributesType BuildStoreItemAttribute(MammothPriceType mammothPrice)
        {
            StoreItemAttributesType storeItemAttributes = new StoreItemAttributesType()
            {
                scanCode = new ScanCodeType[]
                {
                    new ScanCodeType()
                    {
                        code = mammothPrice.ScanCode
                    }
                }
            };

            if(!mammothPrice.PriceType.Equals(Constants.PriceType.RWD) || 
                (mammothPrice.PriceType.Equals(Constants.PriceType.RWD) && mammothPrice.Price > 0))
            {
                storeItemAttributes.prices = new PriceType[]
                {
                    BuildPriceType(mammothPrice)
                };
            }

            if (mammothPrice.PriceType.Equals(Constants.PriceType.RWD) && mammothPrice.PercentOffSpecified)
            {
                storeItemAttributes.rewards = new RewardType[]
                {
                    BuildRewardType(mammothPrice)
                };
            }
            return storeItemAttributes;
        }

        private PriceType BuildPriceType(MammothPriceType mammothPrice)
        {
            PriceType price = new PriceType()
            {
                type = new PriceTypeType()
                {
                    id = GetPriceTypeId(mammothPrice.PriceType, mammothPrice.ScanCode),
                    description = mammothPrice.PriceType.Equals(Constants.PriceType.TPR) ?
                                Constants.Description.TemporaryPriceReduction : Constants.Description.RegularPrice
                },
                uom = BuildUom(mammothPrice.SellableUom, mammothPrice.ScanCode),
                currencyTypeCode = GetCurrencyTypeCode(mammothPrice.CurrencyCode, mammothPrice.ScanCode),
                priceAmount = new PriceAmount()
                {
                    amountSpecified = true,
                    amount = mammothPrice.Price,
                },
                priceMultiple = mammothPrice.Multiple,
                priceStartDateSpecified = true,
                priceStartDate = mammothPrice.PriceType.Equals(Constants.PriceType.REG) ? REGULAR_PRICE_START_DATE : mammothPrice.StartDate,
            };

            if (mammothPrice.EndDate.HasValue)
            {
                price.priceEndDateSpecified = true;
                price.priceEndDate = mammothPrice.EndDate.Value;
            }
            return price;
        }

        private RewardType BuildRewardType(MammothPriceType mammothPrice)
        {
            RewardType reward = new RewardType()
            {
                type = new RewardTypeType()
                {
                    id = PriceTypeIdType.RWD,
                    description = Constants.Description.Rewards,
                    type = new RewardTypeType()
                    {
                        id = GetPriceTypeId(mammothPrice.PriceTypeAttribute, mammothPrice.ScanCode),
                        description = mammothPrice.PriceTypeAttribute.Equals(Constants.PriceType.PMI) ? Constants.Description.PrimeMemberIncremental : 
                            mammothPrice.PriceTypeAttribute.Equals(Constants.PriceType.PMD) ? Constants.Description.PrimeMemberDeal : Constants.Description.Reward
                    }
                },
                uom = BuildUom(mammothPrice.SellableUom, mammothPrice.ScanCode),
                rewardMultiple = mammothPrice.Multiple,
                rewardStartDate = mammothPrice.StartDate,
                rewardStartDateSpecified = true,
            };

            if(mammothPrice.Price > 0)
            {
                reward.rewardAmount = new RewardAmount()
                {
                    amount = mammothPrice.Price
                };
            }
            if (mammothPrice.PercentOff.HasValue)
            {
                reward.rewardPercentageSpecified = true;
                reward.rewardPercentage = mammothPrice.PercentOff.Value;
            }
            if (mammothPrice.EndDate.HasValue)
            {
                reward.rewardEndDateSpecified = true;
                reward.rewardEndDate = mammothPrice.EndDate.Value;
            }
            return reward;
        }

        private UomType BuildUom(string uomCode, string scanCode)
        {
            return new UomType()
            {
                codeSpecified = true,
                code = GetUomCode(uomCode, scanCode)
            };
        }

        private WfmUomCodeEnumType GetUomCode(string uomCode, string scanCode)
        {
            WfmUomCodeEnumType uomEnum;

            if (Enum.TryParse(uomCode, out uomEnum))
            {
                return uomEnum;
            }
            logger.Warn(string.Format("The UOM {0} is not recognized for scan code {1}. EACH will be sent as the UOM.", uomCode, scanCode));
            return WfmUomCodeEnumType.EA;
        }

        private CurrencyTypeCodeEnum GetCurrencyTypeCode(string currencyTypeCode, string scanCode)
        {
            CurrencyTypeCodeEnum currencyTypeCodeEnum;
            if (Enum.TryParse(currencyTypeCode, out currencyTypeCodeEnum))
            {
                return currencyTypeCodeEnum;
            }
            logger.Warn(string.Format("The CurrencyTypeCode {0} is not recognized for scan code {1}. USD will be sent as the Currency.", currencyTypeCode, scanCode));
            return CurrencyTypeCodeEnum.USD;
        }

        private PriceTypeIdType GetPriceTypeId(string priceTypeId, string scanCode)
        {
            PriceTypeIdType priceTypeIdType;
            if(Enum.TryParse(priceTypeId, out priceTypeIdType))
            {
                return priceTypeIdType;
            }
            logger.Warn(string.Format("The PriceType {0} is not recognized for scan code {1}. RWD will be sent as the PriceType.", priceTypeId, scanCode));
            return PriceTypeIdType.RWD;
        }

        public string ToXml(Items itemPrice)
        {
            return serializer.Serialize(itemPrice, new Utf8StringWriter());
        }
    }
}
