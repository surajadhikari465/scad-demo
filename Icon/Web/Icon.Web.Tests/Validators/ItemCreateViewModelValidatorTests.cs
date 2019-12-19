using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Icon.Common;
using Icon.Common.Validators.ItemAttributes;
using Icon.Framework;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class ItemCreateViewModelValidatorTests
    {
        private IconContext context;
        private ItemCreateViewModelValidator validator;
        private Mock<IQueryHandler<DoesScanCodeExistParameters, bool>> mockDoesScanCodeExistQueryHandler;
        private Mock<IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>>> mockGetBarcodeTypeQueryHandler;
        private Mock<IItemAttributesValidatorFactory> mockFactory;
        private ItemCreateViewModel itemCreateViewModel;

        [TestInitialize]
        public void Initialize()
        {
            mockDoesScanCodeExistQueryHandler = new Mock<IQueryHandler<DoesScanCodeExistParameters, bool>>();
            mockGetBarcodeTypeQueryHandler = new Mock<IQueryHandler<GetBarcodeTypeParameters, List<BarcodeTypeModel>>>();
            mockFactory = new Mock<IItemAttributesValidatorFactory>();

            mockGetBarcodeTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetBarcodeTypeParameters>()))
                 .Returns(getBarcodeTypeModels());

            validator = new ItemCreateViewModelValidator(
                mockDoesScanCodeExistQueryHandler.Object,
                mockGetBarcodeTypeQueryHandler.Object,
                mockFactory.Object);

            itemCreateViewModel = new ItemCreateViewModel
            {
                BrandHierarchyClassId = 1,
                MerchandiseHierarchyClassId = 1,
                NationalHierarchyClassId = 1,
                TaxHierarchyClassId = 1
            };
        }

        [TestMethod]
        public void Validate_BarcodeIsUpcAndScanCodeNotSet_ScanCodeRequired()
        {
            //Given
            itemCreateViewModel.BarcodeTypeId = getUpcBarcodeTypeId();

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual("Scan Code is required when choosing UPC.", result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_BarcodeAndScanCodeExistsAlready_ScanCodeExistsError()
        {
            //Given
            itemCreateViewModel.BarcodeTypeId = getUpcBarcodeTypeId();
            itemCreateViewModel.ScanCode = "1234";
            mockDoesScanCodeExistQueryHandler.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(true);
            mockGetBarcodeTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetBarcodeTypeParameters>()))
                .Returns(new List<BarcodeTypeModel>());

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "'1234' is already associated to an item. Please use another scan code.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate__BarcodeAndScanCodeExistsInBarcodeTypeRanges_ScanCodeExistsInBarcodeTypeRangesError()
        {
            //Given
            itemCreateViewModel.BarcodeTypeId = getUpcBarcodeTypeId();
            itemCreateViewModel.ScanCode = "1234";
            mockDoesScanCodeExistQueryHandler.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(false);
            mockGetBarcodeTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetBarcodeTypeParameters>()))
                .Returns(new List<BarcodeTypeModel>
                {
                    new BarcodeTypeModel { BeginRange = "9999999999", EndRange = "999999999999" },
                    new BarcodeTypeModel { BeginRange = "1", EndRange = "10" },
                    new BarcodeTypeModel { BeginRange = "1000", EndRange = "2000" },
                    new BarcodeTypeModel { BeginRange = "1000000", EndRange = "1000001" },
                });

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "'1234' exists in a Barcode Type range. Please enter a scan code not within a Barcode Type range.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ScanCodeDoesStartsWith0_ScanCodeRegexNoMatchError()
        {
            //Given
            itemCreateViewModel.BarcodeTypeId = getUpcBarcodeTypeId();
            itemCreateViewModel.ScanCode = "0123";
            mockDoesScanCodeExistQueryHandler.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(false);
            mockGetBarcodeTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetBarcodeTypeParameters>()))
                .Returns(new List<BarcodeTypeModel>());

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "Scan Code must contain only digits, not start with a 0, and must be 1 to 13 characters long.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ScanCodeDoesContainsNonDigitCharacter_ScanCodeRegexNoMatchError()
        {
            //Given
            itemCreateViewModel.BarcodeTypeId = getUpcBarcodeTypeId();
            itemCreateViewModel.ScanCode = "1234a123";
            mockDoesScanCodeExistQueryHandler.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(false);
            mockGetBarcodeTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetBarcodeTypeParameters>()))
                .Returns(new List<BarcodeTypeModel>());

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "Scan Code must contain only digits, not start with a 0, and must be 1 to 13 characters long.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ScanCodeDoesIsNegativeNumber_ScanCodeRegexNoMatchError()
        {
            //Given
            itemCreateViewModel.BarcodeTypeId = getUpcBarcodeTypeId();
            itemCreateViewModel.ScanCode = "-1234";
            mockDoesScanCodeExistQueryHandler.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(false);
            mockGetBarcodeTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetBarcodeTypeParameters>()))
                .Returns(new List<BarcodeTypeModel>());

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "Scan Code must contain only digits, not start with a 0, and must be 1 to 13 characters long.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ScanCodeDoesIsGreaterThan13CharactersLong_ScanCodeRegexNoMatchError()
        {
            //Given
            itemCreateViewModel.BarcodeTypeId = getUpcBarcodeTypeId();
            itemCreateViewModel.ScanCode = new string('1', 14);
            mockDoesScanCodeExistQueryHandler.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(false);
            mockGetBarcodeTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetBarcodeTypeParameters>()))
                .Returns(new List<BarcodeTypeModel>());

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "Scan Code must contain only digits, not start with a 0, and must be 1 to 13 characters long.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ScanCodeDoesIs13CharactersLong_NoError()
        {
            //Given
            itemCreateViewModel.BarcodeTypeId = getUpcBarcodeTypeId();
            itemCreateViewModel.ScanCode = new string('1', 13);
            mockDoesScanCodeExistQueryHandler.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(false);
            mockGetBarcodeTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetBarcodeTypeParameters>()))
                .Returns(new List<BarcodeTypeModel>());

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ScanCodeDoesIs1CharactersLong_NoError()
        {
            //Given
            itemCreateViewModel.BarcodeTypeId = getUpcBarcodeTypeId();
            itemCreateViewModel.ScanCode = new string('1', 1);
            mockDoesScanCodeExistQueryHandler.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(false);
            mockGetBarcodeTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetBarcodeTypeParameters>()))
                .Returns(new List<BarcodeTypeModel>());

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate__BarcodeIsPluAndBarcodeTypeIsNotSelected_BarcodeTypeRequiredError()
        {
            //Given
            itemCreateViewModel.ScanCodeType = Constants.Plu;
            itemCreateViewModel.BarcodeTypeId = 0;

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "Barcode Type is required.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate__BarcodeIsPluAndBarcodeTypeIsSelected_NoError()
        {
            //Given
            itemCreateViewModel.ScanCodeType = Constants.Plu;
            itemCreateViewModel.BarcodeTypeId = 1;

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_BrandIsNotSelected_BrandRequiredError()
        {
            //Given
            itemCreateViewModel.BrandHierarchyClassId = 0;
            itemCreateViewModel.ScanCodeType = Constants.Plu;
            itemCreateViewModel.BarcodeTypeId = 1;

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "Brand is required. Please select a Brand.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_MerchandiseIsNotSelected_MerchandiseRequiredError()
        {
            //Given
            itemCreateViewModel.MerchandiseHierarchyClassId = 0;
            itemCreateViewModel.ScanCodeType = Constants.Plu;
            itemCreateViewModel.BarcodeTypeId = 1;

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "Merchandise is required. Please select a Merchandise.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_TaxIsNotSelected_TaxRequiredError()
        {
            //Given
            itemCreateViewModel.TaxHierarchyClassId = 0;
            itemCreateViewModel.ScanCodeType = Constants.Plu;
            itemCreateViewModel.BarcodeTypeId = 1;

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "Tax is required. Please select a Tax.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_NationalIsNotSelected_NationalRequiredError()
        {
            //Given
            itemCreateViewModel.NationalHierarchyClassId = 0;
            itemCreateViewModel.ScanCodeType = Constants.Plu;
            itemCreateViewModel.BarcodeTypeId = 1;

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "National is required. Please select a National.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_NoHierarchyClassesSelectedAndScanCodeTypeIsUpcAndScanCodeNotSet_MultipleErrorsReturned()
        {
            //Given
            itemCreateViewModel = new ItemCreateViewModel
            {
                ScanCodeType = Constants.Upc,
                ScanCode = ""
            };
            itemCreateViewModel.BarcodeTypeId = getUpcBarcodeTypeId();

            mockDoesScanCodeExistQueryHandler.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(false);
            mockGetBarcodeTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetBarcodeTypeParameters>()))
                .Returns(new List<BarcodeTypeModel>());

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(6, result.Errors.Count);
            Assert.IsNotNull(result.Errors
                .SingleOrDefault(e => e.ErrorMessage == "Scan Code is required when choosing UPC."));
            Assert.IsNotNull(result.Errors
                .SingleOrDefault(e => e.ErrorMessage == "Brand is required. Please select a Brand."));
            Assert.IsNotNull(result.Errors
                .SingleOrDefault(e => e.ErrorMessage == "Tax is required. Please select a Tax."));
            Assert.IsNotNull(result.Errors
                .SingleOrDefault(e => e.ErrorMessage == "Merchandise is required. Please select a Merchandise."));
            Assert.IsNotNull(result.Errors
                .SingleOrDefault(e => e.ErrorMessage == "National is required. Please select a National."));
            Assert.IsNotNull(result.Errors
                .SingleOrDefault(e => e.ErrorMessage == "Scan Code must contain only digits, not start with a 0, and must be 1 to 13 characters long."));
        }

        [TestMethod]
        public void Validate_ItemAttributesExist_ShouldValidateEachItemAttribute()
        {
            //Given
            Mock<IItemAttributesValidator> mockValidator = new Mock<IItemAttributesValidator>();
            mockValidator.SetupSequence(m => m.Validate(It.IsAny<string>()))
                .Returns(new ItemAttributesValidationResult { IsValid = true })
                .Returns(new ItemAttributesValidationResult { IsValid = false, ErrorMessages = new List<string> { "Error1", "Error2" } })
                .Returns(new ItemAttributesValidationResult { IsValid = true });
            mockFactory.Setup(m => m.CreateItemAttributesJsonValidator(It.IsAny<string>()))
                .Returns(mockValidator.Object);
            itemCreateViewModel.ItemAttributes = new Dictionary<string, string>
            {
                {"test1", "test" },
                {"test2", "test" },
                {"test3", "test" }
            };

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.IsNotNull(result.Errors
                .SingleOrDefault(e => e.ErrorMessage == "Error1"));
            Assert.IsNotNull(result.Errors
                .SingleOrDefault(e => e.ErrorMessage == "Error2"));
        }

        [TestMethod]
        public void Validate_BarcodeIsNotUpcScanCodeDoesIsNegativeNumber_ScanCodeRegexNoMatchError()
        {
            //Given
            itemCreateViewModel.BarcodeTypeId = getAnyBarcodeTypeIdOtherThanUpc();
            itemCreateViewModel.ScanCode = "-1234";
            mockDoesScanCodeExistQueryHandler.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(false);
            mockGetBarcodeTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetBarcodeTypeParameters>()))
                .Returns(new List<BarcodeTypeModel>());

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "Scan Code must contain only digits, not start with a 0, and must be 1 to 13 characters long.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_BarcodeIsNotUpcScanCodeDoesStartsWith0_ScanCodeRegexNoMatchError()
        {
            //Given
            itemCreateViewModel.BarcodeTypeId = getAnyBarcodeTypeIdOtherThanUpc();
            itemCreateViewModel.ScanCode = "0123";
            mockDoesScanCodeExistQueryHandler.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(false);
            mockGetBarcodeTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetBarcodeTypeParameters>()))
                .Returns(new List<BarcodeTypeModel>());

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "Scan Code must contain only digits, not start with a 0, and must be 1 to 13 characters long.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_BarcodeTypeNotUpcScanCodeExistsAlready_ScanCodeExistsError()
        {
            //Given
            itemCreateViewModel.BarcodeTypeId = getAnyBarcodeTypeIdOtherThanUpc();
            itemCreateViewModel.ScanCode = "1234";
            mockDoesScanCodeExistQueryHandler.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(true);
            mockGetBarcodeTypeQueryHandler.Setup(m => m.Search(It.IsAny<GetBarcodeTypeParameters>()))
                .Returns(new List<BarcodeTypeModel>());

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "'1234' is already associated to an item. Please use another scan code.",
                result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate__BarcodeTypeIsNotUpcAndScanCodeIsNotInSelectedBarcodeTypeRanges_ScanCodeNotInBarcodeTypeRangeError()
        {
            //Given
            BarcodeTypeModel barcodeTypeModel = getAnyBarcodeTypeModelOtherThanUpc();
            itemCreateViewModel.BarcodeTypeId = barcodeTypeModel.BarcodeTypeId;
            itemCreateViewModel.ScanCode = barcodeTypeModel.EndRange + 1;
            mockDoesScanCodeExistQueryHandler.Setup(m => m.Search(It.IsAny<DoesScanCodeExistParameters>()))
                .Returns(false);

            //When
            var result = validator.Validate(itemCreateViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(
               "'"+  itemCreateViewModel.ScanCode + "'"+  " should be in selected Barcode Type range. Please enter a scan code within selected Barcode Type range.",
                result.Errors.First().ErrorMessage);
        }

        private int getUpcBarcodeTypeId()
        {
            context = new IconContext();
            return context.Database.SqlQuery<int>("select BarcodeTypeId from BarcodeType where BarcodeType='UPC'").First();
        }

        private int getAnyBarcodeTypeIdOtherThanUpc()
        {
            context = new IconContext();
            return context.Database.SqlQuery<int>("select BarcodeTypeId from BarcodeType where BarcodeType !='UPC'").First();
        }

        private BarcodeTypeModel getAnyBarcodeTypeModelOtherThanUpc()
        {
            context = new IconContext();
            return context.Database.SqlQuery<BarcodeTypeModel>("select * from BarcodeType where BarcodeType !='UPC'").First();
        }

        private List<BarcodeTypeModel> getBarcodeTypeModels()
        {
            context = new IconContext();
            return context.Database.SqlQuery<BarcodeTypeModel>("select * from BarcodeType").ToList();
        }
    }
}
