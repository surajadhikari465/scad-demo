using Icon.Web.DataAccess.Attributes;
using System;

namespace Icon.Web.DataAccess.Models
{
    public class ItemSearchModel
    {
        public int ItemId { get; set; }

        [SqlResult(SqlSortValue = "sc.scanCode")]
        public string ScanCode { get; set; }

        public string BrandName { get; set; }

        [SqlResult(SqlSortValue = "brand.hierarchyClassName")]
        public int? BrandHierarchyClassId { get; set; }

        [SqlResult(SqlSortValue = "pd.traitValue")]
        public string ProductDescription { get; set; }

        [SqlResult(SqlSortValue = "pos.traitValue")]
        public string PosDescription { get; set; }

        [SqlResult(SqlSortValue = "pack.traitValue")]
        public string PackageUnit { get; set; }

        [SqlResult(SqlSortValue = "fs.traitValue")]
        public string FoodStampEligible { get; set; }

        [SqlResult(SqlSortValue = "tare.traitValue")]
        public string PosScaleTare { get; set; }

        [SqlResult(SqlSortValue = "size.traitValue")]
        public string RetailSize { get; set; }

        [SqlResult(SqlSortValue = "uom.traitValue")]
        public string RetailUom { get; set; }

        [SqlResult(SqlSortValue = "ds.traitValue")]
        public string DeliverySystem { get; set; }

        public string MerchandiseHierarchyName { get; set; }

        [SqlResult(SqlSortValue = "merch.hierarchyClassName")]
        public int? MerchandiseHierarchyClassId { get; set; }

        public string TaxHierarchyName { get; set; }

        [SqlResult(SqlSortValue = "tax.hierarchyClassName")]
        public int? TaxHierarchyClassId { get; set; }

        public string BrowsingHierarchyName { get; set; }

        public int? BrowsingHierarchyClassId { get; set; }

        [SqlResult(SqlSortValue = "vld.traitValue")]
        public string ValidatedDate { get; set; }

        [SqlResult(SqlSortValue = "hid.traitValue")]
        public string HiddenItem { get; set; }

        [SqlResult(SqlSortValue = "dept.traitValue")]
        public string DepartmentSale { get; set; }

        public string NationalHierarchyName { get; set; }

        [SqlResult(SqlSortValue = "nat.hierarchyClassName")]
        public int? NationalHierarchyClassId { get; set; }

        [SqlResult(SqlSortValue = "note.traitValue")]
        public string Notes { get; set; }

        [SqlResult(SqlSortValue = "abv.traitValue")]
        public string AlcoholByVolume { get; set; }

        [SqlResult(SqlSortValue = "cf.traitValue")]
        public string CaseinFree { get; set; }

        [SqlResult(SqlSortValue = "dw.traitValue")]
        public string DrainedWeight { get; set; }

        [SqlResult(SqlSortValue = "dwu.traitValue")]
        public string DrainedWeightUom { get; set; }

        [SqlResult(SqlSortValue = "ftc.traitValue")]
        public string FairTradeCertified { get; set; }

        [SqlResult(SqlSortValue = "hem.traitValue")]
        public string Hemp { get; set; }

        [SqlResult(SqlSortValue = "llp.traitValue")]
        public string LocalLoanProducer { get; set; }

        [SqlResult(SqlSortValue = "mpn.traitValue")]
        public string MainProductName { get; set; }

        [SqlResult(SqlSortValue = "nr.traitValue")]
        public string NutritionRequired { get; set; }

        [SqlResult(SqlSortValue = "opc.traitValue")]
        public string OrganicPersonalCare { get; set; }

        [SqlResult(SqlSortValue = "plo.traitValue")]
        public string Paleo { get; set; }

        [SqlResult(SqlSortValue = "pfy.traitValue")]
        public string ProductFlavorType { get; set; }

        [SqlResult(SqlSortValue = "isa.AnimalWelfareRatingId")]
        public int? AnimalWelfareRatingId { get; set; }

        [SqlResult(SqlSortValue = "isa.Biodynamic")]
        public bool? Biodynamic { get; set; }

        [SqlResult(SqlSortValue = "isa.CheeseMilkTypeId")]
        public int? CheeseMilkTypeId { get; set; }

        [SqlResult(SqlSortValue = "isa.CheeseRaw")]
        public bool? CheeseRaw { get; set; }

        [SqlResult(SqlSortValue = "isa.EcoScaleRatingId")]
        public int? EcoScaleRatingId { get; set; }

        [SqlResult(SqlSortValue = "isa.GlutenFreeAgencyId")]
        public int? GlutenFreeAgencyId { get; set; }

        [SqlResult(SqlSortValue = "isa.KosherAgencyId")]
        public int? KosherAgencyId { get; set; }

        [SqlResult(SqlSortValue = "isa.Msc")]
        public bool? Msc { get; set; }

        [SqlResult(SqlSortValue = "isa.NonGmoAgencyId")]
        public int? NonGmoAgencyId { get; set; }

        [SqlResult(SqlSortValue = "isa.OrganicAgencyId")]
        public int? OrganicAgencyId { get; set; }

        [SqlResult(SqlSortValue = "isa.PremiumBodyCare")]
        public bool? PremiumBodyCare { get; set; }

        [SqlResult(SqlSortValue = "isa.SeafoodFreshOrFrozenId")]
        public int? SeafoodFreshOrFrozenId { get; set; }

        [SqlResult(SqlSortValue = "isa.SeafoodCatchTypeId")]
        public int? SeafoodCatchTypeId { get; set; }

        [SqlResult(SqlSortValue = "isa.VeganAgencyId")]
        public int? VeganAgencyId { get; set; }

        [SqlResult(SqlSortValue = "isa.Vegetarian")]
        public bool? Vegetarian { get; set; }

        [SqlResult(SqlSortValue = "isa.WholeTrade")]
        public bool? WholeTrade { get; set; }

        [SqlResult(SqlSortValue = "isa.GrassFed")]
        public bool? GrassFed { get; set; }

        [SqlResult(SqlSortValue = "isa.PastureRaised")]
	    public bool? PastureRaised { get; set; }

        [SqlResult(SqlSortValue = "isa.FreeRange")]
	    public bool? FreeRange { get; set; }

        [SqlResult(SqlSortValue = "isa.DryAged")]
	    public bool? DryAged	{ get; set; }

        [SqlResult(SqlSortValue = "isa.AirChilled")]
	    public bool? AirChilled	{ get; set; }

        [SqlResult(SqlSortValue = "isa.MadeinHouse")]
        public bool? MadeInHouse { get; set; }

        [SqlResult(SqlSortValue = "crdate.traitValue")]
        public string CreatedDate { get; set; }

        [SqlResult(SqlSortValue = "moddate.traitValue")]
        public string LastModifiedDate { get; set; }

        [SqlResult(SqlSortValue = "modusr.traitValue")]
        public string LastModifiedUser { get; set; }

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
            return ValidatedDate == null ? false : true;
        }

        public bool GetHiddenItemStatus()
        {
            return String.IsNullOrEmpty(HiddenItem) || HiddenItem == "0" ? false : true;
        }
    }
}
