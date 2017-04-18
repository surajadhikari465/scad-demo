using Infor.Services.NewItem.Models;
using System;
using System.Collections.Generic;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using System.Linq;
using Icon.Framework;
using Esb.Core.Mappers;
using Esb.Core.Serializer;
using Infor.Services.NewItem.Cache;
using Icon.Common.DataAccess;
using Infor.Services.NewItem.Queries;
using System.Text.RegularExpressions;
using Infor.Services.NewItem.Constants;
using Infor.Services.NewItem.Extensions;

namespace Infor.Services.NewItem.MessageBuilders
{
    public class NewItemMessageBuilder : IMessageBuilder<IEnumerable<NewItemModel>>
    {
        private const string NoSubTeamId = "0000";
        private const string NoSubTeamName = "No Subteam (0000)";
        private IUomMapper uomMapper;
        private ISerializer<Contracts.items> serializer;
        private IIconCache iconCache;
        private IQueryHandler<GetItemIdsQuery, Dictionary<string, int>> getItemIdsQueryHandler;
        private InforNewItemApplicationSettings settings;

        public NewItemMessageBuilder(
            IUomMapper uomMapper, 
            ISerializer<Contracts.items> serializer, 
            IIconCache iconCache, 
            IQueryHandler<GetItemIdsQuery, Dictionary<string, int>> getItemIdsQueryHandler,
            InforNewItemApplicationSettings settings)
        {
            this.uomMapper = uomMapper;
            this.serializer = serializer;
            this.iconCache = iconCache;
            this.getItemIdsQueryHandler = getItemIdsQueryHandler;
            this.settings = settings;
        }

        public string BuildMessage(IEnumerable<NewItemModel> model)
        {
            Dictionary<string, int> itemIds = getItemIdsQueryHandler.Search(new GetItemIdsQuery
            {
                ScanCodes = model.Select(ii => ii.ScanCode).ToList()
            });

            Contracts.items itemMessage = new Contracts.items
            {
                item = model.Select(ni => new Contracts.ItemType
                {
                    Action = Contracts.ActionEnum.AddOrUpdate,
                    ActionSpecified = true,
                    id = itemIds.ContainsKey(ni.ScanCode) ? itemIds[ni.ScanCode] : 0,
                    @base = new Contracts.BaseItemType
                    {
                        type = new Contracts.ItemTypeType
                        {
                            code = ni.IsRetailSale ? ItemTypeCodes.RetailSale : ItemTypeCodes.NonRetail,
                            description = ni.IsRetailSale ? ItemTypes.Descriptions.RetailSale : ItemTypes.Descriptions.NonRetail
                        }
                    },
                    locale = new Contracts.LocaleType[]
                    {
                        new Contracts.LocaleType
                        {
                            id = Locales.WholeFoods.ToString(),
                            name = "Whole Foods Market",
                            type = new Contracts.LocaleTypeType
                            {
                                code = Contracts.LocaleCodeType.CHN,
                                description = Contracts.LocaleDescType.Chain
                            },
                            Item = new Contracts.EnterpriseItemAttributesType
                            {
                                scanCodes = new Contracts.ScanCodeType[]
                                {
                                    new Contracts.ScanCodeType
                                    {
                                        code = ni.ScanCode,
                                        typeDescription = GetScanCodeType(ni.ScanCode)
                                    }
                                },
                                hierarchies = new Contracts.HierarchyType[]
                                {
                                    new Contracts.HierarchyType
                                    {
                                        id = Hierarchies.Brands,
                                        @class = new Contracts.HierarchyClassType[]
                                        {
                                            new Contracts.HierarchyClassType
                                            {
                                                id =  GetBrandId(ni.IconBrandId),// > 0 ? ni.IconBrandId.ToString() : "0",
                                                name = GetBrandName(ni.IconBrandId),// > 0 ? iconCache.BrandDictionary[ni.IconBrandId].Name : string.Empty,
                                                parentId = new Contracts.hierarchyParentClassType
                                                {
                                                    Value = 0
                                                }
                                            }
                                        },
                                        name = HierarchyNames.Brands
                                    },
                                    new Contracts.HierarchyType
                                    {
                                        id = Hierarchies.Tax,
                                        @class = new Contracts.HierarchyClassType[]
                                        {
                                            new Contracts.HierarchyClassType
                                            {
                                                id = ni.TaxClassCode,
                                                name = GetTaxName(ni.TaxClassCode),
                                                parentId = new Contracts.hierarchyParentClassType
                                                {
                                                    Value = 0
                                                }
                                            }
                                        },
                                        name = HierarchyNames.Tax
                                    },
                                    new Contracts.HierarchyType
                                    {
                                        id = Hierarchies.Financial,
                                        @class = new Contracts.HierarchyClassType[]
                                        {
                                            new Contracts.HierarchyClassType
                                            {
                                                id = GetSubTeamId(ni.SubTeamNumber),
                                                name = GetSubTeamName(ni.SubTeamNumber),
                                                parentId = new Contracts.hierarchyParentClassType
                                                {
                                                    Value = 0
                                                }
                                            }
                                        },
                                        name = HierarchyNames.Financial
                                    },
                                    new Contracts.HierarchyType
                                    {
                                        id = Hierarchies.National,
                                        @class = new Contracts.HierarchyClassType[]
                                        {
                                            new Contracts.HierarchyClassType
                                            {
                                                id = GetNationalClassId(ni.NationalClassCode),
                                                name = GetNationalClassName(ni.NationalClassCode),
                                                parentId = new Contracts.hierarchyParentClassType
                                                {
                                                    Value = GetNationalParentClassId(ni.NationalClassCode)
                                                }
                                            }
                                        },
                                        name = HierarchyNames.National
                                    }
                                },
                                traits = BuildItemTraits(ni)
                            }
                        }
                    }
                }).ToArray()
            };

            string xml = this.serializer.Serialize(itemMessage, new Utf8StringWriter());
            return xml;
        }

