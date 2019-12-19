using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Icon.Web.Mvc.Excel.ExcelHelper;

namespace Icon.Web.Mvc.Excel.Columns
{
    public static class NewItemColumns
    {
        public static IReadOnlyCollection<string> Columns
        {
            get
            {
                return new ReadOnlyCollection<string>(new List<string>
                {
                    ExcelExportColumnNames.ScanCode,
                    ExcelExportColumnNames.Brand,
                    ExcelExportColumnNames.ProductDescription,
                    ExcelExportColumnNames.PosDescription,
                    ExcelExportColumnNames.PackageUnit,
                    ExcelExportColumnNames.FoodStampEligible,
                    ExcelExportColumnNames.PosScaleTare,
                    ExcelExportColumnNames.Size,
                    ExcelExportColumnNames.Uom,
                    ExcelExportColumnNames.DeliverySystem,
                    ExcelExportColumnNames.IrmaSubTeam,
                    ExcelExportColumnNames.Merchandise,
                    ExcelExportColumnNames.Tax,
                    ExcelExportColumnNames.AlcoholByVolume,
                    ExcelExportColumnNames.NationalClass,
                    ExcelExportColumnNames.Browsing,
                    ExcelExportColumnNames.Validated,
                    ExcelExportColumnNames.Region,
                    ExcelExportColumnNames.AirChilled,
                    ExcelExportColumnNames.AnimalWelfareRating,
                    ExcelExportColumnNames.Biodynamic,
                    ExcelExportColumnNames.CheeseAttributeMilkType,
                    ExcelExportColumnNames.CheeseAttributeRaw,
                    ExcelExportColumnNames.DryAged,
                    ExcelExportColumnNames.EcoScaleRating,
                    ExcelExportColumnNames.FreeRange,
                    ExcelExportColumnNames.SeafoodFreshOrFrozen,
                    ExcelExportColumnNames.GlutenFree,
                    ExcelExportColumnNames.GrassFed,
                    ExcelExportColumnNames.Kosher,
                    ExcelExportColumnNames.MadeInHouse,
                    ExcelExportColumnNames.Msc,
                    ExcelExportColumnNames.NonGmo,
                    ExcelExportColumnNames.Organic,
                    ExcelExportColumnNames.PastureRaised,
                    ExcelExportColumnNames.PremiumBodyCare,
                    ExcelExportColumnNames.SeafoodWildOrFarmRaised,
                    ExcelExportColumnNames.Vegan,
                    ExcelExportColumnNames.Vegetarian,
                    ExcelExportColumnNames.WholeTrade,
                    ExcelExportColumnNames.CaseinFree,
                    ExcelExportColumnNames.DrainedWeight,
                    ExcelExportColumnNames.DrainedWeightUom,
                    ExcelExportColumnNames.FairTradeCertified,
                    ExcelExportColumnNames.Hemp,
                    ExcelExportColumnNames.LocalLoanProducer,
                    ExcelExportColumnNames.MainProductName,
                    ExcelExportColumnNames.NutritionRequired,
                    ExcelExportColumnNames.OrganicPersonalCare,
                    ExcelExportColumnNames.Paleo,
                    ExcelExportColumnNames.ProductFlavorType
                });
            }
        }
    }
}