using BulkItemUploadProcessor.Common.Models;
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
    public class ExcelRowParserTests
    {
        private ExcelRowParser excelRowParser;
        private ExcelPackage excelPackage;
        private List<string> columnNames = new List<string>
        {
            "Barcode Type",
            "Scan Code",
            "Brands",
            "Merchandise",
            "Tax",
            "National",
            "Manufacturer",
            "Product Description",
            "POS Description",
            "Item Pack",
            "Retail Size",
            "UOM",
            "Food Stamp Eligible",
            "POS Scale Tare",
            "Delivery System",
            "Alcohol By Volume",
            "Notes",
            "Casein Free",
            "Drained Weight",
            "Fair Trade Certified",
            "Drained Weight UOM",
            "Hemp",
            "Local Loan Producer",
            "Nutrition Required",
            "Organic Personal Care",
            "Air Chilled",
            "Animal Welfare Rating",
            "Biodynamic",
            "Cheese Attribute: Milk Type",
            "Raw",
            "Dry Aged",
            "Eco-Scale Rating",
            "Free Range",
            "Fresh or Frozen",
            "Gluten Free",
            "Grass Fed",
            "Kosher",
            "Made In House",
            "MSC",
            "Non-GMO",
            "Organic",
            "Paleo",
            "Pasture Raised",
            "Premium Body Care"
        };

        [TestInitialize]
        public void Initialize()
        {
            excelRowParser = new ExcelRowParser();
        }

        [TestMethod]
        public void Parse_SingleRow_Returns1Row()
        {
            //Given
            excelPackage = new ExcelPackage(new FileInfo(@".\TestData\ExcelRowParserTest_SingleRow.xlsx"));
            List<ColumnHeader> columnHeaders = columnNames
                    .Select((c, i) => new ColumnHeader
                    {
                        Address = null,
                        ColumnIndex = i + 1,
                        Name = c
                    }).ToList();

            //When
            var rows = excelRowParser.Parse(
                excelPackage.Workbook.Worksheets["items"],
                columnHeaders);

            //Then
            Assert.AreEqual(1, rows.Count);
            Assert.AreEqual(17, rows.First().Cells.Count);
            foreach (var columnHeader in columnHeaders.Take(17))
            {
                Assert.IsTrue(rows[0].Cells.Any(c => c.Column.ColumnIndex == columnHeader.ColumnIndex));
            }
        }
    }
}
