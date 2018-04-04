using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class AddBrandMessageCommandHandlerTests
    {
        private AddBrandMessageCommandHandler commandHandler;
        private IconContext context;
        private DbContextTransaction transaction;
        private HierarchyClass testBrand;

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
                Action = MessageActionTypes.AddOrUpdate
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
        }

        [TestMethod]
        public void AddBrandMessage_DeleteMessage_MessageShouldBeCreatedWithDeleteAction()
        {
            // Given.
            var command = new AddBrandMessageCommand
            {
                Brand = testBrand,
                Action = MessageActionTypes.Delete
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
        }
    }
}
