using Icon.Framework;
using Icon.Web.Common.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class EcoScaleRatingValidatorTests
    {
        private EcoScaleRatingValidator validator;

        [TestInitialize]
        public void Initialize()
        {
            validator = new EcoScaleRatingValidator();
        }

        [TestMethod]
        public void Validate_ValidValue_ResultShouldBeTrue()
        {
            // Given.
            string value = EcoScaleRatings.Descriptions.AsArray[0];

            // When.
            bool result = validator.Validate(value);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Validate_InvalidValue_ResultShouldBeFalse()
        {
            // Given.
            string value = "Polluter/Red";

            // When.
            bool result = validator.Validate(value);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Validate_EmptyStringValue_ResultShouldBeTrue()
        {
            // Given.
            string value = String.Empty;

            // When.
            bool result = validator.Validate(value);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Validate_CaseDifference_ShouldNotCauseValidationFailure()
        {
            // Given.
            string value = EcoScaleRatings.Descriptions.AsArray[0].ToUpper();

            // When.
            bool result = validator.Validate(value);

            // Then.
            Assert.IsTrue(result);
        }
    }
}
