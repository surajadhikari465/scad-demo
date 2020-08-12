using Icon.Common;
using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class UpdateBrandCommandHandlerTests
    {
        private BrandCommandHandler commandHandler;
        private IconContext context;
        private DbContextTransaction transaction;
        private HierarchyClass testBrand;
        private HierarchyClass duplicateBrand;
        private IRMAItem testIrmaItem;
        private string updatedBrandName;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new BrandCommandHandler(this.context);

            updatedBrandName = "UpdateBrandTest";

            transaction = context.Database.BeginTransaction();

            testBrand = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands);
            context.HierarchyClass.Add(testBrand);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageDuplicateBrandName()
        {
            duplicateBrand = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands).WithHierarchyClassName(updatedBrandName);
            context.HierarchyClass.Add(duplicateBrand);
            context.SaveChanges();
        }

        private void StageTestNewItem()
        {
            testIrmaItem = new TestIrmaItemBuilder().WithBrandName(testBrand.hierarchyClassName);

            context.IRMAItem.Add(testIrmaItem);
            context.SaveChanges();
        }

        [TestMethod]
        public void UpdateBrand_UserUpdatesBrandName_BrandNameShouldBeUpdated()
        {
            // Given.
            var brandToUpdate = new HierarchyClass
            {
                hierarchyClassID = testBrand.hierarchyClassID,
                hierarchyClassName = updatedBrandName
            };

            var command = new BrandCommand
            {
                Brand = brandToUpdate
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var updatedBrand = context.HierarchyClass.Single(hc => hc.hierarchyClassID == testBrand.hierarchyClassID);

            Assert.AreEqual(updatedBrandName, updatedBrand.hierarchyClassName);
        }
      

        [TestMethod]
        public void UpdateBrand_BrandUpdateIsSuccessful_EventsShouldBeAddedToQueueAccordingToRegionalConfiguration()
        {
            // Given.
            int configuredRegionsCount = AppSettingsAccessor.GetStringSetting("BrandNameUpdateEventConfiguredRegions").Split(',').Length;

            var brandToUpdate = new HierarchyClass
            {
                hierarchyClassID = testBrand.hierarchyClassID,
                hierarchyClassName = updatedBrandName
            };

            var command = new BrandCommand
            {
                Brand = brandToUpdate
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var updatedBrand = context.HierarchyClass.Single(hc => hc.hierarchyClassID == testBrand.hierarchyClassID);
            var events = context.EventQueue.Where(eq => eq.EventReferenceId == updatedBrand.hierarchyClassID).ToList();
            int brandNameUpdateEventId = context.EventType.Single(et => et.EventName == "Brand Name Update").EventId;

            bool allEventsHaveUpdatedBrandName = events.TrueForAll(e => e.EventMessage == updatedBrandName);
            bool allEventsHaveCorrectEventId = events.TrueForAll(e => e.EventId == brandNameUpdateEventId);

            Assert.AreEqual(configuredRegionsCount, events.Count);
            Assert.IsTrue(allEventsHaveUpdatedBrandName);
            Assert.IsTrue(allEventsHaveCorrectEventId);
        }

        [TestMethod]
        public void UpdateBrand_BrandUpdateIsSuccessful_NewItemsWithCurrentBrandNameShouldBeUpdated()
        {
            // Given.
            StageTestNewItem();

            var brandToUpdate = new HierarchyClass
            {
                hierarchyClassID = testBrand.hierarchyClassID,
                hierarchyClassName = updatedBrandName
            };

            var command = new BrandCommand
            {
                Brand = brandToUpdate
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var updatedNewItem = context.IRMAItem.Single(ii => ii.identifier == testIrmaItem.identifier);

            context.Entry(updatedNewItem).Reload();

            Assert.AreEqual(updatedBrandName, updatedNewItem.brandName);
        }
    }
}
