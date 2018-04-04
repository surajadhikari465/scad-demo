using Icon.Web.Common.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Icon.Web.Common;
using Icon.Web.Attributes;
using System.ComponentModel.DataAnnotations;
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
            validator.CanBeNullOrEmprty = true;            

            // Then.
            Assert.IsTrue(validator.CanBeNullOrEmprty);
        }

        [TestMethod]
        public void IconPropertyValidationAttribute_CanNotBeNull_ShouldNotValidate()
        {
            // Given.
            validator = new IconPropertyValidationAttribute(ValidatorPropertyNames.ProductDescription);
            validator.CanBeNullOrEmprty = false; 

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
            validator.CanBeNullOrEmprty = true; 

            
            // When.
            bool result = validator.IsValid(productDescription);

            // Then.
            Assert.IsTrue(result);
        }
    }
}
