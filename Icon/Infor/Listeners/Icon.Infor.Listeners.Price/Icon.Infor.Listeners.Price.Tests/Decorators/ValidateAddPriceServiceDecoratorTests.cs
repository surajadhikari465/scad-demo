using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.Constants;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Icon.Infor.Listeners.Price.DataAccess.Queries;
using Icon.Infor.Listeners.Price.Decorators;
using Icon.Infor.Listeners.Price.Models;
using Icon.Infor.Listeners.Price.Services;
using Icon.Infor.Listeners.Price.Tests.ModelBuilders;
using Mammoth.Common.DataAccess.CommandQuery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Icon.Esb.Schemas.Wfm.ContractTypes;

namespace Icon.Infor.Listeners.Price.Tests.Decorators
{
    [TestClass]
    public class ValidateAddPriceServiceDecoratorTests
    {
        private ValidateAddPriceServiceDecorator decorator;
        private Mock<IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>>> mockGetPricesQuery;
        private Mock<IEsbMessage> mockEsbMessage;
        private Mock<IService<PriceModel>> mockPriceService;
        private List<DbPriceModel> dbPrices;
        private List<PriceModel> prices;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockGetPricesQuery = new Mock<IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>>>();
            this.mockEsbMessage = new Mock<IEsbMessage>();
            this.mockPriceService = new Mock<IService<PriceModel>>();
            this.decorator = new ValidateAddPriceServiceDecorator(this.mockPriceService.Object, this.mockGetPricesQuery.Object);

            this.dbPrices = new List<DbPriceModel>();
            this.prices = new List<PriceModel> { new TestPriceModelBuilder().Build() };
            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>())).Returns(new List<DbPriceModel>());
        }

        [TestMethod]
        public void AddValidationPriceServiceDecorator_PriceWithAddActionNotFoundInDatabase_ErrorPropertiesNotSet()
        {
            // Given
            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>())).Returns(new List<DbPriceModel>());

            // When
            this.decorator.Process(prices, mockEsbMessage.Object);

            // Then
            foreach (var price in this.prices)
            {
                Assert.IsNull(price.ErrorCode);
                Assert.IsNull(price.ErrorDetails);
                this.mockGetPricesQuery.Verify(q => q.Search(It.Is<GetPricesByGpmIdsParameters>(p => p.Prices.Select(r => r.GpmID).Contains(price.GpmId))),
                    Times.Once);
            }

            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(pm => pm == this.prices), It.IsAny<IEsbMessage>()),
                    Times.Once);
        }

        [TestMethod]
        public void AddValidationPriceServiceDecorator_PriceWithAddActionFoundInDatabase_ErrorPropertiesSet()
        {
            // Given
            foreach (var price in this.prices)
            {
                this.dbPrices.Add(new TestDbPriceModelBuilder().WithGpmId(price.GpmId).Build());
            }

            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>())).Returns(this.dbPrices);

            // When
            this.decorator.Process(prices, mockEsbMessage.Object);

            // Then
            foreach (var price in this.prices)
            {
                Assert.AreEqual(Errors.Codes.AddPricesError, price.ErrorCode, "ErrorCode is incorrect");
                Assert.AreEqual(Errors.Details.AddPriceAlreadyExists, price.ErrorDetails, "ErrorDetails is incorrect");
                this.mockGetPricesQuery.Verify(q => q.Search(It.Is<GetPricesByGpmIdsParameters>(p => p.Prices.Select(r => r.GpmID).Contains(price.GpmId))),
                    Times.Once);
            }

            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(pm => pm == this.prices), It.IsAny<IEsbMessage>()),
                    Times.Once);
        }

        [TestMethod]
        public void AddValidationPriceServiceDecorator_PriceWithDeleteAction_DoesNotExecuteGetPricesQuery()
        {
            // Given
            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>())).Returns(new List<DbPriceModel>());
            this.prices.ForEach(p => p.Action = ActionEnum.Delete);

            // When
            this.decorator.Process(this.prices, this.mockEsbMessage.Object);

            // Then
            this.mockGetPricesQuery.Verify(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>()), Times.Never);
            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(pm => pm == this.prices), It.IsAny<IEsbMessage>()),
                    Times.Once);
        }

        [TestMethod]
        public void AddValidationPriceServiceDecorator_PricesWithAddActionFoundInDatabase_ErrorPropertiesSet()
        {
            // Given
            Guid errantGuid = this.prices.First().GpmId;
            this.prices.Add(new TestPriceModelBuilder().WithGpmId(Guid.NewGuid()).Build());
            this.dbPrices.Add(new TestDbPriceModelBuilder().WithGpmId(errantGuid).Build());

            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>())).Returns(this.dbPrices);

            // When
            this.decorator.Process(prices, mockEsbMessage.Object);

            // Then
            var actualPriceWithErrors = this.prices.First(p => p.GpmId == errantGuid);
            Assert.AreEqual(Errors.Codes.AddPricesError, actualPriceWithErrors.ErrorCode, "ErrorCode is incorrect");
            Assert.AreEqual(Errors.Details.AddPriceAlreadyExists, actualPriceWithErrors.ErrorDetails, "ErrorDetails is incorrect");
            this.mockGetPricesQuery.Verify(q => q.Search(It.Is<GetPricesByGpmIdsParameters>(p => p.Prices.Select(r => r.GpmID).Contains(actualPriceWithErrors.GpmId))),
                Times.Once);

            var actualPriceGood = this.prices.First(p => p.GpmId != errantGuid);
            Assert.IsNull(actualPriceGood.ErrorCode);
            Assert.IsNull(actualPriceGood.ErrorDetails);
            this.mockGetPricesQuery.Verify(q => q.Search(It.Is<GetPricesByGpmIdsParameters>(p => p.Prices.Select(r => r.GpmID).Contains(actualPriceGood.GpmId))),
                Times.Once);

            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(pm => pm == this.prices), It.IsAny<IEsbMessage>()),
                    Times.Once);
        }
    }
}
