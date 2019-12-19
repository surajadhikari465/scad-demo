using System;
using System.Collections.Generic;
using System.Linq;
//using MassTransit;
using System.Text;
using OOS.Model;
using OOSCommon;
using Excel = Microsoft.Office.Interop.Excel;
using OOSCommon.DataContext;

namespace OutOfStock.Models
{
    public class KnownOOSUploadModel
    {

        public enum enumColumnNames { UPC, Status, Expiration };
        public static Dictionary<string, enumColumnNames> columnNameDictionary =
            new Dictionary<string, enumColumnNames>()
            {
                { enumColumnNames.UPC.ToString(), enumColumnNames.UPC },
                { enumColumnNames.Status.ToString(), enumColumnNames.Status },
                { enumColumnNames.Expiration.ToString(), enumColumnNames.Expiration }
            };

 
     
        public static List<string> ReadExcelFile(string excelFilename, out int recordsUdpated)
        {
           // OutOfStock.MvcApplication.oosLog.Trace("Enter");
            recordsUdpated = 0;
            var result = new List<string>();
            Excel.Application excelApp = null;
            Excel._Workbook workBook = null;
            Excel._Worksheet workSheet = null;

            var vendorKey = string.Empty;
            var vin = string.Empty;



            try
            {
                // Excel opens with a new, empty workbook
                excelApp = new Excel.Application {Visible = false};
                workBook = excelApp.Workbooks.Open(Filename: excelFilename);
                workSheet = (Excel.Worksheet)workBook.ActiveSheet;

                var rowLast = workSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                var columnLast = workSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Column;
                var rowFirst = rowLast;
                var columnFirst = columnLast;

                

                // Find outer rectangle of data
                for (var row = 1; row <= rowLast; ++row)
                {
                    for (var column = 1; column <= columnLast; ++column)
                    {
                        var cellValue = string.Empty;
                        try
                        {
                            cellValue = workSheet.Cells[row, column].value;
                        }
                        catch (Exception)
                        {
                            cellValue = string.Empty;
                        }
                        if (!string.IsNullOrWhiteSpace(cellValue))
                        {
                            if (rowFirst > row)
                                rowFirst = row;
                            if (columnFirst > column)
                                columnFirst = column;
                            break;
                        }
                    }
                }
                 
                // Id columns from header
                int? rowHeading = null;
                var columnsHeading =
                    new Dictionary<enumColumnNames, int?>()
                    {
                        {enumColumnNames.UPC, null},
                        {enumColumnNames.Status, null},
                        {enumColumnNames.Expiration, null}
                    };
                // Row by row find any cell match a data label
                for (int rowScan = rowFirst; !rowHeading.HasValue && rowScan <= rowLast; ++rowScan)
                {
                    // Clear.  No match
                    foreach (enumColumnNames eCol in typeof(enumColumnNames).GetEnumValues())
                        columnsHeading[eCol] = null;
                    // Scan column search for column names
                    for (int columnScan = columnFirst; columnScan <= columnLast; ++columnScan)
                    {
                        string cellValueString = string.Empty;
                        var cellValue = workSheet.Cells[rowScan, columnScan].value;
                        bool isCandidate = (cellValue != null);
                        if (isCandidate)
                            isCandidate = (cellValue is string);
                        if (isCandidate)
                        {
                            cellValueString = ((string)cellValue).ToString().Trim();
                            isCandidate = !string.IsNullOrWhiteSpace(cellValueString);
                        }
                        if (isCandidate)
                            isCandidate = columnNameDictionary.Keys.Contains(cellValueString);
                        if (isCandidate)
                        {
                            // A match
                            enumColumnNames eCol = columnNameDictionary[cellValueString];
                            // First instance of a label wins
                            if (!columnsHeading[eCol].HasValue)
                                columnsHeading[eCol] = columnScan;
                            // Done if we have matches for each heading
                             if (columnsHeading.All(d => d.Value.HasValue))
                            {
                                rowHeading = rowScan;
                                break;
                            }
                        }
                    }
                }

                if (!rowHeading.HasValue)
                {
                    result.Add(string.Format("The uploaded spreadsheet must contain the following labeled columns: {0}", string.Join(",", columnNameDictionary.Keys.ToList())));
                }
                else
                {
                    //var context = new OOSEntities(OutOfStock.MvcApplication.oosEFConnectionString);
                    var productStatuses = new List<ProductStatus>();
                        
                    
                    for (int row = rowHeading.Value + 1; row <= rowLast; ++row)
                    {
                        Excel.Range upc = null;
                        Excel.Range productStatus = null;
                        string region = string.Empty;
                        Excel.Range expiration = null;
                        var isValidRow = false;

                        var rowErrors = new List<string>();
                        //Excel.Range cellRage = null;
                        DateTime? expirationDate = null;
                        DateTime tempDate;
                        //var errorCount = 0;

                        try
                        {
                                region = OOSUser.userRegion;
                                upc = workSheet.Cells[row, columnsHeading[enumColumnNames.UPC].Value];
                                productStatus = workSheet.Cells[row, columnsHeading[enumColumnNames.Status].Value];
                                expiration = workSheet.Cells[row, columnsHeading[enumColumnNames.Expiration].Value];

                                isValidRow = ( upc.Value != null || productStatus.Value != null);

                                if (isValidRow)
                                {
                                    if (upc.Value != null)
                                    {
                                        if (!CheckUPC(upc.Value.ToString()))
                                        {
                                            rowErrors.Add(string.Format("Row {0}: UPC does not appear to be a valid WFM Item. Please check your UPC formatting [{1}]", row,
                                                                     upc.Value.ToString()));
                                        }
                                    }
                                    else
                                    {
                                        rowErrors.Add(string.Format("Row {0}: Missing required value [ UPC ]", row));
                                    }

                                    if (string.IsNullOrEmpty(region))
                                    {
                                        rowErrors.Add(string.Format("Row {0}: Missing required value [ Region ]", row));
                                    }
                                    if (productStatus.Value == null) { rowErrors.Add(string.Format("Row {0}: Missing required value [ ProductStatus ]", row)); }

                                    if (expiration.Value != null)
                                    {
                                        if (DateTime.TryParse(expiration.Value.ToString(), out tempDate))
                                        {
                                            expirationDate = tempDate;
                                        }
                                        else
                                        {
                                            rowErrors.Add(string.Format("Row {0}: Invalid Expiration Date value [ {1} ]", row, expiration.Value.ToString()));
                                        }
                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                rowErrors.Add(string.Format("Row {0}: Could not process values. {1} " , row, ex.Message));
                            }


                            if (rowErrors.Count == 0 && isValidRow)
                            {
                                // Test boundary condition when known upload is modified
                                var reason = Constants.ReasonNotSet;
                                var startDate = Constants.StartDateNotSet;

                                
                                var status = new ProductStatus(-1, region.ToString(), vendorKey, vin, upc.Value.ToString(), reason, startDate, productStatus.Value.ToString(), expirationDate);
                                productStatuses.Add(status);
                                
                            }
                            else
                            {
                                result.AddRange(rowErrors);
                            }
                            rowErrors.Clear();

                            #region "old code: remove later."


                            //try
                        //{
                        //    region = workSheet.Cells[row, columnsHeading[enumColumnNames.Region].Value].value;
                        //    if (string.IsNullOrWhiteSpace(region))
                        //    {
                        //        ++errorCount;
                        //        result.Add(string.Format("Region is empty at {0}", CellReference(row, columnsHeading[enumColumnNames.Region].Value)));
                        //    }
                        //}
                        //catch (Exception)
                        //{
                        //    ++errorCount;
                        //    result.Add(string.Format("Error accessing Region at {0}", CellReference(row, columnsHeading[enumColumnNames.Region].Value)));
                        //}


                        //try
                        //{
                        //    var tmp = workSheet.Cells[row, columnsHeading[enumColumnNames.VendorKey].Value].value;
                        //    vendorKey = tmp.ToString();
                        //    if (string.IsNullOrWhiteSpace(vendorKey))
                        //    {
                        //        ++errorCount;
                        //        result.Add("Vendor Key is empty at " + CellReference(row, columnsHeading[enumColumnNames.VendorKey].Value).ToString());
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    ++errorCount;
                        //    result.Add(string.Format("Error accessing Vendor Key at {0}: {1}" , CellReference(row, columnsHeading[enumColumnNames.VendorKey].Value), ex.Message));
                        //}

                        //try
                        //{
                        //    vin = Convert.ToString(workSheet.Cells[row, columnsHeading[enumColumnNames.Vin].Value].value);
                        //    if (string.IsNullOrWhiteSpace(vin))
                        //    {
                        //        ++errorCount;
                        //        result.Add("Vin is empty at " + CellReference(row, columnsHeading[enumColumnNames.Vin].Value));
                        //    }
                        //}
                        //catch (Exception)
                        //{
                        //    ++errorCount;
                        //    result.Add("Error accessing Vin at " + CellReference(row, columnsHeading[enumColumnNames.Vin].Value));
                        //}

                        //try
                        //{
                        //    upc = Convert.ToString(workSheet.Cells[row, columnsHeading[enumColumnNames.UPC].Value].value);
                        //    if (string.IsNullOrWhiteSpace(upc))
                        //    {
                        //        ++errorCount;
                        //        result.Add(string.Format("UPC is empty at {0}", CellReference(row, columnsHeading[enumColumnNames.UPC].Value)));
                        //    }
                        //    // to check the list from VIM with current UPC in the loop
                        //    // currently this only checks for 13 digits.

                        //    else if (!CheckUPC(upc))
                        //    {
                        //        ++errorCount;
                        //        result.Add(string.Format("Unknown UPC [{0}] at {1}", upc, CellReference(row, columnsHeading[enumColumnNames.UPC].Value)));
                        //    }
                        //}
                        //catch (Exception)
                        //{
                        //    ++errorCount;
                        //    result.Add(string.Format("Error accessing UPC at {0}", CellReference(row, columnsHeading[enumColumnNames.UPC].Value)));
                        //}
                        
                        
                        //try
                        //{
                        //    productStatus = workSheet.Cells[row, columnsHeading[enumColumnNames.ProductStatus].Value].value;
                        //    if (string.IsNullOrWhiteSpace(productStatus))
                        //    {
                        //        ++errorCount;
                        //        result.Add(string.Format("Product Status is empty at {0}", CellReference(row, columnsHeading[enumColumnNames.ProductStatus].Value)));
                        //    }
                        //}
                        //catch (Exception)
                        //{
                        //    ++errorCount;
                        //    result.Add(string.Format("Error accessing Product Status at {0}", CellReference(row, columnsHeading[enumColumnNames.ProductStatus].Value)));
                        //}

                        //try
                        //{
                        //    expirationDate = workSheet.Cells[row, columnsHeading[enumColumnNames.ExpirationDate].Value].value;
                        //    if (!expirationDate.HasValue)
                        //    {
                        //        ++errorCount;
                        //        result.Add("No Expiration Date on at " + CellReference(row, columnsHeading[enumColumnNames.ExpirationDate].Value));
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    ++errorCount;
                        //    result.Add(string.Format("Error accessing Expiration Date at {0}: {1} ",
                        //                             CellReference(row,
                        //                                           columnsHeading[enumColumnNames.ExpirationDate].Value),
                        //                             ex.Message));
                            //}
                            #endregion



                       
                    }
                    var query = new StringBuilder();


                    const string sql = "exec SaveProductStatus '{0}', '{1}', {2}, '{3}';\r\n";
                    var ItemCount = 0;

                    //using (var dc = new System.Data.Linq.DataContext(config.GetOOSConnectionString()))
                    using (var datacontext =  new OOSEntities(OutOfStock.MvcApplication.oosEFConnectionString))
                    {                        
                        //var uploadCommands = productStatusToKnownUploadCommandMapper.Map(Constants.UploadDateNotSet, productStatuses);
                        foreach (var status in productStatuses)
                        {
                            //    bus.Publish(command);
                            query.Append(string.Format(sql, status.Region, status.Upc, status.ExpirationDate.HasValue ? "'" + status.ExpirationDate.Value.ToShortDateString() + "'" : "null", status.StatusForSQL));
                            ItemCount++;
                            if (ItemCount > 100)
                            {
                                //write in blocks of 100 items. just in case.
                                datacontext.ExecuteStoreCommand(query.ToString(), null);
                                query.Clear();
                            }

                        }
                        if (query.Length > 0) { datacontext.ExecuteStoreCommand(query.ToString(), null); }
                        query.Clear();
                        recordsUdpated = productStatuses.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                OutOfStock.MvcApplication.oosLog.Warn(ex.Message + ", Stack=" + ex.StackTrace);
                result.Add("Serious error '" + ex.Message + "'");
            }
            finally
            {
                if (excelApp != null)
                    excelApp.Quit();
                
            }

           // OutOfStock.MvcApplication.oosLog.Trace("Exit");
            
            return result;
        }

        private static string CellReference(int row, int columnNumber)
        {
            return ColumnLetter(columnNumber) + row.ToString();
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

        private static bool CheckUPC(string upc)
        {
            bool isOk = true;
            //upc = upc.Substring(0, upc.Length - 1);
            upc = upc.PadLeft(13, '0');
            //upc = ("000000".Substring(0, 13 - upc.Length)) + upc; // left pad with zeros to 13

            
            isOk = !string.IsNullOrWhiteSpace(OutOfStock.MvcApplication.vimRepository.GetVimUPC(upc, string.Empty));
            return isOk;
        }

    }
}