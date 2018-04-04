using Icon.Web.Common.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass] [Ignore]
    public class ProductDescriptionValidatorTests
    {
        [TestMethod]
        public void ProductDescriptionValidator_ValidProductDescription_ShouldValidate()
        {
            // Given.
            string productDescription = "Test Product Description";

            ProductDescriptionValidator validator = new ProductDescriptionValidator();

            // When.
            bool result = validator.Validate(productDescription);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProductDescriptionValidator_InvalidProductDescriptionLength_ShouldNotValidate()
        {
            // Given.
            string tooLongDescription = String.Empty;

            // This loop should produce a 61-character string.  The maximum allowed length is 60.
            for (int i = 0; i <= 60; i++)
            {
                tooLongDescription = String.Concat(tooLongDescription, "z");
            }

            ProductDescriptionValidator validator = new ProductDescriptionValidator();

            // When.
            bool result = validator.Validate(tooLongDescription);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ProductDescriptionValidator_NullProductDescription_ShouldNotValidate()
        {
            // Given.
            string productDescription = null;

            ProductDescriptionValidator validator = new ProductDescriptionValidator();

            // When.
            bool result = validator.Validate(productDescription);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ProductDescriptionValidator_EmptyProductDescription_ShouldValidate()
        {
            // Given.
            string productDescription = String.Empty;

            ProductDescriptionValidator validator = new ProductDescriptionValidator();

            // When.
            bool result = validator.Validate(productDescription);

            // Then.
            Assert.IsTrue(result);
        }
    }
}
