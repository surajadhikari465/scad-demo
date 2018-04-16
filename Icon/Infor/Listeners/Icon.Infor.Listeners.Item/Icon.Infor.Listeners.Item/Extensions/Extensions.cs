using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Constants;
using Icon.Infor.Listeners.Item.Constants.ItemValidation;
using Icon.Infor.Listeners.Item.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Item.Extensions
{
    public static class Extensions
    {
        public static ItemModel ToItemModel(
            this Esb.Schemas.Wfm.Contracts.ItemType item, 
            string inforMessageId, 
            DateTime messageParseTime,
            decimal? sequenceId)
        {
            var enterpriseAttributes = item.locale.First().Item as EnterpriseItemAttributesType;
            return new ItemModel
            {
                ItemId = item.id,
                ItemTypeCode = item.@base.type.code,
                ScanCode = enterpriseAttributes.scanCodes.First().code,
                ScanCodeType = GetScanCodeTypeCode(enterpriseAttributes.scanCodes.First().code),
                MerchandiseHierarchyClassId = GetHierarchyClassId(enterpriseAttributes, HierarchyNames.Merchandise),
                BrandsHierarchyClassId = GetHierarchyClassId(enterpriseAttributes, HierarchyNames.Brands),
                TaxHierarchyClassId = GetHierarchyClassId(enterpriseAttributes, HierarchyNames.Tax),
                FinancialHierarchyClassId = GetHierarchyClassId(enterpriseAttributes, HierarchyNames.Financial),
                NationalHierarchyClassId = GetHierarchyClassId(enterpriseAttributes, HierarchyNames.National),
                ProductDescription = GetTraitValue(enterpriseAttributes, TraitCodes.ProductDescription),
                PosDescription = GetTraitValue(enterpriseAttributes, TraitCodes.PosDescription),
                FoodStampEligible = GetTraitValue(enterpriseAttributes, TraitCodes.FoodStampEligible),
                PosScaleTare = GetTraitValue(enterpriseAttributes, TraitCodes.PosScaleTare),
                ProhibitDiscount = GetTraitValue(enterpriseAttributes, TraitCodes.ProhibitDiscount),
                PackageUnit = GetTraitValue(enterpriseAttributes, TraitCodes.PackageUnit),
                RetailSize = GetTraitValue(enterpriseAttributes, TraitCodes.RetailSize),
                RetailUom = GetTraitValue(enterpriseAttributes, TraitCodes.RetailUom),
                AnimalWelfareRating = GetTraitValue(enterpriseAttributes, TraitCodes.AnimalWelfareRating),
                Biodynamic = GetTraitValue(enterpriseAttributes, TraitCodes.Biodynamic, "0"),
                CheeseMilkType = GetTraitValue(enterpriseAttributes, TraitCodes.CheeseMilkType),
                CheeseRaw = GetTraitValue(enterpriseAttributes, TraitCodes.CheeseRaw, "0"),
                EcoScaleRating = GetTraitValue(enterpriseAttributes, TraitCodes.EcoScaleRating),
                GlutenFree = GetTraitValue(enterpriseAttributes, TraitCodes.GlutenFree),
                Kosher = GetTraitValue(enterpriseAttributes, TraitCodes.Kosher),
                Msc = GetTraitValue(enterpriseAttributes, TraitCodes.Msc, "0"),
                NonGmo = GetTraitValue(enterpriseAttributes, TraitCodes.NonGmo),
                Organic = GetTraitValue(enterpriseAttributes, TraitCodes.Organic),
                PremiumBodyCare = GetTraitValue(enterpriseAttributes, TraitCodes.PremiumBodyCare, "0"),
                FreshOrFrozen = GetTraitValue(enterpriseAttributes, TraitCodes.FreshOrFrozen),
                SeafoodCatchType = GetTraitValue(enterpriseAttributes, TraitCodes.SeafoodCatchType),
                Vegan = GetTraitValue(enterpriseAttributes, TraitCodes.Vegan),
                Vegetarian = GetTraitValue(enterpriseAttributes, TraitCodes.Vegetarian, "0"),
                WholeTrade = GetTraitValue(enterpriseAttributes, TraitCodes.WholeTrade, "0"),
                GrassFed = GetTraitValue(enterpriseAttributes, TraitCodes.GrassFed, "0"),
                PastureRaised = GetTraitValue(enterpriseAttributes, TraitCodes.PastureRaised, "0"),
                FreeRange = GetTraitValue(enterpriseAttributes, TraitCodes.FreeRange, "0"),
                DryAged = GetTraitValue(enterpriseAttributes, TraitCodes.DryAged, "0"),
                AirChilled = GetTraitValue(enterpriseAttributes, TraitCodes.AirChilled, "0"),
                MadeInHouse = GetTraitValue(enterpriseAttributes, TraitCodes.MadeInHouse, "0"),
                AlcoholByVolume = GetTraitValue(enterpriseAttributes, TraitCodes.AlcoholByVolume),
                CaseinFree = GetTraitValue(enterpriseAttributes, TraitCodes.CaseinFree),
                DrainedWeight = GetTraitValue(enterpriseAttributes, TraitCodes.DrainedWeight),
                DrainedWeightUom = GetTraitValue(enterpriseAttributes, TraitCodes.DrainedWeightUom),
                FairTradeCertified = GetTraitValue(enterpriseAttributes, TraitCodes.FairTradeCertified),
                Hemp = GetTraitValue(enterpriseAttributes, TraitCodes.Hemp),
                LocalLoanProducer = GetTraitValue(enterpriseAttributes, TraitCodes.LocalLoanProducer),
                MainProductName = GetTraitValue(enterpriseAttributes, TraitCodes.MainProductName),
                NutritionRequired = GetTraitValue(enterpriseAttributes, TraitCodes.NutritionRequired),
                OrganicPersonalCare = GetTraitValue(enterpriseAttributes, TraitCodes.OrganicPersonalCare),
                Paleo = GetTraitValue(enterpriseAttributes, TraitCodes.Paleo),
                ProductFlavorType = GetTraitValue(enterpriseAttributes, TraitCodes.ProductFlavorType),
                InsertDate = GetTraitValue(enterpriseAttributes, TraitCodes.InsertDate),
                ModifiedDate = GetTraitValue(enterpriseAttributes, TraitCodes.ModifiedDate),
                ModifiedUser = GetTraitValue(enterpriseAttributes, TraitCodes.ModifiedUser),
                HiddenItem = GetTraitValue(enterpriseAttributes, TraitCodes.HiddenItem),
                Notes = GetTraitValue(enterpriseAttributes, TraitCodes.Notes),
                DeliverySystem = GetTraitValue(enterpriseAttributes, TraitCodes.DeliverySystem),
                InforMessageId = Guid.Parse(inforMessageId),
                ContainesDuplicateMerchandiseClass = ContainsDuplicateHierarchyClass(enterpriseAttributes, HierarchyNames.Merchandise),
                ContainesDuplicateNationalClass = ContainsDuplicateHierarchyClass(enterpriseAttributes, HierarchyNames.National),
                MessageParseTime = messageParseTime,
                SequenceId = sequenceId,
                CustomerFriendlyDescription = GetTraitValue(enterpriseAttributes, TraitCodes.CustomerFriendlyDescription),
                GlobalPricingProgram = GetTraitValue(enterpriseAttributes, TraitCodes.GlobalPricingProgram),
                FlexibleText = GetTraitValue(enterpriseAttributes, TraitCodes.FlexibleText),
                MadeWithOrganicGrapes = GetTraitValue(enterpriseAttributes, TraitCodes.MadeWithOrganicGrapes),
                PrimeBeef = GetTraitValue(enterpriseAttributes, TraitCodes.PrimeBeef),
                RainforestAlliance = GetTraitValue(enterpriseAttributes, TraitCodes.RainforestAlliance),
                Refrigerated = GetTraitValue(enterpriseAttributes, TraitCodes.Refrigerated),
                SmithsonianBirdFriendly = GetTraitValue(enterpriseAttributes, TraitCodes.SmithsonianBirdFriendly),
                WicEligible = GetTraitValue(enterpriseAttributes, TraitCodes.WicEligible),
                ShelfLife = GetTraitValue(enterpriseAttributes, TraitCodes.ShelfLife),
                SelfCheckoutItemTareGroup = GetTraitValue(enterpriseAttributes, TraitCodes.SelfCheckoutItemTareGroup),

            };
        }

        private static string GetScanCodeTypeCode(string scanCode)
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
            return Regex.IsMatch(scanCode, RegularExpressions.ScalePlu)
                || Regex.IsMatch(scanCode, RegularExpressions.IngredientPlu46)
                || Regex.IsMatch(scanCode, RegularExpressions.IngredientPlu48);
        }

        private static int GetItemTypeCodeId(string typeCode)
        {
            if (ItemTypes.Ids.ContainsKey(typeCode))
            {
                return ItemTypes.Ids[typeCode];
            }
            else
            {
                return -1;
            }
        }

        private static int GetScanCodeTypeId(string scanCode)
        {
            if (scanCode.Length == 11 && scanCode[0] == '2' && scanCode.EndsWith("00000"))
            {
                return ScanCodeTypes.ScalePlu;
            }
            else if (scanCode.Length < 7)
            {
                return ScanCodeTypes.PosPlu;
            }
            else
            {
                return ScanCodeTypes.Upc;
            }
        }

        private static string GetTraitValue(EnterpriseItemAttributesType enterpriseAttributes, string traitCode, string defaultValue = "")
        {
            var traitValue = enterpriseAttributes.traits.FirstOrDefault(i => i.code == traitCode);

            if (traitValue == null || string.IsNullOrWhiteSpace(traitValue.type.value.First().value))
                return defaultValue;
            else
                return traitValue.type.value.First().value;
        }

        private static string GetHierarchyClassId(EnterpriseItemAttributesType enterpriseAttributes, string hierarchyName)
        {
            try
            {
                var hierarchyClassId = enterpriseAttributes.hierarchies.FirstOrDefault(i => i.name == hierarchyName).@class.FirstOrDefault().id;
                if (hierarchyClassId == null)
                {
                    return string.Empty;
                }
                else
                {
                    return hierarchyClassId;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static int? GetIdFromDescription(this Dictionary<string, int> descriptionToIdDictionary, string description)
        {
            if (description == null || !descriptionToIdDictionary.ContainsKey(description))
            {
                return null;
            }
            else
            {
                return descriptionToIdDictionary[description];
            }
        }

        public static bool ToBool(this string boolString)
        {
            return boolString == "1";
        }

        private static bool ContainsDuplicateHierarchyClass(EnterpriseItemAttributesType enterpriseAttributes, string hierarchyName)
        {
            var hierarchyClassCount = enterpriseAttributes.hierarchies.Count(i => i.name == hierarchyName);

            return hierarchyClassCount > 1;
        }
    }
}
