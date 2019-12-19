using OOSCommon.VIM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for VIMOOSItemDataViewTest and is intended
    ///to contain all VIMOOSItemDataViewTest Unit Tests
    ///</summary>
    [TestClass()]
    public class VIMOOSItemDataViewTest
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
        ///A test for VIMOOSItemDataView Constructor
        ///</summary>
        [TestMethod()]
        public void VIMOOSItemDataViewConstructorTest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for RunQuery
        ///</summary>
        [TestMethod()]
        public void RunQueryTest()
        {
            string upc = string.Empty; // TODO: Initialize to an appropriate value
            string ps_bu = string.Empty; // TODO: Initialize to an appropriate value
            IEnumerable<VIMOOSItemDataView> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<VIMOOSItemDataView> actual;
            actual = VIMOOSItemDataView.RunQuery(upc, ps_bu);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CASE_SIZE
        ///</summary>
        [TestMethod()]
        public void CASE_SIZETest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.CASE_SIZE = expected;
            actual = target.CASE_SIZE;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for EFF_COST
        ///</summary>
        [TestMethod()]
        public void EFF_COSTTest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.EFF_COST = expected;
            actual = target.EFF_COST;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for EFF_PRICE
        ///</summary>
        [TestMethod()]
        public void EFF_PRICETest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> expected = new Nullable<Decimal>(); // TODO: Initialize to an appropriate value
            Nullable<Decimal> actual;
            target.EFF_PRICE = expected;
            actual = target.EFF_PRICE;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for EFF_PRICETYPE
        ///</summary>
        [TestMethod()]
        public void EFF_PRICETYPETest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.EFF_PRICETYPE = expected;
            actual = target.EFF_PRICETYPE;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PS_BU
        ///</summary>
        [TestMethod()]
        public void PS_BUTest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PS_BU = expected;
            actual = target.PS_BU;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PS_DEPT_TEAM
        ///</summary>
        [TestMethod()]
        public void PS_DEPT_TEAMTest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PS_DEPT_TEAM = expected;
            actual = target.PS_DEPT_TEAM;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PS_PROD_SUBTEAM
        ///</summary>
        [TestMethod()]
        public void PS_PROD_SUBTEAMTest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PS_PROD_SUBTEAM = expected;
            actual = target.PS_PROD_SUBTEAM;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SUBTEAM_NAME
        ///</summary>
        [TestMethod()]
        public void SUBTEAM_NAMETest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.SUBTEAM_NAME = expected;
            actual = target.SUBTEAM_NAME;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TEAM_NAME
        ///</summary>
        [TestMethod()]
        public void TEAM_NAMETest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.TEAM_NAME = expected;
            actual = target.TEAM_NAME;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UPC
        ///</summary>
        [TestMethod()]
        public void UPCTest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.UPC = expected;
            actual = target.UPC;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for VENDOR_KEY
        ///</summary>
        [TestMethod()]
        public void VENDOR_KEYTest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.VENDOR_KEY = expected;
            actual = target.VENDOR_KEY;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for VEND_ITEM_NUM
        ///</summary>
        [TestMethod()]
        public void VEND_ITEM_NUMTest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.VEND_ITEM_NUM = expected;
            actual = target.VEND_ITEM_NUM;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for V_AUTH_STATUS
        ///</summary>
        [TestMethod()]
        public void V_AUTH_STATUSTest()
        {
            VIMOOSItemDataView target = new VIMOOSItemDataView(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.V_AUTH_STATUS = expected;
            actual = target.V_AUTH_STATUS;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
