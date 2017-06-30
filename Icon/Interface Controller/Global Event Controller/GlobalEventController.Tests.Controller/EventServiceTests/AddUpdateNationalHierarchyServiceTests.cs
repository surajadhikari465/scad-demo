using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace GlobalEventController.Tests.Controller.EventServiceTests
{
    [TestClass]
    public class AddUpdateNationalHierarchyServiceTests
    {
        private AddOrUpdateNationalHierarchyEventService eventService;
        private Mock<ICommandHandler<AddOrUpdateNationalHierarchyCommand>> mockAddOrUpdateNationalHierarchyHandler;
        private Mock<IQueryHandler<GetHierarchyClassQuery, HierarchyClass>> mockGetHierarchyClassQueryHandler;

        [TestInitialize]
        public void InitializeData()
        {
            mockAddOrUpdateNationalHierarchyHandler = new Mock<ICommandHandler<AddOrUpdateNationalHierarchyCommand>>();
            mockGetHierarchyClassQueryHandler = new Mock<IQueryHandler<GetHierarchyClassQuery, HierarchyClass>>();
            eventService = new AddOrUpdateNationalHierarchyEventService(mockAddOrUpdateNationalHierarchyHandler.Object, mockGetHierarchyClassQueryHandler.Object);
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
            mockGetHierarchyClassQueryHandler.Setup(q => q.Handle(It.IsAny<GetHierarchyClassQuery>()))
                .Returns(new HierarchyClass());
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
