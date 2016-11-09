using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Models
{
    public class ItemSpreadsheetModel : IImportItemModel

    {
        public string ScanCode { get; set; }
        public string BrandName { get; set; }
        public string BrandLineage { get; set; }
        public string BrandId { get; set; }
        public string ProductDescription { get; set; }
        public string PosDescription { get; set; }
        public string PackageUnit { get; set; }
        public string FoodStampEligible { get; set; }
        public string PosScaleTare { get; set; }
        public string RetailSize { get; set; }
        public string RetailUom { get; set; }
        public string DeliverySystem { get; set; }
        public string MerchandiseLineage { get; set; }
        public string MerchandiseId { get; set; }
        public string NationalLineage { get; set; }
        public string NationalId { get; set; }
        public string TaxLineage { get; set; }
        public string TaxId { get; set; }
        public string BrowsingLineage { get; set; }
        public string BrowsingId { get; set; }
        public string DepartmentSale { get; set; }
        public string HiddenItem { get; set; }
        public string IsValidated { get; set; }
        public string AnimalWelfareRating { get; set; }
        public string AnimalWelfareRatingId { get; set; }
        public string Biodynamic { get; set; }
        public string CheeseAttributeMilkType { get; set; }
        public string CheeseAttributeMilkTypeId { get; set; }
        public string CheeseAttributeRaw { get; set; }
        public string EcoScaleRating { get; set; }
        public string EcoScaleRatingId { get; set; }
        public string GlutenFreeAgencyId { get; set; }
        public string GlutenFreeAgencyLineage { get; set; }
        public string KosherAgencyId { get; set; }
        public string KosherAgencyLineage { get; set; }
        public string NonGmoAgencyId { get; set; }
        public string NonGmoAgencyLineage { get; set; }
        public string OrganicAgencyId { get; set; }
        public string OrganicAgencyLineage { get; set; }
        public string PremiumBodyCare { get; set; }
        public string ProductionClaimsId { get; set; }
        public string ProductionClaims { get; set; }
        public string SeafoodFreshOrFrozenId { get; set; }
        public string SeafoodFreshOrFrozen { get; set; }
        public string SeafoodWildOrFarmRaisedId { get; set; }
        public string SeafoodWildOrFarmRaised { get; set; }
        public string VeganAgencyId { get; set; }
        public string VeganAgencyLineage { get; set; }
        public string Vegetarian { get; set; }
        public string WholeTrade { get; set; }
        public string GrassFed { get; set; }
        public string PastureRaised { get; set; }
        public string FreeRange { get; set; }
        public string DryAged { get; set; }
        public string AirChilled { get; set; }
        public string MadeInHouse { get; set; }
        public string Error { get; set; }
    }
}