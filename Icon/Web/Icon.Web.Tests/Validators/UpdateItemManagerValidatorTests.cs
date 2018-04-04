using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Testing.Builders;
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
    [TestClass] [Ignore]
    public class UpdateItemManagerValidatorTests
    {
        private UpdateItemManagerValidator validator;
        private UpdateItemManager manager;
        private Mock<IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>>> mockGetItemsByBulkScanCodeSearchQuery;
        private Mock<IQueryHandler<GetTaxClassesWithNoAbbreviationParameters, List<string>>> mockGetTaxClassesWithNoAbbreviationQuery;

        [TestInitialize]
        public void Initialize()
        {
            mockGetItemsByBulkScanCodeSearchQuery = new Mock<IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>>>();
            mockGetTaxClassesWithNoAbbreviationQuery = new Mock<IQueryHandler<GetTaxClassesWithNoAbbreviationParameters, List<string>>>();

            validator = new UpdateItemManagerValidator(mockGetItemsByBulkScanCodeSearchQuery.Object,
                mockGetTaxClassesWithNoAbbreviationQuery.Object);

            manager = new UpdateItemManager
            {
                Item = new BulkImportItemModel
                {
                    ItemId = 1,
                    ScanCode = "111",
                    BrandId = "1",
                    ProductDescription = "TestProductDes",
                    PosDescription = "TestPosDes",
                    PackageUnit = "1",
                    PosScaleTare = "1",
                    RetailSize = "1",
                    RetailUom = "EA",
                    MerchandiseId = "1",
                    TaxId = "1",
                    NationalId = "1",
                    Notes = "Test note."
                }
            };
        }

        [TestMethod]
        public void Validate_ValidManager_ReturnsValidResult()
        {
            //Given
            mockGetItemsByBulkScanCodeSearchQuery.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()))
                .Returns(new List<ItemSearchModel> { new ItemSearchModel { ValidatedDate = DateTime.Now.ToString() } });
            mockGetTaxClassesWithNoAbbreviationQuery.Setup(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()))
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
            // Given.
            manager.Item.ScanCode = null;

            // When.
            var result = validator.Validate(manager);

            // Then.
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(String.Format("Scan code is required and must be {0} or fewer numbers.", Constants.ScanCodeMaxLength), result.Error);
            mockGetItemsByBulkScanCodeSearchQuery.Verify(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_ScanCodeIsInvalid_ReturnsInvalidResult()
        {
            // Given.
            manager.Item.ScanCode = new string('1', 14);

            // When.
            var result = validator.Validate(manager);

            // Then.
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(String.Format("Scan code is required and must be {0} or fewer numbers.", Constants.ScanCodeMaxLength), result.Error);
            mockGetItemsByBulkScanCodeSearchQuery.Verify(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
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
            mockGetItemsByBulkScanCodeSearchQuery.Verify(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
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
            mockGetItemsByBulkScanCodeSearchQuery.Verify(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
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
            mockGetItemsByBulkScanCodeSearchQuery.Verify(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
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
            mockGetItemsByBulkScanCodeSearchQuery.Verify(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
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
            Assert.AreEqual("Item Pack must be a whole number with three or fewer digits.", result.Error);
            mockGetItemsByBulkScanCodeSearchQuery.Verify(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
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
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
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
            mockGetItemsByBulkScanCodeSearchQuery.Verify(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
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
            mockGetItemsByBulkScanCodeSearchQuery.Verify(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_ValidatedAndMerchHierarchyClassIdIsNull_ReturnsInvalidResult()
        {
            //Given
            mockGetItemsByBulkScanCodeSearchQuery.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()))
                .Returns(new List<ItemSearchModel> { new ItemSearchModel { ValidatedDate = DateTime.Now.ToString() } });
            manager.Item.MerchandiseId = null;

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Merchandise hierarchy class is required for validated items.", result.Error);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_ValidatedAndMerchHierarchyClassIdIsLessThanOne_ReturnsInvalidResult()
        {
            //Given
            mockGetItemsByBulkScanCodeSearchQuery.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()))
                .Returns(new List<ItemSearchModel> { new ItemSearchModel { ValidatedDate = DateTime.Now.ToString() } });
            manager.Item.MerchandiseId = "0";

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Merchandise hierarchy class is required for validated items.", result.Error); ;
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_ValidatedAndTaxHierarchyClassIdIsNull_ReturnsInvalidResult()
        {
            //Given
            mockGetItemsByBulkScanCodeSearchQuery.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()))
                .Returns(new List<ItemSearchModel> { new ItemSearchModel { ValidatedDate = DateTime.Now.ToString() } });
            manager.Item.TaxId = null;

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Tax hierarchy class is required for validated items.", result.Error);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_ValidatedAndTaxHierarchyClassIdIsLessThanl_ReturnsInvalidResult()
        {
            // Given.
            mockGetItemsByBulkScanCodeSearchQuery.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()))
                .Returns(new List<ItemSearchModel> { new ItemSearchModel { ValidatedDate = DateTime.Now.ToString() } });
            manager.Item.TaxId = "0";

            // When.
            var result = validator.Validate(manager);

            // Then.
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Tax hierarchy class is required for validated items.", result.Error);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
        }

        [TestMethod]
        public void Validate_TaxClassDoesNotHaveAbbreviation_ReturnsInvalidResult()
        {
            // Given.
            mockGetItemsByBulkScanCodeSearchQuery.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()))
                .Returns(new List<ItemSearchModel> { new ItemSearchModel { ValidatedDate = DateTime.Now.ToString() } });
            mockGetTaxClassesWithNoAbbreviationQuery.Setup(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>())).Returns(new List<string> { "1" });

            // When.
            var result = validator.Validate(manager);

            // Then.
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Tax Hierarchy Class 1 does not have a Tax Abbreviation.  Cannot associate items to Tax Hierarchy Classes with no Tax Abbreviation.", result.Error);
            mockGetItemsByBulkScanCodeSearchQuery.Verify(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()), Times.Once);
        }

        [TestMethod]
        public void Validate_LongNotes_ReturnsInvalidNotesResult()
        {
            //Given
            manager.Item.Notes = new string('k', 300);

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Notes should be 255 or fewer valid characters.", result.Error);
            mockGetItemsByBulkScanCodeSearchQuery.Verify(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()), Times.Never);
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
        }


        [TestMethod]
        public void Validate_NationalClassIdIsNullAndItemIsValidated_ReturnsInvalidResult()
        {
            //Given
            mockGetItemsByBulkScanCodeSearchQuery.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>()))
                .Returns(new List<ItemSearchModel> { new ItemSearchModel { ValidatedDate = DateTime.Now.ToString() } });
            manager.Item.NationalId = "0";

            //When
            var result = validator.Validate(manager);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("National hierarchy class is required for validated items.", result.Error); ;
            mockGetTaxClassesWithNoAbbreviationQuery.Verify(q => q.Search(It.IsAny<GetTaxClassesWithNoAbbreviationParameters>()), Times.Never);
        }
    }
}
