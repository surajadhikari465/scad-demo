using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.ItemLocale.Controller.ApplicationModules;
using Moq;
using System.Collections.Generic;
using Mammoth.ItemLocale.Controller.DataAccess.Commands;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using Mammoth.ItemLocale.Controller.DataAccess.Queries;
using Mammoth.Common.Email;
using Mammoth.Logging;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.ControllerApplication;
using System.Linq;
using Mammoth.Common;

namespace Mammoth.ItemLocale.Controller.ApplicationModules.Tests
{
    [TestClass]
    public class ItemLocaleQueueManagerTests
    {
        private ItemLocaleQueueManager queueManager;
        private ItemLocaleControllerApplicationSettings settings;
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetItemLocaleDataParameters, List<ItemLocaleEventModel>>> mockGetItemLocaleQuery;
        private Mock<ICommandHandler<DeleteEventQueueCommand>> mockDeleteEventQueueCommandHandler;
        private Mock<IQueryHandler<UpdateAndGetEventQueueInProcessQuery, List<EventQueueModel>>> mockUpdateAndGetEventQueueInProcessQueryHandler;
        private Mock<ICommandHandler<ArchiveEventsCommand>> mockArchiveEventsCommandHandler;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IEmailBuilder> mockEmailBuilder;

        [TestInitialize]
        public void InitializeTests()
        {
            this.settings = new ItemLocaleControllerApplicationSettings { CurrentRegion = "FL", MaxNumberOfRowsToMark = 100, Instance = 1111 };
            this.mockGetItemLocaleQuery = new Mock<IQueryHandler<GetItemLocaleDataParameters, List<ItemLocaleEventModel>>>();
            this.mockDeleteEventQueueCommandHandler = new Mock<ICommandHandler<DeleteEventQueueCommand>>();
            this.mockUpdateAndGetEventQueueInProcessQueryHandler = new Mock<IQueryHandler<UpdateAndGetEventQueueInProcessQuery, List<EventQueueModel>>>();
            this.mockArchiveEventsCommandHandler = new Mock<ICommandHandler<ArchiveEventsCommand>>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.mockEmailBuilder = new Mock<IEmailBuilder>();
            this.mockLogger = new Mock<ILogger>();

            this.queueManager = new ItemLocaleQueueManager(
                this.settings,
                this.mockUpdateAndGetEventQueueInProcessQueryHandler.Object,
                this.mockGetItemLocaleQuery.Object,
                this.mockDeleteEventQueueCommandHandler.Object,
                this.mockArchiveEventsCommandHandler.Object,
                this.mockEmailClient.Object,
                this.mockEmailBuilder.Object,
                this.mockLogger.Object);
        }

        [TestMethod]
        public void Get_EventsDoNotExistInQueue_ReturnsZeroChangeQueueEventModels()
        {
            // Given
            this.mockUpdateAndGetEventQueueInProcessQueryHandler
                .Setup(q => q.Search(It.IsAny<UpdateAndGetEventQueueInProcessQuery>())).Returns(new List<EventQueueModel>());

            // When
            var events = this.queueManager.Get();

            // Then
            Assert.AreEqual(0, events.EventModels.Count);
            Assert.AreEqual(0, events.QueuedEvents.Count);
            this.mockGetItemLocaleQuery.Verify(g => g.Search(It.IsAny<GetItemLocaleDataParameters>()), Times.Never);
        }

