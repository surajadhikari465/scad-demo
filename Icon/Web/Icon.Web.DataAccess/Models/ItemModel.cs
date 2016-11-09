namespace Icon.Web.DataAccess.Models
{
    using Icon.Web.Common.Utility;

    public class ItemModel
    {
        public int ItemId { get; set; }
        public int? IrmaItemId { get; set; }
        public string ScanCode { get; set; }

        public string BrandName { get; set; }
        public string brandNameDisplayOnly { get; set; }
        public string BrandLineage { get; set; }
        public int? BrandHierarchyClassId { get; set; }

        public string ProductDescription { get; set; }
        public string PosDescription { get; set; }
        public string PackageUnit { get; set; }
        public string RetailSize { get; set; }
        public string RetailUom { get; set; }
        public string DeliverySystem { get; set; }
        public string FoodStampEligible { get; set; }
        public bool IsFoodStampEligible { get { return ConversionUtility.ToBool(FoodStampEligible); } }
        public string PosScaleTare { get; set; }

        public string MerchandiseLineage { get; set; }
        public int? MerchandiseHierarchyClassId { get; set; }
        public string MerchandiseHierarchyClassName { get; set; }

        public string NationalLineage { get; set; }
        public string NationalHierarchyName { get; set; }
        public int? NationalHierarchyClassId { get; set; }

        public string TaxLineage { get; set; }
        public string TaxHierarchyName { get; set; }
        public int? TaxHierarchyClassId { get; set; }
        public string TaxHierarchyClassName { get; set; }
        public string DefaultTaxClass { get; set; }

        public string BrowsingLineage { get; set; }
        public string BrowsingHierarchyName { get; set; }
        public int? BrowsingHierarchyClassId { get; set; }

        public string IsValidated { get; set; }
        public bool IsValidationStatus { get { return IsValidated != null; } }

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
        public int? SeafoodFreshOrFrozenId { get; set; }
        public int? SeafoodCatchTypeId { get; set; }
        public int? VeganAgencyId { get; set; }
        public bool? Vegetarian { get; set; }
        public bool? WholeTrade { get; set; }
        public bool? GrassFed { get; set; }
        public bool? PastureRaised { get; set; }
        public bool? FreeRange { get; set; }
        public bool? DryAged { get; set; }
        public bool? AirChilled { get; set; }
        public bool? MadeInHouse { get; set; }
        public bool? Msc { get; set; }

        public string DepartmentSale { get; set; }
        public bool IsDepartmentSale { get { return ConversionUtility.ToBool(DepartmentSale); } }
        public string GiftCard { get; set; }
        public int? HealthyEatingRatingId { get; set; }
        public string HiddenItem { get; set; }
        public bool IsHiddenItemStatus { get { return ConversionUtility.ToBool(HiddenItem); } }
        public string Notes { get; set; }

        public string UserName { get; set; }
        public string CreatedDate { get; set; }
        public string LastModifiedDate { get; set; }
        public string LastModifiedUser { get; set; }
    }
}