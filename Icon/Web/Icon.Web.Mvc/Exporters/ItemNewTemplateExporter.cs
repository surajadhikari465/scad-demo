using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using Icon.Common;
using Icon.Common.Models;

namespace Icon.Web.Mvc.Exporters
{
    public class ItemNewTemplateExporter : BaseNewItemExporter<ItemViewModel>
    {
        private const string BarcodeType = "Barcode Type";
        private const string ScanCode = "Scan Code";
        private const int BooleanValidationRequiredCount = 2;
        private const int BooleanValidationNonRequiredCount = 3;

        public ItemNewTemplateExporter(
            IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>> getHierarchyClassesQueryHandler,
            IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributesQueryHandler,
            IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>> getBarcodeTypeQueryHandler)
            : base(getHierarchyClassesQueryHandler, getAttributesQueryHandler, getBarcodeTypeQueryHandler)
        {
        }

        public override void Export(List<Dictionary<string, object>> results = null)
        {
            base.BuildSpreadsheet();

            base.FormatExcelColumns(NewItemExcelHelper.ConsolidatedNewItemColumnIndexes.ScanCodeColumnIndex);
            CreateExcelValidationRules();

            // update hierarchy class name to full lineage
            if (results != null)
            {

                foreach (Dictionary<string, object> result in results)
                {

                    result["Brands"] = base.brandHierarchyClassDictionary[result[Constants.BrandsHierarchyClassId].ToString()];  //todo: fix Cosntant in Icon.Common
                    result[Constants.Tax] = base.taxHierarchyClassesDictionary[result[Constants.TaxHierarchyClassId].ToString()];
                    result[Constants.Merchandise] = base.merchandiseHierarchyClassDictionary[result[Constants.MerchandiseHierarchyClassId].ToString()];
                    result[Constants.National] = base.nationalHierarchyClassDictionary[result[Constants.NationalHierarchyClassId].ToString()];

                    if ((int)result[Constants.ManufacturerHierarchyClassId] > 0)
                    {
                        result[Constants.Manufacturer] = base.manufacturerHierarchyClassDictionary[result[Constants.ManufacturerHierarchyClassId].ToString()];
                    }
                }

                base.AddRows(results);
            }
        }

        protected override List<ExportItemModel> ConvertExportDataToExportItemModel()
        {
            // A template won't contain any data to convert.
            return new List<ExportItemModel>();
        }

        protected override void CreateExcelValidationRules()
        {
            base.CreateReadOnlyExcelValidationRule(BarcodeType);
            base.CreateHierarchyListRuleExcelValidationRule(HierarchyNames.Brands, NewItemExcelHelper.NewExcelExportColumnNames.Brand, base.brandHierarchyClassDictionary.Values.Count);
            base.CreateHierarchyListRuleExcelValidationRule(HierarchyNames.Merchandise, NewItemExcelHelper.NewExcelExportColumnNames.Merchandise, base.merchandiseHierarchyClassDictionary.Values.Count);
            base.CreateHierarchyListRuleExcelValidationRule(HierarchyNames.Tax, NewItemExcelHelper.NewExcelExportColumnNames.Tax, base.taxHierarchyClassesDictionary.Values.Count);
            base.CreateHierarchyListRuleExcelValidationRule(HierarchyNames.National, NewItemExcelHelper.NewExcelExportColumnNames.NationalClass, base.nationalHierarchyClassDictionary.Values.Count);
            base.CreateHierarchyListRuleExcelValidationRule(HierarchyNames.Manufacturer, NewItemExcelHelper.NewExcelExportColumnNames.Manufacturer, base.manufacturerHierarchyClassDictionary.Values.Count);
            base.CreateHierarchyListRuleExcelValidationRule(BarcodeType, NewItemExcelHelper.NewExcelExportColumnNames.BarCodeType, base.barCodeTypeDictionary.Values.Count);

            foreach (var name in new string[] { "Financial", "Item Type Description" })
            {
                base.CreateReadOnlyExcelValidationRule(name);
            }

            if (this.attributeList != null)
            {
                foreach (var item in this.attributeList.Where(x => x != null))
                {
                    if (item.IsReadOnly)
                    {
                        base.CreateReadOnlyExcelValidationRule(item.DisplayName);
                    }
                    else if (item.PickListData != null && item.PickListData.Any())
                    {
                        base.CreateListRuleExcelValidationRule(item.DisplayName, item.AttributeName, item.PickListData.Count());
                    }
                    else if(item.DataTypeName.Equals(Constants.DataTypeNames.Boolean, StringComparison.OrdinalIgnoreCase))
                    {
                        base.CreateListRuleExcelValidationRule(item.DisplayName, item.AttributeName, item.IsRequired ? BooleanValidationRequiredCount : BooleanValidationNonRequiredCount);
                    }
                }
            }
        }