        private string GetBrandName(int? iconBrandId)
        {
            if (!iconBrandId.HasValue || iconBrandId < 0 || !iconCache.BrandDictionary.ContainsKey(iconBrandId.Value))
            {
                return string.Empty;
            }
            else
            {
                return iconCache.BrandDictionary[iconBrandId.Value].Name;
            }
        }

        private string GetBrandId(int? iconBrandId)
        {
            return (iconBrandId.HasValue && iconBrandId.Value > 0) ? iconBrandId.Value.ToString() : "0";
        }

        private string GetSubTeamId(string subTeamNumber)
        {
            if (string.IsNullOrWhiteSpace(subTeamNumber) || !iconCache.SubTeamModels.ContainsKey(subTeamNumber))
            {
                return NoSubTeamId;
            }
            else
            {
                return iconCache.SubTeamModels[subTeamNumber].FinancialHierarchyCode.ToString();
            }
        }

        private string GetSubTeamName(string subTeamNumber)
        {
            if(string.IsNullOrWhiteSpace(subTeamNumber) || !iconCache.SubTeamModels.ContainsKey(subTeamNumber))
            {
                return NoSubTeamName;
            }
            else
            {
                return iconCache.SubTeamModels[subTeamNumber].HierarchyClassName.ToString();
            }
        }

        private int GetNationalParentClassId(string key)
        {
            if (string.IsNullOrEmpty(key) || !iconCache.NationalClassModels.ContainsKey(key))
            {
                return 0;
            }
            else
            {
                return iconCache.NationalClassModels[key].HierarchyParentClassId;
            }
        }

        private string GetNationalClassName(string key)
        {
            if (string.IsNullOrEmpty(key) || !iconCache.NationalClassModels.ContainsKey(key))
            {
                return string.Empty;
            }
            else
            {
                return iconCache.NationalClassModels[key].Name;
            }
        }

