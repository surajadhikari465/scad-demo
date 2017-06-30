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
    public class UpdateBrandEventServiceTests
    {
        private UpdateBrandEventService eventService;
        private Mock<ICommandHandler<AddOrUpdateBrandCommand>> updateBrandHandler;       
        private Mock<ICommandHandler<AddUpdateLastChangeByIdentifiersCommand>> updateLastChangeHandler;
        private Mock<IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>>> getItemIdentifiersHandler;

        [TestInitialize]
        public void InitializeData()
        {
            updateBrandHandler = new Mock<ICommandHandler<AddOrUpdateBrandCommand>>();
            updateLastChangeHandler = new Mock<ICommandHandler<AddUpdateLastChangeByIdentifiersCommand>>();
            getItemIdentifiersHandler = new Mock<IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>>>();
            eventService = new UpdateBrandEventService(
                updateBrandHandler.Object, 
                updateLastChangeHandler.Object, 
                getItemIdentifiersHandler.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateBrandEventService_EventReferenceIdIsNull_ArgumentExceptionThrown()
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
        public void UpdateBrandEventService_EventReferenceIdIsLessThanZero_ArgumentExceptionThrown()
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
        public void UpdateBrandEventService_EventMessageIsNullOrEmpty_ArgumentExceptionThrown()
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
        public void UpdateBrandEventService_EventRegionIsNullOrEmpty_ArgumentExceptionThrown()
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
        public void UpdateBrandEventService_BrandEventMessage_UpdateBrandCommandHandlerCalledOnedTime()
        {
            //Given
            updateBrandHandler.Setup(q => q.Handle(It.IsAny<AddOrUpdateBrandCommand>()));
            getItemIdentifiersHandler.Setup(r => r.Handle(It.IsAny<GetItemIdentifiersQuery>())).Returns(new List<ItemIdentifier> {new ItemIdentifier {Deleted_Identifier = 0, Default_Identifier = 1, Remove_Identifier = 0}});
            updateLastChangeHandler.Setup(l => l.Handle(It.IsAny<AddUpdateLastChangeByIdentifiersCommand>()));
            eventService.ReferenceId = 1;
            eventService.Message = "TestBrandName";
            eventService.Region = "TestRegion";

            //When
            eventService.Run();

            //Then         
            updateBrandHandler.Verify(command => command.Handle(It.IsAny<AddOrUpdateBrandCommand>()), Times.Once);            
        }

        

        [TestMethod]
        public void UpdateBrandEventService_BrandEventMessage_GetItemIdentifiersQueryCalledOneTime()
        {
            //Given
            updateBrandHandler.Setup(q => q.Handle(It.IsAny<AddOrUpdateBrandCommand>()));
            getItemIdentifiersHandler.Setup(r => r.Handle(It.IsAny<GetItemIdentifiersQuery>())).Returns(new List<ItemIdentifier> { new ItemIdentifier { Deleted_Identifier = 0, Default_Identifier = 1, Remove_Identifier = 0 } });
            updateLastChangeHandler.Setup(l => l.Handle(It.IsAny<AddUpdateLastChangeByIdentifiersCommand>()));
            eventService.ReferenceId = 1;
            eventService.Message = "TestBrandName";
            eventService.Region = "TestRegion";

            //When
            eventService.Run();

            //Then         
            getItemIdentifiersHandler.Verify(command => command.Handle(It.IsAny<GetItemIdentifiersQuery>()), Times.Once);    
        }

        [TestMethod]
        public void UpdateBrandEventService_BrandEventMessage_AddUpdateLastChangeCommandCalledOneTime()
        {
            //Given
            updateBrandHandler.Setup(q => q.Handle(It.IsAny<AddOrUpdateBrandCommand>()));
            getItemIdentifiersHandler.Setup(r => r.Handle(It.IsAny<GetItemIdentifiersQuery>())).Returns(new List<ItemIdentifier> { new ItemIdentifier { Deleted_Identifier = 0, Default_Identifier = 1, Remove_Identifier = 0 } });
            updateLastChangeHandler.Setup(l => l.Handle(It.IsAny<AddUpdateLastChangeByIdentifiersCommand>()));
            eventService.ReferenceId = 1;
            eventService.Message = "TestBrandName";
            eventService.Region = "TestRegion";

            //When
            eventService.Run();

            //Then         
            updateLastChangeHandler.Verify(command => command.Handle(It.IsAny<AddUpdateLastChangeByIdentifiersCommand>()), Times.Once);    
        }
    }
}
