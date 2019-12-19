using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Icon.Common.Validators.ItemAttributes;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class ItemEditViewModelValidatorTests
    {
        private ItemEditViewModelValidator validator;
        private Mock<IItemAttributesValidatorFactory> mockFactory;
        private ItemEditViewModel itemEditViewModel;
        private ItemViewModel itemViewModel;

        [TestInitialize]
        public void Initialize()
        {
            mockFactory = new Mock<IItemAttributesValidatorFactory>();

            validator = new ItemEditViewModelValidator(mockFactory.Object);

            itemViewModel = new ItemViewModel
            {
                BrandsHierarchyClassId = 1,
                MerchandiseHierarchyClassId = 1,
                NationalHierarchyClassId = 1,
                TaxHierarchyClassId = 1
            };
            itemEditViewModel = new ItemEditViewModel
            {
                ItemViewModel = itemViewModel
            };
        }

        [TestMethod]
        public void Validate_BrandIsNotSelected_BrandRequiredError()
        {
            //Given
            itemViewModel.BrandsHierarchyClassId = 0;

            //When
            var result = validator.Validate(itemEditViewModel);

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
            itemViewModel.MerchandiseHierarchyClassId = 0;

            //When
            var result = validator.Validate(itemEditViewModel);

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
            itemViewModel.TaxHierarchyClassId = 0;

            //When
            var result = validator.Validate(itemEditViewModel);

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
            itemViewModel.NationalHierarchyClassId = 0;

            //When
            var result = validator.Validate(itemEditViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                "National is required. Please select a National.",
                result.Errors.First().ErrorMessage);
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
            itemViewModel.ItemAttributes = new Dictionary<string, string>
            {
                {"test1", "test" },
                {"test2", "test" },
                {"test3", "test" }
            };

            //When
            var result = validator.Validate(itemEditViewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.IsNotNull(result.Errors
                .SingleOrDefault(e => e.ErrorMessage == "Error1"));
            Assert.IsNotNull(result.Errors
                .SingleOrDefault(e => e.ErrorMessage == "Error2"));
        }
    }
}
