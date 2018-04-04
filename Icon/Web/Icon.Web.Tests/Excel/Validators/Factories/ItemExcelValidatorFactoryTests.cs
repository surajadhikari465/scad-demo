using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.Common;
using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.App_Start;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Mvc.Excel.Validators;
using Icon.Web.Mvc.Excel.Validators.Factories;
using Icon.Web.Tests.Common.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Unit.Excel.Validators.Factories
{
    [TestClass] [Ignore]
    public class ItemExcelValidatorFactoryTests
    {
        private ItemExcelValidatorFactory factory;
        private Container container;
        private List<IExcelValidator<ItemExcelModel>> validators;
        private List<ItemExcelModel> models;
        private Mock<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>> mockGetCertificationAgenciesByTraitQuery;
        private Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>> mockGetHierarchyLineageQuery;
        private Mock<IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>>> mockGetAffinitySubBricksQuery;
        private Mock<IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>>> mockGetTaxHierarchyClassesWithNoAbbreviationQuery;
        private Mock<IQueryHandler<GetScanCodesNotReadyToValidateParameters, List<string>>> mockGetScanCodesNotReadyToValidateQuery;
        private Mock<IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>>> mockGetNewScanCodeUploadsQuery;
        private ItemExcelModel testItem1 = new ItemExcelModel();
        private ItemExcelModel testItem2 = new ItemExcelModel();
        private ItemExcelModel testItem3 = new ItemExcelModel();
        private ItemExcelModel testItem5 = new ItemExcelModel();
        private ItemExcelModel testItem4 = new ItemExcelModel();
        private ItemExcelModel testItem6 = new ItemExcelModel();
        private HierarchyClassModel testBrand = new TestHierarchyClassModelBuilder()
            .WithHierarchyId(Hierarchies.Brands)
            .WithHierarchyClassId(1);
        private HierarchyClassModel testMerchandise = new TestHierarchyClassModelBuilder()
            .WithHierarchyId(Hierarchies.Merchandise)
            .WithHierarchyClassId(2);
        private HierarchyClassModel testNational = new TestHierarchyClassModelBuilder()
            .WithHierarchyId(Hierarchies.National)
            .WithHierarchyClassId(3);
        private HierarchyClassModel testTax = new TestHierarchyClassModelBuilder()
            .WithHierarchyId(Hierarchies.Tax)
            .WithHierarchyClassId(4);
        private HierarchyClassModel testBrowsing = new TestHierarchyClassModelBuilder()
            .WithHierarchyId(Hierarchies.Browsing)
            .WithHierarchyClassId(5);
        private HierarchyClass testGlutenFree = new TestHierarchyClassBuilder()
            .WithHierarchyClassName("Test Gluten Free")
            .WithHierarchyClassId(6)
            .WithGlutenFreeTrait("1")
            .WithHierarchyId(Hierarchies.CertificationAgencyManagement);
        private HierarchyClass testKosher = new TestHierarchyClassBuilder()
            .WithHierarchyClassName("Test Kosher")
            .WithHierarchyClassId(7)
            .WithKosherTrait("1")
            .WithHierarchyId(Hierarchies.CertificationAgencyManagement);
        private HierarchyClass testNonGmo = new TestHierarchyClassBuilder()
            .WithHierarchyClassName("Test Non Gmo")
            .WithHierarchyClassId(8)
            .WithNonGmoTrait("1")
            .WithHierarchyId(Hierarchies.CertificationAgencyManagement);
        private HierarchyClass testOrganic = new TestHierarchyClassBuilder()
            .WithHierarchyClassName("Test Organic")
            .WithHierarchyClassId(9)
            .WithOrganicTrait("1")
            .WithHierarchyId(Hierarchies.CertificationAgencyManagement);
        private HierarchyClass testVegan = new TestHierarchyClassBuilder()
            .WithHierarchyClassName("Test Vegan")
            .WithHierarchyClassId(10)
            .WithVeganTrait("1")
            .WithHierarchyId(Hierarchies.CertificationAgencyManagement);

        [TestInitialize]
        public void Initialize()
        {
            mockGetCertificationAgenciesByTraitQuery = new Mock<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>>();
            mockGetHierarchyLineageQuery = new Mock<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>();
            mockGetAffinitySubBricksQuery = new Mock<IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>>>();
            mockGetTaxHierarchyClassesWithNoAbbreviationQuery = new Mock<IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>>>();
            mockGetScanCodesNotReadyToValidateQuery = new Mock<IQueryHandler<GetScanCodesNotReadyToValidateParameters, List<string>>>();
            mockGetNewScanCodeUploadsQuery = new Mock<IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>>>();

            mockGetCertificationAgenciesByTraitQuery.Setup(m => m.Search(It.Is<GetCertificationAgenciesByTraitParameters>(p => p.AgencyTypeTraitCode == Traits.Codes.GlutenFree)))
                .Returns(new List<HierarchyClass>
                {
                    testGlutenFree
                });
            mockGetCertificationAgenciesByTraitQuery.Setup(m => m.Search(It.Is<GetCertificationAgenciesByTraitParameters>(p => p.AgencyTypeTraitCode == Traits.Codes.Kosher)))
                 .Returns(new List<HierarchyClass>
                 {
                    testKosher
                 });
            mockGetCertificationAgenciesByTraitQuery.Setup(m => m.Search(It.Is<GetCertificationAgenciesByTraitParameters>(p => p.AgencyTypeTraitCode == Traits.Codes.NonGmo)))
                  .Returns(new List<HierarchyClass>
                  {
                    testNonGmo
                  });
            mockGetCertificationAgenciesByTraitQuery.Setup(m => m.Search(It.Is<GetCertificationAgenciesByTraitParameters>(p => p.AgencyTypeTraitCode == Traits.Codes.Organic)))
                  .Returns(new List<HierarchyClass>
                  {
                    testOrganic
                  });
            mockGetCertificationAgenciesByTraitQuery.Setup(m => m.Search(It.Is<GetCertificationAgenciesByTraitParameters>(p => p.AgencyTypeTraitCode == Traits.Codes.Vegan)))
                  .Returns(new List<HierarchyClass>
                  {
                    testVegan
                  });
            mockGetHierarchyLineageQuery.Setup(m => m.Search(It.IsAny<GetHierarchyLineageParameters>()))
                .Returns(new HierarchyClassListModel
                {
                    BrandHierarchyList = new List<HierarchyClassModel> { testBrand },
                    MerchandiseHierarchyList = new List<HierarchyClassModel> { testMerchandise },
                    NationalHierarchyList = new List<HierarchyClassModel> { testNational },
                    TaxHierarchyList = new List<HierarchyClassModel> { testTax },
                    BrowsingHierarchyList = new List<HierarchyClassModel> { testBrowsing }
                });
            mockGetAffinitySubBricksQuery.Setup(m => m.Search(It.IsAny<GetAffinitySubBricksParameters>()))
                .Returns(new List<HierarchyClass>());
            mockGetTaxHierarchyClassesWithNoAbbreviationQuery.Setup(m => m.Search(It.IsAny<GetTaxHierarchyClassesWithNoAbbreviationParameters>()))
                .Returns(new List<HierarchyClass>());
            mockGetScanCodesNotReadyToValidateQuery.Setup(m => m.Search(It.IsAny<GetScanCodesNotReadyToValidateParameters>()))
                .Returns(new List<string>());
            mockGetNewScanCodeUploadsQuery.Setup(m => m.Search(It.IsAny<GetNewScanCodeUploadsParameters>()))
                .Returns(new List<ScanCodeModel>());

            container = new Container();
            SimpleInjectorInitializer.InitializeContainer(container);
            container.Options.AllowOverridingRegistrations = true;
            container.Register<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>>(() => mockGetCertificationAgenciesByTraitQuery.Object);
            container.Register<IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel>>(() => mockGetHierarchyLineageQuery.Object);
            container.Register<IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>>>(() => mockGetAffinitySubBricksQuery.Object);
            container.Register<IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>>>(() => mockGetTaxHierarchyClassesWithNoAbbreviationQuery.Object);
            container.Register<IQueryHandler<GetScanCodesNotReadyToValidateParameters, List<string>>>(() => mockGetScanCodesNotReadyToValidateQuery.Object);
            container.Register<IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>>>(() => mockGetNewScanCodeUploadsQuery.Object);

            factory = new ItemExcelValidatorFactory(container);
            validators = factory.CreateValidators().ToList();

            models = CreateTestItems();
        }


        [TestMethod]
        public void ItemExcelValidatorFactory_ItemsAreValid_ShouldNotSetError()
        {
            //When
            ValidateModels();

            //Then
            foreach (var model in models)
            {
                Assert.IsNull(model.Error, model.Error);
            }
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_ScanCodeIsInvalid_ShouldSetError()
        {
            //Given
            testItem1.ScanCode = new string('1', Constants.ScanCodeMaxLength + 1);
            testItem2.ScanCode = new string('2', Constants.ScanCodeMaxLength);
            testItem3.ScanCode = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(string.Format("Scan code is required and must be {0} or fewer numbers.", Constants.ScanCodeMaxLength), testItem1.Error);
            Assert.IsNull(testItem2.Error);
            Assert.AreEqual(string.Format("Scan code is required and must be {0} or fewer numbers.", Constants.ScanCodeMaxLength), testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_ProductDescriptionIsInvalid_ShouldSetError()
        {
            //Given
            testItem1.ProductDescription = new string('1', Constants.ProductDescriptionMaxLength + 1);
            testItem2.ProductDescription = new string('2', Constants.ProductDescriptionMaxLength);
            testItem3.ProductDescription = string.Empty;
            testItem4.ProductDescription = "" + (char)34;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual("Product Description is invalid. " + ValidatorErrorMessages.ProductDescriptionError, testItem1.Error);
            Assert.IsNull(testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.AreEqual("Product Description is invalid. " + ValidatorErrorMessages.ProductDescriptionError, testItem4.Error);
            Assert.IsNull(testItem5.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_PosDescriptionIsInvalid_ShouldSetError()
        {
            //Given
            testItem1.PosDescription = new string('1', Constants.PosDescriptionMaxLength + 1);
            testItem2.PosDescription = new string('2', Constants.PosDescriptionMaxLength);
            testItem3.PosDescription = string.Empty;
            testItem4.PosDescription = "" + (char)34;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual("POS Description is invalid. " + ValidatorErrorMessages.PosDescriptionError, testItem1.Error);
            Assert.IsNull(testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.AreEqual("POS Description is invalid. " + ValidatorErrorMessages.PosDescriptionError, testItem4.Error);
            Assert.IsNull(testItem5.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_PosScaleTareIsInvalid_ShouldSetError()
        {
            //Given
            testItem1.PosScaleTare = "abc";
            testItem2.PosScaleTare = "9.999";
            testItem3.PosScaleTare = "10.00000001";
            testItem4.PosScaleTare = ".00001";
            testItem5.PosScaleTare = "0";
            testItem6.PosScaleTare = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual("POS Scale Tare must be a valid decimal value less than 10 with no more than 4 digits plus the decimal point.", testItem1.Error);
            Assert.IsNull(testItem2.Error);
            Assert.AreEqual("POS Scale Tare must be a valid decimal value less than 10 with no more than 4 digits plus the decimal point.", testItem3.Error);
            Assert.AreEqual("POS Scale Tare must be a valid decimal value less than 10 with no more than 4 digits plus the decimal point.", testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_PackageUnitIsInvalid_ShouldSetError()
        {
            //Given
            testItem1.PackageUnit = "abc";
            testItem2.PackageUnit = "1234.123";
            testItem3.PackageUnit = "1234.1234";
            testItem4.PackageUnit = "1";
            testItem5.PackageUnit = "0";
            testItem6.PosScaleTare = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(string.Format("Item Pack must be a whole number with {0} or fewer digits.", Constants.PackageUnitMaxLength), testItem1.Error);
            Assert.IsNull(testItem2.Error);
            Assert.AreEqual(string.Format("Item Pack must be a whole number with {0} or fewer digits.", Constants.PackageUnitMaxLength), testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_RetailSizeIsInvalid_ShouldSetError()
        {
            //Given
            string error = "Size must be greater than zero and contain only numbers and decimal points with 5 or less digits to the left of the decimal and 4 or less digits to the right of the decimal.";

            testItem1.RetailSize = "abc";
            testItem2.RetailSize = "12345.1234";
            testItem3.RetailSize = "123456.1234";
            testItem4.RetailSize = "12345.12345";
            testItem5.RetailSize = "0";
            testItem6.RetailSize = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.IsNull(testItem2.Error);
            Assert.AreEqual(error, testItem3.Error);
            Assert.AreEqual(error, testItem4.Error);
            Assert.AreEqual(error, testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_RetailUomIsInvalid_ShouldSetError()
        {
            //Given
            string error = string.Format("UOM should be one of the following: {0}.", string.Join(", ", UomCodes.ByName.Values));

            testItem1.Uom = "abc";
            testItem2.Uom = "12345.1234";
            testItem3.Uom = UomCodes.ByName.Values.First();
            testItem4.Uom = UomCodes.ByName.Values.Last();
            testItem5.Uom = string.Empty;
            testItem6.Uom = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_DeliverySystemIsInvalid_ShouldSetError()
        {
            //Given
            string error = string.Format("Delivery System should be one of the following: {0}.", string.Join(", ", DeliverySystems.AsDictionary.Values));

            testItem1.DeliverySystem = "abc";
            testItem2.DeliverySystem = "12345.1234";
            testItem3.DeliverySystem = DeliverySystems.AsDictionary.Values.First();
            testItem4.DeliverySystem = DeliverySystems.AsDictionary.Values.Last();
            testItem5.DeliverySystem = string.Empty;
            testItem6.DeliverySystem = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_AnimalWelfareRatingIsInvalid_ShouldSetError()
        {
            //Given
            string error = string.Format("Animal Welfare Rating is not recognized.  Valid entries are {0}.", string.Join(", ", AnimalWelfareRatings.Descriptions.AsArray));

            testItem1.AnimalWelfareRating = "abc";
            testItem2.AnimalWelfareRating = "12345.1234";
            testItem3.AnimalWelfareRating = AnimalWelfareRatings.Descriptions.AsArray.First();
            testItem4.AnimalWelfareRating = AnimalWelfareRatings.Descriptions.AsArray.Last();
            testItem5.AnimalWelfareRating = Constants.ExcelImportRemoveValueKeyword;
            testItem6.AnimalWelfareRating = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_CheeseMilkTypeIsInvalid_ShouldSetError()
        {
            //Given
            string error = string.Format("Cheese Attribute: Milk Type is not recognized.  Valid entries are {0}.", string.Join(", ", MilkTypes.Descriptions.AsArray));

            testItem1.CheeseAttributeMilkType = "abc";
            testItem2.CheeseAttributeMilkType = "12345.1234";
            testItem3.CheeseAttributeMilkType = MilkTypes.Descriptions.AsArray.First();
            testItem4.CheeseAttributeMilkType = MilkTypes.Descriptions.AsArray.Last();
            testItem5.CheeseAttributeMilkType = Constants.ExcelImportRemoveValueKeyword;
            testItem6.CheeseAttributeMilkType = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_EcoScaleRatingIsInvalid_ShouldSetError()
        {
            //Given
            string error = string.Format("Eco-Scale Rating is not recognized.  Valid entries are {0}.", string.Join(", ", EcoScaleRatings.Descriptions.AsArray));

            testItem1.EcoScaleRating = "abc";
            testItem2.EcoScaleRating = "12345.1234";
            testItem3.EcoScaleRating = EcoScaleRatings.Descriptions.AsArray.First();
            testItem4.EcoScaleRating = EcoScaleRatings.Descriptions.AsArray.Last();
            testItem5.EcoScaleRating = Constants.ExcelImportRemoveValueKeyword;
            testItem6.EcoScaleRating = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_SeafoodFreshOrFrozenIsInvalid_ShouldSetError()
        {
            //Given
            string error = string.Format("Fresh Or Frozen is not recognized.  Valid entries are {0}.", string.Join(", ", SeafoodFreshOrFrozenTypes.Descriptions.AsArray));

            testItem1.SeafoodFreshOrFrozen = "abc";
            testItem2.SeafoodFreshOrFrozen = "12345.1234";
            testItem3.SeafoodFreshOrFrozen = SeafoodFreshOrFrozenTypes.Descriptions.AsArray.First();
            testItem4.SeafoodFreshOrFrozen = SeafoodFreshOrFrozenTypes.Descriptions.AsArray.Last();
            testItem5.SeafoodFreshOrFrozen = Constants.ExcelImportRemoveValueKeyword;
            testItem6.SeafoodFreshOrFrozen = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_SeafoodCatchTypesIsInvalid_ShouldSetError()
        {
            //Given
            string error = string.Format("Seafood: Wild Or Farm Raised is not recognized.  Valid entries are {0}.", string.Join(", ", SeafoodCatchTypes.Descriptions.AsArray));

            testItem1.SeafoodWildOrFarmRaised = "abc";
            testItem2.SeafoodWildOrFarmRaised = "12345.1234";
            testItem3.SeafoodWildOrFarmRaised = SeafoodCatchTypes.Descriptions.AsArray.First();
            testItem4.SeafoodWildOrFarmRaised = SeafoodCatchTypes.Descriptions.AsArray.Last();
            testItem5.SeafoodWildOrFarmRaised = Constants.ExcelImportRemoveValueKeyword;
            testItem6.SeafoodWildOrFarmRaised = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_GlutenFreeAgencyDoesNotExist_ShouldSetError()
        {
            //Given
            string error = string.Format(
                "{0} agency is not recognized. Valid entries are {1}.",
                typeof(ItemExcelModel).GetProperty("GlutenFree").GetCustomAttribute<ExcelColumnAttribute>().Column,
                testGlutenFree.hierarchyClassName);

            testItem1.GlutenFree = "abc";
            testItem2.GlutenFree = testKosher.hierarchyClassID.ToString();
            testItem3.GlutenFree = testGlutenFree.hierarchyClassID.ToString();
            testItem4.GlutenFree = "test|test||test|||test|" + testGlutenFree.hierarchyClassID.ToString();
            testItem5.GlutenFree = Constants.ExcelRemoveFieldIndicator;
            testItem6.GlutenFree = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_KosherAgencyDoesNotExist_ShouldSetError()
        {
            //Given
            string error = string.Format(
                "{0} agency is not recognized. Valid entries are {1}.",
                typeof(ItemExcelModel).GetProperty("Kosher").GetCustomAttribute<ExcelColumnAttribute>().Column,
                testKosher.hierarchyClassName);

            testItem1.Kosher = "abc";
            testItem2.Kosher = testGlutenFree.hierarchyClassID.ToString();
            testItem3.Kosher = testKosher.hierarchyClassID.ToString();
            testItem4.Kosher = "test|test||test|||test|" + testKosher.hierarchyClassID.ToString();
            testItem5.Kosher = Constants.ExcelRemoveFieldIndicator;
            testItem6.Kosher = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_NonGmoAgencyDoesNotExist_ShouldSetError()
        {
            //Given
            string error = string.Format(
                "{0} agency is not recognized. Valid entries are {1}.",
                typeof(ItemExcelModel).GetProperty("NonGmo").GetCustomAttribute<ExcelColumnAttribute>().Column,
                testNonGmo.hierarchyClassName);

            testItem1.NonGmo = "abc";
            testItem2.NonGmo = testGlutenFree.hierarchyClassID.ToString();
            testItem3.NonGmo = testNonGmo.hierarchyClassID.ToString();
            testItem4.NonGmo = "test|test||test|||test|" + testNonGmo.hierarchyClassID.ToString();
            testItem5.NonGmo = Constants.ExcelRemoveFieldIndicator;
            testItem6.NonGmo = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_OrganicAgencyDoesNotExist_ShouldSetError()
        {
            //Given
            string error = string.Format(
                "{0} agency is not recognized. Valid entries are {1}.",
                typeof(ItemExcelModel).GetProperty("Organic").GetCustomAttribute<ExcelColumnAttribute>().Column,
                testOrganic.hierarchyClassName);

            testItem1.Organic = "abc";
            testItem2.Organic = testGlutenFree.hierarchyClassID.ToString();
            testItem3.Organic = testOrganic.hierarchyClassID.ToString();
            testItem4.Organic = "test|test||test|||test|" + testOrganic.hierarchyClassID.ToString();
            testItem5.Organic = Constants.ExcelRemoveFieldIndicator;
            testItem6.Organic = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_VeganAgencyDoesNotExist_ShouldSetError()
        {
            //Given
            string error = string.Format(
                "{0} agency is not recognized. Valid entries are {1}.",
                typeof(ItemExcelModel).GetProperty("Vegan").GetCustomAttribute<ExcelColumnAttribute>().Column,
                testVegan.hierarchyClassName);

            testItem1.Vegan = "abc";
            testItem2.Vegan = testGlutenFree.hierarchyClassID.ToString();
            testItem3.Vegan = testVegan.hierarchyClassID.ToString();
            testItem4.Vegan = "test|test||test|||test|" + testVegan.hierarchyClassID.ToString();
            testItem5.Vegan = Constants.ExcelRemoveFieldIndicator;
            testItem6.Vegan = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_AirChilledIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.AirChilled)),
                "Air Chilled should be Y, N, or blank.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_BiodynamicIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.Biodynamic)),
                "Biodynamic should be Y, N, or blank.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_CheeseAttributeRawIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.CheeseAttributeRaw)),
                "Cheese Attribute: Raw should be Y, N, or blank.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_DepartmentSaleIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.DepartmentSale)),
                "Department Sale must be blank, Y, or N.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_DryAgedIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.DryAged)),
                "Dry Aged should be Y, N, or blank.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_FoodStampEligibleIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.FoodStampEligible)),
                "Food Stamp Eligible must be blank, Y, or N.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_FreeRangeIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.FreeRange)),
                "Free Range should be Y, N, or blank.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_GrassFedIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.GrassFed)),
                "Grass Fed should be Y, N, or blank.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_HiddenItemIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.HiddenItem)),
                "HiddenItem must be blank, Y, or N.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_MadeInHouseIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.MadeInHouse)),
                "Made In House should be Y, N, or blank.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_MscIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.Msc)),
                "MSC should be Y, N, or blank.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_PastureRaisedIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.PastureRaised)),
                "Pasture Raised should be Y, N, or blank.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_PremiumBodyCareIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.PremiumBodyCare)),
                "Premium Body Care should be Y, N, or blank.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_ValidatedIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.Validated)),
                "Validated should be Y, N, or blank.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_VegetarianIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.Vegetarian)),
                "Vegetarian should be Y, N, or blank.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_WholeTradeIsInvalid_ShouldSetError()
        {
            AssertYesNoTests(
                typeof(ItemExcelModel).GetProperty(nameof(testItem1.WholeTrade)),
                "Whole Trade should be Y, N, or blank.");
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_BrandIsInvalid_ShouldSetError()
        {
            //Given
            string error = "Brand is invalid.  {0} does not exist.";

            testItem1.Brand = "abc";
            testItem2.Brand = testTax.HierarchyClassId.ToString();
            testItem3.Brand = testBrand.HierarchyClassId.ToString();
            testItem4.Brand = "test|test||test|||test|" + testBrand.HierarchyClassId.ToString();
            testItem5.Brand = string.Empty;
            testItem6.Brand = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(string.Format(error, testItem1.Brand), testItem1.Error);
            Assert.AreEqual(string.Format(error, testTax.HierarchyClassId), testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_TaxIsInvalid_ShouldSetError()
        {
            //Given
            string error = "Tax is invalid.  {0} does not exist.";

            testItem1.Tax = "abc";
            testItem2.Tax = testBrand.HierarchyClassId.ToString();
            testItem3.Tax = testTax.HierarchyClassId.ToString();
            testItem4.Tax = "test|test||test|||test|" + testTax.HierarchyClassId.ToString();
            testItem5.Tax = string.Empty;
            testItem6.Tax = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(string.Format(error, testItem1.Tax), testItem1.Error);
            Assert.AreEqual(string.Format(error, testBrand.HierarchyClassId), testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_MerchandiseIsInvalid_ShouldSetError()
        {
            //Given
            string error = "Merchandise is invalid.  {0} does not exist.";

            testItem1.Merchandise = "abc";
            testItem2.Merchandise = testBrand.HierarchyClassId.ToString();
            testItem3.Merchandise = testMerchandise.HierarchyClassId.ToString();
            testItem4.Merchandise = "test|test||test|||test|" + testMerchandise.HierarchyClassId.ToString();
            testItem5.Merchandise = string.Empty;
            testItem6.Merchandise = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(string.Format(error, testItem1.Merchandise), testItem1.Error);
            Assert.AreEqual(string.Format(error, testBrand.HierarchyClassId), testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_BrowsingIsInvalid_ShouldSetError()
        {
            //Given
            string error = "Browsing is invalid.  {0} does not exist.";

            testItem1.Browsing = "abc";
            testItem2.Browsing = testBrand.HierarchyClassId.ToString();
            testItem3.Browsing = testBrowsing.HierarchyClassId.ToString();
            testItem4.Browsing = "test|test||test|||test|" + testBrowsing.HierarchyClassId.ToString();
            testItem5.Browsing = string.Empty;
            testItem6.Browsing = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(string.Format(error, testItem1.Browsing), testItem1.Error);
            Assert.AreEqual(string.Format(error, testBrand.HierarchyClassId), testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_NationalIsInvalid_ShouldSetError()
        {
            //Given
            string error = "National is invalid.  {0} does not exist.";

            testItem1.NationalClass = "abc";
            testItem2.NationalClass = testBrand.HierarchyClassId.ToString();
            testItem3.NationalClass = testNational.HierarchyClassId.ToString();
            testItem4.NationalClass = "test|test||test|||test|" + testNational.HierarchyClassId.ToString();
            testItem5.NationalClass = string.Empty;
            testItem6.NationalClass = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(string.Format(error, testItem1.NationalClass), testItem1.Error);
            Assert.AreEqual(string.Format(error, testBrand.HierarchyClassId), testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_AssociatedSubBrickHasAffinityTrait_ShouldSetError()
        {
            //Given
            string error = "Items cannot be associated to an Affinity sub-brick.";
            mockGetAffinitySubBricksQuery.Setup(m => m.Search(It.IsAny<GetAffinitySubBricksParameters>()))
                .Returns(new List<HierarchyClass> { new HierarchyClass { hierarchyClassID = testMerchandise.HierarchyClassId } });

            testItem1.Merchandise = "Test Merchandise|Test Merch|" + testMerchandise.HierarchyClassId.ToString();
            testItem2.Merchandise = "|" + testMerchandise.HierarchyClassId.ToString();
            testItem3.Merchandise = "|";
            testItem4.Merchandise = "test|test||test|||test|" + testMerchandise.HierarchyClassId.ToString();
            testItem5.Merchandise = "Test Merchandise|" + testMerchandise.HierarchyClassId.ToString();
            testItem6.Merchandise = string.Empty;

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(string.Format(error, testItem1.Merchandise), testItem1.Error);
            Assert.AreEqual(string.Format(error, testBrand.HierarchyClassId), testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.AreEqual(string.Format(error, testBrand.HierarchyClassId), testItem4.Error);
            Assert.AreEqual(string.Format(error, testBrand.HierarchyClassId), testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_ItemsAreNotReadyToBeValidated_ShouldSetError()
        {
            //Given
            string error = "Row is invalid. Row is marked to be validated but the item does not contain all required fields to be validated.";
            mockGetScanCodesNotReadyToValidateQuery.Setup(m => m.Search(It.IsAny<GetScanCodesNotReadyToValidateParameters>()))
                .Returns(new List<string> { testItem1.ScanCode, testItem5.ScanCode });

            testItem1.Validated = "Y";
            testItem2.Validated = "N";
            testItem3.Validated = "N";
            testItem4.Validated = "Y";
            testItem5.Validated = "Y";
            testItem6.Validated = "Y";

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.IsNull(testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.AreEqual(error, testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_ScanCodesDontExistInIcon_ShouldSetError()
        {
            //Given
            string error = "Scan Code does not exist in Icon.";
            mockGetNewScanCodeUploadsQuery.Setup(m => m.Search(It.IsAny<GetNewScanCodeUploadsParameters>()))
                .Returns(new List<ScanCodeModel>
                {
                    new ScanCodeModel { ScanCode = testItem1.ScanCode },
                    new ScanCodeModel { ScanCode = testItem2.ScanCode },
                    new ScanCodeModel { ScanCode = testItem3.ScanCode }
                });

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.AreEqual(error, testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.IsNull(testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        [TestMethod]
        public void ItemExcelValidatorFactory_ItemsHaveNoUpdatesSpecified_ShouldSetError()
        {
            //Given
            string error = "No fields are specified to update.";

            typeof(ItemExcelModel).GetProperties()
                .Where(p => p.IsDefined(typeof(ExcelColumnAttribute), false) && p.Name != nameof(testItem1.ScanCode))
                .ToList()
                .ForEach(p => 
                {
                    p.SetValue(testItem1, string.Empty);
                    p.SetValue(testItem2, string.Empty);
                    p.SetValue(testItem5, string.Empty);
                });

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.AreEqual(error, testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        private void AssertYesNoTests(PropertyInfo property, string error)
        {
            //Given
            property.SetValue(testItem1, "abc");
            property.SetValue(testItem2, "1");
            property.SetValue(testItem3, "Y");
            property.SetValue(testItem4, "N");
            property.SetValue(testItem5, "a");
            property.SetValue(testItem6, string.Empty);

            //When
            ValidateModels();

            //Then
            Assert.AreEqual(error, testItem1.Error);
            Assert.AreEqual(error, testItem2.Error);
            Assert.IsNull(testItem3.Error);
            Assert.IsNull(testItem4.Error);
            Assert.AreEqual(error, testItem5.Error);
            Assert.IsNull(testItem6.Error);
        }

        private void ValidateModels()
        {
            validators.ForEach(v => v.Validate(models.Where(m => string.IsNullOrWhiteSpace(m.Error))));
        }

        private List<ItemExcelModel> CreateTestItems()
        {

            testItem1 = new ItemExcelModel
            {
                ScanCode = "1234",
                Brand = testBrand.HierarchyClassName + "|" + testBrand.HierarchyClassId,
                ProductDescription = "Test",
                PosDescription = "Test",
                PackageUnit = "1",
                FoodStampEligible = "Y",
                PosScaleTare = "1",
                RetailSize = "1",
                Uom = UomCodes.Each,
                DeliverySystem = DeliverySystems.AsDictionary.Values.First(),
                Merchandise = testMerchandise.HierarchyClassName + "|" + testMerchandise.HierarchyClassId,
                NationalClass = testNational.HierarchyClassName + "|" + testNational.HierarchyClassId,
                Tax = testTax.HierarchyClassName + "|" + testTax.HierarchyClassId,
                Browsing = testBrowsing.HierarchyClassName + "|" + testBrowsing.HierarchyClassId,
                Validated = "Y",
                DepartmentSale = "Y",
                HiddenItem = "N",
                Notes = "Test",
                AnimalWelfareRating = AnimalWelfareRatings.Descriptions.Step1,
                Biodynamic = "Y",
                CheeseAttributeMilkType = MilkTypes.Descriptions.BuffaloMilk,
                CheeseAttributeRaw = "N",
                EcoScaleRating = EcoScaleRatings.Descriptions.BaselineOrange,
                GlutenFree = testGlutenFree.hierarchyClassName + "|" + testGlutenFree.hierarchyClassID,
                Kosher = testKosher.hierarchyClassName + "|" + testKosher.hierarchyClassID,
                Msc = "N",
                NonGmo = testNonGmo.hierarchyClassName + "|" + testNonGmo.hierarchyClassID,
                Organic = testOrganic.hierarchyClassName + "|" + testOrganic.hierarchyClassID,
                PremiumBodyCare = "Y",
                SeafoodFreshOrFrozen = SeafoodFreshOrFrozenTypes.Descriptions.Fresh,
                SeafoodWildOrFarmRaised = SeafoodCatchTypes.Descriptions.FarmRaised,
                Vegan = testVegan.hierarchyClassName + "|" + testVegan.hierarchyClassID,
                Vegetarian = "Y",
                WholeTrade = "Y",
                GrassFed = "Y",
                PastureRaised = "Y",
                FreeRange = "Y",
                DryAged = "Y",
                AirChilled = "Y",
                MadeInHouse = "Y",
                CreatedDate = DateTime.Now.ToString(),
                LastModifiedDate = DateTime.Now.ToString(),
                LastModifiedUser = "TestUser"
            };
            testItem2 = new ItemExcelModel
            {
                ScanCode = "12345",
                Brand = testBrand.HierarchyClassName + "|" + testBrand.HierarchyClassId,
                ProductDescription = "Test",
                PosDescription = "Test",
                PackageUnit = "1",
                FoodStampEligible = "Y",
                PosScaleTare = "1",
                RetailSize = "1",
                Uom = UomCodes.Each,
                DeliverySystem = DeliverySystems.AsDictionary.Values.First(),
                Merchandise = testMerchandise.HierarchyClassName + "|" + testMerchandise.HierarchyClassId,
                NationalClass = testNational.HierarchyClassName + "|" + testNational.HierarchyClassId,
                Tax = testTax.HierarchyClassName + "|" + testTax.HierarchyClassId,
                Browsing = testBrowsing.HierarchyClassName + "|" + testBrowsing.HierarchyClassId,
                Validated = "Y",
                DepartmentSale = "Y",
                HiddenItem = "N",
                Notes = "Test",
                AnimalWelfareRating = AnimalWelfareRatings.Descriptions.Step1,
                Biodynamic = "Y",
                CheeseAttributeMilkType = MilkTypes.Descriptions.BuffaloMilk,
                CheeseAttributeRaw = "N",
                EcoScaleRating = EcoScaleRatings.Descriptions.BaselineOrange,
                GlutenFree = testGlutenFree.hierarchyClassName + "|" + testGlutenFree.hierarchyClassID,
                Kosher = testKosher.hierarchyClassName + "|" + testKosher.hierarchyClassID,
                Msc = "N",
                NonGmo = testNonGmo.hierarchyClassName + "|" + testNonGmo.hierarchyClassID,
                Organic = testOrganic.hierarchyClassName + "|" + testOrganic.hierarchyClassID,
                PremiumBodyCare = "Y",
                SeafoodFreshOrFrozen = SeafoodFreshOrFrozenTypes.Descriptions.Fresh,
                SeafoodWildOrFarmRaised = SeafoodCatchTypes.Descriptions.FarmRaised,
                Vegan = testVegan.hierarchyClassName + "|" + testVegan.hierarchyClassID,
                Vegetarian = "Y",
                WholeTrade = "Y",
                GrassFed = "Y",
                PastureRaised = "Y",
                FreeRange = "Y",
                DryAged = "Y",
                AirChilled = "Y",
                MadeInHouse = "Y",
                CreatedDate = DateTime.Now.ToString(),
                LastModifiedDate = DateTime.Now.ToString(),
                LastModifiedUser = "TestUser"
            };
            testItem3 = new ItemExcelModel
            {
                ScanCode = "123456",
                Brand = testBrand.HierarchyClassName + "|" + testBrand.HierarchyClassId,
                ProductDescription = "Test",
                PosDescription = "Test",
                PackageUnit = "1",
                FoodStampEligible = "Y",
                PosScaleTare = "1",
                RetailSize = "1",
                Uom = UomCodes.Each,
                DeliverySystem = DeliverySystems.AsDictionary.Values.First(),
                Merchandise = testMerchandise.HierarchyClassName + "|" + testMerchandise.HierarchyClassId,
                NationalClass = testNational.HierarchyClassName + "|" + testNational.HierarchyClassId,
                Tax = testTax.HierarchyClassName + "|" + testTax.HierarchyClassId,
                Browsing = testBrowsing.HierarchyClassName + "|" + testBrowsing.HierarchyClassId,
                Validated = "Y",
                DepartmentSale = "Y",
                HiddenItem = "N",
                Notes = "Test",
                AnimalWelfareRating = AnimalWelfareRatings.Descriptions.Step1,
                Biodynamic = "Y",
                CheeseAttributeMilkType = MilkTypes.Descriptions.BuffaloMilk,
                CheeseAttributeRaw = "N",
                EcoScaleRating = EcoScaleRatings.Descriptions.BaselineOrange,
                GlutenFree = testGlutenFree.hierarchyClassName + "|" + testGlutenFree.hierarchyClassID,
                Kosher = testKosher.hierarchyClassName + "|" + testKosher.hierarchyClassID,
                Msc = "N",
                NonGmo = testNonGmo.hierarchyClassName + "|" + testNonGmo.hierarchyClassID,
                Organic = testOrganic.hierarchyClassName + "|" + testOrganic.hierarchyClassID,
                PremiumBodyCare = "Y",
                SeafoodFreshOrFrozen = SeafoodFreshOrFrozenTypes.Descriptions.Fresh,
                SeafoodWildOrFarmRaised = SeafoodCatchTypes.Descriptions.FarmRaised,
                Vegan = testVegan.hierarchyClassName + "|" + testVegan.hierarchyClassID,
                Vegetarian = "Y",
                WholeTrade = "Y",
                GrassFed = "Y",
                PastureRaised = "Y",
                FreeRange = "Y",
                DryAged = "Y",
                AirChilled = "Y",
                MadeInHouse = "Y",
                CreatedDate = DateTime.Now.ToString(),
                LastModifiedDate = DateTime.Now.ToString(),
                LastModifiedUser = "TestUser"
            };
            testItem4 = new ItemExcelModel
            {
                ScanCode = "1234567",
                Brand = testBrand.HierarchyClassName + "|" + testBrand.HierarchyClassId,
                ProductDescription = "Test",
                PosDescription = "Test",
                PackageUnit = "1",
                FoodStampEligible = "Y",
                PosScaleTare = "1",
                RetailSize = "1",
                Uom = UomCodes.Each,
                DeliverySystem = DeliverySystems.AsDictionary.Values.First(),
                Merchandise = testMerchandise.HierarchyClassName + "|" + testMerchandise.HierarchyClassId,
                NationalClass = testNational.HierarchyClassName + "|" + testNational.HierarchyClassId,
                Tax = testTax.HierarchyClassName + "|" + testTax.HierarchyClassId,
                Browsing = testBrowsing.HierarchyClassName + "|" + testBrowsing.HierarchyClassId,
                Validated = "Y",
                DepartmentSale = "Y",
                HiddenItem = "N",
                Notes = "Test",
                AnimalWelfareRating = AnimalWelfareRatings.Descriptions.Step1,
                Biodynamic = "Y",
                CheeseAttributeMilkType = MilkTypes.Descriptions.BuffaloMilk,
                CheeseAttributeRaw = "N",
                EcoScaleRating = EcoScaleRatings.Descriptions.BaselineOrange,
                GlutenFree = testGlutenFree.hierarchyClassName + "|" + testGlutenFree.hierarchyClassID,
                Kosher = testKosher.hierarchyClassName + "|" + testKosher.hierarchyClassID,
                Msc = "N",
                NonGmo = testNonGmo.hierarchyClassName + "|" + testNonGmo.hierarchyClassID,
                Organic = testOrganic.hierarchyClassName + "|" + testOrganic.hierarchyClassID,
                PremiumBodyCare = "Y",
                SeafoodFreshOrFrozen = SeafoodFreshOrFrozenTypes.Descriptions.Fresh,
                SeafoodWildOrFarmRaised = SeafoodCatchTypes.Descriptions.FarmRaised,
                Vegan = testVegan.hierarchyClassName + "|" + testVegan.hierarchyClassID,
                Vegetarian = "Y",
                WholeTrade = "Y",
                GrassFed = "Y",
                PastureRaised = "Y",
                FreeRange = "Y",
                DryAged = "Y",
                AirChilled = "Y",
                MadeInHouse = "Y",
                CreatedDate = DateTime.Now.ToString(),
                LastModifiedDate = DateTime.Now.ToString(),
                LastModifiedUser = "TestUser"
            };
            testItem5 = new ItemExcelModel
            {
                ScanCode = "12345678",
                Brand = testBrand.HierarchyClassName + "|" + testBrand.HierarchyClassId,
                ProductDescription = "Test",
                PosDescription = "Test",
                PackageUnit = "1",
                FoodStampEligible = "Y",
                PosScaleTare = "1",
                RetailSize = "1",
                Uom = UomCodes.Each,
                DeliverySystem = DeliverySystems.AsDictionary.Values.First(),
                Merchandise = testMerchandise.HierarchyClassName + "|" + testMerchandise.HierarchyClassId,
                NationalClass = testNational.HierarchyClassName + "|" + testNational.HierarchyClassId,
                Tax = testTax.HierarchyClassName + "|" + testTax.HierarchyClassId,
                Browsing = testBrowsing.HierarchyClassName + "|" + testBrowsing.HierarchyClassId,
                Validated = "Y",
                DepartmentSale = "Y",
                HiddenItem = "N",
                Notes = "Test",
                AnimalWelfareRating = AnimalWelfareRatings.Descriptions.Step1,
                Biodynamic = "Y",
                CheeseAttributeMilkType = MilkTypes.Descriptions.BuffaloMilk,
                CheeseAttributeRaw = "N",
                EcoScaleRating = EcoScaleRatings.Descriptions.BaselineOrange,
                GlutenFree = testGlutenFree.hierarchyClassName + "|" + testGlutenFree.hierarchyClassID,
                Kosher = testKosher.hierarchyClassName + "|" + testKosher.hierarchyClassID,
                Msc = "N",
                NonGmo = testNonGmo.hierarchyClassName + "|" + testNonGmo.hierarchyClassID,
                Organic = testOrganic.hierarchyClassName + "|" + testOrganic.hierarchyClassID,
                PremiumBodyCare = "Y",
                SeafoodFreshOrFrozen = SeafoodFreshOrFrozenTypes.Descriptions.Fresh,
                SeafoodWildOrFarmRaised = SeafoodCatchTypes.Descriptions.FarmRaised,
                Vegan = testVegan.hierarchyClassName + "|" + testVegan.hierarchyClassID,
                Vegetarian = "Y",
                WholeTrade = "Y",
                GrassFed = "Y",
                PastureRaised = "Y",
                FreeRange = "Y",
                DryAged = "Y",
                AirChilled = "Y",
                MadeInHouse = "Y",
                CreatedDate = DateTime.Now.ToString(),
                LastModifiedDate = DateTime.Now.ToString(),
                LastModifiedUser = "TestUser"
            };
            testItem6 = new ItemExcelModel
            {
                ScanCode = "123456789",
                Brand = testBrand.HierarchyClassName + "|" + testBrand.HierarchyClassId,
                ProductDescription = "Test",
                PosDescription = "Test",
                PackageUnit = "1",
                FoodStampEligible = "Y",
                PosScaleTare = "1",
                RetailSize = "1",
                Uom = UomCodes.Each,
                DeliverySystem = DeliverySystems.AsDictionary.Values.First(),
                Merchandise = testMerchandise.HierarchyClassName + "|" + testMerchandise.HierarchyClassId,
                NationalClass = testNational.HierarchyClassName + "|" + testNational.HierarchyClassId,
                Tax = testTax.HierarchyClassName + "|" + testTax.HierarchyClassId,
                Browsing = testBrowsing.HierarchyClassName + "|" + testBrowsing.HierarchyClassId,
                Validated = "Y",
                DepartmentSale = "Y",
                HiddenItem = "N",
                Notes = "Test",
                AnimalWelfareRating = AnimalWelfareRatings.Descriptions.Step1,
                Biodynamic = "Y",
                CheeseAttributeMilkType = MilkTypes.Descriptions.BuffaloMilk,
                CheeseAttributeRaw = "N",
                EcoScaleRating = EcoScaleRatings.Descriptions.BaselineOrange,
                GlutenFree = testGlutenFree.hierarchyClassName + "|" + testGlutenFree.hierarchyClassID,
                Kosher = testKosher.hierarchyClassName + "|" + testKosher.hierarchyClassID,
                Msc = "N",
                NonGmo = testNonGmo.hierarchyClassName + "|" + testNonGmo.hierarchyClassID,
                Organic = testOrganic.hierarchyClassName + "|" + testOrganic.hierarchyClassID,
                PremiumBodyCare = "Y",
                SeafoodFreshOrFrozen = SeafoodFreshOrFrozenTypes.Descriptions.Fresh,
                SeafoodWildOrFarmRaised = SeafoodCatchTypes.Descriptions.FarmRaised,
                Vegan = testVegan.hierarchyClassName + "|" + testVegan.hierarchyClassID,
                Vegetarian = "Y",
                WholeTrade = "Y",
                GrassFed = "Y",
                PastureRaised = "Y",
                FreeRange = "Y",
                DryAged = "Y",
                AirChilled = "Y",
                MadeInHouse = "Y",
                CreatedDate = DateTime.Now.ToString(),
                LastModifiedDate = DateTime.Now.ToString(),
                LastModifiedUser = "TestUser"
            };

            return new List<ItemExcelModel>()
            {
                testItem1,
                testItem2,
                testItem3,
                testItem4,
                testItem5,
                testItem6
            };
        }
    }
}
