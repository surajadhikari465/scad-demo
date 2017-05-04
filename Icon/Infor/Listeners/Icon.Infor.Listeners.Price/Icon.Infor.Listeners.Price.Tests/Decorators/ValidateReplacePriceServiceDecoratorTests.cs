using Icon.Esb.Schemas.Wfm.ContractTypes;
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

namespace Icon.Infor.Listeners.Price.Tests.Decorators
{
    [TestClass]
    public class ValidateReplacePriceServiceDecoratorTests
    {
        private ValidateReplacePriceServiceDecorator decorator;
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
            this.decorator = new ValidateReplacePriceServiceDecorator(this.mockPriceService.Object, this.mockGetPricesQuery.Object, this.mockGetDeletedPricesQuery.Object);

            this.dbPrices = new List<DbPriceModel>();
            this.deletedPrices = new List<DeletedPriceModel>();
            this.prices = new List<PriceModel> { new TestPriceModelBuilder().WithAction(ActionEnum.Replace).WithReplaceGpmId(Guid.NewGuid()).Build() };
            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>())).Returns(new List<DbPriceModel>());
            this.mockGetDeletedPricesQuery.Setup(q => q.Search(It.IsAny<GetDeletedPricesByGpmIdsParameters>())).Returns(new List<DeletedPriceModel>());
        }

        [TestMethod]
        public void ReplaceValidationDecorator_PriceWithNoReplaceAction_QueriesNotExecuted()
        {
            // Given
            this.prices.ForEach(p => p.Action = ActionEnum.Add); // set to action something other than 'Replace'

            // When
            this.decorator.Process(this.prices, this.mockEsbMessage.Object);

            // Then
            this.mockGetPricesQuery.Verify(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>()), Times.Never);
            this.mockGetDeletedPricesQuery.Verify(q => q.Search(It.IsAny<GetDeletedPricesByGpmIdsParameters>()), Times.Never);
            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(p => p == this.prices), this.mockEsbMessage.Object), Times.Once);
        }

        [TestMethod]
        public void ReplaceValidationDecorator_PriceReplaceGpmIdNotFoundInDatabase_ErrorPropertiesNotNullForPrice()
        {
            // Given
            Guid gpmId = this.prices.First().ReplacedGpmId.GetValueOrDefault();

            // When
            this.decorator.Process(this.prices, this.mockEsbMessage.Object);

            // Then
            foreach (var price in this.prices)
            {
                Assert.AreEqual(Errors.Codes.ReplacePricesDeleteError, price.ErrorCode, "ErrorCode incorrect");
                Assert.AreEqual(Errors.Details.ReplacePriceDoesNotExist, price.ErrorDetails, "ErrorDetails incorrect");

                this.mockGetPricesQuery.Verify(q => q.Search(It.Is<GetPricesByGpmIdsParameters>(p => p.Prices.Select(x => x.GpmID).Contains(gpmId)))
                    , Times.Once);
                this.mockGetDeletedPricesQuery.Verify(q => q.Search(It.Is<GetDeletedPricesByGpmIdsParameters>(dp => dp.PriceIds.Contains(gpmId)))
                    , Times.Once);
            }

            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(p => p == this.prices), this.mockEsbMessage.Object), Times.Once);
        }

        [TestMethod]
        public void ReplaceValidationDecorator_PriceReplaceGpmIdFoundInPriceTable_ErrorPropertiesNullAndDeletedPricesQueryNotExecuted()
        {
            // Given
            Guid gpmId = this.prices.First().ReplacedGpmId.GetValueOrDefault();
            this.prices.Add(new TestPriceModelBuilder().WithReplaceGpmId(Guid.NewGuid()).WithAction(ActionEnum.Replace).Build());
            this.prices.ForEach(price => 
                {
                    this.dbPrices.Add(new TestDbPriceModelBuilder().WithGpmId(price.ReplacedGpmId.GetValueOrDefault()).Build());
                });
            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>())).Returns(this.dbPrices);

            // When
            this.decorator.Process(this.prices, this.mockEsbMessage.Object);

            // Then
            foreach (var price in this.prices)
            {
                Assert.IsNull(price.ErrorCode, "ErrorCode is not null");
                Assert.IsNull(price.ErrorDetails, "ErrorDetails is not null");

                this.mockGetPricesQuery.Verify(q => q.Search(It.Is<GetPricesByGpmIdsParameters>(p => p.Prices.Select(x => x.GpmID)
                    .Contains(price.ReplacedGpmId.GetValueOrDefault())))
                        , Times.Once);
                this.mockGetDeletedPricesQuery.Verify(q => q.Search(It.Is<GetDeletedPricesByGpmIdsParameters>(dp => dp.PriceIds
                    .Contains(price.ReplacedGpmId.GetValueOrDefault())))
                        , Times.Never);
            }

            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(p => p == this.prices), this.mockEsbMessage.Object), Times.Once);
        }

        [TestMethod]
        public void ReplaceValidationDecorator_PriceReplaceGpmIdNotFoundInPriceTableAndFoundInDeletedPricesTable_ErrorPropertiesRemainNull()
        {
            // Given
            Guid gpmId = this.prices.First().ReplacedGpmId.GetValueOrDefault();
            this.prices.Add(new TestPriceModelBuilder().WithReplaceGpmId(Guid.NewGuid()).WithAction(ActionEnum.Replace).Build());
            this.prices.ForEach(price => 
                {
                    this.deletedPrices.Add(new TestDeletedPriceModelBuilder().WithGpmId(price.GpmId).Build());
                });
            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>())).Returns(new List<DbPriceModel>());
            this.mockGetDeletedPricesQuery.Setup(q => q.Search(It.IsAny<GetDeletedPricesByGpmIdsParameters>())).Returns(this.deletedPrices);

            // When
            this.decorator.Process(this.prices, this.mockEsbMessage.Object);

            // Then
            foreach (var price in this.prices)
            {
                Assert.IsNull(price.ErrorCode, "ErrorCode is not null");
                Assert.IsNull(price.ErrorDetails, "ErrorDetails is not null");

                this.mockGetPricesQuery.Verify(q => q.Search(It.Is<GetPricesByGpmIdsParameters>(p => p.Prices.Select(x => x.GpmID)
                    .Contains(price.ReplacedGpmId.GetValueOrDefault())))
                        , Times.Once);
                this.mockGetDeletedPricesQuery.Verify(q => q.Search(It.Is<GetDeletedPricesByGpmIdsParameters>(dp => dp.PriceIds
                    .Contains(price.ReplacedGpmId.GetValueOrDefault())))
                    , Times.Once);
            }

            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(p => p == this.prices), this.mockEsbMessage.Object), Times.Once);
        }

        [TestMethod]
        public void ReplaceValidationDecorator_PriceGpmIdToBeAddedFoundInPriceTable_ErrorPropertiesNotNull()
        {
            // Given
            foreach (var price in this.prices)
            {
                this.dbPrices.Add(new TestDbPriceModelBuilder().WithGpmId(price.GpmId).Build()); // matches price to be added
                this.dbPrices.Add(new TestDbPriceModelBuilder().WithGpmId(price.ReplacedGpmId.GetValueOrDefault()).Build()); // matches price to be deleted
            }

            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>())).Returns(this.dbPrices);

            // When
            this.decorator.Process(prices, mockEsbMessage.Object);

            // Then
            foreach (var price in this.prices)
            {
                Assert.AreEqual(Errors.Codes.ReplacePricesAddError, price.ErrorCode, "ErrorCode is incorrect");
                Assert.AreEqual(Errors.Details.ReplacePriceAddAlreadyExists, price.ErrorDetails, "ErrorDetails is incorrect");
                this.mockGetPricesQuery.Verify(q => q.Search(It.Is<GetPricesByGpmIdsParameters>(p => p.Prices.Select(r => r.GpmID).Contains(price.GpmId))),
                    Times.Once);
                this.mockGetDeletedPricesQuery.Verify(q => q.Search(It.Is<GetDeletedPricesByGpmIdsParameters>(dp => dp.PriceIds
                    .Contains(price.ReplacedGpmId.GetValueOrDefault())))
                        , Times.Never);
            }

            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(pm => pm == this.prices), It.IsAny<IEsbMessage>()),
                    Times.Once);
        }

        [TestMethod]
        public void ReplaceValidationDecorator_PriceGpmIdToBeAddedFoundInPriceTableAndIdToBeDeletedNotFound_ErrorPropertiesSetToAddError()
        {
            // Given
            foreach (var price in this.prices)
            {
                this.dbPrices.Add(new TestDbPriceModelBuilder().WithGpmId(price.GpmId).Build()); // matches price to be added
            }

            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByGpmIdsParameters>())).Returns(this.dbPrices);

            // When
            this.decorator.Process(prices, mockEsbMessage.Object);

            // Then
            foreach (var price in this.prices)
            {
                Assert.AreEqual(Errors.Codes.ReplacePricesAddError, price.ErrorCode, "ErrorCode is incorrect");
                Assert.AreEqual(Errors.Details.ReplacePriceAddAlreadyExists, price.ErrorDetails, "ErrorDetails is incorrect");
                this.mockGetPricesQuery.Verify(q => q.Search(It.Is<GetPricesByGpmIdsParameters>(p => p.Prices.Select(r => r.GpmID).Contains(price.GpmId))),
                    Times.Once);
                this.mockGetDeletedPricesQuery.Verify(q => q.Search(It.Is<GetDeletedPricesByGpmIdsParameters>(dp => dp.PriceIds
                    .Contains(price.ReplacedGpmId.GetValueOrDefault())))
                        , Times.Once);
            }

            this.mockPriceService.Verify(s => s.Process(It.Is<IEnumerable<PriceModel>>(pm => pm == this.prices), It.IsAny<IEsbMessage>()),
                    Times.Once);
        }
    }
}