        [TestMethod]
        public void Get_EventsExistInQueue_ReturnsQueuedEventsAndItemLocaleData()
        {
            // Given
            int expectedNumberOfItems = 5;
            List<EventQueueModel> eventQueueModels = BuildEventQueueModels(numberOfItems: expectedNumberOfItems);
            List<ItemLocaleEventModel> itemLocaleEventModels = BuildItemLocaleEventModels(numberOfItems: expectedNumberOfItems, startIndex: 0);

            this.mockUpdateAndGetEventQueueInProcessQueryHandler
                .Setup(q => q.Search(It.IsAny<UpdateAndGetEventQueueInProcessQuery>()))
                .Returns(eventQueueModels);
            this.mockGetItemLocaleQuery
                .Setup(p => p.Search(It.IsAny<GetItemLocaleDataParameters>()))
                .Returns(itemLocaleEventModels);

            // When
            var events = this.queueManager.Get();

            // Then
            Assert.AreEqual(expectedNumberOfItems, events.EventModels.Count);
            Assert.AreEqual(expectedNumberOfItems, events.QueuedEvents.Count);
            this.mockUpdateAndGetEventQueueInProcessQueryHandler.Verify(g => g.Search(It.IsAny<UpdateAndGetEventQueueInProcessQuery>()), Times.Once);
            this.mockGetItemLocaleQuery.Verify(g => g.Search(It.IsAny<GetItemLocaleDataParameters>()), Times.Once);
        }

        [TestMethod]
        public void Get_EventsExistInQueueWithNoItemLocaleData_ReturnsOnlyQueuedEvents()
        {
            // Given
            int expectedNumberOfQueuedEvents = 10;
            int expectedNumberOItemLocaleModel = 0;

            List<EventQueueModel> eventQueueModels = BuildEventQueueModels(numberOfItems: expectedNumberOfQueuedEvents);

            this.mockUpdateAndGetEventQueueInProcessQueryHandler
                .Setup(q => q.Search(It.IsAny<UpdateAndGetEventQueueInProcessQuery>()))
                .Returns(eventQueueModels);
            this.mockGetItemLocaleQuery
                .Setup(p => p.Search(It.IsAny<GetItemLocaleDataParameters>()))
                .Returns(new List<ItemLocaleEventModel>());

            // When
            var events = this.queueManager.Get();

            // Then
            Assert.AreEqual(expectedNumberOfQueuedEvents, events.QueuedEvents.Count);
            Assert.AreEqual(expectedNumberOItemLocaleModel, events.EventModels.Count);
            this.mockUpdateAndGetEventQueueInProcessQueryHandler.Verify(g => g.Search(It.IsAny<UpdateAndGetEventQueueInProcessQuery>()), Times.Once);
            this.mockGetItemLocaleQuery.Verify(g => g.Search(It.IsAny<GetItemLocaleDataParameters>()), Times.Once);
        }

