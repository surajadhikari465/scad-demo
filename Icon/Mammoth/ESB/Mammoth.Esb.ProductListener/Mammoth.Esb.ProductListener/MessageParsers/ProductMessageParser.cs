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

                // Hospitality Attributes
                itemModel.KitItemAttributes = ParseHospitalityElements(enterpriseItemAttributes);
   
                itemModelCollection.Add(itemModel);
            }

            return itemModelCollection;
        }

        private KitItemAttributesModel ParseHospitalityElements(Contracts.EnterpriseItemAttributesType itemInformation)
        {

            var hospitalityModel = new KitItemAttributesModel();
            if (itemInformation == null) return hospitalityModel;

            hospitalityModel.ImageUrl = itemInformation.imageUrl;
            hospitalityModel.KitchenDescription = itemInformation.kitchenDescription;
            if (itemInformation.isHospitalityItemSpecified)
                hospitalityModel.HospitalityItem =  itemInformation.isHospitalityItem;
            if (itemInformation.isKitchenItemSpecified)
                hospitalityModel.KitchenItem = itemInformation.isKitchenItem;
       

            return hospitalityModel;

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
                var consumerProductLabel = item.@base.consumerInformation.stockItemConsumerProductLabel;

				return new NutritionAttributesModel
				{
					ItemID = item.id,
					RecipeName = GetTraitValue(TraitCodes.RecipeName, traits),
					Allergens = GetTraitValue(TraitCodes.Allergens, traits),
					Ingredients = GetTraitValue(TraitCodes.Ingredients, traits),
					ServingsPerPortion = GetFloatTraitValue(TraitCodes.ServingsPerPortion, traits),
					ServingSizeDesc = GetTraitValue(TraitCodes.ServingSizeDesc, traits),
					ServingPerContainer = GetTraitValue(TraitCodes.ServingPerContainer, traits),
					HshRating = GetIntTraitValue(TraitCodes.Hsh, traits),
					ServingUnits = GetIntTraitValue(TraitCodes.ServingUnits, traits),
					SizeWeight = GetIntTraitValue(TraitCodes.SizeWeight, traits),
					CaloriesSaturatedFat = GetIntTraitValue(TraitCodes.CaloriesSaturatedFat, traits),
					PolyunsaturatedFat = GetDecimalTraitValue(TraitCodes.PolyunsaturatedFat, traits),
					MonounsaturatedFat = GetDecimalTraitValue(TraitCodes.MonounsaturatedFat, traits),
					PotassiumWeight = GetDecimalTraitValue(TraitCodes.PotassiumWeight, traits),
					PotassiumPercent = GetIntTraitValue(TraitCodes.PotassiumPercent, traits),
					DietaryFiberPercent = GetIntTraitValue(TraitCodes.DietaryFiberPercent, traits),
					SolubleFiber = GetDecimalTraitValue(TraitCodes.SolubleFiber, traits),
					InsolubleFiber = GetDecimalTraitValue(TraitCodes.InsolubleFiber, traits),
					SugarAlcohol = GetDecimalTraitValue(TraitCodes.SugarAlcohol, traits),
					OtherCarbohydrates = GetDecimalTraitValue(TraitCodes.OtherCarbohydrates, traits),
					ProteinPercent = GetIntTraitValue(TraitCodes.ProteinPercent, traits),
					Betacarotene = GetIntTraitValue(TraitCodes.Betacarotene, traits),
					VitaminD = GetIntTraitValue(TraitCodes.VitaminD, traits),
					VitaminE = GetIntTraitValue(TraitCodes.VitaminE, traits),
					Thiamin = GetIntTraitValue(TraitCodes.Thiamin, traits),
					Riboflavin = GetIntTraitValue(TraitCodes.Riboflavin, traits),
					Niacin = GetIntTraitValue(TraitCodes.Niacin, traits),
					VitaminB6 = GetIntTraitValue(TraitCodes.VitaminB6, traits),
					VitaminB12 = GetIntTraitValue(TraitCodes.VitaminB12, traits),
					Folate = GetIntTraitValue(TraitCodes.Folate, traits),
					Biotin = GetIntTraitValue(TraitCodes.Biotin, traits),
					PantothenicAcid = GetIntTraitValue(TraitCodes.PantothenicAcid, traits),
					Phosphorous = GetIntTraitValue(TraitCodes.Phosphorous, traits),
					Iodine = GetIntTraitValue(TraitCodes.Iodine, traits),
					Magnesium = GetIntTraitValue(TraitCodes.Magnesium, traits),
					Zinc = GetIntTraitValue(TraitCodes.Zinc, traits),
					Copper = GetIntTraitValue(TraitCodes.Copper, traits),
					TransFat = (decimal?)GetFloatTraitValue(TraitCodes.Transfat, traits),
					TransFatWeight = (decimal?)GetFloatTraitValue(TraitCodes.TransfatWeight, traits),
					CaloriesFromTransFat = GetIntTraitValue(TraitCodes.CaloriesFromTransFat, traits),
					Om3Fatty = (decimal?)GetFloatTraitValue(TraitCodes.Om3Fatty, traits),
					Om6Fatty = (decimal?)GetFloatTraitValue(TraitCodes.Om6Fatty, traits),
					Starch = (decimal?)GetFloatTraitValue(TraitCodes.Starch, traits),
					Chloride = GetIntTraitValue(TraitCodes.Chloride, traits),
					Chromium = GetIntTraitValue(TraitCodes.Chromium, traits),
					VitaminK = GetIntTraitValue(TraitCodes.VitaminK, traits),
					Manganese = GetIntTraitValue(TraitCodes.Manganese, traits),
					Molybdenum = GetIntTraitValue(TraitCodes.Molybdenum, traits),
					Selenium = GetIntTraitValue(TraitCodes.Selenium, traits),
					Calories = consumerProductLabel.caloriesCountSpecified ? (int)consumerProductLabel.caloriesCount : (int?)null,
					CaloriesFat = consumerProductLabel.caloriesFromFatCountSpecified ? (int)consumerProductLabel.caloriesFromFatCount : (int?)null,
					TotalFatWeight = consumerProductLabel.totalFatGramsAmountSpecified ? consumerProductLabel.totalFatGramsAmount : (decimal?)null,
					TotalFatPercentage = consumerProductLabel.totalFatDailyIntakePercentSpecified ? (int)consumerProductLabel.totalFatDailyIntakePercent : (int?)null,
					SaturatedFatWeight = consumerProductLabel.saturatedFatGramsAmountSpecified ? consumerProductLabel.saturatedFatGramsAmount : (decimal?)null,
					SaturatedFatPercent = consumerProductLabel.saturatedFatPercentSpecified ? (int)consumerProductLabel.saturatedFatPercent : (int?)null,
					CholesterolWeight = consumerProductLabel.cholesterolMilligramsCountSpecified ? consumerProductLabel.cholesterolMilligramsCount : (decimal?)null,
					CholesterolPercent = consumerProductLabel.cholesterolPercentSpecified ? (int)consumerProductLabel.cholesterolPercent : (int?)null,
					SodiumWeight = consumerProductLabel.sodiumMilligramsCountSpecified ? consumerProductLabel.sodiumMilligramsCount : (decimal?)null,
					SodiumPercent = consumerProductLabel.sodiumPercentSpecified ? (int)consumerProductLabel.sodiumPercent : (int?)null,
					TotalCarbohydrateWeight = consumerProductLabel.totalCarbohydrateMilligramsCountSpecified ? consumerProductLabel.totalCarbohydrateMilligramsCount : (decimal?)null,
					TotalCarbohydratePercent = consumerProductLabel.totalFatDailyIntakePercentSpecified ? (int)consumerProductLabel.totalCarbohydratePercent : (int?)null,
					DietaryFiberWeight = consumerProductLabel.dietaryFiberGramsCountSpecified ? consumerProductLabel.dietaryFiberGramsCount : (decimal?)null,
					Sugar = consumerProductLabel.sugarsGramsCountSpecified ? consumerProductLabel.sugarsGramsCount : (decimal?)null,
					ProteinWeight = consumerProductLabel.proteinGramsCountSpecified ? consumerProductLabel.proteinGramsCount : (decimal?) null,
					VitaminA = consumerProductLabel.vitaminADailyMinimumPercentSpecified ? (int)consumerProductLabel.vitaminADailyMinimumPercent : (int?)null,
					VitaminC = consumerProductLabel.vitaminCDailyMinimumPercentSpecified ? (int)consumerProductLabel.vitaminCDailyMinimumPercent : (int?)null,
					Calcium = consumerProductLabel.calciumDailyMinimumPercentSpecified ? (int)consumerProductLabel.calciumDailyMinimumPercent : (int?)null,
					Iron = consumerProductLabel.ironDailyMinimumPercentSpecified ? (int)consumerProductLabel.ironDailyMinimumPercent : (int?)null,
					AddedSugarsWeight = consumerProductLabel.addedSugarsGramsCountSpecified ? consumerProductLabel.addedSugarsGramsCount : (decimal?)null,
					AddedSugarsPercent = consumerProductLabel.addedSugarDailyPercentSpecified ? (int)consumerProductLabel.addedSugarDailyPercent : (int?)null,
					CalciumWeight = consumerProductLabel.calciumMilligramsCountSpecified ? consumerProductLabel.calciumMilligramsCount : (decimal?)null,
					IronWeight = consumerProductLabel.ironMilligramsCountSpecified ? consumerProductLabel.ironMilligramsCount : (decimal?)null,
					VitaminDWeight = consumerProductLabel.vitaminDMicrogramsCountSpecified ? consumerProductLabel.vitaminDMicrogramsCount : (decimal?)null
				};
            }
        }

        private ExtendedAttributesModel ParseExtendedAttributes(Contracts.ItemType item, TraitType[] traits)
        {
            var extendedAttributes = new ExtendedAttributesModel();
            extendedAttributes.ItemId = item.id;
            extendedAttributes.FairTradeCertified = GetTraitValue(Attributes.Codes.FairTradeCertified, traits);
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
