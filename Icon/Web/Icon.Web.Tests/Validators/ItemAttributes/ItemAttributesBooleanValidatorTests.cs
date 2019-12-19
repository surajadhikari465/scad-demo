
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Icon.Common;
using Icon.Common.Models;
using Icon.Common.Validators.ItemAttributes;

namespace Icon.Web.Tests.Unit.Validators.ItemAttributes
{
    [TestClass]
    public class ItemAttributesBooleanValidatorTests
    {
        private ItemAttributesBooleanValidator validator;
        private AttributeModel attribute;

        [TestInitialize]
        public void Initialize()
        {
            attribute = new AttributeModel { DisplayName = "Test", DataTypeName = Constants.DataTypeNames.Boolean };
            validator = new ItemAttributesBooleanValidator(attribute);
        }

        [TestMethod]
        public void Validate_ValueIsTrue_ShouldBeValid()
        {
            //When
            var result1 = validator.Validate("True");
            var result2 = validator.Validate("true");
            var result3 = validator.Validate("TRUE");

            //Then
            Assert.IsTrue(result1.IsValid);
            Assert.IsTrue(result2.IsValid);
            Assert.IsTrue(result3.IsValid);
        }

        [TestMethod]
        public void Validate_ValueIsFalse_ShouldBeValid()
        {
            //When
            var result1 = validator.Validate("False");
            var result2 = validator.Validate("false");
            var result3 = validator.Validate("FALSE");

            //Then
            Assert.IsTrue(result1.IsValid);
            Assert.IsTrue(result2.IsValid);
            Assert.IsTrue(result3.IsValid);
        }

        [TestMethod]
        public void Validate_ValueIsNotAParsableBoolen_ShouldBeInvalid()
        {
            //When
            var result = validator.Validate("not a boolean");

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Test contains an invalid value. Test can only be true or false.", result.ErrorMessages.Single());
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
    }
}
