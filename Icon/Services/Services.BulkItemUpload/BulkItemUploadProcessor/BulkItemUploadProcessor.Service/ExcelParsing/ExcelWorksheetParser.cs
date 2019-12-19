using BulkItemUploadProcessor.Service.ExcelParsing.Interfaces;
using OfficeOpenXml;
using System;
using System.IO;

namespace BulkItemUploadProcessor.Service.ExcelParsing
{
    public class ExcelWorksheetParser : IExcelWorksheetParser
    {
        public ExcelPackage Parse(byte[] fileContent)
        {
            try
            {
                using (var ms = new MemoryStream(fileContent))
                {
                    return new ExcelPackage(ms);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Excel Workbook data.", ex);
            }
        }
    }
}
