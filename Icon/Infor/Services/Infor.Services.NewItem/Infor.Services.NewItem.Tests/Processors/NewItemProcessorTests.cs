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
using Infor.Services.NewItem.Validators;
using Infor.Services.NewItem.Notifiers;

namespace Infor.Services.NewItem.Tests.Processors
{
    [TestClass]
    public class NewItemProcessorTests
    {
        private NewItemProcessor processor;
        private InforNewItemApplicationSettings settings;
        private Mock<IQueryHandler<GetNewItemsQuery, IEnumerable<NewItemModel>>> mockGetNewItemsQueryHandler;
        private Mock<ICollectionValidator<NewItemModel>> mockNewItemCollectionValidator;
        private Mock<ICommandHandler<AddNewItemsToIconCommand>> mockAddNewItemsToIconCommandHandler;
        private Mock<IInforItemService> mockInforItemService;
        private Mock<ICommandHandler<FinalizeNewItemEventsCommand>> mockFinalizeNewItemEventsCommandHandler;
        private Mock<ICommandHandler<ArchiveNewItemsCommand>> mockArchiveNewItemsCommandHandler;
        private Mock<INewItemNotifier> mockNotifier;
        private Mock<ILogger<NewItemProcessor>> mockLogger;
        private AddNewItemsToInforResponse response;
        private Mock<IRenewableContext<IconContext>> mockIconContext;
        private Mock<IRenewableContext<IrmaContext>> mockIrmaContext;

        [TestInitialize]
        public void Initialize()
        {
            settings = new InforNewItemApplicationSettings();
            mockGetNewItemsQueryHandler = new Mock<IQueryHandler<GetNewItemsQuery, IEnumerable<NewItemModel>>>();
            mockNewItemCollectionValidator = new Mock<ICollectionValidator<NewItemModel>>();
            mockAddNewItemsToIconCommandHandler = new Mock<ICommandHandler<AddNewItemsToIconCommand>>();
            mockInforItemService = new Mock<IInforItemService>();
            mockFinalizeNewItemEventsCommandHandler = new Mock<ICommandHandler<FinalizeNewItemEventsCommand>>();
            mockArchiveNewItemsCommandHandler = new Mock<ICommandHandler<ArchiveNewItemsCommand>>();
            mockNotifier = new Mock<INewItemNotifier>();
            mockLogger = new Mock<ILogger<NewItemProcessor>>();
            response = new AddNewItemsToInforResponse();
            mockIconContext = new Mock<IRenewableContext<IconContext>>();
            mockIrmaContext = new Mock<IRenewableContext<IrmaContext>>();

            processor = new NewItemProcessor(settings,
                mockGetNewItemsQueryHandler.Object,
                mockNewItemCollectionValidator.Object,
                mockAddNewItemsToIconCommandHandler.Object,
                mockInforItemService.Object,
                mockFinalizeNewItemEventsCommandHandler.Object,
                mockArchiveNewItemsCommandHandler.Object,
                mockNotifier.Object,
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
            mockNewItemCollectionValidator.SetupSequence(m => m.ValidateCollection(It.IsAny<IEnumerable<NewItemModel>>()))
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() });

            //When
            processor.ProcessNewItemEvents(1);

