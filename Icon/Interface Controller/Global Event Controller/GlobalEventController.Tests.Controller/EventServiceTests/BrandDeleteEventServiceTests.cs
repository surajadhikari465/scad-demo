using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace GlobalEventController.Tests.Controller.EventServiceTests
{
    [TestClass]
    public class BrandDeleteEventServiceTests
    {
        private IrmaContext irmaContext;
        private IEventService eventService;
        private Mock<ICommandHandler<BrandDeleteCommand>> brandDeleteHandler;
        private Mock<ICommandHandler<AddUpdateLastChangeByIdentifiersCommand>> updateLastChangeHandler;
        private Mock<IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>>> getItemIdentifiersHandler;


        [TestInitialize]
        public void InitializeData()
        {
            irmaContext = new IrmaContext();
            brandDeleteHandler = new Mock<ICommandHandler<BrandDeleteCommand>>();
            updateLastChangeHandler = new Mock<ICommandHandler<AddUpdateLastChangeByIdentifiersCommand>>();
            getItemIdentifiersHandler = new Mock<IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>>>();
            eventService = new BrandDeleteEventService(irmaContext, brandDeleteHandler.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            irmaContext.Dispose();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BrandDeleteEventService_EventReferenceIdIsNull_ArgumentExceptionThrown()
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
        public void BrandDeleteEventService_EventReferenceIdIsLessThanZero_ArgumentExceptionThrown()
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
        public void BrandDeleteEventService_EventMessageIsNullOrEmpty_ArgumentExceptionThrown()
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
        public void BrandDeleteEventService_EventRegionIsNullOrEmpty_ArgumentExceptionThrown()
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
        public void BrandDeleteEventService_BrandEventMessage_BrandDeleteCommandHandlerCalledOnedTime()
        {
            //Given
            brandDeleteHandler.Setup(q => q.Handle(It.IsAny<BrandDeleteCommand>()));
            getItemIdentifiersHandler.Setup(r => r.Handle(It.IsAny<GetItemIdentifiersQuery>())).Returns(new List<ItemIdentifier> { new ItemIdentifier { Deleted_Identifier = 0, Default_Identifier = 1, Remove_Identifier = 0 } });
            updateLastChangeHandler.Setup(l => l.Handle(It.IsAny<AddUpdateLastChangeByIdentifiersCommand>()));
            eventService.ReferenceId = 1;
            eventService.Message = "TestBrandName";
            eventService.Region = "TestRegion";

            //When
            eventService.Run();

            //Then         
            brandDeleteHandler.Verify(command => command.Handle(It.IsAny<BrandDeleteCommand>()), Times.Once);
        }
    }
}
