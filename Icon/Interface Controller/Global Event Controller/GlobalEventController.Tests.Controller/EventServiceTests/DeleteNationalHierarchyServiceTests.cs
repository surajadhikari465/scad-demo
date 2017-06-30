using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
namespace GlobalEventController.Tests.Controller.EventServiceTests
{
    [TestClass]
    public class DeleteNationalHierarchyServiceTests
    {
        private DeleteNationalHierarchyEventService eventService;
        private Mock<ICommandHandler<DeleteNationalHierarchyCommand>> deleteNationalHierarchyHandler;

        [TestInitialize]
        public void InitializeData()
        {
            deleteNationalHierarchyHandler = new Mock<ICommandHandler<DeleteNationalHierarchyCommand>>();
            eventService = new DeleteNationalHierarchyEventService(deleteNationalHierarchyHandler.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteNationalHierarchyEventService_EventReferenceIdIsNull_ArgumentExceptionThrown()
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
        public void DeleteNationalHierarchyEventService_EventReferenceIdIsLessThanZero_ArgumentExceptionThrown()
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
        public void DeleteNationalHierarchyEventService_EventMessageIsNullOrEmpty_ArgumentExceptionThrown()
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
        public void DeleteNationalHierarchyEventService_EventRegionIsNullOrEmpty_ArgumentExceptionThrown()
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
        public void DeleteNationalHierarchyEventService_HierarchyEventMessage_DeleteNationalHierarchyCommandHandlerCalledOneTime()
        {
            //Given
            deleteNationalHierarchyHandler.Setup(q => q.Handle(It.IsAny<DeleteNationalHierarchyCommand>()));
            eventService.ReferenceId = 1;
            eventService.Message = "TestHierarchyName";
            eventService.Region = "TestRegion";

            //When
            eventService.Run();

            //Then         
            deleteNationalHierarchyHandler.Verify(command => command.Handle(It.IsAny<DeleteNationalHierarchyCommand>()), Times.Once);
        }
    }
}
