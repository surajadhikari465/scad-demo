using static Icon.Web.Mvc.Excel.ExcelHelper;

namespace Icon.Web.Mvc.Excel.Models
{
    public class NewItemExcelModel : ExcelModel
    {
        [ExcelColumn(ExcelExportColumnNames.ScanCode, ExcelExportColumnWidths.ScanCode)]
        public string ScanCode { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Brand, ExcelExportColumnWidths.Brand)]
        public string Brand { get; set; }

        [ExcelColumn(ExcelExportColumnNames.ProductDescription, ExcelExportColumnWidths.ProductDescription)]
        public string ProductDescription { get; set; }

        [ExcelColumn(ExcelExportColumnNames.PosDescription, ExcelExportColumnWidths.PosDescription)]
        public string PosDescription { get; set; }

        [ExcelColumn(ExcelExportColumnNames.PackageUnit, ExcelExportColumnWidths.PackageUnit)]
        public string PackageUnit { get; set; }

        [ExcelColumn(ExcelExportColumnNames.FoodStampEligible, ExcelExportColumnWidths.FoodStampEligible)]
        public string FoodStampEligible { get; set; }

        [ExcelColumn(ExcelExportColumnNames.PosScaleTare, ExcelExportColumnWidths.PosScaleTare)]
        public string PosScaleTare { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Size, ExcelExportColumnWidths.Size)]
        public string Size { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Uom, ExcelExportColumnWidths.Uom)]
        public string Uom { get; set; }

        [ExcelColumn(ExcelExportColumnNames.DeliverySystem, ExcelExportColumnWidths.DeliverySystem)]
        public string DeliverySystem { get; set; }

        [ExcelColumn(ExcelExportColumnNames.IrmaSubTeam, ExcelExportColumnWidths.IrmaSubTeam)]
        public string IrmaSubTeam { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Merchandise, ExcelExportColumnWidths.Merchandise)]
        public string Merchandise { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Tax, ExcelExportColumnWidths.Tax)]
        public string Tax { get; set; }

        [ExcelColumn(ExcelExportColumnNames.AlcoholByVolume, ExcelExportColumnWidths.AlcoholByVolume)]
        public string AlcoholByVolume { get; set; }

        [ExcelColumn(ExcelExportColumnNames.NationalClass, ExcelExportColumnWidths.NationalClass)]
        public string NationalClass { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Browsing, ExcelExportColumnWidths.Browsing)]
        public string Browsing { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Validated, ExcelExportColumnWidths.Validated)]
        public string Validated { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Region, ExcelExportColumnWidths.Region)]
        public string Region { get; set; }

        [ExcelColumn(ExcelExportColumnNames.AirChilled, ExcelExportColumnWidths.AirChilled)]
        public string AirChilled { get; set; }

        [ExcelColumn(ExcelExportColumnNames.AnimalWelfareRating, ExcelExportColumnWidths.AnimalWelfareRating)]
        public string AnimalWelfareRating { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Biodynamic, ExcelExportColumnWidths.Biodynamic)]
        public string Biodynamic { get; set; }

        [ExcelColumn(ExcelExportColumnNames.CheeseAttributeMilkType, ExcelExportColumnWidths.CheeseAttributeMilkType)]
        public string CheeseAttributeMilkType { get; set; }

        [ExcelColumn(ExcelExportColumnNames.CheeseAttributeRaw, ExcelExportColumnWidths.CheeseAttributeRaw)]
        public string CheeseAttributeRaw { get; set; }

        [ExcelColumn(ExcelExportColumnNames.DryAged, ExcelExportColumnWidths.DryAged)]
        public string DryAged { get; set; }

        [ExcelColumn(ExcelExportColumnNames.EcoScaleRating, ExcelExportColumnWidths.EcoScaleRating)]
        public string EcoScaleRating { get; set; }

        [ExcelColumn(ExcelExportColumnNames.FreeRange, ExcelExportColumnWidths.FreeRange)]
        public string FreeRange { get; set; }

        [ExcelColumn(ExcelExportColumnNames.SeafoodFreshOrFrozen, ExcelExportColumnWidths.SeafoodFreshOrFrozen)]
        public string SeafoodFreshOrFrozen { get; set; }

        [ExcelColumn(ExcelExportColumnNames.GlutenFree, ExcelExportColumnWidths.GlutenFree)]
        public string GlutenFree { get; set; }

        [ExcelColumn(ExcelExportColumnNames.GrassFed, ExcelExportColumnWidths.GrassFed)]
        public string GrassFed { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Kosher, ExcelExportColumnWidths.Kosher)]
        public string Kosher { get; set; }

        [ExcelColumn(ExcelExportColumnNames.MadeInHouse, ExcelExportColumnWidths.MadeInHouse)]
        public string MadeInHouse { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Msc, ExcelExportColumnWidths.Msc)]
        public string Msc { get; set; }

        [ExcelColumn(ExcelExportColumnNames.NonGmo, ExcelExportColumnWidths.NonGmo)]
        public string NonGmo { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Organic, ExcelExportColumnWidths.Organic)]
        public string Organic { get; set; }

        [ExcelColumn(ExcelExportColumnNames.PastureRaised, ExcelExportColumnWidths.PastureRaised)]
        public string PastureRaised { get; set; }

        [ExcelColumn(ExcelExportColumnNames.PremiumBodyCare, ExcelExportColumnWidths.PremiumBodyCare)]
        public string PremiumBodyCare { get; set; }

        [ExcelColumn(ExcelExportColumnNames.SeafoodWildOrFarmRaised, ExcelExportColumnWidths.SeafoodWildOrFarmRaised)]
        public string SeafoodWildOrFarmRaised { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Vegan, ExcelExportColumnWidths.Vegan)]
        public string Vegan { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Vegetarian, ExcelExportColumnWidths.Vegetarian)]
        public string Vegetarian { get; set; }

        [ExcelColumn(ExcelExportColumnNames.WholeTrade, ExcelExportColumnWidths.WholeTrade)]
        public string WholeTrade { get; set; }

        [ExcelColumn(ExcelExportColumnNames.CaseinFree, ExcelExportColumnWidths.CaseinFree)]
        public string CaseinFree { get; set; }

        [ExcelColumn(ExcelExportColumnNames.DrainedWeight, ExcelExportColumnWidths.DrainedWeight)]
        public string DrainedWeight { get; set; }

        [ExcelColumn(ExcelExportColumnNames.DrainedWeightUom, ExcelExportColumnWidths.DrainedWeightUom)]
        public string DrainedWeightUom { get; set; }

        [ExcelColumn(ExcelExportColumnNames.FairTradeCertified, ExcelExportColumnWidths.FairTradeCertified)]
        public string FairTradeCertified { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Hemp, ExcelExportColumnWidths.Hemp)]
        public string Hemp { get; set; }

        [ExcelColumn(ExcelExportColumnNames.LocalLoanProducer, ExcelExportColumnWidths.LocalLoanProducer)]
        public string LocalLoanProducer { get; set; }

        [ExcelColumn(ExcelExportColumnNames.MainProductName, ExcelExportColumnWidths.MainProductName)]
        public string MainProductName { get; set; }

        [ExcelColumn(ExcelExportColumnNames.NutritionRequired, ExcelExportColumnWidths.NutritionRequired)]
        public string NutritionRequired { get; set; }

        [ExcelColumn(ExcelExportColumnNames.OrganicPersonalCare, ExcelExportColumnWidths.OrganicPersonalCare)]
        public string OrganicPersonalCare { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Paleo, ExcelExportColumnWidths.Paleo)]
        public string Paleo { get; set; }
        
        [ExcelColumn(ExcelExportColumnNames.ProductFlavorType, ExcelExportColumnWidths.ProductFlavorType)]
        public string ProductFlavorType { get; set; }
    }
}