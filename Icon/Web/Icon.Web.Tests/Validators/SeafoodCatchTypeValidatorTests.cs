using Icon.Framework;
using Icon.Web.Common.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass] [Ignore]
    public class SeafoodCatchTypeValidatorTests
    {
        private SeafoodCatchTypeValidator validator;

        [TestInitialize]
        public void Initialize()
        {
            validator = new SeafoodCatchTypeValidator();
        }

        [TestMethod]
        public void Validate_ValidValue_ResultShouldBeTrue()
        {
            // Given.
            string value = SeafoodCatchTypes.Descriptions.AsArray[0];

            // When.
            bool result = validator.Validate(value);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Validate_InvalidValue_ResultShouldBeFalse()
        {
            // Given.
            string value = "Ambushed";

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
            string value = SeafoodCatchTypes.Descriptions.AsArray[0].ToUpper();

            // When.
            bool result = validator.Validate(value);

            // Then.
            Assert.IsTrue(result);
        }
    }
}
