using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddBrandMessageCommandHandlerTests
    {
        private AddBrandMessageCommandHandler commandHandler;
        private IconContext context;
        private DbContextTransaction transaction;
        private HierarchyClass testBrand;
        private string testBrandAbbreviation;
        private string testDesignation;
        private string testZipCode;
        private string testLocality;
        private string testParentCompany;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new AddBrandMessageCommandHandler(context);

            transaction = context.Database.BeginTransaction();
            testBrand = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Brands)
                .WithHierarchyLevel(HierarchyLevels.Parent)
                .WithHierarchyParentClassId(null);

            testBrandAbbreviation = "TestBa";
            testDesignation = "TestDesignation";
            testZipCode = "78745";
            testLocality = "TestLocality";
            testParentCompany = "TestParentCompany";
            context.HierarchyClass.Add(testBrand);

            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void AddBrandMessage_AddOrUpdateMessage_MessageShouldBeCreatedWithAddOrUpdateAction()
        {
            // Given.
            var command = new AddBrandMessageCommand
            {
                Brand = testBrand,
                Action = MessageActionTypes.AddOrUpdate,
                BrandAbbreviation = testBrandAbbreviation,
                Designation = testDesignation,
                ZipCode = testZipCode,
                Locality = testLocality,
                ParentCompany = testParentCompany
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var actualMessage = context.MessageQueueHierarchy
                .First(mq => mq.HierarchyClassId == command.Brand.hierarchyClassID.ToString() && mq.HierarchyName == HierarchyNames.Brands);

            Assert.AreEqual(testBrand.hierarchyClassID.ToString(), actualMessage.HierarchyClassId);
            Assert.AreEqual(testBrand.hierarchyClassName, actualMessage.HierarchyClassName);
            Assert.AreEqual(Hierarchies.Brands, actualMessage.HierarchyId);
            Assert.AreEqual(testBrand.hierarchyParentClassID, actualMessage.HierarchyParentClassId);
            Assert.AreEqual(HierarchyLevelNames.Brand, actualMessage.HierarchyLevelName);
            Assert.AreEqual(testBrand.hierarchyLevel, actualMessage.HierarchyLevel);
            Assert.AreEqual(HierarchyNames.Brands, actualMessage.HierarchyName);
            Assert.AreEqual(true, actualMessage.ItemsAttached);
            Assert.IsNull(actualMessage.MessageHistoryId);
            Assert.AreEqual(MessageTypes.Hierarchy, actualMessage.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Ready, actualMessage.MessageStatusId);
            Assert.AreEqual(MessageActionTypes.AddOrUpdate, actualMessage.MessageActionId);
            Assert.IsNull(actualMessage.InProcessBy);
            Assert.IsNull(actualMessage.ProcessedDate);
            Assert.AreEqual(testBrandAbbreviation, actualMessage.BrandAbbreviation);
            Assert.AreEqual(testDesignation, actualMessage.Designation);
            Assert.AreEqual(testZipCode, actualMessage.ZipCode);
            Assert.AreEqual(testLocality, actualMessage.Locality);
            Assert.AreEqual(testParentCompany, actualMessage.ParentCompany);
        }

        [TestMethod]
        public void AddBrandMessage_DeleteMessage_MessageShouldBeCreatedWithDeleteAction()
        {
            // Given.
            var command = new AddBrandMessageCommand
            {
                Brand = testBrand,
                Action = MessageActionTypes.Delete,
                BrandAbbreviation = testBrandAbbreviation,
                Designation = testDesignation,
                ZipCode = testZipCode,
                Locality = testLocality,
                ParentCompany = testParentCompany
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var actualMessage = context.MessageQueueHierarchy
                .First(mq => mq.HierarchyClassId == command.Brand.hierarchyClassID.ToString() && mq.HierarchyName == HierarchyNames.Brands);

            Assert.AreEqual(testBrand.hierarchyClassID.ToString(), actualMessage.HierarchyClassId);
            Assert.AreEqual(testBrand.hierarchyClassName, actualMessage.HierarchyClassName);
            Assert.AreEqual(Hierarchies.Brands, actualMessage.HierarchyId);
            Assert.AreEqual(testBrand.hierarchyParentClassID, actualMessage.HierarchyParentClassId);
            Assert.AreEqual(HierarchyLevelNames.Brand, actualMessage.HierarchyLevelName);
            Assert.AreEqual(testBrand.hierarchyLevel, actualMessage.HierarchyLevel);
            Assert.AreEqual(HierarchyNames.Brands, actualMessage.HierarchyName);
            Assert.AreEqual(true, actualMessage.ItemsAttached);
            Assert.IsNull(actualMessage.MessageHistoryId);
            Assert.AreEqual(MessageTypes.Hierarchy, actualMessage.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Ready, actualMessage.MessageStatusId);
            Assert.AreEqual(MessageActionTypes.Delete, actualMessage.MessageActionId);
            Assert.IsNull(actualMessage.InProcessBy);
            Assert.IsNull(actualMessage.ProcessedDate);
            Assert.AreEqual(testBrandAbbreviation, actualMessage.BrandAbbreviation);
            Assert.AreEqual(testDesignation, actualMessage.Designation);
            Assert.AreEqual(testZipCode, actualMessage.ZipCode);
            Assert.AreEqual(testLocality, actualMessage.Locality);
            Assert.AreEqual(testParentCompany, actualMessage.ParentCompany);
        }
    }
}
