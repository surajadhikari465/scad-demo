using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Icon.Common.Validators;

namespace Icon.Tests.Validators
{
    [TestClass]
    public class BusinessUnitValidatorTests
    {
        [TestMethod]
        public void BusinessUnitValidator_ValidBusinessUnit_ShouldValidate()
        {
            // Given.
            string businessUnit = "10011";

            BusinessUnitValidator validator = new BusinessUnitValidator();

            // When.
            bool result = validator.Validate(businessUnit);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void BusinessUnitValidator_InvalidBusinessUnitLength_ShouldNotValidate()
        {
            // Given.
            string longBusinessUnit = "2222222";
            string shortBusinessUnit = "1111";

            BusinessUnitValidator validator = new BusinessUnitValidator();

            // When.
            bool longBusinessUnitValidates = validator.Validate(longBusinessUnit);
            bool shortBusinessUnitValidates = validator.Validate(shortBusinessUnit);

            // Then.
            Assert.IsFalse(longBusinessUnitValidates);
            Assert.IsFalse(shortBusinessUnitValidates);
        }

        [TestMethod]
        public void BusinessUnitValidator_InvalidBusinessUnitCharacter_ShouldNotValidate()
        {
            // Given.
            string[] invalidBusinessUnits = new string[] { "E1", "$A", "-2", "1 " };

            BusinessUnitValidator validator = new BusinessUnitValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string packageUnit in invalidBusinessUnits)
            {
                results.Add(validator.Validate(packageUnit));
            }

            // Then.
            Assert.IsTrue(results.TrueForAll(result => result == false));
        }

        [TestMethod]
        public void BusinessUnitValidator_EmptyBusinessUnit_ShouldValidate()
        {
            // Given.
            string businessUnit = String.Empty;

            BusinessUnitValidator validator = new BusinessUnitValidator();

            // When.
            bool result = validator.Validate(businessUnit);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void BusinessUnitValidator_NullBusinessUnit_ShouldNotValidate()
        {
            // Given.
            string businessUnit = null;

            BusinessUnitValidator validator = new BusinessUnitValidator();

            // When.
            bool result = validator.Validate(businessUnit);

            // Then.
            Assert.IsFalse(result);
        }
    }
}
