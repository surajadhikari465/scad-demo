using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Importers
{
    public abstract class BaseItemSpreadsheetImporter<T> : ISpreadsheetImporter<T> where T : IImportItemModel, new()
    {
        protected IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>> getTaxHierarchyClassesWithNoAbbreviationQuery;
        protected IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>> getAffinitySubBricksQuery;
        protected IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQuery;
        protected IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgencyQuery;
        protected string[] requiredHeadersForConsolidatedItemSpreadsheet;
        protected string[] requiredHeadersForTaxCorrectionSpreadsheet;
        protected bool isTaxCorrectionsImport;
        protected IEnumerable<WorksheetRow> nonEmptyRows;
        protected int maxNumberOfRowsToImport;

        public Workbook Workbook { get; set; }
        public List<T> ParsedRows { get; set; }
        public List<T> ErrorRows { get; set; }
        public List<T> ValidRows { get; set; }
        public IObjectValidator<string> Validator { get; set; }

        public BaseItemSpreadsheetImporter(
            IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>> getTaxHierarchyClassesWithNoAbbreviationQuery,
            IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>> getAffinitySubBricksQuery,
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQuery,
            IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgencyQuery)
        {
            this.getTaxHierarchyClassesWithNoAbbreviationQuery = getTaxHierarchyClassesWithNoAbbreviationQuery;
            this.getAffinitySubBricksQuery = getAffinitySubBricksQuery;
            this.getHierarchyLineageQuery = getHierarchyLineageQuery;
            this.getCertificationAgencyQuery = getCertificationAgencyQuery;

            ParsedRows = new List<T>();
            ValidRows = new List<T>();
            ErrorRows = new List<T>();

            maxNumberOfRowsToImport = Int32.MaxValue;

            requiredHeadersForConsolidatedItemSpreadsheet = new[]
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
                ExcelHelper.ExcelExportColumnNames.Validated
            };

            requiredHeadersForTaxCorrectionSpreadsheet = new[]
            {
                ExcelHelper.ExcelExportColumnNames.ScanCode,
                ExcelHelper.ExcelExportColumnNames.Brand,
                ExcelHelper.ExcelExportColumnNames.ProductDescription,
                ExcelHelper.ExcelExportColumnNames.Merchandise,
                ExcelHelper.DefaultTaxMismatchColumnNames.DefaultTaxClass,
                ExcelHelper.DefaultTaxMismatchColumnNames.TaxClassOverride
            };
        }

        public virtual bool IsValidSpreadsheetType()
        {
            int maxHeaders;

            maxHeaders = requiredHeadersForConsolidatedItemSpreadsheet.Length;

            string[] consolidatedItemSpreadsheetHeaders = new string[maxHeaders];
            for (int i = 0; i < maxHeaders; i++)
            {
                var cell = Workbook.Worksheets[0].Rows[0].Cells[i];
                var header = cell.Value == null ? String.Empty : cell.Value.ToString();

                consolidatedItemSpreadsheetHeaders[i] = header;
            }

            maxHeaders = requiredHeadersForTaxCorrectionSpreadsheet.Length;

            string[] taxCorrectionSpreadsheetHeaders = new string[maxHeaders];
            for (int i = 0; i < maxHeaders; i++)
            {
                var cell = Workbook.Worksheets[0].Rows[0].Cells[i];
                var header = cell.Value == null ? String.Empty : cell.Value.ToString();

                taxCorrectionSpreadsheetHeaders[i] = header;
            }

            bool validConsolidatedItemSpreadsheet = requiredHeadersForConsolidatedItemSpreadsheet.SequenceEqual(consolidatedItemSpreadsheetHeaders);
            bool validTaxCorrectionSpreadsheet = requiredHeadersForTaxCorrectionSpreadsheet.SequenceEqual(taxCorrectionSpreadsheetHeaders);

            if (validTaxCorrectionSpreadsheet)
            {
                isTaxCorrectionsImport = true;
            }

            return validConsolidatedItemSpreadsheet || validTaxCorrectionSpreadsheet;
        }

        public abstract void ConvertSpreadsheetToModel();

        public virtual void ValidateSpreadsheetData()
        {
            if (ValidRows.Count > 0)
            {
                ValidateScanCodeFormat();
            }

            if (ValidRows.Count > 0)
            {
                CheckForDuplicateScanCodes();
            }

            if (ValidRows.Count > 0)
            {
                ValidateProductDescription();
            }

            if (ValidRows.Count > 0)
            {
                ValidatePosDescription();
            }

            if (ValidRows.Count > 0)
            {
                ValidatePackageUnit();
            }

            if (ValidRows.Count > 0)
            {
                ValidateFoodStampEligible();
            }

            if (ValidRows.Count > 0)
            {
                ValidatePosScaleTare();
            }

            if (ValidRows.Count > 0)
            {
                ValidateRetailSize();
            }

            if (ValidRows.Count > 0)
            {
                ValidateRetailUom();
            }

            if (ValidRows.Count > 0)
            {
                ValidateDeliverySystem();
            }

            if (ValidRows.Count > 0)
            {
                ValidateValidated();
            }

            if (ValidRows.Count > 0)
            {
                ValidateAnimalWelfareRating();
            }

            if (ValidRows.Count > 0)
            {
                ValidateBiodynamic();
            }

            if (ValidRows.Count > 0)
            {
                ValidateCheeseAttributeMilkType();
            }

            if (ValidRows.Count > 0)
            {
                ValidateCheeseAttributeRaw();
            }

            if (ValidRows.Count > 0)
            {
                ValidateEcoScaleRating();
            }        

            if (ValidRows.Count > 0)
            {
                ValidatePremiumBodyCare();
            }

            if (ValidRows.Count > 0)
            {
                ValidateGrassFed();
            }

            if (ValidRows.Count > 0)
            {
                ValidatePastureRaised();
            }

            if (ValidRows.Count > 0)
            {
                ValidateFreeRange();
            }

            if (ValidRows.Count > 0)
            {
                ValidateDryAged();
            }

            if (ValidRows.Count > 0)
            {
                ValidateAirChilled();
            }

            if (ValidRows.Count > 0)
            {
                ValidateMadeInHouse();
            }

            if (ValidRows.Count > 0)
            {
                ValidateSeafoodFreshOrFrozen();
            }

            if (ValidRows.Count > 0)
            {
                ValidateSeafoodWildOrFarmRaised();
            }

            if (ValidRows.Count > 0)
            {
                ValidateVegetarian();
            }

            if (ValidRows.Count > 0)
            {
                ValidateWholeTrade();
            }

            if (ValidRows.Count > 0)
            {
                CheckThatHierarchyClassExists();
            }

            if (ValidRows.Count > 0)
            {
                CheckForTaxClassWithNoAbbreviation();
            }

            if (ValidRows.Count > 0)
            {
                CheckForAffinitySubBrickAssociations();
            }
        }

        protected void ValidateScanCodeFormat()
        {
            Validator = new ScanCodeValidator();

            var invalidRows = ValidRows.Where(row => !Validator.Validate(row.ScanCode)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = String.Format("Scan code is required and must be {0} or fewer numbers.", Constants.ScanCodeMaxLength);
                AddErrorRows(error, invalidRows);
            }
        }

        private void CheckForDuplicateScanCodes()
        {
            var scanCodeLookupByCount = new Dictionary<string, int>();

            foreach (var row in ValidRows)
            {
                int count;
                if (scanCodeLookupByCount.TryGetValue(row.ScanCode, out count))
                {
                    scanCodeLookupByCount[row.ScanCode] = ++count;
                }
                else
                {
                    scanCodeLookupByCount.Add(row.ScanCode, 1);
                }
            }

            var duplicateScanCodes = scanCodeLookupByCount.Where(sc => sc.Value > 1).Select(sc => sc.Key).ToList();

            var invalidRows = ValidRows.Where(row => duplicateScanCodes.Contains(row.ScanCode)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Scan Code appears multiple times on the spreadsheet.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateProductDescription()
        {
            IconPropertyValidationRules iconPropertyValidationRules = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.ProductDescription);

            var invalidRows = ValidRows.Where(row => !iconPropertyValidationRules.IsValid(row.ProductDescription, null)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Product Description is invalid.  " + ValidatorErrorMessages.ProductDescriptionError;
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidatePosDescription()
        {
            IconPropertyValidationRules iconPropertyValidationRules = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.PosDescription);

            var invalidRows = ValidRows.Where(row => !iconPropertyValidationRules.IsValid(row.PosDescription, null)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "POS Description is invalid.  " + ValidatorErrorMessages.PosDescriptionError;
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidatePosScaleTare()
        {
            Validator = new PosScaleTareValidator();

            var invalidRows = ValidRows.Where(row => !Validator.Validate(row.PosScaleTare)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "POS Scale Tare must be a valid decimal value less than 10 with no more than 4 digits plus the decimal point.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateFoodStampEligible()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !Validator.Validate(row.FoodStampEligible)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Food Stamp Eligible must be blank, Y, or N.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidatePackageUnit()
        {
            Validator = new PackageUnitValidator();

            var invalidRows = ValidRows.Where(row => !Validator.Validate(row.PackageUnit)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = String.Format("Item Pack must be a whole number with {0} or fewer digits.", Constants.PackageUnitMaxLength);
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateRetailSize()
        {
            Validator = new RetailSizeValidator();

            var invalidRows = ValidRows.Where(row => !String.IsNullOrWhiteSpace(row.RetailSize) && !Validator.Validate(row.RetailSize)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Size must be greater than zero and contain only numbers and decimal points with 5 or less digits to the left of the decimal and 4 or less digits to the right of the decimal.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateRetailUom()
        {
            var validUoms = UomCodes.ByName.Values.ToList();

            var invalidRows = ValidRows.Where(row => !String.IsNullOrWhiteSpace(row.RetailUom) && !validUoms.Contains(row.RetailUom)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = String.Format("UOM should be one of the following: {0}.", String.Join(", ", UomCodes.ByName.Values));
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateDeliverySystem()
        {
            var validDeliverySystems = DeliverySystems.AsDictionary.Values.ToList();

            var invalidRows = ValidRows.Where(row => !String.IsNullOrWhiteSpace(row.DeliverySystem) && !validDeliverySystems.Contains(row.DeliverySystem)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = String.Format("Delivery System should be one of the following: {0}.", String.Join(", ", DeliverySystems.AsDictionary.Values));
                AddErrorRows(error, invalidRows);
            }
        }
        protected void ValidateValidated()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !Validator.Validate(row.IsValidated)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Validated should be Y, N, or blank.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateAnimalWelfareRating()
        {
            Validator = new AnimalWelfareRatingValidator();

            var invalidRows = ValidRows.Where(row => !Validator.Validate(row.AnimalWelfareRating)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = String.Format("Animal Welfare Rating is not recognized.  Valid entries are {0}.", String.Join(", ", AnimalWelfareRatings.Descriptions.AsArray));
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateBiodynamic()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !string.IsNullOrEmpty(row.Biodynamic) && !Validator.Validate(row.Biodynamic)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Biodynamic should be Y, N, or blank.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateCheeseAttributeMilkType()
        {
            Validator = new CheeseAttributeMilkTypeValidator();

            var invalidRows = ValidRows.Where(row => !Validator.Validate(row.CheeseAttributeMilkType)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = String.Format("Cheese Attribute: Milk Type is not recognized.  Valid entries are {0}.", String.Join(", ", MilkTypes.Descriptions.AsArray));
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateCheeseAttributeRaw()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !string.IsNullOrEmpty(row.CheeseAttributeRaw) && !Validator.Validate(row.CheeseAttributeRaw)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Cheese Attribute: Raw should be Y, N, or blank.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateEcoScaleRating()
        {
            Validator = new EcoScaleRatingValidator();

            var invalidRows = ValidRows.Where(row => !Validator.Validate(row.EcoScaleRating)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = String.Format("Eco-Scale Rating is not recognized.  Valid entries are {0}.", String.Join(", ", EcoScaleRatings.Descriptions.AsArray));
                AddErrorRows(error, invalidRows);
            }
        }
     

        protected void ValidatePremiumBodyCare()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !string.IsNullOrEmpty(row.PremiumBodyCare) && !Validator.Validate(row.PremiumBodyCare)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Premium Body Care should be Y, N, or blank.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateGrassFed()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !string.IsNullOrEmpty(row.GrassFed) && !Validator.Validate(row.GrassFed)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Grass Fed should be Y, N, or blank.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidatePastureRaised()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !string.IsNullOrEmpty(row.PastureRaised) && !Validator.Validate(row.PastureRaised)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Pasture Raised should be Y, N, or blank.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateFreeRange()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !string.IsNullOrEmpty(row.FreeRange) && !Validator.Validate(row.FreeRange)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Free Range should be Y, N, or blank.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateDryAged()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !string.IsNullOrEmpty(row.DryAged) && !Validator.Validate(row.DryAged)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Dry Aged should be Y, N, or blank.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateAirChilled()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !string.IsNullOrEmpty(row.AirChilled) && !Validator.Validate(row.AirChilled)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Air Chilled should be Y, N, or blank.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateMadeInHouse()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !string.IsNullOrEmpty(row.MadeInHouse) && !Validator.Validate(row.MadeInHouse)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Made In House should be Y, N, or blank.";
                AddErrorRows(error, invalidRows);
            }
        }
        protected void ValidateSeafoodFreshOrFrozen()
        {
            Validator = new SeafoodFreshOrFrozenValidator();

            var invalidRows = ValidRows.Where(row => !Validator.Validate(row.SeafoodFreshOrFrozen)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = String.Format("Fresh Or Frozen is not recognized.  Valid entries are {0}.", String.Join(", ", SeafoodFreshOrFrozenTypes.Descriptions.AsArray));
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateSeafoodWildOrFarmRaised()
        {
            Validator = new SeafoodCatchTypeValidator();

            var invalidRows = ValidRows.Where(row => !Validator.Validate(row.SeafoodWildOrFarmRaised)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = String.Format("Seafood: Wild Or Farm Raised is not recognized.  Valid entries are {0}.", String.Join(", ", SeafoodCatchTypes.Descriptions.AsArray));
                AddErrorRows(error, invalidRows);
            }
        }
     
        protected void ValidateVegetarian()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !string.IsNullOrEmpty(row.Vegetarian) && !Validator.Validate(row.Vegetarian)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Vegetarian should be Y, N, or blank.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void ValidateWholeTrade()
        {
            Validator = new YesNoValidator();

            var invalidRows = ValidRows.Where(row => !string.IsNullOrEmpty(row.WholeTrade) && !Validator.Validate(row.WholeTrade)).ToList();

            if (invalidRows.Count > 0)
            {
                string error = "Whole Trade should be Y, N, or blank.";
                AddErrorRows(error, invalidRows);
            }
        }

        private void CheckThatHierarchyClassExists()
        {
            var hierarchyLineageParameters = new GetHierarchyLineageParameters();

            var hierarchyLineage = getHierarchyLineageQuery.Search(hierarchyLineageParameters);

            List<string> brandHierarchyClassIds = hierarchyLineage.BrandHierarchyList.Select(b => b.HierarchyClassId.ToString()).ToList();
            List<string> merchHierarchyClassIds = hierarchyLineage.MerchandiseHierarchyList.Select(b => b.HierarchyClassId.ToString()).ToList();
            List<string> taxHierarchyClassIds = hierarchyLineage.TaxHierarchyList.Select(b => b.HierarchyClassId.ToString()).ToList();
            List<string> browsingHierarchyClassIds = hierarchyLineage.BrowsingHierarchyList.Select(b => b.HierarchyClassId.ToString()).ToList();
            List<string> nationalHierarchyClassIds = hierarchyLineage.NationalHierarchyList.Select(b => b.HierarchyClassId.ToString()).ToList();

            List<T> invalidRows = new List<T>();
            foreach (var row in ValidRows)
            {
                if (!IsEmptyOrNewHierarchyClass(row.BrandId) && !brandHierarchyClassIds.Contains(row.BrandId))
                {
                    string error = String.Format("Brand is invalid.  {0} does not exist.", row.BrandLineage);
                    row.Error = error;
                    invalidRows.Add(row);
                    continue;
                }

                if (!IsEmptyOrNewHierarchyClass(row.MerchandiseId) && !merchHierarchyClassIds.Contains(row.MerchandiseId))
                {
                    string error = String.Format("Merchandise is invalid.  {0} does not exist.", row.MerchandiseLineage);
                    row.Error = error;
                    invalidRows.Add(row);
                    continue;
                }

                if (!IsEmptyOrNewHierarchyClass(row.TaxId) && !taxHierarchyClassIds.Contains(row.TaxId))
                {
                    string error = String.Format("Tax is invalid.  {0} does not exist.", row.TaxLineage);
                    row.Error = error;
                    invalidRows.Add(row);
                    continue;
                }

                if (!IsEmptyOrNewHierarchyClass(row.BrowsingId) && !browsingHierarchyClassIds.Contains(row.BrowsingId))
                {
                    string error = String.Format("Browsing is invalid.  {0} does not exist.", row.BrowsingLineage);
                    row.Error = error;
                    invalidRows.Add(row);
                    continue;
                }

                if (!IsEmptyOrNewHierarchyClass(row.NationalId) && !nationalHierarchyClassIds.Contains(row.NationalId))
                {
                    string error = String.Format("National is invalid.  {0} does not exist.", row.NationalLineage);
                    row.Error = error;
                    invalidRows.Add(row);
                    continue;
                }
            }

            if (invalidRows.Any())
            {
                AddErrorRows(invalidRows);
            }
        }

        protected void CheckForTaxClassWithNoAbbreviation()
        {
            var parameters = new GetTaxHierarchyClassesWithNoAbbreviationParameters();

            var invalidTaxClasses = getTaxHierarchyClassesWithNoAbbreviationQuery.Search(parameters)
                .Select(hc => hc.hierarchyClassID.ToString());

            var invalidRows = ValidRows.Where(row => invalidTaxClasses.Any(hc => hc == row.TaxId)).ToList();

            if (invalidRows.Any())
            {
                string error = "Tax class has no abbreviation.";
                AddErrorRows(error, invalidRows);
            }
        }

        protected void CheckForAffinitySubBrickAssociations()
        {
            var parameters = new GetAffinitySubBricksParameters();

            var invalidSubBricks = getAffinitySubBricksQuery.Search(parameters)
                .Select(hc => hc.hierarchyClassID.ToString());

            var invalidRows = ValidRows.Where(row => invalidSubBricks.Any(mc => mc == row.MerchandiseId)).ToList();

            if (invalidRows.Any())
            {
                string error = "Items cannot be associated to an Affinity sub-brick.";
                AddErrorRows(error, invalidRows);
            }
        }

        /// <summary>
        /// Adds the invalid rows to the ErrorRows and removes them from ValidRows. Also sets the Error of those rows to the error parameter.
        /// </summary>
        /// <param name="error">The error foreach row.</param>
        /// <param name="invalidRows">The rows to add to ErrorRows and remove from ValidRows.</param>
        protected void AddErrorRows(string error, IEnumerable<T> invalidRows)
        {
            foreach (var invalidRow in invalidRows)
            {
                invalidRow.Error = error;
                ValidRows.Remove(invalidRow);
                ErrorRows.Add(invalidRow);
            }
        }

        /// <summary>
        /// Adds the invalid rows to the ErrorRows and removes them from ValidRows.
        /// </summary>
        /// <param name="invalidRows">The rows to add to ErrorRows and remove from ValidRows.</param>
        protected void AddErrorRows(IEnumerable<T> invalidRows)
        {
            foreach (var invalidRow in invalidRows)
            {
                ValidRows.Remove(invalidRow);
                ErrorRows.Add(invalidRow);
            }
        }

        protected string GetIdFromDescription(Dictionary<int, string> dictionary, string value)
        {
            if (dictionary.ContainsValue(value))
            {
                return dictionary.Single(ed => ed.Value == value).Key.ToString();
            }
            else
            {
                return null;
            }
        }

        private bool IsEmptyOrNewHierarchyClass(string hierarchyClassId)
        {
            return String.IsNullOrWhiteSpace(hierarchyClassId) || hierarchyClassId == "0";
        }
    }
}