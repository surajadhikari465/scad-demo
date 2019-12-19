using Services.NewItem.Processor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Common.DataAccess;
using Services.NewItem.Models;
using Services.NewItem.Queries;
using Services.NewItem.Commands;
using Moq;
using Services.NewItem.Services;
using Icon.Logging;
using Icon.Common.Context;
using Icon.Framework;
using Irma.Framework;

namespace Services.NewItem.Tests.Processors
{
    [TestClass]
    public class NewItemProcessorTests
    {
        private NewItemProcessor processor;
        private NewItemApplicationSettings settings;
        private Mock<IQueryHandler<GetNewItemsQuery, IEnumerable<NewItemModel>>> mockGetNewItemsQueryHandler;
        private Mock<ICommandHandler<UpdateItemSubscriptionInIconCommand>> mockUpdateItemSubscriptionInIconCommandHandler;
        private Mock<IIconItemService> mockIconItemService;
        private Mock<ICommandHandler<FinalizeNewItemEventsCommand>> mockFinalizeNewItemEventsCommandHandler;
        private Mock<ICommandHandler<ArchiveNewItemsCommand>> mockArchiveNewItemsCommandHandler;
        private Mock<ILogger<NewItemProcessor>> mockLogger;
        private Mock<IRenewableContext<IconContext>> mockIconContext;
        private Mock<IRenewableContext<IrmaContext>> mockIrmaContext;

        [TestInitialize]
        public void Initialize()
        {
            settings = new NewItemApplicationSettings();
            mockGetNewItemsQueryHandler = new Mock<IQueryHandler<GetNewItemsQuery, IEnumerable<NewItemModel>>>();
            mockUpdateItemSubscriptionInIconCommandHandler = new Mock<ICommandHandler<UpdateItemSubscriptionInIconCommand>>();
            mockIconItemService = new Mock<IIconItemService>();
            mockFinalizeNewItemEventsCommandHandler = new Mock<ICommandHandler<FinalizeNewItemEventsCommand>>();
            mockArchiveNewItemsCommandHandler = new Mock<ICommandHandler<ArchiveNewItemsCommand>>();
            mockLogger = new Mock<ILogger<NewItemProcessor>>();
            mockIconContext = new Mock<IRenewableContext<IconContext>>();
            mockIrmaContext = new Mock<IRenewableContext<IrmaContext>>();

            processor = new NewItemProcessor(settings,
                mockGetNewItemsQueryHandler.Object,
                mockUpdateItemSubscriptionInIconCommandHandler.Object,
                mockIconItemService.Object,
                mockFinalizeNewItemEventsCommandHandler.Object,
                mockArchiveNewItemsCommandHandler.Object,
                mockIconContext.Object,
                mockIrmaContext.Object,
                mockLogger.Object);
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
            mockUpdateItemSubscriptionInIconCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateItemSubscriptionInIconCommand>()), Times.Exactly(2));
            mockIconItemService.Verify(m => m.AddItemEventsToIconEventQueue(It.IsAny<IEnumerable<NewItemModel>>()), Times.Exactly(2));
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Exactly(2));    
            mockArchiveNewItemsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveNewItemsCommand>()), Times.Exactly(2));
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
            mockUpdateItemSubscriptionInIconCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateItemSubscriptionInIconCommand>()), Times.Exactly(22));
            mockIconItemService.Verify(m => m.AddItemEventsToIconEventQueue(It.IsAny<IEnumerable<NewItemModel>>()), Times.Exactly(22));
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Exactly(22));
              mockArchiveNewItemsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveNewItemsCommand>()), Times.Exactly(22));
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
            mockUpdateItemSubscriptionInIconCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateItemSubscriptionInIconCommand>()), Times.Exactly(6));
             mockIconItemService.Verify(m => m.AddItemEventsToIconEventQueue(It.IsAny<IEnumerable<NewItemModel>>()), Times.Exactly(6));
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Exactly(6));
            mockArchiveNewItemsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveNewItemsCommand>()), Times.Exactly(6));
        }

        [TestMethod]
        public void ProcessNewItemEvents_0Regions_ShouldNotProcessEvents()
        {
            //When
            processor.ProcessNewItemEvents(1);

            //Then
            mockGetNewItemsQueryHandler.Verify(m => m.Search(It.IsAny<GetNewItemsQuery>()), Times.Never);
            mockUpdateItemSubscriptionInIconCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateItemSubscriptionInIconCommand>()), Times.Never);
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Never);
            mockArchiveNewItemsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveNewItemsCommand>()), Times.Never);
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
            mockUpdateItemSubscriptionInIconCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateItemSubscriptionInIconCommand>()), Times.Once);
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Once);
            mockArchiveNewItemsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveNewItemsCommand>()), Times.Once);
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
            mockUpdateItemSubscriptionInIconCommandHandler.Verify(m => m.Execute(It.IsAny<UpdateItemSubscriptionInIconCommand>()), Times.Exactly(19));
            mockIconItemService.Verify(m => m.AddItemEventsToIconEventQueue(It.IsAny<IEnumerable<NewItemModel>>()), Times.Exactly(19));
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Exactly(19));
            mockArchiveNewItemsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveNewItemsCommand>()), Times.Exactly(19));
        }
    }
}
