
namespace Icon.Web.DataAccess.Commands
{
    public class UpdateIrmaItemCommand
    {
        public int IrmaItemId { get; set; }
        public string BrandName { get; set; }
        public string ItemDescription { get; set; }
        public string PosDescription { get; set; }
        public int PackageUnit { get; set; }
        public decimal? RetailSize { get; set; }
        public string RetailUom { get; set; }
        public string DeliverySystem { get; set; }
        public bool FoodStampEligible { get; set; }
        public decimal PosScaleTare { get; set; }
        public int? MerchandiseHierarchyClassId { get; set; }
        public int? TaxHierarchyClassId { get; set; }
        public int? NationalHierarchyClassId { get; set; }
        public string AnimalWelfareRating { get; set; }

        public bool? Biodynamic { get; set; }

        public string CheeseMilkType { get; set; }

        public bool? CheeseRaw { get; set; }

        public string EcoScaleRating { get; set; }

        public bool? Msc { get; set; }

        public bool? PremiumBodyCare { get; set; }

        public string SeafoodFreshOrFrozen { get; set; }

        public string SeafoodCatchType { get; set; }

        public bool? Vegetarian { get; set; }

        public bool? WholeTrade { get; set; }

        public bool? GrassFed { get; set; }
        public bool? PastureRaised { get; set; }
        public bool? FreeRange { get; set; }
        public bool? DryAged { get; set; }
        public bool? AirChilled { get; set; }
        public bool? MadeInHouse { get; set; }
        public decimal? AlcoholByVolume { get; set; }
        public bool IsLoaded { get; set; }
    }
}
