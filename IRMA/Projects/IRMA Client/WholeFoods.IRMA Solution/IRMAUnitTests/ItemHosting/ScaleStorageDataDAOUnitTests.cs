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
    public class ScaleStorageDataDAOUnitTests
    {
        [TestMethod]
        public void ScaleStorageDataDAO_ValidateData_WhenValid_ReturnsValidStatusCode()
        {
            // Given.
            var testSignAttributes = testValues;
            var expected = ScaleStorageDataValidationStatus.Valid;

            // When.
            ArrayList result = ScaleStorageDataDAO.ValidateStorageData(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleStorageDataValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleStorageDataDAO_ValidateData_WhenInvalidCharInDescription_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.Description = "No p|pes allowed";
            var expected = ScaleStorageDataValidationStatus.Error_ScaleStorageDataDescriptionInvalidCharacters;

            // When.
            ArrayList result = ScaleStorageDataDAO.ValidateStorageData(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleStorageDataValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleStorageDataDAO_ValidateData_WhenInvalidCharInStorageData_ReturnsErrorCode()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.StorageData = "No p|pes allowed";
            var expected = ScaleStorageDataValidationStatus.Error_ScaleStorageDataInvalidCharacters;

            // When.
            ArrayList result = ScaleStorageDataDAO.ValidateStorageData(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1, "Validation result should have contained one item");
            Assert.AreEqual(expected, (ScaleStorageDataValidationStatus)result[0]);
        }

        [TestMethod]
        public void ScaleStorageDataDAO_ValidateData_WhenInvalidCharInMultipleFields_ReturnsAllErrorCodes()
        {
            // Given.
            var testSignAttributes = testValues;
            testValues.Description = "No p|pes allowed";
            testValues.StorageData = "No p|pes allowed";

            // When.
            ArrayList result = ScaleStorageDataDAO.ValidateStorageData(testSignAttributes);

            // Then.
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        private StorageDataBO testValues = new StorageDataBO
        {
            ID = 10001,
            Description = "Test Description",
            StorageData = "a whole BUNCH of exciting Storage Data!!"
        };
    }
}