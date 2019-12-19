using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Decorators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Icon.Web.Tests.Integration.Decorators
{
    [TestClass]
    public class UpdateItemSubTeamHierarchyClassTraitDecoratorTests
    {
        private UpdateItemSubTeamHierarchyClassTraitDecorator decorator;
        private Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>> mockCommandHandler;
        private Mock<ICommandHandler<UpdateItemSubTeamByHierarchyClassCommand>> mockUpdateItemsCommandHandler;
        private Mock<ILogger> logger;
        private UpdateHierarchyClassTraitCommand command;
        private string expectedModifiedDateTimeUtc;

        [TestInitialize]
        public void Initialize()
        {
            this.mockCommandHandler = new Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>>();
            this.mockUpdateItemsCommandHandler = new Mock<ICommandHandler<UpdateItemSubTeamByHierarchyClassCommand>>();
            this.logger = new Mock<ILogger>();
            this.decorator = new UpdateItemSubTeamHierarchyClassTraitDecorator(this.mockCommandHandler.Object,
                this.mockUpdateItemsCommandHandler.Object,
                this.logger.Object);
            this.expectedModifiedDateTimeUtc = DateTime.UtcNow.ToFormattedDateTimeString();

            this.command = new UpdateHierarchyClassTraitCommand
            {
                UpdatedHierarchyClass = new HierarchyClass
                {
                    hierarchyClassID = 2,
                    hierarchyClassName = "Test UpdateItemsDecorator by SubBrick",
                    hierarchyID = Hierarchies.Merchandise,
                    hierarchyLevel = 5,
                    hierarchyParentClassID = 1
                },
                ModifiedDateTimeUtc = expectedModifiedDateTimeUtc
            };
        }

        [TestMethod]
        public void UpdateItemSubTeamDecoratorExecute_TaxHierarchyClassUpdated_OnlyExecutesUpdateHierarchyClassTraitCommandHandler()
        {
            // Given
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Tax;

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockUpdateItemsCommandHandler.Verify(ic => ic.Execute(It.IsAny<UpdateItemSubTeamByHierarchyClassCommand>()), Times.Never);
        }

        [TestMethod]
        public void UpdateItemSubTeamDecoratorExecute_NonMerchandiseTraitDidNotChange_OnlyExecutesUpdateHierarchyClassTraitCommandHandler()
        {
            // Given
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.NonMerchandiseTraitChanged = false;

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockUpdateItemsCommandHandler.Verify(ic => ic.Execute(It.IsAny<UpdateItemSubTeamByHierarchyClassCommand>()), Times.Never);
        }

        [TestMethod]
        public void UpdateItemSubTeamDecoratorExecute_MerchSubBrickAndSubTeamChanged_ExecutesUpdateTraitAndItemCommandHandlerOneTime()
        {
            // Given
            int expectedHierarchyClassId = 2;
            string expectedUserName = "TestUserName";
            int expectedSubTeamId = 555;

            this.command.UpdatedHierarchyClass.hierarchyClassID = expectedHierarchyClassId;
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.UpdatedHierarchyClass.hierarchyLevel = HierarchyLevels.SubBrick;
            this.command.SubteamChanged = true;
            this.command.UserName = expectedUserName;
            this.command.SubTeamHierarchyClassId = expectedSubTeamId;

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockUpdateItemsCommandHandler
                .Verify(ch => ch
                    .Execute(It.Is<UpdateItemSubTeamByHierarchyClassCommand>(c =>
                        c.HierarchyClassId == expectedHierarchyClassId
                        && c.SubTeamHierarchyClassId == expectedSubTeamId
                        && c.UserName == expectedUserName
                        && c.ModifiedDateTimeUtc == expectedModifiedDateTimeUtc)),
                    Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateItemSubTeamDecoratorExecute_MerchSubBrickAndSubTeamIdIsZero_ExecutesUpdateTraitAndThrowsArgumentException()
        {
            // Given
            int expectedHierarchyClassId = 2;
            string expectedUserName = "TestUserName";
            int expectedSubTeamId = 0;

            this.command.UpdatedHierarchyClass.hierarchyClassID = expectedHierarchyClassId;
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.UpdatedHierarchyClass.hierarchyLevel = HierarchyLevels.SubBrick;
            this.command.SubteamChanged = true;
            this.command.UserName = expectedUserName;
            this.command.SubTeamHierarchyClassId = expectedSubTeamId;

            // When
            this.decorator.Execute(this.command);

            // Then
            // Expected Argument Exception
        }
    }
}
