using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Icon.Web.Mvc.Exporters
{
    public class DefaultTaxMismatchesExporter
    {
        private Worksheet worksheet;
        private Worksheet taxWorksheet;
        private Dictionary<string, string> taxHierarchyClassesDictionary;
        private IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler;

        private const int firstRow = 1;

        public ExcelExportModel ExportModel { get; set; }
        public List<DefaultTaxClassMismatchExportModel> ExportData { get; set; }

        public DefaultTaxMismatchesExporter(IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler)
        {
            this.getHierarchyLineageQueryHandler = getHierarchyLineageQueryHandler;
            taxHierarchyClassesDictionary = new Dictionary<string, string>();
        }

        public void Export()
        {
            if (ExportData == null)
            {
                throw new InvalidOperationException("ExportData must first be initialized by the calling method.");
            }

            bool displayErrorColumn = ExportData.Any(t => !String.IsNullOrWhiteSpace(t.Error));

            BuildHierarchyClassDictionaries();
            CreateWorksheets(displayErrorColumn);
            PopulateData(displayErrorColumn);
        }

        private void PopulateData(bool displayErrorColumn)
        {
            int i = 1;
            foreach (var row in ExportData)
            {
                worksheet.Rows[i].Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ScanCodeColumnIndex].Value = row.ScanCode;
                worksheet.Rows[i].Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.BrandColumnIndex].Value = row.Brand;
                worksheet.Rows[i].Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ProductDescriptionColumnIndex].Value = row.ProductDescription;
                worksheet.Rows[i].Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.MerchandiseLineage].Value = row.MerchandiseLineage;
                worksheet.Rows[i].Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.DefaultTaxClassColumnIndex].Value = row.DefaultTaxClass;
                worksheet.Rows[i].Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.TaxClassOverrideColumnIndex].Value = row.TaxClassOverride;

                if (displayErrorColumn)
                {
                    worksheet.Rows[i].Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ErrorColumnIndex].Value = row.Error;
                }

                i++;
            }

            CreateHierarchyExcelValidationRule(HierarchyNames.Tax, taxHierarchyClassesDictionary.Values.Count, firstRow, ExcelHelper.DefaultTaxMismatchesColumnIndexes.TaxClassOverrideColumnIndex, ExportData.Count);
        }

        private void CreateWorksheets(bool displayErrorColumn)
        {
            worksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Default Tax Class Mismatches");
            worksheet.Columns[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ScanCodeColumnIndex].CellFormat.FormatString = "@";
            SetUpHeaderRow(worksheet, displayErrorColumn);

            taxWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Tax);
            CreateHierarchyWorksheet(taxHierarchyClassesDictionary, taxWorksheet);
        }

        private void CreateHierarchyWorksheet(Dictionary<string, string> hierarchyClassDictionary, Worksheet hierarchyClassWorksheet)
        {
            int i = 0;
            foreach (string hierarchyClass in hierarchyClassDictionary.Values.OrderBy(hc => hc))
            {
                hierarchyClassWorksheet.Rows[i].Cells[0].Value = hierarchyClass;
                i++;
            }
        }

        private void BuildHierarchyClassDictionaries()
        {
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());

            taxHierarchyClassesDictionary = hierarchyListModel
                .TaxHierarchyList
                .Select(tax => new { Key = tax.HierarchyClassId.ToString(), Value = tax.HierarchyLineage })
                .ToDictionary(t => t.Key, t => t.Value + "|" + t.Key);
        }

        private void SetUpHeaderRow(Worksheet worksheet, bool displayErrorColumn)
        {
            SetUpColumn(worksheet, ExcelHelper.DefaultTaxMismatchesColumnIndexes.ScanCodeColumnIndex, ExcelHelper.ExcelExportColumnNames.ScanCode, ExcelHelper.ExcelExportColumnWidths.ScanCode);
            SetUpColumn(worksheet, ExcelHelper.DefaultTaxMismatchesColumnIndexes.BrandColumnIndex, ExcelHelper.ExcelExportColumnNames.Brand, ExcelHelper.ExcelExportColumnWidths.Brand);
            SetUpColumn(worksheet, ExcelHelper.DefaultTaxMismatchesColumnIndexes.ProductDescriptionColumnIndex, ExcelHelper.ExcelExportColumnNames.ProductDescription, ExcelHelper.ExcelExportColumnWidths.ProductDescription);
            SetUpColumn(worksheet, ExcelHelper.DefaultTaxMismatchesColumnIndexes.MerchandiseLineage, ExcelHelper.ExcelExportColumnNames.Merchandise, ExcelHelper.ExcelExportColumnWidths.Merchandise);
            SetUpColumn(worksheet, ExcelHelper.DefaultTaxMismatchesColumnIndexes.DefaultTaxClassColumnIndex, ExcelHelper.DefaultTaxMismatchesColumnNames.DefaultTaxClass, ExcelHelper.ExcelExportColumnWidths.Tax);
            SetUpColumn(worksheet, ExcelHelper.DefaultTaxMismatchesColumnIndexes.TaxClassOverrideColumnIndex, ExcelHelper.DefaultTaxMismatchesColumnNames.TaxClassOverride, ExcelHelper.ExcelExportColumnWidths.Tax);

            if (displayErrorColumn)
            {
                SetUpColumn(worksheet, ExcelHelper.DefaultTaxMismatchesColumnIndexes.ErrorColumnIndex, ExcelHelper.ExcelExportColumnNames.Error, ExcelHelper.ExcelExportColumnWidths.Error);
            }
        }

        private void SetUpColumn(Worksheet worksheet, int columnIndex, string headerTitle, int columnWidth)
        {
            worksheet.Columns[columnIndex].Width = columnWidth;
            worksheet.Columns[columnIndex].CellFormat.Alignment = HorizontalCellAlignment.Left;

            var headerCell = worksheet.Rows[0].Cells[columnIndex];

            headerCell.Value = headerTitle;
            headerCell.CellFormat.Fill = CellFill.CreateSolidFill(Color.LightGreen);
            headerCell.CellFormat.Font.ColorInfo = new WorkbookColorInfo(Color.Black);
            headerCell.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        }

        private void CreateHierarchyExcelValidationRule(string hierarchyName, int hierarchyClassCount, int firstRow, int column, int lastRow)
        {
            if (hierarchyClassCount > 0)
            {
                var listRule = new ListDataValidationRule();

                listRule.AllowNull = true;
                listRule.ShowDropdown = true;
                listRule.ErrorMessageDescription = "Invalid value entered.";
                listRule.ErrorMessageTitle = "Validation Error";
                listRule.ErrorStyle = DataValidationErrorStyle.Stop;
                listRule.ShowErrorMessageForInvalidValue = true;

                string valuesFormula = String.Format("={0}!$A$1:$A${1}", hierarchyName, hierarchyClassCount);

                listRule.SetValuesFormula(valuesFormula, null);

                var cellRegion = new WorksheetRegion(worksheet, firstRow, column, lastRow, column);
                var cellCollection = new WorksheetReferenceCollection(cellRegion);

                worksheet.DataValidationRules.Add(listRule, cellCollection);
            }
        }
    }
}
