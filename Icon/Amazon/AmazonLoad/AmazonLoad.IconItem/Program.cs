using AmazonLoad.Common;
using Dapper;
using Esb.Core.Mappers;
using Esb.Core.Serializer;
using Icon.Common;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Framework;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace AmazonLoad.IconItem
{
    class Program
    {
        public static IUomMapper uomMapper;
        public static Serializer<Contracts.items> serializer = new Serializer<Contracts.items>();
        public static ProductSelectionGroupsMapper productSelectionGroupsMapper;
        public static string saveMessagesDirectory = "Messages";

        static void Main(string[] args)
        {
            var startTime = DateTime.Now;
            Console.WriteLine($"[{startTime}] beginning...");

            var maxNumberOfRows = AppSettingsAccessor.GetIntSetting("MaxNumberOfRows", 0);
            var saveMessages = AppSettingsAccessor.GetBoolSetting("SaveMessages");
            var saveMessagesDirectory = AppSettingsAccessor.GetStringSetting("SaveMessagesDirectory");
            var nonReceivingSysName = AppSettingsAccessor.GetStringSetting("NonReceivingSysName");
            var sendToEsb = AppSettingsAccessor.GetBoolSetting("SendMessagesToEsb", false);
            
            Console.WriteLine("Flags:");
            Console.WriteLine($"  MaxNumberOfRows: {maxNumberOfRows}");
            Console.WriteLine($"  SaveMessages: {saveMessages}");
            Console.WriteLine($"  SaveMessagesDirectory: \"{saveMessagesDirectory}\"");
            Console.WriteLine($"  NonReceivingSysName: \"{nonReceivingSysName}\"");
            Console.WriteLine($"  SendMessagesToEsb: {sendToEsb}");
            if (!sendToEsb)
            {
                Console.WriteLine($"  \"SendMessagesToEsb\" flag is OFF: messages not actually sending to ESB queue!");
            }
            Console.WriteLine("");

            if (saveMessages)
            {
                if (!Directory.Exists(saveMessagesDirectory))
                {
                    Directory.CreateDirectory(saveMessagesDirectory);
                }
            }
            uomMapper = new UomMapper();
            productSelectionGroupsMapper = new ProductSelectionGroupsMapper();
            productSelectionGroupsMapper.LoadProductSelectionGroups();

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString))
            {
                var formattedSql = SqlQueries.ItemSql;
                if (maxNumberOfRows != 0)
                {
                    formattedSql = formattedSql.Replace("{top query}", $"top {maxNumberOfRows}");
                }
                else
                {
                    formattedSql = formattedSql.Replace("{top query}", "");
                }
                var models = sqlConnection.Query<ItemModel>(formattedSql, buffered: false, commandTimeout: 60);

                var producer = new EsbConnectionFactory
                {
                    Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("esb")
                }.CreateProducer();

                int numberOfRecordsSent = 0;
                int numberOfMessagesSent = 0;
                foreach (var modelSubSet in models.Batch(10000))
                {
                    foreach (var modelBatch in modelSubSet.Batch(100))
                    {
                        foreach (var modelGroup in modelBatch.GroupBy(i => i.ItemTypeCode))
                        {
                            string message = BuildMessage(modelGroup);
                            string messageId = Guid.NewGuid().ToString();

                            if (sendToEsb)
                            {
                                producer.Send(
                                message,
                                messageId,
                                new Dictionary<string, string>
                                {
                                { "IconMessageID", messageId },
                                { "Source", "Icon"},
                                { "nonReceivingSysName", AppSettingsAccessor.GetStringSetting("NonReceivingSysName") }
                                });
                            }
                            numberOfRecordsSent += modelGroup.Count();
                            numberOfMessagesSent += 1;
                            if (saveMessages)
                            {
                                File.WriteAllText($"{saveMessagesDirectory}/{messageId}.xml", message);
                            }
                        }
                    }
                }
                Console.WriteLine($"Number of records sent: {numberOfRecordsSent}.");
                Console.WriteLine($"Number of messages sent: {numberOfMessagesSent}.");
                var endTime = DateTime.Now;
                Console.WriteLine($"[{endTime}] ({(endTime - startTime):hh\\:mm\\:ss} elapsed)");
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }
        }

        private static string BuildMessage(IGrouping<object, ItemModel> itemGroup)
        {
            Contracts.items items = new Contracts.items
            {
                item = itemGroup.Select(i => ConvertToContractsItemType(i)).ToArray()
            };

            return serializer.Serialize(items);
        }

        private static Contracts.ItemType ConvertToContractsItemType(ItemModel i)
        {
            Contracts.ItemType miniBulkEntry = new Contracts.ItemType();
            if (i.DepartmentSale == "1")
            {
                miniBulkEntry = BuildDepartmentSaleItem(i);
            }
            else
            {
                miniBulkEntry = BuildProductMiniBulk(i);
            }

            return miniBulkEntry;
        }

        private static Contracts.ItemType BuildDepartmentSaleItem(ItemModel i)
        {
            var miniBulkEntry = new Contracts.ItemType
            {
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = i.ItemId,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = i.ItemTypeCode,
                        description = i.ItemTypeDesc
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
                                    id = i.ScanCodeId,
                                    code = i.ScanCode,
                                    typeId = i.ScanCodeTypeId,
                                    typeIdSpecified = true,
                                    typeDescription = i.ScanCodeTypeDesc
                                }
                            },
                            hierarchies = new Contracts.HierarchyType[]
                            {
                                BuildMerchandiseHierarchy(i),
                                BuildTaxHierarchy(i)
                            },
                            traits = new Contracts.TraitType[]
                            {
                                new Contracts.TraitType
                                {
                                    code = TraitCodes.DepartmentSale,
                                    type = new Contracts.TraitTypeType
                                    {
                                        description = TraitDescriptions.DepartmentSale,
                                        value = new Contracts.TraitValueType[]
                                        {
                                            new Contracts.TraitValueType
                                            {
                                                value = i.FinancialClassId
                                            }
                                        }
                                    }
                                }
                            },
                        }
                    }
                   }
            };

            return miniBulkEntry;
        }

        private static Contracts.ItemType BuildProductMiniBulk(ItemModel item)
        {
            var miniBulkEntry = new Contracts.ItemType
            {
                Action = Contracts.ActionEnum.AddOrUpdate,
                ActionSpecified = true,
                id = item.ItemId,
                @base = new Contracts.BaseItemType
                {
                    type = new Contracts.ItemTypeType
                    {
                        code = item.ItemTypeCode,
                        description = item.ItemTypeDesc
                    },
                    consumerInformation = BuildConsumerInformation(item)
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
                                BuildScanCodeType(item)
                            },
                            hierarchies = new Contracts.HierarchyType[]
                            {
                                BuildMerchandiseHierarchy(item),
                                BuildBrandHierarchy(item),
                                BuildTaxHierarchy(item),
                                BuildFinancialHierarchy(item),
                                BuildNationalHierarchy(item)
                            },
                            traits = BuildItemTraits(item),
                            selectionGroups = productSelectionGroupsMapper.GetProductSelectionGroups(item)
                        }
                    }
                }
            };
            return miniBulkEntry;
        }

        private static Contracts.TraitType[] BuildItemTraits(ItemModel item)
        {
            var itemTraits = new List<Contracts.TraitType>
            {
                BuildTrait(TraitCodes.ProductDescription, TraitDescriptions.ProductDescription, item.ProductDescription),
                BuildTrait(TraitCodes.PosDescription, TraitDescriptions.PosDescription, item.PosDescription),
                BuildTrait(TraitCodes.FoodStampEligible, TraitDescriptions.FoodStampEligible, item.FoodStampEligible),
                BuildTrait(TraitCodes.ProhibitDiscount, TraitDescriptions.ProhibitDiscount, item.ProhibitDiscount),
                BuildTrait(TraitCodes.GlobalPricingProgram, TraitDescriptions.GlobalPricingProgram, item.GlobalPricingProgram),
                BuildTrait(TraitCodes.FairTradeCertified, TraitDescriptions.FairTradeCertified, item.FairTradeCertified),
                BuildTrait(TraitCodes.FlexibleText, TraitDescriptions.FlexibleText, item.FlexibleText),
                BuildTrait(TraitCodes.MadeWithOrganicGrapes, TraitDescriptions.MadeWithOrganicGrapes, item.MadeWithOrganicGrapes),
                BuildTrait(TraitCodes.PrimeBeef, TraitDescriptions.PrimeBeef, item.PrimeBeef),
                BuildTrait(TraitCodes.RainforestAlliance, TraitDescriptions.RainforestAlliance, item.RainforestAlliance),
                BuildTrait(TraitCodes.Refrigerated, TraitDescriptions.Refrigerated, item.Refrigerated),
                BuildTrait(TraitCodes.SmithsonianBirdFriendly, TraitDescriptions.SmithsonianBirdFriendly, item.SmithsonianBirdFriendly),
                BuildTrait(TraitCodes.WicEligible, TraitDescriptions.WicEligible, item.WicEligible),
                BuildTrait(TraitCodes.ShelfLife, TraitDescriptions.ShelfLife, item.ShelfLife),
                BuildTrait(TraitCodes.SelfCheckoutItemTareGroup, TraitDescriptions.SelfCheckoutItemTareGroup, item.SelfCheckoutItemTareGroup),
                BuildTrait(TraitCodes.HiddenItem, TraitDescriptions.HiddenItem, item.Hidden),
                BuildTrait(TraitCodes.HiddenItem, TraitDescriptions.HiddenItem, item.Hidden),
                BuildTrait(TraitCodes.Line, TraitDescriptions.Line, item.Line),
                BuildTrait(TraitCodes.Sku, TraitDescriptions.Sku, item.SKU),
                BuildTrait(TraitCodes.PriceLine, TraitDescriptions.PriceLine, item.PriceLine),
                BuildTrait(TraitCodes.VariantSize, TraitDescriptions.VariantSize, item.VariantSize),
                BuildTrait(TraitCodes.EstoreNutritionRequired, TraitDescriptions.EstoreNutritionRequired, item.EStoreNutritionRequired),
                BuildTrait(TraitCodes.EstoreEligible, TraitDescriptions.EstoreEligible, item.EstoreEligible),
                BuildTraitBlankIfNull(TraitCodes.PrimeNowEligible, TraitDescriptions.PrimeNowEligible, item.PrimeNowEligible),
                BuildTraitBlankIfNull(TraitCodes.Tsf365Eligible, "365 Eligible", item.TSFEligible),
                BuildTraitBlankIfNull(TraitCodes.WfmEligilble, TraitDescriptions.WfmEligilble, item.WFMEligilble),
                BuildTraitBlankIfNull(TraitCodes.Other3pEligible, TraitDescriptions.Other3pEligible, item.Other3PEligible),
            };           

            if (ShouldSendPhysicalCharacteristicTraits(item))
            {
                var physicalCharacteristicTraits = BuildPhysicalTraits(item);
                itemTraits.AddRange(physicalCharacteristicTraits);
            }

            var signAttributeTraits = BuildSignAttributes(item);
            itemTraits.AddRange(signAttributeTraits);

            var nutritionTraits = BuildNutritionTraits(item);
            if (nutritionTraits != null)
            {
                itemTraits.AddRange(nutritionTraits);
            }

            return itemTraits.ToArray();
        }

        private static List<Contracts.TraitType> BuildNutritionTraits(ItemModel item)
        {
            if (item.Plu == null)
            {
                return null;
            }

            var nutritionTraits = new List<Contracts.TraitType>
            {
                BuildTrait(TraitCodes.RecipeName, TraitDescriptions.RecipeName, item.RecipeName),
                BuildTrait(TraitCodes.Allergens, TraitDescriptions.Allergens, item.Allergens),
                BuildTrait(TraitCodes.Ingredients, TraitDescriptions.Ingredients, item.Ingredients),
                BuildTrait(TraitCodes.Hsh, TraitDescriptions.Hsh, item.HshRating),
                BuildTrait(TraitCodes.PolyunsaturatedFat, TraitDescriptions.PolyunsaturatedFat, item.PolyunsaturatedFat),
                BuildTrait(TraitCodes.MonounsaturatedFat, TraitDescriptions.MonounsaturatedFat, item.MonounsaturatedFat),
                BuildTrait(TraitCodes.PotassiumWeight, TraitDescriptions.PotassiumWeight, item.PotassiumWeight),
                BuildTrait(TraitCodes.PotassiumPercent, TraitDescriptions.PotassiumPercent, item.PotassiumPercent),
                BuildTrait(TraitCodes.DietaryFiberPercent, TraitDescriptions.DietaryFiberPercent, item.DietaryFiberPercent),
                BuildTrait(TraitCodes.SolubleFiber, TraitDescriptions.SolubleFiber, item.SolubleFiber),
                BuildTrait(TraitCodes.InsolubleFiber, TraitDescriptions.InsolubleFiber, item.InsolubleFiber),
                BuildTrait(TraitCodes.SugarAlcohol, TraitDescriptions.SugarAlcohol, item.SugarAlcohol),
                BuildTrait(TraitCodes.OtherCarbohydrates, TraitDescriptions.OtherCarbohydrates, item.OtherCarbohydrates),
                BuildTrait(TraitCodes.ProteinPercent, TraitDescriptions.ProteinPercent, item.ProteinPercent),
                BuildTrait(TraitCodes.Betacarotene, TraitDescriptions.Betacarotene, item.Betacarotene),
                BuildTrait(TraitCodes.VitaminD, TraitDescriptions.VitaminD, item.VitaminD),
                BuildTrait(TraitCodes.VitaminE, TraitDescriptions.VitaminE, item.VitaminE),
                BuildTrait(TraitCodes.Thiamin, TraitDescriptions.Thiamin, item.Thiamin),
                BuildTrait(TraitCodes.Riboflavin, TraitDescriptions.Riboflavin, item.Riboflavin),
                BuildTrait(TraitCodes.Niacin, TraitDescriptions.Niacin, item.Niacin),
                BuildTrait(TraitCodes.VitaminB6, TraitDescriptions.VitaminB6, item.VitaminB6),
                BuildTrait(TraitCodes.Folate, TraitDescriptions.Folate, item.Folate),
                BuildTrait(TraitCodes.VitaminB12, TraitDescriptions.VitaminB12, item.VitaminB12),
                BuildTrait(TraitCodes.Biotin, TraitDescriptions.Biotin, item.Biotin),
                BuildTrait(TraitCodes.PantothenicAcid, TraitDescriptions.PantothenicAcid, item.PantothenicAcid),
                BuildTrait(TraitCodes.Phosphorous, TraitDescriptions.Phosphorous, item.Phosphorous),
                BuildTrait(TraitCodes.Iodine, TraitDescriptions.Iodine, item.Iodine),
                BuildTrait(TraitCodes.Magnesium, TraitDescriptions.Magnesium, item.Magnesium),
                BuildTrait(TraitCodes.Zinc, TraitDescriptions.Zinc, item.Zinc),
                BuildTrait(TraitCodes.Copper, TraitDescriptions.Copper, item.Copper),
                BuildTrait(TraitCodes.Transfat, TraitDescriptions.Transfat, item.Transfat),
                BuildTrait(TraitCodes.Om6Fatty, TraitDescriptions.Om6Fatty, item.Om6Fatty),
                BuildTrait(TraitCodes.Om3Fatty, TraitDescriptions.Om3Fatty, item.Om3Fatty),
                BuildTrait(TraitCodes.Starch, TraitDescriptions.Starch, item.Starch),
                BuildTrait(TraitCodes.Chloride, TraitDescriptions.Chloride, item.Chloride),
                BuildTrait(TraitCodes.Chromium, TraitDescriptions.Chromium, item.Chromium),
                BuildTrait(TraitCodes.VitaminK, TraitDescriptions.VitaminK, item.VitaminK),
                BuildTrait(TraitCodes.Manganese, TraitDescriptions.Manganese, item.Manganese),
                BuildTrait(TraitCodes.Molybdenum, TraitDescriptions.Molybdenum, item.Molybdenum),
                BuildTrait(TraitCodes.Selenium, TraitDescriptions.Selenium, item.Selenium),
                BuildTrait(TraitCodes.TransfatWeight, TraitDescriptions.TransfatWeight, item.TransfatWeight),
                BuildTrait(TraitCodes.CaloriesFromTransFat, TraitDescriptions.CaloriesFromTransFat, item.CaloriesFromTransfat),
                BuildTrait(TraitCodes.CaloriesSaturatedFat, TraitDescriptions.CaloriesSaturatedFat, item.CaloriesSaturatedFat),
                BuildTrait(TraitCodes.ServingPerContainer, TraitDescriptions.ServingPerContainer, item.ServingPerContainer),
                BuildTrait(TraitCodes.ServingSizeDesc, TraitDescriptions.ServingSizeDesc, item.ServingSizeDesc),
                BuildTrait(TraitCodes.ServingsPerPortion, TraitDescriptions.ServingsPerPortion, item.ServingsPerPortion),
                BuildTrait(TraitCodes.ServingUnits, TraitDescriptions.ServingUnits, item.ServingUnits),
                BuildTrait(TraitCodes.SizeWeight, TraitDescriptions.SizeWeight, item.SizeWeight),
            };

            return nutritionTraits;
        }

        private static Contracts.ConsumerInformationType BuildConsumerInformation(ItemModel item)
        {
            if (item.Plu == null)
            {
                return null;
            }

            var consumerInformation = new Contracts.ConsumerInformationType
            {
                stockItemConsumerProductLabel = new Contracts.StockProductLabelType
                {
                    consumerLabelTypeCode = null,
                    servingSizeUom = null,
                    servingSizeUomCount = Decimal.Zero,
                    servingsInRetailSaleUnitCount = null,
                    caloriesCount = item.Calories.ToDecimal(),
                    caloriesFromFatCount = item.CaloriesFat.ToDecimal(),
                    totalFatGramsAmount = item.TotalFatWeight.ToDecimal(),
                    totalFatDailyIntakePercent = item.TotalFatPercentage.ToDecimal(),
                    saturatedFatGramsAmount = item.SaturatedFatWeight.ToDecimal(),
                    saturatedFatPercent = item.SaturatedFatPercent.ToDecimal(),
                    cholesterolMilligramsCount = item.CholesterolWeight.ToDecimal(),
                    cholesterolPercent = item.CholesterolPercent.ToDecimal(),
                    sodiumMilligramsCount = item.SodiumWeight.ToDecimal(),
                    sodiumPercent = item.SodiumPercent.ToDecimal(),
                    totalCarbohydrateMilligramsCount = item.TotalCarbohydrateWeight.ToDecimal(),
                    totalCarbohydratePercent = item.TotalCarbohydratePercent.ToDecimal(),
                    dietaryFiberGramsCount = item.DietaryFiberWeight.ToDecimal(),
                    sugarsGramsCount = item.Sugar.ToDecimal(),
                    proteinGramsCount = item.ProteinWeight.ToDecimal(),
                    vitaminADailyMinimumPercent = item.VitaminA.ToDecimal(),
                    vitaminBDailyMinimumPercent = Decimal.Zero,
                    vitaminCDailyMinimumPercent = item.VitaminC.ToDecimal(),
                    calciumDailyMinimumPercent = item.Calcium.ToDecimal(),
                    ironDailyMinimumPercent = item.Iron.ToDecimal(),
                    nutritionalDescriptionText = null,
                    isHazardousMaterial = item.HazardousMaterialFlag == null ? false : (item.HazardousMaterialFlag.Value == 1 ? true : false),
                    hazardousMaterialTypeCode = null
                }
            };

            return consumerInformation;
        }

        private static List<Contracts.TraitType> BuildSignAttributes(ItemModel item)
        {
            return new List<Contracts.TraitType>
            {
                BuildTrait(TraitCodes.AnimalWelfareRating, TraitDescriptions.AnimalWelfareRating, item.AnimalWelfareRating),
                BuildTrait(TraitCodes.Biodynamic, TraitDescriptions.Biodynamic, item.Biodynamic),
                BuildTrait(TraitCodes.CheeseMilkType, TraitDescriptions.CheeseMilkType, item.CheeseMilkType),
                BuildTrait(TraitCodes.CheeseRaw, TraitDescriptions.CheeseRaw, item.CheeseRaw),
                BuildTrait(TraitCodes.EcoScaleRating, TraitDescriptions.EcoScaleRating, item.EcoScaleRating),
                BuildTrait(TraitCodes.GlutenFree, TraitDescriptions.GlutenFree, item.GlutenFreeAgency),
                BuildTrait(TraitCodes.HealthyEatingRating, TraitDescriptions.HealthyEatingRating, item.HealthyEatingRating),
                BuildTrait(TraitCodes.Kosher, TraitDescriptions.Kosher, item.KosherAgency),
                BuildTrait(TraitCodes.Msc, TraitDescriptions.Msc, item.Msc),
                BuildTrait(TraitCodes.NonGmo, TraitDescriptions.NonGmo, item.NonGmoAgency),
                BuildTrait(TraitCodes.Organic, TraitDescriptions.Organic, item.OrganicAgency),
                BuildTrait(TraitCodes.PremiumBodyCare, TraitDescriptions.PremiumBodyCare, item.PremiumBodyCare),
                BuildTrait(TraitCodes.FreshOrFrozen, TraitDescriptions.FreshOrFrozen, item.SeafoodFreshOrFrozen),
                BuildTrait(TraitCodes.SeafoodCatchType, TraitDescriptions.SeafoodCatchType, item.SeafoodCatchType),
                BuildTrait(TraitCodes.Vegan, TraitDescriptions.Vegan, item.VeganAgency),
                BuildTrait(TraitCodes.Vegetarian, TraitDescriptions.Vegetarian, item.Vegetarian),
                BuildTrait(TraitCodes.WholeTrade, TraitDescriptions.WholeTrade, item.WholeTrade),
                BuildTrait(TraitCodes.GrassFed, TraitDescriptions.GrassFed, item.GrassFed),
                BuildTrait(TraitCodes.PastureRaised, TraitDescriptions.PastureRaised, item.PastureRaised),
                BuildTrait(TraitCodes.FreeRange, TraitDescriptions.FreeRange, item.FreeRange),
                BuildTrait(TraitCodes.DryAged, TraitDescriptions.DryAged, item.DryAged),
                BuildTrait(TraitCodes.AirChilled, TraitDescriptions.AirChilled, item.AirChilled),
                BuildTrait(TraitCodes.MadeInHouse, TraitDescriptions.MadeInHouse, item.MadeInHouse),
                BuildTrait(TraitCodes.CustomerFriendlyDescription, TraitDescriptions.CustomerFriendlyDescription, item.CustomerFriendlyDescription),
                BuildTrait(TraitCodes.NutritionRequired, TraitDescriptions.NutritionRequired, item.NutritionRequired),
            };
        }

        private static List<Contracts.TraitType> BuildPhysicalTraits(ItemModel item)
        {
            var physicalCharacteristicTraits = new List<Contracts.TraitType>
            {
                BuildTrait(TraitCodes.PackageUnit, TraitDescriptions.PackageUnit, item.PackageUnit),
                BuildTrait(TraitCodes.RetailSize , TraitDescriptions.RetailSize , item.RetailSize ),
                BuildTrait(TraitCodes.RetailUom  , TraitDescriptions.RetailUom  , item.RetailUom, BuildUomType(item.RetailUom)),
            };
            return physicalCharacteristicTraits;
        }

        private static Contracts.TraitType BuildTrait(string traitCode, string traitDescription, string value, Contracts.UomType uom)
        {
            var trait = BuildTrait(traitCode, traitDescription, value);
            trait.type.value.FirstOrDefault().uom = uom;
            return trait;
        }

        private static Contracts.TraitType BuildTrait(string traitCode, string traitDescription, string value)
        {
            var trait = new Contracts.TraitType
            {
                code = traitCode,
                type = new Contracts.TraitTypeType
                {
                    description = traitDescription,
                    value = new Contracts.TraitValueType[]
                    {
                        new Contracts.TraitValueType
                        {
                            value = value ?? string.Empty,
                        }
                    }
                }
            };
            return trait;
        }

        private static Contracts.TraitType BuildTrait(string traitCode, string traitDescription, bool value)
        {
            return BuildTrait(traitCode, traitDescription, value ? "1" : "0");
        }

        private static Contracts.TraitType BuildTrait(string traitCode, string traitDescription, bool? value)
        {
            return BuildTrait(traitCode, traitDescription, value.GetValueOrDefault(false));
        }

        private static Contracts.TraitType BuildTraitBlankIfNull(string traitCode, string traitDescription, bool? value)
        {
            return BuildTrait(traitCode, traitDescription, value.HasValue 
                ? value.Value ? "1" : "0"
                : string.Empty);
        }

        private static Contracts.TraitType BuildTrait(string traitCode, string traitDescription, int? value)
        {
            return BuildTrait(traitCode, traitDescription, value?.ToString());
        }

        private static Contracts.TraitType BuildTrait(string traitCode, string traitDescription, double? value)
        {
            return BuildTrait(traitCode, traitDescription, value?.ToString());
        }

        private static Contracts.TraitType BuildTrait(string traitCode, string traitDescription, decimal? value)
        {
            return BuildTrait(traitCode, traitDescription, value?.ToString());
        }

        private static Contracts.UomType BuildUomType(string productMessageUom)
        {
            var uomType = new Contracts.UomType
            {
                code = uomMapper.GetEsbUomCode(productMessageUom),
                codeSpecified = true,
                name = uomMapper.GetEsbUomDescription(productMessageUom),
                nameSpecified = true
            };
            return uomType;
        }

        private static Contracts.ScanCodeType BuildScanCodeType(ItemModel item)
        {
            return new Contracts.ScanCodeType
            {
                id = item.ScanCodeId,
                code = item.ScanCode,
                typeId = item.ScanCodeTypeId,
                typeIdSpecified = true,
                typeDescription = item.ScanCodeTypeDesc
            };
        }

        private static Contracts.HierarchyType BuildMerchandiseHierarchy(ItemModel item)
        {
            return new Contracts.HierarchyType
            {
                id = Hierarchies.Merchandise,
                @class = new Contracts.HierarchyClassType[]
                {
                    new Contracts.HierarchyClassType
                    {
                        id = item.MerchandiseClassId.ToString(),
                        name = item.MerchandiseClassName,
                        level = item.MerchandiseLevel,
                        parentId = new Contracts.hierarchyParentClassType
                        {
                            Value = item.MerchandiseParentId.HasValue ? item.MerchandiseParentId.Value : default(int)
                        }
                    }
                },
                name = HierarchyNames.Merchandise
            };
        }

        private static Contracts.HierarchyType BuildBrandHierarchy(ItemModel item)
        {
            return new Contracts.HierarchyType
            {
                id = Hierarchies.Brands,
                @class = new Contracts.HierarchyClassType[]
                {
                    new Contracts.HierarchyClassType
                    {
                        id = item.BrandId.ToString(),
                        name = item.BrandName,
                        level = item.BrandLevel,
                        parentId = new Contracts.hierarchyParentClassType
                        {
                            Value = item.BrandParentId.HasValue ? item.BrandParentId.Value : default(int)
                        }
                    }
                },
                name = HierarchyNames.Brands
            };
        }

        private static Contracts.HierarchyType BuildTaxHierarchy(ItemModel item)
        {
            return new Contracts.HierarchyType
            {
                id = Hierarchies.Tax,
                @class = new Contracts.HierarchyClassType[]
                {
                    new Contracts.HierarchyClassType
                    {
                        id = item.TaxClassName.Split(' ')[0],
                        name = item.TaxClassName,
                        level = item.TaxLevel,
                        parentId = new Contracts.hierarchyParentClassType
                        {
                            Value = item.TaxParentId.HasValue ? item.TaxParentId.Value : default(int)
                        }
                    }
                },
                name = HierarchyNames.Tax
            };
        }

        private static Contracts.HierarchyType BuildFinancialHierarchy(ItemModel item)
        {
            return new Contracts.HierarchyType
            {
                id = Hierarchies.Financial,
                @class = new Contracts.HierarchyClassType[]
                {
                    new Contracts.HierarchyClassType
                    {
                        id = item.FinancialClassId,
                        name = item.FinancialClassName,
                        level = item.FinancialLevel,
                        parentId = new Contracts.hierarchyParentClassType
                        {
                            Value = item.FinancialParentId.HasValue ? item.FinancialParentId.Value : default(int)
                        }
                    }
                },
                name = HierarchyNames.Financial
            };
        }

        private static Contracts.HierarchyType BuildNationalHierarchy(ItemModel item)
        {
            return new Contracts.HierarchyType
            {
                id = Hierarchies.National,
                @class = new Contracts.HierarchyClassType[]
                {
                    new Contracts.HierarchyClassType
                    {
                        id = item.NationalClassId.ToString(),
                        name = item.NationalClassName,
                        level = item.NationalLevel,
                        parentId = new Contracts.hierarchyParentClassType
                        {
                            Value = item.NationalParentId.HasValue ? item.NationalParentId.Value : default(int)
                        }
                    }
                },
                name = HierarchyNames.National
            };
        }

        private static bool ShouldSendPhysicalCharacteristicTraits(ItemModel item)
        {
            // These three traits must all have values for them to be included in the message.
            // R10 will reject products with a 0 PackageUnit so we should not send products with a 0 package unit
            return (!string.IsNullOrWhiteSpace(item.PackageUnit) && item.PackageUnit != "0")
                && !string.IsNullOrWhiteSpace(item.RetailSize)
                && !string.IsNullOrWhiteSpace(item.RetailUom);
        }
    }
}