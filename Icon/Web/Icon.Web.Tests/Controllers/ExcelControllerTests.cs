using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.ModelMappers;
using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Mvc.Excel.Services;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass] [Ignore]
    public class ExcelControllerTests
    {
        private ExcelController controller;
        private Mock<ControllerContext> context;
        private Mock<HttpSessionStateBase> session;
        private Mock<HttpResponseBase> response;
        private Mock<Stream> outputStream;
        private Mock<IExcelExporterService> mockExcelExporterService;
        private Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>> mockGetHierarchyQuery;
        private Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>> mockGetHierarchyClassQuery;
        private Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>> mockGetHierarchyLineageQueryHandler;
        private Mock<IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>>> mockGetMerchTaxMappingsQueryHandler;
        private Mock<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>> mockGetCertificationAgenciesByTraitQueryHandler;
        private Mock<IQueryHandler<GetBrandsParameters, List<BrandModel>>> mockGetBrandsQueryHandler;
        private Mock<IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>>> mockBulkScanCodeSearchQueryHandler;
        private Mock<IQueryHandler<GetItemsBySearchParameters, ItemsBySearchResultsModel>> mockGetItemsBySearchQueryHandler;
        private Mock<IQueryHandler<GetDefaultTaxClassMismatchesParameters, List<DefaultTaxClassMismatchModel>>> mockGetDefaultTaxClassMismatchesQuery;
        private Mock<IInfragisticsHelper> mockInfragisticsHelper;
        private Mock<IExcelService<ItemExcelModel>> mockItemExcelService;
        private Mock<IExcelModelMapper<ItemSearchModel, ItemExcelModel>> mockItemModelMapper;

        [TestInitialize]
        public void Initialize()
        {
            context = new Mock<ControllerContext>();
            session = new Mock<HttpSessionStateBase>();
            response = new Mock<HttpResponseBase>();
            outputStream = new Mock<Stream>();

            mockExcelExporterService = new Mock<IExcelExporterService>();
            mockGetHierarchyQuery = new Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>>();
            mockGetHierarchyClassQuery = new Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>>();
            mockGetHierarchyLineageQueryHandler = new Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            mockGetMerchTaxMappingsQueryHandler = new Mock<IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>>>();
            mockGetCertificationAgenciesByTraitQueryHandler = new Mock<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>>();       
            mockGetBrandsQueryHandler = new Mock<IQueryHandler<GetBrandsParameters, List<BrandModel>>>();
            mockBulkScanCodeSearchQueryHandler = new Mock<IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>>>();
            mockGetItemsBySearchQueryHandler = new Mock<IQueryHandler<GetItemsBySearchParameters, ItemsBySearchResultsModel>>();
            mockGetDefaultTaxClassMismatchesQuery = new Mock<IQueryHandler<GetDefaultTaxClassMismatchesParameters, List<DefaultTaxClassMismatchModel>>>();
            mockInfragisticsHelper = new Mock<IInfragisticsHelper>();
            mockItemExcelService = new Mock<IExcelService<ItemExcelModel>>();
            mockItemModelMapper = new Mock<IExcelModelMapper<ItemSearchModel, ItemExcelModel>>();

            mockGetCertificationAgenciesByTraitQueryHandler.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(new List<HierarchyClass>());
            mockGetBrandsQueryHandler.Setup(q => q.Search(It.IsAny<GetBrandsParameters>())).Returns(GetFakeBrandHierarchy());

            controller = new ExcelController(
                mockExcelExporterService.Object,
                mockGetHierarchyClassQuery.Object,
                mockBulkScanCodeSearchQueryHandler.Object,
                mockGetItemsBySearchQueryHandler.Object,
                mockGetDefaultTaxClassMismatchesQuery.Object,
                mockInfragisticsHelper.Object,
                mockItemExcelService.Object,
                mockItemModelMapper.Object);
        }

        [TestMethod]
        public void ItemExport_NoErrors_SpreadsheetShouldBeCreated()
        {
            // Given.
            response.Setup(r => r.OutputStream).Returns(outputStream.Object);
            context.Setup(c => c.HttpContext.Session).Returns(session.Object);
            context.Setup(c => c.HttpContext.Response).Returns(response.Object);

            var workbook = new Workbook();
            workbook.Worksheets.Add("Items");

            mockItemExcelService.Setup(m => m.Export(It.IsAny<ExportRequest<ItemExcelModel>>()))
                .Returns(new ExportResponse { ExcelWorkbook = workbook });

            controller.ControllerContext = context.Object;

            // When.
            controller.ItemExport(new List<string> { "22222222" });

            // Then.

            // Check that the http resonse was created.
            response.Verify(r => r.End());
        }

        [TestMethod]
        public void BulkItemExport_NoErrors_SpreadsheetShouldBeCreated()
            {
            // Given.
            response.Setup(r => r.OutputStream).Returns(outputStream.Object);
            session.SetupGet(s => s["consolidated_item_upload_errors"]).Returns(new List<ItemExcelModel> { new ItemExcelModel() });

            context.Setup(c => c.HttpContext.Session).Returns(session.Object);
            context.Setup(c => c.HttpContext.Response).Returns(response.Object);
            controller.ControllerContext = context.Object;

            var workbook = new Workbook();
            workbook.Worksheets.Add("Items");

            mockItemExcelService.Setup(m => m.Export(It.IsAny<ExportRequest<ItemExcelModel>>()))
                .Returns(new ExportResponse { ExcelWorkbook = workbook });

            // When.
            controller.BulkItemExport();

            // Then.

            // Check that the http resonse was created.
            response.Verify(r => r.End());
        }

        [TestMethod]
        public void IrmaItemExport_NoErrors_SpreadsheetShouldBeCreated()
        {
            // Given.
            response.Setup(r => r.OutputStream).Returns(outputStream.Object);
            context.Setup(c => c.HttpContext.Response).Returns(response.Object);

            mockGetHierarchyClassQuery.Setup(query => query.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(new HierarchyClass { hierarchyClassName = "Test Hierarchy Class" });
            mockGetHierarchyQuery.Setup(r => r.Search(It.IsAny<GetHierarchyParameters>())).Returns(TestHelpers.GetFakeHierarchyList());
            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());
            mockGetMerchTaxMappingsQueryHandler.Setup(r => r.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(new List<MerchTaxMappingModel>());

            var exporter = new IrmaItemExporter(
                mockGetHierarchyLineageQueryHandler.Object,
                mockGetMerchTaxMappingsQueryHandler.Object,
                mockGetCertificationAgenciesByTraitQueryHandler.Object,
                mockGetBrandsQueryHandler.Object);

            exporter.ExportModel = new ExcelExportModel(WorkbookFormat.Excel2007);

            mockExcelExporterService.Setup(moq => moq.GetIrmaItemExporter()).Returns(exporter);

            controller.ControllerContext = context.Object;

            // When.
            var testViewModel = new IrmaItemViewModel();

            controller.IrmaItemExport(new List<IrmaItemViewModel> { testViewModel });

            // Then.

            // Check that the http resonse was created.
            response.Verify(r => r.End());
        }

        [TestMethod]
        public void PluExport_NoErrors_SpreadsheetShouldBeCreated()
        {
            // Given.
            response.Setup(r => r.OutputStream).Returns(outputStream.Object);
            session.SetupGet(s => s["grid_search_results"]).Returns(new List<PluMappingViewModel>());

            context.Setup(c => c.HttpContext.Session).Returns(session.Object);
            context.Setup(c => c.HttpContext.Response).Returns(response.Object);

            var exporter = new PluExporter();
            exporter.ExportModel = new ExcelExportModel(WorkbookFormat.Excel2007);

            mockExcelExporterService.Setup(moq => moq.GetPluExporter()).Returns(exporter);

            controller.ControllerContext = context.Object;

            // When.
            controller.PluExport();

            // Then.

            // Check that the http resonse was created.
            response.Verify(r => r.End());
        }

        [TestMethod]
        public void BulkNewItemExport_NoErrors_SpreadsheetShouldBeCreated()
        {
            // Given.
            var testModel = new BulkImportNewItemModel
            {
                ScanCode = String.Empty,
                BrandName = String.Empty,
                BrandLineage = String.Empty,
                BrandId = String.Empty,
                ProductDescription = String.Empty,
                PosDescription = String.Empty,
                PackageUnit = String.Empty,
                FoodStampEligible = String.Empty,
                PosScaleTare = String.Empty,
                RetailSize = String.Empty,
                RetailUom = String.Empty,
                IrmaSubTeamName = String.Empty,
                MerchandiseLineage = String.Empty,
                MerchandiseId = String.Empty,
                TaxLineage = String.Empty,
                TaxId = String.Empty,
                BrowsingLineage = String.Empty,
                BrowsingId = String.Empty,
                NationalLineage = String.Empty,
                NationalId = String.Empty,
                IsValidated = String.Empty,
                Error = String.Empty
            };

            response.Setup(r => r.OutputStream).Returns(outputStream.Object);
            session.SetupGet(s => s["new_item_upload_errors"]).Returns(new List<BulkImportNewItemModel> { testModel });

            context.Setup(c => c.HttpContext.Session).Returns(session.Object);
            context.Setup(c => c.HttpContext.Response).Returns(response.Object);

            var exporter = new BulkNewItemExporter(
                mockGetHierarchyLineageQueryHandler.Object,
                mockGetMerchTaxMappingsQueryHandler.Object,
                mockGetCertificationAgenciesByTraitQueryHandler.Object,
                mockGetBrandsQueryHandler.Object);

            exporter.ExportModel = new ExcelExportModel(WorkbookFormat.Excel2007);

            mockGetHierarchyQuery.Setup(r => r.Search(It.IsAny<GetHierarchyParameters>())).Returns(TestHelpers.GetFakeHierarchyList());
            mockExcelExporterService.Setup(moq => moq.GetBulkNewItemExporter()).Returns(exporter);
            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());
            mockGetMerchTaxMappingsQueryHandler.Setup(r => r.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(new List<MerchTaxMappingModel>());

            controller.ControllerContext = context.Object;

            // When.
            controller.BulkNewItemExport();

            // Then.

            // Check that the http resonse was created.
            response.Verify(r => r.End());
        }

        [TestMethod]
        public void BulkPluExport_NoErrors_SpreadsheetShouldBeCreated()
        {
            // Given.
            response.Setup(r => r.OutputStream).Returns(outputStream.Object);
            session.SetupGet(s => s["grid_search_results"]).Returns(new List<BulkImportPluModel>());

            context.Setup(c => c.HttpContext.Session).Returns(session.Object);
            context.Setup(c => c.HttpContext.Response).Returns(response.Object);

            var exporter = new BulkPluExporter();
            exporter.ExportModel = new ExcelExportModel(WorkbookFormat.Excel2007);

            mockExcelExporterService.Setup(moq => moq.GetBulkPluExporter()).Returns(exporter);

            controller.ControllerContext = context.Object;
            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());

            // When.
            controller.BulkPluExport();

            // Then.

            // Check that the http resonse was created.
            response.Verify(r => r.End());
        }

        [TestMethod]
        public void BrandExport_JsonString_SpreadsheetShouldBeCreated()
        {
            // Given
            response.Setup(r => r.OutputStream).Returns(outputStream.Object);
            session.SetupGet(s => s["grid_search_results"]).Returns(new List<BrandViewModel>());

            context.Setup(c => c.HttpContext.Session).Returns(session.Object);
            context.Setup(c => c.HttpContext.Response).Returns(response.Object);

            var exporter = new BrandExporter();
            exporter.ExportModel = new ExcelExportModel(WorkbookFormat.Excel2007);
            mockExcelExporterService.Setup(moq => moq.GetBrandExporter()).Returns(exporter);

            controller.ControllerContext = context.Object;

            string json = "[{\"BrandName\":\"Test Json1|1\",\"BrandAbbreviation\":\"TJS1\"},{\"BrandName\":\"Test Json|2\",\"BrandAbbreviation\":\"TJS2\"}]";

            // When
            controller.BrandExport(json);

            // Then
            response.Verify(r => r.End());
        }

        [TestMethod]
        public void BulkCertificationAgencyExport_ItemsExistInSession_ShouldExportItems()
        {
            //Given
            context.Setup(c => c.HttpContext.Response).Returns(response.Object);
            context.Setup(c => c.HttpContext.Session).Returns(session.Object);
            controller.ControllerContext = context.Object;
            response.Setup(r => r.OutputStream).Returns(outputStream.Object);
            var agencies = new List<BulkImportCertificationAgencyModel>
                {
                    new BulkImportCertificationAgencyModel { CertificationAgencyNameAndId = "Test1|123" },
                    new BulkImportCertificationAgencyModel { CertificationAgencyNameAndId = "Test2|1234" },
                    new BulkImportCertificationAgencyModel { CertificationAgencyNameAndId = "Test3|12345" }
                };
            session.SetupGet(s => s["certification_agency_import_errors"]).Returns(agencies);

            var exporter = new CertificationAgencyExporter
            {
                ExportModel = new ExcelExportModel(WorkbookFormat.Excel2007)
            };
            mockExcelExporterService.Setup(m => m.GetCertificationAgencyExporter()).Returns(exporter);

            //When
            controller.BulkCertificationAgencyExport();

            //Then
            response.Verify(r => r.End());
            Assert.AreEqual(agencies, exporter.ExportData);
        }

        [TestMethod]
        public void ItemSearchExport_ItemsExist_ShouldExportItems()
        {
            //Given
            context.SetupGet(m => m.HttpContext.Request.QueryString).Returns(new NameValueCollection());
            response.Setup(r => r.OutputStream).Returns(outputStream.Object);
            context.Setup(c => c.HttpContext.Response).Returns(response.Object);
            mockGetItemsBySearchQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsBySearchParameters>()))
                .Returns(new ItemsBySearchResultsModel() { Items = new List<ItemSearchModel>() { new ItemSearchModel { ScanCode = "12345" } } });
            mockInfragisticsHelper.Setup(m => m.ParseSortParameterFromQueryString(It.IsAny<NameValueCollection>()))
                .Returns(new InfragisticsSortParameterPaseResult());

            Workbook workbook = new Workbook();
            workbook.Worksheets.Add("Items");
            mockItemExcelService.Setup(m => m.Export(It.IsAny<ExportRequest<ItemExcelModel>>()))
                .Returns(new ExportResponse { ExcelWorkbook = workbook });

            controller.ControllerContext = context.Object;

            //When
            controller.ItemSearchExport(new ItemSearchViewModel());

            //Then
            response.Verify(r => r.End());
            mockGetItemsBySearchQueryHandler.Verify(m => m.Search(It.IsAny<GetItemsBySearchParameters>()), Times.Once);
            mockInfragisticsHelper.Verify(m => m.ParseSortParameterFromQueryString(It.IsAny<NameValueCollection>()), Times.Once);
        }

        [TestMethod]
        public void CertificationAgencyTemplateExport()
        {
            //Given
            controller.ControllerContext = context.Object;
            context.Setup(c => c.HttpContext.Response).Returns(response.Object);
            response.Setup(r => r.OutputStream).Returns(outputStream.Object);
            mockExcelExporterService.Setup(m => m.GetCertificationAgencyExporter()).Returns(new CertificationAgencyExporter
            {
                ExportModel = new ExcelExportModel(WorkbookFormat.Excel2007)
            });

            //When
            controller.CertificationAgencyTemplateExport();

            //Then
            response.Verify(r => r.End());
        }

        private HierarchyClassListModel GetFakeHierarchy()
        {
            HierarchyClassListModel hierarchyListModal = new HierarchyClassListModel();
            HierarchyClassModel hierarchyModel = new HierarchyClassModel();

            hierarchyModel.HierarchyClassId = 2;
            hierarchyModel.HierarchyClassName = "Brand";
            hierarchyModel.HierarchyParentClassId = null;


            HierarchyClassModel hierarchyModelTax = new HierarchyClassModel();
            hierarchyModelTax.HierarchyClassId = 3;
            hierarchyModelTax.HierarchyClassName = "Tax";
            hierarchyModelTax.HierarchyParentClassId = null;


            HierarchyClassModel hierarchyModelMerch = new HierarchyClassModel();
            hierarchyModelMerch.HierarchyClassId = 4;
            hierarchyModelMerch.HierarchyClassName = "Merch";
            hierarchyModelMerch.HierarchyParentClassId = null;

            hierarchyListModal.BrandHierarchyList = new List<HierarchyClassModel> { hierarchyModel };
            hierarchyListModal.TaxHierarchyList = new List<HierarchyClassModel> { hierarchyModelTax };
            hierarchyListModal.MerchandiseHierarchyList = new List<HierarchyClassModel> { hierarchyModelMerch };


            HierarchyClassModel hierarchyModelBrowsing = new HierarchyClassModel();
            hierarchyModelBrowsing.HierarchyClassId = 5;
            hierarchyModelBrowsing.HierarchyClassName = "Browsing";
            hierarchyModelBrowsing.HierarchyParentClassId = null;

            HierarchyClassModel hierarchyModelNational = new HierarchyClassModel();
            hierarchyModelNational.HierarchyClassId = 6;
            hierarchyModelNational.HierarchyClassName = "National";
            hierarchyModelNational.HierarchyParentClassId = null;

            hierarchyListModal.BrandHierarchyList = new List<HierarchyClassModel> { hierarchyModel };
            hierarchyListModal.TaxHierarchyList = new List<HierarchyClassModel> { hierarchyModelTax };
            hierarchyListModal.MerchandiseHierarchyList = new List<HierarchyClassModel> { hierarchyModelMerch };
            hierarchyListModal.BrowsingHierarchyList = new List<HierarchyClassModel> { hierarchyModelBrowsing };
            hierarchyListModal.NationalHierarchyList = new List<HierarchyClassModel> { hierarchyModelNational };

            return hierarchyListModal;
        }

        private List<BrandModel> GetFakeBrandHierarchy()
        {
            List<BrandModel> hierarchyListModel = new List<BrandModel>();
            BrandModel hierarchyModel = new BrandModel();

            hierarchyModel.HierarchyClassId = 2;
            hierarchyModel.HierarchyClassName = "Brand";
            hierarchyModel.HierarchyParentClassId = null;
            hierarchyModel.BrandAbbreviation = "Br";

            BrandModel hierarchyModel2 = new BrandModel();

            hierarchyModel2.HierarchyClassId = 3;
            hierarchyModel2.HierarchyClassName = "Brand 2";
            hierarchyModel2.HierarchyParentClassId = null;

            hierarchyListModel.Add(hierarchyModel);
            hierarchyListModel.Add(hierarchyModel2);

            return hierarchyListModel;
        }
    }
}
