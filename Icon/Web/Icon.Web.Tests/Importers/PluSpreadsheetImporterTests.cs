using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc;
using Icon.Web.Mvc.Importers;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Importers
{
    [TestClass] [Ignore]
    public class PluSpreadsheetImporterTests
    {
        private Workbook workbook;
        private Worksheet worksheet;

        private Mock<IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>>> mockGetNewScanCodeUploadsQuery;
        private Mock<IQueryHandler<GetPluRemappingsParameters, List<BulkImportPluRemapModel>>> mockRemappingQuery;

        [TestInitialize]
        public void Initialize()
        {
            workbook = new Workbook();
            worksheet = workbook.Worksheets.Add("Sheet1");
            mockGetNewScanCodeUploadsQuery = new Mock<IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>>>();
            mockRemappingQuery = new Mock<IQueryHandler<GetPluRemappingsParameters, List<BulkImportPluRemapModel>>>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            workbook = null;
            worksheet = null;
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_UploadedSpreadsheet_ShouldConvertToModelClass()
        {
            // Given.
            string nationalPlu = "1234";
            string regionalPlu = "5678";

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            
            worksheet.Rows[1].Cells[2].Value = nationalPlu;
            for (int i = 3; i <= 14; i++)
            {
                worksheet.Rows[1].Cells[i].Value = regionalPlu;
            }

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();

            // Then.
            Assert.AreEqual(nationalPlu, importer.ParsedRows[0].NationalPlu);
            Assert.AreEqual(regionalPlu, importer.ParsedRows[0].swPLU);
            Assert.AreEqual(1, importer.ParsedRows.Count);
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_EmptyRow_ShouldNotBeAddedToModel()
        {
            // Given.
            for (int i = 0; i <= 14; i++)
            {
                worksheet.Rows[1].Cells[i].Value = String.Empty;
            }

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();

            // Then.
            Assert.AreEqual(0, importer.ParsedRows.Count);
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_RegionalPluOfZero_ShouldConvertToZeroInModel()
        {
            // Given.
            string nationalPlu = "1234";
            string regionalPlu = "0";

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;

            worksheet.Rows[1].Cells[2].Value = nationalPlu;
            worksheet.Rows[1].Cells[3].Value = regionalPlu;

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();

            // Then.
            Assert.AreEqual(nationalPlu, importer.ParsedRows[0].NationalPlu);
            Assert.AreEqual(regionalPlu, importer.ParsedRows[0].flPLU);
            Assert.AreEqual(String.Empty, importer.ParsedRows[0].swPLU);
            Assert.AreEqual(1, importer.ParsedRows.Count);
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_EmptyRegionalPlu_ShouldConvertToEmptyString()
        {
            // Given.
            string nationalPlu = "1234";

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            
            worksheet.Rows[1].Cells[2].Value = nationalPlu;

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();

            // Then.
            Assert.AreEqual(nationalPlu, importer.ParsedRows[0].NationalPlu);
            Assert.AreEqual(String.Empty, importer.ParsedRows[0].swPLU);
            Assert.AreEqual(1, importer.ParsedRows.Count);
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_PlusWithLeadingZeros_LeadingZerosShouldBeTrimmed()
        {
            // Given.
            string nationalPlu = "001234";
            string regionalPlu = "5678";

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            
            worksheet.Rows[1].Cells[2].Value = nationalPlu;
            for (int i = 3; i < 14; i++)
            {
                worksheet.Rows[1].Cells[i].Value = regionalPlu;
            }

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();

            // Then.
            Assert.AreEqual("1234", importer.ParsedRows[0].NationalPlu);
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_PlusWithInnerOrTrailingZeros_InnerOrTrailingZerosShouldNotBeTrimmed()
        {
            // Given.
            string nationalPlu = "1034";
            string regionalPlu = "5670";

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            
            worksheet.Rows[1].Cells[2].Value = nationalPlu;
            for (int i = 3; i < 14; i++)
            {
                worksheet.Rows[1].Cells[i].Value = regionalPlu;
            }

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();

            // Then.
            Assert.AreEqual(nationalPlu, importer.ParsedRows[0].NationalPlu);
            Assert.AreEqual(regionalPlu, importer.ParsedRows[0].swPLU);
        }

        [TestMethod]
        public void ValidSpreadsheetType_ValidSpreadsheet_ShouldValidate()
        {
            // Given.
            worksheet.Rows[0].Cells[0].Value = "Brand";
            worksheet.Rows[0].Cells[1].Value = "Product Description";
            worksheet.Rows[0].Cells[2].Value = "National PLU";
            worksheet.Rows[0].Cells[3].Value = "FL PLU";
            worksheet.Rows[0].Cells[4].Value = "MA PLU";
            worksheet.Rows[0].Cells[5].Value = "MW PLU";
            worksheet.Rows[0].Cells[6].Value = "NA PLU";
            worksheet.Rows[0].Cells[7].Value = "NC PLU";
            worksheet.Rows[0].Cells[8].Value = "NE PLU";
            worksheet.Rows[0].Cells[9].Value = "PN PLU";
            worksheet.Rows[0].Cells[10].Value = "RM PLU";
            worksheet.Rows[0].Cells[11].Value = "SO PLU";
            worksheet.Rows[0].Cells[12].Value = "SP PLU";
            worksheet.Rows[0].Cells[13].Value = "SW PLU";
            worksheet.Rows[0].Cells[14].Value = "UK PLU";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            bool result = importer.ValidSpreadsheetType();

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidSpreadsheetType_SpreadsheetWithWrongColumnName_VerificationFails()
        {
            // Given.
            worksheet.Rows[0].Cells[0].Value = "Brand";
            worksheet.Rows[0].Cells[1].Value = "Product Description";
            worksheet.Rows[0].Cells[2].Value = "NationalPLU"; // error.
            worksheet.Rows[0].Cells[3].Value = "FL";
            worksheet.Rows[0].Cells[4].Value = "MA";
            worksheet.Rows[0].Cells[5].Value = "MW";
            worksheet.Rows[0].Cells[6].Value = "NA";
            worksheet.Rows[0].Cells[7].Value = "NC";
            worksheet.Rows[0].Cells[8].Value = "NE";
            worksheet.Rows[0].Cells[9].Value = "PN";
            worksheet.Rows[0].Cells[10].Value = "RM";
            worksheet.Rows[0].Cells[11].Value = "SO";
            worksheet.Rows[0].Cells[12].Value = "SP";
            worksheet.Rows[0].Cells[13].Value = "SW";
            worksheet.Rows[0].Cells[14].Value = "UK";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            bool result = importer.ValidSpreadsheetType();

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidSpreadsheetType_SpreadsheetWithNullColumnName_VerificationFails()
        {
            // Given.
            worksheet.Rows[0].Cells[0].Value = "Brand";
            worksheet.Rows[0].Cells[1].Value = "Product Description";
            worksheet.Rows[0].Cells[4].Value = "National PLU";
            worksheet.Rows[0].Cells[5].Value = "FL";
            worksheet.Rows[0].Cells[6].Value = "MA";
            worksheet.Rows[0].Cells[7].Value = null;
            worksheet.Rows[0].Cells[8].Value = "NA";
            worksheet.Rows[0].Cells[9].Value = "NC";
            worksheet.Rows[0].Cells[10].Value = "NE";
            worksheet.Rows[0].Cells[11].Value = "PN";
            worksheet.Rows[0].Cells[12].Value = "RM";
            worksheet.Rows[0].Cells[13].Value = "SO";
            worksheet.Rows[0].Cells[14].Value = "SP";
            worksheet.Rows[0].Cells[15].Value = "SW";
            worksheet.Rows[0].Cells[16].Value = "UK";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            bool result = importer.ValidSpreadsheetType();

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidSpreadsheetType_SpreadsheetWithEmptyColumnName_VerificationFails()
        {
            // Given.
            worksheet.Rows[0].Cells[0].Value = "Brand";
            worksheet.Rows[0].Cells[1].Value = "Product Description";
            worksheet.Rows[0].Cells[4].Value = "National PLU";
            worksheet.Rows[0].Cells[5].Value = "FL";
            worksheet.Rows[0].Cells[6].Value = "MA";
            worksheet.Rows[0].Cells[7].Value = String.Empty;
            worksheet.Rows[0].Cells[8].Value = "NA";
            worksheet.Rows[0].Cells[9].Value = "NC";
            worksheet.Rows[0].Cells[10].Value = "NE";
            worksheet.Rows[0].Cells[11].Value = "PN";
            worksheet.Rows[0].Cells[12].Value = "RM";
            worksheet.Rows[0].Cells[13].Value = "SO";
            worksheet.Rows[0].Cells[14].Value = "SP";
            worksheet.Rows[0].Cells[15].Value = "SW";
            worksheet.Rows[0].Cells[16].Value = "UK";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            bool result = importer.ValidSpreadsheetType();

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_MissingNationalPlu_ShouldBeMarkedAsError()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());

            string nationalPlu = String.Empty;

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            
            worksheet.Rows[1].Cells[2].Value = nationalPlu;
            worksheet.Rows[1].Cells[3].Value = "1234";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(nationalPlu, importer.ParsedRows[0].NationalPlu);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_NullNationalPlu_ShouldBeMarkedAsError()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());

            string nationalPlu = null;

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;

            worksheet.Rows[1].Cells[2].Value = nationalPlu;
            worksheet.Rows[1].Cells[3].Value = "1234";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(String.Empty, importer.ParsedRows[0].NationalPlu);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_NationalPluWithNoRegionalMappings_ShouldBeMarkedAsError()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());

            string nationalPlu = "1234";

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = "Brand";
            worksheet.Rows[1].Cells[1].Value = "Product Description";
            
            worksheet.Rows[1].Cells[2].Value = nationalPlu;
            worksheet.Rows[1].Cells[3].Value = String.Empty;
            worksheet.Rows[1].Cells[4].Value = String.Empty;
            worksheet.Rows[1].Cells[5].Value = String.Empty;
            worksheet.Rows[1].Cells[6].Value = String.Empty;
            worksheet.Rows[1].Cells[7].Value = String.Empty;
            worksheet.Rows[1].Cells[8].Value = String.Empty;
            worksheet.Rows[1].Cells[9].Value = String.Empty;
            worksheet.Rows[1].Cells[10].Value = String.Empty;
            worksheet.Rows[1].Cells[11].Value = String.Empty;
            worksheet.Rows[1].Cells[12].Value = String.Empty;
            worksheet.Rows[1].Cells[13].Value = String.Empty;
            worksheet.Rows[1].Cells[14].Value = String.Empty;

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(0, importer.ValidRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_MultipleNationalPluWithNoRegionalMappings_ShouldBeMarkedAsError()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = "Brand";
            worksheet.Rows[1].Cells[1].Value = "Product Description";

            worksheet.Rows[1].Cells[2].Value = "1234";
            worksheet.Rows[1].Cells[3].Value = String.Empty;
            worksheet.Rows[1].Cells[4].Value = String.Empty;
            worksheet.Rows[1].Cells[5].Value = String.Empty;
            worksheet.Rows[1].Cells[6].Value = String.Empty;
            worksheet.Rows[1].Cells[7].Value = String.Empty;
            worksheet.Rows[1].Cells[8].Value = String.Empty;
            worksheet.Rows[1].Cells[9].Value = String.Empty;
            worksheet.Rows[1].Cells[10].Value = String.Empty;
            worksheet.Rows[1].Cells[11].Value = String.Empty;
            worksheet.Rows[1].Cells[12].Value = String.Empty;
            worksheet.Rows[1].Cells[13].Value = String.Empty;
            worksheet.Rows[1].Cells[14].Value = String.Empty;

            // Informational columns - not needed for the model.
            worksheet.Rows[2].Cells[0].Value = "Brand";
            worksheet.Rows[2].Cells[1].Value = "Product Description";
                           
            worksheet.Rows[2].Cells[2].Value = "2345";
            worksheet.Rows[2].Cells[3].Value = String.Empty;
            worksheet.Rows[2].Cells[4].Value = String.Empty;
            worksheet.Rows[2].Cells[5].Value = String.Empty;
            worksheet.Rows[2].Cells[6].Value = String.Empty;
            worksheet.Rows[2].Cells[7].Value = String.Empty;
            worksheet.Rows[2].Cells[8].Value = String.Empty;
            worksheet.Rows[2].Cells[9].Value = String.Empty;
            worksheet.Rows[2].Cells[10].Value = String.Empty;
            worksheet.Rows[2].Cells[11].Value = String.Empty;
            worksheet.Rows[2].Cells[12].Value = String.Empty;
            worksheet.Rows[2].Cells[13].Value = String.Empty;
            worksheet.Rows[2].Cells[14].Value = String.Empty;

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(2, importer.ParsedRows.Count);
            Assert.AreEqual(2, importer.ErrorRows.Count);
            Assert.AreEqual(0, importer.ValidRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_NationalPluWithNoRegionalMappingsAndEmptyBrandAndDescription_ShouldBeMarkedAsError()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());

            string nationalPlu = "1234";

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;

            worksheet.Rows[1].Cells[2].Value = nationalPlu;
            worksheet.Rows[1].Cells[3].Value = String.Empty;
            worksheet.Rows[1].Cells[4].Value = String.Empty;
            worksheet.Rows[1].Cells[5].Value = String.Empty;
            worksheet.Rows[1].Cells[6].Value = String.Empty;
            worksheet.Rows[1].Cells[7].Value = String.Empty;
            worksheet.Rows[1].Cells[8].Value = String.Empty;
            worksheet.Rows[1].Cells[9].Value = String.Empty;
            worksheet.Rows[1].Cells[10].Value = String.Empty;
            worksheet.Rows[1].Cells[11].Value = String.Empty;
            worksheet.Rows[1].Cells[12].Value = String.Empty;
            worksheet.Rows[1].Cells[13].Value = String.Empty;
            worksheet.Rows[1].Cells[14].Value = String.Empty;

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(0, importer.ValidRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_InvalidPlu_ShouldBeMarkedAsError()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            worksheet.Rows[1].Cells[2].Value = String.Empty;
            worksheet.Rows[1].Cells[3].Value = String.Empty;

            worksheet.Rows[1].Cells[4].Value = "1234";
            worksheet.Rows[1].Cells[5].Value = "2345a";
            worksheet.Rows[1].Cells[6].Value = "3456";
            worksheet.Rows[1].Cells[7].Value = "4567";
            worksheet.Rows[1].Cells[8].Value = "5678";
            worksheet.Rows[1].Cells[9].Value = "6789";
            worksheet.Rows[1].Cells[10].Value = "7890";
            worksheet.Rows[1].Cells[11].Value = "8901";
            worksheet.Rows[1].Cells[12].Value = "9012";
            worksheet.Rows[1].Cells[13].Value = "0123";
            worksheet.Rows[1].Cells[14].Value = "1234";
            worksheet.Rows[1].Cells[15].Value = "2345";
            worksheet.Rows[1].Cells[16].Value = "3456";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 0);
            Assert.IsTrue(importer.ErrorRows.Count == 1);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_InvalidPlus_ShouldBeTrackedForEachColumn()
        {
            // This test sometimes fails because Random() generates duplicate PLUs.

            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel> { new ScanCodeModel { ScanCode = "8888" } });
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(new List<BulkImportPluRemapModel>());

            Random random = new Random();
            for (int row = 1; row < 7; row++)
            {
                // Informational columns - not needed for the model.
                worksheet.Rows[row].Cells[0].Value = String.Empty;
                worksheet.Rows[row].Cells[1].Value = String.Empty;
                
                for (int column = 2; column <= 14; column++)
                {
                    worksheet.Rows[row].Cells[column].Value = GenerateRandomSixDigitPlu(random);
                }
            }

            worksheet.Rows[2].Cells[7].Value = "1234a";
            worksheet.Rows[6].Cells[14].Value = "1234a";
            worksheet.Rows[3].Cells[2].Value = "1234a";
            worksheet.Rows[4].Cells[2].Value = "8888"; // non-existent PLU.

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 2, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 4, String.Format("ErrorItems.Count = {0}", importer.ErrorRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_OneRegionHasDuplicateData_DuplicatesShouldBeMarkedAsInvalid()
        {
            // This test sometimes fails because Random() generates duplicate PLUs.

            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(new List<BulkImportPluRemapModel>());

            Random random = new Random();
            for (int row = 1; row < 7; row++)
            {
                // Informational columns - not needed for the model.
                worksheet.Rows[row].Cells[0].Value = String.Empty;
                worksheet.Rows[row].Cells[1].Value = String.Empty;
                
                for (int column = 2; column <= 14; column++)
                {
                    worksheet.Rows[row].Cells[column].Value = GenerateRandomSixDigitPlu(random);
                }
            }

            worksheet.Rows[1].Cells[5].Value = "1234";
            worksheet.Rows[6].Cells[5].Value = "1234";
            worksheet.Rows[3].Cells[5].Value = "1234";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 3, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 3, String.Format("ErrorItems.Count = {0}", importer.ErrorRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_DuplicateZeroOrEmptyString_ShouldNotBeMarkedAsInvalid()
        {
            // This test sometimes fails because Random() generates duplicate PLUs.

            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(new List<BulkImportPluRemapModel>());

            Random random = new Random();
            for (int row = 1; row < 7; row++)
            {
                // Informational columns - not needed for the model.
                worksheet.Rows[row].Cells[0].Value = String.Empty;
                worksheet.Rows[row].Cells[1].Value = String.Empty;

                for (int column = 2; column <= 14; column++)
                {
                    worksheet.Rows[row].Cells[column].Value = GenerateRandomSixDigitPlu(random);
                }
            }

            worksheet.Rows[1].Cells[5].Value = "0";
            worksheet.Rows[6].Cells[5].Value = "0";
            worksheet.Rows[3].Cells[5].Value = "0";

            worksheet.Rows[1].Cells[6].Value = String.Empty;
            worksheet.Rows[6].Cells[6].Value = String.Empty;
            worksheet.Rows[3].Cells[6].Value = String.Empty;

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 6, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 0, String.Format("ErrorItems.Count = {0}", importer.ErrorRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_OneRegionHasMultipleDuplicates_DuplicatesShouldBeMarkedAsInvalid()
        {
            // This test sometimes fails because Random() generates duplicate PLUs.

            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(new List<BulkImportPluRemapModel>());

            Random random = new Random();
            for (int row = 1; row < 8; row++)
            {
                // Informational columns - not needed for the model.
                worksheet.Rows[row].Cells[0].Value = String.Empty;
                worksheet.Rows[row].Cells[1].Value = String.Empty;
                
                for (int column = 2; column <= 14; column++)
                {
                    worksheet.Rows[row].Cells[column].Value = GenerateRandomSixDigitPlu(random);
                }
            }

            worksheet.Rows[1].Cells[5].Value = "1234";
            worksheet.Rows[2].Cells[5].Value = "1234";
            worksheet.Rows[3].Cells[5].Value = "1234";

            worksheet.Rows[4].Cells[5].Value = "2345";
            worksheet.Rows[5].Cells[5].Value = "2345";
            worksheet.Rows[6].Cells[5].Value = "2345";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 1, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 6, String.Format("ErrorItems.Count = {0}", importer.ErrorRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_MoreThanOneRegionHasDuplicateData_DuplicatesShouldBeMarkedAsInvalid()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(new List<BulkImportPluRemapModel>());

            Random random = new Random();
            for (int row = 1; row < 10; row++)
            {
                // Informational columns - not needed for the model.
                worksheet.Rows[row].Cells[0].Value = String.Empty;
                worksheet.Rows[row].Cells[1].Value = String.Empty;
                
                for (int column = 2; column <= 14; column++)
                {
                    worksheet.Rows[row].Cells[column].Value = GenerateRandomSixDigitPlu(random);
                }
            }

            worksheet.Rows[1].Cells[5].Value = "1234";
            worksheet.Rows[2].Cells[5].Value = "1234";
            worksheet.Rows[3].Cells[5].Value = "1234";

            worksheet.Rows[4].Cells[6].Value = "1234";
            worksheet.Rows[5].Cells[6].Value = "1234";
            worksheet.Rows[6].Cells[6].Value = "1234";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 3, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 6, String.Format("ErrorItems.Count = {0}", importer.ErrorRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_MoreThanOneRegionHasMultipleDuplicates_DuplicatesShouldBeMarkedAsInvalid()
        {
            // This test sometimes fails because Random() generates duplicate PLUs.

            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(new List<BulkImportPluRemapModel>());

            Random random = new Random();
            for (int row = 1; row < 10; row++)
            {
                // Informational columns - not needed for the model.
                worksheet.Rows[row].Cells[0].Value = String.Empty;
                worksheet.Rows[row].Cells[1].Value = String.Empty;
                
                for (int column = 2; column <= 14; column++)
                {
                    worksheet.Rows[row].Cells[column].Value = GenerateRandomSixDigitPlu(random);
                }
            }

            worksheet.Rows[1].Cells[5].Value = "1234";
            worksheet.Rows[2].Cells[5].Value = "1234";

            worksheet.Rows[3].Cells[5].Value = "2345";
            worksheet.Rows[4].Cells[5].Value = "2345";

            worksheet.Rows[5].Cells[6].Value = "3456";
            worksheet.Rows[6].Cells[6].Value = "3456";

            worksheet.Rows[7].Cells[6].Value = "4567";
            worksheet.Rows[8].Cells[6].Value = "4567";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 1, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 8, String.Format("ErrorItems.Count = {0}", importer.ErrorRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_DuplicatesForDifferentRegionsOccurOnTheSameColumn_DuplicatesShouldBeMarkedAsInvalid()
        {
            // This test sometimes fails because Random() generates duplicate PLUs.

            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(new List<BulkImportPluRemapModel>());

            Random random = new Random();
            for (int row = 1; row < 7; row++)
            {
                // Informational columns - not needed for the model.
                worksheet.Rows[row].Cells[0].Value = String.Empty;
                worksheet.Rows[row].Cells[1].Value = String.Empty;
                
                for (int column = 2; column <= 14; column++)
                {
                    worksheet.Rows[row].Cells[column].Value = GenerateRandomSixDigitPlu(random);
                }
            }

            worksheet.Rows[1].Cells[5].Value = "1234";
            worksheet.Rows[2].Cells[5].Value = "1234";

            worksheet.Rows[2].Cells[6].Value = "2345";
            worksheet.Rows[3].Cells[6].Value = "2345";

            worksheet.Rows[3].Cells[7].Value = "3456";
            worksheet.Rows[4].Cells[7].Value = "3456";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 2, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 4, String.Format("ErrorItems.Count = {0}", importer.ErrorRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RegionalPluIsZero_ShouldValidate()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(new List<BulkImportPluRemapModel>());

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;

            worksheet.Rows[1].Cells[2].Value = "1234";
            worksheet.Rows[1].Cells[3].Value = "123";
            worksheet.Rows[1].Cells[4].Value = "1234";
            worksheet.Rows[1].Cells[5].Value = "12345";
            worksheet.Rows[1].Cells[6].Value = "0";
            worksheet.Rows[1].Cells[7].Value = "654321";
            worksheet.Rows[1].Cells[8].Value = "123";
            worksheet.Rows[1].Cells[9].Value = "1234";
            worksheet.Rows[1].Cells[10].Value = "1234";
            worksheet.Rows[1].Cells[11].Value = "4321";
            worksheet.Rows[1].Cells[12].Value = "9876";
            worksheet.Rows[1].Cells[13].Value = "7654";
            worksheet.Rows[1].Cells[14].Value = "4561";

            // Informational columns - not needed for the model.
            worksheet.Rows[2].Cells[0].Value = String.Empty;
            worksheet.Rows[2].Cells[1].Value = String.Empty;
                           
            worksheet.Rows[2].Cells[2].Value = "12345678987";
            worksheet.Rows[2].Cells[3].Value = "12345678987";
            worksheet.Rows[2].Cells[4].Value = "12345678987";
            worksheet.Rows[2].Cells[5].Value = "12345678987";
            worksheet.Rows[2].Cells[6].Value = "12345678987";
            worksheet.Rows[2].Cells[7].Value = "12345678987";
            worksheet.Rows[2].Cells[8].Value = "12345678987";
            worksheet.Rows[2].Cells[9].Value = "0";
            worksheet.Rows[2].Cells[10].Value = "12345678987";
            worksheet.Rows[2].Cells[11].Value = "12345678987";
            worksheet.Rows[2].Cells[12].Value = "12345678987";
            worksheet.Rows[2].Cells[13].Value = "12345678987";
            worksheet.Rows[2].Cells[14].Value = "12345678987";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 2, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 0, String.Format("ErrorItems.Count = {0}", importer.ErrorRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_SixDigitOrLessNationalPluWithMatchingRegionalPluLength_ShouldValidate()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(new List<BulkImportPluRemapModel>());

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            
            worksheet.Rows[1].Cells[2].Value = "1234";
            worksheet.Rows[1].Cells[3].Value = "123";
            worksheet.Rows[1].Cells[4].Value = "1234";
            worksheet.Rows[1].Cells[5].Value = "12345";
            worksheet.Rows[1].Cells[6].Value = "12";
            worksheet.Rows[1].Cells[7].Value = "654321";
            worksheet.Rows[1].Cells[8].Value = "123";
            worksheet.Rows[1].Cells[9].Value = "1234";
            worksheet.Rows[1].Cells[10].Value = "1234";
            worksheet.Rows[1].Cells[11].Value = "4321";
            worksheet.Rows[1].Cells[12].Value = "9876";
            worksheet.Rows[1].Cells[13].Value = "7654";
            worksheet.Rows[1].Cells[14].Value = "4561";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 1, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 0, String.Format("ErrorItems.Count = {0}", importer.ErrorRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_SixDigitOrLessNationalPluWithNonMatchingRegionalPluLength_ShouldNotValidate()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            worksheet.Rows[1].Cells[2].Value = String.Empty;
            worksheet.Rows[1].Cells[3].Value = String.Empty;

            worksheet.Rows[1].Cells[4].Value = "123456";
            worksheet.Rows[1].Cells[5].Value = "123";
            worksheet.Rows[1].Cells[6].Value = "1234";
            worksheet.Rows[1].Cells[7].Value = "12345";
            worksheet.Rows[1].Cells[8].Value = "12";
            worksheet.Rows[1].Cells[9].Value = "65432167";
            worksheet.Rows[1].Cells[10].Value = "123";
            worksheet.Rows[1].Cells[11].Value = "123444444";
            worksheet.Rows[1].Cells[12].Value = "1234";
            worksheet.Rows[1].Cells[13].Value = "4321";
            worksheet.Rows[1].Cells[14].Value = "9876";
            worksheet.Rows[1].Cells[15].Value = "7654";
            worksheet.Rows[1].Cells[16].Value = "4561";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 0, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 1, String.Format("ErrorItems.Count = {0}", importer.ErrorRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_ElevenDigitNationalPluWithMatchingRegionalPluLength_ShouldValidate()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(new List<BulkImportPluRemapModel>());

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            
            worksheet.Rows[1].Cells[2].Value = "12345678911";
            worksheet.Rows[1].Cells[3].Value = "12345678911";
            worksheet.Rows[1].Cells[4].Value = "12345678911";
            worksheet.Rows[1].Cells[5].Value = "11234567891";
            worksheet.Rows[1].Cells[6].Value = "12345678911";
            worksheet.Rows[1].Cells[7].Value = "12345678911";
            worksheet.Rows[1].Cells[8].Value = "12345678911";
            worksheet.Rows[1].Cells[9].Value = "12345678911";
            worksheet.Rows[1].Cells[10].Value = "12345678911";
            worksheet.Rows[1].Cells[11].Value = "12345678911";
            worksheet.Rows[1].Cells[12].Value = "12345678911";
            worksheet.Rows[1].Cells[13].Value = "12345678911";
            worksheet.Rows[1].Cells[14].Value = "12345678911";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 1, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 0, String.Format("ErrorItems.Count = {0}", importer.ErrorRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_ElevenDigitNationalPluWithNonMatchingRegionalPluLength_ShouldNotValidate()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            worksheet.Rows[1].Cells[2].Value = String.Empty;
            worksheet.Rows[1].Cells[3].Value = String.Empty;

            worksheet.Rows[1].Cells[4].Value = "12345678911";
            worksheet.Rows[1].Cells[5].Value = "123456781";
            worksheet.Rows[1].Cells[6].Value = "1234567811";
            worksheet.Rows[1].Cells[7].Value = "1123456791";
            worksheet.Rows[1].Cells[8].Value = "1234567";
            worksheet.Rows[1].Cells[9].Value = "1234567911";
            worksheet.Rows[1].Cells[10].Value = "1234567911";
            worksheet.Rows[1].Cells[11].Value = "12345678911";
            worksheet.Rows[1].Cells[12].Value = "12345678911";
            worksheet.Rows[1].Cells[13].Value = "1234567911";
            worksheet.Rows[1].Cells[14].Value = "12345611";
            worksheet.Rows[1].Cells[15].Value = "1234568911";
            worksheet.Rows[1].Cells[16].Value = "1234568911";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 0, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 1, String.Format("ErrorItems.Count = {0}", importer.ErrorRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_EmptyRegionalPluCells_ShouldNotBeMatchedToNationalPlu()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>())).Returns(new List<BulkImportPluRemapModel>());

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            
            worksheet.Rows[1].Cells[2].Value = "1234";
            worksheet.Rows[1].Cells[3].Value = "1234";
            worksheet.Rows[1].Cells[4].Value = String.Empty;
            worksheet.Rows[1].Cells[5].Value = String.Empty;
            worksheet.Rows[1].Cells[6].Value = String.Empty;
            worksheet.Rows[1].Cells[7].Value = String.Empty;
            worksheet.Rows[1].Cells[8].Value = String.Empty;
            worksheet.Rows[1].Cells[9].Value = String.Empty;
            worksheet.Rows[1].Cells[10].Value = String.Empty;
            worksheet.Rows[1].Cells[11].Value = String.Empty;
            worksheet.Rows[1].Cells[12].Value = String.Empty;
            worksheet.Rows[1].Cells[13].Value = String.Empty;
            worksheet.Rows[1].Cells[14].Value = String.Empty;

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 1, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 0, String.Format("ErrorItems.Count = {0}", importer.ErrorRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_SingleRemapAttempt_RemappingShouldBeAddedToRemapCollection()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>()))
                .Returns(new List<BulkImportPluRemapModel>
                    {
                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "123456",
                            NewNationalPlu = "9876",
                            Region = "flPLU",
                            RegionalPlu = "1234"
                        }
                    }
                );

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            
            worksheet.Rows[1].Cells[2].Value = "9876";
            worksheet.Rows[1].Cells[3].Value = "1234";
            worksheet.Rows[1].Cells[4].Value = "1234";
            worksheet.Rows[1].Cells[5].Value = "1234";
            worksheet.Rows[1].Cells[6].Value = "1234";
            worksheet.Rows[1].Cells[7].Value = "1234";
            worksheet.Rows[1].Cells[8].Value = "1234";
            worksheet.Rows[1].Cells[9].Value = "1234";
            worksheet.Rows[1].Cells[10].Value = "1234";
            worksheet.Rows[1].Cells[11].Value = "1234";
            worksheet.Rows[1].Cells[12].Value = "1234";
            worksheet.Rows[1].Cells[13].Value = "1234";
            worksheet.Rows[1].Cells[14].Value = "1234";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 1, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.Remappings.Count == 1, String.Format("RemapItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 0, String.Format("ErrorItems.Count = {0}", importer.ValidRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_SingleRemapAttempt_RemappingShouldBeRemovedFromValidCollection()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>()))
                .Returns(new List<BulkImportPluRemapModel>
                    {
                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "123456",
                            NewNationalPlu = "9876",
                            Region = "flPLU",
                            RegionalPlu = "1234"
                        }
                    }
                );

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            
            worksheet.Rows[1].Cells[2].Value = "9876";
            worksheet.Rows[1].Cells[3].Value = "1234";
            worksheet.Rows[1].Cells[4].Value = "1234";
            worksheet.Rows[1].Cells[5].Value = "1234";
            worksheet.Rows[1].Cells[6].Value = "1234";
            worksheet.Rows[1].Cells[7].Value = "1234";
            worksheet.Rows[1].Cells[8].Value = "1234";
            worksheet.Rows[1].Cells[9].Value = "1234";
            worksheet.Rows[1].Cells[10].Value = "1234";
            worksheet.Rows[1].Cells[11].Value = "1234";
            worksheet.Rows[1].Cells[12].Value = "1234";
            worksheet.Rows[1].Cells[13].Value = "1234";
            worksheet.Rows[1].Cells[14].Value = "1234";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows[0].flPLU == String.Empty, "Remapped regional PLU is not an empty string.");
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RemapAttemptResultsInEmptyValidRow_ValidRowShouldBeMarkedAsRemap()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>()))
                .Returns(new List<BulkImportPluRemapModel>
                    {
                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "123456",
                            NewNationalPlu = "9876",
                            Region = "flPLU",
                            RegionalPlu = "1234"
                        }
                    }
                );

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = "My Description";
            
            worksheet.Rows[1].Cells[2].Value = "9876";
            worksheet.Rows[1].Cells[3].Value = "1234";
            worksheet.Rows[1].Cells[4].Value = String.Empty;
            worksheet.Rows[1].Cells[5].Value = String.Empty;
            worksheet.Rows[1].Cells[6].Value = String.Empty;
            worksheet.Rows[1].Cells[7].Value = String.Empty;
            worksheet.Rows[1].Cells[8].Value = String.Empty;
            worksheet.Rows[1].Cells[9].Value = String.Empty;
            worksheet.Rows[1].Cells[10].Value = String.Empty;
            worksheet.Rows[1].Cells[11].Value = String.Empty;
            worksheet.Rows[1].Cells[12].Value = String.Empty;
            worksheet.Rows[1].Cells[13].Value = String.Empty;
            worksheet.Rows[1].Cells[14].Value = String.Empty;

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.Remappings.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_MultipleRemapAttemptsForOneRegionResultInEmptyValidRows_ValidRowsShouldBeMarkedAsRemaps()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>()))
                .Returns(new List<BulkImportPluRemapModel>
                    {
                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "123456",
                            NewNationalPlu = "9876",
                            Region = "flPLU",
                            RegionalPlu = "1234"
                        },
                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "123456",
                            NewNationalPlu = "9877",
                            Region = "flPLU",
                            RegionalPlu = "1235"
                        },
                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "123456",
                            NewNationalPlu = "9878",
                            Region = "flPLU",
                            RegionalPlu = "1236"
                        }
                    }
                );

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = "My Description";

            worksheet.Rows[1].Cells[2].Value = "9876";
            worksheet.Rows[1].Cells[3].Value = "1234";
            worksheet.Rows[1].Cells[4].Value = String.Empty;
            worksheet.Rows[1].Cells[5].Value = String.Empty;
            worksheet.Rows[1].Cells[6].Value = String.Empty;
            worksheet.Rows[1].Cells[7].Value = String.Empty;
            worksheet.Rows[1].Cells[8].Value = String.Empty;
            worksheet.Rows[1].Cells[9].Value = String.Empty;
            worksheet.Rows[1].Cells[10].Value = String.Empty;
            worksheet.Rows[1].Cells[11].Value = String.Empty;
            worksheet.Rows[1].Cells[12].Value = String.Empty;
            worksheet.Rows[1].Cells[13].Value = String.Empty;
            worksheet.Rows[1].Cells[14].Value = String.Empty;

            // Informational columns - not needed for the model.
            worksheet.Rows[2].Cells[0].Value = String.Empty;
            worksheet.Rows[2].Cells[1].Value = "My Description";
                           
            worksheet.Rows[2].Cells[2].Value = "9877";
            worksheet.Rows[2].Cells[3].Value = "1235";
            worksheet.Rows[2].Cells[4].Value = String.Empty;
            worksheet.Rows[2].Cells[5].Value = String.Empty;
            worksheet.Rows[2].Cells[6].Value = String.Empty;
            worksheet.Rows[2].Cells[7].Value = String.Empty;
            worksheet.Rows[2].Cells[8].Value = String.Empty;
            worksheet.Rows[2].Cells[9].Value = String.Empty;
            worksheet.Rows[2].Cells[10].Value = String.Empty;
            worksheet.Rows[2].Cells[11].Value = String.Empty;
            worksheet.Rows[2].Cells[12].Value = String.Empty;
            worksheet.Rows[2].Cells[13].Value = String.Empty;
            worksheet.Rows[2].Cells[14].Value = String.Empty;

            // Informational columns - not needed for the model.
            worksheet.Rows[3].Cells[0].Value = String.Empty;
            worksheet.Rows[3].Cells[1].Value = "My Description";
                           
            worksheet.Rows[3].Cells[2].Value = "9878";
            worksheet.Rows[3].Cells[3].Value = "1236";
            worksheet.Rows[3].Cells[4].Value = String.Empty;
            worksheet.Rows[3].Cells[5].Value = String.Empty;
            worksheet.Rows[3].Cells[6].Value = String.Empty;
            worksheet.Rows[3].Cells[7].Value = String.Empty;
            worksheet.Rows[3].Cells[8].Value = String.Empty;
            worksheet.Rows[3].Cells[9].Value = String.Empty;
            worksheet.Rows[3].Cells[10].Value = String.Empty;
            worksheet.Rows[3].Cells[11].Value = String.Empty;
            worksheet.Rows[3].Cells[12].Value = String.Empty;
            worksheet.Rows[3].Cells[13].Value = String.Empty;
            worksheet.Rows[3].Cells[14].Value = String.Empty;

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(3, importer.Remappings.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_MultipleRemapAttemptsForTwoRegionsResultInEmptyValidRows_ValidRowsShouldBeMarkedAsRemaps()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>()))
                .Returns(new List<BulkImportPluRemapModel>
                    {
                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "123456",
                            NewNationalPlu = "9876",
                            Region = "flPLU",
                            RegionalPlu = "1234"
                        },
                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "123456",
                            NewNationalPlu = "9877",
                            Region = "flPLU",
                            RegionalPlu = "1235"
                        },
                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "123456",
                            NewNationalPlu = "9878",
                            Region = "flPLU",
                            RegionalPlu = "1236"
                        },
                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "123456",
                            NewNationalPlu = "9877",
                            Region = "maPLU",
                            RegionalPlu = "1235"
                        },
                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "123456",
                            NewNationalPlu = "9878",
                            Region = "maPLU",
                            RegionalPlu = "1236"
                        },
                    }
                );

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = "My Description";

            worksheet.Rows[1].Cells[2].Value = "9876";
            worksheet.Rows[1].Cells[3].Value = "1234";
            worksheet.Rows[1].Cells[4].Value = String.Empty;
            worksheet.Rows[1].Cells[5].Value = String.Empty;
            worksheet.Rows[1].Cells[6].Value = String.Empty;
            worksheet.Rows[1].Cells[7].Value = String.Empty;
            worksheet.Rows[1].Cells[8].Value = String.Empty;
            worksheet.Rows[1].Cells[9].Value = String.Empty;
            worksheet.Rows[1].Cells[10].Value = String.Empty;
            worksheet.Rows[1].Cells[11].Value = String.Empty;
            worksheet.Rows[1].Cells[12].Value = String.Empty;
            worksheet.Rows[1].Cells[13].Value = String.Empty;
            worksheet.Rows[1].Cells[14].Value = String.Empty;

            // Informational columns - not needed for the model.
            worksheet.Rows[2].Cells[0].Value = String.Empty;
            worksheet.Rows[2].Cells[1].Value = "My Description";

            worksheet.Rows[2].Cells[2].Value = "9877";
            worksheet.Rows[2].Cells[3].Value = "1235";
            worksheet.Rows[2].Cells[4].Value = "1235";
            worksheet.Rows[2].Cells[5].Value = String.Empty;
            worksheet.Rows[2].Cells[6].Value = String.Empty;
            worksheet.Rows[2].Cells[7].Value = String.Empty;
            worksheet.Rows[2].Cells[8].Value = String.Empty;
            worksheet.Rows[2].Cells[9].Value = String.Empty;
            worksheet.Rows[2].Cells[10].Value = String.Empty;
            worksheet.Rows[2].Cells[11].Value = String.Empty;
            worksheet.Rows[2].Cells[12].Value = String.Empty;
            worksheet.Rows[2].Cells[13].Value = String.Empty;
            worksheet.Rows[2].Cells[14].Value = String.Empty;

            // Informational columns - not needed for the model.
            worksheet.Rows[3].Cells[0].Value = String.Empty;
            worksheet.Rows[3].Cells[1].Value = "My Description";

            worksheet.Rows[3].Cells[2].Value = "9878";
            worksheet.Rows[3].Cells[3].Value = "1236";
            worksheet.Rows[3].Cells[4].Value = "1236";
            worksheet.Rows[3].Cells[5].Value = String.Empty;
            worksheet.Rows[3].Cells[6].Value = String.Empty;
            worksheet.Rows[3].Cells[7].Value = String.Empty;
            worksheet.Rows[3].Cells[8].Value = String.Empty;
            worksheet.Rows[3].Cells[9].Value = String.Empty;
            worksheet.Rows[3].Cells[10].Value = String.Empty;
            worksheet.Rows[3].Cells[11].Value = String.Empty;
            worksheet.Rows[3].Cells[12].Value = String.Empty;
            worksheet.Rows[3].Cells[13].Value = String.Empty;
            worksheet.Rows[3].Cells[14].Value = String.Empty;

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(5, importer.Remappings.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_MultipleRemapAttempts_ShouldBeAddedToRemapCollection()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>()))
                .Returns(new List<BulkImportPluRemapModel>
                    {
                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "123456",
                            NewNationalPlu = "9876",
                            Region = "flPLU",
                            RegionalPlu = "1234"
                        },

                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "234567",
                            NewNationalPlu = "8765",
                            Region = "ukPLU",
                            RegionalPlu = "3456"
                        }
                    }
                );

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            
            worksheet.Rows[1].Cells[2].Value = "9876";
            worksheet.Rows[1].Cells[3].Value = "1234";
            worksheet.Rows[1].Cells[4].Value = "1234";
            worksheet.Rows[1].Cells[5].Value = "1234";
            worksheet.Rows[1].Cells[6].Value = "1234";
            worksheet.Rows[1].Cells[7].Value = "1234";
            worksheet.Rows[1].Cells[8].Value = "1234";
            worksheet.Rows[1].Cells[9].Value = "1234";
            worksheet.Rows[1].Cells[10].Value = "1234";
            worksheet.Rows[1].Cells[11].Value = "1234";
            worksheet.Rows[1].Cells[12].Value = "1234";
            worksheet.Rows[1].Cells[13].Value = "1234";
            worksheet.Rows[1].Cells[14].Value = "1234";

            // Informational columns - not needed for the model.
            worksheet.Rows[2].Cells[0].Value = String.Empty;
            worksheet.Rows[2].Cells[1].Value = String.Empty;
            
            worksheet.Rows[2].Cells[2].Value = "8765";
            worksheet.Rows[2].Cells[3].Value = "2345";
            worksheet.Rows[2].Cells[4].Value = "2345";
            worksheet.Rows[2].Cells[5].Value = "2345";
            worksheet.Rows[2].Cells[6].Value = "2345";
            worksheet.Rows[2].Cells[7].Value = "2345";
            worksheet.Rows[2].Cells[8].Value = "2345";
            worksheet.Rows[2].Cells[9].Value = "2345";
            worksheet.Rows[2].Cells[10].Value = "2345";
            worksheet.Rows[2].Cells[11].Value = "2345";
            worksheet.Rows[2].Cells[12].Value = "2345";
            worksheet.Rows[2].Cells[13].Value = "2345";
            worksheet.Rows[2].Cells[14].Value = "3456";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows.Count == 2, String.Format("ValidItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.Remappings.Count == 2, String.Format("RemapItems.Count = {0}", importer.ValidRows.Count));
            Assert.IsTrue(importer.ErrorRows.Count == 0, String.Format("ErrorItems.Count = {0}", importer.ValidRows.Count));
        }

        [TestMethod]
        public void ValidateSpreadsheetData_MultipleRemapAttempts_RemappedPluShouldBeRemovedFromValidRows()
        {
            // Given.
            mockGetNewScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetNewScanCodeUploadsParameters>())).Returns(new List<ScanCodeModel>());
            mockRemappingQuery.Setup(q => q.Search(It.IsAny<GetPluRemappingsParameters>()))
                .Returns(new List<BulkImportPluRemapModel>
                    {
                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "123456",
                            NewNationalPlu = "9876",
                            Region = "flPLU",
                            RegionalPlu = "1234"
                        },

                        new BulkImportPluRemapModel
                        {
                            CurrentNationalPlu = "234567",
                            NewNationalPlu = "8765",
                            Region = "rmPLU",
                            RegionalPlu = "3456"
                        }
                    }
                );

            // Informational columns - not needed for the model.
            worksheet.Rows[1].Cells[0].Value = String.Empty;
            worksheet.Rows[1].Cells[1].Value = String.Empty;
            
            worksheet.Rows[1].Cells[2].Value = "9876";
            worksheet.Rows[1].Cells[3].Value = "1234";
            worksheet.Rows[1].Cells[4].Value = "1234";
            worksheet.Rows[1].Cells[5].Value = "1234";
            worksheet.Rows[1].Cells[6].Value = "1234";
            worksheet.Rows[1].Cells[7].Value = "1234";
            worksheet.Rows[1].Cells[8].Value = "1234";
            worksheet.Rows[1].Cells[9].Value = "1234";
            worksheet.Rows[1].Cells[10].Value = "1234";
            worksheet.Rows[1].Cells[11].Value = "1234";
            worksheet.Rows[1].Cells[12].Value = "1234";
            worksheet.Rows[1].Cells[13].Value = "1234";
            worksheet.Rows[1].Cells[14].Value = "1234";

            // Informational columns - not needed for the model.
            worksheet.Rows[2].Cells[0].Value = String.Empty;
            worksheet.Rows[2].Cells[1].Value = String.Empty;
            
            worksheet.Rows[2].Cells[2].Value = "8765";
            worksheet.Rows[2].Cells[3].Value = "2345";
            worksheet.Rows[2].Cells[4].Value = "2345";
            worksheet.Rows[2].Cells[5].Value = "2345";
            worksheet.Rows[2].Cells[6].Value = "2345";
            worksheet.Rows[2].Cells[7].Value = "2345";
            worksheet.Rows[2].Cells[8].Value = "2345";
            worksheet.Rows[2].Cells[9].Value = "2345";
            worksheet.Rows[2].Cells[10].Value = "3456";
            worksheet.Rows[2].Cells[11].Value = "2345";
            worksheet.Rows[2].Cells[12].Value = "2345";
            worksheet.Rows[2].Cells[13].Value = "2345";
            worksheet.Rows[2].Cells[14].Value = "2345";

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.IsTrue(importer.ValidRows[0].flPLU == String.Empty, "Remapped regional PLU is not an empty string.");
            Assert.IsTrue(importer.ValidRows[1].rmPLU == String.Empty, "Remapped regional PLU is not an empty string.");
        }

        [TestMethod]
        public void ConvertRemappingToModel_SingleRemapping_ShouldConvertToModelAndBeAddedToValidCollection()
        {
            // Given.
            var remapping = new BulkImportPluRemapModel
            {
                CurrentNationalPlu = "123456",
                CurrentNationalPluId = 99887,
                NewNationalPlu = "9876",
                NewNationalPluId = 88997,
                Region = "flPLU",
                RegionalPlu = "1234"
            };

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Remappings.Add(remapping);

            // When.
            importer.ConvertRemappingsToModel();

            // Then.
            Assert.AreEqual(remapping.NewNationalPlu, importer.ValidRows[0].NationalPlu);
            Assert.AreEqual(remapping.RegionalPlu, importer.ValidRows[0].flPLU);
        }

        [TestMethod]
        public void ConvertRemappingToModel_MultipleRemappingsForDifferentRegions_ShouldConvertToModelAndBeAddedToValidCollection()
        {
            // Given.
            var remappings = new List<BulkImportPluRemapModel>
            {
                new BulkImportPluRemapModel
                {
                    CurrentNationalPlu = "123456",
                    CurrentNationalPluId = 99887,
                    NewNationalPlu = "9876",
                    NewNationalPluId = 88997,
                    Region = "flPLU",
                    RegionalPlu = "1234"
                },

                new BulkImportPluRemapModel
                {
                    CurrentNationalPlu = "2345",
                    CurrentNationalPluId = 88887,
                    NewNationalPlu = "3456",
                    NewNationalPluId = 88886,
                    Region = "swPLU",
                    RegionalPlu = "1234"
                }
            };

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Remappings = remappings;

            // When.
            importer.ConvertRemappingsToModel();

            // Then.
            Assert.AreEqual(remappings[0].NewNationalPlu, importer.ValidRows[0].NationalPlu);
            Assert.AreEqual(remappings[0].RegionalPlu, importer.ValidRows[0].flPLU);
            Assert.AreEqual(remappings[1].NewNationalPlu, importer.ValidRows[1].NationalPlu);
            Assert.AreEqual(remappings[1].RegionalPlu, importer.ValidRows[1].swPLU);
            Assert.AreEqual(2, importer.ValidRows.Count);
        }

        [TestMethod]
        public void ConvertRemappingToModel_AfterConversion_ModelShouldContainEmptyStringsForUnmappedRegionalPlu()
        {
            // Given.
            var remapping = new BulkImportPluRemapModel
            {
                CurrentNationalPlu = "123456",
                CurrentNationalPluId = 99887,
                NewNationalPlu = "9876",
                NewNationalPluId = 88997,
                Region = "flPLU",
                RegionalPlu = "1234"
            };

            PluSpreadsheetImporter importer = new PluSpreadsheetImporter(mockGetNewScanCodeUploadsQuery.Object, mockRemappingQuery.Object);
            importer.Remappings.Add(remapping);

            // When.
            importer.ConvertRemappingsToModel();

            // Then.
            Assert.AreEqual(remapping.NewNationalPlu, importer.ValidRows[0].NationalPlu);
            Assert.AreEqual(remapping.RegionalPlu, importer.ValidRows[0].flPLU);
            Assert.AreEqual(String.Empty, importer.ValidRows[0].maPLU);
        }

        private string GenerateRandomSixDigitPlu(Random random)
        {
            string plu = random.Next(1, 9).ToString();

            for (int i = 0; i < 5; i++)
            {
                plu += random.Next(0, 9).ToString();
            }

            return plu;
        }
    }
}
