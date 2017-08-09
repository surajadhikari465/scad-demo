using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using OfficeOpenXml;
using OfficeOpenXml.Style;



namespace IRMAUserAuditConsole
{
    class ExcelHelper
    {
        public void NewWorksheet(ExcelPackage package, string worksheetName)
        {
            if (package == null)
                return;

            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetName);
        }

        public void CreateHeader(ExcelPackage package, string worksheetName, string[] columns, int? row)
        {
            int headerRow = row ?? 1;
            ExcelWorksheet ws = package.Workbook.Worksheets[worksheetName];
            for (int i = 0; i < columns.Length; i++)
            {
                ws.Cells[headerRow, i + 1].Value = columns[i];
            }
            var range = ws.Cells[headerRow, 1, headerRow, columns.Length];
            range.Style.Font.Bold = true;
            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            
        }

        public void AddRow(ExcelPackage package, string worksheetName, int rowNumber, object[] values)
        {
            ExcelWorksheet ws = package.Workbook.Worksheets[worksheetName];
            for (int i = 0; i < values.Length; i++)
            {
                ws.Cells[rowNumber, i + 1].Value = values[i];
                if(values[i] != null)
                    if (values[i].ToString().Length > 100)
                        ws.Column(i + 1).Width = 50;
            }
        }
        /// <summary>
        /// Creates a Dropdown List in cellTarget, using options as the values
        /// </summary>
        /// <param name="worksheetName"></param>
        /// <param name="cellTarget"></param>
        /// <param name="options"></param>
        public void CreateDropdownList(ExcelPackage package, string worksheetName, string cellTarget, List<string> options)
        {
            ExcelWorksheet ws = package.Workbook.Worksheets[worksheetName];
            var val = ws.DataValidations.AddListValidation(cellTarget);
            foreach (string option in options)
            {
                val.Formula.Values.Add(option);
            }
            val.ShowErrorMessage = true;
            val.Error = "Select value from the list...";
        }

        /// <summary>
        /// Creates a dropdown list with the values embedded in a list of cells
        /// </summary>
        /// <param name="worksheet">The worksheet to use</param>
        /// <param name="cellTarget">cell where dropdown list will appear</param>
        /// <param name="formula">the address where the list options are pulled from. Ex: A4:A25</param>
        public void CreateDropdownFromCells(ExcelPackage package, string worksheet, string cellTarget, string formula)
        {
            ExcelWorksheet ws = package.Workbook.Worksheets[worksheet];
            var val = ws.DataValidations.AddListValidation(cellTarget);
            val.ShowErrorMessage = true;
            val.Error = "select value from list...";
            val.Formula.ExcelFormula = formula;
        }

        /// <summary>
        /// Create a list of cells to be used for data validation elsewhere on the spreadsheet
        /// </summary>
        /// <param name="worksheet">Which worksheet</param>
        /// <param name="column">what column to place the list values in</param>
        /// <param name="startRow">what row to start on</param>
        /// <param name="options">the list options</param>
        /// <param name="hide">whether or not to hide this column on the sheet (not implemented yet)</param>
        /// <returns></returns>
        public string EmbedListOptions(ExcelPackage package, string worksheet, string column, int startRow, object[] options)
        {
            ExcelWorksheet ws = package.Workbook.Worksheets[worksheet];
            int row = startRow;
            foreach (object option in options)
            {
                ws.Cells[column + row.ToString()].Value = option;
                row++;
            }
            
            string formula = column + startRow.ToString() + ":" + column + (row - 1).ToString();
            return formula;
            
        }

        public void SetCellFormula(ExcelPackage package, string worksheet, string cell, string formula)
        {
            ExcelWorksheet ws = package.Workbook.Worksheets[worksheet];
            var val = ws.DataValidations.AddListValidation(cell);
            val.ShowErrorMessage = true;
            val.Error = "You must select a value from the list...";
            val.ErrorTitle = "Oops!";
            val.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
            val.Formula.ExcelFormula = formula;
        }

        public void AutosizeColumns(ExcelPackage package, string worksheet, int minWidth)
        {
            ExcelWorksheet ws = package.Workbook.Worksheets[worksheet];
            ws.Cells.AutoFitColumns(minWidth);
           
            for (int i = 0; i < ws.Dimension.End.Column; i++)
            {
                if (ws.Column(i + 1).Width > 50)
                    ws.Column(i + 1).Width = 50;
            }
        }

        public void HideColumn(ExcelPackage package, string worksheet, int index)
        {
            ExcelWorksheet ws = package.Workbook.Worksheets[worksheet];
            ws.Column(index).Width = 0;
        }

        public List<object> RangeToObjectList(ExcelPackage package, string worksheet, string range)
        {
            ExcelWorksheet ws = package.Workbook.Worksheets[worksheet];
            var cells = ws.Cells[range];
            List<object> objectList = new List<object>();
            int cellCount = cells.Count();
            int row = cells.Start.Row;
            for (int i = 1; i <= cellCount; i++)
            {
                objectList.Add(cells[row, i].Value);
            }
            return objectList;
        }
    }
}
