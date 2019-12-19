using OOSCommon.Import;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for IOOSImportReportedTest and is intended
    ///to contain all IOOSImportReportedTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IOOSImportReportedTest
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


        internal virtual IOOSImportReported CreateIOOSImportReported()
        {
            // TODO: Instantiate an appropriate concrete class.
            IOOSImportReported target = null;
            return target;
        }

        /// <summary>
        ///A test for Import
        ///</summary>
        [TestMethod()]
        public void ImportTest()
        {
            IOOSImportReported target = CreateIOOSImportReported(); // TODO: Initialize to an appropriate value
            string filePath = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Import(filePath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsMyFormat
        ///</summary>
        [TestMethod()]
        public void IsMyFormatTest()
        {
            IOOSImportReported target = CreateIOOSImportReported(); // TODO: Initialize to an appropriate value
            string filePath = string.Empty; // TODO: Initialize to an appropriate value
            OOSImportIsMyFormat expected = new OOSImportIsMyFormat(); // TODO: Initialize to an appropriate value
            OOSImportIsMyFormat actual;
            actual = target.IsMyFormat(filePath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
