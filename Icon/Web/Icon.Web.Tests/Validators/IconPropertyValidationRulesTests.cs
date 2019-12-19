using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Icon.Common.Validators;
using Icon.Web.Common;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass] [Ignore]
    public class IconPropertyValidationRulesTests
    {
        private IconPropertyValidationRules validator;

        [TestInitialize]
        public void IntializeProdDescriptionValidator()
        {
            
        }
        [TestMethod]
        public void IconPropertyValidationRules_ProductDescription_ShouldValidate()
        {
            // Given.
            string productDescription = "Test Product Description";
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.ProductDescription);

            // When.
            bool result = validator.IsValid(productDescription, null);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IconPropertyValidationRules_InvalidProductDescriptionLength_ShouldNotValidate()
        {
            // Given.
            string tooLongDescription = new string('k', 61);
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.ProductDescription);

            // When.
            bool result = validator.IsValid(tooLongDescription, null);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IconPropertyValidationRules_NullProductDescription_ShouldNotValidate()
        {
            // Given.
            string productDescription = null;
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.ProductDescription);
            
            // When.
            bool result = validator.IsValid(productDescription, null);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IconPropertyValidationRules_EmptyProductDescription_ShouldValidate()
        {
            // Given.
            string productDescription = String.Empty;
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.ProductDescription);

            // When.
            bool result = validator.IsValid(productDescription, null);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IconPropertyValidationRules_ProdcutDescription_InvalidCharacter_ShouldNotValidate()
        {
            // Given.
            string productDescription = "Test+Test";
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.ProductDescription);

            // When.
            bool result = validator.IsValid(productDescription, null);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IconPropertyValidationRules_ProdcutDescription_ValidCharacter_ShouldNotValidate()
        {
            // Given.
            string productDescription = "Test&Test";
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.ProductDescription);

            // When.
            bool result = validator.IsValid(productDescription, null);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IconPropertyValidationRules_InvalidPosDescriptionLength_ShouldNotValidate()
        {
            // Given.
            string tooLongDescription = new string('k', 26);
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.PosDescription);

            // When.
            bool result = validator.IsValid(tooLongDescription, null);

            // Then.
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IconPropertyValidationRules_PosDescription_InvalidCharacter_ShouldNotValidate()
        {
            // Given.
            string posDescription = "Test+Test";
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.PosDescription);

            // When.
            bool result = validator.IsValid(posDescription, null);

            // Then.
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IconPropertyValidationRules_PosDescription_ValidCharacter_ShouldNotValidate()
        {
            // Given.
            string posDescription = "Test&Test";
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.PosDescription);
            // When.
            bool result = validator.IsValid(posDescription, null);

            // Then.
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void IconPropertyValidationRules_InvalidBrandNameLength_ShouldNotValidate()
        {
            // Given.
            string tooLongDescription = new string('k', 36);
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.BrandName);

            // When.
            bool result = validator.IsValid(tooLongDescription, null);

            // Then.
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IconPropertyValidationRules_BrandName_InvalidCharacter_ShouldNotValidate()
        {
            // Given.
            string brandName = "Test+Test";
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.BrandName);

            // When.
            bool result = validator.IsValid(brandName, null);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IconPropertyValidationRules_Brand_ValidCharacter_ShouldValidate()
        {
            // Given.
            string brandName = "Test&Test";
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.BrandName);

            // When.
            bool result = validator.IsValid(brandName, null);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IconPropertyValidationRules_CreatedDate_ValidCharacter_ShouldNotValidate()
        {
            // Given.
            string createdDate = "09092012";
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.CreatedDate);

            // When.
            bool result = validator.IsValid(createdDate, null);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IconPropertyValidationRules_CreatedDate_ValidCharacter_ShouldValidate()
        {
            // Given.
            string createdDate = "09/30/2012";
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.CreatedDate);

            // When.
            bool result = validator.IsValid(createdDate, null);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IconPropertyValidationRules_ModifiedDate_ValidCharacter_ShouldNotValidate()
        {
            // Given.
            string modifiedDate = "09092012";
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.CreatedDate);

            // When.
            bool result = validator.IsValid(modifiedDate, null);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IconPropertyValidationRules_ModifiedDate_ValidCharacter_ShouldValidate()
        {
            // Given.
            string modifiedDate = "09/30/2012";
            validator = IconPropertyValidationRules.GetIconPropertyValidationRules(ValidatorPropertyNames.CreatedDate);

            // When.
            bool result = validator.IsValid(modifiedDate, null);

            // Then.
            Assert.IsTrue(result);
        }
    }
}
