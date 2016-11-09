using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Decorators;
using Icon.Web.DataAccess.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Unit.Decorators
{
    [TestClass]
    public class UpdateSubTeamMammothEventDecoratorTests
    {
        private IconContext context;
        private DbContextTransaction transaction;
        private UpdateSubTeamMammothEventDecorator updateSubTeamMammothEventDecorator;
        private UpdateHierarchyClassTraitCommand updateHierarchyClassTraitCommand;
        private Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>> mockBulkImportCommandHandler;
        private Mock<ILogger> mockLogger;

        private int maxQueueId = 0;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();

            updateHierarchyClassTraitCommand = new UpdateHierarchyClassTraitCommand();

            mockBulkImportCommandHandler = new Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>>();
            mockLogger = new Mock<ILogger>();
            updateSubTeamMammothEventDecorator = new UpdateSubTeamMammothEventDecorator(mockBulkImportCommandHandler.Object, context, mockLogger.Object);

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (transaction != null)
            {
                transaction.Rollback();
            }
        }

        [TestMethod]
        public void UpdateProductSubTeamInIcon_UpdateSubTeamMammothEventDecoratorCalled_ShouldCallUpdateHierarchyClassTraitCommandHandler()
        {
            //Given
            updateHierarchyClassTraitCommand.SubTeamHierarchyClassId = (from ih in context.ItemHierarchyClass
                                                                        join ht in context.HierarchyClassTrait on ih.hierarchyClassID equals ht.hierarchyClassID
                                                                        join t in context.Trait on ht.traitID equals t.traitID
                                                                        where t.traitID == Traits.MerchFinMapping
                                                                        select ih.hierarchyClassID).First();

            //When
            updateSubTeamMammothEventDecorator.Execute(updateHierarchyClassTraitCommand);

            //Then
            mockBulkImportCommandHandler.Verify(v => v.Execute(It.IsAny<UpdateHierarchyClassTraitCommand>()), Times.Once);
        }

        [TestMethod]
        public void UpdateProductSubTeamInIcon_UpdateSubTeamMammothEventDecoratorCalled_MammothProductUpdateEventsCreated()
        {
            //Given
            updateHierarchyClassTraitCommand.SubteamChanged = true;

            updateHierarchyClassTraitCommand.SubTeamHierarchyClassId = (from ih in context.ItemHierarchyClass
                                                                        join ht in context.HierarchyClassTrait on ih.hierarchyClassID equals ht.hierarchyClassID
                                                                        join t in context.Trait on ht.traitID equals t.traitID
                                                                        where t.traitID == Traits.MerchFinMapping
                                                                        select ih.hierarchyClassID).First();

            updateHierarchyClassTraitCommand.UpdatedHierarchyClass = context.HierarchyClass.Where(hc => hc.hierarchyClassID == updateHierarchyClassTraitCommand.SubTeamHierarchyClassId).First();

            updateHierarchyClassTraitCommand.TeamName = "Test Team Name";

            maxQueueId = context.Database.SqlQuery<int>("select count(QueueId) from mammoth.EventQueue").First();

            if (maxQueueId > 0)
                maxQueueId = context.Database.SqlQuery<int>("select max(QueueId) from mammoth.EventQueue").First();

            List<string> scanCodes = (from sc in context.ScanCode
                                      join i in context.Item on sc.itemID equals i.itemID
                                      join ih in context.ItemHierarchyClass on i.itemID equals ih.itemID
                                      where (ih.hierarchyClassID == updateHierarchyClassTraitCommand.SubTeamHierarchyClassId)
                                      orderby sc.scanCode
                                      select sc.scanCode).ToList();

            //When
            updateSubTeamMammothEventDecorator.Execute(updateHierarchyClassTraitCommand);

            //Then
            var actualEventMessage = context.Database.SqlQuery<string>("select EventMessage from mammoth.EventQueue where QueueId > " + this.maxQueueId.ToString() + " order by EventMessage").ToList();
            var actualEventTypeId = context.Database.SqlQuery<int>("select EventTypeId from mammoth.EventQueue where QueueId > " + this.maxQueueId.ToString()).ToList();

            for (int i = 0; i < scanCodes.Count(); i++)
            {
                Assert.AreEqual(scanCodes[i], actualEventMessage[i].ToString(), "The inserted event's EventMessage did not match the expected value.");
                Assert.AreEqual(MommothEventTypes.Productupdate, actualEventTypeId[i], "The inserted event's EventTypeId did not match the expected value.");
            }
        }
    }
}
