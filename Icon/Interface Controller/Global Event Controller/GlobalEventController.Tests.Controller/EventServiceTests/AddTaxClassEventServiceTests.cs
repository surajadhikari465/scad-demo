using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using Icon.Testing.Builders;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GlobalEventController.Tests.Controller.EventServiceTests
{
    [TestClass]
    public class AddTaxClassEventServiceTests
    {
        private IrmaContext irmaContext;
        private IEventService eventService;
        private Mock<ICommandHandler<AddTaxClassCommand>> mockAddTaxClassHandler;
        private Mock<IQueryHandler<GetTaxAbbreviationQuery, string>> mockGetTaxAbbreviationQueryHandler;

        [TestInitialize]
        public void InitializeData()
        {
            irmaContext = new IrmaContext();
            mockAddTaxClassHandler = new Mock<ICommandHandler<AddTaxClassCommand>>();
            mockGetTaxAbbreviationQueryHandler = new Mock<IQueryHandler<GetTaxAbbreviationQuery, string>>();

            eventService = new AddTaxClassEventService(this.irmaContext,
                this.mockAddTaxClassHandler.Object,
                this.mockGetTaxAbbreviationQueryHandler.Object);
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
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddTaxClassEventService_HierarchyClassTaxAbbreviationTraitIsNull_InvalidOperationExceptionThrown()
        {
            //Given
            eventService.ReferenceId = 1;
            eventService.Message = "TestMessage";
            eventService.Region = "SE";

            mockGetTaxAbbreviationQueryHandler.Setup(q => q.Handle(It.IsAny<GetTaxAbbreviationQuery>()))
                .Returns((string)null);

            //When
            eventService.Run();

            //Then
            //Should get InvalidOperationException
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddTaxClassEventService_HierarchyClassTaxAbbreviationTraitIsEmpty_InvalidOperationExceptionThrown()
        {
            //Given
            eventService.ReferenceId = 1;
            eventService.Message = "TestMessage";
            eventService.Region = "SE";

            mockGetTaxAbbreviationQueryHandler.Setup(q => q.Handle(It.IsAny<GetTaxAbbreviationQuery>()))
                .Returns(String.Empty);

            //When
            eventService.Run();

            //Then
            //Should get InvalidOperationException
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddTaxClassEventService_HierarchyClassTaxAbbreviationTraitIsWhitespaceInvalidOperationExceptionThrown()
        {
            //Given
            eventService.ReferenceId = 1;
            eventService.Message = "TestMessage";
            eventService.Region = "SE";

            mockGetTaxAbbreviationQueryHandler.Setup(q => q.Handle(It.IsAny<GetTaxAbbreviationQuery>()))
                .Returns("  ");

            //When
            eventService.Run();

            //Then
            //Should get InvalidOperationException
        }

        [TestMethod]
        public void AddTaxClassEventService_TaxCodeAsReferenceIdValue_GetTaxAbbreviationQueryQueryCalledOnce()
        {
            //Given
            eventService.ReferenceId = 999;
            eventService.Message = "TestMessage";
            eventService.Region = "SE";
            mockGetTaxAbbreviationQueryHandler.Setup(q => q.Handle(It.IsAny<GetTaxAbbreviationQuery>()))
                .Returns("7654321 STUFF - THINGS");

            //When
            eventService.Run();

            //Then           
            mockGetTaxAbbreviationQueryHandler.Verify(h => h.Handle(It.Is<GetTaxAbbreviationQuery>(
                    (qry) => qry.HierarchyClassId == 999 && qry.TaxTraitCode == TraitCodes.TaxAbbreviation)),
                    Times.Once);
        }

        [TestMethod]
        public void AddTaxClassEventService_TaxAbbreviationFound_AddTaxClassCommandHandlerCalledOnce()
        {
            //Given;
            eventService.ReferenceId = 1;
            eventService.Message = "TestMessage";
            eventService.Region = "SE";
            mockGetTaxAbbreviationQueryHandler.Setup(q => q.Handle(It.IsAny<GetTaxAbbreviationQuery>()))
                .Returns("7654321 STUFF - THINGS");

            //When
            eventService.Run();

            //Then
            mockAddTaxClassHandler.Verify(h => h.Handle(It.Is<AddTaxClassCommand>(
                (cmd) => cmd.TaxCode== "TestMessage" && cmd.TaxClassDescription== "7654321 STUFF - THINGS")),
                Times.Once);
        }
    }
}