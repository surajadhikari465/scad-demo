
namespace Icon.Web.DataAccess.Models
{
    public interface IImportItemModel
    {
        string ScanCode { get; set; }
        string BrandName { get; set; }
        string BrandLineage { get; set; }
        string BrandId { get; set; }
        string ProductDescription { get; set; }
        string PosDescription { get; set; }
        string PackageUnit { get; set; }
        string RetailSize { get; set; }
        string RetailUom { get; set; }
        string DeliverySystem { get; set; }
        string FoodStampEligible { get; set; }
        string PosScaleTare { get; set; }
        string MerchandiseLineage { get; set; }
        string MerchandiseId { get; set; }
        string NationalLineage { get; set; }
        string NationalId { get; set; }
        string TaxLineage { get; set; }
        string TaxId { get; set; }
        string BrowsingLineage { get; set; }
        string BrowsingId { get; set; }
        string IsValidated { get; set; }
        string AnimalWelfareRating { get; set; }
        string AnimalWelfareRatingId { get; set; }
        string Biodynamic { get; set; }
        string CheeseAttributeMilkType { get; set; }
        string CheeseAttributeMilkTypeId { get; set; }
        string CheeseAttributeRaw { get; set; }
        string EcoScaleRating { get; set; }
        string EcoScaleRatingId { get; set; }
        string GlutenFreeAgency { get; set; }
        string GlutenFreeAgencyLineage { get; set; }
        string KosherAgency { get; set; }
        string KosherAgencyLineage { get; set; }
        string NonGmoAgency { get; set; }
        string NonGmoAgencyLineage { get; set; }
        string OrganicAgency { get; set; }
        string OrganicAgencyLineage { get; set; }
        string PremiumBodyCare { get; set; }
        string SeafoodFreshOrFrozenId { get; set; }
        string SeafoodFreshOrFrozen { get; set; }
        string SeafoodWildOrFarmRaisedId { get; set; }
        string SeafoodWildOrFarmRaised { get; set; }
        string VeganAgency { get; set; }
        string VeganAgencyLineage { get; set; }
        string Vegetarian { get; set; }
        string WholeTrade { get; set; }
        string GrassFed { get; set; }
        string PastureRaised { get; set; }
        string FreeRange { get; set; }
        string DryAged { get; set; }
        string AirChilled { get; set; }
        string MadeInHouse { get; set; }
        string Error { get; set; }
    }
}
