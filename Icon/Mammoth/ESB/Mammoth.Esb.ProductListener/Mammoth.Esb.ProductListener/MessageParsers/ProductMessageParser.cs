using Icon.Esb.MessageParsers;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Mammoth.Common.DataAccess;
using Mammoth.Esb.ProductListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Mammoth.Esb.ProductListener.MessageParsers
{
    public class ProductMessageParser : MessageParserBase<Contracts.items, List<ItemModel>>
    {
        public override List<ItemModel> ParseMessage(IEsbMessage message)
        {
            List<ItemModel> itemModelCollection = new List<ItemModel>();
            Contracts.items items = DeserializeMessage(message);

            if (items.item.First().locale.First().Item.GetType() != typeof(Contracts.EnterpriseItemAttributesType))
            {
                return null;
            }

            foreach (var item in items.item)
            {
                var enterpriseItemAttributes = item.locale.First().Item as Contracts.EnterpriseItemAttributesType;
                
                var traits = enterpriseItemAttributes.traits;
                var hierarchyClasses = enterpriseItemAttributes.hierarchies;
                var consumerInformation = item.@base.consumerInformation;

                var itemModel = new ItemModel();

                // Global Product Attributes
                itemModel.GlobalAttributes = new GlobalAttributesModel();
                itemModel.GlobalAttributes.ItemID = item.id;
                itemModel.GlobalAttributes.ItemTypeID = GetItemTypeId(item);
                itemModel.GlobalAttributes.ScanCode = enterpriseItemAttributes.scanCodes.First().code;
                itemModel.GlobalAttributes.Desc_Product = GetTraitValue(TraitCodes.ProductDescription, traits);
                itemModel.GlobalAttributes.Desc_POS = GetTraitValue(TraitCodes.PosDescription, traits);
                itemModel.GlobalAttributes.PackageUnit = GetTraitValue(TraitCodes.PackageUnit, traits);
                itemModel.GlobalAttributes.RetailSize = GetTraitValue(TraitCodes.RetailSize, traits);
                itemModel.GlobalAttributes.RetailUOM = GetTraitValue(TraitCodes.RetailUom, traits);
                itemModel.GlobalAttributes.FoodStampEligible = GetBoolTraitValue(TraitCodes.FoodStampEligible, traits);
                itemModel.GlobalAttributes.SubBrickID = GetHierarchyClassIntId(HierarchyNames.Merchandise, hierarchyClasses);
                itemModel.GlobalAttributes.NationalClassID = GetHierarchyClassIntId(HierarchyNames.National, hierarchyClasses);
                itemModel.GlobalAttributes.BrandHCID = GetHierarchyClassIntId(HierarchyNames.Brands, hierarchyClasses);
                itemModel.GlobalAttributes.MessageTaxClassHCID = GetHierarchyClassStringId(HierarchyNames.Tax, hierarchyClasses);
                itemModel.GlobalAttributes.PSNumber = GetHierarchyClassIntId(HierarchyNames.Financial, hierarchyClasses);
                itemModel.GlobalAttributes.Desc_CustomerFriendly = GetTraitValue(Attributes.Codes.CustomerFriendlyDescription, traits);

                // Global Sign Attributes
                itemModel.SignAttributes = ParseSignAttributes(item, traits);

                // Nutrition
                itemModel.NutritionAttributes = ParseNutritionInformation(item, traits);

                // Extended Attributes
                itemModel.ExtendedAttributes = ParseExtendedAttributes(item, traits);

                itemModelCollection.Add(itemModel);
            }

            return itemModelCollection;
        }

        private SignAttributesModel ParseSignAttributes(Contracts.ItemType item, Contracts.TraitType[] traits)
        {
            var signAttributes = new SignAttributesModel();
            signAttributes.ItemID = item.id;
            signAttributes.IsAirChilled = GetBoolTraitValue(TraitCodes.AirChilled, traits);
            signAttributes.Rating_AnimalWelfare = GetTraitValue(TraitCodes.AnimalWelfareRating, traits);
            signAttributes.IsBiodynamic = GetBoolTraitValue(TraitCodes.Biodynamic, traits);
            signAttributes.CheeseMilkType = GetTraitValue(TraitCodes.CheeseMilkType, traits);
            signAttributes.IsCheeseRaw = GetBoolTraitValue(TraitCodes.CheeseRaw, traits);
            signAttributes.IsDryAged = GetBoolTraitValue(TraitCodes.DryAged, traits);
            signAttributes.Rating_EcoScale = GetTraitValue(TraitCodes.EcoScaleRating, traits);
            signAttributes.IsFreeRange = GetBoolTraitValue(TraitCodes.FreeRange, traits);
            signAttributes.Agency_GlutenFree = GetTraitValue(TraitCodes.GlutenFree, traits);
            signAttributes.IsGrassFed = GetBoolTraitValue(TraitCodes.GrassFed, traits);
            signAttributes.Rating_HealthyEating = GetTraitValue(TraitCodes.HealthyEatingRating, traits);
            signAttributes.Agency_Kosher = GetTraitValue(TraitCodes.Kosher, traits);
            signAttributes.IsMadeInHouse = GetBoolTraitValue(TraitCodes.MadeInHouse, traits);
            signAttributes.IsMsc = GetBoolTraitValue(TraitCodes.Msc, traits);
            signAttributes.Agency_NonGMO = GetTraitValue(TraitCodes.NonGmo, traits);
            signAttributes.Agency_Organic = GetTraitValue(TraitCodes.Organic, traits);
            signAttributes.IsPastureRaised = GetBoolTraitValue(TraitCodes.PastureRaised, traits);
            signAttributes.IsPremiumBodyCare = GetBoolTraitValue(TraitCodes.PremiumBodyCare, traits);
            signAttributes.Seafood_CatchType = GetTraitValue(TraitCodes.SeafoodCatchType, traits);
            signAttributes.Seafood_FreshOrFrozen = GetTraitValue(TraitCodes.FreshOrFrozen, traits);
            signAttributes.Agency_Vegan = GetTraitValue(TraitCodes.Vegan, traits);
            signAttributes.IsVegetarian = GetBoolTraitValue(TraitCodes.Vegetarian, traits);
            signAttributes.IsWholeTrade = GetBoolTraitValue(TraitCodes.WholeTrade, traits);
            return signAttributes;
        }

        private NutritionAttributesModel ParseNutritionInformation(Contracts.ItemType item, Contracts.TraitType[] traits)
        {
            if (item.@base.consumerInformation == null)
            {
                return null;
            }
            else
            {
                var stockItemConsumerProductLabel = item.@base.consumerInformation.stockItemConsumerProductLabel;

                var nutritionAttributes = new NutritionAttributesModel();
                nutritionAttributes.ItemID = item.id;
                nutritionAttributes.RecipeName = GetTraitValue(TraitCodes.RecipeName, traits);
                nutritionAttributes.Allergens = GetTraitValue(TraitCodes.Allergens, traits);
                nutritionAttributes.Ingredients = GetTraitValue(TraitCodes.Ingredients, traits);
                nutritionAttributes.ServingsPerPortion = GetFloatTraitValue(TraitCodes.ServingsPerPortion, traits);
                nutritionAttributes.ServingSizeDesc = GetTraitValue(TraitCodes.ServingSizeDesc, traits);
                nutritionAttributes.ServingPerContainer = GetTraitValue(TraitCodes.ServingPerContainer, traits);
                nutritionAttributes.HshRating = GetIntTraitValue(TraitCodes.Hsh, traits);
                nutritionAttributes.ServingUnits = GetIntTraitValue(TraitCodes.ServingUnits, traits);
                nutritionAttributes.SizeWeight = GetIntTraitValue(TraitCodes.SizeWeight, traits);
                nutritionAttributes.Calories = (int)stockItemConsumerProductLabel.caloriesCount;
                nutritionAttributes.CaloriesFat = (int)stockItemConsumerProductLabel.caloriesFromFatCount;
                nutritionAttributes.CaloriesSaturatedFat = GetIntTraitValue(TraitCodes.CaloriesSaturatedFat, traits);
                nutritionAttributes.TotalFatWeight = (int)stockItemConsumerProductLabel.totalFatGramsAmount;
                nutritionAttributes.TotalFatPercentage = (int)stockItemConsumerProductLabel.totalFatDailyIntakePercent;
                nutritionAttributes.SaturatedFatWeight = (int)stockItemConsumerProductLabel.saturatedFatGramsAmount;
                nutritionAttributes.SaturatedFatPercent = (int)stockItemConsumerProductLabel.saturatedFatPercent;
                nutritionAttributes.PolyunsaturatedFat = GetDecimalTraitValue(TraitCodes.PolyunsaturatedFat, traits);
                nutritionAttributes.MonounsaturatedFat = GetDecimalTraitValue(TraitCodes.MonounsaturatedFat, traits);
                nutritionAttributes.CholesterolWeight = (int)stockItemConsumerProductLabel.cholesterolMilligramsCount;
                nutritionAttributes.CholesterolPercent = (int)stockItemConsumerProductLabel.cholesterolPercent;
                nutritionAttributes.SodiumWeight = (int)stockItemConsumerProductLabel.sodiumMilligramsCount;
                nutritionAttributes.SodiumPercent = (int)stockItemConsumerProductLabel.sodiumPercent;
                nutritionAttributes.PotassiumWeight = GetDecimalTraitValue(TraitCodes.PotassiumWeight, traits);
                nutritionAttributes.PotassiumPercent = GetIntTraitValue(TraitCodes.PotassiumPercent, traits);
                nutritionAttributes.TotalCarbohydrateWeight = (int)stockItemConsumerProductLabel.totalCarbohydrateMilligramsCount;
                nutritionAttributes.TotalCarbohydratePercent = (int)stockItemConsumerProductLabel.totalCarbohydratePercent;
                nutritionAttributes.DietaryFiberWeight = stockItemConsumerProductLabel.dietaryFiberGramsCount;
                nutritionAttributes.DietaryFiberPercent = GetIntTraitValue(TraitCodes.DietaryFiberPercent, traits);
                nutritionAttributes.SolubleFiber = GetDecimalTraitValue(TraitCodes.SolubleFiber, traits);
                nutritionAttributes.InsolubleFiber = GetDecimalTraitValue(TraitCodes.InsolubleFiber, traits);
                nutritionAttributes.Sugar = stockItemConsumerProductLabel.sugarsGramsCount;
                nutritionAttributes.SugarAlcohol = GetDecimalTraitValue(TraitCodes.SugarAlcohol, traits);
                nutritionAttributes.OtherCarbohydrates = GetDecimalTraitValue(TraitCodes.OtherCarbohydrates, traits);
                nutritionAttributes.ProteinWeight = (int)stockItemConsumerProductLabel.proteinGramsCount;
                nutritionAttributes.ProteinPercent = GetIntTraitValue(TraitCodes.ProteinPercent, traits);
                nutritionAttributes.VitaminA = (int)stockItemConsumerProductLabel.vitaminADailyMinimumPercent;
                nutritionAttributes.Betacarotene = GetIntTraitValue(TraitCodes.Betacarotene, traits);
                nutritionAttributes.VitaminC = (int)stockItemConsumerProductLabel.vitaminCDailyMinimumPercent;
                nutritionAttributes.Calcium = (int)stockItemConsumerProductLabel.calciumDailyMinimumPercent;
                nutritionAttributes.Iron = (int)stockItemConsumerProductLabel.ironDailyMinimumPercent;
                nutritionAttributes.VitaminD = GetIntTraitValue(TraitCodes.VitaminD, traits);
                nutritionAttributes.VitaminE = GetIntTraitValue(TraitCodes.VitaminE, traits);
                nutritionAttributes.Thiamin = GetIntTraitValue(TraitCodes.Thiamin, traits);
                nutritionAttributes.Riboflavin = GetIntTraitValue(TraitCodes.Riboflavin, traits);
                nutritionAttributes.Niacin = GetIntTraitValue(TraitCodes.Niacin, traits);
                nutritionAttributes.VitaminB6 = GetIntTraitValue(TraitCodes.VitaminB6, traits);
                nutritionAttributes.VitaminB12 = GetIntTraitValue(TraitCodes.VitaminB12, traits);
                nutritionAttributes.Folate = GetIntTraitValue(TraitCodes.Folate, traits);
                nutritionAttributes.Biotin = GetIntTraitValue(TraitCodes.Biotin, traits);
                nutritionAttributes.PantothenicAcid = GetIntTraitValue(TraitCodes.PantothenicAcid, traits);
                nutritionAttributes.Phosphorous = GetIntTraitValue(TraitCodes.Phosphorous, traits);
                nutritionAttributes.Iodine = GetIntTraitValue(TraitCodes.Iodine, traits);
                nutritionAttributes.Magnesium = GetIntTraitValue(TraitCodes.Magnesium, traits);
                nutritionAttributes.Zinc = GetIntTraitValue(TraitCodes.Zinc, traits);
                nutritionAttributes.Copper = GetIntTraitValue(TraitCodes.Copper, traits);
                nutritionAttributes.TransFat = (int)GetFloatTraitValue(TraitCodes.Transfat, traits);
                nutritionAttributes.TransFatWeight = (int)GetFloatTraitValue(TraitCodes.TransfatWeight, traits);
                nutritionAttributes.CaloriesFromTransFat = GetIntTraitValue(TraitCodes.CaloriesFromTransFat, traits);
                nutritionAttributes.Om3Fatty = (int)GetFloatTraitValue(TraitCodes.Om3Fatty, traits);
                nutritionAttributes.Om6Fatty = (int)GetFloatTraitValue(TraitCodes.Om6Fatty, traits);
                nutritionAttributes.Starch = (int)GetFloatTraitValue(TraitCodes.Starch, traits);
                nutritionAttributes.Chloride = GetIntTraitValue(TraitCodes.Chloride, traits);
                nutritionAttributes.Chromium = GetIntTraitValue(TraitCodes.Chromium, traits);
                nutritionAttributes.VitaminK = GetIntTraitValue(TraitCodes.VitaminK, traits);
                nutritionAttributes.Manganese = GetIntTraitValue(TraitCodes.Manganese, traits);
                nutritionAttributes.Molybdenum = GetIntTraitValue(TraitCodes.Molybdenum, traits);
                nutritionAttributes.Selenium = GetIntTraitValue(TraitCodes.Selenium, traits);
                return nutritionAttributes;
            }
        }

        private ExtendedAttributesModel ParseExtendedAttributes(Contracts.ItemType item, TraitType[] traits)
        {
            var extendedAttributes = new ExtendedAttributesModel();
            extendedAttributes.ItemId = item.id;
            extendedAttributes.FairTrade = GetTraitValue(Attributes.Codes.FairTrade, traits);
            extendedAttributes.FlexibleText = GetTraitValue(Attributes.Codes.FlexibleText, traits);
            extendedAttributes.GlobalPricingProgram = GetTraitValue(Attributes.Codes.GlobalPricingProgram, traits);
            extendedAttributes.MadeWithBiodynamicGrapes = GetTraitValue(Attributes.Codes.MadeWithBiodynamicGrapes, traits);
            extendedAttributes.MadeWithOrganicGrapes = GetTraitValue(Attributes.Codes.MadeWithOrganicGrapes, traits);
            extendedAttributes.NutritionRequired = GetTraitValue(Attributes.Codes.NutritionRequired, traits);
            extendedAttributes.PrimeBeef = GetTraitValue(Attributes.Codes.PrimeBeef, traits);
            extendedAttributes.RainforestAlliance = GetTraitValue(Attributes.Codes.RainforestAlliance, traits);
            extendedAttributes.RefrigeratedOrShelfStable = GetTraitValue(Attributes.Codes.RefrigeratedOrShelfStable, traits);
            extendedAttributes.SmithsonianBirdFriendly = GetTraitValue(Attributes.Codes.SmithsonianBirdFriendly, traits);
            extendedAttributes.Wic = GetTraitValue(Attributes.Codes.Wic, traits);
            return extendedAttributes;
        }

        private int GetItemTypeId(Contracts.ItemType item)
        {
            switch(item.@base.type.code)
            {
                case ItemTypeCodes.RetailSale:
                    return Mammoth.Common.DataAccess.ItemTypes.RetailSale;
                case ItemTypeCodes.Coupon:
                    return Mammoth.Common.DataAccess.ItemTypes.Coupon;
                case ItemTypeCodes.Deposit:
                    return Mammoth.Common.DataAccess.ItemTypes.Deposit;
                case ItemTypeCodes.Fee:
                    return Mammoth.Common.DataAccess.ItemTypes.Fee;
                case ItemTypeCodes.NonRetail:
                    return Mammoth.Common.DataAccess.ItemTypes.NonRetail;
                case ItemTypeCodes.Return:
                    return Mammoth.Common.DataAccess.ItemTypes.Return;
                case ItemTypeCodes.Tare:
                    return Mammoth.Common.DataAccess.ItemTypes.Tare;
                default: throw new ArgumentException(string.Format("Item type {0} is not supported.", item.@base.type.code));
            }
        }

        private bool? GetBoolTraitValue(string traitCode, Contracts.TraitType[] traits)
        {
            string traitValue = GetTraitValue(traitCode, traits);
            if (String.IsNullOrEmpty(traitValue))
            {
                return null;
            }
            else if(traitValue == "0")
            {
                return false;
            }
            else if(traitValue == "1")
            {
                return true;
            }
            else
            {
                throw new ArgumentException(string.Format("'{0}' is not a supported boolean value for trait code '{1}'", traitValue, traitCode));
            }
        }

        private float? GetFloatTraitValue(string traitCode, Contracts.TraitType[] traits)
        {
            string traitValue = GetTraitValue(traitCode, traits);
            float value;
            if (String.IsNullOrEmpty(traitValue))
            {
                return null;
            }
            else if (float.TryParse(traitValue, out value))
            {
                return value;
            }
            else
            {
                throw new ArgumentException(string.Format("'{0}' is not a supported float value for trait code '{1}'", traitValue, traitCode));
            }
        }

        private int? GetIntTraitValue(string traitCode, Contracts.TraitType[] traits)
        {
            string traitValue = GetTraitValue(traitCode, traits);
            int value;
            if (String.IsNullOrEmpty(traitValue))
            {
                return null;
            }
            else if (int.TryParse(traitValue, out value))
            {
                return value;
            }
            else
            {
                throw new ArgumentException(string.Format("'{0}' is not a supported int value for trait code '{1}'", traitValue, traitCode));
            }
        }

        private decimal? GetDecimalTraitValue(string traitCode, Contracts.TraitType[] traits)
        {
            string traitValue = GetTraitValue(traitCode, traits);
            decimal value;
            if (String.IsNullOrEmpty(traitValue))
            {
                return null;
            }
            else if (decimal.TryParse(traitValue, out value))
            {
                return value;
            }
            else
            {
                throw new ArgumentException(string.Format("'{0}' is not a supported decimal value for trait code '{1}'", traitValue, traitCode));
            }
        }

        private int? GetHierarchyClassIntId(string hierarchyName, Contracts.HierarchyType[] hierarchyClasses)
        {
            var hierarchy = hierarchyClasses.FirstOrDefault(h => h.name == hierarchyName);
            if(hierarchy != null)
            {
                return int.Parse(hierarchy.@class.First().id);
            }

            return null;
        }

        private string GetHierarchyClassStringId(string hierarchyName, Contracts.HierarchyType[] hierarchyClasses)
        {
            var hierarchy = hierarchyClasses.FirstOrDefault(h => h.name == hierarchyName);
            if (hierarchy != null)
            {
                return hierarchy.@class.First().id;
            }

            return null;
        }

        private string GetTraitValue(string traitCode, Contracts.TraitType[] traits)
        {
            var trait = traits.FirstOrDefault(t => t.code == traitCode);

            if (trait != null)
            {
                var value = trait.type.value.First().value;
                if (!string.IsNullOrWhiteSpace(value))
                    return value;
            }

            return null;
        }
    }
}
