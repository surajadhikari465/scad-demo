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
    public class UpdateTaxClassEventServiceTests
    {
        private UpdateTaxClassEventService eventService;
        private Mock<ICommandHandler<UpdateTaxClassCommand>> mockUpdateTaxClassHandler;
        private Mock<IQueryHandler<GetTaxAbbreviationQuery, string>> mockGetTaxAbbreviationQueryHandler;

        [TestInitialize]
        public void InitializeData()
        {
            mockUpdateTaxClassHandler = new Mock<ICommandHandler<UpdateTaxClassCommand>>();
            mockGetTaxAbbreviationQueryHandler = new Mock<IQueryHandler<GetTaxAbbreviationQuery, string>>();
            eventService = new UpdateTaxClassEventService(
                this.mockUpdateTaxClassHandler.Object,
                this.mockGetTaxAbbreviationQueryHandler.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateTaxClassEventServiceRun_EventReferenceIdIsNull_ArgumentExceptionThrown()
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
        public void UpdateTaxClassEventServiceRun_EventReferenceIdIsLessThanZero_ArgumentExceptionThrown()
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
        public void UpdateTaxClassEventServiceRun_EventMessageIsNullOrEmpty_ArgumentExceptionThrown()
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
        public void UpdateTaxClassEventServiceRun_EventRegionIsNullOrEmpty_ArgumentExceptionThrown()
        {
            //Given
            eventService.ReferenceId = 1;
            eventService.Message = "TestMessage";
            eventService.Region = null;

            //When
            eventService.Run();

            //Then
            //Should throw ArgumentException
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UpdateTaxClassEventServiceRun_HierarchyClassTaxAbbreviationTraitIsNull_InvalidOperationExceptionThrown()
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
        public void UpdateTaxClassEventServiceRun_HierarchyClassTaxAbbreviationTraitIsEmpty_InvalidOperationExceptionThrown()
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
        public void UpdateTaxClassEventServiceRun_HierarchyClassTaxAbbreviationTraitIsWhitespaceInvalidOperationExceptionThrown()
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
        public void UpdateTaxClassEventServiceRun_TaxCodeAsReferenceIdValue_GetTaxAbbreviationQueryQueryCalledOneTime()
        {
            //Given
            eventService.ReferenceId = 999;
            eventService.Message = "TestMessage";
            eventService.Region = "SE";
            mockGetTaxAbbreviationQueryHandler.Setup(h => h.Handle(It.Is<GetTaxAbbreviationQuery>(
                (qry) => qry.HierarchyClassId == eventService.ReferenceId && qry.TaxTraitCode == TraitCodes.TaxAbbreviation)))
                .Returns("7654321 STUFF - THINGS");

            //When
            eventService.Run();

            //Then           
            mockGetTaxAbbreviationQueryHandler.Verify(h => h.Handle(It.Is<GetTaxAbbreviationQuery>(
                    (qry) => qry.HierarchyClassId == 999 && qry.TaxTraitCode == "ABR")),
                    Times.Once);
        }

        [TestMethod]
        public void UpdateTaxClassEventServiceRun_HierarchyClassTaxAbbreviationFound_UpdateTaxClassCommandCalledOneTime()
        {
            //Given
            eventService.ReferenceId = 1;
            eventService.Message = "TestMessage";
            eventService.Region = "SE";
            mockGetTaxAbbreviationQueryHandler.Setup(h => h.Handle(It.Is<GetTaxAbbreviationQuery>(
                (qry) => qry.HierarchyClassId==1 && qry.TaxTraitCode=="ABR")))
                .Returns("7654321 STUFF - THINGS");

            //When
            eventService.Run();

            //Then
            mockUpdateTaxClassHandler.Verify(h => h.Handle(It.Is<UpdateTaxClassCommand>(
                (cmd) => cmd.TaxCode == "TestMessage" && cmd.TaxClassDescription == "7654321 STUFF - THINGS")),
                Times.Once);
        }
    }
}
