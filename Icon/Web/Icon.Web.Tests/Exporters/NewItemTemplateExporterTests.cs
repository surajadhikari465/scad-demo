using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
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
    [TestClass]
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

        [TestMethod]
        public void Export_ColumnHeaders_AllHeadersShouldAppearInTheCorrectOrder()
        {
            // Given.
         

            List<Hierarchy> fakeHierarchyList = new List<Hierarchy>
            {
                TestHelpers.GetFakeHierarchy()
            };

            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(fakeHierarchyList);
            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());
            mockGetMerchTaxMappingsQueryHandler.Setup(r => r.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(new List<MerchTaxMappingModel>());
            mockGetCertificationAgenciesQueryHandler.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(new List<HierarchyClass>());

            var exporter = new NewItemTemplateExporter(
                mockGetHierarchyLineageQueryHandler.Object,
                mockGetMerchTaxMappingsQueryHandler.Object,
                mockGetCertificationAgenciesQueryHandler.Object,
                mockGetBrandsQueryHandler.Object);

            exporter.ExportModel = exportModel;

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.ScanCode, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.ScanCodeColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Brand, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.BrandColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.ProductDescription, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.ProductDescriptionColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.PosDescription, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.PosDescriptionColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.PackageUnit, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.PackageUnitColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.FoodStampEligible, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.FoodStampEligibleColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.PosScaleTare, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.PosScaleTareColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Size, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.SizeColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Uom, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.UomColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.DeliverySystem, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.DeliverySystemColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.IrmaSubTeam, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.IrmaSubTeamColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Merchandise, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.NationalClass, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Tax, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Browsing, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Validated, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Region, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.RegionCodeColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.AnimalWelfareRating, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Biodynamic, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.CheeseAttributeMilkType, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.CheeseAttributeRaw, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.EcoScaleRating, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.GlutenFree, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.GlutenFreeColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Kosher, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.KosherColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.NonGmo, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.NonGmoColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Organic, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.OrganicColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.PremiumBodyCare, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.SeafoodFreshOrFrozen, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.SeafoodWildOrFarmRaised, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Vegan, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.VeganColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Vegetarian, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.WholeTrade, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.GrassFed, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.PastureRaised, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.FreeRange, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.DryAged, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.AirChilled, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.MadeInHouse, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex].Value);
            
            Assert.IsNull(firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.ErrorColumnIndex].Value);
        }

        [TestMethod]
        public void Export_CellValuesInSpreadsheet_AllCellsShouldBeBlank()
        {
            // Given.
           
            List<Hierarchy> fakeHierarchyList = new List<Hierarchy>
            {
                TestHelpers.GetFakeHierarchy()
            };

            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(fakeHierarchyList);
            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());
            mockGetMerchTaxMappingsQueryHandler.Setup(r => r.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(new List<MerchTaxMappingModel>());
            mockGetCertificationAgenciesQueryHandler.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(new List<HierarchyClass>());

            var exporter = new NewItemTemplateExporter(
                mockGetHierarchyLineageQueryHandler.Object,
                mockGetMerchTaxMappingsQueryHandler.Object,
                mockGetCertificationAgenciesQueryHandler.Object,
                mockGetBrandsQueryHandler.Object);

            exporter.ExportModel = exportModel;

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ScanCodeColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BrandColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ProductDescriptionColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PosDescriptionColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PackageUnitColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.FoodStampEligibleColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PosScaleTareColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SizeColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.UomColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.DeliverySystemColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.IrmaSubTeamColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.RegionCodeColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.GlutenFreeColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.KosherColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.NonGmoColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.OrganicColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.VeganColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ScanCodeColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ScanCodeColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.GrassFedColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PastureRaisedColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.FreeRangeColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DryAgedColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AirChilledColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MadeInHouseColumnIndex].Value);
        }

        [TestMethod]
        public void Export_PreGeneratedWorksheets_AllWorksheetsShouldBeGenerated()
        {
            // Given.
           
            List<Hierarchy> fakeHierarchyList = new List<Hierarchy>
            {
                TestHelpers.GetFakeHierarchy()
            };

            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(fakeHierarchyList);
            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());
            mockGetMerchTaxMappingsQueryHandler.Setup(r => r.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(new List<MerchTaxMappingModel>());
            mockGetCertificationAgenciesQueryHandler.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(new List<HierarchyClass>());

            var exporter = new NewItemTemplateExporter(
                mockGetHierarchyLineageQueryHandler.Object,
                mockGetMerchTaxMappingsQueryHandler.Object,
                mockGetCertificationAgenciesQueryHandler.Object,
                mockGetBrandsQueryHandler.Object);

            exporter.ExportModel = exportModel;

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Worksheet brandWorksheet = exportModel.ExcelWorkbook.Worksheets[1];
            Worksheet merchandiseWorksheet = exportModel.ExcelWorkbook.Worksheets[2];
            Worksheet taxWorksheet = exportModel.ExcelWorkbook.Worksheets[3];
            Worksheet browsingWorksheet = exportModel.ExcelWorkbook.Worksheets[4];

            Assert.AreEqual(HierarchyNames.Brands, brandWorksheet.Name);
            Assert.AreEqual(HierarchyNames.Merchandise, merchandiseWorksheet.Name);
            Assert.AreEqual(HierarchyNames.Tax, taxWorksheet.Name);
            Assert.AreEqual(HierarchyNames.Browsing, browsingWorksheet.Name);
        }

        [TestMethod]
        public void Export_HierarchyCellsExceptBrand_ShouldHaveValidationRules()
        {
            // Brand should not have validation rules since the users need the ability to add new brands.

            // Given.
            List<Hierarchy> fakeHierarchyList = new List<Hierarchy>
            {
                TestHelpers.GetFakeHierarchy()
            };

            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(fakeHierarchyList);
            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());
            mockGetMerchTaxMappingsQueryHandler.Setup(r => r.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(new List<MerchTaxMappingModel>());
            mockGetCertificationAgenciesQueryHandler.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(new List<HierarchyClass>());

            var exporter = new NewItemTemplateExporter(
                mockGetHierarchyLineageQueryHandler.Object,
                mockGetMerchTaxMappingsQueryHandler.Object,
                mockGetCertificationAgenciesQueryHandler.Object,
                mockGetBrandsQueryHandler.Object);

            exporter.ExportModel = exportModel;

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.IsNull(firstWorksheet.Rows[1].Cells[1].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex].DataValidationRule);
        }

        [TestMethod]
        public void Export_CustomValidationCells_DataValidationRulesShouldBeApplied()
        {
            // Given.
            List<Hierarchy> fakeHierarchyList = new List<Hierarchy>
            {
                TestHelpers.GetFakeHierarchy()
            };

            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(fakeHierarchyList);
            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(GetFakeHierarchy());
            mockGetMerchTaxMappingsQueryHandler.Setup(r => r.Search(It.IsAny<GetMerchTaxMappingsParameters>())).Returns(new List<MerchTaxMappingModel>());
            mockGetCertificationAgenciesQueryHandler.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(new List<HierarchyClass>());

            var exporter = new NewItemTemplateExporter(
                mockGetHierarchyLineageQueryHandler.Object,
                mockGetMerchTaxMappingsQueryHandler.Object,
                mockGetCertificationAgenciesQueryHandler.Object,
                mockGetBrandsQueryHandler.Object);

            exporter.ExportModel = exportModel;

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.FoodStampEligibleColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex].DataValidationRule);
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
