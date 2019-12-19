using BulkItemUploadProcessor.Service.ExcelParsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkItemUploadProcessor.Service.Tests.ExcelParsing
{
    [TestClass]
    public class ExcelWorksheetHeaderParserTests
    {
        private ExcelWorksheetHeadersParser excelParser;
        private ExcelPackage excelPackage;

        [TestInitialize]
        public void Initialize()
        {
            excelParser = new ExcelWorksheetHeadersParser();
        }

        [TestMethod]
        public void Parse_9thHeaderCellIsBlank_ReturnsTextOfNonBlankCellsInFirstRow()
        {
            //Given
            var expectedHeaders = new List<string>
            {
                "Scan Code",
                "Brands",
                "Merchandise",
                "Tax",
                "National",
                "Financial",
                "Manufacturer",
                "Product Description",
                "Item Pack",
                "Retail Size"
            };

            excelPackage = new ExcelPackage(new FileInfo(@".\TestData\HeadersTest.xlsx"));

            //When
            var headers = excelParser.Parse(excelPackage.Workbook.Worksheets["items"]);

            //Then
            Assert.AreEqual(expectedHeaders.Count, headers.Count);
            for (int i = 0; i < expectedHeaders.Count; i++)
            {
                Assert.AreEqual(expectedHeaders[i], headers[i].Name);
            }
            Assert.AreEqual(1, headers[0].ColumnIndex);
            Assert.AreEqual(2, headers[1].ColumnIndex);
            Assert.AreEqual(3, headers[2].ColumnIndex);
            Assert.AreEqual(4, headers[3].ColumnIndex);
            Assert.AreEqual(5, headers[4].ColumnIndex);
            Assert.AreEqual(6, headers[5].ColumnIndex);
            Assert.AreEqual(7, headers[6].ColumnIndex);
            Assert.AreEqual(8, headers[7].ColumnIndex);
            Assert.AreEqual(10, headers[8].ColumnIndex);
            Assert.AreEqual(11, headers[9].ColumnIndex);
        }
    }
}
