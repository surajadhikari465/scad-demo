using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Icon.Common;
using Icon.Common.Models;
using Icon.Common.Validators.ItemAttributes;

namespace Icon.Web.Tests.Unit.Validators.ItemAttributes
{
    [TestClass]
    public class ItemAttributesDateValidatorTests
    {
        private ItemAttributesDateValidator validator;
        private AttributeModel attribute;

        [TestInitialize]
        public void Initialize()
        {
            attribute = new AttributeModel { DisplayName = "Test", DataTypeName = Constants.DataTypeNames.Date };
            validator = new ItemAttributesDateValidator(attribute);
        }

        [TestMethod]
        public void Validate_ValueIsDate_ShouldBeValid()
        {
            //Given
            var date1 = "8/28/2019 12:03:54 PM";
            var date2 = "11/22/1990 00:00:00";
            var date3 = "12/12/1121 01:40:00";
            var date4 = "2019-08-27 13:50:42.3505663";

            //When
            var result1 = validator.Validate(date1);
            var result2 = validator.Validate(date2);
            var result3 = validator.Validate(date3);
            var result4 = validator.Validate(date4);

            //Then
            Assert.IsTrue(result1.IsValid);
            Assert.IsTrue(result2.IsValid);
            Assert.IsTrue(result3.IsValid);
            Assert.IsTrue(result4.IsValid);
        }

        [TestMethod]
        public void Validate_ValueIsNotAParsableDate_ShouldBeInvalid()
        {
            //When
            var result = validator.Validate("not a date");

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Test contains an invalid value. Test must be a date.", result.ErrorMessages.Single());
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
