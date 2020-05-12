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
using Icon.Common.Models;
using Icon.Common;
using System.Text.RegularExpressions;
using Icon.Web.Mvc.Utility;

namespace Icon.Web.Mvc.Exporters
{
    public abstract class BaseNewItemExporter<T>
    {
        protected const int AsciiA = 65;
        protected const int AsciiZ = 90;
        protected const int MaxRowCount = 1048575;
        protected const string RemovePickListValue = "REMOVE";
        private const string JsonTrue = "true";
        private const string JsonFalse = "false";
        private const string CreatedOnAttributeName = "CreatedDateTimeUtc";
        private const string ModifiedOnAttributeName = "ModifiedDateTimeUtc";
        private const string CreatedOnColumnName = "CreatedOn";
        private const string ModifiedOnColumnName = "ModifiedOn";

        public List<T> ExportData { get; set; }
        public ExcelExportModel ExportModel { get; set; }
        public List<string> SelectedColumnNames { get; set; }
        public bool ExportAllAttributes { get; set; }
        public bool ExportNewItemTemplate { get; set; }
        public List<string> ListHiddenColumnNames { get; set; }

        protected Worksheet itemsWorksheet;
        protected Worksheet brandWorksheet;
        protected Worksheet merchandiseWorksheet;
        protected Worksheet taxWorksheet;
        protected Worksheet nationalWorksheet;
        protected Worksheet manufacturerWorksheet;
        protected Worksheet barCodeTypeWorksheet;

        protected List<AttributeModel> attributeList;
        protected Dictionary<string, string> brandHierarchyClassDictionary;
        protected Dictionary<string, string> merchandiseHierarchyClassDictionary;
        protected Dictionary<string, string> taxHierarchyClassesDictionary;
        protected Dictionary<string, string> nationalHierarchyClassDictionary;
        protected Dictionary<string, string> financialHierarchyClassDictionary;
        protected Dictionary<string, string> manufacturerHierarchyClassDictionary;
        protected Dictionary<string, string> barCodeTypeDictionary;

        public List<SpreadsheetColumn<ExportItemModel>> spreadsheetColumns;
        protected IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler;
        protected IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler;
        protected IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypeQueryHandler;
        protected IQueryHandler<EmptyQueryParameters<List<ItemColumnOrderModel>>, List<ItemColumnOrderModel>> getItemColumnOrderQueryHandler;

        public BaseNewItemExporter(
            IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler,
            IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler,
            IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypeQueryHandler,
            IQueryHandler<EmptyQueryParameters<List<ItemColumnOrderModel>>, List<ItemColumnOrderModel>> getItemColumnOrderQueryHandler)
        {
            this.getHierarchyClassesQueryHandler = getHierarchyClassesQueryHandler;
            this.getAttributesQueryHandler = getAttributesQueryHandler;
            this.getBarcodeTypeQueryHandler = getBarcodeTypeQueryHandler;
            this.getItemColumnOrderQueryHandler = getItemColumnOrderQueryHandler;

            brandHierarchyClassDictionary = new Dictionary<string, string>();
            merchandiseHierarchyClassDictionary = new Dictionary<string, string>();
            taxHierarchyClassesDictionary = new Dictionary<string, string>();
            nationalHierarchyClassDictionary = new Dictionary<string, string>();
            manufacturerHierarchyClassDictionary = new Dictionary<string, string>();
            barCodeTypeDictionary = new Dictionary<string, string>();
            spreadsheetColumns = new List<SpreadsheetColumn<ExportItemModel>>();
        }
        public abstract void Export(List<Dictionary<string, object>> results);
        protected abstract void CreateExcelValidationRules();
        protected abstract List<ExportItemModel> ConvertExportDataToExportItemModel();
        public abstract void AddSpreadsheetColumns();

