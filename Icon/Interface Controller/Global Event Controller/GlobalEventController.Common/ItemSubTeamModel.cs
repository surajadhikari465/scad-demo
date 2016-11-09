using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Common
{
    public class ItemSubTeamModel
    {
        public int ItemId { get; set; }
        public string ValidationDate { get; set; }
        public string ScanCode { get; set; }
        public string ScanCodeType { get; set; }
        public string ProductDescription { get; set; }
        public string PosDescription { get; set; }
        public string PackageUnit { get; set; }
        public string FoodStampEligible { get; set; }
        public string Tare { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string TaxClassName { get; set; }
        public string SubTeamName { get; set; }
        public int SubTeamNo { get; set; }
        public int DeptNo { get; set; }
        public bool SubTeamNotAligned { get; set; }

        public ItemSubTeamModel() { }

        public ItemSubTeamModel(ScanCode scanCode)
        {
            ItemId = scanCode.itemID;

            var validationDateQuery = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.ValidationDate);
            ValidationDate = validationDateQuery.Count() == 0 ? String.Empty : validationDateQuery.Single().traitValue;

            var scanCodeQuery = scanCode;
            ScanCode = scanCode.scanCode;
            ScanCodeType = scanCode.ScanCodeType.scanCodeTypeDesc;

            var productDescriptionQuery = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.ProductDescription);
            ProductDescription = productDescriptionQuery.Count() == 0 ? String.Empty : productDescriptionQuery.Single().traitValue;

            var posDescriptionQuery = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.PosDescription);
            PosDescription = posDescriptionQuery.Count() == 0 ? String.Empty : posDescriptionQuery.Single().traitValue;

            var packageUnitQuery = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.PackageUnit);
            PackageUnit = packageUnitQuery.Count() == 0 ? String.Empty : packageUnitQuery.Single().traitValue;

            var foodStampEligibleQuery = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.FoodStampEligible);
            FoodStampEligible = foodStampEligibleQuery.Count() == 0 ? String.Empty : foodStampEligibleQuery.Single().traitValue;

            var posScaleTare = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.Trait.traitCode == TraitCodes.PosScaleTare && itemTrait.Locale.localeTypeID == LocaleTypes.Chain);
            Tare = posScaleTare.Count() == 0 ? String.Empty : posScaleTare.Single().traitValue;

            var brandQuery = scanCode.Item.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.Hierarchy.hierarchyName == HierarchyNames.Brands);
            BrandId = brandQuery.Count() == 0 ? -1 : brandQuery.Single().HierarchyClass.hierarchyClassID;
            BrandName = brandQuery.Count() == 0 ? String.Empty : brandQuery.Single().HierarchyClass.hierarchyClassName;
            BrandName = BrandName.Length > 25 ? BrandName.Substring(0, 25) : BrandName;

            var taxHierarchyClassQuery = scanCode.Item.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.Hierarchy.hierarchyName == HierarchyNames.Tax);
            if (taxHierarchyClassQuery.Count() == 0)
            {
                TaxClassName = String.Empty;
            }
            else
            {
                var taxQuery = taxHierarchyClassQuery.Single().HierarchyClass.HierarchyClassTrait.Where(hct => hct.Trait.traitCode == TraitCodes.TaxAbbreviation);
                if (taxQuery.Count() == 0)
                {
                    TaxClassName = String.Empty;
                }
                else
                {
                    TaxClassName = taxQuery.Single().traitValue;
                }
            }

            var subTeamQuery = scanCode.Item.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.Hierarchy.hierarchyName == HierarchyNames.Merchandise);
            if (subTeamQuery.Count() > 0)
            {
                var subteamHierarchy = subTeamQuery.Single().HierarchyClass.HierarchyClassTrait.Where(ht => ht.Trait.traitCode == TraitCodes.MerchFinMapping);
                SubTeamName = subteamHierarchy.Count() == 0 ? String.Empty : subteamHierarchy.Single().traitValue;
            }
        }
    }
}
