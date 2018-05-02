using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Exporters
{
    public class ItemTemplateExporter : BaseItemExporter<ItemViewModel>
    {
        private const int lastRow = 1000;

        public ItemTemplateExporter(
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler,
            IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>> getmerchTaxMappingQuerryHandler,
            IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgenciesQueryHandler)
            : base(getHierarchyLineageQueryHandler, getmerchTaxMappingQuerryHandler, getCertificationAgenciesQueryHandler)
        {
            base.brandColumnPosition = GetColumnPosition(ExcelHelper.ConsolidatedItemColumnIndexes.BrandColumnIndex);
            base.merchandiseColumnPosition = GetColumnPosition(ExcelHelper.ConsolidatedItemColumnIndexes.MerchandiseColumnIndex);
            base.taxColumnPosition = GetColumnPosition(ExcelHelper.ConsolidatedItemColumnIndexes.TaxColumnIndex);
            base.nationalClassColumnPosition = GetColumnPosition(ExcelHelper.ConsolidatedItemColumnIndexes.NationalColumnIndex);
            base.browsingColumnPosition = GetColumnPosition(ExcelHelper.ConsolidatedItemColumnIndexes.BrowsingColumnIndex);

            AddSpreadsheetColumns();
        }

        public override void Export()
        {
            base.BuildSpreadsheet();
            base.FormatExcelColumns(ExcelHelper.ConsolidatedItemColumnIndexes.ScanCodeColumnIndex);

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
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Brands, base.brandHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.BrandColumnIndex, lastRow - 1);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Merchandise, base.merchandiseHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.MerchandiseColumnIndex, lastRow - 1);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Tax, base.taxHierarchyClassesDictionary.Values.Count, firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.TaxColumnIndex, lastRow - 1);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Browsing, base.browsingHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.BrowsingColumnIndex, lastRow - 1);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.National, base.nationalHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.NationalColumnIndex, lastRow - 1);
        }

        protected override void CreateCustomExcelValidationRules()
        {
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.FoodStampEligibleColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Food Stamp Eligible", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.DepartmentSaleColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Department Sale", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.ValidatedColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Validated", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.HiddenItemColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Hidden Item", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.AnimalWelfareRatingColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Animal Welfare Rating", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.BiodynamicColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Biodynamic", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Cheese Attribute: Milk Type", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeRawColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Cheese Attribute: Raw", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.EcoScaleRatingColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Eco-Scale Rating", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.PremiumBodyCareColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Premium Body Care", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Fresh Or Frozen", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Seafood: Wild Or Farm Raised", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.VegetarianColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Vegetarian", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.WholeTradeColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Whole Trade", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.GrassFedColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Grass Fed", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.PastureRaisedColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Pasture Raised", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.FreeRangeColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Free Range", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.DryAgedColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Dry Aged", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.AirChilledColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Air Chilled", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.MadeInHouseColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("Made In House", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.MscColumnIndex, lastRow - 1, ExcelHelper.GetExcelValidationValues("MSC", includeRemoveOption: true));
            
        }
       
        private void AddSpreadsheetColumns()
        {
            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.ScanCodeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.ScanCode,
                ExcelHelper.ExcelExportColumnWidths.ScanCode,
                HorizontalCellAlignment.Right,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ScanCodeColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.BrandColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Brand,
                ExcelHelper.ExcelExportColumnWidths.Brand,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BrandColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.ProductDescriptionColumnIndex,
                ExcelHelper.ExcelExportColumnNames.ProductDescription,
                ExcelHelper.ExcelExportColumnWidths.ProductDescription,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ProductDescriptionColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.PosDescriptionColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PosDescription,
                ExcelHelper.ExcelExportColumnWidths.PosDescription,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PosDescriptionColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.PackageUnitColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PackageUnit,
                ExcelHelper.ExcelExportColumnWidths.PackageUnit,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PackageUnitColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.FoodStampEligibleColumnIndex,
                ExcelHelper.ExcelExportColumnNames.FoodStampEligible,
                ExcelHelper.ExcelExportColumnWidths.FoodStampEligible,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.FoodStampEligibleColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.PosScaleTareColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PosScaleTare,
                ExcelHelper.ExcelExportColumnWidths.PosScaleTare,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PosScaleTareColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.SizeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Size,
                ExcelHelper.ExcelExportColumnWidths.Size,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SizeColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.UomColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Uom,
                ExcelHelper.ExcelExportColumnWidths.Uom,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.UomColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.DeliverySystemColumnIndex,
                ExcelHelper.ExcelExportColumnNames.DeliverySystem,
                ExcelHelper.ExcelExportColumnWidths.DeliverySystem,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DeliverySystemColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.MerchandiseColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Merchandise,
                ExcelHelper.ExcelExportColumnWidths.Merchandise,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MerchandiseColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.NationalColumnIndex,
                ExcelHelper.ExcelExportColumnNames.NationalClass,
                ExcelHelper.ExcelExportColumnWidths.NationalClass,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.NationalColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.TaxColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Tax,
                ExcelHelper.ExcelExportColumnWidths.Tax,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.TaxColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.BrowsingColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Browsing,
                ExcelHelper.ExcelExportColumnWidths.Browsing,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BrowsingColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.ValidatedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Validated,
                ExcelHelper.ExcelExportColumnWidths.Validated,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ValidatedColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.HiddenItemColumnIndex,
                ExcelHelper.ExcelExportColumnNames.HiddenItem,
                ExcelHelper.ExcelExportColumnWidths.HiddenItem,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.HiddenItemColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.DepartmentSaleColumnIndex,
                ExcelHelper.ExcelExportColumnNames.DepartmentSale,
                ExcelHelper.ExcelExportColumnWidths.DepartmentSale,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DepartmentSaleColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.NotesColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Notes,
                ExcelHelper.ExcelExportColumnWidths.Notes,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.NotesColumnIndex].Value = String.Empty);


            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.AnimalWelfareRatingColumnIndex,
                ExcelHelper.ExcelExportColumnNames.AnimalWelfareRating,
                ExcelHelper.ExcelExportColumnWidths.AnimalWelfareRating,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.BiodynamicColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Biodynamic,
                ExcelHelper.ExcelExportColumnWidths.Biodynamic,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BiodynamicColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.CheeseAttributeMilkType,
                ExcelHelper.ExcelExportColumnWidths.CheeseAttributeMilkType,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeRawColumnIndex,
                ExcelHelper.ExcelExportColumnNames.CheeseAttributeRaw,
                ExcelHelper.ExcelExportColumnWidths.CheeseAttributeRaw,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeRawColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.EcoScaleRatingColumnIndex,
                ExcelHelper.ExcelExportColumnNames.EcoScaleRating,
                ExcelHelper.ExcelExportColumnWidths.EcoScaleRating,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.EcoScaleRatingColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.MscColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Msc,
                ExcelHelper.ExcelExportColumnWidths.Msc,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MscColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.PremiumBodyCareColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PremiumBodyCare,
                ExcelHelper.ExcelExportColumnWidths.PremiumBodyCare,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PremiumBodyCareColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex,
                ExcelHelper.ExcelExportColumnNames.SeafoodFreshOrFrozen,
                ExcelHelper.ExcelExportColumnWidths.SeafoodFreshOrFrozen,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.SeafoodWildOrFarmRaised,
                ExcelHelper.ExcelExportColumnWidths.SeafoodWildOrFarmRaised,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.VegetarianColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Vegetarian,
                ExcelHelper.ExcelExportColumnWidths.Vegetarian,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.VegetarianColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.WholeTradeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.WholeTrade,
                ExcelHelper.ExcelExportColumnWidths.WholeTrade,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.WholeTradeColumnIndex].Value = String.Empty);           

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.GrassFedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.GrassFed,
                ExcelHelper.ExcelExportColumnWidths.GrassFed,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.GrassFedColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.PastureRaisedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PastureRaised,
                ExcelHelper.ExcelExportColumnWidths.PastureRaised,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PastureRaisedColumnIndex].Value = String.Empty);
           
            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.FreeRangeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.FreeRange,
                ExcelHelper.ExcelExportColumnWidths.FreeRange,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.FreeRangeColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.DryAgedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.DryAged,
                ExcelHelper.ExcelExportColumnWidths.DryAged,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DryAgedColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
               ExcelHelper.ConsolidatedItemColumnIndexes.AirChilledColumnIndex,
               ExcelHelper.ExcelExportColumnNames.AirChilled,
               ExcelHelper.ExcelExportColumnWidths.AirChilled,
               HorizontalCellAlignment.Left,
               (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AirChilledColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
               ExcelHelper.ConsolidatedItemColumnIndexes.MadeInHouseColumnIndex,
               ExcelHelper.ExcelExportColumnNames.MadeInHouse,
               ExcelHelper.ExcelExportColumnWidths.MadeInHouse,
               HorizontalCellAlignment.Left,
               (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MadeInHouseColumnIndex].Value = String.Empty);

            AddSpreadsheetColumn(
              ExcelHelper.ConsolidatedItemColumnIndexes.CreatedDateColumnIndex,
              ExcelHelper.ExcelExportColumnNames.CreatedDate,
              ExcelHelper.ExcelExportColumnWidths.CreatedDate,
              HorizontalCellAlignment.Left,
              (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.CreatedDateColumnIndex].Value = item.CreatedDate);

            AddSpreadsheetColumn(
               ExcelHelper.ConsolidatedItemColumnIndexes.LastModifiedDateColumnIndex,
               ExcelHelper.ExcelExportColumnNames.LastModifiedDate,
               ExcelHelper.ExcelExportColumnWidths.LastModifiedDate,
               HorizontalCellAlignment.Left,
               (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.LastModifiedDateColumnIndex].Value = item.LastModifiedDate);

            AddSpreadsheetColumn(
               ExcelHelper.ConsolidatedItemColumnIndexes.LastModifiedUserColumnIndex,
               ExcelHelper.ExcelExportColumnNames.LastModifiedUser,
               ExcelHelper.ExcelExportColumnWidths.LastModifiedUser,
               HorizontalCellAlignment.Left,
               (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.LastModifiedUserColumnIndex].Value = item.LastModifiedUser);
        }
    }
}