using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Validators.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class AddItemManagerValidatorTests
    {
        private AddItemManagerValidator validator;
        private AddItemManager manager;
        private Mock<IQueryHandler<GetExistingScanCodeUploadsParameters, List<ScanCodeModel>>> mockGetExistingScanCodeUploadsQuery;
        private Mock<IQueryHandler<GetTaxClassesWithNoAbbreviationParameters, List<string>>> mockGetTaxClassesWithNoAbbreviationQuery;
        private Mock<IQueryHandler<GetDuplicateBrandsByTrimmedNameParameters, List<string>>> mockGetDuplicateBrandsByTrimmedNameQuery;

        [TestInitialize]
        public void Initialize()
        {
            mockGetExistingScanCodeUploadsQuery = new Mock<IQueryHandler<GetExistingScanCodeUploadsParameters, List<ScanCodeModel>>>();
            mockGetTaxClassesWithNoAbbreviationQuery = new Mock<IQueryHandler<GetTaxClassesWithNoAbbreviationParameters, List<string>>>();
            mockGetDuplicateBrandsByTrimmedNameQuery = new Mock<IQueryHandler<GetDuplicateBrandsByTrimmedNameParameters, List<string>>>();
            
            validator = new AddItemManagerValidator(mockGetExistingScanCodeUploadsQuery.Object,
                mockGetTaxClassesWithNoAbbreviationQuery.Object,
                mockGetDuplicateBrandsByTrimmedNameQuery.Object);

            manager = new AddItemManager
            {
                Item = new BulkImportNewItemModel
                {
                    ScanCode = "111",
                    BrandName = "TestBrand",
                    ProductDescription = "TestPosDes",
                    PosDescription = "TestProductDes",
                    FoodStampEligible = "1",
                    PackageUnit = "1",
                    PosScaleTare = "1",
                    RetailSize = "1",
                    RetailUom = "EA",
                    MerchandiseId = "1",
                    TaxId = "1",
                    NationalId = "1",
                    IsValidated = "1"
                }
            };
        }

        [TestMethod]
        public void Validate_ValidManager_ReturnsValidResult()
        {
            //Given
            mockGetExistingScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()))
                .Returns(new List<ScanCodeModel>());
            mockGetTaxClassesWithNoAbbreviationQuery.Setup(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()))
                .Returns(new List<string>());
            mockGetDuplicateBrandsByTrimmedNameQuery.Setup(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()))
                .Returns(new List<string>());
            
            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.Error);
        }

        [TestMethod]
        public void Validate_ScanCodeIsNull_ReturnsInvalidResult()
        {
            //Given
            manager.Item.ScanCode = null;

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Scan code is required and must be 13 or fewer numbers.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_ScanCodeIsInvalid_ReturnsInvalidResult()
        {
            //Given
            manager.Item.ScanCode = new string('1', 14);

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Scan code is required and must be 13 or fewer numbers.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_BrandNameIsNull_ReturnsInvalidResult()
        {
            //Given
            manager.Item.BrandName = null;

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Brand name is required: Please enter 35 or fewer valid characters.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_BrandNameIsInvalid_ReturnsInvalidResult()
        {
            //Given
            manager.Item.BrandName = new String('b', 36);

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Brand name is required: Please enter 35 or fewer valid characters.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_ProductDescriptionIsNull_ReturnsInvalidResult()
        {
            //Given
            manager.Item.ProductDescription = null;

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Product description is required: Please enter 60 or fewer valid characters.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_ProductDescriptionIsInvalid_ReturnsInvalidResult()
        {
            //Given
            manager.Item.ProductDescription = new String('b', 61);

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Product description is required: Please enter 60 or fewer valid characters.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_PosDescriptionIsNull_ReturnsInvalidResult()
        {
            //Given
            manager.Item.PosDescription = null;

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("POS description is required: Please enter 25 or fewer valid characters.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_PosDescriptionIsInvalid_ReturnsInvalidResult()
        {
            //Given
            manager.Item.PosDescription = new String('b', 26);

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("POS description is required: Please enter 25 or fewer valid characters.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_PackageUnitIsInvalid_ReturnsInvalidResult()
        {
            //Given
            manager.Item.PackageUnit = "11111";

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Item Pack must be a whole number with 3 or fewer digits.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_PosScaleTareIsInvalid_ReturnsInvalidResult()
        {
            //Given
            manager.Item.PosScaleTare = "11";

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("POS scale tare must be a valid decimal value less than 10 with no more than 4 digits plus the decimal point.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_RetailSizeIsInvalid_ReturnsInvalidResult()
        {
            //Given
            manager.Item.RetailSize = "111111111";

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Size must be greater than zero and contain only numbers and decimal points with 5 or less digits to the left of the decimal and 4 or less digits to the right of the decimal.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_RetailUom_ReturnsInvalidResult()
        {
            //Given
            manager.Item.RetailUom = "----";

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("UOM should be one of the following: EA, LB, CT, OZ, CS, PK, LT, PT, KG, ML, GL, GR, CG, FT, YD, QT, SQFT, MT, FZ.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_ValidatingAndMerchHierarchyClassIdIsNull_ReturnsInvalidResult()
        {
            //Given
            manager.Item.MerchandiseId = null;

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Merchandise hierarchy class is required for validated items.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_ValidatingAndMerchHierarchyClassIdIsLessThan1_ReturnsInvalidResult()
        {
            //Given
            manager.Item.MerchandiseId = "0";

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Merchandise hierarchy class is required for validated items.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_ValidatingAndTaxHierarchyClassIdIsNull_ReturnsInvalidResult()
        {
            //Given
            manager.Item.TaxId = null;

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Tax hierarchy class is required for validated items.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_ValidatingAndTaxHierarchyClassIdIsLessThanl_ReturnsInvalidResult()
        {
            //Given
            manager.Item.TaxId = "0";

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Tax hierarchy class is required for validated items.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_ScanCodeExists_ReturnsInvalidResult()
        {
            //Given
            mockGetExistingScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()))
                .Returns(new List<ScanCodeModel>() { new ScanCodeModel { ScanCode = manager.Item.ScanCode } });

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Scan code 111 already exists.", result.Error);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_TaxClassDoesNotHaveAbbreviation_ReturnsInvalidResult()
        {
            //Given
            mockGetExistingScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()))
                .Returns(new List<ScanCodeModel>());
            mockGetTaxClassesWithNoAbbreviationQuery.Setup(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()))
                .Returns(new List<string> { "1" });

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Tax Hierarchy Class 1 does not have a Tax Abbreviation.  Cannot associate items to Tax Hierarchy Classes with no Tax Abbreviation.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Once);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Once);

        }

        [TestMethod]
        public void Validate_BrandNameIsLargerThanIrmaBrandLimitAndThereIsABrandWithTheSameNameAsTheBrandNameShortenedToTheLengthOfTheIrmaBrandLimit_ReturnsInvalidResult()
        {
            //Given
            mockGetDuplicateBrandsByTrimmedNameQuery.Setup(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()))
                .Returns(new List<string> 
                {
                    new String('b', 25)
                });
            mockGetExistingScanCodeUploadsQuery.Setup(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()))
                .Returns(new List<ScanCodeModel>());
            mockGetTaxClassesWithNoAbbreviationQuery.Setup(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()))
                .Returns(new List<string>());
            manager.Item.BrandName = new String('b', 26);

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(String.Format("Brand name {0} when shortened to {1} characters is {2} which is an already existing Brand in Icon and may cause conflicts with IRMA.", 
                    manager.Item.BrandName, 
                    Constants.IrmaBrandNameMaxLength, 
                    manager.Item.BrandName.Substring(0, Constants.IrmaBrandNameMaxLength)), 
                result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Once);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Once);
        }

        [TestMethod]
        public void Validate_ValidatingAndNationalHierarchyClassIdIsLessThan1_ReturnsInvalidResult()
        {
            //Given
            manager.Item.NationalId = "0";

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("National hierarchy class is required for validated items.", result.Error);
            mockGetExistingScanCodeUploadsQuery.Verify(q => q.Search(It.IsAny<GetExistingScanCodeUploadsParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
            mockGetDuplicateBrandsByTrimmedNameQuery.Verify(q => q.Search(It.IsAny<GetDuplicateBrandsByTrimmedNameParameters>()), Times.Never);
        }
    }
}
