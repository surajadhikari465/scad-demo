using System;
using System.Collections.Generic;
using OfficeOpenXml;
using System.Linq;
using BulkItemUploadProcessor.Common.Models;

namespace BulkItemUploadProcessor.Common
{
    public static class Extensions
    {
        public static ExcelRange GetRowData(this ExcelWorksheet sheet, int rowId)
        {
            return sheet.Cells[$"{rowId}:{rowId}"];
        }

        public static NewItemViewModel ToNewItemViewModel(this ItemDbModel item)
        {
            return new NewItemViewModel(item);
        }

        public static NewItemViewModel CreateItemModel(this RowObject row, string[] nonAttributeColumns)
        {
            var item = new NewItemViewModel();
            item.RowId = row.Row;
            try
            {
                item.ScanCode = row.Cells.FirstOrDefault(c => c.Column.Name == "Scan Code")?.CellValue.ToString();

                item.BrandHierarchyName =
                    row.Cells.FirstOrDefault(c => c.Column.Name == "Brands")?.CellValue.ToString();
                item.BrandHierarchyClassId = item.BrandHierarchyName.ParseClassId();

                item.NationalHierarchyName =
                    row.Cells.FirstOrDefault(c => c.Column.Name == "National")?.CellValue.ToString();
                item.NationalHierarchyClassId = item.NationalHierarchyName.ParseClassId();

                item.MerchandiseHierarchyName = row.Cells.FirstOrDefault(c => c.Column.Name == "Merchandise")?.CellValue
                    .ToString();
                item.MerchandiseHierarchyClassId = item.MerchandiseHierarchyName.ParseClassId();

                item.TaxHierarchyName = row.Cells.FirstOrDefault(c => c.Column.Name == "Tax")?.CellValue.ToString();
                item.TaxHierarchyClassId = item.TaxHierarchyName.ParseClassId();


                item.FinancialHierarchyName = row.Cells.FirstOrDefault(c => c.Column.Name == "Financial")?.CellValue.ToString();
                item.FinancialHierarchyClassId = item.FinancialHierarchyName.ParseClassId();

                //optional
                item.ManufacturerHierarchyName = row.Cells.FirstOrDefault(c => c.Column.Name == "Manufacturer")?.CellValue.ToString();
                if (!string.IsNullOrWhiteSpace(item.ManufacturerHierarchyName))
                {
                    if (item.ManufacturerHierarchyName.ToLower() == "remove")
                    {
                        item.ManufacturerHierarchyClassId = "0";
                    }
                    else
                    {
                        item.ManufacturerHierarchyClassId = item.ManufacturerHierarchyName.ParseClassId();
                    }
                }
                else
                {
                    item.ManufacturerHierarchyClassId = "";
                }

                if (string.IsNullOrWhiteSpace(item.MerchandiseHierarchyClassId)) item.MerchandiseHierarchyClassId = "0";



                item.BarcodeTypeName = row.Cells.FirstOrDefault(c => c.Column.Name == "Barcode Type")?.CellValue.ToString();
                item.BarcodeTypeId = item.BarcodeTypeName.ParseClassId();
                item.BarcodeType = item.BarcodeTypeName.ParseName();

                item.ItemAttributes = new Dictionary<string, string>();

                foreach (var attributeCell in row.Cells.Where(c => !nonAttributeColumns.Contains(c.Column.Name)))
                {
                    //if cell has no value. or has "remove" as a value. ignore it. it will be removed from the item.
                    if (!string.IsNullOrWhiteSpace(attributeCell.CellValue.ToString()) && !attributeCell.CellValue.ToString().ToLower().Equals("remove"))
                    {
                        item.ItemAttributes.Add(attributeCell.Column.Name, attributeCell.CellValue.ToString());
                    }

                }

                if (!item.ItemAttributes.ContainsKey("Inactive")) item.ItemAttributes.Add("Inactive", "false");

                // make sure boolean values are lowercase 

                var boolValues = new[] { "true", "false" };
                foreach (var kvp in item.ItemAttributes.Where(a => boolValues.Contains(a.Value.ToLower())).ToList())
                {
                    item.ItemAttributes[kvp.Key] = kvp.Value.ToLower();
                }


            }
            catch (Exception ex)
            {
                item.Errors.Add($"Unable to parse row: {row.Row}. {ex.Message}");
            }

            return item;
        }

        public static string ToFormattedDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ");
        }

        public static string ParseName(this string hierarchy)
        {
            if (hierarchy == null) return null;
            var lastPipeIndex = hierarchy.LastIndexOf('|');
            if (lastPipeIndex == -1) return hierarchy;
            if (lastPipeIndex == hierarchy.Length) return string.Empty;
            var id = hierarchy.Substring(0, lastPipeIndex).Trim();
            return id;
        }

        public static string ParseClassId(this string hierarchy)
        {
            if (hierarchy == null) return null;
            var lastPipeIndex = hierarchy.LastIndexOf('|') + 1;
            if (lastPipeIndex == hierarchy.Length) return string.Empty;
            var id = hierarchy.Substring(lastPipeIndex, hierarchy.Length - lastPipeIndex).Trim();
            return id;
        }

        public static string CellToString(this object cellValue)
        {
            if (cellValue == null)
                return string.Empty;
            else
                return cellValue.ToString().Trim();
        }
    }
}
