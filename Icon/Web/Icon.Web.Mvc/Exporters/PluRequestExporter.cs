using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Icon.Web.Mvc.Exporters
{
    public class PluRequestExporter
    {
        protected const int AsciiA = 65;
        protected const int ScanCodeColumnIndex = 0;
        protected const int BrandColumnIndex = 1;
        protected const int ProductDescriptionColumnIndex = 2;
        protected const int PosDescriptionColumnIndex = 3;
        protected const int PackageUnitColumnIndex = 4;
        protected const int SizeColumnIndex = 5;
        protected const int UomColumnIndex = 6;
        protected const int SubTeamColumnIndex = 7;
        protected const int MerchandiseColumnIndex = 8;
        protected const int NationalColumnIndex = 9;
        protected const int RequestStatusColumnIndex = 10;
        protected const int RequestedByColumnIndex = 11;

        private Worksheet pluWorksheet;

        protected Worksheet brandWorksheet;
        protected Worksheet merchandiseWorksheet;
        protected Worksheet subTeamWorksheet;
        protected Worksheet merchTaxMapWorksheet;
        protected Worksheet nationalWorksheet;
        protected Dictionary<string, string> brandHierarchyClassDictionary;
        protected Dictionary<string, string> merchandiseHierarchyClassDictionary;
        protected Dictionary<string, string> nationalHierarchyClassDictionary;
        protected Dictionary<string, string> subTeamHierarchyClassDictionary;
        protected List<SpreadsheetColumn<ExportPluRequestModel>> spreadsheetColumns;
        protected IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler;

        protected string brandColumnPosition = Convert.ToChar(AsciiA + BrandColumnIndex).ToString();
        protected string merchandiseColumnPosition = Convert.ToChar(AsciiA + MerchandiseColumnIndex).ToString();
        protected string subTeamColumnPosition = Convert.ToChar(AsciiA + SubTeamColumnIndex).ToString();
        protected string nationalClassColumnPosition = Convert.ToChar(AsciiA + NationalColumnIndex).ToString();

        public List<PluRequestViewModel> ExportData { get; set; }
        public ExcelExportModel ExportModel { get; set; }


        public PluRequestExporter(IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler)
        {
            this.getHierarchyLineageQueryHandler = getHierarchyLineageQueryHandler;

            brandHierarchyClassDictionary = new Dictionary<string, string>();
            merchandiseHierarchyClassDictionary = new Dictionary<string, string>();
            nationalHierarchyClassDictionary = new Dictionary<string, string>();
            subTeamHierarchyClassDictionary = new Dictionary<string, string>();

            spreadsheetColumns = new List<SpreadsheetColumn<ExportPluRequestModel>>();

            AddSpreadsheetColumns();
        }

        private List<ExportPluRequestModel> ConvertExportDataToExportItemModel()
        {
            // IRMA Item exports do not contain the hierarchyClassID of the brand, which is required for the New Item importer.  Given the brand
            // name, its ID can be determined from the dictionary in the base class.
            string brandId;

            foreach (var viewModel in ExportData)
            {
                brandId = brandHierarchyClassDictionary.SingleOrDefault(b => b.Value.Split('|')[0].Equals(viewModel.BrandName, StringComparison.InvariantCultureIgnoreCase)).Key;

                if (brandId == null)
                {
                    viewModel.BrandId = 0;
                }
                else
                {
                    viewModel.BrandId = Int32.Parse(brandId);
                }
            }

            List<ExportPluRequestModel> exportItems = ExportData.Select(i => new ExportPluRequestModel
            {
                NationalPLU = i.NationalPLU,
                BrandName = brandHierarchyClassDictionary.ContainsKey(i.BrandId.ToString()) ? brandHierarchyClassDictionary[i.BrandId.ToString()] : String.Join("|", i.BrandName, i.BrandId.ToString()),
                ProductDescription = i.ItemDescription,
                PosDescription = i.PosDescription,
                PackageUnit = i.PackageUnit.ToString(),
                RetailSize = i.RetailSize.ToString(),
                RetailUom = i.RetailUom,
                SubTeam = subTeamHierarchyClassDictionary.ContainsKey(i.FinancialClassID.ToString()) ? subTeamHierarchyClassDictionary[i.FinancialClassID.ToString()] : String.Empty,
                Merchandise = merchandiseHierarchyClassDictionary.ContainsKey(i.MerchandiseClassID.ToString()) ? merchandiseHierarchyClassDictionary[i.MerchandiseClassID.ToString()] : String.Empty,
                National = nationalHierarchyClassDictionary.ContainsKey(i.NationalClassID.ToString()) ? nationalHierarchyClassDictionary[i.NationalClassID.ToString()] : String.Empty,
                RequestStatus = i.RequestStatus,
                RequestedUser = i.RequesterName
            })
            .ToList();

            return exportItems;
        }

        protected void CreateExcelValidationRules(int row)
        {
            CreateExcelValidationRule(HierarchyNames.Merchandise, merchandiseColumnPosition, merchandiseHierarchyClassDictionary.Values.Count, row, merchandiseWorksheet);
            CreateExcelValidationRule(HierarchyNames.National, nationalClassColumnPosition, nationalHierarchyClassDictionary.Values.Count, row, nationalWorksheet);
            CreateExcelValidationRule(HierarchyNames.Financial, subTeamColumnPosition, subTeamHierarchyClassDictionary.Values.Count, row, subTeamWorksheet);
        }

        public void Export()
        {
            BuildSpreadsheet();

            var exportItemData = ConvertExportDataToExportItemModel();

            // Start at 1 to exclude the header row.
            int i = 1;
            foreach (ExportPluRequestModel item in exportItemData)
            {
                foreach (var column in spreadsheetColumns)
                {
                    column.SetValue(pluWorksheet.Rows[i], item);
                }

                // When calling this method, incremement i since the object row array is 0-indexed, but the actual spreadsheet is 1-indexed.
                CreateExcelValidationRules(i + 1);

                i++;
            }
        }

        protected void BuildSpreadsheet()
        {
            pluWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("PLURequests");
            brandWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Brands);
            merchandiseWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Merchandise);
            nationalWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.National);
            subTeamWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Financial);

            BuildHierarchyClassDictionaries();
            CreateHierarchyWorksheets();
            CreateHeaderRow();
            FormatExcelColumns();
        }

        protected void AddSpreadsheetColumn(int columnIndex, string headerTitle, int width, HorizontalCellAlignment alignment, Action<WorksheetRow, ExportPluRequestModel> setValue)
        {
            spreadsheetColumns.Add(new SpreadsheetColumn<ExportPluRequestModel>
            {
                Index = columnIndex,
                HeaderTitle = headerTitle,
                HeaderBackground = CellFill.CreateSolidFill(Color.LightGreen),
                HeaderForeground = new WorkbookColorInfo(Color.Black),
                IsHeaderFontBold = ExcelDefaultableBoolean.True,
                Width = width,
                Alignment = alignment,
                SetValue = setValue
            });
        }

        protected void RemoveSpreadsheetColumn(int columnIndex)
        {
            var columnToRemove = spreadsheetColumns.FirstOrDefault(c => c.Index == columnIndex);
            if (columnToRemove != null)
            {
                spreadsheetColumns.Remove(columnToRemove);
            }
        }

        protected void RemoveSpreadsheetColumn(string columnName)
        {
            var columnToRemove = spreadsheetColumns.FirstOrDefault(c => c.HeaderTitle == columnName);
            if (columnToRemove != null)
            {
                spreadsheetColumns.Remove(columnToRemove);
            }
        }

        protected void CreateExcelValidationRule(string hierarchyName, string hierarchyPosition, int hierarchyClassCount, int row, Worksheet worksheet)
        {
            if (hierarchyClassCount > 0)
            {
                ListDataValidationRule listRule = new ListDataValidationRule();
                WorksheetReferenceCollection cellCollection = null;

                listRule.AllowNull = true;
                listRule.ShowDropdown = true;
                listRule.ErrorMessageDescription = "Invalid value entered.";
                listRule.ErrorMessageTitle = "Validation Error";
                listRule.ErrorStyle = DataValidationErrorStyle.Stop;
                listRule.ShowErrorMessageForInvalidValue = true;

                string valuesFormula = String.Format("={0}!A1:A{1}", hierarchyName, hierarchyClassCount);
                string address = String.Format("{0}{1}", hierarchyPosition, row);

                listRule.SetValuesFormula(valuesFormula, address);
                cellCollection = new WorksheetReferenceCollection(pluWorksheet, String.Format("{0}{1}", hierarchyPosition, row));

                pluWorksheet.DataValidationRules.Add(listRule, cellCollection);
            }
        }

        private void AddSpreadsheetColumns()
        {
            AddSpreadsheetColumn(ScanCodeColumnIndex, "National PLU", 8000, HorizontalCellAlignment.Left, (row, item) => row.Cells[ScanCodeColumnIndex].Value = item.NationalPLU);

            AddSpreadsheetColumn(BrandColumnIndex, "Brand", 8000, HorizontalCellAlignment.Left, (row, item) => row.Cells[BrandColumnIndex].Value = item.BrandName);

            AddSpreadsheetColumn(ProductDescriptionColumnIndex, "Product Description", 14000, HorizontalCellAlignment.Left, (row, item) => row.Cells[ProductDescriptionColumnIndex].Value = item.ProductDescription);

            AddSpreadsheetColumn(PosDescriptionColumnIndex, "POS Description", 9000, HorizontalCellAlignment.Left, (row, item) => row.Cells[PosDescriptionColumnIndex].Value = item.PosDescription);

            AddSpreadsheetColumn(PackageUnitColumnIndex, "Item Pack", 3200, HorizontalCellAlignment.Left, (row, item) => row.Cells[PackageUnitColumnIndex].Value = item.PackageUnit);

            AddSpreadsheetColumn(SizeColumnIndex, "Size", 3500, HorizontalCellAlignment.Left, (row, item) => row.Cells[SizeColumnIndex].Value = item.RetailSize);

            AddSpreadsheetColumn(UomColumnIndex, "UOM", 3500, HorizontalCellAlignment.Left, (row, item) => row.Cells[UomColumnIndex].Value = item.RetailUom);

            AddSpreadsheetColumn(MerchandiseColumnIndex, "Merchandise Hierarchy", 27000, HorizontalCellAlignment.Left, (row, item) => row.Cells[MerchandiseColumnIndex].Value = item.Merchandise);

            AddSpreadsheetColumn(SubTeamColumnIndex, "SubTeam", 15000, HorizontalCellAlignment.Left, (row, item) => row.Cells[SubTeamColumnIndex].Value = item.SubTeam);

            AddSpreadsheetColumn(NationalColumnIndex, "National Class", 15000, HorizontalCellAlignment.Left, (row, item) => row.Cells[NationalColumnIndex].Value = item.National);

            AddSpreadsheetColumn(RequestStatusColumnIndex, "Status", 3200, HorizontalCellAlignment.Left, (row, item) => row.Cells[RequestStatusColumnIndex].Value = item.RequestStatus);

            AddSpreadsheetColumn(RequestedByColumnIndex, "Requested By", 8000, HorizontalCellAlignment.Left, (row, item) => row.Cells[RequestedByColumnIndex].Value = item.RequestedUser);
        }

        private void CreateHeaderRow()
        {
            foreach (var column in spreadsheetColumns)
            {
                pluWorksheet.Rows[0].Cells[column.Index].Value = column.HeaderTitle;
                pluWorksheet.Rows[0].Cells[column.Index].CellFormat.Fill = column.HeaderBackground;
                pluWorksheet.Rows[0].Cells[column.Index].CellFormat.Font.ColorInfo = column.HeaderForeground;
                pluWorksheet.Rows[0].Cells[column.Index].CellFormat.Font.Bold = column.IsHeaderFontBold;
                pluWorksheet.Columns[column.Index].Width = column.Width;
                pluWorksheet.Columns[column.Index].CellFormat.Alignment = column.Alignment;
            }
        }

        private void BuildHierarchyClassDictionaries()
        {
            HierarchyClassListModel hierarchyListModel = getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters());

            brandHierarchyClassDictionary = hierarchyListModel
                .BrandHierarchyList
                .Select(brand => new { Key = brand.HierarchyClassId.ToString(), Value = brand.HierarchyLineage })
                .ToDictionary(b => b.Key, b => b.Value + "|" + b.Key);

            merchandiseHierarchyClassDictionary = hierarchyListModel
                .MerchandiseHierarchyList
                .Select(merch => new { Key = merch.HierarchyClassId.ToString(), Value = merch.HierarchyLineage })
                .ToDictionary(m => m.Key, m => m.Value + "|" + m.Key);

            nationalHierarchyClassDictionary = hierarchyListModel
                .NationalHierarchyList
                .Select(nat => new { Key = nat.HierarchyClassId.ToString(), Value = nat.HierarchyLineage })
                .ToDictionary(br => br.Key, br => br.Value + "|" + br.Key);

            subTeamHierarchyClassDictionary = hierarchyListModel
                .FinancialHierarchyList
                .Select(nat => new { Key = nat.HierarchyClassId.ToString(), Value = nat.HierarchyLineage })
                .ToDictionary(br => br.Key, br => br.Value + "|" + br.Key);
        }

        private void CreateHierarchyWorksheets()
        {
            CreateHierarchyWorksheet(brandHierarchyClassDictionary, brandWorksheet);
            CreateHierarchyWorksheet(merchandiseHierarchyClassDictionary, merchandiseWorksheet);
            CreateHierarchyWorksheet(nationalHierarchyClassDictionary, nationalWorksheet);
            CreateHierarchyWorksheet(subTeamHierarchyClassDictionary, subTeamWorksheet);
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

        private void CreateHierarchyMappingWorksheet(Dictionary<string, string> hierarchyClassMappingDictionary, Worksheet hierarchyClassWorksheet)
        {
            int i = 0;

            hierarchyClassWorksheet.Rows[i].Cells[0].Value = "Merchandise";
            hierarchyClassWorksheet.Rows[i].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            hierarchyClassWorksheet.Columns[0].Width = 27000;
            hierarchyClassWorksheet.Columns[0].CellFormat.Alignment = HorizontalCellAlignment.Left;

            hierarchyClassWorksheet.Rows[i].Cells[1].Value = "Tax";
            hierarchyClassWorksheet.Rows[i].Cells[1].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            hierarchyClassWorksheet.Columns[1].Width = 15000;
            hierarchyClassWorksheet.Columns[1].CellFormat.Alignment = HorizontalCellAlignment.Left;
            
            i++;

            foreach (string hierarchyClass in hierarchyClassMappingDictionary.Keys.OrderBy(hc => hc))
            {
                hierarchyClassWorksheet.Rows[i].Cells[0].Value = hierarchyClass;
                hierarchyClassWorksheet.Rows[i].Cells[1].Value = hierarchyClassMappingDictionary[hierarchyClass];
                i++;
            }
        }

        private void FormatExcelColumns()
        {
            pluWorksheet.Columns[ScanCodeColumnIndex].CellFormat.FormatString = "@";
        }
    }
}