using OOSCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NLog;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for OOSLogTest and is intended
    ///to contain all OOSLogTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OOSLogTest
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
        ///A test for OOSLog Constructor
        ///</summary>
        [TestMethod()]
        public void OOSLogConstructorTest()
        {
            string oosNLogLoggerName = string.Empty; // TODO: Initialize to an appropriate value
            string oosBasePath = string.Empty; // TODO: Initialize to an appropriate value
            OOSLog.GetSessionId getSessionId = null; // TODO: Initialize to an appropriate value
            OOSLog.EchoEntry echoEntry = null; // TODO: Initialize to an appropriate value
            OOSLog target = new OOSLog(oosNLogLoggerName, oosBasePath, getSessionId, echoEntry);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Debug
        ///</summary>
        [TestMethod()]
        public void DebugTest()
        {
            string oosNLogLoggerName = string.Empty; // TODO: Initialize to an appropriate value
            string oosBasePath = string.Empty; // TODO: Initialize to an appropriate value
            OOSLog.GetSessionId getSessionId = null; // TODO: Initialize to an appropriate value
            OOSLog.EchoEntry echoEntry = null; // TODO: Initialize to an appropriate value
            OOSLog target = new OOSLog(oosNLogLoggerName, oosBasePath, getSessionId, echoEntry); // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            target.Debug(message);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Error
        ///</summary>
        [TestMethod()]
        public void ErrorTest()
        {
            string oosNLogLoggerName = string.Empty; // TODO: Initialize to an appropriate value
            string oosBasePath = string.Empty; // TODO: Initialize to an appropriate value
            OOSLog.GetSessionId getSessionId = null; // TODO: Initialize to an appropriate value
            OOSLog.EchoEntry echoEntry = null; // TODO: Initialize to an appropriate value
            OOSLog target = new OOSLog(oosNLogLoggerName, oosBasePath, getSessionId, echoEntry); // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            target.Error(message);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Fatal
        ///</summary>
        [TestMethod()]
        public void FatalTest()
        {
            string oosNLogLoggerName = string.Empty; // TODO: Initialize to an appropriate value
            string oosBasePath = string.Empty; // TODO: Initialize to an appropriate value
            OOSLog.GetSessionId getSessionId = null; // TODO: Initialize to an appropriate value
            OOSLog.EchoEntry echoEntry = null; // TODO: Initialize to an appropriate value
            OOSLog target = new OOSLog(oosNLogLoggerName, oosBasePath, getSessionId, echoEntry); // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            target.Fatal(message);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Info
        ///</summary>
        [TestMethod()]
        public void InfoTest()
        {
            string oosNLogLoggerName = string.Empty; // TODO: Initialize to an appropriate value
            string oosBasePath = string.Empty; // TODO: Initialize to an appropriate value
            OOSLog.GetSessionId getSessionId = null; // TODO: Initialize to an appropriate value
            OOSLog.EchoEntry echoEntry = null; // TODO: Initialize to an appropriate value
            OOSLog target = new OOSLog(oosNLogLoggerName, oosBasePath, getSessionId, echoEntry); // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            target.Info(message);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Log
        ///</summary>
        [TestMethod()]
        public void LogTest()
        {
            string oosNLogLoggerName = string.Empty; // TODO: Initialize to an appropriate value
            string oosBasePath = string.Empty; // TODO: Initialize to an appropriate value
            OOSLog.GetSessionId getSessionId = null; // TODO: Initialize to an appropriate value
            OOSLog.EchoEntry echoEntry = null; // TODO: Initialize to an appropriate value
            OOSLog target = new OOSLog(oosNLogLoggerName, oosBasePath, getSessionId, echoEntry); // TODO: Initialize to an appropriate value
            LogLevel level = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            target.Log(level, message);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LogInner
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSCommon.dll")]
        public void LogInnerTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            OOSLog_Accessor target = new OOSLog_Accessor(param0); // TODO: Initialize to an appropriate value
            LogLevel level = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            target.LogInner(level, message);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Trace
        ///</summary>
        [TestMethod()]
        public void TraceTest()
        {
            string oosNLogLoggerName = string.Empty; // TODO: Initialize to an appropriate value
            string oosBasePath = string.Empty; // TODO: Initialize to an appropriate value
            OOSLog.GetSessionId getSessionId = null; // TODO: Initialize to an appropriate value
            OOSLog.EchoEntry echoEntry = null; // TODO: Initialize to an appropriate value
            OOSLog target = new OOSLog(oosNLogLoggerName, oosBasePath, getSessionId, echoEntry); // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            target.Trace(message);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Warn
        ///</summary>
        [TestMethod()]
        public void WarnTest()
        {
            string oosNLogLoggerName = string.Empty; // TODO: Initialize to an appropriate value
            string oosBasePath = string.Empty; // TODO: Initialize to an appropriate value
            OOSLog.GetSessionId getSessionId = null; // TODO: Initialize to an appropriate value
            OOSLog.EchoEntry echoEntry = null; // TODO: Initialize to an appropriate value
            OOSLog target = new OOSLog(oosNLogLoggerName, oosBasePath, getSessionId, echoEntry); // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            target.Warn(message);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for echoEntry
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSCommon.dll")]
        public void echoEntryTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            OOSLog_Accessor target = new OOSLog_Accessor(param0); // TODO: Initialize to an appropriate value
            OOSLog.EchoEntry expected = null; // TODO: Initialize to an appropriate value
            OOSLog.EchoEntry actual;
            target.echoEntry = expected;
            actual = target.echoEntry;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for getSessionId
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSCommon.dll")]
        public void getSessionIdTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            OOSLog_Accessor target = new OOSLog_Accessor(param0); // TODO: Initialize to an appropriate value
            OOSLog.GetSessionId expected = null; // TODO: Initialize to an appropriate value
            OOSLog.GetSessionId actual;
            target.getSessionId = expected;
            actual = target.getSessionId;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for oosBasePath
        ///</summary>
        [TestMethod()]
        [DeploymentItem("OOSCommon.dll")]
        public void oosBasePathTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            OOSLog_Accessor target = new OOSLog_Accessor(param0); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.oosBasePath = expected;
            actual = target.oosBasePath;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for oosLogger
        ///</summary>
        [TestMethod()]
        public void oosLoggerTest()
        {
            string oosNLogLoggerName = string.Empty; // TODO: Initialize to an appropriate value
            string oosBasePath = string.Empty; // TODO: Initialize to an appropriate value
            OOSLog.GetSessionId getSessionId = null; // TODO: Initialize to an appropriate value
            OOSLog.EchoEntry echoEntry = null; // TODO: Initialize to an appropriate value
            OOSLog target = new OOSLog(oosNLogLoggerName, oosBasePath, getSessionId, echoEntry); // TODO: Initialize to an appropriate value
            Logger expected = null; // TODO: Initialize to an appropriate value
            Logger actual;
            target.oosLogger = expected;
            actual = target.oosLogger;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
