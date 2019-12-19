using OOSCommon.VIM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for ItemMasterModelTest and is intended
    ///to contain all ItemMasterModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ItemMasterModelTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ItemMasterModel Constructor
        ///</summary>
        [TestMethod()]
        public void ItemMasterModelConstructorTest()
        {
            string NAT_UPC = string.Empty; // TODO: Initialize to an appropriate value
            string BRAND = string.Empty; // TODO: Initialize to an appropriate value
            string LONG_DESCRIPTION = string.Empty; // TODO: Initialize to an appropriate value
            string ITEM_SIZE = string.Empty; // TODO: Initialize to an appropriate value
            string ITEM_UOM = string.Empty; // TODO: Initialize to an appropriate value
            string CATEGORY_NAME = string.Empty; // TODO: Initialize to an appropriate value
            string CLASS_NAME = string.Empty; // TODO: Initialize to an appropriate value
            ItemMasterModel target = new ItemMasterModel(NAT_UPC, BRAND, LONG_DESCRIPTION, ITEM_SIZE, ITEM_UOM, CATEGORY_NAME, CLASS_NAME);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ItemMasterModel Constructor
        ///</summary>
        [TestMethod()]
        public void ItemMasterModelConstructorTest1()
        {
            ItemMasterModel target = new ItemMasterModel();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for RunQuery
        ///</summary>
        [TestMethod()]
        public void RunQueryTest()
        {
            IEnumerable<ItemMasterModel> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<ItemMasterModel> actual;
            actual = ItemMasterModel.RunQuery();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for RunQuery
        ///</summary>
        [TestMethod()]
        public void RunQueryTest1()
        {
            string upc = string.Empty; // TODO: Initialize to an appropriate value
            ItemMasterModel expected = null; // TODO: Initialize to an appropriate value
            ItemMasterModel actual;
            actual = ItemMasterModel.RunQuery(upc);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for RunQuery
        ///</summary>
        [TestMethod()]
        public void RunQueryTest2()
        {
            IEnumerable<string> upcs = null; // TODO: Initialize to an appropriate value
            Dictionary<string, ItemMasterModel> expected = null; // TODO: Initialize to an appropriate value
            Dictionary<string, ItemMasterModel> actual;
            actual = ItemMasterModel.RunQuery(upcs);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for BRAND
        ///</summary>
        [TestMethod()]
        public void BRANDTest()
        {
            ItemMasterModel target = new ItemMasterModel(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BRAND = expected;
            actual = target.BRAND;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CATEGORY_NAME
        ///</summary>
        [TestMethod()]
        public void CATEGORY_NAMETest()
        {
            ItemMasterModel target = new ItemMasterModel(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CATEGORY_NAME = expected;
            actual = target.CATEGORY_NAME;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CLASS_NAME
        ///</summary>
        [TestMethod()]
        public void CLASS_NAMETest()
        {
            ItemMasterModel target = new ItemMasterModel(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.CLASS_NAME = expected;
            actual = target.CLASS_NAME;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ITEM_SIZE
        ///</summary>
        [TestMethod()]
        public void ITEM_SIZETest()
        {
            ItemMasterModel target = new ItemMasterModel(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ITEM_SIZE = expected;
            actual = target.ITEM_SIZE;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ITEM_UOM
        ///</summary>
        [TestMethod()]
        public void ITEM_UOMTest()
        {
            ItemMasterModel target = new ItemMasterModel(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.ITEM_UOM = expected;
            actual = target.ITEM_UOM;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LONG_DESCRIPTION
        ///</summary>
        [TestMethod()]
        public void LONG_DESCRIPTIONTest()
        {
            ItemMasterModel target = new ItemMasterModel(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.LONG_DESCRIPTION = expected;
            actual = target.LONG_DESCRIPTION;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for NAT_UPC
        ///</summary>
        [TestMethod()]
        public void NAT_UPCTest()
        {
            ItemMasterModel target = new ItemMasterModel(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.NAT_UPC = expected;
            actual = target.NAT_UPC;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
