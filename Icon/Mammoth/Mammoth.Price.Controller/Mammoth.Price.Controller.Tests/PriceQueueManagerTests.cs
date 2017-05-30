using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Price.Controller.DataAccess.Queries;
using System.Collections.Generic;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.Email;
using Mammoth.Logging;
using Mammoth.Price.Controller.DataAccess.Models;
using Mammoth.Price.Controller.DataAccess.Commands;
using Mammoth.Price.Controller.ApplicationModules;
using System.Linq;
using Mammoth.Common.ControllerApplication;
using Mammoth.Common;

namespace Mammoth.Price.Controller.Tests
{
    [TestClass]
    public class PriceQueueManagerTests
    {
        private PriceControllerApplicationSettings settings;
        private Mock<IQueryHandler<UpdateAndGetEventQueueInProcessParameters, List<EventQueueModel>>> mockUpdateAndGetEventQueueInProcessQueryHandler;
        private Mock<IQueryHandler<GetPriceDataParameters, List<PriceEventModel>>> mockGetPricesQueryHandler;
        private Mock<IQueryHandler<GetExistingPriceDataParameters, List<PriceEventModel>>> mockGetExistingPricesQueryHandler;
        private Mock<ICommandHandler<DeleteEventQueueCommand>> mockDeleteEventQueueCommandHandler;
        private Mock<ICommandHandler<ArchiveEventsCommand>> mockArchiveEventsCommandHandler;
        private Mock<ILogger> logger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IEmailBuilder> mockEmailBuilder;

        private PriceQueueManager queueManager;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockUpdateAndGetEventQueueInProcessQueryHandler = new Mock<IQueryHandler<UpdateAndGetEventQueueInProcessParameters, List<EventQueueModel>>>();
            this.mockGetPricesQueryHandler = new Mock<IQueryHandler<GetPriceDataParameters, List<PriceEventModel>>>();
            this.mockGetExistingPricesQueryHandler = new Mock<IQueryHandler<GetExistingPriceDataParameters, List<PriceEventModel>>>();
            this.mockDeleteEventQueueCommandHandler = new Mock<ICommandHandler<DeleteEventQueueCommand>>();
            this.mockArchiveEventsCommandHandler = new Mock<ICommandHandler<ArchiveEventsCommand>>();
            this.logger = new Mock<ILogger>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockEmailBuilder = new Mock<IEmailBuilder>();

            this.settings = new PriceControllerApplicationSettings
            {
                Instance = 7777,
                ControllerName = "Price",
                CurrentRegion = "FL",
                MaxNumberOfRowsToMark = 100,
                Regions = new List<string> { "FL" },
                NonAlertErrors = new List<string>()
            };

            this.queueManager = new PriceQueueManager(
                this.settings,
                this.mockUpdateAndGetEventQueueInProcessQueryHandler.Object,
                this.mockGetPricesQueryHandler.Object,
                this.mockGetExistingPricesQueryHandler.Object,
                this.mockDeleteEventQueueCommandHandler.Object,
                this.mockArchiveEventsCommandHandler.Object,
                this.mockEmailClient.Object,
                this.mockEmailBuilder.Object,
                this.logger.Object);
        }

        [TestMethod]
        public void Get_EventsDoNotExistInQueue_ReturnsZeroChangeQueueEventModels()
        {
            // Given
            this.mockUpdateAndGetEventQueueInProcessQueryHandler
                .Setup(q => q.Search(It.IsAny<UpdateAndGetEventQueueInProcessParameters>())).Returns(new List<EventQueueModel>());

            // When
            var events = this.queueManager.Get();

            // Then
            Assert.AreEqual(0, events.EventModels.Count);
            Assert.AreEqual(0, events.QueuedEvents.Count);
            this.mockGetPricesQueryHandler.Verify(g => g.Search(It.IsAny<GetPriceDataParameters>()), Times.Never);
            this.mockGetExistingPricesQueryHandler.Verify(g => g.Search(It.IsAny<GetExistingPriceDataParameters>()), Times.Never);
        }

