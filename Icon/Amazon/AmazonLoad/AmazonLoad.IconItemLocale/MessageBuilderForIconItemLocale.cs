using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esb.Core.Serializer;
using Icon.Framework;
using Icon.Esb.Schemas.Wfm.Contracts;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace AmazonLoad.IconItemLocale
{
    public static class MessageBuilderForIconItemLocale
    {
        public static Serializer<Contracts.items> serializer = new Serializer<Contracts.items>();
        public static IconItemLocalePsgMapper PsgMapper;

        public static string BuildMessage(IEnumerable<ItemLocaleModelForWormhole> itemLocaleModels, IconItemLocalePsgMapper psgMapper)
        {
            if (itemLocaleModels == null || itemLocaleModels.Count() == 0)
            {
                throw new ArgumentException("BuildMessage() was called with invalid arguments.  Parameter 'itemLocaleModels' must be a non-null and non-empty list.");
            }
            PsgMapper = psgMapper;

            Contracts.items items = new Contracts.items
            {
                item = itemLocaleModels.Select(i => ConvertToContractsItemType(i)).ToArray()
            };

            return serializer.Serialize(items);
        }

        internal static Contracts.ItemType ConvertToContractsItemType(ItemLocaleModelForWormhole itemLocaleModel)
        {
            var contractsItemType = new Contracts.ItemType
            {
                Action = itemLocaleModel.Authorized ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete,
                ActionSpecified = true,
                id = itemLocaleModel.InforItemId,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = itemLocaleModel.ItemTypeCode,
                        description = itemLocaleModel.ItemTypeDesc
                    }
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        Action =  itemLocaleModel.Authorized ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete,
                        ActionSpecified = true,
                        id = itemLocaleModel.BusinessUnit.ToString(),
                        name = itemLocaleModel.LocaleName,
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
                                            value = itemLocaleModel.BusinessUnit.ToString()
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
                                    code = itemLocaleModel.ScanCode,
                                    typeId = itemLocaleModel.ScanCodeTypeId,
                                    typeIdSpecified = true,
                                    typeDescription = itemLocaleModel.ScanCodeTypeDesc
                                }
                            },
                            traits = CreateTraitsForItemLocale(itemLocaleModel),
                            selectionGroups  = new Contracts.SelectionGroupsType
                            {
                                group = PsgMapper.CreatePsgElementsForTraits(itemLocaleModel).ToArray()
                            },
                        }
                    }
                }
            };

            ProcessLinkedItem(itemLocaleModel, contractsItemType);

            return contractsItemType;
        }

        internal static Contracts.TraitType[] CreateTraitsForItemLocale(ItemLocaleModelForWormhole itemLocaleModel)
        {
            return new Contracts.TraitType[]
            {
                CreateTrait(TraitCodes.LockedForSale, TraitDescriptions.LockedForSale,
                    itemLocaleModel.LockedForSale ? "1" : "0",
                    itemLocaleModel.LockedForSale ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete),
                CreateTrait(TraitCodes.Recall, TraitDescriptions.Recall,
                    itemLocaleModel.Recall ? "1" : "0",
                    itemLocaleModel.Recall ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete),
                CreateTrait(TraitCodes.SoldByWeight, TraitDescriptions.SoldByWeight,
                    itemLocaleModel.Sold_By_Weight ? "1" : "0",
                    itemLocaleModel.Sold_By_Weight ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete),
                CreateTrait(TraitCodes.QuantityRequired, TraitDescriptions.QuantityRequired,
                    itemLocaleModel.Quantity_Required ? "1" : "0",
                    itemLocaleModel.Quantity_Required ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete),
                CreateTrait(TraitCodes.PriceRequired, TraitDescriptions.PriceRequired,
                    itemLocaleModel.Price_Required ? "1" : "0",
                    itemLocaleModel.Price_Required ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete),
                CreateTrait(TraitCodes.QuantityProhibit, TraitDescriptions.QuantityProhibit,
                    itemLocaleModel.QtyProhibit ? "1" : "0",
                    itemLocaleModel.QtyProhibit ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete),
                CreateTrait(TraitCodes.VisualVerify, TraitDescriptions.VisualVerify,
                    itemLocaleModel.VisualVerify ? "1" : "0",
                    itemLocaleModel.VisualVerify ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete),
                CreateTrait(TraitCodes.PosScaleTare, TraitDescriptions.PosScaleTare,
                    itemLocaleModel.PosScaleTare.GetValueOrDefault(0).ToString(),
                    itemLocaleModel.PosScaleTare.GetValueOrDefault(0)>0 ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete),
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

        private static void ProcessLinkedItem(ItemLocaleModelForWormhole itemLocaleModel, Contracts.ItemType miniBulkEntry)
        {
            if (itemLocaleModel.LinkedItemId != null)
            {
                var links = new List<Contracts.LinkTypeType>();
                var groups = new List<Contracts.GroupTypeType>();

                AddLinkedItem(itemLocaleModel, links, groups, Contracts.ActionEnum.AddOrUpdate);
                
                (miniBulkEntry.locale[0].Item as Contracts.StoreItemAttributesType).links = links.Any() ? links.ToArray() : null;
                (miniBulkEntry.locale[0].Item as Contracts.StoreItemAttributesType).groups = groups.Any() ? groups.ToArray() : null;
            }
        }

        private static void AddLinkedItem(ItemLocaleModelForWormhole itemLocaleModel,
            List<Contracts.LinkTypeType> links,
            List<Contracts.GroupTypeType> groups,
            ActionEnum action)
        {
            if (itemLocaleModel.LinkedItemId != null)
            {
                string groupTypeDescription = null;

                if (itemLocaleModel.LinkedItemTypeCode == ItemTypeCodes.Deposit)
                {
                    groupTypeDescription = Contracts.RetailTransactionItemTypeEnum.Deposit.ToString();
                }
                else if (itemLocaleModel.LinkedItemTypeCode == ItemTypeCodes.Fee)
                {
                    groupTypeDescription = Contracts.RetailTransactionItemTypeEnum.Warranty.ToString();
                }
                else
                {
                    return;
                }

                links.Add(new Contracts.LinkTypeType
                {
                    parentIdSpecified = true,
                    childIdSpecified = true,
                    parentId = itemLocaleModel.LinkedItemId.GetValueOrDefault(),
                    childId = itemLocaleModel.ItemId
                });
                groups.Add(new Contracts.GroupTypeType
                {
                    ActionSpecified = true,
                    Action = action,
                    id = itemLocaleModel.LinkedItemId.ToString() + "_" + itemLocaleModel.ItemId.ToString(),
                    description = groupTypeDescription
                });
            }
        }
    }
}
