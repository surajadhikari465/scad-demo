using System;

namespace Mammoth.Esb.ProductListener.Models
{
    public class ProductModel
    {
        public int ItemID { get; set; }
        public int ItemTypeID { get; set; }
        public string ScanCode { get; set; }
        public string Desc_Product { get; set; }
        public string Desc_POS { get; set; }
        public string PackageUnit { get; set; }
        public string RetailSize { get; set; }
        public string RetailUOM { get; set; }
        public bool? FoodStampEligible { get; set; }
        public int? TaxClassHCID { get; set; }
        public int? BrandHCID { get; set; }
        public int? PSNumber { get; set; }
        public int? SubBrickID { get; set; }
        public int? NationalClassID { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageTaxClassHCID { get; set; }
        public string AnimalWelfareRating { get; set; }
        public bool? Biodynamic { get; set; }
        public string CheeseMilkType { get; set; }
        public bool? CheeseRaw { get; set; }
        public string EcoScaleRating { get; set; }
        public string GlutenFreeAgency { get; set; }
        public string HealthyEatingRating { get; set; }
        public string KosherAgency { get; set; }
        public bool? Msc { get; set; }
        public string NonGmoAgency { get; set; }
        public string OrganicAgency { get; set; }
        public bool? PremiumBodyCare { get; set; }
        public string SeafoodFreshOrFrozen { get; set; }
        public string SeafoodCatchType { get; set; }
        public string VeganAgency { get; set; }
        public bool? Vegetarian { get; set; }
        public bool? WholeTrade { get; set; }
        public bool? GrassFed { get; set; }
        public bool? PastureRaised { get; set; }
        public bool? FreeRange { get; set; }
        public bool? DryAged { get; set; }
        public bool? AirChilled { get; set; }
        public bool? MadeInHouse { get; set; }
    }
}