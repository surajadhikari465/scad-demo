using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass] [Ignore]
    public class NewItemTemplateExporterTests
    {
        private WorkbookFormat excelFormat;
        private ExcelExportModel exportModel;
        private Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>> mockGetHierarchyQuery;
        private Mock<IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>>> mockGetMerchTaxMappingsQueryHandler;
        private Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>> mockGetHierarchyLineageQueryHandler;
        private Mock<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>> mockGetCertificationAgenciesQueryHandler;
        private Mock<IQueryHandler<GetBrandsParameters, List<BrandModel>>> mockGetBrandsQueryHandler;

        [TestInitialize]
        public void Initialize()
        {
            excelFormat = WorkbookFormat.Excel2007;
            exportModel = new ExcelExportModel(excelFormat);
            mockGetHierarchyQuery = new Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>>();
            mockGetMerchTaxMappingsQueryHandler = new Mock<IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>>>();
            mockGetHierarchyLineageQueryHandler = new Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            mockGetCertificationAgenciesQueryHandler = new Mock<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>>();
            mockGetBrandsQueryHandler = new Mock<IQueryHandler<GetBrandsParameters, List<BrandModel>>>();
            mockGetBrandsQueryHandler.Setup(q => q.Search(It.IsAny<GetBrandsParameters>())).Returns(GetFakeBrandHierarchy());
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
            List<BrandModel> hierarchyListModal = new List<BrandModel>();
            BrandModel hierarchyModel = new BrandModel();

            hierarchyModel.HierarchyClassId = 2;
            hierarchyModel.HierarchyClassName = "Brand";
            hierarchyModel.HierarchyParentClassId = null;
            hierarchyModel.BrandAbbreviation = "Br";

            BrandModel hierarchyModel2 = new BrandModel();

            hierarchyModel2.HierarchyClassId = 3;
            hierarchyModel2.HierarchyClassName = "Brand 2";
            hierarchyModel2.HierarchyParentClassId = null;

            hierarchyListModal.Add(hierarchyModel);
            hierarchyListModal.Add(hierarchyModel2);
            return hierarchyListModal;
        }
    }
}