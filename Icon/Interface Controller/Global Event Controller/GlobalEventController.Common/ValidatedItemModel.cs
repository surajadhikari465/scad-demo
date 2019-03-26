using Icon.Framework;
using System;
using System.Linq;

namespace GlobalEventController.Common
{
    public class ValidatedItemModel
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
        public string NationalClassCode { get; set; }
        public string SubTeamName { get; set; }
        public int SubTeamNo { get; set; }
        public int DeptNo { get; set; }
        public bool SubTeamNotAligned { get; set; }
        public int EventTypeId { get; set; }
        public string AnimalWelfareRating { get; set; }
        public bool? Biodynamic { get; set; }
        public string MilkType { get; set; }
        public bool? CheeseRaw { get; set; }
        public string EcoScaleRating { get; set; }
        public bool? GlutenFree { get; set; }
        public bool? Kosher { get; set; }
        public string HealthyEatingRating { get; set; }
        public bool? NonGmo { get; set; }
        public bool? Organic { get; set; }
        public bool? PremiumBodyCare { get; set; }
        public string FreshOrFrozen { get; set; }
        public string SeafoodCatchType { get; set; }
        public bool? Vegan { get; set; }
        public bool? Vegetarian { get; set; }
        public bool? WholeTrade { get; set; }
        public bool? Msc { get; set; }
        public bool? GrassFed { get; set; }
        public bool? PastureRaised { get; set; }
        public bool? FreeRange { get; set; }
        public bool? DryAged { get; set; }
        public bool? AirChilled { get; set; }
        public bool? MadeInHouse { get; set; }
        public bool HasItemSignAttributes { get; set; }
        public decimal RetailSize { get; set; }
        public string RetailUom { get; set; }
        public string ItemTypeCode { get; set; }
        public string CustomerFriendlyDescription { get; set; }

        public ValidatedItemModel() { }

        public ValidatedItemModel(ScanCode scanCode)
        {
            ItemId = scanCode.itemID;
            ItemTypeCode = scanCode.Item.ItemType.itemTypeCode;

            var validationDateQuery = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.traitID == Traits.ValidationDate);
            ValidationDate = validationDateQuery.Count() == 0 ? String.Empty : validationDateQuery.Single().traitValue;

            var scanCodeQuery = scanCode;
            ScanCode = scanCode.scanCode;
            ScanCodeType = scanCode.ScanCodeType.scanCodeTypeDesc;

