using Icon.Framework;
using Icon.Web.Common.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass] [Ignore]
    public class DeliverySystemValidatorTests
    {
        [TestMethod]
        public void DeliverySystemValidator_ValidDeliverySystem_ShouldValidate()
        {
            // Given.
            string[] validDeliverySystems = DeliverySystems.AsDictionary.Values.ToArray(); ;

            DeliverySystemValidator validator = new DeliverySystemValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string deliverySystem in validDeliverySystems)
            {
                results.Add(validator.Validate(deliverySystem));
            }

            // Then.
            Assert.IsTrue(results.TrueForAll(result => result == true));
        }

        [TestMethod]
        public void DeliverySystemValidator_InvalidDeliverySystemCharacters_ShouldNotValidate()
        {
            // Given.
            string[] invalidDeliverySystems = new string[] { "CAP1", "CAPLET" };

            DeliverySystemValidator validator = new DeliverySystemValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string deliverySystem in invalidDeliverySystems)
            {
                results.Add(validator.Validate(deliverySystem));
            }

            // Then.
            Assert.IsFalse(results.TrueForAll(result => result == true));
        }

        [TestMethod]
        public void DeliverySystemValidator_EmptyDeliverySystem_ShouldValidate()
        {
            // Given.
            string deliverySystem = String.Empty;

            DeliverySystemValidator validator = new DeliverySystemValidator();

            // When.
            bool result = validator.Validate(deliverySystem);

            // Then.
            Assert.IsTrue(result);
        }
    }
}
