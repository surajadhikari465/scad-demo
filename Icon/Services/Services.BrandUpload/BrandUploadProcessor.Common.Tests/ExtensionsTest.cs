using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrandUploadProcessor.Common.Tests
{
    [TestClass]
    public class ExtensionsTest
    {


        [TestMethod]
        public void GetCellValue_DontAllowRemoveMismatchedCase_ReturnsNullIfValueIsRemove()
        {

            var columnIndex1 = 1;
            var columnIndex2 = 2;
            var cells = new Dictionary<int, string> { { 1, "remove" }, { 2, "Test" } };

            var result1 = Extensions.GetCellValue(columnIndex1, cells, false, "REMOVE");
            var result2 = Extensions.GetCellValue(columnIndex2, cells, false, "REMOVE");

            Assert.AreEqual(null, result1);
            Assert.AreNotEqual("REMOVE", result2);
        }

        [TestMethod]
        public void GetCellValue_AllowRemove_ReturnsRemoveIfValueIsRemove()
        {

            var columnIndex1 = 1;
            var columnIndex2 = 2;
            var cells = new Dictionary<int, string> {{1, "REMOVE"}, {2, "Test"}};

            var result1 = Extensions.GetCellValue(columnIndex1, cells, true, "REMOVE");
            var result2 = Extensions.GetCellValue(columnIndex2, cells, true, "REMOVE");

            Assert.AreEqual("REMOVE", result1);
            Assert.AreNotEqual("REMOVE", result2);
        }

        [TestMethod]
        public void GetCellValue_DontAllowRemove_ReturnsNullIfValueIsRemove()
        {

            var columnIndex1 = 1;
            var columnIndex2 = 2;
            var cells = new Dictionary<int, string> { { 1, "REMOVE" }, { 2, "Test" } };

            var result1 = Extensions.GetCellValue(columnIndex1, cells, false, "REMOVE");
            var result2 = Extensions.GetCellValue(columnIndex2, cells, false, "REMOVE");

            Assert.AreEqual(null, result1);
            Assert.AreNotEqual("REMOVE", result2);
        }


        [TestMethod]
        public void GetCellValue_IndexDoesntExist_ReturnsNull()
        {
            var columnIndex3 = 3;
            var cells = new Dictionary<int, string> { { 1, "REMOVE" }, { 2, "Test" } };

            var result1 = Extensions.GetCellValue(columnIndex3, cells, false, "REMOVE");
            Assert.AreEqual(null, result1);
            
        }
    }
}
