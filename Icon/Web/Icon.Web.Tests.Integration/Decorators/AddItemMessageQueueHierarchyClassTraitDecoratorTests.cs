using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Decorators;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Icon.Web.Tests.Integration.Decorators
{
    [TestClass]
    public class AddItemMessageQueueHierarchyClassTraitDecoratorTests
    {
        private AddItemMessageQueueHierarchyClassTraitDecorator decorator;
        private Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>> mockCommandHandler;
        private Mock<ICommandHandler<AddMessageQueueItemByHierarchyClassIdCommand>> mockAddMessageQueueCommandHandler;
        private Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>> mockGetHierarchyClassQueryHandler;
        private Mock<ILogger> logger;
        private UpdateHierarchyClassTraitCommand command;

        [TestInitialize]
        public void Initialize()
        {
            this.mockCommandHandler = new Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>>();
            this.mockAddMessageQueueCommandHandler = new Mock<ICommandHandler<AddMessageQueueItemByHierarchyClassIdCommand>>();
            this.mockGetHierarchyClassQueryHandler = new Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>>();
            this.logger = new Mock<ILogger>();
            this.decorator = new AddItemMessageQueueHierarchyClassTraitDecorator(this.mockCommandHandler.Object,
                this.mockAddMessageQueueCommandHandler.Object,
                this.mockGetHierarchyClassQueryHandler.Object,
                this.logger.Object);

            this.command = new UpdateHierarchyClassTraitCommand
            {
                UpdatedHierarchyClass = new HierarchyClass
                {
                    hierarchyClassID = 2,
                    hierarchyClassName = "Test UpdateItemsDecorator by SubBrick",
                    hierarchyID = Hierarchies.Merchandise,
                    hierarchyLevel = 5,
                    hierarchyParentClassID = 1
                }
            };
        }

        [TestMethod]
        public void AddMessageQueueItemDecoratorExecute_MerchGs1BrickWithProhibitDiscountChangeAndTwoChildSubBricks_ExecutesUpdateHierarchyClassTraitOneTimeAndAddMessageQueueItemCommandsTwoTimes()
        {
            // Given
            int expectedHierarchyClassId1 = 999;
            int expectedHierarchyClassId2 = 888;

            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.UpdatedHierarchyClass.hierarchyLevel = HierarchyLevels.Gs1Brick;
            this.command.ProhibitDiscountChanged = true;

            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassID = this.command.UpdatedHierarchyClass.hierarchyClassID,
                hierarchyParentClassID = this.command.UpdatedHierarchyClass.hierarchyParentClassID,
                HierarchyClass1 = new List<HierarchyClass>
                {
                    new HierarchyClass { hierarchyClassID = expectedHierarchyClassId1 },
                    new HierarchyClass { hierarchyClassID = expectedHierarchyClassId2 }
                }
            };
            this.mockGetHierarchyClassQueryHandler
                .Setup(h => h.Search(It.Is<GetHierarchyClassByIdParameters>(x => x.HierarchyClassId == this.command.UpdatedHierarchyClass.hierarchyClassID)))
                .Returns(hierarchyClass);

            // for verifying each speciric call to UpdateItemsByHierarchyClassCommandHandler
            var invocations = new List<AddMessageQueueItemByHierarchyClassIdCommand>();
            this.mockAddMessageQueueCommandHandler
                .Setup(f => f.Execute(It.IsAny<AddMessageQueueItemByHierarchyClassIdCommand>()))
                .Callback<AddMessageQueueItemByHierarchyClassIdCommand>(c =>
                    invocations.Add(new AddMessageQueueItemByHierarchyClassIdCommand
                    {
                        HierarchyClassId = c.HierarchyClassId
                    }));

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockAddMessageQueueCommandHandler.Verify(ic => ic.Execute(It.IsAny<AddMessageQueueItemByHierarchyClassIdCommand>()), Times.Exactly(2));

            Assert.AreEqual(expectedHierarchyClassId1, invocations[0].HierarchyClassId);
            Assert.AreEqual(expectedHierarchyClassId2, invocations[1].HierarchyClassId);
        }

        [TestMethod]
        public void AddMessageQueueItemDecoratorExecute_MerchSubBrickWithNonMerchTraitChange_ExecutesUpdateHierarchyClassTraitAndAddMessageQueueItemCommands()
        {
            // Given
            int expectedHierarchyClassId = 8778;
            this.command.UpdatedHierarchyClass.hierarchyClassID = expectedHierarchyClassId;
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.UpdatedHierarchyClass.hierarchyLevel = HierarchyLevels.SubBrick;
            this.command.NonMerchandiseTraitChanged = true;

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockAddMessageQueueCommandHandler.Verify(ic => ic.Execute(It.Is<AddMessageQueueItemByHierarchyClassIdCommand>(c => 
                c.HierarchyClassId == expectedHierarchyClassId)), Times.Once);
        }

        [TestMethod]
        public void AddMessageQueueItemDecoratorExecute_MerchSubBrickWithSubTeamChange_ExecutesUpdateHierarchyClassTraitAndAddMessageQueueItemCommands()
        {
            // Given
            int expectedHierarchyClassId = 8778;
            this.command.UpdatedHierarchyClass.hierarchyClassID = expectedHierarchyClassId;
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.UpdatedHierarchyClass.hierarchyLevel = HierarchyLevels.SubBrick;
            this.command.SubteamChanged = true;

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockAddMessageQueueCommandHandler.Verify(ic => ic.Execute(It.Is<AddMessageQueueItemByHierarchyClassIdCommand>(c =>
                c.HierarchyClassId == expectedHierarchyClassId)), Times.Once);
        }

        [TestMethod]
        public void AddMessageQueueItemDecoratorExecute_MerchAtSegmentLevelWithNoTraitChange_OnlyExecutesUpdateHierarchyClassTraitCommand()
        {
            // Given
            int expectedHierarchyClassId = 8778;
            this.command.UpdatedHierarchyClass.hierarchyClassID = expectedHierarchyClassId;
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.UpdatedHierarchyClass.hierarchyLevel = HierarchyLevels.Segment;
            this.command.SubteamChanged = false;
            this.command.ProhibitDiscountChanged = false;
            this.command.NonMerchandiseTraitChanged = false;

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockAddMessageQueueCommandHandler.Verify(ic => ic.Execute(It.IsAny<AddMessageQueueItemByHierarchyClassIdCommand>()), Times.Never);
        }

        [TestMethod]
        public void AddMessageQueueItemDecoratorExecute_MerchSubBrickWithoutTraitChange_OnlyExecutesUpdateHierarchyClassTraitCommand()
        {
            // Given
            int expectedHierarchyClassId = 8778;
            this.command.UpdatedHierarchyClass.hierarchyClassID = expectedHierarchyClassId;
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.UpdatedHierarchyClass.hierarchyLevel = HierarchyLevels.SubBrick;
            this.command.SubteamChanged = false;
            this.command.ProhibitDiscountChanged = false;
            this.command.NonMerchandiseTraitChanged = false;

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockAddMessageQueueCommandHandler.Verify(ic => ic.Execute(It.IsAny<AddMessageQueueItemByHierarchyClassIdCommand>()), Times.Never);
        }

        [TestMethod]
        public void AddMessageQueueItemDecoratorExecute_TaxHierarchyChange_OnlyExecutesUpdateHierarchyClassTraitCommand()
        {
            // Given
            int expectedHierarchyClassId = 8778;
            this.command.UpdatedHierarchyClass.hierarchyClassID = expectedHierarchyClassId;
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Tax;
            this.command.UpdatedHierarchyClass.hierarchyLevel = HierarchyLevels.Tax;
            this.command.SubteamChanged = false;
            this.command.ProhibitDiscountChanged = false;
            this.command.NonMerchandiseTraitChanged = false;

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockAddMessageQueueCommandHandler.Verify(ic => ic.Execute(It.IsAny<AddMessageQueueItemByHierarchyClassIdCommand>()), Times.Never);
        }
    }
}
