using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.Mvc.Importers;
using Infragistics.Documents.Excel;
using Moq;
using Icon.Web.DataAccess.Queries;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;
using Icon.Web.Common;
using Icon.Common.DataAccess;

namespace Icon.Web.Tests.Unit.Importers
{
    [TestClass]
    public class BrandSpreadsheetImporterTests
    {
        private BrandSpreadsheetImporter importer;
        private Mock<IQueryHandler<GetExistingBrandsParameters, List<string>>> mockGetBrandsThatExistQuery;
        private Mock<IQueryHandler<GetBrandAbbreviationsThatExistParameters, List<string>>> mockGetBrandAbbreviationsThatExistQuery;
        private Mock<IQueryHandler<GetDuplicateBrandsByTrimmedNameParameters, List<string>>> mockGetDuplicateBrandsByTrimmedNameQuery;
        private Workbook workbook;
        private int brandNameColumn = 0;
        private int brandAbbreviationColumn = 1;

        [TestInitialize]
        public void Initialize()
        {
            mockGetBrandsThatExistQuery = new Mock<IQueryHandler<GetExistingBrandsParameters, List<string>>>();
            mockGetBrandAbbreviationsThatExistQuery = new Mock<IQueryHandler<GetBrandAbbreviationsThatExistParameters, List<string>>>();
            mockGetDuplicateBrandsByTrimmedNameQuery = new Mock<IQueryHandler<GetDuplicateBrandsByTrimmedNameParameters, List<string>>>();
            importer = new BrandSpreadsheetImporter(mockGetBrandsThatExistQuery.Object,
                mockGetBrandAbbreviationsThatExistQuery.Object,
                mockGetDuplicateBrandsByTrimmedNameQuery.Object);
            CreateWorkbook();
            importer.Workbook = workbook;
        }

        private void CreateWorkbook()
        {
            workbook = new Workbook();
            workbook.Worksheets.Add("Brands");
            workbook.Worksheets[0].Rows[0].Cells[0].Value = "Brand";
            workbook.Worksheets[0].Rows[0].Cells[1].Value = "Brand Abbreviation";
            for (int i = 1; i <= 10; i++)
            {
                InsertBrandRow(i, "TestBrand" + i + "|" + i, "TestAbbr" + i);
            }
        }

        private void InsertBrandRow(int rowIndex, string brandName, string brandAbbreviation)
        {
            workbook.Worksheets[0].Rows[rowIndex].Cells[brandNameColumn].Value = brandName;
            workbook.Worksheets[0].Rows[rowIndex].Cells[brandAbbreviationColumn].Value = brandAbbreviation;
        }

        [TestMethod]
        public void IsValidSpreadsheetType_BrandHeaderIsValid_ShouldReturnTrue()
        {
            //When
            var isValidSpreadsheetType = importer.IsValidSpreadsheetType();

            //Then
            Assert.IsTrue(isValidSpreadsheetType);
        }

        [TestMethod]
        public void IsValidSpreadsheetType_BrandHeaderIsValidAndHasErrorColumn_ShouldIgnoreErrorColumnAndReturnTrue()
        {
            //Given
            workbook.Worksheets[0].Rows[0].Cells[2].Value = "Error";

            //When
            var isValidSpreadsheetType = importer.IsValidSpreadsheetType();

            //Then
            Assert.IsTrue(isValidSpreadsheetType);
        }

