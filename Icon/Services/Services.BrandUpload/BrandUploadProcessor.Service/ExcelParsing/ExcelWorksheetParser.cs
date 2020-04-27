using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BrandUploadProcessor.Service.ExcelParsing.Interfaces;
using OfficeOpenXml;

namespace BrandUploadProcessor.Service.ExcelParsing
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
