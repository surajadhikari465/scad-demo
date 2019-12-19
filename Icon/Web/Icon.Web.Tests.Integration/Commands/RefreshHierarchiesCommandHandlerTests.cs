using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class RefreshHierarchiesCommandHandlerTests
    {
        private RefreshHierarchiesCommandHandler refreshHierarchiesCommandHandler;
        private IconContext context;
        private DbContextTransaction transaction;
        private DateTime time;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            refreshHierarchiesCommandHandler = new RefreshHierarchiesCommandHandler(context);
            transaction = context.Database.BeginTransaction();
            time = DateTime.Now;
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void RefreshHierarchies_SuccessfulExecution_RefreshMerchandiseShouldHappen()
        {
            // Given.
            int hierarchyClassId = 82819;
            var time = System.DateTime.Now;
            RefreshHierarchiesCommand data = new RefreshHierarchiesCommand
            {
                HierarchyClassIds = new List<int> { hierarchyClassId }
            };

            // When.
            refreshHierarchiesCommandHandler.Execute(data);

            // Then.
            var result = context.MessageQueueHierarchy.Where(mqh => mqh.HierarchyClassId == hierarchyClassId.ToString()).OrderByDescending(m => m.InsertDate).First();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.InsertDate > time);
        }

        [TestMethod]
        public void RefreshHierarchies_SuccessfulExecution_RefreshNationalShouldHappen()
        {
            // Given.
            int hierarchyClassId = 121173;
            RefreshHierarchiesCommand data = new RefreshHierarchiesCommand
            {
                HierarchyClassIds = new List<int> { hierarchyClassId }
            };

            // When.
            refreshHierarchiesCommandHandler.Execute(data);

            // Then.
            var result = context.MessageQueueHierarchy.Where(mqh => mqh.HierarchyClassId == hierarchyClassId.ToString()).OrderByDescending(m => m.InsertDate).First();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.InsertDate > time);
        }

        [TestMethod]
        public void RefreshHierarchies_SuccessfulExecution_RefreshBrandShouldHappen()
        {
            // Given.
            int hierarchyClassId = 40430;
            RefreshHierarchiesCommand data = new RefreshHierarchiesCommand
            {
                HierarchyClassIds = new List<int> { hierarchyClassId }
            };

            // When.
            refreshHierarchiesCommandHandler.Execute(data);

            // Then.
            var result = context.MessageQueueHierarchy.Where(mqh => mqh.HierarchyClassId == hierarchyClassId.ToString()).OrderByDescending(m => m.InsertDate).First();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.InsertDate > time);
        }

        [TestMethod]
        public void RefreshHierarchies_SuccessfulExecution_ShouldNotAddToDB()
        {
            // Given.
            int hierarchyClassId = 1;
            RefreshHierarchiesCommand data = new RefreshHierarchiesCommand
            {
                HierarchyClassIds = new List<int> { hierarchyClassId }
            };

            // When.
            refreshHierarchiesCommandHandler.Execute(data);

            // Then.
            var result = context.MessageQueueHierarchy.Where(mqh => mqh.HierarchyClassId == hierarchyClassId.ToString()).OrderByDescending(m => m.InsertDate).FirstOrDefault();
            Assert.IsNull(result);
        }
    }
}
