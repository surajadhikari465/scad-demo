using OOSCommon.Import;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OOSCommon.DataContext;
using OOSCommon;
using OOSCommon.VIM;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for OOSUpdateReportedTest and is intended
    ///to contain all OOSUpdateReportedTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OOSUpdateReportedTest
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
        ///A test for OOSUpdateReported Constructor
        ///</summary>
        [TestMethod()]
        public void OOSUpdateReportedConstructorTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            OOSUpdateReported target = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for BeginBatch
        ///</summary>
        [TestMethod()]
        public void BeginBatchTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            OOSUpdateReported target = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            DateTime dtScan = new DateTime(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.BeginBatch(dtScan);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WriteUPC
        ///</summary>
        [TestMethod()]
        public void WriteUPCTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            OOSUpdateReported target = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            string upc = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.WriteUPC(upc);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for db
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSCommon.dll")]
        public void dbTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            OOSUpdateReported_Accessor target = new OOSUpdateReported_Accessor(param0); // TODO: Initialize to an appropriate value
            OOSEntities expected = null; // TODO: Initialize to an appropriate value
            OOSEntities actual;
            target.db = expected;
            actual = target.db;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for dtScan
        ///</summary>
        [TestMethod()]
        public void dtScanTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            OOSUpdateReported target = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            target.dtScan = expected;
            actual = target.dtScan;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for isValidationMode
        ///</summary>
        [TestMethod()]
        public void isValidationModeTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            OOSUpdateReported target = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.isValidationMode = expected;
            actual = target.isValidationMode;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for itemCount
        ///</summary>
        [TestMethod()]
        public void itemCountTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            OOSUpdateReported target = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.itemCount = expected;
            actual = target.itemCount;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for logging
        ///</summary>
        [TestMethod()]
        public void loggingTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            OOSUpdateReported target = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            IOOSLog expected = null; // TODO: Initialize to an appropriate value
            IOOSLog actual;
            target.logging = expected;
            actual = target.logging;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for oosEFConnectionString
        ///</summary>
        [TestMethod()]
        public void oosEFConnectionStringTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            OOSUpdateReported target = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.oosEFConnectionString = expected;
            actual = target.oosEFConnectionString;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for recordCount
        ///</summary>
        [TestMethod()]
        public void recordCountTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            OOSUpdateReported target = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.recordCount = expected;
            actual = target.recordCount;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for reportHeader
        ///</summary>
        [TestMethod()]
        public void reportHeaderTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            OOSUpdateReported target = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            REPORT_HEADER expected = null; // TODO: Initialize to an appropriate value
            REPORT_HEADER actual;
            target.reportHeader = expected;
            actual = target.reportHeader;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for store
        ///</summary>
        [TestMethod()]
        public void storeTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            OOSUpdateReported target = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            STORE expected = null; // TODO: Initialize to an appropriate value
            STORE actual;
            target.store = expected;
            actual = target.store;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for vimRepository
        ///</summary>
        [TestMethod()]
        public void vimRepositoryTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            OOSUpdateReported target = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            IVIMRepository expected = null; // TODO: Initialize to an appropriate value
            IVIMRepository actual;
            target.vimRepository = expected;
            actual = target.vimRepository;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