        [TestMethod]
        public void Finalize_GivenChangeQueueEventModel_ArchivesAndDeletesEvents()
        {
            // Given
            ChangeQueueEvents<ItemLocaleEventModel> changeQueueEvents = new ChangeQueueEvents<ItemLocaleEventModel>();
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
            this.mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Finalize_GivenNoChangeQueueEventModel_DoesNotDeleteOrArchiveEvents()
        {
            // Given
            ChangeQueueEvents<ItemLocaleEventModel> changeQueueEvents = new ChangeQueueEvents<ItemLocaleEventModel>();

            // When
            this.queueManager.Finalize(changeQueueEvents);

            // Then
            this.mockDeleteEventQueueCommandHandler.Verify(d => d.Execute(It.IsAny<DeleteEventQueueCommand>()), Times.Never);
            this.mockArchiveEventsCommandHandler.Verify(a => a.Execute(It.IsAny<ArchiveEventsCommand>()), Times.Never);
            this.mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Finalize_GivenQueueEventsWithoutItemLocaleEventModel_ArchivesQueuedEventsWithUnprocessableEventsError()
        {
            // Given
            ChangeQueueEvents<ItemLocaleEventModel> changeQueueEvents = new ChangeQueueEvents<ItemLocaleEventModel>();
            var eventQueueModel = new EventQueueModel
            {
                QueueId = 1
            };

            changeQueueEvents.QueuedEvents.Add(eventQueueModel);

            // When
            this.queueManager.Finalize(changeQueueEvents);

            // Then
            this.mockDeleteEventQueueCommandHandler
                .Verify(d => d.Execute(It.Is<DeleteEventQueueCommand>(c => c.QueueIds.Count() == 1)), Times.Once);
            this.mockArchiveEventsCommandHandler
                .Verify(a => a.Execute(It.Is<ArchiveEventsCommand>(c => c.Events.All(e => e.ErrorCode == ApplicationErrors.UnprocessableEventCode))), Times.Once);
            this.mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Finalize_GivenQueueEventsWithItemLocaleDataAndNoErrors_DeletesAndArchivesQueuedEventsWithNoErrorsAndDoesNotSendEmailAlert()
        {
            // Given
            ChangeQueueEvents<ItemLocaleEventModel> changeQueueEvents = new ChangeQueueEvents<ItemLocaleEventModel>();
            var eventQueueModel = new EventQueueModel
            {
                QueueId = 1
            };

            var itemLocaleEventModel = new ItemLocaleEventModel
            {
                QueueId = 1
            };

            changeQueueEvents.QueuedEvents.Add(eventQueueModel);
            changeQueueEvents.EventModels.Add(itemLocaleEventModel);

            // When
            this.queueManager.Finalize(changeQueueEvents);

            // Then
            this.mockDeleteEventQueueCommandHandler
                .Verify(d => d.Execute(It.Is<DeleteEventQueueCommand>(c => c.QueueIds.Count() == 1)), Times.Once);
            this.mockArchiveEventsCommandHandler
                .Verify(a => a.Execute(It.Is<ArchiveEventsCommand>(c =>
                    c.Events.All(e => e.ErrorCode != ApplicationErrors.UnprocessableEventCode)
                    && c.Events.All(x => x.QueueID == eventQueueModel.QueueId))), Times.Once);
            this.mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Finalize_GivenQueueEventsWithItemLocaleDataWhichHasErrors_DeletesAndArchivesQueuedEventsWithEventsErrorAndSendsAlert()
        {
            // Given
            ChangeQueueEvents<ItemLocaleEventModel> changeQueueEvents = new ChangeQueueEvents<ItemLocaleEventModel>();
            var eventQueueModel = new EventQueueModel
            {
                QueueId = 1
            };

            var itemLocaleEventModel = new ItemLocaleEventModel
            {
                QueueId = 1,
                BusinessUnitId = 1,
                ScanCode = "1",
                EventTypeId = Constants.EventTypes.Price,
                ErrorMessage = "Error Adding ItemLocale",
                ErrorDetails = "Error Details Adding ItemLocale",
                ErrorSource = Constants.SourceSystem.MammothWebApi
            };

            changeQueueEvents.QueuedEvents.Add(eventQueueModel);
            changeQueueEvents.EventModels.Add(itemLocaleEventModel);

            // When
            this.queueManager.Finalize(changeQueueEvents);

            // Then
            this.mockDeleteEventQueueCommandHandler
                .Verify(d => d.Execute(It.Is<DeleteEventQueueCommand>(c => c.QueueIds.Count() == 1)), Times.Once);
            this.mockArchiveEventsCommandHandler
                .Verify(a => a.Execute(It.Is<ArchiveEventsCommand>(c =>
                    c.Events.All(e => e.ErrorCode == itemLocaleEventModel.ErrorMessage)
                    && c.Events.All(x => x.QueueID == eventQueueModel.QueueId))), Times.Once);
            this.mockEmailClient.Verify(ec => ec.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
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

        private List<ItemLocaleEventModel> BuildItemLocaleEventModels(int numberOfItems, int startIndex)
        {
            List<ItemLocaleEventModel> itemLocaleEventModels = new List<ItemLocaleEventModel>();

            for (int i = 0; i < numberOfItems; i++)
            {
                ItemLocaleEventModel itemLocaleModel = new ItemLocaleEventModel
                {
                    QueueId = startIndex + i
                };

                itemLocaleEventModels.Add(itemLocaleModel);
            }

            return itemLocaleEventModels;
        }
    }
}
