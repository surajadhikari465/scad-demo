using Infor.Services.NewItem.Processor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Common.DataAccess;
using Infor.Services.NewItem.Models;
using Infor.Services.NewItem.Queries;
using Infor.Services.NewItem.Commands;
using Moq;
using Infor.Services.NewItem.Services;
using Icon.Logging;
using Icon.Common.Context;
using Icon.Framework;
using Irma.Framework;

namespace Infor.Services.NewItem.Tests.Processors
{
    [TestClass]
    public class NewItemProcessorTests
    {
        private NewItemProcessor processor;
        private InforNewItemApplicationSettings settings;
        private Mock<IQueryHandler<GetNewItemsQuery, IEnumerable<NewItemModel>>> mockGetNewItemsQueryHandler;
        private Mock<ICommandHandler<AddNewItemsToIconCommand>> mockAddNewItemsToIconCommandHandler;
        private Mock<IInforItemService> mockInforItemService;
        private Mock<ICommandHandler<FinalizeNewItemEventsCommand>> mockFinalizeNewItemEventsCommandHandler;
        private Mock<ILogger<NewItemProcessor>> mockLogger;
        private AddNewItemsToInforResponse response;
        private Mock<IRenewableContext<IconContext>> mockIconContext;
        private Mock<IRenewableContext<IrmaContext>> mockIrmaContext;

        [TestInitialize]
        public void Initialize()
        {
            settings = new InforNewItemApplicationSettings();
            mockGetNewItemsQueryHandler = new Mock<IQueryHandler<GetNewItemsQuery, IEnumerable<NewItemModel>>>();
            mockAddNewItemsToIconCommandHandler = new Mock<ICommandHandler<AddNewItemsToIconCommand>>();
            mockInforItemService = new Mock<IInforItemService>();
            mockFinalizeNewItemEventsCommandHandler = new Mock<ICommandHandler<FinalizeNewItemEventsCommand>>();
            mockLogger = new Mock<ILogger<NewItemProcessor>>();
            response = new AddNewItemsToInforResponse();
            mockIconContext = new Mock<IRenewableContext<IconContext>>();
            mockIrmaContext = new Mock<IRenewableContext<IrmaContext>>();

            processor = new NewItemProcessor(settings,
                mockGetNewItemsQueryHandler.Object,
                mockAddNewItemsToIconCommandHandler.Object,
                mockInforItemService.Object,
                mockFinalizeNewItemEventsCommandHandler.Object,
                mockIconContext.Object,
                mockIrmaContext.Object,
                mockLogger.Object);

            mockInforItemService.Setup(m => m.AddNewItemsToInfor(It.IsAny<AddNewItemsToInforRequest>()))
                .Returns(response);
        }

        [TestMethod]
        public void ProcessNewItemEvents_1RegionAnd1BatchOfNewItemEvents_ShouldProcessEvents()
        {
            //Given
            settings.Regions.Add("FL");
            mockGetNewItemsQueryHandler.SetupSequence(m => m.Search(It.IsAny<GetNewItemsQuery>()))
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>());

            //When
            processor.ProcessNewItemEvents(1);

