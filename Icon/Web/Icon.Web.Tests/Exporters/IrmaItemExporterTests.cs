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
using System.Drawing;

namespace Icon.Web.Tests.Unit.Exporters
{
    [TestClass]
    public class IrmaItemExporterTests
    {
        private IrmaItemExporter exporter;
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
        private HierarchyClassModel testNationalHierarchyClass;

        [TestInitialize]
        public void Initialize()
        {
            excelFormat = WorkbookFormat.Excel2007;
            exportModel = new ExcelExportModel(excelFormat);

            mockGetHierarchyLineageQueryHandler = new Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            mockGetMerchTaxMappingsQueryHandler = new Mock<IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>>>();
            mockGetCertificationAgenciesQueryHandler = new Mock<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>>();
            mockGetBrandsQueryHandler = new Mock<IQueryHandler<GetBrandsParameters, List<BrandModel>>>();

            exporter = new IrmaItemExporter(
                mockGetHierarchyLineageQueryHandler.Object,
                mockGetMerchTaxMappingsQueryHandler.Object,
                mockGetCertificationAgenciesQueryHandler.Object,
                mockGetBrandsQueryHandler.Object);

            exporter.ExportData = new List<IrmaItemViewModel>();
            exporter.ExportModel = exportModel;

            SetupMocksHierarchyCommandHandler();
        }

