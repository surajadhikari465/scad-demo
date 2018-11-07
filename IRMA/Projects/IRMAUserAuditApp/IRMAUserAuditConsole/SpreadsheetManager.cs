using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using OfficeOpenXml;

namespace IRMAUserAuditConsole
{
    public class SpreadsheetManager
    {
        private ExcelPackage package;
        private ExcelHelper helper;
        private Dictionary<string, Dictionary<string, string>> formulas = new Dictionary<string,Dictionary<string,string>>();
        private Dictionary<string, int> rowCounters = new Dictionary<string,int>();
        private string formulaSheetName = "__formulas__";

        /// <summary>
        /// Creates a new Spreadsheet Manager with the specified filename
        /// </summary>
        /// <param name="fileName">where to save this spreadsheet</param>
        public SpreadsheetManager(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            package = new ExcelPackage(fi);
           // package.Load(new FileStream(fileName, FileMode.OpenOrCreate));
            helper = new ExcelHelper();
        }
        /// <summary>
        /// Creates a new Spreadsheet Manager with the specified filename
        /// and creates the named worksheet
        /// </summary>
        /// <param name="fileName">where to save this spreadsheet</param>
        /// <param name="worksheetName">the worksheet to create</param>
        public SpreadsheetManager(string fileName, string worksheetName)
        {
            FileInfo fi = new FileInfo(fileName);
            package = new ExcelPackage(fi);
            helper = new ExcelHelper();
            CreateWorksheet(worksheetName);
        }

        /// <summary>
        /// Creates a spreadsheet row of Bold column headings on the first row
        /// of the specified sheet
        /// </summary>
        /// <param name="worksheet">the worksheet to create a header for</param>
        /// <param name="columnTitles">a list of column names for the header</param>
        public void CreateHeader(string worksheet, List<string> columnTitles)
        {
            helper.CreateHeader(package, worksheet, columnTitles.ToArray(), 1);
        }

        /// <summary>
        /// Creates a header on the specified row, instead of row 1
        /// </summary>
        /// <param name="worksheet">the worksheet to create a header for</param>
        /// <param name="columnTitles">a list of column names for the header</param>
        /// <param name="row">the row number on which to place the header</param>
        public void CreateHeader(string worksheet, List<string> columnTitles, int row)
        {
            helper.CreateHeader(package, worksheet, columnTitles.ToArray(), row);
        }

        /// <summary>
        /// Create a new worksheet
        /// </summary>
        /// <param name="worksheetName">the name of the new worksheet</param>
        public void CreateWorksheet(string worksheetName)
        {
            try
            {
                if (!rowCounters.ContainsKey(worksheetName))
                {
                    rowCounters.Add(worksheetName, 1);
                }
                package.Workbook.Worksheets.Add(worksheetName);
                
            }
            catch (InvalidOperationException ioex)
            {
                if (ioex.Message == "A worksheet with this name already exists in the workbook")
                {
                    // we can safely ignore this.
                }

            }
            catch (Exception ex)
            {
                // TODO:  Hope we never arrive here
            }
        }

        private void CreateFormulaSheet()
        {
            try
            {
                package.Workbook.Worksheets.Add(formulaSheetName);
                ExcelWorksheet wsFormulas = package.Workbook.Worksheets[formulaSheetName];
                wsFormulas.Hidden = eWorkSheetHidden.Hidden;
            }
            catch (InvalidOperationException ioex)
            {
                if (ioex.Message == "A worksheet with this name already exists in the workbook")
                {
                    // this can be ignored
                }
            }
            catch (Exception ex)
            {
                // ummmmm....
            }
        }

        /// <summary>
        /// adds a row of data to the spreadsheet
        /// </summary>
        /// <param name="worksheet">which sheet to add to</param>
        /// <param name="items">the values for the row</param>
        public void AddRow(string worksheet, object[] items)
        {
            int row = rowCounters[worksheet];
            helper.AddRow(package, worksheet, row, items);
            IncrementRowCounter(worksheet);
        }

        /// <summary>
        /// get the excel formula string for a given formula
        /// </summary>
        /// <param name="worksheet">which sheet the formula exists on</param>
        /// <param name="formulaName">the name of the formula</param>
        /// <returns></returns>
        public string GetFormula(string worksheet, string formulaName)
        {
            if (formulas.ContainsKey(worksheet))
            {
                if (formulas[worksheet].ContainsKey(formulaName))
                {
                    return (formulas[worksheet])[formulaName];
                }
                throw new Exception("Formula Name not found!");
            }
            throw new Exception("Worksheet not found!");
        }

        /// <summary>
        /// add a new formula to a worksheet.  Create if it doesn't exist,
        /// overwrite if it does.
        /// </summary>
        /// <param name="worksheet">which worksheet to add it to</param>
        /// <param name="formulaName">the name you want to refer to the formula as</param>
        /// <param name="formula">the formula string itself, e.g.: Z2:Z26</param>
        public void AddFormula(string worksheet, string formulaName, string formula)
        {
            formula = formulaSheetName + "!" + formula;
            if (!formulas.ContainsKey(worksheet))
            {
                Dictionary<string, string> newFormula = new Dictionary<string, string>();
                newFormula.Add(formulaName, formula);
                formulas.Add(worksheet, newFormula);
            }
            else
            {
                if (formulas[worksheet].ContainsKey(formulaName))
                {
                    (formulas[worksheet])[formulaName] = formula;
                }
                else
                {
                    (formulas[worksheet]).Add(formulaName, formula);
                }
            }
        }
        /// <summary>
        /// makes a particular cell use the named formula for validation
        /// </summary>
        /// <param name="worksheet">which worksheet</param>
        /// <param name="formulaName">which formula</param>
        /// <param name="cellAddress">the address of the cell to validate using the formula</param>
        public void AssignFormulaToCell(string worksheet, string formulaName, string cellAddress)
        {
            string formula = "";
            if (formulas.ContainsKey(worksheet))
            {
                if ((formulas[worksheet]).ContainsKey(formulaName))
                {
                    formula = (formulas[worksheet])[formulaName];
                }
                else
                {
                    throw new Exception("Formula not found!");
                }
            }
            else
            {
                throw new Exception("Worksheet not found!");
            }

            helper.SetCellFormula(package, worksheet, cellAddress, formula);

        }
        /// <summary>
        /// Closes out the manager and saves the file
        /// </summary>
        public void Close()
        {
            package.Save();
        }
        /// <summary>
        /// Closes out the manager and saves the file using the supplied filename.
        /// Allows filename to be changed when saving
        /// </summary>
        /// <param name="saveFileName">the new filename to save as</param>
        public void Close(string saveFileName)
        {
            FileInfo fi = new FileInfo(saveFileName);
            package.SaveAs(fi);
        }

