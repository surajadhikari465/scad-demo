using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Icon.Common.Validators;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass] [Ignore]
    public class RetailSizeValidatorTests
    {
        [TestMethod]
        public void RetailSizeValidator_ValidRetailSize_ShouldValidate()
        {
            // Given.
            string[] validRetailSizes = new string[] { "0.01", ".01", "1", "1.", "1.0", "10000.0", "1.1234" };

            RetailSizeValidator validator = new RetailSizeValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string retailSize in validRetailSizes)
            {
                results.Add(validator.Validate(retailSize));
            }

            // Then.
            Assert.IsTrue(results.TrueForAll(result => result == true));
        }

        [TestMethod]
        public void RetailSizeValidator_OutOfRangeRetailSize_ShouldNotValidate()
        {
            // Given.
            string[] invalidRetailSizes = new string[] { "1000000", "1.00111" };

            RetailSizeValidator validator = new RetailSizeValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string retailSize in invalidRetailSizes)
            {
                results.Add(validator.Validate(retailSize));
            }

            // Then.
            Assert.IsFalse(results.TrueForAll(result => result == true));
        }

        [TestMethod]
        public void RetailSizeValidator_EmptyRetailSize_ShouldNotValidate()
        {
            // Given.
            string retailSize = String.Empty;

            RetailSizeValidator validator = new RetailSizeValidator();

            // When.
            bool result = validator.Validate(retailSize);
            
            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RetailSizeValidator_InvalidRetailSizeCharacters_ShouldNotValidate()
        {
            // Given.
            string[] invalidRetailSizes = new string[] { "abc", "1.1.1", "-9", "1%" };

            RetailSizeValidator validator = new RetailSizeValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string retailSize in invalidRetailSizes)
            {
                results.Add(validator.Validate(retailSize));
            }

            // Then.
            Assert.IsTrue(results.TrueForAll(result => result == false));
        }

        [TestMethod]
        public void RetailSizeValidator_NullRetailSize_ShouldNotValidate()
        {
            // Given.
            string retailSize = null;

            RetailSizeValidator validator = new RetailSizeValidator();

            // When.
            bool result = validator.Validate(retailSize);

            // Then.
            Assert.IsFalse(result);
        }
    }
}
