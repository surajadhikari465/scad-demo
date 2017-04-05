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
    /// <summary>
    /// Summary description for DeleteValidationPriceServiceDecoratorTests
    /// </summary>
    [TestClass]
    public class DeleteValidationPriceServiceDecoratorTests
    {
        private DeleteValidationPricesServiceDecorator decorator;
        private Mock<IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>>> mockGetPricesQuery;
        private Mock<IQueryHandler<GetDeletedPricesByGpmIdsParameters, IEnumerable<DeletedPriceModel>>> mockGetDeletedPricesQuery;
        private Mock<IEsbMessage> mockEsbMessage;
        private Mock<IService<PriceModel>> mockPriceService;
        private List<DbPriceModel> dbPrices;
        private List<PriceModel> prices;
        private List<DeletedPriceModel> deletedPrices;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockGetPricesQuery = new Mock<IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>>>();
            this.mockGetDeletedPricesQuery = new Mock<IQueryHandler<GetDeletedPricesByGpmIdsParameters, IEnumerable<DeletedPriceModel>>>();
            this.mockEsbMessage = new Mock<IEsbMessage>();
            this.mockPriceService = new Mock<IService<PriceModel>>();
            this.decorator = new DeleteValidationPricesServiceDecorator(this.mockPriceService.Object, this.mockGetPricesQuery.Object, this.mockGetDeletedPricesQuery.Object);

            this.dbPrices = new List<DbPriceModel>();
            this.deletedPrices = new List<DeletedPriceModel>();
            this.prices = new List<PriceModel> { new TestPriceModelBuilder().WithAction(ActionEnum.Delete).Build() };
            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>())).Returns(new List<DbPriceModel>());
            this.mockGetDeletedPricesQuery.Setup(q => q.Search(It.IsAny<GetDeletedPricesByGpmIdsParameters>())).Returns(new List<DeletedPriceModel>());
        }

        [TestMethod]
        public void DeleteValidationDecorator_PriceWithNoDeleteAction_QueriesNotExecuted()
        {
            // Given
            this.prices.ForEach(p => p.Action = ActionEnum.Replace);

            // When
            this.decorator.Process(this.prices, this.mockEsbMessage.Object);

            // Then
            this.mockGetPricesQuery.Verify(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>()), Times.Never);
            this.mockGetDeletedPricesQuery.Verify(q => q.Search(It.IsAny<GetDeletedPricesByGpmIdsParameters>()), Times.Never);
            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(p => p == this.prices), this.mockEsbMessage.Object), Times.Once);
        }

        [TestMethod]
        public void DeleteValidationDecorator_PriceWithDeleteActionNotFoundInDatabase_SetsErrorPropertiesNotNullForPrice()
        {
            // Given
            Guid gpmId = this.prices.First().GpmId;

            // When
            this.decorator.Process(this.prices, this.mockEsbMessage.Object);

            // Then
            foreach (var price in this.prices)
            {
                Assert.AreEqual(Errors.Codes.DeletePricesError, price.ErrorCode, "ErrorCode incorrect");
                Assert.AreEqual(Errors.Details.DeletePriceDoesNotExist, price.ErrorDetails, "ErrorDetails incorrect");

                this.mockGetPricesQuery.Verify(q => q.Search(It.Is<GetPricesByGpmIdsParameters>(p => p.Prices.Select(x => x.GpmID).Contains(gpmId)))
                    , Times.Once);
                this.mockGetDeletedPricesQuery.Verify(q => q.Search(It.Is<GetDeletedPricesByGpmIdsParameters>(dp => dp.PriceIds.Contains(gpmId)))
                    , Times.Once);
            }
            
            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(p => p == this.prices), this.mockEsbMessage.Object), Times.Once);
        }

        [TestMethod]
        public void DeleteValidationDecorator_PriceWithDeleteActionFoundInPriceTable_ErrorPropertiesNullAndDeletedPricesQueryNotExecuted()
        {
            // Given
            Guid gpmId = this.prices.First().GpmId;
            this.prices.Add(new TestPriceModelBuilder().WithGpmId(Guid.NewGuid()).WithAction(ActionEnum.Delete).Build());
            this.prices.ForEach(price => { this.dbPrices.Add(new TestDbPriceModelBuilder().WithGpmId(price.GpmId).Build()); });
            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>())).Returns(this.dbPrices);

            // When
            this.decorator.Process(this.prices, this.mockEsbMessage.Object);

            // Then
            foreach (var price in this.prices)
            {
                Assert.IsNull(price.ErrorCode, "ErrorCode is not null");
                Assert.IsNull(price.ErrorDetails, "ErrorDetails is not null");

                this.mockGetPricesQuery.Verify(q => q.Search(It.Is<GetPricesByGpmIdsParameters>(p => p.Prices.Select(x => x.GpmID).Contains(price.GpmId)))
                    , Times.Once);
                this.mockGetDeletedPricesQuery.Verify(q => q.Search(It.Is<GetDeletedPricesByGpmIdsParameters>(dp => dp.PriceIds.Contains(price.GpmId)))
                    , Times.Never);
            }

            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(p => p == this.prices), this.mockEsbMessage.Object), Times.Once);
        }

        [TestMethod]
        public void DeleteValidationDecorator_PriceWithDeleteActionNotFoundInPriceTableAndFoundInDeletedPricesTable_ErrorPropertiesNull()
        {
            // Given
            Guid gpmId = this.prices.First().GpmId;
            this.prices.Add(new TestPriceModelBuilder().WithGpmId(Guid.NewGuid()).WithAction(ActionEnum.Delete).Build());
            this.prices.ForEach(price => { this.deletedPrices.Add(new TestDeletedPriceModelBuilder().WithGpmId(price.GpmId).Build()); });
            
            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>())).Returns(new List<DbPriceModel>());
            this.mockGetDeletedPricesQuery.Setup(q => q.Search(It.IsAny<GetDeletedPricesByGpmIdsParameters>())).Returns(this.deletedPrices);

            // When
            this.decorator.Process(this.prices, this.mockEsbMessage.Object);

            // Then
            foreach (var price in this.prices)
            {
                Assert.IsNull(price.ErrorCode, "ErrorCode is not null");
                Assert.IsNull(price.ErrorDetails, "ErrorDetails is not null");

                this.mockGetPricesQuery.Verify(q => q.Search(It.Is<GetPricesByGpmIdsParameters>(p => p.Prices.Select(x => x.GpmID).Contains(price.GpmId)))
                    , Times.Once);
                this.mockGetDeletedPricesQuery.Verify(q => q.Search(It.Is<GetDeletedPricesByGpmIdsParameters>(dp => dp.PriceIds.Contains(price.GpmId)))
                    , Times.Once);
            }

            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(p => p == this.prices), this.mockEsbMessage.Object), Times.Once);
        }
    }
}
