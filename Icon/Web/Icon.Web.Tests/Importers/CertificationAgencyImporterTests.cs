using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Importers;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Unit.Importers
{
    [TestClass]
    public class CertificationAgencyImporterTests
    {
        private CertificationAgencyImporter importer;
        private Mock<IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>>> mockGetCertificationAgenciesQueryHandler;
        private Mock<IQueryHandler<GetCertificationAgencyIdsAssociatedToItemsParameters, List<int>>> mockGetCertificationAgencyIdsAssociatedToItemsQueryHandler;
        private Workbook workbook;
        private Worksheet worksheet;
        private List<CertificationAgencyModel> testCertificationAgencyModels;

        [TestInitialize]
        public void Initialize()
        {
            mockGetCertificationAgenciesQueryHandler = new Mock<IQueryHandler<GetCertificationAgenciesParameters,List<CertificationAgencyModel>>>();
            mockGetCertificationAgencyIdsAssociatedToItemsQueryHandler = new Mock<IQueryHandler<GetCertificationAgencyIdsAssociatedToItemsParameters, List<int>>>();
            
            importer = new CertificationAgencyImporter(mockGetCertificationAgenciesQueryHandler.Object,
                mockGetCertificationAgencyIdsAssociatedToItemsQueryHandler.Object);

            workbook = new Workbook();
            importer.Workbook = workbook;
            worksheet = workbook.Worksheets.Add("Test");

            testCertificationAgencyModels = new List<CertificationAgencyModel>();
            mockGetCertificationAgenciesQueryHandler.Setup(m => m.Search(It.IsAny<GetCertificationAgenciesParameters>()))
                .Returns(testCertificationAgencyModels);
            mockGetCertificationAgencyIdsAssociatedToItemsQueryHandler.Setup(m => m.Search(It.IsAny<GetCertificationAgencyIdsAssociatedToItemsParameters>()))
                .Returns(new List<int>());
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_ValidDataInSpreadsheet_ShouldPopulateParsedRows()
        {
            //Given
            List<BulkImportCertificationAgencyModel> expectedAgencies = CreateTestAgencies();

            for (int i = 0; i < 6; i++)
            {
                worksheet.Rows[0].Cells[i].Value = "Test Header";
            }
            for (int i = 0, j = 1; i < expectedAgencies.Count;i++, j++)
            {
                worksheet.Rows[j].Cells[0].Value = expectedAgencies[i].CertificationAgencyNameAndId;
                worksheet.Rows[j].Cells[1].Value = ConvertToBoolString(expectedAgencies[i].GlutenFree);
                worksheet.Rows[j].Cells[2].Value = ConvertToBoolString(expectedAgencies[i].Kosher);
                worksheet.Rows[j].Cells[3].Value = ConvertToBoolString(expectedAgencies[i].NonGmo);
                worksheet.Rows[j].Cells[4].Value = ConvertToBoolString(expectedAgencies[i].Organic);
                worksheet.Rows[j].Cells[5].Value = ConvertToBoolString(expectedAgencies[i].Vegan);
            }

            //When
            importer.ConvertSpreadsheetToModel();

            //Then
            var actualAgencies = importer.ParsedRows;

            Assert.AreEqual(expectedAgencies.Count, actualAgencies.Count);
            for (int i = 0; i < expectedAgencies.Count; i++)
            {
                Assert.AreEqual(expectedAgencies[i].CertificationAgencyNameAndId, actualAgencies[i].CertificationAgencyNameAndId);
                Assert.AreEqual(expectedAgencies[i].CertificationAgencyNameAndId.Split('|').First(), actualAgencies[i].CertificationAgencyName);
                Assert.AreEqual(expectedAgencies[i].CertificationAgencyNameAndId.Split('|').Last(), actualAgencies[i].CertificationAgencyId);
                Assert.AreEqual(expectedAgencies[i].GlutenFree, actualAgencies[i].GlutenFree);
                Assert.AreEqual(expectedAgencies[i].Kosher, actualAgencies[i].Kosher);
                Assert.AreEqual(expectedAgencies[i].NonGmo, actualAgencies[i].NonGmo);
                Assert.AreEqual(expectedAgencies[i].Organic, actualAgencies[i].Organic);
                Assert.AreEqual(expectedAgencies[i].Vegan, actualAgencies[i].Vegan);
            }
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_ValidDataInSpreadsheet_ShouldSkipBlankRows()
        {
            //Given
            for (int i = 0; i < 6; i++)
            {
                worksheet.Rows[0].Cells[i].Value = "Test Header";
            }
            worksheet.Rows[1].Cells[0].Value = string.Empty;

            worksheet.Rows[100].Cells[0].Value = "Test|123";
            worksheet.Rows[100].Cells[1].Value = "Y";
            worksheet.Rows[100].Cells[2].Value = "Y";
            worksheet.Rows[100].Cells[3].Value = "Y";
            worksheet.Rows[100].Cells[4].Value = "Y";
            worksheet.Rows[100].Cells[5].Value = "Y";

            //When
            importer.ConvertSpreadsheetToModel();

            //Then
            var parsedRows = importer.ParsedRows;

            Assert.AreEqual(1, parsedRows.Count);
            Assert.AreEqual("Test|123", parsedRows[0].CertificationAgencyNameAndId);
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_SpreadsheetHasMoreThan1000Rows_ShouldOnlyProcessTheFirst1000Rows()
        {
            //Given
            for (int i = 0; i < 6; i++)
            {
                worksheet.Rows[0].Cells[i].Value = "Test Header";
            }
            for (int i = 1; i < 2000; i++)
            {
                worksheet.Rows[i].Cells[0].Value = "Test|123";
            }

            //When
            importer.ConvertSpreadsheetToModel();

            //Then
            Assert.AreEqual(1000, importer.ParsedRows.Count);
        }

        [TestMethod]
        public void IsValidSpreadsheetType_HeadersAreValid_ShouldReturnTrue()
        {
            //Given
            worksheet.Rows[0].Cells[0].Value = "Agency";
            worksheet.Rows[0].Cells[1].Value = "Gluten Free";
            worksheet.Rows[0].Cells[2].Value = "Kosher";
            worksheet.Rows[0].Cells[3].Value = "Non-GMO";
            worksheet.Rows[0].Cells[4].Value = "Organic";
            worksheet.Rows[0].Cells[5].Value = "Vegan";


            //When
            var result = importer.IsValidSpreadsheetType();

            //Then
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidSpreadsheetType_HeadersAreNotValid_ShouldReturnFalse()
        {
            //Given
            worksheet.Rows[0].Cells[0].Value = "Smagency";
            worksheet.Rows[0].Cells[1].Value = "Gluten Free";
            worksheet.Rows[0].Cells[2].Value = "Kosher";
            worksheet.Rows[0].Cells[3].Value = "Non-GMO";
            worksheet.Rows[0].Cells[4].Value = "Organic";
            worksheet.Rows[0].Cells[5].Value = "Vegan";


            //When
            var result = importer.IsValidSpreadsheetType();

            //Then
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_DataIsValid_ShouldAddRowsToValidRows()
        {
            //Given
            var agencies = CreateTestAgencies();
            importer.ParsedRows = agencies;

            testCertificationAgencyModels.AddRange(CreateTestAgencies().Select(a => new CertificationAgencyModel
                    {
                        HierarchyClassId = int.Parse(a.CertificationAgencyId)
                    }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(3, importer.ValidRows.Count);
            Assert.AreEqual(0, importer.ErrorRows.Count);
            Assert.AreEqual(3, importer.ParsedRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_DataHasNewCertificationAgency_ShouldAddAgencyToValidRows()
        {
            //Given
            var agency = CreateTestAgencies().First();
            agency.CertificationAgencyName = "Test New Agency";
            agency.CertificationAgencyId = "0";
            importer.ParsedRows.Add(agency);

            testCertificationAgencyModels.AddRange(CreateTestAgencies().Select(a => new CertificationAgencyModel
            {
                HierarchyClassId = int.Parse(a.CertificationAgencyId)
            }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(1, importer.ValidRows.Count);
            Assert.AreEqual(0, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RowHasNegativeId_ShouldAddRowToInvalidRows()
        {
            //Given
            var agency = CreateTestAgencies().First();
            agency.CertificationAgencyName = "Test New Agency";
            agency.CertificationAgencyId = "-1";
            importer.ParsedRows.Add(agency);

            testCertificationAgencyModels.AddRange(CreateTestAgencies().Select(a => new CertificationAgencyModel
            {
                HierarchyClassId = int.Parse(a.CertificationAgencyId)
            }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(string.Format("{0} is not a valid ID. IDs must be integers greater than negative 1.", agency.CertificationAgencyId), agency.Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RowHasEmptyId_ShouldAddRowToInvalidRows()
        {
            //Given
            var agency = CreateTestAgencies().First();
            agency.CertificationAgencyName = "Test New Agency";
            agency.CertificationAgencyId = "";
            importer.ParsedRows.Add(agency);

            testCertificationAgencyModels.AddRange(CreateTestAgencies().Select(a => new CertificationAgencyModel
            {
                HierarchyClassId = int.Parse(a.CertificationAgencyId)
            }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(string.Format("{0} is not a valid ID. IDs must be integers greater than negative 1.", agency.CertificationAgencyId), agency.Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RowHasNonIntegerId_ShouldAddRowToInvalidRows()
        {
            //Given
            var agency = CreateTestAgencies().First();
            agency.CertificationAgencyName = "Test New Agency";
            agency.CertificationAgencyId = "Bad";
            importer.ParsedRows.Add(agency);

            testCertificationAgencyModels.AddRange(CreateTestAgencies().Select(a => new CertificationAgencyModel
            {
                HierarchyClassId = int.Parse(a.CertificationAgencyId)
            }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(string.Format("{0} is not a valid ID. IDs must be integers greater than negative 1.", agency.CertificationAgencyId), agency.Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RowHasInvalidName_ShouldAddRowToInvalidRows()
        {
            //Given
            var agency = CreateTestAgencies().First();
            agency.CertificationAgencyName = "";
            agency.CertificationAgencyId = "123";
            importer.ParsedRows.Add(agency);

            testCertificationAgencyModels.AddRange(CreateTestAgencies().Select(a => new CertificationAgencyModel
            {
                HierarchyClassId = int.Parse(a.CertificationAgencyId)
            }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual("Certification Agency Name cannot be empty.", agency.Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_WorksheetHasDuplicateNames_ShouldAddRowToInvalidRows()
        {
            //Given
            var agency1 = CreateTestAgencies()[0];
            agency1.CertificationAgencyNameAndId = "Agency1|123";
            agency1.CertificationAgencyName = "Agency1";
            agency1.CertificationAgencyId = "123";
            importer.ParsedRows.Add(agency1);

            var agency2 = CreateTestAgencies()[1];
            agency2.CertificationAgencyNameAndId = "Agency1|1234";
            agency2.CertificationAgencyName = "Agency1";
            agency2.CertificationAgencyId = "1234";
            importer.ParsedRows.Add(agency2);

            testCertificationAgencyModels.AddRange(CreateTestAgencies().Select(a => new CertificationAgencyModel
            {
                HierarchyClassId = int.Parse(a.CertificationAgencyId)
            }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(2, importer.ErrorRows.Count);
            Assert.AreEqual(2, importer.ParsedRows.Count);
            Assert.AreEqual(string.Format("The name {0} appears multiple times on the spreadsheet", agency1.CertificationAgencyName), agency1.Error);
            Assert.AreEqual(string.Format("The name {0} appears multiple times on the spreadsheet", agency2.CertificationAgencyName), agency2.Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_WorksheetHasDuplicateIds_ShouldAddRowToInvalidRows()
        {
            //Given
            var agency1 = CreateTestAgencies()[0];
            agency1.CertificationAgencyName = "Agency1";
            agency1.CertificationAgencyId = "123";
            importer.ParsedRows.Add(agency1);

            var agency2 = CreateTestAgencies()[1];
            agency2.CertificationAgencyName = "Agency2";
            agency2.CertificationAgencyId = "123";
            importer.ParsedRows.Add(agency2);

            testCertificationAgencyModels.AddRange(CreateTestAgencies().Select(a => new CertificationAgencyModel
            {
                HierarchyClassId = int.Parse(a.CertificationAgencyId)
            }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(2, importer.ErrorRows.Count);
            Assert.AreEqual(2, importer.ParsedRows.Count);
            Assert.AreEqual(string.Format("The ID {0} appears multiple times on the spreadsheet", agency1.CertificationAgencyId), agency1.Error);
            Assert.AreEqual(string.Format("The ID {0} appears multiple times on the spreadsheet", agency2.CertificationAgencyId), agency2.Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RowHasNonExistentId_ShouldAddRowToInvalidRows()
        {
            //Given
            var agency = CreateTestAgencies()[0];
            agency.CertificationAgencyName = "Agency1";
            agency.CertificationAgencyId = "123";
            importer.ParsedRows.Add(agency);

            testCertificationAgencyModels.AddRange(CreateTestAgencies()
                .Skip(1)
                .Select(a => new CertificationAgencyModel
                    {
                        HierarchyClassId = int.Parse(a.CertificationAgencyId)
                    }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(string.Format("The ID {0} does not exist or is not associated to a Certification Agency.", agency.CertificationAgencyId), agency.Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_AgencyAlreadyExists_ShouldAddRowToInvalidRows()
        {
            //Given
            var agency = CreateTestAgencies()[0];
            agency.CertificationAgencyId = "0";
            importer.ParsedRows.Add(agency);

            testCertificationAgencyModels.AddRange(CreateTestAgencies()
                .Select(a => new CertificationAgencyModel
                {
                    HierarchyClassId = int.Parse(a.CertificationAgencyId),
                    HierarchyClassName = a.CertificationAgencyName
                }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(string.Format("{0} already exists.", agency.CertificationAgencyName), agency.Error);

        }

        [TestMethod]
        public void ValidateSpreadsheetData_RowHasInvalidGlutenFree_ShouldAddRowToInvalidRows()
        {
            //Given
            var agency = CreateTestAgencies()[0];
            agency.GlutenFree = "Bad";
            importer.ParsedRows.Add(agency);

            testCertificationAgencyModels.AddRange(CreateTestAgencies()
                .Select(a => new CertificationAgencyModel
                {
                    HierarchyClassId = int.Parse(a.CertificationAgencyId),
                }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(string.Format("{0} is not a valid value for Gluten Free. Values must be Y or N.", agency.GlutenFree), agency.Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RowHasInvalidKosher_ShouldAddRowToInvalidRows()
        {
            //Given
            var agency = CreateTestAgencies()[0];
            agency.GlutenFree = "Bad";
            importer.ParsedRows.Add(agency);

            testCertificationAgencyModels.AddRange(CreateTestAgencies()
                .Select(a => new CertificationAgencyModel
                {
                    HierarchyClassId = int.Parse(a.CertificationAgencyId),
                }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(string.Format("{0} is not a valid value for Gluten Free. Values must be Y or N.", agency.GlutenFree), agency.Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RowHasInvalidNonGmo_ShouldAddRowToInvalidRows()
        {

            //Given
            var agency = CreateTestAgencies()[0];
            agency.NonGmo = "Bad";
            importer.ParsedRows.Add(agency);

            testCertificationAgencyModels.AddRange(CreateTestAgencies()
                .Select(a => new CertificationAgencyModel
                {
                    HierarchyClassId = int.Parse(a.CertificationAgencyId),
                }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(string.Format("{0} is not a valid value for Non-GMO. Values must be Y or N.", agency.NonGmo), agency.Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RowHasInvalidOrganic_ShouldAddRowToInvalidRows()
        {

            //Given
            var agency = CreateTestAgencies()[0];
            agency.Organic = "Bad";
            importer.ParsedRows.Add(agency);

            testCertificationAgencyModels.AddRange(CreateTestAgencies()
                .Select(a => new CertificationAgencyModel
                {
                    HierarchyClassId = int.Parse(a.CertificationAgencyId),
                }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(string.Format("{0} is not a valid value for Organic. Values must be Y or N.", agency.Organic), agency.Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RowHasInvalidVegan_ShouldAddRowToInvalidRows()
        {

            //Given
            var agency = CreateTestAgencies()[0];
            agency.Vegan = "Bad";
            importer.ParsedRows.Add(agency);

            testCertificationAgencyModels.AddRange(CreateTestAgencies()
                .Select(a => new CertificationAgencyModel
                {
                    HierarchyClassId = int.Parse(a.CertificationAgencyId),
                }));

            //When
            importer.ValidateSpreadsheetData();

            //Then
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(string.Format("{0} is not a valid value for Vegan. Values must be Y or N.", agency.Vegan), agency.Error);
        }

        public string ConvertToBoolString(string cellText)
        {
            if(string.IsNullOrWhiteSpace(cellText))
            {
                return string.Empty;
            }
            else if(cellText == "1")
            {
                return "Y";
            }
            else if(cellText == "0")
            {
                return "N";
            }
            else
            {
                return cellText;
            }
        }

        private List<BulkImportCertificationAgencyModel> CreateTestAgencies()
        {
            List<BulkImportCertificationAgencyModel> expectedAgencies = new List<BulkImportCertificationAgencyModel>
            {
                new BulkImportCertificationAgencyModel
                {
                    CertificationAgencyNameAndId = "Test Name1|123",
                    CertificationAgencyId = "123",
                    CertificationAgencyName = "Test Name1",
                    GlutenFree = "1",
                    Kosher = "1",
                    NonGmo = "1",
                    Organic = "1",
                    Vegan = "1"
                },
                new BulkImportCertificationAgencyModel
                {
                    CertificationAgencyNameAndId = "Test Name2|1234",
                    CertificationAgencyId = "1234",
                    CertificationAgencyName = "Test Name2",
                    GlutenFree = "0",
                    Kosher = "0",
                    NonGmo = "0",
                    Organic = "0",
                    Vegan = "0"
                },
                new BulkImportCertificationAgencyModel
                {
                    CertificationAgencyNameAndId = "Test Name3|12345",
                    CertificationAgencyId = "12345",
                    CertificationAgencyName = "Test Name3",
                    GlutenFree = "",
                    Kosher = "",
                    NonGmo = "",
                    Organic = "",
                    Vegan = ""
                }
            };
            return expectedAgencies;
        }
    }
}
