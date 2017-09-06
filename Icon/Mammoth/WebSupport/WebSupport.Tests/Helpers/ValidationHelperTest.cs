using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.Helpers;
using WebSupport.Tests.TestData;

namespace WebSupport.Tests.Helpers
{
    [TestClass]
    public class ValidationHelperTest
    {
        [TestMethod]
        public void ValidationHelper_ParseScanCodes_WhenGivenValidListOfScanCodes_ParsesToAList()
        {
            //Arrange
            string scanCodeData = PriceResetTestData.NewlineSeparatedScanCodes;
            //Act
            var parsedList = ValidationHelper.ParseScanCodes(scanCodeData);
            //Assert
            Assert.IsNotNull(parsedList);
            Assert.AreEqual(15, parsedList.Count());
        }

        [TestMethod]
        public void ValidationHelper_ParseScanCodes_IgnoresBlankLines()
        {
            //Arrange
            string scanCodeData = PriceResetTestData.NewlineSeparatedScanCodesWithBlankLinesAndWhitespace;
            //Act
            var parsedList = ValidationHelper.ParseScanCodes(scanCodeData);
            //Assert
            Assert.IsNotNull(parsedList);
            Assert.AreEqual(15, parsedList.Count());
        }

        [TestMethod]
        public void ValidationHelper_ParseScanCodes_DoesNotValidate()
        {
            //Arrange
            string scanCodeData =
@"46000063225
invalidScanCode 
81387901082";
            //Act
            var parsedList = ValidationHelper.ParseScanCodes(scanCodeData);
            //Assert
            Assert.IsNotNull(parsedList);
            Assert.AreEqual(3, parsedList.Count());
        }
    }
}
