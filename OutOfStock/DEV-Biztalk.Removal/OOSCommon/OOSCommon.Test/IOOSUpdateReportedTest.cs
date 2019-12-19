using OOSCommon.Import;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for IOOSUpdateReportedTest and is intended
    ///to contain all IOOSUpdateReportedTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IOOSUpdateReportedTest
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


        internal virtual IOOSUpdateReported CreateIOOSUpdateReported()
        {
            // TODO: Instantiate an appropriate concrete class.
            IOOSUpdateReported target = null;
            return target;
        }

        /// <summary>
        ///A test for BeginBatch
        ///</summary>
        [TestMethod()]
        public void BeginBatchTest()
        {
            IOOSUpdateReported target = CreateIOOSUpdateReported(); // TODO: Initialize to an appropriate value
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
            IOOSUpdateReported target = CreateIOOSUpdateReported(); // TODO: Initialize to an appropriate value
            string upc = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.WriteUPC(upc);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
