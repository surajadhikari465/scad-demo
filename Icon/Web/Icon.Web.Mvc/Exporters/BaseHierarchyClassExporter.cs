﻿using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Icon.Web.Extensions;
using System.Drawing;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.Mvc.Exporters
{
    public abstract class BaseHierarchyClassExporter<T>
    {
        protected const int HierarchyClassNameColumnIndex = 0;
        public List<T> ExportData { get; set; }
        public ExcelExportModel ExportModel { get; set; }

        protected Worksheet worksheet;
        protected List<SpreadsheetColumn<T>> spreadsheetColumns;
        protected List<T> extraColumns;

        public BaseHierarchyClassExporter()
        {
            this.spreadsheetColumns = new List<SpreadsheetColumn<T>>();
        }

        public virtual void Export()
        {
            BuildSpreadsheet();

            var exportHierarchyClassData = ConvertExportDataToExportHierarchyClassModel();
            // Start at 1 to exclude the header row.
            int i = 1;
            foreach (T hierarchyClass in exportHierarchyClassData)
            {
                foreach (var column in spreadsheetColumns)
                {
                    column.SetValue(worksheet.Rows[i], hierarchyClass);
                }

                i++;
            }
        }

        protected void BuildSpreadsheet()
        {
            worksheet = ExportModel.ExcelWorkbook.Worksheets.Add("Exported");
            CreateHeaderRow();
        }

        protected void AddSpreadsheetColumn(int columnIndex, string headerTitle, int width, HorizontalCellAlignment alignment,
            Action<WorksheetRow, T> setValue)
        {
            spreadsheetColumns.Add(new SpreadsheetColumn<T>
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

        private void CreateHeaderRow()
        {
            foreach (var column in spreadsheetColumns)
            {
                worksheet.Rows[0].Cells[column.Index].Value = column.HeaderTitle;
                worksheet.Rows[0].Cells[column.Index].CellFormat.Fill = column.HeaderBackground;
                worksheet.Rows[0].Cells[column.Index].CellFormat.Font.ColorInfo = column.HeaderForeground;
                worksheet.Rows[0].Cells[column.Index].CellFormat.Font.Bold = column.IsHeaderFontBold;
                worksheet.Columns[column.Index].Width = column.Width;
                worksheet.Columns[column.Index].CellFormat.Alignment = column.Alignment;
            }
        }

        protected abstract List<T> ConvertExportDataToExportHierarchyClassModel();
    }
}