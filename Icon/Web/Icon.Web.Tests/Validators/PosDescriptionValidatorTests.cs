using Icon.Web.Common.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class PosDescriptionValidatorTests
    {
        [TestMethod]
        public void PosDescriptionValidator_ValidPosDescription_ShouldValidate()
        {
            // Given.
            string posDescription = "TST POS DESC";

            PosDescriptionValidator validator = new PosDescriptionValidator();

            // When.
            bool result = validator.Validate(posDescription);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PosDescriptionValidator_InvalidPosDescriptionLength_ShouldNotValidate()
        {
            // Given.
            string tooLongDescription = String.Empty;

            // This loop should produce a 26-character string.  The maximum allowed is 25.
            for (int i = 0; i < 26; i++)
            {
                tooLongDescription = String.Concat(tooLongDescription, "z");
            }

            PosDescriptionValidator validator = new PosDescriptionValidator();

            // When.
            bool result = validator.Validate(tooLongDescription);

            // Then.
            Assert.AreEqual(26, tooLongDescription.Length);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void PosDescriptionValidator_PosDescriptionLengthIs25Characters_ShouldValidate()
        {
            // Given.
            string posDescription = String.Empty;

            // This loop should produce a 25-character string.  The maximum allowed is 25.
            for (int i = 0; i < 25; i++)
            {
                posDescription = String.Concat(posDescription, "z");
            }

            PosDescriptionValidator validator = new PosDescriptionValidator();

            // When.
            bool result = validator.Validate(posDescription);

            // Then.
            Assert.AreEqual(25, posDescription.Length);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PosDescriptionValidator_NullPosDescription_ShouldNotValidate()
        {
            // Given.
            string posDescription = null;

            PosDescriptionValidator validator = new PosDescriptionValidator();

            // When.
            bool result = validator.Validate(posDescription);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void PosDescriptionValidator_EmptyPosDescription_ShouldValidate()
        {
            // Given.
            string posDescription = String.Empty;

            PosDescriptionValidator validator = new PosDescriptionValidator();

            // When.
            bool result = validator.Validate(posDescription);

            // Then.
            Assert.IsTrue(result);
        }
    }
}
