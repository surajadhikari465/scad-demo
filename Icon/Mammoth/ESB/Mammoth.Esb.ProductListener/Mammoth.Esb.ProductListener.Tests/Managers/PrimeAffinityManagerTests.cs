using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.ProductListener.Managers;
using Mammoth.Esb.ProductListener.Models;
using Mammoth.Esb.ProductListener.Queries;
using Mammoth.PrimeAffinity.Library.Processors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Esb.ProductListener.Tests.Managers
{
    [TestClass]
    public class PrimeAffinityManagerTests
    {
        private PrimeAffinityManager primeAffinityManager;
        private ProductListenerSettings settings;
        private Mock<IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters>> mockPrimeAffinityPsgProcessor;
        private Mock<IQueryHandler<GetItemsParameters, IEnumerable<ItemDataAccessModel>>> mockGetItemSubTeamsQueryHandler;
        private Mock<IQueryHandler<GetPrimeAffinityItemStoreModelsParameters, IEnumerable<PrimeAffinityItemStoreModel>>> mockGetPrimeAffinityItemStoreModelsQueryHandler;
        private Mock<IQueryHandler<GetPrimeAffinityItemStoreModelsForActiveSalesParameters, List<PrimeAffinityItemStoreModel>>> mockGetPrimeAffinityItemStoreModelsForActiveSalesQueryHandler;

        [TestInitialize]
        public void Initialize()
        {
            settings = new ProductListenerSettings();
            mockPrimeAffinityPsgProcessor = new Mock<IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters>>();
            mockGetItemSubTeamsQueryHandler = new Mock<IQueryHandler<GetItemsParameters, IEnumerable<ItemDataAccessModel>>>();
            mockGetPrimeAffinityItemStoreModelsQueryHandler = new Mock<IQueryHandler<GetPrimeAffinityItemStoreModelsParameters, IEnumerable<PrimeAffinityItemStoreModel>>>();
            mockGetPrimeAffinityItemStoreModelsForActiveSalesQueryHandler = new Mock<IQueryHandler<GetPrimeAffinityItemStoreModelsForActiveSalesParameters, List<PrimeAffinityItemStoreModel>>>();

            primeAffinityManager = new PrimeAffinityManager(
                settings,
                mockPrimeAffinityPsgProcessor.Object,
                mockGetItemSubTeamsQueryHandler.Object,
                mockGetPrimeAffinityItemStoreModelsQueryHandler.Object,
                mockGetPrimeAffinityItemStoreModelsForActiveSalesQueryHandler.Object);
            settings.EligiblePriceTypes = new List<string>
            {
                "SAL",
                "ISS",
                "FRZ"
            };
            settings.ExcludedPSNumbers = new List<int>
            {
                2100,
                2200,
                2220
            };
        }

        [TestMethod]
        public void SendPrimeAffinityMessages_NoItemsAreChangingSubTeams_DoesNotSendPsgs()
        {
            //Given
            List<ItemModel> items = new List<ItemModel>
            {
                CreateItemModel(1, settings.ExcludedPSNumbers[0]),
                CreateItemModel(2, 2222),
                CreateItemModel(3, 3333),
            };
            mockGetItemSubTeamsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsParameters>()))
                .Returns(new List<ItemDataAccessModel>
                {
                    new ItemDataAccessModel{ ItemID = 1, PSNumber = settings.ExcludedPSNumbers[0] },
                    new ItemDataAccessModel{ ItemID = 2, PSNumber = 2222 },
                    new ItemDataAccessModel{ ItemID = 3, PSNumber = 3333 },
                });

            //When
            primeAffinityManager.SendPrimeAffinityMessages(items);

            //Then
            mockGetItemSubTeamsQueryHandler.Verify(m => m.Search(It.IsAny<GetItemsParameters>()), Times.Once);
            mockGetPrimeAffinityItemStoreModelsQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsParameters>()), Times.Never);
            mockGetPrimeAffinityItemStoreModelsForActiveSalesQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsForActiveSalesParameters>()), Times.Never);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.IsAny<PrimeAffinityPsgProcessorParameters>()), Times.Never);
        }

        [TestMethod]
        public void SendPrimeAffinityMessages_ItemsAreChangingFromNonExcludedToNonExcludedSubteams_DoesNotSendPsgs()
        {
            //Given
            List<ItemModel> items = new List<ItemModel>
            {
                CreateItemModel(1, settings.ExcludedPSNumbers[0]),
                CreateItemModel(2, 2222),
                CreateItemModel(3, 3333),
            };
            mockGetItemSubTeamsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsParameters>()))
                .Returns(new List<ItemDataAccessModel>
                {
                    new ItemDataAccessModel{ ItemID = 1, PSNumber = settings.ExcludedPSNumbers[0] },
                    new ItemDataAccessModel{ ItemID = 2, PSNumber = 3333 },
                    new ItemDataAccessModel{ ItemID = 3, PSNumber = 2222 },
                });

            //When
            primeAffinityManager.SendPrimeAffinityMessages(items);

            //Then
            mockGetItemSubTeamsQueryHandler.Verify(m => m.Search(It.IsAny<GetItemsParameters>()), Times.Once);
            mockGetPrimeAffinityItemStoreModelsQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsParameters>()), Times.Never);
            mockGetPrimeAffinityItemStoreModelsForActiveSalesQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsForActiveSalesParameters>()), Times.Never);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.IsAny<PrimeAffinityPsgProcessorParameters>()), Times.Never);
        }

        [TestMethod]
        public void SendPrimeAffinityMessages_ItemsAreChangingFromExcludedToExcludedSubteams_DoesNotSendPsgs()
        {
            //Given
            List<ItemModel> items = new List<ItemModel>
            {
                CreateItemModel(1, settings.ExcludedPSNumbers[0]),
                CreateItemModel(2, settings.ExcludedPSNumbers[1]),
                CreateItemModel(3, settings.ExcludedPSNumbers[2]),
            };
            mockGetItemSubTeamsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsParameters>()))
                .Returns(new List<ItemDataAccessModel>
                {
                    new ItemDataAccessModel{ ItemID = 1, PSNumber = settings.ExcludedPSNumbers[1] },
                    new ItemDataAccessModel{ ItemID = 2, PSNumber = settings.ExcludedPSNumbers[2] },
                    new ItemDataAccessModel{ ItemID = 3, PSNumber = settings.ExcludedPSNumbers[0] },
                });

            //When
            primeAffinityManager.SendPrimeAffinityMessages(items);

            //Then
            mockGetItemSubTeamsQueryHandler.Verify(m => m.Search(It.IsAny<GetItemsParameters>()), Times.Once);
            mockGetPrimeAffinityItemStoreModelsQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsParameters>()), Times.Never);
            mockGetPrimeAffinityItemStoreModelsForActiveSalesQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsForActiveSalesParameters>()), Times.Never);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.IsAny<PrimeAffinityPsgProcessorParameters>()), Times.Never);
        }

        [TestMethod]
        public void SendPrimeAffinityMessages_ItemsAreChangingToExcludedSubTeams_SendsDeletePsgs()
        {
            //Given
            List<ItemModel> items = new List<ItemModel>
            {
                CreateItemModel(1, settings.ExcludedPSNumbers[0]),
                CreateItemModel(2, settings.ExcludedPSNumbers[1]),
                CreateItemModel(3, settings.ExcludedPSNumbers[2]),
            };
            mockGetItemSubTeamsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsParameters>()))
                .Returns(new List<ItemDataAccessModel>
                {
                    new ItemDataAccessModel{ ItemID = 1, PSNumber = 1111 },
                    new ItemDataAccessModel{ ItemID = 2, PSNumber = 2222 },
                    new ItemDataAccessModel{ ItemID = 3, PSNumber = 3333 },
                });
            mockGetPrimeAffinityItemStoreModelsQueryHandler.Setup(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsParameters>()))
                .Returns(items.Select(i => new PrimeAffinityItemStoreModel
                {
                    BusinessUnitId = 1234,
                    ItemId = i.GlobalAttributes.ItemID,
                    Region = "FL"
                }));

            //When
            primeAffinityManager.SendPrimeAffinityMessages(items);

            //Then
            mockGetItemSubTeamsQueryHandler.Verify(m => m.Search(It.IsAny<GetItemsParameters>()), Times.Once);
            mockGetPrimeAffinityItemStoreModelsQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsParameters>()), Times.Once);
            mockGetPrimeAffinityItemStoreModelsForActiveSalesQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsForActiveSalesParameters>()), Times.Never);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.Is<PrimeAffinityPsgProcessorParameters>(
                p => p.MessageAction == ActionEnum.Delete)), Times.Once);
        }

        [TestMethod]
        public void SendPrimeAffinityMessages_ItemsAreChangingToNonExcludedSubTeamsAndItemIsNotOnSaleWithPrimeEligiblePriceTypes_DoesNotSendPsgs()
        {
            //Given
            List<ItemModel> items = new List<ItemModel>
            {
                CreateItemModel(1, 1111),
                CreateItemModel(2, 2222),
                CreateItemModel(3, 3333),
            };
            mockGetItemSubTeamsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsParameters>()))
                .Returns(new List<ItemDataAccessModel>
                {
                    new ItemDataAccessModel{ ItemID = 1, PSNumber = settings.ExcludedPSNumbers[0] },
                    new ItemDataAccessModel{ ItemID = 2, PSNumber = settings.ExcludedPSNumbers[1] },
                    new ItemDataAccessModel{ ItemID = 3, PSNumber = settings.ExcludedPSNumbers[2] },
                });
            mockGetPrimeAffinityItemStoreModelsForActiveSalesQueryHandler.Setup(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsForActiveSalesParameters>()))
                .Returns(new List<PrimeAffinityItemStoreModel>());

            //When
            primeAffinityManager.SendPrimeAffinityMessages(items);

            //Then
            mockGetItemSubTeamsQueryHandler.Verify(m => m.Search(It.IsAny<GetItemsParameters>()), Times.Once);
            mockGetPrimeAffinityItemStoreModelsQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsParameters>()), Times.Never);
            mockGetPrimeAffinityItemStoreModelsForActiveSalesQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsForActiveSalesParameters>()), Times.Once);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.IsAny<PrimeAffinityPsgProcessorParameters>()), Times.Never);
        }

        [TestMethod]
        public void SendPrimeAffinityMessages_ItemsAreChangingToNonExcludedSubTeamsAndItemIsOnSaleWithPrimeEligiblePriceTypes_SendsAddOrUpdatePsgs()
        {
            //Given
            List<ItemModel> items = new List<ItemModel>
            {
                CreateItemModel(1, 1111),
                CreateItemModel(2, 2222),
                CreateItemModel(3, 3333),
            };
            mockGetItemSubTeamsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsParameters>()))
                .Returns(new List<ItemDataAccessModel>
                {
                    new ItemDataAccessModel{ ItemID = 1, PSNumber = settings.ExcludedPSNumbers[0] },
                    new ItemDataAccessModel{ ItemID = 2, PSNumber = settings.ExcludedPSNumbers[1] },
                    new ItemDataAccessModel{ ItemID = 3, PSNumber = settings.ExcludedPSNumbers[2] },
                });
            mockGetPrimeAffinityItemStoreModelsForActiveSalesQueryHandler.Setup(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsForActiveSalesParameters>()))
                .Returns(items.Select(i => new PrimeAffinityItemStoreModel
                {
                    BusinessUnitId = 1234,
                    ItemId = i.GlobalAttributes.ItemID,
                    ItemTypeCode = "TST",
                    Region = "FL",
                    ScanCode = i.ToString(),
                    StoreName = "TEST"
                }).ToList());

            //When
            primeAffinityManager.SendPrimeAffinityMessages(items);

            //Then
            mockGetItemSubTeamsQueryHandler.Verify(m => m.Search(It.IsAny<GetItemsParameters>()), Times.Once);
            mockGetPrimeAffinityItemStoreModelsQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsParameters>()), Times.Never);
            mockGetPrimeAffinityItemStoreModelsForActiveSalesQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimeAffinityItemStoreModelsForActiveSalesParameters>()), Times.Once);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.Is<PrimeAffinityPsgProcessorParameters>(
                p => p.MessageAction == ActionEnum.AddOrUpdate)), Times.Once);
        }

        private ItemModel CreateItemModel(int itemId, int psNumber)
        {
            return new ItemModel
            {
                GlobalAttributes = new GlobalAttributesModel
                {
                    ItemID = itemId,
                    PSNumber = psNumber
                }
            };
        }
    }
}
