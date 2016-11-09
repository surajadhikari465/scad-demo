using Vim.Common.DataAccess;
using Vim.Common.DataAccess.Commands;
using Vim.Common.Email;
using Vim.Locale.Controller.ApplicationModules;
using Vim.Locale.Controller.DataAccess.Models;
using Vim.Locale.Controller.DataAccess.Queries;
using Vim.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vim.Locale.Controller.Tests.ApplicationModules
{
    [TestClass]
    public class LocalesQueueManagerTests
    {
        private LocaleQueueManager queueManager;
        private ControllerApplicationSettings settings;
        private Mock<ICommandHandler<UpdateEventQueueInProcessCommand>> mockUpdateEventQueueInProcessCommandHandler;
        private Mock<IQueryHandler<GetLocaleEventsQuery, List<LocaleEventModel>>> mockGetLocaleEventsQueryHandler;
        private Mock<ICommandHandler<DeleteEventQueueCommand>> mockDeleteEventQueueCommandHandler;
        private Mock<ICommandHandler<UpdateFailedEventQueueCommand>> mockUpdateFailedEventQueueCommandHandler;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IEmailMessageBuilder<List<LocaleEventModel>>> mockEmailMessageBuilder;
        private Mock<ILogger> mockLogger;
        private List<int> eventTypeIds;
        private List<LocaleEventModel> queueRecords;

        [TestInitialize]
        public void Initialize()
        {
            settings = new ControllerApplicationSettings();
            mockUpdateEventQueueInProcessCommandHandler = new Mock<ICommandHandler<UpdateEventQueueInProcessCommand>>();
            mockGetLocaleEventsQueryHandler = new Mock<IQueryHandler<GetLocaleEventsQuery, List<LocaleEventModel>>>();
            mockDeleteEventQueueCommandHandler = new Mock<ICommandHandler<DeleteEventQueueCommand>>();
            mockUpdateFailedEventQueueCommandHandler = new Mock<ICommandHandler<UpdateFailedEventQueueCommand>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockEmailMessageBuilder = new Mock<IEmailMessageBuilder<List<LocaleEventModel>>>();
            mockLogger = new Mock<ILogger>();

            queueManager = new LocaleQueueManager(settings,
                mockUpdateEventQueueInProcessCommandHandler.Object,
                mockGetLocaleEventsQueryHandler.Object,
                mockDeleteEventQueueCommandHandler.Object,
                mockUpdateFailedEventQueueCommandHandler.Object,
                mockEmailClient.Object,
                mockEmailMessageBuilder.Object,
                mockLogger.Object);

            eventTypeIds = new List<int>();
        }

        [TestMethod]
        public void Get_EventsExist_ShouldReturnEvents()
        {
            //Given
            settings.Instance = 10;
            settings.MaxNumberOfRowsToMark = 100;
            queueRecords = new List<LocaleEventModel>
                {
                    new LocaleEventModel { EventReferenceId = 1 },
                    new LocaleEventModel { EventReferenceId = 2 },
                    new LocaleEventModel { EventReferenceId = 3 }
                };
            mockGetLocaleEventsQueryHandler.Setup(m => m.Search(It.Is<GetLocaleEventsQuery>(q => q.Instance == settings.Instance)))
                .Returns(queueRecords);

            //When
            var results = queueManager.Get(eventTypeIds);

            //Then
            Assert.AreEqual(queueRecords, results);
            mockUpdateEventQueueInProcessCommandHandler
                .Verify(m => m.Execute(It.Is<UpdateEventQueueInProcessCommand>(
                    c => c.EventTypeIds == eventTypeIds
                        && c.Instance == settings.Instance
                        && c.MaxNumberOfRowsToMark == settings.MaxNumberOfRowsToMark)));
            mockGetLocaleEventsQueryHandler
                .Verify(m => m.Search(It.Is<GetLocaleEventsQuery>(q => q.Instance == settings.Instance)));
        }

        [TestMethod]
        public void Get_EventListIsNull_ShouldReturnEmptyList()
        {
            //Given
            mockGetLocaleEventsQueryHandler.Setup(m => m.Search(It.IsAny<GetLocaleEventsQuery>()))
                .Returns(() => null);

            //When
            var results = queueManager.Get(eventTypeIds);

            //Then
            Assert.IsNotNull(results);
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void Finalize_QueueRecordsWithNoErrors_ShouldNotUpdateFailedEventsAndDeleteQueueRecords()
        {
            //Given
            queueRecords = new List<LocaleEventModel>
                {
                    new LocaleEventModel { EventReferenceId = 1 },
                    new LocaleEventModel { EventReferenceId = 2 },
                    new LocaleEventModel { EventReferenceId = 3 }
                };

            //When
            queueManager.Finalize(queueRecords);

            //Then
            mockUpdateFailedEventQueueCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateFailedEventQueueCommand>()), Times.Never);
            mockDeleteEventQueueCommandHandler.Verify(m => m.Execute(It.IsAny<DeleteEventQueueCommand>()), Times.Once);
        }

        [TestMethod]
        public void Finalize_QueueRecordsWithErrors_ShouldUpdateFailedEventsAndNotDeleteQueueRecords()
        {
            //Given
            queueRecords = new List<LocaleEventModel>
                {
                    new LocaleEventModel { EventReferenceId = 1, ErrorMessage = "Test" },
                    new LocaleEventModel { EventReferenceId = 2, ErrorMessage = "Test" },
                    new LocaleEventModel { EventReferenceId = 3, ErrorMessage = "Test" }
                };

            //When
            queueManager.Finalize(queueRecords);

            //Then
            mockUpdateFailedEventQueueCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateFailedEventQueueCommand>()), Times.Once);
            mockDeleteEventQueueCommandHandler.Verify(m => m.Execute(It.IsAny<DeleteEventQueueCommand>()), Times.Never);
        }
    }
}

