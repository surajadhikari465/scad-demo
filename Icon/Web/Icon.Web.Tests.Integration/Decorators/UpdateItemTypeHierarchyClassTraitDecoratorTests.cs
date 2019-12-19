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
    public class UpdateItemTypeHierarchyClassTraitDecoratorTests
    {
        private UpdateItemTypeHierarchyClassTraitDecorator decorator;
        private Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>> mockCommandHandler;
        private Mock<ICommandHandler<UpdateItemTypeByHierarchyClassCommand>> mockUpdateItemsCommandHandler;
        private Mock<ILogger> logger;
        private UpdateHierarchyClassTraitCommand command;
        private string expectedModifiedDateTimeUtc;

        [TestInitialize]
        public void Initialize()
        {
            this.mockCommandHandler = new Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>>();
            this.mockUpdateItemsCommandHandler = new Mock<ICommandHandler<UpdateItemTypeByHierarchyClassCommand>>();
            this.logger = new Mock<ILogger>();
            this.decorator = new UpdateItemTypeHierarchyClassTraitDecorator(this.mockCommandHandler.Object,
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
        public void UpdateItemTypeDecoratorExecute_TaxHierarchyClassUpdated_OnlyExecutesUpdateHierarchyClassTraitCommandHandler()
        {
            // Given
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Tax;

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockUpdateItemsCommandHandler.Verify(ic => ic.Execute(It.IsAny<UpdateItemTypeByHierarchyClassCommand>()), Times.Never);
        }

        [TestMethod]
        public void UpdateItemTypeDecoratorExecute_NonMerchandiseTraitsDidNotChange_OnlyExecutesUpdateHierarchyClassTraitCommandHandler()
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
            this.mockUpdateItemsCommandHandler.Verify(ic => ic.Execute(It.IsAny<UpdateItemTypeByHierarchyClassCommand>()), Times.Never);
        }

        [TestMethod]
        public void UpdateItemTypeDecoratorExecute_MerchandiseLevelIsNotSubBrick_OnlyExecutesUpdateHierarchyClassTraitCommandHandler()
        {
            // Given
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.NonMerchandiseTraitChanged = true;
            this.command.UpdatedHierarchyClass.hierarchyLevel = HierarchyLevels.Gs1Brick;

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockUpdateItemsCommandHandler.Verify(ic => ic.Execute(It.IsAny<UpdateItemTypeByHierarchyClassCommand>()), Times.Never);
        }

        [TestMethod]
        public void UpdateItemTypeDecoratorExecute_MerchSubBrickAndNonMerchTraitChanged_ExecutesUpdateTraitAndItemCommandHandlerOneTime()
        {
            // Given
            int expectedItemTypeId = ItemTypes.Fee;
            int expectedHierarchyClassId = 2;
            string expectedUserName = "TestUserName";

            this.command.UpdatedHierarchyClass.hierarchyClassID = expectedHierarchyClassId;
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.NonMerchandiseTraitChanged = true;
            this.command.SubteamChanged = false;
            this.command.ProhibitDiscountChanged = false;
            this.command.NonMerchandiseTrait = NonMerchandiseTraits.BlackhawkFee;
            this.command.UserName = expectedUserName;

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockUpdateItemsCommandHandler
                .Verify(ch => ch
                    .Execute(It.Is<UpdateItemTypeByHierarchyClassCommand>(c =>
                        c.HierarchyClassId == expectedHierarchyClassId
                        && c.ItemTypeId == expectedItemTypeId
                        && c.UserName == expectedUserName
                        && c.ModifiedDateTimeUtc == expectedModifiedDateTimeUtc)),
                    Times.Once);
        }
    }
}