            //Then
            mockGetNewItemsQueryHandler.Verify(m => m.Search(It.IsAny<GetNewItemsQuery>()), Times.Exactly(2));
            mockAddNewItemsToIconCommandHandler.Verify(m => m.Execute(It.IsAny<AddNewItemsToIconCommand>()), Times.Exactly(1));
            mockInforItemService.Verify(m => m.AddNewItemsToInfor(It.IsAny<AddNewItemsToInforRequest>()), Times.Exactly(1));
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Exactly(2));
            mockNotifier.Verify(m => m.NotifyOfNewItemError(It.IsAny<IEnumerable<NewItemModel>>()), Times.Exactly(2));
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
            mockNewItemCollectionValidator.SetupSequence(m => m.ValidateCollection(It.IsAny<IEnumerable<NewItemModel>>()))
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() });


            //When
            processor.ProcessNewItemEvents(1);

            //Then
            mockGetNewItemsQueryHandler.Verify(m => m.Search(It.IsAny<GetNewItemsQuery>()), Times.Exactly(22));
            mockAddNewItemsToIconCommandHandler.Verify(m => m.Execute(It.IsAny<AddNewItemsToIconCommand>()), Times.Exactly(11));
            mockInforItemService.Verify(m => m.AddNewItemsToInfor(It.IsAny<AddNewItemsToInforRequest>()), Times.Exactly(11));
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Exactly(22));
            mockNotifier.Verify(m => m.NotifyOfNewItemError(It.IsAny<IEnumerable<NewItemModel>>()), Times.Exactly(22));
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

            mockNewItemCollectionValidator.SetupSequence(m => m.ValidateCollection(It.IsAny<IEnumerable<NewItemModel>>()))
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel> { new NewItemModel() } })
                .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() });


            //When
            processor.ProcessNewItemEvents(1);

            //Then
            mockGetNewItemsQueryHandler.Verify(m => m.Search(It.IsAny<GetNewItemsQuery>()), Times.Exactly(6));
            mockAddNewItemsToIconCommandHandler.Verify(m => m.Execute(It.IsAny<AddNewItemsToIconCommand>()), Times.Exactly(5));
            mockInforItemService.Verify(m => m.AddNewItemsToInfor(It.IsAny<AddNewItemsToInforRequest>()), Times.Exactly(5));
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Exactly(6));
            mockNotifier.Verify(m => m.NotifyOfNewItemError(It.IsAny<IEnumerable<NewItemModel>>()), Times.Exactly(6));
            mockArchiveNewItemsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveNewItemsCommand>()), Times.Exactly(6));
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
            mockNotifier.Verify(m => m.NotifyOfNewItemError(It.IsAny<IEnumerable<NewItemModel>>()), Times.Never);
            mockArchiveNewItemsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveNewItemsCommand>()), Times.Never);
        }

        [TestMethod]
        public void ProcessNewItemEvents_1RegionAndNoEvents_ShouldNotAttemptToProcessEventsASecondTime()
        {
            //Given
            settings.Regions.Add("FL");
            mockGetNewItemsQueryHandler.SetupSequence(m => m.Search(It.IsAny<GetNewItemsQuery>()))
                .Returns(new List<NewItemModel>());
            mockNewItemCollectionValidator.SetupSequence(m => m.ValidateCollection(It.IsAny<IEnumerable<NewItemModel>>()))
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() });

            //When
            processor.ProcessNewItemEvents(1);

            //Then
            mockGetNewItemsQueryHandler.Verify(m => m.Search(It.IsAny<GetNewItemsQuery>()), Times.Once);
            mockAddNewItemsToIconCommandHandler.Verify(m => m.Execute(It.IsAny<AddNewItemsToIconCommand>()), Times.Never);
            mockInforItemService.Verify(m => m.AddNewItemsToInfor(It.IsAny<AddNewItemsToInforRequest>()), Times.Never);
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Once);
            mockNotifier.Verify(m => m.NotifyOfNewItemError(It.IsAny<IEnumerable<NewItemModel>>()), Times.Once);
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

            mockNewItemCollectionValidator.SetupSequence(m => m.ValidateCollection(It.IsAny<IEnumerable<NewItemModel>>()))
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() { new NewItemModel() } })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() { new NewItemModel() } })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() { new NewItemModel() } })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() { new NewItemModel() } })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() { new NewItemModel() } })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() { new NewItemModel() } })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() { new NewItemModel() } })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() { new NewItemModel() } })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() })
               .Returns(new CollectionValidatorResult<NewItemModel> { ValidEntities = new List<NewItemModel>() });

            //When
            processor.ProcessNewItemEvents(1);

            //Then
            mockGetNewItemsQueryHandler.Verify(m => m.Search(It.IsAny<GetNewItemsQuery>()), Times.Exactly(19));
            mockAddNewItemsToIconCommandHandler.Verify(m => m.Execute(It.IsAny<AddNewItemsToIconCommand>()), Times.Exactly(8));
            mockInforItemService.Verify(m => m.AddNewItemsToInfor(It.IsAny<AddNewItemsToInforRequest>()), Times.Exactly(8));
            mockFinalizeNewItemEventsCommandHandler.Verify(m => m.Execute(It.IsAny<FinalizeNewItemEventsCommand>()), Times.Exactly(19));
            mockNotifier.Verify(m => m.NotifyOfNewItemError(It.IsAny<IEnumerable<NewItemModel>>()), Times.Exactly(19));
            mockArchiveNewItemsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveNewItemsCommand>()), Times.Exactly(19));
        }
    }
}