        [TestMethod]
        public void Get_EventsExistInQueue_ReturnsQueuedEventsAndPriceData()
        {
            // Given
            int expectedNumberOfItems = 5;
            List<EventQueueModel> eventQueueModels = BuildEventQueueModels(numberOfItems: expectedNumberOfItems);
            List<PriceEventModel> priceEventModels = BuildPriceEventModels(numberOfItems: expectedNumberOfItems, startIndex: 0);

            this.mockUpdateAndGetEventQueueInProcessQueryHandler
                .Setup(q => q.Search(It.IsAny<UpdateAndGetEventQueueInProcessParameters>()))
                .Returns(eventQueueModels);
            this.mockGetPricesQueryHandler
                .Setup(p => p.Search(It.IsAny<GetPriceDataParameters>()))
                .Returns(priceEventModels);
            this.mockGetExistingPricesQueryHandler
                .Setup(e => e.Search(It.IsAny<GetExistingPriceDataParameters>()))
                .Returns(new List<PriceEventModel>());

            // When
            var events = this.queueManager.Get();

            // Then
            Assert.AreEqual(expectedNumberOfItems, events.EventModels.Count);
            Assert.AreEqual(expectedNumberOfItems, events.QueuedEvents.Count);
            this.mockGetPricesQueryHandler.Verify(g => g.Search(It.IsAny<GetPriceDataParameters>()), Times.Once);
            this.mockGetExistingPricesQueryHandler.Verify(g => g.Search(It.IsAny<GetExistingPriceDataParameters>()), Times.Once);
        }

        [TestMethod]
        public void Get_EventsExistInQueueForFutureAndExistingPrices_ReturnsFutureAndExistingPrices()
        {
            // Given
            int expectedNumberOfQueuedEvents = 10;
            int expectedNumberOfFuturePriceModel = 5;
            int expectedNumberOfExistingPriceModel = 5;

            List<EventQueueModel> eventQueueModels = BuildEventQueueModels(numberOfItems: expectedNumberOfQueuedEvents);
            List<PriceEventModel> priceEventModels = BuildPriceEventModels(numberOfItems: expectedNumberOfFuturePriceModel, startIndex: 0);
            List<PriceEventModel> existingPriceEventModel = BuildPriceEventModels(numberOfItems: expectedNumberOfExistingPriceModel, startIndex: expectedNumberOfExistingPriceModel + 1);

            this.mockUpdateAndGetEventQueueInProcessQueryHandler
                .Setup(q => q.Search(It.IsAny<UpdateAndGetEventQueueInProcessParameters>()))
                .Returns(eventQueueModels);
            this.mockGetPricesQueryHandler
                .Setup(p => p.Search(It.IsAny<GetPriceDataParameters>()))
                .Returns(priceEventModels);
            this.mockGetExistingPricesQueryHandler
                .Setup(e => e.Search(It.IsAny<GetExistingPriceDataParameters>()))
                .Returns(existingPriceEventModel);

            // When
            var events = this.queueManager.Get();

            // Then
            Assert.AreEqual(expectedNumberOfQueuedEvents, events.QueuedEvents.Count);
            Assert.AreEqual(expectedNumberOfFuturePriceModel + expectedNumberOfExistingPriceModel, events.EventModels.Count);
        }

