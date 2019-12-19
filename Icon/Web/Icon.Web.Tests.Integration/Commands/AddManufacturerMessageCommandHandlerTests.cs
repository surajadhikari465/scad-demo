using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddManufacturerMessageCommandHandlerTests
    {
        private AddManufacturerMessageCommandHandler commandHandler;
        private IconContext context;
        private DbContextTransaction transaction;
        private HierarchyClass testManufacturer;
        private string testZipCode;
        private string testArCustomerId;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new AddManufacturerMessageCommandHandler(context);

            transaction = context.Database.BeginTransaction();
            testManufacturer = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.Manufacturer)
                .WithHierarchyLevel(HierarchyLevels.Parent)
                .WithHierarchyParentClassId(null);

            testZipCode = "78704";
            testArCustomerId = "1234";
            context.HierarchyClass.Add(testManufacturer);

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
        public void AddManufacturerMessage_AddOrUpdateMessage_MessageShouldBeCreatedWithAddOrUpdateAction()
        {
            // Given.
            var command = new AddManufacturerMessageCommand
            {
                Manufacturer = testManufacturer,
                Action = MessageActionTypes.AddOrUpdate,
                ZipCode = testZipCode,
                ArCustomerId = testArCustomerId
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var actualMessage = context.MessageQueueHierarchy
                .First(mq => mq.HierarchyClassId == command.Manufacturer.hierarchyClassID.ToString() && mq.HierarchyName == HierarchyNames.Manufacturer);

            Assert.AreEqual(testManufacturer.hierarchyClassID.ToString(), actualMessage.HierarchyClassId);
            Assert.AreEqual(testManufacturer.hierarchyClassName, actualMessage.HierarchyClassName);
            Assert.AreEqual(Hierarchies.Manufacturer, actualMessage.HierarchyId);
            Assert.AreEqual(testManufacturer.hierarchyParentClassID, actualMessage.HierarchyParentClassId);
            Assert.AreEqual(HierarchyLevelNames.Manufacturer, actualMessage.HierarchyLevelName);
            Assert.AreEqual(testManufacturer.hierarchyLevel, actualMessage.HierarchyLevel);
            Assert.AreEqual(HierarchyNames.Manufacturer, actualMessage.HierarchyName);
            Assert.AreEqual(true, actualMessage.ItemsAttached);
            Assert.IsNull(actualMessage.MessageHistoryId);
            Assert.AreEqual(MessageTypes.Hierarchy, actualMessage.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Ready, actualMessage.MessageStatusId);
            Assert.AreEqual(MessageActionTypes.AddOrUpdate, actualMessage.MessageActionId);
            Assert.IsNull(actualMessage.InProcessBy);
            Assert.IsNull(actualMessage.ProcessedDate);
            Assert.AreEqual(testZipCode, actualMessage.ZipCode);
            Assert.AreEqual(testArCustomerId, actualMessage.ArCustomerId);
        }

        [TestMethod]
        public void AddManufacturerMessage_DeleteMessage_MessageShouldBeCreatedWithDeleteAction()
        {
            // Given.
            var command = new AddManufacturerMessageCommand
            {
                Manufacturer = testManufacturer,
                Action = MessageActionTypes.Delete,
                ZipCode = testZipCode,
                ArCustomerId = testArCustomerId
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var actualMessage = context.MessageQueueHierarchy
                .First(mq => mq.HierarchyClassId == command.Manufacturer.hierarchyClassID.ToString() && mq.HierarchyName == HierarchyNames.Manufacturer);

            Assert.AreEqual(testManufacturer.hierarchyClassID.ToString(), actualMessage.HierarchyClassId);
            Assert.AreEqual(testManufacturer.hierarchyClassName, actualMessage.HierarchyClassName);
            Assert.AreEqual(Hierarchies.Manufacturer, actualMessage.HierarchyId);
            Assert.AreEqual(testManufacturer.hierarchyParentClassID, actualMessage.HierarchyParentClassId);
            Assert.AreEqual(HierarchyLevelNames.Manufacturer, actualMessage.HierarchyLevelName);
            Assert.AreEqual(testManufacturer.hierarchyLevel, actualMessage.HierarchyLevel);
            Assert.AreEqual(HierarchyNames.Manufacturer, actualMessage.HierarchyName);
            Assert.AreEqual(true, actualMessage.ItemsAttached);
            Assert.IsNull(actualMessage.MessageHistoryId);
            Assert.AreEqual(MessageTypes.Hierarchy, actualMessage.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Ready, actualMessage.MessageStatusId);
            Assert.AreEqual(MessageActionTypes.Delete, actualMessage.MessageActionId);
            Assert.IsNull(actualMessage.InProcessBy);
            Assert.IsNull(actualMessage.ProcessedDate);
            Assert.AreEqual(testZipCode, actualMessage.ZipCode);
            Assert.AreEqual(testArCustomerId, actualMessage.ArCustomerId);
        }
    }
}
