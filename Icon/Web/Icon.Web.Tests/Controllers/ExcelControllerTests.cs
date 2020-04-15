using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
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

            mockGetCertificationAgenciesByTraitQueryHandler.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(new List<HierarchyClass>());
            mockGetBrandsQueryHandler.Setup(q => q.Search(It.IsAny<GetBrandsParameters>())).Returns(GetFakeBrandHierarchy());

            controller = new ExcelController(
                mockExcelExporterService.Object,
                mockGetHierarchyClassQuery.Object,
                mockBulkScanCodeSearchQueryHandler.Object,
                mockGetItemsBySearchQueryHandler.Object,
                mockGetDefaultTaxClassMismatchesQuery.Object);
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
