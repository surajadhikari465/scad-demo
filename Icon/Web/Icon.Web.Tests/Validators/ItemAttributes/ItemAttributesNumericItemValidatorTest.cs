using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Icon.Common;
using Icon.Common.Models;
using Icon.Common.Validators.ItemAttributes;

namespace Icon.Web.Tests.Unit.Validators.ItemAttributes
{
    [TestClass]
    public class ItemAttributesNumericItemValidatorTest
    {
        private ItemAttributesNumericItemValidator validator;
        private AttributeModel attribute;

        [TestInitialize]
        public void Initialize()
        {
            attribute = new AttributeModel
            {
                DisplayName = "Test",
                DataTypeName = Constants.DataTypeNames.Number,
                NumberOfDecimals = "1",
                MinimumNumber = "0",
                MaximumNumber = "10"
            };
            validator = new ItemAttributesNumericItemValidator(attribute);
        }

        [TestMethod]
        public void Validate_ValueIsGreaterThanMinAndLessThanMaxAndHasLessNumberOfDecimals_ShouldBeValid()
        {
            //When
            var result = validator.Validate("5");

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ValueIsLessThanMin_ShouldBeInvalid()
        {
            //When
            var result = validator.Validate("-1");

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Test must be greater than or equal to 0.", result.ErrorMessages.Single());
        }

        [TestMethod]
        public void Validate_ValueIsGreaterThanMax_ShouldBeInvalid()
        {
            //When
            var result = validator.Validate("11");

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Test must be less than or equal to 10.", result.ErrorMessages.Single());
        }

        [TestMethod]
        public void Validate_ValueHasTooManyDecimals_ShouldBeInvalid()
        {
            //When
            var result = validator.Validate("1.22");

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Test must have at most 1 number of decimals.", result.ErrorMessages.Single());
        }

        [TestMethod]
        public void Validate_ValueHas0DecimalsAndNoDecimalsSpecified_ShouldBeValid()
        {
            //Given
            attribute.NumberOfDecimals = "0";

            //When
            var result = validator.Validate("1");

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ValueHas2DecimalsAnd2DecimalsSpecified_ShouldBeValid()
        {
            //Given
            attribute.NumberOfDecimals = "2";
            validator = new ItemAttributesNumericItemValidator(attribute);

            //When
            var result = validator.Validate("1.11");

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ValueHas2DecimalsAnd3DecimalsSpecified_ShouldBeValid()
        {
            //Given
            attribute.NumberOfDecimals = "3";
            validator = new ItemAttributesNumericItemValidator(attribute);

            //When
            var result = validator.Validate("1.11");

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ValueHas4DecimalsAnd3DecimalsSpecified_ShouldBeValid()
        {
            //Given
            attribute.NumberOfDecimals = "3";

            //When
            var result = validator.Validate("1.1111");

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Test must have at most 3 number of decimals.", result.ErrorMessages.Single());
        }

        [TestMethod]
        public void Validate_ValueEmptyButIsMarkedRequired_ShouldBeInvalid()
        {
            //Given
            attribute.IsRequired = true;

            //When
            var result1 = validator.Validate("   ");
            var result2 = validator.Validate(string.Empty);
            var result3 = validator.Validate(null);

            //Then
            Assert.IsFalse(result1.IsValid);
            Assert.AreEqual("Test is required.", result1.ErrorMessages.Single());
            Assert.IsFalse(result2.IsValid);
            Assert.AreEqual("Test is required.", result2.ErrorMessages.Single());
            Assert.IsFalse(result3.IsValid);
            Assert.AreEqual("Test is required.", result3.ErrorMessages.Single());
        }

        [TestMethod]
        public void Validate_ValueEmptyAndIsMarkedNotRequired_ShouldBeValid()
        {
            //Given
            attribute.IsRequired = false;

            //When
            var result1 = validator.Validate("   ");
            var result2 = validator.Validate(string.Empty);
            var result3 = validator.Validate(null);

            //Then
            Assert.IsTrue(result1.IsValid);
            Assert.IsTrue(result2.IsValid);
            Assert.IsTrue(result3.IsValid);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Validate_MinimumNumberNotSet_ThrowsException()
        {
            //Given
            attribute.MinimumNumber = null;

            //When
            validator = new ItemAttributesNumericItemValidator(attribute);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Validate_MaximumNumberNotSet_ThrowsException()
        {
            //Given
            attribute.MaximumNumber = null;

            //When
            validator = new ItemAttributesNumericItemValidator(attribute);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Validate_NumberOfDecimalsNotSet_ThrowsException()
        {
            //Given
            attribute.NumberOfDecimals = null;

            //When
            validator = new ItemAttributesNumericItemValidator(attribute);
        }
    }
}
