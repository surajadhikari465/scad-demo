using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using System.Data.Entity;
using Icon.Framework;
using System.Collections.Generic;
using Icon.Common.Context;
using Moq;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using System.Linq;
using Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class GenerateNationalClassEventsCommandHandlerTests : BaseHierarchyClassesCommandTest
    {
        private GenerateHierarchyClassEventsCommandHandler commandHandler;
        private GenerateHierarchyClassEventsCommand command;

        [TestInitialize]
        public void Initialize()
        {
            commandHandler = new GenerateHierarchyClassEventsCommandHandler(
                mockRenewableContext.Object,
                regions);

            command = new GenerateHierarchyClassEventsCommand();
        }

        [TestMethod]
        public void GenerateHierarchyClassEventsCommand_WhenNoData_DoesNothing()
        {
            //Given
            SetCommandHierarchyClasses(command, new InforHierarchyClassModel[0]);
            var testTime = DateTime.Now;
            var countBefore = GetQueuedEventCount(context, regions, testTime);

            //When
            commandHandler.Execute(command);

            //Then
            var countAfter = GetQueuedEventCount(context, regions, testTime);
            Assert.AreEqual(countAfter, countBefore);
        }

        [TestMethod]
        public void GenerateHierarchyClassEventsCommand_BrandAddOrUpdate_GeneratesEvent()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test Brand 1",
                HierarchyNames.Brands, HierarchyLevelNames.Brand, ActionEnum.AddOrUpdate);
            SetCommandHierarchyClasses(command, testModel);

            //When
            commandHandler.Execute(command);

            //Then
            var queuedEvents = GetQueuedEvents(context, id1234);
            AssertEventsAreEqualToTestModel(testModel, queuedEvents, regions.Count, EventTypes.BrandNameUpdate);
        }

        [TestMethod]
        public void GenerateHierarchyClassEventsCommand_BrandDelete_GeneratesEvent()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test Brand 1",
                HierarchyNames.Brands, HierarchyLevelNames.Brand, ActionEnum.Delete);
            SetCommandHierarchyClasses(command, testModel);

            //When
            commandHandler.Execute(command);

            //Then
            var queuedEvents = GetQueuedEvents(context, id1234);
            AssertEventsAreEqualToTestModel(testModel, queuedEvents, regions.Count, EventTypes.BrandDelete);
        }

        [TestMethod]
        public void GenerateHierarchyClassEventsCommand_NationalAddOrUpdate_GeneratesEvent()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test National 1",
                HierarchyNames.National, HierarchyLevelNames.NationalFamily, ActionEnum.AddOrUpdate);
            SetCommandHierarchyClasses(command, testModel);

            //When
            commandHandler.Execute(command);

            //Then
            var queuedEvents = GetQueuedEvents(context, id1234);
            AssertEventsAreEqualToTestModel(testModel, queuedEvents, regions.Count, EventTypes.NationalClassUpdate);
        }

        [TestMethod]
        public void GenerateHierarchyClassEventsCommand_NationalDelete_GeneratesEvent()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test National 1",
                HierarchyNames.National, HierarchyLevelNames.NationalFamily, ActionEnum.Delete);
            SetCommandHierarchyClasses(command, testModel);

            //When
            commandHandler.Execute(command);

            //Then
            var queuedEvents = GetQueuedEvents(context, id1234);
            AssertEventsAreEqualToTestModel(testModel,
                queuedEvents, regions.Count, EventTypes.NationalClassDelete);
        }

        [TestMethod]
        public void GenerateHierarchyClassEventsCommand_TaxDelete_DoesNothing()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test Tax 1",
                HierarchyNames.Tax, HierarchyLevelNames.Tax, ActionEnum.Delete);
            SetCommandHierarchyClasses(command, testModel);
            var testTime = DateTime.Now;
            var countBefore = GetQueuedEventCount(context, regions, testTime);

            //When
            commandHandler.Execute(command);

            //Then
            var countAfter = GetQueuedEventCount(context, regions, testTime);
            Assert.AreEqual(countAfter, countBefore);
        }

        [TestMethod]
        public void GenerateHierarchyClassEventsCommand_TaxAddOrUpdate_DoesNothing()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test Tax 1",
                HierarchyNames.Tax, HierarchyLevelNames.Tax, ActionEnum.AddOrUpdate);
            SetCommandHierarchyClasses(command, testModel);
            var testTime = DateTime.Now;
            var countBefore = GetQueuedEventCount(context, regions, testTime);

            //When
            commandHandler.Execute(command);

            //Then
            var countAfter = GetQueuedEventCount(context, regions, testTime);
            Assert.AreEqual(countAfter, countBefore);
        }

        [TestMethod]
        public void GenerateHierarchyClassEventsCommand_MerchDelete_DoesNothing()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test Merch 1",
                HierarchyNames.Merchandise, HierarchyLevelNames.Segment, ActionEnum.Delete);
            SetCommandHierarchyClasses(command, testModel);
            var testTime = DateTime.Now;
            var countBefore = GetQueuedEventCount(context, regions, testTime);

            //When
            commandHandler.Execute(command);

            //Then
            var countAfter = GetQueuedEventCount(context, regions, testTime);
            Assert.AreEqual(countAfter, countBefore);
        }

        [TestMethod]
        public void GenerateHierarchyClassEventsCommand_MerchAddOrUpdate_DoesNothing()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test Merch 1",
                HierarchyNames.Merchandise, HierarchyLevelNames.Segment, ActionEnum.AddOrUpdate);
            SetCommandHierarchyClasses(command, testModel);
            var testTime = DateTime.Now;
            var countBefore = GetQueuedEventCount(context, regions, testTime);

            //When
            commandHandler.Execute(command);

            //Then
            var countAfter = GetQueuedEventCount(context, regions, testTime);
            Assert.AreEqual(countAfter, countBefore);
        }

        [TestMethod]
        public void GenerateHierarchyClassEventsCommand_FinancialDelete_DoesNothing()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test Fin 1",
                HierarchyNames.Financial, HierarchyLevelNames.Financial, ActionEnum.Delete);
            SetCommandHierarchyClasses(command, testModel);
            var testTime = DateTime.Now;
            var countBefore = GetQueuedEventCount(context, regions, testTime);

            //When
            commandHandler.Execute(command);

            //Then
            var countAfter = GetQueuedEventCount(context, regions, testTime);
            Assert.AreEqual(countAfter, countBefore);
        }

        [TestMethod]
        public void GenerateHierarchyClassEventsCommand_FinancialAddOrUpdate_DoesNothing()
        {
            //Given
            var testModel = CreateInforHierarchyClassModel(id1234, "Test Fin 1",
                HierarchyNames.Financial, HierarchyLevelNames.Financial, ActionEnum.AddOrUpdate);
            SetCommandHierarchyClasses(command, testModel);
            var testTime = DateTime.Now;
            var countBefore = GetQueuedEventCount(context, regions, testTime);

            //When
            commandHandler.Execute(command);

            //Then
            var queuedEvents = GetQueuedEvents(context, id1234);
            var countAfter = GetQueuedEventCount(context, regions, testTime);
            Assert.AreEqual(countAfter, countBefore);
        }
    }
}
