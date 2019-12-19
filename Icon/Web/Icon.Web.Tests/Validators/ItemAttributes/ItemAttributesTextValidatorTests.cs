using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Icon.Common;
using Icon.Common.Models;
using Icon.Common.Validators.ItemAttributes;

namespace Icon.Web.Tests.Unit.Validators.ItemAttributes
{
    [TestClass]
    public class ItemAttributesTextValidatorTests
    {
        private ItemAttributesTextValidator validator;
        private AttributeModel attribute;

        [TestInitialize]
        public void Initialize()
        {
            attribute = new AttributeModel
            {
                DisplayName = "Test",
                DataTypeName = Constants.DataTypeNames.Text,
                MaxLengthAllowed = 10,
                CharacterSetRegexPattern = "^[a-z]*$",
                CharacterSets = new List<AttributeCharacterSetModel>
                {
                    new AttributeCharacterSetModel { CharacterSetModel = new CharacterSetModel { Name = "CharacterSet1" } },
                    new AttributeCharacterSetModel { CharacterSetModel = new CharacterSetModel { Name = "CharacterSet2" } },
                    new AttributeCharacterSetModel { CharacterSetModel = new CharacterSetModel { Name = "CharacterSet3" } }
                },
                SpecialCharactersAllowed = "!@#"
            };
            validator = new ItemAttributesTextValidator(attribute);
        }

        [TestMethod]
        public void Validate_ValueIsIsLessThanMaxLengthAndMatchesRegexPattern_ShouldBeValid()
        {
            //When
            var result = validator.Validate("abcdefg");

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ValueIsGreaterThanMaxLength_ShouldBeInvalid()
        {
            //When
            var result = validator.Validate(new string('a', attribute.MaxLengthAllowed.Value + 1));

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Test has a max length of 10.", result.ErrorMessages.Single());
        }

        [TestMethod]
        public void Validate_ValueDoesNotMatchTheCharacterSetRegexPattern_ShouldBeInvalid()
        {
            //When
            var result = validator.Validate("A");

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(
                "Test does not meet character set restrictions. Test only accepts CharacterSet1, CharacterSet2, CharacterSet3 and allows the following special characters: !@#.", 
                result.ErrorMessages.Single());
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