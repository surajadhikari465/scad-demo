using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Decorators;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Integration.Decorators
{
    [TestClass]
    public class UpdateItemProhibitDiscountHierarchyClassTraitDecoratorTests
    {
        private UpdateItemProhibitDiscountHierarchyClassTraitDecorator decorator;
        private Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>> mockCommandHandler;
        private Mock<ICommandHandler<UpdateItemProhibitDiscountByHierarchyClassCommand>> mockUpdateItemsCommandHandler;
        private Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>> mockGetHierarchyClassQueryHandler;
        private Mock<ILogger> logger;
        private UpdateHierarchyClassTraitCommand command;
        private string expectedModifiedDateTimeUtc;

        [TestInitialize]
        public void Initialize()
        {
            this.mockCommandHandler = new Mock<ICommandHandler<UpdateHierarchyClassTraitCommand>>();
            this.mockUpdateItemsCommandHandler = new Mock<ICommandHandler<UpdateItemProhibitDiscountByHierarchyClassCommand>>();
            this.mockGetHierarchyClassQueryHandler = new Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>>();
            this.logger = new Mock<ILogger>();
            this.decorator = new UpdateItemProhibitDiscountHierarchyClassTraitDecorator(this.mockCommandHandler.Object,
                this.mockUpdateItemsCommandHandler.Object,
                this.mockGetHierarchyClassQueryHandler.Object,
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
        public void UpdateItemProhibitDiscountDecoratorExecute_TaxHierarchyClassUpdated_OnlyExecutesUpdateHierarchyClassTraitCommandHandler()
        {
            // Given
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Tax;

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockUpdateItemsCommandHandler.Verify(ic => ic.Execute(It.IsAny<UpdateItemProhibitDiscountByHierarchyClassCommand>()), Times.Never);
        }

        [TestMethod]
        public void UpdateItemProhibitDiscountDecoratorExecute_NonMerchandiseTraitDidNotChange_OnlyExecutesUpdateHierarchyClassTraitCommandHandler()
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
            this.mockUpdateItemsCommandHandler.Verify(ic => ic.Execute(It.IsAny<UpdateItemProhibitDiscountByHierarchyClassCommand>()), Times.Never);
        }

        [TestMethod]
        public void UpdateItemProhibitDiscountDecoratorExecute_MerchHierarchyIsNotGs1BrickLevel_OnlyExecutesUpdateHierarchyClassTraitCommandHandler()
        {
            // Given
            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.NonMerchandiseTraitChanged = true;
            this.command.UpdatedHierarchyClass.hierarchyLevel = HierarchyLevels.SubBrick;

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockUpdateItemsCommandHandler.Verify(ic => ic.Execute(It.IsAny<UpdateItemProhibitDiscountByHierarchyClassCommand>()), Times.Never);
        }

        [TestMethod]
        public void UpdateItemProhibitDiscountDecoratorExecute_Gs1BrickLevelWithTwoChildSubBricksAndProhibitDiscountChanged_ExecutesUpdateTraitOneTimeAndItemCommandHandlerTwoTimes()
        {
            // Given
            string expectedProhibitDiscount = "true";
            int expectedHierarchyClassId1 = 999;
            int expectedHierarchyClassId2 = 888;
            string expectedUserName = "TestUserName";

            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.UpdatedHierarchyClass.hierarchyLevel = HierarchyLevels.Gs1Brick;
            this.command.ProhibitDiscountChanged = true;
            this.command.ProhibitDiscount = expectedProhibitDiscount;
            this.command.UserName = expectedUserName;

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
            var invocations = new List<UpdateItemProhibitDiscountByHierarchyClassCommand>();
            this.mockUpdateItemsCommandHandler
                .Setup(f => f.Execute(It.IsAny<UpdateItemProhibitDiscountByHierarchyClassCommand>()))
                .Callback<UpdateItemProhibitDiscountByHierarchyClassCommand>(c =>
                    invocations.Add(new UpdateItemProhibitDiscountByHierarchyClassCommand
                    {
                        HierarchyClassId = c.HierarchyClassId,
                        ProhibitDiscount = c.ProhibitDiscount,
                        UserName = c.UserName,
                        ModifiedDateTimeUtc = c.ModifiedDateTimeUtc
                    }));

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockUpdateItemsCommandHandler.Verify(ic => ic.Execute(It.IsAny<UpdateItemProhibitDiscountByHierarchyClassCommand>()), Times.Exactly(2));

            Assert.AreEqual(expectedHierarchyClassId1, invocations[0].HierarchyClassId);
            Assert.AreEqual(expectedHierarchyClassId2, invocations[1].HierarchyClassId);
            foreach (var invocation in invocations)
            {

                Assert.AreEqual(expectedProhibitDiscount, invocation.ProhibitDiscount);
                Assert.AreEqual(expectedUserName, invocation.UserName);
                Assert.AreEqual(expectedModifiedDateTimeUtc, invocation.ModifiedDateTimeUtc);
            }
        }

        [TestMethod]
        public void UpdateItemProhibitDiscountDecoratorExecute_MerchGs1BrickLevelAndProhibitDiscountIsNull_ExecutesUpdateTraitAndUpdatesValuesAsFalse()
        {
            string expectedProhibitDiscount = "false";
            int expectedHierarchyClassId1 = 999;
            int expectedHierarchyClassId2 = 888;
            string expectedUserName = "TestUserName";

            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.UpdatedHierarchyClass.hierarchyLevel = HierarchyLevels.Gs1Brick;
            this.command.ProhibitDiscountChanged = true;
            this.command.ProhibitDiscount = null;
            this.command.UserName = expectedUserName;

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
            var invocations = new List<UpdateItemProhibitDiscountByHierarchyClassCommand>();
            this.mockUpdateItemsCommandHandler
                .Setup(f => f.Execute(It.IsAny<UpdateItemProhibitDiscountByHierarchyClassCommand>()))
                .Callback<UpdateItemProhibitDiscountByHierarchyClassCommand>(c =>
                    invocations.Add(new UpdateItemProhibitDiscountByHierarchyClassCommand
                    {
                        HierarchyClassId = c.HierarchyClassId,
                        ProhibitDiscount = c.ProhibitDiscount,
                        UserName = c.UserName,
                        ModifiedDateTimeUtc = c.ModifiedDateTimeUtc
                    }));

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockUpdateItemsCommandHandler.Verify(ic => ic.Execute(It.IsAny<UpdateItemProhibitDiscountByHierarchyClassCommand>()), Times.Exactly(2));

            Assert.AreEqual(expectedHierarchyClassId1, invocations[0].HierarchyClassId);
            Assert.AreEqual(expectedHierarchyClassId2, invocations[1].HierarchyClassId);
            foreach (var invocation in invocations)
            {

                Assert.AreEqual(expectedProhibitDiscount, invocation.ProhibitDiscount);
                Assert.AreEqual(expectedUserName, invocation.UserName);
                Assert.AreEqual(expectedModifiedDateTimeUtc, invocation.ModifiedDateTimeUtc);
            }
        }

        [TestMethod]
        public void UpdateItemProhibitDiscountDecoratorExecute_MerchGs1BrickLevelAndProhibitDiscountIsEmptyString_ExecutesUpdateTraitAndUpdatesValuesAsFalse()
        {
            string expectedProhibitDiscount = "false";
            int expectedHierarchyClassId1 = 999;
            int expectedHierarchyClassId2 = 888;
            string expectedUserName = "TestUserName";

            this.command.UpdatedHierarchyClass.hierarchyID = Hierarchies.Merchandise;
            this.command.UpdatedHierarchyClass.hierarchyLevel = HierarchyLevels.Gs1Brick;
            this.command.ProhibitDiscountChanged = true;
            this.command.ProhibitDiscount = string.Empty;
            this.command.UserName = expectedUserName;

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
            var invocations = new List<UpdateItemProhibitDiscountByHierarchyClassCommand>();
            this.mockUpdateItemsCommandHandler
                .Setup(f => f.Execute(It.IsAny<UpdateItemProhibitDiscountByHierarchyClassCommand>()))
                .Callback<UpdateItemProhibitDiscountByHierarchyClassCommand>(c =>
                    invocations.Add(new UpdateItemProhibitDiscountByHierarchyClassCommand
                    {
                        HierarchyClassId = c.HierarchyClassId,
                        ProhibitDiscount = c.ProhibitDiscount,
                        UserName = c.UserName,
                        ModifiedDateTimeUtc = c.ModifiedDateTimeUtc
                    }));

            // When
            this.decorator.Execute(this.command);

            // Then
            this.mockCommandHandler.Verify(ch => ch.Execute(It.Is<UpdateHierarchyClassTraitCommand>(c => c == this.command)),
                Times.Once,
                "UpdateHierarchyClassTrait was not called only once");
            this.mockUpdateItemsCommandHandler.Verify(ic => ic.Execute(It.IsAny<UpdateItemProhibitDiscountByHierarchyClassCommand>()), Times.Exactly(2));

            Assert.AreEqual(expectedHierarchyClassId1, invocations[0].HierarchyClassId);
            Assert.AreEqual(expectedHierarchyClassId2, invocations[1].HierarchyClassId);
            foreach (var invocation in invocations)
            {

                Assert.AreEqual(expectedProhibitDiscount, invocation.ProhibitDiscount);
                Assert.AreEqual(expectedUserName, invocation.UserName);
                Assert.AreEqual(expectedModifiedDateTimeUtc, invocation.ModifiedDateTimeUtc);
            }
        }
    }
}
