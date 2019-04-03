using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeFoods.IRMA.ItemHosting.BusinessLogic;
using WholeFoods.IRMA.ItemHosting.DataAccess;

namespace IRMAUnitTests.ItemHosting
{
    [TestClass]
    public class ScaleAllergenDAOUnitTests
    {
        [TestMethod]
        public void ScaleAllergenDAO_ValidateData_WhenValid_ReturnsValidStatusCode()
        {
            // Given.
            var testSignAttributes = testValues;
            var expected = ScaleAllergensValidationStatus.Valid;

            // When.
            ArrayList result = ScaleAllergensDAO.ValidateAllergens(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleAllergensValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleAllergenDAO_ValidateData_WhenInvalidCharInDescription_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.Description = "No p|pes allowed";
            var expected = ScaleAllergensValidationStatus.Error_DescriptionInvalidCharacters;

            // When.
            ArrayList result = ScaleAllergensDAO.ValidateAllergens(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleAllergensValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleAllergenDAO_ValidateData_WhenInvalidCharInAllergens_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.Allergens = "No p|pes allowed";
            var expected = ScaleAllergensValidationStatus.Error_AllergensInvalidCharacters;

            // When.
            ArrayList result = ScaleAllergensDAO.ValidateAllergens(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleAllergensValidationStatus)result[0]);
        }
               
        [TestMethod]
        public void ScaleAllergenDAO_ValidateData_WhenInvalidCharInMultipleFields_ReturnsAllErrorCodes()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.Description = "No p|pes allowed";
            testValues.Allergens += "|";

            // When.
            ArrayList result = ScaleAllergensDAO.ValidateAllergens(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        private AllergensBO testValues = new AllergensBO
        {
            ID = 10001,
            Description = "",
            Allergens = "",
            LabelTypeID = 0,
            LableTypeDescription = "Default",
           
        };
    }
}