        private string GetNationalClassId(string key)
        {
            if (string.IsNullOrEmpty(key) || !iconCache.NationalClassCodesToIdDictionary.ContainsKey(key))
            {
                return string.Empty;
            }
            else
            { 
                return iconCache.NationalClassCodesToIdDictionary[key].ToString();
            }
        }

        private string GetTaxName(string key)
        {
            if (string.IsNullOrEmpty(key) || !iconCache.TaxDictionary.ContainsKey(key))
            {
                return string.Empty;
            }
            else
            { 
                return iconCache.TaxDictionary[key].Name;
            }
        }

        private string GetScanCodeType(string scanCode)
        {
            if (IsScalePlu(scanCode))
            {
                return ScanCodeTypes.Descriptions.ScalePlu;
            }
            else if (scanCode.Length < 7)
            {
                return ScanCodeTypes.Descriptions.PosPlu;
            }
            else
            {
                return ScanCodeTypes.Descriptions.Upc;
            }
        }

        private static bool IsScalePlu(string scanCode)
        {
            return Regex.IsMatch(scanCode, CustomRegexPatterns.ScalePlu)
                || Regex.IsMatch(scanCode, CustomRegexPatterns.IngredientPlu46)
                || Regex.IsMatch(scanCode, CustomRegexPatterns.IngredientPlu48);
        }

        private Contracts.TraitType[] BuildItemTraits(NewItemModel newItemModel)
        {
            var itemTraits = new List<Contracts.TraitType>
            {
                new Contracts.TraitType
                {
                    code = TraitCodes.ProductDescription,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.ProductDescription,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = newItemModel.ItemDescription
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.PosDescription,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.PosDescription,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = GetPosDescription(newItemModel)
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.FoodStampEligible,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.FoodStampEligible,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = newItemModel.FoodStampEligible ? "1" : "0"
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.PackageUnit,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.PackageUnit,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = newItemModel.PackageUnit.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.RetailSize,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.RetailSize,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = newItemModel.RetailSize.ToString()
                            }
                        }
                    }
                },
                new Contracts.TraitType
                {
                    code = TraitCodes.RetailUom,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.RetailUom,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = newItemModel.RetailUom == null ? string.Empty : newItemModel.RetailUom,
                                uom = new Contracts.UomType
                                {
                                    code = uomMapper.GetEsbUomCode(newItemModel.RetailUom),
                                    codeSpecified = true,
                                    name = uomMapper.GetEsbUomDescription(newItemModel.RetailUom),
                                    nameSpecified = true
                                }
                            }
                        }
                    }
                }
            };

            if(settings.SendOrganic)
            {
                itemTraits.Add(new Contracts.TraitType
                {
                    code = TraitCodes.Organic,
                    type = new Contracts.TraitTypeType
                    {
                        description = TraitDescriptions.Organic,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = newItemModel.Organic.ToMessageBoolString()
                            }
                        }
                    }
                });
            }
            return itemTraits.ToArray();
        }

        private string GetPosDescription(NewItemModel newItemModel)
        {
            if (newItemModel.IconBrandId.HasValue && this.iconCache.BrandIdToAbbreviationDictionary.ContainsKey(newItemModel.IconBrandId.Value))
            {
                var brandAbbrAndPosDescription = this.iconCache.BrandIdToAbbreviationDictionary[newItemModel.IconBrandId.Value] + " " + newItemModel.PosDescription;
                brandAbbrAndPosDescription = brandAbbrAndPosDescription.Trim();

                return brandAbbrAndPosDescription.Length > LengthConstants.PosDescriptionMaxLength ? brandAbbrAndPosDescription.Substring(0, LengthConstants.PosDescriptionMaxLength) : brandAbbrAndPosDescription;
            }
            else
            {
                return newItemModel.PosDescription.Length > LengthConstants.PosDescriptionMaxLength ? newItemModel.PosDescription.Substring(0, LengthConstants.PosDescriptionMaxLength).Trim() : newItemModel.PosDescription.Trim();
            }
        }
    }
}
