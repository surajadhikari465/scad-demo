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
    public class ScaleNutrifactsDAOUnitTests
    {

        [TestMethod]
        public void ScaleNutrifactsDAO_ValidateData_WhenInvalidCharInDescription_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.Description = "No p|pes allowed";
            var expected = ScaleNutrifactsValidationStatus.Error_DescriptionInvalidCharacters;

            // When.
            ArrayList result = ScaleNutrifactDAO.ValidateScaleNutrifacts(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleNutrifactsValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleNutrifactsDAO_ValidateData_WhenInvalidCharInServingPerContainer_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.ServingPerContainer = "No p|pes allowed";
            var expected = ScaleNutrifactsValidationStatus.Error_ServingPerContainerInvalidCharacters;

            // When.
            ArrayList result = ScaleNutrifactDAO.ValidateScaleNutrifacts(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleNutrifactsValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleNutrifactsDAO_ValidateData_WhenInvalidCharInServingSizeDesc_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.ServingSizeDesc = "No p|pes allowed";
            var expected = ScaleNutrifactsValidationStatus.Error_ServingSizeDescInvalidCharacters;

            // When.
            ArrayList result = ScaleNutrifactDAO.ValidateScaleNutrifacts(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleNutrifactsValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleNutrifactsDAO_ValidateData_WhenInvalidCharInMultipleFields_ReturnsAllErrorCodes()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.Description = "No p|pes allowed";
            testValues.ServingPerContainer = "No p|pes allowed";
            testValues.ServingSizeDesc = "No p|pes allowed";

            // When.
            ArrayList result = ScaleNutrifactDAO.ValidateScaleNutrifacts(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        } 

        private ScaleNutrifactBO testValues = new ScaleNutrifactBO
        {
            ID = 10001,
            Description = "24955300000",
            ServingSizeDesc = "4 OZ.",
            ServingUnits = 1,
            ServingPerContainer = "VARIED",
            ServingsPerPortion = 1,
            SizeWeight = 1
        };
    }
}