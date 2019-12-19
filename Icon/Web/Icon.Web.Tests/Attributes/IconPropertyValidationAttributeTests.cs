using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.Common;
using Icon.Web.Attributes;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass] [Ignore]
    public class IconPropertyValidationAttributeTests
    {
        private IconPropertyValidationAttribute validator;

        [TestMethod]
        public void IconPropertyValidationAttribute_Properties_ShouldSetProperties()
        {
            // When
            validator = new IconPropertyValidationAttribute(ValidatorPropertyNames.ProductDescription);
            validator.CanBeNullOrEmpty = true;            

            // Then.
            Assert.IsTrue(validator.CanBeNullOrEmpty);
        }

        [TestMethod]
        public void IconPropertyValidationAttribute_CanNotBeNull_ShouldNotValidate()
        {
            // Given.
            validator = new IconPropertyValidationAttribute(ValidatorPropertyNames.ProductDescription);
            validator.CanBeNullOrEmpty = false; 

            // When.
            bool result = validator.IsValid(null);

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IconPropertyValidationAttribute_CanBeNull_ShouldValidate()
        {
            // Given.
            string productDescription = null;
            validator = new IconPropertyValidationAttribute(ValidatorPropertyNames.ProductDescription);
            validator.CanBeNullOrEmpty = true; 

            
            // When.
            bool result = validator.IsValid(productDescription);

            // Then.
            Assert.IsTrue(result);
        }
    }
}
