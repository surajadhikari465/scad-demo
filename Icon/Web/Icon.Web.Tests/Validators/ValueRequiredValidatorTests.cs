using System;
using Icon.Common.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class ValueRequiredValidatorTests
    {
        [TestCategory("Validator")]
        [TestMethod]
        public void ValidatorForValueRequired_NullInput_IsNotValid()
        {
            // Given.
            string testValue = null;

            ValueRequiredValidator valueRequiredValidator = new ValueRequiredValidator();
            
            // When.
            bool result = valueRequiredValidator.Validate(testValue);

            // Then.
            Assert.IsFalse(result);
        }

        [TestCategory("Validator")]
        [TestMethod]
        public void ValidatorForValueRequired_EmptyInput_IsNotValid()
        {
            // Given.
            string testValue = String.Empty;

            ValueRequiredValidator valueRequiredValidator = new ValueRequiredValidator();

            // When.
            bool result = valueRequiredValidator.Validate(testValue);

            // Then.
            Assert.IsFalse(result);
        }

        [TestCategory("Validator")]
        [TestMethod]
        public void ValidatorForValueRequired_SpaceCharsInput_IsNotValid()
        {
            // Given.
            string testValue = "   ";

            ValueRequiredValidator valueRequiredValidator = new ValueRequiredValidator();

            // When.
            bool result = valueRequiredValidator.Validate(testValue);

            // Then.
            Assert.IsFalse(result);
        }

        [TestCategory("Validator")]
        [TestMethod]
        public void ValidatorForValueRequired_NonEmptyString_IsValid()
        {
            // Given.
            string testValue = "test";

            ValueRequiredValidator valueRequiredValidator = new ValueRequiredValidator();

            // When.
            bool result = valueRequiredValidator.Validate(testValue);

            // Then.
            Assert.IsTrue(result);
        }
    }
}
