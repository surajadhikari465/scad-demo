using OOSCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for UtilityTest and is intended
    ///to contain all UtilityTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UtilityTest
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
        ///A test for Utility Constructor
        ///</summary>
        [TestMethod()]
        public void UtilityConstructorTest()
        {
            Utility target = new Utility();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for GetCurrentMethodName
        ///</summary>
        [TestMethod()]
        public void GetCurrentMethodNameTest()
        {
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = Utility.GetCurrentMethodName();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for UPCCheck
        ///</summary>
        [TestMethod()]
        public void UPCCheckTest()
        {
            string checkUPC = string.Empty; // TODO: Initialize to an appropriate value
            string vimUPC = string.Empty; // TODO: Initialize to an appropriate value
            string vimUPCExpected = string.Empty; // TODO: Initialize to an appropriate value
            Utility.eUPCCheck expected = new Utility.eUPCCheck(); // TODO: Initialize to an appropriate value
            Utility.eUPCCheck actual;
            actual = Utility.UPCCheck(checkUPC, out vimUPC);
            Assert.AreEqual(vimUPCExpected, vimUPC);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
