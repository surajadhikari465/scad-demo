using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Exporters
{
    public class BrandTemplateExporter : BaseHierarchyClassExporter<BrandExportViewModel>
    {
        private const int BrandNameIndex = 0;
        private const int BrandIdIndex = 1;
        private const int BrandAbbreviationIndex = 2;
        private const int DesignationIndex = 3;
        private const int ParentCompanyIndex = 4;
        private const int ZipCodeIndex = 5;
        private const int LocalityIndex = 6;
        private const int MaxRowCount = 1048575;

        private IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler;

        public BrandTemplateExporter(IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler)
            : base()
        {
            this.getHierarchyClassesQueryHandler = getHierarchyClassesQueryHandler;

            AddSpreadsheetColumn(BrandNameIndex,
                "Brand Name",
                4000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[BrandNameIndex].Value =
                    (string.IsNullOrWhiteSpace(hierarchyClass.BrandName))
                        ? string.Empty
                        : hierarchyClass.BrandName);

            AddSpreadsheetColumn(BrandIdIndex,
                "Brand ID",
                4000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[BrandIdIndex].Value =
                    (string.IsNullOrWhiteSpace(hierarchyClass.BrandId))
                        ? string.Empty
                        : hierarchyClass.BrandId);

            AddSpreadsheetColumn(BrandAbbreviationIndex,
                "Brand Abbreviation",
                5000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[BrandAbbreviationIndex].Value = hierarchyClass.BrandAbbreviation);

            AddSpreadsheetColumn(DesignationIndex,
                "Designation",
                5000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[DesignationIndex].Value = hierarchyClass.Designation);

            AddSpreadsheetColumn(ParentCompanyIndex,
                "Parent Company",
                5000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[ParentCompanyIndex].Value = hierarchyClass.ParentCompany);

            AddSpreadsheetColumn(ZipCodeIndex,
                "Zip Code",
                5000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[ZipCodeIndex].Value = hierarchyClass.ZipCode);

            AddSpreadsheetColumn(LocalityIndex,
                "Locality",
                5000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[LocalityIndex].Value = hierarchyClass.Locality);
        }

        public override void Export()
        {
            BuildSpreadsheet();

            var referenceBrandsWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Reference Brands");
            var referenceBrands = getHierarchyClassesQueryHandler.Search(new GetHierarchyClassesParameters { HierarchyId = Hierarchies.Brands })
                .OrderBy(b => b.HierarchyClassName)
                .ToList();
            for (int i = 0; i < referenceBrands.Count; i++)
            {
                referenceBrandsWorksheet.Rows[i].Cells[0].Value = referenceBrands[i].HierarchyClassName;
                referenceBrandsWorksheet.Rows[i].Cells[1].Value = referenceBrands[i].HierarchyClassId;
            }

            List<string> designations = new List<string> { "REMOVE", "Global", "Regional" };
            List<string> parentCompanies = referenceBrands
                .Select(b => b.HierarchyClassName)
                .Prepend("REMOVE")
                .ToList();

            var designationWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Designation");
            for (int i = 0; i < designations.Count; i++)
            {
                designationWorksheet.Rows[i].Cells[0].Value = designations[i];
            }

            var parentCompanyWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("ParentCompany");
            for (int i = 0; i < parentCompanies.Count; i++)
            {
                parentCompanyWorksheet.Rows[i].Cells[0].Value = parentCompanies[i];
            }

            CreateListRuleExcelValidationRule(worksheet, DesignationIndex, "Designation", designations);
            CreateListRuleExcelValidationRule(worksheet, ParentCompanyIndex, "ParentCompany", parentCompanies);
        }

        protected override List<BrandExportViewModel> ConvertExportDataToExportHierarchyClassModel()
        {
            throw new NotImplementedException();
        }

        protected void CreateListRuleExcelValidationRule(Worksheet worksheet, int colIndex, string gridColumnKey, List<string> values)
        {
            var listRule = new ListDataValidationRule
            {
                AllowNull = false,
                ShowDropdown = true,
                ErrorMessageDescription = "Invalid value entered.",
                ErrorMessageTitle = "Validation Error",
                ErrorStyle = DataValidationErrorStyle.Stop,
                ShowErrorMessageForInvalidValue = true
            };

            string valuesFormula = string.Format("='{0}'!$A$1:$A${1}", gridColumnKey, values.Count + 1);

            listRule.SetValuesFormula(valuesFormula, null);

            var cellRegion = new WorksheetRegion(worksheet, 1, colIndex, MaxRowCount, colIndex);
            var cellCollection = new WorksheetReferenceCollection(cellRegion);

            worksheet.DataValidationRules.Add(listRule, cellCollection);
        }
    }
}