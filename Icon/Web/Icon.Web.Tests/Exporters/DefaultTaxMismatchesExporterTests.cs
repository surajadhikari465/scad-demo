using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Icon.Web.Tests.Common.Builders;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass]
    public class DefaultTaxMismatchesExporterTests
    {
        private WorkbookFormat excelFormat;
        private ExcelExportModel exportModel;
        private List<DefaultTaxClassMismatchExportModel> exportData;
        private DefaultTaxMismatchesExporter exporter;
        private Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>> mockGetHierarchyLineageQueryHandler;
        private string testScanCode;
        private string testBrand;
        private string testProductDescription;
        private string testMerchandiseLineage;
        private string testDefaultTaxClass;
        private string testTaxClassOverride;
        private string testError;

        [TestInitialize]
        public void Initialize()
        {
            mockGetHierarchyLineageQueryHandler = new Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>()))
                .Returns(new HierarchyClassListModel { TaxHierarchyList = new List<HierarchyClassModel>() });

            excelFormat = WorkbookFormat.Excel2007;
            exportModel = new ExcelExportModel(excelFormat);

            exporter = new DefaultTaxMismatchesExporter(mockGetHierarchyLineageQueryHandler.Object);
            exporter.ExportModel = exportModel;
            exporter.ExportData = exportData;

            testScanCode = "2222222";
            testBrand = "Brand";
            testProductDescription = "Desc";
            testMerchandiseLineage = "Merch|Merch|01";
            testDefaultTaxClass = "Tax";
            testTaxClassOverride = "Tax";
            testError = "Error";
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DefaultTaxMismatchesExporter_ExportDataIsNull_ExceptionShouldBeThrown()
        {
            // Given.
            exporter.ExportData = null;

            // When.
            exporter.Export();

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void DefaultTaxMismatchesExporter_Export_WorksheetShouldBeNamedCorrectly()
        {
            // Given.
            exportData = new List<DefaultTaxClassMismatchExportModel>
            {
                new DefaultTaxClassMismatchExportModel()
            };

            exporter.ExportData = exportData;

            // When.
            exporter.Export();

            // Then.
            Worksheet worksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.AreEqual(worksheet.Name, "Default Tax Class Mismatches");
        }

        [TestMethod]
        public void DefaultTaxMismatchesExporter_Export_CorrectHeadersShouldBeAssigned()
        {
            // Given.
            exportData = new List<DefaultTaxClassMismatchExportModel>
            {
                new DefaultTaxClassMismatchExportModel()
            };

            exporter.ExportData = exportData;

            // When.
            exporter.Export();

            // Then.
            Worksheet worksheet = exportModel.ExcelWorkbook.Worksheets[0];
            var headerRow = worksheet.Rows[0];

            Assert.AreEqual(headerRow.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ScanCodeColumnIndex].Value, ExcelHelper.ExcelExportColumnNames.ScanCode);
            Assert.AreEqual(headerRow.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.BrandColumnIndex].Value, ExcelHelper.ExcelExportColumnNames.Brand);
            Assert.AreEqual(headerRow.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ProductDescriptionColumnIndex].Value, ExcelHelper.ExcelExportColumnNames.ProductDescription);
            Assert.AreEqual(headerRow.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.MerchandiseLineage].Value, ExcelHelper.ExcelExportColumnNames.Merchandise);
            Assert.AreEqual(headerRow.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.DefaultTaxClassColumnIndex].Value, ExcelHelper.DefaultTaxMismatchesColumnNames.DefaultTaxClass);
            Assert.AreEqual(headerRow.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.TaxClassOverrideColumnIndex].Value, ExcelHelper.DefaultTaxMismatchesColumnNames.TaxClassOverride);
        }

        [TestMethod]
        public void DefaultTaxMismatchesExporter_ExportWithErrors_ErrorColumnShouldBeDisplayed()
        {
            // Given.
            exportData = new List<DefaultTaxClassMismatchExportModel>
            {
                new DefaultTaxClassMismatchExportModel
                {
                    Error = testError
                }
            };

            exporter.ExportData = exportData;

            // When.
            exporter.Export();

            // Then.
            Worksheet worksheet = exportModel.ExcelWorkbook.Worksheets[0];
            var headerRow = worksheet.Rows[0];

            Assert.AreEqual(headerRow.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ScanCodeColumnIndex].Value, ExcelHelper.ExcelExportColumnNames.ScanCode);
            Assert.AreEqual(headerRow.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.BrandColumnIndex].Value, ExcelHelper.ExcelExportColumnNames.Brand);
            Assert.AreEqual(headerRow.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ProductDescriptionColumnIndex].Value, ExcelHelper.ExcelExportColumnNames.ProductDescription);
            Assert.AreEqual(headerRow.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.MerchandiseLineage].Value, ExcelHelper.ExcelExportColumnNames.Merchandise);
            Assert.AreEqual(headerRow.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.DefaultTaxClassColumnIndex].Value, ExcelHelper.DefaultTaxMismatchesColumnNames.DefaultTaxClass);
            Assert.AreEqual(headerRow.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.TaxClassOverrideColumnIndex].Value, ExcelHelper.DefaultTaxMismatchesColumnNames.TaxClassOverride);
            Assert.AreEqual(headerRow.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ErrorColumnIndex].Value, ExcelHelper.ExcelExportColumnNames.Error);
        }

        [TestMethod]
        public void DefaultTaxMismatchesExporter_Export_WorksheetShouldBePopulatedWithData()
        {
            // Given.
            exportData = new List<DefaultTaxClassMismatchExportModel>
            {
                new DefaultTaxClassMismatchExportModel
                {
                    ScanCode = testScanCode,
                    Brand = testBrand,
                    ProductDescription = testProductDescription,
                    MerchandiseLineage = testMerchandiseLineage,
                    DefaultTaxClass = testDefaultTaxClass,
                    TaxClassOverride = testTaxClassOverride
                },
                new DefaultTaxClassMismatchExportModel
                {
                    ScanCode = testScanCode,
                    Brand = testBrand,
                    ProductDescription = testProductDescription,
                    MerchandiseLineage = testMerchandiseLineage,
                    DefaultTaxClass = testDefaultTaxClass,
                    TaxClassOverride = testTaxClassOverride
                },
                new DefaultTaxClassMismatchExportModel
                {
                    ScanCode = testScanCode,
                    Brand = testBrand,
                    ProductDescription = testProductDescription,
                    MerchandiseLineage = testMerchandiseLineage,
                    DefaultTaxClass = testDefaultTaxClass,
                    TaxClassOverride = testTaxClassOverride
                }
            };

            exporter.ExportData = exportData;

            // When.
            exporter.Export();

            // Then.
            Worksheet worksheet = exportModel.ExcelWorkbook.Worksheets[0];
            var rows = worksheet.Rows;

            foreach (var row in rows)
            {
                if (row.Index == 0) { continue; }

                Assert.AreEqual(row.Cells[ExcelHelper. DefaultTaxMismatchesColumnIndexes.ScanCodeColumnIndex].Value, testScanCode);
                Assert.AreEqual(row.Cells[ExcelHelper. DefaultTaxMismatchesColumnIndexes.BrandColumnIndex].Value, testBrand);
                Assert.AreEqual(row.Cells[ExcelHelper. DefaultTaxMismatchesColumnIndexes.ProductDescriptionColumnIndex].Value, testProductDescription);
                Assert.AreEqual(row.Cells[ExcelHelper. DefaultTaxMismatchesColumnIndexes.MerchandiseLineage].Value, testMerchandiseLineage);
                Assert.AreEqual(row.Cells[ExcelHelper. DefaultTaxMismatchesColumnIndexes.DefaultTaxClassColumnIndex].Value, testDefaultTaxClass);
                Assert.AreEqual(row.Cells[ExcelHelper. DefaultTaxMismatchesColumnIndexes.TaxClassOverrideColumnIndex].Value, testTaxClassOverride);
            }
        }

        [TestMethod]
        public void DefaultTaxMismatchesExporter_ExportWithErrors_WorksheetShouldBePopulatedWithDataAndErrors()
        {
            // Given.
            exportData = new List<DefaultTaxClassMismatchExportModel>
            {
                new DefaultTaxClassMismatchExportModel
                {
                    ScanCode = testScanCode,
                    Brand = testBrand,
                    ProductDescription = testProductDescription,
                    MerchandiseLineage = testMerchandiseLineage,
                    DefaultTaxClass = testDefaultTaxClass,
                    TaxClassOverride = testTaxClassOverride,
                    Error = testError
                },
                new DefaultTaxClassMismatchExportModel
                {
                    ScanCode = testScanCode,
                    Brand = testBrand,
                    ProductDescription = testProductDescription,
                    MerchandiseLineage = testMerchandiseLineage,
                    DefaultTaxClass = testDefaultTaxClass,
                    TaxClassOverride = testTaxClassOverride,
                    Error = testError
                },
                new DefaultTaxClassMismatchExportModel
                {
                    ScanCode = testScanCode,
                    Brand = testBrand,
                    ProductDescription = testProductDescription,
                    MerchandiseLineage = testMerchandiseLineage,
                    DefaultTaxClass = testDefaultTaxClass,
                    TaxClassOverride = testTaxClassOverride,
                    Error = testError
                }
            };

            exporter.ExportData = exportData;

            // When.
            exporter.Export();

            // Then.
            Worksheet worksheet = exportModel.ExcelWorkbook.Worksheets[0];
            var rows = worksheet.Rows;

            foreach (var row in rows)
            {
                if (row.Index == 0) { continue; }

                Assert.AreEqual(row.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ScanCodeColumnIndex].Value, testScanCode);
                Assert.AreEqual(row.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.BrandColumnIndex].Value, testBrand);
                Assert.AreEqual(row.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ProductDescriptionColumnIndex].Value, testProductDescription);
                Assert.AreEqual(row.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.MerchandiseLineage].Value, testMerchandiseLineage);
                Assert.AreEqual(row.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.DefaultTaxClassColumnIndex].Value, testDefaultTaxClass);
                Assert.AreEqual(row.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.TaxClassOverrideColumnIndex].Value, testTaxClassOverride);
                Assert.AreEqual(row.Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.ErrorColumnIndex].Value, testError);
            }
        }

        [TestMethod]
        public void DefaultTaxMismatchesExporter_TaxOverrideCell_ShouldHaveValidationRules()
        {
            // Given.
            mockGetHierarchyLineageQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyLineageParameters>()))
                .Returns(new HierarchyClassListModel
                    {
                        TaxHierarchyList = new List<HierarchyClassModel> {new TestHierarchyClassModelBuilder()
                                .WithHierarchyClassId(Hierarchies.Tax)
                                .WithHierarchyClassName("Test Tax")
                                .WithHierarchyClassLineage("Test Tax")
                    }
                });

            exportData = new List<DefaultTaxClassMismatchExportModel>
            {
                new DefaultTaxClassMismatchExportModel()
            };

            exporter.ExportData = exportData;

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.DefaultTaxMismatchesColumnIndexes.TaxClassOverrideColumnIndex].DataValidationRule);
        }
    }
}
