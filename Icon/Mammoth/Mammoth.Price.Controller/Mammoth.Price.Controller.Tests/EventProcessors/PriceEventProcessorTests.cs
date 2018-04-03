using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using Mammoth.Price.Controller.ApplicationModules;
using Mammoth.Price.Controller.DataAccess.Commands;
using Mammoth.Price.Controller.DataAccess.Models;
using Mammoth.Price.Controller.DataAccess.Queries;
using Mammoth.Price.Controller.EventProcessors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Mammoth.Price.Controller.Tests.EventProcessors
{
    [TestClass]
    public class PriceEventProcessorTests
    {
        private PriceEventProcessor processor;
        private PriceControllerApplicationSettings settings;
        private Mock<IQueryHandler<GetPriceDataParameters, List<PriceEventModel>>> mockGetPricesQueryHandler;
        private Mock<IQueryHandler<GetExistingPriceDataParameters, List<PriceEventModel>>> mockGetExistingPricesQueryHandler;
        private Mock<IService<PriceEventModel>> mockService;
        private Mock<ICommandHandler<ArchiveEventsCommand>> mockArchiveEventsCommandHandler;
        private Mock<IErrorAlerter> mockErrorAlerter;
        private Mock<ILogger> mockLogger;
        private List<EventQueueModel> events;

        [TestInitialize]
        public void Initialize()
        {
            settings = new PriceControllerApplicationSettings();
            mockGetPricesQueryHandler = new Mock<IQueryHandler<GetPriceDataParameters, List<PriceEventModel>>>();
            mockGetExistingPricesQueryHandler = new Mock<IQueryHandler<GetExistingPriceDataParameters, List<PriceEventModel>>>();
            mockService = new Mock<IService<PriceEventModel>>();
            mockArchiveEventsCommandHandler = new Mock<ICommandHandler<ArchiveEventsCommand>>();
            mockErrorAlerter = new Mock<IErrorAlerter>();
            mockLogger = new Mock<ILogger>();

            processor = new PriceEventProcessor(
                settings,
                mockGetPricesQueryHandler.Object,
                mockGetExistingPricesQueryHandler.Object,
                mockService.Object,
                mockArchiveEventsCommandHandler.Object,
                mockErrorAlerter.Object,
                mockLogger.Object);
        }

        [TestMethod]
        public void PriceEventProcessorProcess_PriceEventsExist_ShouldProcessEvents()
        {
            //Given
            events = new List<EventQueueModel>
            {
                new EventQueueModel { EventTypeId = IrmaEventTypes.Price },
                new EventQueueModel { EventTypeId = IrmaEventTypes.Price },
                new EventQueueModel { EventTypeId = IrmaEventTypes.Price }
            };
            mockGetPricesQueryHandler.Setup(m => m.Search(It.IsAny<GetPriceDataParameters>()))
                .Returns(new List<PriceEventModel>());
            mockGetExistingPricesQueryHandler.Setup(m => m.Search(It.IsAny<GetExistingPriceDataParameters>()))
                .Returns(new List<PriceEventModel>());

            //When
            processor.ProcessEvents(events);

            //Then
            mockGetPricesQueryHandler.Verify(m => m.Search(It.IsAny<GetPriceDataParameters>()), Times.Once);
            mockGetExistingPricesQueryHandler.Verify(m => m.Search(It.IsAny<GetExistingPriceDataParameters>()), Times.Once);
            mockService.Verify(m => m.Process(It.IsAny<List<PriceEventModel>>()), Times.Once);
            mockArchiveEventsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveEventsCommand>()), Times.Once);
            mockErrorAlerter.Verify(m => m.AlertErrors(It.IsAny<List<PriceEventModel>>()), Times.Never);
        }

        [TestMethod]
        public void PriceEventProcessorProcess_PriceRollbackEventsExist_ShouldProcessEvents()
        {
            //Given
            events = new List<EventQueueModel>
            {
                new EventQueueModel { EventTypeId = IrmaEventTypes.PriceRollback },
                new EventQueueModel { EventTypeId = IrmaEventTypes.PriceRollback },
                new EventQueueModel { EventTypeId = IrmaEventTypes.PriceRollback }
            };
            mockGetPricesQueryHandler.Setup(m => m.Search(It.IsAny<GetPriceDataParameters>()))
                .Returns(new List<PriceEventModel>());
            mockGetExistingPricesQueryHandler.Setup(m => m.Search(It.IsAny<GetExistingPriceDataParameters>()))
                .Returns(new List<PriceEventModel>());

            //When
            processor.ProcessEvents(events);

            //Then
            mockGetPricesQueryHandler.Verify(m => m.Search(It.IsAny<GetPriceDataParameters>()), Times.Once);
            mockGetExistingPricesQueryHandler.Verify(m => m.Search(It.IsAny<GetExistingPriceDataParameters>()), Times.Once);
            mockService.Verify(m => m.Process(It.IsAny<List<PriceEventModel>>()), Times.Once);
            mockArchiveEventsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveEventsCommand>()), Times.Once);
            mockErrorAlerter.Verify(m => m.AlertErrors(It.IsAny<List<PriceEventModel>>()), Times.Never);
        }

        [TestMethod]
        public void PriceEventProcessorProcess_PriceAndPriceRollbackEventsExist_ShouldProcessEvents()
        {
            //Given
            events = new List<EventQueueModel>
            {
                new EventQueueModel { EventTypeId = IrmaEventTypes.Price },
                new EventQueueModel { EventTypeId = IrmaEventTypes.Price },
                new EventQueueModel { EventTypeId = IrmaEventTypes.PriceRollback }
            };
            mockGetPricesQueryHandler.Setup(m => m.Search(It.IsAny<GetPriceDataParameters>()))
                .Returns(new List<PriceEventModel>());
            mockGetExistingPricesQueryHandler.Setup(m => m.Search(It.IsAny<GetExistingPriceDataParameters>()))
                .Returns(new List<PriceEventModel>());

            //When
            processor.ProcessEvents(events);

            //Then
            mockGetPricesQueryHandler.Verify(m => m.Search(It.IsAny<GetPriceDataParameters>()), Times.Once);
            mockGetExistingPricesQueryHandler.Verify(m => m.Search(It.IsAny<GetExistingPriceDataParameters>()), Times.Once);
            mockService.Verify(m => m.Process(It.IsAny<List<PriceEventModel>>()), Times.Once);
            mockArchiveEventsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveEventsCommand>()), Times.Once);
            mockErrorAlerter.Verify(m => m.AlertErrors(It.IsAny<List<PriceEventModel>>()), Times.Never);
        }

        [TestMethod]
        public void PriceEventProcessorProcess_NoPriceOrPriceRollbackEventsExist_ShouldNotProcessEvents()
        {
            //Given
            events = new List<EventQueueModel>
            {
                new EventQueueModel { EventTypeId = IrmaEventTypes.CancelAllSales },
                new EventQueueModel { EventTypeId = IrmaEventTypes.CancelAllSales },
                new EventQueueModel { EventTypeId = IrmaEventTypes.CancelAllSales }
            };

            //When
            processor.ProcessEvents(events);

            //Then
            mockGetPricesQueryHandler.Verify(m => m.Search(It.IsAny<GetPriceDataParameters>()), Times.Never);
            mockGetExistingPricesQueryHandler.Verify(m => m.Search(It.IsAny<GetExistingPriceDataParameters>()), Times.Never);
            mockService.Verify(m => m.Process(It.IsAny<List<PriceEventModel>>()), Times.Never);
            mockArchiveEventsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveEventsCommand>()), Times.Never);
            mockErrorAlerter.Verify(m => m.AlertErrors(It.IsAny<List<PriceEventModel>>()), Times.Never);
        }
    }
}