        [TestMethod]
        public void IsValidSpreadsheetType_BrandHeaderIsInvalid_ShouldReturnFalse()
        {
            //Given
            workbook.Worksheets[0].Rows[0].Cells[1].Value = "Invalid Column Name";

            //When
            var isValidSpreadsheetType = importer.IsValidSpreadsheetType();

            //Then
            Assert.IsFalse(isValidSpreadsheetType);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_AllParsedRowsAreValid_ShouldHaveAllValidRowsAndNoErrorRows()
        {
            //Given
            SetupMocksWithDefaultReturnValues();
            importer.ConvertSpreadsheetToModel();
            SetupMockGetDuplicateBrandsByTrimmedNameQueryWithDefaultReturnValue();

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(10, importer.ParsedRows.Count);
            Assert.AreEqual(10, importer.ValidRows.Count);
            Assert.AreEqual(0, importer.ErrorRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_4RowsHaveNoBrand_ShouldHave4ErrorRows()
        {
            //Given
            SetupMocksWithDefaultReturnValues();
            workbook.Worksheets[0].Rows[1].Cells[brandNameColumn].Value = "|0";
            workbook.Worksheets[0].Rows[2].Cells[brandNameColumn].Value = "TestBrand";
            workbook.Worksheets[0].Rows[3].Cells[brandNameColumn].Value = "";
            workbook.Worksheets[0].Rows[4].Cells[brandNameColumn].Value = null;
            importer.ConvertSpreadsheetToModel();
            SetupMockGetDuplicateBrandsByTrimmedNameQueryWithDefaultReturnValue();

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(10, importer.ParsedRows.Count);
            Assert.AreEqual(6, importer.ValidRows.Count);
            Assert.AreEqual(4, importer.ErrorRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RowHasBrandNameEqualToBrandLimit_ShouldHaveAllValidRowsAndNoErrorRows()
        {
            //Given
            SetupMocksWithDefaultReturnValues();
            workbook.Worksheets[0].Rows[1].Cells[brandNameColumn].Value = new string('b', Constants.BrandAbbreviationMaxLength) + "|1";
            importer.ConvertSpreadsheetToModel();
            SetupMockGetDuplicateBrandsByTrimmedNameQueryWithDefaultReturnValue();

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(10, importer.ParsedRows.Count);
            Assert.AreEqual(10, importer.ValidRows.Count);
            Assert.AreEqual(0, importer.ErrorRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_2RowsHaveBrandNamesLongerThanIconBrandLimit_ShouldHave2ErrorRows()
        {
            //Given
            SetupMocksWithDefaultReturnValues();
            workbook.Worksheets[0].Rows[1].Cells[brandNameColumn].Value = new string('b', Constants.IconBrandNameMaxLength + 1) + "|1";
            workbook.Worksheets[0].Rows[4].Cells[brandNameColumn].Value = new string('c', Constants.IconBrandNameMaxLength + 1) + "|4";
            importer.ConvertSpreadsheetToModel();
            SetupMockGetDuplicateBrandsByTrimmedNameQueryWithDefaultReturnValue();

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(10, importer.ParsedRows.Count);
            Assert.AreEqual(8, importer.ValidRows.Count);
            Assert.AreEqual(2, importer.ErrorRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_4DuplicateBrandsExistOnSpreadsheet_ShouldHave4ErrorRows()
        {
            //Given
            SetupMocksWithDefaultReturnValues();
            workbook.Worksheets[0].Rows[1].Cells[brandNameColumn].Value = "DupName|1";
            workbook.Worksheets[0].Rows[2].Cells[brandNameColumn].Value = "DupName|2";
            workbook.Worksheets[0].Rows[3].Cells[brandNameColumn].Value = "DupId1|63";
            workbook.Worksheets[0].Rows[4].Cells[brandNameColumn].Value = "DupId2|63";
            importer.ConvertSpreadsheetToModel();
            SetupMockGetDuplicateBrandsByTrimmedNameQueryWithDefaultReturnValue();

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(10, importer.ParsedRows.Count);
            Assert.AreEqual(6, importer.ValidRows.Count);
            Assert.AreEqual(4, importer.ErrorRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_3BrandAbbreviationsAreLongerThanBrandAbbreviationLimit_ShouldHave3ErrorRows()
        {
            //Given
            SetupMocksWithDefaultReturnValues();
            workbook.Worksheets[0].Rows[1].Cells[brandAbbreviationColumn].Value = new string('a', Constants.BrandAbbreviationMaxLength + 1);
            workbook.Worksheets[0].Rows[2].Cells[brandAbbreviationColumn].Value = new string('b', Constants.BrandAbbreviationMaxLength + 100);
            workbook.Worksheets[0].Rows[3].Cells[brandAbbreviationColumn].Value = new string('c', Constants.BrandAbbreviationMaxLength + 5);
            importer.ConvertSpreadsheetToModel();
            SetupMockGetDuplicateBrandsByTrimmedNameQueryWithDefaultReturnValue();

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(10, importer.ParsedRows.Count);
            Assert.AreEqual(7, importer.ValidRows.Count);
            Assert.AreEqual(3, importer.ErrorRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_1BrandAbbreviationIsEqualToBrandAbbreviationLimit_ShouldHaveAllValidRowsAndNoErrorRows()
        {
            //Given
            SetupMocksWithDefaultReturnValues();
            workbook.Worksheets[0].Rows[1].Cells[brandAbbreviationColumn].Value = new string('a', Constants.BrandAbbreviationMaxLength);
            importer.ConvertSpreadsheetToModel();
            SetupMockGetDuplicateBrandsByTrimmedNameQueryWithDefaultReturnValue();

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(10, importer.ParsedRows.Count);
            Assert.AreEqual(10, importer.ValidRows.Count);
            Assert.AreEqual(0, importer.ErrorRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_4DuplicateBrandAbbreviationsExistOnSpreadsheet_ShoulHave4ErrorRows()
        {
            //Given
            SetupMocksWithDefaultReturnValues();
            workbook.Worksheets[0].Rows[1].Cells[brandAbbreviationColumn].Value = "DupAbbr1";
            workbook.Worksheets[0].Rows[2].Cells[brandAbbreviationColumn].Value = "DupAbbr1";
            workbook.Worksheets[0].Rows[3].Cells[brandAbbreviationColumn].Value = "DupAbbr2";
            workbook.Worksheets[0].Rows[4].Cells[brandAbbreviationColumn].Value = "DupAbbr2";
            importer.ConvertSpreadsheetToModel();
            SetupMockGetDuplicateBrandsByTrimmedNameQueryWithDefaultReturnValue();

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(10, importer.ParsedRows.Count);
            Assert.AreEqual(6, importer.ValidRows.Count);
            Assert.AreEqual(4, importer.ErrorRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_2NewBrandsExist_ShouldHave2ErrorRows()
        {
            //Given
            workbook.Worksheets[0].Rows[1].Cells[brandNameColumn].Value = "TestBrand1|0";
            workbook.Worksheets[0].Rows[2].Cells[brandNameColumn].Value = "TestBrand2|0";
            mockGetBrandsThatExistQuery.Setup(m => m.Search(It.IsAny<GetExistingBrandsParameters>()))
                .Returns(new List<string>
                {
                    workbook.Worksheets[0].Rows[1].Cells[brandNameColumn].Value.ToString().Split('|')[0],
                    workbook.Worksheets[0].Rows[2].Cells[brandNameColumn].Value.ToString().Split('|')[0]
                });
            SetupMockGetBrandAbbreviationsQueryWithDefaultReturnValue();
            SetupMockGetDuplicateBrandsByTrimmedNameQueryWithDefaultReturnValue();
            importer.ConvertSpreadsheetToModel();

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(10, importer.ParsedRows.Count);
            Assert.AreEqual(8, importer.ValidRows.Count);
            Assert.AreEqual(2, importer.ErrorRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_3BrandsExistWithNamesEqualToNewBrandNamesReducedToIrmaBrandNameLimit_ShouldHave3ErrorRows()
        {
            //Given
            workbook.Worksheets[0].Rows[1].Cells[brandNameColumn].Value = new string('a', 26) + "|0";
            workbook.Worksheets[0].Rows[2].Cells[brandNameColumn].Value = new string('b', 35) + "|0";
            workbook.Worksheets[0].Rows[3].Cells[brandNameColumn].Value = new string('c', 28) + "|0";
            mockGetDuplicateBrandsByTrimmedNameQuery.Setup(m => m.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()))
                .Returns(new List<string>
                {
                    workbook.Worksheets[0].Rows[1].Cells[brandNameColumn].Value.ToString().Split('|')[0],
                    workbook.Worksheets[0].Rows[2].Cells[brandNameColumn].Value.ToString().Split('|')[0],
                    workbook.Worksheets[0].Rows[3].Cells[brandNameColumn].Value.ToString().Split('|')[0]
                });
            mockGetBrandsThatExistQuery.Setup(m => m.Search(It.IsAny<GetExistingBrandsParameters>()))
               .Returns(new List<string>());
            SetupMockGetBrandAbbreviationsQueryWithDefaultReturnValue();
            importer.ConvertSpreadsheetToModel();

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(10, importer.ParsedRows.Count);
            Assert.AreEqual(7, importer.ValidRows.Count);
            Assert.AreEqual(3, importer.ErrorRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_3BrandAbbreviationsExist_ShouldHave3ErrorRows()
        {
            //Given
            mockGetBrandAbbreviationsThatExistQuery.Setup(m => m.Search(It.IsAny<GetBrandAbbreviationsThatExistParameters>()))
                .Returns(new List<string>
                {
                    workbook.Worksheets[0].Rows[1].Cells[brandAbbreviationColumn].Value.ToString(),
                    workbook.Worksheets[0].Rows[2].Cells[brandAbbreviationColumn].Value.ToString(),
                    workbook.Worksheets[0].Rows[3].Cells[brandAbbreviationColumn].Value.ToString()
                });
            SetupMockGetBrandsQueryWithDefaultReturnValue();
            SetupMockGetDuplicateBrandsByTrimmedNameQueryWithDefaultReturnValue();
            importer.ConvertSpreadsheetToModel();

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(10, importer.ParsedRows.Count);
            Assert.AreEqual(7, importer.ValidRows.Count);
            Assert.AreEqual(3, importer.ErrorRows.Count);
        }

        private void SetupMocksWithDefaultReturnValues()
        {
            SetupMockGetBrandsQueryWithDefaultReturnValue();
            SetupMockGetBrandAbbreviationsQueryWithDefaultReturnValue();
        }

        private void SetupMockGetBrandsQueryWithDefaultReturnValue()
        {
            mockGetBrandsThatExistQuery.Setup(m => m.Search(It.IsAny<GetExistingBrandsParameters>()))
                .Returns(new List<string>());
        }
        private void SetupMockGetBrandAbbreviationsQueryWithDefaultReturnValue()
        {
            mockGetBrandAbbreviationsThatExistQuery.Setup(m => m.Search(It.IsAny<GetBrandAbbreviationsThatExistParameters>()))
                .Returns(new List<string>());
        }
        private void SetupMockGetDuplicateBrandsByTrimmedNameQueryWithDefaultReturnValue()
        {
            mockGetDuplicateBrandsByTrimmedNameQuery.Setup(m => m.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()))
                 .Returns(new List<string>());
        }
    }
}
