using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
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
    [TestClass] [Ignore]
    public class BulkNewItemExporterTests
    {
        private BulkNewItemExporter exporter;
        private Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>> mockGetHierarchyLineageQueryHandler;
        private Mock<IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>>> mockGetMerchTaxMappingsQueryHandler;
        private Mock<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>> mockGetCertificationAgenciesQueryHandler;
        private Mock<IQueryHandler<GetBrandsParameters, List<BrandModel>>> mockGetBrandsQueryHandler;
        private WorkbookFormat excelFormat;
        private ExcelExportModel exportModel;
        private HierarchyClassListModel testHierarchyClassList;
        private HierarchyClassModel testBrandHierarchyClass;
        private HierarchyClassModel testTaxHierarchyClass;
        private HierarchyClassModel testMerchHierarchyClass;
        private HierarchyClassModel testBrowsingHierarchyClass;
        private HierarchyClassModel testNationaHierarchyClass;

        [TestInitialize]
        public void Initialize()
        {
            excelFormat = WorkbookFormat.Excel2007;
            exportModel = new ExcelExportModel(excelFormat);
            mockGetHierarchyLineageQueryHandler = new Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            mockGetMerchTaxMappingsQueryHandler = new Mock<IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>>>();
            mockGetCertificationAgenciesQueryHandler = new Mock<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>>();
            mockGetBrandsQueryHandler = new Mock<IQueryHandler<GetBrandsParameters, List<BrandModel>>>();

            exporter = new BulkNewItemExporter(
                mockGetHierarchyLineageQueryHandler.Object, 
                mockGetMerchTaxMappingsQueryHandler.Object,
                mockGetCertificationAgenciesQueryHandler.Object,
                mockGetBrandsQueryHandler.Object);
            
            exporter.ExportData = new List<BulkImportNewItemModel>();
            exporter.ExportModel = exportModel;

            SetupMocksHierarchyCommandHandler();
        }

        [TestMethod]
        public void Export_ColumnHeaders_AllHeadersShouldAppearInTheCorrectOrder()
        {
            // Given.
            exporter.ExportData.Add(new TestBulkImportNewItemModelBuilder());

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
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.PremiumBodyCare, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.SeafoodFreshOrFrozen, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.SeafoodWildOrFarmRaised, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Vegetarian, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.WholeTrade, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.Error, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.ErrorColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.GrassFed, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.PastureRaised, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.FreeRange, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.DryAged, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.AirChilled, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.ExcelExportColumnNames.MadeInHouse, firstWorksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex].Value);
            
        }

        [TestMethod]
        public void Export_IrmaItemList_ShouldBeConvertedToWorkbookObject()
        {
            // Given.
            // This test is assuming that the hierarchies are displayed on-screen without the trailing |{hierarchyClassId}.  For that reason, we 
            // use "Test Brand" rather than "Test Brand|1234".
            string scanCode = "88776655";
            string productDescription = "Test Product Description";
            string posDesctiption = "Test POS Description";
            int packageUnit = 2;
            string foodStampEligible = "Y";
            decimal posScaleTare = 0m;
            decimal retailSize = 1m;
            string retailUom = "EACH";
            string deliverySystem = "CAP";
            string irmaSubteam = "Grocery";
            string isValidated = "Y";
            string regionCode = "SW";
            string animalWelfareRating = "No Step";
            string biodynamic = "Y";
            string milk = "Cow";
            string raw = "N";
            string eco = "Green";
            string glutenFree = "GF Agency|1";
            string kosher = "Kosher Agency|1";
            string nonGmo = "GMO Agency|1";
            string organic = "OG Agency|1";
            string premiumBodyCare = "N";
            string productionClaims = String.Empty;
            string freshOrFrozen = "Fresh";
            string wildOrFarm = "Wild";
            string vegan = "VG Agency|1";
            string vegetarian = "N";
            string wholeTrade = "Y";
            string error = "error";
            string grassFed = "Y";
            string pastureRaised = "Y";
            string freeRange = "Y";
            string dryAged = "Y";
            string airChilled = "Y";
            string madeInHouse = "Y";

            exporter.ExportData.Add(
                new BulkImportNewItemModel
                {
                    ScanCode = scanCode,
                    BrandName = testBrandHierarchyClass.HierarchyClassName,
                    BrandId = testBrandHierarchyClass.HierarchyClassId.ToString(),
                    ProductDescription = productDescription,
                    PosDescription = posDesctiption,
                    PackageUnit = packageUnit.ToString(),
                    FoodStampEligible = foodStampEligible,
                    PosScaleTare = posScaleTare.ToString(),
                    RetailSize = retailSize.ToString(),
                    RetailUom = retailUom,
                    DeliverySystem = deliverySystem,
                    IrmaSubTeamName = irmaSubteam,
                    MerchandiseLineage = testMerchHierarchyClass.HierarchyClassName,
                    MerchandiseId = testMerchHierarchyClass.HierarchyClassId.ToString(),
                    TaxLineage = testTaxHierarchyClass.HierarchyClassName,
                    TaxId = testTaxHierarchyClass.HierarchyClassId.ToString(),
                    NationalLineage = testNationaHierarchyClass.HierarchyClassName,
                    NationalId = testNationaHierarchyClass.HierarchyClassId.ToString(),
                    IsValidated = isValidated,
                    RegionCode = regionCode,
                    AnimalWelfareRating = animalWelfareRating,
                    Biodynamic = biodynamic,
                    CheeseAttributeMilkType = milk,
                    CheeseAttributeRaw = raw,
                    EcoScaleRating = eco,
                    GlutenFreeAgencyLineage = glutenFree,
                    KosherAgencyLineage = kosher,
                    NonGmoAgencyLineage = nonGmo,
                    OrganicAgencyLineage = organic,
                    PremiumBodyCare = premiumBodyCare,
                    SeafoodFreshOrFrozen = freshOrFrozen,
                    SeafoodWildOrFarmRaised = wildOrFarm,
                    VeganAgencyLineage = vegan,
                    Vegetarian = vegetarian,
                    WholeTrade = wholeTrade,
                    Error = error,
                    GrassFed = grassFed,
                    PastureRaised = pastureRaised,
                    FreeRange = freeRange,
                    DryAged = dryAged,
                    AirChilled = airChilled,
                    MadeInHouse = madeInHouse
                });

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.AreEqual(scanCode, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ScanCodeColumnIndex].Value);
            Assert.AreEqual(testBrandHierarchyClass.HierarchyClassName + "|" + testBrandHierarchyClass.HierarchyClassId, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BrandColumnIndex].Value);
            Assert.AreEqual(productDescription, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ProductDescriptionColumnIndex].Value);
            Assert.AreEqual(posDesctiption, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PosDescriptionColumnIndex].Value);
            Assert.AreEqual(packageUnit.ToString(), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PackageUnitColumnIndex].Value);
            Assert.AreEqual(foodStampEligible, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.FoodStampEligibleColumnIndex].Value);
            Assert.AreEqual(posScaleTare.ToString(), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PosScaleTareColumnIndex].Value);
            Assert.AreEqual(retailSize.ToString(), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SizeColumnIndex].Value);
            Assert.AreEqual(retailUom.ToString(), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.UomColumnIndex].Value);
            Assert.AreEqual(deliverySystem.ToString(), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.DeliverySystemColumnIndex].Value);
            Assert.AreEqual(irmaSubteam.ToString(), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.IrmaSubTeamColumnIndex].Value);
            Assert.AreEqual(testMerchHierarchyClass.HierarchyClassName + "|" + testMerchHierarchyClass.HierarchyClassId, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex].Value);
            Assert.AreEqual(testTaxHierarchyClass.HierarchyClassName + "|" + testTaxHierarchyClass.HierarchyClassId, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex].Value);
            Assert.AreEqual(testNationaHierarchyClass.HierarchyClassName + "|" + testNationaHierarchyClass.HierarchyClassId, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex].Value);
            Assert.AreEqual(String.Empty, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex].Value);
            Assert.AreEqual(isValidated, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex].Value);
            Assert.AreEqual(regionCode, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.RegionCodeColumnIndex].Value);
            Assert.AreEqual(animalWelfareRating, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value);
            Assert.AreEqual(biodynamic, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex].Value);
            Assert.AreEqual(milk, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value);
            Assert.AreEqual(raw, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex].Value);
            Assert.AreEqual(eco, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex].Value);
            Assert.AreEqual(premiumBodyCare, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex].Value);
            Assert.AreEqual(freshOrFrozen, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value);
            Assert.AreEqual(wildOrFarm, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value);
            Assert.AreEqual(vegetarian, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex].Value);
            Assert.AreEqual(wholeTrade, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex].Value);
            Assert.AreEqual(error, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ErrorColumnIndex].Value);
            Assert.AreEqual(grassFed, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex].Value);
            Assert.AreEqual(pastureRaised, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex].Value);
            Assert.AreEqual(freeRange, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex].Value);
            Assert.AreEqual(dryAged, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex].Value);
            Assert.AreEqual(airChilled, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex].Value);
            Assert.AreEqual(madeInHouse, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex].Value);
        }

        [TestMethod]
        public void Export_NoMerchandiseOrTaxValues_SpreadsheetValuesShouldBeEmptyString()
        {
            // Given.
            exporter.ExportData.Add(new TestBulkImportNewItemModelBuilder()
                .WithMerchandiseLineage(String.Empty)
                .WithMerchandiseId(String.Empty)
                .WithTaxLineage(String.Empty)
                .WithTaxId(String.Empty));

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.AreEqual(String.Empty, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex].Value);
            Assert.AreEqual(String.Empty, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex].Value);
        }

        [TestMethod]
        public void Export_HierarchyCellsExceptBrand_ShouldHaveValidationRules()
        {
            // Given.
            exporter.ExportData.Add(new TestBulkImportNewItemModelBuilder());

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];

            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BrandColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex].DataValidationRule);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex].DataValidationRule);
        }

        [TestMethod]
        public void Export_CustomValidationCells_DataValidationRulesShouldBeApplied()
        {
            // Given.
            exporter.ExportData.Add(new TestBulkImportNewItemModelBuilder());

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
        }

        private void SetupMocksHierarchyCommandHandler()
        {
            testBrandHierarchyClass = new TestHierarchyClassModelBuilder()
                            .WithHierarchyClassId(2)
                            .WithHierarchyClassName("Test Brand")
                            .WithHierarchyClassLineage("Test Brand");

            testTaxHierarchyClass = new TestHierarchyClassModelBuilder()
                            .WithHierarchyClassId(3)
                            .WithHierarchyClassName("Test Tax")
                            .WithHierarchyClassLineage("Test Tax");

            testMerchHierarchyClass = new TestHierarchyClassModelBuilder()
                            .WithHierarchyClassId(4)
                            .WithHierarchyClassName("Test Merch")
                            .WithHierarchyClassLineage("Test Merch");

            testBrowsingHierarchyClass = new TestHierarchyClassModelBuilder()
                            .WithHierarchyClassId(5)
                            .WithHierarchyClassName("Test Browsing")
                            .WithHierarchyClassLineage("Test Browsing");

            testNationaHierarchyClass = new TestHierarchyClassModelBuilder()
                            .WithHierarchyClassId(5)
                            .WithHierarchyClassName("Test National")
                            .WithHierarchyClassLineage("Test National");

            testHierarchyClassList = new HierarchyClassListModel()
            {
                BrandHierarchyList = new List<HierarchyClassModel>
                {
                    testBrandHierarchyClass
                },
                TaxHierarchyList = new List<HierarchyClassModel>
                {
                    testTaxHierarchyClass
                },
                BrowsingHierarchyList = new List<HierarchyClassModel>
                {
                    testBrowsingHierarchyClass
                },
                MerchandiseHierarchyList = new List<HierarchyClassModel>
                {
                    testMerchHierarchyClass
                },
                NationalHierarchyList = new List<HierarchyClassModel> 
                {
                    testNationaHierarchyClass
                }
            };

            mockGetHierarchyLineageQueryHandler.Setup(r => r.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(testHierarchyClassList);
            mockGetCertificationAgenciesQueryHandler.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(new List<HierarchyClass>());
            mockGetBrandsQueryHandler.Setup(q => q.Search(It.IsAny<GetBrandsParameters>())).Returns(GetFakeBrandHierarchy());
        }
        private List<BrandModel> GetFakeBrandHierarchy()
        {
            List<BrandModel> hierarchyListModal = new List<BrandModel>();
            BrandModel hierarchyModel = new BrandModel();

            hierarchyModel.HierarchyClassId = 2;
            hierarchyModel.HierarchyClassName = "Test Brand";
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
