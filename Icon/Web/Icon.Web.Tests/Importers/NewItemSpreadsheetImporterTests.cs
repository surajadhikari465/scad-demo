using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Importers;
using Icon.Web.Tests.Common.Builders;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Tests.Unit.Importers
{
    [TestClass] [Ignore]
    public class NewItemSpreadsheetImporterTests
    {
        private NewItemSpreadsheetImporter importer;
        private Workbook workbook;
        private Worksheet worksheet;
        private Mock<IQueryHandler<GetExistingScanCodeUploadsParameters, List<ScanCodeModel>>> mockGetExistingScanCodeUploadsQuery;
        private Mock<IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>>> mockGetTaxHierarchyClassesWithNoAbbreviationQuery;
        private Mock<IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>>> mockGetAffinitySubBricksQuery;
        private Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>> mockGetHierarchyLineageQuery;
        private Mock<IQueryHandler<GetDuplicateBrandsByTrimmedNameParameters, List<string>>> mockGetDuplicateBrandsByTrimmedNameQuery;
        private Mock<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>> mockGetCertificationAgenciesQuery;
        private List<BulkImportNewItemModel> importItems;
        private List<ScanCodeModel> existingScanCodes;
        private List<string> existingHierarchyClasses;
        private List<HierarchyClass> taxHierarchyClassesWithNoAbbreviations;
        private List<HierarchyClass> affinitySubBricks;
        private HierarchyClassListModel hierarchyClassListModel;

        [TestInitialize]
        public void Initialize()
        {
            workbook = new Infragistics.Documents.Excel.Workbook();
            worksheet = workbook.Worksheets.Add("Sheet1");

            worksheet.Rows[0].Cells[0].Value = "Header Placeholder";

            mockGetExistingScanCodeUploadsQuery = new Mock<IQueryHandler<GetExistingScanCodeUploadsParameters, List<ScanCodeModel>>>();
            mockGetDuplicateBrandsByTrimmedNameQuery = new Mock<IQueryHandler<GetDuplicateBrandsByTrimmedNameParameters, List<string>>>();
            mockGetTaxHierarchyClassesWithNoAbbreviationQuery = new Mock<IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>>>();
            mockGetAffinitySubBricksQuery = new Mock<IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>>>();
            mockGetHierarchyLineageQuery = new Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            mockGetCertificationAgenciesQuery = new Mock<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>>();

            existingScanCodes = new List<ScanCodeModel>();
            mockGetExistingScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>())).Returns(existingScanCodes);

            existingHierarchyClasses = new List<string>();
            mockGetDuplicateBrandsByTrimmedNameQuery.Setup(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>())).Returns(existingHierarchyClasses);

            taxHierarchyClassesWithNoAbbreviations = new List<HierarchyClass>();
            mockGetTaxHierarchyClassesWithNoAbbreviationQuery.Setup(m => m.Search(It.IsAny<GetTaxHierarchyClassesWithNoAbbreviationParameters>())).Returns(taxHierarchyClassesWithNoAbbreviations);

            affinitySubBricks = new List<HierarchyClass>();
            mockGetAffinitySubBricksQuery.Setup(m => m.Search(It.IsAny<GetAffinitySubBricksParameters>())).Returns(affinitySubBricks);

            hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                BrowsingHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            mockGetHierarchyLineageQuery.Setup(m => m.Search(It.IsAny<GetHierarchyLineageParameters>())).Returns(hierarchyClassListModel);

            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(new List<HierarchyClass>());

            importer = new NewItemSpreadsheetImporter(mockGetExistingScanCodeUploadsQuery.Object,
                mockGetTaxHierarchyClassesWithNoAbbreviationQuery.Object,
                mockGetAffinitySubBricksQuery.Object,
                mockGetHierarchyLineageQuery.Object,
                mockGetDuplicateBrandsByTrimmedNameQuery.Object,
                mockGetCertificationAgenciesQuery.Object);

            importer.Workbook = workbook;

            importItems = new List<BulkImportNewItemModel>();
        }

        [TestMethod]
        public void IsValidSpreadsheetType_ValidIrmaItemSpreadsheet_ShouldValidate()
        {
            // Given.
            ApplyIrmaItemHeaders();

            importer.Workbook = workbook;

            // When.
            bool result = importer.IsValidSpreadsheetType();

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidSpreadsheetType_ValidConsolidatedItemSpreadsheet_ShouldValidate()
        {
            // Given.
            ApplyConsolidatedItemHeaders();

            importer.Workbook = workbook;

            // When.
            bool result = importer.IsValidSpreadsheetType();

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidSpreadsheetType_ConsolidatedItemSpreadsheetWithWrongColumnName_ValidationFails()
        {
            // Given.
            ApplyIrmaItemHeaders();

            // Incorrect column name.
            worksheet.Rows[0].Cells[0].Value = "National PLU";

            importer.Workbook = workbook;

            // When.
            bool result = importer.IsValidSpreadsheetType();

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidSpreadsheetType_IrmaItemSpreadsheetWithWrongColumnName_ValidationFails()
        {
            // Given.
            ApplyConsolidatedItemHeaders();

            worksheet.Rows[0].Cells[1].Value = "BrandName";

            importer.Workbook = workbook;

            // When.
            bool result = importer.IsValidSpreadsheetType();

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_IrmaItemSpreadsheetType_AllDataShouldConvertToModelClass()
        {
            // Given.
            ApplyIrmaItemHeaders();

            string scanCode = "1234567890";
            string brand = "Brand|1234";
            string productDescription = "Test Description";
            string posDescription = "Test POS";
            string packageUnit = "EA";
            string foodStampEligible = "n";
            string posScaleTare = "0";
            string retailSize = "1";
            string retailUom = "EA";
            string deliverySystem = "CAP";
            string irmaSubTeam = "Subteam";
            string merchandise = "Merch|Merch|Merch|1234";
            string tax = "Tax|1234";
            string alcoholByVolume = "23.66";
            string national = "Class1|111";
            string browsing = "Browsing|1234";
            string isValidated = "y";
            string regionCode = "SW";
            string animalWelfareRating = String.Empty;
            string biodynamic = "Y";
            string cheeseAttributeMilkType = "Cow";
            string cheeseAttributeRaw = "N";
            string ecoScaleRating = String.Empty;
            string glutenFree = "GF Agency|1";
            string kosher = "Kosher Agency|1";
            string nonGmo = "GMO Agency|1";
            string organic = "OG Agency|1";
            string premiumBodyCare = "N";
            string seafoodFreshOrFrozen = String.Empty;
            string seafoodWildOrFarmRaised = "Wild";
            string vegan = "VG Agency|1";
            string vegetarian = "N";
            string wholeTrade = "Y";
            string grassFed = "Y";
            string pastureRaised = "Y";
            string freeRange = "Y";
            string dryAged = "Y";
            string airChilled = "Y";
            string madeInHouse = "Y";
            string msc = "y";

            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ScanCodeColumnIndex].Value = scanCode;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BrandColumnIndex].Value = brand;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ProductDescriptionColumnIndex].Value = productDescription;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PosDescriptionColumnIndex].Value = posDescription;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PackageUnitColumnIndex].Value = packageUnit;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.FoodStampEligibleColumnIndex].Value = foodStampEligible;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PosScaleTareColumnIndex].Value = posScaleTare;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SizeColumnIndex].Value = retailSize;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.UomColumnIndex].Value = retailUom;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.DeliverySystemColumnIndex].Value = deliverySystem;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.IrmaSubTeamColumnIndex].Value = irmaSubTeam;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex].Value = merchandise;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex].Value = tax;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.AlcoholByVolumeIndex].Value = alcoholByVolume;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex].Value = national;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex].Value = browsing;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex].Value = isValidated;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.RegionCodeColumnIndex].Value = regionCode;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value = animalWelfareRating;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex].Value = biodynamic;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value = cheeseAttributeMilkType;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex].Value = cheeseAttributeRaw;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex].Value = ecoScaleRating;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex].Value = premiumBodyCare;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value = seafoodFreshOrFrozen;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value = seafoodWildOrFarmRaised;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex].Value = vegetarian;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex].Value = wholeTrade;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex].Value = grassFed;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex].Value = pastureRaised;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex].Value = freeRange;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex].Value = dryAged;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex].Value = airChilled;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex].Value = madeInHouse;
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.MscColumnIndex].Value = msc;
            

            importer.Workbook = workbook;

            // When.
            importer.IsValidSpreadsheetType();
            importer.ConvertSpreadsheetToModel();

            // Then.
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(scanCode, importer.ParsedRows[0].ScanCode);
            Assert.AreEqual("Brand", importer.ParsedRows[0].BrandName);
            Assert.AreEqual("Brand|1234", importer.ParsedRows[0].BrandLineage);
            Assert.AreEqual("1234", importer.ParsedRows[0].BrandId);
            Assert.AreEqual(productDescription, importer.ParsedRows[0].ProductDescription);
            Assert.AreEqual(posDescription, importer.ParsedRows[0].PosDescription);
            Assert.AreEqual(packageUnit, importer.ParsedRows[0].PackageUnit);
            Assert.AreEqual(foodStampEligible.GetBoolStringFromCellText(), importer.ParsedRows[0].FoodStampEligible);
            Assert.AreEqual(posScaleTare, importer.ParsedRows[0].PosScaleTare);
            Assert.AreEqual(retailSize, importer.ParsedRows[0].RetailSize);
            Assert.AreEqual(retailUom, importer.ParsedRows[0].RetailUom);
            Assert.AreEqual(deliverySystem, importer.ParsedRows[0].DeliverySystem);
            Assert.AreEqual(irmaSubTeam, importer.ParsedRows[0].IrmaSubTeamName);
            Assert.AreEqual("Merch|Merch|Merch|1234", importer.ParsedRows[0].MerchandiseLineage);
            Assert.AreEqual("1234", importer.ParsedRows[0].MerchandiseId);
            Assert.AreEqual("Tax|1234", importer.ParsedRows[0].TaxLineage);
            Assert.AreEqual(alcoholByVolume, importer.ParsedRows[0].AlcoholByVolume);
            Assert.AreEqual("1234", importer.ParsedRows[0].TaxId);
            Assert.AreEqual(national, importer.ParsedRows[0].NationalLineage);
            Assert.AreEqual("111", importer.ParsedRows[0].NationalId);
            Assert.AreEqual("Browsing|1234", importer.ParsedRows[0].BrowsingLineage);
            Assert.AreEqual("1234", importer.ParsedRows[0].BrowsingId);
            Assert.AreEqual(isValidated.GetBoolStringFromCellText(), importer.ParsedRows[0].IsValidated);
            Assert.AreEqual(regionCode, importer.ParsedRows[0].RegionCode);
            Assert.AreEqual(String.Empty, importer.ParsedRows[0].AnimalWelfareRating);
            Assert.AreEqual(biodynamic.GetBoolStringFromCellText(), importer.ParsedRows[0].Biodynamic);
            Assert.AreEqual(cheeseAttributeMilkType, importer.ParsedRows[0].CheeseAttributeMilkType);
            Assert.AreEqual(cheeseAttributeRaw.GetBoolStringFromCellText(), importer.ParsedRows[0].CheeseAttributeRaw);
            Assert.AreEqual(ecoScaleRating, importer.ParsedRows[0].EcoScaleRating);
            Assert.AreEqual(glutenFree, importer.ParsedRows[0].GlutenFreeAgencyLineage);
            Assert.AreEqual(kosher, importer.ParsedRows[0].KosherAgencyLineage);
            Assert.AreEqual(nonGmo, importer.ParsedRows[0].NonGmoAgencyLineage);
            Assert.AreEqual(organic, importer.ParsedRows[0].OrganicAgencyLineage);
            Assert.AreEqual(premiumBodyCare.GetBoolStringFromCellText(), importer.ParsedRows[0].PremiumBodyCare);
            Assert.AreEqual(seafoodFreshOrFrozen, importer.ParsedRows[0].SeafoodFreshOrFrozen);
            Assert.AreEqual(seafoodWildOrFarmRaised, importer.ParsedRows[0].SeafoodWildOrFarmRaised);
            Assert.AreEqual(vegan, importer.ParsedRows[0].VeganAgencyLineage);
            Assert.AreEqual(vegetarian.GetBoolStringFromCellText(), importer.ParsedRows[0].Vegetarian);
            Assert.AreEqual(wholeTrade.GetBoolStringFromCellText(), importer.ParsedRows[0].WholeTrade);
            Assert.AreEqual(grassFed.GetBoolStringFromCellText(), importer.ParsedRows[0].GrassFed);
            Assert.AreEqual(pastureRaised.GetBoolStringFromCellText(), importer.ParsedRows[0].PastureRaised);
            Assert.AreEqual(freeRange.GetBoolStringFromCellText(), importer.ParsedRows[0].FreeRange);
            Assert.AreEqual(dryAged.GetBoolStringFromCellText(), importer.ParsedRows[0].DryAged);
            Assert.AreEqual(airChilled.GetBoolStringFromCellText(), importer.ParsedRows[0].AirChilled);
            Assert.AreEqual(madeInHouse.GetBoolStringFromCellText(), importer.ParsedRows[0].MadeInHouse);
            Assert.AreEqual(msc.GetBoolStringFromCellText(), importer.ParsedRows[0].Msc);
            Assert.AreEqual(String.Empty, importer.ParsedRows[0].Error);
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_ConsolidatedItemSpreadsheetType_AllDataShouldConvertToModelClass()
        {
            // Given.
            ApplyConsolidatedItemHeaders();

            string scanCode = "1234567890";
            string brand = "Brand|1234";
            string productDescription = "Test Description";
            string posDescription = "Test POS";
            string packageUnit = "EA";
            string foodStampEligible = "n";
            string posScaleTare = "0";
            string retailSize = "1";
            string retailUom = "EA";
            string deliverySystem = "CAP";
            string merchandise = "Merch|Merch|Merch|1234";
            string tax = "Tax|1234";
            string alcoholByVolume = "15.66";
            string national = "Class1|111";
            string browsing = "Browsing|1234";
            string isValidated = "y";
            string departmentSale = "N";
            string hiddenItem = String.Empty;
            string notes = "test note";
            string animalWelfareRating = String.Empty;
            string biodynamic = "Y";
            string cheeseAttributeMilkType = "Cow";
            string cheeseAttributeRaw = "N";
            string ecoScaleRating = String.Empty;
            string glutenFree = "GF Agency|1";
            string kosher = "Kosher Agency|1";
            string nonGmo = "GMO Agency|1";
            string organic = "OG Agency|1";
            string premiumBodyCare = "N";
            string seafoodFreshOrFrozen = String.Empty;
            string seafoodWildOrFarmRaised = "Wild";
            string vegan = "VG Agency|1";
            string vegetarian = "N";
            string wholeTrade = "Y";
            string grassFed = "Y";
            string pastureRaised = "Y";
            string freeRange = "Y";
            string dryAged = "Y";
            string airChilled = "Y";
            string madeInHouse = "Y";
            string msc = "Y";

            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ScanCodeColumnIndex].Value = scanCode;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BrandColumnIndex].Value = brand;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ProductDescriptionColumnIndex].Value = productDescription;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PosDescriptionColumnIndex].Value = posDescription;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PackageUnitColumnIndex].Value = packageUnit;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.FoodStampEligibleColumnIndex].Value = foodStampEligible;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PosScaleTareColumnIndex].Value = posScaleTare;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SizeColumnIndex].Value = retailSize;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.UomColumnIndex].Value = retailUom;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DeliverySystemColumnIndex].Value = deliverySystem;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MerchandiseColumnIndex].Value = merchandise;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.TaxColumnIndex].Value = tax;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AlcoholByVolumeColumnIndex].Value = alcoholByVolume;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.NationalColumnIndex].Value = national;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BrowsingColumnIndex].Value = browsing;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ValidatedColumnIndex].Value = isValidated;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DepartmentSaleColumnIndex].Value = departmentSale;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.HiddenItemColumnIndex].Value = hiddenItem;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.NotesColumnIndex].Value = notes;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value = animalWelfareRating;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BiodynamicColumnIndex].Value = biodynamic;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value = cheeseAttributeMilkType;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeRawColumnIndex].Value = cheeseAttributeRaw;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.EcoScaleRatingColumnIndex].Value = ecoScaleRating;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PremiumBodyCareColumnIndex].Value = premiumBodyCare;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value = seafoodFreshOrFrozen;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value = seafoodWildOrFarmRaised;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.VegetarianColumnIndex].Value = vegetarian;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.WholeTradeColumnIndex].Value = wholeTrade;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.GrassFedColumnIndex].Value = grassFed;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PastureRaisedColumnIndex].Value = pastureRaised;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.FreeRangeColumnIndex].Value = freeRange;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DryAgedColumnIndex].Value = dryAged;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AirChilledColumnIndex].Value = airChilled;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MadeInHouseColumnIndex].Value = madeInHouse;
            worksheet.Rows[1].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MscColumnIndex].Value = msc;

            importer.Workbook = workbook;

            // When.
            importer.IsValidSpreadsheetType();
            importer.ConvertSpreadsheetToModel();

            // Then.
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(scanCode, importer.ParsedRows[0].ScanCode);
            Assert.AreEqual("Brand", importer.ParsedRows[0].BrandName);
            Assert.AreEqual("Brand|1234", importer.ParsedRows[0].BrandLineage);
            Assert.AreEqual("1234", importer.ParsedRows[0].BrandId);
            Assert.AreEqual(productDescription, importer.ParsedRows[0].ProductDescription);
            Assert.AreEqual(posDescription, importer.ParsedRows[0].PosDescription);
            Assert.AreEqual(packageUnit, importer.ParsedRows[0].PackageUnit);
            Assert.AreEqual(foodStampEligible.GetBoolStringFromCellText(), importer.ParsedRows[0].FoodStampEligible);
            Assert.AreEqual(posScaleTare, importer.ParsedRows[0].PosScaleTare);
            Assert.AreEqual(retailSize, importer.ParsedRows[0].RetailSize);
            Assert.AreEqual(retailUom, importer.ParsedRows[0].RetailUom);
            Assert.AreEqual(deliverySystem, importer.ParsedRows[0].DeliverySystem);
            Assert.AreEqual(String.Empty, importer.ParsedRows[0].IrmaSubTeamName);
            Assert.AreEqual("Merch|Merch|Merch|1234", importer.ParsedRows[0].MerchandiseLineage);
            Assert.AreEqual("1234", importer.ParsedRows[0].MerchandiseId);
            Assert.AreEqual("Tax|1234", importer.ParsedRows[0].TaxLineage);
            Assert.AreEqual(alcoholByVolume, importer.ParsedRows[0].AlcoholByVolume);
            Assert.AreEqual("1234", importer.ParsedRows[0].TaxId);
            Assert.AreEqual(national, importer.ParsedRows[0].NationalLineage);
            Assert.AreEqual("111", importer.ParsedRows[0].NationalId);
            Assert.AreEqual("Browsing|1234", importer.ParsedRows[0].BrowsingLineage);
            Assert.AreEqual("1234", importer.ParsedRows[0].BrowsingId);
            Assert.AreEqual(isValidated.GetBoolStringFromCellText(), importer.ParsedRows[0].IsValidated);
            Assert.AreEqual(String.Empty, importer.ParsedRows[0].RegionCode);
            Assert.AreEqual(String.Empty, importer.ParsedRows[0].AnimalWelfareRating);
            Assert.AreEqual(biodynamic.GetBoolStringFromCellText(), importer.ParsedRows[0].Biodynamic);
            Assert.AreEqual(cheeseAttributeMilkType, importer.ParsedRows[0].CheeseAttributeMilkType);
            Assert.AreEqual(cheeseAttributeRaw.GetBoolStringFromCellText(), importer.ParsedRows[0].CheeseAttributeRaw);
            Assert.AreEqual(ecoScaleRating, importer.ParsedRows[0].EcoScaleRating);
            Assert.AreEqual(glutenFree, importer.ParsedRows[0].GlutenFreeAgencyLineage);
            Assert.AreEqual(kosher, importer.ParsedRows[0].KosherAgencyLineage);
            Assert.AreEqual(nonGmo, importer.ParsedRows[0].NonGmoAgencyLineage);
            Assert.AreEqual(organic, importer.ParsedRows[0].OrganicAgencyLineage);
            Assert.AreEqual(premiumBodyCare.GetBoolStringFromCellText(), importer.ParsedRows[0].PremiumBodyCare);
            Assert.AreEqual(seafoodFreshOrFrozen, importer.ParsedRows[0].SeafoodFreshOrFrozen);
            Assert.AreEqual(seafoodWildOrFarmRaised, importer.ParsedRows[0].SeafoodWildOrFarmRaised);
            Assert.AreEqual(vegan, importer.ParsedRows[0].VeganAgencyLineage);
            Assert.AreEqual(vegetarian.GetBoolStringFromCellText(), importer.ParsedRows[0].Vegetarian);
            Assert.AreEqual(wholeTrade.GetBoolStringFromCellText(), importer.ParsedRows[0].WholeTrade);
            Assert.AreEqual(String.Empty, importer.ParsedRows[0].Error);
            Assert.AreEqual(grassFed.GetBoolStringFromCellText(), importer.ParsedRows[0].GrassFed);
            Assert.AreEqual(pastureRaised.GetBoolStringFromCellText(), importer.ParsedRows[0].PastureRaised);
            Assert.AreEqual(freeRange.GetBoolStringFromCellText(), importer.ParsedRows[0].FreeRange);
            Assert.AreEqual(dryAged.GetBoolStringFromCellText(), importer.ParsedRows[0].DryAged);
            Assert.AreEqual(airChilled.GetBoolStringFromCellText(), importer.ParsedRows[0].AirChilled);
            Assert.AreEqual(madeInHouse.GetBoolStringFromCellText(), importer.ParsedRows[0].MadeInHouse);
            Assert.AreEqual(msc.GetBoolStringFromCellText(), importer.ParsedRows[0].Msc);
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_BlankCells_ShouldConvertToEmptyString()
        {
            // Given.
            ApplyIrmaItemHeaders();

            string scanCode = "1234567890";

            worksheet.Rows[1].Cells[0].Value = scanCode;

            importer.Workbook = workbook;

            // When.
            importer.IsValidSpreadsheetType();
            importer.ConvertSpreadsheetToModel();

            // Then.
            Assert.AreEqual(1, importer.ParsedRows.Count);

            var parsedRow = importer.ParsedRows[0];

            Assert.AreEqual(scanCode, parsedRow.ScanCode);
            Assert.AreEqual(String.Empty, parsedRow.BrandName);
            Assert.AreEqual(String.Empty, parsedRow.BrandLineage);
            Assert.AreEqual(String.Empty, parsedRow.BrandId);
            Assert.AreEqual(String.Empty, parsedRow.ProductDescription);
            Assert.AreEqual(String.Empty, parsedRow.PosDescription);
            Assert.AreEqual(String.Empty, parsedRow.PackageUnit);
            Assert.AreEqual(String.Empty, parsedRow.FoodStampEligible);
            Assert.AreEqual(String.Empty, parsedRow.PosScaleTare);
            Assert.AreEqual(String.Empty, parsedRow.RetailSize);
            Assert.AreEqual(String.Empty, parsedRow.RetailUom);
            Assert.AreEqual(String.Empty, parsedRow.DeliverySystem);
            Assert.AreEqual(String.Empty, parsedRow.IrmaSubTeamName);
            Assert.AreEqual(String.Empty, parsedRow.MerchandiseLineage);
            Assert.AreEqual(String.Empty, parsedRow.MerchandiseId);
            Assert.AreEqual(String.Empty, parsedRow.TaxLineage);
            Assert.AreEqual(String.Empty, parsedRow.AlcoholByVolume);
            Assert.AreEqual(String.Empty, parsedRow.TaxId);
            Assert.AreEqual(String.Empty, parsedRow.NationalLineage);
            Assert.AreEqual(String.Empty, parsedRow.NationalId);
            Assert.AreEqual(String.Empty, parsedRow.BrowsingLineage);
            Assert.AreEqual(String.Empty, parsedRow.BrowsingId);
            Assert.AreEqual(String.Empty, parsedRow.IsValidated);
            Assert.AreEqual(String.Empty, parsedRow.RegionCode);
            Assert.AreEqual(String.Empty, parsedRow.Biodynamic);
            Assert.AreEqual(String.Empty, parsedRow.CheeseAttributeMilkType);
            Assert.AreEqual(String.Empty, parsedRow.CheeseAttributeRaw);
            Assert.AreEqual(String.Empty, parsedRow.EcoScaleRating);
            Assert.AreEqual(String.Empty, parsedRow.GlutenFreeAgency);
            Assert.AreEqual(String.Empty, parsedRow.KosherAgency);
            Assert.AreEqual(String.Empty, parsedRow.OrganicAgency);
            Assert.AreEqual(String.Empty, parsedRow.NonGmoAgency);
            Assert.AreEqual(String.Empty, parsedRow.PremiumBodyCare);
            Assert.AreEqual(String.Empty, parsedRow.SeafoodFreshOrFrozen);
            Assert.AreEqual(String.Empty, parsedRow.SeafoodWildOrFarmRaised);
            Assert.AreEqual(String.Empty, parsedRow.VeganAgency);
            Assert.AreEqual(String.Empty, parsedRow.Vegetarian);
            Assert.AreEqual(String.Empty, parsedRow.WholeTrade);
            Assert.AreEqual(String.Empty, parsedRow.Error);
            Assert.AreEqual(String.Empty, parsedRow.GrassFed);
            Assert.AreEqual(String.Empty, parsedRow.PastureRaised);
            Assert.AreEqual(String.Empty, parsedRow.FreeRange);
            Assert.AreEqual(String.Empty, parsedRow.DryAged);
            Assert.AreEqual(String.Empty, parsedRow.AirChilled);
            Assert.AreEqual(String.Empty, parsedRow.MadeInHouse);
            
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_SpreadsheetContainsMoreItemsThanDefaultRowLimitOfTenThousand_ParsingLimitShouldBeApplied()
        {
            // Given.
            ApplyIrmaItemHeaders();

            string scanCode = "1234567890";
            string brand = "New Brand|0";
            string productDescription = "Test Product Description";
            string posDescription = "Test POS";
            string packageUnit = "EA";
            string foodStampEligible = "n";
            string posScaleTare = "0";

            for (int i = 1; i <= 10001; i++)
            {
                worksheet.Rows[i].Cells[0].Value = scanCode;
                worksheet.Rows[i].Cells[1].Value = brand;
                worksheet.Rows[i].Cells[2].Value = productDescription;
                worksheet.Rows[i].Cells[3].Value = posDescription;
                worksheet.Rows[i].Cells[4].Value = packageUnit;
                worksheet.Rows[i].Cells[5].Value = foodStampEligible;
                worksheet.Rows[i].Cells[6].Value = posScaleTare;
            }

            importer.Workbook = workbook;

            // When.
            importer.IsValidSpreadsheetType();
            importer.ConvertSpreadsheetToModel();

            // Then.
            int rowCount = 0;
            foreach (var row in worksheet.Rows)
            {
                rowCount++;
            }

            Assert.AreEqual(10002, rowCount);
            Assert.AreEqual(10000, importer.ParsedRows.Count);
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_EmptyRow_ShouldNotBeAddedToModel()
        {
            // Given.
            for (int i = 0; i <= 40; i++)
            {
                worksheet.Rows[1].Cells[i].Value = String.Empty;
            }

            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();

            // Then.
            Assert.AreEqual(0, importer.ParsedRows.Count);
        }

        [TestMethod]
        public void ConvertSpreadsheetToModel_BrandWithNoBrandId_BrandIdShouldBeSetToEmptyString()
        {
            // Given.
            string scanCode = "1234567890";
            string brand = "New Brand|";
            string productDescription = "Test Product Description";
            string posDescription = "Test POS";
            string packageUnit = "EA";
            string foodStampEligible = "n";
            string posScaleTare = "0";

            worksheet.Rows[1].Cells[0].Value = scanCode;
            worksheet.Rows[1].Cells[1].Value = brand;
            worksheet.Rows[1].Cells[2].Value = productDescription;
            worksheet.Rows[1].Cells[3].Value = posDescription;
            worksheet.Rows[1].Cells[4].Value = packageUnit;
            worksheet.Rows[1].Cells[5].Value = foodStampEligible;
            worksheet.Rows[1].Cells[6].Value = posScaleTare;

            importer.Workbook = workbook;

            // When.
            importer.ConvertSpreadsheetToModel();

            // Then.
            var parsedRow = importer.ParsedRows[0];

            Assert.AreEqual(String.Empty, parsedRow.BrandId);
            Assert.AreEqual("New Brand", parsedRow.BrandName);
            Assert.AreEqual(brand, parsedRow.BrandLineage);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_ScanCodeFormatIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string validScanCode = new String('1', Constants.ScanCodeMaxLength);
            string invalidScanCode = new String('1', Constants.ScanCodeMaxLength + 1);

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode(invalidScanCode));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode(validScanCode));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(2, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(String.Format("Scan code is required and must be {0} or fewer numbers.", Constants.ScanCodeMaxLength), importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_DuplicateScanCodesInSpreadsheet_ShouldAddErrorRows()
        {
            // Given.
            string duplicateScanCode = "1111";

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode(duplicateScanCode));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode(duplicateScanCode));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode(duplicateScanCode));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode("1111222"));

            importer.Workbook = workbook;

            importer.ParsedRows = importItems;

            AddHierarchyClassesToHierarchyLineageQueryResult(importer.ParsedRows);

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(4, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ValidRows.Count);
            Assert.AreEqual(3, importer.ErrorRows.Count);

            foreach (var errorRow in importer.ErrorRows)
            {
                Assert.AreEqual(duplicateScanCode, errorRow.ScanCode);
                Assert.AreEqual("Scan Code appears multiple times on the spreadsheet.", errorRow.Error);
            }
        }

        [TestMethod]
        public void ValidateSpreadsheetData_ProductDescriptionIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string validProductDescription = new String('a', Constants.ProductDescriptionMaxLength);
            string invalidProductDescription = new String('a', Constants.ProductDescriptionMaxLength + 1);

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithProductDescription(invalidProductDescription));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode("111").WithProductDescription(validProductDescription));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(2, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual("Product Description is invalid.  " + ValidatorErrorMessages.ProductDescriptionError, importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_PosDescriptionIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string validPosDescription = new String('a', Constants.PosDescriptionMaxLength);
            string invalidPosDescription = new String('a', Constants.PosDescriptionMaxLength + 1);

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithPosDescription(invalidPosDescription));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode("111").WithPosDescription(validPosDescription));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(2, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual("POS Description is invalid.  " + ValidatorErrorMessages.PosDescriptionError, importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_PosScaleTareIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string validPosScaleTare = "9.999";
            string invalidPosScaleTare = new String('2', Constants.PosScaleTareMaxLength);

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithPosScaleTare(validPosScaleTare));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode("111").WithPosScaleTare(invalidPosScaleTare));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(2, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual("POS Scale Tare must be a valid decimal value less than 10 with no more than 4 digits plus the decimal point.", importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_FoodStampEligibleIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string validFoodStampEligible = "1";
            string invalidFoodStampEligible = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithFoodStampEligible(validFoodStampEligible));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode("111").WithFoodStampEligible(invalidFoodStampEligible));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(2, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual("Food Stamp Eligible must be blank, Y, or N.", importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_PackageUnitIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string validPackageUnit = new String('1', Constants.PackageUnitMaxLength);
            string invalidPackageUnit = "1.1";

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithPackageUnit(validPackageUnit));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode("111").WithPackageUnit(invalidPackageUnit));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(2, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(String.Format("Item Pack must be a whole number with {0} or fewer digits.", Constants.PackageUnitMaxLength), importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RetailSizeIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string validRetailSize = Constants.RetailSizeMax.ToString();
            string invalidRetailSize = (Constants.RetailSizeMax + 1).ToString();

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithRetailSize(validRetailSize));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode("111").WithRetailSize(invalidRetailSize));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(2, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual("Size must be greater than zero and contain only numbers and decimal points with 5 or less digits to the left of the decimal and 4 or less digits to the right of the decimal.",
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RetailUomIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string validRetailUom = UomCodes.Each;
            string invalidRetailUom = "INVALID UOM";

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithRetailUom(validRetailUom));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode("111").WithRetailUom(invalidRetailUom));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(2, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual("UOM should be one of the following: EA, LB, CT, OZ, CS, PK, LT, PT, KG, ML, GL, GR, CG, FT, YD, QT, SQFT, MT, FZ.",
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_DeliverySystemIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string validDeliverySystem = DeliverySystems.Descriptions.Cap;
            string invalidDeliverySystem = "Invalid Delivery System";

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithDeliverySystem(validDeliverySystem));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode("111").WithDeliverySystem(invalidDeliverySystem));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(2, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual("Delivery System should be one of the following: CAP, CHW, LZ, SG, TB, VC, VS.",
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_ValidatedIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValidated = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode("111"));
            importItems[0].IsValidated = invalidValidated;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual("Validated should be Y, N, or blank.", importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_AnimalWelfareRatingIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].AnimalWelfareRating = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual(String.Format("Animal Welfare Rating is not recognized.  Valid entries are {0}.", String.Join(", ", AnimalWelfareRatings.Descriptions.AsArray)),
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_BiodynamicIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].Biodynamic = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual("Biodynamic should be Y, N, or blank.", importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_CheeseAttributeMilkTypeIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].CheeseAttributeMilkType = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual(String.Format("Cheese Attribute: Milk Type is not recognized.  Valid entries are {0}.", String.Join(", ", MilkTypes.Descriptions.AsArray)),
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_CheeseAttributeRawIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].CheeseAttributeRaw = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual("Cheese Attribute: Raw should be Y, N, or blank.", importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_EcoScaleIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].EcoScaleRating = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual(String.Format("Eco-Scale Rating is not recognized.  Valid entries are {0}.", String.Join(", ", EcoScaleRatings.Descriptions.AsArray)),
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_GlutenFreeIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            var mockHierarchyClasses = new List<HierarchyClass>
            {
                new TestHierarchyClassBuilder().WithHierarchyClassName("Agency1").WithHierarchyId(Hierarchies.CertificationAgencyManagement),
                new TestHierarchyClassBuilder().WithHierarchyClassName("Agency2").WithHierarchyId(Hierarchies.CertificationAgencyManagement)
            };

            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(mockHierarchyClasses);

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].GlutenFreeAgency = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual(String.Format("Gluten-Free agency is not recognized.  Valid entries are {0}.", String.Join(", ", mockHierarchyClasses.Select(hc => hc.hierarchyClassName).ToList())),
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_KosherIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            var mockHierarchyClasses = new List<HierarchyClass>
            {
                new TestHierarchyClassBuilder().WithHierarchyClassName("Agency1").WithHierarchyId(Hierarchies.CertificationAgencyManagement),
                new TestHierarchyClassBuilder().WithHierarchyClassName("Agency2").WithHierarchyId(Hierarchies.CertificationAgencyManagement)
            };

            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(mockHierarchyClasses);

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].KosherAgency = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual(String.Format("Kosher agency is not recognized.  Valid entries are {0}.", String.Join(", ", mockHierarchyClasses.Select(hc => hc.hierarchyClassName).ToList())),
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_NonGmoIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            var mockHierarchyClasses = new List<HierarchyClass>
            {
                new TestHierarchyClassBuilder().WithHierarchyClassName("Agency1").WithHierarchyId(Hierarchies.CertificationAgencyManagement),
                new TestHierarchyClassBuilder().WithHierarchyClassName("Agency2").WithHierarchyId(Hierarchies.CertificationAgencyManagement)
            };

            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(mockHierarchyClasses);

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].NonGmoAgency = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual(String.Format("Non-GMO agency is not recognized.  Valid entries are {0}.", String.Join(", ", mockHierarchyClasses.Select(hc => hc.hierarchyClassName).ToList())),
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_OrganicIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            var mockHierarchyClasses = new List<HierarchyClass>
            {
                new TestHierarchyClassBuilder().WithHierarchyClassName("Agency1").WithHierarchyId(Hierarchies.CertificationAgencyManagement),
                new TestHierarchyClassBuilder().WithHierarchyClassName("Agency2").WithHierarchyId(Hierarchies.CertificationAgencyManagement)
            };

            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(mockHierarchyClasses);

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].OrganicAgency = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual(String.Format("Organic agency is not recognized.  Valid entries are {0}.", String.Join(", ", mockHierarchyClasses.Select(hc => hc.hierarchyClassName).ToList())),
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_PremiumBodyCareIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].PremiumBodyCare = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual("Premium Body Care should be Y, N, or blank.", importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_GrassFedIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].GrassFed = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual("Grass Fed should be Y, N, or blank.", importer.ErrorRows[0].Error);
        }


        [TestMethod]
        public void ValidateSpreadsheetData_PastureRaisedIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].PastureRaised = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual("Pasture Raised should be Y, N, or blank.", importer.ErrorRows[0].Error);
        }


        [TestMethod]
        public void ValidateSpreadsheetData_FreeRangeIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].FreeRange = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual("Free Range should be Y, N, or blank.", importer.ErrorRows[0].Error);
        }


        [TestMethod]
        public void ValidateSpreadsheetData_DryAgedIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].DryAged = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual("Dry Aged should be Y, N, or blank.", importer.ErrorRows[0].Error);
        }


        [TestMethod]
        public void ValidateSpreadsheetData_AirChilledIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].AirChilled = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual("Air Chilled should be Y, N, or blank.", importer.ErrorRows[0].Error);
        }


        [TestMethod]
        public void ValidateSpreadsheetDataMadeInHouseIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].MadeInHouse = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual("Made In House should be Y, N, or blank.", importer.ErrorRows[0].Error);
        }
    
        
        [TestMethod]
        public void ValidateSpreadsheetData_SeafoodFreshOrFrozenIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].SeafoodFreshOrFrozen = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual(String.Format("Fresh Or Frozen is not recognized.  Valid entries are {0}.", String.Join(", ", SeafoodFreshOrFrozenTypes.Descriptions.AsArray)),
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_SeafoodWildOrFarmRaisedIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].SeafoodWildOrFarmRaised = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual(String.Format("Seafood: Wild Or Farm Raised is not recognized.  Valid entries are {0}.", String.Join(", ", SeafoodCatchTypes.Descriptions.AsArray)),
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_VeganIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            var mockHierarchyClasses = new List<HierarchyClass>
            {
                new TestHierarchyClassBuilder().WithHierarchyClassName("Agency1").WithHierarchyId(Hierarchies.CertificationAgencyManagement),
                new TestHierarchyClassBuilder().WithHierarchyClassName("Agency2").WithHierarchyId(Hierarchies.CertificationAgencyManagement)
            };

            mockGetCertificationAgenciesQuery.Setup(q => q.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>())).Returns(mockHierarchyClasses);

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].VeganAgency = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(String.Format("Vegan agency is not recognized.  Valid entries are {0}.", String.Join(", ", mockHierarchyClasses.Select(hc => hc.hierarchyClassName).ToList())),
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_VegetarianIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].Vegetarian = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual("Vegetarian should be Y, N, or blank.", importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_WholeTradeIsInvalid_ShouldAddErrorRow()
        {
            // Given.
            string invalidValue = "INVALID";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems[0].WholeTrade = invalidValue;

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(importItems.Count, importer.ErrorRows.Count);
            Assert.AreEqual("Whole Trade should be Y, N, or blank.", importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_HierarchyClassesDoNotExist_ShouldAddErrorRow()
        {
            // Given.
            string invalidBrandId = "111";
            string invalidMerchandiseId = "222";
            string invalidTaxId = "333";
            string invalidBrowsingId = "444";

            importItems.Add(new TestBulkImportNewItemModelBuilder());
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode("111"));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode("222"));
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode("333"));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importItems[0].BrandId = invalidBrandId;
            importItems[1].MerchandiseId = invalidMerchandiseId;
            importItems[2].TaxId = invalidTaxId;
            importItems[3].BrowsingId = invalidBrowsingId;

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(4, importer.ParsedRows.Count);
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(4, importer.ErrorRows.Count);
            Assert.AreEqual(String.Format("Brand is invalid.  {0} does not exist.", importer.ErrorRows[0].BrandLineage), importer.ErrorRows[0].Error);
            Assert.AreEqual(String.Format("Merchandise is invalid.  {0} does not exist.", importer.ErrorRows[0].MerchandiseLineage), importer.ErrorRows[1].Error);
            Assert.AreEqual(String.Format("Tax is invalid.  {0} does not exist.", importer.ErrorRows[0].TaxLineage), importer.ErrorRows[2].Error);
            Assert.AreEqual(String.Format("Browsing is invalid.  {0} does not exist.", importer.ErrorRows[0].BrowsingLineage), importer.ErrorRows[3].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_TaxClassWithNoAbbreviation_ShouldAddErrorRow()
        {
            // Given.
            int invalidTaxClassId = 1;

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithTaxId(invalidTaxClassId.ToString()));

            mockGetTaxHierarchyClassesWithNoAbbreviationQuery.Setup(m => m.Search(It.IsAny<GetTaxHierarchyClassesWithNoAbbreviationParameters>()))
                .Returns(new List<HierarchyClass>
                {
                    new HierarchyClass { hierarchyClassID = invalidTaxClassId }
                });

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual("Tax class has no abbreviation.", importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_HasAffinitySubBrick_ShouldAddErrorRow()
        {
            // Given.
            int affinitySubBrickId = 1234;

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithMerchandiseId(affinitySubBrickId.ToString()));
            affinitySubBricks.Add(new TestHierarchyClassBuilder().WithHierarchyClassId(affinitySubBrickId));

            mockGetAffinitySubBricksQuery.Setup(m => m.Search(It.IsAny<GetAffinitySubBricksParameters>()))
                .Returns(new List<HierarchyClass>
                {
                    new HierarchyClass { hierarchyClassID = affinitySubBrickId }
                });

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual("Items cannot be associated to an Affinity sub-brick.", importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_RowDoesNotContainRequiredInformation_ShouldAddErrorRow()
        {
            // Given.
            BulkImportNewItemModel invalidImportModelWithNullField = new TestBulkImportNewItemModelBuilder().WithProductDescription(null);
            BulkImportNewItemModel invalidImportModelWithEmptyField = new TestBulkImportNewItemModelBuilder().WithScanCode("111").WithProductDescription(String.Empty);
            BulkImportNewItemModel validImportModel = new TestBulkImportNewItemModelBuilder()
                .WithScanCode("222")
                .WithMerchandiseId(String.Empty)
                .WithTaxId(String.Empty)
                .WithBrowsingId(String.Empty);

            importItems.Add(invalidImportModelWithNullField);
            importItems.Add(invalidImportModelWithEmptyField);
            importItems.Add(validImportModel);

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(3, importer.ParsedRows.Count);
            Assert.AreEqual(2, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ValidRows.Count);
            Assert.AreEqual("Product Description is invalid.  Please enter 60 or fewer valid characters.", importer.ErrorRows[0].Error);
            Assert.AreEqual("The row does not contain all information required to add the item.", importer.ErrorRows[1].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_InvalidAlcoholByVolume_ShouldAddErrorRow()
        {
            // Given.
            Random r = new Random();
            int rInt = r.Next(2, 100); //for ints
            double range = 100 * 0.99;
            double rDouble = r.NextDouble() * range; //for doubles
            string invalidAlcoholByVolume = rDouble.ToString();

            importItems.Add(new TestBulkImportNewItemModelBuilder()
                .WithAlcoholByVolume(invalidAlcoholByVolume));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual("Alcohol By Volume must be a number between 0 and 99.99 with up to two decimal places.", importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_ScanCodeAlreadyExists_ShouldAddErrorRow()
        {
            // Given.
            string existingScanCode = "111";

            mockGetExistingScanCodeUploadsQuery.Setup(m => m.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()))
                .Returns(new List<ScanCodeModel>
                {
                    new ScanCodeModel { ScanCode = existingScanCode }
                });

            importItems.Add(new TestBulkImportNewItemModelBuilder().WithScanCode(existingScanCode));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual("Scan Code already exists.", importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_InvalidNewBrandLength_ShouldAddErrorRow()
        {
            // Given.
            string invalidBrandName = new String('b', Constants.IconBrandNameMaxLength + 1);
            string invalidBrandId = "1";

            importItems.Add(new TestBulkImportNewItemModelBuilder()
                .WithBrandLineage(invalidBrandName + "|" + invalidBrandId)
                .WithBrandName(invalidBrandName)
                .WithBrandId(invalidBrandId));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual("Brand name is invalid.  Please enter 35 or fewer valid characters.", importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_TrimmedBrandIsDuplicate_ShouldAddErrorRow()
        {
            // Given.
            string duplicateBrand = new String('b', Constants.IrmaBrandNameMaxLength + 1);
            string duplicateBrandId = "1";

            importItems.Add(new TestBulkImportNewItemModelBuilder()
                .WithBrandLineage(duplicateBrand)
                .WithBrandName(duplicateBrand)
                .WithBrandId(duplicateBrandId));

            List<string> hierarchyClasses = new List<string>
                {
                    duplicateBrand
                };

            mockGetDuplicateBrandsByTrimmedNameQuery.Setup(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>())).Returns(hierarchyClasses);

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual("Brand name reduced to 25 characters already exists in Icon.  Adding the brand name may cause conflicts with IRMA.",
                importer.ErrorRows[0].Error);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_NewBrandWithBrandIdOfZero_ShouldNotAddErrorRow()
        {
            // Given.
            worksheet.Rows[1].Cells[0].Value = "111222333444";
            worksheet.Rows[1].Cells[1].Value = "Brand|0";
            worksheet.Rows[1].Cells[2].Value = "Test Product Description";
            worksheet.Rows[1].Cells[3].Value = "Test POS Description";
            worksheet.Rows[1].Cells[4].Value = "1"; // PackageUnit
            worksheet.Rows[1].Cells[5].Value = "Y"; // FoodStampEligible
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.SizeColumnIndex].Value = "1";
            worksheet.Rows[1].Cells[ExcelHelper.IrmaItemColumnIndexes.UomColumnIndex].Value = "EA";
            worksheet.Rows[1].Cells[7].Value = "1234"; // RetailSize

            // When.
            importer.ConvertSpreadsheetToModel();
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(1, importer.ValidRows.Count);
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(0, importer.ErrorRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_ValidatedItemsHaveRequiredFields_ShouldValidate()
        {
            // Given.
            importItems.Add(new TestBulkImportNewItemModelBuilder().WithIsValidated(true));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(0, importer.ErrorRows.Count);
            Assert.AreEqual(1, importer.ValidRows.Count);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_ValidatedItemsDoNotHaveRequiredFields_ShouldAddErrorRow()
        {
            // Given.
            importItems.Add(new TestBulkImportNewItemModelBuilder()
                .WithIsValidated(true)
                .WithMerchandiseId(String.Empty));

            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);

            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(1, importer.ParsedRows.Count);
            Assert.AreEqual(1, importer.ErrorRows.Count);
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual("Rows marked for validation must have all canonical information in the spreadsheet.", importer.ErrorRows[0].Error);
        }

        private void AddHierarchyClassesToHierarchyLineageQueryResult(List<BulkImportNewItemModel> importModels)
        {
            var hierarchyClassListModel = new HierarchyClassListModel
            {
                BrandHierarchyList = new List<HierarchyClassModel>(),
                BrowsingHierarchyList = new List<HierarchyClassModel>(),
                MerchandiseHierarchyList = new List<HierarchyClassModel>(),
                TaxHierarchyList = new List<HierarchyClassModel>(),
                NationalHierarchyList = new List<HierarchyClassModel>()
            };

            foreach (var importModel in importModels)
            {
                if (!String.IsNullOrWhiteSpace(importModel.BrandId) && importModel.BrandId != "0")
                    hierarchyClassListModel.BrandHierarchyList.Add(new HierarchyClassModel { HierarchyClassId = Int32.Parse(importModel.BrandId) });

                if (!String.IsNullOrWhiteSpace(importModel.MerchandiseId) && importModel.MerchandiseId != "0")
                    hierarchyClassListModel.MerchandiseHierarchyList.Add(new HierarchyClassModel { HierarchyClassId = Int32.Parse(importModel.MerchandiseId) });

                if (!String.IsNullOrWhiteSpace(importModel.TaxId) && importModel.TaxId != "0")
                    hierarchyClassListModel.TaxHierarchyList.Add(new HierarchyClassModel { HierarchyClassId = Int32.Parse(importModel.TaxId) });

                if (!String.IsNullOrWhiteSpace(importModel.BrowsingId) && importModel.BrowsingId != "0")
                    hierarchyClassListModel.BrowsingHierarchyList.Add(new HierarchyClassModel { HierarchyClassId = Int32.Parse(importModel.BrowsingId) });


                if (!String.IsNullOrWhiteSpace(importModel.NationalId) && importModel.NationalId != "0")
                    hierarchyClassListModel.NationalHierarchyList.Add(new HierarchyClassModel { HierarchyClassId = Int32.Parse(importModel.NationalId) });
            }

            mockGetHierarchyLineageQuery.Setup(m => m.Search(It.IsAny<GetHierarchyLineageParameters>()))
                .Returns(hierarchyClassListModel);
        }

        [TestMethod]
        public void ValidateSpreadsheetData_MultipleRowsWithSameBrand_TrimmedBrandIsDuplicate_ShouldAddErrorRows()
        {
            // Given.
            string duplicateBrand = new String('b', Constants.IrmaBrandNameMaxLength + 1);
            string duplicateBrandId = "1";
            importItems.Add(new TestBulkImportNewItemModelBuilder()
                .WithBrandLineage(duplicateBrand)
                .WithBrandName(duplicateBrand)
                .WithBrandId(duplicateBrandId));

            importItems.Add(new TestBulkImportNewItemModelBuilder()
               .WithScanCode("222222222223")
               .WithBrandLineage(duplicateBrand)
               .WithBrandName(duplicateBrand)
               .WithBrandId(duplicateBrandId));

            List<string> hierarchyClasses = new List<string>
                {
                    duplicateBrand
                };
            mockGetDuplicateBrandsByTrimmedNameQuery.Setup(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>())).Returns(hierarchyClasses);
            AddHierarchyClassesToHierarchyLineageQueryResult(importItems);
            importer.ParsedRows = importItems;

            // When.
            importer.ValidateSpreadsheetData();

            // Then.
            Assert.AreEqual(2, importer.ErrorRows.Count);
            Assert.AreEqual(0, importer.ValidRows.Count);
            Assert.AreEqual(2, importer.ParsedRows.Count);
            Assert.AreEqual("Brand name reduced to 25 characters already exists in Icon.  Adding the brand name may cause conflicts with IRMA.",
                importer.ErrorRows[0].Error);
            Assert.AreEqual("Brand name reduced to 25 characters already exists in Icon.  Adding the brand name may cause conflicts with IRMA.",
                importer.ErrorRows[1].Error);

        }

        private void ApplyConsolidatedItemHeaders()
        {
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ScanCodeColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.ScanCode;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BrandColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Brand;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ProductDescriptionColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.ProductDescription;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PosDescriptionColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.PosDescription;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PackageUnitColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.PackageUnit;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.FoodStampEligibleColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.FoodStampEligible;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PosScaleTareColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.PosScaleTare;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SizeColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Size;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.UomColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Uom;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DeliverySystemColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.DeliverySystem;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MerchandiseColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Merchandise;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.TaxColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Tax;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AlcoholByVolumeColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.AlcoholByVolume;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.NationalColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.NationalClass;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BrowsingColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Browsing;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.ValidatedColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Validated;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.HiddenItemColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.HiddenItem;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DepartmentSaleColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.DepartmentSale;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.NotesColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Notes;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.AnimalWelfareRating;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.BiodynamicColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Biodynamic;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.CheeseAttributeMilkType;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.CheeseAttributeRawColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.CheeseAttributeRaw;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.EcoScaleRatingColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.EcoScaleRating;    
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PremiumBodyCareColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.PremiumBodyCare;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.SeafoodFreshOrFrozen;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.SeafoodWildOrFarmRaised;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.VegetarianColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Vegetarian;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.WholeTradeColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.WholeTrade;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.GrassFedColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.GrassFed;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.PastureRaisedColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.PastureRaised;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.FreeRangeColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.FreeRange;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.DryAgedColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.DryAged;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.AirChilledColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.AirChilled;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MadeInHouseColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.MadeInHouse;
            worksheet.Rows[0].Cells[ExcelHelper.ConsolidatedItemColumnIndexes.MscColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Msc;
        }

        private void ApplyIrmaItemHeaders()
        {
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.ScanCodeColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.ScanCode;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.BrandColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Brand;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.ProductDescriptionColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.ProductDescription;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.PosDescriptionColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.PosDescription;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.PackageUnitColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.PackageUnit;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.FoodStampEligibleColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.FoodStampEligible;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.PosScaleTareColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.PosScaleTare;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.SizeColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Size;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.UomColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Uom;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.DeliverySystemColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.DeliverySystem;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.IrmaSubTeamColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.IrmaSubTeam;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.MerchandiseColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Merchandise;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.TaxColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Tax;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.AlcoholByVolumeIndex].Value = ExcelHelper.ExcelExportColumnNames.AlcoholByVolume;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.NationalColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.NationalClass;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.BrowsingColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Browsing;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.ValidatedColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Validated;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.RegionCodeColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Region;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.AnimalWelfareRatingColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.AnimalWelfareRating;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.BiodynamicColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Biodynamic;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeMilkTypeColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.CheeseAttributeMilkType;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.CheeseAttributeRawColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.CheeseAttributeRaw;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.EcoScaleRatingColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.EcoScaleRating;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.NutritionRequiredIndex].Value = ExcelHelper.ExcelExportColumnNames.NutritionRequired;         
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.PremiumBodyCareColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.PremiumBodyCare;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodFreshOrFrozenColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.SeafoodFreshOrFrozen;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.SeafoodWildOrFarmRaisedColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.SeafoodWildOrFarmRaised;        
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.VegetarianColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Vegetarian;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.WholeTradeColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.WholeTrade;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.GrassFedColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.GrassFed;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.PastureRaisedColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.PastureRaised;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.FreeRangeColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.FreeRange;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.DryAgedColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.DryAged;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.AirChilledColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.AirChilled;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.MadeInHouseColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.MadeInHouse;
            worksheet.Rows[0].Cells[ExcelHelper.IrmaItemColumnIndexes.MscColumnIndex].Value = ExcelHelper.ExcelExportColumnNames.Msc;
        }
    }
}
