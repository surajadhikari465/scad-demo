using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazonLoad.Common;
using Esb.Core.Serializer;
using System.Globalization;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;

namespace AmazonLoad.PrimeAffinityPsg
{
    internal class MessageBuilderForPrimeAffinityPsg
    {
        public static Serializer<Contracts.items> serializer = new Serializer<Contracts.items>();
        public static string PrimeAffinityPsgGroupId = "PrimeAffinityPSG";
        public static string PrimeAffinityPsgGroupName = "PrimeAffinityPSG";
        public static string PrimeAffinityPsgGroupType = "Consumable";

        internal protected static string BuildMessage(IEnumerable<PrimeAffinityPsgModel> itemLocaleModels)
        {
            Contracts.items items = new Contracts.items
            {
                item = itemLocaleModels.Select(i => ConvertToContractsItemType(i)).ToArray()
            };

            return serializer.Serialize(items);
        }

        internal protected static string BuildMessage(IEnumerable<PrimeAffinityPsgModel> itemLocaleModels,
            string primeAffinityPsgGroupId, string primeAffinityPsgGroupName, string primeAffinityPsgGroupType)
        {
            PrimeAffinityPsgGroupId = primeAffinityPsgGroupId;
            PrimeAffinityPsgGroupName = primeAffinityPsgGroupName;
            PrimeAffinityPsgGroupType = primeAffinityPsgGroupType;

            return BuildMessage(itemLocaleModels);
        }

        internal protected static Contracts.ItemType ConvertToContractsItemType(PrimeAffinityPsgModel model)
        {
            return new Contracts.ItemType
            {
                id = model.ItemId,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = model.ItemTypeCode,
                        description = model.ItemTypeDesc
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        id = model.BusinessUnit.ToString(),
                        name = model.LocaleName,
                        traits = new Contracts.TraitType[]
                        {
                            new Contracts.TraitType
                            {
                                code = TraitCodes.PsBusinessUnitId,
                                type = new Contracts.TraitTypeType
                                {
                                    description = TraitDescriptions.PsBusinessUnitId,
                                    value = new Contracts.TraitValueType[]
                                    {
                                        new Contracts.TraitValueType
                                        {
                                            value = model.BusinessUnit.ToString()
                                        }
                                    }
                                }
                            }
                        },
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
                                    code = model.ScanCode,
                                }
                            },
                            selectionGroups = CreatePrimeAffinitySelectionGroup(
                                model.PrimeEligible,
                                PrimeAffinityPsgGroupId,
                                PrimeAffinityPsgGroupName,
                                PrimeAffinityPsgGroupType),
                        }
                    }
                }
            };
        }

        internal protected static Contracts.SelectionGroupsType CreatePrimeAffinitySelectionGroup(bool isPrimeEligible,
            string primePsgGroupId, string primePsgGroupName, string primePsgGroupType)
        {
            return new Contracts.SelectionGroupsType
            {
                group = new Contracts.GroupTypeType[]
                {
                    new Contracts.GroupTypeType
                    {
                        Action = isPrimeEligible ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete,
                        ActionSpecified = true,
                        id = primePsgGroupId,
                        name = primePsgGroupName,
                        type = primePsgGroupType
                    }
                }
            };
        }
    }
}

