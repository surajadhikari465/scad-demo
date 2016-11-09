using Icon.Web.Common.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class PluValidatorTests
    {
        [TestMethod]
        public void PluValidator_ValidPlu_ShouldValidate()
        {
            // Given.
            string plu = "1234";

            PluValidator validator = new PluValidator();

            // When.
            bool result = validator.Validate(plu);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PluValidator_EmptyPlu_ShouldValidate()
        {
            // Given.
            string plu = String.Empty;

            PluValidator validator = new PluValidator();

            // When.
            bool result = validator.Validate(plu);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PluValidator_PluExceedsMaximumLength_ShouldNotValidate()
        {
            // Given.
            string plu = "1234567891234";

            PluValidator validator = new PluValidator();

            // When.
            bool result = validator.Validate(plu);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void PluValidator_PluLengthBetweenSixAndEleven_ShouldNotValidate()
        {
            // Given.
            string plu = "12345678";

            PluValidator validator = new PluValidator();

            // When.
            bool result = validator.Validate(plu);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void PluValidator_NullPlu_ShouldNotValidate()
        {
            // Given.
            string plu = null;

            PluValidator validator = new PluValidator();

            // When.
            bool result = validator.Validate(plu);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void PluValidator_InvalidPluCharacters_ShouldNotValidate()
        {
            // Given.
            string[] invalidPlus = new string[] { "126a70", "129.0", "-12890", "&123", " 1234" };

            PluValidator validator = new PluValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string plu in invalidPlus)
            {
                results.Add(validator.Validate(plu));
            }

            // Then.
            Assert.IsTrue(results.TrueForAll(result => result == false));
        }
    }
}