            var productDescriptionQuery = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.traitID == Traits.ProductDescription);
            ProductDescription = productDescriptionQuery.Count() == 0 ? String.Empty : productDescriptionQuery.Single().traitValue;

            var posDescriptionQuery = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.traitID == Traits.PosDescription);
            PosDescription = posDescriptionQuery.Count() == 0 ? String.Empty : posDescriptionQuery.Single().traitValue;

            var packageUnitQuery = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.traitID == Traits.PackageUnit);
            PackageUnit = packageUnitQuery.Count() == 0 ? String.Empty : packageUnitQuery.Single().traitValue;

            var foodStampEligibleQuery = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.traitID == Traits.FoodStampEligible);
            FoodStampEligible = foodStampEligibleQuery.Count() == 0 ? String.Empty : foodStampEligibleQuery.Single().traitValue;

            var posScaleTare = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.traitID == Traits.PosScaleTare && itemTrait.localeID == Locales.WholeFoods);
            Tare = posScaleTare.Count() == 0 ? String.Empty : posScaleTare.Single().traitValue;

            var brandQuery = scanCode.Item.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Brands);
            BrandId = brandQuery.Count() == 0 ? -1 : brandQuery.Single().HierarchyClass.hierarchyClassID;
            BrandName = brandQuery.Count() == 0 ? String.Empty : brandQuery.Single().HierarchyClass.hierarchyClassName;
            BrandName = BrandName.Length > 25 ? BrandName.Substring(0, 25) : BrandName;

            var taxHierarchyClassQuery = scanCode.Item.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Tax);
            if (taxHierarchyClassQuery.Count() == 0)
            {
                TaxClassName = String.Empty;
            }
            else
            {
                var taxQuery = taxHierarchyClassQuery.Single().HierarchyClass.HierarchyClassTrait.Where(hct => hct.traitID == Traits.TaxAbbreviation);
                if (taxQuery.Count() == 0)
                {
                    TaxClassName = String.Empty;
                }
                else
                {
                    TaxClassName = taxQuery.Single().traitValue;
                }
            }
            var nationalHierarchyClassQuery = scanCode.Item.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.National);
            if (nationalHierarchyClassQuery.Count() == 0)
            {
                NationalClassCode = String.Empty;
            }
            else
            {
                var natQuery = nationalHierarchyClassQuery.Single().HierarchyClass.HierarchyClassTrait.Where(hct => hct.traitID == Traits.NationalClassCode);
                if (natQuery.Count() == 0)
                {
                    NationalClassCode = String.Empty;
                }
                else
                {
                    NationalClassCode = natQuery.Single().traitValue;
                }
            }
            var subTeamQuery = scanCode.Item.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Merchandise);
            if (subTeamQuery.Any())
            {
                var subteamHierarchy = subTeamQuery.Single().HierarchyClass.HierarchyClassTrait.Where(ht => ht.traitID == Traits.MerchFinMapping);
                SubTeamName = subteamHierarchy.Count() == 0 ? String.Empty : subteamHierarchy.Single().traitValue;
            }

            var retailSizeQuery = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.traitID == Traits.RetailSize);
            RetailSize = Convert.ToDecimal(retailSizeQuery.Single().traitValue);

            var retailUomQuery = scanCode.Item.ItemTrait.Where(itemTrait => itemTrait.traitID == Traits.RetailUom);
            RetailUom = retailUomQuery.Single().traitValue;

            SetItemSignAttributes(scanCode);
        }

        private void SetItemSignAttributes(ScanCode scanCode)
        {
            var itemSignAttributes = scanCode.Item.ItemSignAttribute.FirstOrDefault();

            if (itemSignAttributes == null)
            {
                HasItemSignAttributes = false;
            }
            else
            {
                HasItemSignAttributes = true;
                AnimalWelfareRating = !String.IsNullOrEmpty(itemSignAttributes.AnimalWelfareRating) ? itemSignAttributes.AnimalWelfareRating : null;
                HealthyEatingRating = itemSignAttributes.HealthyEatingRatingId.HasValue ? HealthyEatingRatings.AsDictionary[itemSignAttributes.HealthyEatingRatingId.Value] : null;
                Biodynamic = itemSignAttributes.Biodynamic;
                MilkType = !String.IsNullOrEmpty(itemSignAttributes.MilkType) ? itemSignAttributes.MilkType : null;
                CheeseRaw = itemSignAttributes.CheeseRaw;
                EcoScaleRating = !String.IsNullOrEmpty(itemSignAttributes.EcoScaleRating) ? itemSignAttributes.EcoScaleRating : null;
                GlutenFree =!String.IsNullOrEmpty(itemSignAttributes.GlutenFreeAgencyName) ? new Nullable<bool>(true) : null;
                Kosher = !String.IsNullOrEmpty(itemSignAttributes.KosherAgencyName) ? new Nullable<bool>(true) : null;
                Msc = itemSignAttributes.Msc;
                NonGmo = !String.IsNullOrEmpty(itemSignAttributes.NonGmoAgencyName) ? new Nullable<bool>(true) : null;
                Organic = !String.IsNullOrEmpty(itemSignAttributes.OrganicAgencyName) ? new Nullable<bool>(true) : null;
                PremiumBodyCare = itemSignAttributes.PremiumBodyCare;
                FreshOrFrozen = !String.IsNullOrEmpty(itemSignAttributes.FreshOrFrozen) ? itemSignAttributes.FreshOrFrozen : null;
                SeafoodCatchType = !String.IsNullOrEmpty(itemSignAttributes.SeafoodCatchType) ? itemSignAttributes.SeafoodCatchType : null;
                Vegan = !String.IsNullOrEmpty(itemSignAttributes.VeganAgencyName) ? new Nullable<bool>(true) : null;
                Vegetarian = itemSignAttributes.Vegetarian;
                WholeTrade = itemSignAttributes.WholeTrade;
                GrassFed = itemSignAttributes.GrassFed;
                PastureRaised = itemSignAttributes.PastureRaised;
                FreeRange = itemSignAttributes.FreeRange;
                DryAged = itemSignAttributes.DryAged;
                AirChilled = itemSignAttributes.AirChilled;
                MadeInHouse = itemSignAttributes.MadeInHouse;
                CustomerFriendlyDescription = itemSignAttributes.CustomerFriendlyDescription;
            }
        }
    }
}