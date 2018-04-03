using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using Mammoth.PrimeAffinity.Library.Processors;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Service.Decorators;
using MammothWebApi.Service.Models;
using MammothWebApi.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using MammothWebApi.Common;

namespace MammothWebApi.Tests.Service.Decorators
{
    [TestClass]
    public class PrimeAffinityPsgAddPriceServiceDecoratorTests
    {
        private PrimeAffinityPsgAddPriceServiceDecorator decorator;
        private Mock<IUpdateService<AddUpdatePrice>> mockPriceService;
        private Mock<IQueryHandler<GetPrimePsgItemDataByScanCodeQuery, IEnumerable<PrimePsgItemStoreDataModel>>> mockQueryHandler;
        private Mock<IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters>> mockPrimeAffinityPsgProcessor;
        private Mock<IPrimeAffinityPsgSettings> mockSettings;
        private AddUpdatePrice data;

        [TestInitialize]
        public void Initialize()
        {
            mockPriceService = new Mock<IUpdateService<AddUpdatePrice>>();
            mockQueryHandler = new Mock<IQueryHandler<GetPrimePsgItemDataByScanCodeQuery, IEnumerable<PrimePsgItemStoreDataModel>>>();
            mockPrimeAffinityPsgProcessor = new Mock<IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters>>();
            mockSettings = new Mock<IPrimeAffinityPsgSettings>();

            decorator = new PrimeAffinityPsgAddPriceServiceDecorator(
                mockPriceService.Object,
                mockQueryHandler.Object, 
                mockPrimeAffinityPsgProcessor.Object,
                mockSettings.Object);
            data = new AddUpdatePrice();
            mockSettings.SetupGet(m => m.EnablePrimeAffinityPsgMessages)
                .Returns(true);
        }

