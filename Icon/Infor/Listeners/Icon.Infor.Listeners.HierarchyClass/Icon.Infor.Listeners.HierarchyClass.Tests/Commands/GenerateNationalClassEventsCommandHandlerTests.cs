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

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class GenerateNationalClassEventsCommandHandlerTests
    {
        private GenerateNationalClassEventsCommandHandler commandHandler;
        private GenerateNationalClassEventsCommand command;
        private Mock<IRenewableContext<IconContext>> mockRenewableContext;
        private List<string> regions;
        private IconContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            mockRenewableContext = new Mock<IRenewableContext<IconContext>>();
            mockRenewableContext.SetupGet(m => m.Context).Returns(context);

            regions = new List<string> { "FL", "MA", "MW" };

            commandHandler = new GenerateNationalClassEventsCommandHandler(
                mockRenewableContext.Object,
                regions);

            command = new GenerateNationalClassEventsCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (transaction.UnderlyingTransaction.Connection != null)
                transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void GenerateNationalClassEvents_BrandAddOrUpdate_GeneratesBrandNameUpdateEvent()
        {
            //Given
            InforHierarchyClassModel testModel = new InforHierarchyClassModel
            {
                HierarchyClassId = 1234,
                HierarchyClassName = "Test Brand 1",
                HierarchyName = HierarchyNames.Brands,
                HierarchyLevelName = HierarchyLevelNames.Brand,
                Action = Contracts.ActionEnum.AddOrUpdate,
                HierarchyClassTraits = new Dictionary<string, string>(),
            };
            command.HierarchyClasses = new List<InforHierarchyClassModel>
            {
                testModel
            };

            //When
            commandHandler.Execute(command);

            //Then
            var queuedEvents = context.EventQueue.Where(e => e.EventReferenceId == 1234);
            AssertEventsAreEquelToTestModel(testModel, queuedEvents, regions.Count, EventTypes.BrandNameUpdate);
        }

        [TestMethod]
        public void GenerateNationalClassEvents_BrandDelete_GeneratesBrandDeleteEvent()
        {
            //Given
            InforHierarchyClassModel testModel = new InforHierarchyClassModel
            {
                HierarchyClassId = 1234,
                HierarchyClassName = "Test Brand 1",
                HierarchyName = HierarchyNames.Brands,
                HierarchyLevelName = HierarchyLevelNames.Brand,
                Action = Contracts.ActionEnum.Delete,
                HierarchyClassTraits = new Dictionary<string, string>(),
            };
            command.HierarchyClasses = new List<InforHierarchyClassModel>
            {
                testModel
            };

            //When
            commandHandler.Execute(command);

            //Then
            var queuedEvents = context.EventQueue.Where(e => e.EventReferenceId == 1234);
            AssertEventsAreEquelToTestModel(testModel, queuedEvents, regions.Count, EventTypes.BrandDelete);
        }

        [TestMethod]
        public void GenerateNationalClassEvents_NationalAddOrUpdate_GeneratesNationalUpdateEvent()
        {
            //Given
            InforHierarchyClassModel testModel = new InforHierarchyClassModel
            {
                HierarchyClassId = 1234,
                HierarchyClassName = "Test National 1",
                HierarchyName = HierarchyNames.National,
                HierarchyLevelName = HierarchyLevelNames.NationalFamily,
                Action = Contracts.ActionEnum.AddOrUpdate,
                HierarchyClassTraits = new Dictionary<string, string>(),
            };
            command.HierarchyClasses = new List<InforHierarchyClassModel>
            {
                testModel
            };

            //When
            commandHandler.Execute(command);

            //Then
            var queuedEvents = context.EventQueue.Where(e => e.EventReferenceId == 1234);
            AssertEventsAreEquelToTestModel(testModel, queuedEvents, regions.Count, EventTypes.NationalClassUpdate);
        }

        [TestMethod]
        public void GenerateNationalClassEvents_NationalDelete_GeneratesNationalDeleteEvent()
        {
            //Given
            InforHierarchyClassModel testModel = new InforHierarchyClassModel
            {
                HierarchyClassId = 1234,
                HierarchyClassName = "Test National 1",
                HierarchyName = HierarchyNames.National,
                HierarchyLevelName = HierarchyLevelNames.NationalFamily,
                Action = Contracts.ActionEnum.Delete,
                HierarchyClassTraits = new Dictionary<string, string>(),
            };
            command.HierarchyClasses = new List<InforHierarchyClassModel>
            {
                testModel
            };

            //When
            commandHandler.Execute(command);

            //Then
            var queuedEvents = context.EventQueue.Where(e => e.EventReferenceId == 1234);
            AssertEventsAreEquelToTestModel(testModel, queuedEvents, regions.Count, EventTypes.NationalClassDelete);
        }

        private void AssertEventsAreEquelToTestModel(InforHierarchyClassModel testModel, IQueryable<EventQueue> queuedEvents, int numberOfEvents, int eventTypeId)
        {
            Assert.AreEqual(numberOfEvents, queuedEvents.Count());
            foreach (var queuedEvent in queuedEvents)
            {
                Assert.AreEqual(testModel.HierarchyClassName, queuedEvent.EventMessage);
                Assert.AreEqual(eventTypeId, queuedEvent.EventId);
                Assert.IsTrue(regions.Contains(queuedEvent.RegionCode));
            }
            Assert.IsTrue(regions.OrderBy(r => r).SequenceEqual(queuedEvents.Select(q => q.RegionCode).OrderBy(r => r)));
        }
    }
}
