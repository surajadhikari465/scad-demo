using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model;
using OOSCommon;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.Objects;
using System.Globalization;
using System.Web.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Data.Common;
using System.IO;

namespace OutOfStock.Models
{
    public class CustomReportExcelModel 
    {
        private IOOSLog logger;

        private readonly string[] heading = new[] { "Subteam", "UPC", "Brand Name", "Description", "Size", 
                "UOM", "Vendor Key", "Order Code", "Avg Unit Opportunity",
                "Sales Opportunity", "Avg Sales Opportunity", "Times Scanned","Stores List", "Product Status", "Cost",
                "Margin", "Price", "Eff PriceType", "Category Name", "Class Name" };

        private const int ROW_FIRST = 1;
        private const int COL_MAX = 20;
        private readonly string ColMaxLetter = ((char)(((byte)'A') + COL_MAX - 1)).ToString();

        private Excel.Application excelApp;
        private Excel._Worksheet workSheet;

        private int rowCurrent;
        private int rowHeader;
        private int rowLast;

        public CustomReportExcelModel(ILogService logService)
        {
            logger = logService.GetLogger();
        }


        public string CreateExcelFile(string excelFilename, IEnumerable<ICustomReportViewModel> enumerables, 
            string headerMain, string headerStores, string headerTeams, string headerSubTeams)
        {
            try
            {
                ProcessEnumerablesWriteExcel(excelFilename, enumerables, headerMain, headerStores, headerTeams, headerSubTeams);
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



        private void ProcessEnumerablesWriteExcel(string excelFilename, IEnumerable<ICustomReportViewModel> enumerables, 
            string headerMain, string headerStores, string headerTeams, string headerSubTeams)
        {
            logger.Trace("ProcessEnumerablesWriteExcel()");
            DateTime dtStart = DateTime.Now;

            
            InitExcelAppActiveWorksheet();
            rowCurrent = ROW_FIRST + 4;

            rowHeader = rowCurrent;

            BatchupHeading();
            BatchupGrid(enumerables);

            rowLast = rowCurrent - 1;

            SetupFilter();
            BoldHeadingsSizeColumnsToData();

            // must be after Autofit in BoldHeadingsSizeColumnsToData()
            FillHeader(headerMain, headerStores, headerTeams, headerSubTeams);
            
            PrinterSetup();
            SaveWorksheet(excelFilename);

            logger.Trace("ProcessEnumerablesWriteExcel() Exit, Elapsed=" + DateTime.Now.Subtract(dtStart));
        }

        
        private void InitExcelAppActiveWorksheet()
        {
            excelApp = new Excel.Application { Visible = false };
            excelApp.Workbooks.Add();
            workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
        }


        private void AddComments()
        {
            // todo: add coments in excel spreadsheet
        }

        private void BatchupHeading()
        {            
            var range = workSheet.Range[CellReference(rowCurrent, 1), CellReference(rowCurrent, COL_MAX)];
            range.set_Value(Type.Missing, heading);
            ++rowCurrent;
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

        //private void BatchupRows(IEnumerable<CustomReportViewModel> enumerables)
        //{
        //    logger.Trace("BatchupRows() Enter");
        //    DateTime dtStart = DateTime.Now;

        //    foreach (var item in enumerables)
        //    {
        //        object[] rowValues = GetRow(item);
        //        Excel.Range range = workSheet.Range[CellReference(rowCurrent, 1), CellReference(rowCurrent, COL_MAX)];
        //        range.set_Value(Type.Missing, rowValues);
        //        ++rowCurrent;
        //    }

        //    logger.Trace("BatchupRows() Exit, Elapsed=" + DateTime.Now.Subtract(dtStart));
        //}

        private void BatchupGrid(IEnumerable<ICustomReportViewModel> enumerables)
        {            
            var grid = new object[enumerables.Count(), heading.Count()];
            for (int i = 0; i < enumerables.Count(); i++)
            {
                var item = enumerables.ElementAt(i);
                object[] rowValues = GetRow(item);
                for (int j = 0; j < rowValues.Count(); j++)
                {
                    grid[i, j] = rowValues[j];
                }
            }
            var topLeft = CellReference(rowCurrent, 1);
            rowCurrent = rowCurrent + grid.GetLength(0);
            var lowerRight = CellReference(rowCurrent-1, COL_MAX);
            var range = workSheet.Range[topLeft, lowerRight];
            range.set_Value(Type.Missing, grid);
        }

        private object[] GetRow(ICustomReportViewModel item)
        {
            return new object[] {
                        //item.Avg_Mov_Sales PS_SUBTEAM, "=\"" + item.UPC + "\"", item.BRAND_NAME,
                        //item.LONG_DESCRIPTION, item.ITEM_SIZE, item.ITEM_UOM,
                        //item.VENDOR_KEY, item.VIN,
                        //item.Avg_daily_Units, item.sales, item.Avg_Mov_Sales, 
                        //item.times_scanned,  item.StoresList,  item.Product_Status, item.cost,
                        //item.margin, item.EFF_PRICE, item.EFF_PRICETYPE, 
                        //item.CATEGORY_NAME, item.CLASS_NAME
                    };
        }

        private void SetupFilter()
        {
            var rangeToFilter = workSheet.Range["A" + rowHeader, ColMaxLetter + rowLast];
            rangeToFilter.AutoFilter(2, Type.Missing, Criteria2: Type.Missing, VisibleDropDown: true);
        }

        private void BoldHeadingsSizeColumnsToData()
        {
            for (var col = 1; col <= COL_MAX; ++col)
            {
                Excel.Range r = workSheet.Cells[rowHeader, col];
                r.Font.Bold = true;
                r.Orientation = 90;
                workSheet.Columns[col].AutoFit();
                
            }
        }

        // Must be AFTER autofit (more than 255 character may cause an exception)
        private void FillHeader(string headerMain, string headerStores, string headerTeams, string headerSubTeams)
        {
            
            workSheet.Cells[ROW_FIRST, 1] = headerMain;
            workSheet.Cells[ROW_FIRST + 1, 1] = headerStores;
            workSheet.Cells[ROW_FIRST + 2, 1] = headerTeams;
            workSheet.Cells[ROW_FIRST + 3, 1] = headerSubTeams;
        }

        // automatically zoomed out for formatting to the printer? 
        // Zoom is not stored in the workbook so we cannot do that.  
        // If the customer has an Excel printer setup they’d like we can do that.
        // Select “landscape” and other printing properties like fit all columns to one page?
        private void PrinterSetup()
        {
            try
            {
                PrintSetup();
            }
            catch (Exception ex)
            {
                logger.Warn(ex.Message + ", Stack=" + ex.StackTrace);
            }
        }

        private void PrintSetup()
        {
            workSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
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

        private void SaveWorksheet(string excelFilename)
        {
            workSheet.SaveAs(excelFilename);
        }


    } 
}