            //Then
            mockGetNewItemsQueryHandler.Verify(m => m.Search(It.IsAny<GetNewItemsQuery>()), Times.Exactly(2));
            mockAddNewItemsToIconCommandHandler.Verify(m => m.Execute(It.IsAny<AddNewItemsToIconCommand>()), Times.Exactly(2));
            mockInforItemService.Verify(m => m.AddNewItemsToInfor(It.IsAny<AddNewItemsToInforRequest>()), Times.Exactly(2));
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Exactly(2));
        }

        [TestMethod]
        public void ProcessNewItemEvents_11RegionsAnd1BatchOfNewItemEvents_ShouldProcessEventsForEachRegion()
        {
            //Given
            settings.Regions.AddRange(new List<string> { "FL", "MA", "MW", "NA", "NC", "NE", "PN", "RM", "SO", "SP", "SW" });
            mockGetNewItemsQueryHandler.SetupSequence(m => m.Search(It.IsAny<GetNewItemsQuery>()))
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>());

            //When
            processor.ProcessNewItemEvents(1);

            //Then
            mockGetNewItemsQueryHandler.Verify(m => m.Search(It.IsAny<GetNewItemsQuery>()), Times.Exactly(22));
            mockAddNewItemsToIconCommandHandler.Verify(m => m.Execute(It.IsAny<AddNewItemsToIconCommand>()), Times.Exactly(22));
            mockInforItemService.Verify(m => m.AddNewItemsToInfor(It.IsAny<AddNewItemsToInforRequest>()), Times.Exactly(22));
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Exactly(22));
        }

        [TestMethod]
        public void ProcessNewItemEvents_1RegionAnd5BatchesOfNewItemEvents_ShouldProcessAllEventsForRegion()
        {
            //Given
            settings.Regions.Add("FL");
            mockGetNewItemsQueryHandler.SetupSequence(m => m.Search(It.IsAny<GetNewItemsQuery>()))
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>());

            //When
            processor.ProcessNewItemEvents(1);

            //Then
            mockGetNewItemsQueryHandler.Verify(m => m.Search(It.IsAny<GetNewItemsQuery>()), Times.Exactly(6));
            mockAddNewItemsToIconCommandHandler.Verify(m => m.Execute(It.IsAny<AddNewItemsToIconCommand>()), Times.Exactly(6));
            mockInforItemService.Verify(m => m.AddNewItemsToInfor(It.IsAny<AddNewItemsToInforRequest>()), Times.Exactly(6));
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Exactly(6));
        }

        [TestMethod]
        public void ProcessNewItemEvents_0Regions_ShouldNotProcessEvents()
        {
            //When
            processor.ProcessNewItemEvents(1);

            //Then
            mockGetNewItemsQueryHandler.Verify(m => m.Search(It.IsAny<GetNewItemsQuery>()), Times.Never);
            mockAddNewItemsToIconCommandHandler.Verify(m => m.Execute(It.IsAny<AddNewItemsToIconCommand>()), Times.Never);
            mockInforItemService.Verify(m => m.AddNewItemsToInfor(It.IsAny<AddNewItemsToInforRequest>()), Times.Never);
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Never);
        }

        [TestMethod]
        public void ProcessNewItemEvents_1RegionAndNoEvents_ShouldNotAttemptToProcessEventsASecondTime()
        {
            //Given
            settings.Regions.Add("FL");
            mockGetNewItemsQueryHandler.SetupSequence(m => m.Search(It.IsAny<GetNewItemsQuery>()))
                .Returns(new List<NewItemModel>());

            //When
            processor.ProcessNewItemEvents(1);

            //Then
            mockGetNewItemsQueryHandler.Verify(m => m.Search(It.IsAny<GetNewItemsQuery>()), Times.Once);
            mockAddNewItemsToIconCommandHandler.Verify(m => m.Execute(It.IsAny<AddNewItemsToIconCommand>()), Times.Once);
            mockInforItemService.Verify(m => m.AddNewItemsToInfor(It.IsAny<AddNewItemsToInforRequest>()), Times.Once);
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Once);
        }

        [TestMethod]
        public void ProcessNewItemEvents_SomeRegionDoNotHaveEvents_ShouldProcessEventsForRegionsThatHaveEvents()
        {
            //Given
            settings.Regions.AddRange(new List<string> { "FL", "MA", "MW", "NA", "NC", "NE", "PN", "RM", "SO", "SP", "SW" });
            mockGetNewItemsQueryHandler.SetupSequence(m => m.Search(It.IsAny<GetNewItemsQuery>()))
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel> { new NewItemModel() })
                .Returns(new List<NewItemModel>())
                .Returns(new List<NewItemModel>());

            //When
            processor.ProcessNewItemEvents(1);

            //Then
            mockGetNewItemsQueryHandler.Verify(m => m.Search(It.IsAny<GetNewItemsQuery>()), Times.Exactly(19));
            mockAddNewItemsToIconCommandHandler.Verify(m => m.Execute(It.IsAny<AddNewItemsToIconCommand>()), Times.Exactly(19));
            mockInforItemService.Verify(m => m.AddNewItemsToInfor(It.IsAny<AddNewItemsToInforRequest>()), Times.Exactly(19));
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Exactly(19));
        }
    }
}
