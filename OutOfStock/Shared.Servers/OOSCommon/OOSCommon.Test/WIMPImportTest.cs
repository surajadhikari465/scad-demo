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
    ///This is a test class for WIMPImportTest and is intended
    ///to contain all WIMPImportTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WIMPImportTest
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
        ///A test for WIMPImport Constructor
        ///</summary>
        [TestMethod()]
        public void WIMPImportConstructorTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            WIMPImport target = new WIMPImport(store, isValidationMode, logging, vimRepository, oosEFConnectionString);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Complete
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSCommon.dll")]
        public void CompleteTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            WIMPImport_Accessor target = new WIMPImport_Accessor(param0); // TODO: Initialize to an appropriate value
            target.Complete();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Import
        ///</summary>
        [TestMethod()]
        public void ImportTest()
        {
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            WIMPImport target = new WIMPImport(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
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
            WIMPImport_Accessor target = new WIMPImport_Accessor(param0); // TODO: Initialize to an appropriate value
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
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            WIMPImport target = new WIMPImport(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            string filePath = string.Empty; // TODO: Initialize to an appropriate value
            OOSImportIsMyFormat expected = new OOSImportIsMyFormat(); // TODO: Initialize to an appropriate value
            OOSImportIsMyFormat actual;
            actual = target.IsMyFormat(filePath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ValidateEndOfSection
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSCommon.dll")]
        public void ValidateEndOfSectionTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            WIMPImport_Accessor target = new WIMPImport_Accessor(param0); // TODO: Initialize to an appropriate value
            string[] record = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ValidateEndOfSection(record);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ValidateHeader
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSCommon.dll")]
        public void ValidateHeaderTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            WIMPImport_Accessor target = new WIMPImport_Accessor(param0); // TODO: Initialize to an appropriate value
            string[] record = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ValidateHeader(record);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ValidateItem
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSCommon.dll")]
        public void ValidateItemTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            WIMPImport_Accessor target = new WIMPImport_Accessor(param0); // TODO: Initialize to an appropriate value
            string[] record = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ValidateItem(record);
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
            WIMPImport target = new WIMPImport(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
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
            WIMPImport target = new WIMPImport(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
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
            WIMPImport target = new WIMPImport(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
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
            WIMPImport target = new WIMPImport(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
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
        [DeploymentItem("OOSCommon.dll")]
        public void outputDataTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            WIMPImport_Accessor target = new WIMPImport_Accessor(param0); // TODO: Initialize to an appropriate value
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
            STORE store = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            IOOSLog logging = null; // TODO: Initialize to an appropriate value
            IVIMRepository vimRepository = null; // TODO: Initialize to an appropriate value
            string oosEFConnectionString = string.Empty; // TODO: Initialize to an appropriate value
            WIMPImport target = new WIMPImport(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.recordCount = expected;
            actual = target.recordCount;
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
            WIMPImport target = new WIMPImport(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
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
            WIMPImport target = new WIMPImport(store, isValidationMode, logging, vimRepository, oosEFConnectionString); // TODO: Initialize to an appropriate value
            IVIMRepository expected = null; // TODO: Initialize to an appropriate value
            IVIMRepository actual;
            target.vimRepository = expected;
            actual = target.vimRepository;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
