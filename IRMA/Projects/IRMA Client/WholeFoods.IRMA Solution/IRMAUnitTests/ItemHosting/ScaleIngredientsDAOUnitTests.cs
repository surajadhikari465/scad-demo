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
    public class ScaleIngredientsDAOUnitTests
    {
        [TestMethod]
        public void ScaleIngredientsDAO_ValidateData_WhenValid_ReturnsValidStatusCode()
        {
            // Given.
            var testSignAttributes = testValues;
            var expected = ScaleIngredientsValidationStatus.Valid;

            // When.
            ArrayList result = ScaleIngredientsDAO.ValidateIngredients(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleIngredientsValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleIngredientsDAO_ValidateData_WhenInvalidCharInDescription_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.Description = "No p|pes allowed";
            var expected = ScaleIngredientsValidationStatus.Error_ScaleIngredientsDescriptionInvalidCharacters;

            // When.
            ArrayList result = ScaleIngredientsDAO.ValidateIngredients(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleIngredientsValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleIngredientsDAO_ValidateData_WhenInvalidCharInIngredients_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.Ingredients = "No p|pes allowed";
            var expected = ScaleIngredientsValidationStatus.Error_ScaleIngredientsInvalidCharacters;

            // When.
            ArrayList result = ScaleIngredientsDAO.ValidateIngredients(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleIngredientsValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleIngredientsDAO_ValidateData_WhenInvalidCharInMultipleFields_ReturnsAllErrorCodes()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.Description = "No p|pes allowed";
            testValues.Ingredients += "|";

            // When.
            ArrayList result = ScaleIngredientsDAO.ValidateIngredients(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        private IngredientsBO testValues = new IngredientsBO
        {
            ID = 10001,
            Description = "46000006823",
            Ingredients = "Ingredients: Gelato Base (skim milk, sugar [sucrose], corn syrup, guar gum, mono & diglycerides, cellulose gum, carrageenan), Mixed Berries (strawberries, blueberries, raspberries, blackberries).",
            LabelTypeDescription = "15 x 42 Characters",
            LabelTypeID = 4
        };
    }
}