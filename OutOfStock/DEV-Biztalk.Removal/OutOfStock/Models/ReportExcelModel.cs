using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using OOS.Model;
using OOSCommon;
using OutOfStock.Classes;
using Excel = Microsoft.Office.Interop.Excel;

namespace OutOfStock.Models
{
    public class ReportExcelModel : ICustomReportExcelModel
    {
        private IOOSLog logger;

        


        private Excel.Application excelApp;
        private Excel._Worksheet workSheet;
        private Excel.Workbook workBook;

        private int rowCurrent;
        private int rowHeader;
        private int rowLast;

         
        public ReportExcelModel(IOOSLog logService)
        {
            logger = logService;
        }


        public string CreateExcelFile(string excelFilename, List<ColumnDataModel> columnNames, ref IEnumerable<ICustomReportExcelModel> reportData, ref ReportHeaderDataModel headerData, ref IEnumerable<ScanWithNoVimData> ScansWithNoVimData)
        {
            try
            {
                WriteExcel(excelFilename, ref columnNames, ref reportData, ref headerData, ref ScansWithNoVimData);
            }
            catch (Exception ex)
            {
                logger.Warn(ex.Message + ", Stack=" + ex.StackTrace);
            }
            finally
            {
                if (excelApp != null)
                    excelApp.Quit();
            }
            return string.Empty;
        }

