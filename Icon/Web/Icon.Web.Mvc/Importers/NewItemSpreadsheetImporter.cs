using Icon.Common;
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
    public class NewItemSpreadsheetImporter : BaseItemSpreadsheetImporter<BulkImportNewItemModel>
    {
        private IQueryHandler<GetExistingScanCodeUploadsParameters, List<ScanCodeModel>> getExistingScanCodeUploadsQuery;
        private IQueryHandler<GetDuplicateBrandsByTrimmedNameParameters, List<string>> getDuplicateBrandsByTrimmedNameQuery;
        private string[] requiredNewItemPropertyNames = new string[6] { "ScanCode", "BrandLineage", "ProductDescription", "PosDescription", "FoodStampEligible", "PackageUnit" };
        private bool spreadsheetIsConsolidatedItemType;

        public NewItemSpreadsheetImporter(
            IQueryHandler<GetExistingScanCodeUploadsParameters, List<ScanCodeModel>> getExistingScanCodeUploadsQuery,
            IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>> getTaxHierarchyClassesWithNoAbbreviationQuery,
            IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>> getAffinitySubBricksQuery,
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQuery,
            IQueryHandler<GetDuplicateBrandsByTrimmedNameParameters, List<string>> getDuplicateBrandsByTrimmedNameQuery,
            IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgencyQuery)
            : base(getTaxHierarchyClassesWithNoAbbreviationQuery, getAffinitySubBricksQuery, getHierarchyLineageQuery, getCertificationAgencyQuery)
        {
            this.getExistingScanCodeUploadsQuery = getExistingScanCodeUploadsQuery;
            this.getDuplicateBrandsByTrimmedNameQuery = getDuplicateBrandsByTrimmedNameQuery;
            base.maxNumberOfRowsToImport = AppSettingsAccessor.GetIntSetting("BulkNewItemMaxRowsLimit", 10000);
        }

        public override bool IsValidSpreadsheetType()
        {
            var headerCells = Workbook.Worksheets[0].Rows[0].Cells
                .Where(c => c.Value != null && !String.IsNullOrWhiteSpace(c.ToString()) && c.Value.ToString() != "Error")
                .Select(c => c.Value);

            if (headerCells.SequenceEqual(ValidIrmaItemColumnHeaders()))
            {
                spreadsheetIsConsolidatedItemType = false;
                return true;
            }
            else if (headerCells.SequenceEqual(ValidConsolidatedItemColumnHeaders()))
            {
                spreadsheetIsConsolidatedItemType = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        private string[] ValidIrmaItemColumnHeaders()
        {
            return new[]
            {
                ExcelHelper.ExcelExportColumnNames.ScanCode,
                ExcelHelper.ExcelExportColumnNames.Brand,
                ExcelHelper.ExcelExportColumnNames.ProductDescription,
                ExcelHelper.ExcelExportColumnNames.PosDescription,
                ExcelHelper.ExcelExportColumnNames.PackageUnit,
                ExcelHelper.ExcelExportColumnNames.FoodStampEligible,
                ExcelHelper.ExcelExportColumnNames.PosScaleTare,
                ExcelHelper.ExcelExportColumnNames.Size,
                ExcelHelper.ExcelExportColumnNames.Uom,
                ExcelHelper.ExcelExportColumnNames.DeliverySystem,
                ExcelHelper.ExcelExportColumnNames.IrmaSubTeam,
                ExcelHelper.ExcelExportColumnNames.Merchandise,
                ExcelHelper.ExcelExportColumnNames.Tax,
                ExcelHelper.ExcelExportColumnNames.AlcoholByVolume,
                ExcelHelper.ExcelExportColumnNames.NationalClass,
                ExcelHelper.ExcelExportColumnNames.Browsing,
                ExcelHelper.ExcelExportColumnNames.Validated,
                ExcelHelper.ExcelExportColumnNames.Region,
                ExcelHelper.ExcelExportColumnNames.AirChilled,
                ExcelHelper.ExcelExportColumnNames.AnimalWelfareRating,
                ExcelHelper.ExcelExportColumnNames.Biodynamic,
                ExcelHelper.ExcelExportColumnNames.CheeseAttributeMilkType,
                ExcelHelper.ExcelExportColumnNames.CheeseAttributeRaw,
                ExcelHelper.ExcelExportColumnNames.DryAged,
                ExcelHelper.ExcelExportColumnNames.EcoScaleRating,
                ExcelHelper.ExcelExportColumnNames.FreeRange,
                ExcelHelper.ExcelExportColumnNames.SeafoodFreshOrFrozen,
                ExcelHelper.ExcelExportColumnNames.GlutenFree,
                ExcelHelper.ExcelExportColumnNames.GrassFed,
                ExcelHelper.ExcelExportColumnNames.Kosher,
                ExcelHelper.ExcelExportColumnNames.MadeInHouse,
                ExcelHelper.ExcelExportColumnNames.Msc,
                ExcelHelper.ExcelExportColumnNames.NonGmo,
                ExcelHelper.ExcelExportColumnNames.NutritionRequired,
                ExcelHelper.ExcelExportColumnNames.Organic,
                ExcelHelper.ExcelExportColumnNames.PastureRaised,
                ExcelHelper.ExcelExportColumnNames.PremiumBodyCare,
                ExcelHelper.ExcelExportColumnNames.SeafoodWildOrFarmRaised,
                ExcelHelper.ExcelExportColumnNames.Vegan,
                ExcelHelper.ExcelExportColumnNames.Vegetarian,
                ExcelHelper.ExcelExportColumnNames.WholeTrade,
            };
        }

        private string[] ValidConsolidatedItemColumnHeaders()
        {
            return new[]
            {
                ExcelHelper.ExcelExportColumnNames.ScanCode,
                ExcelHelper.ExcelExportColumnNames.Brand,
                ExcelHelper.ExcelExportColumnNames.ProductDescription,
                ExcelHelper.ExcelExportColumnNames.PosDescription,
                ExcelHelper.ExcelExportColumnNames.PackageUnit,
                ExcelHelper.ExcelExportColumnNames.FoodStampEligible,
                ExcelHelper.ExcelExportColumnNames.PosScaleTare,
                ExcelHelper.ExcelExportColumnNames.Size,
                ExcelHelper.ExcelExportColumnNames.Uom,
                ExcelHelper.ExcelExportColumnNames.DeliverySystem,
                ExcelHelper.ExcelExportColumnNames.Merchandise,
                ExcelHelper.ExcelExportColumnNames.Tax,
                ExcelHelper.ExcelExportColumnNames.AlcoholByVolume,
                ExcelHelper.ExcelExportColumnNames.NationalClass,
                ExcelHelper.ExcelExportColumnNames.Browsing,
                ExcelHelper.ExcelExportColumnNames.Validated,
                ExcelHelper.ExcelExportColumnNames.HiddenItem,
                ExcelHelper.ExcelExportColumnNames.DepartmentSale,
                ExcelHelper.ExcelExportColumnNames.Notes,
                ExcelHelper.ExcelExportColumnNames.AirChilled,
                ExcelHelper.ExcelExportColumnNames.AnimalWelfareRating,
                ExcelHelper.ExcelExportColumnNames.Biodynamic,
                ExcelHelper.ExcelExportColumnNames.CheeseAttributeMilkType,
                ExcelHelper.ExcelExportColumnNames.CheeseAttributeRaw,
                ExcelHelper.ExcelExportColumnNames.DryAged,
                ExcelHelper.ExcelExportColumnNames.EcoScaleRating,
                ExcelHelper.ExcelExportColumnNames.FreeRange,
                ExcelHelper.ExcelExportColumnNames.SeafoodFreshOrFrozen,
                ExcelHelper.ExcelExportColumnNames.GlutenFree,
                ExcelHelper.ExcelExportColumnNames.GrassFed,
                ExcelHelper.ExcelExportColumnNames.Kosher,
                ExcelHelper.ExcelExportColumnNames.MadeInHouse,
                ExcelHelper.ExcelExportColumnNames.Msc,
                ExcelHelper.ExcelExportColumnNames.NonGmo,
                ExcelHelper.ExcelExportColumnNames.Organic,
                ExcelHelper.ExcelExportColumnNames.PastureRaised,
                ExcelHelper.ExcelExportColumnNames.PremiumBodyCare,
                ExcelHelper.ExcelExportColumnNames.SeafoodWildOrFarmRaised,
                ExcelHelper.ExcelExportColumnNames.Vegan,
                ExcelHelper.ExcelExportColumnNames.Vegetarian,
                ExcelHelper.ExcelExportColumnNames.WholeTrade,
            };
        }

        public override void ConvertSpreadsheetToModel()
        {
            int? irmaSubTeamColumnIndex;
            int merchandiseColumnIndex;
            int nationalColumnIndex;
            int taxColumnIndex;
            int browsingColumnIndex;
            int validatedColumnIndex;
            int? regionCodeColumnIndex;
            int animalWelfareColumnIndex;
            int biodynamicColumnIndex;
            int cheeseAttributeMilkTypeColumnIndex;
            int cheeseAttributeRawColumnIndex;
            int ecoScaleRatingColumnIndex;
            int mscColumnIndex;
            int premiumBodyCareColumnIndex;
            int seafoodFreshOrFrozenColumnIndex;
            int seafoodWildOrFarmRaisedColumnIndex;
            int vegetarianColumnIndex;
            int wholeTradeColumnIndex;
            int grassFedColumnIndex;
            int pastureRaisedColumnIndex;
            int freeRangeColumnIndex;
            int dryAgedColumnIndex;
            int airChilledColumnIndex;
            int madeInHouseColumnIndex;
            int alcoholByVolumnIndex;
            int nutritionRequiredIndex;

            if (spreadsheetIsConsolidatedItemType)
            {
                irmaSubTeamColumnIndex = null;
                merchandiseColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.MerchandiseColumnIndex;
                taxColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.TaxColumnIndex;
                nationalColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.NationalColumnIndex;
                browsingColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.BrowsingColumnIndex;
                validatedColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.ValidatedColumnIndex;
                regionCodeColumnIndex = null;
                animalWelfareColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.AnimalWelfareRatingColumnIndex;
                biodynamicColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.BiodynamicColumnIndex;
                cheeseAttributeMilkTypeColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex;
                cheeseAttributeRawColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeRawColumnIndex;
                ecoScaleRatingColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.EcoScaleRatingColumnIndex;
                mscColumnIndex = ExcelHelper.IrmaItemColumnIndexes.MscColumnIndex;           
                premiumBodyCareColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.PremiumBodyCareColumnIndex;
                seafoodFreshOrFrozenColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex;
                seafoodWildOrFarmRaisedColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex;
                vegetarianColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.VegetarianColumnIndex;
                wholeTradeColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.WholeTradeColumnIndex;
                grassFedColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.GrassFedColumnIndex;
                pastureRaisedColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.PastureRaisedColumnIndex;
                freeRangeColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.FreeRangeColumnIndex;
                dryAgedColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.DryAgedColumnIndex;
                airChilledColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.AirChilledColumnIndex;
                madeInHouseColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.MadeInHouseColumnIndex;
                alcoholByVolumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.AlcoholByVolumeColumnIndex;
                nutritionRequiredIndex = ExcelHelper.ConsolidatedItemColumnIndexes.NutritionRequiredColumnIndex;
            }
            else
            {
                irmaSubTeamColumnIndex = ExcelHelper.IrmaItemColumnIndexes.IrmaSubTeamColumnIndex;
                merchandiseColumnIndex = ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex;
                taxColumnIndex = ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex;
                nationalColumnIndex = ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex;
                browsingColumnIndex = ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex;
                validatedColumnIndex = ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex;
                regionCodeColumnIndex = ExcelHelper.IrmaItemColumnIndexes.RegionCodeColumnIndex;
                animalWelfareColumnIndex = ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex;
                biodynamicColumnIndex = ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex;
                cheeseAttributeMilkTypeColumnIndex = ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex;
                cheeseAttributeRawColumnIndex = ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex;
                ecoScaleRatingColumnIndex = ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex;
                mscColumnIndex = ExcelHelper.ConsolidatedItemColumnIndexes.MscColumnIndex;
                premiumBodyCareColumnIndex = ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex;
                seafoodFreshOrFrozenColumnIndex = ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex;
                seafoodWildOrFarmRaisedColumnIndex = ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex;
                vegetarianColumnIndex = ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex;
                wholeTradeColumnIndex = ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex;
                grassFedColumnIndex = ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex;
                pastureRaisedColumnIndex = ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex;
                freeRangeColumnIndex = ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex;
                dryAgedColumnIndex = ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex;
                airChilledColumnIndex = ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex;
                madeInHouseColumnIndex = ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex;
                alcoholByVolumnIndex = ExcelHelper.IrmaItemColumnIndexes.AlcoholByVolumeIndex;
                nutritionRequiredIndex = ExcelHelper.IrmaItemColumnIndexes.NutritionRequiredIndex;
            }

            // Begin at 1 to skip the header row (object model is 0-indexed; an actual spreadsheet is 1-indexed).
            // Only import up to a certain number of rows per spreadsheet. Default is int.Max.
            nonEmptyRows = Workbook.Worksheets[0].Rows
                .Skip(1)
                .Where(r => r.Cells.Any(c => c.Value != null && !string.IsNullOrWhiteSpace(c.Value.ToString())))
                .Take(maxNumberOfRowsToImport);

            foreach (var row in nonEmptyRows)
            {
                string scanCode = ExcelHelper.GetCellStringValue(row.Cells[0].Value);
                string brand = ExcelHelper.GetCellStringValue(row.Cells[1].Value);
                string productDescription = ExcelHelper.GetCellStringValue(row.Cells[2].Value);
                string posDescription = ExcelHelper.GetCellStringValue(row.Cells[3].Value);
                string packageUnit = ExcelHelper.GetCellStringValue(row.Cells[4].Value);
                string foodStampEligible = ExcelHelper.GetCellStringValue(row.Cells[5].Value).GetBoolStringFromCellText();
                string posScaleTare = ExcelHelper.GetCellStringValue(row.Cells[6].Value);
                string retailSize = ExcelHelper.GetCellStringValue(row.Cells[7].Value);
                string retailUom = ExcelHelper.GetCellStringValue(row.Cells[8].Value);
                string deliverySystem = ExcelHelper.GetCellStringValue(row.Cells[9].Value);
                string irmaSubteam = spreadsheetIsConsolidatedItemType ? String.Empty : ExcelHelper.GetCellStringValue(row.Cells[irmaSubTeamColumnIndex.Value].Value);
                string merchandise = ExcelHelper.GetCellStringValue(row.Cells[merchandiseColumnIndex].Value);
                string national = ExcelHelper.GetCellStringValue(row.Cells[nationalColumnIndex].Value);
                string tax = ExcelHelper.GetCellStringValue(row.Cells[taxColumnIndex].Value);
                string browsing = ExcelHelper.GetCellStringValue(row.Cells[browsingColumnIndex].Value);
                string isValidated = ExcelHelper.GetCellStringValue(row.Cells[validatedColumnIndex].Value).GetBoolStringFromCellText();
                string regionCode = spreadsheetIsConsolidatedItemType ? String.Empty : ExcelHelper.GetCellStringValue(row.Cells[regionCodeColumnIndex.Value].Value);
                string animalWelfareRating = ExcelHelper.GetCellStringValue(row.Cells[animalWelfareColumnIndex].Value).GetBoolStringFromCellText();
                string biodynamic = ExcelHelper.GetCellStringValue(row.Cells[biodynamicColumnIndex].Value).GetBoolStringFromCellText();
                string cheeseAttributeMilkType = ExcelHelper.GetCellStringValue(row.Cells[cheeseAttributeMilkTypeColumnIndex].Value);
                string cheeseAttributeRaw = ExcelHelper.GetCellStringValue(row.Cells[cheeseAttributeRawColumnIndex].Value).GetBoolStringFromCellText();
                string ecoScaleRating = ExcelHelper.GetCellStringValue(row.Cells[ecoScaleRatingColumnIndex].Value);
                string msc = ExcelHelper.GetCellStringValue(row.Cells[mscColumnIndex].Value).GetBoolStringFromCellText();
      
                string premiumBodyCare = ExcelHelper.GetCellStringValue(row.Cells[premiumBodyCareColumnIndex].Value).GetBoolStringFromCellText();
                string seafoodFreshOrFrozen = ExcelHelper.GetCellStringValue(row.Cells[seafoodFreshOrFrozenColumnIndex].Value);
                string seafoodWildOrFarmRaised = ExcelHelper.GetCellStringValue(row.Cells[seafoodWildOrFarmRaisedColumnIndex].Value);
                string vegetarian = ExcelHelper.GetCellStringValue(row.Cells[vegetarianColumnIndex].Value).GetBoolStringFromCellText();
                string wholeTrade = ExcelHelper.GetCellStringValue(row.Cells[wholeTradeColumnIndex].Value).GetBoolStringFromCellText();
                string grassFed = ExcelHelper.GetCellStringValue(row.Cells[grassFedColumnIndex].Value).GetBoolStringFromCellText();
                string pastureRaised = ExcelHelper.GetCellStringValue(row.Cells[pastureRaisedColumnIndex].Value).GetBoolStringFromCellText();
                string freeRange = ExcelHelper.GetCellStringValue(row.Cells[freeRangeColumnIndex].Value).GetBoolStringFromCellText();
                string dryAged = ExcelHelper.GetCellStringValue(row.Cells[dryAgedColumnIndex].Value).GetBoolStringFromCellText();
                string airChilled = ExcelHelper.GetCellStringValue(row.Cells[airChilledColumnIndex].Value).GetBoolStringFromCellText();
                string madeInHouse = ExcelHelper.GetCellStringValue(row.Cells[madeInHouseColumnIndex].Value).GetBoolStringFromCellText();
                string alcoholByVolume = ExcelHelper.GetCellStringValue(row.Cells[alcoholByVolumnIndex].Value);
                string nutritionRequired = ExcelHelper.GetCellStringValue(row.Cells[nutritionRequiredIndex].Value).GetBoolStringFromCellText();

                var parsedRow = new BulkImportNewItemModel
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
                    IrmaSubTeamName = irmaSubteam,
                    MerchandiseLineage = merchandise,
                    MerchandiseId = merchandise.GetIdFromCellText(),
                    NationalId = national.GetIdFromCellText(),
                    NationalLineage = national,
                    TaxLineage = tax,
                    TaxId = tax.GetIdFromCellText(),
                    BrowsingLineage = browsing,
                    BrowsingId = browsing.GetIdFromCellText(),
                    IsValidated = isValidated,
                    RegionCode = regionCode,
                    AnimalWelfareRating = animalWelfareRating,
                    Biodynamic = biodynamic,
                    CheeseAttributeMilkType = cheeseAttributeMilkType,
                    CheeseAttributeRaw = cheeseAttributeRaw,
                    EcoScaleRating = ecoScaleRating,
                    Msc = msc,
                    PremiumBodyCare = premiumBodyCare,
                    SeafoodFreshOrFrozen = seafoodFreshOrFrozen,
                    SeafoodWildOrFarmRaised = seafoodWildOrFarmRaised,
                    Vegetarian = vegetarian,
                    WholeTrade = wholeTrade,
                    GrassFed = grassFed,
                    PastureRaised = pastureRaised,
                    FreeRange = freeRange,
                    DryAged = dryAged,
                    AirChilled = airChilled,
                    MadeInHouse = madeInHouse,
                    AlcoholByVolume = alcoholByVolume,
                    NutritionRequired = nutritionRequired,
                    Error = String.Empty
                };

                ParsedRows.Add(parsedRow);
            }
        }

        public override void ValidateSpreadsheetData()
        {
            ValidRows.AddRange(ParsedRows);

            base.ValidateSpreadsheetData();

            if (ValidRows.Count > 0)
            {
                CheckForAllRequiredInformation();
            }

            if (ValidRows.Count > 0)
            {
                CheckForAlreadyExistingScanCodes();
            }

            if (ValidRows.Count > 0)
            {
                ValidateBrandLengthAndName();
            }

            if (ValidRows.Count > 0)
            {
                ValidateRetailSizeFormat();
            }

            if (ValidRows.Count > 0)
            {
                CheckForTrimmedBrandDuplication();
            }

            if (ValidRows.Count > 0)
            {
                CheckForTrimmedBrandDuplication();
            }

            if (ValidRows.Count > 0)
            {
                CheckThatValidatedItemsHaveAllRequiredFields();
            }

            if (ValidRows.Count > 0)
            {
                CheckAlcolholByColumnFormat();
            }

            if(ValidRows.Count > 0)
            {
                CheckNutritionRequiredFormat();
            }
        }

        private void CheckNutritionRequiredFormat()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !Validator.Validate(row.NutritionRequired)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Nutrition Required must be blank, Y, or N.";
                AddErrorRows(error, invalidRows);
            }
        }

        private void CheckAlcolholByColumnFormat()
        {
            IconPropertyValidationRules iconPropertyValidationRules = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.AlcoholByVolume);

            var invalidRows = ValidRows.Where(row => !String.IsNullOrWhiteSpace(row.AlcoholByVolume) && !iconPropertyValidationRules.IsValid(row.AlcoholByVolume, null)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = ValidatorErrorMessages.AlcoholByVolumeError;
                AddErrorRows(error, invalidRows);
            }
        }

        private void CheckForAllRequiredInformation()
        {
            Type itemModelType = typeof(BulkImportNewItemModel);
            var itemModelProperties = itemModelType.GetProperties();

            List<BulkImportNewItemModel> invalidRows = new List<BulkImportNewItemModel>();

            foreach (var row in ValidRows)
            {
                var requiredCells = itemModelProperties.Where(itemProperty => requiredNewItemPropertyNames.Contains(itemProperty.Name)).ToList();

                var populatedRequiredCells = requiredCells.Where(cell => cell.GetValue(row, null).ToString() != String.Empty).ToList();

                if (populatedRequiredCells.Count != requiredCells.Count)
                {
                    invalidRows.Add(row);
                }
            }

            if (invalidRows.Count > 0)
            {
                string error = "The row does not contain all information required to add the item.";
                AddErrorRows(error, invalidRows);
            }
        }

        private void CheckForAlreadyExistingScanCodes()
        {
            var parameters = new GetExistingScanCodeUploadsParameters
            {
                ScanCodes = ValidRows.Select(row => new ScanCodeModel { ScanCode = row.ScanCode }).ToList()
            };

            var alreadyExistingScanCodes = getExistingScanCodeUploadsQuery.Search(parameters);

            if (alreadyExistingScanCodes.Count > 0)
            {
                var invalidRows = ValidRows.Where(row => alreadyExistingScanCodes.Any(scanCode => scanCode.ScanCode == row.ScanCode)).ToList();
                string error = "Scan Code already exists.";
                AddErrorRows(error, invalidRows);
            }
        }

        private void ValidateBrandLengthAndName()
        {
            IconPropertyValidationRules iconPropertyValidationRules = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.BrandName);

            var invalidRows = ValidRows.Where(row => !iconPropertyValidationRules.IsValid(row.BrandName, null)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Brand name is invalid.  " + ValidatorErrorMessages.BrandNameError;
                AddErrorRows(error, invalidRows);
            }
        }

        private void ValidateRetailSizeFormat()
        {
            IconPropertyValidationRules iconPropertyValidationRules = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.RetailSize);

            var invalidRows = ValidRows.Where(row => !iconPropertyValidationRules.IsValid(row.RetailSize, null)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Retail size is invalid.  " + ValidatorErrorMessages.RetailSizeError;
                AddErrorRows(error, invalidRows);
            }
        }

        private void CheckForTrimmedBrandDuplication()
        {
            // If the brand name is greater than 25 characters, and the brand's name trimmed to 25 characters already exists in Icon, add it to error rows.
            var longBrandNames = ValidRows
                    .Where(row => row.BrandName.Length > Constants.IrmaBrandNameMaxLength)
                    .Distinct()
                    .Select(row => row.BrandName).Distinct().ToDictionary(bl => bl, bl => bl.Substring(0, Constants.IrmaBrandNameMaxLength));

            var invalidBrands = getDuplicateBrandsByTrimmedNameQuery.Search(new GetDuplicateBrandsByTrimmedNameParameters() { LongBrandNameList = longBrandNames });

            var invalidRows = ValidRows.Where(row => invalidBrands.Any(ib =>
                    ib.Equals(row.BrandName, StringComparison.InvariantCultureIgnoreCase)))
                    .ToList();

            if (invalidRows.Any())
            {
                string error = String.Format("Brand name reduced to {0} characters already exists in Icon.  Adding the brand name may cause conflicts with IRMA.", Constants.IrmaBrandNameMaxLength);
                AddErrorRows(error, invalidRows);
            }
        }

        protected void CheckThatValidatedItemsHaveAllRequiredFields()
        {
            var invalidRows = ValidRows.Where(r => r.IsValidated == "1"
                && (String.IsNullOrWhiteSpace(r.ScanCode)
                    || String.IsNullOrWhiteSpace(r.BrandId)
                    || String.IsNullOrWhiteSpace(r.BrandName)
                    || String.IsNullOrWhiteSpace(r.ProductDescription)
                    || String.IsNullOrWhiteSpace(r.PosDescription)
                    || String.IsNullOrWhiteSpace(r.PackageUnit)
                    || String.IsNullOrWhiteSpace(r.FoodStampEligible)
                    || String.IsNullOrWhiteSpace(r.TaxId)
                    || String.IsNullOrWhiteSpace(r.MerchandiseId)
                    || String.IsNullOrWhiteSpace(r.NationalId)
                    || String.IsNullOrWhiteSpace(r.RetailSize)
                    || String.IsNullOrWhiteSpace(r.RetailUom)))
                    .ToList();

            if (invalidRows.Any())
            {
                string error = "Rows marked for validation must have all canonical information in the spreadsheet.";
                AddErrorRows(error, invalidRows);
            }
        }
    }
}