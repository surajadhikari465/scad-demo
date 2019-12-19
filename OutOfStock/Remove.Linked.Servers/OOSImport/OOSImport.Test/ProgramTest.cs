using OOSImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NLog;
using OOSCommon;
using OOSCommon.VIM;

namespace OOSImport.Test
{
    
    
    /// <summary>
    ///This is a test class for ProgramTest and is intended
    ///to contain all ProgramTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProgramTest
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
        ///A test for AttachConsole
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSImport.exe")]
        public void AttachConsoleTest()
        {
            int dwProcessId = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Program_Accessor.AttachConsole(dwProcessId);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Main
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSImport.exe")]
        public void MainTest()
        {
            string[] args = null; // TODO: Initialize to an appropriate value
            Program_Accessor.Main(args);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for OOSEchoEntry
        ///</summary>
        [TestMethod()]
        public void OOSEchoEntryTest()
        {
            LogLevel level = null; // TODO: Initialize to an appropriate value
            string timeStamp = string.Empty; // TODO: Initialize to an appropriate value
            string fileName = string.Empty; // TODO: Initialize to an appropriate value
            string lineNumberText = string.Empty; // TODO: Initialize to an appropriate value
            string methodName = string.Empty; // TODO: Initialize to an appropriate value
            string sessionID = string.Empty; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            Program.OOSEchoEntry(level, timeStamp, fileName, lineNumberText, methodName, sessionID, message);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for formOOSImportUI
        ///</summary>
        [TestMethod()]
        public void formOOSImportUITest()
        {
            OOSImportUI expected = null; // TODO: Initialize to an appropriate value
            OOSImportUI actual;
            Program.formOOSImportUI = expected;
            actual = Program.formOOSImportUI;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for nLogBasePath
        ///</summary>
        [TestMethod()]
        public void nLogBasePathTest()
        {
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            Program.nLogBasePath = expected;
            actual = Program.nLogBasePath;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for oosEFConnectionString
        ///</summary>
        [TestMethod()]
        public void oosEFConnectionStringTest()
        {
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            Program.oosEFConnectionString = expected;
            actual = Program.oosEFConnectionString;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for oosLogging
        ///</summary>
        [TestMethod()]
        public void oosLoggingTest()
        {
            IOOSLog expected = null; // TODO: Initialize to an appropriate value
            IOOSLog actual;
            Program.oosLogging = expected;
            actual = Program.oosLogging;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for vimRepository
        ///</summary>
        [TestMethod()]
        public void vimRepositoryTest()
        {
            VIMRepository expected = null; // TODO: Initialize to an appropriate value
            VIMRepository actual;
            Program.vimRepository = expected;
            actual = Program.vimRepository;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
