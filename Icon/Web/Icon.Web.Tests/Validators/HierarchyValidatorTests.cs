using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Icon.Common.Validators;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class HierarchyValidatorTests
    {
        [TestMethod]
        public void HierarchyValidator_ValidHierarchy_ShouldValidate()
        {
            // Given.
            string hierarchy = "TST Hierarchy";

            HierarchyClassNameValidator validator = new HierarchyClassNameValidator();

            // When.
            bool result = validator.Validate(hierarchy);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HierarchyValidator_HierarchyLengthGreaterThan35_ShouldNotValidate()
        {
            // Given.
            string tooLongHierarchy = String.Empty;

            // This loop should produce a 26-character string.  The maximum allowed is 25.
            for (int i = 0; i <= 35; i++)
            {
                tooLongHierarchy = String.Concat(tooLongHierarchy, "z");
            }

            HierarchyClassNameValidator validator = new HierarchyClassNameValidator();

            // When.
            bool result = validator.Validate(tooLongHierarchy);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HierarchyValidator_NullHierarchy_ShouldNotValidate()
        {
            // Given.
            string hierarchy = null;

            HierarchyClassNameValidator validator = new HierarchyClassNameValidator();

            // When.
            bool result = validator.Validate(hierarchy);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HierarchyValidator_EmptyHierarchy_ShouldValidate()
        {
            // Given.
            string hierarchy = String.Empty;

            HierarchyClassNameValidator validator = new HierarchyClassNameValidator();

            // When.
            bool result = validator.Validate(hierarchy);

            // Then.
            Assert.IsTrue(result);
        }
    }
}
