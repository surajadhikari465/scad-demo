using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GlobalEventController.Controller.EventOperations;
using Icon.Logging;
using Moq;
using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;
using Icon.Testing.Builders;
using System.Data.Entity;
using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using Irma.Framework;
using GlobalEventController.DataAccess.Commands;


namespace GlobalEventController.Tests.Controller.EventServiceTests
{
    [TestClass]
    public class AddTaxClassEventServiceTests
    {

        private IrmaContext irmaContext;
        private Mock<ICommandHandler<AddTaxClassCommand>> addTaxClassHandler;
        private IEventService eventService;
        private Mock<IQueryHandler<GetHierarchyClassQuery, HierarchyClass>> getHierarchyClassHandler;

        [TestInitialize]
        public void InitializeData()
        {
            irmaContext = new IrmaContext();
            addTaxClassHandler = new Mock<ICommandHandler<AddTaxClassCommand>>();
            getHierarchyClassHandler = new Mock<IQueryHandler<GetHierarchyClassQuery, HierarchyClass>>();
            eventService = new AddTaxClassEventService(this.irmaContext, this.addTaxClassHandler.Object, this.getHierarchyClassHandler.Object);

        }

        [TestCleanup]
        public void Cleanup()
        {
            irmaContext.Dispose();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTaxClassEventService_EventReferenceIdIsNull_ArgumentExceptionThrown()
        {
            //Given
            eventService.ReferenceId = null;

            //When
            eventService.Run();

            //Then
            //Should get ArgumentException

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTaxClassEventService_EventReferenceIdIsLessThanZero_ArgumentExceptionThrown()
        {

            //Given
            eventService.ReferenceId = -1;

            //When
            eventService.Run();

            //Then
            //Should get ArgumentException            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTaxClassEventService_EventMessageIsNullOrEmpty_ArgumentExceptionThrown()
        {
            //Given
            eventService.ReferenceId = 1;
            eventService.Message = null;

            //When
            eventService.Run();

            //Then
            //Should get ArgumentException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTaxClassEventService_EventRegionIsNullOrEmpty_ArgumentExceptionThrown()
        {
            //Given
            eventService.ReferenceId = 1;
            eventService.Message = "TestMessage";
            eventService.Region = null;

            //When
            eventService.Run();

            //Then
            //Should get ArgumentException
        }

        [TestMethod]
        public void AddTaxClassEventService_TaxCodeAsReferenceIdValue_GetHierarchyClassQueryCalledOneTime()
        {

            //Given
            TestHierarchyClassBuilder testHierArchyClassBuilder = new TestHierarchyClassBuilder().WithHierarchyClassId(1).WithTaxAbbreviationTrait("TestTax");
            HierarchyClass testHierArchyClass = testHierArchyClassBuilder;
            testHierArchyClass.HierarchyClassTrait.FirstOrDefault().Trait = new Trait() { traitCode = TraitCodes.TaxAbbreviation };

            eventService.ReferenceId = 1;
            eventService.Message = "TestMessage";
            eventService.Region = "SE";
            getHierarchyClassHandler.Setup(q => q.Handle(It.IsAny<GetHierarchyClassQuery>()))
                .Returns(testHierArchyClass);

            //When
            eventService.Run();

            //Then           
            getHierarchyClassHandler.Verify(command => command.Handle(It.IsAny<GetHierarchyClassQuery>()), Times.Once);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddTaxClassEventService_HierarchyClassTaxAbbreviationTraitIsNull_InvalidOperationExceptionThrown()
        {
            //Given
            eventService.ReferenceId = 1;
            eventService.Message = "TestMessage";
            eventService.Region = "SE";

            TestHierarchyClassBuilder testHierArchyClass = new TestHierarchyClassBuilder().WithHierarchyClassId(1);

            getHierarchyClassHandler.Setup(q => q.Handle(It.IsAny<GetHierarchyClassQuery>()))
                .Returns(testHierArchyClass);

            //When
            eventService.Run();

            //Then
            //Should throw InvalidOprationexception.

        }

        [TestMethod]
        public void AddTaxClassEventService_TaxAbbreviationFound_AddTaxClassCommandHandlerCalledOneTime()
        {
            //Given
            TestHierarchyClassBuilder testHierArchyClassBuilder = new TestHierarchyClassBuilder().WithHierarchyClassId(1).WithTaxAbbreviationTrait("TestTax");
            HierarchyClass testHierArchyClass = testHierArchyClassBuilder;
            //Can this go into Builder class
            testHierArchyClass.HierarchyClassTrait.FirstOrDefault().Trait = new Trait() { traitCode = TraitCodes.TaxAbbreviation };
            eventService.ReferenceId = 1;
            eventService.Message = "TestMessage";
            eventService.Region = "SE";
            getHierarchyClassHandler.Setup(q => q.Handle(It.IsAny<GetHierarchyClassQuery>()))
                .Returns(testHierArchyClass);

            //When
            eventService.Run();

            //Then
            getHierarchyClassHandler.Verify(command => command.Handle(It.IsAny<GetHierarchyClassQuery>()), Times.Once);
        }
    }
}
