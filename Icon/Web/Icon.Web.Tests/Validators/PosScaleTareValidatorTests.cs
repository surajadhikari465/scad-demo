using Icon.Web.Common.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class PosScaleTareValidatorTests
    {
        [TestMethod]
        public void PosScaleTareValidator_ValidPosScaleTare_ShouldValidate()
        {
            // Given.
            string[] validPosScaleTares = new string[] { "0.01", "1", ".01", "1.", "1.0" };

            PosScaleTareValidator validator = new PosScaleTareValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string posScaleTare in validPosScaleTares)
            {
                results.Add(validator.Validate(posScaleTare));
            }

            // Then.
            Assert.IsTrue(results.TrueForAll(result => result == true));
        }

        [TestMethod]
        public void PosScaleTareValidator_OutOfRangePosScaleTare_ShouldNotValidate()
        {
            // Given.
            string[] validPosScaleTares = new string[] { "10", "1.00111" };

            PosScaleTareValidator validator = new PosScaleTareValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string posScaleTare in validPosScaleTares)
            {
                results.Add(validator.Validate(posScaleTare));
            }

            // Then.
            Assert.IsFalse(results.TrueForAll(result => result == true));
        }

        [TestMethod]
        public void PosScaleTareValidator_EmptyPosScaleTare_ShouldValidate()
        {
            // Given.
            string posScaleTare = String.Empty;

            PosScaleTareValidator validator = new PosScaleTareValidator();

            // When.
            bool result = validator.Validate(posScaleTare);
            
            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PosScaleTareValidator_InvalidPosScaleTareCharacters_ShouldNotValidate()
        {
            // Given.
            string[] validPosScaleTares = new string[] { "abc", "1.1.1", "-9", "1%" };

            PosScaleTareValidator validator = new PosScaleTareValidator();

            // When.
            List<bool> results = new List<bool>();
            foreach (string posScaleTare in validPosScaleTares)
            {
                results.Add(validator.Validate(posScaleTare));
            }

            // Then.
            Assert.IsTrue(results.TrueForAll(result => result == false));
        }

        [TestMethod]
        public void PosScaleTareValidator_NullPosScaleTare_ShouldNotValidate()
        {
            // Given.
            string posScaleTare = null;

            PosScaleTareValidator validator = new PosScaleTareValidator();

            // When.
            bool result = validator.Validate(posScaleTare);

            // Then.
            Assert.IsFalse(result);
        }
    }
}
