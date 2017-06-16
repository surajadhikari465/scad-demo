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
    public class AddUpdateNationalHierarchyServiceTests
    {
        private IrmaContext irmaContext;
        private IconContext iconContext;
        private IEventService eventService;
        private Mock<ICommandHandler<AddOrUpdateNationalHierarchyCommand>> mockAddOrUpdateNationalHierarchyHandler;
        private Mock<IQueryHandler<GetHierarchyClassQuery, HierarchyClass>> mockGetHierarchyClassQueryHandler;

        [TestInitialize]
        public void InitializeData()
        {
            irmaContext = new IrmaContext();
            iconContext = new IconContext();
            mockAddOrUpdateNationalHierarchyHandler = new Mock<ICommandHandler<AddOrUpdateNationalHierarchyCommand>>();
            mockGetHierarchyClassQueryHandler = new Mock<IQueryHandler<GetHierarchyClassQuery, HierarchyClass>>();
            eventService = new AddOrUpdateNationalHierarchyEventService(irmaContext, iconContext, mockAddOrUpdateNationalHierarchyHandler.Object, mockGetHierarchyClassQueryHandler.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            irmaContext.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddOrUpdateNationalHierarchyEventService_EventReferenceIdIsNull_ArgumentExceptionThrown()
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
        public void AddOrUpdateNationalHierarchyEventService_EventReferenceIdIsLessThanZero_ArgumentExceptionThrown()
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
        public void AddOrUpdateNationalHierarchyEventService_EventMessageIsNullOrEmpty_ArgumentExceptionThrown()
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
        public void AddOrUpdateNationalHierarchyEventService_EventRegionIsNullOrEmpty_ArgumentExceptionThrown()
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
        public void AddOrUpdateNationalHierarchyEventService_HeirarchyEventMessage_AddOrUpdateNationalHierarchyCommandHandlerCalledOneTime()
        {
            //Given
            mockAddOrUpdateNationalHierarchyHandler.Setup(q => q.Handle(It.IsAny<AddOrUpdateNationalHierarchyCommand>()));
            eventService.ReferenceId = 1;
            eventService.Message = "TestHierarchyName";
            eventService.Region = "TestRegion";

            //When
            eventService.Run();

            //Then         
            mockAddOrUpdateNationalHierarchyHandler.Verify(command => command.Handle(It.IsAny<AddOrUpdateNationalHierarchyCommand>()), Times.Once);
        }
    }
}