        [TestMethod]
        public void Get_EventsExistInQueueOnlyForFuturePrices_ReturnsFuturePricesOnly()
        {
            // Given
            int expectedNumberOfQueuedEvents = 10;
            int expectedNumberOfFuturePriceModel = 5;
            int expectedNumberOfExistingPriceModel = 0;

            List<EventQueueModel> eventQueueModels = BuildEventQueueModels(numberOfItems: expectedNumberOfQueuedEvents);
            List<PriceEventModel> priceEventModels = BuildPriceEventModels(numberOfItems: expectedNumberOfFuturePriceModel, startIndex: 0);

            this.mockUpdateAndGetEventQueueInProcessQueryHandler
                .Setup(q => q.Search(It.IsAny<UpdateAndGetEventQueueInProcessParameters>()))
                .Returns(eventQueueModels);
            this.mockGetPricesQueryHandler
                .Setup(p => p.Search(It.IsAny<GetPriceDataParameters>()))
                .Returns(priceEventModels);
            this.mockGetExistingPricesQueryHandler
                .Setup(e => e.Search(It.IsAny<GetExistingPriceDataParameters>()))
                .Returns(new List<PriceEventModel>());

            // When
            var events = this.queueManager.Get();

            // Then
            Assert.AreEqual(expectedNumberOfQueuedEvents, events.QueuedEvents.Count);
            Assert.AreEqual(expectedNumberOfFuturePriceModel + expectedNumberOfExistingPriceModel, events.EventModels.Count);
        }

        [TestMethod]
        public void Get_EventsExistInQueueOnlyForExistingPrices_ReturnsExistingPricesOnly()
        {
            // Given
            int expectedNumberOfQueuedEvents = 10;
            int expectedNumberOfFuturePriceModel = 0;
            int expectedNumberOfExistingPriceModel = 5;

            List<EventQueueModel> eventQueueModels = BuildEventQueueModels(numberOfItems: expectedNumberOfQueuedEvents);
            List<PriceEventModel> priceEventModels = BuildPriceEventModels(numberOfItems: expectedNumberOfExistingPriceModel, startIndex: 0);

            this.mockUpdateAndGetEventQueueInProcessQueryHandler
                .Setup(q => q.Search(It.IsAny<UpdateAndGetEventQueueInProcessParameters>()))
                .Returns(eventQueueModels);
            this.mockGetPricesQueryHandler
                .Setup(p => p.Search(It.IsAny<GetPriceDataParameters>()))
                .Returns(new List<PriceEventModel>());
            this.mockGetExistingPricesQueryHandler
                .Setup(e => e.Search(It.IsAny<GetExistingPriceDataParameters>()))
                .Returns(priceEventModels);

            // When
            var events = this.queueManager.Get();

            // Then
            Assert.AreEqual(expectedNumberOfQueuedEvents, events.QueuedEvents.Count);
            Assert.AreEqual(expectedNumberOfFuturePriceModel + expectedNumberOfExistingPriceModel, events.EventModels.Count);
        }

        [TestMethod]
        public void Get_EventsExistInQueueWithNoPriceData_ReturnsOnlyQueuedEvents()
        {
            // Given
            int expectedNumberOfQueuedEvents = 10;
            int expectedNumberOfFuturePriceModel = 0;
            int expectedNumberOfExistingPriceModel = 0;

            List<EventQueueModel> eventQueueModels = BuildEventQueueModels(numberOfItems: expectedNumberOfQueuedEvents);

            this.mockUpdateAndGetEventQueueInProcessQueryHandler
                .Setup(q => q.Search(It.IsAny<UpdateAndGetEventQueueInProcessParameters>()))
                .Returns(eventQueueModels);
            this.mockGetPricesQueryHandler
                .Setup(p => p.Search(It.IsAny<GetPriceDataParameters>()))
                .Returns(new List<PriceEventModel>());
            this.mockGetExistingPricesQueryHandler
                .Setup(e => e.Search(It.IsAny<GetExistingPriceDataParameters>()))
                .Returns(new List<PriceEventModel>());

            // When
            var events = this.queueManager.Get();

            // Then
            Assert.AreEqual(expectedNumberOfQueuedEvents, events.QueuedEvents.Count);
            Assert.AreEqual(expectedNumberOfFuturePriceModel + expectedNumberOfExistingPriceModel, events.EventModels.Count);
        }

