using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Icon.Common.Validators;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass] [Ignore]
    public class PackageUnitValidatorTests
    {
        [TestMethod]
        public void PackageUnitValidator_ValidPackageUnit_ShouldValidate()
        {
            // Given.
            string packageUnit = "12";

            PackageUnitValidator validator = new PackageUnitValidator();

            // When.
            bool result = validator.Validate(packageUnit);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PackageUnitValidator_InvalidPackageUnitLength_ShouldNotValidate()
        {
            // Given.
            string packageUnit = "2222";

            PackageUnitValidator validator = new PackageUnitValidator();

            // When.
            bool result = validator.Validate(packageUnit);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void PackageUnitValidator_InvalidPackageUnitCharacter_ShouldNotValidate()
        {
            // Given.
            string[] invalidPackageUnits = new string[] { "E1", "$A", "-2", "1 " };

            PackageUnitValidator validator = new PackageUnitValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string packageUnit in invalidPackageUnits)
            {
                results.Add(validator.Validate(packageUnit));
            }

            // Then.
            Assert.IsTrue(results.TrueForAll(result => result == false));
        }

        [TestMethod]
        public void PackageUnitValidator_EmptyPackageUnit_ShouldValidate()
        {
            // Given.
            string packageUnit = String.Empty;

            PackageUnitValidator validator = new PackageUnitValidator();

            // When.
            bool result = validator.Validate(packageUnit);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PackageUnitValidator_NullPackageUnit_ShouldNotValidate()
        {
            // Given.
            string packageUnit = null;

            PackageUnitValidator validator = new PackageUnitValidator();

            // When.
            bool result = validator.Validate(packageUnit);

            // Then.
            Assert.IsFalse(result);
        }
    }
}
