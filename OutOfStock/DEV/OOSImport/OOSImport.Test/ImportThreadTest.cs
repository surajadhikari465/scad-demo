using OOSImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OOSImport.Test
{
    
    
    /// <summary>
    ///This is a test class for ImportThreadTest and is intended
    ///to contain all ImportThreadTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ImportThreadTest
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
        ///A test for ImportThread Constructor
        ///</summary>
        [TestMethod()]
        public void ImportThreadConstructorTest()
        {
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            ListView.ListViewItemCollection itemsFromList = null; // TODO: Initialize to an appropriate value
            ImportThread target = new ImportThread(isValidationMode, itemsFromList);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for OnProgressChanged
        ///</summary>
        [TestMethod()]
        public void OnProgressChangedTest()
        {
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            ListView.ListViewItemCollection itemsFromList = null; // TODO: Initialize to an appropriate value
            ImportThread target = new ImportThread(isValidationMode, itemsFromList); // TODO: Initialize to an appropriate value
            ImportThreadProgressArgs e = null; // TODO: Initialize to an appropriate value
            target.OnProgressChanged(e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for StartWork
        ///</summary>
        [TestMethod()]
        public void StartWorkTest()
        {
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            ListView.ListViewItemCollection itemsFromList = null; // TODO: Initialize to an appropriate value
            ImportThread target = new ImportThread(isValidationMode, itemsFromList); // TODO: Initialize to an appropriate value
            target.StartWork();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for isValidationMode
        ///</summary>
        [TestMethod()]
        public void isValidationModeTest()
        {
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            ListView.ListViewItemCollection itemsFromList = null; // TODO: Initialize to an appropriate value
            ImportThread target = new ImportThread(isValidationMode, itemsFromList); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.isValidationMode = expected;
            actual = target.isValidationMode;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for itemsFromList
        ///</summary>
        [TestMethod()]
        public void itemsFromListTest()
        {
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            ListView.ListViewItemCollection itemsFromList = null; // TODO: Initialize to an appropriate value
            ImportThread target = new ImportThread(isValidationMode, itemsFromList); // TODO: Initialize to an appropriate value
            List<ListViewItem> expected = null; // TODO: Initialize to an appropriate value
            List<ListViewItem> actual;
            target.itemsFromList = expected;
            actual = target.itemsFromList;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for logging
        ///</summary>
        [TestMethod()]
        public void loggingTest()
        {
            bool isValidationMode = false; // TODO: Initialize to an appropriate value
            ListView.ListViewItemCollection itemsFromList = null; // TODO: Initialize to an appropriate value
            ImportThread target = new ImportThread(isValidationMode, itemsFromList); // TODO: Initialize to an appropriate value
            ImportThreadLogging expected = null; // TODO: Initialize to an appropriate value
            ImportThreadLogging actual;
            target.logging = expected;
            actual = target.logging;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
