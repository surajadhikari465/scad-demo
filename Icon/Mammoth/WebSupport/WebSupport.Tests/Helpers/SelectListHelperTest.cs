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
    public class SelectListHelperTest
    {
        protected PriceResetTestData testData = new PriceResetTestData();

        [TestMethod]
        public void SelectListHelper_ArrayToSelectList_WhenGivenAnArrayOfInts_CreatesSelectList()
        {
            //Arrange
            var data = new int[8] { 5, 230, 2039, 4398, 234098432, 1, -234, -234 };
            //Act
            var selectList = SelectListHelper.ArrayToSelectList(data);
            //Assert
            Assert.IsInstanceOfType(selectList, typeof(IEnumerable<SelectListItem>));
            Assert.AreEqual(8, selectList.Count());
        }

        [TestMethod]
        public void SelectListHelper_ArrayToSelectList_WhenGivenAnArrayOfStrings_CreatesSelectList()
        {
            //Arrange
            var data = new string[9] { "When", " in the ", "Course", "of", "human events", "it", "becomes", "necessary", "for one " };
            //Act
            var selectList = SelectListHelper.ArrayToSelectList(data);
            //Assert
            Assert.IsInstanceOfType(selectList, typeof(IEnumerable<SelectListItem>));
            Assert.AreEqual(9, selectList.Count());
        }

        [TestMethod]
        public void SelectListHelper_ArrayToSelectList_WhenGivenAnArrayOfCustomObjects_CreatesSelectList()
        {
            //Arrange
            var data = new TestClass[5] {
                new TestClass(1, 4.0M, "thing one"),
                new TestClass(2, -11.309M, "thing yeo"),
                new TestClass(333, 12.12M, "A.N. Other"),
                new TestClass(444, 9999.99999M, "jyn erso"),
                new TestClass(99999, -1000000M, "poop")
            };
            //Act
            var selectList = SelectListHelper.ArrayToSelectList(data);
            //Assert
            Assert.IsInstanceOfType(selectList, typeof(IEnumerable<SelectListItem>));
            Assert.AreEqual(5, selectList.Count());
        }

        [TestMethod]
        public void SelectListHelper_ArrayToSelectList_WhenGivenAnArrayOfInts_SetsTextAsDataValueToString()
        {
            //Arrange
            var data = new int[8] { 5, 230, 2039, 4398, 234098432, 1, -234, -234 };
            int indexOfSelected = 3;
            //Act
            var selectList = SelectListHelper.ArrayToSelectList(data, indexOfSelected);
            //Assert
            Assert.IsInstanceOfType(selectList, typeof(IEnumerable<SelectListItem>));
            var fourthItem = selectList.ElementAt(indexOfSelected);
            Assert.AreEqual(4398.ToString(), fourthItem.Text);
        }

        [TestMethod]
        public void SelectListHelper_ArrayToSelectList_WhenGivenAnArrayOfInts_SetsValueToIndexAsString()
        {
            //Arrange
            var data = new int[8] { 5, 230, 2039, 4398, 234098432, 1, -234, -234 };
            int indexOfSelected = 3;
            //Act
            var selectList = SelectListHelper.ArrayToSelectList(data, indexOfSelected);
            //Assert
            Assert.IsInstanceOfType(selectList, typeof(IEnumerable<SelectListItem>));
            var fourthItem = selectList.ElementAt(indexOfSelected);
            Assert.AreEqual(indexOfSelected.ToString(), fourthItem.Value);
        }

        [TestMethod]
        public void SelectListHelper_ArrayToSelectList_WhenGivenIdForSelectedItem_SelectListItemIsSelected()
        {
            //Arrange
            var data = new int[8] { 5, 230, 2039, 4398, 234098432, 1, -234, -234 };
            int indexOfSelected = 3;
            //Act
            var selectList = SelectListHelper.ArrayToSelectList(data, indexOfSelected);
            //Assert
            Assert.IsInstanceOfType(selectList, typeof(IEnumerable<SelectListItem>));
            var fourthItem = selectList.ElementAt(indexOfSelected);
            Assert.IsNotNull(fourthItem);
            Assert.IsTrue(fourthItem.Selected);
        }

        [TestMethod]
        public void SelectListHelper_StoreArrayToSelectList_CreatesSelectList()
        {
            //Arrange
            var storeData = testData.StoreViewModels;
            //Act
            var selectList = SelectListHelper.StoreArrayToSelectList(storeData);
            //Assert
            Assert.IsInstanceOfType(selectList, typeof(IEnumerable<SelectListItem>));
            Assert.AreEqual(5, selectList.Count());
        }

        [TestMethod]
        public void SelectListHelper_StoreArrayToSelectList_CreatesSelectList_WithExpectedText()
        {
            //Arrange
            var storeData = testData.StoreViewModels;
            //Act
            var selectList = SelectListHelper.StoreArrayToSelectList(storeData);
            //Assert
            Assert.AreEqual("10000: Test Store 1", selectList.ElementAt(0).Text);
            Assert.AreEqual("10010: the abc (ABC)", selectList.ElementAt(1).Text);
            Assert.AreEqual("10555: blah blah (BTH)", selectList.ElementAt(4).Text);
            Assert.AreEqual("11411: ahed31", selectList.ElementAt(3).Text);
            Assert.AreEqual("10100: nnnnnnnnnn", selectList.ElementAt(2).Text);
        }

        [TestMethod]
        public void SelectListHelper_StoreArrayToSelectList_NeverAllowsPreSelecting()
        {
            //Arrange
            var storeData = testData.StoreViewModels;
            var selectedIndex = 1;
            //Act
            var selectList = SelectListHelper.StoreArrayToSelectList(storeData);
            //Assert
            var selectedItem = selectList.ElementAt(selectedIndex);
            Assert.AreEqual("10010: the abc (ABC)", selectedItem.Text);
            Assert.IsFalse(selectedItem.Selected);
            Assert.IsFalse((selectList.ElementAt(0)).Selected);
            Assert.IsFalse((selectList.ElementAt(2)).Selected);
        }

        [TestMethod]
        public void SelectListHelper_StoreArrayToSelectList_SetsValueToStoreNumber()
        {
            //Arrange
            var storeData = testData.StoreViewModels;
            //Act
            var selectList = SelectListHelper.StoreArrayToSelectList(storeData);
            //Assert
            Assert.AreEqual(storeData.ElementAt(0).BusinessUnit, selectList.ElementAt(0).Value);
            Assert.AreEqual(storeData.ElementAt(1).BusinessUnit, selectList.ElementAt(1).Value);
            Assert.AreEqual(storeData.ElementAt(2).BusinessUnit, selectList.ElementAt(2).Value);
            Assert.AreEqual(storeData.ElementAt(3).BusinessUnit, selectList.ElementAt(3).Value);
            Assert.AreEqual(storeData.ElementAt(4).BusinessUnit, selectList.ElementAt(4).Value);
        }

        protected class TestClass
        {
            public TestClass() { }
            public TestClass(int ID, decimal value, string description): this()
            {
                this.id = ID;
                this.value = value;
                this.desc = description;
            }
            public int id { get; set; }
            public decimal value { get; set; }
            public string desc { get; set; }
        }
    }
}
