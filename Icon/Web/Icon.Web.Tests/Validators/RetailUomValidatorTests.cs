using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Icon.Common.Validators;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class RetailUomValidatorTests
    {
        [TestMethod]
        public void RetailUomValidator_ValidRetailUom_ShouldValidate()
        {
            // Given.
            string[] validRetailUoms = UomCodes.ByName.Values.ToArray(); ;

            RetailUomValidator validator = new RetailUomValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string retailUom in validRetailUoms)
            {
                results.Add(validator.Validate(retailUom));
            }

            // Then.
            Assert.IsTrue(results.TrueForAll(result => result == true));
        }

        [TestMethod]
        public void RetailUomValidator_InvalidRetailUomCharacters_ShouldNotValidate()
        {
            // Given.
            string[] invalidRetailUoms = new string[] { "EACH1", "IMPERIAL FLUID OUNCE" };

            RetailUomValidator validator = new RetailUomValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string retailUom in invalidRetailUoms)
            {
                results.Add(validator.Validate(retailUom));
            }

            // Then.
            Assert.IsFalse(results.TrueForAll(result => result == true));
        }

        [TestMethod]
        public void RetailUomValidator_EmptyRetailUom_ShouldNotValidate()
        {
            // Given.
            string retailUom = String.Empty;

            RetailUomValidator validator = new RetailUomValidator();

            // When.
            bool result = validator.Validate(retailUom);
            
            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RetailUomValidator_NullRetailUom_ShouldNotValidate()
        {
            // Given.
            string retailUom = null;

            RetailUomValidator validator = new RetailUomValidator();

            // When.
            bool result = validator.Validate(retailUom);

            // Then.
            Assert.IsFalse(result);
        }
    }
}
