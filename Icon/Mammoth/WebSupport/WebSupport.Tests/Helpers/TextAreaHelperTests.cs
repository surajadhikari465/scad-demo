using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebSupport.Helpers;
using WebSupport.Tests.TestData;
using WebSupport.ViewModels;

namespace WebSupport.Tests.Helpers
{
    [TestClass]
    public class TextAreaHelperTests
    {

        [TestMethod]
        public void TextAreaHelper_GetRowsByListCount_ReturnsDefaultListCountWhenZero()
        {
            //Arrange
            int listCount = 0;
            int expectedRows = TextAreaHelper.DefaultRows;

            //Act
            var actualRows = TextAreaHelper.GetRowsByListCount(listCount);

            //Assert
            Assert.AreEqual(expectedRows, actualRows);
        }

        [TestMethod]
        public void TextAreaHelper_GetRowsByListCount_ReturnsDefaultListCountWhenBelowDefault()
        {
            //Arrange
            int listCount = 19;
            int expectedRows = TextAreaHelper.DefaultRows;

            //Act
            var actualRows = TextAreaHelper.GetRowsByListCount(listCount);

            //Assert
            Assert.AreEqual(expectedRows, actualRows);
        }

        [TestMethod]
        public void TextAreaHelper_GetRowsByListCount_ReturnsListCountWhenBetweenDefaultAndMax()
        {
            //Arrange
            int listCount = 33;
            int expectedRows = 33;

            //Act
            var actualRows = TextAreaHelper.GetRowsByListCount(listCount);

            //Assert
            Assert.AreEqual(expectedRows, actualRows);
        }

        [TestMethod]
        public void TextAreaHelper_GetRowsByListCount_LimitsToMaxWhenGreaterThanMax()
        {
            //Arrange
            int listCount = 50;
            int expectedRows = TextAreaHelper.MaxRows;

            //Act
            var actualRows = TextAreaHelper.GetRowsByListCount(listCount);

            //Assert
            Assert.AreEqual(expectedRows, actualRows);
        }
    }
}