        /// <summary>
        /// return the current row counter for the given worksheet
        /// </summary>
        /// <param name="worksheet">which worksheet</param>
        /// <returns></returns>
        public int GetCurrentRow(string worksheet)
        {
            if (rowCounters.ContainsKey(worksheet))
                return rowCounters[worksheet];

            return -1;
        }

        /// <summary>
        /// increments the row counter for a worksheet
        /// </summary>
        /// <param name="worksheet">which worksheet</param>
        private void IncrementRowCounter(string worksheet)
        {
            if (rowCounters.ContainsKey(worksheet))
            {
                rowCounters[worksheet]++;
            }
            else
            {
                rowCounters.Add(worksheet, 1);
            }
        }

        /// <summary>
        /// convenience method to skip a row on a sheet
        /// </summary>
        /// <param name="worksheet">which worksheet</param>
        public void SkipRow(string worksheet)
        {
            IncrementRowCounter(worksheet);
        }

        /// <summary>
        /// Jump to a specific row for a given worksheet
        /// </summary>
        /// <param name="worksheet">which worksheet</param>
        /// <param name="newRow">the row to jump to</param>
        public void JumpToRow(string worksheet, int newRow)
        {
            if (rowCounters.ContainsKey(worksheet))
            {
                rowCounters[worksheet] = newRow;
            }
        }

        /// <summary>
        /// Adds a dropdown list to the worksheet in the specified location
        /// </summary>
        /// <param name="worksheet">which worksheet</param>
        /// <param name="formulaName">what to call the formula for using this dropdown as validation</param>
        /// <param name="targetColumn">which column to place the first value in</param>
        /// <param name="targetRow">which row to place the first value in</param>
        /// <param name="items">a list of the dropdown items</param>
        public void AddDropdown(string worksheet, string formulaName, string targetColumn, int targetRow, object[] items)
        {
            VerifyFormulaSheetExists();
            string formula = helper.EmbedListOptions(package, formulaSheetName, targetColumn, targetRow, items);
            AddFormula(worksheet, formulaName, formula);
        }

        /// <summary>
        /// set the column width of the given column to 0 (hidden)
        /// </summary>
        /// <param name="worksheet">which worksheet</param>
        /// <param name="columnIndex">the column to hide</param>
        public void LockWorkSheet(string worksheet, int editableFromRow, int editableFromCol, int editableToRow, int editableToCol)
        {
            helper.LockWorkSheet(package, worksheet, editableFromRow, editableFromCol, editableToRow, editableToCol);
        }

        public void AutosizeColumns(string worksheet)
        {
            helper.AutosizeColumns(package, worksheet, 1);
        }

        /// <summary>
        /// Grab data for a specified row
        /// </summary>
        /// <param name="worksheet">the worksheet to use</param>
        /// <param name="lastColumn">the last column containing data</param>
        /// <param name="row">which row to grab</param>
        /// <returns>an array of object containing each column's value</returns>
        public object[] GetRowData(string worksheet, string lastColumn, int row)
        {
            List<object> rowData = new List<object>();
            string rangeAddress = "A" + row.ToString() + ":" + lastColumn + row.ToString();
            rowData = helper.RangeToObjectList(package, worksheet, rangeAddress);
            return rowData.ToArray();
        }

        /// <summary>
        /// Grabs data for the current row
        /// </summary>
        /// <param name="worksheet">which worksheet to use</param>
        /// <param name="lastColumn">the last column containing data</param>
        /// <returns>an array of object containing each column's value</returns>
        public object[] GetCurrentRowData(string worksheet, string lastColumn)
        {
            List<object> rowData = new List<object>();
            if (rowCounters.ContainsKey(worksheet))
            {
                int row = rowCounters[worksheet];
                string rangeAddress = "A" + row.ToString() + ":" + lastColumn + row.ToString();
                rowData = helper.RangeToObjectList(package, worksheet, rangeAddress);
            }
            return rowData.ToArray();
        }

        /// <summary>
        /// Get the last in-use row for a worksheet
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public int GetLastRow(string worksheet)
        {
            ExcelWorkbook wb = package.Workbook;
            ExcelWorksheet ws = wb.Worksheets[worksheet];
            
            return ws.Dimension.End.Row;
        }

        public bool WorksheetExists(string worksheet)
        {
            ExcelWorksheet ws = package.Workbook.Worksheets[worksheet];
            return ws != null;
        }

        private void VerifyFormulaSheetExists()
        {
            ExcelWorksheet ws = package.Workbook.Worksheets[formulaSheetName];
            if (ws == null)
                CreateFormulaSheet();
        }

    }
}