        [TestMethod]
        public void Finalize_GivenChangeQueueEventModel_ArchivesAndDeletesEventsOneTime()
        {
            // Given
            ChangeQueueEvents<PriceEventModel> changeQueueEvents = new ChangeQueueEvents<PriceEventModel>();
            var eventQueueModel = new EventQueueModel
            {
                QueueId = 1
            };
            changeQueueEvents.QueuedEvents.Add(eventQueueModel);

            // When
            this.queueManager.Finalize(changeQueueEvents);

            // Then
            this.mockDeleteEventQueueCommandHandler.Verify(d => d.Execute(It.IsAny<DeleteEventQueueCommand>()), Times.Once);
            this.mockArchiveEventsCommandHandler.Verify(a => a.Execute(It.IsAny<ArchiveEventsCommand>()), Times.Once);
        }

        [TestMethod]
        public void Finalize_GivenNoChangeQueueEventModel_DoesNotDeleteOrArchiveEvents()
        {
            // Given
            ChangeQueueEvents<PriceEventModel> changeQueueEvents = new ChangeQueueEvents<PriceEventModel>();

            // When
            this.queueManager.Finalize(changeQueueEvents);

            // Then
            this.mockDeleteEventQueueCommandHandler.Verify(d => d.Execute(It.IsAny<DeleteEventQueueCommand>()), Times.Never);
            this.mockArchiveEventsCommandHandler.Verify(a => a.Execute(It.IsAny<ArchiveEventsCommand>()), Times.Never);
        }

        [TestMethod]
        public void Finalize_GivenQueueEventsWithoutPriceEventModel_ArchivesQueuedEventsWithUnprocessableEventsError()
        {
            // Given
            ChangeQueueEvents<PriceEventModel> changeQueueEvents = new ChangeQueueEvents<PriceEventModel>();
            var eventQueueModel = new EventQueueModel
            {
                QueueId = 1
            };;

            changeQueueEvents.QueuedEvents.Add(eventQueueModel);

            // When
            this.queueManager.Finalize(changeQueueEvents);

            // Then
            this.mockDeleteEventQueueCommandHandler
                .Verify(d => d.Execute(It.Is<DeleteEventQueueCommand>(c => c.QueueIds.Count() == 1)), Times.Once);
            this.mockArchiveEventsCommandHandler
                .Verify(a => a.Execute(It.Is<ArchiveEventsCommand>(c => c.Events.All(e => e.ErrorCode == ApplicationErrors.UnprocessableEventCode))), Times.Once);
        }

        [TestMethod]
        public void Finalize_GivenQueueEventsWithPriceDataAndNoErrors_ArchivesQueuedEventsWithNoUnprocessableEventsError()
        {
            // Given
            ChangeQueueEvents<PriceEventModel> changeQueueEvents = new ChangeQueueEvents<PriceEventModel>();
            var eventQueueModel = new EventQueueModel
            {
                QueueId = 1
            };

            var priceEventModel = new PriceEventModel
            {
                QueueId = 1
            };

            changeQueueEvents.QueuedEvents.Add(eventQueueModel);
            changeQueueEvents.EventModels.Add(priceEventModel);

            // When
            this.queueManager.Finalize(changeQueueEvents);

            // Then
            this.mockDeleteEventQueueCommandHandler
                .Verify(d => d.Execute(It.Is<DeleteEventQueueCommand>(c => c.QueueIds.Count() == 1)), Times.Once);
            this.mockArchiveEventsCommandHandler
                .Verify(a => a.Execute(It.Is<ArchiveEventsCommand>(c => 
                    c.Events.All(e => e.ErrorCode != ApplicationErrors.UnprocessableEventCode)
                    && c.Events.All(x => x.QueueID == eventQueueModel.QueueId))), Times.Once);
        }

