using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Api.Models
{
    public class ItemSearchModel
    {
        public int ItemId { get; set; }

        public string ScanCode { get; set; }

        public string BrandName { get; set; }

        public int? BrandHierarchyClassId { get; set; }

        public string ProductDescription { get; set; }

        public string PosDescription { get; set; }

        public string PackageUnit { get; set; }

        public string FoodStampEligible { get; set; }

        public string PosScaleTare { get; set; }

        public string RetailSize { get; set; }

        public string RetailUom { get; set; }

        public string MerchandiseHierarchyName { get; set; }

        public int? MerchandiseHierarchyClassId { get; set; }

        public string TaxHierarchyName { get; set; }

        public int? TaxHierarchyClassId { get; set; }

        public string BrowsingHierarchyName { get; set; }

        public int? BrowsingHierarchyClassId { get; set; }

        public string IsValidated { get; set; }

        public string HiddenItem { get; set; }

        public string DepartmentSale { get; set; }

        public string NationalHierarchyName { get; set; }

        public int? NationalHierarchyClassId { get; set; }

        public string Notes { get; set; }

        public int? AnimalWelfareRatingId { get; set; }

        public bool? Biodynamic { get; set; }

        public int? CheeseMilkTypeId { get; set; }

        public bool? CheeseRaw { get; set; }

        public int? EcoScaleRatingId { get; set; }

        public int? GlutenFreeAgencyId { get; set; }

        public int? KosherAgencyId { get; set; }

        public int? NonGmoAgencyId { get; set; }

        public int? OrganicAgencyId { get; set; }

        public bool? PremiumBodyCare { get; set; }

        public int? ProductionClaimsId { get; set; }

        public int? SeafoodFreshOrFrozenId { get; set; }

        public int? SeafoodCatchTypeId { get; set; }

        public int? VeganAgencyId { get; set; }

        public bool? Vegetarian { get; set; }

        public bool? WholeTrade { get; set; }

        public bool GetFoodStampEligible()
        {
            return String.IsNullOrEmpty(FoodStampEligible) || FoodStampEligible == "0" ? false : true;
        }

        public bool GetDepartmentSale()
        {
            return String.IsNullOrEmpty(DepartmentSale) || DepartmentSale == "0" ? false : true;
        }

        public bool GetValidationStatus()
        {
            return IsValidated == null ? false : true;
        }

        public bool GetHiddenItemStatus()
        {
            return String.IsNullOrEmpty(HiddenItem) || HiddenItem == "0" ? false : true;
        }
    }
}