        [TestMethod]
        public void Handle_EnablePrimeAffinityPsgMessagesIsFalse_DecoratorSkipsSendingPrices()
        {
            //Given
            mockSettings.Reset();
            mockSettings.SetupGet(m => m.EnablePrimeAffinityPsgMessages)
                .Returns(false);
            var region = "FL";
            data.Prices = new List<PriceServiceModel>
            {
                new PriceServiceModel{ BusinessUnitId = 123, ScanCode = "111", Region = region, PriceType = "ISS", StartDate = DateTime.Today },
                new PriceServiceModel{ BusinessUnitId = 456, ScanCode = "222", Region = region, PriceType = "FRZ", StartDate = DateTime.Today  },
                new PriceServiceModel{ BusinessUnitId = 789, ScanCode = "333", Region = region, PriceType = "SAL", StartDate = DateTime.Today  },
                new PriceServiceModel{ BusinessUnitId = 147, ScanCode = "444", Region = region, PriceType = "SAL", StartDate = DateTime.Today  },
            };
            var primePsgItemStoreDataModels = data.Prices
                .Select(p => new PrimePsgItemStoreDataModel
                {
                    BusinessUnitId = p.BusinessUnitId,
                    ItemId = int.Parse(p.ScanCode),
                    ItemTypeCode = "RTL",
                    PsSubTeamNumber = p.BusinessUnitId + int.Parse(p.ScanCode),
                    Region = region,
                    StoreName = "Test" + p.BusinessUnitId,
                    ScanCode = p.ScanCode
                });
            mockQueryHandler.Setup(m => m.Search(It.IsAny<GetPrimePsgItemDataByScanCodeQuery>()))
                .Returns(primePsgItemStoreDataModels);

            //When
            decorator.Handle(data);

            //Then
            mockPriceService.Verify(m => m.Handle(It.IsAny<AddUpdatePrice>()), Times.Once);
            mockQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimePsgItemDataByScanCodeQuery>()), Times.Never);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.IsAny<PrimeAffinityPsgProcessorParameters>()), Times.Never);
        }

        [TestMethod]
        public void Handle_PricesArePrimeEligibleSalesAndStartToday_ShouldSendAddOrUpdatePsgs()
        {
            //Given
            var region = "FL";
            data.Prices = new List<PriceServiceModel>
            {
                new PriceServiceModel{ BusinessUnitId = 123, ScanCode = "111", Region = region, PriceType = "ISS", StartDate = DateTime.Today },
                new PriceServiceModel{ BusinessUnitId = 456, ScanCode = "222", Region = region, PriceType = "FRZ", StartDate = DateTime.Today  },
                new PriceServiceModel{ BusinessUnitId = 789, ScanCode = "333", Region = region, PriceType = "SAL", StartDate = DateTime.Today  },
                new PriceServiceModel{ BusinessUnitId = 147, ScanCode = "444", Region = region, PriceType = "SAL", StartDate = DateTime.Today  },
            };
            var primePsgItemStoreDataModels = data.Prices
                .Select(p => new PrimePsgItemStoreDataModel
                {
                    BusinessUnitId = p.BusinessUnitId,
                    ItemId = int.Parse(p.ScanCode),
                    ItemTypeCode = "RTL",
                    PsSubTeamNumber = p.BusinessUnitId + int.Parse(p.ScanCode),
                    Region = region,
                    StoreName = "Test" + p.BusinessUnitId,
                    ScanCode = p.ScanCode
                });
            mockQueryHandler.Setup(m => m.Search(It.IsAny<GetPrimePsgItemDataByScanCodeQuery>()))
                .Returns(primePsgItemStoreDataModels);

            //When
            decorator.Handle(data);

            //Then
            mockPriceService.Verify(m => m.Handle(It.IsAny<AddUpdatePrice>()), Times.Once);
            mockQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimePsgItemDataByScanCodeQuery>()), Times.Once);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.Is<PrimeAffinityPsgProcessorParameters>(
                p => p.MessageAction == ActionEnum.AddOrUpdate
                    && p.Region == region
                    && p.PrimeAffinityMessageModels.Count() == 4
                    && AssertDataModelsAreEqualToMessageModels(primePsgItemStoreDataModels, p.PrimeAffinityMessageModels, ActionEnum.AddOrUpdate))), Times.Once);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.Is<PrimeAffinityPsgProcessorParameters>(
                p => p.MessageAction == ActionEnum.Delete)), Times.Never);
        }

        [TestMethod]
        public void Handle_PricesArePrimeEligibleSalesAndStartInTheFuture_ShouldNotSendPsgs()
        {
            //Given
            var region = "FL";
            data.Prices = new List<PriceServiceModel>
            {
                new PriceServiceModel{ BusinessUnitId = 123, ScanCode = "111", Region = region, PriceType = "ISS", StartDate = DateTime.Today.AddDays(1) },
                new PriceServiceModel{ BusinessUnitId = 456, ScanCode = "222", Region = region, PriceType = "FRZ", StartDate = DateTime.Today.AddDays(1)  },
                new PriceServiceModel{ BusinessUnitId = 789, ScanCode = "333", Region = region, PriceType = "SAL", StartDate = DateTime.Today.AddDays(1)  },
                new PriceServiceModel{ BusinessUnitId = 147, ScanCode = "444", Region = region, PriceType = "SAL", StartDate = DateTime.Today.AddDays(1)  },
            };
            var primePsgItemStoreDataModels = data.Prices
                .Select(p => new PrimePsgItemStoreDataModel
                {
                    BusinessUnitId = p.BusinessUnitId,
                    ItemId = int.Parse(p.ScanCode),
                    ItemTypeCode = "RTL",
                    PsSubTeamNumber = p.BusinessUnitId + int.Parse(p.ScanCode),
                    Region = region,
                    StoreName = "Test" + p.BusinessUnitId,
                    ScanCode = p.ScanCode
                });
            mockQueryHandler.Setup(m => m.Search(It.IsAny<GetPrimePsgItemDataByScanCodeQuery>()))
                .Returns(primePsgItemStoreDataModels);

            //When
            decorator.Handle(data);

            //Then
            mockPriceService.Verify(m => m.Handle(It.IsAny<AddUpdatePrice>()), Times.Once);
            mockQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimePsgItemDataByScanCodeQuery>()), Times.Never);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.Is<PrimeAffinityPsgProcessorParameters>(
                p => p.MessageAction == ActionEnum.AddOrUpdate)), Times.Never);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.Is<PrimeAffinityPsgProcessorParameters>(
                p => p.MessageAction == ActionEnum.Delete)), Times.Never);
        }

        [TestMethod]
        public void Handle_PricesAreNotPrimeEligibleSalesAndStartToday_ShouldSendDeletePsgs()
        {
            //Given
            var region = "FL";
            data.Prices = new List<PriceServiceModel>
            {
                new PriceServiceModel{ BusinessUnitId = 123, ScanCode = "111", Region = region, PriceType = "TST", StartDate = DateTime.Today },
                new PriceServiceModel{ BusinessUnitId = 456, ScanCode = "222", Region = region, PriceType = "TST", StartDate = DateTime.Today },
                new PriceServiceModel{ BusinessUnitId = 789, ScanCode = "333", Region = region, PriceType = "TST", StartDate = DateTime.Today },
                new PriceServiceModel{ BusinessUnitId = 147, ScanCode = "444", Region = region, PriceType = "TST", StartDate = DateTime.Today },
            };
            var primePsgItemStoreDataModels = data.Prices
                .Select(p => new PrimePsgItemStoreDataModel
                {
                    BusinessUnitId = p.BusinessUnitId,
                    ItemId = int.Parse(p.ScanCode),
                    ItemTypeCode = "RTL",
                    PsSubTeamNumber = p.BusinessUnitId + int.Parse(p.ScanCode),
                    Region = region,
                    StoreName = "Test" + p.BusinessUnitId,
                    ScanCode = p.ScanCode
                });
            mockQueryHandler.Setup(m => m.Search(It.IsAny<GetPrimePsgItemDataByScanCodeQuery>()))
                .Returns(primePsgItemStoreDataModels);

            //When
            decorator.Handle(data);

            //Then
            mockPriceService.Verify(m => m.Handle(It.IsAny<AddUpdatePrice>()), Times.Once);
            mockQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimePsgItemDataByScanCodeQuery>()), Times.Once);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.Is<PrimeAffinityPsgProcessorParameters>(
                p => p.MessageAction == ActionEnum.AddOrUpdate)), Times.Never);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.Is<PrimeAffinityPsgProcessorParameters>(
                p => p.MessageAction == ActionEnum.Delete
                    && p.Region == region
                    && p.PrimeAffinityMessageModels.Count() == 4
                    && AssertDataModelsAreEqualToMessageModels(primePsgItemStoreDataModels, p.PrimeAffinityMessageModels, ActionEnum.Delete))), Times.Once);
        }

        [TestMethod]
        public void Handle_PricesAreReg_ShouldNotSendPsgs()
        {
            //Given
            var region = "FL";
            data.Prices = new List<PriceServiceModel>
            {
                new PriceServiceModel{ BusinessUnitId = 123, ScanCode = "111", Region = region, PriceType = "REG", StartDate = DateTime.Today },
                new PriceServiceModel{ BusinessUnitId = 456, ScanCode = "222", Region = region, PriceType = "REG", StartDate = DateTime.Today.AddDays(1)  },
                new PriceServiceModel{ BusinessUnitId = 789, ScanCode = "333", Region = region, PriceType = "REG", StartDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today  },
                new PriceServiceModel{ BusinessUnitId = 147, ScanCode = "444", Region = region, PriceType = "REG", StartDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today.AddDays(5) },
            };
            var primePsgItemStoreDataModels = ConvertToDataModels(data.Prices, region);
            mockQueryHandler.Setup(m => m.Search(It.IsAny<GetPrimePsgItemDataByScanCodeQuery>()))
                .Returns(primePsgItemStoreDataModels);

            //When
            decorator.Handle(data);

            //Then
            mockPriceService.Verify(m => m.Handle(It.IsAny<AddUpdatePrice>()), Times.Once);
            mockQueryHandler.Verify(m => m.Search(It.IsAny<GetPrimePsgItemDataByScanCodeQuery>()), Times.Never);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.Is<PrimeAffinityPsgProcessorParameters>(
                p => p.MessageAction == ActionEnum.AddOrUpdate)), Times.Never);
            mockPrimeAffinityPsgProcessor.Verify(m => m.SendPsgs(It.Is<PrimeAffinityPsgProcessorParameters>(
                p => p.MessageAction == ActionEnum.Delete)), Times.Never);
        }

        private bool AssertDataModelsAreEqualToMessageModels(
            IEnumerable<PrimePsgItemStoreDataModel> primePsgItemStoreDataModels, 
            IEnumerable<PrimeAffinityMessageModel> primeAffinityMessageModels,
            ActionEnum expectedAction)
        {
            Assert.AreEqual(primePsgItemStoreDataModels.Count(), primeAffinityMessageModels.Count());
            foreach (var dataModel in primePsgItemStoreDataModels)
            {
                var messageModel = primeAffinityMessageModels
                    .Single(m => m.ScanCode == dataModel.ScanCode && m.BusinessUnitID == dataModel.BusinessUnitId);
                Assert.AreEqual(dataModel.ItemId, messageModel.ItemID);
                Assert.AreEqual(dataModel.ItemTypeCode, messageModel.ItemTypeCode);
                Assert.AreEqual(dataModel.Region, messageModel.Region);
                Assert.AreEqual(dataModel.StoreName, messageModel.StoreName);
                Assert.AreEqual(expectedAction, messageModel.MessageAction);
                Assert.IsNotNull(messageModel.InternalPriceObject);
                Assert.IsNull(messageModel.ErrorCode);
                Assert.IsNull(messageModel.ErrorDetails);
            }
            return true;
        }

        private IEnumerable<PrimePsgItemStoreDataModel> ConvertToDataModels(IEnumerable<PriceServiceModel> prices, string region)
        {
            return prices
                .Select(p => new PrimePsgItemStoreDataModel
                {
                    BusinessUnitId = p.BusinessUnitId,
                    ItemId = int.Parse(p.ScanCode),
                    ItemTypeCode = "RTL",
                    PsSubTeamNumber = p.BusinessUnitId + int.Parse(p.ScanCode),
                    Region = region,
                    StoreName = "Test" + p.BusinessUnitId,
                    ScanCode = p.ScanCode
                });
        }
    }
}
