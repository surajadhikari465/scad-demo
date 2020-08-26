using Icon.Framework;
using Icon.Web.Common;
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
        private AppSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            settings = new AppSettings();
            refreshHierarchiesCommandHandler = new RefreshHierarchiesCommandHandler(context, settings);
            transaction = context.Database.BeginTransaction();
            time = DateTime.Now.AddMinutes(-1); //minus 1 minute to avoid server time synchronization issues.
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
            var time = System.DateTime.Now.AddMinutes(-1); // Add one minute to avoid issue with server time synchronization
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
            context.EventQueue.RemoveRange(context.EventQueue.Where(e => e.EventReferenceId == hierarchyClassId));
            context.SaveChanges();
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
            var queueEvent = context.EventQueue.Single(e => e.EventReferenceId == hierarchyClassId);
            Assert.AreEqual(settings.HierarchyClassRefreshEventConfiguredRegions.Single(), queueEvent.RegionCode);
        }

        [TestMethod]
        public void RefreshHierarchies_SuccessfulExecution_RefreshBrandShouldHappen()
        {
            // Given.
            int hierarchyClassId = 40430;
            context.EventQueue.RemoveRange(context.EventQueue.Where(e => e.EventReferenceId == hierarchyClassId));
            context.SaveChanges();
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
            var queueEvent = context.EventQueue.Single(e => e.EventReferenceId == hierarchyClassId);
            Assert.AreEqual(settings.HierarchyClassRefreshEventConfiguredRegions.Single(), queueEvent.RegionCode);
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
