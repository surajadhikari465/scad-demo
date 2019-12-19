using OOSCommon.VIM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for VimSubTeamTest and is intended
    ///to contain all VimSubTeamTest Unit Tests
    ///</summary>
    [TestClass()]
    public class VimSubTeamTest
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
        ///A test for VimSubTeam Constructor
        ///</summary>
        [TestMethod()]
        public void VimSubTeamConstructorTest()
        {
            VimSubTeam target = new VimSubTeam();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for MakeOracleQuery
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSCommon.dll")]
        public void MakeOracleQueryTest()
        {
            string region = string.Empty; // TODO: Initialize to an appropriate value
            List<string> storeNumbers = null; // TODO: Initialize to an appropriate value
            List<string> teams = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = VimSubTeam_Accessor.MakeOracleQuery(region, storeNumbers, teams);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for RunQuery
        ///</summary>
        [TestMethod()]
        public void RunQueryTest()
        {
            string region = string.Empty; // TODO: Initialize to an appropriate value
            List<string> storeNumbers = null; // TODO: Initialize to an appropriate value
            List<string> teams = null; // TODO: Initialize to an appropriate value
            IEnumerable<VimSubTeam> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<VimSubTeam> actual;
            actual = VimSubTeam.RunQuery(region, storeNumbers, teams);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for subteam_name
        ///</summary>
        [TestMethod()]
        public void subteam_nameTest()
        {
            VimSubTeam target = new VimSubTeam(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.subteam_name = expected;
            actual = target.subteam_name;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