        [TestMethod]
        public void Export_ColumnHeaders_AllHeadersShouldAppearInTheCorrectOrder()
        {
            // Given.
            exporter.ExportData.Add(new IrmaItemViewModel());

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
            string testRegion = "SW";

            exporter.ExportData.Add(
                new IrmaItemViewModel
                {
                    Identifier = scanCode,
                    BrandName = testBrandHierarchyClass.HierarchyClassName,
                    BrandId = testBrandHierarchyClass.HierarchyClassId,
                    ItemDescription = productDescription,
                    PosDescription = posDesctiption,
                    PackageUnit = packageUnit,
                    FoodStamp = true,
                    PosScaleTare = posScaleTare,
                    RetailSize = retailSize,
                    RetailUom = retailUom,
                    DeliverySystem = deliverySystem,
                    IrmaSubTeamName = irmaSubteam,
                    MerchandiseHierarchyClassName = testMerchHierarchyClass.HierarchyClassName,
                    MerchandiseHierarchyClassId = testMerchHierarchyClass.HierarchyClassId,
                    TaxHierarchyClassName = testTaxHierarchyClass.HierarchyClassName,
                    TaxHierarchyClassId = testTaxHierarchyClass.HierarchyClassId,
                    NationalHierarchyClassName = testNationalHierarchyClass.HierarchyClassName,
                    NationalHierarchyClassId = testNationalHierarchyClass.HierarchyClassId,
                    Region = testRegion
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
            Assert.AreEqual(testNationalHierarchyClass.HierarchyClassName + "|" + testNationalHierarchyClass.HierarchyClassId, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex].Value);
            Assert.AreEqual(String.Empty, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex].Value);
            Assert.AreEqual(testRegion, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.RegionCodeColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.GlutenFreeColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.KosherColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.NonGmoColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.OrganicColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.VeganColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex].Value);
            Assert.IsNotNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex].Value);
            
        }

        [TestMethod]
        public void Export_IrmaItemListWithSignAttributes_ShouldBeConvertedToWorkbookObject()
        {
            // Given.

            // Given.
            var certificationAgencies = new Dictionary<int, string> { { 1, "Agency|1" } };

            var mockCertificationAgencyResults = new List<HierarchyClass>
            {
                new HierarchyClass { hierarchyClassID = 1, hierarchyClassName = "Agency" }
            };
            mockGetCertificationAgenciesQueryHandler.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(mockCertificationAgencyResults);

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
            string testRegion = "SW";
            int animalWelfareRatingId = AnimalWelfareRatings.NoStep;
            string biodynamic = "Y";
            int milkTypeId = MilkTypes.BuffaloMilk;
            string cheeseRaw = "Y";
            int ecoScaleRatingId = EcoScaleRatings.BaselineOrange;
            int glutenFreeAgencyId = 1;
            int kosherAgencyId = 1;
            int nonGmoAgencyId = 1;
            int organicAgencyId = 1;
            string premiumBodyCare = "Y";
            int seafoodFreshOrFrozenId = SeafoodFreshOrFrozenTypes.Fresh;
            int seafoodCatchTypeId = SeafoodCatchTypes.FarmRaised;
            int veganAgencyId = 1;
            string vegetarian = "Y";
            string wholeTrade = "Y";
            string grassFed = "Y";
            string pastureRaised = "Y";
            string freeRange = "Y";
            string dryAged = "Y";
            string airChilled = "Y";
            string madeInHouse = "Y";

            exporter.ExportData.Add(
                new IrmaItemViewModel
                {
                    Identifier = scanCode,
                    BrandName = testBrandHierarchyClass.HierarchyClassName,
                    BrandId = testBrandHierarchyClass.HierarchyClassId,
                    ItemDescription = productDescription,
                    PosDescription = posDesctiption,
                    PackageUnit = packageUnit,
                    FoodStamp = true,
                    PosScaleTare = posScaleTare,
                    RetailSize = retailSize,
                    RetailUom = retailUom,
                    DeliverySystem = deliverySystem,
                    IrmaSubTeamName = irmaSubteam,
                    MerchandiseHierarchyClassName = testMerchHierarchyClass.HierarchyClassName,
                    MerchandiseHierarchyClassId = testMerchHierarchyClass.HierarchyClassId,
                    TaxHierarchyClassName = testTaxHierarchyClass.HierarchyClassName,
                    TaxHierarchyClassId = testTaxHierarchyClass.HierarchyClassId,
                    NationalHierarchyClassName = testNationalHierarchyClass.HierarchyClassName,
                    NationalHierarchyClassId = testNationalHierarchyClass.HierarchyClassId,
                    Region = testRegion,
                    AnimalWelfareRatingId = animalWelfareRatingId,
                    Biodynamic = true,
                    CheeseMilkTypeId = milkTypeId,
                    CheeseRaw = true,
                    EcoScaleRatingId = ecoScaleRatingId,
                    GlutenFreeAgencyId = glutenFreeAgencyId,
                    KosherAgencyId = kosherAgencyId,
                    NonGmoAgencyId = nonGmoAgencyId,
                    OrganicAgencyId = organicAgencyId,
                    PremiumBodyCare = true,
                    SeafoodFreshOrFrozenId = seafoodFreshOrFrozenId,
                    SeafoodCatchTypeId = seafoodCatchTypeId,
                    VeganAgencyId = veganAgencyId,
                    Vegetarian = true,
                    WholeTrade = true,
                    GrassFed = true,
                    PastureRaised = true,
                    FreeRange = true,
                    DryAged = true,
                    AirChilled = true,
                    MadeInHouse = true
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
            Assert.AreEqual(testNationalHierarchyClass.HierarchyClassName + "|" + testNationalHierarchyClass.HierarchyClassId, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex].Value);
            Assert.AreEqual(String.Empty, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex].Value);
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex].Value);
            Assert.AreEqual(testRegion, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.RegionCodeColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.GetValueFromDictionary(AnimalWelfareRatings.AsDictionary, animalWelfareRatingId), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value);
            Assert.AreEqual(biodynamic, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.GetValueFromDictionary(MilkTypes.AsDictionary, milkTypeId), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value);
            Assert.AreEqual(cheeseRaw, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.GetValueFromDictionary(EcoScaleRatings.AsDictionary, ecoScaleRatingId), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.GetValueFromDictionary(certificationAgencies, glutenFreeAgencyId), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.GlutenFreeColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.GetValueFromDictionary(certificationAgencies, kosherAgencyId), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.KosherColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.GetValueFromDictionary(certificationAgencies, nonGmoAgencyId), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.NonGmoColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.GetValueFromDictionary(certificationAgencies, organicAgencyId), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.OrganicColumnIndex].Value);
            Assert.AreEqual(premiumBodyCare, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.GetValueFromDictionary(SeafoodFreshOrFrozenTypes.AsDictionary, seafoodFreshOrFrozenId), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.GetValueFromDictionary(SeafoodCatchTypes.AsDictionary, seafoodCatchTypeId), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value);
            Assert.AreEqual(ExcelHelper.GetValueFromDictionary(certificationAgencies, veganAgencyId), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.VeganColumnIndex].Value);
            Assert.AreEqual(vegetarian, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex].Value);
            Assert.AreEqual(wholeTrade, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex].Value);
            Assert.AreEqual(grassFed, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex].Value);
            Assert.AreEqual(pastureRaised, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex].Value);
            Assert.AreEqual(freeRange, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex].Value);
            Assert.AreEqual(dryAged, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex].Value);
            Assert.AreEqual(airChilled, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex].Value);
            Assert.AreEqual(madeInHouse, firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex].Value);
            

        }

        [TestMethod]
        public void Export_NewBrand_BrandValueShouldBeFormattedAsNewBrand()
        {
            // Given.
            exporter.ExportData.Add(new TestIrmaItemViewModelBuilder()
                .WithBrandName("New Brand"));

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Assert.AreEqual("New Brand|0", firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BrandColumnIndex].Value);
        }

        [TestMethod]
        public void Export_NoMerchandiseOrTaxValues_SpreadsheetValuesShouldBeEmptyString()
        {
            // Given.
            exporter.ExportData.Add(new TestIrmaItemViewModelBuilder()
                .WithMerchandiseClassName(String.Empty)
                .WithMerchandiseClassId(0)
                .WithTaxClassName(String.Empty)
                .WithTaxClassId(0));

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
            exporter.ExportData.Add(new TestIrmaItemViewModelBuilder());

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
            exporter.ExportData.Add(new TestIrmaItemViewModelBuilder());

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

        [TestMethod]
        public void Export_NewBrand_PosDescriptionCellShouldBeColorFormatted()
        {
            // Given.
            exporter.ExportData.Add(new TestIrmaItemViewModelBuilder()
                .WithBrandName("New Brand"));

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Assert.AreEqual(CellFill.CreateSolidFill(System.Drawing.Color.Red), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PosDescriptionColumnIndex].CellFormat.Fill);
        }

        [TestMethod]
        public void Export_OldBrand_MissingBrandAbbreviation_PosDescriptionCellShouldBeColorFormatted()
        {
            // Given.
            exporter.ExportData.Add(new TestIrmaItemViewModelBuilder()
                .WithBrandName("Brand 2"));

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Assert.AreEqual(CellFill.CreateSolidFill(System.Drawing.Color.Red), firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PosDescriptionColumnIndex].CellFormat.Fill);
        }

        [TestMethod]
        public void Export_OldBrand_WithBrandAbbreviation_PosDescriptionCellShouldNotBeColorFormatted()
        {
            // Given.
            exporter.ExportData.Add(new TestIrmaItemViewModelBuilder()
                .WithBrandName("Test Brand"));

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PosDescriptionColumnIndex].CellFormat.Fill);
        }


        [TestMethod]
        public void Export_EmptyBrand_PosDescriptionCellShouldBeColorFormatted()
        {
            // Given.
            exporter.ExportData.Add(new TestIrmaItemViewModelBuilder()
                .WithBrandName(""));

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];
            Assert.IsNull(firstWorksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PosDescriptionColumnIndex].CellFormat.Fill);
        }

        [TestMethod]
        public void Export_ItemsHaveExisting_ShouldExportBrandWithEsistingBrandsId()
        {
            // Given.
            var brandName = "Test Brand";
            exporter.ExportData.Add(new TestIrmaItemViewModelBuilder()
                .WithBrandName(brandName));

            // When.
            exporter.Export();

            // Then.
            Worksheet firstWorksheet = exportModel.ExcelWorkbook.Worksheets[0];
            var brandCell = firstWorksheet.Rows[1].Cells[1];
            Assert.AreEqual(brandName + "|2", brandCell.Value.ToString());
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

            testNationalHierarchyClass = new TestHierarchyClassModelBuilder()
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
                    testNationalHierarchyClass
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
            hierarchyModel.HierarchyLineage = "Test Brand";
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
