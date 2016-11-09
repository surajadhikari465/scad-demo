using Icon.Common.DataAccess;
using Icon.Web.Common;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Importers
{
    public class BrandSpreadsheetImporter : ISpreadsheetImporter<BulkImportBrandModel>
    {
        public Workbook Workbook { get; set; }
        public List<BulkImportBrandModel> ErrorRows { get; set; }
        public List<BulkImportBrandModel> ParsedRows { get; set; }
        public List<BulkImportBrandModel> ValidRows { get; set; }
        public IObjectValidator<string> Validator { get; set; }

        private IQueryHandler<GetExistingBrandsParameters, List<string>> getBrandsThatExistQuery;
        private IQueryHandler<GetBrandAbbreviationsThatExistParameters, List<string>> getBrandAbbreviationsThatExistQuery;
        private IQueryHandler<GetDuplicateBrandsByTrimmedNameParameters, List<string>> getDuplicateBrandsByTrimmedNameQuery;
        private string[] validHeaders;
        private int maxNumberOfRowsToImport;

        public BrandSpreadsheetImporter(IQueryHandler<GetExistingBrandsParameters, List<string>> getBrandsThatExistQuery,
            IQueryHandler<GetBrandAbbreviationsThatExistParameters, List<string>> getBrandAbbreviationsThatExistQuery,
            IQueryHandler<GetDuplicateBrandsByTrimmedNameParameters, List<string>> getDuplicateBrandsByTrimmedNameQuery)
        {
            this.getBrandsThatExistQuery = getBrandsThatExistQuery;
            this.getBrandAbbreviationsThatExistQuery = getBrandAbbreviationsThatExistQuery;
            this.getDuplicateBrandsByTrimmedNameQuery = getDuplicateBrandsByTrimmedNameQuery;
            ErrorRows = new List<BulkImportBrandModel>();
            ValidRows = new List<BulkImportBrandModel>();
            ParsedRows = new List<BulkImportBrandModel>();
            validHeaders = new[]
            {
                "Brand",
                "Brand Abbreviation"
            };
            maxNumberOfRowsToImport = 10000;
        }

        public bool IsValidSpreadsheetType()
        {
            var headerRow = Workbook.Worksheets[0].Rows[0];

            for (int i = 0; i < validHeaders.Length; i++)
            {
                var headerCell = headerRow.Cells[i].Value;
                if (headerCell == null || validHeaders[i] != headerCell.ToString())
                {
                    return false;
                }
            }

            return true;
        }

        public void ConvertSpreadsheetToModel()
        {
            var rows = Workbook.Worksheets[0].Rows
                .Skip(1)
                .Where(r => r.Cells.Any(c => c.Value != null && !string.IsNullOrEmpty(c.Value.ToString())))
                .Take(maxNumberOfRowsToImport)
                .ToList();

            foreach (var row in rows)
            {
                object brandLineage = row.Cells[0].Value;
                object brandAbbreviation = row.Cells[1].Value;

                BulkImportBrandModel importBrandModel = new BulkImportBrandModel
                {
                    BrandLineage = ExcelHelper.GetCellStringValue(brandLineage),
                    BrandId = ExcelHelper.GetCellStringValue(brandLineage).Split('|').Last().Trim(),
                    BrandName = ExcelHelper.GetCellStringValue(brandLineage).Split('|').First().Trim(),
                    BrandAbbreviation = ExcelHelper.GetCellStringValue(brandAbbreviation).Trim()
                };

                ParsedRows.Add(importBrandModel);
            }
        }

        public void ValidateSpreadsheetData()
        {
            ValidRows.AddRange(ParsedRows);

            CheckForRowsWithInvalidBrandColumn();

            if (ValidRows.Count > 0)
            {
                ValidateBrandName();
            }

            if (ValidRows.Count > 0)
            {
                CheckForDuplicateBrandsOnSpreadsheet();
            }

            if (ValidRows.Count > 0)
            {
                ValidateBrandAbbreviations();
            }

            if (ValidRows.Count > 0)
            {
                CheckForDuplicateBrandAbbreviationsOnSpreadsheet();
            }

            if (ValidRows.Count > 0)
            {
                ValidateNewBrandsDontExist();
            }

            if (ValidRows.Count > 0)
            {
                ValidateNewBrandsReducedToIrmaLimitDontExist();
            }

            if (ValidRows.Count > 0)
            {
                ValidateBrandAbbreviationsDontExist();
            }
        }

        private void CheckForRowsWithInvalidBrandColumn()
        {
            var invalidRows = ValidRows
                .Where(r => !r.BrandId.IsDigitsOnly() || String.IsNullOrWhiteSpace(r.BrandName))
                .ToList();

            string error = "Brand column is invalid. Must be in the format (Brand Name|Brand ID).";

            foreach (var invalidRow in invalidRows)
            {
                invalidRow.Error = error;
                ErrorRows.Add(invalidRow);
                ValidRows.Remove(invalidRow);
            }
        }

        private void ValidateBrandName()
        {
            IconPropertyValidationRules iconPropertyValidationRules = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.BrandName);

            var invalidRows = ValidRows
                .Where(r => !iconPropertyValidationRules.IsValid(r.BrandName, null))
                .ToList();

            string error = "Brand name is invalid. " + ValidatorErrorMessages.BrandNameError;

            foreach (var invalidRow in invalidRows)
            {
                invalidRow.Error = error;
                ErrorRows.Add(invalidRow);
                ValidRows.Remove(invalidRow);
            }
        }

        private void CheckForDuplicateBrandsOnSpreadsheet()
        {
            var duplicateBrandNames = ValidRows.GroupBy(r => r.BrandName)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .ToList();

            string error = "Brand name exists multiple times in spreadsheet.";

            foreach (var invalidRow in duplicateBrandNames)
            {
                invalidRow.Error = error;
                ErrorRows.Add(invalidRow);
                ValidRows.Remove(invalidRow);
            }

            var duplicateBrandIds = ValidRows.Where(r => r.BrandId != "0")
                .GroupBy(r => r.BrandId)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .ToList();

            error = "Brand ID exists multiple times in spreadsheet.";

            foreach (var invalidRow in duplicateBrandIds)
            {
                invalidRow.Error = error;
                ErrorRows.Add(invalidRow);
                ValidRows.Remove(invalidRow);
            }
        }

        private void ValidateBrandAbbreviations()
        {
            IconPropertyValidationRules iconPropertyValidationRules = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.BrandAbbreviation);

            var invalidRows = ValidRows
                .Where(r => !String.IsNullOrWhiteSpace(r.BrandAbbreviation) && !iconPropertyValidationRules.IsValid(r.BrandAbbreviation, null))
                .ToList();

            string error = "Brand abbreviation is invalid. " + ValidatorErrorMessages.BrandAbbreviationError;

            foreach (var invalidRow in invalidRows)
            {
                invalidRow.Error = error;
                ErrorRows.Add(invalidRow);
                ValidRows.Remove(invalidRow);
            }
        }

        private void CheckForDuplicateBrandAbbreviationsOnSpreadsheet()
        {
            var duplicateBrandAbbreviations = ValidRows
                .Where(r => !String.IsNullOrWhiteSpace(r.BrandAbbreviation))
                .GroupBy(r => r.BrandAbbreviation)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .ToList();

            string error = "Brand abbreviation exists multiple times on spreadsheet.";

            foreach (var invalidRow in duplicateBrandAbbreviations)
            {
                invalidRow.Error = error;
                ErrorRows.Add(invalidRow);
                ValidRows.Remove(invalidRow);
            }
        }

        private void ValidateNewBrandsDontExist()
        {
            GetExistingBrandsParameters parameters = new GetExistingBrandsParameters
                {
                    BrandNames = ValidRows
                        .Where(r => r.BrandId == "0")
                        .Select(r => r.BrandName)
                        .ToList()
                };

            var brandsThatExist = getBrandsThatExistQuery.Search(parameters);

            var invalidRows = ValidRows
                .Where(r => brandsThatExist.Contains(r.BrandName))
                .ToList();

            string error = "Brand name already exists in Icon.";

            foreach (var invalidRow in invalidRows)
            {
                invalidRow.Error = error;
                ErrorRows.Add(invalidRow);
                ValidRows.Remove(invalidRow);
            }
        }

        private void ValidateNewBrandsReducedToIrmaLimitDontExist()
        {
            var longBrandNames = ValidRows
                   .Where(row => row.BrandId == "0" && row.BrandName.Length > Constants.IrmaBrandNameMaxLength)
                   .Distinct()
                   .Select(row => row.BrandName).ToDictionary(bl => bl, bl => bl.Substring(0, Constants.IrmaBrandNameMaxLength));

            var invalidBrands = getDuplicateBrandsByTrimmedNameQuery.Search(new GetDuplicateBrandsByTrimmedNameParameters() { LongBrandNameList = longBrandNames });

            var invalidRows = ValidRows.Where(row => invalidBrands.Any(ib =>
                    ib.Equals(row.BrandName, StringComparison.InvariantCultureIgnoreCase)))
                    .ToList();

            string error = String.Format("Brand name reduced to {0} characters already exists in Icon. Adding brand name may cause conflicts with IRMA.", Constants.IrmaBrandNameMaxLength);

            foreach (var invalidRow in invalidRows)
            {
                invalidRow.Error = error;
                ErrorRows.Add(invalidRow);
                ValidRows.Remove(invalidRow);
            }
        }

        private void ValidateBrandAbbreviationsDontExist()
        {
            GetBrandAbbreviationsThatExistParameters parameters = new GetBrandAbbreviationsThatExistParameters
            {
                BrandAbbreviations = ValidRows
                    .Where(r => !String.IsNullOrWhiteSpace(r.BrandAbbreviation))
                    .Select(r => r.BrandAbbreviation)
                    .ToList()
            };

            var brandAbbreviationsThatExist = getBrandAbbreviationsThatExistQuery.Search(parameters);

            var invalidRows = ValidRows
                .Where(r => brandAbbreviationsThatExist.Contains(r.BrandAbbreviation))
                .ToList();

            string error = "Brand abbreviation already exists in Icon.";

            foreach (var invalidRow in invalidRows)
            {
                invalidRow.Error = error;
                ErrorRows.Add(invalidRow);
                ValidRows.Remove(invalidRow);
            }
        }
    }
}