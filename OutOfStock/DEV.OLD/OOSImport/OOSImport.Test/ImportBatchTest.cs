using OOSImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OOSCommon;
using OOSCommon.DataContext;

namespace OOSImport.Test
{
    
    
    /// <summary>
    ///This is a test class for ImportBatchTest and is intended
    ///to contain all ImportBatchTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ImportBatchTest
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
        ///A test for ImportBatch Constructor
        ///</summary>
        [TestMethod()]
        public void ImportBatchConstructorTest()
        {
            string formatText = string.Empty; // TODO: Initialize to an appropriate value
            string dateText = string.Empty; // TODO: Initialize to an appropriate value
            string storeText = string.Empty; // TODO: Initialize to an appropriate value
            string fileName = string.Empty; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            Moq.Mock<OOSLog> loggingMock = new Moq.Mock<OOSLog>(string.Empty, string.Empty, null, null);
            IOOSLog logging = loggingMock.Object;
            ImportBatch target = new ImportBatch(formatText, dateText, storeText, fileName, isValidationMode, logging);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ImportBatch Constructor
        ///</summary>
        [TestMethod()]
        public void ImportBatchConstructorTest1()
        {
            string[] args = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            Moq.Mock<OOSLog> loggingMock = new Moq.Mock<OOSLog>(string.Empty, string.Empty, null, null);
            IOOSLog logging = loggingMock.Object;
            ImportBatch target = new ImportBatch(args, isValidationMode, logging);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ImportNextFile
        ///</summary>
        [TestMethod()]
        public void ImportNextFileTest()
        {
            string[] args = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            Moq.Mock<OOSLog> loggingMock = new Moq.Mock<OOSLog>(string.Empty, string.Empty, null, null);
            IOOSLog logging = loggingMock.Object;
            ImportBatch target = new ImportBatch(args, isValidationMode, logging); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ImportNextFile();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for InterpretOptions
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSImport.exe")]
        public void InterpretOptionsTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            ImportBatch_Accessor target = new ImportBatch_Accessor(param0); // TODO: Initialize to an appropriate value
            target.InterpretOptions();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for args
        ///</summary>
        [TestMethod()]
        public void argsTest()
        {
            string[] args = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            Moq.Mock<OOSLog> loggingMock = new Moq.Mock<OOSLog>(string.Empty, string.Empty, null, null);
            IOOSLog logging = loggingMock.Object;
            ImportBatch target = new ImportBatch(args, isValidationMode, logging); // TODO: Initialize to an appropriate value
            string[] expected = null; // TODO: Initialize to an appropriate value
            string[] actual;
            target.args = expected;
            actual = target.args;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for currentArg
        ///</summary>
        [TestMethod()]
        public void currentArgTest()
        {
            string[] args = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            Moq.Mock<OOSLog> loggingMock = new Moq.Mock<OOSLog>(string.Empty, string.Empty, null, null);
            IOOSLog logging = loggingMock.Object;
            ImportBatch target = new ImportBatch(args, isValidationMode, logging); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.currentArg = expected;
            actual = target.currentArg;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for currentFormat
        ///</summary>
        [TestMethod()]
        public void currentFormatTest()
        {
            string[] args = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            Moq.Mock<OOSLog> loggingMock = new Moq.Mock<OOSLog>(string.Empty, string.Empty, null, null);
            IOOSLog logging = loggingMock.Object;
            ImportBatch target = new ImportBatch(args, isValidationMode, logging); // TODO: Initialize to an appropriate value
            ImportBatch.OOSFormat expected = new ImportBatch.OOSFormat(); // TODO: Initialize to an appropriate value
            ImportBatch.OOSFormat actual;
            target.currentFormat = expected;
            actual = target.currentFormat;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for currentScanDate
        ///</summary>
        [TestMethod()]
        public void currentScanDateTest()
        {
            string[] args = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            Moq.Mock<OOSLog> loggingMock = new Moq.Mock<OOSLog>(string.Empty, string.Empty, null, null);
            IOOSLog logging = loggingMock.Object;
            ImportBatch target = new ImportBatch(args, isValidationMode, logging); // TODO: Initialize to an appropriate value
            Nullable<DateTime> expected = new Nullable<DateTime>(); // TODO: Initialize to an appropriate value
            Nullable<DateTime> actual;
            target.currentScanDate = expected;
            actual = target.currentScanDate;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for currentStore
        ///</summary>
        [TestMethod()]
        public void currentStoreTest()
        {
            string[] args = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            Moq.Mock<OOSLog> loggingMock = new Moq.Mock<OOSLog>(string.Empty, string.Empty, null, null);
            IOOSLog logging = loggingMock.Object;
            ImportBatch target = new ImportBatch(args, isValidationMode, logging); // TODO: Initialize to an appropriate value
            STORE expected = null; // TODO: Initialize to an appropriate value
            STORE actual;
            target.currentStore = expected;
            actual = target.currentStore;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for db
        ///</summary>
        [TestMethod()]
        public void dbTest()
        {
            string[] args = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            Moq.Mock<OOSLog> loggingMock = new Moq.Mock<OOSLog>(string.Empty, string.Empty, null, null);
            IOOSLog logging = loggingMock.Object;
            ImportBatch target = new ImportBatch(args, isValidationMode, logging); // TODO: Initialize to an appropriate value
            OOSEntities expected = null; // TODO: Initialize to an appropriate value
            OOSEntities actual;
            target.db = expected;
            actual = target.db;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for isValidationMode
        ///</summary>
        [TestMethod()]
        public void isValidationModeTest()
        {
            string[] args = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            Moq.Mock<OOSLog> loggingMock = new Moq.Mock<OOSLog>(string.Empty, string.Empty, null, null);
            IOOSLog logging = loggingMock.Object;
            ImportBatch target = new ImportBatch(args, isValidationMode, logging); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.isValidationMode = expected;
            actual = target.isValidationMode;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for logging
        ///</summary>
        [TestMethod()]
        public void loggingTest()
        {
            string[] args = null; // TODO: Initialize to an appropriate value
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            Moq.Mock<OOSLog> loggingMock = new Moq.Mock<OOSLog>(string.Empty, string.Empty, null, null);
            IOOSLog logging = loggingMock.Object;
            ImportBatch target = new ImportBatch(args, isValidationMode, logging); // TODO: Initialize to an appropriate value
            IOOSLog expected = null; // TODO: Initialize to an appropriate value
            IOOSLog actual;
            target.logging = expected;
            actual = target.logging;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
