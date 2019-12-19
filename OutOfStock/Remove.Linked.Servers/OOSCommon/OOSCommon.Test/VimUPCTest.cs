using OOSCommon.VIM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for VimUPCTest and is intended
    ///to contain all VimUPCTest Unit Tests
    ///</summary>
    [TestClass()]
    public class VimUPCTest
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
        ///A test for VimUPC Constructor
        ///</summary>
        [TestMethod()]
        public void VimUPCConstructorTest()
        {
            VimUPC target = new VimUPC();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for MakeSQLQuery
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSCommon.dll")]
        public void MakeSQLQueryTest()
        {
            string upc = string.Empty; // TODO: Initialize to an appropriate value
            string userRegion = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = VimUPC_Accessor.MakeSQLQuery(upc, userRegion);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for RunQuery
        ///</summary>
        [TestMethod()]
        public void RunQueryTest()
        {
            string upc = string.Empty; // TODO: Initialize to an appropriate value
            string userRegion = string.Empty; // TODO: Initialize to an appropriate value
            IEnumerable<VimUPC> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<VimUPC> actual;
            actual = VimUPC.RunQuery(upc, userRegion);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UPC
        ///</summary>
        [TestMethod()]
        public void UPCTest()
        {
            VimUPC target = new VimUPC(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.UPC = expected;
            actual = target.UPC;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
