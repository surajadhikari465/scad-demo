using OOSCommon.Import;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for PSATest and is intended
    ///to contain all PSATest Unit Tests
    ///</summary>
    [TestClass()]
    public class PSATest
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
        ///A test for PSA Constructor
        ///</summary>
        [TestMethod()]
        public void PSAConstructorTest()
        {
            PSA.pSt lineState = new PSA.pSt(); // TODO: Initialize to an appropriate value
            PSA.pAct lineAction = new PSA.pAct(); // TODO: Initialize to an appropriate value
            PSA target = new PSA(lineState, lineAction);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ImportNextLine
        ///</summary>
        [TestMethod()]
        public void ImportNextLineTest()
        {
            string line = string.Empty; // TODO: Initialize to an appropriate value
            string[] expected = null; // TODO: Initialize to an appropriate value
            string[] actual;
            actual = PSA.ImportNextLine(line);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for lineAction
        ///</summary>
        [TestMethod()]
        public void lineActionTest()
        {
            PSA.pSt lineState = new PSA.pSt(); // TODO: Initialize to an appropriate value
            PSA.pAct lineAction = new PSA.pAct(); // TODO: Initialize to an appropriate value
            PSA target = new PSA(lineState, lineAction); // TODO: Initialize to an appropriate value
            PSA.pAct expected = new PSA.pAct(); // TODO: Initialize to an appropriate value
            PSA.pAct actual;
            target.lineAction = expected;
            actual = target.lineAction;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for lineState
        ///</summary>
        [TestMethod()]
        public void lineStateTest()
        {
            PSA.pSt lineState = new PSA.pSt(); // TODO: Initialize to an appropriate value
            PSA.pAct lineAction = new PSA.pAct(); // TODO: Initialize to an appropriate value
            PSA target = new PSA(lineState, lineAction); // TODO: Initialize to an appropriate value
            PSA.pSt expected = new PSA.pSt(); // TODO: Initialize to an appropriate value
            PSA.pSt actual;
            target.lineState = expected;
            actual = target.lineState;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for stateTransitions
        ///</summary>
        [TestMethod()]
        public void stateTransitionsTest()
        {
            PSA[,] expected = null; // TODO: Initialize to an appropriate value
            PSA[,] actual;
            PSA.stateTransitions = expected;
            actual = PSA.stateTransitions;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
