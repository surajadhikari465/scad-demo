using OOSCommon.Import;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for OOSImportKnownUNFITest and is intended
    ///to contain all OOSImportKnownUNFITest Unit Tests
    ///</summary>
    [TestClass()]
    public class OOSImportKnownUNFITest
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
        ///A test for GetKnownOOSFromUNFI
        ///</summary>
        [TestMethod()]
        public void GetKnownOOSFromUNFITest()
        {
            string filePath = string.Empty; // TODO: Initialize to an appropriate value
            List<OOSImportKnownUNFI.ItemData> itemData = null; // TODO: Initialize to an appropriate value
            List<OOSImportKnownUNFI.ItemData> itemDataExpected = null; // TODO: Initialize to an appropriate value
            List<OOSImportKnownUNFI.VendorRegionMap> vendorRegionMap = null; // TODO: Initialize to an appropriate value
            List<OOSImportKnownUNFI.VendorRegionMap> vendorRegionMapExpected = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = OOSImportKnownUNFI.GetKnownOOSFromUNFI(filePath, out itemData, out vendorRegionMap);
            Assert.AreEqual(itemDataExpected, itemData);
            Assert.AreEqual(vendorRegionMapExpected, vendorRegionMap);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
