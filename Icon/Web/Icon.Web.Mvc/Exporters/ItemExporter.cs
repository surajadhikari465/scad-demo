using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Exporters
{
    public class ItemExporter : BaseItemExporter<ItemViewModel>
    {
        public ItemExporter(
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

            var exportItemData = ConvertExportDataToExportItemModel();

            // Start at 1 to exclude the header row.
            int i = 1;
            foreach (ExportItemModel item in exportItemData)
            {
                foreach (var column in spreadsheetColumns)
                {
                    column.SetValue(itemsWorksheet.Rows[i], item);
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
                Brand = brandHierarchyClassDictionary.ContainsKey(i.BrandHierarchyClassId.ToString()) ? brandHierarchyClassDictionary[i.BrandHierarchyClassId.ToString()] : String.Empty,
                ProductDescription = i.ProductDescription,
                PosDescription = i.PosDescription,
                PackageUnit = i.PackageUnit,
                FoodStampEligible = i.FoodStampEligible.BoolToYesOrNo(),
                PosScaleTare = i.PosScaleTare,
                RetailSize = i.RetailSize,
                RetailUom = i.RetailUom,
                DeliverySystem = i.DeliverySystem,
                Merchandise = merchandiseHierarchyClassDictionary.ContainsKey(i.MerchandiseHierarchyClassId.ToString()) ? merchandiseHierarchyClassDictionary[i.MerchandiseHierarchyClassId.ToString()] : String.Empty,
                Tax = taxHierarchyClassesDictionary.ContainsKey(i.TaxHierarchyClassId.ToString()) ? taxHierarchyClassesDictionary[i.TaxHierarchyClassId.ToString()] : String.Empty,
                Browsing = browsingHierarchyClassDictionary.ContainsKey(i.BrowsingHierarchyClassId.ToString()) ? browsingHierarchyClassDictionary[i.BrowsingHierarchyClassId.ToString()] : String.Empty,
                National = nationalHierarchyClassDictionary.ContainsKey(i.NationalHierarchyClassId.ToString()) ? nationalHierarchyClassDictionary[i.NationalHierarchyClassId.ToString()] : String.Empty,
                IsValidated = i.IsValidated.BoolToYesOrNo(),
                HiddenItem = i.HiddenItem.BoolToYesOrNo(),
                DepartmentSale = i.DepartmentSale.BoolToYesOrNo(),
                Notes = i.Notes,
                AnimalWelfareRating = i.AnimalWelfareRating,
                Biodynamic = i.Biodynamic.ToSpreadsheetBoolean(),
                CheeseAttributeMilkType =  i.CheeseMilkType,
                CheeseAttributeRaw = i.CheeseRaw.ToSpreadsheetBoolean(),
                EcoScaleRating =i.EcoScaleRating,
                GlutenFree = i.GlutenFreeAgency,
                Kosher = i.KosherAgency,
                Msc = i.Msc.ToSpreadsheetBoolean(),
                NonGmo =i.NonGmoAgency,
                Organic = i.OrganicAgency,
                PremiumBodyCare = i.PremiumBodyCare.ToSpreadsheetBoolean(),
                SeafoodFreshOrFrozen =  i.SeafoodFreshOrFrozen,
                SeafoodWildOrFarmRaised = i.SeafoodCatchType,
                Vegan = i.VeganAgency,
                Vegetarian = i.Vegetarian.ToSpreadsheetBoolean(),
                WholeTrade = i.WholeTrade.ToSpreadsheetBoolean(),
                GrassFed = i.GrassFed.ToSpreadsheetBoolean(),
                PastureRaised = i.PastureRaised.ToSpreadsheetBoolean(),
                FreeRange = i.FreeRange.ToSpreadsheetBoolean(),
                DryAged = i.DryAged.ToSpreadsheetBoolean(),
                AirChilled = i.AirChilled.ToSpreadsheetBoolean(),
                MadeInHouse = i.MadeInHouse.ToSpreadsheetBoolean(),
                AlcoholByVolume = i.AlcoholByVolume?.ToString(),
                CaseinFree = i.CaseinFree.ToSpreadsheetBoolean(),
                FairTradeCertified = i.FairTradeCertified,
                Hemp = i.Hemp.ToSpreadsheetBoolean(),
                OrganicPersonalCare = i.OrganicPersonalCare.ToSpreadsheetBoolean(),
                Paleo = i.Paleo.ToSpreadsheetBoolean(),
                LocalLoanProducer = i.LocalLoanProducer.ToSpreadsheetBoolean(),
                NutritionRequired = i.NutritionRequired.ToSpreadsheetBoolean(),
                MainProductName = i.MainProductName,
                ProductFlavorType = i.ProductFlavorType,
                DrainedWeight = i.DrainedWeight?.ToString(),
                DrainedWeightUom = i.DrainedWeightUom,
                CreatedDate = i.CreatedDate,
                LastModifiedDate = i.LastModifiedDate,
                LastModifiedUser = i.LastModifiedUser
            })
            .ToList();

            return exportItems;
        }

        protected override void CreateHierarchyExcelValidationRules()
        {
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Brands, base.brandHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.BrandColumnIndex, base.ExportData.Count);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Merchandise, base.merchandiseHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.MerchandiseColumnIndex, base.ExportData.Count);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Tax, base.taxHierarchyClassesDictionary.Values.Count, firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.TaxColumnIndex, base.ExportData.Count);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.Browsing, base.browsingHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.BrowsingColumnIndex, base.ExportData.Count);
            base.CreateHierarchyExcelValidationRule(HierarchyNames.National, base.nationalHierarchyClassDictionary.Values.Count, firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.NationalColumnIndex, base.ExportData.Count);
        }

        protected override void CreateCustomExcelValidationRules()
        {
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.FoodStampEligibleColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Food Stamp Eligible", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.DepartmentSaleColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Department Sale", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.ValidatedColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Validated", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.HiddenItemColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Hidden Item", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.AnimalWelfareRatingColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Animal Welfare Rating", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.BiodynamicColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Biodynamic", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Cheese Attribute: Milk Type", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeRawColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Cheese Attribute: Raw", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.EcoScaleRatingColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Eco-Scale Rating", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.PremiumBodyCareColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Premium Body Care", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Fresh Or Frozen", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Seafood: Wild Or Farm Raised", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.VegetarianColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Vegetarian", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.WholeTradeColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Whole Trade", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.GrassFedColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Grass Fed", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.PastureRaisedColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Pasture Raised", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.FreeRangeColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Free Range", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.DryAgedColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Dry Aged", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.AirChilledColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Air Chilled", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.MadeInHouseColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Made In House", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.MscColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("MSC", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.DrainedWeightUomColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Drained Weight Uom", includeRemoveOption: true));
            base.CreateCustomExcelValidationRule(firstRow, ExcelHelper.ConsolidatedItemColumnIndexes.FairTradeCertifiedColumnIndex, base.ExportData.Count, ExcelHelper.GetExcelValidationValues("Fair Trade Certified", includeRemoveOption: true));
        }

        private void AddSpreadsheetColumns()
        {
            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.ScanCodeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.ScanCode,
                ExcelHelper.ExcelExportColumnWidths.ScanCode,
                HorizontalCellAlignment.Right,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ScanCodeColumnIndex].Value = item.ScanCode);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.BrandColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Brand,
                ExcelHelper.ExcelExportColumnWidths.Brand,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BrandColumnIndex].Value = item.Brand);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.ProductDescriptionColumnIndex,
                ExcelHelper.ExcelExportColumnNames.ProductDescription,
                ExcelHelper.ExcelExportColumnWidths.ProductDescription,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ProductDescriptionColumnIndex].Value = item.ProductDescription);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.PosDescriptionColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PosDescription,
                ExcelHelper.ExcelExportColumnWidths.PosDescription,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PosDescriptionColumnIndex].Value = item.PosDescription);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.PackageUnitColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PackageUnit,
                ExcelHelper.ExcelExportColumnWidths.PackageUnit,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PackageUnitColumnIndex].Value = item.PackageUnit);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.FoodStampEligibleColumnIndex,
                ExcelHelper.ExcelExportColumnNames.FoodStampEligible,
                ExcelHelper.ExcelExportColumnWidths.FoodStampEligible,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.FoodStampEligibleColumnIndex].Value = item.FoodStampEligible);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.PosScaleTareColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PosScaleTare,
                ExcelHelper.ExcelExportColumnWidths.PosScaleTare,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PosScaleTareColumnIndex].Value = item.PosScaleTare);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.SizeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Size,
                ExcelHelper.ExcelExportColumnWidths.Size,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SizeColumnIndex].Value = item.RetailSize);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.UomColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Uom,
                ExcelHelper.ExcelExportColumnWidths.Uom,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.UomColumnIndex].Value = item.RetailUom);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.DeliverySystemColumnIndex,
                ExcelHelper.ExcelExportColumnNames.DeliverySystem,
                ExcelHelper.ExcelExportColumnWidths.DeliverySystem,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DeliverySystemColumnIndex].Value = item.DeliverySystem);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.MerchandiseColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Merchandise,
                ExcelHelper.ExcelExportColumnWidths.Merchandise,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MerchandiseColumnIndex].Value = item.Merchandise);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.NationalColumnIndex,
                ExcelHelper.ExcelExportColumnNames.NationalClass,
                ExcelHelper.ExcelExportColumnWidths.NationalClass,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.NationalColumnIndex].Value = item.National);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.TaxColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Tax,
                ExcelHelper.ExcelExportColumnWidths.Tax,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.TaxColumnIndex].Value = item.Tax);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.AlcoholByVolumeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.AlcoholByVolume,
                ExcelHelper.ExcelExportColumnWidths.AlcoholByVolume,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AlcoholByVolumeColumnIndex].Value = item.AlcoholByVolume);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.BrowsingColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Browsing,
                ExcelHelper.ExcelExportColumnWidths.Browsing,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BrowsingColumnIndex].Value = item.Browsing);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.ValidatedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Validated,
                ExcelHelper.ExcelExportColumnWidths.Validated,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ValidatedColumnIndex].Value = item.IsValidated);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.HiddenItemColumnIndex,
                ExcelHelper.ExcelExportColumnNames.HiddenItem,
                ExcelHelper.ExcelExportColumnWidths.HiddenItem,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.HiddenItemColumnIndex].Value = item.HiddenItem);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.DepartmentSaleColumnIndex,
                ExcelHelper.ExcelExportColumnNames.DepartmentSale,
                ExcelHelper.ExcelExportColumnWidths.DepartmentSale,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DepartmentSaleColumnIndex].Value = item.DepartmentSale);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.NotesColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Notes,
                ExcelHelper.ExcelExportColumnWidths.Notes,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.NotesColumnIndex].Value = item.Notes);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.AnimalWelfareRatingColumnIndex,
                ExcelHelper.ExcelExportColumnNames.AnimalWelfareRating,
                ExcelHelper.ExcelExportColumnWidths.AnimalWelfareRating,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value = item.AnimalWelfareRating);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.BiodynamicColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Biodynamic,
                ExcelHelper.ExcelExportColumnWidths.Biodynamic,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BiodynamicColumnIndex].Value = item.Biodynamic);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.CheeseAttributeMilkType,
                ExcelHelper.ExcelExportColumnWidths.CheeseAttributeMilkType,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value = item.CheeseAttributeMilkType);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeRawColumnIndex,
                ExcelHelper.ExcelExportColumnNames.CheeseAttributeRaw,
                ExcelHelper.ExcelExportColumnWidths.CheeseAttributeRaw,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeRawColumnIndex].Value = item.CheeseAttributeRaw);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.EcoScaleRatingColumnIndex,
                ExcelHelper.ExcelExportColumnNames.EcoScaleRating,
                ExcelHelper.ExcelExportColumnWidths.EcoScaleRating,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.EcoScaleRatingColumnIndex].Value = item.EcoScaleRating);

             AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.MscColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Msc,
                ExcelHelper.ExcelExportColumnWidths.Msc,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MscColumnIndex].Value = item.Msc);

       
            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.PremiumBodyCareColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PremiumBodyCare,
                ExcelHelper.ExcelExportColumnWidths.PremiumBodyCare,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PremiumBodyCareColumnIndex].Value = item.PremiumBodyCare);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex,
                ExcelHelper.ExcelExportColumnNames.SeafoodFreshOrFrozen,
                ExcelHelper.ExcelExportColumnWidths.SeafoodFreshOrFrozen,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value = item.SeafoodFreshOrFrozen);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.SeafoodWildOrFarmRaised,
                ExcelHelper.ExcelExportColumnWidths.SeafoodWildOrFarmRaised,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value = item.SeafoodWildOrFarmRaised);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.VegetarianColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Vegetarian,
                ExcelHelper.ExcelExportColumnWidths.Vegetarian,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.VegetarianColumnIndex].Value = item.Vegetarian);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.WholeTradeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.WholeTrade,
                ExcelHelper.ExcelExportColumnWidths.WholeTrade,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.WholeTradeColumnIndex].Value = item.WholeTrade);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.GrassFedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.GrassFed,
                ExcelHelper.ExcelExportColumnWidths.GrassFed,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.GrassFedColumnIndex].Value = item.GrassFed);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.PastureRaisedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.PastureRaised,
                ExcelHelper.ExcelExportColumnWidths.PastureRaised,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PastureRaisedColumnIndex].Value = item.PastureRaised);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.FreeRangeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.FreeRange,
                ExcelHelper.ExcelExportColumnWidths.FreeRange,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.FreeRangeColumnIndex].Value = item.FreeRange);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.DryAgedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.DryAged,
                ExcelHelper.ExcelExportColumnWidths.DryAged,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DryAgedColumnIndex].Value = item.DryAged);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.AirChilledColumnIndex,
                ExcelHelper.ExcelExportColumnNames.AirChilled,
                ExcelHelper.ExcelExportColumnWidths.AirChilled,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AirChilledColumnIndex].Value = item.AirChilled);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.MadeInHouseColumnIndex,
                ExcelHelper.ExcelExportColumnNames.MadeInHouse,
                ExcelHelper.ExcelExportColumnWidths.MadeInHouse,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MadeInHouseColumnIndex].Value = item.MadeInHouse);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.CaseinFreeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.CaseinFree,
                ExcelHelper.ExcelExportColumnWidths.CaseinFree,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.CaseinFreeColumnIndex].Value = item.CaseinFree);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.DrainedWeightColumnIndex,
                ExcelHelper.ExcelExportColumnNames.DrainedWeight,
                ExcelHelper.ExcelExportColumnWidths.DrainedWeight,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DrainedWeightColumnIndex].Value = item.DrainedWeight);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.DrainedWeightUomColumnIndex,
                ExcelHelper.ExcelExportColumnNames.DrainedWeightUom,
                ExcelHelper.ExcelExportColumnWidths.DrainedWeightUom,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DrainedWeightUomColumnIndex].Value = item.DrainedWeightUom);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.FairTradeCertifiedColumnIndex,
                ExcelHelper.ExcelExportColumnNames.FairTradeCertified,
                ExcelHelper.ExcelExportColumnWidths.FairTradeCertified,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.FairTradeCertifiedColumnIndex].Value = item.FairTradeCertified);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.HempColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Hemp,
                ExcelHelper.ExcelExportColumnWidths.Hemp,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.HempColumnIndex].Value = item.Hemp);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.LocalLoanProducerColumnIndex,
                ExcelHelper.ExcelExportColumnNames.LocalLoanProducer,
                ExcelHelper.ExcelExportColumnWidths.LocalLoanProducer,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.LocalLoanProducerColumnIndex].Value = item.LocalLoanProducer);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.MainProductNameColumnIndex,
                ExcelHelper.ExcelExportColumnNames.MainProductName,
                ExcelHelper.ExcelExportColumnWidths.MainProductName,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MainProductNameColumnIndex].Value = item.MainProductName);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.NutritionRequiredColumnIndex,
                ExcelHelper.ExcelExportColumnNames.NutritionRequired,
                ExcelHelper.ExcelExportColumnWidths.NutritionRequired,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.NutritionRequiredColumnIndex].Value = item.NutritionRequired);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.OrganicPersonalCareColumnIndex,
                ExcelHelper.ExcelExportColumnNames.OrganicPersonalCare,
                ExcelHelper.ExcelExportColumnWidths.OrganicPersonalCare,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.OrganicPersonalCareColumnIndex].Value = item.OrganicPersonalCare);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.PaleoColumnIndex,
                ExcelHelper.ExcelExportColumnNames.Paleo,
                ExcelHelper.ExcelExportColumnWidths.Paleo,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PaleoColumnIndex].Value = item.Paleo);

            AddSpreadsheetColumn(
                ExcelHelper.ConsolidatedItemColumnIndexes.ProductFlavorTypeColumnIndex,
                ExcelHelper.ExcelExportColumnNames.ProductFlavorType,
                ExcelHelper.ExcelExportColumnWidths.ProductFlavorType,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ProductFlavorTypeColumnIndex].Value = item.ProductFlavorType);

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