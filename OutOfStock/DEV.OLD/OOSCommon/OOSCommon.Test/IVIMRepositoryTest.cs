using OOSCommon.VIM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for IVIMRepositoryTest and is intended
    ///to contain all IVIMRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IVIMRepositoryTest
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


        internal virtual IVIMRepository CreateIVIMRepository()
        {
            // TODO: Instantiate an appropriate concrete class.
            IVIMRepository target = null;
            return target;
        }

        /// <summary>
        ///A test for GetItemMasterModel
        ///</summary>
        [TestMethod()]
        public void GetItemMasterModelTest()
        {
            IVIMRepository target = CreateIVIMRepository(); // TODO: Initialize to an appropriate value
            string upc = string.Empty; // TODO: Initialize to an appropriate value
            ItemMasterModel expected = null; // TODO: Initialize to an appropriate value
            ItemMasterModel actual;
            actual = target.GetItemMasterModel(upc);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetItemMasterModel
        ///</summary>
        [TestMethod()]
        public void GetItemMasterModelTest1()
        {
            IVIMRepository target = CreateIVIMRepository(); // TODO: Initialize to an appropriate value
            IEnumerable<string> upcs = null; // TODO: Initialize to an appropriate value
            Dictionary<string, ItemMasterModel> expected = null; // TODO: Initialize to an appropriate value
            Dictionary<string, ItemMasterModel> actual;
            actual = target.GetItemMasterModel(upcs);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetVIMOOSItemDataView
        ///</summary>
        [TestMethod()]
        public void GetVIMOOSItemDataViewTest()
        {
            IVIMRepository target = CreateIVIMRepository(); // TODO: Initialize to an appropriate value
            string upc = string.Empty; // TODO: Initialize to an appropriate value
            string ps_bu = string.Empty; // TODO: Initialize to an appropriate value
            IEnumerable<VIMOOSItemDataView> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<VIMOOSItemDataView> actual;
            actual = target.GetVIMOOSItemDataView(upc, ps_bu);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetVimSubTeam
        ///</summary>
        [TestMethod()]
        public void GetVimSubTeamTest()
        {
            IVIMRepository target = CreateIVIMRepository(); // TODO: Initialize to an appropriate value
            string region = string.Empty; // TODO: Initialize to an appropriate value
            List<string> storeNumbers = null; // TODO: Initialize to an appropriate value
            List<string> teams = null; // TODO: Initialize to an appropriate value
            IEnumerable<VimSubTeam> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<VimSubTeam> actual;
            actual = target.GetVimSubTeam(region, storeNumbers, teams);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetVimTeam
        ///</summary>
        [TestMethod()]
        public void GetVimTeamTest()
        {
            IVIMRepository target = CreateIVIMRepository(); // TODO: Initialize to an appropriate value
            IEnumerable<VimTeam> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<VimTeam> actual;
            actual = target.GetVimTeam();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetVimUPC
        ///</summary>
        [TestMethod()]
        public void GetVimUPCTest()
        {
            IVIMRepository target = CreateIVIMRepository(); // TODO: Initialize to an appropriate value
            string upc = string.Empty; // TODO: Initialize to an appropriate value
            string userRegion = string.Empty; // TODO: Initialize to an appropriate value
            IEnumerable<VimUPC> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<VimUPC> actual;
            actual = target.GetVimUPC(upc, userRegion);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ItemMasterModel
        ///</summary>
        [TestMethod()]
        public void ItemMasterModelTest()
        {
            IVIMRepository target = CreateIVIMRepository(); // TODO: Initialize to an appropriate value
            IEnumerable<ItemMasterModel> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<ItemMasterModel> actual;
            actual = target.ItemMasterModel();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
