using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace Icon.Web.Mvc.Exporters
{
    public abstract class BaseNewContactExporter<T>
    {
        protected const int AsciiA = 65;
        protected const int AsciiZ = 90;
        protected const int MaxRowCount = 1048575;
        public List<T> ExportData { get; set; }
        public ExcelExportModel ExportModel { get; set; }

        protected Worksheet contactWorksheet;
        protected Worksheet hierarchyNameWorksheet;
        protected Worksheet brandWorksheet;
        protected Worksheet manufacturerWorksheet;
        protected Worksheet contactTypeWorksheet;

        protected Dictionary<string, string> hierarchyNameDictionary;
        protected Dictionary<string, string> contactTypeDictionary;
        protected Dictionary<string, string> brandHierarchyClassDictionary;
        protected Dictionary<string, string> manufacturerHierarchyClassDictionary;


        public List<SpreadsheetColumn<ContactExportViewModel>> spreadsheetColumns;

        protected IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler;
        protected IQueryHandler<GetContactsParameters, List<ContactModel>> getContactsQuery;
        protected IQueryHandler<GetContactTypesParameters, List<ContactTypeModel>> getContactTypeQuery;

        public BaseNewContactExporter(
            IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler,
            IQueryHandler<GetContactsParameters, List<ContactModel>> getContactsQuery,
            IQueryHandler<GetContactTypesParameters, List<ContactTypeModel>> getContactTypeQuery)
        {
            this.getContactsQuery = getContactsQuery;
            this.getHierarchyClassesQueryHandler = getHierarchyClassesQueryHandler;
            this.getContactTypeQuery = getContactTypeQuery;

            hierarchyNameDictionary = new Dictionary<string, string>();
            brandHierarchyClassDictionary = new Dictionary<string, string>();
            manufacturerHierarchyClassDictionary = new Dictionary<string, string>();
            contactTypeDictionary = new Dictionary<string, string>();

            spreadsheetColumns = new List<SpreadsheetColumn<ContactExportViewModel>>();
        }
        public abstract void Export(List<Dictionary<string, object>> results);
        protected abstract void CreateExcelValidationRules();
        protected abstract List<ContactExportViewModel> ConvertExportDataToExportContactModel();
        public abstract void AddSpreadsheetColumns();

        protected void BuildSpreadsheet()
        {
            contactWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Contact");
            hierarchyNameWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Hierarchy");
            brandWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Brands);
            manufacturerWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Manufacturer);
            contactTypeWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Contact Type");
           
            hierarchyNameWorksheet.DisplayOptions.Visibility = WorksheetVisibility.Hidden;
            brandWorksheet.DisplayOptions.Visibility = WorksheetVisibility.Visible;
            manufacturerWorksheet.DisplayOptions.Visibility = WorksheetVisibility.Visible;
            contactTypeWorksheet.DisplayOptions.Visibility = WorksheetVisibility.Hidden;

            BuildContactDictionaries();
            CreateContactWorksheets();
            CreateHeaderRow();
        }

        protected void AddSpreadsheetColumn(string headerKey, string headerTitle, int width, HorizontalCellAlignment alignment, Action<WorksheetRow, ContactExportViewModel> setValue, ref int currentIndex)
        {
            spreadsheetColumns.Add(new SpreadsheetColumn<ContactExportViewModel>
            {
                Index = currentIndex,
                HeaderTitle = headerTitle,
                HeaderBackground = CellFill.CreateSolidFill(Color.LightGreen),
                HeaderForeground = new WorkbookColorInfo(Color.Black),
                IsHeaderFontBold = ExcelDefaultableBoolean.True,
                Width = width,
                Alignment = alignment,
                SetValue = setValue
            });
            currentIndex++;
        }

        public int GetColumnByName(Worksheet ws, string columnName)
        {
            var cell = ws.Rows[0].Cells.Where(s => s.Value.ToString() == columnName).FirstOrDefault();
            return cell == null ? -1 : cell.ColumnIndex;
        }

        protected void CreateContactRuleExcelValidationRule(string excelColName, string gridColumnKey, int colCount)
        {
            if (colCount > 0)
            {
                int colIndex = GetColumnByName(contactWorksheet, excelColName);
                if (colIndex < 0)
                {
                    return;
                }

                var listRule = new ListDataValidationRule
                {
                    AllowNull = false,
                    ShowDropdown = true,
                    ErrorMessageDescription = "Invalid value entered.",
                    ErrorMessageTitle = "Validation Error",
                    ErrorStyle = DataValidationErrorStyle.Stop,
                    ShowErrorMessageForInvalidValue = true
                };

                string valuesFormula = String.Format("='{0}'!$A$1:$A${1}", excelColName, colCount + 1);

                listRule.SetValuesFormula(valuesFormula, null);

                var cellRegion = new WorksheetRegion(contactWorksheet, 1, colIndex, MaxRowCount, colIndex);
                var cellCollection = new WorksheetReferenceCollection(cellRegion);

                contactWorksheet.DataValidationRules.Add(listRule, cellCollection);
            }
        }
        protected void FormatExcelColumns(int columnIndex)
        {
            contactWorksheet.Columns[columnIndex].CellFormat.FormatString = "@";
        }

        internal void AddRows(IEnumerable<Dictionary<string, object>> results)
        {
            int row = 1;
            String[] columnHeaders = new String[spreadsheetColumns.Count()];

            int count = 0;
            foreach (SpreadsheetColumn<ContactExportViewModel> spreadsheetColumn in spreadsheetColumns.OrderBy(x => x.Index))
            {
                columnHeaders[count] = Regex.Replace(spreadsheetColumn.HeaderTitle, @"[^0-9a-zA-Z]+", "");
                count++;
            }

            foreach (Dictionary<string, object> result in results)
            {
                int col = 0;
                foreach (string columnHeader in columnHeaders)
                {
                    var key = columnHeader;

                    if (result.ContainsKey(columnHeader))
                        contactWorksheet.Rows[row].Cells[col].Value = result[columnHeader];
                    else
                        contactWorksheet.Rows[row].Cells[col].Value = string.Empty;
                    col++;
                }

                row = row + 1;
            }
        }

        protected string GetColumnPosition(int columnIndex)
        {
            int columnPositionAscii = AsciiA + columnIndex;
            int columnPositionOverflow = columnPositionAscii - AsciiZ;

            if (columnPositionOverflow > 0)
            {
                string secondCharacter = Convert.ToChar(AsciiA + (columnPositionOverflow - 1)).ToString();
                return "A" + secondCharacter;
            }
            else
            {
                return Convert.ToChar(AsciiA + columnIndex).ToString();
            }
        }

        private void CreateHeaderRow()
        {
            foreach (var column in spreadsheetColumns)
            {
                contactWorksheet.Rows[0].Cells[column.Index].Value = column.HeaderTitle;
                contactWorksheet.Rows[0].Cells[column.Index].CellFormat.Fill = column.HeaderBackground;
                contactWorksheet.Rows[0].Cells[column.Index].CellFormat.Font.ColorInfo = column.HeaderForeground;
                contactWorksheet.Rows[0].Cells[column.Index].CellFormat.Font.Bold = column.IsHeaderFontBold;
                contactWorksheet.Columns[column.Index].Width = column.Width;
                contactWorksheet.Columns[column.Index].CellFormat.Alignment = HorizontalCellAlignment.Left;
            }

            contactWorksheet.DisplayOptions.PanesAreFrozen = true;
            contactWorksheet.DisplayOptions.FrozenPaneSettings.FrozenRows = 1;
        }

        private void BuildContactDictionaries()
        {
            hierarchyNameDictionary = new Dictionary<string, string>() { { "Brands", "Brands" }, { "Manufacturer", "Manufacturer" } };

            IEnumerable<HierarchyClassModel> brandHierarchyModel = getHierarchyClassesQueryHandler.Search(new GetHierarchyClassesParameters
            {
                HierarchyId = Hierarchies.Brands
            });

            brandHierarchyClassDictionary = brandHierarchyModel
                .Select(brand => new { Key = brand.HierarchyClassId.ToString(), Value = brand.HierarchyLineage })
                .ToDictionary(b => b.Key, b => b.Value + " | " + b.Key);

            IEnumerable<HierarchyClassModel> manufacturerHierarchyModel = getHierarchyClassesQueryHandler.Search(new GetHierarchyClassesParameters
            {
                HierarchyId = Hierarchies.Manufacturer
            });

            manufacturerHierarchyClassDictionary = manufacturerHierarchyModel
                .Select(man => new { Key = man.HierarchyClassId.ToString(), Value = man.HierarchyLineage })
                .ToDictionary(m => m.Key, m => m.Value + " | " + m.Key);

            List<ContactTypeModel> contactTypeModel = getContactTypeQuery.Search(new GetContactTypesParameters());
            contactTypeDictionary = contactTypeModel
                .Select(con => new { Key = con.ContactTypeId.ToString(), Value = con.ContactTypeName })
                .ToDictionary(ct => ct.Key, ct => ct.Value);
        }

        private void CreateContactWorksheets()
        {
            CreateContactWorksheet(hierarchyNameDictionary, hierarchyNameWorksheet);
            CreateContactWorksheet(brandHierarchyClassDictionary, brandWorksheet);
            CreateContactWorksheet(manufacturerHierarchyClassDictionary, manufacturerWorksheet);
            CreateContactWorksheet(contactTypeDictionary, contactTypeWorksheet);

        }
        private void CreateContactWorksheet(Dictionary<string, string> contactClassDictionary, Worksheet contactClassWorksheet)
        {
            int i = 0;
            foreach (string hierarchyClass in contactClassDictionary.Values.OrderBy(hc => hc))
            {
                contactClassWorksheet.Rows[i].Cells[0].Value = hierarchyClass;
                i++;
            }
        }
    }
}