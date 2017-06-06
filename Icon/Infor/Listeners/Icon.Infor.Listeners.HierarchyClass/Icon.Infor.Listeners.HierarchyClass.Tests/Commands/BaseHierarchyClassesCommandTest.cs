﻿using Icon.Common.Context;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class BaseHierarchyClassesCommandTest
    {
        protected List<string> regions;
        protected Mock<IRenewableContext<IconContext>> mockRenewableContext;
        protected IconContext context;
        protected DbContextTransaction transaction;

        public const int id87654321 = 87654321;
        public const int id1234 = 1234;

        [TestInitialize]
        public void BaseInitialize()
        {
            regions = new List<string> { "FL", "MA", "MW" };

            context = new IconContext();
            mockRenewableContext = new Mock<IRenewableContext<IconContext>>();
            mockRenewableContext.SetupGet(m => m.Context).Returns(context);

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void BaseCleanup()
        {
            if (transaction.UnderlyingTransaction.Connection != null)
                transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }
        protected void SetCommandHierarchyClasses(GenerateHierarchyClassEventsCommand generateHierarchyClassEventsCommand,
            params InforHierarchyClassModel[] inforHierarchyClassModels)
        {
            generateHierarchyClassEventsCommand.HierarchyClasses = inforHierarchyClassModels?.ToList();
        }

        protected void AssertEventsAreEqualToTestModel(InforHierarchyClassModel testModel,
            IQueryable<EventQueue> queuedEvents, int numberOfEvents, int eventTypeId)
        {
            Assert.AreEqual(numberOfEvents, queuedEvents.Count());
            foreach (var queuedEvent in queuedEvents)
            {
                Assert.AreEqual(testModel.HierarchyClassName, queuedEvent.EventMessage);
                Assert.AreEqual(eventTypeId, queuedEvent.EventId);
                Assert.IsTrue(regions.Contains(queuedEvent.RegionCode));
            }
            Assert.IsTrue(regions.OrderBy(r => r)
                .SequenceEqual(queuedEvents.Select(q => q.RegionCode).OrderBy(r => r)));
        }

        protected InforHierarchyClassModel CreateInforHierarchyClassModel(int hierarchyClassId,
            string hierarchyClassName, string hierarchyName, string hierarchyLevelName,
            ActionEnum action, Dictionary<string, string> hierarchyClassTraits = null)
        {
            InforHierarchyClassModel testModel = new InforHierarchyClassModel
            {
                HierarchyClassId = hierarchyClassId,
                HierarchyClassName = hierarchyClassName,
                HierarchyName = hierarchyName,
                HierarchyLevelName = hierarchyLevelName,
                Action = action,
                HierarchyClassTraits = hierarchyClassTraits ?? new Dictionary<string, string>(),
            };
            return testModel;
        }

        protected InforHierarchyClassModel CreateInforHierarchyClassModel(string hierarchyName,
            string hierarchyLevelName, ActionEnum action, Dictionary<string, string> hierarchyClassTraits = null)
        {
            var defaultClassId = id87654321;
            var defaultHierarchyClassName = hierarchyName == "Financial"
                ? "Test Financial HierarchyClass(1234)"
                : $"Test {hierarchyName} 1";

            return CreateInforHierarchyClassModel(defaultClassId,
                defaultHierarchyClassName, hierarchyName, hierarchyLevelName,
                action, hierarchyClassTraits);
        }

        protected List<Framework.HierarchyClass> CreateTestHierarchyClassesForDelete(
            int hierarchyId, int numToCreate = 1, bool wantAssociatedItemHierarchy = false)
        {
            List<Framework.HierarchyClass> hierarchyClasses = new List<Framework.HierarchyClass>(numToCreate);

            var hierarchyClassName = String.Empty;
            var hierarchyLevel = 1;
            var traitID = 0;
            var traitValue = "Test";

            switch (hierarchyId)
            {
                case Hierarchies.Brands:
                    hierarchyClassName = $"Test Brand";
                    traitID = Traits.BrandAbbreviation;
                    break;
                case Hierarchies.National:
                    hierarchyClassName = $"Test National";
                    traitID = Traits.NationalClassCode;
                    break;
                case Hierarchies.Merchandise:
                case Hierarchies.Tax:
                case Hierarchies.Browsing:
                case Hierarchies.Financial:
                case Hierarchies.CertificationAgencyManagement:
                default:
                    throw new NotImplementedException();
            }

            for (int i = 0; i < numToCreate; i++)
            {
                var hierarchyClass = new Framework.HierarchyClass
                {
                    hierarchyID = hierarchyId,
                    hierarchyClassName = $"{hierarchyClassName} {i + 1}",
                    hierarchyLevel = hierarchyLevel,
                    HierarchyClassTrait = new List<HierarchyClassTrait>
                    {
                        new HierarchyClassTrait { traitID = traitID, traitValue = traitValue }
                    }
                };
                if (wantAssociatedItemHierarchy && i == 0)
                {
                    hierarchyClass.ItemHierarchyClass = new List<ItemHierarchyClass>
                    {
                        new ItemHierarchyClass
                        {
                            Item = new Item
                            {
                                itemTypeID = ItemTypes.RetailSale } } };
                }
                hierarchyClasses.Add(hierarchyClass);
            }

            return hierarchyClasses;
        }

        protected void SaveTestHierarchyClasses(IconContext iconContext,
            IEnumerable<Framework.HierarchyClass> hierarchyClasses)
        {
            iconContext.HierarchyClass.AddRange(hierarchyClasses);
            iconContext.SaveChanges();
        }

        protected void AddDataToDeleteCommand(
            DeleteHierarchyClassesCommand deleteCommand,
            IEnumerable<Framework.HierarchyClass> hierarchyClassData,
            string hierarchyName)
        {
            deleteCommand.HierarchyClasses = hierarchyClassData
                .Select(hc => new InforHierarchyClassModel
                {
                    HierarchyClassId = hc.hierarchyClassID,
                    HierarchyName = hierarchyName,
                    Action = ActionEnum.Delete
                }).ToList();
        }

        protected int GetQueuedEventCount(IconContext iconContext, IEnumerable<string> regions, DateTime since)
        {
            return iconContext.EventQueue
                .Count(e => regions.Contains(e.RegionCode) && e.InsertDate >= since);

        }

        protected IQueryable<EventQueue> GetQueuedEvents(IconContext iconContext, int eventReferenceId)
        {
            return iconContext.EventQueue.Where(e => e.EventReferenceId == eventReferenceId);

        }
        protected IQueryable<EventQueue> GetQueuedEvents(IconContext iconContext, string eventMessage)
        {
            return iconContext.EventQueue.Where(e => e.EventMessage == eventMessage);

        }
    }
}
