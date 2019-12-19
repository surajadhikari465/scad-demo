using OOSCommon.Import;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OOSCommon.DataContext;
using OOSCommon;
using OOSCommon.VIM;
using System.IO;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for UPCImportTest and is intended
    ///to contain all UPCImportTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UPCImportTest
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
        ///A test for UPCImport Constructor
        ///</summary>
        [TestMethod()]
        public void UPCImportConstructorTest()
        {
            Nullable<DateTime> scanDate = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            UPCImport target = new UPCImport(scanDate, store, isValidationMode, logging, vimRepository, oosEFConnectionString);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Import
        ///</summary>
        [TestMethod()]
        public void ImportTest()
        {
            Nullable<DateTime> scanDate = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            UPCImport target = new UPCImport(scanDate, store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            string filePath = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Import(filePath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ImportNextBatch
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSCommon.dll")]
        public void ImportNextBatchTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            UPCImport_Accessor target = new UPCImport_Accessor(param0); // TODO: Initialize to an appropriate value
            StreamReader fileStream = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ImportNextBatch(fileStream);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsMyFormat
        ///</summary>
        [TestMethod()]
        public void IsMyFormatTest()
        {
            Nullable<DateTime> scanDate = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            UPCImport target = new UPCImport(scanDate, store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            string filePath = string.Empty; // TODO: Initialize to an appropriate value
            OOSImportIsMyFormat expected = new OOSImportIsMyFormat(); // TODO: Initialize to an appropriate value
            OOSImportIsMyFormat actual;
            actual = target.IsMyFormat(filePath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for isValidationMode
        ///</summary>
        [TestMethod()]
        public void isValidationModeTest()
        {
            Nullable<DateTime> scanDate = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            UPCImport target = new UPCImport(scanDate, store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
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
            Nullable<DateTime> scanDate = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            UPCImport target = new UPCImport(scanDate, store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
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
            Nullable<DateTime> scanDate = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            UPCImport target = new UPCImport(scanDate, store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
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
            Nullable<DateTime> scanDate = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            UPCImport target = new UPCImport(scanDate, store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.oosEFConnectionString = expected;
            actual = target.oosEFConnectionString;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for outputData
        ///</summary>
        [TestMethod()]
        public void outputDataTest()
        {
            Nullable<DateTime> scanDate = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            UPCImport target = new UPCImport(scanDate, store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            OOSUpdateReported expected = null; // TODO: Initialize to an appropriate value
            OOSUpdateReported actual;
            target.outputData = expected;
            actual = target.outputData;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for recordCount
        ///</summary>
        [TestMethod()]
        public void recordCountTest()
        {
            Nullable<DateTime> scanDate = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            UPCImport target = new UPCImport(scanDate, store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.recordCount = expected;
            actual = target.recordCount;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for scanDate
        ///</summary>
        [TestMethod()]
        public void scanDateTest()
        {
            Nullable<DateTime> scanDate = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            UPCImport target = new UPCImport(scanDate, store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.scanDate = expected;
            actual = target.scanDate;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for store
        ///</summary>
        [TestMethod()]
        public void storeTest()
        {
            Nullable<DateTime> scanDate = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            UPCImport target = new UPCImport(scanDate, store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
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
            Nullable<DateTime> scanDate = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            UPCImport target = new UPCImport(scanDate, store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            IVIMRepository expected = null; // TODO: Initialize to an appropriate value
            IVIMRepository actual;
            target.vimRepository = expected;
            actual = target.vimRepository;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
