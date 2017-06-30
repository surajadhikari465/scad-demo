using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetHierarchyClassQueryHandlerTests
    {
        private GetHierarchyClassQueryHandler queryHandler;
        private IconContext context;
        private TransactionScope transaction;
        private HierarchyClass hierarchyClass;
        private IconDbContextFactory contextFactory;

        [TestInitialize]
        public void InitializeData()
        {
            transaction = new TransactionScope();
            this.context = new IconContext();
            this.contextFactory = new IconDbContextFactory();
            this.queryHandler = new GetHierarchyClassQueryHandler(contextFactory);
            this.hierarchyClass = new TestHierarchyClassBuilder();
            this.context.HierarchyClass.Add(this.hierarchyClass);
            this.context.SaveChanges();
        }

        [TestCleanup]
        public void CleanupData()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetHierarchyClassQuery_ValidHierarchyClassId_ReturnsHierarchyClassObject()
        {
            // Given
            GetHierarchyClassQuery query = new GetHierarchyClassQuery { HierarchyClassId = this.hierarchyClass.hierarchyClassID };
            HierarchyClass expected = this.hierarchyClass;

            // When
            HierarchyClass actual = queryHandler.Handle(query);

            // Then
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.hierarchyClassID, actual.hierarchyClassID);
            Assert.AreEqual(expected.hierarchyClassName, actual.hierarchyClassName);
            Assert.AreEqual(expected.hierarchyParentClassID, actual.hierarchyParentClassID);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetHierarchyClassQuery_HierarchyClassIdLessThanOne_ThrowsArgumentException()
        {
            // Given
            GetHierarchyClassQuery query = new GetHierarchyClassQuery { HierarchyClassId = 0 };

            // When
            HierarchyClass actual = queryHandler.Handle(query);

            // Then
            // Expected ArgumentOutOfRangeException
        }
    }
}
