using Icon.Framework;
using Icon.Web.Extensions;
using System;
using System.Linq;

namespace Icon.Web.Mvc.Models
{
    public class ItemAccessor
    {
        public static string GetScanCode(Item item)
        {
            var scanCodeQuery = item.ScanCode;
            return scanCodeQuery.Count == 0 ? String.Empty : scanCodeQuery.Single().scanCode;
        }

        public static string GetProductDescription(Item item)
        {
            var productDescriptionQuery = item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.ProductDescription);
            return productDescriptionQuery.Count() == 0 ? String.Empty : productDescriptionQuery.Single().traitValue;
        }

        public static string GetPosDescription(Item item)
        {
            var posDescriptionQuery = item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.PosDescription);
            return posDescriptionQuery.Count() == 0 ? String.Empty : posDescriptionQuery.Single().traitValue;
        }

        public static string GetPackageUnit(Item item)
        {
            var packageUnitQuery = item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.PackageUnit);
            return packageUnitQuery.Count() == 0 ? String.Empty : packageUnitQuery.Single().traitValue;
        }

        public static bool GetFoodStampEligible(Item item)
        {
            var foodStampEligibleQuery = item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.FoodStampEligible);
            var foodStampEligibleValue = foodStampEligibleQuery.Count() == 0 ? String.Empty : foodStampEligibleQuery.Single().traitValue;
            return String.IsNullOrEmpty(foodStampEligibleValue) || foodStampEligibleValue == "0" ? false : true;
        }

        public static string GetPosScaleTare(Item item)
        {
            var posScaleTare = item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.PosScaleTare);
            return posScaleTare.Count() == 0 ? String.Empty : posScaleTare.Single(sct => sct.localeID == Locales.WholeFoods).traitValue;
        }

        public static bool GetDepartmentSale(Item item)
        {
            var departmentSaleQuery = item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.DepartmentSale);
            var departmentSaleValue = departmentSaleQuery.Count() == 0 ? String.Empty : departmentSaleQuery.Single().traitValue;
            return String.IsNullOrEmpty(departmentSaleValue) || departmentSaleValue == "0" ? false : true;
        }

        public static string GetHierarchyClassName(Item item, string hierarchyName)
        {
            var hierarchyQuery = item.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.Hierarchy.hierarchyName == hierarchyName);
            return hierarchyQuery.Count() == 0 ? String.Empty : hierarchyQuery.Single().HierarchyClass.hierarchyClassName;
        }

        public static string GetHierarchyClassLineage(Item item, string hierarchyName)
        {
            var hierarchyQuery = item.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.Hierarchy.hierarchyName == hierarchyName);
            var splitHierarchyLineage = hierarchyQuery.Count() == 0 ? new string[] { } : hierarchyQuery.Single().HierarchyClass
                .ToFlattenedHierarchyClassString()
                .Split('|');

            return String.Join("|", splitHierarchyLineage.Take(splitHierarchyLineage.Length - 1));
        }

        public static int GetHierarchyClassId(Item item, string hierarchyName)
        {
            var hierarchyQuery = item.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.Hierarchy.hierarchyName == hierarchyName);
            return hierarchyQuery.Count() == 0 ? -1 : hierarchyQuery.Single().HierarchyClass.hierarchyClassID;
        }

        public static bool GetStatus(Item item)
        {
            bool IsValidated = item.ItemTrait.Any(it => it.Trait.traitCode == TraitCodes.ValidationDate);
            return IsValidated;
        }

        public static string GetRetailSize(Item item)
        {
            var retailSize = item.ItemTrait.Single(it => it.traitID == Traits.RetailSize);
            return retailSize.traitValue == null ? String.Empty : retailSize.traitValue;
        }

        public static string GetRetailUom(Item item)
        {
            var retailUom = item.ItemTrait.Single(it => it.traitID == Traits.RetailUom);
            return retailUom.traitValue == null ? String.Empty : retailUom.traitValue;
        }

        public static string GetDeliverySystem(Item item)
        {
            var deliverySystem = item.ItemTrait.SingleOrDefault(it => it.traitID == Traits.DeliverySystem);
            return deliverySystem == null ? String.Empty : deliverySystem.traitValue;
        }
        public static bool GetHiddenItem(Item item)
        {
            var hiddenItemQuery = item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.HiddenItem);
            var hiddenItemValue = hiddenItemQuery.Count() == 0 ? String.Empty : hiddenItemQuery.Single().traitValue;
            return String.IsNullOrEmpty(hiddenItemValue) || hiddenItemValue == "0" ? false : true;
        }

        public static string GetNotes(Item item)
        {
            var notesQuery = item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.Notes);
            return notesQuery.Count() == 0 ? String.Empty : notesQuery.Single().traitValue;
        }

        public static string GetAnimalWelfareRating(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ?  null : signAttributes.AnimalWelfareRating;
        }

        public static string GetBiodynamic(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? string.Empty : signAttributes.Biodynamic.BoolToYesNoStringValue();
        }

        public static int? GetCheeseMilkTypeId(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? (int?) null : signAttributes.CheeseMilkTypeId;
        }

        public static string GetCheeseRaw(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? string.Empty : signAttributes.CheeseRaw.BoolToYesNoStringValue();
        }

        public static int? GetEcoScaleRatingId(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? (int?) null : signAttributes.EcoScaleRatingId;
        }

        public static string GetPremiumBodyCare(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? string.Empty : signAttributes.PremiumBodyCare.BoolToYesNoStringValue();
        }

        public static DateTime? GetValidatedDate(Item item)
        {
            var trait = item.ItemTrait.FirstOrDefault(it => it.traitID == Traits.ValidationDate);
            return trait == null ? (DateTime?)null : DateTime.Parse(trait.traitValue);
        }

        public static int? GetSeafoodFreshOrFrozenId(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? (int?) null : signAttributes.SeafoodFreshOrFrozenId;
        }

        public static int? GetSeafoodCatchTypeId(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? (int?) null : signAttributes.SeafoodCatchTypeId;
        }
        
        public static string GetVegetarian(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? string.Empty : signAttributes.Vegetarian.BoolToYesNoStringValue();
        }
        
        public static string GetWholeTrade(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? string.Empty : signAttributes.WholeTrade.BoolToYesNoStringValue();
        }

        internal static string GetMsc(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? string.Empty : signAttributes.Msc.BoolToYesNoStringValue();
        }

        public static string GetGrassFed(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? string.Empty : signAttributes.GrassFed.BoolToYesNoStringValue();
        }

        public static string GetPastureRaised(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? string.Empty : signAttributes.PastureRaised.BoolToYesNoStringValue();
        }

        public static string GetFreeRange(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? string.Empty : signAttributes.FreeRange.BoolToYesNoStringValue();
        }

        public static string GetDryAged(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? string.Empty : signAttributes.DryAged.BoolToYesNoStringValue();
        }

        public static string GetAirChilled(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? string.Empty : signAttributes.AirChilled.BoolToYesNoStringValue();
        }

        public static string GetMadeInHouse(Item item)
        {
            var signAttributes = item.ItemSignAttribute.FirstOrDefault();
            return signAttributes == null ? string.Empty : signAttributes.MadeInHouse.BoolToYesNoStringValue();
        }
    }
}