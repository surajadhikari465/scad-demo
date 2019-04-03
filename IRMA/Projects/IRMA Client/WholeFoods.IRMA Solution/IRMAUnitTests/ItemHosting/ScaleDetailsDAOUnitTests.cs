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
    public class ScaleDetailsDAOUnitTests
    {
        [TestMethod]
        public void ScaleDetailsDAO_ValidateData_WhenValid_ReturnsValidStatusCode()
        {
            // Given.
            var testSignAttributes = testValues;
            var expected = ScaleDetailsValidationStatus.Valid;

            // When.
            ArrayList result = ScaleDetailsDAO.ValidateScaleDetails(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleDetailsValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleDetailsDAO_ValidateData_WhenInvalidCharInScaleDescription1_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.ScaleDescription1 = "No p|pes allowed";
            var expected = ScaleDetailsValidationStatus.Error_ScaleDescription1InvalidCharacters;

            // When.
            ArrayList result = ScaleDetailsDAO.ValidateScaleDetails(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleDetailsValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleDetailsDAO_ValidateData_WhenInvalidCharInScaleDescription2_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.ScaleDescription2 = "No p|pes allowed";
            var expected = ScaleDetailsValidationStatus.Error_ScaleDescription2InvalidCharacters;

            // When.
            ArrayList result = ScaleDetailsDAO.ValidateScaleDetails(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleDetailsValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleDetailsDAO_ValidateData_WhenInvalidCharInScaleDescription3_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.ScaleDescription3 = "No p|pes allowed";
            var expected = ScaleDetailsValidationStatus.Error_ScaleDescription3InvalidCharacters;

            // When.
            ArrayList result = ScaleDetailsDAO.ValidateScaleDetails(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleDetailsValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleDetailsDAO_ValidateData_WhenInvalidCharInScaleDescription4_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.ScaleDescription4 = "No p|pes allowed";
            var expected = ScaleDetailsValidationStatus.Error_ScaleDescription4InvalidCharacters;

            // When.
            ArrayList result = ScaleDetailsDAO.ValidateScaleDetails(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleDetailsValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleDetailsDAO_ValidateData_WhenInvalidCharInFixedWeight_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.FixedWeight = "No p|pes allowed";
            var expected = ScaleDetailsValidationStatus.Error_FixedWeightInvalidCharacters;

            // When.
            ArrayList result = ScaleDetailsDAO.ValidateScaleDetails(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleDetailsValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleDetailsDAO_ValidateData_WhenInvalidCharInMultipleFields_ReturnsAllErrorCodes()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.ScaleDescription1 = "No p|pes allowed!";
            testValues.ScaleDescription2 = "No p|pes allowed!";
            testValues.ScaleDescription3 = "No p|pes allowed!";
            testValues.ScaleDescription4 = "No p|pes allowed!";
            testValues.FixedWeight = "|0|";

            // When.
            ArrayList result = ScaleDetailsDAO.ValidateScaleDetails(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
        }

        private ScaleDetailsBO testValues = new ScaleDetailsBO
        {
            ItemScaleID = 283072,
            ItemKey = 421904,
            ItemIdentifier = "20360100000",
            EatBy = 7,
            Grade = 0,
            LabelStyle = 56,
            Nutrifact = 104421,
            ExtraTextID = 975363,
            ExtraText = "20360100000",
            Tare = 2,
            RandomWeightType = 0,
            UOM = 15,
            ByCount = 1,
            ForceTare = false,
            PrintBlankShelfLife = true,
            PrintBlankUnitPrice = true,
            PrintBlankShelfEatBy = true,
            PrintBlankWeight = false,
            FixedWeight = "24",
            ScaleDescription1 = "SALMON MEAL FOR TWO",
            ScaleDescription2 = "PINK PEPPERCORN SALMON w LEEK CREAM SAUCE",
            ScaleDescription3 = "SAFFRON LEEK RICE",
            ScaleDescription4 = null
        };
    }
}