using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Infrastructure.Models.Mappers;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Icon.Services.ItemPublisher.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb
{
    /// <summary>
    /// EsbMessageBuilder creates ESB messages from a list of message queue items
    /// </summary>
    public class EsbMessageBuilder : IEsbMessageBuilder
    {
        private readonly ILogger<EsbMessageBuilder> logger;
        private readonly IEsbServiceCache esbServiceCache;
        private readonly ITraitMessageBuilder traitMessageBuilder;
        private readonly IHierarchyValueParser hierarchyNameParser;
        private readonly IValueFormatter valueFormatter;
        private readonly IUomMapper uomMapper;
        private readonly ServiceSettings serviceSettings;

        public EsbMessageBuilder(ILogger<EsbMessageBuilder> logger,
            IEsbServiceCache esbServiceCache,
            ITraitMessageBuilder traitMessageBuilder,
            IHierarchyValueParser hierarchyValuesParser,
            IValueFormatter valueFormatter,
            IUomMapper uomMapper,
            Services.ServiceSettings serviceSettings)
        {
            this.logger = logger;
            this.esbServiceCache = esbServiceCache;
            this.traitMessageBuilder = traitMessageBuilder;
            this.hierarchyNameParser = hierarchyValuesParser;
            this.valueFormatter = valueFormatter;
            this.uomMapper = uomMapper;
            this.serviceSettings = serviceSettings;
        }

        /// <summary>
        /// Determines if this class is ready to process queue records.
        /// </summary>
        public bool CacheLoaded
        {

            get
            {
                var cacheRefreshResults = this.RefreshCache().ContinueWith(t => t).Result;
                return this.esbServiceCache.CacheLoaded;
            }
        }

        public async Task RefreshCache()
        {
            await this.esbServiceCache.RefreshCache();
        }

        public async Task<BuildMessageResult> BuildItem(List<MessageQueueItemModel> models)
        {
            return await this.BuildMessageResult(models);
        }

        public async Task<BuildMessageResult> BuildMessageResult(List<MessageQueueItemModel> models)
        {
            List<string> errors = new List<string>();
            Action<string> processLogger = (string message) =>
            {
                errors.Add(message);
            };

            List<Contracts.ItemType> itemTypes = new List<Contracts.ItemType>();

            foreach (MessageQueueItemModel model in models)
            {
                try
                {
                    itemTypes.Add(await this.BuildItemType(model, processLogger));
                }
                catch (Exception ex)
                {
                    logger.Error("Item Id:" + model.Item.ItemId + " has following issue:" + ex.Message);
                }
            }

            var item = new Contracts.items
            {
                item = itemTypes.ToArray(),
            };

            return new BuildMessageResult(errors.Count == 0 ? true : false, item, errors);
        }

        public async Task<Contracts.ItemType> BuildItemType(MessageQueueItemModel message, Action<string> processLogger)
        {
            return new Contracts.ItemType
            {
                Action = ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = message.Item.ItemId,

                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = message.Item.ItemTypeCode,
                        description = message.Item.ItemTypeDescription
                    },
                    consumerInformation = await BuildConsumerInformation(message.Nutrition, processLogger),
                },
                locale = new Contracts.LocaleType[]
                {
                    new Contracts.LocaleType
                    {
                        id = Framework.Locales.WholeFoods.ToString(),
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
                                await BuildScanCodeType(message, processLogger)
                            },
                            hierarchies = (await this.BuildHierarchies(message.Hierarchy, processLogger)).ToArray(),
                            traits = (await this.BuildTraits(message.Item.ItemAttributes, message.Nutrition, processLogger)).ToArray(),
                            selectionGroups = await this.BuildProductSelectionGroupRootNode(message,processLogger),
                            isHospitalityItemSpecified = message.Item.IsHospitalityItemSpecified,
                            isHospitalityItem = message.Item.IsHospitalityItem,
                            isKitchenItemSpecified = message.Item.IsKitchenItemSpecified,
                            isKitchenItem = message.Item.IsKitchenItem,
                            imageUrl = message.Item.ImageUrl,
                            kitchenDescription = message.Item.KitchenDescription
                        },
                    }
                },
            };
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task<List<Contracts.HierarchyType>> BuildHierarchies(List<Hierarchy> hierarchies, Action<string> processLogger)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            List<Contracts.HierarchyType> response = new List<Contracts.HierarchyType>();
            foreach (Hierarchy hierarchy in hierarchies)
            {
                if (hierarchy.HierarchyName == Framework.Hierarchies.Names.Manufacturer && !this.serviceSettings.IncludeManufacturerHierarchy)
                {
                    continue;
                }

                string contractHierarchyClassId = this.hierarchyNameParser.ParseHierarchyClassIdForContract(hierarchy.HierarchyClassName, hierarchy.HierarchyClassId, hierarchy.HierarchyId);
                string contractHierarchyClassName = this.hierarchyNameParser.ParseHierarchyNameForContract(hierarchy.HierarchyClassName, hierarchy.HierarchyClassId, hierarchy.HierarchyId);

                Contracts.HierarchyType item = new Contracts.HierarchyType
                {
                    id = hierarchy.HierarchyId,
                    @class = new Contracts.HierarchyClassType[]
                    {
                        new Contracts.HierarchyClassType
                        {
                            id = contractHierarchyClassId,
                            name = contractHierarchyClassName,
                            level = hierarchy.HierarchyLevel ?? 0,
                            parentId = new Contracts.hierarchyParentClassType
                            {
                                Value = hierarchy.HierarchyClassParentId ?? 0
                            }
                        }
                    },
                    name = hierarchy.HierarchyName
                };

                response.Add(item);
            }

            return response;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task<Contracts.ConsumerInformationType> BuildConsumerInformation(Nutrition nutrition, Action<string> processLogger)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (nutrition == null)
            {
                return null;
            }

            return nutrition.IsDeleted
                ? new Contracts.ConsumerInformationType
                {
                    stockItemConsumerProductLabel = new Contracts.StockProductLabelType
                    {
                        Action = Contracts.ActionEnum.Delete,
                        ActionSpecified = true
                    }
                }
                : new Contracts.ConsumerInformationType
                {
                    stockItemConsumerProductLabel = new Contracts.StockProductLabelType
                    {
                        Action = Contracts.ActionEnum.AddOrUpdate,
                        // TODO: Include IsSpecified
                        ActionSpecified = true,
                        consumerLabelTypeCode = null,
                        servingSizeUom = null,
                        servingsInRetailSaleUnitCount = null,
                        nutritionalDescriptionText = null,
                        isHazardousMaterial = false,
                        hazardousMaterialTypeCode = null,
                        servingSizeUomCount = Decimal.Zero,
                        servingSizeUomCountSpecified = true,
                        caloriesCount = nutrition.Calories.ToDecimal(),
                        caloriesCountSpecified = true,
                        caloriesFromFatCount = nutrition.CaloriesFat.ToDecimal(),
                        caloriesFromFatCountSpecified = true,
                        totalFatGramsAmount = nutrition.TotalFatWeight.ToDecimal(),
                        totalFatGramsAmountSpecified = true,
                        totalFatDailyIntakePercent = nutrition.TotalFatPercentage.ToDecimal(),
                        totalFatDailyIntakePercentSpecified = true,
                        saturatedFatGramsAmount = nutrition.SaturatedFatWeight.ToDecimal(),
                        saturatedFatGramsAmountSpecified = true,
                        saturatedFatPercent = nutrition.SaturatedFatPercent.ToDecimal(),
                        saturatedFatPercentSpecified = true,
                        cholesterolMilligramsCount = nutrition.CholesterolWeight.ToDecimal(),
                        cholesterolMilligramsCountSpecified = true,
                        cholesterolPercent = nutrition.CholesterolPercent.ToDecimal(),
                        cholesterolPercentSpecified = true,
                        sodiumMilligramsCount = nutrition.SodiumWeight.ToDecimal(),
                        sodiumMilligramsCountSpecified = true,
                        sodiumPercent = nutrition.SodiumPercent.ToDecimal(),
                        sodiumPercentSpecified = true,
                        totalCarbohydrateMilligramsCount = nutrition.TotalCarbohydrateWeight.ToDecimal(),
                        totalCarbohydrateMilligramsCountSpecified = true,
                        totalCarbohydratePercent = nutrition.TotalCarbohydratePercent.ToDecimal(),
                        totalCarbohydratePercentSpecified = true,
                        dietaryFiberGramsCount = nutrition.DietaryFiberWeight.ToDecimal(),
                        dietaryFiberGramsCountSpecified = true,
                        sugarsGramsCount = nutrition.Sugar.ToDecimal(),
                        sugarsGramsCountSpecified = true,
                        proteinGramsCount = nutrition.ProteinWeight.ToDecimal(),
                        proteinGramsCountSpecified = true,
                        vitaminADailyMinimumPercent = nutrition.VitaminA.ToDecimal(),
                        vitaminADailyMinimumPercentSpecified = true,
                        vitaminBDailyMinimumPercent = Decimal.Zero,
                        vitaminBDailyMinimumPercentSpecified = true,
                        vitaminCDailyMinimumPercent = nutrition.VitaminC.ToDecimal(),
                        vitaminCDailyMinimumPercentSpecified = true,
                        calciumDailyMinimumPercent = nutrition.Calcium.ToDecimal(),
                        calciumDailyMinimumPercentSpecified = true,
                        ironDailyMinimumPercent = nutrition.Iron.ToDecimal(),
                        ironDailyMinimumPercentSpecified = true,
                        addedSugarsGramsCount = nutrition.AddedSugarsWeight.ToDecimal(),
                        addedSugarsGramsCountSpecified = nutrition.AddedSugarsWeight.HasValue,
                        addedSugarDailyPercent = nutrition.AddedSugarsPercent ?? 0,
                        addedSugarDailyPercentSpecified = nutrition.AddedSugarsPercent.HasValue,
                        calciumMilligramsCount = nutrition.CalciumWeight.ToDecimal(),
                        calciumMilligramsCountSpecified = nutrition.CalciumWeight.HasValue,
                        ironMilligramsCount = nutrition.IronWeight.ToDecimal(),
                        ironMilligramsCountSpecified = nutrition.IronWeight.HasValue,
                        vitaminDMicrogramsCount = nutrition.VitaminDWeight.ToDecimal(),
                        vitaminDMicrogramsCountSpecified = nutrition.VitaminDWeight.HasValue
                    }
                };
        }

    
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task<Contracts.ScanCodeType> BuildScanCodeType(MessageQueueItemModel message, Action<string> processLogger)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return new Contracts.ScanCodeType
            {
                id = message.Item.ScanCodeId,
                code = message.Item.ScanCode,
                typeId = message.Item.ScanCodeTypeId,
                typeIdSpecified = true,
                typeDescription = message.Item.ScanCodeTypeDesc
            };
        }

        public async Task<List<Contracts.TraitType>> BuildTraits(Dictionary<string, string> itemAttributes, Nutrition nutrition, Action<string> processLogger)
        {
            List<Contracts.TraitType> response = new List<Contracts.TraitType>();
            response.AddRange(await this.BuildTraitsFromAttributes(itemAttributes, processLogger));
            if (nutrition!= null && !nutrition.IsDeleted)
            {
                response.AddRange(await this.BuildTraitsFromNutrition(nutrition, processLogger));
            }
            return response;
        }

        public async Task<List<Contracts.TraitType>> BuildTraitsFromAttributes(Dictionary<string, string> itemAttributes, Action<string> processLogger)
        {
            List<Contracts.TraitType> response = new List<Contracts.TraitType>();

            foreach (KeyValuePair<string, string> attribute in itemAttributes)
            {
                Attributes cacheItem = await this.esbServiceCache.AttributeFromCache(attribute.Key);
                if (cacheItem == null)
                {
                    processLogger($"Trait {attribute.Key} was not found in the attribute cache and will not be sent in the ESB message");
                    continue;
                }
                else if (cacheItem.TraitCode == null)
                {
                    processLogger($"Trait {attribute.Key} has a null trait code and will not be sent in the ESB message");
                    continue;
                }

                string formattedAttributeValue = this.valueFormatter.FormatValueForMessage(cacheItem, attribute.Value);

                if (string.IsNullOrWhiteSpace(formattedAttributeValue))
                {
                    continue;
                }

                var traitType = new Contracts.TraitType
                {
                    code = cacheItem.TraitCode,
                    type = new Contracts.TraitTypeType
                    {
                        description = cacheItem.XmlTraitDescription,
                        value = new Contracts.TraitValueType[]
                        {
                            new Contracts.TraitValueType
                            {
                                value = formattedAttributeValue
                            }
                        }
                    }
                };

                // UOM is the only trait that requires the uom type otherwise R10 will not receive the message
                if (cacheItem.TraitCode == ItemPublisherConstants.TraitCodes.UomTraitCode)
                {
                    var uomType = new Contracts.UomType
                    {
                        code = this.uomMapper.GetEsbUomCode(formattedAttributeValue),
                        codeSpecified = true,
                        name = this.uomMapper.GetEsbUomDescription(formattedAttributeValue),
                        nameSpecified = true
                    };
                    traitType.type.value.FirstOrDefault().uom = uomType;
                }

                response.Add(traitType);
            }

            return response;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task<List<Contracts.TraitType>> BuildTraitsFromNutrition(Nutrition nutrition, Action<string> processLogger)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var response = new List<TraitType>();

            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.RecipeName, Icon.Framework.TraitDescriptions.RecipeName, nutrition.RecipeName));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Allergens, Icon.Framework.TraitDescriptions.Allergens, nutrition.Allergens));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Ingredients, Icon.Framework.TraitDescriptions.Ingredients, nutrition.Ingredients));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Hsh, Icon.Framework.TraitDescriptions.Hsh, nutrition.HshRating));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.PolyunsaturatedFat, Icon.Framework.TraitDescriptions.PolyunsaturatedFat, nutrition.PolyunsaturatedFat));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.MonounsaturatedFat, Icon.Framework.TraitDescriptions.MonounsaturatedFat, nutrition.MonounsaturatedFat));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.PotassiumWeight, Icon.Framework.TraitDescriptions.PotassiumWeight, nutrition.PotassiumWeight));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.PotassiumPercent, Icon.Framework.TraitDescriptions.PotassiumPercent, nutrition.PotassiumPercent));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.DietaryFiberPercent, Icon.Framework.TraitDescriptions.DietaryFiberPercent, nutrition.DietaryFiberPercent));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.SolubleFiber, Icon.Framework.TraitDescriptions.SolubleFiber, nutrition.SolubleFiber));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.InsolubleFiber, Icon.Framework.TraitDescriptions.InsolubleFiber, nutrition.InsolubleFiber));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.SugarAlcohol, Icon.Framework.TraitDescriptions.SugarAlcohol, nutrition.SugarAlcohol));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.OtherCarbohydrates, Icon.Framework.TraitDescriptions.OtherCarbohydrates, nutrition.OtherCarbohydrates));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.ProteinPercent, Icon.Framework.TraitDescriptions.ProteinPercent, nutrition.ProteinPercent));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Betacarotene, Icon.Framework.TraitDescriptions.Betacarotene, nutrition.Betacarotene));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.VitaminD, Icon.Framework.TraitDescriptions.VitaminD, nutrition.VitaminD));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.VitaminE, Icon.Framework.TraitDescriptions.VitaminE, nutrition.VitaminE));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Thiamin, Icon.Framework.TraitDescriptions.Thiamin, nutrition.Thiamin));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Riboflavin, Icon.Framework.TraitDescriptions.Riboflavin, nutrition.Riboflavin));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Niacin, Icon.Framework.TraitDescriptions.Niacin, nutrition.Niacin));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.VitaminB6, Icon.Framework.TraitDescriptions.VitaminB6, nutrition.VitaminB6));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Folate, Icon.Framework.TraitDescriptions.Folate, nutrition.Folate));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.VitaminB12, Icon.Framework.TraitDescriptions.VitaminB12, nutrition.VitaminB12));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Biotin, Icon.Framework.TraitDescriptions.Biotin, nutrition.Biotin));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.PantothenicAcid, Icon.Framework.TraitDescriptions.PantothenicAcid, nutrition.PantothenicAcid));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Phosphorous, Icon.Framework.TraitDescriptions.Phosphorous, nutrition.Phosphorous));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Iodine, Icon.Framework.TraitDescriptions.Iodine, nutrition.Iodine));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Magnesium, Icon.Framework.TraitDescriptions.Magnesium, nutrition.Magnesium));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Zinc, Icon.Framework.TraitDescriptions.Zinc, nutrition.Zinc));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Copper, Icon.Framework.TraitDescriptions.Copper, nutrition.Copper));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Transfat, Icon.Framework.TraitDescriptions.Transfat, nutrition.Transfat));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Om6Fatty, Icon.Framework.TraitDescriptions.Om6Fatty, nutrition.Om6Fatty));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Om3Fatty, Icon.Framework.TraitDescriptions.Om3Fatty, nutrition.Om3Fatty));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Starch, Icon.Framework.TraitDescriptions.Starch, nutrition.Starch));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Chloride, Icon.Framework.TraitDescriptions.Chloride, nutrition.Chloride));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Chromium, Icon.Framework.TraitDescriptions.Chromium, nutrition.Chromium));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.VitaminK, Icon.Framework.TraitDescriptions.VitaminK, nutrition.VitaminK));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Manganese, Icon.Framework.TraitDescriptions.Manganese, nutrition.Manganese));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Molybdenum, Icon.Framework.TraitDescriptions.Molybdenum, nutrition.Molybdenum));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.Selenium, Icon.Framework.TraitDescriptions.Selenium, nutrition.Selenium));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.TransfatWeight, Icon.Framework.TraitDescriptions.TransfatWeight, nutrition.TransfatWeight));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.CaloriesFromTransFat, Icon.Framework.TraitDescriptions.CaloriesFromTransFat, nutrition.CaloriesFromTransfat));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.CaloriesSaturatedFat, Icon.Framework.TraitDescriptions.CaloriesSaturatedFat, nutrition.CaloriesSaturatedFat));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.ServingPerContainer, Icon.Framework.TraitDescriptions.ServingPerContainer, nutrition.ServingPerContainer));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.ServingSizeDesc, Icon.Framework.TraitDescriptions.ServingSizeDesc, nutrition.ServingSizeDesc));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.ServingsPerPortion, Icon.Framework.TraitDescriptions.ServingsPerPortion, nutrition.ServingsPerPortion));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.ServingUnits, Icon.Framework.TraitDescriptions.ServingUnits, nutrition.ServingUnits));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.SizeWeight, Icon.Framework.TraitDescriptions.SizeWeight, nutrition.SizeWeight));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.SizeWeight, Icon.Framework.TraitDescriptions.SizeWeight, nutrition.SizeWeight));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.ProfitCenter, Icon.Framework.TraitDescriptions.ProfitCenter, nutrition.ProfitCenter));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.CanadaAllergens, Icon.Framework.TraitDescriptions.CanadaAllergens, nutrition.CanadaAllergens));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.CanadaIngredients, Icon.Framework.TraitDescriptions.CanadaIngredients, nutrition.CanadaIngredients));
            response.Add(this.traitMessageBuilder.BuildTrait(Icon.Framework.TraitCodes.CanadaSugarPercentage, Icon.Framework.TraitDescriptions.CanadaSugarPercentage, nutrition.CanadaSugarPercent));

            return response;
        }

        /// <summary>
        /// Returns the root node for product selection groups
        /// </summary>
        /// <param name="message"></param>
        /// <param name="processLogger"></param>
        /// <returns></returns>
        public async Task<Contracts.SelectionGroupsType> BuildProductSelectionGroupRootNode(MessageQueueItemModel message, Action<string> processLogger)
        {
            return new Contracts.SelectionGroupsType()
            {
                group = (await this.BuildProductSelectionGroups(message, processLogger)).ToArray()
            };
        }

        /// <summary>
        /// Loops through each product selection group that has an association to a MerchandiseHierarchyClassId or an AttributeId
        /// (It excludes any PSG that has only a Trait association since these would not be related to global item).
        /// We send AddOrUpdate if the value of the property on item matches the attribute value in the ProductSelectionGroup AttributeValue column.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="processLogger"></param>
        /// <returns></returns>
        public async Task<List<GroupTypeType>> BuildProductSelectionGroups(MessageQueueItemModel message, Action<string> processLogger)
        {
            List<Contracts.GroupTypeType> groups = new List<GroupTypeType>();
            var psgGroups = this.esbServiceCache.ProductSelectionGroupCache
                .Values
                .Where(c => c.AttributeId.HasValue || c.MerchandiseHierarchyClassId.HasValue)
                .GroupBy(p => p.ProductSelectionGroupName);

            foreach (var psg in psgGroups)
            {
                bool isAddOrUpdate = false;

                if (psg.Any(p => p.AttributeId.HasValue && !string.IsNullOrWhiteSpace(p.AttributeValue)))
                {
                    string attributeValue;
                    isAddOrUpdate = (from p in psg
                                     where
                                         message.Item.ItemAttributes.TryGetValue(p.AttributeName, out attributeValue)
                                         && (attributeValue == p.AttributeValue)
                                     select p).Any();
                }
                else if (psg.Any(p => p.MerchandiseHierarchyClassId.HasValue))
                {
                    int? merchSubBrickId = message.Hierarchy.FirstOrDefault(h => h.HierarchyName == Icon.Framework.HierarchyNames.Merchandise)?.HierarchyClassId;
                    if (merchSubBrickId.HasValue)
                    {
                        isAddOrUpdate = psg.Any(p => p.MerchandiseHierarchyClassId.Value == merchSubBrickId.Value);
                    }
                }
                else
                {
                    isAddOrUpdate = false;
                }

                groups.Add(await this.CreateProductSelectionGroupElement(group: psg.First(), isAddOrUpdate: isAddOrUpdate, processLogger: processLogger));
            }

            return groups;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task<GroupTypeType> CreateProductSelectionGroupElement(ProductSelectionGroup group, bool isAddOrUpdate, Action<string> processLogger)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return new Contracts.GroupTypeType
            {
                Action = isAddOrUpdate ? Contracts.ActionEnum.AddOrUpdate : Contracts.ActionEnum.Delete,
                ActionSpecified = true,
                id = group.ProductSelectionGroupName,
                name = group.ProductSelectionGroupName,
                type = group.ProductSelectionGroupTypeName
            };
        }
    }
}