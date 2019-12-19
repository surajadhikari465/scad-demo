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
    public class CustomReportExcelModel : ICustomReportExcelModel
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

       
        public string CreateExcelFile(string excelFilename, IEnumerable<CustomReportViewModel> enumerables, 
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



        private void ProcessEnumerablesWriteExcel(string excelFilename, IEnumerable<CustomReportViewModel> enumerables, 
            string headerMain, string headerStores, string headerTeams, string headerSubTeams)
        {
            logger.Trace("ProcessEnumerablesWriteExcel() Enter");
            DateTime dtStart = DateTime.Now;

            
            InitExcelAppActiveWorksheet();
            rowCurrent = ROW_FIRST + 4;

            rowHeader = rowCurrent;

            BatchupHeading();
            BatchupGrid(enumerables);

            rowLast = rowCurrent - 1;

            SetupFilter();
            BoldHeadingsSizeColumnsToData();

            FillHeader(headerMain, headerStores, headerTeams, headerSubTeams);
            PrinterSetup();
            SaveWorksheet(excelFilename);

            logger.Trace("ProcessEnumerablesWriteExcel() Exit, Elapsed=" + DateTime.Now.Subtract(dtStart));
        }

        
        private void InitExcelAppActiveWorksheet()
        {
            logger.Trace("InitExcelAppActiveWorksheet() Enter");
            DateTime dtStart = DateTime.Now;
            
            excelApp = new Excel.Application { Visible = false };
            excelApp.Workbooks.Add();
            workSheet = (Excel.Worksheet)excelApp.ActiveSheet;

            logger.Trace("InitExcelAppActiveWorksheet() Exit, Elapsed=" + DateTime.Now.Subtract(dtStart));
        }

        private void BatchupHeading()
        {
            logger.Trace("BatchupHeading() Enter");
            DateTime dtStart = DateTime.Now;
            
            Excel.Range range = workSheet.Range[CellReference(rowCurrent, 1), CellReference(rowCurrent, COL_MAX)];
            range.set_Value(Type.Missing, heading);
            ++rowCurrent;

            logger.Trace("BatchupHeading() Exit, Elapsed=" + DateTime.Now.Subtract(dtStart));
        }

        private static string CellReference(int row, int columnNumber)
        {
            return ColumnLetter(columnNumber) + row;
        }

        private static string ColumnLetter(int columnNumber)
        {
            string result = string.Empty;
            for (--columnNumber; ; )
            {
                int currentLetter = 0;
                columnNumber = Math.DivRem(columnNumber, 26, out currentLetter);
                char next = (char)(((byte)'A') + currentLetter);
                result = next + result;
                if (columnNumber <= 0)
                    break;
            }
            return result;
        }

        private void BatchupRows(IEnumerable<CustomReportViewModel> enumerables)
        {
            logger.Trace("BatchupRows() Enter");
            DateTime dtStart = DateTime.Now;

            foreach (var item in enumerables)
            {
                object[] rowValues = GetRow(item);
                Excel.Range range = workSheet.Range[CellReference(rowCurrent, 1), CellReference(rowCurrent, COL_MAX)];
                range.set_Value(Type.Missing, rowValues);
                ++rowCurrent;
            }

            logger.Trace("BatchupRows() Exit, Elapsed=" + DateTime.Now.Subtract(dtStart));
        }

        private void BatchupGrid(IEnumerable<CustomReportViewModel> enumerables)
        {
            logger.Trace("BatchupGrid() Enter");
            DateTime dtStart = DateTime.Now;

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

            logger.Trace("BatchupGrid() Exit, Elapsed=" + DateTime.Now.Subtract(dtStart));
        }

        private object[] GetRow(CustomReportViewModel item)
        {
            return new object[] {
                        item.PS_SUBTEAM, "=\"" + item.UPC + "\"", item.BRAND_NAME,
                        item.LONG_DESCRIPTION, item.ITEM_SIZE, item.ITEM_UOM,
                        item.VENDOR_KEY, item.VIN,
                        item.Avg_daily_Units, item.sales, item.Avg_Mov_Sales, 
                        item.times_scanned,  item.StoresList,  item.Product_Status, item.cost,
                        item.margin, item.EFF_PRICE, item.EFF_PRICETYPE, 
                        item.CATEGORY_NAME, item.CLASS_NAME
                    };
        }

        private void SetupFilter()
        {
            logger.Trace("SetupFilter() Enter");
            DateTime dtStart = DateTime.Now;
            
            Excel.Range rangeToFilter = workSheet.Range["A" + rowHeader, ColMaxLetter + rowLast];
            rangeToFilter.AutoFilter(Field: 2, Criteria1: Type.Missing,
                Criteria2: Type.Missing, VisibleDropDown: true);

            logger.Trace("SetupFilter() Exit, Elapsed=" + DateTime.Now.Subtract(dtStart));
        }

        private void BoldHeadingsSizeColumnsToData()
        {
            logger.Trace("BoldHeadingsSizeColumnsToData() Enter");
            DateTime dtStart = DateTime.Now;
            
            for (int col = 1; col <= COL_MAX; ++col)
            {
                workSheet.Cells[rowHeader, col].Font.Bold = true;
                workSheet.Columns[col].AutoFit();
            }

            logger.Trace("BoldHeadingsSizeColumnsToData() Exit, Elapsed=" + DateTime.Now.Subtract(dtStart));
        }

        // Must be AFTER autofit (more than 255 character may cause an exception)
        private void FillHeader(string headerMain, string headerStores, string headerTeams, string headerSubTeams)
        {
            logger.Trace("FillHeader() Enter");
            DateTime dtStart = DateTime.Now;
            
            workSheet.Cells[ROW_FIRST, 1] = headerMain;
            workSheet.Cells[ROW_FIRST + 1, 1] = headerStores;
            workSheet.Cells[ROW_FIRST + 2, 1] = headerTeams;
            workSheet.Cells[ROW_FIRST + 3, 1] = headerSubTeams;

            logger.Trace("FillHeader() Exit, Elapsed=" + DateTime.Now.Subtract(dtStart));
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
            logger.Trace("PrintSetup() Enter");
            DateTime dtStart = DateTime.Now;

            workSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
            workSheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperLetter;
            workSheet.PageSetup.PrintArea = "A" + ROW_FIRST + ":" + ColMaxLetter + rowLast;
            workSheet.PageSetup.PrintTitleRows = "$" + ROW_FIRST + ":$" + rowHeader;
            workSheet.PageSetup.Zoom = false;
            workSheet.PageSetup.FitToPagesTall = 5000;
            workSheet.PageSetup.FitToPagesWide = 1;

            logger.Trace("PrintSetup() Exit, Elapsed=" + DateTime.Now.Subtract(dtStart));
        }

        private void SaveWorksheet(string excelFilename)
        {
            logger.Trace("SaveWorksheet() Enter");
            DateTime dtStart = DateTime.Now;
         
            workSheet.SaveAs(excelFilename);

            logger.Trace("SaveWorksheet() Exit, Elapsed=" + DateTime.Now.Subtract(dtStart));
        }


    } 
}
