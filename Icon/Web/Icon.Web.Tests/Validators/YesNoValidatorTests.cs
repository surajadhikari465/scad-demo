using Icon.Web.Common.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class YesNoValidatorTests
    {
        [TestMethod]
        public void Validate_ValidValue_ShouldValidate()
        {
            // Given.
            string[] validValues = new string[] { "0", "1" };

            YesNoValidator validator = new YesNoValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string value in validValues)
            {
                results.Add(validator.Validate(value));
            }

            // Then.
            Assert.IsTrue(results.TrueForAll(result => result == true));
        }

        [TestMethod]
        public void Validate_InvalidValueCharacters_ShouldNotValidate()
        {
            // Given.
            string[] validValues = new string[] { "z", "y2", "$", " " };

            YesNoValidator validator = new YesNoValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string value in validValues)
            {
                results.Add(validator.Validate(value));
            }

            // Then.
            Assert.IsTrue(results.TrueForAll(result => result == false));
        }

        [TestMethod]
        public void Validate_NullValue_ShouldNotValidate()
        {
            // Given.
            string value = null;

            YesNoValidator validator = new YesNoValidator();

            // When.
            bool result = validator.Validate(value);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Validate_EmptyValue_ShouldValidate()
        {
            // Given.
            string value = String.Empty;

            YesNoValidator validator = new YesNoValidator();

            // When.
            bool result = validator.Validate(value);

            // Then.
            Assert.IsTrue(result);
        }
    }
}
