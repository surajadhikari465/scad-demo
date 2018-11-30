using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Importers
{
    public class ItemSpreadsheetImporter : BaseItemSpreadsheetImporter<BulkImportItemModel>
    {
        private IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>> getNewScanCodeUploadsQuery;
        private IQueryHandler<GetScanCodesNotReadyToValidateParameters, List<string>> getScanCodesNotReadyToValidateQuery;

        public ItemSpreadsheetImporter(
            IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>> getNewScanCodeUploadsQuery,
            IQueryHandler<GetScanCodesNotReadyToValidateParameters, List<string>> getScanCodesNotReadyToValidateQuery,
            IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>> getTaxHierarchyClassesWithNoAbbreviationQuery,
            IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>> getAffinitySubBricksQuery,
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQuery,
            IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgencyQuery)
            : base(getTaxHierarchyClassesWithNoAbbreviationQuery, getAffinitySubBricksQuery, getHierarchyLineageQuery, getCertificationAgencyQuery)
        {
            this.getNewScanCodeUploadsQuery = getNewScanCodeUploadsQuery;
            this.getScanCodesNotReadyToValidateQuery = getScanCodesNotReadyToValidateQuery;
        }

        public override void ConvertSpreadsheetToModel()
        {
            // Begin at 1 to skip the header row (object model is 0-indexed; an actual spreadsheet is 1-indexed).
            // Only import up to a certain number of rows per spreadsheet.  Default is Int32.Max.
            nonEmptyRows = Workbook.Worksheets[0].Rows
                .Skip(1)
                .Where(r => r.Cells.Any(c => c.Value != null && !String.IsNullOrWhiteSpace(c.Value.ToString())))
                .Take(maxNumberOfRowsToImport);

            string scanCode;
            string brand;
            string productDescription;
            string posDescription;
            string packageUnit;
            string foodStampEligible;
            string posScaleTare;
            string retailSize;
            string retailUom;
            string deliverySystem;
            string merchandise;
            string tax;
            string national;
            string browsing;
            string isValidated;
            string hiddenItem;
            string notes;
            string animalWelfareRating;
            string biodynamic;
            string cheeseAttributeMilkType;
            string cheeseAttributeRaw;
            string ecoScaleRating;
            string glutenFree;
            string kosher;
            string msc;
            string nonGmo;
            string organic;
            string premiumBodyCare;
            string seafoodFreshOrFrozen;
            string seafoodWildOrFarmRaised;
            string vegan;
            string vegetarian;
            string wholeTrade;
            string grassFed;
            string pastureRaised;
            string freeRange;
            string dryAged;
            string airChilled;
            string madeInHouse;
            string createdDate;
            string modifiedDate;
            string modifiedUser;
            string defaultTaxClass;
            string productionDescriptionDisplayOnly;
            string brandNameDisplayOnly;
            string merchandiseClassDisplayOnly;

            if (isTaxCorrectionsImport)
            {
                foreach (var row in nonEmptyRows)
                {
                    scanCode = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ScanCodeColumnIndex].Value);
                    brand = String.Empty;
                    productDescription = String.Empty;
                    posDescription = String.Empty;
                    packageUnit = String.Empty;
                    foodStampEligible = String.Empty;
                    posScaleTare = String.Empty;
                    retailSize = String.Empty;
                    retailUom = String.Empty;
                    deliverySystem = String.Empty;
                    merchandise = String.Empty;
                    tax = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.TaxClassOverrideColumnIndex].Value);
                    national = String.Empty;
                    browsing = String.Empty;
                    isValidated = String.Empty;
                    hiddenItem = String.Empty;
                    notes = String.Empty;
                    animalWelfareRating = String.Empty;
                    biodynamic = String.Empty;
                    cheeseAttributeMilkType = String.Empty;
                    cheeseAttributeRaw = String.Empty;
                    ecoScaleRating = String.Empty;
                    glutenFree = String.Empty;
                    kosher = String.Empty;
                    msc = String.Empty;
                    nonGmo = String.Empty;
                    organic = String.Empty;
                    premiumBodyCare = String.Empty;
                    seafoodFreshOrFrozen = String.Empty;
                    seafoodWildOrFarmRaised = String.Empty;
                    vegan = String.Empty;
                    vegetarian = String.Empty;
                    wholeTrade = String.Empty;
                    grassFed = String.Empty;
                    pastureRaised = String.Empty;
                    freeRange = String.Empty;
                    dryAged = String.Empty;
                    airChilled = String.Empty;
                    madeInHouse = String.Empty;
                    createdDate = String.Empty;
                    modifiedDate = String.Empty;
                    modifiedUser = String.Empty;
                    defaultTaxClass = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.DefaultTaxClassColumnIndex].Value);
                    productionDescriptionDisplayOnly = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ProductDescriptionColumnIndex].Value);
                    brandNameDisplayOnly = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.BrandColumnIndex].Value);
                    merchandiseClassDisplayOnly = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.MerchandiseLineage].Value);

                    var parsedRow = new BulkImportItemModel
                    {
                        ScanCode = scanCode,
                        BrandLineage = brand,
                        BrandName = brand.Split('|').First(),
                        BrandId = brand.GetIdFromCellText(),
                        ProductDescription = productDescription,
                        PosDescription = posDescription,
                        PackageUnit = packageUnit,
                        FoodStampEligible = foodStampEligible,
                        PosScaleTare = posScaleTare,
                        RetailSize = retailSize,
                        RetailUom = retailUom,
                        DeliverySystem = deliverySystem,
                        MerchandiseLineage = merchandise,
                        MerchandiseId = merchandise.GetIdFromCellText(),
                        NationalLineage = national,
                        NationalId = national.GetIdFromCellText(),
                        TaxLineage = tax,
                        TaxId = tax.GetIdFromCellText(),
                        BrowsingLineage = browsing,
                        BrowsingId = browsing.GetIdFromCellText(),
                        IsValidated = isValidated,
                        HiddenItem = hiddenItem,
                        DepartmentSale = String.Empty,
                        Notes = notes,
                        AnimalWelfareRating = animalWelfareRating,
                        Biodynamic = biodynamic == null ? String.Empty : biodynamic,
                        CheeseAttributeMilkType = cheeseAttributeMilkType,
                        CheeseAttributeRaw = cheeseAttributeRaw == null ? String.Empty : cheeseAttributeRaw,
                        EcoScaleRating = ecoScaleRating,
                        GlutenFreeAgencyLineage = glutenFree,
                        GlutenFreeAgency = glutenFree == Constants.ExcelImportRemoveValueKeyword ? "0" : glutenFree.GetIdFromCellText(),
                        KosherAgencyLineage = kosher,
                        KosherAgency = kosher == Constants.ExcelImportRemoveValueKeyword ? "0" : kosher.GetIdFromCellText(),
                        Msc = msc == null ? String.Empty : msc,
                        NonGmoAgencyLineage = nonGmo,
                        NonGmoAgency = nonGmo == Constants.ExcelImportRemoveValueKeyword ? "0" : nonGmo.GetIdFromCellText(),
                        OrganicAgencyLineage = organic,
                        OrganicAgency = organic == Constants.ExcelImportRemoveValueKeyword ? "0" : organic.GetIdFromCellText(),
                        PremiumBodyCare = premiumBodyCare == null ? String.Empty : premiumBodyCare,
                        SeafoodFreshOrFrozen = seafoodFreshOrFrozen,
                        SeafoodWildOrFarmRaised = seafoodWildOrFarmRaised,          
                        VeganAgencyLineage = vegan,
                        VeganAgency = vegan == Constants.ExcelImportRemoveValueKeyword ? "0" : vegan.GetIdFromCellText(),
                        Vegetarian = vegetarian == null ? String.Empty : vegetarian,
                        WholeTrade = wholeTrade == null ? String.Empty : wholeTrade,
                        GrassFed = grassFed == null ? String.Empty : grassFed,
                        PastureRaised = pastureRaised == null ? String.Empty : pastureRaised,
                        FreeRange = freeRange == null ? String.Empty : freeRange,
                        DryAged = dryAged == null ? String.Empty : dryAged,
                        AirChilled = airChilled == null ? String.Empty : airChilled,
                        MadeInHouse = madeInHouse == null ? String.Empty : madeInHouse,
                        CreatedDate = createdDate,
                        LastModifiedDate = modifiedDate,
                        LastModifiedUser = modifiedUser,
                        DefaultTaxClass = defaultTaxClass,
                        brandNameDisplayOnly = brandNameDisplayOnly,
                        productionDescriptionDisplayOnly = productionDescriptionDisplayOnly,
                        merchandiseClassDisplayOnly = merchandiseClassDisplayOnly,
                        Error = String.Empty
                    };

                    ParsedRows.Add(parsedRow);
                }
            }
            else
            {
                foreach (var row in nonEmptyRows)
                {
                    scanCode = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ScanCodeColumnIndex].Value);
                    brand = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BrandColumnIndex].Value);
                    productDescription = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ProductDescriptionColumnIndex].Value);
                    posDescription = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PosDescriptionColumnIndex].Value);
                    packageUnit = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PackageUnitColumnIndex].Value);
                    foodStampEligible = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.FoodStampEligibleColumnIndex].Value).GetBoolStringFromCellText();
                    posScaleTare = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PosScaleTareColumnIndex].Value);
                    retailSize = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SizeColumnIndex].Value);
                    retailUom = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.UomColumnIndex].Value);
                    deliverySystem = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DeliverySystemColumnIndex].Value);
                    merchandise = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MerchandiseColumnIndex].Value);
                    tax = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.TaxColumnIndex].Value);
                    national = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.NationalColumnIndex].Value);
                    browsing = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BrowsingColumnIndex].Value);
                    isValidated = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ValidatedColumnIndex].Value).GetBoolStringFromCellText();
                    hiddenItem = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.HiddenItemColumnIndex].Value).GetBoolStringFromCellText();
                    notes = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.NotesColumnIndex].Value);
                    animalWelfareRating = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value).GetBoolStringFromCellText();
                    biodynamic = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BiodynamicColumnIndex].Value).GetBoolStringFromCellText();
                    cheeseAttributeMilkType = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value);
                    cheeseAttributeRaw = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeRawColumnIndex].Value).GetBoolStringFromCellText();
                    ecoScaleRating = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.EcoScaleRatingColumnIndex].Value);
                    msc = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MscColumnIndex].Value).GetBoolStringFromCellText();
                    premiumBodyCare = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PremiumBodyCareColumnIndex].Value).GetBoolStringFromCellText();
                    seafoodFreshOrFrozen = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value);
                    seafoodWildOrFarmRaised = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value);  
                    vegetarian = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.VegetarianColumnIndex].Value).GetBoolStringFromCellText();
                    wholeTrade = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.WholeTradeColumnIndex].Value).GetBoolStringFromCellText();
                    grassFed = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.GrassFedColumnIndex].Value).GetBoolStringFromCellText();
                    pastureRaised = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PastureRaisedColumnIndex].Value).GetBoolStringFromCellText();
                    freeRange = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.FreeRangeColumnIndex].Value).GetBoolStringFromCellText();
                    dryAged = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DryAgedColumnIndex].Value).GetBoolStringFromCellText();
                    airChilled = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AirChilledColumnIndex].Value).GetBoolStringFromCellText();
                    madeInHouse = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MadeInHouseColumnIndex].Value).GetBoolStringFromCellText();
                    createdDate = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.CreatedDateColumnIndex].Value);
                    modifiedDate = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.LastModifiedDateColumnIndex].Value);
                    modifiedUser = ExcelHelper.GetCellStringValue(row.Cells[ExcelHelper.ConsolidatedItemColumnIndexes.LastModifiedUserColumnIndex].Value);

                    var parsedRow = new BulkImportItemModel
                    {
                        ScanCode = scanCode,
                        BrandLineage = brand,
                        BrandName = brand.Split('|').First(),
                        BrandId = brand.GetIdFromCellText(),
                        ProductDescription = productDescription,
                        PosDescription = posDescription,
                        PackageUnit = packageUnit,
                        FoodStampEligible = foodStampEligible,
                        PosScaleTare = posScaleTare,
                        RetailSize = retailSize,
                        RetailUom = retailUom,
                        DeliverySystem = deliverySystem,
                        MerchandiseLineage = merchandise,
                        MerchandiseId = merchandise.GetIdFromCellText(),
                        NationalLineage = national,
                        NationalId = national.GetIdFromCellText(),
                        TaxLineage = tax,
                        TaxId = tax.GetIdFromCellText(),
                        BrowsingLineage = browsing,
                        BrowsingId = browsing.GetIdFromCellText(),
                        IsValidated = isValidated,
                        HiddenItem = hiddenItem,
                        DepartmentSale = String.Empty,
                        Notes = notes,
                        AnimalWelfareRating = animalWelfareRating,
                        Biodynamic = biodynamic == null ? String.Empty : biodynamic,
                        CheeseAttributeMilkType = cheeseAttributeMilkType,
                        CheeseAttributeRaw = cheeseAttributeRaw == null ? String.Empty : cheeseAttributeRaw,
                        EcoScaleRating = ecoScaleRating,       
                        Msc = msc == null ? String.Empty : msc,
                        PremiumBodyCare = premiumBodyCare == null ? String.Empty : premiumBodyCare,
                        SeafoodFreshOrFrozen = seafoodFreshOrFrozen,           
                        SeafoodWildOrFarmRaised = seafoodWildOrFarmRaised,
                        Vegetarian = vegetarian == null ? String.Empty : vegetarian,
                        WholeTrade = wholeTrade == null ? String.Empty : wholeTrade,
                        GrassFed = grassFed == null ? String.Empty : grassFed,
                        PastureRaised = pastureRaised == null ? String.Empty : pastureRaised,
                        FreeRange = freeRange == null ? String.Empty : freeRange,
                        DryAged = dryAged == null ? String.Empty : dryAged,
                        AirChilled = airChilled == null ? String.Empty : airChilled,
                        MadeInHouse = madeInHouse == null ? String.Empty : madeInHouse,
                        CreatedDate = createdDate,
                        LastModifiedDate = modifiedDate,
                        LastModifiedUser = modifiedUser,
                        Error = String.Empty
                    };

                    ParsedRows.Add(parsedRow);
                }
            }
        }

        public override void ValidateSpreadsheetData()
        {
            ValidRows.AddRange(ParsedRows);

            base.ValidateSpreadsheetData();

            if (ValidRows.Count > 0)
            {
                ValidateDepartmentSale();
            }

            if (ValidRows.Count > 0)
            {
                ValidateHiddenItem();
            }

            if (ValidRows.Count > 0)
            {
                CheckForScanCodeWithNoUpdates();
            }

            if (ValidRows.Count > 0)
            {
                CheckForNewScanCodeUploads();
            }

            if (ValidRows.Count > 0)
            {
                CheckThatRowsWithValidateHaveRequiredCanonicalTraits();
            }
        }

        private void ValidateDepartmentSale()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !Validator.Validate(row.DepartmentSale)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Department Sale should be Y, N, or blank.";
                AddErrorRows(error, invalidRows);
            }
        }

        private void CheckForScanCodeWithNoUpdates()
        {
            List<BulkImportItemModel> invalidRows = new List<BulkImportItemModel>();

            foreach (var row in ValidRows)
            {
                var allVisiblePropertiesEmpty =
                    String.IsNullOrWhiteSpace(row.ProductDescription)
                    && String.IsNullOrWhiteSpace(row.BrandLineage)
                    && String.IsNullOrWhiteSpace(row.PosDescription)
                    && String.IsNullOrWhiteSpace(row.PackageUnit)
                    && String.IsNullOrWhiteSpace(row.FoodStampEligible)
                    && String.IsNullOrWhiteSpace(row.PosScaleTare)
                    && String.IsNullOrWhiteSpace(row.RetailSize)
                    && String.IsNullOrWhiteSpace(row.RetailUom)
                    && String.IsNullOrWhiteSpace(row.DeliverySystem)
                    && String.IsNullOrWhiteSpace(row.MerchandiseId)
                    && String.IsNullOrWhiteSpace(row.TaxId)
                    && String.IsNullOrWhiteSpace(row.NationalId)
                    && String.IsNullOrWhiteSpace(row.HiddenItem)
                    && String.IsNullOrWhiteSpace(row.Notes)
                    && String.IsNullOrWhiteSpace(row.AnimalWelfareRating)
                    && String.IsNullOrWhiteSpace(row.Biodynamic)
                    && String.IsNullOrWhiteSpace(row.CheeseAttributeMilkType)
                    && String.IsNullOrWhiteSpace(row.CheeseAttributeRaw)
                    && String.IsNullOrWhiteSpace(row.EcoScaleRating)
                    && String.IsNullOrWhiteSpace(row.GlutenFreeAgencyLineage)
                    && String.IsNullOrWhiteSpace(row.GlutenFreeAgency)
                    && String.IsNullOrWhiteSpace(row.KosherAgencyLineage)
                    && String.IsNullOrWhiteSpace(row.KosherAgency)
                    && String.IsNullOrWhiteSpace(row.NonGmoAgencyLineage)
                    && String.IsNullOrWhiteSpace(row.NonGmoAgency)
                    && String.IsNullOrWhiteSpace(row.OrganicAgencyLineage)
                    && String.IsNullOrWhiteSpace(row.OrganicAgency)
                    && String.IsNullOrWhiteSpace(row.PremiumBodyCare)
                    && String.IsNullOrWhiteSpace(row.SeafoodFreshOrFrozen)
                    && String.IsNullOrWhiteSpace(row.SeafoodWildOrFarmRaised)
                    && String.IsNullOrWhiteSpace(row.VeganAgencyLineage)
                    && String.IsNullOrWhiteSpace(row.VeganAgency)
                    && String.IsNullOrWhiteSpace(row.Vegetarian)
                    && String.IsNullOrWhiteSpace(row.WholeTrade);

                // If ScanCode is the only populated cell, then mark as error.
                if (allVisiblePropertiesEmpty)
                {
                    string error = "No fields are specified to update.";
                    row.Error = error;
                    invalidRows.Add(row);
                }
            }

            if (invalidRows.Any())
            {
                AddErrorRows(invalidRows);
            }
        }

        private void CheckForNewScanCodeUploads()
        {
            var parameters = new GetNewScanCodeUploadsParameters
            {
                ScanCodes = ValidRows.Select(row => new ScanCodeModel { ScanCode = row.ScanCode }).ToList()
            };

            var newScanCodeUploads = getNewScanCodeUploadsQuery.Search(parameters);

            if (newScanCodeUploads.Count > 0)
            {
                string error = "Scan Code does not exist in Icon.";
                var invalidRows = ValidRows.Where(row => newScanCodeUploads.Any(scanCode => scanCode.ScanCode == row.ScanCode)).ToList();
                AddErrorRows(error, invalidRows);
            }
        }

        private void CheckThatRowsWithValidateHaveRequiredCanonicalTraits()
        {
            var rowsWithValidate = ValidRows
                .Where(i => i.IsValidated == "1")
                .ToList();

            // Return if no scan codes are set to validate.
            if (!rowsWithValidate.Any())
            {
                return;
            }

            var parameters = new GetScanCodesNotReadyToValidateParameters
            {
                Items = rowsWithValidate
            };

            var scanCodesNotReadyToValidate = getScanCodesNotReadyToValidateQuery.Search(parameters);

            if (scanCodesNotReadyToValidate.Any())
            {
                string error = "Row is invalid. Row is marked to be validated but the item does not contain all required fields to be validated.";
                var invalidRows = ValidRows.Where(row => scanCodesNotReadyToValidate.Contains(row.ScanCode)).ToList();
                AddErrorRows(error, invalidRows);
            }
        }

        private void ValidateHiddenItem()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !Validator.Validate(row.HiddenItem)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Hidden Item must be blank, Y, or N.";
                AddErrorRows(error, invalidRows);
            }
        }
    }
}