        private void WriteExcel(string excelFilename, ref List<ColumnDataModel> columnNames, ref IEnumerable<ICustomReportExcelModel> reportData, ref ReportHeaderDataModel headerData, ref IEnumerable<ScanWithNoVimData> ScansWithNoVimData )
        {
            
            const int ROW_FIRST = 1;
            var COL_MAX = columnNames.Count();
            var ColMaxLetter = ((char)(((byte)'A') + COL_MAX - 1)).ToString();



            //InitExcelAppActiveWorksheet();
            excelApp = new Excel.Application { Visible = false };
            workBook= excelApp.Workbooks.Add();
            workSheet = workBook.ActiveSheet;


            Excel.Range r;

            rowCurrent = ROW_FIRST + 4;
            rowHeader = rowCurrent;

            var scanList = ScansWithNoVimData.ToList();

            if (scanList.Any())
            {
                workSheet.Name = "Scanned Items With No VIM Data";
                
                workSheet.Cells[rowHeader, 1].value = "UPC";
                workSheet.Cells[rowHeader, 2].value = "Store";
                workSheet.Cells[rowHeader, 3].value = "ScanCount";





                var cnt = 1;
                scanList.ForEach(s =>
                {
                    workSheet.Cells[rowHeader + cnt, 1].value = s.UPC;
                    workSheet.Cells[rowHeader + cnt, 2].value = s.StoreAbbreviation;
                    workSheet.Cells[rowHeader + cnt, 3].value = s.scanCount;
                    cnt++;
                });




                r = workSheet.Cells[rowHeader, 1];
                r.Font.Bold = true;
                r = workSheet.Cells[rowHeader, 2];
                r.Font.Bold = true;
                r = workSheet.Cells[rowHeader, 3];
                r.Font.Bold = true;


                workSheet.Columns[1].AutoFit();
                workSheet.Columns[2].AutoFit();
                workSheet.Columns[3].AutoFit();

                //// must be after Autofit in BoldHeadingsSizeColumnsToData()
                //FillHeader(headerMain, headerStores, headerTeams, headerSubTeams);
                workSheet.Cells[ROW_FIRST, 1] = headerData.Main;
                workSheet.Cells[ROW_FIRST + 1, 1] = headerData.Stores;
                workSheet.Cells[ROW_FIRST + 2, 1] = headerData.Teams;
                workSheet.Cells[ROW_FIRST + 3, 1] = headerData.SubTeams;

                workSheet = workBook.Worksheets.Add();
                
            }


            workSheet.Name = "Custom Report";


            //BatchupHeading();
            var range = workSheet.Range[CellReference(rowCurrent, 1), CellReference(rowCurrent, COL_MAX)];
            var columnout = (from c in columnNames
                             select c.Name).ToArray();
            range.set_Value(Type.Missing, columnout);
            ++rowCurrent;
            //BatchupGrid(reportData);

            var grid = new object[reportData.Count(), columnNames.Count()];
            for (var i = 0; i < reportData.Count(); i++)
            {
                ICustomReportExcelModel item = reportData.ElementAt(i);
                

                for (var ci = 0; ci < columnNames.Count; ci++)
                {
                    // its no voodoo, its reflection.
                    var c = columnNames[ci];
                    PropertyInfo p = item.GetType().GetProperty(c.DataName);    
                    grid[i, ci] = p.GetValue(item, null);
                }
            }
            var topLeft = CellReference(rowCurrent, 1);
            rowCurrent = rowCurrent + grid.GetLength(0);
            var lowerRight = CellReference(rowCurrent - 1, COL_MAX);
            range = workSheet.Range[topLeft, lowerRight];
            range.set_Value(Type.Missing, grid);



            // format last date sold as a date format if it exists.
            var datecolumn = (from c in columnNames where c.Name == "Last Date Sold" select c).FirstOrDefault();

            if (datecolumn != null)
            {
                var datecolumnindex = columnNames.IndexOf(datecolumn) + 1;

                r = workSheet.Cells[1,datecolumnindex];
                r.EntireColumn.NumberFormat = "mm/dd/yyyy";
            }

            rowLast = rowCurrent - 1;

            

            //SetupFilter();
            var rangeToFilter = workSheet.Range["A" + rowHeader, ColMaxLetter + rowLast];
            rangeToFilter.AutoFilter(2, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
            
            //BoldHeadingsSizeColumnsToData();
            for (var col = 1; col <= COL_MAX; ++col)
            {
                r = workSheet.Cells[rowHeader, col];
                r.Font.Bold = true;
                workSheet.Columns[col].AutoFit();
            }

            //// must be after Autofit in BoldHeadingsSizeColumnsToData()
            //FillHeader(headerMain, headerStores, headerTeams, headerSubTeams);
            workSheet.Cells[ROW_FIRST, 1] = headerData.Main;
            workSheet.Cells[ROW_FIRST + 1, 1] = headerData.Stores;
            workSheet.Cells[ROW_FIRST + 2, 1] = headerData.Teams;
            workSheet.Cells[ROW_FIRST + 3, 1] = headerData.SubTeams;

            //PrinterSetup();
            try
            {
              //  workSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                workSheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperLegal;
                workSheet.PageSetup.LeftMargin = workSheet.Application.InchesToPoints(.2);
                workSheet.PageSetup.RightMargin = workSheet.Application.InchesToPoints(.2);
                workSheet.PageSetup.TopMargin = workSheet.Application.InchesToPoints(.2);
                workSheet.PageSetup.BottomMargin = workSheet.Application.InchesToPoints(.2);
                workSheet.PageSetup.PrintArea = "A" + ROW_FIRST + ":" + ColMaxLetter + rowLast;
                workSheet.PageSetup.PrintTitleRows = "$" + ROW_FIRST + ":$" + rowHeader;
                workSheet.PageSetup.Zoom = false;
                workSheet.PageSetup.FitToPagesTall = 5000;
                workSheet.PageSetup.FitToPagesWide = 1;
            }
            catch (Exception ex)
            {
                // skip 
                //logger.Warn(ex.Message + ", Stack=" + ex.StackTrace); 
            }





            //SaveWorksheet(excelFilename);
            workSheet.SaveAs(excelFilename);
            
        }

        private static string CellReference(int row, int columnNumber)
        {
            return ColumnLetter(columnNumber) + row;
        }

        private static string ColumnLetter(int columnNumber)
        {
            var result = string.Empty;
            for (--columnNumber; ; )
            {
                var currentLetter = 0;
                columnNumber = Math.DivRem(columnNumber, 26, out currentLetter);
                var next = (char)(((byte)'A') + currentLetter);
                result = next + result;
                if (columnNumber <= 0)
                    break;
            }
            return result;
        }

    }
}