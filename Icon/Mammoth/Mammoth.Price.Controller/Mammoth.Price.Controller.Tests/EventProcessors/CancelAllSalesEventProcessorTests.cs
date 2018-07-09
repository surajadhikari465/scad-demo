using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using Mammoth.Price.Controller.ApplicationModules;
using Mammoth.Price.Controller.Common;
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
    public class CancelAllSalesEventProcessorTests
    {
        private CancelAllSalesEventProcessor processor;
        private PriceControllerApplicationSettings settings;
        private Mock<IQueryHandler<GetCancelAllSalesDataParameters, List<CancelAllSalesEventModel>>> mockGetCancelAllSalesDataQuery;
        private Mock<IService<CancelAllSalesEventModel>> mockService;
        private Mock<ICommandHandler<ArchiveEventsCommand>> mockArchiveEventsCommandHandler;
        private Mock<ICommandHandler<ReprocessFailedCancelAllSalesEventsCommand>> mockReprocessFailedCancelAllSalesEventsCommandHandler;
        private Mock<IErrorAlerter> mockErrorAlerter;
        private Mock<ILogger> mockLogger;
        private List<EventQueueModel> events;

        [TestInitialize]
        public void Initialize()
        {
            settings = new PriceControllerApplicationSettings();
            mockGetCancelAllSalesDataQuery = new Mock<IQueryHandler<GetCancelAllSalesDataParameters, List<CancelAllSalesEventModel>>>();
            mockService = new Mock<IService<CancelAllSalesEventModel>>();
            mockArchiveEventsCommandHandler = new Mock<ICommandHandler<ArchiveEventsCommand>>();
            mockReprocessFailedCancelAllSalesEventsCommandHandler = new Mock<ICommandHandler<ReprocessFailedCancelAllSalesEventsCommand>>();
            mockErrorAlerter = new Mock<IErrorAlerter>();
            mockLogger = new Mock<ILogger>();

            processor = new CancelAllSalesEventProcessor(
                settings,
                mockGetCancelAllSalesDataQuery.Object,                
                mockService.Object,
                mockArchiveEventsCommandHandler.Object,
                mockReprocessFailedCancelAllSalesEventsCommandHandler.Object,
                mockErrorAlerter.Object,
                mockLogger.Object);
        }

        [TestMethod]
        public void CancelAllSalesEventProcessor_CancelAllSalesEventsExist_ShouldProcessEvents()
        {
            //Given
            events = new List<EventQueueModel>
            {
                new EventQueueModel { EventTypeId = IrmaEventTypes.CancelAllSales },
                new EventQueueModel { EventTypeId = IrmaEventTypes.CancelAllSales },
                new EventQueueModel { EventTypeId = IrmaEventTypes.CancelAllSales }
            };
            mockGetCancelAllSalesDataQuery.Setup(m => m.Search(It.IsAny<GetCancelAllSalesDataParameters>()))
                .Returns(new List<CancelAllSalesEventModel>());

            //When
            processor.ProcessEvents(events);

            //Then
            mockGetCancelAllSalesDataQuery.Verify(m => m.Search(It.IsAny<GetCancelAllSalesDataParameters>()), Times.Once);
            mockService.Verify(m => m.Process(It.IsAny<List<CancelAllSalesEventModel>>()), Times.Once);
            mockArchiveEventsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveEventsCommand>()), Times.Once);
            mockErrorAlerter.Verify(m => m.AlertErrors(It.IsAny<List<PriceEventModel>>()), Times.Never);
        }

        [TestMethod]
        public void CancelAllSalesEventProcessor_NoCancelAllSalesEventsExist_ShouldNotProcessEvents()
        {
            //Given
            events = new List<EventQueueModel>
            {
                new EventQueueModel { EventTypeId = IrmaEventTypes.Price },
                new EventQueueModel { EventTypeId = IrmaEventTypes.Price },
                new EventQueueModel { EventTypeId = IrmaEventTypes.Price }
            };

            //When
            processor.ProcessEvents(events);

            //Then
            mockGetCancelAllSalesDataQuery.Verify(m => m.Search(It.IsAny<GetCancelAllSalesDataParameters>()), Times.Never);
            mockService.Verify(m => m.Process(It.IsAny<List<CancelAllSalesEventModel>>()), Times.Never);
            mockArchiveEventsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveEventsCommand>()), Times.Never);
            mockErrorAlerter.Verify(m => m.AlertErrors(It.IsAny<List<PriceEventModel>>()), Times.Never);
        }
    }
}
