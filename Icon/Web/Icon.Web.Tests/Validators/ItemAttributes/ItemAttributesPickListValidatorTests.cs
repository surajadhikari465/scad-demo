using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Icon.Common;
using Icon.Common.Models;
using Icon.Common.Validators.ItemAttributes;


namespace Icon.Web.Tests.Unit.Validators.ItemAttributes
{
    [TestClass]
    public class ItemAttributesPickListValidatorTests
    {
        private ItemAttributesPickListValidator validator;
        private AttributeModel attribute;

        [TestInitialize]
        public void Initialize()
        {
            attribute = new AttributeModel
            {
                DisplayName = "Test",
                DataTypeName = Constants.DataTypeNames.Text,
                IsPickList = true,
                PickListData = new List<PickListModel>
                {
                    new PickListModel { PickListValue = "PickListValue1" },
                    new PickListModel { PickListValue = "PickListValue2" },
                    new PickListModel { PickListValue = "PickListValue3" },
                    new PickListModel { PickListValue = "PickListValue4" },
                    new PickListModel { PickListValue = "PickListValue5" },
                }
            };
            validator = new ItemAttributesPickListValidator(attribute);
        }

        [TestMethod]
        public void Validate_ValueIsInPickListDataOfAttribute_ShouldBeValid()
        {
            //When
            var result = validator.Validate("PickListValue1");

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ValueIsNoInPickListDataOfAttribute_ShouldBeInvalid()
        {
            //When
            var result = validator.Validate("Invalid");

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Test does not contain 'Invalid' as a possible value.", result.ErrorMessages.Single());
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