        [TestMethod]
        public void Finalize_GivenQueueEventsWithPriceDataWhichHasErrors_ArchivesQueuedEventsWithNoUnprocessableEventsError()
        {
            // Given
            ChangeQueueEvents<PriceEventModel> changeQueueEvents = new ChangeQueueEvents<PriceEventModel>();
            var eventQueueModel = new EventQueueModel
            {
                QueueId = 1,
            };

            var priceEventModel = new PriceEventModel
            {
                QueueId = 1,
                BusinessUnitId = 1,
                ScanCode = "1",
                EventTypeId = Constants.EventTypes.Price,
                ErrorMessage = "Error Adding Price",
                ErrorDetails = "Error Details Adding Price",
                ErrorSource = Constants.SourceSystem.MammothWebApi
            };

            changeQueueEvents.QueuedEvents.Add(eventQueueModel);
            changeQueueEvents.EventModels.Add(priceEventModel);

            // When
            this.queueManager.Finalize(changeQueueEvents);

            // Then
            this.mockDeleteEventQueueCommandHandler
                .Verify(d => d.Execute(It.Is<DeleteEventQueueCommand>(c => c.QueueIds.Count() == 1)), Times.Once);
            this.mockArchiveEventsCommandHandler
                .Verify(a => a.Execute(It.Is<ArchiveEventsCommand>(c =>
                    c.Events.All(e => e.ErrorCode == priceEventModel.ErrorMessage
                    && c.Events.All(y => y.ErrorDetails == priceEventModel.ErrorDetails))
                    && c.Events.All(x => x.QueueID == eventQueueModel.QueueId))), Times.Once);
            this.mockEmailClient.Verify(c => c.Send(It.IsAny<string>(), "Mammoth Price Error - ACTION REQUIRED"), Times.Once);
        }

        [TestMethod]
        public void Finalize_GivenQueueEventsWithPriceDataWhichHasInvalidDataErrors_DoesNotSendEmailAlert()
        {
            // Given
            settings.NonAlertErrors.Add(ApplicationErrors.InvalidDataErrorCode);

            ChangeQueueEvents<PriceEventModel> changeQueueEvents = new ChangeQueueEvents<PriceEventModel>();
            var eventQueueModel = new EventQueueModel
            {
                QueueId = 1,
            };

            var priceEventModel = new PriceEventModel
            {
                QueueId = 1,
                BusinessUnitId = 1,
                ScanCode = "1",
                EventTypeId = Constants.EventTypes.Price,
                ErrorMessage = ApplicationErrors.InvalidDataErrorCode,
                ErrorDetails = "Error Details Adding Price"
            };

            changeQueueEvents.QueuedEvents.Add(eventQueueModel);
            changeQueueEvents.EventModels.Add(priceEventModel);

            // When
            this.queueManager.Finalize(changeQueueEvents);

            // Then
            this.mockDeleteEventQueueCommandHandler
                .Verify(d => d.Execute(It.Is<DeleteEventQueueCommand>(c => c.QueueIds.Count() == 1)), Times.Once);
            this.mockArchiveEventsCommandHandler
                .Verify(a => a.Execute(It.Is<ArchiveEventsCommand>(c =>
                    c.Events.All(e => e.ErrorCode == priceEventModel.ErrorMessage
                    && c.Events.All(y => y.ErrorDetails == priceEventModel.ErrorDetails))
                    && c.Events.All(x => x.QueueID == eventQueueModel.QueueId))), Times.Once);
            this.mockEmailClient.Verify(c => c.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        private List<EventQueueModel> BuildEventQueueModels(int numberOfItems)
        {
            List<EventQueueModel> eventQueueModels = new List<EventQueueModel>();

            for (int i = 0; i < numberOfItems; i++)
            {
                EventQueueModel eventQueue = new EventQueueModel
                {
                    EventReferenceId = i,
                    EventTypeId = 3,
                    Identifier = i.ToString(),
                    InProcessBy = i,
                    InsertDate = DateTime.Now,
                    ItemKey = i,
                    QueueId = i,
                    StoreNo = i
                };

                eventQueueModels.Add(eventQueue);
            }

            return eventQueueModels;
        }

        private List<PriceEventModel> BuildPriceEventModels(int numberOfItems, int startIndex)
        {
            List<PriceEventModel> priceEventModels = new List<PriceEventModel>();

            for (int i = 0; i < numberOfItems; i++)
            {
                PriceEventModel priceModel = new PriceEventModel
                {
                    QueueId = startIndex + i
                };

                priceEventModels.Add(priceModel);
            }

            return priceEventModels;
        }
    }
}