        protected void BuildSpreadsheet()
        {
            itemsWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Items");
            brandWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Brands);
            merchandiseWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Merchandise);
            taxWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Tax);
            nationalWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.National);
            manufacturerWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(HierarchyNames.Manufacturer);
            barCodeTypeWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Barcode Type");

            brandWorksheet.DisplayOptions.Visibility = WorksheetVisibility.Visible;
            merchandiseWorksheet.DisplayOptions.Visibility = WorksheetVisibility.Visible;
            taxWorksheet.DisplayOptions.Visibility = WorksheetVisibility.Visible;
            nationalWorksheet.DisplayOptions.Visibility = WorksheetVisibility.Visible;
            manufacturerWorksheet.DisplayOptions.Visibility = WorksheetVisibility.Visible;
            barCodeTypeWorksheet.DisplayOptions.Visibility = WorksheetVisibility.Hidden;

            BuildHierarchyClassDictionaries();
            CreateHierarchyWorksheets();

            attributeList = getAttributesQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>())
                .Where(x => x.AttributeGroupId != (int)AttributeType.Nutrition && x.IsActive)
                .OrderByDescending(a => a.IsRequired).ThenBy(a => a.DisplayOrder)
                .ToList();

            foreach (var attribute in attributeList.Where(x => x.PickListData != null && x.PickListData.Count() > 0))
            {
                var itemAttributeWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(attribute.AttributeName);
                itemAttributeWorksheet.DisplayOptions.Visibility = WorksheetVisibility.Hidden;
                CreateAttributesWorksheet(attribute, itemAttributeWorksheet);
            }
            foreach (var attribute in attributeList.Where(a => a.DataTypeName.Equals(Constants.DataTypeNames.Boolean, StringComparison.OrdinalIgnoreCase)))
            {
                var itemAttributeWorksheet = ExportModel.ExcelWorkbook.Worksheets.Add(attribute.AttributeName);
                itemAttributeWorksheet.DisplayOptions.Visibility = WorksheetVisibility.Hidden;
                CreateBooleanAttributesWorksheet(attribute, itemAttributeWorksheet);
            }
            CreateHeaderRow();
        }

        protected void AddSpreadsheetColumn(string headerKey, string headerTitle, int width, HorizontalCellAlignment alignment, Action<WorksheetRow, ExportItemModel> setValue, ref int currentIndex)
        {
            if ((!this.ExportNewItemTemplate && (this.ExportAllAttributes || this.SelectedColumnNames.Contains(headerKey))) || this.ExportNewItemTemplate && !this.ListHiddenColumnNames.Contains(headerKey))
            {
                spreadsheetColumns.Add(new SpreadsheetColumn<ExportItemModel>
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
        }

        public int GetColumnByName(Worksheet ws, string columnName)
        {
            var cell = ws.Rows[0].Cells.Where(s => s.Value.ToString() == columnName).FirstOrDefault();
            return cell == null ? -1 : cell.ColumnIndex;
        }

        protected void CreateHierarchyListRuleExcelValidationRule(string excelColName, string gridColumnKey, int colCount)
        {
            if ((this.ExportAllAttributes || this.SelectedColumnNames.Contains(gridColumnKey)) && colCount > 0)
            {
                int colIndex = GetColumnByName(itemsWorksheet, excelColName);
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

                var cellRegion = new WorksheetRegion(itemsWorksheet, 1, colIndex, MaxRowCount, colIndex);
                var cellCollection = new WorksheetReferenceCollection(cellRegion);

                itemsWorksheet.DataValidationRules.Add(listRule, cellCollection);
            }
        }

        protected void CreateListRuleExcelValidationRule(string excelColName, string gridColumnKey, int colCount)
        {
            if ((this.ExportAllAttributes || this.SelectedColumnNames.Contains(gridColumnKey)) && colCount > 0)
            {
                int colIndex = GetColumnByName(itemsWorksheet, excelColName);
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

                string valuesFormula = String.Format("='{0}'!$A$1:$A${1}", gridColumnKey, colCount + 1);

                listRule.SetValuesFormula(valuesFormula, null);

                var cellRegion = new WorksheetRegion(itemsWorksheet, 1, colIndex, MaxRowCount, colIndex);
                var cellCollection = new WorksheetReferenceCollection(cellRegion);

                itemsWorksheet.DataValidationRules.Add(listRule, cellCollection);
            }
        }

        protected void CreateReadOnlyExcelValidationRule(string excelColName)
        {
            int colIndex = GetColumnByName(itemsWorksheet, excelColName);
            if (colIndex < 0)
            {
                return;
            }

            var colName = GetColumnPosition(colIndex);

            var customRule = new CustomDataValidationRule
            {
                AllowNull = false,
                ErrorMessageDescription = "Read-Only value cannot be changed.",
                ErrorMessageTitle = "Read-Only field",
                ErrorStyle = DataValidationErrorStyle.Stop,
                ShowErrorMessageForInvalidValue = true
            };

            customRule.SetFormula($"=INDIRECT(\"{colName}\" & ROW())", null);
            itemsWorksheet.DataValidationRules.Add(customRule, new WorksheetRegion(itemsWorksheet, 1, colIndex, MaxRowCount, colIndex));
        }

        protected void FormatExcelColumns(int columnIndex)
        {
            itemsWorksheet.Columns[columnIndex].CellFormat.FormatString = "@";
        }

        internal void AddRows(IEnumerable<Dictionary<string, object>> results)
        {
            int row = 1;
            List<string> columnHeaders = spreadsheetColumns
                .OrderBy(x => x.Index)
                .Select(c => Regex.Replace(c.HeaderTitle, @"[^0-9a-zA-Z]+", ""))
                .ToList();
            bool containsCreatedOnColumnHeader = columnHeaders.Contains(CreatedOnColumnName);
            bool containsModifiedOnColumnHeader = columnHeaders.Contains(ModifiedOnColumnName);

            foreach (Dictionary<string, object> result in results)
            {
                int col = 0;
                foreach (string columnHeader in columnHeaders)
                {
                    var key = columnHeader;

                    if (result.ContainsKey(columnHeader))
                        itemsWorksheet.Rows[row].Cells[col].Value = result[columnHeader];
                    else
                        itemsWorksheet.Rows[row].Cells[col].Value = string.Empty;
                    col++;
                }

                //Created On and Modified On are handled differently than the other attributes. Hardcoding setting them here if they exist
                if (containsCreatedOnColumnHeader && result.ContainsKey(CreatedOnAttributeName))
                {
                    var index = columnHeaders.IndexOf(CreatedOnColumnName);
                    itemsWorksheet.Rows[row].Cells[index].Value = result[CreatedOnAttributeName];
                }

                if (containsModifiedOnColumnHeader && result.ContainsKey(ModifiedOnAttributeName))
                {
                    var index = columnHeaders.IndexOf(ModifiedOnColumnName);
                    itemsWorksheet.Rows[row].Cells[index].Value = result[ModifiedOnAttributeName];
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
                itemsWorksheet.Rows[0].Cells[column.Index].Value = column.HeaderTitle;
                itemsWorksheet.Rows[0].Cells[column.Index].CellFormat.Fill = column.HeaderBackground;
                itemsWorksheet.Rows[0].Cells[column.Index].CellFormat.Font.ColorInfo = column.HeaderForeground;
                itemsWorksheet.Rows[0].Cells[column.Index].CellFormat.Font.Bold = column.IsHeaderFontBold;
                itemsWorksheet.Columns[column.Index].Width = column.Width;
                itemsWorksheet.Columns[column.Index].CellFormat.Alignment = HorizontalCellAlignment.Left;
            }

            itemsWorksheet.DisplayOptions.PanesAreFrozen = true;
            itemsWorksheet.DisplayOptions.FrozenPaneSettings.FrozenRows = 1;
        }

        private void BuildHierarchyClassDictionaries()
        {
            IEnumerable<HierarchyClassModel> brandHierarchyModel = getHierarchyClassesQueryHandler.Search(new GetHierarchyClassesParameters
            {
                HierarchyId = Hierarchies.Brands
            });

            brandHierarchyClassDictionary = brandHierarchyModel
                .Select(brand => new { Key = brand.HierarchyClassId.ToString(), Value = brand.HierarchyLineage })
                .ToDictionary(b => b.Key, b => b.Value + " | " + b.Key);

            IEnumerable<HierarchyClassModel> merchandiseHierarchyModel = getHierarchyClassesQueryHandler.Search(new GetHierarchyClassesParameters
            {
                HierarchyId = Hierarchies.Merchandise
            });

            merchandiseHierarchyClassDictionary = merchandiseHierarchyModel
                .Select(merch => new { Key = merch.HierarchyClassId.ToString(), Value = merch.HierarchyLineage })
                .ToDictionary(m => m.Key, m => m.Value + " | " + m.Key);

            IEnumerable<HierarchyClassModel> taxHierarchyModel = getHierarchyClassesQueryHandler.Search(new GetHierarchyClassesParameters
            {
                HierarchyId = Hierarchies.Tax
            });

            taxHierarchyClassesDictionary = taxHierarchyModel
                .Select(tax => new { Key = tax.HierarchyClassId.ToString(), Value = tax.HierarchyLineage })
                .ToDictionary(t => t.Key, t => t.Value + " | " + t.Key);

            IEnumerable<HierarchyClassModel> nationalHierarchyModel = getHierarchyClassesQueryHandler.Search(new GetHierarchyClassesParameters
            {
                HierarchyId = Hierarchies.National
            });

            nationalHierarchyClassDictionary = nationalHierarchyModel
                .Select(nat => new { Key = nat.HierarchyClassId.ToString(), Value = nat.HierarchyLineage })
                .ToDictionary(nc => nc.Key, nc => nc.Value + " | " + nc.Key);

            IEnumerable<HierarchyClassModel> manufacturerHierarchyModel = getHierarchyClassesQueryHandler.Search(new GetHierarchyClassesParameters
            {
                HierarchyId = Hierarchies.Manufacturer
            });

            manufacturerHierarchyClassDictionary = manufacturerHierarchyModel
                .Select(man => new { Key = man.HierarchyClassId.ToString(), Value = man.HierarchyLineage })
                .ToDictionary(nc => nc.Key, nc => nc.Value + " | " + nc.Key);




            List<BarcodeTypeModel> listBarCodeModel = getBarcodeTypeQueryHandler.Search(new GetBarcodeTypeParameters());
            barCodeTypeDictionary = listBarCodeModel
                .Select(bar => new { Key = bar.BarcodeTypeId.ToString(), Value = bar.BarcodeType })
                .ToDictionary(br => br.Key, br => br.Value + " | " + br.Key);
        }

        private void CreateHierarchyWorksheets()
        {
            CreateHierarchyWorksheet(brandHierarchyClassDictionary, brandWorksheet);
            CreateHierarchyWorksheet(merchandiseHierarchyClassDictionary, merchandiseWorksheet);
            CreateHierarchyWorksheet(taxHierarchyClassesDictionary, taxWorksheet);
            CreateHierarchyWorksheet(nationalHierarchyClassDictionary, nationalWorksheet);
            CreateManufacturerHierarchyWorksheet(manufacturerHierarchyClassDictionary, manufacturerWorksheet);
            CreateHierarchyWorksheet(barCodeTypeDictionary, barCodeTypeWorksheet);
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

        private void CreateManufacturerHierarchyWorksheet(Dictionary<string, string> hierarchyClassDictionary, Worksheet hierarchyClassWorksheet)
        {
            hierarchyClassWorksheet.Rows[0].Cells[0].Value = RemovePickListValue;
            int i = 1;
            foreach (string hierarchyClass in hierarchyClassDictionary.Values.OrderBy(hc => hc))
            {
                hierarchyClassWorksheet.Rows[i].Cells[0].Value = hierarchyClass;
                i++;
            }
        }

        private void CreateAttributesWorksheet(AttributeModel model, Worksheet attributesWorksheet)
        {
            int i = -1;
            if (!model.IsRequired) attributesWorksheet.Rows[++i].Cells[0].Value = RemovePickListValue; //Available for non-required attributes only

            foreach (var item in model.PickListData.OrderBy(x => x.PickListValue))
            {
                attributesWorksheet.Rows[++i].Cells[0].Value = item.PickListValue;
            }
        }

        private void CreateBooleanAttributesWorksheet(AttributeModel model, Worksheet attributesWorksheet)
        {
            int i = 0;
            if (!model.IsRequired) attributesWorksheet.Rows[i++].Cells[0].Value = RemovePickListValue; //Available for non-required attributes only

            attributesWorksheet.Rows[i++].Cells[0].Value = JsonTrue;
            attributesWorksheet.Rows[i++].Cells[0].Value = JsonFalse;
        }
    }
}