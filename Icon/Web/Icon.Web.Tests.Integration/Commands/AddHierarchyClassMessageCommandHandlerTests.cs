using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddHierarchyClassMessageCommandHandlerTests
    {
        private AddHierarchyClassMessageCommandHandler commandHandler;

        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new AddHierarchyClassMessageCommandHandler(context);
            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            context.Dispose();
        }

        [TestMethod]
        public void MessageGeneratorHierarchy_Brand_MessageCreated()
        {
            // Given
            AddHierarchyClassMessageCommand command = new AddHierarchyClassMessageCommand();
            command.HierarchyClass = context.HierarchyClass.First(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Brands);
            command.ClassNameChange = true;

            // When
            commandHandler.Execute(command);

            // Then
            var actualMessage = context.MessageQueueHierarchy.OrderByDescending(mq => mq.MessageQueueId)
                .First(mq => mq.HierarchyClassId == command.HierarchyClass.hierarchyClassID.ToString() && mq.HierarchyName == HierarchyNames.Brands);
            var entry = context.Entry(actualMessage);
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.AreEqual(command.HierarchyClass.hierarchyClassID.ToString(), actualMessage.HierarchyClassId);
            Assert.AreEqual(command.HierarchyClass.hierarchyClassName, actualMessage.HierarchyClassName);
            Assert.AreEqual(command.HierarchyClass.hierarchyID, actualMessage.HierarchyId);
            Assert.AreEqual(command.HierarchyClass.hierarchyParentClassID, actualMessage.HierarchyParentClassId);
            Assert.AreEqual(command.HierarchyClass.HierarchyPrototype.hierarchyLevelName, actualMessage.HierarchyLevelName);
            Assert.AreEqual(command.HierarchyClass.hierarchyLevel, actualMessage.HierarchyLevel);
            Assert.AreEqual(command.HierarchyClass.Hierarchy.hierarchyName, actualMessage.HierarchyName);
            Assert.AreEqual(command.HierarchyClass.HierarchyPrototype.itemsAttached, actualMessage.ItemsAttached);
            Assert.IsNull(actualMessage.MessageHistoryId);
            Assert.AreEqual(MessageTypes.Hierarchy, actualMessage.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Ready, actualMessage.MessageStatusId);
            Assert.AreEqual(MessageActionTypes.AddOrUpdate, actualMessage.MessageActionId);
            Assert.IsNull(actualMessage.InProcessBy);
            Assert.IsNull(actualMessage.ProcessedDate);
        }

        [TestMethod]
        public void MessageGeneratorHierarchy_Tax_MessageShouldNotBeCreated()
        {
            // Given
            AddHierarchyClassMessageCommand command = new AddHierarchyClassMessageCommand();
            command.ClassNameChange = true;
            command.HierarchyClass = context.HierarchyClass.First(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Tax);
            string hierarchyClassName = command.HierarchyClass.hierarchyClassName;

            // When
            var now = DateTime.Now;

            commandHandler.Execute(command);

            //Then
            var messageThatShouldNotExist = context.MessageQueueHierarchy.Where(
              mq => mq.HierarchyName == HierarchyNames.Tax &&
              mq.HierarchyClassName == hierarchyClassName &&
              mq.InsertDate > now).ToList();

            Assert.AreEqual(0, messageThatShouldNotExist.Count);
        }

        [TestMethod]
        public void MessageGeneratorHierarchy_MerchSubBrick_MessageCreated()
        {
            // Given
            AddHierarchyClassMessageCommand command = new AddHierarchyClassMessageCommand();
            command.ClassNameChange = true;
            command.HierarchyClass = context.HierarchyClass.First(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Merchandise);

            // When
            commandHandler.Execute(command);

            // Then
            var actualMessage = context.MessageQueueHierarchy
                .OrderByDescending(mqh => mqh.InsertDate)
                .First(mq => mq.HierarchyClassId == command.HierarchyClass.hierarchyClassID.ToString());
            var entry = context.Entry(actualMessage);
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.AreEqual(command.HierarchyClass.hierarchyClassID.ToString(), actualMessage.HierarchyClassId);
            Assert.AreEqual(command.HierarchyClass.hierarchyClassName, actualMessage.HierarchyClassName);
            Assert.AreEqual(command.HierarchyClass.hierarchyID, actualMessage.HierarchyId);
            Assert.AreEqual(command.HierarchyClass.hierarchyParentClassID, actualMessage.HierarchyParentClassId);
            Assert.AreEqual(command.HierarchyClass.HierarchyPrototype.hierarchyLevelName, actualMessage.HierarchyLevelName);
            Assert.AreEqual(command.HierarchyClass.hierarchyLevel, actualMessage.HierarchyLevel);
            Assert.AreEqual(command.HierarchyClass.Hierarchy.hierarchyName, actualMessage.HierarchyName);
            Assert.AreEqual(command.HierarchyClass.HierarchyPrototype.itemsAttached, actualMessage.ItemsAttached);
            Assert.IsNull(actualMessage.MessageHistoryId);
            Assert.AreEqual(MessageTypes.Hierarchy, actualMessage.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Ready, actualMessage.MessageStatusId);
            Assert.AreEqual(MessageActionTypes.AddOrUpdate, actualMessage.MessageActionId);
            Assert.IsNull(actualMessage.InProcessBy);
            Assert.IsNull(actualMessage.ProcessedDate);
        }

        [TestMethod]
        public void MessageGeneratorHierarchy_Financial_MessageCreated()
        {
            // Given
            AddHierarchyClassMessageCommand command = new AddHierarchyClassMessageCommand();
            command.ClassNameChange = true;
            command.HierarchyClass = context.HierarchyClass
                .First(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Financial);

            // When
            commandHandler.Execute(command);

            // Then
            var peoplesoftNumber = command.HierarchyClass.hierarchyClassName.Split('(')[1].Trim(')');
            var actualMessage = context.MessageQueueHierarchy
                .OrderByDescending(mqh => mqh.InsertDate)
                .First(mq => mq.HierarchyClassName == peoplesoftNumber);

            var entry = context.Entry(actualMessage);
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.AreEqual(command.HierarchyClass.hierarchyClassName.Split('(')[1].Trim(')'), actualMessage.HierarchyClassId);
            Assert.AreEqual(command.HierarchyClass.hierarchyClassName.Split('(')[1].Trim(')'), actualMessage.HierarchyClassName);
            Assert.AreEqual(command.HierarchyClass.hierarchyID, actualMessage.HierarchyId);
            Assert.AreEqual(command.HierarchyClass.hierarchyParentClassID, actualMessage.HierarchyParentClassId);
            Assert.AreEqual(command.HierarchyClass.HierarchyPrototype.hierarchyLevelName, actualMessage.HierarchyLevelName);
            Assert.AreEqual(command.HierarchyClass.hierarchyLevel, actualMessage.HierarchyLevel);
            Assert.AreEqual(command.HierarchyClass.Hierarchy.hierarchyName, actualMessage.HierarchyName);
            Assert.AreEqual(command.HierarchyClass.HierarchyPrototype.itemsAttached, actualMessage.ItemsAttached);
            Assert.IsNull(actualMessage.MessageHistoryId);
            Assert.AreEqual(MessageTypes.Hierarchy, actualMessage.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Ready, actualMessage.MessageStatusId);
            Assert.AreEqual(MessageActionTypes.AddOrUpdate, actualMessage.MessageActionId);
            Assert.IsNull(actualMessage.InProcessBy);
            Assert.IsNull(actualMessage.ProcessedDate);
        }

        [TestMethod]
        public void MessageGeneratorHierarchy_Browsing_MessageShouldNotBeCreated()
        {
            // Given.
            AddHierarchyClassMessageCommand command = new AddHierarchyClassMessageCommand();
            command.ClassNameChange = true;
            command.HierarchyClass = context.HierarchyClass.First(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Browsing);

            string hierarchyClassName = command.HierarchyClass.hierarchyClassName;
            var timestamp = DateTime.Now;

            // When.
            commandHandler.Execute(command);

            // Then.
            var messageThatShouldNotExist = context.MessageQueueHierarchy.Where(
                mq => mq.HierarchyName == HierarchyNames.Browsing &&
                mq.HierarchyClassName == hierarchyClassName &&
                mq.InsertDate > timestamp).ToList();

            Assert.AreEqual(0, messageThatShouldNotExist.Count);
        }

        [TestMethod]
        public void MessageGeneratorHierarchy_ClassNameChangesIsFalse_NoMessageCreated()
        {
            // Given
            AddHierarchyClassMessageCommand command = new AddHierarchyClassMessageCommand();
            command.ClassNameChange = false;
            command.HierarchyClass = context.HierarchyClass.First(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Browsing);

            // When
            commandHandler.Execute(command);

            // Then
            var actualMessage = context.MessageQueueHierarchy.Where(mq => mq.HierarchyClassId == command.HierarchyClass.hierarchyClassID.ToString());
            Assert.IsTrue(actualMessage.Count() == 0);
        }

        [TestMethod]
        public void MessageGeneratorHierarchy_HierarchyClassDelete_MessageHasDeleteAction()
        {
            // Given
            AddHierarchyClassMessageCommand command = new AddHierarchyClassMessageCommand();
            command.ClassNameChange = true;
            command.HierarchyClass = context.HierarchyClass.First(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Merchandise && hc.hierarchyLevel == HierarchyLevels.SubBrick);
            command.DeleteMessage = true;

            // When
            commandHandler.Execute(command);

            // Then
            var actualmessage = context.MessageQueueHierarchy.OrderByDescending(mq => mq.MessageQueueId)
                .First(mq => mq.HierarchyClassId == command.HierarchyClass.hierarchyClassID.ToString() && mq.HierarchyClassName == command.HierarchyClass.hierarchyClassName);
            var entry = context.Entry(actualmessage);
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.AreEqual(actualmessage.MessageActionId, MessageActionTypes.Delete);
        }

        [TestMethod]
        public void MessageGeneratorHierarchy_HierarchyClassAddOrUpdate_MessageAddOrUpdateAction()
        {
            // Given
            AddHierarchyClassMessageCommand command = new AddHierarchyClassMessageCommand();
            command.ClassNameChange = true;
            command.HierarchyClass = context.HierarchyClass.First(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Brands);
            command.DeleteMessage = false;

            // When
            commandHandler.Execute(command);

            // Then
            var actualmessage = context.MessageQueueHierarchy.First(mq => mq.HierarchyClassId == command.HierarchyClass.hierarchyClassID.ToString());
            var entry = context.Entry(actualmessage);
            Assert.IsTrue(entry.State == EntityState.Unchanged);
            Assert.AreEqual(actualmessage.MessageActionId, MessageActionTypes.AddOrUpdate);
        }
    }
}
