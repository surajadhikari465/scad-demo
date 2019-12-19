using OOSCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NLog;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for IOOSLogTest and is intended
    ///to contain all IOOSLogTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IOOSLogTest
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


        internal virtual IOOSLog CreateIOOSLog()
        {
            // TODO: Instantiate an appropriate concrete class.
            IOOSLog target = null;
            return target;
        }

        /// <summary>
        ///A test for Debug
        ///</summary>
        [TestMethod()]
        public void DebugTest()
        {
            IOOSLog target = CreateIOOSLog(); // TODO: Initialize to an appropriate value
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
            IOOSLog target = CreateIOOSLog(); // TODO: Initialize to an appropriate value
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
            IOOSLog target = CreateIOOSLog(); // TODO: Initialize to an appropriate value
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
            IOOSLog target = CreateIOOSLog(); // TODO: Initialize to an appropriate value
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
            IOOSLog target = CreateIOOSLog(); // TODO: Initialize to an appropriate value
            LogLevel level = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            target.Log(level, message);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Trace
        ///</summary>
        [TestMethod()]
        public void TraceTest()
        {
            IOOSLog target = CreateIOOSLog(); // TODO: Initialize to an appropriate value
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
            IOOSLog target = CreateIOOSLog(); // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            target.Warn(message);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
