using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Icon.Common.Validators;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class ScanCodeValidatorTests
    {
        [TestMethod]
        public void ScanCodeValidator_ValidScanCode_ShouldValidate()
        {
            // Given.
            string scanCode = "1234567890";

            ScanCodeValidator validator = new ScanCodeValidator();

            // When.
            bool result = validator.Validate(scanCode);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ScanCodeValidator_InvalidScanCodeLength_ShouldNotValidate()
        {
            // Given.
            string scanCode = "12345678901234567890";

            ScanCodeValidator validator = new ScanCodeValidator();

            // When.
            bool result = validator.Validate(scanCode);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ScanCodeValidator_NullScanCode_ShouldNotValidate()
        {
            // Given.
            string scanCode = null;

            ScanCodeValidator validator = new ScanCodeValidator();

            // When.
            bool result = validator.Validate(scanCode);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ScanCodeValidator_EmptyScanCode_ShouldNotValidate()
        {
            // Given.
            string scanCode = String.Empty;

            ScanCodeValidator validator = new ScanCodeValidator();

            // When.
            bool result = validator.Validate(scanCode);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ScanCodeValidator_InvalidScanCodeCharacters_ShouldNotValidate()
        {
            // Given.
            string[] invalidScanCodes = new string[] { "123456a7890", "123456789.0", "-1234567890", "&1234567890" };

            ScanCodeValidator validator = new ScanCodeValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string scanCode in invalidScanCodes)
            {
                results.Add(validator.Validate(scanCode));
            }

            // Then.
            Assert.IsTrue(results.TrueForAll(result => result == false));
        }
    }
}
