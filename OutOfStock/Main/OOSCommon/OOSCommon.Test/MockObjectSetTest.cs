using OOSCommon.DataContext.OOSEntitiesMockObjectSet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq.Expressions;
using System.Linq;

namespace OOSCommon.Test
{
    
    
    /// <summary>
    ///This is a test class for MockObjectSetTest and is intended
    ///to contain all MockObjectSetTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MockObjectSetTest
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
        ///A test for MockObjectSet`1 Constructor
        ///</summary>
        public void MockObjectSetConstructorTestHelper<T>()
            where T : class
        {
            MockObjectSet<T> target = new MockObjectSet<T>();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        [TestMethod()]
        public void MockObjectSetConstructorTest()
        {
            MockObjectSetConstructorTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for AddObject
        ///</summary>
        public void AddObjectTestHelper<T>()
            where T : class
        {
            MockObjectSet<T> target = new MockObjectSet<T>(); // TODO: Initialize to an appropriate value
            T entity = null; // TODO: Initialize to an appropriate value
            target.AddObject(entity);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void AddObjectTest()
        {
            AddObjectTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Attach
        ///</summary>
        public void AttachTestHelper<T>()
            where T : class
        {
            MockObjectSet<T> target = new MockObjectSet<T>(); // TODO: Initialize to an appropriate value
            T entity = null; // TODO: Initialize to an appropriate value
            target.Attach(entity);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void AttachTest()
        {
            AttachTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for DeleteObject
        ///</summary>
        public void DeleteObjectTestHelper<T>()
            where T : class
        {
            MockObjectSet<T> target = new MockObjectSet<T>(); // TODO: Initialize to an appropriate value
            T entity = null; // TODO: Initialize to an appropriate value
            target.DeleteObject(entity);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void DeleteObjectTest()
        {
            DeleteObjectTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Detach
        ///</summary>
        public void DetachTestHelper<T>()
            where T : class
        {
            MockObjectSet<T> target = new MockObjectSet<T>(); // TODO: Initialize to an appropriate value
            T entity = null; // TODO: Initialize to an appropriate value
            target.Detach(entity);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void DetachTest()
        {
            DetachTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetEnumerator
        ///</summary>
        public void GetEnumeratorTestHelper<T>()
            where T : class
        {
            MockObjectSet<T> target = new MockObjectSet<T>(); // TODO: Initialize to an appropriate value
            IEnumerator<T> expected = null; // TODO: Initialize to an appropriate value
            IEnumerator<T> actual;
            actual = target.GetEnumerator();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void GetEnumeratorTest()
        {
            GetEnumeratorTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for System.Collections.IEnumerable.GetEnumerator
        ///</summary>
        public void GetEnumeratorTest1Helper<T>()
            where T : class
        {
            IEnumerable target = new MockObjectSet<T>(); // TODO: Initialize to an appropriate value
            IEnumerator expected = null; // TODO: Initialize to an appropriate value
            IEnumerator actual;
            actual = target.GetEnumerator();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        [DeploymentItem("OOSCommon.dll")]
        public void GetEnumeratorTest1()
        {
            GetEnumeratorTest1Helper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for ElementType
        ///</summary>
        public void ElementTypeTestHelper<T>()
            where T : class
        {
            MockObjectSet<T> target = new MockObjectSet<T>(); // TODO: Initialize to an appropriate value
            Type actual;
            actual = target.ElementType;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void ElementTypeTest()
        {
            ElementTypeTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Expression
        ///</summary>
        public void ExpressionTestHelper<T>()
            where T : class
        {
            MockObjectSet<T> target = new MockObjectSet<T>(); // TODO: Initialize to an appropriate value
            Expression actual;
            actual = target.Expression;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void ExpressionTest()
        {
            ExpressionTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Provider
        ///</summary>
        public void ProviderTestHelper<T>()
            where T : class
        {
            MockObjectSet<T> target = new MockObjectSet<T>(); // TODO: Initialize to an appropriate value
            IQueryProvider actual;
            actual = target.Provider;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void ProviderTest()
        {
            ProviderTestHelper<GenericParameterHelper>();
        }
    }
}
