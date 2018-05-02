using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Exporters
{
    public class NewItemTemplateExporter : BaseIrmaItemExporter<ExportItemModel>
    {
        private const int lastRow = 1000;

        public NewItemTemplateExporter(
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler,
            IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>> getMerchTaxMappingQuerryHandler,
            IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgenciesQueryHandler,
             IQueryHandler<GetBrandsParameters, List<BrandModel>> getBrandsQuery)
            : base(getHierarchyLineageQueryHandler, getMerchTaxMappingQuerryHandler, getCertificationAgenciesQueryHandler, getBrandsQuery)
        {
            base.merchandiseColumnPosition = GetColumnPosition(ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex);
            base.taxColumnPosition = GetColumnPosition(ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex);
            base.nationalClassColumnPosition = GetColumnPosition(ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex);
            base.browsingColumnPosition = GetColumnPosition(ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex);

            AddSpreadsheetColumns();
        }

        public override void Export()
        {
            base.BuildSpreadsheet();
            base.FormatExcelColumns(ExcelHelper.IrmaItemColumnIndexes.ScanCodeColumnIndex);

            CreateHierarchyExcelValidationRules();
            CreateCustomExcelValidationRules();
        }

        protected override List<ExportItemModel> ConvertExportDataToExportItemModel()
        {
            // A template won't contain any data to convert.
            return new List<ExportItemModel>();
        }

        protected override void CreateHierarchyExcelValidationRules()
        {
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Merchandise, base.merchandiseHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex, lastRow - 1);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Tax, base.taxHierarchyClassesDictionary.Values.Count, firstRow, ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex, lastRow - 1);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Browsing, base.browsingHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex, lastRow - 1);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.National, base.nationalHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex, lastRow - 1);         
        }

        protected override void CreateCustomExcelValidationRules()
        {
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.FoodStampEligibleColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Food Stamp Eligible", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Validated", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Animal Welfare Rating", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Biodynamic", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Cheese Attribute: Milk Type", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Cheese Attribute: Raw", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Eco-Scale Rating", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Premium Body Care", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Fresh Or Frozen", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Seafood: Wild Or Farm Raised", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Vegetarian", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Whole Trade", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Grass Fed", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Pasture Raised", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Free Range", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Dry Aged", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Air Chilled", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Made In House", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.MscColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("MSC", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.NutritionRequiredIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Nutrition Required", includeRemoveOption: false));
        }

        private void AddSpreadsheetColumns()
        {
            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.ScanCodeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.ScanCode,
                ExcelHelper.ExcelExportColumnWidths.ScanCode,
                HorizontalCellAlignment.Right,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.ScanCodeColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.BrandColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Brand,
                ExcelHelper.ExcelExportColumnWidths.Brand,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.BrandColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.ProductDescriptionColumnIndex,
                ExcelHelper.ExcelExportColumnNames.ProductDescription,
                ExcelHelper.ExcelExportColumnWidths.ProductDescription,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.ProductDescriptionColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.PosDescriptionColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PosDescription,
                ExcelHelper.ExcelExportColumnWidths.PosDescription,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.PosDescriptionColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.PackageUnitColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PackageUnit,
                ExcelHelper.ExcelExportColumnWidths.PackageUnit,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.PackageUnitColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.FoodStampEligibleColumnIndex,
                ExcelHelper.ExcelExportColumnNames.FoodStampEligible,
                ExcelHelper.ExcelExportColumnWidths.FoodStampEligible,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.FoodStampEligibleColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.PosScaleTareColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PosScaleTare,
                ExcelHelper.ExcelExportColumnWidths.PosScaleTare,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.PosScaleTareColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.SizeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Size,
                ExcelHelper.ExcelExportColumnWidths.Size,
                HorizontalCellAlignment.Left, 
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.SizeColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.UomColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Uom,
                ExcelHelper.ExcelExportColumnWidths.Uom,
                HorizontalCellAlignment.Left, 
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.UomColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.DeliverySystemColumnIndex,
                ExcelHelper.ExcelExportColumnNames.DeliverySystem,
                ExcelHelper.ExcelExportColumnWidths.DeliverySystem,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.DeliverySystemColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.IrmaSubTeamColumnIndex,
                ExcelHelper.ExcelExportColumnNames.IrmaSubTeam,
                ExcelHelper.ExcelExportColumnWidths.IrmaSubTeam,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.IrmaSubTeamColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Merchandise,
                ExcelHelper.ExcelExportColumnWidths.Merchandise,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex,
                ExcelHelper.ExcelExportColumnNames.NationalClass,
                ExcelHelper.ExcelExportColumnWidths.NationalClass,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Tax,
                ExcelHelper.ExcelExportColumnWidths.Tax,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Browsing,
                ExcelHelper.ExcelExportColumnWidths.Browsing,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Validated,
                ExcelHelper.ExcelExportColumnWidths.Validated,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.RegionCodeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Region,
                ExcelHelper.ExcelExportColumnWidths.Region,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.RegionCodeColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex,
                ExcelHelper.ExcelExportColumnNames.AnimalWelfareRating,
                ExcelHelper.ExcelExportColumnWidths.AnimalWelfareRating,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Biodynamic,
                ExcelHelper.ExcelExportColumnWidths.Biodynamic,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.CheeseAttributeMilkType,
                ExcelHelper.ExcelExportColumnWidths.CheeseAttributeMilkType,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex,
                ExcelHelper.ExcelExportColumnNames.CheeseAttributeRaw,
                ExcelHelper.ExcelExportColumnWidths.CheeseAttributeRaw,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex,
                ExcelHelper.ExcelExportColumnNames.EcoScaleRating,
                ExcelHelper.ExcelExportColumnWidths.EcoScaleRating,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex].Value = String.Empty);

           
            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PremiumBodyCare,
                ExcelHelper.ExcelExportColumnWidths.PremiumBodyCare,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex].Value = String.Empty);
            
            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex,
                ExcelHelper.ExcelExportColumnNames.SeafoodFreshOrFrozen,
                ExcelHelper.ExcelExportColumnWidths.SeafoodFreshOrFrozen,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.SeafoodWildOrFarmRaised,
                ExcelHelper.ExcelExportColumnWidths.SeafoodWildOrFarmRaised,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value = String.Empty);

        
            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Vegetarian,
                ExcelHelper.ExcelExportColumnWidths.Vegetarian,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.WholeTrade,
                ExcelHelper.ExcelExportColumnWidths.WholeTrade,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.GrassFed,
                ExcelHelper.ExcelExportColumnWidths.GrassFed,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PastureRaised,
                ExcelHelper.ExcelExportColumnWidths.PastureRaised,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.FreeRange,
                ExcelHelper.ExcelExportColumnWidths.FreeRange,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.DryAged,
                ExcelHelper.ExcelExportColumnWidths.DryAged,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
               ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex,
               ExcelHelper.ExcelExportColumnNames.AirChilled,
               ExcelHelper.ExcelExportColumnWidths.AirChilled,
               HorizontalCellAlignment.Left,
               (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
               ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex,
               ExcelHelper.ExcelExportColumnNames.MadeInHouse,
               ExcelHelper.ExcelExportColumnWidths.MadeInHouse,
               HorizontalCellAlignment.Left,
               (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
               ExcelHelper.IrmaItemColumnIndexes.AlcoholByVolumeIndex,
               ExcelHelper.ExcelExportColumnNames.AlcoholByVolume,
               ExcelHelper.ExcelExportColumnWidths.AlcoholByVolume,
               HorizontalCellAlignment.Left,
               (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.AlcoholByVolumeIndex].Value = item.AlcoholByVolume);

            AddSpreadsheetColumn(
               ExcelHelper.IrmaItemColumnIndexes.NutritionRequiredIndex,
               ExcelHelper.ExcelExportColumnNames.NutritionRequired,
               ExcelHelper.ExcelExportColumnWidths.NutritionRequired,
               HorizontalCellAlignment.Left,
               (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.NutritionRequiredIndex].Value = String.Empty);
        }
    }
}
