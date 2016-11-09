using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using System.Data.Entity;
using Icon.Testing.Builders;
using GlobalEventController.DataAccess.Queries;

namespace GlobalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetHierarchyClassQueryHandlerTests
    {
        private IconContext context;
        private DbContextTransaction transaction;
        private HierarchyClass hierarchyClass;
        private GetHierarchyClassQueryHandler queryHandler;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.queryHandler = new GetHierarchyClassQueryHandler(this.context);
            this.transaction = this.context.Database.BeginTransaction();
            this.hierarchyClass = new TestHierarchyClassBuilder();
            this.context.HierarchyClass.Add(this.hierarchyClass);
            this.context.SaveChanges();
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Rollback();
            this.context.Dispose();
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
            Assert.AreEqual(expected, actual);
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
