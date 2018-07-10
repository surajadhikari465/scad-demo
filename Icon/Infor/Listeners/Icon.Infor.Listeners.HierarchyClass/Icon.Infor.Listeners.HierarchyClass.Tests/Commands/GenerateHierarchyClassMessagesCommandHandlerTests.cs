using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class GenerateHierarchyClassMessagesCommandHandlerTests : BaseHierarchyClassesCommandTest
    {
        private GenerateHierarchyClassMessagesCommandHandler commandHandler;
        private GenerateHierarchyClassMessagesCommand command;

        [TestInitialize]
        public void Initialize()
        {
            commandHandler = new GenerateHierarchyClassMessagesCommandHandler(contextFactory);

            command = new GenerateHierarchyClassMessagesCommand();
        }

        [TestMethod]
        public void GenerateHierarchyClassMessages_WhenNoData_DoesNothing()
        {
            //Given
            SetCommandHierarchyClasses(command, new InforHierarchyClassModel[0]);
            var testTime = DateTime.Now;
            var countBefore = GetQueuedMessageCount(context, testTime);

            //When
            commandHandler.Execute(command);

            //Then
            var countAfter = GetQueuedMessageCount(context, testTime);
            Assert.AreEqual(countAfter, countBefore);
        }

        [TestMethod]
        public void GenerateHierarchyClassMessages_BrandAddOrUpdate_GeneratesMessage()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(
                id1234, 
                "Test Brand 1",
                HierarchyNames.Brands, 
                HierarchyLevelNames.Brand, 
                ActionEnum.AddOrUpdate);
            SetCommandHierarchyClasses(command, testModel);

            //When
            commandHandler.Execute(command);

            //Then
            var queuedMessages = GetQueuedMessages(context, id1234);
            AssertMessagesAreEqualToTestModel(testModel, queuedMessages);
        }

        [TestMethod]
        public void GenerateHierarchyClassMessages_BrandDelete_GeneratesMessage()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test Brand 1",
                HierarchyNames.Brands, HierarchyLevelNames.Brand, ActionEnum.Delete);
            SetCommandHierarchyClasses(command, testModel);

            //When
            commandHandler.Execute(command);

            //Then
            var queuedMessages = GetQueuedMessages(context, id1234);
            AssertMessagesAreEqualToTestModel(testModel, queuedMessages);
        }

        [TestMethod]
        public void GenerateHierarchyClassMessages_MerchandiseAddOrUpdate_GeneratesMessage()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test Merch 1",
                HierarchyNames.Merchandise, HierarchyLevelNames.SubBrick, ActionEnum.AddOrUpdate);
            SetCommandHierarchyClasses(command, testModel);

            //When
            commandHandler.Execute(command);

            //Then
            var queuedMessages = GetQueuedMessages(context, id1234);
            AssertMessagesAreEqualToTestModel(testModel, queuedMessages);
        }

        [TestMethod]
        public void GenerateHierarchyClassMessages_MerchandiseDelete_GeneratesMessage()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test Merch 1",
                HierarchyNames.Merchandise, HierarchyLevelNames.SubBrick, ActionEnum.Delete);
            SetCommandHierarchyClasses(command, testModel);

            //When
            commandHandler.Execute(command);

            //Then
            var queuedMessages = GetQueuedMessages(context, id1234);
            AssertMessagesAreEqualToTestModel(testModel, queuedMessages);
        }

        [TestMethod]
        public void GenerateHierarchyClassMessages_NationalAddOrUpdate_GeneratesMessage()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test National 1",
                HierarchyNames.National, HierarchyLevelNames.NationalClass, ActionEnum.AddOrUpdate);
            SetCommandHierarchyClasses(command, testModel);

            //When
            commandHandler.Execute(command);

            //Then
            var queuedMessages = GetQueuedMessages(context, id1234);
            AssertMessagesAreEqualToTestModel(testModel, queuedMessages);
        }
    }
}
