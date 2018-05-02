using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Utility;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Exporters
{
    public class BulkNewItemExporter : BaseIrmaItemExporter<BulkImportNewItemModel>
    {
        public BulkNewItemExporter(
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler, 
            IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>> getMerchTaxMappingQueryHandler,
            IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgenciesQueryHandler,
             IQueryHandler<GetBrandsParameters, List<BrandModel>> getBrandsQuery)
            : base(getHierarchyLineageQueryHandler, getMerchTaxMappingQueryHandler, getCertificationAgenciesQueryHandler, getBrandsQuery)
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

            var exportItemData = ConvertExportDataToExportItemModel();

            // Start at 1 to exclude the header row.
            int i = 1;
            foreach (var itemModel in exportItemData)
            {
                foreach (var column in base.spreadsheetColumns)
                {
                    column.SetValue(itemsWorksheet.Rows[i], itemModel);
                }

                i++;
            }

            CreateHierarchyExcelValidationRules();
            CreateCustomExcelValidationRules();
        }

        protected override List<ExportItemModel> ConvertExportDataToExportItemModel()
        {
            List<ExportItemModel> exportItems = ExportData.Select(i => new ExportItemModel
            {
                ScanCode = i.ScanCode,
                Brand = GetBrand(i),
                ProductDescription = i.ProductDescription,
                PosDescription = i.PosDescription,
                PackageUnit = i.PackageUnit,
                FoodStampEligible = i.FoodStampEligible.ToSpreadsheetBoolean(),
                PosScaleTare = i.PosScaleTare,
                RetailSize = i.RetailSize,
                RetailUom = i.RetailUom,
                DeliverySystem = i.DeliverySystem,
                IrmaSubTeamName = i.IrmaSubTeamName,
                Merchandise = merchandiseHierarchyClassDictionary.ContainsKey(i.MerchandiseId.ToString()) ? merchandiseHierarchyClassDictionary[i.MerchandiseId.ToString()] : String.Empty,
                Tax = taxHierarchyClassesDictionary.ContainsKey(i.TaxId.ToString()) ? taxHierarchyClassesDictionary[i.TaxId.ToString()] : String.Empty,
                National = nationalHierarchyClassDictionary.ContainsKey(i.NationalId.ToString()) ? nationalHierarchyClassDictionary[i.NationalId.ToString()] : String.Empty,
                Browsing = String.Empty,
                IsValidated = i.IsValidated.ToSpreadsheetBoolean(),
                RegionCode = i.RegionCode,
                AnimalWelfareRating = i.AnimalWelfareRating,
                Biodynamic = i.Biodynamic.ToSpreadsheetBoolean(),
                CheeseAttributeMilkType = i.CheeseAttributeMilkType,
                CheeseAttributeRaw = i.CheeseAttributeRaw.ToSpreadsheetBoolean(),
                EcoScaleRating = i.EcoScaleRating,
                GlutenFree = i.GlutenFreeAgencyLineage,
                Kosher = i.KosherAgencyLineage,
                Msc = i.Msc.ToSpreadsheetBoolean(),
                NonGmo = i.NonGmoAgencyLineage,
                Organic = i.OrganicAgencyLineage,
                PremiumBodyCare = i.PremiumBodyCare.ToSpreadsheetBoolean(),
                SeafoodFreshOrFrozen = i.SeafoodFreshOrFrozen,
                SeafoodWildOrFarmRaised = i.SeafoodWildOrFarmRaised,
                Vegan = i.VeganAgencyLineage,
                Vegetarian = i.Vegetarian.ToSpreadsheetBoolean(),
                WholeTrade = i.WholeTrade.ToSpreadsheetBoolean(),
                GrassFed = i.GrassFed.ToSpreadsheetBoolean(),
                PastureRaised = i.PastureRaised.ToSpreadsheetBoolean(),
                FreeRange = i.FreeRange.ToSpreadsheetBoolean(),
                DryAged = i.DryAged.ToSpreadsheetBoolean(),
                AirChilled = i.AirChilled.ToSpreadsheetBoolean(),
                MadeInHouse = i.MadeInHouse.ToSpreadsheetBoolean(),
                AlcoholByVolume = i.AlcoholByVolume,
                NutritionRequired = i.NutritionRequired.ToSpreadsheetBoolean(),
                Error = i.Error
            })
            .ToList();

            return exportItems;
        }

        private string GetBrand(BulkImportNewItemModel model)
        {
            // BrandID of 0 indicates a new brand.
            if (model.BrandId == "0")
            {
                if (!String.IsNullOrWhiteSpace(model.BrandLineage))
                {
                    return model.BrandLineage;
                }
                else if (!String.IsNullOrWhiteSpace(model.BrandName))
                {
                    return model.BrandName;
                }
                else
                {
                    return String.Empty;
                }
            }
            else
            {
                if (brandHierarchyClassDictionary.ContainsKey(model.BrandId))
                {
                    var brand = brandHierarchyClassDictionary[model.BrandId];
                    return String.Join("|", brand.HierarchyClassName, model.BrandId);
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        protected override void CreateHierarchyExcelValidationRules()
        {
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Merchandise, base.merchandiseHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex, base.ExportData.Count);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Tax, base.taxHierarchyClassesDictionary.Values.Count, firstRow, ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex, base.ExportData.Count);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Browsing, base.browsingHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex, base.ExportData.Count);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.National, base.nationalHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex, base.ExportData.Count);          
        }

        protected override void CreateCustomExcelValidationRules()
        {
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.FoodStampEligibleColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Food Stamp Eligible", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Validated", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Animal Welfare Rating", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Biodynamic", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Cheese Attribute: Milk Type", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Cheese Attribute: Raw", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Eco-Scale Rating", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Premium Body Care", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Fresh Or Frozen", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Seafood: Wild Or Farm Raised", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Vegetarian", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Whole Trade", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Grass Fed", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Pasture Raised", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Free Range", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Dry Aged", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Air Chilled", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Made In House", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.MscColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("MSC", includeRemoveOption: false));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.IrmaItemColumnIndexes.NutritionRequiredIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Nutrition Required", includeRemoveOption: false));
        }

        private void AddSpreadsheetColumns()
        {
            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.ScanCodeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.ScanCode,
                ExcelHelper.ExcelExportColumnWidths.ScanCode,
                HorizontalCellAlignment.Right,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.ScanCodeColumnIndex].Value = item.ScanCode);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.BrandColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Brand,
                ExcelHelper.ExcelExportColumnWidths.Brand,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.BrandColumnIndex].Value = item.Brand);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.ProductDescriptionColumnIndex,
                ExcelHelper.ExcelExportColumnNames.ProductDescription,
                ExcelHelper.ExcelExportColumnWidths.ProductDescription,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.ProductDescriptionColumnIndex].Value = item.ProductDescription);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.PosDescriptionColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PosDescription,
                ExcelHelper.ExcelExportColumnWidths.PosDescription,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.PosDescriptionColumnIndex].Value = item.PosDescription);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.PackageUnitColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PackageUnit,
                ExcelHelper.ExcelExportColumnWidths.PackageUnit,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.PackageUnitColumnIndex].Value = item.PackageUnit);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.FoodStampEligibleColumnIndex,
                ExcelHelper.ExcelExportColumnNames.FoodStampEligible,
                ExcelHelper.ExcelExportColumnWidths.FoodStampEligible,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.FoodStampEligibleColumnIndex].Value = item.FoodStampEligible);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.PosScaleTareColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PosScaleTare,
                ExcelHelper.ExcelExportColumnWidths.PosScaleTare,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.PosScaleTareColumnIndex].Value = item.PosScaleTare);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.SizeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Size,
                ExcelHelper.ExcelExportColumnWidths.Size,
                HorizontalCellAlignment.Left, 
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.SizeColumnIndex].Value = item.RetailSize);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.UomColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Uom,
                ExcelHelper.ExcelExportColumnWidths.Uom,
                HorizontalCellAlignment.Left, 
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.UomColumnIndex].Value = item.RetailUom);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.DeliverySystemColumnIndex,
                ExcelHelper.ExcelExportColumnNames.DeliverySystem,
                ExcelHelper.ExcelExportColumnWidths.DeliverySystem,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.DeliverySystemColumnIndex].Value = item.DeliverySystem);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.IrmaSubTeamColumnIndex,
                ExcelHelper.ExcelExportColumnNames.IrmaSubTeam,
                ExcelHelper.ExcelExportColumnWidths.IrmaSubTeam,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.IrmaSubTeamColumnIndex].Value = item.IrmaSubTeamName);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Merchandise,
                ExcelHelper.ExcelExportColumnWidths.Merchandise,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex].Value = item.Merchandise);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex,
                ExcelHelper.ExcelExportColumnNames.NationalClass,
                ExcelHelper.ExcelExportColumnWidths.NationalClass,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex].Value = item.National);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Tax,
                ExcelHelper.ExcelExportColumnWidths.Tax,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex].Value = item.Tax);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Browsing,
                ExcelHelper.ExcelExportColumnWidths.Browsing,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex].Value = item.Browsing);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Validated,
                ExcelHelper.ExcelExportColumnWidths.Validated,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex].Value = item.IsValidated);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.RegionCodeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Region,
                ExcelHelper.ExcelExportColumnWidths.Region,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.RegionCodeColumnIndex].Value = item.RegionCode);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex,
                ExcelHelper.ExcelExportColumnNames.AnimalWelfareRating,
                ExcelHelper.ExcelExportColumnWidths.AnimalWelfareRating,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value = item.AnimalWelfareRating);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Biodynamic,
                ExcelHelper.ExcelExportColumnWidths.Biodynamic,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex].Value = item.Biodynamic);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.CheeseAttributeMilkType,
                ExcelHelper.ExcelExportColumnWidths.CheeseAttributeMilkType,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value = item.CheeseAttributeMilkType);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex,
                ExcelHelper.ExcelExportColumnNames.CheeseAttributeRaw,
                ExcelHelper.ExcelExportColumnWidths.CheeseAttributeRaw,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex].Value = item.CheeseAttributeRaw);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex,
                ExcelHelper.ExcelExportColumnNames.EcoScaleRating,
                ExcelHelper.ExcelExportColumnWidths.EcoScaleRating,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex].Value = item.EcoScaleRating);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PremiumBodyCare,
                ExcelHelper.ExcelExportColumnWidths.PremiumBodyCare,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex].Value = item.PremiumBodyCare);


            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex,
                ExcelHelper.ExcelExportColumnNames.SeafoodFreshOrFrozen,
                ExcelHelper.ExcelExportColumnWidths.SeafoodFreshOrFrozen,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value = item.SeafoodFreshOrFrozen);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.SeafoodWildOrFarmRaised,
                ExcelHelper.ExcelExportColumnWidths.SeafoodWildOrFarmRaised,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value = item.SeafoodWildOrFarmRaised);

           
            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Vegetarian,
                ExcelHelper.ExcelExportColumnWidths.Vegetarian,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex].Value = item.Vegetarian);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.WholeTrade,
                ExcelHelper.ExcelExportColumnWidths.WholeTrade,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex].Value = item.WholeTrade);

            AddSpreadsheetColumn(
               ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex,
               ExcelHelper.ExcelExportColumnNames.GrassFed,
               ExcelHelper.ExcelExportColumnWidths.GrassFed,
               HorizontalCellAlignment.Left,
               (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex].Value = item.GrassFed);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PastureRaised,
                ExcelHelper.ExcelExportColumnWidths.PastureRaised,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex].Value = item.PastureRaised);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.FreeRange,
                ExcelHelper.ExcelExportColumnWidths.FreeRange,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex].Value = item.FreeRange);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.DryAged,
                ExcelHelper.ExcelExportColumnWidths.DryAged,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex].Value = item.DryAged);

            AddSpreadsheetColumn(
               ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex,
               ExcelHelper.ExcelExportColumnNames.AirChilled,
               ExcelHelper.ExcelExportColumnWidths.AirChilled,
               HorizontalCellAlignment.Left,
               (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex].Value = item.AirChilled);

            AddSpreadsheetColumn(
               ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex,
               ExcelHelper.ExcelExportColumnNames.MadeInHouse,
               ExcelHelper.ExcelExportColumnWidths.MadeInHouse,
               HorizontalCellAlignment.Left,
               (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex].Value = item.MadeInHouse);

            AddSpreadsheetColumn(
                ExcelHelper.IrmaItemColumnIndexes.ErrorColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Error,
                ExcelHelper.ExcelExportColumnWidths.Error,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.ErrorColumnIndex].Value = item.Error);

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
               (row, item) => row.Cells[ExcelHelper.IrmaItemColumnIndexes.NutritionRequiredIndex].Value = item.NutritionRequired);
        }
    }
}