        public override void AddSpreadsheetColumns()
        {
            
            if(SelectedColumnNames != null)
            {
                //Only export selected columns and keep them in order
                AddSpreadSheetColumnsCustomView();
                return;
            }

            int currentIndex = 0;
           
            AddSpreadsheetColumn(
                NewItemExcelHelper.NewExcelExportColumnNames.BarCodeType,
                ExportItemColumnNameMapper.keyToDisplayNameDictionay[NewItemExcelHelper.NewExcelExportColumnNames.BarCodeType],
                NewItemExcelHelper.NewExcelExportColumnWidths.BarCodeType,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[currentIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(
                NewItemExcelHelper.NewExcelExportColumnNames.ScanCode,
                ExportItemColumnNameMapper.keyToDisplayNameDictionay[NewItemExcelHelper.NewExcelExportColumnNames.ScanCode],
                NewItemExcelHelper.NewExcelExportColumnWidths.ScanCode,
                HorizontalCellAlignment.Right,
                (row, item) => row.Cells[currentIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(
                NewItemExcelHelper.NewExcelExportColumnNames.Brand,
                ExportItemColumnNameMapper.keyToDisplayNameDictionay[NewItemExcelHelper.NewExcelExportColumnNames.Brand],
                NewItemExcelHelper.NewExcelExportColumnWidths.HierarchyClass,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[currentIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(
                NewItemExcelHelper.NewExcelExportColumnNames.Merchandise,
                ExportItemColumnNameMapper.keyToDisplayNameDictionay[NewItemExcelHelper.NewExcelExportColumnNames.Merchandise],
                NewItemExcelHelper.NewExcelExportColumnWidths.HierarchyClass,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[currentIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(
                NewItemExcelHelper.NewExcelExportColumnNames.Tax,
                ExportItemColumnNameMapper.keyToDisplayNameDictionay[NewItemExcelHelper.NewExcelExportColumnNames.Tax],
                NewItemExcelHelper.NewExcelExportColumnWidths.HierarchyClass,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[currentIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(
                NewItemExcelHelper.NewExcelExportColumnNames.NationalClass,
                ExportItemColumnNameMapper.keyToDisplayNameDictionay[NewItemExcelHelper.NewExcelExportColumnNames.NationalClass],
                NewItemExcelHelper.NewExcelExportColumnWidths.HierarchyClass,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[currentIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(
                NewItemExcelHelper.NewExcelExportColumnNames.Financial,
                ExportItemColumnNameMapper.keyToDisplayNameDictionay[NewItemExcelHelper.NewExcelExportColumnNames.Financial],
                NewItemExcelHelper.NewExcelExportColumnWidths.Financial,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[currentIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(
                NewItemExcelHelper.NewExcelExportColumnNames.Manufacturer,
                ExportItemColumnNameMapper.keyToDisplayNameDictionay[NewItemExcelHelper.NewExcelExportColumnNames.Manufacturer],
                NewItemExcelHelper.NewExcelExportColumnWidths.Manufacturer,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[currentIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(
                NewItemExcelHelper.NewExcelExportColumnNames.ItemId,
                ExportItemColumnNameMapper.keyToDisplayNameDictionay[NewItemExcelHelper.NewExcelExportColumnNames.ItemId],
                NewItemExcelHelper.NewExcelExportColumnWidths.ItemId,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[currentIndex].Value = String.Empty,
                ref currentIndex);

            AddSpreadsheetColumn(
                NewItemExcelHelper.NewExcelExportColumnNames.ItemType,
                ExportItemColumnNameMapper.keyToDisplayNameDictionay[NewItemExcelHelper.NewExcelExportColumnNames.ItemType],
                NewItemExcelHelper.NewExcelExportColumnWidths.ItemType,
                HorizontalCellAlignment.Left,
                (row, item) => row.Cells[currentIndex].Value = String.Empty,
                ref currentIndex);

            EmptyQueryParameters<IEnumerable<AttributeModel>> attributesParam = new EmptyQueryParameters<IEnumerable<AttributeModel>>();
            var attributesModel = getAttributesQueryHandler.Search(attributesParam).OrderByDescending(a => a.IsRequired).ThenBy(a => a.DisplayOrder);

            foreach (var attributeModel in attributesModel.Where(a => a.AttributeGroupId != (int)AttributeType.Nutrition))
            {
                AddSpreadsheetColumn(
                    attributeModel.AttributeName,
                    attributeModel.DisplayName,
                    NewItemExcelHelper.NewExcelExportColumnWidths.AttributeNames,
                    HorizontalCellAlignment.Left,
                    (row, item) => row.Cells[currentIndex].Value = String.Empty,
                    ref currentIndex);
            }
        }

        /*
         * AddSpreadSheetColumnsCustomView allows the data to be exported with the column order
         * matching the order the columns are in on the grid in the UI.
         */
        private void AddSpreadSheetColumnsCustomView()
        {
            int columnIndex;
            //Removing Actions. That column does not get exported and would mess up the excel sheet
            if (SelectedColumnNames.Contains("Actions"))
            {
                SelectedColumnNames.Remove("Actions");
            }
            
            //Adding Column info to a list so we can loop through
            List<(string columnName,int columnWidth)> baseColumnInfo = new List<(string, int)>()
            {
                (NewItemExcelHelper.NewExcelExportColumnNames.BarCodeType, NewItemExcelHelper.NewExcelExportColumnWidths.BarCodeType),
                (NewItemExcelHelper.NewExcelExportColumnNames.Brand, NewItemExcelHelper.NewExcelExportColumnWidths.HierarchyClass),
                (NewItemExcelHelper.NewExcelExportColumnNames.Financial, NewItemExcelHelper.NewExcelExportColumnWidths.HierarchyClass),
                (NewItemExcelHelper.NewExcelExportColumnNames.ItemId, NewItemExcelHelper.NewExcelExportColumnWidths.ItemId),
                (NewItemExcelHelper.NewExcelExportColumnNames.ItemType, NewItemExcelHelper.NewExcelExportColumnWidths.ItemType),
                (NewItemExcelHelper.NewExcelExportColumnNames.Manufacturer, NewItemExcelHelper.NewExcelExportColumnWidths.Manufacturer),
                (NewItemExcelHelper.NewExcelExportColumnNames.Merchandise, NewItemExcelHelper.NewExcelExportColumnWidths.HierarchyClass),
                (NewItemExcelHelper.NewExcelExportColumnNames.ScanCode, NewItemExcelHelper.NewExcelExportColumnWidths.ScanCode),
                (NewItemExcelHelper.NewExcelExportColumnNames.Tax, NewItemExcelHelper.NewExcelExportColumnWidths.HierarchyClass),
                (NewItemExcelHelper.NewExcelExportColumnNames.NationalClass, NewItemExcelHelper.NewExcelExportColumnWidths.HierarchyClass)
            };

            foreach((string columnName, int columnWidth) in baseColumnInfo)
            {
                columnIndex = SelectedColumnNames.IndexOf(columnName);
                if (columnIndex >= 0)
                {
                    AddSpreadsheetColumn(
                        columnName,
                        ExportItemColumnNameMapper.keyToDisplayNameDictionay[columnName],
                        columnWidth,
                        HorizontalCellAlignment.Left,
                        (row, item) => row.Cells[columnIndex].Value = String.Empty,
                        ref columnIndex);
                }
            }

            EmptyQueryParameters<IEnumerable<AttributeModel>> attributesParam = new EmptyQueryParameters<IEnumerable<AttributeModel>>();
            var attributesModel = getAttributesQueryHandler.Search(attributesParam).OrderByDescending(a => a.IsRequired).ThenBy(a => a.DisplayOrder);

            foreach (var attributeModel in attributesModel.Where(a => a.AttributeGroupId != (int)AttributeType.Nutrition))
            {
                columnIndex = SelectedColumnNames.IndexOf(attributeModel.AttributeName);
                if (columnIndex >= 0)
                {
                    AddSpreadsheetColumn(
                        attributeModel.AttributeName,
                        attributeModel.DisplayName,
                        NewItemExcelHelper.NewExcelExportColumnWidths.AttributeNames,
                        HorizontalCellAlignment.Left,
                        (row, item) => row.Cells[columnIndex].Value = String.Empty,
                        ref columnIndex);
                }
            }
        }
    }
}